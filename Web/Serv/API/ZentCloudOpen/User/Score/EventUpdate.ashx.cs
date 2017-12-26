using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.User.Score
{
    /// <summary>
    /// 积分事件触发
    /// </summary>
    public class EventUpdate : BaseHanderOpen
    {

        /// <summary>
        /// 积分Bll
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "请用POST提交";
                bllScore.ContextResponse(context, resp);
                return;
            }
            string openId = context.Request["openid"];//openId
            string value = context.Request["value"];//值
            string scoreEvent = context.Request["event"];//类型
            string remark=context.Request["remark"];//备注
            string showName = context.Request["user_name"];//显示名称
            string serialNumber = context.Request["serial_number"];//流水号

            scoreEvent = HttpUtility.UrlDecode(scoreEvent);
            remark = HttpUtility.UrlDecode(remark);
            showName = HttpUtility.UrlDecode(showName);
            if (string.IsNullOrEmpty(openId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 参数必传";
                bllScore.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(value))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "value 参数必传";
                bllScore.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(scoreEvent))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "event 参数必传";
                bllScore.ContextResponse(context, resp);
                return;
            }

            decimal valueD = 0;
            if (!decimal.TryParse(value, out valueD))
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "value 参数错误";
                bllScore.ContextResponse(context, resp);
                return;
            }
            if (valueD<=0)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "value 参数需大于0";
                bllScore.ContextResponse(context, resp);
                return;
            }
            string msg = "";
            int addScore = 0;
            if (bllScore.EventUpdate(bllScore.WebsiteOwner,openId, valueD, scoreEvent,remark,out msg,out addScore,showName,serialNumber))
            {
                resp.msg ="ok";
                resp.status = true;
                resp.result = addScore;
            }
            else
            {
                resp.msg = msg;
                resp.code = (int)APIErrCode.OperateFail;
            }
            bllScore.ContextResponse(context, resp);

        }

    }
}