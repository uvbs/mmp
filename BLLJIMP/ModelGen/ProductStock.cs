using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
   public class ProductStock:ZentCloud.ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 库存Id 自增 主键
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// SkuID
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 2:267:颜色:红色;1:1:尺码:S
        /// </summary>
        public string Props { get; set; }

        /// <summary>
        /// 登记个数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WXNickname { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string WXHeadimgurl { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
