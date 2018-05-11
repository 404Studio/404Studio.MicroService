using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using YH.Etms.Utility.Extensions;

namespace YH.Etms.Settlement.Api.Infrastructure.Attributes
{
    public class EnumHelper
    {
        private static Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        /// 标识已经经过扫描的类型集合
        /// </summary>
        private static HashSet<string> m_scanedHash = new HashSet<string>();

        public static string GetEnumDescription(Enum @enum)
        {
            if (@enum == null) throw new ArgumentNullException(nameof(@enum));
            string description = @enum.ToString();
            var fieldInfo = @enum.GetType().GetField(description);
            if (fieldInfo == null) return default(string);
            var desc = GetEnumDescription(fieldInfo);
            description = desc.IsNotNullOrEmpty() ? desc : description;
            return description;
        }

        public static string GetEnumDescription(FieldInfo fieldInfo)
        {
            var description = "";
            var attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        /// <summary>
        /// 列举枚举所有值和描述
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetValueDescriptionDic<T>() where T : struct
        {
            Type type = typeof(T);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (int item in System.Enum.GetValues(type))
            {
                string description = string.Empty;
                try
                {
                    FieldInfo fieldInfo = type.GetField(System.Enum.GetName(type, item));
                    if (fieldInfo == null)
                    {
                        continue;
                    }
                    DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                    if (da == null)
                    {
                        continue;
                    }
                    description = da.Description;
                }
                catch { }
                dictionary.Add(item, description);
            }
            return dictionary;
        }
    }
}
