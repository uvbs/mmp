using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.Common
{
    /// <summary>
    /// 定义系统Session键
    /// </summary>
    public class SessionKey
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public const string UserID = "userID";
        /// <summary>
        /// 登录cookie
        /// </summary>
        public const string LoginCookie = "comeoncloudAutoLoginToken";
        /// <summary>
        /// 用户类型
        /// </summary>
        public const string UserType = "userType";

        /// <summary>
        /// 用户登录状态
        /// </summary>
        public const string LoginStatu = "login";

        /// <summary>
        /// 菜单选择
        /// </summary>
        public const string SelectMenu = "selectMenu";

        /// <summary>
        /// 页面跳转及跳转参数
        /// </summary>
        public const string PageRedirect = "PageRedirect";
        public const string PageRedirectCondition = "PageRedirectCondition";
        public const string PageCacheName = "PageCacheName";

        /// <summary>
        /// 微博授权
        /// </summary>
        public const string AccessToken = "AccessToken";
        /// <summary>
        /// 微博ID
        /// </summary>
        public const string WeiboID = "WeiboID";

        /// <summary>
        /// 加密key
        /// </summary>
        public const string EncryptKey = "至云";

        /// <summary>
        /// 商品列表键值集合，具体使用需要配置成  websiteowner:ShopKeyList
        /// </summary>
        public const string ShopListKeys = "ShopListKeys";

        /// <summary>
        /// 评价列表键值集合，具体使用需要配置成  websiteowner:ReviewKeyList
        /// </summary>
        public const string ReviewListKeys = "ReviewListKeys";

        /// <summary>
        /// 单个商品的所有sku，var key = WebsiteOwner + ":" + Common.SessionKey.ProductSkus + ":" + productId;
        /// </summary>
        public const string ProductSkus = "ProductSkus";

        /// <summary>
        /// 商品单个sku， var key = string.Format("{0}:{1}:{2}", WebsiteOwner, Common.SessionKey.ProductSkuSingle, skuId);
        /// </summary>
        public const string ProductSkuSingle = "ProductSkuSingle";

        /// <summary>
        /// 站点幻灯片 var key = string.Format("{0}:{1}:{2}", WebsiteOwner, Common.SessionKey.SliderByType, type);
        /// </summary>
        public const string SliderByType = "SliderByType";

        /// <summary>
        /// 购物车 var key =string.Format("{0}:{1}:{2}",WebsiteOwner,Common.SessionKey.ShoppingCart,userId) ;
        /// </summary>
        public const string ShoppingCart = "ShoppingCart";

    }
}
