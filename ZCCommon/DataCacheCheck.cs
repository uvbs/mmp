using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace ZentCloud.Common
{
    public class DataCacheCheck
    {
        //是否启用缓存
        public static bool EnableDataCache = Common.ConfigHelper.GetConfigBool("EnableDataCache");

        private static string MapPath = string.Empty;
        private static DateTime LastWriteJsonTime = DateTime.MinValue;
        private static DateTime CheckTime = DateTime.MinValue;

        private static Dictionary<string, List<string>> WebsiteOwnerStaticJsonList = new Dictionary<string, List<string>>();
        private static Dictionary<string, bool> RealTableWebsiteEnableList = new Dictionary<string, bool>();
        /// <summary>
        /// 获取当前站点信息(猜测session阻线程了)
        /// </summary>
        /// <returns></returns>
        protected static string GetStaticWebsiteOwner()
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["WebsiteOwner"] != null)
                return (string)System.Web.HttpContext.Current.Session["WebsiteOwner"];
            return "";
        }
        private static void GetWebsiteOwnerStaticJson()
        {
            if (CheckTime < DateTime.Now.AddMinutes(5))
            {
                DateTime tempLastWriteJsonTime = File.GetLastWriteTime(MapPath);
                if (LastWriteJsonTime < tempLastWriteJsonTime)
                {
                    InitWebsiteOwnerStaticJson();
                    LastWriteJsonTime = tempLastWriteJsonTime;
                }
                CheckTime = DateTime.Now;
            }
        }
        public static void InitWebsiteOwnerStaticJson()
        {
            if (string.IsNullOrWhiteSpace(MapPath))
            {
                MapPath = System.Web.HttpContext.Current.Server.MapPath("/JsonConfig/WebsiteOwnerCache.json");
            }

            WebsiteOwnerStaticJsonList = new Dictionary<string, List<string>>();
            RealTableWebsiteEnableList = new Dictionary<string, bool>();
            string WebsiteOwnerJson = File.ReadAllText(MapPath);
            JObject CacheJo = JObject.Parse(WebsiteOwnerJson);
            List<JProperty> listProperty = CacheJo.Properties().ToList();
            foreach (JProperty item in listProperty)
            {
                if (WebsiteOwnerStaticJsonList.Keys.Contains(item.Name)) continue;
                JArray WebsiteJo = (JArray)CacheJo[item.Name];
                List<string> tempList = new List<string>();
                foreach (JValue val in WebsiteJo)
                {
                    tempList.Add(val.Value.ToString());
                }
                WebsiteOwnerStaticJsonList.Add(item.Name, tempList);
            }
        }

        private static string CheckWebsiteOwnerTableCacheEnable(string realTableName)
        {
            if (!EnableDataCache) return "";
            //return "Common";
            string nt = string.Empty;
            //对全站配置的缓存表进行缓存
            if (WebsiteOwnerStaticJsonList.Keys.Contains("Common"))
            {
                nt = WebsiteOwnerStaticJsonList["Common"].FirstOrDefault(p => p == realTableName);
                if (nt != null) return "Common";
            }
            else { return ""; }
            //猜测session阻线程了 取消 对站点缓存

            if (WebsiteOwnerStaticJsonList.Keys.Contains("WebsiteOwner"))
            {
                string nWebsiteOwner = GetStaticWebsiteOwner();
                if (!string.IsNullOrWhiteSpace(nWebsiteOwner))
                {
                    if (WebsiteOwnerStaticJsonList.Keys.Contains("WebsiteOwner"))
                    {
                        nt = WebsiteOwnerStaticJsonList["WebsiteOwner"].FirstOrDefault(p => p == realTableName);
                        if (nt != null) return nWebsiteOwner;
                    }
                    if (WebsiteOwnerStaticJsonList.Keys.Contains(nWebsiteOwner))
                    {
                        nt = WebsiteOwnerStaticJsonList[nWebsiteOwner].FirstOrDefault(p => p == realTableName);
                        if (nt != null) return nWebsiteOwner;
                    }
                }
            }
            return "";
        }

        public static bool CheckEnableDataCache(string RealTableName)
        {
            if (!EnableDataCache) return false;
            GetWebsiteOwnerStaticJson();//检查json配置
            string nWebsiteOwner = CheckWebsiteOwnerTableCacheEnable(RealTableName);
            if (string.IsNullOrWhiteSpace(nWebsiteOwner)) return false;
            return true;
        }

        public static string GetWebsiteOwner(string RealTableName)
        {
            if (!EnableDataCache) return "";
            GetWebsiteOwnerStaticJson();//检查json配置
            string nWebsiteOwner = CheckWebsiteOwnerTableCacheEnable(RealTableName);
            return nWebsiteOwner;
        }
    }
}
