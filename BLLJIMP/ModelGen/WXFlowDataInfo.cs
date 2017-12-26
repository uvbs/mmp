using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WXFlowDataInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WXFlowDataInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public WXFlowDataInfo()
        { }
        #region Model
        private int _flowid;
        private int _stepid;
        private string _openid;
        private string _flowfield;
        private string _data;
        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID
        {
            set { _flowid = value; }
            get { return _flowid; }
        }
        /// <summary>
        /// 步骤ID
        /// </summary>
        public int StepID
        {
            set { _stepid = value; }
            get { return _stepid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OpenID
        {
            set { _openid = value; }
            get { return _openid; }
        }
        /// <summary>
        /// 流程字段
        /// </summary>
        public string FlowField
        {
            set { _flowfield = value; }
            get { return _flowfield; }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data
        {
            set { _data = value; }
            get { return _data; }
        }

        public DateTime InsertDate { get; set; }
        #endregion Model

    }
}
