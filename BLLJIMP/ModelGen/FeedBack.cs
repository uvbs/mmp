using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class FeedBack : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _feedbackid;
        private string _title;
        private string _feedbackcontent;
        private DateTime _submitdate;
        private string _userid;
        private string _feedbacktype;
        private string _platformcategory;
        private string _feedbackstatus;
        private string _assignationuserid;
        private string _phone;
        private string _email;
        private string _attachments;
        private DateTime? _assignationdate;
        private DateTime? _processingdate;
        private DateTime? _processcompleteddate;
        private DateTime? _reprocessingdate;
        private DateTime? _closeddate;
        /// <summary>
        /// ID
        /// </summary>
        public int FeedBackID
        {
            set { _feedbackid = value; }
            get { return _feedbackid; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FeedBackContent
        {
            set { _feedbackcontent = value; }
            get { return _feedbackcontent; }
        }
        /// <summary>
        /// 提交反馈时间 (待处理时间)
        /// </summary>
        public DateTime SubmitDate
        {
            set { _submitdate = value; }
            get { return _submitdate; }
        }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 问题类型
        /// </summary>
        public string FeedBackType
        {
            set { _feedbacktype = value; }
            get { return _feedbacktype; }
        }
        /// <summary>
        /// 所属模块
        /// </summary>
        public string PlatformCategory
        {
            set { _platformcategory = value; }
            get { return _platformcategory; }
        }
        /// <summary>
        /// 问题状态
        /// </summary>
        public string FeedBackStatus
        {
            set { _feedbackstatus = value; }
            get { return _feedbackstatus; }
        }
        /// <summary>
        /// 问题分配给的用户ID
        /// </summary>
        public string AssignationUserID
        {
            set { _assignationuserid = value; }
            get { return _assignationuserid; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 附件路径 多个附件用，分隔
        /// </summary>
        public string Attachments
        {
            set { _attachments = value; }
            get { return _attachments; }
        }
        /// <summary>
        /// 已分派时间
        /// </summary>
        public DateTime? AssignationDate
        {
            set { _assignationdate = value; }
            get { return _assignationdate; }
        }
        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime? ProcessingDate
        {
            set { _processingdate = value; }
            get { return _processingdate; }
        }
        /// <summary>
        /// 处理完成时间
        /// </summary>
        public DateTime? ProcessCompletedDate
        {
            set { _processcompleteddate = value; }
            get { return _processcompleteddate; }
        }
        /// <summary>
        /// 再开放时间
        /// </summary>
        public DateTime? ReProcessingDate
        {
            set { _reprocessingdate = value; }
            get { return _reprocessingdate; }
        }
        /// <summary>
        /// 已关闭时间
        /// </summary>
        public DateTime? ClosedDate
        {
            set { _closeddate = value; }
            get { return _closeddate; }
        }
        #endregion Model


    }
}
