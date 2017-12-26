using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
    /// 车型详情
	/// </summary>
	[Serializable]
    public partial class CarModelDetailInfo : ZCBLLEngine.ModelTable
	{
		public CarModelDetailInfo()
		{}
		#region Model
		private int _carmodelid;
		private string _detailjson;
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
		/// 
		/// </summary>
		public string DetailJson
		{
			set{ _detailjson=value;}
			get{return _detailjson;}
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

