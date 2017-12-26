using System;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 权限组
    /// </summary>
    [Serializable]
    public partial class PermissionGroupInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public PermissionGroupInfo()
        { }
        #region Model
        private long _groupid;
        private string _groupname;
        private string _groupdescription;
        private long _preid = 0;
        private int _grouptype = 0;
        /// <summary>
        /// 组ID
        /// </summary>
        public long GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
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
        /// 组说明
        /// </summary>
        public string GroupDescription
        {
            set { _groupdescription = value; }
            get { return _groupdescription; }
        }
        /// <summary>
        /// 所属ID
        /// </summary>
        public long PreID
        {
            set { _preid = value; }
            get { return _preid; }
        }
        /// <summary>
        /// 所属站点
        /// </summary>
        public string WebsiteOwner { set; get; }
        /// <summary>
        /// 组类型 
        /// 空或0未权限
        /// 1 为版本
        /// 2 为角色
        /// 3 管理员
        /// 4 渠道
        /// </summary>
        public int GroupType
        {
            set { _grouptype = value; }
            get { return _grouptype; }
        }
        #endregion Model

    }
}
