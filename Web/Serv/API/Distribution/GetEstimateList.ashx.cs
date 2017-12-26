using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Distribution
{
    /// <summary>
    /// GetEstimateList 的摘要说明
    /// </summary>
    public class GetEstimateList : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        BLLUserScoreDetailsInfo bllScore = new BLLUserScoreDetailsInfo();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string userId = bll.GetCurrUserID();
            string websiteOwner = bll.WebsiteOwner;
            string sum_col = context.Request["sum_col"];
            string score_events = context.Request["score_events"];
            double sum = 0;
            //int total = bllScore.GetScoreRowCount(websiteOwner, "TotalAmount", "", userId, score_events);
            int total = bll.GetUserDeductScoreTotalCount(userId, websiteOwner);

            List<UserScoreDetailsInfo> list = new List<UserScoreDetailsInfo>();
            
            if(total>0){
                //list = bllScore.GetScoreList(rows, page, websiteOwner, "TotalAmount", "", userId, "", score_events,"");
                //sum = bllScore.GetSumScore(websiteOwner, "TotalAmount", "", userId, null, score_events, "", sum_col);
                list = bll.GetUserDeductScoreList(userId, websiteOwner, page, rows);
                sum = bll.GetUserDeductScore(userId, websiteOwner);
            }

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取账面列表";
            apiResp.result = new
            {
                totalcount = total,
                sum = sum,
                list = from p in list
                       select new
                       {
                           id = p.AutoID,
                           project = p.ScoreEvent,
                           desc = p.AddNote,
                           amount = p.Score,
                           deduct_amount = p.DeductScore,
                           time_str = p.AddTime.ToString("yyyy/MM/dd HH:mm:ss")
                       }
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}