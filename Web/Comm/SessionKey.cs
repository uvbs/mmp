using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web
{
    /// <summary>
    /// 定义系统Session键
    /// </summary>
    public class SessionKey
    {
        /// <summary>
        /// 系统设置
        /// </summary>
        public static SystemSet systemset = new BLLJIMP.BLL().Get<SystemSet>("");

        /// <summary>
        /// 用户ID
        /// </summary>
        public const string UserID = "userID";

        /// <summary>
        /// 用户类型
        /// </summary>
        public const string UserType = "userType";

        /// <summary>
        /// 用户登录状态 1登入 0登出
        /// </summary>
        public const string LoginStatu = "login";

        /// <summary>
        /// 菜单选择
        /// </summary>
        //public const string SelectMenu = "selectMenu";

        ///// <summary>
        ///// 页面跳转及跳转参数
        ///// </summary>
        //public const string PageRedirect = "PageRedirect";
        //public const string PageRedirectCondition = "PageRedirectCondition";
        //public const string PageCacheName = "PageCacheName";
        ///// <summary>
        ///// 微博列表
        ///// </summary>
        //public const string WeiboDetailsList = "WeiboDetailsList";

        ///// <summary>
        ///// 微博授权
        ///// </summary>
        //public const string AccessToken = "AccessToken";
        /// <summary>
        /// 微博ID
        ///// </summary>
        //public const string WeiboID = "WeiboID";

        public const string LastUpdateEdmReportTime = "";

        public static string WXCurrOpenerOpenIDKey = systemset.WXCurrOpenerOpenIDKey;

        /// <summary>
        /// 用户自增ID16进制关键字
        /// </summary>
        public static string UserAutoIDHexKey = systemset.UserAutoIDHexKey;

        /// <summary>
        /// 万邦用户名 对应ZCJ_WBBaseInfo UserId 或 ZCJ_WBCompanyInfo UserId
        /// </summary>
        public const string WanBangUserID = "WanBangUserID";

        /// <summary>
        /// 万邦用户类型 0代表基地 1代表企业
        /// </summary>
        public const string WanBangUserType = "WanBangUserType";
    }
}
