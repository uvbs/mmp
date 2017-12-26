using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Model
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent(); 
        BLLTransaction tran = new BLLTransaction();
        public void ProcessRequest(HttpContext context)
        {
            string component_model_id = context.Request["component_model_id"];
            ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", component_model_id);
            if (componentModel == null)
            {
                apiResp.msg = "组件库未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (bll.DeleteByKey<ComponentModel>("AutoId", component_model_id, tran) >= 0 &&
                bll.DeleteByKey<ComponentModelField>("ComponentModelKey", componentModel.ComponentModelKey, tran) >= 0)
            {
                tran.Commit();
                apiResp.status = true;
                apiResp.msg = "删除完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                tran.Rollback();
                apiResp.msg = "删除失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}