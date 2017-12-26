using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ZentCloud.JubitIMP.Web
{
    public class RouteModel
    {
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路由URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 处理路由的网页
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// 路由配置
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            //TODO:后期可考虑改为文件配置或者数据库配置

            List<RouteModel> routeList = new List<RouteModel>() {
                


                #region API配置


                #endregion


                #region 定制化应用

                //new RouteModel{ Name="", Url = "/{*P1}", Path="~/customize/ubi/view/index.aspx" },//ubi主页地址
                new RouteModel{ Name="m", Url = "m/{*P1}", Path="~/customize/ubi/m/app.aspx" },
                new RouteModel{ Name="haima", Url = "m/{*P1}", Path="~/customize/HaiMaPlatform/m/app.aspx" },
                new RouteModel{ Name="yingfu", Url = "m/{*P1}", Path="~/customize/EducationFirst/m/app.aspx" },
                new RouteModel{ Name="pureCar", Url = "pureCar/{*P1}", Path="~/customize/pureCar/m/app.aspx" },
                
                #endregion

                #region 基本配置
                new RouteModel{ Name="", Url = "/{*P1}", Path="~/adminlogin.aspx" },//站点后台登录
                new RouteModel{ Name="login", Url = "login/{*P1}", Path="~/adminlogin.aspx" },//站点后台登录
                new RouteModel{ Name="adminlogin", Url = "admin/{*P1}", Path="~/adminlogin.aspx" },//超级管理后台登录
                new RouteModel{ Name="index", Url = "index/{*P1}", Path="~/IndexV2.aspx" },//站点后台管理
                new RouteModel{ Name="index2", Url = "index/{*P1}", Path="~/index.aspx" },//站点后台管理
                
	            #endregion

                new RouteModel{ Name="distributionCenter", Url = "distributionCenter/{*P1}", Path="~/App/Distribution/m/index.aspx" },
                
                //new RouteModel{ Name="jikuhome", Url = "jikuhome/{*P1}", Path="~/customize/jikuwifi/Index.aspx" },
                //new RouteModel{ Name="jikuProductList", Url = "jikuProductList/{*P1}", Path="~/customize/jikuwifi/Index.aspx" },
                //new RouteModel{ Name="jikuProductDetail", Url = "jikuProductDetail/{*P1}", Path="~/customize/jikuwifi/Index.aspx" },

            };
            foreach (var item in routeList)
            {
                routes.MapPageRoute(
                     item.Name,
                     item.Name,
                     item.Path
                 );
            }
            //列表模板路由
            routes.MapPageRoute("mobile", "mobile/{page}/{preid}/{id}", "~/customize/comeoncloud/m/Index1.aspx", false, new RouteValueDictionary { { "page", "app" }, { "preid", "0" }, { "id", "0" } });

        }
    }
}