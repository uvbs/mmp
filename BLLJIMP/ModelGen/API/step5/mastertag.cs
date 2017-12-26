using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 标签模型
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 标签编号
        /// </summary>
        public int tagid { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string tagname { get; set; }


    }
    /// <summary>
    /// 标签管理
    /// </summary>
    public class TagApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<Tag> list { get; set; }
    }


}
