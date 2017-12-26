using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.ModelGen;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Express
{
    /// <summary>
    /// 添加快递公司
    /// </summary>
    public class Add :BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            resp.errcode = 1;
            resp.errmsg = "暂不支持添加";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
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

            if (string.IsNullOrEmpty(requestModel.express_company_name))
            {
                resp.errcode = 1;
                resp.errmsg = "express_company_name 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.express_company_code))
            {
                resp.errcode = 1;
                resp.errmsg = "express_company_code 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ExpressInfo express = new ExpressInfo();
            express.ExpressCompanyName = requestModel.express_company_name;
            express.ExpressCompanyCode = requestModel.express_company_code;
            express.WebsiteOwner = bllUser.WebsiteOwner;
            if (bllUser.Add(express))
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "添加出错";
                resp.errcode = -1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }

        public class RequestModel 
        {
            /// <summary>
            /// 快递公司名称
            /// </summary>
            public string express_company_name { get; set; }

            /// <summary>
            /// 快递公司代码
            /// </summary>
            public string express_company_code { get; set; }
        }
    }
}