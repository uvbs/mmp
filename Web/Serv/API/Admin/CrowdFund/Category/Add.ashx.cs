using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CrowdFund.Category
{
    /// <summary>
    /// 添加众筹分类
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
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
            if(string.IsNullOrEmpty(requestModel.category_name))
            {
                resp.errcode=-1;
                resp.errmsg="分类名称 必填";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ArticleCategory category = new ArticleCategory();
            category.CategoryName = requestModel.category_name;
            category.CategoryType = "crowdfund";
            category.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Add(category))
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "添加分类失败";
                resp.errcode = 1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }

        public class RequestModel
        {
            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name{get;set;}
        }


    }
}