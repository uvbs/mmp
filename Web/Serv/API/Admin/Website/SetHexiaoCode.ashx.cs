using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Website
{
    /// <summary>
    /// 设置站点核销码
    /// </summary>
    public class SetHexiaoCode : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLL bll = new BLL();
        public void ProcessRequest(HttpContext context)
        {

            string hexiaoCode = context.Request["hexiao_code"];
            var websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            websiteInfo.HexiaoCode = hexiaoCode;
            if (bll.Update(websiteInfo))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
                
            }
            else
            {
                apiResp.msg = "操作失败";
            }
            bll.ContextResponse(context, apiResp);

        }

    }
}