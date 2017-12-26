using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Weixin.Message
{
    /// <summary>
    /// 自定义发送
    /// </summary>
    public class CustomizeSend : BaseHanderOpen
    {

        /// <summary>
        ///  微信
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 日志
        /// </summary>
        BLLJIMP.BLLApiLog bllLog = new BLLJIMP.BLLApiLog();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "请用POST提交";
                bllUser.ContextResponse(context, resp);
                return;
            }
            string openId = context.Request["openid"];//openId
            string json = context.Request["json"];//json
            string serialNumber = context.Request["serial_number"];//流水号

            json = HttpUtility.UrlDecode(json);
            if (string.IsNullOrEmpty(openId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 参数必传";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(json))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "json 参数必传";
                bllWeixin.ContextResponse(context, resp);
                return;
            }

            UserInfo userInfo = bllUser.GetUserInfoByOpenId(openId);
            if (userInfo == null && (bllUser.WebsiteOwner == "dongwu"||bllUser.WebsiteOwner=="dongwudev"))
            {
                userInfo = bllUser.CreateNewUser(bllUser.WebsiteOwner, openId, "");
            }
            if (userInfo == null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 不存在";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            string msg = "";
            var isSuccess = bllWeixin.SendTemplateMessageCustomize(json,out msg);
            if (isSuccess)
            {
                resp.status = true;
                resp.msg = "ok";
                bllLog.Add(bllLog.WebsiteOwner, EnumApiModule.WeixinMessage, string.Format("发送微信消息\njson内容{0}", json), openId, userInfo.UserID, serialNumber);
            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = msg;
            }
            bllWeixin.ContextResponse(context, resp);



        }


    }
}