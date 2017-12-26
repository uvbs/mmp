using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微商城 积分订单信息
    /// </summary>
    [Serializable]
    public partial class WXMallScoreOrderInfo : ZCBLLEngine.ModelTable
    {
        BLLUser bllUser = new BLLUser();
        BLL bll = new BLL("");
        public WXMallScoreOrderInfo()
        { }
        #region Model
        private string _orderid;
        private string _orderuserid;
        private string _address;
        private string _phone;
        private decimal _totalamount;
        private DateTime _inserdate = DateTime.Now;
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
        /// 收货地址
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
        /// 总金额
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
            get { return _ordermemo; }
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
        ///// 配送方式 
        ///// </summary>
        //public string DeliveryId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否已经删除 0未删除 1已删除
        /// </summary>
        public int IsDelete { get; set; }

        #endregion EXModel
        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get{

            int count = 0;
           
            foreach (var item in bll.GetList<WXMallScoreOrderDetailsInfo>(string.Format("OrderID='{0}'", _orderid)))
            {
                count+=item.TotalCount;

            }

            return count;
        
        
        } }
        /// <summary>
        /// 配送时间
        /// </summary>
        public DateTime ?DeliveryTime { get; set; }

        /// <summary>
        /// 后台管理员留言
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 下单用户信息
        /// </summary>
        public UserInfo OrderUserInfo
        {
            get
            {
                return bllUser.GetUserInfo(OrderUserID);

            }
        }

    }
}

