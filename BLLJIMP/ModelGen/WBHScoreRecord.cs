using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///积分记录
    /// </summary>
    public class WBHScoreRecord : ZCBLLEngine.ModelTable
    {
        public WBHScoreRecord() { }

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string NameStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ScoreNum { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nums { get; set; }
        /// <summary>
        /// 1 支出 2 收入
        /// </summary>
        public string RecordType { get; set; }

    }
}
