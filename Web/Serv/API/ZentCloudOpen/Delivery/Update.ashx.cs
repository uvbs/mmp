using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.ModelGen;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Delivery
{
    /// <summary>
    /// 更新物流信息
    /// </summary>
    public class Update : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 快递100
        /// </summary>
        BLLJIMP.BllKuaidi100 bllKuaidi100 = new BLLJIMP.BllKuaidi100();
        public void ProcessRequest(HttpContext context)
        {
            WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
            string orderSn = context.Request["order_sn"];
            string expressCompanyCode = context.Request["express_company_code"];
            string expressNumber = context.Request["express_number"];
            if (string.IsNullOrEmpty(orderSn))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(expressCompanyCode))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "express_company_code 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(expressNumber))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "express_number 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
            if (orderInfo==null)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "order_sn 不存在,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ExpressInfo expressInfo = bllMall.Get<ExpressInfo>(string.Format(" ExpressCompanyCode='{0}'", expressCompanyCode));
            if (expressInfo==null)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "express_company_code 不支持";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            orderInfo.ExpressCompanyCode = expressCompanyCode;
            orderInfo.ExpressCompanyName = expressInfo.ExpressCompanyName;
            orderInfo.ExpressNumber = expressNumber;
            orderInfo.DeliveryTime = DateTime.Now;
            if (bllMall.WebsiteOwner=="jikuwifi")
            {
                orderInfo.Status = "待归还";
            }
            else
            {
                orderInfo.Status = "已发货";
            }
            
            orderInfo.LastUpdateTime = DateTime.Now;
            orderInfo.Ex18 = "";
            orderInfo.Ex19 = "";
            if (bllMall.Update(orderInfo))
            {

                resp.status = true;
                string msg = "";
                bllKuaidi100.Poll(orderInfo.ExpressCompanyCode, orderInfo.ExpressNumber, out  msg);
            }
            else
            {
                resp.msg = "更新物流信息失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }


    }
}