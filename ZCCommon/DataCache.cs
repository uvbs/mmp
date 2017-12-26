using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace ZentCloud.Common
{
    /// <summary>
    /// 缓存相关的操作类
    /// Copyright (C) Maticsoft
    /// </summary>
    public class DataCache   
    {
        private static ObjectCache cache = MemoryCache.Default;
        
        public static string FormatKey(string Key)
        {
            int oKeyLength = Key.Length;
            Key = StringHelper.StrToMD5(Key);
            Key = oKeyLength + "_" + Key;
            return Key;
        }
        public static string FormatKey(byte[] Key)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(Key);
            int oKeyLength = output.Length;
            string result = BitConverter.ToString(output).Replace("-", "");
            result = oKeyLength + "_" + result;
            return result;
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            return cache.Get(CacheKey);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// 默认6小时
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            if (objObject == null) return;
            CacheItemPolicy policy = new CacheItemPolicy();
            if(slidingExpiration.HasValue){
                policy.SlidingExpiration = slidingExpiration.Value;
            }
            else if(absoluteExpiration.HasValue){
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            //else
            //{
            //    policy.SlidingExpiration = TimeSpan.FromHours(6);
            //}
            cache.Add(CacheKey, objObject, policy);
        }

        public static void ClearCache(string CacheKey)
        {
            cache.Remove(CacheKey);
        }

        public static void ClearCacheStartsWith(string StartKey)
        {
            //整表清理的
            string[] ClearTableByStart = new string[] { "MenuInfo_", "KeyVauleDataInfo_", "PermissionGroupInfo_"
                , "WebsiteInfo_", "WebsiteDomainInfo_" };
            string ClearTableKey = ClearTableByStart.FirstOrDefault(p=>StartKey.StartsWith(p));
            if (!string.IsNullOrWhiteSpace(ClearTableKey)) StartKey = ClearTableKey;

            List<KeyValuePair<string, object>> list = cache.Where(p => p.Key.StartsWith(StartKey)).ToList();
            foreach (var item in list)
            {
                cache.Remove(item.Key);
            }
        }
        public static void ClearCacheAll()
        {
            List<KeyValuePair<string,object>> list = cache.ToList();
            foreach (var item in list)
            {
                cache.Remove(item.Key);
            }
        }
    }
}
