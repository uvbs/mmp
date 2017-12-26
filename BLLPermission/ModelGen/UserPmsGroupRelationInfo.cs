using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 用户组关系
    /// </summary>
    [Serializable]
    public partial class UserPmsGroupRelationInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public UserPmsGroupRelationInfo()
        { }
        #region Model
        private string _userid;
        private long _groupid;
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        #endregion Model

    }
}
