using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
    /// 车系分类
	/// </summary>
	[Serializable]
    public partial class CarSeriesCateInfo : ZCBLLEngine.ModelTable
	{
		public CarSeriesCateInfo()
		{}
		#region Model
		private int _carseriescateid;
		private int? _carbrandid;
		private string _carseriescatename;
		private DateTime _updatetime;
		private string _updator;
		/// <summary>
		/// 
		/// </summary>
		public int CarSeriesCateId
		{
			set{ _carseriescateid=value;}
			get{return _carseriescateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarBrandId
		{
			set{ _carbrandid=value;}
			get{return _carbrandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarSeriesCateName
		{
			set{ _carseriescatename=value;}
			get{return _carseriescatename;}
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

