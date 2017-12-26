using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Config
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class Get :BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            var websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            apiResp.status = true;
            apiResp.msg = "ok"; 
            apiResp.result = new
            {
                #region 商城配置
                malll = new
                {
                    is_have_unread_refund_order = websiteInfo.IsHaveUnReadRefundOrder==1?true:false

                } 
                #endregion

                #region 其它模块配置
		        
	            #endregion

            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}