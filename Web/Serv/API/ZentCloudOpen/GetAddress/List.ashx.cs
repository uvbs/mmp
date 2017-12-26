using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.GetAddress
{
    /// <summary>
    /// 自提点列表
    /// </summary>
    public class List : BaseHanderOpen
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

             int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
             int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
             string isDisable = context.Request["is_disable"];//0 启用 1 禁用
            string keyWord = context.Request["keyword"];
            int totalCount = 0;
            var sourceData = bllMall.GetAddressList(pageIndex, pageSize, keyWord, out totalCount, isDisable);
            var list = from p in sourceData
                       select new
                       {
                           id=p.GetAddressId,
                           name=p.GetAddressName,
                           address=p.GetAddressLocation,
                           is_disable=p.IsDisable,
                           imgurl = p.ImgUrl
                       };
            resp.status = true;
            resp.msg = "ok";
            resp.result = new
            {
                totalcount = totalCount,
                list = list,//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }


    }
}