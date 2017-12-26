using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Template
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            Add.RequestModel requestModel = new Add.RequestModel();
            try
            {
                requestModel = bll.ConvertRequestToModel<Add.RequestModel>(requestModel);
            }
            catch (Exception ex)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                //apiResp.msg = ex.Message;
                apiResp.msg = "json格式错误,请检查。";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestModel.name))
            {
                apiResp.msg = "请输入模板名称";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestModel.img))
            {
                apiResp.msg = "请上传模板缩略图";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            ComponentTemplate componentTemplate = bll.GetByKey<ComponentTemplate>("AutoId", requestModel.id.ToString());
            if (componentTemplate == null)
            {
                apiResp.msg = "原模板未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            componentTemplate.Name = requestModel.name;
            componentTemplate.ThumbnailsPath = requestModel.img;
            componentTemplate.Config = requestModel.config;
            componentTemplate.Data = requestModel.data;
            componentTemplate.CateId = requestModel.cate;
            if (bll.Update(componentTemplate))
            {
                apiResp.msg = "修改模板成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "修改模板失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}