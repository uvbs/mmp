using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Category
{
    /// <summary>
    /// 添加分类接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
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
            if (string.IsNullOrEmpty(requestModel.category_name))
            {
                resp.errmsg = "category_name 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            WXMallCategory model = new WXMallCategory();
            model.CategoryName = requestModel.category_name;
            model.CategoryImg = requestModel.category_img_url;
            model.Description = requestModel.category_description;
            model.PreID = requestModel.category_parent_id;
            model.WebsiteOwner = bllMall.WebsiteOwner;
            if (string.IsNullOrEmpty(requestModel.type) || requestModel.type.ToLower().ToString() == "mall")
            {
                model.Type = "Mall";
            }
            else
            {
                model.Type = requestModel.type;
            }
           
            if ((!string.IsNullOrEmpty(model.CategoryName)) && model.CategoryName.Length > 20)
            {
                model.CategoryName = model.CategoryName.Substring(0,20);
            }
            if ((!string.IsNullOrEmpty(model.Description)) && model.Description.Length > 30)
            {
                model.Description = model.Description.Substring(0, 30);
            }
            if (bllMall.Add(model))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加分类出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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
            /// 分类描述
            /// </summary>
            public string category_description { get; set; }

            /// <summary>
            /// 父级id
            /// </summary>
            public int category_parent_id { get; set; }

            /// <summary>
            /// 分类图片
            /// </summary>
            public string category_img_url { get; set; }

            /// <summary>
            /// 分类 类型
            /// </summary>
            public string type { get; set; }
        }
    }
}