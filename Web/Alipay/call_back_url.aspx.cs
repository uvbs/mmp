using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Payment.Alipay;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Alipay
{
    /// <summary>
    /// 投票通知 支付宝异步通知 功能：页面跳转同步通知页面
    /// 版本：3.3
    /// 日期：2012-07-10
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// ///////////////////////页面功能说明///////////////////////
    /// 该页面可在本机电脑测试
    /// 可放入HTML等美化页面的代码、商户业务逻辑程序代码
    /// 该页面可以使用ASP.NET开发工具调试，也可以使用写文本函数LogResult进行调试
    /// </summary>
    public partial class call_back_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
            Dictionary<string, string> dicAll = GetRequestGet();
            if (dicAll.Count > 0)//判断是否有带返回参数
            {
                Notify notify = new Notify();
                bool verifyResult = notify.VerifyReturn(dicAll, Request.QueryString["sign"]);
                if (verifyResult)//验证成功
                {
                    try
                    {

                   
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码



                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //商户订单号
                    string outTrade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号
                    string tradeNo = Request.QueryString["trade_no"];

                    //交易状态
                    string result = Request.QueryString["result"];
                    var orderPay = bllOrder.GetOrderPay(outTrade_no);
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
                                    Hmsg.InnerHtml="更新订单失败";
                                }
                                if (bllUser.Update(userInfo, string.Format(" AvailableVoteCount={0}", userInfo.AvailableVoteCount), string.Format(" AutoID={0}", userInfo.AutoID), tran) < 1)
                                {
                                    tran.Rollback();
                                    Hmsg.InnerHtml = "更新用户信息失败";

                                }
                                tran.Commit();
                                Hmsg.InnerHtml = "交易成功!";


                            }
                            catch (Exception ex)
                            {
                                Log(DateTime.Now.ToString()+ex.ToString());
                                tran.Rollback();
                                Hmsg.InnerHtml = ex.ToString();
                            }


                        }
                        else
                        {

                        }
                    }
                    if (orderPay.Status.Equals(1))
                    {
                        Hmsg.InnerHtml = "交易成功!";
                    }



                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    catch (Exception ex)
                    {
                        Log(DateTime.Now.ToString() + ex.ToString());
                        Hmsg.InnerHtml = ex.ToString();

                    }
                }
                else//验证失败
                {
                    
                    Hmsg.InnerHtml = "验证失败";
                }
            }
            else
            {
               
                 Hmsg.InnerHtml = "无返回参数";
               
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        private void Log(string msg)
        {

            using (System.IO.StreamWriter sr = new System.IO.StreamWriter("C:\\Alipay\\callbacklog.txt", true))
            {
                sr.WriteLine(msg);

            }


        }
    }

}