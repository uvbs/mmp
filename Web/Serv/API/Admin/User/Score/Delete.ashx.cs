using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Score
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            var websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
            
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                apiResp.msg = "ids为必填参数,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            string[] strIds = ids.Split(',');
            for (int i = 0; i < strIds.Length; i++)
            {
                UserInfo userModel = bllUser.GetUserInfoByAutoID(int.Parse(strIds[i]));
                if (websiteInfo.IsUnionHongware == 1)
                {
                    Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                    var memberInfo = hongWareClient.GetMemberInfo(userModel.WXOpenId);
                    if (memberInfo.member == null)
                    {
                        apiResp.msg = "宏巍会员不存在";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }
                    if (!hongWareClient.UpdateMemberScore(memberInfo.member.mobile,userModel.WXOpenId,-memberInfo.member.point))
                    {
                        apiResp.msg = "清空宏巍会员积分失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                }

                if (userModel.TotalScore == 0) continue;
                double score = userModel.TotalScore;
                bllUser.Update(new UserInfo(), string.Format(" TotalScore=0"), string.Format("  AutoId={0} ", userModel.AutoID));

                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                scoreRecord.UserID = userModel.UserID;
                scoreRecord.AddTime = DateTime.Now;
                scoreRecord.TotalScore = 0;
                scoreRecord.Score = -score;
                scoreRecord.ScoreType = "AdminSubmit";
                scoreRecord.AddNote = "积分清零";
                scoreRecord.RelationID = bllUser.GetCurrUserID();
                scoreRecord.WebSiteOwner = bllUser.WebsiteOwner;
                bllUser.Add(scoreRecord);
            }
            apiResp.msg = "操作完成";
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }
    }
}