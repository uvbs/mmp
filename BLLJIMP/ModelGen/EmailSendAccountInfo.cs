using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 邮件发送账号实体
    /// </summary>
    [Serializable]
    public partial class EmailSendAccountInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public EmailSendAccountInfo()
        { }
        #region Model
        private string _sendaccountid;
        private string _password;
        private int _isenable = 1;
        private string _smtpid;
        private string _otherdescription;
        /// <summary>
        /// 账号
        /// </summary>
        public string SendAccountID
        {
            set { _sendaccountid = value; }
            get { return _sendaccountid; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 启用状态
        /// </summary>
        public int IsEnable
        {
            set { _isenable = value; }
            get { return _isenable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SmtpID
        {
            set { _smtpid = value; }
            get { return _smtpid; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string OtherDescription
        {
            set { _otherdescription = value; }
            get { return _otherdescription; }
        }
        #endregion Model

    }
}
