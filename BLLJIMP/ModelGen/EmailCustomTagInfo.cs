using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 邮件个性化标签表
	/// </summary>
	[Serializable]
    public partial class EmailCustomTagInfo : ZCBLLEngine.ModelTable
	{
		public EmailCustomTagInfo()
		{}
		#region Model
		private string _customtag;
		private string _tagdescription;
		/// <summary>
		/// 
		/// </summary>
		public string CustomTag
		{
			set{ _customtag=value;}
			get{return _customtag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TagDescription
		{
			set{ _tagdescription=value;}
			get{return _tagdescription;}
		}
		#endregion Model

	}
}

