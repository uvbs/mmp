using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WXFlowInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WXFlowInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public WXFlowInfo()
        { }
        #region Model
        private int _flowid;
        private string _userid;
        private string _flowname;
        private string _flowkeyword;
        private string _flowendmsg;
        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID
        {
            set { _flowid = value; }
            get { return _flowid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName
        {
            set { _flowname = value; }
            get { return _flowname; }
        }
        /// <summary>
        /// 流程关键字
        /// </summary>
        public string FlowKeyword
        {
            set { _flowkeyword = value; }
            get { return _flowkeyword; }
        }
        /// <summary>
        /// 流程结束信息
        /// </summary>
        public string FlowEndMsg
        {
            set { _flowendmsg = value; }
            get { return _flowendmsg; }
        }
        /// <summary>
        /// 会员限制状态:启用则一个流程一个会员只能走一次，禁用则多次
        /// </summary>
        public int MemberLimitState { get; set; }
        /// <summary>
        /// 流程限制信息
        /// </summary>
        public string FlowLimitMsg { get; set; }

        /// <summary>
        /// 流程系统类型:1正常流程，3内定注册流程
        /// </summary>
        public int FlowSysType { get; set; }

        /// <summary>
        /// 是否启用流程
        /// </summary>
        public int IsEnable { get; set; }

        #endregion Model

    }
}
