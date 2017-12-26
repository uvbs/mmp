using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.JDPay
{
    /// <summary>
    /// PayUpgrade 的摘要说明
    /// </summary>
    public class PayUpgrade : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            Alipay.PayUpgrade pup = new Alipay.PayUpgrade();
            pup.BuildOrder(context, 2, CurrentUserInfo);
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