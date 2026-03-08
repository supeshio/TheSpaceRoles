using System;

namespace TSR;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class SmartPatchAttribute : Attribute
{
    public Type TargetType { get; }
    public string TargetMethodName { get; }
    public Type[] ArgumentTypes { get; }

    public SmartPatchAttribute(Type targetType, string methodName, params Type[] argumentTypes)
    {
        TargetType = targetType;
        TargetMethodName = methodName;
        ArgumentTypes = argumentTypes;
    }

    public SmartPatchAttribute(Type targetType, params Type[] argumentTypes)
    {
        TargetType = targetType;
        TargetMethodName = null;
        ArgumentTypes = argumentTypes;
    }

    public SmartPatchAttribute(string methodName, params Type[] argumentTypes)
    {
        TargetType = null;
        TargetMethodName = methodName;
        ArgumentTypes = argumentTypes;
    }

    public SmartPatchAttribute()
    { }
}