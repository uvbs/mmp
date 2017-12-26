using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// 更新权限栏目
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermissionColumn bllPermissionColumn = new BLLPermissionColumn();
        ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = bll.ConvertRequestToModel<RequestModel>(new RequestModel());
            BLLPermission.Model.PermissionColumn nPermissionColumn = bllPermissionColumn.GetByKey<BLLPermission.Model.PermissionColumn>("PermissionColumnID", requestModel.id.ToString());
            if (nPermissionColumn == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "未找到。";
                bllPermissionColumn.ContextResponse(context, apiResp);
                return;
            }
            nPermissionColumn.OrderNum = requestModel.order_num;
            nPermissionColumn.PermissionColumnName = requestModel.name;
            nPermissionColumn.PermissionColumnPreID = requestModel.pre_id;
            if (nPermissionColumn.PermissionColumnPreID == nPermissionColumn.PermissionColumnID)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "所属栏目不能选择自己。";
                bllPermissionColumn.ContextResponse(context, apiResp);
                return;
            }
            bool isResult = false;
            if (string.IsNullOrWhiteSpace(requestModel.websiteOwner) || requestModel.websiteOwner == nPermissionColumn.WebsiteOwner)
            {
                isResult = bllPermissionColumn.Update(nPermissionColumn);
            }
            else
            {
                isResult = bllPermissionColumn.AddColumn(requestModel.name, requestModel.pre_id, requestModel.order_num, requestModel.websiteOwner, requestModel.id);
            }
            if (isResult)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "编辑完成。";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "编辑失败。";
            }
            bll.ContextResponse(context, apiResp);
        }
        public class RequestModel
        {
            /// <summary>
            /// 栏目编号
            /// </summary>
            public long id { get; set; }
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
            /// 站点名称
            /// </summary>
            public string websiteOwner { get; set; }
        }
    }
}