using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.JDPay
{
    /// <summary>
    /// PayRegister 的摘要说明
    /// </summary>
    public class PayRegister : BaseHandlerNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            Alipay.PayRegister preg = new Alipay.PayRegister();
            preg.BuildOrder(context, 2);
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