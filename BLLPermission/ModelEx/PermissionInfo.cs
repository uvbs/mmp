using System;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 权限实体
    /// </summary>
    public partial class PermissionInfo
    {
        public string MenuName
        {
            get
            {
                try
                {
                    if (_menuid.Value > 0)
                    {
                        return new BLLMenuPermission("").Get<MenuInfo>(string.Format(" MenuID = {0}", _menuid.Value.ToString())).NodeName;
                    }
                }
                catch { }

                return "-";
            }
        }
        public string PermissionCate { get; set; }
    }
}
