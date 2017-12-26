using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 留言表
    /// </summary>
    [Serializable]
    public partial class JuMasterFeedBack : ZCBLLEngine.ModelTable
    {
        public JuMasterFeedBack()
        { }
        #region Model
        private int _feedbackid;
        private string _masterid;
        private string _userid;
        private string _usernickname;
        private string _feedbackcontent;
        private string _feedbacktype;
        private DateTime _submitdate = DateTime.Now;
        private string _submitip;
        /// <summary>
        /// 
        /// </summary>
        public int FeedBackID
        {
            set { _feedbackid = value; }
            get { return _feedbackid; }
        }
        /// <summary>
        /// 专家ID
        /// </summary>
        public string MasterID
        {
            set { _masterid = value; }
            get { return _masterid; }
        }
        /// <summary>
        /// 留言用户ID
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 留言用户昵称
        /// </summary>
        public string UserNickName
        {
            set { _usernickname = value; }
            get { return _usernickname; }
        }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string FeedBackContent
        {
            set { _feedbackcontent = value; }
            get { return _feedbackcontent; }
        }
        /// <summary>
        /// 留言类型：互动中心、联系专家
        /// </summary>
        public string FeedBackType
        {
            set { _feedbacktype = value; }
            get { return _feedbacktype; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
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
        /// 处理状态：未处理、已回复
        /// </summary>
        public string ProcessStatus { get; set; }

        public string WebsiteOwner { get; set; }
        #endregion Model


        #region ModelEx

        public string SortContent
        {
            get
            {
                return FeedBackContent.Length > 100 ? FeedBackContent.Substring(0, 100) : FeedBackContent;
            }

        }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string WXHeadimgurlLocal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserID))
                    return "";
                return new BLLUser("").GetUserInfo(UserID).WXHeadimgurlLocal;
            }
        }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WXNickname
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserID))
                    return "";
                return new BLLUser("").GetUserInfo(UserID).WXNickname;
            }
        }

        /// <summary>
        /// 报名用户所属鸿风用户组
        /// </summary>
        public string HFUserPmsGroup
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserID))
                    return "";
                return new BLLUser("").GetUserInfo(UserID).HFUserPmsGroup;
            }
        }

        #endregion


    }
}

