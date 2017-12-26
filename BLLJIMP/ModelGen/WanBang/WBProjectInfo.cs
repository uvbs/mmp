using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class WBProjectInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 用户名 对应WBCompanyInfo UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目缩略图
        /// </summary>
        public string Thumbnails { get; set; }
        /// <summary>
        /// 项目位置区县
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 项目分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 项目物流 0代表基地负责配送 1代表企业负责配送
        /// </summary>
        public int Logistics { get; set; }
        /// <summary>
        /// 项目周期 0表示 临时(1个月以内) 1表示 短期(1-3个月) 2表示 中期(3-6个月) 3表示 长期(6-12个月)
        /// </summary>
        public int ProjectCycle { get; set; }
        /// <summary>
        /// 项目状态 0代表审核中 1代表征集中 2代表已结束
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 项目工期要求
        /// </summary>
        public string TimeRequirement{get;set;}
        /// <summary>
        /// 项目介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 页面访问量
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        public string InsertDateStr { get {
                return InsertDate.ToShortDateString();
            } }

    }
}
