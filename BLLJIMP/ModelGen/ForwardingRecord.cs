using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 五步会注册转发记录表
    /// </summary>
    public class ForwardingRecord : ZCBLLEngine.ModelTable
    {
        public ForwardingRecord() { }

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 注册人用户名
        /// </summary>
        public string RUserID { get; set; }

        /// <summary>
        /// 注册人姓名
        /// </summary>
        public string RUserName { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RdateTime { get; set; }

        /// <summary>
        /// 邀请人用户名
        /// </summary>
        public string FUserID { get; set; }

        /// <summary>
        ///  邀请人姓名
        /// </summary>
        public string FuserName { get; set; }

        /// <summary>
        /// 记录类别
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }


    }
}
