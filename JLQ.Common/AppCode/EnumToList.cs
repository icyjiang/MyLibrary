using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace JLQ.Common
{
    /// <summary>
    /// 枚举工具类，将枚举的描述和相应值存到字典中缓存。
    /// </summary>
    public class EnumToList
    {
        private static ConcurrentDictionary<string, Dictionary<int, string>> _EnumList =
            new ConcurrentDictionary<string, Dictionary<int, string>>(); //枚举缓存池

        /// <summary>
        /// 将枚举绑定到ListControl
        /// </summary>
        /// <param name="listControl">ListControl</param>
        /// <param name="enumType">枚举类型</param>
        public static void FillListControl(ListControl listControl, Type enumType)
        {
            listControl.DataSource = null;
            listControl.DataSource = EnumToDictionary(enumType).Values.ToList();
        }

        /// <summary>
        /// 将枚举转换成Dictionary&lt;int, string&gt;
        /// Dictionary中，key为枚举项对应的int值；value为：若定义了EnumShowName属性，则取它，否则取name
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        static Dictionary<int, string> EnumToDictionary(Type enumType)
        {
            string keyName = enumType.FullName;

            if (!_EnumList.ContainsKey(keyName))
            {
                Dictionary<int, string> list = new Dictionary<int, string>();

                foreach (int i in Enum.GetValues(enumType))
                {
                    string name = Enum.GetName(enumType, i);

                    //取显示名称
                    string description = string.Empty;
                    object[] atts = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (atts.Length > 0) description = ((DescriptionAttribute)atts[0]).Description;

                    list.Add(i, string.IsNullOrEmpty(description) ? name : description);
                }
                _EnumList.AddOrUpdate(keyName, list, (a, b) => list);
            }

            return _EnumList[keyName];
        }

        /// <summary>
        /// 获取枚举值对应的显示名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="intValue">枚举项对应的int值</param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, int intValue)
        {
            return EnumToDictionary(enumType)[intValue];
        }
    }

    /*实际调用：
     * EnumToList.FillListControl(cbRelatedParam, typeof(VariableName));
     */
}
