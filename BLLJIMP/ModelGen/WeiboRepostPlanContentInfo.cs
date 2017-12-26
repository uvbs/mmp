using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 微博转发内容表
	/// </summary>
	[Serializable]
	public partial class WeiboRepostPlanContentInfo:ZCBLLEngine.ModelTable
	{
		public WeiboRepostPlanContentInfo()
		{}
		#region Model
		private int _planid;
		private string _repostcontent;
		/// <summary>
		/// 任务ID
		/// </summary>
		public int PlanID
		{
			set{ _planid=value;}
			get{return _planid;}
		}
		/// <summary>
		/// 转发内容
		/// </summary>
		public string RepostContent
		{
			set{ _repostcontent=value;}
			get{return _repostcontent;}
		}
		#endregion Model

	}
}

