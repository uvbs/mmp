using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户通道
    /// </summary>
    [Serializable]
    public class SMSUserPipeSet : ZentCloud.ZCBLLEngine.ModelTable
    {
        public SMSUserPipeSet()
        { }
        #region Model
        private string _userid;
        private string _userpipe;
        private string _sendpipe;
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户通道
        /// </summary>
        public string UserPipe
        {
            set { _userpipe = value; }
            get { return _userpipe; }
        }
        /// <summary>
        /// 真实发送通道
        /// </summary>
        public string SendPipe
        {
            set { _sendpipe = value; }
            get { return _sendpipe; }
        }
        #endregion Model
    }
}
