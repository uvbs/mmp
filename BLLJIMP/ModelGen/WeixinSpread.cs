using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public partial class WeixinSpread : ZCBLLEngine.ModelTable
    {
        #region Model
        private long _weixinspreadid;
        private string _spreadname;
        private string _spreadurl;
        private string _activityid;
        private string _planid;
        private string _status;
        private DateTime _insertdate;
        private string _userid;
        /// <summary>
        /// 微信推广ID
        /// </summary>
        public long WeixinSpreadID
        {
            set { _weixinspreadid = value; }
            get { return _weixinspreadid; }
        }
        /// <summary>
        /// 推广名称
        /// </summary>
        public string SpreadName
        {
            set { _spreadname = value; }
            get { return _spreadname; }
        }
        /// <summary>
        /// 真实链接
        /// </summary>
        public string SpreadUrl
        {
            set { _spreadurl = value; }
            get { return _spreadurl; }
        }
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityID
        {
            set { _activityid = value; }
            get { return _activityid; }
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
        /// 状态 0 1
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 插入日期
        /// </summary>
        public DateTime InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        #endregion Model
    }
}
