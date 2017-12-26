using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 礼品订单信息
    /// </summary>
    [Serializable]
    public partial class WXMallGiftOrderInfo : ZCBLLEngine.ModelTable
    {
        public WXMallGiftOrderInfo()
        { }
        /// <summary>
        /// 礼品订单号
        /// </summary>
        public int GiftOrderId { get; set; }
        /// <summary>
        /// 订单号 关联WXMallOrderInfo OrderId
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 收货人联系方式
        /// </summary>
        public string ReceivePhone { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string ExpressCompanyCode { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressCompanyName { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }

        /// <summary>
        /// 收货人省份
        /// </summary>
        public string ReceiverProvince { get; set; }
        /// <summary>
        /// 收货人省份代码
        /// </summary>
        public string ReceiverProvinceCode { get; set; }
        /// <summary>
        /// 收货人城市
        /// </summary>
        public string ReceiverCity { get; set; }
        /// <summary>
        /// 收货人城市代码
        /// </summary>
        public string ReceiverCityCode { get; set; }
        /// <summary>
        /// 收货人城市区域
        /// </summary>
        public string ReceiverDist { get; set; }
        /// <summary>
        /// 收货人区域代码
        /// </summary>
        public string ReceiverDistCode { get; set; }
        /// <summary>
        /// 地址 不包括省市区
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }

    }
}

