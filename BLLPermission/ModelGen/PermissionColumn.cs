using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLPermission.Model
{
    [Serializable]
    public partial class PermissionColumn : ModelTable
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        public long PermissionColumnID { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string PermissionColumnName { get; set; }
        /// <summary>
        /// 上级栏目ID
        /// </summary>
        public long PermissionColumnPreID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 基础来源ID
        /// </summary>
        public long PermissionColumnBaseID { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int IsHide { get; set; }
    }
}
