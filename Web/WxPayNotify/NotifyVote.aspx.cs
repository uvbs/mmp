using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using Payment.WeiXin;
using System.Xml;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    /// <summary>
    /// 投票微信支付通知
    /// </summary>
    public partial class NotifyVote : System.Web.UI.Page
    {
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Request.InputStream);
                xmlDoc.Save(string.Format("C:\\WXPay\\NotifyVote{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志
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

                //验签参数 不包括 sign 参数
                Dictionary<string, string> parametersSign = (from entry in parametersAll
                                                             where !entry.Key.Equals("sign")
                                                             orderby entry.Key ascending
                                                             select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                var orderInfo = bllOrder.GetOrderPay(parametersAll["out_trade_no"]);
                var payConfig = bllPay.GetPayConfig();
                string strSign = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(parametersSign, false);
                if (!MD5SignUtil.VerifySignature(strSign, parametersAll["sign"], payConfig.WXPartnerKey))//验证签名
                {

                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                    return;
                }
                if (orderInfo == null)
                {
                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

                    return;
                }
                if (orderInfo.Status.Equals(1))
                {
                    Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                    return;
                }
                if (orderInfo.Status.Equals(0))//只有未付款状态
                {
                        ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                        try
                        {
                            UserInfo userInfo = bllUser.GetUserInfo(orderInfo.UserId);
                            if (userInfo.AvailableVoteCount == null)
                            {
                                userInfo.AvailableVoteCount = 0;
                            }
                            userInfo.AvailableVoteCount += int.Parse(orderInfo.Ex1);
                            orderInfo.Status = 1;
                            orderInfo.Trade_No = parametersAll["out_trade_no"];
                            if (!bllOrder.Update(orderInfo, tran))
                            {
                                tran.Rollback();
                                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                            }
                            if (bllUser.Update(userInfo, string.Format(" AvailableVoteCount={0}", userInfo.AvailableVoteCount), string.Format(" AutoID={0}", userInfo.AutoID), tran) < 1)
                            {
                                tran.Rollback();
                                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

                            }
                            tran.Commit();
                            Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");




                        }
                        catch
                        {

                            tran.Rollback();
                            Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                        }



                }
                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");



            }
            catch (Exception)
            {
                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

            }



        }
    }
}