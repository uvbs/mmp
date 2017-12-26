using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
    /// 车型
	/// </summary>
	[Serializable]
    public partial class CarModelInfo : ZCBLLEngine.ModelTable
	{
		public CarModelInfo()
		{}
		#region Model
		private int _carmodelid;
		private int? _carmodelcateid;
		private string _carmodelname;
		private int? _year;
		private int? _guideprice;
		private string _servicetype;
		private DateTime _updatetime;
		private string _updator;
		/// <summary>
		/// 
		/// </summary>
		public int CarModelId
		{
			set{ _carmodelid=value;}
			get{return _carmodelid;}
		}
        /// <summary>
        /// 0即将销售，1在售车型，2停售车型
        /// </summary>
        public int? CarModelCateId
		{
			set{ _carmodelcateid=value;}
			get{return _carmodelcateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarModelName
		{
			set{ _carmodelname=value;}
			get{return _carmodelname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Year
		{
			set{ _year=value;}
			get{return _year;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? GuidePrice
		{
			set{ _guideprice=value;}
			get{return _guideprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ServiceType
		{
			set{ _servicetype=value;}
			get{return _servicetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Updator
		{
			set{ _updator=value;}
			get{return _updator;}
		}
        public int CarBrandId { get; set; }
        public int CarSeriesCateId { get; set; }
        public int CarSeriesId { get; set; }
        public string Colors { get; set; }
        /// <summary>
        /// 图片，多的用逗号分隔
        /// </summary>
        public string Img { get; set; }
        #endregion Model


        #region ModelEx
        public string ShowName
        {
            get
            {
                return string.Format("{0}款  {1}",Year,CarModelName);
            }

        }

        public string AllName
        {
            get
            {
                return new BLLCarLibrary().GetAllCarModelName(CarModelId);
            }
        }
        #endregion
    }
}

