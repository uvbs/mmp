using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Category
{
    /// <summary>
    /// 获取分类详细信息接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
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
            WXMallCategory category = bllMall.Get<WXMallCategory>(string.Format(" WebsiteOwner='{0}' AND AutoID={1}",bllMall.WebsiteOwner,categoryId));
            if (category == null)
            {
                resp.errmsg = "没有找到商城分类";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.category_name = category.CategoryName;
            requestModel.category_img_url = category.CategoryImg;
            requestModel.category_description = category.Description;
            requestModel.category_parent_id = category.PreID;
            requestModel.type = category.Type;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }
        /// <summary>
        /// 请求实体
        /// </summary>
        public class RequestModel 
        {
            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }

            /// <summary>
            /// 分类图片
            /// </summary>
            public string category_img_url { get; set; }

            /// <summary>
            /// 分类描述
            /// </summary>
            public string category_description { get; set; }

            /// <summary>
            /// 父级id
            /// </summary>
            public int category_parent_id { get; set; }

            /// <summary>
            /// 分类 类型
            /// </summary>
            public string type { get; set; }
        }

    }
}