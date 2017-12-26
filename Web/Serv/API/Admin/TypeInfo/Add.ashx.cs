using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TypeInfo
{
    /// <summary>
    /// 添加
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {

            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                apiResp.code = -1;
                apiResp.msg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.type_name))
            {
                apiResp.code = 1;
                apiResp.msg = "type_name 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            ZentCloud.BLLJIMP.Model.TypeInfo model = new ZentCloud.BLLJIMP.Model.TypeInfo();
            model.TypeId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
            model.TypeName = requestModel.type_name;
            model.WebsiteOwner = bll.WebsiteOwner;
            model.InsertDate = DateTime.Now;
            if (bll.Add(model))
            {
                apiResp.status = true;
                apiResp.msg="ok";
            }
            else
            {
                apiResp.msg = "添加失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
           
        }

        public class RequestModel
        {
            /// <summary>
            /// 类型名称
            /// </summary>
            public string type_name { get; set; }


        }

    }
}