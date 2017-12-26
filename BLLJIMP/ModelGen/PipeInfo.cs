using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// PipeInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class PipeInfo : ZentCloud.ZCBLLEngine.ModelTable
	{
		public PipeInfo()
		{}
        #region Model
        private int _autoid;
        private string _pipeid;
        private string _pipename;
        private string _pipesource;
        private string _description;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 通道ID
        /// </summary>
        public string PipeID
        {
            set { _pipeid = value; }
            get { return _pipeid; }
        }
        /// <summary>
        /// 通道名
        /// </summary>
        public string PipeName
        {
            set { _pipename = value; }
            get { return _pipename; }
        }
        /// <summary>
        /// 通道来源
        /// </summary>
        public string PipeSource
        {
            set { _pipesource = value; }
            get { return _pipesource; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        #endregion Model
	}
}

