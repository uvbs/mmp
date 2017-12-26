using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 积分订单详细
    /// </summary>
    [Serializable]
    public partial class WXMallScoreOrderDetailsInfo : ZCBLLEngine.ModelTable
    {
        public WXMallScoreOrderDetailsInfo()
        { }
        #region Model
        private long? _autoid;
        private string _orderid;
        private string _pid;
        private int _totalcount;
        private decimal? _orderprice;
        /// <summary>
        /// 编号
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
        #endregion Model

    }
}

