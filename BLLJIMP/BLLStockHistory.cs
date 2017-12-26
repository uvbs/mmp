using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCJson;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 股票历史 逻辑层
    /// </summary>
    public class BLLStockHistory : BLL
    {

        public const string url = "https://ali-stock.showapi.com/stockIndex";


        public string GetAliStockInfo(string aliAppKey, string aliAppSecret)
        {
            String result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(aliAppKey) && !string.IsNullOrEmpty(aliAppSecret))
                {
                    AliStock.AliStock resp = new AliStock.AliStock(aliAppKey, aliAppSecret, "http://ali-stock.showapi.com/stockIndex");
                    result = resp.doGet();

                    if (!string.IsNullOrEmpty(result))
                    {
                        ResponseModel respStockModel = JsonConvert.DeserializeObject<ResponseModel>(result);
                        if (respStockModel.showapi_res_code == 0)
                        {
                            for (int i = 0; i < respStockModel.showapi_res_body.indexList.Count; i++)
                            {
                                if (string.IsNullOrWhiteSpace(respStockModel.showapi_res_body.indexList[i].nowPrice) 
                                    || respStockModel.showapi_res_body.indexList[i].nowPrice == "0") continue;

                                string currDateMinute = DateTime.Parse(respStockModel.showapi_res_body.indexList[i].time).ToString("yyyy-MM-dd HH:mm");
                                StockHistory stockModel = Get<StockHistory>(string.Format(" Code='{1}' and DateMinute='{0}' ", currDateMinute, respStockModel.showapi_res_body.indexList[i].code));
                                if (stockModel != null)
                                {
                                    stockModel.Code = respStockModel.showapi_res_body.indexList[i].code;
                                    stockModel.MaxPrice = respStockModel.showapi_res_body.indexList[i].maxPrice;
                                    stockModel.MinPrice = respStockModel.showapi_res_body.indexList[i].minPrice;
                                    stockModel.Name = respStockModel.showapi_res_body.indexList[i].name;
                                    stockModel.NowPrice = respStockModel.showapi_res_body.indexList[i].nowPrice;
                                    stockModel.Time = respStockModel.showapi_res_body.indexList[i].time;
                                    stockModel.TradeAmount = respStockModel.showapi_res_body.indexList[i].tradeAmount;
                                    stockModel.TradeNum = respStockModel.showapi_res_body.indexList[i].tradeNum;
                                    stockModel.YestodayClosePrice = respStockModel.showapi_res_body.indexList[i].yestodayClosePrice;
                                    stockModel.TodayOpenPrice = respStockModel.showapi_res_body.indexList[i].todayOpenPrice;
                                    stockModel.DiffMoney = (Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].nowPrice) - Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].yestodayClosePrice)).ToString();
                                    stockModel.DiffRate = ((Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].nowPrice) - Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].yestodayClosePrice)) / Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].yestodayClosePrice) * 100).ToString();
                                    stockModel.DateMinute = currDateMinute;
                                    stockModel.LastUpdateDate = DateTime.Now;
                                    Update(stockModel);
                                }
                                else
                                {
                                    stockModel = new StockHistory();
                                    stockModel.Code = respStockModel.showapi_res_body.indexList[i].code;
                                    stockModel.MaxPrice = respStockModel.showapi_res_body.indexList[i].maxPrice;
                                    stockModel.MinPrice = respStockModel.showapi_res_body.indexList[i].minPrice;
                                    stockModel.Name = respStockModel.showapi_res_body.indexList[i].name;
                                    stockModel.NowPrice = respStockModel.showapi_res_body.indexList[i].nowPrice;
                                    stockModel.Time = respStockModel.showapi_res_body.indexList[i].time;
                                    stockModel.TradeAmount = respStockModel.showapi_res_body.indexList[i].tradeAmount;
                                    stockModel.TradeNum = respStockModel.showapi_res_body.indexList[i].tradeNum;
                                    stockModel.YestodayClosePrice = respStockModel.showapi_res_body.indexList[i].yestodayClosePrice;
                                    stockModel.TodayOpenPrice = respStockModel.showapi_res_body.indexList[i].todayOpenPrice;
                                    stockModel.DiffMoney = (Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].nowPrice) - Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].yestodayClosePrice)).ToString();
                                    stockModel.DiffRate = ((Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].nowPrice) - Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].yestodayClosePrice)) / Convert.ToDecimal(respStockModel.showapi_res_body.indexList[i].yestodayClosePrice) * 100).ToString();
                                    stockModel.DateMinute = currDateMinute;
                                    stockModel.LastUpdateDate = DateTime.Now;
                                    Add(stockModel);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("{0}：{1}", respStockModel.showapi_res_error, result));
                        }
                    }
                    else
                    {
                        throw new Exception("获取阿里上证指数数据出错");
                    }
                }
                else
                {
                    throw new Exception("站点还未配置阿里股票账号");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("接口返回信息：{0}；错误信息：{1}；StackTrace：{2}", result, ex.Message, ex.StackTrace));
            }
            return result;
        }



        public List<StockHistory> GetStockHistoryList(string day,string code)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(code))
            {
                sbWhere.AppendFormat(" AND Code='{0}' ",code);
            }

            if (string.IsNullOrEmpty(day))
            {
                sbWhere.AppendFormat(" AND DateDiff(dd,[Time],getdate())=0 ");
            }
            else
            {
                sbWhere.AppendFormat(" AND DateDiff(dd,[Time],getdate())<={0} ",int.Parse(day));
            }

            sbWhere.AppendFormat(" order by [Time] ASC ");


            return GetList<StockHistory>(sbWhere.ToString());
        }



    }
}

public class ResponseModel
{

    public int showapi_res_code { get; set; }

    public string showapi_res_error { get; set; }

    public bodyModel showapi_res_body { get; set; }


}

public class bodyModel
{
    public int ret_code { get; set; }

    public List<IndexList> indexList { get; set; }
}

public class IndexList
{
    /// <summary>
    /// 指数名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 昨日收盘价
    /// </summary>
    public string yestodayClosePrice { get; set; }

    /// <summary>
    /// 刷新时间
    /// </summary>
    public string time { get; set; }

    /// <summary>
    /// 成交量
    /// </summary>
    public string tradeNum { get; set; }

    /// <summary>
    /// 成交金额
    /// </summary>
    public string tradeAmount { get; set; }

    /// <summary>
    /// 最高价
    /// </summary>
    public string maxPrice { get; set; }

    /// <summary>
    /// 最低价
    /// </summary>
    public string minPrice { get; set; }

    /// <summary>
    /// 指数编码
    /// </summary>
    public string code { get; set; }

    /// <summary>
    /// 当前价
    /// </summary>
    public string nowPrice { get; set; }

    /// <summary>
    /// 今日开盘价
    /// </summary>
    public string todayOpenPrice { get; set; }

}
