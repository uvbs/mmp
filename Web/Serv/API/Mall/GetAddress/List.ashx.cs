using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.GetAddress
{
    /// <summary>
    /// 自提点
    /// </summary>
    public class List :BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            int totalCount = 0;
            var sourceData = bllMall.GetAddressList(pageIndex, pageSize, keyWord, out totalCount,"0");
            var list = from p in sourceData
                       select new
                       {
                           id = p.GetAddressId,
                           name = p.GetAddressName,
                           address = p.GetAddressLocation,
                           imgurl = p.ImgUrl
                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list,//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}