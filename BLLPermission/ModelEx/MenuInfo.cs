using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLPermission.Model
{
    public partial class MenuInfo
    {
        public MenuInfo()
        {
            ChildMenus = new List<MenuInfo>();
            IsHide = 0;
        }
        /// <summary>
        /// 是否是新增
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BaseName { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuInfo> ChildMenus { get; set; }
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
