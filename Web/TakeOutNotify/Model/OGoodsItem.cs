using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.TakeOutNotify.Model
{
    public class OGoodsItem
    {
        /// <summary>
        /// 规格Id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// SkuId
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        ///商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 订单中商品项的标识(注意，此处不是商品分类Id)
        /// 2配送费  11食物活动  12餐厅活动  13	红包抵扣  15商家代金券抵扣  102	餐盒费  103	饿配送会员减免配送费   108最低消费   200限时抢购   300饿了么会员免配送费
        /// </summary>
        public long categoryId { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public double? price { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public double? total { get; set; }
        /// <summary>
        /// 多规格
        /// </summary>
        public List<OGroupItemSpec> newSpecs { get; set; }
        /// <summary>
        /// 多属性
        /// </summary>
        public List<OGroupItemAttribute> attributes { get; set; }
        /// <summary>
        /// 商品扩展码
        /// </summary>
        public string extendCode { get; set; }
        /// <summary>
        ///商品条形码
        /// </summary>
        public string barCode { get; set; }
        /// <summary>
        /// 商品重量(单位克)
        /// </summary>
        public double? weight { get; set; }
        /// <summary>
        /// 使用
        /// </summary>
        public double? userPrice { get; set; }
        /// <summary>
        /// 商铺价格
        /// </summary>

        public double? shopPrice { get; set; }
    }
}