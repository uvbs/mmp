using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 站点微信用户注册方式
    /// </summary>
    public enum WebsiteWXUserRegType
    {

        /// <summary>
        /// 自动注册
        /// </summary>
        AutoReg = 0,
        /// <summary>
        /// 手动注册(任何操作前跳转注册页：参考复启做的注册方式)
        /// </summary>
        ManualRegBeforePage = 1,
        /// <summary>
        /// 手动注册(不跳转注册页，在操作具体步骤的时候才跳转到手机号码绑定页)
        /// </summary>
        ManualRegAfterOperate = 2

    }
}
