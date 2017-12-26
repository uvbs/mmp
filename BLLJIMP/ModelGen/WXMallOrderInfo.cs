using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微商城 订单信息
    /// </summary>
    [Serializable]
    public partial class WXMallOrderInfo : ZCBLLEngine.ModelTable
    {
        BLL bll = new BLL("");
        /// <summary>
        /// 构造函数
        /// </summary>
        public WXMallOrderInfo()
        { }
        #region Model
        /// <summary>
        /// 订单号
        /// </summary>
        private string _orderid;
        /// <summary>
        /// 下单用户
        /// </summary>
        private string _orderuserid;
        /// <summary>
        /// 街道地址
        /// </summary>
        private string _address;
        /// <summary>
        /// 手机号
        /// </summary>
        private string _phone;
        /// <summary>
        /// 总金额
        /// </summary>
        private decimal _totalamount;
        /// <summary>
        /// 下单时间
        /// </summary>
        private DateTime _inserdate = DateTime.Now;
        /// <summary>
        /// 留言
        /// </summary>
        private string _ordermemo;
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        ///下单用户名
        /// </summary>
        public string OrderUserID
        {
            set { _orderuserid = value; }
            get { return _orderuserid; }
        }
        /// <summary>
        /// 代付用户
        /// </summary>
        public string OtherUserId { get; set; }
        /// <summary>
        /// 收货地址 不包含省市区
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 实付金额=商品金额+物流费用-其它优惠(等级优惠,优惠券优惠)
        /// </summary>
        public decimal TotalAmount
        {
            set { _totalamount = value; }
            get { return _totalamount; }
        }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime InsertDate
        {
            set { _inserdate = value; }
            get { return _inserdate; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string OrderMemo
        {
            set { _ordermemo = value; }
            get
            {
                string result = _ordermemo;

                if (result == null)
                {
                    result = "";
                }

                DateTime tempTime = DateTime.Now;
                if (DateTime.TryParse(ClaimArrivalTime, out tempTime))
                {

                    if (result.IndexOf(" [ 要求送达日期：") == -1)
                    {
                        result += string.Format(" [ 要求送达日期：{0} ]", tempTime.ToString("yyyy-MM-dd"));
                    }
                    
                }
                
                return result;
            }
        }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        ///// <summary>
        ///// 配送方式 废弃不用
        ///// </summary>
        //public string DeliveryId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 门店ID 对应ZCJ_WXMallStores 无用
        /// </summary>
        public string WxMallStoreId { get; set; }


        #endregion Model
        /// <summary>
        ///订单商品总数量
        /// </summary>
        public int ProductCount
        {
            get
            {
                int count = 0;
                foreach (var item in bll.GetColList<WXMallOrderDetailsInfo>(int.MaxValue, 1, string.Format("OrderID='{0}'", _orderid), "AutoID,TotalCount"))
                {
                    count += item.TotalCount;
                }
                return count;
            }
        }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }
        /// <summary>
        /// 配送员 暂时无用
        /// </summary>
        public string DeliveryStaff { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(CategoryId))
                        return "";
                    WXMallCategory category = bll.GetColByKey<WXMallCategory>("AutoID", CategoryId, "AutoID,CategoryName");
                    if (category == null) return "";
                    return category.CategoryName;

                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 配送类型
        /// 0代表快递 
        /// 1 门店自提
        /// 2 送货上门
        /// </summary>
        public int DeliveryType { get; set; }
        /// <summary>
        /// 支付类型 0代表线下支付 1代表支付宝 2代表微信支付 3代表京东支付4代表银联5paypal
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// 配送方式编号 关联ZCJ_WXMallDelivery 无用
        /// </summary>
        public string DeliveryAutoId { get; set; }
        /// <summary>
        /// 支付方式编号 关联ZCJ_WXMallPaymentType 无用
        /// </summary>
        public string PaymentTypeAutoId { get; set; }
        /// <summary>
        /// 付款状态 0未付款 1 已经付款
        /// </summary>
        public int PaymentStatus { get; set; }
        /// <summary>
        /// 商品总金额 不包含运费与其它 
        /// </summary>
        public decimal Product_Fee { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Transport_Fee { get; set; }
        /// <summary>
        /// 微信预支付ID
        /// </summary>
        public string WXPrepay_Id { get; set; }
        /// <summary>
        /// 订单的分销状态 
        /// 0未付款
        /// 1已付款
        /// 2已收货
        /// 3已审核
        /// </summary>
        public int DistributionStatus { get; set; }
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
        /// 商城分销一级分销比例 0-100 无用
        /// </summary>
        public decimal DistributionRateLevel1 { get; set; }
        ///// <summary>
        ///// 商城分销二级分销比例 0-100 无用
        ///// </summary>
        //public decimal DistributionRateLevel2 { get; set; }
        ///// <summary>
        ///// 商城分销三级分销比例 0-100 无用
        ///// </summary>
        //public decimal DistributionRateLevel3 { get; set; }
        /// <summary>
        ///优惠券号码
        /// </summary> 
        public string CouponNumber { get; set; }
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
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        ///使用积分
        /// </summary>
        public int UseScore { get; set; }
        /// <summary>
        /// 使用优惠券ID 或我的储值卡id
        /// </summary>
        public string MyCouponCardId { get; set; }
        /// <summary>
        /// 代付优惠券ID 或储值卡id
        /// </summary>
        public string OtherMyCouponCardId { get; set; }
        /// <summary>
        ///卡券类型
        ///0 优惠券
        ///1 储值卡
        /// </summary>
        public int CouponType { get; set; }
        /// <summary>
        ///代付卡券类型
        ///0 优惠券
        ///1 储值卡
        /// </summary>
        public int OtherCouponType { get; set; }
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string OutOrderId { get; set; }
        /// <summary>
        /// 推广人ID
        /// </summary>
        public long SellerId { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal PayableAmount { get; set; }
        /// <summary>
        /// 是否是退款中的订单
        /// 0 否
        /// 1 是
        /// </summary>
        public int IsRefund { get; set; }
        /// <summary>
        /// 订单类型
        /// 0 普通订单
        /// 1 礼品订单
        /// 2 拼团订单
        /// 3 预约订单
        /// 4 付费活动订单
        /// 5 医生预约
        /// 6 医生预约(推荐)
        /// 7 课程订单
        /// 8 外卖订单
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 父订单
        /// </summary>
        public string ParentOrderId { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 优惠券优惠金额
        /// </summary>
        public decimal CardcouponDisAmount { get; set; }

        /// <summary>
        /// 代付储值卡优惠金额
        /// </summary>
        public decimal OtherCardcouponDisAmount { get; set; }
        /// <summary>
        /// 外部交易号，对接外部系统时候的交易号，如某ERP的流水号
        /// </summary>
        public string OutTranNo { get; set; }
        /// <summary>
        /// 支付交易号，存储微信或者支付宝之类的交易号
        /// </summary>
        public string PayTranNo { get; set;}
        /// <summary>
        /// 拼团-团长订单号
        /// </summary>
        public string GroupBuyParentOrderId { get; set; }
        /// <summary>
        /// 拼团-团长折扣0-10
        /// </summary>
        public decimal HeadDiscount { get; set; }
        /// <summary>
        /// 拼团-团员折扣0-10
        /// </summary>
        public decimal MemberDiscount { get; set; }
        /// <summary>
        /// 拼团-拼团人数
        /// </summary>
        public int PeopleCount { get; set; }
        /// <summary>
        /// 拼团-过期天数 
        /// </summary>
        public int ExpireDay { get; set; }
        /// <summary>
        /// 拼团订单状态
        /// 0 拼团中
        /// 1 拼团成功
        /// 2 拼团失败
        /// 3 待退款
        /// </summary>
        public string GroupBuyStatus { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 扩展字段1 出国时间   
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段2 回国时间  预约选择时间类型   饿了么：预计送达时间
        /// </summary>
        public string Ex2 { get; set; } 
        /// <summary>
        ///  自提点ID 配送方式为上门自提时有值   饿了么：发票抬头
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 自提点名称 配送方式为上门自提时有值 预约开始时间         饿了么：是否预订单
        /// </summary>
        public string Ex4 { get; set; }  
        /// <summary>
        ///自提点地址 预约结束时间   饿了么：是否在线支付
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// 租金 预约日期   饿了么：预约日期
        /// </summary>
        public string Ex6 { get; set; }
        /// <summary>
        /// 自提时间 预约日期时间段  饿了么：店铺id
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        ///是否已申诉0未申诉1 已申诉   饿了么：订单绑定的外部id
        /// </summary>
        public string Ex8 { get; set; }
        /// <summary>
        ///申诉内容    饿了么：店铺名称
        /// </summary>
        public string Ex9 { get; set; }
        /// <summary>
        /// 是否申请退押金0 未申请1 已申请  饿了么：订单原始价格 
        /// </summary>
        public string Ex10 { get; set; }     
        /// <summary>
        /// 是否申请退款0 未申请1 已申请  饿了么：订单收货地址经纬度
        /// </summary>
        public string Ex11 { get; set; }
        /// <summary>
        /// 是否可以退款0 不可以退款1 可以退款  饿了么：送餐地址
        /// </summary>
        public string Ex12 { get; set; }
        /// <summary>
        /// 是否可以退押金0 不可以退押金1 可以退押金  饿了么：顾客是否需要发票
        /// </summary>
        public string Ex13 { get; set; }
        /// <summary>
        /// 申请启用退款,退押金原因Json:[‘原因1’,’原因2’]  饿了么：店铺实收
        /// </summary>
        public string Ex14 { get; set; }
        /// <summary>
        /// 申请启用退款退押金 金额   饿了么：饿了么服务费率
        /// </summary>
        public string Ex15 { get; set; }
        /// <summary>
        /// 打款交易流水号 饿了么：饿了么服务费
        /// </summary>
        public string Ex16 { get; set; }
        /// <summary>
        /// 微信退款单号  饿了么：红包
        /// </summary>
        public string Ex17 { get; set; }
        /// <summary>
        /// 是否同意申请退款     饿了么：餐盒费
        /// </summary>
        public string Ex18 { get; set; }
        /// <summary>
        /// 申请退款反馈  饿了么：订单实付总额
        /// </summary>
        public string Ex19 { get; set; }
        /// <summary>
        /// 是否同意退押金 饿了么：店铺承担活动费用
        /// </summary>
        public string Ex20 { get; set; }
        /// <summary>
        /// 申请退押金反馈(仅极酷wifi)  饿了么：饿了么承担活动费用
        /// 后台备注
        /// </summary>
        public string Ex21 { get; set; }

        /// <summary>
        /// 饿了么:降级标识  true为已降级，false为未降级。
        /// 当此字段为降级标识true的时候会影响本单收入的金额值计算不准确，请开发者务必注意。
        /// 
        /// </summary>
        public string Ex22 { get; set; }
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
        /// 使用余额
        /// </summary>
        public decimal UseAmount { get; set; }
        /// <summary>
        /// 代付余额
        /// </summary>
        public decimal OtherUseAmount { get; set; }
        /// <summary>
        /// 确认收货时间
        /// </summary>
        public DateTime? ReceivingTime { get; set; }

        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public decimal ScoreExchangAmount { get; set; }

        /// <summary>
        /// 无需物流：1是，0否
        /// </summary>
        public int IsNoExpress { get; set; }

        /// <summary>
        /// 需要姓名手机，跟无需物流配套使用
        /// </summary>
        public int IsNeedNamePhone { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public double ReviewScore { get; set; }
        /// <summary>
        /// 是否预购订单
        /// </summary>
        public int IsAppointment { get; set; }
        /// <summary>
        /// 供应商账号
        /// </summary>
        public string SupplierUserId { get; set; }
        /// <summary>
        /// 供应商名称 门店名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 门店地址 配送方式为门店自提时有值
        /// </summary>
        public string StoreAddress { get; set; }
        /// <summary>
        /// 是否主订单
        /// </summary>
        public int IsMain { get; set; }
        /// <summary>
        /// 要求送达时间
        /// </summary>
        public string ClaimArrivalTime { get; set; }
        /// <summary>
        /// 渠道账户名
        /// </summary>
        public string ChannelUserId { get; set; }
        /// <summary>
        /// 分销上级 
        /// </summary>
        public string DistributionOwner { get; set; }

        /// <summary>
        /// 自定义名称
        /// </summary>
        public string CustomCreaterName { get; set; }

        /// <summary>
        /// 自定义手机
        /// </summary>
        public string CustomCreaterPhone { get; set; }
        /// <summary>
        /// 其它信息
        /// </summary>
        public string OtherInfo { get; set; }
        /// <summary>
        /// 外卖类型
        /// </summary>
        public string TakeOutType { get; set; }
        /// <summary>
        /// 外部退单状态
        /// </summary>

        public string OutRefundStatus { get; set; }

        /// <summary>
        /// 外部订单状态
        /// </summary>
        public string OutOrderStatus { get; set; }
        #region ModelEx
        /// <summary>
        /// 是否全额付款
        /// </summary>
        public bool IsAllCash
        {
            get
            {
                if (
                    (!string.IsNullOrWhiteSpace(MyCouponCardId) && MyCouponCardId != "0")
                    || UseAmount != 0
                    || UseScore != 0
                    )
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 是否可以结算
        /// 1 可以
        /// </summary>
        public string IsCanSettlement { get; set; }

        #endregion

    }
}

