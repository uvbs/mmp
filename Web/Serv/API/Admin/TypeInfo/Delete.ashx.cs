using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TypeInfo
{
    /// <summary>
    /// 删除
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {

            string typeIds=context.Request["type_ids"];
            if (string.IsNullOrEmpty(typeIds))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "type_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (bll.Delete(new ZentCloud.BLLJIMP.Model.TypeInfo(), string.Format(" WebsiteOwner='{0}' and TypeId in ({1}) ", bll.WebsiteOwner, typeIds)) == typeIds.Split(',').Length)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = "删除失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }



    }
}