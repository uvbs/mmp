
using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
    /// 车型分类
	/// </summary>
	[Serializable]
    public partial class CarModelCateInfo : ZCBLLEngine.ModelTable
	{
		public CarModelCateInfo()
		{}
		#region Model
		private int _carmodelcateid;
		private int? _carseriesid;
		private string _carmodelcatename;
		private DateTime _updatetime;
		private string _updator;
        /// <summary>
        /// 0即将销售，1在售车型，2停售车型
        /// </summary>
        public int CarModelCateId
		{
			set{ _carmodelcateid=value;}
			get{return _carmodelcateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarSeriesId
		{
			set{ _carseriesid=value;}
			get{return _carseriesid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarModelCateName
		{
			set{ _carmodelcatename=value;}
			get{return _carmodelcatename;}
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
		#endregion Model

	}
}

