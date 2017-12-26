using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class WBHBannaImg : ZCBLLEngine.ModelTable
    {
        public WBHBannaImg() { }


        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string BannaName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string BannaImg { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string BannaUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

    }
}
