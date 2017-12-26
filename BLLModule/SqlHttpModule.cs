using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
namespace ZentCloud.BLLModule
{
    /// <summary> 
    /// 简单防止sql注入 
    /// </summary> 
    public class SqlHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }
        /// <summary> 
        /// 处理sql注入 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        private void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            try
            {
                string key = string.Empty;
                string value = string.Empty;
                //url提交数据 get方式 
                if (context.Request.QueryString != null)
                {
                    for (int i = 0; i < context.Request.QueryString.Count; i++)
                    {
                        key = context.Request.QueryString.Keys[i];
                        value = context.Server.UrlDecode(context.Request.QueryString[key]);
                        if (!FilterSql(value))
                        {
                            context.Response.Redirect("/error/sqlerror.htm");
                        }
                    }
                }
                //表单提交数据 post方式 
                if (context.Request.Form != null)
                {
                    for (int i = 0; i < context.Request.Form.Count; i++)
                    {
                        key = context.Request.Form.Keys[i];
                        if (key == "__VIEWSTATE") continue;
                        value = context.Server.HtmlDecode(context.Request.Form[i]);
                        if (!FilterSql(value))
                        {
                            context.Response.Redirect("/error/sqlerror.htm");
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/error/sqlerror.htm");
            }
        }
        /// <summary> 
        /// 过滤非法关键字
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        private bool FilterSql(string key)
        {
            bool flag = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {

                    List<string> sqlKeyWordList = new List<string>();
                    sqlKeyWordList.Add("insert");
                    sqlKeyWordList.Add("delete");
                    sqlKeyWordList.Add("select");
                    sqlKeyWordList.Add("update");
                    sqlKeyWordList.Add("exec");
                    sqlKeyWordList.Add("varchar");
                    sqlKeyWordList.Add("drop");
                    sqlKeyWordList.Add("creat");
                    sqlKeyWordList.Add("declare");
                    sqlKeyWordList.Add("truncate");
                    sqlKeyWordList.Add("cursor");
                    sqlKeyWordList.Add("begin");
                    sqlKeyWordList.Add("open");
                    sqlKeyWordList.Add("<--");
                    sqlKeyWordList.Add("-->");
                    foreach (string item in sqlKeyWordList)
                    {
                        if (key.ToUpper().IndexOf(item.ToUpper()) != -1)
                        {
                            flag = false;
                            break;
                        }
                    }

                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
}
