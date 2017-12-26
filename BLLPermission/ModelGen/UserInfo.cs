using System;

namespace ZentCloud.BLLPermission.Model
{
    [Serializable]
    public partial class UserInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public UserInfo()
        { }
        public int AutoID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 所属权限组ID
        /// </summary>
        public long? PermissionGroupID { get; set; }

        public int UserType { get; set; }

        /// <summary>
        /// 是否已被禁用用户
        /// </summary>
        public int IsDisable { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
