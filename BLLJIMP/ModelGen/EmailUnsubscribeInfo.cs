using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 邮件退订信息
    /// </summary>
    [Serializable]
    public partial class EmailUnsubscribeInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public EmailUnsubscribeInfo()
        { }
        #region Model
        private string _userid;
        private string _unsubscribeemail;
        private string _emailid;
        private string _submitip;
        private DateTime _unsubscribedate;
        private string _unsubscribedescription;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 退订邮箱地址
        /// </summary>
        public string UnsubscribeEmail
        {
            set { _unsubscribeemail = value; }
            get { return _unsubscribeemail; }
        }
        /// <summary>
        /// 退订邮件ID
        /// </summary>
        public string EmailID
        {
            set { _emailid = value; }
            get { return _emailid; }
        }
        /// <summary>
        /// 提交IP
        /// </summary>
        public string SubmitIP
        {
            set { _submitip = value; }
            get { return _submitip; }
        }
        /// <summary>
        /// 退订时间
        /// </summary>
        public DateTime UnsubscribeDate
        {
            set { _unsubscribedate = value; }
            get { return _unsubscribedate; }
        }
        /// <summary>
        /// 退订说明
        /// </summary>
        public string UnsubscribeDescription
        {
            set { _unsubscribedescription = value; }
            get { return _unsubscribedescription; }
        }
        #endregion Model

    }
}
