using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLJIMP.BLLUserScoreDetailsInfo bll = new BLLJIMP.BLLUserScoreDetailsInfo();
        BLLJIMP.BLLJuActivity bllActivity = new BLLJIMP.BLLJuActivity();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string relation_id = context.Request["relation_id"];
            string score_type = context.Request["score_type"];
            string score_events = context.Request["score_events"];
            string allScore = context.Request["all_score"];
            string sum_score = context.Request["sum_score"];
            string month_score = context.Request["month_score"];
            string websiteOwner = bll.WebsiteOwner;
            //if (string.IsNullOrWhiteSpace(score_type)) score_type = "Reward";  --注释掉这行是因为   需要查询全部淘股币明细 且 没有影响查询赠送列表
            double sumScore = 0;
            string search_userid = "";
            if (!string.IsNullOrEmpty(allScore)) search_userid = bll.GetCurrUserID();

            if (!string.IsNullOrWhiteSpace(relation_id))
            {
                BLLJIMP.Model.JuActivityInfo activity = bllActivity.GetJuActivity(Convert.ToInt32(relation_id), false, bllActivity.WebsiteOwner);
                if (activity != null) sumScore = activity.RewardTotal;
            }
            else if(sum_score =="1"){
                sumScore = bll.GetSumScore(bll.WebsiteOwner, score_type, relation_id, search_userid, scoreEvents: score_events);
            }

            List<UserScoreDetailsInfo> list = bll.GetScoreList(rows, page, bll.WebsiteOwner, score_type, relation_id, search_userid, scoreEvents: score_events);
            int total = bll.GetScoreRowCount(bll.WebsiteOwner, score_type, relation_id, search_userid, scoreEvents: score_events);

            List<UserInfo> users = new List<UserInfo>();
            if(list.Count>0){
                string userIds = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p=>p.UserID).ToList(),"'",",");
                users = bll.GetColMultListByKey<UserInfo>(rows, 1, "UserID", userIds, "AutoID,UserID,TrueName,WXNickname", websiteOwner: websiteOwner);
            }
            Dictionary<string, double> months = new Dictionary<string, double>();
            List<dynamic> rList = new List<dynamic>();
            foreach (UserScoreDetailsInfo item in list)
	        {
                UserInfo nu = users.FirstOrDefault(p=>p.UserID == item.UserID);
                double monthScore = 0;
                if (month_score == "1"){
                  string month = item.AddTime.ToString("yyyy-MM");
                  if (months.ContainsKey(month))
                  {
                      monthScore = months[month];
                  }
                  else
                  {
                      monthScore = bll.GetSumScore(bll.WebsiteOwner, score_type, relation_id, search_userid, scoreEvents: score_events, month: month);
                      months.Add(month, monthScore);
                  }
                }
                rList.Add(new{
                    id = item.AutoID,
                    uid = nu==null?"": nu.AutoID.ToString(),
                    nickname= bllUser.GetUserDispalyName(nu),
                    score = Math.Round(item.Score, 2),
                    totalscore = Math.Round(item.TotalScore, 2),
                    addnote=item.AddNote,
                    time = item.AddTime,
                    time_str = item.AddTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    score_event = item.ScoreEvent,
                    event_score = Math.Round(item.EventScore, 2),
                    deduct_score = Math.Round(item.DeductScore, 2),
                    month_score = Math.Round(monthScore, 2)
                });
	        }
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询列表完成";
            apiResp.result = new
            {
                total = total,
                list = rList,
                sum = Math.Round(sumScore,2)
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}