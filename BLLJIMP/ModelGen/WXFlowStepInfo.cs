using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WXFlowStepInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WXFlowStepInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public WXFlowStepInfo()
        { }
        #region Model
        private int _flowid;
        private int _stepid;
        private string _flowfield;
        private string _fielddescription;
        private string _sendmsg;
        private string _errormsg;
        private string _authfunc;

        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID
        {
            set { _flowid = value; }
            get { return _flowid; }
        }
        /// <summary>
        /// 步骤ID：每个流程步骤ID从1开始增长
        /// </summary>
        public int StepID
        {
            set { _stepid = value; }
            get { return _stepid; }
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
        /// 字段描述
        /// </summary>
        public string FieldDescription
        {
            set { _fielddescription = value; }
            get { return _fielddescription; }
        }
        /// <summary>
        /// 下发信息
        /// </summary>
        public string SendMsg
        {
            set { _sendmsg = value; }
            get { return _sendmsg; }
        }
        /// <summary>
        /// 验证失败信息
        /// </summary>
        public string ErrorMsg
        {
            set { _errormsg = value; }
            get { return _errormsg; }
        }
        /// <summary>
        /// 验证方法
        /// </summary>
        public string AuthFunc
        {
            set { _authfunc = value; }
            get { return _authfunc; }
        }
        /// <summary>
        /// 自增ID
        /// </summary>
        public int AutoID {get;set;}
        /// <summary>
        /// 是否需要验证码
        /// </summary>
        public int IsVerifyCode { get; set; }

        #endregion Model

    }
}
