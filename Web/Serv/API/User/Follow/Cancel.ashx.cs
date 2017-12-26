using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Follow
{
    /// <summary>
    /// Cancel 的摘要说明   取消关注
    /// </summary>
    public class Cancel : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
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

            if (this.bLLCommRelation.DelCommRelation(BLLJIMP.Enums.CommRelationType.FollowUser, toUserModel.UserID, CurrentUserInfo.UserID))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "取消完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "取消失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}