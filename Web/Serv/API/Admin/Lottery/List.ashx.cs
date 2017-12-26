using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery
{
    /// <summary>
    /// 刮奖活动列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string lotteryName = context.Request["lottery_name"];
            string lotteryType = context.Request["lottery_type"];
            int totalCount = 0;
            List<WXLotteryV1> data = bllLottery.GetLotteryList(pageSize, pageIndex, lotteryName,lotteryType,bllLottery.WebsiteOwner, out totalCount);

            List<ResponseModel> result = new List<ResponseModel>();
            foreach (var item in data)
	        {
                int LogCount = bllLottery.GetWXLotteryLogCountV1(item.LotteryID);
                int UserCount = bllLottery.GetWXLotteryLogUserCountV1(item.LotteryID);
                int GetPrizeCount = bllLottery.GetWXLotteryRecordCountV1(item.LotteryID, null, 1);
                int NoGetPrizeCount = bllLottery.GetWXLotteryRecordCountV1(item.LotteryID, null, 1);

                result.Add(new ResponseModel()
                {
                    id = item.LotteryID,
                    lottery_name = item.LotteryName,
                    status = item.Status,
                    thumbnails_path = item.ThumbnailsPath,
                    start_time =bllLottery.GetTimeStamp((DateTime)item.StartTime),
                    end_time = string.IsNullOrEmpty(item.EndTime.ToString()) ? 0 : bllLottery.GetTimeStamp((DateTime)item.StartTime),
                    use_points = item.UsePoints,
                    limit_type = item.LuckLimitType,
                    win_limit_type = item.WinLimitType,
                    is_get_prize_from_mobile = item.IsGetPrizeFromMobile,
                    log_count = LogCount,
                    user_count = UserCount,
                    getprize_count = GetPrizeCount,
                    nogetprize_count = NoGetPrizeCount
                });
            };
            resp.isSuccess = true;
            resp.returnObj = new{
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
            /// 活动图片
            /// </summary>
            public string thumbnails_path { get; set; }
            /// <summary>
            ///活动名称
            /// </summary>
            public string lottery_name { get; set; }

            ///// <summary>
            ///// 活动类型
            ///// scratch 刮刮奖    
            ///// shake   摇一摇
            ///// </summary>
            //public string lottery_type { get; set; }

            /// <summary>
            /// 状态 0停止，1运行，如果设置了开始结束时间则失效
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public double start_time { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public double end_time { get; set; }
            /// <summary>
            /// 上限类型：默认0每用户多少次，1为每天多少次
            /// </summary>
            public int limit_type { get; set; }
            /// <summary>
            /// 使用积分，当超过免费次数则需要扣除积分
            /// </summary>
            public int use_points { get; set; }
            /// <summary>
            /// 中奖上限类型 默认0不允许中多次，1
            /// </summary>
            public int win_limit_type { get; set; }
            /// <summary>
            /// 是否在手机端领奖 0 后台设置已领奖 1移动端直接领奖
            /// </summary>
            public int is_get_prize_from_mobile { get; set; }
            /// <summary>
            /// 抽奖次数
            /// </summary>
            public int log_count { get; set; }
            /// <summary>
            /// 抽奖人数
            /// </summary>
            public int user_count { get; set; }
            /// <summary>
            /// 领奖人数
            /// </summary>
            public int getprize_count { get; set; }
            /// <summary>
            /// 未领奖人数
            /// </summary>
            public int nogetprize_count { get; set; }
        }
    }
}