using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 商品特征值表
    /// </summary>
    public class ProductPropertyValue : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 特征值ID
        /// </summary>
        public int PropValueId { get; set; }
        /// <summary>
        /// 特征量ID
        /// </summary>
        public int PropID { get; set; }
        /// <summary>
        /// 特征值名称 如 S 蓝色等
        /// </summary>
        public string PropValue { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 对应代码 无用
        /// </summary>
        public string PropCode { get; set; }
        /// <summary>
        /// 说明 无用
        /// </summary>
        public string PropDescription { get; set; }

    }
}
