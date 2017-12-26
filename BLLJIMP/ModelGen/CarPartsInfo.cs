using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// CarPartsInfo
	/// </summary>
	[Serializable]
	public partial class CarPartsInfo : ZCBLLEngine.ModelTable
	{
		public CarPartsInfo()
		{}
		#region Model
		private int _partid;
		private string _partname;
		private double? _price;
		private int? _carmodelid;
		private int _status=0;
		private string _updater;
		private DateTime _updatetime;
		/// <summary>
		/// 
		/// </summary>
		public int PartId
		{
			set{ _partid=value;}
			get{return _partid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PartName
		{
			set{ _partname=value;}
			get{return _partname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double? Price
		{
			set{ _price=value;}
			get{return _price;}
		}


		/// <summary>
		/// 
		/// </summary>
		public int? CarModelId
		{
			set{ _carmodelid=value;}
			get{return _carmodelid;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public string Updater
		{
			set{ _updater=value;}
			get{return _updater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}

        public string WebsiteOwner { get; set; }

        public int CarBrandId { get; set; }
        public int CarSeriesCateId { get; set; }
        public int CarSeriesId { get; set; }
        /// <summary>
        /// 配件数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 所属分类id
        /// </summary>
        public int PartsCateId { get; set; }
        /// <summary>
        /// 所属分类名
        /// </summary>
        public string PartsCateName { get; set; }
        /// <summary>
        /// 所属品牌id
        /// </summary>
        public int PartsBrandId { get; set; }
        /// <summary>
        /// 所属品牌名
        /// </summary>
        public string PartsBrandName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string PartsSpecs { get; set; }

        #endregion Model

        #region ModelEx
        /// <summary>
        /// 显示适配车型
        /// </summary>
        public string ShowCarModel { get; set; }

        #endregion
    }
}

