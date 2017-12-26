using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 监测链接
    /// </summary>
    [Serializable]
    public partial class MonitorLinkInfo : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _linkid;
        private int _monitorplanid;
        private long? _wxmemberid;
        private string _linkname;
        private string _reallink;
        private DateTime? _insertdate;
        private string _encryptparameter;
        private int _opencount;
        private int _distinctopencount;
       
        /// <summary>
        /// 链接ID
        /// </summary>
        public int LinkID
        {
            set { _linkid = value; }
            get { return _linkid; }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int MonitorPlanID
        {
            set { _monitorplanid = value; }
            get { return _monitorplanid; }
        }
        /// <summary>
        /// 公众号注册会员ID
        /// </summary>
        public long? WXMemberID
        {
            set { _wxmemberid = value; }
            get { return _wxmemberid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string LinkName
        {
            get { return _linkname; }
            set { _linkname = value; }
            
        }
        /// <summary>
        /// 文章名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 文章缩略图
        /// </summary>
        public string ThumbnailsPath { get; set; }

        /// <summary>
        /// 真实链接
        /// </summary>
        public string RealLink
        {
            set { _reallink = value; }
            get { return _reallink; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EncryptParameter
        {
            set { _encryptparameter = value; }
            get { return _encryptparameter; }
        }
        /// <summary>
        /// 打开人数
        /// </summary>
        public int OpenCount
        {
            set { _opencount = value; }
            get { return _opencount; }
        }
        /// <summary>
        /// 打开人次
        /// </summary>
        public int DistinctOpenCount
        {
            set { _distinctopencount = value; }
            get { return _distinctopencount; }
        }
        /// <summary>
        /// 分享次数
        /// </summary>
        public int ShareCount
        {
            get;
            set;
           
        }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 报名活动编号
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 类型   null为转发   fans为粉丝
        /// </summary>
        public string ForwardType { get; set; }

        /// <summary>
        /// 吸粉人数
        /// </summary>
        public int PowderCount { get; set; }

        /// <summary>
        /// 微信阅读人数
        /// </summary>
        public int UV { get; set; }

        /// <summary>
        /// 活动报名人数
        /// </summary>
        public int ActivitySignUpCount { get; set; }

        /// <summary>
        /// 答题人数
        /// </summary>
        public int AnswerCount { get; set; }

        #endregion Model

    }
}
