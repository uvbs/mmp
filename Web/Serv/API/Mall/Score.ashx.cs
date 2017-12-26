using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 积分
    /// </summary>
    public class Score :BaseHandler
    {
        /// <summary>
        /// 积分BLL
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 获取积分兑换比例
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ExchangeRate(HttpContext context)
        {
            ScoreConfig scoreConfig = bllScore.GetScoreConfig();

            //if (scoreConfig!=null)
            //{
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    score = scoreConfig.ExchangeScore,
                    amount = scoreConfig.ExchangeAmount

                });
            //}
            //else
            //{
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(new
            //    {
            //        errcode =1,
            //        errmsg="尚未配置积分兑换比例"

            //    });
            //}


        }

        /// <summary>
        /// 获取订单金额可以获得的积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ScoreGetRate(HttpContext context)
        {
            ScoreConfig scoreConfig = bllScore.GetScoreConfig();
            //if (scoreConfig != null)
            //{
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    score = scoreConfig.OrderScore == null ? 0 : scoreConfig.OrderScore,
                    amount = scoreConfig.OrderAmount

                });
            //}
            //else
            //{
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(new
            //    {
            //        errcode = 1,
            //        errmsg = "尚未配置订单获取积分比例"

            //    });
            //}


        }


        /// <summary>
        /// 获取订单金额可以获得的积分V2
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ScoreGetRateV2(HttpContext context)
        {
            ScoreConfig scoreConfig = bllScore.GetScoreConfig();
            if (scoreConfig != null)
            {

                string data = context.Request["data"];
                decimal productFee = 0;//商品总价格 不包含邮费
                OrderRequestModel orderRequestModel;//订单模型
                try
                {
                    orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderRequestModel>(data);

                }
                catch (Exception ex)
                {
                    resp.errcode = 1;
                    resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }

                #region 格式检查

                if (orderRequestModel.skus == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "参数skus 不能为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                //相关检查 
                #endregion


                #region 商品检查 订单详情生成
                ///订单详情
                List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情
                orderRequestModel.skus = orderRequestModel.skus.Distinct().ToList();
                foreach (var sku in orderRequestModel.skus)
                {
                    //先检查库存
                    ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                    if (productSku == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "SKU不存在,请检查";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    if (productInfo.IsOnSale == "0")
                    {
                        resp.errcode = 1;
                        resp.errmsg = string.Format("{0}已下架", productInfo.PName);
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }


                    WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                    detailModel.PID = productInfo.PID;
                    detailModel.TotalCount = sku.count;
                    detailModel.OrderPrice = bllMall.GetSkuPrice(productSku);
                    detailModel.ProductName = productInfo.PName;
                    detailModel.SkuId = productSku.SkuId;
                    detailModel.SkuShowProp = bllMall.GetProductShowProperties(productSku.SkuId);
                    detailList.Add(detailModel);




                }
                #endregion

                productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用


                //物流费用旧的
                // orderInfo.Transport_Fee = bllMall.CalcFreight(orderRequestModel.skus.Sum(p => p.count));
                #region 运费计算
                FreightModel freightModel = new FreightModel();
                freightModel.receiver_province_code = orderRequestModel.receiver_province_code;
                freightModel.receiver_city_code = orderRequestModel.receiver_city_code;
                freightModel.receiver_dist_code = orderRequestModel.receiver_dist_code;
                freightModel.skus = orderRequestModel.skus;
                decimal freight = 0;
                string freightMsg = "";
                if (!bllMall.CalcFreight(freightModel, out freight, out  freightMsg))
                {
                    resp.errcode = 1;
                    resp.errmsg = freightMsg;
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单信息
                orderInfo.Transport_Fee = freight;
                #endregion

                #region 优惠券计算
                decimal discountAmount = 0;//优惠金额
                bool canUseCardCoupon = false;
                string msg = "";
                if (orderRequestModel.cardcoupon_id > 0)//有优惠券
                {
                    discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, bllMall.GetCurrUserID(), out canUseCardCoupon, out msg);
                    if (!canUseCardCoupon)
                    {
                        resp.errcode = 1;
                        resp.errmsg = msg;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }



                }
                //优惠券计算 
                #endregion

                //积分计算 
                decimal scoreExchangeAmount = 0;///积分抵扣的金额
                if (orderRequestModel.use_score > 0)
                {
                    scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);


                }

                //计算
                orderInfo.Product_Fee = productFee;
                orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
                orderInfo.TotalAmount -= discountAmount;//折扣金额
                orderInfo.PayableAmount = orderInfo.TotalAmount - freight;
                orderInfo.TotalAmount -= scoreExchangeAmount;//积分抵现金
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = orderInfo.PayableAmount
                  
                });
            }
            else
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    errmsg = "尚未配置订单获取积分比例"

                });
            }


        }


        /// <summary>
        /// 获取我的积分记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context) {


            if (!bllMall.IsLogin)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new { 
                errcode=(int)APIErrCode.UserIsNotLogin,
                errmsg="请先登录"
                
                });
            }

            var currentUserInfo = bllMall.GetCurrentUserInfo();
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string type = context.Request["score_type"];
            string is_amount = context.Request["is_amount"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" UserId='{0}' ", currentUserInfo.UserID));
            if (!string.IsNullOrEmpty(type))
            {
                if (type=="0")
                {
                    sbWhere.AppendFormat(" And Score <0");
                }
                else
                {
                    sbWhere.AppendFormat(" And Score >0");
                }
            }
            if (is_amount == "1")
            {
                sbWhere.AppendFormat(" And ScoreType ='AccountAmount'");
            }
            else
            {
                sbWhere.AppendFormat(" And ScoreType !='AccountAmount' And ScoreType !='TotalAmount' ");
            }
            int totalCount = bllScore.GetCount<UserScoreDetailsInfo>(sbWhere.ToString());
            var sourceData = bllScore.GetLit<UserScoreDetailsInfo>(pageSize, pageIndex, sbWhere.ToString(),"AutoID DESC");
            var list = from p in sourceData
                       select new
                       {
                           title=p.AddNote,
                           score=p.Score,
                           time=bllMall.GetTimeStamp(p.AddTime)

                       };

            var data = new
            {
                totalcount = totalCount,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);
        
        
        }


       
    }
}