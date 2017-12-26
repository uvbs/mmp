using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 邮件链接表
	/// </summary>
	[Serializable]
    public partial class EmailLinkInfo : ZCBLLEngine.ModelTable
	{
		public EmailLinkInfo()
		{}
		#region Model
		private string _linkid;
		private string _emailid;
		private string _reallink;
		/// <summary>
		/// 链接ID
		/// </summary>
		public string LinkID
		{
			set{ _linkid=value;}
			get{return _linkid;}
		}
		/// <summary>
		/// 邮件ID
		/// </summary>
		public string EmailID
		{
			set{ _emailid=value;}
			get{return _emailid;}
		}
		/// <summary>
		/// 实际链接地址
		/// </summary>
		public string RealLink
		{
			set{ _reallink=value;}
			get{return _reallink;}
		}
		#endregion Model

	}
}

