using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product.Property
{
    /// <summary>
    /// 获取商品规格列表
    /// </summary>
    public class List : BaseHanderOpen
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            var sourceData = bllMall.GetProductPropertyList();
            var data = from p in sourceData
                       select new
                       {

                           property_id = p.PropID,
                           property_name = p.PropName

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