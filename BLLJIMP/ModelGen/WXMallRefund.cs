using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 退款申请表
    /// </summary>
    public class WXMallRefund : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 退款编号
        /// </summary>
        public string RefundId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 订单详情Id
        /// </summary>
        public int OrderDetailId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string RefundReason { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 买家手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 图片例证集合,多个图片用,分隔
        /// </summary>
        public string ImageList { get; set; }
        ///<summary>
        ///0 等待商家处理 
        ///1 商家同意退款 
        ///2 商家不同意退款申请 
        ///3 买家已发货
        ///4 商家已经确认收货 
        ///5 商家未收到货拒绝退款 
        ///6 商家已经退款 
        ///7 关闭退款申请
        ///8 退款失败
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 退货地址
        /// </summary>
        public string RefundAddress { get; set; }
        /// <summary>
        /// 商家拒绝理由
        /// </summary>
        public string RefuseReason { get; set; }
        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string ExpressCompanyCode { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressCompanyName { get; set; }
        /// <summary>
        ///快递单号
        /// </summary>
        public string ExpressNumber { get; set; }
        /// <summary>
        /// 是否退货
        /// </summary>
        public int IsReturnProduct { get; set; }
        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string WeiXinRefundId { get; set; }
        /// <summary>
        /// sku 条码
        /// </summary>
        public string SkuSn { get; set; }

        /// <summary>
        /// 货物状态
        /// 未收到货
        /// 已收到货
        /// </summary>
        public string ProductStatus { get; set; }
        /// <summary>
        /// 退还积分
        /// </summary>
        public decimal RefundScore { get; set; }
        /// <summary>
        /// 退还余额
        /// </summary>
        public decimal RefundAccountAmount { get; set; }
        /// <summary>
        /// 是否包含运费
        /// </summary>
        public int IsContainTransportFee { get; set; }
        /// <summary>
        /// 外部退款Id
        /// </summary>
        public string OutRefundId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }
}
