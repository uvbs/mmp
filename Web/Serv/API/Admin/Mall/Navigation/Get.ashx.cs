using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Navigation
{
    /// <summary>
    /// 获取导航详细信息接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public  void ProcessRequest(HttpContext context)
        {
            string navigationId = context.Request["navigation_id"];
            if (string.IsNullOrEmpty(navigationId))
            {
                resp.errcode = 1;
                resp.errmsg = "navigation_id 为空,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ZentCloud.BLLJIMP.Model.Navigation navigation = bll.Get<ZentCloud.BLLJIMP.Model.Navigation>(string.Format(" WebsiteOwner='{0}' AND AutoID={1}", bll.WebsiteOwner, navigationId));
            if (navigation == null)
            {
                resp.errmsg = "没找到导航数据";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.pre_id = navigation.ParentId;
            requestModel.navigation_img = navigation.NavigationImage;
            requestModel.navigation_name = navigation.NavigationName;
            requestModel.navigation_link = navigation.NavigationLink;
            requestModel.navigation_link_type = navigation.NavigationLinkType;
            requestModel.navigation_sort = navigation.Sort;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
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