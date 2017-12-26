using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product
{
    /// <summary>
    /// 更新商品库存
    /// </summary>
    public class UpdateStock : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {

            string productCode = context.Request["product_sn"];//商品编码
            string stockStr = context.Request["stock"];//库存字符串
            string skuId = context.Request["sku_id"];//SkuId
            string skuSn=context.Request["sku_sn"];//条型码
            string storeId = "";
            string userId=context.Request["user_id"];
            string updateType = string.IsNullOrEmpty(context.Request["update_type"]) ? "cover" : context.Request["update_type"];//更新类型

            if (!string.IsNullOrEmpty(userId))
            {
               var userInfo = bllUser.GetUserInfo(userId,bllUser.WebsiteOwner);
               storeId = userInfo.AutoID.ToString();
                
            }
            int stock = 0;//库存
            if (string.IsNullOrEmpty(productCode))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "product_sn 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(stockStr))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "stock 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (!int.TryParse(stockStr, out stock))
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "stock 参数错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var productInfo = bllMall.GetProductByProductCode(productCode);
            if (productInfo == null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "product_sn 不存在,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            var skuList = bllMall.GetProductSkuList(int.Parse(productInfo.PID));
            if (skuList.Count == 0)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "sku 不存在,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #region 多规格

            if (string.IsNullOrEmpty(skuId)&&(!string.IsNullOrEmpty(skuSn)))
            {
                
                var skuBySn = skuList.SingleOrDefault(p => p.SkuSN ==skuSn);
                if (skuBySn!=null)
                {
                    skuId = skuBySn.SkuId.ToString();
                }

            }
            if (!string.IsNullOrEmpty(skuId))
            {
                var sku = skuList.SingleOrDefault(p => p.SkuId == int.Parse(skuId));
                if (sku == null)
                {
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "sku_id 不存在,请检查";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

            }
            #endregion

            #region 单品
            else
            {

                var defaultSku = skuList.SingleOrDefault(p => p.Props == null || p.Props == "");
                if (defaultSku == null)
                {
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "sku 不存在,请检查";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                skuId = defaultSku.SkuId.ToString();

            }

            #endregion


            string tableName = " ZCJ_ProductSku ";
            if (!string.IsNullOrEmpty(storeId))
            {
                tableName = " ZCJ_ProductSkuSupplier ";
            }
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();

            sbSql.AppendFormat(" Update ");
            sbSql.AppendFormat("{0}", tableName);

            sbSql.AppendFormat(" Set ");

            if (updateType == "cover")//覆盖库存
            {
                sbSql.AppendFormat(" Stock={0} ", stock);
            }
            else if (updateType == "increment")//增量库存
            {
                sbSql.AppendFormat(" Stock+={0} ", stock);
            }

            sbSql.AppendFormat(" Where SkuId={0} ", skuId);
            if (!string.IsNullOrEmpty(storeId))
            {
                sbSql.AppendFormat(" And SupplierId={0} ", storeId);
            }
            if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSql.ToString()) > 0)
            {

                resp.status = true;
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "更新库存失败";
                resp.code = (int)APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }


    }
}