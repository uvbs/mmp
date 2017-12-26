using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ZentCloud.JubitIMP.Web.Tool
{
    public class AjaxMessgeBox
    {
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowMessgeBoxForAjax(UpdatePanel up,Type type, string msg)
        {
            ScriptManager.RegisterClientScriptBlock(up, type, "click", string.Format("alert('{0}')", msg), true);
        }

        /// <summary>
        /// 显示提示信息并跳转
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowMessgeBoxForAjax(UpdatePanel up,Type type, string msg, string url)
        {
            ScriptManager.RegisterClientScriptBlock(up, type, "click", string.Format("alert('{0}');window.location='{1}'", msg, url), true);
        }
    }
}