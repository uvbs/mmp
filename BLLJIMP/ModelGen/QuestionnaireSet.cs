using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 答题
    /// </summary>
    public partial class QuestionnaireSet : ModelTable
    {
        public QuestionnaireSet()
        { }
        #region Model
        private int _autoid;
        private string _title;
        private string _img;
        private string _description;
        private string _bgimgindex;
        private string _bgimganswer;
        private string _bgimgend;
        private int? _questionnaireid;
        private int? _questioncount;
        private int? _isquestionrandom;
        private int? _isoptionrandom;
        private string _websiteowner;
        private int? _ismoreanswer;
        private int _isdelete = 0;
        private int? _wincount;
        private string _windescription;
        private string _winbtntext;
        private string _winbtnurl;
        private DateTime _createtime;
        private string _createuserid;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Img
        {
            set { _img = value; }
            get { return _img; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BgImgIndex
        {
            set { _bgimgindex = value; }
            get { return _bgimgindex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BgImgAnswer
        {
            set { _bgimganswer = value; }
            get { return _bgimganswer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BgImgEnd
        {
            set { _bgimgend = value; }
            get { return _bgimgend; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? QuestionnaireId
        {
            set { _questionnaireid = value; }
            get { return _questionnaireid; }
        }
        /// <summary>
        /// 题目数量
        /// </summary>
        public int? QuestionCount
        {
            set { _questioncount = value; }
            get { return _questioncount; }
        }
        /// <summary>
        /// 是否题目rand
        /// </summary>
        public int? IsQuestionRandom
        {
            set { _isquestionrandom = value; }
            get { return _isquestionrandom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsOptionRandom
        {
            set { _isoptionrandom = value; }
            get { return _isoptionrandom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WebsiteOwner
        {
            set { _websiteowner = value; }
            get { return _websiteowner; }
        }
        /// <summary>
        /// 是否多次答题
        /// </summary>
        public int? IsMoreAnswer
        {
            set { _ismoreanswer = value; }
            get { return _ismoreanswer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        /// <summary>
        /// 答对多少题过关
        /// </summary>
        public int? WinCount
        {
            set { _wincount = value; }
            get { return _wincount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WinDescription
        {
            set { _windescription = value; }
            get { return _windescription; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WinBtnText
        {
            set { _winbtntext = value; }
            get { return _winbtntext; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WinBtnUrl
        {
            set { _winbtnurl = value; }
            get { return ZentCloud.Common.StringHelper.ReplaceLinkRD(_winbtnurl); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUserId
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        public int Score { set; get; }
        public int ScoreNum { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public int QuestionScore { set; get; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// 阅读人数
        /// </summary>
        public int UV { get; set; }
        #endregion Model
    }
}
