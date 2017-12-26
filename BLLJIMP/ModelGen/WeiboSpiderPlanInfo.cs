using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WeiboSpiderPlanInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WeiboSpiderPlanInfo : ZCBLLEngine.ModelTable
    {
        public WeiboSpiderPlanInfo()
        { }
        #region Model
        private long _planid;
        private string _userid;
        private int _weibospiderplantype = 1;
        private string _lng;
        private string _lat;
        private int? _distance = 500;
        private string _address;
        private int? _weibospiderplanstatus;
        private DateTime? _startdate;
        private DateTime? _enddate;
        private DateTime _createdate = DateTime.Now;
        private string _oterdescription;
        /// <summary>
        /// 
        /// </summary>
        public long PlanID
        {
            set { _planid = value; }
            get { return _planid; }
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
        public int WeiboSpiderPlanType
        {
            set { _weibospiderplantype = value; }
            get { return _weibospiderplantype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Lng
        {
            set { _lng = value; }
            get { return _lng; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Lat
        {
            set { _lat = value; }
            get { return _lat; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Distance
        {
            set { _distance = value; }
            get { return _distance; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WeiboSpiderPlanStatus
        {
            set { _weibospiderplanstatus = value; }
            get { return _weibospiderplanstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
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
        public string OterDescription
        {
            set { _oterdescription = value; }
            get { return _oterdescription; }
        }
        #endregion Model

    }
}

