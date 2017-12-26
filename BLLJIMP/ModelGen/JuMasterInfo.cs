using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 专家表
    /// </summary>
    [Serializable]
    public partial class JuMasterInfo : ZCBLLEngine.ModelTable
    {
        public JuMasterInfo()
        { }
        #region Model
        private string _masterid;
        private string _adduserid;
        private string _mastername;
        private string _gender;
        private string _company;
        private string _title;
        private string _summary;
        private string _introductioncontent;
        private string _headimg;
        private DateTime _inserdate = DateTime.Now;
        /// <summary>
        /// 专家ID
        /// </summary>
        public string MasterID
        {
            set { _masterid = value; }
            get { return _masterid; }
        }
        /// <summary>
        /// 添加用户ID
        /// </summary>
        public string AddUserID
        {
            set { _adduserid = value; }
            get { return _adduserid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string MasterName
        {
            set { _mastername = value; }
            get { return _mastername; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        {
            set { _gender = value; }
            get { return _gender; }
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
        /// 简介
        /// </summary>
        public string Summary
        {
            set { _summary = value; }
            get { return _summary; }
        }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string IntroductionContent
        {
            set { _introductioncontent = value; }
            get { return _introductioncontent; }
        }
        /// <summary>
        /// 头像缩略图
        /// </summary>
        public string HeadImg
        {
            set { _headimg = value; }
            get { return _headimg; }
        }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InserDate
        {
            set { _inserdate = value; }
            get { return _inserdate; }
        }

        public string WebsiteOwner { get; set; }

        #endregion Model

    }
}

