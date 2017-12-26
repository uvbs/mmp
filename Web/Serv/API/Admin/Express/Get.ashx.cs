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
    ///获取快递公司详细信息
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {


        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string expressId = context.Request["express_company_id"];
            if (string.IsNullOrEmpty(expressId))
            {
                resp.errcode = 1;
                resp.errmsg = "express_company_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ExpressInfo expressInfo = bll.Get<ExpressInfo>(string.Format(" WebSiteOwner='{0}' AND AutoID={1} ", bll.WebsiteOwner, expressId));
            if (expressInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "没有找到快递公司";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.express_company_name = expressInfo.ExpressCompanyName;
            requestModel.express_company_code = expressInfo.ExpressCompanyCode;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
            return;
        }

        /// <summary>
        /// 模型
        /// </summary>
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