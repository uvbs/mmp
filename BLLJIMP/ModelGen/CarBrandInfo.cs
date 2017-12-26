using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
    /// 品牌
	/// </summary>
	[Serializable]
    public partial class CarBrandInfo : ZCBLLEngine.ModelTable
	{
		public CarBrandInfo()
		{}
		#region Model
		private int _carbrandid;
		private string _carbrandname;
		private string _firstletter;
		private DateTime _updatetime;
		private string _updator;
		/// <summary>
		/// 品牌ID
		/// </summary>
		public int CarBrandId
		{
			set{ _carbrandid=value;}
			get{return _carbrandid;}
		}
		/// <summary>
		/// 品牌名称
		/// </summary>
		public string CarBrandName
		{
			set{ _carbrandname=value;}
			get{return _carbrandname;}
		}
		/// <summary>
		/// 品牌开头字母
		/// </summary>
		public string FirstLetter
		{
			set{ _firstletter=value;}
			get{return _firstletter;}
		}
		/// <summary>
		/// 最后更新时间
		/// </summary>
        public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 更新人
		/// </summary>
		public string Updator
		{
			set{ _updator=value;}
			get{return _updator;}
		}
        /// <summary>
        /// 品牌图路径地址
        /// </summary>
        public string BrandImg { get; set; }
		#endregion Model

        #region ModelEx
        /// <summary>
        /// 是否是当前用户购车库品牌
        /// </summary>
        public bool IsCurrBuyCarBrand { get; set; }
        /// <summary>
        /// 是否是当前养车车库品牌
        /// </summary>
        public bool IsCurrServiceCarBrand { get; set; } 
        #endregion

	}
}

