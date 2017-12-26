using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WXMemberFlowHistory:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WXMemberFlowHistory : ZentCloud.ZCBLLEngine.ModelTable
    {
        public WXMemberFlowHistory()
        { }
        #region Model
        private int _flowid;
        private string _weixinmemberid;
        private DateTime _begindate;
        private DateTime? _enddate;
        /// <summary>
        /// 
        /// </summary>
        public int FlowID
        {
            set { _flowid = value; }
            get { return _flowid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeixinMemberID
        {
            set { _weixinmemberid = value; }
            get { return _weixinmemberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BeginDate
        {
            set { _begindate = value; }
            get { return _begindate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        #endregion Model

    }
}
