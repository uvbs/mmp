using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    public partial class NotifyAplipay : System.Web.UI.Page
    {

        /// <summary>
        ///支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> parametersAll = bllOrder.GetRequestParameter();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(parametersAll["notify_data"]);
                xmlDoc.Save(string.Format("C:\\Alipay\\mallnotify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                //商户订单号
                string outTradeNo = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                //支付宝交易号
                string tradeNo = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                //交易状态
                string tradeStatus = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                var orderInfo = bllOrder.GetOrderPay(outTradeNo);

                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                if (parametersAll.Count > 0)//判断是否有带返回参数
                {
                    Payment.Alipay.Notify aliNotify = new Payment.Alipay.Notify();
                    PayConfig payConfig = bllPay.GetPayConfig();
                    bool verifyResult = aliNotify.VerifyNotifyMall(parametersAll, Request.Form["sign"], payConfig.Partner, payConfig.PartnerKey);
                    if (verifyResult)//验证成功
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码

                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                        //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
                        //sPara = aliNotify.Decrypt(sPara);

                        //XML解析notify_data数据
                        if (tradeStatus == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在两种情况下出现
                            //1、开通了普通即时到账，买家付款成功后。
                            //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

                            if (orderInfo.Status.Equals(0))//只有未付款状态
                            {
                                orderInfo.Status = 1;
                                if (!bllOrder.Update(orderInfo, tran))
                                {
                                    tran.Rollback();
                                    Response.Write("fail");
                                }
                                UserInfo userInfo = bllUser.GetUserInfo(orderInfo.UserId);
                                switch (orderInfo.Type)
                                {
                                    case "1":
                                        if (bllUser.Update(userInfo, string.Format(" Account=isnull(Account,0)+{0} ", orderInfo.Total_Fee), string.Format(" AutoID={0} ", userInfo.AutoID), tran) <= 0)
                                        {
                                            tran.Rollback();
                                            Response.Write("fail");
                                            return;
                                        }
                                        break;
                                    case "2":
                                        break;
                                    case "3":
                                        if (bllUser.Update(userInfo, string.Format(" CreditAcount=isnull(CreditAcount,0)+{0} ", orderInfo.Total_Fee), string.Format(" AutoID={0} ", userInfo.AutoID), tran) <= 0)
                                        {
                                            tran.Rollback();
                                            Response.Write("fail");
                                            return;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                tran.Commit();
                                Response.Write("success"); 
                            }
                            else
                            {
                                Response.Write("success");  //请不要修改或删除
                            }
                        }
                        else if (tradeStatus == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。


                            orderInfo.Status = 1;
                            if (!bllOrder.Update(orderInfo, tran))
                            {
                                tran.Rollback();
                                Response.Write("fail");
                            }
                            UserInfo userInfo = bllUser.GetUserInfo(orderInfo.UserId);
                            switch (orderInfo.Type)
                            {
                                case "1":
                                    if (bllUser.Update(userInfo, string.Format(" Account=isnull(Account,0)+{0} ", orderInfo.Total_Fee), string.Format(" AutoID={0} ", userInfo.AutoID), tran) <= 0)
                                    {
                                        tran.Rollback();
                                        Response.Write("fail");
                                        return;
                                    }
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    if (bllUser.Update(userInfo, string.Format(" CreditAcount=isnull(CreditAcount,0)+{0} ", orderInfo.Total_Fee), string.Format(" AutoID={0} ", userInfo.AutoID), tran) <= 0)
                                    {
                                        tran.Rollback();
                                        Response.Write("fail");
                                        return;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            tran.Commit();
                            Response.Write("success"); 
                        }
                        else
                        {
                            Response.Write(tradeStatus);
                        }
                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else//验证失败
                    {
                        Response.Write("fail");
                    }
                }
                else
                {
                    Response.Write("无通知参数");
                }
            }
            catch (Exception)
            {
                Response.Write("fail");
            }







        }
    }
}