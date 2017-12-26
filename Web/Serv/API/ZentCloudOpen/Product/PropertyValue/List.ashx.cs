using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product.PropertyValue
{
    /// <summary>
    /// 获取商品规格值列表
    /// </summary>
    public class List : BaseHanderOpen
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
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

            resp.status = true;
            resp.msg = "ok";
            resp.result = new
            {
                totalcount = sourceData.Count,
                list = data
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}