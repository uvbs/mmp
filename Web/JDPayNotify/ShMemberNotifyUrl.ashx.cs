using Payment.JDPay.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.JDPayNotify
{
    /// <summary>
    /// ShMemberNotifyUrl 的摘要说明
    /// </summary>
    public class ShMemberNotifyUrl : IHttpHandler, IReadOnlySessionState
    {

        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        /// <summary>
        /// 成功
        /// </summary>
        private string successStr = "ok";
        /// <summary>
        /// 失败
        /// </summary>
        private string failStr = "fail";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Tolog("京东支付通知start");
                PayConfig payConfig = bllPay.GetPayConfig();
                byte[] byts = new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(byts, 0, byts.Length);
                string req = Encoding.UTF8.GetString(byts);
                Tolog("通知参数" + req);
                //string req = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><jdpay><version>V2.0</version><merchant>110226843002</merchant><result><code>000000</code><desc>success</desc></result><encrypt>NDdmNmZkNjQwNzBiYzEzMmE5N2ExMTg3YjkxNmQ4NzkxYzQyMmFjZjM4YTM1MjZjY2JjNzM1ZDkxY2Q5ZDNmMTMzMGFjYTBkNTI0MzYzNjc3MTVhODI3OWUzZTAxMmY5ODEyOTVmNDNiNWY5MDZhZGJiYTcwYTYyOTFlYzVmYTU2Y2EyN2U1YzhkNzllMGE3ZTUyNDE4NWU4OWMxNjIwNDFkODcyYzJiZDA4ZWY4YWEyMmY5ZWUxNDk3YTg3MTI3YmU1NTMxNjc5NWJiOTlmN2ZmNGU1MTc1YWJhNjNkYjUzYWUwZDQzOTk4ZjIzMjBiYmVkNGJkMDcxOWUzOThjZjU1ODUwNzM4Y2RiNzM2Njc1N2U2ZTcxN2Q0N2ZiYTZmN2M1YzBiYzRmMjc4ZThiZDNhYTkxZTExNzBiYjg2ZDNjNmQ3OWUyZTBlYjUzZWNlZjFjODQ2MzdiN2E5MTQxN2Y3NmRhZmNkNDdiMzMwNjc1MGRhYjhiYzg0NTFkOWNiZTQ2NGQ0M2FmY2Q0NDcwZDU2NzQzM2UzM2UxNjg2M2ViMDYzMzU0MjQ1NDZiNGZmM2RmNDA2NWJmYjQ5MjdkZWMyMDcwYjM3ODQ5OTQzMjRmNDJlMzllODUxN2ZkYmQwYTU3MTBhOTIyMzAwM2JiZDQ0MmNhYzE5ZjZkMzE5OWY4MDdmNjJlMWZiMWM1MjkzYjA1NWIwZDk0NmMxMjk0NzgwMDliYWRhZThiNjVkZDZiZDk5Y2E5YzIxZWFkN2NkZmUxOWE2MDc3MzQyYjhiYTIzY2ZkZTBhYjliOTdiMjkzYjRjYzNmMzFmOTkwYzExZGJjNzViZDJjNGQzMzU5NWFkMmMzM2UzZGUyZGRhMjljY2Q0MjBiZTg5OTYzMGEzNDdlM2FkZDY2YzNhOTE2YTNmMjUyNmU0MDQyYTM3YjYyOTRkYzI4MThiNWNmMmZmY2I1YTNjNDdiNmUyY2U0NTJiNDE2N2ZmOWIwY2MwZmYxOTQwZTliZGViNTQ0ZjMzNjYxYzZiNTBhNDAyZDM3NmI2YWQ4YjA0M2NmZDNhMzViZjYzZTg4OWUyYmMwNmExNmEwZmVhMzdiYTQzNDVkMWUyMWE5OGM3MDhjNTA4ODBjZTYyYTM2NmQyMjM4MDQ0OTY1YWVjOTUxNTVkYmJlYzYzYzgwZTFkNGRiZTJmYzA4Yzc1MWE5ZTdiNTFjYmM3MDZkMWJhOGM2Njc1OWQ2MWViOGE2YjM0YWZkYTIwMTMxZDE3N2IzZWE5NDljYTY4N2IxZGY3MGQxNjhmYTBjN2U0MDhhNTFkZDhjYjUwYmVhYTA5NDdmMmQ1YzM2YjViMzYxNWU5YjYzNDcxNWJmYzNmYWQ0NjBjYzI2NTNiNTM1ZWNhNjUzMDU0NGVkM2ExZmE3NGE0Yzg2OThkMmY0ZTIyM2U4MjMwZDYyNDYzZWY2NDFhOTlhZmU3NmRiZDgyMmE0NWQ3ODA1ZTNmMTZjNTUxMWVhZTI4ZTZiMmQwN2Y1N2EwZDVjMjZjOWNjMzBlZDAzYmE0YWM2M2UzMmJmMTMwNjk1ZmUzN2NhMjlhMWZjN2FlYjgwY2JlYWM2ZjRiZTk4OWFkZGI3N2ZhMTgyNDc2ZDA2MDIyMzllNDY5ODc5ZmE3MzUyNWI0Yzc4MDAyNWIyY2VlYzY3NTExNmVhMjk4ZGNiZGU2ZWE1ODlkOTNjMTA0M2ZkNTBiNTI0OTYzODg2OWRmOTFiNjNmZDUzZDRhM2FjZmIwNWZiZmJkMTU1NWYyNGRiNDc1ZTIzN2I5ZWIwZjcwNGI5NzRiMTlmYzE2N2MyYzE0Y2M4ZGZmYzQ4YmZkNjI0MmZiZTQwMDMzODg2OWRmOTFiNjNmZDUzZDkxZGE4ZWNhODI0ZjU2YzhkODNhMGViNzYyNDI5NmE1ZmZkODQ0YzcxNTA1NjY2NDI4MTZiYWI3YmNlZmFkZjdkOWMwMmYxYTBlNTg2MzhkMWNiYWMwZWE4NzA5YmViMmE4NzY5Mjc0ZTcwN2MyNGFiMGM3YzAzYjBiYjNkZmYyZDI4OWRjNjZjYzU4N2U2ZDBjM2RjOTZhY2M4OTFhN2JkNWNmZWNmYWViYjIxODE5OTRhMjBlYTllMmU1ZWRhNzFiNDI0MmUwNmI1YWMyNzZlNzJkNWNmMDk2M2MyODlhNzFhOTY1M2I5MzkzNDk0NjA3MTQ3NTg2YjlmNmU4ZThhMDRjYTAzMDlkYmNkYzNlOThlMjRmYWExMTRiYWY0ODcxNzY2ZjNlNDViMzljMzdlNWZlYTQzMWZjNTQwNWZlMjYzNGJmYTM2YWM4OWFhNzVmYjhhMWNiMjUwZTJlODE=</encrypt></jdpay>";
                //var jdPubKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCKE5N2xm3NIrXON8Zj19GNtLZ8xwEQ6uDIyrS3S03UhgBJMkGl4msfq4Xuxv6XUAN7oU1XhV3/xtabr9rXto4Ke3d6WwNbxwXnK5LSgsQc1BhT5NcXHXpGBdt7P8NMez5qGieOKqHGvT0qvjyYnYA29a8Z4wzNR7vAVHp36uD5RwIDAQAB";

                //req = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><jdpay><version>V2.0</version><merchant>110226843002</merchant><result><code>000000</code><desc>success</desc></result><encrypt>MDIxZjNkNjI1YjU1NzQ4MWE5N2ExMTg3YjkxNmQ4NzkxYzQyMmFjZjM4YTM1MjZjY2JjNzM1ZDkxY2Q5ZDNmMTMzMGFjYTBkNTI0MzYzNjc3MTVhODI3OWUzZTAxMmY5ODEyOTVmNDNiNWY5MDZhZGJiYTcwYTYyOTFlYzVmYTU2Y2EyN2U1YzhkNzllMGE3ZTUyNDE4NWU4OWMxNjIwNDFkODcyYzJiZDA4ZWY4YWEyMmY5ZWUxNDk3YTg3MTI3YmU1NTMxNjc5NWJiOTlmN2ZmNGU1MTc1YWJhNjNkYjUzYWUwZDQzOTk4ZjIzMjBiYmVkNGJkMDcxOWUzOThjZjU1ODUwNzM4Y2RiNzM2Njc1N2U2ZTcxN2Q0N2ZiYTZmN2M1YzBiYzRmMjc4ZThiZDNhYTkxZTExNzBiYjg2ZDNjNmQ3OWUyZTBlYjUzZWNlZjFjODQ2MzdiN2E5MTQxN2Y3NmRhZmNkNDdiMzMwNjc1MGRhYjhiYzg0NTFkOWNiY2QyMDZhOTJiYzU2YTFiOWEwMjRmNTZhMWZhNTVhNjlmYTA1ZDFlMmI0YTI4MGE1YTU0N2NlMjc3ZWMwM2QyNWE4ODdlOTA0ZGM3YTY2MDViY2I1OTI5MDBlYWU4MGU0Mzc4MmEwOWY3ZjEwZTk5MGZkOGUzYTA4MzNkNGMyZGZkNWM3MDhkOGU4N2NhMmQyZGM1MDgwYzUyOTg3OGNkMzFhZWRkMTE1NDM5ZjExYTM2OGM2OGE4MGZjZTYyMjJkNzlmODcwNGYyNDMzMTYyZThhZTBkODM2ODBjYzg5ZGRjMWY3ZGVmYzQzYjc4MDZiMDNhMTBmZTc2YjI3MThjYjQ0YjQwZDkyY2E3OGUzYmYzZmFlNjBlOTI4OGU1ODVkNjBjMWZiNDBmMjFjNzVmNDkxYmRkYWFlNzQ0YjZkYmU1ODNkOWQ4OGYwN2EyMDViOWQ5MGNjMzViMTE3MDQ4NWVlNTdlN2Y5MTRhZDM3YzFlODY1NDFiZmQyNDg1MzhlZGZiNDNiZWZmZjY3YmE3NWQ5YjI0MzE4ZDMzMDE5NTE3YWM4ZTJiMDZhZWYyM2NhNjMwODc3MDhkNTdkZWI3MWVhMmY2MzA2ZDliZjBmZGFlNzQ3ODgzODg0ZjVhOWFkODIxYWM0NGQ1ZDlmMGRlNDhkMjBiYTJjYmQ4NTlkMmU3NDMxZDExZmRjMzkxMDU3ZmE3NGE0Yzg2OThkMmY0ZWQ1ZjE3MjIwOWQ1ZTBmZGIzZjFhNGYwOTllZWY5YzRiMWYyZDAxZjlhNzhlYWY5ZjU0YmIyZjczNmUxMjJkNWY2NzhlMDFmYjU0YjY3NWRlYTc1ZWZkNmMwNTJhZmY3ZGVhMGM5NjAyMWQwMGQyMjI2NzdhM2RlNDdhMTdkMWI4ZWQxYmEyYWZlZDg1ZjI4NDk2ZmI2MGVjOTc5OTc4MjgzZTEyNzY2ZmI1OWUzZjY1MWI4OTVlMmQ4OGNlNGRhODg0NzJiNTFiN2RmZDc5NzdkNDk4NTY0NGU4ZjBmNGZjMTM3MmUzMGNhMTUwOTFkNDFhODIzMjZiODU0YjMzNmI1N2EwZDVjMjZjOWNjMzBlMjFjZjNkMDA0NjQ4Zjk0NjQzZWRhOTU1MmIyZjJkYzZjNGJmOTU5MDIzNTBlODlmOTNhZDRhYmEyMzZiY2E1OWE4NzY5Mjc0ZTcwN2MyNGFjNGJmOTU5MDIzNTBlODlmNDNlZGE5NTUyYjJmMmRjNjU0MTNlZjYxZTNlODc4MDk3MmNiYjg3NTVjNmU1NWNlZDljNWU0ZDE5ZTRmZjJjZTAxNmJiMGIzYWFlOTdlYTQzYmM5NWVmMGU0ZjUyNzY4NDNlZGE5NTUyYjJmMmRjNmMyOWRiYWZkYTNjYzQ1M2E3ZGJhZWNjZWJmNGIxZmQ5MTMxMmRiMzliOTU2YzBmNmNiODMxMDQ1ZDBiYjM1ZTNmMzlmMGE0ODNiN2M3ODYyZjNjMTFiN2ZiODljNDNkMjE4NGFlNzU5M2JhMmQ2YTJkOWMwMmYxYTBlNTg2MzhkYzU0NzE0NjExNzkyNGU4ZjQ4NDgzMTAyNDY5OGRlNGZiMDVmNGQzNzE3NGMwOGI2NGU2NjkxMmU2NGY5M2I0YjNiMThmMzZiZmY0NTgwN2FjMDAxOWRlY2ZkYTcyOGFmNzIyZTQwNjhlMTViM2UwMmQzMmRiYjJkOTE2MmQzNWMzZWM2OGRkZjJjMjdmOTRhZGNmYzEzOTdlOWY0NjQwODFiYWU0Y2E3Y2NjZDY0NjQzNGFmZWU4ODExYWRiZTBlY2MwY2JlOThmNDliMDZkYjE2YjNjNTZhOTRiOGZkMDU=</encrypt></jdpay>";

                //req = Regex.Replace(req, @"[\t\n\r]", "", RegexOptions.IgnoreCase);

                req = req.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                AsynNotifyResponse anyResponse = Payment.JDPay.XMLUtil.decryptResXml<AsynNotifyResponse>(Payment.JDPay.Config.JDPubKey, payConfig.JDPayDESKey, req);
                // System.Diagnostics.Debug.WriteLine("异步通知订单号：" + anyResponse.tradeNum + ",状态：" + anyResponse.status);
                Tolog("异步通知订单号：" + anyResponse.tradeNum + ",状态：" + anyResponse.status);
                string orderId = anyResponse.tradeNum;
                OrderPay orderInfo = bllOrder.GetOrderPay(orderId,payType:2);
                if (orderInfo == null)
                {
                    //context.Response.Write("订单未找到");
                    context.Response.StatusCode = 500;
                    context.Response.Write(failStr);
                    return;
                }
                if (orderInfo.Status.Equals(1))
                {
                    //Tolog("已支付");
                    // context.Response.Write("订单已支付");
                    context.Response.StatusCode = 200;
                    context.Response.Write(successStr);
                    return;
                }

                if (anyResponse.status == "2")
                {
                    //京东支付未返回流水号
                    bool result = false;
                    Alipay.ShMemberNotifyUrl shNotify = new Alipay.ShMemberNotifyUrl();
                    if (orderInfo.Type == "4")
                    {
                        result = shNotify.PayRecharge(orderInfo, "");
                    }
                    else if (orderInfo.Type == "5")
                    {
                        result = shNotify.PayRegister(orderInfo, "");
                    }
                    else if (orderInfo.Type == "6")
                    {
                        result = shNotify.PayUpgrade(orderInfo, "");
                    }
                    if (result)
                    {
                        context.Response.StatusCode = 200;
                        context.Response.Write(successStr);
                        Tolog("支付成功" + orderInfo.OrderId);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        context.Response.Write(failStr);
                        Tolog("支付失败");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write(failStr);
                    return;
                }

            }
            catch (Exception ex)
            {
                //error = "fail";
                Tolog("京东支付异常:" + ex.ToString());
            }

            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 500;
            context.Response.Write(failStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        public void Tolog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\log\\ShMemberNotifyUrl.txt", true, Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
                }

            }
            catch { }
        }
    }
}