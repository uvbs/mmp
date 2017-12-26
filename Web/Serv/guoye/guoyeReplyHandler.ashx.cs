using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZCJson.Linq;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.guoye
{
    /// <summary>
    /// 国烨
    /// </summary>
    public class guoyeReplyHandler : IHttpHandler, IReadOnlySessionState
    {
        BLLJuActivity bllActivity = new BLLJuActivity();
        UserInfo currentUserInfo = new UserInfo();
        DefaultResponse resp = new DefaultResponse();

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string response = context.Request["response"];

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(response))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "参数不完整";
                bllActivity.ContextResponse(context, resp);
                return;
            }
            JuActivityInfo activity = bllActivity.GetJuActivity(Convert.ToInt32(id));
            activity.K10 = response;
            activity.K11 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            bool isSyncPass = true;
            #region 同步账号注册检查
            if (bllActivity.WebsiteOwner == "guoye" || bllActivity.WebsiteOwner == "guoyetest")
            {
                isSyncPass = false;

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("dealid", activity.K7);
                param.Add("response", activity.K10);
                param.Add("openid", activity.K5);
                param.Add("phone", activity.K6);
                param.Add("time", activity.K11);

                using (HttpWebResponse wr = ZentCloud.Common.HttpInterFace.CreatePostHttpResponse(
                    "http://www.chinayie.com/api/response", param, null, null, Encoding.UTF8, null))
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        JToken jto = JToken.Parse(sr.ReadToEnd());
                        if (jto["result"] != null && jto["result"].ToString().ToLower() == "true")
                        {
                            isSyncPass = true;
                        }
                        else if (jto["result"] != null && jto["msg"] != null && jto["result"].ToString().ToLower() == "false")
                        {
                            resp.errcode = (int)APIErrCode.OperateFail;
                            resp.errmsg = jto["msg"].ToString();
                            bllActivity.ContextResponse(context, resp);
                            return;
                        }
                    }
                }
            }
            #endregion

            if (isSyncPass && bllActivity.Update(activity))
            {
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "提交失败";
            }
            bllActivity.ContextResponse(context, resp);

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