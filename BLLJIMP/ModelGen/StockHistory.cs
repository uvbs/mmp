using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class StockHistory:ModelTable
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        public long AutoId { get; set; }

        /// <summary>
        /// 指数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public string YestodayClosePrice { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public string TradeNum { get; set; }

        /// <summary>
        /// 成交金额
        /// </summary>
        public string TradeAmount { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public string MaxPrice { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public string MinPrice { get; set; }

        /// <summary>
        /// 指数编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 当前价
        /// </summary>
        public string NowPrice { get; set; }

        /// <summary>
        /// 今日开盘价
        /// </summary>
        public string TodayOpenPrice { get; set; }

        /// <summary>
        /// 涨跌金额
        /// </summary>
        public string DiffMoney { get; set; }

        /// <summary>
        /// 涨跌幅度
        /// </summary>
        public string DiffRate { get; set; }

        /// <summary>
        /// 最好修改时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 区分  分
        /// </summary>
        public string DateMinute { get; set; }
    }
}
