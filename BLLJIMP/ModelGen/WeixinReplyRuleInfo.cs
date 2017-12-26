using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 图文文本自动回复
    /// </summary>
    [Serializable]
    public partial class WeixinReplyRuleInfo : ZCBLLEngine.ModelTable
    {
        public WeixinReplyRuleInfo()
        { }
        #region Model
        private string _uid;
        private string _userid;
        private string _receivetype;
        private string _msgkeyword;
        private string _replytype;
        private string _replycontent;
        private int? _replyarticlecount;
        private DateTime _createdate = DateTime.Now;
        private string _description;
        /// <summary>
        /// 
        /// </summary>
        public string UID
        {
            set { _uid = value; }
            get { return _uid; }
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
        /// 推送信息类型：文本、图片、地理位置
        /// </summary>
        public string ReceiveType
        {
            set { _receivetype = value; }
            get { return _receivetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MsgKeyword
        {
            set { _msgkeyword = value; }
            get { return _msgkeyword; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReplyType
        {
            set { _replytype = value; }
            get { return _replytype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReplyContent
        {
            set { _replycontent = value; }
            get { return _replycontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ReplyArticleCount
        {
            set { _replyarticlecount = value; }
            get { return _replyarticlecount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 匹配类型:全文匹配、包含匹配、开始匹配、结尾匹配
        /// </summary>
        public string MatchType { get; set; }
        /// <summary>
        /// 规则类型：1消息自动回复、2被添加自动回复、3注册回复、4菜单回复，234类型都是每个用户只允许有一个
        /// </summary>
        public int RuleType { get; set; }
        #endregion Model
    }
}
