using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微博评论任务表
    /// </summary>
    [Serializable]
    public partial class WeiboCommentsPlanInfo : ZCBLLEngine.ModelTable
    {
        public WeiboCommentsPlanInfo()
        { }
        #region Model
        private int _planid;
        private string _userid;
        private DateTime _submitdate = DateTime.Now;
        private string _planstatus;
        private string _execuserid;
        /// <summary>
        /// 任务ID
        /// </summary>
        public int PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 提交人
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
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
        /// 执行人
        /// </summary>
        public string ExecUserID
        {
            set { _execuserid = value; }
            get { return _execuserid; }
        }
        /// <summary>
        /// 微博转发内容
        /// </summary>
        public string PlanContents { get; set; }
        /// <summary>
        /// 微博@用户ID
        /// </summary>
        public string PlanWeiboUIDs { get; set; }
        #endregion Model

    }
}

