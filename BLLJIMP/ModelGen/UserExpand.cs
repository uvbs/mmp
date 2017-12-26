using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户扩展信息表
    /// </summary>
    public class UserExpand : ZCBLLEngine.ModelTable
    {
        public int AutoId { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 根据不同类型定义不同扩展数据，如工作经历等
        /// </summary>
        public string DataType { get; set; }

        public string DataValue { get; set; }

        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 站点所有着
        /// </summary>
        public string WebsiteOwner { get; set; }

        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public string Ex3 { get; set; }
        public string Ex4 { get; set; }
        public string Ex5 { get; set; }
        public string Ex6 { get; set; }
        public string Ex7 { get; set; }
        public string Ex8 { get; set; }
        public string Ex9 { get; set; }
        public string Ex10 { get; set;}

    }
}
