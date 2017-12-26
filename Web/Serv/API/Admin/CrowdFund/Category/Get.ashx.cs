using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CrowdFund.Category
{
    /// <summary>
    /// 获取众筹分类
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string cotegroyId = context.Request["categroy_id"];
            if (string.IsNullOrEmpty(cotegroyId))
            {
                resp.errmsg = "categroy_id 必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ArticleCategory category = bll.Get<ArticleCategory>(string.Format(" WebSiteOwner='{0}' AND AutoID={1}", bll.WebsiteOwner, cotegroyId));
            if (category == null)
            {
                resp.errcode = 1;
                resp.errmsg = "没有找到众筹分类信息";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.category_id = category.AutoID;
            requestModel.category_name = category.CategoryName;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
            return;
        }


        public class RequestModel
        {
            /// <summary>
            /// 分类编号
            /// </summary>
            public int category_id { get; set; }
            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }
        }
    }
}