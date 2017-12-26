using System;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 权限实体
    /// </summary>
    [Serializable]
    public partial class PermissionInfo: ZentCloud.ZCBLLEngine.ModelTable
    {
        public PermissionInfo()
        { }
        #region Model
        private long _permissionid;
        private string _url;
        private long? _menuid;
        private string _permissionname;
        private string _permissiondescription;
        private int _permissioncateid=0;
        private int _termissiontype=0;
        private string _permissionaction;
        /// <summary>
        /// 权限ID
        /// </summary>
        public long PermissionID
        {
            set { _permissionid = value; }
            get { return _permissionid; }
        }
        /// <summary>
        /// 权限链接
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long? MenuID
        {
            set { _menuid = value; }
            get { return _menuid; }
        }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName
        {
            set { _permissionname = value; }
            get { return _permissionname; }
        }
        /// <summary>
        /// 权限说明
        /// </summary>
        public string PermissionDescription
        {
            set { _permissiondescription = value; }
            get { return _permissiondescription; }
        }
        /// <summary>
        /// 权限键值代码
        /// </summary>
        public string PermissionKey { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public int PermissionCateId
        {
            set { _permissioncateid = value; }
            get { return _permissioncateid; }
        }
        /// <summary>
        /// 类型 0页面 1处理器 2操作
        /// </summary>
        public int PermissionType
        {
            set { _termissiontype = value; }
            get { return _termissiontype; }
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        public string PermissionAction
        {
            set { _permissionaction = value; }
            get { return _permissionaction; }
        }
        #endregion Model

    }
}
