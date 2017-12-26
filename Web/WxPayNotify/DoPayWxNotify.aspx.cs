using CommonPlatform.Helper;
using Payment.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    public partial class DoPayWxNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Request.InputStream);
                try
                {
                    xmlDoc.Save(string.Format("C:\\WXPay\\Notify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                }
                catch (Exception)
                {
                }
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

                BllOrder bllOrder = new BllOrder();
                BLLJIMP.Model.OrderPay orderPay = bllOrder.GetOrderPay(parametersAll["out_trade_no"], "", bllOrder.WebsiteOwner);

                BllPay bllPay = new BllPay();
                PayConfig payConfig = bllPay.GetPayConfig();
                if (bllPay.VerifySignatureWx(parametersAll, payConfig.WXPartnerKey))//验证签名
                {
                    if (orderPay == null)
                    {
                        Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                        return;
                    }

                    if (orderPay.Status == 1)
                    {
                        Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                        return;
                    }
                    orderPay.Status = 1;
                    if (bllOrder.Update(orderPay))
                    {
                        BLLUser bllUser = new BLLUser();
                        if (orderPay.Type == "1")
                        {
                            int score = 0;
                            int.TryParse(orderPay.Ex1, out score);
                            BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
                            string ScoreDispalyName = bllKeyValueData.GetDataVaule("ScoreDispalyName", "1", bllKeyValueData.WebsiteOwner);

                            string msg = "消费" + orderPay.Total_Fee + "元，充值" + score + ScoreDispalyName;
                            if (bllUser.AddUserScoreDetail(orderPay.UserId, EnumStringHelper.ToString(ScoreDefineType.Recharge), bllUser.WebsiteOwner, score, msg))
                            {
                                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                            }
                            else
                            {
                                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                            }
                            //BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
                            //bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, orderPay.UserId, msg);
                        }
                        else if (orderPay.Type == "2")
                        {
                            string invoiceMsg;
                            if(orderPay.Ex1 == "1"){
                                invoiceMsg = "带发票，";
                            }
                            else{
                                invoiceMsg = "无发票，";
                            }

                            string msg = "充值VIP，" + invoiceMsg + "消费" + orderPay.Total_Fee.ToString() + "元";


                            BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
                            string VIPDatelong = bllKeyValueData.GetDataVaule("VIPDatelong", "1", bllKeyValueData.WebsiteOwner);
                            if (string.IsNullOrWhiteSpace(VIPDatelong)) VIPDatelong = "12";
                            int datelong = Convert.ToInt32(VIPDatelong);

                            BLLUserExpand bllUserExpand = new BLLUserExpand();
                            UserExpand userVip = bllUserExpand.GetUserExpand(BLLJIMP.Enums.UserExpandType.UserIsVip, orderPay.UserId);
                            string userVipEndDate;
                            if (userVip == null || DateTime.Parse(userVip.DataValue)<DateTime.Now)
                            {
                                userVipEndDate = DateTime.Now.AddMonths(datelong).ToString("yyyy-MM-dd");
                            }
                            else{
                                userVipEndDate = DateTime.Parse(userVip.DataValue).AddMonths(datelong).ToString("yyyy-MM-dd");
                            }
                            bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.UserIsVip, orderPay.UserId, userVipEndDate);

                            //更新用户字段
                            UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
                            scoreModel.AddNote = msg;
                            scoreModel.AddTime = DateTime.Now;
                            scoreModel.Score = 0;
                            scoreModel.UserID = orderPay.UserId;
                            scoreModel.ScoreType = "RechargeVIP";
                            UserInfo currUser = bllUser.GetUserInfo(orderPay.UserId);
                            scoreModel.TotalScore = currUser.TotalScore;
                            scoreModel.WebSiteOwner = currUser.WebsiteOwner;
                            if (bllUser.Add(scoreModel))
                            {
                                BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
                                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.SystemMessage, null, null, orderPay.UserId, msg);

                                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                            }
                            else
                            {
                                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                            }
                        }

                        return;
                    }
                    else
                    {
                        Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                        return;
                    }
                }
                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
            }
            catch (Exception)
            {
                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

            }
           
            
        }
    }
}