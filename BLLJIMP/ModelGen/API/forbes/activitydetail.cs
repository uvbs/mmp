using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// api 活动模型
    /// </summary>
    public class ActivityDetail
    {
        /// <summary>
        /// 活动id编号
        /// </summary>
        public int activityid { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string activityname { get; set; }
        /// <summary>
        /// 活动图片
        /// </summary>
        public string activityimage { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 阅读量
        /// </summary>
        public int pv { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
        public int signcount { get; set; }
        /// <summary>
        /// 活动状态 0代表进行中 1代表已结束 2代表已满员
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 分类名称 也就是标签名称
        /// </summary>
        public string categoryname { get; set; }
        /// <summary>
        /// 活动详情
        /// </summary>
        public string activitycontent { get; set; }
        /// <summary>
        /// 活动报名所需要的积分
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 报名字段集合
        /// </summary>
        public List<SignField> signfield { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int commentcount { get; set; }

    }

    /// <summary>
    ///报名字段模型
    /// </summary>
    public class SignField
    {
        /// <summary>
        /// 显示名称 如姓名
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 如 K1
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public int isnull { get; set; }

    }

}
