using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.ProductPrice
{
    /// <summary>
    /// 添加商品价格配置
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                apiResp.code =-1;
                apiResp.msg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            if (requestModel.config_list==null||requestModel.config_list==null)
            {
                apiResp.code =-1;
                apiResp.msg = "配置不能为空,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {

                foreach (var item in requestModel.config_list)
                {
                    ProductPriceConfig model = new ProductPriceConfig();
                    model.WebsiteOwner = bllMall.WebsiteOwner;
                    model.Date = item.date;
                    model.Price = item.price;
                    model.ProductId = item.product_id;
                    model.SkuId = item.sku_id;
                    if (!bllMall.Add(model))
                    {
                        tran.Rollback();
                        apiResp.code = -1;
                        apiResp.msg = "操作失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                        
                    }


                }
                tran.Commit();
                apiResp.msg = "ok";

            }
            catch (Exception ex)
            {

                tran.Rollback();
                apiResp.code = -1;
                apiResp.msg = ex.Message;
            }

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }


        /// <summary>
        /// 请求实体
        /// </summary>
        public class RequestModel
        {
            public List<ProductConfigModel> config_list { get; set; }
        }
        /// <summary>
        /// 商品价格配置模型
        /// </summary>
        public class ProductConfigModel {

            /// <summary>
            /// 商品编号
            /// </summary>
            public string product_id { get; set; }
            /// <summary>
            /// 日期 格式2016/05/30
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// skuid
            /// </summary>
            public string sku_id { get; set; }
        
        }

    }
}