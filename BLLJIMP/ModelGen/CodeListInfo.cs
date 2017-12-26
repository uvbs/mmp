using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// CodeListInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CodeListInfo : ZCBLLEngine.ModelTable
    {
        public CodeListInfo()
        { }
        #region Model
        private int _autoid;
        private string _codevalue;
        private string _codename;
        private string _codetype;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 代码值
        /// </summary>
        public string CodeValue
        {
            set { _codevalue = value; }
            get { return _codevalue; }
        }
        /// <summary>
        /// 代码名称
        /// </summary>
        public string CodeName
        {
            set { _codename = value; }
            get { return _codename; }
        }
        /// <summary>
        /// 代码类型
        /// </summary>
        public string CodeType
        {
            set { _codetype = value; }
            get { return _codetype; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        #endregion Model

    }
}

