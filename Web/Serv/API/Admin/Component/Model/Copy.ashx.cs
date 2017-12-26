using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Model
{
    /// <summary>
    /// Copy 的摘要说明
    /// </summary>
    public class Copy : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            string component_model_id = context.Request["component_model_id"];
            BLLJIMP.Model.ComponentModel model = bll.Get<BLLJIMP.Model.ComponentModel>(string.Format(" AutoId={0}", component_model_id));
            if (model == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "未找到组件库";
                bll.ContextResponse(context, apiResp);
                return;
            }
            List<BLLJIMP.Model.ComponentModelField> modelFieldList = bll.GetList<BLLJIMP.Model.ComponentModelField>(string.Format(" ComponentModelKey='{0}'", model.ComponentModelKey));

            model.ComponentModelKey = string.Format("Model{0}{1}", ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now), ZentCloud.Common.Rand.Str(5));
            model.ComponentModelName = model.ComponentModelName + "--复制";
            foreach (BLLJIMP.Model.ComponentModelField item in modelFieldList)
            {
                item.ComponentModelKey = model.ComponentModelKey;
            }
            string errmsg = string.Empty;
            if (bll.AddComponentModel(model, modelFieldList, out errmsg))
            {
                apiResp.status = true;
                apiResp.msg = "复制完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "复制出错";
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