using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 系统消息模型
    /// </summary>
    public class SystemNoticeModel
    {
        /// <summary>
        ///id编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public double time { get; set; }
    }
    /// <summary>
    /// 系统消息api模型
    /// </summary>
    public class SystemNoticeApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<SystemNoticeModel> list { get; set; }
    }


}
