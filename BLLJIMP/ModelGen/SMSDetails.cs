using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	///短信详情
	/// </summary>
	[Serializable]
    public partial class SMSDetails : ZentCloud.ZCBLLEngine.ModelTable
	{
        public SMSDetails()
		{}
        #region Model
        private int _autoid;
        private string _planid;
        private string _receiver;
        private DateTime? _senddate;
        private int _submitstatus;
        private string _unionflag;
        private string _realstatus;
        private int _issimulate;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 发送任务ID
        /// </summary>
        public string PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 短信接收者
        /// </summary>
        public string Receiver
        {
            set { _receiver = value; }
            get { return _receiver; }
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SendDate
        {
            set { _senddate = value; }
            get { return _senddate; }
        }
        /// <summary>
        /// 提交状态
        /// </summary>
        public int SubmitStatus
        {
            set { _submitstatus = value; }
            get { return _submitstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UnionFlag
        {
            set { _unionflag = value; }
            get { return _unionflag; }
        }
        /// <summary>
        /// 送达状态
        /// </summary>
        public string RealStatus
        {
            set { _realstatus = value; }
            get { return _realstatus; }
        }
        /// <summary>
        /// 模拟发送状态
        /// </summary>
        public int IsSimulate
        {
            set { _issimulate = value; }
            get { return _issimulate; }
        }

        #endregion Model

       
	}
}

