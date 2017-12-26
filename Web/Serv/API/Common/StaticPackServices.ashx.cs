using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 静态资源打包服务
    /// </summary>
    public class StaticPackServices : BaseHandlerNoAction
    {
        public void ProcessRequest(HttpContext context)
        {
            string url = context.Request["f"];
            string result = string.Empty;
            string[] files = url.Split(',');
            foreach (var item in files)
            {
                string str = Path.GetExtension(item).ToLower();
                if (str.ToLower() != ".js" && str.ToLower() != ".css")
                {
                    continue;
                }
                string path = HttpContext.Current.Server.MapPath(item);
                if (!System.IO.File.Exists(path))
                {
                    apiResp.msg = item + "文件不存在";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    continue;
                }
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
                {
                    String input;
                    while ((input = sr.ReadLine()) != null)
                    {
                        apiResp.result += input;
                    }
                    sr.Close();
                }
            }
            context.Response.Write(result);
        }

    }
}