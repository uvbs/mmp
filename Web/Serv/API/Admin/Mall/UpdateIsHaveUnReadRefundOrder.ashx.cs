using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 更新退款订单
    /// </summary>
    public class UpdateIsHaveUnReadRefundOrder : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            var websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            websiteInfo.IsHaveUnReadRefundOrder=0;
            if (bll.Update(websiteInfo))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = "更新失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}