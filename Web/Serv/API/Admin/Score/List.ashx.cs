using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Score
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bllDistribution = new BLLDistribution();
        BLLUserScoreDetailsInfo bll = new BLLUserScoreDetailsInfo();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string score_type = context.Request["score_type"];
            string score_events = context.Request["score_events"];
            string member = context.Request["member"];
            string start = context.Request["start"];
            string end = context.Request["end"];
            string sum_score = context.Request["sum_score"];
            string win_score = context.Request["win_score"];
            string lose_score = context.Request["lose_score"];
            string accumulationfund_score = context.Request["accumulationfund_score"];
            string taxation_score = context.Request["taxation_score"];
            string is_print = context.Request["is_print"];
            if (!string.IsNullOrWhiteSpace(end)) end = Convert.ToDateTime(end).ToString("yyyy-MM-dd 23:59:59.999");
            string websiteOwner = bll.WebsiteOwner;

            string memberUserIds = "";
            if (!string.IsNullOrWhiteSpace(member))
            {
                memberUserIds = bllUser.GetSpreadUserIds(member, websiteOwner);
            }
            int total = bll.GetScoreRowCount(bll.WebsiteOwner, score_type, userIDs: memberUserIds,
                scoreEvents: score_events, startTime: start, endTime: end, isPrint: is_print);
            List<UserScoreDetailsInfo> list = new List<UserScoreDetailsInfo>();
            if (total > 0) {
                list = bll.GetScoreList(rows, page, bll.WebsiteOwner, score_type, userIDs: memberUserIds,
                    colName: "AutoID,UserID,Score,AddNote,AddTime,ScoreEvent,EventScore,DeductScore,Ex1,Ex2,Ex3,Ex4,Ex5,RelationID,SerialNumber",
                    scoreEvents: score_events, startTime: start, endTime: end, isPrint: is_print);
            }
            double sumScore = 0;
            if (sum_score == "1"){
                sumScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: score_events,
                    startTime: start, endTime: end);
            }
            double winScore = 0;
            if (win_score == "1")
            {
                winScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: score_events,
                    startTime: start, endTime: end, scoreWinStatus: "1");
            }
            double loseScore = 0;
            if (lose_score == "1"){
                loseScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: score_events,
                    startTime: start, endTime: end, scoreWinStatus:"2");
            }
            double accumulationfundScore = 0;
            if (accumulationfund_score == "1" )
            {
                accumulationfundScore = bllDistribution.GetUserDeductScore(memberUserIds, bll.WebsiteOwner);
                //if(string.IsNullOrWhiteSpace(score_events)){
                //    accumulationfundScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: "返利,返购房补助,撤单扣返利,撤单扣购房补助,变更扣返利,变更扣购房补助,管理奖",
                //        sumColName:"DeductScore",startTime: start, endTime: end);
                //}else{
                //    string[] aevents = new string[] { "返利", "返购房补助", "撤单扣返利", "撤单扣购房补助", "变更扣返利", "变更扣购房补助", "管理奖" };
                //    List<string> evList = score_events.Split(',').Where(p => aevents.Contains(p)).ToList();
                //    if (evList.Count > 0)
                //    {
                //        string evString = ZentCloud.Common.MyStringHelper.ListToStr(evList, "", ",");
                //        accumulationfundScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: evString,
                //            sumColName: "DeductScore", startTime: start, endTime: end);
                //    }
                //}
            }
            double taxationScore = 0;
            if (taxation_score == "1")
            {
                if (string.IsNullOrWhiteSpace(score_events))
                {
                    taxationScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: "申请提现,提现退款",
                        sumColName: "DeductScore", startTime: start, endTime: end);
                }
                else
                {
                    List<string> evList = score_events.Split(',').Where(p => p == "申请提现" || p == "提现退款").ToList();
                    if (evList.Count > 0)
                    {
                        string evString = ZentCloud.Common.MyStringHelper.ListToStr(evList, "", ",");
                        taxationScore = bll.GetSumScore(bll.WebsiteOwner, score_type, userIDs: memberUserIds, scoreEvents: evString,
                            sumColName: "DeductScore", startTime: start, endTime: end);
                    }
                }
            }

            List<UserInfo> users = new List<UserInfo>();
            if (list.Count > 0)
            {
                list.Select(p => p.UserID).ToList();
                string userIds = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p => p.UserID).ToList(), "'", ",");
                users = bll.GetColMultListByKey<UserInfo>(rows, 1, "UserID", userIds, "AutoID,UserID,TrueName,WXNickname,Phone", websiteOwner: websiteOwner);
            }
            List<dynamic> rList = new List<dynamic>();
            foreach (UserScoreDetailsInfo item in list)
            {
                UserInfo nu = users.FirstOrDefault(p => p.UserID == item.UserID);

                string id = nu == null ? "" : nu.AutoID.ToString();
                string name = bllUser.GetUserDispalyName(nu);
                string phone = nu == null || string.IsNullOrWhiteSpace(nu.Phone) ? "" : nu.Phone;
                string note = item.AddNote;
                if(nu!=null){
                    note = note.Replace("转账给您", string.Format("转账给{0}[{1}]", name, phone));
                }
                else
                {
                    note = note.Replace("转账给您", string.Format("转账给[{0}]", item.UserID));
                }
                rList.Add(new
                {
                    id = item.AutoID,
                    uid = id,
                    nickname = name,
                    phone = phone,
                    score = Math.Round(item.Score, 2),
                    totalscore = Math.Round(item.TotalScore, 2),
                    addnote = note,
                    time = item.AddTime,
                    time_str = item.AddTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    score_event = item.ScoreEvent,
                    event_score = Math.Round(item.EventScore, 2),
                    deduct_score = Math.Round(item.DeductScore, 2),
                    ex1 = item.Ex1,
                    ex2 = item.Ex2,
                    ex3 = item.Ex3,
                    ex4 = item.Ex4,
                    ex5 = item.Ex5,
                    rel_id = item.RelationID,
                    serial_number = item.SerialNumber
                });
            }
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询列表完成";
            apiResp.result = new
            {
                totalcount = total,
                list = rList,
                sum = Math.Round(sumScore, 2),
                win = Math.Round(winScore, 2),
                lose = Math.Round(loseScore, 2),
                accumulationfund = Math.Round(accumulationfundScore, 2),
                taxation = Math.Round(taxationScore, 2)
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}