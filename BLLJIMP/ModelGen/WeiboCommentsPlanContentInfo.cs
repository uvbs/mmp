using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 微博评论任务内容表
	/// </summary>
	[Serializable]
	public partial class WeiboCommentsPlanContentInfo:ZCBLLEngine.ModelTable
	{
		public WeiboCommentsPlanContentInfo()
		{}
		#region Model
		private int _planid;
		private string _commentscontent;
		/// <summary>
		/// 任务ID
		/// </summary>
		public int PlanID
		{
			set{ _planid=value;}
			get{return _planid;}
		}
		/// <summary>
		/// 评论内容
		/// </summary>
		public string CommentsContent
		{
			set{ _commentscontent=value;}
			get{return _commentscontent;}
		}
		#endregion Model

	}
}

