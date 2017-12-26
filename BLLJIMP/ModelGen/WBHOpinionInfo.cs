using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class WBHOpinionInfo : ZCBLLEngine.ModelTable
    {
        public WBHOpinionInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string OContext { get; set; }

        /// <summary>
        ///提交类型
        /// </summary>
        public string Otype { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime InsertDate { get; set; }
    }
}
