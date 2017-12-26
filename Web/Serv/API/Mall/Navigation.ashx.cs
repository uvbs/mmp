using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 导航
    /// </summary>
    public class Navigation : BaseHandler
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 顶部菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Top(HttpContext context)
        {
            var sourceData = bllMall.GetList<BLLJIMP.Model.Navigation>(string.Format(" WebsiteOwner='{0}' And NavigationLinkType='top' order by Sort DESC,AutoID ASC", bllMall.WebsiteOwner));
            var rootData = sourceData.Where(p => p.ParentId == 0);
            List<NavigationModel> list = new List<NavigationModel>();
            foreach (var item in rootData)
            {
                var model = new NavigationModel();
                model.navigation_id = item.AutoID;
                model.navigation_name = item.NavigationName;
                model.navigation_link = item.NavigationLink;
                model.navigation_img_url = bllMall.GetImgUrl(item.NavigationImage);
                list.Add(GetTree(model, sourceData, model.navigation_id));
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(list);

        }
        /// <summary>
        /// 左侧菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Left(HttpContext context)
        {

            var sourceData = bllMall.GetList<BLLJIMP.Model.Navigation>(string.Format(" WebsiteOwner='{0}' And NavigationLinkType='left' order by Sort DESC,AutoID ASC", bllMall.WebsiteOwner));
            var rootData = sourceData.Where(p => p.ParentId == 0);
            List<NavigationModel> list = new List<NavigationModel>();
            foreach (var item in rootData)
            {
                var model= new NavigationModel();
                model.navigation_id=item.AutoID;
                model.navigation_name=item.NavigationName;
                model.navigation_link=item.NavigationLink;
                model.navigation_img_url=bllMall.GetImgUrl(item.NavigationImage);
                list.Add(GetTree(model, sourceData, model.navigation_id));
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(list);

        }

        /// <summary>
        /// 左侧菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string TypeList(HttpContext context)
        {

            string navigationLinkType = context.Request["navigation_link_type"];
            var sourceData = bllMall.GetList<BLLJIMP.Model.Navigation>(string.Format(" WebsiteOwner='{0}' And NavigationLinkType='{1}' order by Sort DESC,AutoID ASC", bllMall.WebsiteOwner, navigationLinkType));
            var rootData = sourceData.Where(p => p.ParentId == 0);
            List<NavigationModel> list = new List<NavigationModel>();
            foreach (var item in rootData)
            {
                var model = new NavigationModel();
                model.navigation_id = item.AutoID;
                model.navigation_name = item.NavigationName;
                model.navigation_link = item.NavigationLink;
                model.navigation_img_url = bllMall.GetImgUrl(item.NavigationImage);
                list.Add(GetTree(model, sourceData, model.navigation_id));
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(list);

        }


        /// <summary>
        /// 底部菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Bottom(HttpContext context)
        {
            var sourceData = bllMall.GetList<BLLJIMP.Model.Navigation>(string.Format(" WebsiteOwner='{0}' And NavigationLinkType='bottom' order by Sort DESC,AutoID ASC", bllMall.WebsiteOwner));
            var rootData = sourceData.Where(p => p.ParentId == 0);
            List<NavigationModel> list = new List<NavigationModel>();
            foreach (var item in rootData)
            {
                var model = new NavigationModel();
                model.navigation_id = item.AutoID;
                model.navigation_name = item.NavigationName;
                model.navigation_link = item.NavigationLink;
                model.navigation_img_url = bllMall.GetImgUrl(item.NavigationImage);
                list.Add(GetTree(model, sourceData, model.navigation_id));
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(list);

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
            /// 导航链接
            /// </summary>
            public string navigation_link { get; set; }

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
        private NavigationModel GetTree(NavigationModel model, IList<BLLJIMP.Model.Navigation> list, int? parentID =0)
        {
            var query = list.Where(m => m.ParentId == parentID);
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
                        navigation_name = item.NavigationName,
                        navigation_link = item.NavigationLink,
                        navigation_img_url=bllMall.GetImgUrl(item.NavigationImage)
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, item.AutoID);
                }
            }
            return model;
        }




    }
}