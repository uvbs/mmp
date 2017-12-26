using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///  页面模板
    /// </summary>
    [Serializable]
    public partial class ComponentTemplate : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbnailsPath { get; set; }
        /// <summary>
        /// 模板设置
        /// </summary>
        public string Config { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 原页面
        /// </summary>
        public int ComponentId { get; set; }
        /// <summary>
        /// 原组件库ID
        /// </summary>
        public int ComponentModelId { get; set; }
        /// <summary>
        /// 原站点
        /// </summary>
        public string FromWebsite { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string InsertUserID { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 排序 数字大的排前面
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CateId { get; set; }
    }
}