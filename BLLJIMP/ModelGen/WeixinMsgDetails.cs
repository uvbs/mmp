using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public partial class WeixinMsgDetails : ZCBLLEngine.ModelTable
    {
        public WeixinMsgDetails()
        { }
        #region Model
        private string _uid;
        private string _userid;
        private string _userweixinpuborgid;
        private string _receiveopenid;
        private string _receivetype;
        private string _receivedateorg;
        private DateTime? _receivedate;
        private string _receivecontent;
        private string _receivepicurl;
        private string _receivelocationx;
        private string _receivelocationy;
        private int _receivelocationscale;
        private string _receivelocationlabel;
        private long? _receivemsgid;
        private string _replytype;
        private string _replycontent;
        private int? _replyreplyarticlecount;
        private DateTime? _replydate;
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
        /// 系统用户微信号
        /// </summary>
        public string UserWeixinPubOrgID
        {
            set { _userweixinpuborgid = value; }
            get { return _userweixinpuborgid; }
        }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string ReceiveOpenID
        {
            set { _receiveopenid = value; }
            get { return _receiveopenid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveType
        {
            set { _receivetype = value; }
            get { return _receivetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveDateOrg
        {
            set { _receivedateorg = value; }
            get { return _receivedateorg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReceiveDate
        {
            set { _receivedate = value; }
            get { return _receivedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveContent
        {
            set { _receivecontent = value; }
            get { return _receivecontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceivePicUrl
        {
            set { _receivepicurl = value; }
            get { return _receivepicurl; }
        }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string ReceiveLocationX
        {
            set { _receivelocationx = value; }
            get { return _receivelocationx; }
        }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string ReceiveLocationY
        {
            set { _receivelocationy = value; }
            get { return _receivelocationy; }
        }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int ReceiveLocationScale
        {
            set { _receivelocationscale = value; }
            get { return _receivelocationscale; }
        }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string ReceiveLocationLabel
        {
            set { _receivelocationlabel = value; }
            get { return _receivelocationlabel; }
        }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long? ReceiveMsgID
        {
            set { _receivemsgid = value; }
            get { return _receivemsgid; }
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
        public int? ReplyReplyArticleCount
        {
            set { _replyreplyarticlecount = value; }
            get { return _replyreplyarticlecount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReplyDate
        {
            set { _replydate = value; }
            get { return _replydate; }
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
        /// 虽然userID可以充当WebsiteOwner功能，为了统一使用WebsiteOwner，建立该字段
        /// </summary>
        public string WebsiteOwner { get; set; }

        #endregion Model

        private string _wXHeadimgurlLocal;
        private string _wXNickname;

        public string WXHeadimgurlLocal
        {
            get
            {

                try
                {
                    if (string.IsNullOrWhiteSpace(_wXHeadimgurlLocal))
                    {

                        _wXHeadimgurlLocal = new BLLUser("").GetUserInfoByOpenId(ReceiveOpenID).WXHeadimgurlLocal;
                    }
                }
                catch {
                    _wXHeadimgurlLocal = "";
                }

                return _wXHeadimgurlLocal;
            }
            set { _wXHeadimgurlLocal = value; }
        }

        public string WXNickname
        {
            get
            {

                try
                {
                    if (string.IsNullOrWhiteSpace(_wXNickname))
                    {
                        _wXNickname = new BLLUser("").GetUserInfoByOpenId(ReceiveOpenID).WXNickname;
                    }
                }
                catch (Exception ex)
                {
                    _wXNickname = "";
                }

                return _wXNickname;
            }
            set { _wXNickname = value; }
        }

        public string ReplyStatus
        {
            get
            {
                string result = "已回复";

                if (string.IsNullOrWhiteSpace(ReplyContent))
                {
                    if (new BLLWeixin("").GetCount<WeixinMsgDetailsImgsInfo>(string.Format(" MsgID = '{0}' ", UID)) < 1)
                        result = "未回复";
                }

                return result;
            }
        }


    }
}
