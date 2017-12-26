using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 短信交易
	/// </summary>
	[Serializable]
	public partial class UTransactionInfo : ZentCloud.ZCBLLEngine.ModelTable
	{
		public UTransactionInfo()
		{}
		#region Model
		private int? _autoid;
		private string _userid;
		private string _tractype;
		private decimal _tracmoney;
		private DateTime? _tractime;
		private int? _refeventid;
		/// <summary>
		/// 
		/// </summary>
		public int? AutoID
		{
			set{ _autoid=value;}
			get{return _autoid;}
		}
		/// <summary>
		/// 用户id
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 交易类型(充值，sms,email,weibo,return)
		/// </summary>
		public string Tractype
		{
			set{ _tractype=value;}
			get{return _tractype;}
		}
		/// <summary>
		/// 交易金额
		/// </summary>
		public decimal TracMoney
		{
			set{ _tracmoney=value;}
			get{return _tracmoney;}
		}
		/// <summary>
		/// 交易时间
		/// </summary>
		public DateTime? TracTime
		{
			set{ _tractime=value;}
			get{return _tractime;}
		}
		/// <summary>
		/// 关联事件id
		/// </summary>
		public int? RefeventID
		{
			set{ _refeventid=value;}
			get{return _refeventid;}
		}
        /// <summary>
        /// 消费说明
        /// </summary>
        public string TracNote
        {
            get;
            set;
        }
		#endregion Model

        public string TractypeStr
        {
            get
            {
                switch (Tractype)
                {
                    case "smsSend":
                        return "短信发送扣费";
                        break;
                    default:
                        return "";
                        break;
                }
            }
        }
	}
}

