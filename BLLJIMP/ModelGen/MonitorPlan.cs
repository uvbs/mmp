using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 监测任务
    /// </summary>
    [Serializable]
    public partial class MonitorPlan : ZCBLLEngine.ModelTable
    {

        #region Model
        private int _monitorplanid;
        private string _planname;
        private string _planstatus;
        private string _userid;
        private DateTime _insertdate;
        private string _remark;
        /// <summary>
        /// 任务ID
        /// </summary>
        public int MonitorPlanID
        {
            set { _monitorplanid = value; }
            get { return _monitorplanid; }
        }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string PlanName
        {
            set { _planname = value; }
            get { return _planname; }
        }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string PlanStatus
        {
            set { _planstatus = value; }
            get { return _planstatus; }
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
        /// 插入日期
        /// </summary>
        public DateTime InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// 删除标识
        /// </summary>
        public int IsDelete { get; set; }

        #endregion Model
    }
}
