using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    ///商品属性值
    /// </summary>
    public class ProductPropertyValue : BaseHandlerNeedLoginAdmin
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 列表
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
                           property_id=p.PropID,
                           property_value_id = p.PropValueId,
                           property_value_name = p.PropValue

                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = sourceData.Count,
                list = data
            });

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            int propId = int.Parse(context.Request["property_id"]);//特征量ID
            string propValue = context.Request["property_value_name"];//属性名称
            string msg = "";
            int propertyValueId = 0;
            bool result = bllMall.AddProductPropertyValue(propId, propValue,out propertyValueId , out msg);
            if (result)
            {

                return ZentCloud.Common.JSONHelper.ObjectToJson(new { 
                errcode = 0,
                errmsg = "ok",
                property_value_id=propertyValueId
                
                });
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            int propValueId = int.Parse(context.Request["property_value_id"]);//特征量值ID
            int propId = int.Parse(context.Request["property_id"]);
            string propValueName = context.Request["property_value_name"];//特征量值名称
            string msg = "";
            bool result = bllMall.UpdateProductPropertyValue(propValueId, propId,propValueName, out msg);
            if (result)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string propValueId = context.Request["property_value_id"];//属性ID
            bool result = bllMall.DeleteProductPropertyValue(propValueId);
            if (result)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";


            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除商品特征量值失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }




    }
}