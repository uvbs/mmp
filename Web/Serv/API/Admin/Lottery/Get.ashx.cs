using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery
{
    /// <summary>
    /// 获取单个刮奖详细信息
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            WXLotteryV1 lotteryModel = bllLottery.GetByKey<WXLotteryV1>("LotteryID", id);
            if (lotteryModel == null)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "活动没有找到";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            resp.isSuccess = true;
            
            resp.returnObj = new ResponseModel
            {
                id = lotteryModel.LotteryID,
                lottery_name = lotteryModel.LotteryName,
                lottery_content = lotteryModel.LotteryContent,
                status = lotteryModel.Status,
                max_count = lotteryModel.MaxCount,
                thumbnails_path = lotteryModel.ThumbnailsPath,
                background_color = lotteryModel.BackGroundColor,
                share_img = lotteryModel.ShareImg,
                share_desc = lotteryModel.ShareDesc,
                toolbar_button=lotteryModel.ToolbarButton,
                start_time = DateTimeHelper.DateTimeToUnixTimestamp(Convert.ToDateTime(lotteryModel.StartTime)),
                end_time = string.IsNullOrEmpty(lotteryModel.EndTime.ToString()) ? 0 : bllLottery.GetTimeStamp((DateTime)lotteryModel.StartTime),
                use_points = lotteryModel.UsePoints,
                limit_type = lotteryModel.LuckLimitType,
                win_limit_type = lotteryModel.WinLimitType,
                is_get_prize_from_mobile = lotteryModel.IsGetPrizeFromMobile,
                lottery_type=lotteryModel.LotteryType
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

            /// <summary>
            /// 活动类型
            /// </summary>
            public string lottery_type { get; set; }
            /// <summary>
            /// 活动描述
            /// </summary>
            public string lottery_content { get; set; }
            /// <summary>
            /// 状态 0停止，1运行，如果设置了开始结束时间则失效
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 最大免费次数
            /// </summary>
            public int max_count { get; set; }
            /// <summary>
            /// 背景色
            /// </summary>
            public string background_color { get; set; }
            /// <summary>
            /// 分享图片
            /// </summary>
            public string share_img { get; set; }
            /// <summary>
            /// 分享说明
            /// </summary>
            public string share_desc { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public long start_time { get; set; }
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
            /// 底部工具栏
            /// </summary>
            public string toolbar_button { get; set; }

        }
    }
}