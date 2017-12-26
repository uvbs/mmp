using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Score
{
    /// <summary>
    /// PrintDetail 的摘要说明
    /// </summary>
    public class PrintDetail : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bllUser.WebsiteOwner;
            UserScoreDetailsInfo scoreDetail = bllUser.GetByKey<UserScoreDetailsInfo>("AutoID", id, websiteOwner: websiteOwner);
            UserInfo member = bllUser.GetUserInfo(scoreDetail.UserID, websiteOwner);
            UserInfo upUser = new UserInfo();
            if(!string.IsNullOrWhiteSpace(member.DistributionOwner)) upUser = bllUser.GetUserInfo(member.DistributionOwner, websiteOwner);
            FlowAction act = new FlowAction();
            List<FlowActionDetail> actDetails = new List<FlowActionDetail>();
            List<FlowActionFile> actFiles = new List<FlowActionFile>();
            dynamic flow_action = null;
            if (!string.IsNullOrWhiteSpace(scoreDetail.RelationID) && 
                (scoreDetail.ScoreEvent == "线下注册充值" || scoreDetail.ScoreEvent == "线下充值" || scoreDetail.ScoreEvent == "申请提现"))
            {
                bool isReg = scoreDetail.ScoreEvent.Contains("注册");
                act = bllFlow.GetByKey<FlowAction>("AutoID", scoreDetail.RelationID, websiteOwner: websiteOwner);
                actDetails = bllFlow.GetListByKey<FlowActionDetail>("ActionID", scoreDetail.RelationID, websiteOwner: websiteOwner);
                actFiles = bllFlow.GetListByKey<FlowActionFile>("ActionID", scoreDetail.RelationID, websiteOwner: websiteOwner);
                UserInfo createUser = bllUser.GetUserInfo(act.CreateUserID, websiteOwner);
                List<dynamic> details = new List<dynamic>();
                foreach (var item in actDetails)
                {
                    UserInfo ru = bllUser.GetUserInfo(item.HandleUserID, websiteOwner);
                    details.Add(new
                    {
                        id = item.AutoID,
                        aid = item.ActionID,
                        fid = item.FlowID,
                        sid = item.StepID,
                        ex1 = item.Ex1,
                        ex2 = isReg ? "":item.Ex2,
                        ex3 = item.Ex3,
                        content = item.HandleContent,
                        handle_time = item.HandleDate.ToString("yyyy/MM/dd HH:mm:ss"),
                        select_date = item.HandleSelectDate.ToString("yyyy/MM/dd HH:mm:ss"),
                        stepname = item.StepName,
                        handler_phone = ru == null ? "" : ru.Phone,
                        handler_name = ru == null ? "" : ru.TrueName,
                        files = (from fp in actFiles
                                 where fp.StepID.Equals(item.StepID)
                                 select new
                                 {
                                     id = fp.AutoID,
                                     url = fp.FilePath
                                 })
                    });
                }
                flow_action = new
                {
                    id = act.AutoID,
                    flow_name = act.FlowName,
                    creater_phone = createUser.Phone,
                    creater_name = createUser.TrueName,
                    create_time = act.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    end_time = act.EndDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    member_phone = act.MemberPhone,
                    member_name = act.MemberName,
                    member_lv = act.MemberLevelName,
                    ex1 = act.StartEx1,
                    ex2 = isReg ? "" : act.StartEx2,
                    ex3 = act.StartEx3,
                    content = act.StartContent,
                    amount = act.Amount,
                    true_amount = act.TrueAmount,
                    deduct_amount = act.DeductAmount,
                    files = (from fp in actFiles
                             where fp.StepID.Equals(act.StartStepID)
                             select new {
                                 id = fp.AutoID,
                                 url = fp.FilePath
                             }),
                    details = details
                };
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new
            {
                id = scoreDetail.AutoID,
                member_phone = member.Phone,
                member_name = member.TrueName,
                time = scoreDetail.AddTime.ToString("yyyy/MM/dd HH:mm:ss"),
                score_event = scoreDetail.ScoreEvent,
                rel_id = scoreDetail.RelationID,
                serial_number = scoreDetail.SerialNumber,
                amount = scoreDetail.Score,
                event_amount = scoreDetail.EventScore,
                deduct_amount = scoreDetail.DeductScore,
                ex1 = scoreDetail.Ex1,
                ex2 = scoreDetail.Ex2,
                ex3 = scoreDetail.Ex3,
                ex4 = scoreDetail.Ex4,
                ex5 = scoreDetail.Ex5,
                flow_action = flow_action
            };
            bllUser.ContextResponse(context, apiResp);
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