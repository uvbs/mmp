using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 限时特卖活动
    /// </summary>
    public class PromotionActivity : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord=context.Request["keyword"];
            string promotionActivityType = context.Request["promotion_activity_type"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ",bllMall.WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord)) sbWhere.AppendFormat(" And ActivityName like '%{0}%' ",keyWord);
            if (string.IsNullOrEmpty(promotionActivityType))
            {
                sbWhere.AppendFormat(" AND  (PromotionActivityType is NULL OR PromotionActivityType='') ");
            }
            else
            {
                sbWhere.AppendFormat(" AND PromotionActivityType='{0}' ", promotionActivityType);
            }
            int totalCount = bllMall.GetCount<BLLJIMP.Model.PromotionActivity>(sbWhere.ToString());
            var sourceData = bllMall.GetLit<BLLJIMP.Model.PromotionActivity>(pageSize, pageIndex,sbWhere.ToString(), " Sort DESC,ActivityId ASC");
            var list = from p in sourceData
                       select new
                       {
                           promotion_activity_id = p.ActivityId,
                           promotion_activity_name = p.ActivityName,
                           promotion_activity_summary = p.ActivitySummary,
                           promotion_activity_image = p.ActivityImage,
                           promotion_activity_start_time = p.StartTime,
                           promotion_activity_stop_time = p.StopTime,
                           promotion_activity_sort = p.Sort,
                           promotion_activity_product_count = bllMall.GetCount<BLLJIMP.Model.WXMallProductInfo>(string.Format(" PromotionActivityId={0} And IsDelete=0 And IsOnSale=1", p.ActivityId)),
                           promotion_activity_status = GetPromotionActivitySatus(p.StartTime, p.StopTime),
                           limit_buy_product_count = p.LimitBuyProductCount,
                           promotion_activity_type = p.PromotionActivityType,
                           promotion_activity_rule = p.PromotionActivityRule
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list
            });




        }

        /// <summary>
        /// 获取单个特卖活动信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string promotionActivityId = context.Request["promotion_activity_id"];

            var promotionActivity = bllMall.Get<BLLJIMP.Model.PromotionActivity>(string.Format(" WebSiteOwner='{0}' And ActivityId={1}", bllMall.WebsiteOwner,promotionActivityId));
           
                       var data= new
                       {
                           promotion_activity_id =promotionActivity.ActivityId,
                           promotion_activity_name =promotionActivity.ActivityName,
                           promotion_activity_summary =promotionActivity.ActivitySummary,
                           promotion_activity_image =promotionActivity.ActivityImage,
                           promotion_activity_start_time = promotionActivity.StartTime,
                           promotion_activity_stop_time = promotionActivity.StopTime,
                           promotion_activity_sort = promotionActivity.Sort,
                           promotion_activity_product_count = bllMall.GetCount<BLLJIMP.Model.WXMallProductInfo>(string.Format(" PromotionActivityId={0}", promotionActivity.ActivityId)),
                           promotion_activity_status = GetPromotionActivitySatus(promotionActivity.StartTime, promotionActivity.StopTime),
                           limit_buy_product_count = promotionActivity.LimitBuyProductCount,
                           promotion_activity_type = promotionActivity.PromotionActivityType,
                           promotion_activity_rule = promotionActivity.PromotionActivityRule
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data );
           




        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            PromotionActivityRequestModel requestModel;
            try
            {
                string data = context.Request["data"];
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<PromotionActivityRequestModel>(data);
            }
            catch (Exception)
            {

                resp.errcode = 1;
                resp.errmsg = "json格式错误,请检查。";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            BLLJIMP.Model.PromotionActivity model = new BLLJIMP.Model.PromotionActivity();
            model.ActivityId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddPromotionActivity));
            model.ActivityImage = requestModel.promotion_activity_image;
            model.ActivityName = requestModel.promotion_activity_name;
            model.ActivitySummary = requestModel.promotion_activity_summary;
            model.InsertDate = DateTime.Now;
            model.StartTime = requestModel.promotion_activity_start_time;
            model.StopTime = requestModel.promotion_activity_stop_time;
            model.Sort = requestModel.promotion_activity_sort;
            model.PromotionActivityType = requestModel.promotion_activity_type;
            model.PromotionActivityRule = requestModel.promotion_activity_rule;
            model.WebSiteOwner = bllMall.WebsiteOwner;

            int limitBuyProductCount = 0;
            if (int.TryParse(requestModel.limit_buy_product_count, out limitBuyProductCount))
            {
                model.LimitBuyProductCount = limitBuyProductCount;
            }
            
            if (bllMall.Add(model))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";


            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            PromotionActivityRequestModel requestModel;
            try
            {
                string data = context.Request["data"];
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<PromotionActivityRequestModel>(data);
            }
            catch (Exception)
            {

                resp.errcode = 1;
                resp.errmsg = "json格式错误,请检查。";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                BLLJIMP.Model.PromotionActivity model = bllMall.Get<BLLJIMP.Model.PromotionActivity>(string.Format(" WebSiteOwner='{0}' And ActivityId={1}", bllMall.WebsiteOwner, requestModel.promotion_activity_id));
                model.ActivityImage = requestModel.promotion_activity_image;
                model.ActivityName = requestModel.promotion_activity_name;
                model.ActivitySummary = requestModel.promotion_activity_summary;
                model.StartTime = requestModel.promotion_activity_start_time;
                model.StopTime = requestModel.promotion_activity_stop_time;
                model.Sort = requestModel.promotion_activity_sort;
                model.PromotionActivityType = requestModel.promotion_activity_type;
                model.PromotionActivityRule = requestModel.promotion_activity_rule;
                int limitBuyProductCount = 0;
                if (int.TryParse(requestModel.limit_buy_product_count, out limitBuyProductCount))
                {
                    model.LimitBuyProductCount = limitBuyProductCount;
                }

                if (!bllMall.Update(model, tran))
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                //TODO
                //更新SKU 表时间
                string sql = string.Format(" Update ZCJ_ProductSku Set PromotionStartTime={0},PromotionStopTime={1} where ProductId in( Select PID from ZCJ_WXMallProductInfo where PromotionActivityId={2});", model.StartTime, model.StopTime, model.ActivityId);//更新SKU表
                sql += string.Format(" Update ZCJ_WXMallProductInfo set IsPromotionProduct=1,PromotionStartTime={0},PromotionStopTime={1} where PromotionActivityId={2}", model.StartTime, model.StopTime, model.ActivityId);//更新商品表
                ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sql, tran);
                tran.Commit();
                resp.errcode = 0;
                resp.errmsg = "ok";

                bllMall.ClearProductListCacheByWhere(string.Format(" PromotionActivityId={0} ", model.ActivityId));

            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.Message;
                tran.Rollback();
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string ids = context.Request["promotion_activity_ids"];
            ///TODO 
            ///1.修改SKU表信息
            ///2.修改商品表信息
            ///3.删除特卖活动表信息
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                string sql = "";
                sql += string.Format(" Update  ZCJ_ProductSku Set PromotionPrice=0,PromotionStock=0,PromotionSaleStock=0,PromotionStartTime=0,PromotionStopTime=0 where ProductId  in (select PID from ZCJ_WXMallProductInfo where PromotionActivityId in ({0}) And WebSiteOwner='{1}');", ids, bllMall.WebsiteOwner);
                sql += string.Format(" Update ZCJ_WXMallProductInfo Set IsPromotionProduct=0,PromotionStartTime=0,PromotionStopTime=0,PromotionActivityId='' where PromotionActivityId in ({0}) And WebSiteOwner='{1}';", ids, bllMall.WebsiteOwner);
                sql += string.Format(" Delete from ZCJ_PromotionActivity Where ActivityId in ({0}) And WebSiteOwner='{1}';", ids, bllMall.WebsiteOwner);
                ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sql, tran);
                tran.Commit();
                resp.errcode = 0;
                resp.errmsg = "ok";

                bllMall.ClearProductListCacheByWhere(string.Format(" PromotionActivityId in ({0}) ", ids));

            }
            catch (Exception ex)
            {

                tran.Rollback();
                resp.errcode = -1;
                resp.errmsg = ex.Message;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取限时特卖活动状态
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="stopTime"></param>
        /// <returns></returns>
        private string GetPromotionActivitySatus(double startTime, double stopTime)
        {

            double dtNowStamp = bllMall.GetTimeStamp(DateTime.Now);
            if (dtNowStamp < startTime)
            {
                return "待开始";
            }
            if (dtNowStamp > stopTime)
            {
                return "已经结束";
            }
            if (dtNowStamp >= startTime && dtNowStamp <= stopTime)
            {
                return "进行中";
            }
            return "";

        }

        /// <summary>
        /// 限时特卖活动请求模型
        /// </summary>
        public class PromotionActivityRequestModel
        {

            /// <summary>
            /// 限时特卖活动Id
            /// </summary>
            public int promotion_activity_id { get; set; }
            /// <summary>
            /// 限时特卖活动名称
            /// </summary>
            public string promotion_activity_name { get; set; }
            /// <summary>
            /// 限时特卖活动描述
            /// </summary>
            public string promotion_activity_summary { get; set; }
            /// <summary>
            /// 限时特卖活动图片
            /// </summary>
            public string promotion_activity_image { get; set; }
            /// <summary>
            /// 限时特卖活动开始时间
            /// </summary>
            public long promotion_activity_start_time { get; set; }
            /// <summary>
            /// 限时牧场活动结束时间
            /// </summary>
            public long promotion_activity_stop_time { get; set; }
            /// <summary>
            /// 排序号 
            /// </summary>
            public int promotion_activity_sort { get; set; }
            /// <summary>
            /// 购买限制
            /// </summary>
            public string limit_buy_product_count { get; set; }

            /// <summary>
            /// 特卖类型
            /// </summary>
            public string promotion_activity_type { get; set; }

            /// <summary>
            /// 特卖配置 （件数特卖使用）
            /// </summary>
            public string promotion_activity_rule { get; set; }

        }


    }
}