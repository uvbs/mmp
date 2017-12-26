using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Follow
{
    /// <summary>
    /// IsFollow 的摘要说明  判断是否关注
    /// </summary>
    public class IsFollow : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 通用关系业务
        /// </summary>
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            string toUserId = context.Request["autoid"];
            if (string.IsNullOrEmpty(toUserId))
            {
                apiResp.msg = "autoid 为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bLLCommRelation.ContextResponse(context, apiResp);
                return;
            }
            UserInfo toUserModel = bllUser.GetUserInfoByAutoID(int.Parse(toUserId));
            if (toUserModel == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "不存在关注的用户";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.FollowUser, toUserModel.UserID, CurrentUserInfo.UserID))
            {
                apiResp.msg = "已经关注";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.msg = "没有关注过此用户";
            apiResp.status = true;
            bllUser.ContextResponse(context, apiResp);
        }
    }
}