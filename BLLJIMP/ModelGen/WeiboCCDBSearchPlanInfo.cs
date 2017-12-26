using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 至云数据库找人任务表
    /// </summary>
    [Serializable]
    public partial class WeiboCCDBSearchPlanInfo : ZCBLLEngine.ModelTable
    {
        public WeiboCCDBSearchPlanInfo()
        { }
        #region Model
        private int _planid;
        private string _userid;
        private DateTime _submitdate = DateTime.Now;
        private string _planstatus;
        private string _datadownloadpath;
        private string _screenname;
        private string _tags;
        private string _userdescription;
        private string _gender;
        private string _followerscount;
        private string _friendscount;
        private string _bifollowerscount;
        private string _statusescount;
        private string _favouritescount;
        private string _location;
        private string _verified;
        private string _headpic;
        private string _plandescription;
        private string _execuserid;
        /// <summary>
        /// 任务ID
        /// </summary>
        public int PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 提交人
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
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
        /// 任务状态
        /// </summary>
        public string PlanStatus
        {
            set { _planstatus = value; }
            get { return _planstatus; }
        }
        /// <summary>
        /// 数据下载地址
        /// </summary>
        public string DataDownloadPath
        {
            set { _datadownloadpath = value; }
            get { return _datadownloadpath; }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string ScreenName
        {
            set { _screenname = value; }
            get { return _screenname; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags
        {
            set { _tags = value; }
            get { return _tags; }
        }
        /// <summary>
        /// 个人描述
        /// </summary>
        public string UserDescription
        {
            set { _userdescription = value; }
            get { return _userdescription; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        {
            set { _gender = value; }
            get { return _gender; }
        }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public string FollowersCount
        {
            set { _followerscount = value; }
            get { return _followerscount; }
        }
        /// <summary>
        /// 关注数
        /// </summary>
        public string FriendsCount
        {
            set { _friendscount = value; }
            get { return _friendscount; }
        }
        /// <summary>
        /// 互粉数
        /// </summary>
        public string BiFollowersCount
        {
            set { _bifollowerscount = value; }
            get { return _bifollowerscount; }
        }
        /// <summary>
        /// 微博数
        /// </summary>
        public string StatusesCount
        {
            set { _statusescount = value; }
            get { return _statusescount; }
        }
        /// <summary>
        /// 收藏数
        /// </summary>
        public string FavouritesCount
        {
            set { _favouritescount = value; }
            get { return _favouritescount; }
        }
        /// <summary>
        /// 所在地
        /// </summary>
        public string Location
        {
            set { _location = value; }
            get { return _location; }
        }
        /// <summary>
        /// 是否加V
        /// </summary>
        public string Verified
        {
            set { _verified = value; }
            get { return _verified; }
        }
        /// <summary>
        /// 是否有头像
        /// </summary>
        public string HeadPic
        {
            set { _headpic = value; }
            get { return _headpic; }
        }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string PlanDescription
        {
            set { _plandescription = value; }
            get { return _plandescription; }
        }
        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecUserID
        {
            set { _execuserid = value; }
            get { return _execuserid; }
        }
        /// <summary>
        /// 任务名
        /// </summary>
        public string PlanName { get; set; }
        #endregion Model

    }
}

