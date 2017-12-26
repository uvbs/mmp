using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Navigation
{
    /// <summary>
    /// 添加商城导航接口
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
            if (string.IsNullOrEmpty(requestModel.navigation_img))
            {
                resp.errcode = 1;
                resp.errmsg = "navigation_img 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.navigation_name))
            {
                resp.errcode = 1;
                resp.errmsg = "navigation_name 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.navigation_link))
            {
                resp.errcode = 1;
                resp.errmsg = "navigation_link 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.navigation_link_type))
            {
                resp.errcode = 1;
                resp.errmsg = "navigation_link_type 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ZentCloud.BLLJIMP.Model.Navigation navigation = new BLLJIMP.Model.Navigation();
            navigation.ParentId = requestModel.pre_id;
            navigation.NavigationImage = requestModel.navigation_img;
            navigation.NavigationLink = requestModel.navigation_link;
            navigation.NavigationName = requestModel.navigation_name;
            navigation.NavigationLinkType = requestModel.navigation_link_type;
            navigation.Sort = requestModel.navigation_sort;
            navigation.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Add(navigation))
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "添加导航出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        /// <summary>
        /// 请求实体
        /// </summary>
        public class RequestModel 
        {
            /// <summary>
            /// 上级编号
            /// </summary>
            public int pre_id { get; set; }

            /// <summary>
            /// 导航图片
            /// </summary>
            public string navigation_img { get; set; }

            /// <summary>
            /// 导航名称
            /// </summary>
            public string navigation_name { get; set; }

            /// <summary>
            /// 导航链接
            /// </summary>
            public string navigation_link { get; set; }

            /// <summary>
            /// 导航类型
            /// </summary>
            public string navigation_link_type { get; set; }

            /// <summary>
            /// 导航排序
            /// </summary>
            public int navigation_sort { get; set; }

        }

    }
}