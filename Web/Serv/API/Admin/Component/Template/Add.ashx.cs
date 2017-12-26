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
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = bll.ConvertRequestToModel<RequestModel>(requestModel);
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
            ComponentTemplate componentTemplate = new ComponentTemplate();
            componentTemplate.ComponentId = requestModel.component_id;
            componentTemplate.ComponentModelId = requestModel.component_model_id;
            componentTemplate.Name = requestModel.name;
            componentTemplate.ThumbnailsPath = requestModel.img;
            componentTemplate.Config = requestModel.config;
            componentTemplate.Data = requestModel.data;
            componentTemplate.FromWebsite = bll.WebsiteOwner;
            componentTemplate.InsertUserID = bll.GetCurrUserID();
            componentTemplate.InsertDate = DateTime.Now;
            componentTemplate.CateId = requestModel.cate;
            if (bll.Add(componentTemplate))
            {
                apiResp.msg = "保存模板成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "保存模板失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }

        public class RequestModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public string img { get; set; }
            public string config { get; set; }
            public string data { get; set; }
            public int component_id { get; set; }
            public int component_model_id { get; set; }
            public int cate { get; set; }
            
        }
    }
}