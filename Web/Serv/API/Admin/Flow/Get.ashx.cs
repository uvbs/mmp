using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Flow
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLUser bllUser = new BLLUser();
        BLLDistribution bll = new BLLDistribution();
        BLLUserScoreDetailsInfo bllScore = new BLLUserScoreDetailsInfo();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bllFlow.WebsiteOwner;
            FlowAction act = bllFlow.GetByKey<FlowAction>("AutoID", id, websiteOwner: websiteOwner);
            List<FlowActionDetail> actDetails = bllFlow.GetActionDetails(websiteOwner, act.AutoID, act.FlowID);

            string handleUserId = currentUserInfo.UserID;
            string handleGroupId = currentUserInfo.PermissionGroupID.HasValue ? currentUserInfo.PermissionGroupID.Value.ToString() : "";
            bool isCanAction = bllFlow.IsCanAction(websiteOwner, act.AutoID, handleUserId, handleGroupId);
            List<FlowActionFile> actFiles = bllFlow.GetActionFiles(websiteOwner, act.AutoID, act.FlowID);
            List<UserInfo> uList = new List<UserInfo>();
            UserInfo member = bllUser.GetUserInfo(act.MemberID, websiteOwner);
            if (member!=null) uList.Add(member);
            UserInfo cu = uList.FirstOrDefault(p => p.UserID == act.CreateUserID);
            if (cu == null){
                cu = bllUser.GetUserInfo(act.CreateUserID, websiteOwner);
                if (cu != null) uList.Add(cu);
            }
            UserInfo regUser = null;
            string regUserLevelString = "";
            double regUseAmount = 0;
            if (act.FlowKey == "CancelRegister" && !string.IsNullOrWhiteSpace(member.RegUserID))
            {
                regUser = bllUser.GetUserInfo(member.RegUserID, websiteOwner);
                UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", regUser.MemberLevel.ToString());
                if (levelConfig != null) regUserLevelString = levelConfig.LevelString;
                UserScoreDetailsInfo regScore = bllScore.GetNewScore(websiteOwner, "TotalAmount", userIDs: member.UserID, colName: "AutoID,Score", scoreEvents: "他人注册转入", startTime: member.Regtime.Value.ToString("yyyy-MM-dd"));
                if (regScore != null) regUseAmount = regScore.Score;
            }

            UserInfo upUser = null;
            string upUserLevelString = "";
            if (act.FlowKey == "RegisterOffLine" || act.FlowKey == "CancelRegister" || act.FlowKey == "EmptyBilFill")
            {
                upUser = bllUser.GetUserInfo(member.DistributionOwner, websiteOwner);
                UserLevelConfig upLevelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", upUser.MemberLevel.ToString());
                if (upLevelConfig != null) upUserLevelString = upLevelConfig.LevelString;
            }

            List<dynamic> handles = new List<dynamic>();
            foreach (var item in actDetails)
            {
                UserInfo ru = uList.FirstOrDefault(p => p.UserID == item.HandleUserID);
                if (ru == null){
                    ru = bllUser.GetUserInfo(item.HandleUserID, websiteOwner);
                    if (ru != null) uList.Add(ru);
                }
                var files = (from fp in actFiles
                             where fp.StepID.Equals(item.StepID)
                             select new
                             {
                                 id = fp.AutoID,
                                 url = fp.FilePath
                             });

                handles.Add(new
                {
                    id = item.AutoID,
                    ex1 = item.Ex1,
                    ex2 = item.Ex2,
                    ex3 = item.Ex3,
                    content = item.HandleContent,
                    handle_time = item.HandleDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    stepname = item.StepName,
                    handle_user_id = ru == null ? 0 : ru.AutoID,
                    handle_user_name = ru == null ? "" : bllUser.GetUserDispalyName(ru),
                    select_date = item.HandleSelectDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    files = files
                });
            }
            apiResp.result = new
            {
                id = act.AutoID,
                flowname = act.FlowName,
                stepname = act.StepName,
                amount = act.Amount,
                true_amount = act.TrueAmount,
                deduct_amount = act.DeductAmount,
                status = act.Status,
                ex1 = act.StartEx1,
                ex2 = act.StartEx2,
                ex3 = act.StartEx3,
                content = act.StartContent,
                select_date = act.StartSelectDate.ToString("yyyy/MM/dd HH:mm:ss"),
                member_id = member == null ? act.MemberAutoID : member.AutoID,
                member_name = member == null ? act.MemberName : bllUser.GetUserDispalyName(member),
                member_phone = member == null ? act.MemberPhone : member.Phone,
                member_regtime = member == null ? "" : member.Regtime.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                lvname = act.MemberLevelName,
                create_user_id = cu == null ? 0 : cu.AutoID,
                create_user_name = cu == null ? "" : bllUser.GetUserDispalyName(cu),
                start = act.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                end = act.EndDate.ToString("yyyy/MM/dd HH:mm:ss"),
                handles = handles,
                can_act = isCanAction,
                way = member == null ? "":member.RegisterWay,
                reg = regUser==null ? null:new
                {
                    uid = regUser.AutoID,
                    name = bllUser.GetUserDispalyName(regUser),
                    level = regUserLevelString,
                    phone = regUser.Phone,
                    amount = regUseAmount
                },
                up = upUser == null ? null : new
                {
                    uid = upUser.AutoID,
                    name = bllUser.GetUserDispalyName(upUser),
                    level = upUserLevelString,
                    phone = upUser.Phone
                },
            };
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            bllFlow.ContextResponse(context, apiResp);
        }
    }
}