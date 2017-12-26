using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// List 的摘要说明   抽奖参与者列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            int page = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int rows = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 20;
            string lotteryId = context.Request["lottery_id"];
            string isWinning=context.Request["is_winning"];
            string keyWords=context.Request["keyword"];
            string currDate = context.Request["curr_date"];

            string orderBy = " AutoId ASC ";
            StringBuilder sbWhere = new StringBuilder();

            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", bllUser.WebsiteOwner);
            if (!string.IsNullOrEmpty(lotteryId))
            {
                sbWhere.AppendFormat(" AND LotteryId={0} ", lotteryId);
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                sbWhere.AppendFormat(" AND (WXNickname like '%{0}%' or Number={0}) ", keyWords);
            }
            if (!string.IsNullOrEmpty(isWinning))
            {
                sbWhere.AppendFormat(" AND IsWinning={0} ",isWinning);
                orderBy = " WinnerDate ASC ";
            }
            if (!string.IsNullOrEmpty(currDate))
            {
                sbWhere.AppendFormat(" AND CreateDate>'{0}' ",DateTime.Parse(currDate));
            }


            List<BLLJIMP.Model.LotteryUserInfo> userList = bllUser.GetLit<BLLJIMP.Model.LotteryUserInfo>(rows, page, sbWhere.ToString(), orderBy);
            
            int total = bllUser.GetCount<BLLJIMP.Model.LotteryUserInfo>(sbWhere.ToString());

            List<RequestModel> returnList = new List<RequestModel>();

            foreach (var item in userList)
            {
                RequestModel model = new RequestModel();
                UserInfo userModel = bllUser.GetUserInfo(item.UserId);
                model.autoid = item.AutoId;
                model.userid = item.UserId;
                model.lottery_id = item.LotteryId;
                model.head_img_url = item.WXHeadimgurl;
                model.nick_name = bllUser.GetUserDispalyName(userModel);
                model.time = item.CreateDate.ToString();
                model.wintime = item.WinnerDate.ToString("yyyy-MM-dd HH:mm:ss");
                model.iswiner = item.IsWinning.ToString();
                model.number = item.Number;
                returnList.Add(model);
               
            }
            var result = new {
                list=returnList,
                totalcount=total
            };
            apiResp.msg = DateTime.Now.ToString(); 
            apiResp.status = true;
            apiResp.result = result;
            bllUser.ContextResponse(context,apiResp);

        }

        public class RequestModel
        {
            public int autoid { get; set; }

            public string userid { get; set; }

            public int lottery_id { get; set; }

            public string head_img_url { get; set; }

            public string nick_name { get; set; }

            public string time { get; set; }
            public string wintime { get; set; }

            public string iswiner { get; set; }

            public int number { get; set; }
        }
    }
}