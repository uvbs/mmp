using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 订单商品详情
    /// </summary>
    [Serializable]
    public partial class WXMallOrderDetailsInfo : ZCBLLEngine.ModelTable
    {
        public WXMallOrderDetailsInfo()
        { }
        #region Model
        private long? _autoid;
        private string _orderid;
        private string _pid;
        private int _totalcount;
        private decimal? _orderprice;
        /// <summary>
        /// 自动编号
        /// </summary>
        public long? AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string PID
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount
        {
            set { _totalcount = value; }
            get { return _totalcount; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal? OrderPrice
        {
            set { _orderprice = value; }
            get { return _orderprice; }
        }

        /// <summary>
        /// sku基础价，在下单的时候确定
        /// </summary>
        public decimal BasePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OrderScore { get; set; }

        /// <summary>
        /// SKU 编号
        /// </summary>
        public int? SkuId { get; set; }

        /// <summary>
        /// SKU 显示属性 如 颜色:蓝色;尺码:S
        /// </summary>
        public string SkuShowProp { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImage { get; set; }
        /// <summary>
        /// 是否完成 0未完成 1已经完成 用于销量统计 下单就赋值为1
        /// </summary>
        public int IsComplete { get; set; }
        /// <summary>
        /// 均摊价
        /// </summary>
        public decimal PaymentFt { get; set; }
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
        public string RefundStatus { get; set; }
        /// <summary>
        /// 最多退款金额
        /// </summary>
        public decimal MaxRefundAmount { get; set; }

        /// <summary>
        /// 父商品ID
        /// </summary>
        public string ParentProductId { get; set; }

        /// <summary>
        /// 类型 
        /// Mall或空为普通商品
        /// MeetingRoom 会议室
        /// MeetingRoomAdded 会议室增值
        /// BookingTutor 导师预约
        /// BookingTutorAdded 导师预约增值
        /// </summary>
        private string _article_category_type = "Mall";
        public string ArticleCategoryType
        {
            set { _article_category_type = value; }
            get { return _article_category_type; }
        }
        /// <summary>
        /// 会议室预定类的商品 开始时间
        /// </summary>
        public DateTime StartDate { set; get; }
        /// <summary>
        /// 会议室预定类的商品 结束时间
        /// </summary>
        public DateTime EndDate { set; get; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { set; get; }
        /// <summary>
        /// 收货时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 特卖活动Id
        /// </summary>
        public string PromotionActivityId { get; set; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public string ExQuestionnaireID { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 商品重量 饿了么:单位(克)
        /// </summary>
        public decimal Wegith { get; set; }
        /// <summary>
        ///   饿了么:分组名称
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 饿了么:分组类型  normal 正常的菜品  extra配送费等  discount折扣,如红包,满减等
        /// </summary>

        public string Ex2 { get; set; }
        /// <summary>
        /// 饿了么:规格Id
        /// </summary>

        public string Ex3 { get; set; }
        /// <summary>
        /// 饿了么:订单中商品项的标识
        /// </summary>

        public string Ex4 { get; set; }
        /// <summary>
        /// 饿了么:多规格
        /// </summary>

        public string Ex5 { get; set; }
        /// <summary>
        /// 饿了么:多属性
        /// </summary>

        public string Ex6 { get; set; }
        /// <summary>
        /// 饿了么:商品扩展码
        /// </summary>

        public string Ex7 { get; set; }
        /// <summary>
        /// 饿了么:商品条形码
        /// </summary>

        public string Ex8 { get; set; }
        /// <summary>
        /// 饿了么：SkuId
        /// </summary>

        public string Ex9 { get; set; }
        /// <summary>
        /// 饿了么：
        /// </summary>

        public string Ex10 { get; set; }

        /// <summary>
        /// 饿了么:商铺价格
        /// </summary>
        public string Ex11 { get; set; }


        #endregion Model

    }
}

