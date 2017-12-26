using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Supplier
{
    /// <summary>
    /// 根据供应商代码查询
    /// </summary>
    public class GetByCode : BaseHandlerNoAction
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {


                string code = context.Request["code"];
                var userInfo = bllUser.Get<UserInfo>(string.Format("WebsiteOwner='{0}' And Ex2='{1}'",bllUser.WebsiteOwner,code));
                if (userInfo != null && userInfo.UserType == 7)
                {
                    apiResp.status = true;
                    apiResp.result = new
                    {
                        id = userInfo.AutoID,
                        user_id = userInfo.UserID
                    };


                }
                else
                {
                    apiResp.msg = "专柜码错误";
                }
            }
            catch (Exception)
            {

                apiResp.msg = "专柜码错误";
            }
            bllUser.ContextResponse(context, apiResp);




        }

    }
}