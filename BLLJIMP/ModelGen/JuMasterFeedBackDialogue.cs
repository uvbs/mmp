using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 留言回复表
    /// </summary>
    [Serializable]
    public partial class JuMasterFeedBackDialogue : ZCBLLEngine.ModelTable
    {
        public JuMasterFeedBackDialogue()
        { }
        #region Model
        private int _dialogueid;
        private int? _feedbackid;
        private string _userid;
        private string _dialoguecontent;
        private DateTime _submitdate = DateTime.Now;
        private string _submitip;
        /// <summary>
        /// 
        /// </summary>
        public int DialogueID
        {
            set { _dialogueid = value; }
            get { return _dialogueid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FeedBackID
        {
            set { _feedbackid = value; }
            get { return _feedbackid; }
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
        public string DialogueContent
        {
            set { _dialoguecontent = value; }
            get { return _dialoguecontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SubmitIP
        {
            set { _submitip = value; }
            get { return _submitip; }
        }
        #endregion Model


        #region ModelEx


        public string SortContent
        {
            get
            {
                return DialogueContent.Length > 100 ? DialogueContent.Substring(0, 100) : DialogueContent;
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

