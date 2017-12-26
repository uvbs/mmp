using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// CheckWebsiteCommRelation 的摘要说明
    /// </summary>
    public class CheckWebsiteCommRelation : BaseHandlerNoAction
    {
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request["key"];
            CommRelationType nType = new CommRelationType();
            if (!Enum.TryParse(key, out nType))
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "该关系不能识别";
                bllCommRelation.ContextResponse(context, apiResp);
                return;
            }

            if (bllCommRelation.ExistRelation(nType, bllCommRelation.WebsiteOwner, ""))
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "该关系存在";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "该关系不存在";
            }
            bllCommRelation.ContextResponse(context, apiResp);
        }
    }
}