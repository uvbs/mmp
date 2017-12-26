using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// ZCJ_MenuRelationInfo
    /// </summary>
    [Serializable]
    public partial class MenuRelationInfo : ModelTable
    {
        /// <summary>
        /// 关系ID
        /// </summary>
        public string RelationID { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuID { get; set; }
        /// <summary>
        /// 关系类型 
        /// 0权限组跟菜单关系  RelationID权限组ID 
        /// 1菜单隐藏 RelationID站点WebsiteOwner
        /// 2菜单显示级别 RelationID站点WebsiteOwner
        /// 3菜单显示排序 RelationID站点WebsiteOwner Ex1显示排序
        /// 4菜单显示名称 RelationID站点WebsiteOwner Ex2显示名称
        /// 5权限栏目对应菜单 RelationID权限栏目ID
        /// 6权限栏目隐藏 RelationID站点WebsiteOwner MenuID代表栏目
        /// 7权限栏目显示排序 RelationID站点WebsiteOwner MenuID代表栏目 Ex1显示排序
        /// 8权限栏目显示名称 RelationID站点WebsiteOwner MenuID代表栏目 Ex2显示名称
        /// </summary>
        public int RelationType { get; set; }

        /// <summary>
        /// 扩展字段
        /// 关系类型2时 显示级别
        /// 关系类型3时 显示排序
        /// </summary>
        public int Ex1 { get; set; }
        /// <summary>
        /// 扩展字段
        /// 关系类型3时 显示名称
        /// </summary>
        public string Ex2 { get; set; }
    }
}