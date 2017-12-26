using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web
{
    //<summary>
    //发送短信验证码
    //</summary>
    public class SendSmsResponse : AshxResponse
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerCode { get; set; }
    }

}