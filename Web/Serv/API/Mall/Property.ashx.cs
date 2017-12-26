using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class Property : BaseHandler
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ColorList(HttpContext context)
        {
            string propName = "颜色";
            var sourceData = bllMall.GetProductPropValueList(propName);
            var list = from p in sourceData
                       select new
                       {
                         
                           prop_name=p.PropValue

                       };

            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取所有尺寸列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SizeList(HttpContext context)
        {
            string propName = "尺码";
            var sourceData = bllMall.GetProductPropValueList(propName);
            var list = from p in sourceData
                       select new
                       {
                           
                           prop_name = p.PropValue

                       };

            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }


        /// <summary> 
        /// 属性列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            var sourceData = bllMall.GetProductPropertyList();
            var data = from p in sourceData
                       select new
                       {

                           property_id = p.PropID,
                           property_name = p.PropName

                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = sourceData.Count,
                list = data
            });

        }




    }
}