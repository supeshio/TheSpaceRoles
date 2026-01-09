using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TSR.Game.Options
{
    //public abstract class OptionBase
    //{
    //    public int Selection;
    //    public string OptionName;
    //    /// <summary>
    //    /// Translation後を入れる
    //    /// </summary>
    //    public abstract List<string> Selections();

    //    //選択肢 and 選択肢名 and どの選択肢か

    //    public void CreateOptionUI()
    //    {
    //    }
    //}
    public static class CustomOption
    {
        public static Dictionary<string, CustomOptionAttribute> CustomOptions { get; private set; } = new();

        public static CustomOptionAttribute Get(string id) => CustomOptions.TryGetValue(id, out CustomOptionAttribute? option) ? option : throw new KeyNotFoundException($"Option Id:[{id}] not found");

        public static Dictionary<string, CustomOptionBoolAttribute> CustomBoolOptions = new();
        public static Dictionary<string, CustomOptionEnumAttribute> CustomEnumOptions = new();
        public static Dictionary<string, CustomOptionFloatAttribute> CustomFloatOptions = new();
        public static Dictionary<string, CustomOptionIntAttribute> CustomIntOptions = new();

        public static void Load()
        {
            CustomBoolOptions = GetOption<CustomOptionBoolAttribute>();
            CustomEnumOptions = GetOption<CustomOptionEnumAttribute>();
            CustomFloatOptions = GetOption<CustomOptionFloatAttribute>();
            CustomIntOptions = GetOption<CustomOptionIntAttribute>();


            CustomOptions = new Dictionary<string, CustomOptionAttribute>();

            foreach (var p in CustomBoolOptions)
                CustomOptions[p.Key] = p.Value;

            foreach (var p in CustomEnumOptions)
                CustomOptions[p.Key] = p.Value;

            foreach (var p in CustomFloatOptions)
                CustomOptions[p.Key] = p.Value;

            foreach (var p in CustomIntOptions)
                CustomOptions[p.Key] = p.Value;

            foreach (var option in CustomOptions)
            {
                Logger.Message($"{option.Key}:{option.Value.Id}", "option");
            }
        }

        // private static Dictionary<string, T> GetOption<T>() where T : CustomOptionAttribute
        // {
        //     var nestedType = typeof(T).GetNestedTypes(System.Reflection.BindingFlags.Public);
        //     var att = nestedType
        //         .Where(type => 0 < type.GetCustomAttributes(typeof(T), false).Length)
        //         .Select(type => (T)type.GetCustomAttributes())
        //         .ToDictionary(type => type.Id, type => type);
        //     return att;
        // }
        private static Dictionary<string, T> GetOption<T>() where T : CustomOptionAttribute
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetProperties())
                .Select(p => p.GetCustomAttribute<T>())
                .Where(a => a != null)
                .ToDictionary(a => a.Id, a => a);
        }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class CustomOptionAttribute : Attribute
    {
        public string GroupId;
        public string Id;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="id"></param>
        protected CustomOptionAttribute(string groupId, string id)
        {
            this.Id = id;
            this.GroupId = groupId;
        }

        public abstract object[] GetOptions();
    }

    /// <summary>
    /// int選択肢｡
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="id">設定ID</param>
    /// <param name="step"></param>
    /// <param name="optionSelectionID">設定選択肢翻訳ID</param>
    /// <param name="defaultValue"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class CustomOptionNumAttribute<T>(
        string groupId,
        string id,
        T defaultValue,
        T min,
        T max,
        T step,
        string optionSelectionID)
        : CustomOptionAttribute(groupId, id)
    {
        public readonly T Min = min;
        public readonly T Max = max;
        public readonly T Step = step;
        public readonly T DefaultValue = defaultValue;
        public readonly string OptionSelectionID = optionSelectionID;

        //public override object[] CreateOptions()
        //{
        //    List<string> strs = [];
        //    int i = Min;
        //    while (i < Max)
        //    {
        //        i += Step;
        //        strs.Add(string.Format(Translation.Get(OptionSelectionID), i.ToString()));
        //    }
        //    return [..strs];
        //}
    }

    /// <summary>
    /// float選択肢｡
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomOptionIntAttribute : CustomOptionNumAttribute<int>
    {
        private readonly int _defaultValue;
        private readonly string _optionSelectionID;

        /// <param name="groupId"></param>
        /// <param name="id">設定ID</param>
        /// <param name="step"></param>
        /// <param name="optionSelectionID">設定選択肢翻訳ID</param>
        /// <param name="defaultValue"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public CustomOptionIntAttribute(string groupId, string id, int defaultValue, int min, int max, int step,
            string optionSelectionID) : base(groupId, id, defaultValue, min, max, step, optionSelectionID)
        {
        }

        public override object[] GetOptions()
        {
            List<string> translations = [];
            for (int i = Min; i <= Max; i++)
            {
                translations.Add(string.Format(Translation.Get(OptionSelectionID), i.ToString()));
            }

            return [.. translations];
        }
    }

    /// <summary>
    /// float選択肢｡
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomOptionFloatAttribute : CustomOptionNumAttribute<float>
    {
        public CustomOptionFloatAttribute(string groupId, string id, float defaultValue, float min, float max,
            float step, string optionSelectionID) : base(groupId, id, defaultValue, min, max, step, optionSelectionID)
        {
        }

        public override object[] GetOptions()
        {
            List<string> strs = [];
            for (float i = Min; i <= Max; i++)
            {
                strs.Add(string.Format(Translation.Get(OptionSelectionID), i.ToString()));
            }

            return [.. strs];
        }
    }

    /// <summary>
    /// オンオフ選択肢
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomOptionBoolAttribute : CustomOptionAttribute
    {
        public static string On() => Translation.Get("option.selection.on");
        public static string Off() => Translation.Get("option.selection.off");
        public bool DefaultValue;

        public CustomOptionBoolAttribute(string groupId, string id, bool defaultValue) : base(groupId, id)
        {
            this.DefaultValue = defaultValue;
        }

        public override object[] GetOptions()
        {
            return [On(), Off()];
        }
    }

    /// <summary>
    /// enum選択肢
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomOptionEnumAttribute : CustomOptionAttribute
    {
        public byte DefaultValue;
        public Type enumType;

        public CustomOptionEnumAttribute(string groupId, string id, Type enumType, byte defaultValue) : base(groupId,
            id)
        {
            this.DefaultValue = defaultValue;
            this.enumType = enumType;
        }

        public override object[] GetOptions()
        {
            return Enum.GetNames(enumType);
        }
    }
}