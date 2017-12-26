using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TypeInfo
{
    /// <summary>
    /// 更新类型
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
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
            if (requestModel.type_id==0)
            {
                apiResp.code = 1;
                apiResp.msg = "type_id 为必填项,请检查";
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

            ZentCloud.BLLJIMP.Model.TypeInfo model = bll.Get<ZentCloud.BLLJIMP.Model.TypeInfo>(string.Format(" WebsiteOwner='{0}' And TypeId={1}", bll.WebsiteOwner, requestModel.type_id));
            model.TypeName = requestModel.type_name;
            if (bll.Update(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";

            }
            else
            {
                apiResp.msg = "编辑失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

        public class RequestModel
        {

            /// <summary>
            /// 类型ID
            /// </summary>
            public int type_id { get; set; }
            /// <summary>
            /// 类型名称
            /// </summary>
            public string type_name { get; set; }


        }



    }
}