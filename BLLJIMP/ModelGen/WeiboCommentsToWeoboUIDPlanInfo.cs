using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 微博评论任务微博ID表
	/// </summary>
	[Serializable]
	public partial class WeiboCommentsToWeoboUIDPlanInfo:ZCBLLEngine.ModelTable
	{
		public WeiboCommentsToWeoboUIDPlanInfo()
		{}
		#region Model
		private int _planid;
		private string _weibouid;
		/// <summary>
		/// 任务ID
		/// </summary>
		public int PlanID
		{
			set{ _planid=value;}
			get{return _planid;}
		}
		/// <summary>
		/// 微博ID
		/// </summary>
		public string WeiboUID
		{
			set{ _weibouid=value;}
			get{return _weibouid;}
		}
		#endregion Model

	}
}

