using System;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// 菜单实体
    /// </summary>
    [Serializable]
    public partial class MenuInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        #region Model
        private long _menuid;
        private string _nodename;
        private string _url;
        private long _preid = 0;
        private string _icocss;
        private int _menusort = 0;
        private int? _ishide = 0;
        private string _websiteowner;
        private int? _menutype = 1;
        private int _showlevel = 3;
        private int _targetblank = 0;

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuID
        {
            set { _menuid = value; }
            get { return _menuid; }
        }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName
        {
            set { _nodename = value; }
            get { return _nodename; }
        }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 所属ID
        /// </summary>
        public long PreID
        {
            set { _preid = value; }
            get { return _preid; }
        }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string ICOCSS
        {
            set { _icocss = value; }
            get { return _icocss; }
        }
        /// <summary>
        /// 菜单排序，越大排序越高
        /// </summary>
        public int MenuSort
        {
            set { _menusort = value; }
            get { return _menusort; }
        }
        /// <summary>
        /// 是否隐藏菜单
        /// </summary>
        public int? IsHide
        {
            set { _ishide = value; }
            get { return _ishide; }
        }
        /// <summary>
        /// 所属站点
        /// </summary>
        public string WebsiteOwner
        {
            set { _websiteowner = value; }
            get { return _websiteowner; }
        }
        /// <summary>
        /// 菜单所属类型
        /// </summary>
        public int? MenuType
        {
            set { _menutype = value; }
            get { return _menutype; }
        }

        /// <summary>
        /// 显示级别 3 所有人可见 2主账号可见 1仅超级管理员可见
        /// </summary>
        public int ShowLevel
        {
            get { return _showlevel; }
            set { _showlevel = value; }
        }
        /// <summary>
        /// 是否新标签显示
        /// </summary>
        public int TargetBlank
        {
            get { return _targetblank; }
            set { _targetblank = value; }
        }
        
        #endregion Model
    }
}
