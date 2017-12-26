using System;

namespace ZentCloud.BLLPermission.Model
{
    /// <summary>
    /// module过滤验证信息
    /// </summary>
    [Serializable]
    public partial class ModuleFilterInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public ModuleFilterInfo()
        { }
        #region Model
        private string _filtertype;
        private string _pagepath;
        private string _filterdescription;
        /// <summary>
        /// 过滤类型
        /// </summary>
        public string FilterType
        {
            set { _filtertype = value; }
            get { return _filtertype; }
        }
        /// <summary>
        /// 过滤页面
        /// </summary>
        public string PagePath
        {
            set { _pagepath = value; }
            get { return _pagepath; }
        }
        /// <summary>
        /// 过滤说明
        /// </summary>
        public string FilterDescription
        {
            set { _filterdescription = value; }
            get { return _filterdescription; }
        }
        /// <summary>
        /// 匹配类型：all,start,end,contains
        /// </summary>
        public string MatchType { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex1 { get; set; }
        #endregion Model

    }
}
