using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLTutor bllTutor = new BLLTutor();
        BLLUserExpand bllUserExpand = new BLLUserExpand();
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        UserInfo currentUserInfo;
        CommonPlatform.Helper.DataTableHelper dtHelper = new CommonPlatform.Helper.DataTableHelper();


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if(currentUserInfo==null)
                {
                    resp.Status = -1;
                    resp.Msg = "未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string action = context.Request["Action"];
                switch (action)
                {
                    case "getUserList":
                        result = GetUserList(context);
                        break;
                    case "setTutor":
                        result = SetTutor(context);
                        break;
                    case "setVip":
                        result = SetVip(context);
                        break;
                    case "setLawyer":
                        result = SetLawyer(context);
                        break;
                    case "updateLawyer":
                        result = UpdateLawyer(context);
                        break;
                    case "setUser":
                        result = SetUser(context);
                        break;
                    case "delVIP":
                        result = delVIP(context);
                        break;
                    case "addScore":
                        result = AddScore(context);
                        break;
                    case "SendNotice":
                        result = SendNotice(context);
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

        private string GetUserList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            string type = context.Request["type"];

            int totalCount = 0;
            List<UserInfo> userList = bllUser.GetUserList(rows, page, keyword, bllUser.WebsiteOwner, out totalCount, type);

            List<dynamic> result = new List<dynamic>();
            foreach (UserInfo item in userList)
            {
                string userType = bllUser.IsTutor(item) ? "专家" : bllUser.GetUserTypeName(item.UserType);
                DataTable dt = bllUserExpand.GetUserExpands("IdCardNo,LawyerLicenseNo,LawyerLicensePhoto,IDPhoto1,IDPhoto2,UserTel,UserIsVip,UserCompanyAddress", item.UserID);
                object userIsVip = dtHelper.GetValueByDataTableTop(dt, "UserIsVip");
                object lawyerLicenseNo = dtHelper.GetValueByDataTableTop(dt, "LawyerLicenseNo");
                result.Add(new
                {
                    id = item.AutoID,
                    userId = item.UserID,
                    name = item.TrueName,
                    avatar = bllUser.GetUserDispalyAvatar(item),
                    email = item.Email,
                    phone = item.Phone,
                    company = item.Company,
                    postion = item.Postion,
                    userTotalScore = item.TotalScore,
                    userType = userType,
                    idCardNo = dtHelper.GetValueByDataTableTop(dt, "IdCardNo"),
                    tel = dtHelper.GetValueByDataTableTop(dt, "UserTel"),
                    companyAddress = dtHelper.GetValueByDataTableTop(dt, "UserCompanyAddress"),
                    lawyerLicenseNo = (lawyerLicenseNo != null && !string.IsNullOrWhiteSpace(lawyerLicenseNo.ToString())) ? lawyerLicenseNo.ToString() : "",
                    lawyerLicensePhoto = dtHelper.GetValueByDataTableTop(dt, "LawyerLicensePhoto"),
                    IDPhoto1 = dtHelper.GetValueByDataTableTop(dt, "IDPhoto1"),
                    IDPhoto2 = dtHelper.GetValueByDataTableTop(dt, "IDPhoto2"),
                    isVip = (userIsVip != null && !string.IsNullOrWhiteSpace(userIsVip.ToString())) ? "VIP" : "",
                    regtime = item.Regtime.HasValue ? item.Regtime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                    province = item.Province,
                    city = item.City,
                    district = item.District,
                    address = item.Address,
                    ex1 = item.Ex1,
                    ex2 = item.Ex2,
                    ex3 = item.Ex3,
                    ex4 = item.Ex4,
                    ex5 = item.Ex5,
                    ex6 = item.Ex6,
                    ex7 = item.Ex7,
                    ex8 = item.Ex8,
                    ex9 = item.Ex9,
                    ex10 = item.Ex10,
                    pwd = item.Password
                }); 
            }

            return Common.JSONHelper.ObjectToJson(new
            {
                rows = result,
                total = totalCount
            });
        }

        private string SetTutor(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds =  ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
			{
                if(string.IsNullOrWhiteSpace(userIds[i])) continue;
                if (!bllTutor.ExistTutor(userIds[i]))
                { 
                    UserInfo user = bllUser.GetUserInfo(userIds[i]);
                    bllTutor.UpdateTutorInfoByUserInfo(user);
                }
                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您被设置为专家！");
			}
            resp.Status = 1;
            resp.Msg = "设置完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string SetVip(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds =  ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(userIds[i])) continue;
                if (!bllUser.IsVip(userIds[i]))
                {
                    bllUserExpand.AddUserExpand(BLLJIMP.Enums.UserExpandType.UserIsVip, userIds[i], "1");
                }
                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您被设置为VIP！");
			}
            resp.Status = 1;
            resp.Msg = "设置完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 审核通过为律师
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetLawyer(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds = ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(userIds[i])) continue;
                if (bllUser.UpdateUserType(userIds[i], 3))
                {
                    bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您通过了律师审核，被设置为律师！");
                }
            }
            resp.Status = 1;
            resp.Msg = "设置完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 修改律师信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateLawyer(HttpContext context)
        {
            string userId = context.Request["UserID"];
            UserInfo user = bllUser.GetUserInfo(userId);
            user = bllUser.ConvertRequestToModel<UserInfo>(user);

            string companyAddress = context.Request["CompanyAddress"];
            string lawyerLicenseNo = context.Request["LawyerLicenseNo"];
            string lawyerLicensePhoto = context.Request["LawyerLicensePhoto"];

            if(bllUser.Update(user,
                string.Format("TrueName='{0}',Postion='{1}',Company='{2}'",user.TrueName,user.Postion,user.Company),
                string.Format("UserID='{0}'", user.UserID)) > 0)
            {

                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.UserCompanyAddress, user.UserID, companyAddress);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.LawyerLicensePhoto, user.UserID, lawyerLicensePhoto);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.LawyerLicenseNo, user.UserID, lawyerLicenseNo);

                resp.Status = 1;
                resp.Msg = "修改完成";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "修改失败";
            }
            
            return Common.JSONHelper.ObjectToJson(resp);
        }
        
        /// <summary>
        /// 审核不通过设为普通用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetUser(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds = ids.Split(',').ToList();
            string ly = context.Request["ly"];
            for (int i = 0; i < userIds.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(userIds[i])) continue;
                if (bllUser.UpdateUserType(userIds[i], 2))
                {
                    bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您没有通过律师审核，原因：" + ly + "！");
                }
            }
            resp.Status = 1;
            resp.Msg = "设置完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string delVIP(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> userIds = ids.Split(',').ToList();
            for (int i = 0; i < userIds.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(userIds[i])) continue;
                if (bllUserExpand.ExistUserExpand(BLLJIMP.Enums.UserExpandType.UserIsVip, userIds[i]))
                {
                    bllUserExpand.DeleteUserExpand(BLLJIMP.Enums.UserExpandType.UserIsVip, userIds[i]);
                    bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, userIds[i], "您的VIP被取消！");
                }
            }
            resp.Status = 1;
            resp.Msg = "取消完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string AddScore(HttpContext context)
        {
            string userId = context.Request["UserId"];
            string type = context.Request["Type"];
            int score = int.Parse( context.Request["Score"]);
            string rmk = context.Request["Rmk"];

            score = type=="-"?0-score:score;
            if (!bllUser.AddUserScoreDetail(userId, EnumStringHelper.ToString(ScoreDefineType.ManageUpdate), bllUser.WebsiteOwner, score, rmk))
            {
                resp.Status = -1;
                resp.Msg = "设置失败";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            
            resp.Status = 1;
            resp.Msg = "设置完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 系统消息发送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SendNotice(HttpContext context)
        {
            string notice = context.Request["Notice"];

            if (string.IsNullOrWhiteSpace(notice))
            {
                resp.Status = -1;
                resp.Msg = "请输入消息";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            try
            {
                BLLSystemNotice bllSystemNoticeThread = new BLLSystemNotice();
                bllSystemNoticeThread.Notice = notice;
                bllSystemNoticeThread.Site = bllSystemNotice.WebsiteOwner;
                Thread thNotice = new Thread(bllSystemNoticeThread.SendNoticeToAllUser);
                thNotice.Start();

                resp.Status = 1;
                resp.Msg = "发送完成";
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = "发送失败";
            }
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