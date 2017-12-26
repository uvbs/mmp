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
    /// 设置表映射
    /// </summary>
    public class SetField : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string field = context.Request["field"];
            string value = context.Request["value"];
            if (string.IsNullOrWhiteSpace(ids) || string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(value))
            {
                apiResp.msg = "参数错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }

            if (bll.Update(
                new TableFieldMapping(),
                string.Format("{0}='{1}'", field, value),
                string.Format("AutoId In ({0}) AND WebsiteOwner='{1}' ", ids, bll.WebsiteOwner)) > 0)
            {

                apiResp.status = true;
                apiResp.msg = "设置完成";
                apiResp.code = (int)APIErrCode.IsSuccess;

            }
            else
            {
                apiResp.msg = "设置失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }

            bll.ContextResponse(context, apiResp);
        }
    }
}