using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ApiMgr.Module
{
    /// <summary>
    /// Delete 的摘要说明  删除api模块信息接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// api 业务逻辑层
        /// </summary>
        BLLJIMP.BLLApi bllApi = new BLLJIMP.BLLApi();
        public void ProcessRequest(HttpContext context)
        {
            string moduleId = context.Request["module_id"];
            if (string.IsNullOrEmpty(moduleId))
            {
                apiResp.msg = "module_id为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            ApiModule model = bllApi.GetApiModule(int.Parse(moduleId));
            if (model == null)
            {
                apiResp.msg = "模块不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            if (bllApi.Delete(model)>0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "操作出错";
            }
            bllApi.ContextResponse(context, apiResp);
        }
    }
}