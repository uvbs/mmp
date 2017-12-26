using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 真实活动信息 中间表
    /// </summary>
    [Serializable]
    public partial class ActivityInfo : ZCBLLEngine.ModelTable
    {
        public ActivityInfo()
        { }
        #region Model
        private string _activityid;
        private string _userid;
        private string _activityname;
        private DateTime? _activitydate;
        private string _activityaddress;
        private string _activitywebsite;
        private string _activitydescription;
        private string _confirmsmscontent;
        private int _activitystatus = 1;
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityID
        {
            set { _activityid = value; }
            get { return _activityid; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string ActivityName
        {
            set { _activityname = value; }
            get { return _activityname; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? ActivityDate
        {
            set { _activitydate = value; }
            get { return _activitydate; }
        }

       
        /// <summary>
        /// 地点
        /// </summary>
        public string ActivityAddress
        {
            set { _activityaddress = value; }
            get { return _activityaddress; }
        }
        /// <summary>
        /// 网址
        /// </summary>
        public string ActivityWebsite
        {
            set { _activitywebsite = value; }
            get { return _activitywebsite; }
        }
        /// <summary>
        /// 描述说明
        /// </summary>
        public string ActivityDescription
        {
            set { _activitydescription = value; }
            get { return _activitydescription; }
        }
        /// <summary>
        /// 确认短信
        /// </summary>
        public string ConfirmSMSContent
        {
            set { _confirmsmscontent = value; }
            get { return _confirmsmscontent; }
        }
        /// <summary>
        /// 活动状态:1启用，0禁用
        /// </summary>
        public int ActivityStatus
        {
            set { _activitystatus = value; }
            get { return _activitystatus; }
        }
        /// <summary>
        /// 同一IP限制访问次数
        /// </summary>
        public int LimitCount { get; set; }
        /// <summary>
        /// 管理员通知手机号，多个手机号用,分隔
        /// </summary>
        public string AdminPhone { get; set; }

        /// <summary>
        /// 管理员短信通知内容
        /// </summary>
        public string AdminSMSContent { get; set; }

        /// <summary>
        /// 设置报名不允许重复的字段 格式如K1,K2,K3 多个字段用,分隔, 如要禁用重复检查用 none ,默认为姓名手机检查
        /// </summary>
        public string DistinctKeys { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 字段排序
        /// </summary>
        public string FieldSort { get; set; }

        /// <summary>
        /// 群发短信次数
        /// </summary>
        public int GroupSendSmsCount { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 有人报名通知到的客服ID 关联表ZCJ_WXKeFu
        /// </summary>
        public string ActivityNoticeKeFuId { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int SignInCount
        {
            get
            {
                try
                {

                    return new BLLActivity("").GetSignInCount(_activityid);

                }
                catch (Exception)
                {

                    return 0;
                }
            }
        }
        #endregion Model

    }
}
