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
    /// 更新表映射
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            TableFieldMapping model = new TableFieldMapping();
            model = bll.ConvertRequestToModel<TableFieldMapping>(model);
            TableFieldMapping oModel = bll.GetByKey<TableFieldMapping>("AutoId", model.AutoId.ToString());
            oModel.MappingName = model.MappingName;
            oModel.FieldType = model.FieldType;
            oModel.FormatValiFunc = model.FormatValiFunc;
            oModel.FieldIsNull = model.FieldIsNull;
            oModel.IsShowInList = model.IsShowInList;
            oModel.IsReadOnly = model.IsReadOnly;
            oModel.Options = model.Options;
            oModel.ForeignkeyId = model.ForeignkeyId;
            bool result = false;
            result = bll.Update(oModel);
            if (!result)
            {
                apiResp.msg = "编辑失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.msg = "编辑完成";
            apiResp.code = (int)APIErrCode.IsSuccess;

            bll.ContextResponse(context, apiResp);
        }
    }
}