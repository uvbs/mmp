using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TypeInfo
{
    /// <summary>
    /// 获取
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string typeId = context.Request["type_id"];
            if (string.IsNullOrEmpty(typeId))
            {
                apiResp.code = 1;
                apiResp.msg = "type_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            ZentCloud.BLLJIMP.Model.TypeInfo model = bll.Get<ZentCloud.BLLJIMP.Model.TypeInfo>(string.Format(" WebSiteOwner='{0}' AND TypeId={1} ", bll.WebsiteOwner, typeId));
            if (model == null)
            {
                apiResp.code = 1;
                apiResp.msg = "没有找到信息";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                type_id =model.TypeId,
                type_name =model.TypeName

            };



            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            return;
        }




    }
}