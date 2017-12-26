using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 商品SKU
    /// </summary>
    public partial class ProductSku : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// SKU 编号
        /// </summary>
        public int SkuId { get; set; }
        /// <summary>
        ///商品ID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// sku基础价，如果是0，则读取商品的总基础价
        /// </summary>
        public decimal BasePrice { get; set; }
        /// <summary>
        /// 库存 
        /// </summary>
        public int Stock { get; set; }
        ///<summary>
        ///特征量特征值组合 
        /// 格式 商品属性id:商品属性值id:商品属性名称:商品属性值名称 多个组合用;分隔 
        /// 示例 1:1:尺码:S;2:5:颜色:蓝色
        ///</summary>
        public string Props { get; set; }
        /// <summary>
        /// 属性 显示名称 示例 尺码:XS;颜色:红色;
        /// </summary>
        public string ShowProps { get; set; }
        /// <summary>
        /// SKU 编码
        /// </summary>
        public string SkuSN { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 外部条码
        /// </summary>
        public string OutBarCode { get; set; }
        /// <summary>
        /// 尺码Id 对应 ZCJ_ProductPropertyValue PropValueId efast 用到
        /// 预约 开始时间  
        /// </summary>
        public string PropValueIdEx1 { get; set; }
        /// <summary>
        /// 颜色Id 对应 ZCJ_ProductPropertyValue PropValueId efast 用到
        /// 预约 结束时间  
        /// </summary>
        public string PropValueIdEx2 { get; set; }
        /// <summary>
        /// mixblu中，对应efast的分类id
        /// </summary>
        public string PropValueIdEx3 { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 限时特卖价格
        /// </summary>
        public decimal PromotionPrice { get; set; }
        /// <summary>
        ///限时特卖剩余库存
        /// </summary>
        public int PromotionStock { get; set; }
        /// <summary>
        /// 特卖开始时间
        /// </summary>
        public double PromotionStartTime { get; set; }
        /// <summary>
        /// 特卖结束时间
        /// </summary>
        public double PromotionStopTime { get; set; }

        /// <summary>
        ///限时特卖 特卖库存
        /// </summary>
        public int PromotionSaleStock { get; set; }

        /// <summary>
        /// 类型 
        /// Mall或空为普通商品
        /// MeetingRoom 会议室
        /// MeetingRoomAdded 会议室增值
        /// BookingTutor 导师预约
        /// BookingTutorAdded 导师预约增值
        /// </summary>
        private string _article_category_type = "Mall";
        /// <summary>
        /// 类型 
        /// Mall或空为普通商品
        /// MeetingRoom 会议室
        /// MeetingRoomAdded 会议室增值
        /// BookingTutor 导师预约
        /// BookingTutorAdded 导师预约增值
        /// </summary>
        public string ArticleCategoryType
        {
            set { _article_category_type = value; }
            get { return _article_category_type; }
        }


        /// <summary>
        /// 扩展试卷id
        /// </summary>
        public string ExQuestionnaireID { get; set; }

        /// <summary>
        /// 重量 单位Kg 四位小数
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Sku对应的图片
        /// </summary>
        public string SkuImg { get; set; }

             
    }
}
