using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 邮箱地址库数据
    /// </summary>
    [Serializable]
    public partial class EmailAddressInfo : ZCBLLEngine.ModelTable
    {
        public EmailAddressInfo()
        { }
        #region Model
        private string _userid;
        private string _email;
        private string _groupid;
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
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }

        public long AutoID { get; set; }

        #endregion Model

    }
}
