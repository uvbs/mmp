using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 商品属性值
    /// </summary>
    public class PropertyValue : BaseHandler
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 查询 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int propId = int.Parse(context.Request["property_id"]);//特征量ID
            var sourceData = bllMall.GetProductPropertyValueList(propId);
            var data = from p in sourceData
                       select new
                       {
                           property_id = p.PropID,
                           property_value_id = p.PropValueId,
                           property_value_name = p.PropValue

                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = sourceData.Count,
                list = data
            });

        }


    }
}