using System;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 权限关系实体
    /// </summary>
    [Serializable]
    public partial class PermissionRelationInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public PermissionRelationInfo()
        { }
        #region Model
        private string _relationid;
        private long _permissionid;
        private int _relationtype = 0;
        /// <summary>
        /// 用户|组ID
        /// </summary>
        public string RelationID
        {
            set { _relationid = value; }
            get { return _relationid; }
        }
        /// <summary>
        /// 权限ID
        /// </summary>
        public long PermissionID
        {
            set { _permissionid = value; }
            get { return _permissionid; }
        }
        /// <summary>
        /// 关系类型:
        /// 0为权限组权限关系,
        /// 1为用户权限关系 
        /// 2权限栏目权限关系 
        /// 3权限组权限栏目关系 
        /// 9站点禁用权限关系
        /// </summary>
        public int RelationType
        {
            set { _relationtype = value; }
            get { return _relationtype; }
        }
        #endregion Model

    }
}
