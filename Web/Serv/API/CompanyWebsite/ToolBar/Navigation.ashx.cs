using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.CompanyWebsite.ToolBar
{
    
    /// <summary>
    /// 导航
    /// </summary>
    public class Navigation : BaseHandlerNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        BLLCompanyWebSite bllCompanyWebSite = new BLLCompanyWebSite();
        public void ProcessRequest(HttpContext context)
        {
             string key_type = context.Request["action"];
             string use_type = "nav";
             List<CompanyWebsite_ToolBar> dataList = bllCompanyWebSite.GetToolBarList(int.MaxValue, 1, bllCompanyWebSite.WebsiteOwner, use_type, key_type, false);

             var rootData = dataList.Where(p => p.PreID == 0);
             List<NavigationModel> list = new List<NavigationModel>();
             foreach (var item in rootData)
             {
                 var model = new NavigationModel();
                 model.navigation_id = item.AutoID;
                 model.navigation_name = item.ToolBarName;
                 model.navigation_link = item.ToolBarTypeValue;
                 model.navigation_type = item.ToolBarType;
                 model.navigation_ico = item.ToolBarImage;
                 model.navigation_img_url = item.ImageUrl;
                 list.Add(GetTree(model, dataList, model.navigation_id));
             }
             bllCompanyWebSite.ContextResponse(context, list);
        }


        /// <summary>
        /// 返回模型
        /// </summary>
        public class NavigationModel
        {
            /// <summary>
            /// 导航id
            /// </summary>
            public int navigation_id { get; set; }
            /// <summary>
            /// 导航名称
            /// </summary>
            public string navigation_name { get; set; }
            /// <summary>
            /// 导航类型
            /// </summary>
            public string navigation_type { get; set; }
            /// <summary>
            /// 导航链接
            /// </summary>
            public string navigation_link { get; set; }
            /// <summary>
            /// 导航图标
            /// </summary>
            public string navigation_ico { get; set; }
            /// <summary>
            /// 导航图片地址
            /// </summary>
            public string navigation_img_url { get; set; }
            /// <summary>
            /// 子节点
            /// </summary>
            public List<NavigationModel> children { get; set; }
        }

        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private NavigationModel GetTree(NavigationModel model, List<CompanyWebsite_ToolBar> list, int? parentID = 0)
        {
            var query = list.Where(m => m.PreID == parentID);
            if (query.Any())
            {
                if (model.children == null)
                {
                    model.children = new List<NavigationModel>();
                }
                foreach (var item in query)
                {

                    NavigationModel child = new NavigationModel()
                    {
                        navigation_id = item.AutoID,
                        navigation_name = item.ToolBarName,
                        navigation_link = item.ToolBarTypeValue,
                        navigation_type = item.ToolBarType,
                        navigation_ico = item.ToolBarImage,
                        navigation_img_url = item.ImageUrl
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, item.AutoID);
                }
            }
            return model;
        }
    }
}