using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{

    /// <summary>
    /// 订单商品列表
    /// </summary>
    public class OrderProductModel
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string product_id { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string category_name { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal quote_price { get; set; }
        /// <summary>
        /// 实价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 展示属性名称
        /// </summary>
        public string show_property { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 订单详情ID
        /// </summary>
        public long? order_detail_id { get; set; }
        ///<summary>
        /// 退款状态
        /// -2 可申请退款
        ///0等待商家处理 
        ///1商家同意退款
        ///2商家不同意退款申请
        ///3买家已发货,等待商家收货
        ///4商家已经确认收货
        ///5商家未收货拒绝退款
        ///6 商家已经退款 
        ///7 关闭退款申请
        /// </summary>
        public string refund_status { get; set; }
        /// <summary>
        /// 最多退款金额
        /// </summary>
        public decimal max_refund_amount { get; set; }

        ///// <summary>
        ///// 关联商品ID
        ///// </summary>
        //public string relation_product_id { get; set; }
        /// <summary>
        /// 父商品ID
        /// </summary>
        public string parent_product_id { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public double review_score { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string review_content { get; set; }

        /// <summary>
        /// 评论图片
        /// </summary>
        public string comment_img { get; set; }

        /// <summary>
        /// 饿了么：分组名称
        /// </summary>
        public string ex1 { get; set; }
        /// <summary>
        /// 分组类型 normal	普通商品  extra	配送费等额外信息   discount	折扣信息，红包，满减等
        /// </summary>

        public string ex2 { get; set; }
        /// <summary>
        /// 规格Id
        /// </summary>

        public string ex3 { get; set; }
        /// <summary>
        /// 饿了么:订单中商品项的标识
        /// </summary>

        public string ex4 { get; set; }
        /// <summary>
        /// 饿了么:多规格
        /// </summary>

        public string ex5 { get; set; }
        /// <summary>
        ///  饿了么:多属性
        /// </summary>

        public string ex6 { get; set; }
        /// <summary>
        /// 饿了么:商品扩展码
        /// </summary>

        public string ex7 { get; set; }
        /// <summary>
        /// 饿了么:商品条形码
        /// </summary>

        public string ex8 { get; set; }
        /// <summary>
        ///  饿了么：SkuId
        /// </summary>

        public string ex9 { get; set; }
        /// <summary>
        /// 饿了么：使用价格
        /// </summary>

        public string ex10 { get; set; }

        /// <summary>
        /// 饿了么:商铺价格
        /// </summary>
        public string ex11 { get; set; }

        
    }




}
