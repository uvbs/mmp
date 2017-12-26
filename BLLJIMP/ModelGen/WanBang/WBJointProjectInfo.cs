using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 对接成果表
    /// </summary>
    public class WBJointProjectInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 基地名称
        /// </summary>
        public string BaseName { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnails { get; set; }
        /// <summary>
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
