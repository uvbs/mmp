using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 邮件表
    /// </summary>
    [Serializable]
    public partial class EmailInfo : ZCBLLEngine.ModelTable
    {
        public EmailInfo()
        { }
        #region Model
        private string _emailid;
        private string _userid;
        private string _emailname;
        private string _emailsubject;
        private string _emailcontent;
        private string _sendername;
        private DateTime? _definitedate;
        private DateTime _submitdate;
        private int _emailsendstatus = 0;
        private DateTime? _processstartdate;
        private DateTime? _processenddate;
        private DateTime? _sendstartdate;
        private DateTime? _sendenddate;
        private string _exguid;
        private string _otherdescription;
        private string _replyemail;
        private string _bodyencoding;
        private string _submitip;
        /// <summary>
        /// 邮件ID
        /// </summary>
        public string EmailID
        {
            set { _emailid = value; }
            get { return _emailid; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 邮件名称
        /// </summary>
        public string EmailName
        {
            set { _emailname = value; }
            get { return _emailname; }
        }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string EmailSubject
        {
            set { _emailsubject = value; }
            get { return _emailsubject; }
        }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailContent
        {
            set { _emailcontent = value; }
            get { return _emailcontent; }
        }
        /// <summary>
        /// 发件人名称
        /// </summary>
        public string SenderName
        {
            set { _sendername = value; }
            get { return _sendername; }
        }
        /// <summary>
        /// 定时发送时间
        /// </summary>
        public DateTime? DefiniteDate
        {
            set { _definitedate = value; }
            get { return _definitedate; }
        }
        /// <summary>
        /// 邮件提交时间
        /// </summary>
        public DateTime SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
        }
        /// <summary>
        /// 邮件发送状态:0.等待处理；1.正在处理；2.等待发送；3.正在发送；4.完成发送；
        /// </summary>
        public int EmailSendStatus
        {
            set { _emailsendstatus = value; }
            get { return _emailsendstatus; }
        }
        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime? ProcessStartDate
        {
            set { _processstartdate = value; }
            get { return _processstartdate; }
        }
        /// <summary>
        /// 结束处理时间
        /// </summary>
        public DateTime? ProcessEndDate
        {
            set { _processenddate = value; }
            get { return _processenddate; }
        }
        /// <summary>
        /// 开始发送时间
        /// </summary>
        public DateTime? SendStartDate
        {
            set { _sendstartdate = value; }
            get { return _sendstartdate; }
        }
        /// <summary>
        /// 发送结束时间
        /// </summary>
        public DateTime? SendEndDate
        {
            set { _sendenddate = value; }
            get { return _sendenddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExGUID
        {
            set { _exguid = value; }
            get { return _exguid; }
        }
        /// <summary>
        /// 其他说明
        /// </summary>
        public string OtherDescription
        {
            set { _otherdescription = value; }
            get { return _otherdescription; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReplyEmail
        {
            set { _replyemail = value; }
            get { return _replyemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BodyEncoding
        {
            set { _bodyencoding = value; }
            get { return _bodyencoding; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SubmitIP
        {
            set { _submitip = value; }
            get { return _submitip; }
        }
        /// <summary>
        /// 是否已失败重发，1表示已重发
        /// </summary>
        public int? IsFailReSend
        {
            get;
            set;
        }
        /// <summary>
        /// 发送地址是否是回复地址
        /// </summary>
        public int IsSendByReplyEmail { get; set; }

        /// <summary>
        /// EDM网页版链接
        /// </summary>
        public string EDMWebVersionUrl { get; set; }

        /// <summary>
        /// 网页阅读的时候是否跳转到指定链接地址
        /// </summary>
        public int IsReadByWebUrl { get; set; }
        /// <summary>
        /// 发件人地址:如果没有则按系统地址发
        /// </summary>
        public string SenderEmail { get; set; }
        public string SenderEmailPwd { get; set; }

        public string SetSmtp { get; set; }
        public int? SetSmtpPoit { get; set; }
        /// <summary>
        /// 最后更新报表时间
        /// </summary>
        public DateTime? LastUpdateReportDate { get; set; }

        /// <summary>
        /// 发送总数
        /// </summary>
        public int OSendTotalCount { get; set; }
        /// <summary>
        /// 投递成功量
        /// </summary>
        public int ODeliverySuccessCount { get; set; }
        /// <summary>
        /// 投递失败量
        /// </summary>
        public int ODeliveryFailureCount { get; set; }
        /// <summary>
        /// 阅读人数
        /// </summary>
        public int ODistOpensCount { get; set; }
        /// <summary>
        /// 阅读人次
        /// </summary>
        public int OOpensCount { get; set; }
        /// <summary>
        /// 链接点击人数
        /// </summary>
        public int ODistOUrlClicksCount { get; set; }
        /// <summary>
        /// 链接点击人次
        /// </summary>
        public int OUrlClicksCount { get; set; }
        /// <summary>
        /// 最后客户端处理时间
        /// </summary>
        public DateTime? LastClientProcessDate { get; set; }


        #endregion Model
    }
}

