using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Relation
{
    /// <summary>
    /// GetCommRelationUserList 的摘要说明
    /// </summary>
    public class GetCommRelationUserList : BaseHandlerNoAction
    {
        /// <summary>
        /// 通用关系业务
        /// </summary>
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string userAutoId = context.Request["userAutoId"];
            string keyword = context.Request["keyword"]; //昵称搜索
            string type = context.Request["type"]; //昵称搜索
            string exchange = context.Request["exchange"]; //1时mainId，relationId互换
            string isid = context.Request["isid"]; //AutoID建立的关系

            BLLJIMP.Enums.CommRelationType nType = BLLJIMP.Enums.CommRelationType.FollowUser;
            if (!string.IsNullOrWhiteSpace(type))
            {
                if (!Enum.TryParse(type, out nType))
                {
                    apiResp.code = 1;
                    apiResp.msg = "类型格式不能识别";
                    bLLCommRelation.ContextResponse(context, apiResp);
                    return;
                }
            }
            currentUserInfo = bLLCommRelation.GetCurrentUserInfo();

            string mainId = "";
            string relationId = "";
            if (isid == "1"){
                relationId = userAutoId;
            }
            else{
                UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
                relationId = user.UserID;
            }
            if (exchange == "1"){
                mainId = relationId;
                relationId = "";
            }

            int TCount = 0;
            List<UserInfo> users = bllUser.GetRelationUserList(pageSize, pageIndex, nType, mainId, relationId, keyword, out TCount, isid == "1");

            var list = from p in users
                       select new
                       {
                           id = p.AutoID,
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           describe = p.Description,
                           phone = p.ViewType ==1?"": p.Phone,
                           score = p.TotalScore,
                           times = p.OnlineTimes,
                           userHasRelation = this.currentUserInfo == null ? false : (isid == "1" ? bLLCommRelation.ExistRelation(nType, p.AutoID.ToString(), this.currentUserInfo.AutoID.ToString()) : bLLCommRelation.ExistRelation(nType, p.UserID, this.currentUserInfo.UserID)),
                           relationTime = p.LastLoginDate.ToString("yyyy/MM/dd hh:mm:ss")
                       };

            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.status = true;
            apiResp.result = new
            {
                totalcount = TCount,
                list = list
            };
            bLLCommRelation.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}