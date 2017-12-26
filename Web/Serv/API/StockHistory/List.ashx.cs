using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.StockHistory
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLStockHistory bllStockHistory = new BLLStockHistory();
        public void ProcessRequest(HttpContext context)
        {
            string dayNum=context.Request["day"];
            string code=context.Request["code"];

            List<BLLJIMP.Model.StockHistory> stockList = bllStockHistory.GetStockHistoryList(dayNum, code);

            List<RespStockModel> returnList = new List<RespStockModel>();

            foreach (var item in stockList)
            {
                RespStockModel temp = new RespStockModel();
                temp.code = item.Code;
                temp.diff_money = item.DiffMoney;
                temp.diff_rate = item.DiffRate;
                temp.max_price = item.MaxPrice;
                temp.min_price = item.MinPrice;
                temp.name = item.Name;
                temp.now_price = item.NowPrice;
                temp.time = item.Time;
                temp.today_open_price = item.TodayOpenPrice;
                temp.trade_amount = item.TradeAmount;
                temp.trade_num = item.TradeNum;
                temp.yestoday_close_price = item.YestodayClosePrice;
                temp.date_minute = item.DateMinute;
                returnList.Add(temp);
            }

            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.result = returnList;

            bllStockHistory.ContextResponse(context,apiResp);

        }
    }
}



public class RespStockModel
{
    /// <summary>
    /// 昨日收盘价
    /// </summary>
    public string yestoday_close_price { get; set; }

    /// <summary>
    /// 指数编码
    /// </summary>
    public string code { get; set; }

    /// <summary>
    /// 指数名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 今日开盘价
    /// </summary>
    public string today_open_price { get; set; }

    /// <summary>
    ///当前价
    /// </summary>
    public string now_price { get; set; }

    /// <summary>
    /// 最高价
    /// </summary>
    public string max_price { get; set; }

    /// <summary>
    /// 最低价
    /// </summary>
    public string min_price { get; set; }

    /// <summary>
    /// 成交量
    /// </summary>
    public string trade_num { get; set; }

    /// <summary>
    /// 成交金额
    /// </summary>
    public string trade_amount { get; set; }

    /// <summary>
    /// 刷新时间
    /// </summary>
    public string time { get; set; }

    /// <summary>
    /// 涨跌幅度
    /// </summary>
    public string diff_rate { get; set; }

    /// <summary>
    /// 涨跌金额
    /// </summary>
    public string diff_money { get; set; }

    /// <summary>
    /// 间隔
    /// </summary>
    public string date_minute { get; set; }
}