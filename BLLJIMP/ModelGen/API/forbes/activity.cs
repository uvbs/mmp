using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// api 活动模型
    /// </summary>
    public class Activity
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
        /// 开始时间
        /// </summary>
        public string starttimestr { get; set; }
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
        /// 活动报名所需要的积分
        /// </summary>
        public int score { get; set; }

        public List<string> tags { get; set; }

    }

    /// <summary>
    ///活动api返回结果模型
    /// </summary>
    public class ActivityApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<Activity> list { get; set; }
    }

}
