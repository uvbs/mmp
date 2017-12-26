using NetDimension.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// EZRproCallBackMember 的摘要说明
    /// </summary>
    public class EZRproCallBackMember : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                TologTemp(" INTO EZRproCallBackMember");
                context.Response.ContentType = "application/json";
                context.Response.Expires = 0;
                string result = "false";
                result = EZRproCallBackMemberAPI(context);
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                TologTemp(ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// 驿氪会员数据回传
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EZRproCallBackMemberAPI(HttpContext context)
        {
            string result = string.Empty;

            if (context.Request["token"] != "74F2EBA1C83264DEC0ACD7E8D2CAB50CBF767535")
            {
                return JsonConvert.SerializeObject(new
                {
                    Status = false,
                    Msg = "错误:token",
                    StatusCode = 0,
                    Result = 0
                });
            }

            Open.EZRproSDK.Entity.MemberCallBackReq info = new Open.EZRproSDK.Entity.MemberCallBackReq();
            info = bll.ConvertRequestToModel<Open.EZRproSDK.Entity.MemberCallBackReq>(info);
            var args = context.Request["Args"];

            TologTemp("MemberInfo ConvertRequestToModel:" + JsonConvert.SerializeObject(info));
            TologTemp("args:" + args);

            TologTemp("AppId:" + context.Request["AppId"]);
            TologTemp("Sign:" + context.Request["Sign"]);

            info.Args = JsonConvert.DeserializeObject<Open.EZRproSDK.Entity.MemberInfo>(args);

            TologTemp("args ok");

            var client = new Open.EZRproSDK.Client();

            result = client.CallBackMemberInfo(info);


            TologTemp("result:" + result);

            return result;
        }


        private void TologTemp(string msg)
        {
            if (File.Exists(@"D:\EZRproCallBackMember_log.txt"))
            {
                using (StreamWriter sw = new StreamWriter(@"D:\EZRproCallBackMember_log.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(msg);
                }
            }

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}