using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class ProductProperty : BaseHandlerNeedLoginAdmin
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 商品属性列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

          var sourceData= bllMall.GetProductPropertyList();
          var data= from p in sourceData
                       select new
                       {
                           
                           property_id=p.PropID,
                           property_name=p.PropName

                       };

          return ZentCloud.Common.JSONHelper.ObjectToJson(new
          {
              totalcount=sourceData.Count,
              list=data
          });

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            string propName=context.Request["property_name"];//属性名称
            string msg="";
            int propId = 0;
            bool result = bllMall.AddProductProperty(propName,out propId,out msg);
            if (result)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new { 
                errcode = 0,
                errmsg = "ok",
                property_id = propId
                
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
            int propId = int.Parse(context.Request["property_id"]);//属性ID
            string propName = context.Request["property_name"];//属性名称
            string msg = "";
            bool result = bllMall.UpdateProductProperty(propId,propName, out msg);
            if (result)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg =msg;
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
            string propId = context.Request["property_id"];//属性ID
            bool result = bllMall.DeleteProductProperty(propId);
            if (result)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除商品特征量失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
    }
}