using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
     [Serializable]
   public partial class WeiboUserCollect : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _collectid;
        private string _userid;
        private string _weibouid;
        private string _weiboname;
        private DateTime? _adddate;
        private string _largeimageurl;
        private string _smallimageurl;
        private string _longitude;
        private string _latitude;
        private DateTime? _lastattime;
        private string _description;
        private string _location;
        private string _groupid;
        /// <summary>
        /// 主键
        /// </summary>
        public int CollectID
        {
            set { _collectid = value; }
            get { return _collectid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 微博用户ID
        /// </summary>
        public string WeiboUid
        {
            set { _weibouid = value; }
            get { return _weibouid; }
        }
        /// <summary>
        /// 微博名
        /// </summary>
        public string WeiboName
        {
            set { _weiboname = value; }
            get { return _weiboname; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddDate
        {
            set { _adddate = value; }
            get { return _adddate; }
        }
        /// <summary>
        /// 大图地址
        /// </summary>
        public string LargeImageUrl
        {
            set { _largeimageurl = value; }
            get { return _largeimageurl; }
        }
        /// <summary>
        /// 小图地址
        /// </summary>
        public string SmallImageUrl
        {
            set { _smallimageurl = value; }
            get { return _smallimageurl; }
        }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            set { _longitude = value; }
            get { return _longitude; }
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            set { _latitude = value; }
            get { return _latitude; }
        }
        /// <summary>
        /// 最后搜索时间
        /// </summary>
        public DateTime? LastAtTime
        {
            set { _lastattime = value; }
            get { return _lastattime; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 用户所在位置
        /// </summary>
        public string Location
        {
            set { _location = value; }
            get { return _location; }
        }
        /// <summary>
        /// 分组id
        /// </summary>
        public string GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }

        #endregion Model
    }
}
