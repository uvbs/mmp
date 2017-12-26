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
    /// 更新快递公司
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {  
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            resp.errcode =1;
            resp.errmsg = "暂不支持编辑";
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
            if (requestModel.express_company_id<=0)
            {
                resp.errcode = 1;
                resp.errmsg = "express_company_id 为必填项,请检查";
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
            express.AutoID =requestModel.express_company_id;
            express.ExpressCompanyName = requestModel.express_company_name;
            express.ExpressCompanyCode = requestModel.express_company_code;
            if (bll.Update(express))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "修改出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }
        public class RequestModel
        {
            /// <summary>
            /// 快递公司编号
            /// </summary>
            public int express_company_id { get; set; }
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