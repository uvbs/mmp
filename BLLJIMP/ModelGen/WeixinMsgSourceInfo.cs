using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 图文素材
    /// </summary>
    [Serializable]
  public partial  class WeixinMsgSourceInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 图文素材
        /// </summary>
        public WeixinMsgSourceInfo()
		{}
		#region Model
		private string _sourceid;
		private string _userid;
		private string _title;
		private string _description;
		private string _picurl;
		private string _url;
		/// <summary>
		/// 缩略图ID
		/// </summary>
		public string SourceID
		{
			set{ _sourceid=value;}
			get{return _sourceid;}
		}
		/// <summary>
		/// 用户名 站点所有者
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 图片URL
		/// </summary>
		public string PicUrl
		{
			set{ _picurl=value;}
			get{return _picurl;}
		}
		/// <summary>
		/// 跳转链接
		/// </summary>
		public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
        /// <summary>
        /// 发送成功数
        /// </summary>
        public int SendSuccessCount { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 缩略图媒体ID
        /// </summary>
        public string ThumbMediaId { get; set; }
		#endregion Model

    }
}
