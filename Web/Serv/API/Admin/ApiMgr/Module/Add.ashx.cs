using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ApiMgr.Module
{
    /// <summary>
    /// Add 的摘要说明  添加模块接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
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
            if (string.IsNullOrEmpty(requestModel.module_name))
            {
                apiResp.msg = "module_name参数为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            ApiModule model = new ApiModule();
            model.ModuleName = requestModel.module_name;
            model.Sort = requestModel.sort;
            model.Description = requestModel.module_desc;
            if (bllApi.Add(model))
            {
                apiResp.msg = "ok";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "添加api模板出错";
            }
            bllApi.ContextResponse(context, apiResp);
        }

        public class RequestModel 
        {
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