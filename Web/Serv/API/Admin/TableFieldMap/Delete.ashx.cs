using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TableFieldMap
{
    /// <summary>
    /// 删除表映射
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            int dInt = bll.UpdateMultByKey<TableFieldMapping>("AutoId", ids, "IsDelete", "1", null, true);
            if (dInt > 0)
            {
                apiResp.status = true;
                apiResp.msg = "共删除" + dInt+"行记录";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "删除失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}