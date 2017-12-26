using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
  public class UserRemind : ZCBLLEngine.ModelTable
    {
		#region Model
		private int _remindid;
		private string _userid;
		private string _memberid;
		private string _title;
		private string _content;
		private int? _isdefinitetime;
		private DateTime? _reminddate;
		private DateTime? _remindtime;
		private int? _isenable;
		private DateTime? _adddate;
		/// <summary>
		/// 
		/// </summary>
		public int RemindID
		{
			set{ _remindid=value;}
			get{return _remindid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MemberID
		{
			set{ _memberid=value;}
			get{return _memberid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsDefiniteTime
		{
			set{ _isdefinitetime=value;}
			get{return _isdefinitetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? RemindDate
		{
			set{ _reminddate=value;}
			get{return _reminddate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? RemindTime
		{
			set{ _remindtime=value;}
			get{return _remindtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsEnable
		{
			set{ _isenable=value;}
			get{return _isenable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddDate
		{
			set{ _adddate=value;}
			get{return _adddate;}
		}
        /// <summary>
        /// 是否已经提醒过
        /// </summary>
        public int IsRemind { get; set; }
		#endregion Model

    }
}
