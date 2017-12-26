using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
    /// 车系
	/// </summary>
	[Serializable]
    public partial class CarSeriesInfo : ZCBLLEngine.ModelTable
	{
		public CarSeriesInfo()
		{}
		#region Model
		private int _carseriesid;
		private int? _carseriescateid;
		private string _carseriesname;
		private DateTime _updatetime;
		private string _updator;
		/// <summary>
		/// 
		/// </summary>
		public int CarSeriesId
		{
			set{ _carseriesid=value;}
			get{return _carseriesid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarSeriesCateId
		{
			set{ _carseriescateid=value;}
			get{return _carseriescateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarSeriesName
		{
			set{ _carseriesname=value;}
			get{return _carseriesname;}
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

		#endregion Model

	}
}

