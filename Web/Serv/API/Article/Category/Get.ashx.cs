using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article.Category
{
    /// <summary>
    /// 文章分类详细接口
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string categoryId = context.Request["category_id"];
            if (string.IsNullOrEmpty(categoryId))
            {
                resp.errmsg = "category_id 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ArticleCategory category = bll.Get<ArticleCategory>(string.Format(" WebsiteOwner='{0}' AND CategoryType='article' AND AutoID={1}", bll.WebsiteOwner, categoryId));
            if (category == null)
            {
                resp.errmsg = "没有找到文章分类目录";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.category_name = category.CategoryName;
            requestModel.category_parent_id = category.PreID;
            requestModel.category_img_url = category.ImgSrc;
            requestModel.category_sort = category.Sort;
            requestModel.category_summary = category.Summary;
            requestModel.category_systype = category.SysType;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }
        /// <summary>
        /// 返回实体
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 分类目录名称
            /// </summary>
            public string category_name { get; set; }

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