using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 字典表
    /// </summary>
    public class DictionaryInfo : ZCBLLEngine.ModelTable
    {
        public DictionaryInfo() { }

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        ///   key
        /// </summary>
        public string KeyStr { get; set; }

        /// <summary>
        ///  值
        /// </summary>
        public string ValueStr { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        /// 票数
        /// </summary>
        public int VoteNums { get; set; }
    }
}
