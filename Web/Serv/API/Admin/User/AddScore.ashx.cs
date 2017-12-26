using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// 添加积分
    /// </summary>
    public class AddScore : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int autoId = !string.IsNullOrEmpty(context.Request["autoid"]) ? int.Parse(context.Request["autoid"]) : 0;
            string totalScore = context.Request["total_score"];
            double addScoreD = 0;
            if(autoId<=0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "autoid 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (!string.IsNullOrEmpty(totalScore))
            {
                if (!double.TryParse(totalScore, out addScoreD))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    resp.errmsg = "积分不正确";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(autoId,bllUser.WebsiteOwner);
            if (userInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该条会员信息";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            userInfo.TotalScore += addScoreD;
            if (bllUser.Update(userInfo, string.Format(" TotalScore={0}", userInfo.TotalScore), string.Format(" AutoID={0} AND WebsiteOwner='{1}'", userInfo.AutoID,bllUser.WebsiteOwner)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "添加积分出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}