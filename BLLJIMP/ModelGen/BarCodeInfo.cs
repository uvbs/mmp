using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 证书
    /// </summary>
    [Serializable]
    public class BarCodeInfo : ZCBLLEngine.ModelTable
    {
        public BarCodeInfo() { }

        /// <summary>
        /// 自动编号
        /// </summary>
        public int? AutoId { get; set; }

        /// <summary>
        /// 条形码名称 姓名
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 证书编号
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 型号 身份证号
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 商户
        /// </summary>
        public string Agency { get; set; }


        /// <summary>
        /// 插入时间
        /// </summary>
        public string InsetData { get; set; }

        /// <summary>
        /// 查询第一次
        /// </summary>
        public string TimeOne { get; set; }

        /// <summary>
        /// 第二次
        /// </summary>
        public string TimeTwo { get; set; }

        /// <summary>
        /// 第三次
        /// </summary>
        public string TimeThree { get; set; }

        /// <summary>
        /// 站点属于谁
        /// </summary>
        public string websiteOwner { get; set; }

        ///// <summary>
        ///// 插入时间
        ///// </summary>
        //public string InsetDatastr { get { return this.InsetData; } }

        /// <summary>
        /// 证书图片
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
