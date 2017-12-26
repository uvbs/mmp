using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PayConfig
{
    /// <summary>
    /// 更新支付配置接口
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
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

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.payconfig_wxappid))
            {
                resp.errmsg = "payconfig_wxappid 为必填参数,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.payconfig_wxmch_id))
            {
                resp.errmsg = "payconfig_wxmch_id 为必填参数,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.payconfig_wxpartnerkey))
            {
                resp.errmsg = "payconfig_wxpartnerkey 为必填参数,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            ZentCloud.BLLJIMP.Model.PayConfig model = bllPay.GetPayConfig();
            if (model != null)
            {
                model.WXAppId = requestModel.payconfig_wxappid;
                model.WXMCH_ID = requestModel.payconfig_wxmch_id;
                model.WXPartnerKey = requestModel.payconfig_wxpartnerkey;
                if (bllPay.Update(model))
                {
                    resp.isSuccess = true;
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "修改支付配置出错";
                }
            }
            else
            {
                model =new  ZentCloud.BLLJIMP.Model.PayConfig();
                model.WebsiteOwner = bllPay.WebsiteOwner;
                model.WXAppId = requestModel.payconfig_wxappid;
                model.WXMCH_ID = requestModel.payconfig_wxmch_id;
                model.WXPartnerKey = requestModel.payconfig_wxpartnerkey;
                if (bllPay.Add(model))
                {
                    resp.isSuccess = true;
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "添加支付配置出错";
                }
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        /// <summary>
        /// 请求实体
        /// </summary>
        public class RequestModel 
        {
            /// <summary>
            /// 微信AppId
            /// </summary>
            public string payconfig_wxappid { get; set; }
            /// <summary>
            /// 微信MCH_ID
            /// </summary>
            public string payconfig_wxmch_id { get; set; }

            /// <summary>
            /// 微信 Key
            /// </summary>
            public string payconfig_wxpartnerkey { get; set; }
        }
    }
}