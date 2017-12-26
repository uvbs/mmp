using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class WeixinMsgDetailsImgsInfo : ZCBLLEngine.ModelTable
    {
        public WeixinMsgDetailsImgsInfo()
        { }
        #region Model
        private string _uid;
        private string _msgid;
        private string _title;
        private string _description;
        private string _picurl;
        private string _url;
        private int? _orderindex;
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
        public string MsgID
        {
            set { _msgid = value; }
            get { return _msgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
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
        /// 
        /// </summary>
        public string PicUrl
        {
            set { _picurl = value; }
            get { return _picurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderIndex
        {
            set { _orderindex = value; }
            get { return _orderindex; }
        }
        #endregion Model
    }
}
