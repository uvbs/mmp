using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    public static class DictionaryExtensionMethodClass
    {
        /// <summary>
        /// SortedDictionary的扩展方法，将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static SortedDictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// SortedDictionary的扩展方法，根据key获取对应的值，如果不存在，返回null
        /// </summary>
        public static TValue getVaule<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return default(TValue);
        }

        /// <summary>
        /// Dictionary的扩展方法，将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// Dictionary的扩展方法，根据key获取对应的值，如果不存在，返回null
        /// </summary>
        public static TValue getVaule<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return default(TValue);
        }
    }
}
