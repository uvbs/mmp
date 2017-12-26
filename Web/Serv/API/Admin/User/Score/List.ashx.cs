using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Score
{
    /// <summary>
    /// List 的摘要说明 积分历史列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLUserScoreDetailsInfo bllUserScoreDetail = new BLLJIMP.BLLUserScoreDetailsInfo();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string startTime=context.Request["start_time"];
            string stopTime=context.Request["stop_time"];
            string keyWord=context.Request["keyword"];
            string userId=context.Request["user_id"];
            string type = context.Request["type"];
            string websiteOwner = bllUserScoreDetail.WebsiteOwner;
            int totalCount=0;
            List<UserScoreDetailsInfo> scoreList = bllUser.GetScoreDetailsList(pageSize, pageIndex, userId, keyWord, out totalCount,0,type,"","",startTime,stopTime);
            
            List<dynamic> returnList = new List<dynamic>();

            foreach (UserScoreDetailsInfo item in scoreList)
            {
                returnList.Add(new 
                {
                    score=item.Score,
                    create_time=item.AddTime,
                    add_note=item.AddNote,
                    curr_total_score=item.TotalScore,
                    true_name=bllUser.GetUserDispalyName(item.UserID)
                });
            }

           

            //当前积分
            UserInfo user = bllUser.GetUserInfo(userId);

            double currTotalScore = user != null ? user.TotalScore : 0;

            //一共获得积分

            double sumTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, userIDs: userId, sumColName: "Score", scoreWinStatus: "1");

            //一共消费积分
            double deleteTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, userIDs: userId, sumColName: "Score", scoreWinStatus: "2");

            //签到总积分

            double signinTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, "SignIn", userIDs: userId, sumColName: "Score", scoreWinStatus: "1");

            //补签总积分
            double retroactivTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, "SignIn", userIDs: userId, sumColName: "Score", scoreWinStatus: "2");

            //积分下单
            double orderTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, "OrderSubmit", userIDs: userId, sumColName: "Score", scoreWinStatus: "2");
            //分享商品获得积分
            double shareTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, "ShareProduct", userIDs: userId, sumColName: "Score", scoreWinStatus: "1");

            //发展新会员总共获得的积分
            double memberTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, "DistQRcodeMember", userIDs: userId, sumColName: "Score", scoreWinStatus: "1");

            //下单成功获得积分
            double orderSuccessTotalScore = bllUserScoreDetail.GetSumScore(websiteOwner, "OrderSuccess", userIDs: userId, sumColName: "Score", scoreWinStatus: "1");

            ResponseModel response = new ResponseModel();
            response.curr_total_score = Math.Round(currTotalScore,2);
            response.sum_total_score = Math.Round(sumTotalScore, 2);
            response.delete_total_score =Math.Round(deleteTotalScore, 2)*-1;
            response.signin_total_score =  Math.Round(signinTotalScore, 2);
            response.retroactive_total_score =  Math.Round(retroactivTotalScore, 2)*-1;
            response.order_total_score = Math.Round(orderTotalScore, 2)*-1;
            response.share_total_score =  Math.Round(shareTotalScore, 2);
            response.member_total_score = Math.Round(memberTotalScore, 2);
            response.order_success_total_score = Math.Round(orderSuccessTotalScore,2);
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new 
            {
                total=totalCount,
                rows=returnList,
                score_info = response

            }));
        }

        
    }


    public class ResponseModel
    {
        public double curr_total_score { get; set; }

        public double sum_total_score { get; set; }

        public double delete_total_score { get; set; }

        public double signin_total_score { get; set; }

        public double retroactive_total_score { get; set; }

        public double order_total_score { get; set; }

        public double share_total_score { get; set; }

        public double member_total_score { get; set; }

        public double order_success_total_score { get; set; }
    }
}