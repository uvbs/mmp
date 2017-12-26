using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ueditor.Helper
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContext context) : base(context) { }
        public string ConfigPath{get;set;}
        public ConfigHandler(HttpContext context, string configPath)
            : base(context)
        {
            this.ConfigPath = configPath;
        }

        public override void Process()
        {
            WriteJson(Config.GetItems(this.ConfigPath));
        }
    }
}