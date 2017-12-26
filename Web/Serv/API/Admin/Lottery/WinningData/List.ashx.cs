using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.WinningData
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string lotteryId = context.Request["lottery_id"];
            int totalCount = bllLottery.GetCountByKey<WXLotteryWinningDataV1>("LotteryId", lotteryId);
            List<WXLotteryWinningDataV1> data = bllLottery.GetListByKey<WXLotteryWinningDataV1>(pageSize, pageIndex, "LotteryId", lotteryId);
            var result = from p in data
                         select new ResponseModel
                         {
                             id = p.AutoID,
                             lottery_id = p.LotteryId,
                             user_id = p.UserId,
                             award_id = p.WXAwardsId,
                             award_name = p.WXAwardName
                         };

            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalcount = totalCount,
                list = result
            };
            bllLottery.ContextResponse(context, resp);
        }
        public class ResponseModel
        {
            /// <summary>
            /// 编号
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 抽奖活动编号
            /// </summary>
            public int lottery_id { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string user_id { get; set; }
            /// <summary>
            /// 奖项Id
            /// </summary>
            public int award_id { get; set; }
            /// <summary>
            /// 奖项
            /// </summary>
            public string award_name { get; set; }
           
        }
    }
}