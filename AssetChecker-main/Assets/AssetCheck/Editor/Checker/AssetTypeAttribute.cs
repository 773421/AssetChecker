using System;
namespace Assets.Checker
{
    /// <summary>
    /// 检测器枚举类型属性
    /// </summary>
    public class AssetTypeAttribute : Attribute
    {
        public string pattern;
        public AssetTypeAttribute()
        {
        }

        public AssetTypeAttribute(string pattern)
        {
            this.pattern = pattern;
        }
        public static string GetPattern(Enum e) {
            var type = e.GetType();
            var fieldInfo = type.GetField(e.ToString());
            if (null == fieldInfo) {
                return string.Empty;
            }
            var pattern = string.Empty;
            var attrs = fieldInfo.GetCustomAttributes(typeof(AssetTypeAttribute), false);
            foreach (var attr in attrs) {
                pattern += (attr as AssetTypeAttribute).pattern;
            }
            return pattern;
        }
    }
}
