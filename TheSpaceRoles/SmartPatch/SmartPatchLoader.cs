using HarmonyLib;
using Il2CppSystem.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace TSR;

//by KYMario / SmartPatch v1.2
//note: ログ出力はデバッグ用です。不要であれば削除して構いません。(nullにすると無効化できます。)

public static class SmartPatchLoader
{
    public static BepInEx.Logging.ManualLogSource Logger = TSR._Logger;
    public static Dictionary<Type, HashSet<MethodInfo>> EnumeratorInfos;

    private enum SmartType
    {
        Unknown,
        Prefix,
        Postfix
    }

    public static void PatchAll()
    {
        EnumeratorInfos = new();
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var type in assembly.GetTypes())
        {
            var methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var typeAttributes = type.GetCustomAttributes<SmartPatchAttribute>();
            if (!typeAttributes.Any()) continue;

            foreach (var method in methodInfos)
            {
                var methodAttributes = method.GetCustomAttributes<SmartPatchAttribute>();

                if (!methodAttributes.Any() && typeAttributes.First().TargetMethodName != null)
                    CheckAttribute(null);
                foreach (var attribute in methodAttributes)
                    CheckAttribute(attribute);

                void CheckAttribute(SmartPatchAttribute attribute)
                {
                    var classAttribute = method.DeclaringType?.GetCustomAttributes<SmartPatchAttribute>()?.First();
                    if (attribute == null && classAttribute == null) return;

                    var TargetType = attribute?.TargetType ?? classAttribute?.TargetType;
                    var TargetMethodName = attribute?.TargetMethodName ?? classAttribute?.TargetMethodName;

                    if (TargetType == null || TargetMethodName == null) return;

                    var argumentTypes = attribute?.ArgumentTypes;

                    var targetMethod = argumentTypes != null && argumentTypes.Any()
                        ? TargetType.GetMethod(TargetMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null)
                        : TargetType.GetMethod(TargetMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    if (targetMethod == null)
                    {
                        Logger?.LogError($"メソッドが見つかりません: {TargetType.FullName}.{TargetMethodName}");
                        return;
                    }

                    var patchType = GetType(method);
                    MethodInfo patchMethod = null;

                    if (patchType == SmartType.Unknown)
                    {
                        if (attribute != null) Logger?.LogError($"{method.Name}のタイプが指定されていません");
                        return;
                    }

                    if (typeof(IEnumerator).IsAssignableFrom(targetMethod.ReturnType))
                    {
                        var stateMachine = TargetType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public)
                            .FirstOrDefault(x => x.Name.StartsWith($"_{TargetMethodName}_"));

                        if (stateMachine == null)
                        {
                            Logger?.LogError($"ステートマシン型が見つかりません: {TargetType.FullName}<{TargetMethodName}>");
                            return;
                        }

                        targetMethod = stateMachine.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        if (targetMethod == null)
                        {
                            Logger?.LogError("MoveNextが見つかりません");
                            return;
                        }

                        if (!EnumeratorInfos.ContainsKey(stateMachine)) EnumeratorInfos[stateMachine] = new();
                        EnumeratorInfos[stateMachine].Add(method);

                        var patchName = patchType == SmartType.Prefix ? nameof(EnumeratorPatch) : nameof(EnumeratorPostfixPatch);
                        patchMethod = typeof(SmartPatchLoader).GetMethod(patchName,
                                        BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    }
                    else
                    {
                        patchMethod = method;
                    }

                    Patch(targetMethod, patchMethod, patchType);
                }
            }
        }

        TSR.Harmony.GetPatchedMethods().Do(x => Logger?.LogWarning($"PatchedMethods: {x.DeclaringType.Name}.{x.Name}"));
    }

    private static void EnumeratorPostfixPatch(object __instance)
        => EnumeratorPatch(__instance);

    private static bool EnumeratorPatch(object __instance)
    {
        var type = __instance.GetType();
        if (!EnumeratorInfos.TryGetValue(type, out var methods)) return true;
        var state = type.GetProperty("__1__state", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        var instance = type.GetProperty("__4__this", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            var smartType = GetType(method);
            if ((int)state.GetValue(__instance) != (smartType == SmartType.Postfix ? 1 : 0)) continue;

            var parameters = method.GetParameters();
            var args = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                args[i] = GetObject(parameters[i]);

            var del = CreateTypedDelegate(method);
            var result = del.DynamicInvoke(args);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                {
                    var fld = type.GetField(parameters[i].Name,
                              BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (fld != null) fld.SetValue(__instance, args[i]);
                }
            }

            if (result is bool _result && _result == false) return false;
        }
        return true;

        object GetObject(ParameterInfo param, MethodInfo method = null)
        {
            Type paramType = param.ParameterType.IsByRef
                         ? param.ParameterType.GetElementType()
                         : param.ParameterType;

            if (param.IsOut)
                return null;

            if ((param.Name == "__instance" || paramType == instance?.PropertyType) && instance != null)
            {
                return instance.GetValue(__instance);
            }
            var field = type.GetField(param.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                return field.GetValue(__instance);
            }
            var property = type.GetProperty(param.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
            {
                return property.GetValue(__instance);
            }

            //debuglog
            Logger?.LogWarning($"フィールド一覧:\n{string.Join("\n", type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Select(f => $"{f.FieldType.Name} {f.Name}"))}");
            Logger?.LogWarning($"プロパティ一覧:\n{string.Join("\n", type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Select(p => $"{p.PropertyType.Name} {p.Name}"))}");
            Logger?.LogWarning($"{method?.Name ?? "不明なメゾット"} の引数、{param.Name}の検索に失敗");
            return null;
        }
    }

    private static Delegate CreateTypedDelegate(MethodInfo method)
    {
        var parameters = method.GetParameters();
        var paramTypes = parameters.Select(p => p.ParameterType).ToArray();

        // 戻り値が void なら Action<> 系、そうでなければ Func<>
        var returnType = method.ReturnType;

        var dynamicMethod = new DynamicMethod(
            $"Dynamic_{method.Name}",
            returnType,
            paramTypes,
            method.DeclaringType.Module,
            skipVisibility: true
        );

        var il = dynamicMethod.GetILGenerator();

        // 引数を push
        for (int i = 0; i < parameters.Length; i++)
        {
            il.Emit(OpCodes.Ldarg, i); // 普通に push。ByRef もそのままでよい
        }

        // static or instance 呼び出し
        il.EmitCall(method.IsStatic ? OpCodes.Call : OpCodes.Callvirt, method, null);

        // 戻り値の処理
        if (returnType == typeof(void))
        {
            il.Emit(OpCodes.Ret);
        }
        else
        {
            il.Emit(OpCodes.Ret);
        }

        return dynamicMethod.CreateDelegate(Expression.GetDelegateType(paramTypes.Concat(new[] { returnType }).ToArray()));
    }

    private static SmartType GetType(MethodInfo method)
    {
        if (method.GetCustomAttribute<SmartPrefixAttribute>() != null)
            return SmartType.Prefix;
        if (method.GetCustomAttribute<SmartPostfixAttribute>() != null)
            return SmartType.Postfix;
        if (method.Name == "Prefix")
            return SmartType.Prefix;
        if (method.Name == "Postfix")
            return SmartType.Postfix;
        return SmartType.Unknown;
    }

    private static void Patch(MethodInfo targetMethod, MethodInfo method, SmartType type)
    {
        var harmonyMethod = new HarmonyMethod(method)
        {
            priority = method.GetCustomAttribute<HarmonyPriority>()?.info.priority ?? Priority.Normal
        };
        if (type == SmartType.Prefix)
            TSR.Harmony.Patch(targetMethod, prefix: harmonyMethod);
        else if (type == SmartType.Postfix)
            TSR.Harmony.Patch(targetMethod, postfix: harmonyMethod);
    }
}