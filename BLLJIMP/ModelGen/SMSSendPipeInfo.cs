using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 发送通道设置
    /// </summary>
    public class SMSSendPipeInfo : ZCBLLEngine.ModelTable
    {
        public SMSSendPipeInfo()
        { }
        #region Model
        private string _pipeid;
        private string _apiurl;
        private string _smsuser;
        private string _smspwd;
        private string _otherdescription;
        /// <summary>
        /// 
        /// </summary>
        public string PipeID
        {
            set { _pipeid = value; }
            get { return _pipeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APIUrl
        {
            set { _apiurl = value; }
            get { return _apiurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SmsUser
        {
            set { _smsuser = value; }
            get { return _smsuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SmsPwd
        {
            set { _smspwd = value; }
            get { return _smspwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OtherDescription
        {
            set { _otherdescription = value; }
            get { return _otherdescription; }
        }
        /// <summary>
        /// 一般是运营商提供的产品通道号等
        /// </summary>
        public string SmsChannel { get; set; }
        #endregion Model
    }
}
