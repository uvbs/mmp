using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 运费模板
    /// </summary>
    public class FreightTemplate : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 模板编号
        /// </summary>
        public int TemplateId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime? LastModifyDate { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 启用/禁用 1启用 1禁用 
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 计算方式
        /// count  按数量
        /// weight 按重量
        /// volume 按体积
        /// </summary>
        public string CalcType { get; set; }



        /// <summary>
        /// 0 不开启   
        /// 1 满多少件（重量）  
        /// 2 满多少金额
        /// </summary>
        public int FreightFreeLimitType { get; set; }

        /// <summary>
        /// 包邮类型值
        /// </summary>
        public decimal FreightFreeLimitValue { get; set; }

    }
}
