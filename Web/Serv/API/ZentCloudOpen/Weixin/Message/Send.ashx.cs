using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Weixin.Message
{
    /// <summary>
    /// 发送微信模板消息
    /// </summary>
    public class Send:BaseHanderOpen
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
            if (context.Request.HttpMethod!="POST")
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "请用POST提交";
                bllUser.ContextResponse(context, resp);
                return;
            }
            string openId = context.Request["openid"];//openId
            string title = context.Request["title"];//标题
            string content = context.Request["content"];//备注
            string link=context.Request["link"];//网页链接
            string serialNumber = context.Request["serial_number"];//流水号
            title = HttpUtility.UrlDecode(title);
            content = HttpUtility.UrlDecode(content);
            if (string.IsNullOrEmpty(openId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 参数必传";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(title))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "title 参数必传";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(content))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "content 参数必传";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            UserInfo userInfo = bllUser.GetUserInfoByOpenId(openId);
            if (userInfo==null&&(bllUser.WebsiteOwner=="dongwu"||bllUser.WebsiteOwner=="dongwudev"))
            {
               userInfo= bllUser.CreateNewUser(bllUser.WebsiteOwner, openId, "");
            }
            if (userInfo == null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 不存在";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            var isSuccess = bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, link);
            if (isSuccess)
            {
                resp.status = true;
                resp.msg = "ok";
                bllLog.Add(bllLog.WebsiteOwner, EnumApiModule.WeixinMessage, string.Format("发送微信消息\n标题:{0},内容:{1},链接:{2}",title,content,link), openId,userInfo.UserID,serialNumber);
            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "发送失败";
            }
            bllWeixin.ContextResponse(context, resp);







        }



    }
}