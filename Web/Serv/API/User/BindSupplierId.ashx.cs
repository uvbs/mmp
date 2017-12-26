using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    public class BindSupplierId : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
                 CurrentUserInfo = bllUser.GetUserInfo(bllUser.GetCurrUserID(), bllUser.WebsiteOwner);
               string supplierId=context.Request["supplier_id"];

               if (string.IsNullOrEmpty(supplierId))
               {
                   apiResp.msg = "supplier_id 参数必传";
                   bllUser.ContextResponse(context, apiResp);
                   return;

               }
               CurrentUserInfo.BindId = supplierId;
               if (bllUser.Update(CurrentUserInfo))
               {
                   apiResp.status = true;
               }
               else
               {
                   apiResp.msg = "绑定门店失败";
               }
            bllUser.ContextResponse(context, apiResp);
        }

       
    }
}