using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 报名字段表
    /// </summary>
    [Serializable]
    public partial class ActivityFieldMappingInfo : ZCBLLEngine.ModelTable
    {
        public ActivityFieldMappingInfo()
        { }
        #region Model
        private string _activityid;
        private int _exfieldindex = 0;
        private string _mappingname;
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityID
        {
            set { _activityid = value; }
            get { return _activityid; }
        }
        /// <summary>
        /// 字段索引 1-60
        /// </summary>
        public int ExFieldIndex
        {
            set { _exfieldindex = value; }
            get { return _exfieldindex; }
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string MappingName
        {
            set { _mappingname = value; }
            get { return _mappingname; }
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        public int FieldIsNull { get; set; }
        /// <summary>
        /// 数据格式验证方法：邮箱、手机、表达式
        /// </summary>
        public string FormatValiFunc { get; set; }

        /// <summary>
        /// 格式验证表达式
        /// </summary>
        public string FormatValiExpression { get; set; }

        /// <summary>
        /// 字段类型 1是微信推广字段  0或null 表示普通字段 
        /// </summary>
        public int FieldType { get; set; }

        /// <summary>
        /// 是否多行
        /// </summary>
        public int IsMultiline { get; set; }

        /// <summary>
        /// 1 在提交活动信息页面隐藏
        /// 空或0 显示
        /// </summary>
        public string IsHideInSubmitPage { get; set; }

        /// <summary>
        /// 输入类型
        /// text 文本框
        /// combox 下拉框
        /// checkbox 多选框
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// 选项列表 多个选项用逗号隔开
        /// </summary>
        public string Options { get; set; }
        #endregion Model

    }
}
