using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.Record
{
    /// <summary>
    /// List 的摘要说明  获取中奖名单列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// 刮奖活动BLL
        /// </summary>
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int lotteryId = int.Parse(context.Request["lottery_id"]);
            string userId = context.Request["user_id"];
            string awardId = context.Request["award_id"];
            if (lotteryId<=0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "lottery_id 为必填项, 请检查";
                bllLottery.ContextResponse(context,resp);
                return;
            }
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format("  LotteryId={0}",lotteryId));
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId ='{0}'", userId);
            }
            if (!string.IsNullOrEmpty(awardId))
            {
                sbWhere.AppendFormat(" And WXAwardsId ={0}", awardId);
            }
            int totalCount = bllLottery.GetCount<WXLotteryRecordV1>(sbWhere.ToString());
            List<WXLotteryRecordV1> data = bllLottery.GetLit<WXLotteryRecordV1>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in data)
            {
                returnList.Add(new ResponseModel
                {
                    name=item.Name,
                    phone=item.Phone,
                    time=bllLottery.GetTimeStamp(item.InsertDate),
                    is_get_prize=item.IsGetPrize,
                    head_img_url=item.HeadImg,
                    wx_nick_name=item.WXNickName,
                    prize_name = item.WXAwardName
                });
            }
            resp.isSuccess = true;
            resp.returnObj= new 
            {
                totalcount=totalCount,
                list=returnList
            };
            bllLottery.ContextResponse(context, resp);
        }
        /// <summary>
        /// 返回模型
        /// </summary>
        public class ResponseModel
        {

            /// <summary>
            /// 姓名
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 手机
            /// </summary>
            public string phone { get; set; }

            /// <summary>
            /// 中奖时间
            /// </summary>
            public double time { get; set; }

            /// <summary>
            /// 是否已领奖 0未领取  1已领取
            /// </summary>
            public int is_get_prize { get; set; }

            /// <summary>
            /// 头像
            /// </summary>
            public string head_img_url { get; set; }

            /// <summary>
            /// 微信昵称
            /// </summary>
            public string wx_nick_name { get; set; }

            /// <summary>
            /// 奖品名称
            /// </summary>
            public string prize_name { get; set; }
        }
       
    }
}