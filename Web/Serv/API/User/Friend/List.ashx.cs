using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Friend
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            string relationId = context.Request["rel_id"];
            int page = 1;
            int rows = int.MaxValue;
            if (!string.IsNullOrWhiteSpace(context.Request["page"])) page = Convert.ToInt32(context.Request["page"]);
            if (!string.IsNullOrWhiteSpace(context.Request["rows"])) rows = Convert.ToInt32(context.Request["rows"]);

            UserInfo CurrentUserInfo = bllUser.GetCurrentUserInfo();
            if (string.IsNullOrWhiteSpace(relationId) && CurrentUserInfo!=null) relationId = CurrentUserInfo.AutoID.ToString();
            if (string.IsNullOrWhiteSpace(relationId)) relationId = "-999";
            List<CommRelationInfo> rellist = bLLCommRelation.GetRelationList(CommRelationType.Friend, null, relationId, 1, int.MaxValue, colName: "AutoId,MainId");
            if (rellist.Count == 0)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "查询完成";
                apiResp.status = true;
                apiResp.result = new
                {
                    totalcount = 0,
                    list = rellist
                };
                bLLCommRelation.ContextResponse(context, apiResp);
                return;
            }

            string autoIds = ZentCloud.Common.MyStringHelper.ListToStr(rellist.Select(p => p.MainId).ToList(), "", ",");
            int total = 0;
            List<UserInfo> uList = bllUser.GetUserList(page,rows,null,null,null,null,null,out total,autoIds,
                "AutoID,UserID,WXNickname,TrueName,WebsiteOwner,WXHeadimgurl,Avatar,Phone,ViewType,TotalScore,OnlineTimes,Description");

            var list = from p in uList
                             join r in rellist
                             on p.AutoID.ToString() equals r.MainId
                             orderby r.AutoId descending
                       select new
                       {
                           id = p.AutoID,
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           describe = p.Description,
                           phone = p.ViewType == 1 ? "" : p.Phone,
                           score = p.TotalScore,
                           times = p.OnlineTimes,
                           userHasRelation = CurrentUserInfo == null ? false : bLLCommRelation.ExistRelation(CommRelationType.Friend, p.AutoID.ToString(), CurrentUserInfo.AutoID.ToString())
                       };



            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.status = true;
            apiResp.result = new
            {
                totalcount = total,
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