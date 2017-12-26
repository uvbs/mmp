using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 字段映射表
    /// </summary>
    public partial class TableFieldMapping : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoId { get; set; }
       /// <summary>
       /// 数据库表名
       /// </summary>
       public string TableName { get; set; }
       /// <summary>
       /// 字段显示名称
       /// </summary>
       public string MappingName { get; set; }
       /// <summary>
       /// 字段名称
       /// </summary>
       public string Field { get; set; }
       /// <summary>
       /// 字段类型 img sex date number 
       /// </summary>
       private string fieldType;
       /// <summary>
       /// 输入类型
       /// text 或空 文本框
       /// combox 下拉框
       /// checkbox 多选框
       /// </summary>
       public string FieldType {
           get {
               return fieldType;
           }
           set
           { 
               if(!string.IsNullOrWhiteSpace(value)){
                   fieldType = value;
               }
            } 
       }
       /// <summary>
       /// 字段可为空
       /// </summary>
       public int FieldIsNull { get; set; }
       /// <summary>
       /// 字段规则
       /// </summary>
       private string formatValiFunc;
       public string FormatValiFunc
       {
           get
           {
               return formatValiFunc;
           }
           set
           {
               if (!string.IsNullOrWhiteSpace(value))
               {
                   formatValiFunc = value;
               }
           }
       }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebSiteOwner { get; set; }
        /// <summary>
        /// 映射类型
        /// </summary>
       public int MappingType { get; set; }
        /// <summary>
        /// 模块类型
        /// </summary>
       public string ModuleType { get; set; }
        /// <summary>
        /// 关联ID，如一个表根据不同分类ID展现不同意义
        /// 
       /// ZCJ_JuActivityInfo中网点 表示 文章类型 如：Instructor
        /// </summary>
       public string ForeignkeyId { get; set; }
       private int isShowInList = 1;
       /// <summary>
       /// 是否显示在列表
       /// </summary>
        public int IsShowInList
        {
            get { return isShowInList; }
            set { isShowInList = value; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 编辑时是否只读
        /// </summary>
        public int IsReadOnly{ get; set; }
        /// <summary>
        /// 选项的值
        /// </summary>
        public string Options { get; set; }
        /// <summary>
        /// 是否多行
        /// 0 单行
        /// 1 多行
        /// </summary>
        public int IsMultiline { get; set; }
        
    }
}
