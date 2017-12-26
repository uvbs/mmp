using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ApiMgr.Parameter
{
    /// <summary>
    /// Add 的摘要说明  添加参数接口
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
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.api_id <= 0)
            {
                apiResp.msg = "api_id参数为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.parameter_name))
            {
                apiResp.msg = "parameter_name参数为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllApi.ContextResponse(context, apiResp);
                return;
            }
            ApiParameterInfo model = new ApiParameterInfo();
            model.ApiId = requestModel.api_id;
            model.IsNull = requestModel.parameter_isnull;
            model.ParameterName = requestModel.parameter_name;
            model.DataType = requestModel.data_type;
            model.Description = requestModel.parameter_desc;
            if (bllApi.Add(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
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
            /// <summary>
            /// 参数名称
            /// </summary>
            public string parameter_name { get; set; }

            /// <summary>
            /// 是否必传   0否  1是
            /// </summary>
            public int parameter_isnull { get; set; }

            /// <summary>
            /// 对应主表id
            /// </summary>
            public int api_id { get; set; }

            /// <summary>
            /// 数据类型
            /// </summary>
            public string data_type { get; set; }

            /// <summary>
            /// 参数描述
            /// </summary>
            public string parameter_desc { get; set; }
        }
    }
}