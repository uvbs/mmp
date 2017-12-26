using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery
{
    /// <summary>
    /// 添加刮奖
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = JsonConvert.DeserializeObject<RequestModel>(context.Request["data"]);
            }
            catch (Exception ex)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestModel.lottery_name))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入活动名称";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.lottery_type))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入活动类型";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (requestModel.awards == null || requestModel.awards.Count <= 0)
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "至少添加一个奖项";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            foreach (var item in requestModel.awards)
            {
                if (string.IsNullOrEmpty(item.prize_name))
                {
                    resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "奖项名称不能为空";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(item.value))
                {
                    resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "value为必填项,请检查";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }

            }

            if (requestModel.awards.Sum(p => p.probability) > 100)
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "中奖比例之和不能大于100";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            BLLTransaction tran = new BLLTransaction();
            try
            {
                WXLotteryV1 lotteryModel = new WXLotteryV1();
                lotteryModel.InsertDate = DateTime.Now;
                lotteryModel.LotteryID = int.Parse(bllLottery.GetGUID(TransacType.AddLottery));
                lotteryModel.BackGroundColor = requestModel.background_color;
                lotteryModel.LotteryType = requestModel.lottery_type;
                lotteryModel.LotteryName = requestModel.lottery_name;
                lotteryModel.LotteryContent = requestModel.lottery_content;
                lotteryModel.MaxCount = requestModel.max_count;
                lotteryModel.Status = requestModel.status;
                lotteryModel.ThumbnailsPath = requestModel.thumbnails_path;
                lotteryModel.WebsiteOwner = bllLottery.WebsiteOwner;
                if (requestModel.start_time > 0)
                {
                    lotteryModel.StartTime = bllLottery.GetTime(requestModel.start_time);
                }
                lotteryModel.ShareImg = requestModel.share_img;
                lotteryModel.ShareDesc = requestModel.share_desc;
                lotteryModel.IsGetPrizeFromMobile = requestModel.is_get_prize_from_mobile;
                lotteryModel.ToolbarButton = requestModel.toolbar_button;
                if (requestModel.end_time > 0)
                {
                    lotteryModel.EndTime = bllLottery.GetTime(requestModel.end_time);
                }
                lotteryModel.LuckLimitType = requestModel.limit_type;
                lotteryModel.UsePoints = requestModel.use_points;
                lotteryModel.WinLimitType = requestModel.win_limit_type;

                if (!bllLottery.Add(lotteryModel, tran))
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "添加活动失败";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }

                foreach (var item in requestModel.awards)
                {
                    WXAwardsV1 award = new WXAwardsV1();
                    award.LotteryId = lotteryModel.LotteryID;
                    award.PrizeCount = item.prize_count;
                    award.PrizeName = item.prize_name;
                    award.Probability = item.probability;
                    award.Img = item.img;
                    award.AwardsType = item.awards_type;
                    award.Value = item.value;
                    award.Description = item.description;

                    if (!bllLottery.Add(award, tran))
                    {
                        tran.Rollback();
                        resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                        resp.errmsg = "添加奖项失败";
                        bllLottery.ContextResponse(context, resp);
                        return;
                    }

                }
                tran.Commit();
                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            bllLottery.ContextResponse(context, resp);
        }

        public class RequestModel
        {
            /// <summary>
            /// 活动图片
            /// </summary>
            public string thumbnails_path { get; set; }

            /// <summary>
            /// 活动类型
            /// </summary>
            public string lottery_type { get; set; }

            /// <summary>
            ///活动名称
            /// </summary>
            public string lottery_name { get; set; }
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
            /// 奖项
            /// </summary>
            public List<RequestAwardModel> awards { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public long end_time { get; set; }
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

        public class RequestAwardModel
        {
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
            /// <summary>
            /// 奖项说明
            /// </summary>
            public string description { get; set; }
        }

    }
}