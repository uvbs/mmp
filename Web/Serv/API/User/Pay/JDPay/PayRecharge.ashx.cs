﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.JDPay
{
    /// <summary>
    /// PayRecharge 的摘要说明
    /// </summary>
    public class PayRecharge : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            Alipay.PayRecharge prec = new Alipay.PayRecharge();
            prec.BuildOrder(context, 2, CurrentUserInfo);
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