using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 短信任务
	/// </summary>
	[Serializable]
	public partial class SMSPlanInfo : ZentCloud.ZCBLLEngine.ModelTable
	{
		public SMSPlanInfo()
		{}
        #region Model
        private int _autoid;
        private string _planid;
        private string _senderid;
        private string _sendcontent;
        private string _description;
        private int? _submitcount;
        private int? _chargecount;
        private string _sendFrom;
        private DateTime? _submitdate;
        private string _usepipe;
        private int _ischarge;
        private int _procstatus;
        private string _procserverid;
        private DateTime? _procstartdate;
        private DateTime? _procenddate;
        private int? _plantype;
        private DateTime? _plantime;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 发送用户id
        /// </summary>
        public string SenderID
        {
            set { _senderid = value; }
            get { return _senderid; }
        }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent
        {
            set { _sendcontent = value; }
            get { return _sendcontent; }
        }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 提交号码数
        /// </summary>
        public int? SubmitCount
        {
            set { _submitcount = value; }
            get { return _submitcount; }
        }

        /// <summary>
        /// 扣费点数
        /// </summary>
        public int? ChargeCount
        {
            set { _chargecount = value; }
            get { return _chargecount; }
        }

        /// <summary>
        /// 发送方式：
        /// </summary>
        public string SendFrom
        {
            set { _sendFrom = value; }
            get { return _sendFrom; }
        }

        /// <summary>
        /// 任务提交时间
        /// </summary>
        public DateTime? SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
        }
        /// <summary>
        /// 使用通道
        /// </summary>
        public string UsePipe
        {
            set { _usepipe = value; }
            get { return _usepipe; }
        }
        /// <summary>
        /// 扣费标识
        /// </summary>
        public int IsCharge
        {
            set { _ischarge = value; }
            get { return _ischarge; }
        }
        /// <summary>
        /// 处理状态
        /// 1.待处理
        /// 2.进行中
        /// 3.已结束
        /// </summary>
        public int ProcStatus
        {
            set { _procstatus = value; }
            get { return _procstatus; }
        }
        /// <summary>
        /// 处理服务端ID
        /// </summary>
        public string ProcServerID
        {
            set { _procserverid = value; }
            get { return _procserverid; }
        }
        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime? ProcStartDate
        {
            set { _procstartdate = value; }
            get { return _procstartdate; }
        }
        /// <summary>
        /// 完成处理时间
        /// </summary>
        public DateTime? ProcEndDate
        {
            set { _procenddate = value; }
            get { return _procenddate; }
        }

        public int? PlanType
        {
            get { return _plantype; }
            set { _plantype = value; }
        }

        public DateTime? PlanTime
        {
            get { return _plantime; }
            set { _plantime = value; }
        }
        /// <summary>
        /// 发送的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        ///成功数
        /// </summary>
        public int SuccessCount { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        #endregion Model
        /// <summary>
        ///处理状态
        /// </summary>
        public string ProcstatusStr 
        {
            get {

                switch (_procstatus)
                {
                    case 0:
                        return "等待提交";
                        
                    case 1:
                        return "成功提交";
                        
                    case 2:
                        return "正在提交...";
                    case -1:
                        return "提交失败";
                    default:
                        return "";
                        
                }
            }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public string SubmitDateStr
        {
            get
            {
                return SubmitDate == null ? "" : SubmitDate.ToString();
            }
        }
        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailCount { get; set; }


    }
}

