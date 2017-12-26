using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNoAction 
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
           string jid=context.Request["jid"];


           JuActivityInfo model = bllJuActivity.GetJuActivity(int.Parse(jid),true,bllJuActivity.WebsiteOwner);
           if (model == null)
           {
               apiResp.msg = "不存在";
               apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
               bllJuActivity.ContextResponse(context,apiResp);
               return;
           }
           if (model.UserID != bllJuActivity.GetCurrUserID())
           {
               apiResp.msg = "没有权限";
               apiResp.code = (int)BLLJIMP.Enums.APIErrCode.NoPms;
               bllJuActivity.ContextResponse(context, apiResp);
               return;
           }
           model.IsDelete = 1;
           if (bllJuActivity.Update(model))
           {
               apiResp.msg = "操作完成";
               apiResp.status = true;
           }
           else
           {
               apiResp.msg = "操作出错";
               apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
           }
           bllJuActivity.ContextResponse(context,apiResp);

           
        }

    }
}