using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    ///hrlove
    /// </summary>
    public class HrLove
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 星座
        /// </summary>
        public string starsign { get; set; }
        /// <summary>
        /// 捐献金额
        /// </summary>
        public double donateamount { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
    }

    /// <summary>
    ///hrlove返回结果模型
    /// </summary>
    public class HrLoveApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<HrLove> list { get; set; }
    }

}
