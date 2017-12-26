using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户个性化信息
    /// </summary>
    [Serializable]
    public partial class UserPersonalizeDataInfo : ZCBLLEngine.ModelTable
    {
        public UserPersonalizeDataInfo()
        { }
        #region Model
        private int _personalizeid;
        private string _userid;
        private int _personalizetype;
        private string _val1;
        private string _val2;
        /// <summary>
        /// 
        /// </summary>
        public int PersonalizeID
        {
            set { _personalizeid = value; }
            get { return _personalizeid; }
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
        /// 1为发件人名称，2为回复邮箱地址，3为发件人地址，4为smtp地址
        /// </summary>
        public int PersonalizeType
        {
            set { _personalizetype = value; }
            get { return _personalizetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Val1
        {
            set { _val1 = value; }
            get { return _val1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Val2
        {
            set { _val2 = value; }
            get { return _val2; }
        }

        public string WebsiteOwner { get; set; }

        #endregion Model


    }
}

