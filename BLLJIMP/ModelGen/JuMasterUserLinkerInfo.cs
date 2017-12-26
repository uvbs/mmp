using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 专家用户联系信息表
    /// </summary>
    [Serializable]
    public partial class JuMasterUserLinkerInfo : ZCBLLEngine.ModelTable
    {
        public JuMasterUserLinkerInfo()
        { }
        #region Model
        private int _linkerid;
        private string _masterid;
        private string _subuserid;
        private string _name;
        private string _company;
        private string _title;
        private string _mobile;
        private string _email;
        private string _otherdescription;
        private DateTime _submitdate = DateTime.Now;
        private string _submitip;
        /// <summary>
        /// 联系信息ID
        /// </summary>
        public int LinkerID
        {
            set { _linkerid = value; }
            get { return _linkerid; }
        }
        /// <summary>
        /// 专家ID
        /// </summary>
        public string MasterID
        {
            set { _masterid = value; }
            get { return _masterid; }
        }
        /// <summary>
        /// 提交用户ID
        /// </summary>
        public string SubUserID
        {
            set { _subuserid = value; }
            get { return _subuserid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company
        {
            set { _company = value; }
            get { return _company; }
        }
        /// <summary>
        /// 职位
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 其他说明
        /// </summary>
        public string OtherDescription
        {
            set { _otherdescription = value; }
            get { return _otherdescription; }
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
        /// 提交IP
        /// </summary>
        public string SubmitIP
        {
            set { _submitip = value; }
            get { return _submitip; }
        }
        #endregion Model

    }
}

