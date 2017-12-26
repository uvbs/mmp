using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Exam.Cer
{
    /// <summary>
    /// 证书查询
    /// </summary>
    public class Get : IHttpHandler, IReadOnlySessionState
    {
        BaseResponse resp = new BaseResponse();
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
             string code=context.Request["code"];
            string name=context.Request["name"];
            string idCode=context.Request["idcode"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'",bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(code))
            {
                 sbWhere.AppendFormat(" And BarCode='{0}'",code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                sbWhere.AppendFormat(" And CodeName='{0}'", name);
            }
            if (!string.IsNullOrEmpty(idCode))
            {
                sbWhere.AppendFormat(" And ModelCode='{0}'", idCode);
            }
             var data = bll.GetList<BarCodeInfo>(sbWhere.ToString());
             if (data.Count>0)
             {
                 resp.status = true;
                 resp.result = data;
             }
             context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}