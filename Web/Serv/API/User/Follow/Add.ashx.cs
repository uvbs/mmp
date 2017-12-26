using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Follow
{
    /// <summary>
    /// Add 的摘要说明   添加关注
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 通用关系业务
        /// </summary>
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            int toUserId = Convert.ToInt32(context.Request["to_user_id"]);
            UserInfo toUserModel = bllUser.GetUserInfoByAutoID(toUserId);
            if (toUserModel == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "不存在关注的用户";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (this.bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.FollowUser, toUserModel.UserID, CurrentUserInfo.UserID))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                apiResp.msg = "你已经关注该用户";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (this.bLLCommRelation.AddCommRelation(BLLJIMP.Enums.CommRelationType.FollowUser, toUserModel.UserID, CurrentUserInfo.UserID))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "关注完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "关注失败";
            }
            bllUser.ContextResponse(context, apiResp);
            return;
        }
    }
}