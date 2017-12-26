using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 接收人列表
	/// </summary>
	[Serializable]
    public partial class EmailDetails : ZCBLLEngine.ModelTable
	{
        public EmailDetails()
		{}
		#region Model
		private string _emailid;
		private string _receiveremail;
		private string _receivername;
		private int _sendstatus=0;
		private DateTime? _senddate;
		private string _sendaccountid;
		private string _receivecontent;
		private string _otherdescription;
		/// <summary>
		/// 邮件ID
		/// </summary>
		public string EmailID
		{
			set{ _emailid=value;}
			get{return _emailid;}
		}
		/// <summary>
		/// 接收人邮箱地址
		/// </summary>
		public string ReceiverEmail
		{
			set{ _receiveremail=value;}
			get{return _receiveremail;}
		}
		/// <summary>
		/// 接收人姓名
		/// </summary>
		public string ReceiverName
		{
			set{ _receivername=value;}
			get{return _receivername;}
		}
		/// <summary>
		/// 发送状态
        ///0.	等待处理；
        ///1.	正在处理；
        ///2.	等待发送；
        ///3.	正在发送；
        ///4.	完成发送；
        ///-1.	发送失败；
		/// </summary>
		public int SendStatus
		{
			set{ _sendstatus=value;}
			get{return _sendstatus;}
		}
		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime? SendDate
		{
			set{ _senddate=value;}
			get{return _senddate;}
		}
		/// <summary>
		/// 发送账号
		/// </summary>
		public string SendAccountID
		{
			set{ _sendaccountid=value;}
			get{return _sendaccountid;}
		}
		/// <summary>
		/// 接收内容
		/// </summary>
		public string ReceiveContent
		{
			set{ _receivecontent=value;}
			get{return _receivecontent;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string OtherDescription
		{
			set{ _otherdescription=value;}
			get{return _otherdescription;}
		}
		#endregion Model

	}
}

