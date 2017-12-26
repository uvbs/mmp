using System;
using System.Linq;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 权限组
    /// </summary>
    public partial class PermissionGroupInfo
    {
        public string PmsIdsStr{ get; set; }

        public string MenuIdsStr{ get; set; }
        public string PmsColumnIdsStr { get; set; }
        /// <summary>
        /// 有无设置栏目
        /// </summary>
        public bool has_column { get; set; }
    }
}
