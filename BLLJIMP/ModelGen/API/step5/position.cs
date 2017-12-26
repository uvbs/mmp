using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    ///职位
    /// </summary>
    public class Position
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string imgurl { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string positionname { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 学历要求
        /// </summary>
        public string education { get; set; }
        /// <summary>
        /// 工作年限要求
        /// </summary>
        public string workyear { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public List<string> tags { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int pv { get; set; }


    }

    /// <summary>
    ///返回结果模型
    /// </summary>
    public class PositionApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<Position> list { get; set; }
    }

}
