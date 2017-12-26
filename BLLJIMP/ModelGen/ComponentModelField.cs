using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 模板字段
    /// </summary>
    [Serializable]
    public partial class ComponentModelField : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 组件模板Key
        /// </summary>
        public string ComponentModelKey { get; set; }
        /// <summary>
        /// 组件模板字段Key
        /// </summary>
        public string ComponentField { get; set; }
        /// <summary>
        /// 组件模板字段名称
        /// </summary>
        public string ComponentFieldName { get; set; }
        /// <summary>
        /// 组件模板字段类型 0输入 1选择 2图片 3颜色 4数组 5对象 6广告 7导航 8页面 9导航数组 10商品数组
        /// </summary>
        public int ComponentFieldType { get; set; }
        /// <summary>
        /// 组件模板字段数据来源 0固定 1数据库查询(暂无)
        /// </summary>
        public int ComponentFieldDataType { get; set; }
        /// <summary>
        /// 组件模板字段数据
        /// 当 ComponentFieldType 为1 
        /// DataType为0时 左边显示|1@右边显示|2@不显示|3     @分隔option    |分隔textvalue
        /// DataType为1时 数据库查询语句(暂无)
        /// </summary>
        public string ComponentFieldDataValue { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int ComponentFieldSort { get; set; }
    }
}