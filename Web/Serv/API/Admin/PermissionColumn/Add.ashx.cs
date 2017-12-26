using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// 添加权限栏目接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermissionColumn bllPermissionColumn = new BLLPermissionColumn();
        ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = bll.ConvertRequestToModel<RequestModel>(new RequestModel());
            if (bllPermissionColumn.AddColumn(requestModel.name, requestModel.pre_id, requestModel.order_num, requestModel.websiteOwner))
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "添加完成。";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "添加失败。";
            }
            bll.ContextResponse(context,apiResp);
        }
        public class RequestModel
        {
            /// <summary>
            /// 栏目名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 上级栏目局面
            /// </summary>
            public long pre_id { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int order_num { get; set; }
            /// <summary>
            /// 基础栏目ID
            /// </summary>
            public long base_id { get; set; }
            /// <summary>
            /// 站点名称
            /// </summary>
            public string websiteOwner { get; set; }
        }
    }
}