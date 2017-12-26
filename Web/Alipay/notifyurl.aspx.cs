using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Payment.Alipay;
using System.Xml;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Alipay
{
    /// <summary>
    /// 投票 支付宝异步通知
    /// </summary>
    public partial class notifyurl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
            Dictionary<string, string> dicAll = bllOrder.GetRequestParameter();
            if (dicAll.Count > 0)//判断是否有带返回参数
            {
                Notify notify = new Notify();
                bool verifyResult = notify.VerifyNotify(dicAll, dicAll["sign"]);
                if (verifyResult)//验证成功
                {

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
                    //sPara = aliNotify.Decrypt(sPara);

                    //XML解析notify_data数据
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(dicAll["notify_data"]);
                        xmlDoc.Save(string.Format("C:\\Alipay\\notify{0}.xml",DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        //商户订单号
                        string outTradeNo = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                        //支付宝交易号
                        string tradeNo = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                        //交易状态
                        string tradeStatus = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                        var orderPay = bllOrder.GetOrderPay(outTradeNo);
                        if (tradeStatus == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在两种情况下出现
                            //1、开通了普通即时到账，买家付款成功后。
                            //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

                            if (orderPay.Status.Equals(0))//只有未付款状态
                            {
                                if (orderPay.Type.Equals("1"))//投票充值
                                {
                                    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                                    try
                                    {
                                        UserInfo userInfo = bllUser.GetUserInfo(orderPay.UserId);
                                        if (userInfo.AvailableVoteCount == null)
                                        {
                                            userInfo.AvailableVoteCount = 0;
                                        }
                                        userInfo.AvailableVoteCount += int.Parse(orderPay.Ex1);
                                        orderPay.Status = 1;
                                        orderPay.Trade_No = tradeNo;
                                        if (!bllOrder.Update(orderPay, tran))
                                        {
                                            tran.Rollback();
                                            Response.Write("fail");
                                        }
                                        if (bllUser.Update(userInfo,string.Format(" AvailableVoteCount={0}",userInfo.AvailableVoteCount),string.Format(" AutoID={0}",userInfo.AutoID), tran)<1)
                                        {
                                            tran.Rollback();
                                            Response.Write("fail");

                                        }
                                        tran.Commit();
                                        Response.Write("success");  //请不要修改或删除




                                    }
                                    catch (Exception ex)
                                    {
                                       
                                        tran.Rollback();
                                        Response.Write("fail");
                                    }


                                }
                                else
                                {

                                }
                            }


                            Response.Write("success");  //请不要修改或删除
                        }
                        else if (tradeStatus == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
                            if (orderPay.Status.Equals(0))//只有未付款状态
                            {
                                if (orderPay.Type.Equals(1))//投票充值
                                {
                                    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                                    try
                                    {
                                        UserInfo userInfo = bllUser.GetUserInfo(orderPay.UserId);
                                        if (userInfo.AvailableVoteCount == null)
                                        {
                                            userInfo.AvailableVoteCount = 0;
                                        }
                                        userInfo.AvailableVoteCount += int.Parse(orderPay.Ex1);
                                        orderPay.Status = 1;
                                        orderPay.Trade_No = tradeNo;
                                        if (!bllOrder.Update(orderPay, tran))
                                        {
                                            tran.Rollback();
                                            Response.Write("fail");
                                        }
                                       if (bllUser.Update(userInfo,string.Format(" AvailableVoteCount={0}",userInfo.AvailableVoteCount),string.Format(" AutoID={0}",userInfo.AutoID), tran)<1)
                                        {
                                            tran.Rollback();
                                            Response.Write("fail");

                                        }
                                        tran.Commit();
                                        Response.Write("success");  //请不要修改或删除




                                    }
                                    catch (Exception Ex)
                                    {
                                       
                                        tran.Rollback();
                                        Response.Write("fail");
                                    }


                                }
                                else
                                {

                                }
                            }

                            Response.Write("success");  //请不要修改或删除
                        }
                        else
                        {
                            Response.Write(tradeStatus);
                        }

                    }
                    catch (Exception Ex)
                    {
                        Response.Write("fail");
                       
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
        ///// <summary>
        ///// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        ///// </summary>
        ///// <returns>request回来的信息组成的数组</returns>
        //public Dictionary<string, string> GetRequestPost()
        //{
        //    int i = 0;
        //    Dictionary<string, string> sArray = new Dictionary<string, string>();
        //    NameValueCollection coll;
        //    //Load Form variables into NameValueCollection variable.
        //    coll = Request.Form;

        //    // Get names of all forms into a string array.
        //    String[] requestItem = coll.AllKeys;

        //    for (i = 0; i < requestItem.Length; i++)
        //    {
        //        sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
               
        //    }

        //    return sArray;
        //}

    }
}