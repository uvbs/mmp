using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// SMTP
    /// </summary>
    [Serializable]
    public partial class EmailSmtpInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public EmailSmtpInfo()
        { }
        #region Model
        private string _smtpid;
        private string _smtp;
        private int _serverpoit;
        /// <summary>
        /// 
        /// </summary>
        public string SmtpID
        {
            set { _smtpid = value; }
            get { return _smtpid; }
        }
        /// <summary>
        /// Smtp服务器地址
        /// </summary>
        public string Smtp
        {
            set { _smtp = value; }
            get { return _smtp; }
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServerPoit
        {
            set { _serverpoit = value; }
            get { return _serverpoit; }
        }
        #endregion Model

    }
}
