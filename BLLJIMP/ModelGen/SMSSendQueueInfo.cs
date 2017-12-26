using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// SMSSendQueueInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class SMSSendQueueInfo : ZentCloud.ZCBLLEngine.ModelTable
	{
		public SMSSendQueueInfo()
		{}
        #region Model
        private int _autoid;
        private string _planid;
        private string _receiver;
        private int _issimulate;
        private string _smscontent;

        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 发送号码
        /// </summary>
        public string Receiver
        {
            set { _receiver = value; }
            get { return _receiver; }
        }
        /// <summary>
        /// 模拟发送状态
        /// </summary>
        public int IsSimulate
        {
            set { _issimulate = value; }
            get { return _issimulate; }
        }

        public string SMSContent
        {
            set { _smscontent = value; }
            get { return _smscontent; }
        }
        #endregion Model

	}
}

