
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ueditor.Helper
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public static class Config
    {
        private static bool noCache = true;
        private static List<ConfigList> ConfigList = new List<ConfigList>();
        public static JObject GetItems(string configPath)
        {
            configPath = !string.IsNullOrWhiteSpace(configPath)? configPath :"config.json";
            if(ConfigList.Where(p=>p.ConfigName==configPath).Count()>0)
            {
                return ConfigList.First(p=>p.ConfigName==configPath).ConfigObject;
            }
            else
            {
                ConfigList newObject =  new ConfigList();
                newObject.ConfigName = configPath;
                var json = File.ReadAllText(HttpContext.Current.Server.MapPath("/UeditorConfig/"+configPath));
                newObject.ConfigObject = JObject.Parse(json);
                ConfigList.Add(newObject);
                return newObject.ConfigObject;
            }
        }

        public static string GetConfigPath(string configPath)
        {
            configPath = !string.IsNullOrWhiteSpace(configPath) ? configPath : "config.json";
            return configPath;
        }

        public static T GetValue<T>(string key,string configPath)
        {
            configPath = GetConfigPath(configPath);
            var Items = GetItems(configPath);
            return Items[key].Value<T>();
        }

        public static String[] GetStringList(string key, string configPath)
        {
            configPath = GetConfigPath(configPath);
            var Items = GetItems(configPath);
            return Items[key].Select(x => x.Value<String>()).ToArray();
        }

        public static String GetString(string key, string configPath)
        {
            return GetValue<String>(key, configPath);
        }

        public static int GetInt(string key, string configPath)
        {
            return GetValue<int>(key, configPath);
        }
    }

    public class ConfigList{
        public string ConfigName{ get; set;}

        public JObject ConfigObject{get;set;}
    }
}