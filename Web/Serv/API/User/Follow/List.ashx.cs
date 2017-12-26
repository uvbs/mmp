using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Follow
{
    /// <summary>
    /// List 的摘要说明   我关注人的的接口列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 通用关系业务
        /// </summary>
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount = 0;
            var list = this.bLLCommRelation.GetRelationListDesc(BLLJIMP.Enums.CommRelationType.FollowUser, null, CurrentUserInfo.UserID
                , pageIndex, pageSize, out totalCount);

            apiResp.status = true;
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in list)
            {
                UserInfo userInfo = bllUser.Get<UserInfo>(string.Format("  UserID='{0}'", item.MainId));
                if (userInfo == null) continue;

                returnList.Add(new
                {
                    id = userInfo.AutoID,
                    avatar = userInfo.Avatar,
                    nick_name = userInfo.WXNickname,
                    birthday = DateTimeHelper.DateTimeToUnixTimestamp(userInfo.Birthday),
                    gender = userInfo.Gender,
                    identification = userInfo.Ex5,
                    distance = userInfo.Distance
                });
            }
            apiResp.result = new 
            {
                totalcount = totalCount,
                list=returnList
            };
            bllUser.ContextResponse(context, apiResp);
        }
    }
}