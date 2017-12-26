using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    public partial class PayUpgradeNotify : System.Web.UI.Page
    {
        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        BLLUser bllUser = new BLLUser();
        BLLDistribution bll = new BLLDistribution();
        /// <summary>
        /// 成功xml
        /// </summary>
        private string successXml = "<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>";
        /// <summary>
        /// 失败xml
        /// </summary>
        private string failXml = "<xml><return_code><![CDATA[FAIL]]></return_code></xml>";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //Tolog("进入支付回调");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Request.InputStream);
                xmlDoc.Save(string.Format("C:\\WXPay\\PayUpgradeNotify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志

                //全部参数
                Dictionary<string, string> parametersAll = new Dictionary<string, string>();
                foreach (XmlElement item in xmlDoc.DocumentElement.ChildNodes)
                {
                    string key = item.Name;
                    string value = item.InnerText;
                    if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(value)))
                    {
                        parametersAll.Add(key, value);
                    }
                }
                parametersAll = (from entry in parametersAll
                                 orderby entry.Key ascending
                                 select entry).ToDictionary(pair => pair.Key, pair => pair.Value);//全部参数排序
                PayConfig payConfig = bllPay.GetPayConfig();
                if (!bllPay.VerifySignatureWx(parametersAll, payConfig.WXPartnerKey))//验证签名
                {
                    Tolog("验证签名出错");
                    Response.Write(failXml);
                    return;
                }
                OrderPay orderPay = bllOrder.GetOrderPay(parametersAll["out_trade_no"]);
                if (orderPay == null)
                {
                    Tolog("订单未找到");
                    Response.Write(failXml);
                    return;
                }
                if (orderPay.Status.Equals(1))
                {
                    Tolog("已支付");
                    Response.Write(successXml);
                    return;
                }
                BLLJIMP.Model.API.User.PayUpgrade payUpgrade = JsonConvert.DeserializeObject<BLLJIMP.Model.API.User.PayUpgrade>(orderPay.Ex1);
                //更新订单状态
                if (parametersAll["return_code"].Equals("SUCCESS") && parametersAll["result_code"].Equals("SUCCESS"))//交易成功
                {
                    string msg = "";
                    orderPay.Trade_No = parametersAll["transaction_id"];
                    UserLevelConfig levelConfig = bll.QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", payUpgrade.toLevel.ToString());
                    UserInfo payUser = bllUser.GetUserInfo(orderPay.UserId, orderPay.WebsiteOwner);
                    if (!string.IsNullOrEmpty(levelConfig.CouponId))
                    {
                        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
                        bllCardCoupon.SendCardCouponsByCurrUserInfo(payUser, levelConfig.CouponId);
                    }
                    //支付升级分佣
                    if (!bll.PayUpgradeTransfers(orderPay, payUpgrade, parametersAll["openid"], parametersAll["transaction_id"], out msg))
                    {
                        Tolog(msg);
                        Response.Write(failXml);
                        return;
                    }

                    Response.Write(successXml);
                    return;
                }
                Tolog("返回信息有误");
                Response.Write(failXml);
            }
            catch (Exception ex)
            {
                Tolog("出错了：" + ex.Message);
                Response.Write(failXml);

            } 
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\PayUpgradeLog.txt", true, Encoding.GetEncoding("GB2312")))
            {
                sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
            }
        }
    }
}