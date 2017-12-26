using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微网站 -底部工具栏
    /// </summary>
    public partial class CompanyWebsite_ToolBar : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 幻灯片名称
        /// </summary>
        public string ToolBarName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string ToolBarDescription { get; set; }
        /// <summary>
        /// 播放顺序 从小到大
        /// </summary>
        public int PlayIndex { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string ToolBarImage { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public string IsShow { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string KeyType { get; set; }
        /// <summary>
        /// 使用分类 foottool底部工具 tab选项卡工具 button按钮工具
        /// </summary>
        public string UseType { get; set; }
        
        /// <summary>
        /// 类型 
        /// </summary>
        public string ToolBarType { get; set; }
        /// <summary>
        /// 类型 对应值
        /// </summary>
        private string _link;
        public string ToolBarTypeValue
        {
            get { return ZentCloud.Common.StringHelper.ReplaceLinkRD(_link); }
            set { _link = value; }
        }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 选中背景色
        /// </summary>
        public string ActBgColor { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public string BgColor { get; set; }
        /// <summary>
        /// 选中字色
        /// </summary>
        public string ActColor { get; set; }
        /// <summary>
        /// 字色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public int PreID { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string ImageUrl { get; set; }
        
        /// <summary>
        /// 图标颜色
        /// </summary>
        public string IcoColor { get; set; }
        /// <summary>
        /// 是否基础导航
        /// </summary>
        public int IsSystem
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(WebsiteOwner))
                    return 0;
                return 1;
            }
            set{
                int isSystem = value;
            }
        }
        /// <summary>
        /// 可见权限组
        /// </summary>
        public string PermissionGroup { get; set; }
        /// <summary>
        /// 原基础导航ID
        /// </summary>
        public int BaseID { get; set; }

        /// <summary>
        /// 选中背景图片
        /// </summary>
        public string ActBgImage { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        public string BgImage { get; set; }
        
        /// <summary>
        /// 图标位置 0文字上方 1文字左边 2文字右边 3文字下边 4仅显示图标
        /// </summary>
        public int IcoPosition { get; set; }
        /// <summary>
        /// 链接选择类型
        /// </summary>
        public string Stype { get; set; }
        /// <summary>
        /// 链接选择值
        /// </summary>
        public string Svalue { get; set; }
        /// <summary>
        /// 链接选择名称
        /// </summary>
        public string Stext { get; set; }
        /// <summary>
        /// 是否电脑端
        /// </summary>
        public int IsPc { get; set; }
        /// <summary>
        /// 0所有人可见 1仅分销员可见 2指定权限组可见
        /// </summary>
        public int VisibleSet { get; set; }
        public string RightText { get; set; }
    }
}
