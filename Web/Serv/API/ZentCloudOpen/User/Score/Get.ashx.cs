using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.User.Score
{
    /// <summary>
    /// 获取用户积分
    /// </summary>
    public class Get : BaseHanderOpen
    {

        /// <summary>
        /// 积分
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 日志
        /// </summary>
        BLLJIMP.BLLApiLog bllLog = new BLLJIMP.BLLApiLog();
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
            string serialNumber = context.Request["serial_number"];//流水号
            if (string.IsNullOrEmpty(openId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 参数必传";
                bllScore.ContextResponse(context, resp);
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
                bllScore.ContextResponse(context, resp);
                return;
            }
            if (bllLog.Add(bllLog.WebsiteOwner, EnumApiModule.Score, "获取用户积分", openId, userInfo.UserID, serialNumber))
            {
                resp.status = true;
                resp.msg = "ok";
                resp.result = new
                {

                    score = userInfo.TotalScore,
                    history_totalscore=userInfo.HistoryTotalScore,
                    used_totalscore=Math.Abs(bllScore.GetUsedTotalScore(userInfo.UserID))

                };

            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "操作失败";
            }
            bllScore.ContextResponse(context, resp);

        }



    }
}