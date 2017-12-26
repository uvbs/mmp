using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 无用
    /// </summary>
    [Serializable]
    public partial class MemberGroupInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public MemberGroupInfo()
        { }
        #region Model
        private string _groupid;
        private string _userid;
        private string _groupname;
        private DateTime _adddate;
        private int? _grouptype;

        private int? _membercount;

        /// <summary>
        /// 组编号
        /// </summary>
        public string GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
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
        /// 组名称
        /// </summary>
        public string GroupName
        {
            set { _groupname = value; }
            get { return _groupname; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate
        {
            set { _adddate = value; }
            get { return _adddate; }
        }
        /// <summary>
        /// 组类型：1.客户管理分组组；2.邮箱库分组；3.微博用户分组
        /// </summary>
        public int? GroupType
        {
            set { _grouptype = value; }
            get { return _grouptype; }
        }

        /// <summary>
        /// 成员数量
        /// </summary>
        public int? MemberCount
        {
            set { _membercount = value; }
            get { return _membercount; }
            //get 
            //{
            //    if (_membercount == null)
            //    {
            //        int count = new BLLEDM("").UpdateEmailGroupMemberCount(_groupid);
            //        return count;
            //    }
            //    else
            //    {
            //        return _membercount;
            //    }
            //}
        }
        /// <summary>
        /// 组的描述说明
        /// </summary>
        public string Description { get; set; }

        #endregion Model

    }
}
