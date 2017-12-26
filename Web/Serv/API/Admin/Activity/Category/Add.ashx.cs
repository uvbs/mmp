using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.Category
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
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
            if (string.IsNullOrEmpty(requestModel.category_name))
            {
                resp.errcode = 1;
                resp.errmsg = "category_name 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ArticleCategory category = new ArticleCategory();
            category.CategoryName = requestModel.category_name;
            category.CategoryType = "activity";
            category.CreateTime = DateTime.Now;
            category.WebsiteOwner = bll.WebsiteOwner;
            category.Sort = requestModel.category_sort;
            category.Summary = requestModel.category_summary;
            category.SysType = requestModel.category_systype;
            category.ImgSrc = requestModel.category_img_url;
            category.PreID = requestModel.category_parent_id;
            if (bll.Add(category))
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
                resp.errcode = 0;
            }
            else
            {
                resp.errmsg = "添加活动分类目录出错";
                resp.errcode = 1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


        public class RequestModel 
        {
            /// <summary>
            /// 分类目录名称
            /// </summary>
            public string category_name { get; set; }

            /// <summary>
            /// 分类目录类型
            /// </summary>
            public string category_type { get; set; }

            /// <summary>
            /// 父级id
            /// </summary>
            public int category_parent_id { get; set; }

            /// <summary>
            /// sort 从小到大
            /// </summary>
            public int category_sort { get; set; }

            /// <summary>
            /// 系统类型
            /// </summary>
            public int category_systype { get; set; }

            /// <summary>
            /// 图片地址
            /// </summary>
            public string category_img_url { get; set; }


            /// <summary>
            /// 分类介绍
            /// </summary>
            public string category_summary { get; set; }
        }

    }
}