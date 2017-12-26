using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WeiboDetails:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WeiboDetails : ZentCloud.ZCBLLEngine.ModelTable
    {
        BLL bll = new BLL("");

        public WeiboDetails()
        { }
        #region Model
        private long _uid;
        private string _userid;
        private int _weibosendtype;
        private DateTime _submitdate;
        private string _submitip;
        private DateTime? _senddate;
        private DateTime? _definitedate;
        private string _sendcontent;
        private int _weibosendstatus;
        private string _description;
        private string _imgpath;
        private string _imgrelativepath;
        private string _weiboid;
        private string _weiboscreenname;
        private string _sendclient;
        private int _weibosendenable;
        private long? _weibosinaid;
        private int? _isdelete = 0;
        /// <summary>
        /// 
        /// </summary>
        public long UID
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
        /// 发送类型
        /// </summary>
        public int WeiboSendType
        {
            set { _weibosendtype = value; }
            get { return _weibosendtype; }
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
        /// 发送时间
        /// </summary>
        public DateTime? SendDate
        {
            set { _senddate = value; }
            get { return _senddate; }
        }
        /// <summary>
        /// 定时
        /// </summary>
        public DateTime? DefiniteDate
        {
            set { _definitedate = value; }
            get { return _definitedate; }
        }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent
        {
            set { _sendcontent = value; }
            get { return _sendcontent; }
        }
        /// <summary>
        /// 发送状态
        /// </summary>
        public int WeiboSendStatus
        {
            set { _weibosendstatus = value; }
            get { return _weibosendstatus; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 图片本地路径
        /// </summary>
        public string ImgPath
        {
            set { _imgpath = value; }
            get { return _imgpath; }
        }
        /// <summary>
        /// 图片相对路径
        /// </summary>
        public string ImgRelativePath
        {
            set { _imgrelativepath = value; }
            get { return _imgrelativepath; }
        }
        /// <summary>
        /// 微博用户ID
        /// </summary>
        public string WeiboID
        {
            set { _weiboid = value; }
            get { return _weiboid; }
        }
        /// <summary>
        /// 微博用户昵称
        /// </summary>
        public string WeiboScreenName
        {
            set { _weiboscreenname = value; }
            get { return _weiboscreenname; }
        }
        /// <summary>
        /// 发送端
        /// </summary>
        public string SendClient
        {
            set { _sendclient = value; }
            get { return _sendclient; }
        }
        /// <summary>
        /// 发送启用状态
        /// </summary>
        public int WeiboSendEnable
        {
            set { _weibosendenable = value; }
            get { return _weibosendenable; }
        }
        /// <summary>
        /// 新浪微博ID
        /// </summary>
        public long? WeiboSinaID
        {
            set { _weibosinaid = value; }
            get { return _weibosinaid; }
        }
        /// <summary>
        /// 删除标识:1表示已删除，0表示未删除;
        /// </summary>
        public int? IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        /// <summary>
        /// 是否是转发微博
        /// </summary>
        public int? IsRepost { get; set; }
        /// <summary>
        /// 转发的微博ID
        /// </summary>
        public long? RepostWeiboID { get; set; }

        #endregion Model

     
    }
}
