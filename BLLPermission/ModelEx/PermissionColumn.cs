using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLPermission.Model
{
    public partial class PermissionColumn
    {
        //有菜单
        public bool HaveMenu { get; set; }
        //有权限
        public bool HavePermission { get; set; }
    }
}
