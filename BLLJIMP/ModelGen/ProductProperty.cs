using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    
    /// <summary>
    /// 商品特征量表
    /// </summary>
    public class ProductProperty : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 特征量ID
        /// </summary>
        public int PropID { get; set; }
        /// <summary>
        /// 商品分类ID
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 特征量名称 颜色 尺码等
        /// </summary>
        public string PropName { get; set; }
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




    }
}
