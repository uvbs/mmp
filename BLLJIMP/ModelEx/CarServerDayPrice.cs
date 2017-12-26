using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 服务每天价格实体
    /// </summary>
    public class CarServerDayPrice
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 价格：服务工时*（店铺默认工时单价/指定车型工时单价）* 工时折扣率 + 配件价格（服务配件默认配件总和/制定商户配件总和）* 配件折扣率
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 工时数
        /// </summary>
        public int Workhours { get; set; }

        /// <summary>
        /// 工时价
        /// </summary>
        public double WorkhoursPrice { get; set; }

        /// <summary>
        /// 工时折扣率
        /// </summary>
        public double WorkhoursRate { get; set; }

        /// <summary>
        /// 配件折扣率
        /// </summary>
        public double PartsRate { get; set; }

        /// <summary>
        /// 配件总价
        /// </summary>
        public double PartsTotalPrice { get; set; }

        /// <summary>
        /// 星期：["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"]
        /// </summary>
        public int Week
        {
            get
            {
                DateTime dt = DateTime.Parse(Date);
                return Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            }
        }

        /// <summary>
        /// 配件详情
        /// </summary>
        public List<CarServerDayPartsDetail> PartsDetail { get; set; }
    }
}
