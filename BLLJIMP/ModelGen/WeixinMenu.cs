using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信自定义菜单类
    /// </summary>
    [Serializable]
    public partial class WeixinMenu : ZentCloud.ZCBLLEngine.ModelTable
    {
        #region Model
        private long _menuid;
        private string _nodename;
        private long _preid;
        private int _menusort;
        private int? _ishide;
        private string _keyorurl;
        private string _type;
        private string _userid;
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuID
        {
            set { _menuid = value; }
            get { return _menuid; }
        }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string NodeName
        {
            set { _nodename = value; }
            get { return _nodename; }
        }
        /// <summary>
        /// 上级菜单ID
        /// </summary>
        public long PreID
        {
            set { _preid = value; }
            get { return _preid; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int MenuSort
        {
            set { _menusort = value; }
            get { return _menusort; }
        }
        /// <summary>
        /// 是否显示 0表示隐藏 1表示显示
        /// </summary>
        public int? IsHide
        {
            set { _ishide = value; }
            get { return _ishide; }
        }
        /// <summary>
        /// Key 值 或链接地址
        /// </summary>
        public string KeyOrUrl
        {
            set { _keyorurl = value; }
            get { return _keyorurl; }
        }
        /// <summary>
        /// 菜单按钮类型 click 或view
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        #endregion Model
    }
}
