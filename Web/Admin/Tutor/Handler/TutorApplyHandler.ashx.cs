using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// TutorApplyHander 的摘要说明
    /// </summary>
    public class TutorApplyHander : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLTutor bllTutor = new BLLTutor();
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        UserInfo currentUserInfo;
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.Status = -1;
                    resp.Msg = "未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string action = context.Request["Action"];
                switch (action)
                {
                    case "getApplyList":
                        result=getApplyList(context);
                        break;
                    case "getTutorList":
                        result = getTutorList(context);
                        break;
                    case "approvedApply":
                        result = approvedApply(context);
                        break;
                    case "deleteApply":
                        result = deleteApply(context);
                        break;
                    case "deleteTutor":
                        result = deleteTutor(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }
            context.Response.Write(result);
        }

        private string getApplyList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            List<CommRelationInfo> cris = bllCommRelation.GetRelationList(BLLJIMP.Enums.CommRelationType.ApplyToTutor, null, null, page, rows);
            int TCount = bllCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.ApplyToTutor, null, null);
            List<dynamic> result = new List<dynamic>();
            foreach (var item in cris)
            {
                UserInfo user = bllUser.GetUserInfo(item.RelationId);
                result.Add(new
                {
                    id = item.AutoId,
                    userAutoId = user.AutoID,
                    userId = user.UserID,
                    name = user.TrueName,
                    company = user.Company,
                    postion = user.Postion,
                    phone = user.Phone,
                    userFollowCount = bllCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, null, item.RelationId),
                    followUserCount = bllCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, item.RelationId, null),
                    articleCount = bllUser.GetCount<JuActivityInfo>(string.Format(" UserId ='{0}' And IsDelete=0 And IsHide=0  And WebsiteOwner='{1}'", item.RelationId, bllUser.WebsiteOwner)),
                    applyDate = item.RelationTime.ToString("yyyy-MM-dd")
                });
            }

            return Common.JSONHelper.ObjectToJson(new {
                rows=result,
                total=TCount
            });
        }
        private string getTutorList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            int TCount = 0;
            List<TutorInfo> tutors = bllTutor.GetTutorsList(page, rows, null, null, keyword, "new",out TCount);

            List<dynamic> result = new List<dynamic>();
            foreach (var item in tutors)
            {
                UserInfo user = bllUser.GetUserInfo(item.UserId);
                result.Add(new
                {
                    id = item.AutoId,
                    userAutoId = user.AutoID,
                    userId = item.UserId,
                    name = item.TutorName,
                    company = item.Company,
                    postion = item.Position,
                    userFollowCount = bllCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, null, item.UserId),
                    followUserCount = bllCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, item.UserId, null),
                    articleCount = bllUser.GetCount<JuActivityInfo>(string.Format(" UserId ='{0}' And IsDelete=0 And IsHide=0  And WebsiteOwner='{1}'", item.UserId, bllUser.WebsiteOwner)),
                    createDate = item.RDataTime.Value.ToString("yyyy-MM-dd")
                });
            }

            return Common.JSONHelper.ObjectToJson(new
            {
                rows = result,
                total = TCount
            });
        }
        private string approvedApply(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds =  ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
			{
                if(string.IsNullOrWhiteSpace(userIds[i])) continue;
                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.ApplyToTutor, null, userIds[i]) && !bllTutor.ExistTutor(userIds[i]))
                { 
                    UserInfo user = bllUser.GetUserInfo(userIds[i]);
                    bllTutor.UpdateTutorInfoByUserInfo(user);
                }
                bllCommRelation.DelCommRelation(BLLJIMP.Enums.CommRelationType.ApplyToTutor, null, userIds[i]);
                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您已通过审核成为专家！");
			}
            resp.Status = 1;
            resp.Msg = "审核完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string deleteTutor(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds =  ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
			{
                bllTutor.DelTutor(userIds[i]);
                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您的专家身份被取消！");
			}
            resp.Status = 1;
            resp.Msg = "删除完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string deleteApply(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds = ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
            {
                bllCommRelation.DelCommRelation(BLLJIMP.Enums.CommRelationType.ApplyToTutor, null, userIds[i]);
                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您的专家申请未通过审核！");
            }
            resp.Status = 1;
            resp.Msg = "删除完成";
            return Common.JSONHelper.ObjectToJson(resp);
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