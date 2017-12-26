using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 短信详细信息 触发
    /// </summary>
    [Serializable]
    public partial class SMSTriggerDetails : ZCBLLEngine.ModelTable
    {
        public SMSTriggerDetails()
        { }
        #region Model
        private int? _planid;
        private string _meetingid;
        private int? _triggertype;
        private string _sourceid;
        private string _sendcontent;
        private string _userid;
        private DateTime? _submitdate;
        private string _receiver;
        private DateTime? _senddate;
        private int? _submitstatus;
        private string _submitstatusorg;
        private string _unionflag;
        private string _realstatus;
        private int? _issimulate;
        /// <summary>
        /// 
        /// </summary>
        public int? PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MeetingID
        {
            set { _meetingid = value; }
            get { return _meetingid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TriggerType
        {
            set { _triggertype = value; }
            get { return _triggertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SourceID
        {
            set { _sourceid = value; }
            get { return _sourceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SendContent
        {
            set { _sendcontent = value; }
            get { return _sendcontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Receiver
        {
            set { _receiver = value; }
            get { return _receiver; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SendDate
        {
            set { _senddate = value; }
            get { return _senddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SubmitStatus
        {
            set { _submitstatus = value; }
            get { return _submitstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SubmitStatusORG
        {
            set { _submitstatusorg = value; }
            get { return _submitstatusorg; }
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
        /// 
        /// </summary>
        public string RealStatus
        {
            set { _realstatus = value; }
            get { return _realstatus; }
        }
        /// <summary>
        /// 是否模拟发送
        /// </summary>
        public int? IsSimulate
        {
            set { _issimulate = value; }
            get { return _issimulate; }
        }
        /// <summary>
        /// 其他说明
        /// </summary>
        public string OtherDescription { get; set; }
        /// <summary>
        /// 扣费条数
        /// </summary>
        public int ChargeCount { get; set; }
        #endregion Model

    }
}

