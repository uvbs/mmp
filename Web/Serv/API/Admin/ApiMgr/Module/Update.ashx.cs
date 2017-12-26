using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ApiMgr.Module
{
    /// <summary>
    /// Update 的摘要说明  修改api模块信息接口
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// api 业务逻辑层
        /// </summary>
        BLLJIMP.BLLApi bllApi = new BLLJIMP.BLLApi();

        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                apiResp.msg = "JSON格式错误,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllApi.ContextResponse(context,apiResp);
                return;
            }
            if (requestModel.module_id <= 0)
            {
                apiResp.msg = "module_id参数为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.module_name))
            {
                apiResp.msg = "module_name参数为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            ApiModule model = bllApi.GetApiModule(requestModel.module_id);
            if (model == null)
            {
                apiResp.msg = "模块不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context,apiResp);
                return;
            }
            model.ModuleName = requestModel.module_name;
            model.Description = requestModel.module_desc;
            model.Sort = requestModel.sort;
            if (bllApi.Update(model))
            {
                apiResp.msg = "ok";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "操作出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllApi.ContextResponse(context, apiResp);
        }

        public class RequestModel
        {
            public int module_id { get; set; }
            /// <summary>
            /// 模块名称
            /// </summary>
            public string module_name { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string module_desc { get; set; }

            /// <summary>
            /// 排序
            /// </summary>
            public int sort { get; set; }
        }
    }
}