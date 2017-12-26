
using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// CarServerOrderInfo
    /// </summary>
    [Serializable]
    public partial class CarServerOrderInfo : ZCBLLEngine.ModelTable
    {
        public CarServerOrderInfo()
        { }
        #region Model
        private int _orderid;
        private string _userid;
        private int _serverid;
        private string _sallerid;
        private DateTime _createtime;
        private int? _carmodelid;
        private string _carownername;
        private string _carownerphone;
        private int _score = 0;
        private string _review;
        private double? _totalprice;
        /// <summary>
        /// 
        /// </summary>
        public int OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ServerId
        {
            set { _serverid = value; }
            get { return _serverid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SallerId
        {
            set { _sallerid = value; }
            get { return _sallerid; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarModelId
        {
            set { _carmodelid = value; }
            get { return _carmodelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarOwnerName
        {
            set { _carownername = value; }
            get { return _carownername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarOwnerPhone
        {
            set { _carownerphone = value; }
            get { return _carownerphone; }
        }

        /// <summary>
        /// 车辆里程数
        /// </summary>
        public int CarOwnerMiles { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Score
        {
            set { _score = value; }
            get { return _score; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Review
        {
            set { _review = value; }
            get { return _review; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? TotalPrice
        {
            set { _totalprice = value; }
            get { return _totalprice; }
        }
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 门店类型：4S店、专修店
        /// </summary>
        public string ShopType { get; set; }

        public string ServerType { get; set; }

        public int CarBrandId { get; set; }

        public int CarSeriesCateId { get; set; }

        public int CarSeriesId { get; set; }

        /// <summary>
        /// 核销时间
        /// </summary>
        public DateTime? UseTime { get; set; }

        ///// <summary>
        ///// 核销状态
        ///// </summary>
        //public int UseStatus { get; set; }
        ///// <summary>
        ///// 作废状态
        ///// </summary>
        //public int VoidStatus { get; set; }

        /// <summary>
        /// 评论状态:0未评论  1已评论
        /// </summary>
        public int CommentStatus { get; set; }
        /// <summary>
        /// 是否代客取车
        /// </summary>
        public int IsDesignatedDriving { get; set; }
        /// <summary>
        /// 预定到店日期
        /// </summary>
        public string BookArrvieDate { get; set; }
        /// <summary>
        /// 预定到店开始时间
        /// </summary>
        public string BookArrvieStartTime { get; set; }
        /// <summary>
        /// 预定到店结束时间
        /// </summary>
        public string BookArrvieEndTime { get; set; }

        /// <summary>
        /// 状态：0受理中，1已确认，2已完成，3已取消
        /// </summary>
        public int Status { get; set; }

        #endregion Model


        #region ModelEx
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusStr
        {
            get
            {
                string result = string.Empty;

                switch (Status)
                {
                    case 0:
                        result = "受理中";
                        break;
                    case 1:
                        result = "已确认";
                        break;
                    case 2:
                        result = "已完成";
                        break;
                    case 3:
                        result = "已取消";
                        break;

                }

                return result;
            }
        }
        /// <summary>
        /// 预约到店时间文本
        /// </summary>
        public string BookArrvieDateStr
        {
            get
            {
                return string.Format("{0} {1}至{2}", BookArrvieDate, BookArrvieStartTime, BookArrvieEndTime);
            }
        }

        public CarModelInfo CarModel
        {
            get
            {
                return CarModelId == null ? null : new BLLCarLibrary().GetCarModelInfo(CarModelId.Value);
            }
        }

        public dynamic Saller
        {
            get
            {
                var saller = new BLLUser().GetUserInfo(SallerId);

                return new
                {
                    Company = saller.Company,
                };
            }
        }

        public CarServerInfo Server
        {
            get
            {
                var server = new BLLCarLibrary().GetServer(ServerId);
                return server;
            }
        }

        #endregion
    }
}

