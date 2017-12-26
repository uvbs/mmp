using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 运费模块
    /// </summary>
    public class Freight : BaseHandler
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        /// <summary>
        /// 计算运费 统一计算 旧的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Calc(HttpContext context)
        {

            string data = context.Request["data"];
            SkuList skuList;
            try
            {
                skuList = ZentCloud.Common.JSONHelper.JsonToModel<SkuList>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            int totalCount = skuList.skus.Sum(p => p.count);
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                freight = bllMall.CalcFreight(totalCount)
            });


        }


        /// <summary>
        /// 计算运费按运费模板计算 新的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CalcFreight(HttpContext context)
        {

            string data = context.Request["data"];
            FreightModel freightModel;//接收运费模板模型
            try
            {
                freightModel = ZentCloud.Common.JSONHelper.JsonToModel<FreightModel>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (freightModel.receiver_province_code == 0 && freightModel.receiver_city_code == 0 && freightModel.receiver_dist_code == 0)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "无运费",
                    freight = 0
                });
            }
            if (freightModel.skus.Count==0)
            {
                resp.errcode = 1;
                resp.errmsg = "skus不能为空" ;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            decimal freight = 0;
            string msg = "";
            var isSuccess = bllMall.CalcFreight(freightModel, out  freight, out msg);
            if (isSuccess)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg =msg,
                    freight = freight
                });
            }
            else
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    errmsg = msg
                });
            }



        }






        /// <summary>
        /// SKU 列表模型
        /// </summary>
        public class SkuList
        {

            /// <summary>
            /// SKu 列表
            /// </summary>
            public List<SkuModel> skus { get; set; }


        }

        /// <summary>
        /// SKU 模型
        /// </summary>
        public class SkuModel
        {
            /// <summary>
            /// SKU  编号
            /// </summary>
            public int sku_id { get; set; }
            /// <summary>
            /// 数量
            /// </summary>
            public int count { get; set; }


        }




    }
}