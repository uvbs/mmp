using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 限时特卖活动商品
    /// </summary>
    public class PromotionActivityProduct : BaseHandler
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// KeyValue
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {


            string promotionActivityId = context.Request["promotion_activity_id"];//限时特卖活动ID
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;//页码
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;//页数
            pageSize = 1000;
            BLLJIMP.Model.PromotionActivity promotionActivity = new BLLJIMP.Model.PromotionActivity();
            if (!string.IsNullOrEmpty(promotionActivityId))
            {
                promotionActivity = bllMall.Get<BLLJIMP.Model.PromotionActivity>(string.Format(" ActivityId={0}", promotionActivityId));

            }
            int totalCount = 0;
            var sourceData = bllMall.GetPromotionProductList(context, out totalCount);
            //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\WXOpenOAuthDevLog.txt", true, System.Text.Encoding.GetEncoding("gb2312")))
            //{
            //    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), ZentCloud.Common.JSONHelper.ObjectToJson(sourceData)));
            //}
            var list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           category_id = p.CategoryId,
                           title = p.PName,
                           quote_price = p.PreviousPrice,
                           price = bllMall.GetShowPrice(p),
                           promotion_price = bllMall.GetMinPrommotionPrice(int.Parse(p.PID)),
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           totalcount = bllMall.GetProductTotalStockPromotion(int.Parse(p.PID)),
                           tags = p.Tags,
                           promotion_count = bllMall.GetProductTotalPromotionSaleStock(int.Parse(p.PID)),
                           //start_time = bllMall.Get<BLLJIMP.Model.PromotionActivity>(string.Format(" ActivityId={0}", p.PromotionActivityId)).StartTime,
                           //stop_time = bllMall.Get<BLLJIMP.Model.PromotionActivity>(string.Format(" ActivityId={0}", p.PromotionActivityId)).StopTime,
                           start_time = promotionActivity.StartTime,
                           stop_time = promotionActivity.StopTime,
                           is_sms_remind = IsRemind(p.PromotionActivityId.ToString(), bllMall.GetCurrUserID()),
                           is_collect = IsCollectProduct(int.Parse(p.PID)),
                           promotion_activity_id = p.PromotionActivityId
                       };
            list = list.Where(p => p.promotion_count > 0).ToList();
            totalCount = list.Count();
           // list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                start_time = promotionActivity.StartTime,
                stop_time = promotionActivity.StopTime,
                totalcount = totalCount,
                list = list
            });


        }


        /// <summary>
        /// 限时特卖活动商品模型
        /// </summary>
        public class PromotionActivityProductModel
        {
            /// <summary>
            /// 商品Id
            /// </summary>
            public int product_id { get; set; }
            /// <summary>
            /// 分类Id
            /// </summary>
            public string category_id { get; set; }

            /// <summary>
            /// 商品名称
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 吊牌价
            /// </summary>
            public decimal quote_price { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// 限时特卖最低价格
            /// </summary>
            public decimal promotion_price { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string img_url { get; set; }
            /// <summary>
            /// 是否上架
            /// </summary>
            public int is_onsale { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            public string tags { get; set; }
            /// <summary>
            /// 商品编码
            /// </summary>
            public string product_code { get; set; }



        }

        /// <summary>
        /// 检查是否已经提醒过
        /// </summary>
        /// <param name="promotionActivityId"></param>
        /// <param name="phone"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int IsRemind(string promotionActivityId, string userId)
        {

            if (promotionActivityId == "0")
            {
                return 0;

            }
            if (string.IsNullOrEmpty(userId))
            {
                return 0;

            }
            string strWhere = string.Format("MainId='{0}' And RelationId='{1}' And RelationType='PromotionSmsRemind'", promotionActivityId, userId);
            int recordCount = bllMall.GetCount<CommRelationInfo>(strWhere);
            if (recordCount > 0)
            {
                return 1;
            }
            return 0;

        }

        /// <summary>
        /// 是否已经收藏商品
        /// </summary>
        /// <returns></returns>
        private int IsCollectProduct(int productId)
        {

            if (!bllMall.IsLogin)
            {
                return 0;
            }
            UserInfo currentUserInfo = bllMall.GetCurrentUserInfo();
            string strWhere = string.Format("MainId='{0}' And RelationId='{1}' And RelationType='ProductCollect'", currentUserInfo.UserID, productId);
            int totalCount = bllMall.GetCount<CommRelationInfo>(strWhere);
            if (totalCount >= 1)
            {
                return 1;
            }
            return 0;


        }

    }
}