using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    ///
    /// </summary>
    public class PositionDetail
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
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 企业规模
        /// </summary>
        public string enterprisescale { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int pv { get; set; }
        /// <summary>
        /// 行业标签
        /// </summary>
        public List<string> tradetags { get; set; }
        /// <summary>
        /// 专业标签
        /// </summary>
        public List<string> professionaltags { get; set; }


    }



}
