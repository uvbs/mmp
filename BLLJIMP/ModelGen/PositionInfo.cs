using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 五步会职位表
    /// </summary>
    public class PositionInfo : ZCBLLEngine.ModelTable
    {
        public PositionInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// logo图片
        /// </summary>
        public string IocnImg { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 薪资范围
        /// </summary>
        public string SalaryRange { get; set; }

        /// <summary>
        /// 公司范围
        /// </summary>
        public string EnterpriseScale { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 招聘数据
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 企业描述
        /// </summary>
        public string Personal { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int PersonNum { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int Pv { get; set; }

        /// <summary>
        /// 工作年限
        /// </summary>
        public string WorkYear { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// 行业 id
        /// </summary>
        public string TradeIds { get; set; }
        /// <summary>
        /// 专业  id
        /// </summary>
        public string ProfessionalIds { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }


        public List<ArticleCategory> Ctype { get; set; }


    }
}
