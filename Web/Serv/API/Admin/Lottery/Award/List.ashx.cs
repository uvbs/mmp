using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.Award
{
    /// <summary>
    /// 奖项列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            var id = Convert.ToInt32(context.Request["id"]);

            List<WXAwardsV1> OldAwardList = bllLottery.GetAwardsListV1(id);//旧奖项


            var result = from p in OldAwardList
                         select new ResponseAwardModel
                         {
                             id = p.AutoID,
                             lottery_id = p.LotteryId,
                             prize_name = p.PrizeName,
                             prize_count = p.PrizeCount,
                             probability = p.Probability,
                             img = p.Img,
                             awards_type = p.AwardsType,
                             value = p.Value,
                             value_name = p.ValueName,
                             description = p.Description
                         };
            resp.isSuccess = true;
            resp.returnObj = result;
            bllLottery.ContextResponse(context, resp);
        }
        public class ResponseAwardModel
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
            /// 奖品名称
            /// </summary>
            public string prize_name { get; set; }
            /// <summary>
            /// 奖品数量
            /// </summary>
            public int prize_count { get; set; }
            /// <summary>
            /// 中奖概率 1-100
            /// </summary>
            public int probability { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string img { get; set; }
            /// <summary>
            /// 奖项类型：0为默认自定义，1积分，2优惠券
            /// </summary>
            public int awards_type { get; set; }
            /// <summary>
            /// 价值
            /// </summary>
            public string value { get; set; }

            public string value_name { get; set; }
            /// <summary>
            /// 奖项说明
            /// </summary>
            public string description { get; set; }
        }
    }
}