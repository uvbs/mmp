using Newtonsoft.Json;
using Payment.Alipay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Alipay
{
    public partial class ShMemberNotifyUrl : System.Web.UI.Page
    {
        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> parametersAll = bllPay.GetRequestParameter();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(parametersAll["notify_data"]);
                xmlDoc.Save(string.Format("C:\\Alipay\\mallnotify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                //商户订单号
                string outTradeNo = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                //支付宝交易号
                string tradeNo = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                //交易状态
                string tradeStatus = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                string websiteOwner = bllOrder.WebsiteOwner;
                OrderPay orderPay = bllOrder.GetOrderPay(outTradeNo,"",websiteOwner);
                if (parametersAll.Count > 0)//判断是否有带返回参数
                {
                    Notify aliNotify = new Notify();
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

                            if (orderPay.Status.Equals(0))//只有未付款状态
                            {
                                bool result = false;
                                if (orderPay.Type == "4") {
                                    result = PayRecharge(orderPay, tradeNo);
                                }
                                else if (orderPay.Type == "5") {
                                    result = PayRegister(orderPay, tradeNo);
                                }
                                else if (orderPay.Type == "6")
                                {
                                    result = PayUpgrade(orderPay, tradeNo);
                                }
                                if (result)
                                {
                                    Response.Write("success");
                                }
                                else
                                {
                                    Response.Write("fail");
                                }
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


                            if (orderPay.Status.Equals(0))//只有未付款状态
                            {
                                bool result = false;
                                if (orderPay.Type == "4")
                                {
                                    result = PayRecharge(orderPay, tradeNo);
                                }
                                else if (orderPay.Type == "5")
                                {
                                    result = PayRegister(orderPay, tradeNo);
                                }
                                else if (orderPay.Type == "6")
                                {
                                    result = PayUpgrade(orderPay, tradeNo);
                                }
                                if (result)
                                {
                                    Response.Write("success");
                                }
                                else
                                {
                                    Response.Write("fail");
                                }
                            }
                            else
                            {
                                Response.Write("success");  //请不要修改或删除
                            }
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
        /// <summary>
        /// 支付充值
        /// </summary>
        /// <param name="orderPay"></param>
        /// <returns></returns>
        public bool PayRecharge(OrderPay orderPay, string tradeNo)
        {
            string msg = "";
            orderPay.Trade_No = tradeNo;
            return bll.PayRechargeTransfers(orderPay, "", tradeNo, out msg);
        }
        /// <summary>
        /// 支付注册
        /// </summary>
        /// <param name="orderPay"></param>
        /// <returns></returns>
        public bool PayRegister(OrderPay orderPay, string tradeNo)
        {
            BLLDistribution bllDist = new BLLDistribution();
            string msg = "";
            BLLJIMP.Model.API.User.PayRegisterUser requestUser = JsonConvert.DeserializeObject<BLLJIMP.Model.API.User.PayRegisterUser>(orderPay.Ex1);
            UserLevelConfig levelConfig = bll.QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", requestUser.level.ToString());
            if (levelConfig == null) return false;
            UserInfo regUser = bllUser.GetUserInfoByPhone(requestUser.phone, orderPay.WebsiteOwner);
            if (regUser != null && regUser.MemberLevel >= 10) return false;
            if (regUser != null && regUser.MemberLevel >= requestUser.level) return false;
            if (regUser != null)
            {
                regUser.UserType = 2;
                regUser.TrueName = requestUser.truename;
                regUser.DistributionOwner = requestUser.spreadid;
                regUser.MemberLevel = requestUser.level;
                regUser.MemberStartTime = DateTime.Now;
                regUser.MemberApplyTime = orderPay.InsertDate;
                regUser.RegIP = requestUser.regIP;
                regUser.Password = requestUser.password;
                regUser.MemberApplyStatus = 9;
                regUser.IdentityCard = requestUser.idcard;
                regUser.Province = requestUser.province;
                regUser.City = requestUser.city;
                regUser.District = requestUser.district;
                regUser.Town = requestUser.town;
                regUser.ProvinceCode = requestUser.provinceCode;
                regUser.CityCode = requestUser.cityCode;
                regUser.DistrictCode = requestUser.districtCode;
                regUser.TownCode = requestUser.townCode;
                regUser.RegUserID = null;
                regUser.RegisterWay = "线上";
            }
            else
            {
                regUser = new UserInfo();
                regUser.UserID = requestUser.userid;
                regUser.UserType = 2;
                regUser.TrueName = requestUser.truename;
                regUser.WebsiteOwner = orderPay.WebsiteOwner;
                regUser.DistributionOwner = requestUser.spreadid;
                regUser.Phone = requestUser.phone;
                regUser.MemberLevel = requestUser.level;
                regUser.MemberStartTime = DateTime.Now;
                regUser.MemberApplyTime = orderPay.InsertDate;
                regUser.Regtime = DateTime.Now;
                regUser.LastLoginDate = DateTime.Parse("1970-01-01");
                regUser.RegIP = requestUser.regIP;
                regUser.Password = requestUser.password;
                regUser.MemberApplyStatus = 9;
                regUser.IdentityCard = requestUser.idcard;
                regUser.Province = requestUser.province;
                regUser.City = requestUser.city;
                regUser.District = requestUser.district;
                regUser.Town = requestUser.town;
                regUser.ProvinceCode = requestUser.provinceCode;
                regUser.CityCode = requestUser.cityCode;
                regUser.DistrictCode = requestUser.districtCode;
                regUser.TownCode = requestUser.townCode;
                regUser.RegisterWay = "线上";
            }
            orderPay.Trade_No = tradeNo;
            if (!string.IsNullOrEmpty(levelConfig.CouponId))
            {
                BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
                bllCardCoupon.SendCardCouponsByCurrUserInfo(regUser, levelConfig.CouponId);
            }
            return bll.PayRegisterTransfers(regUser, orderPay, "", tradeNo, levelConfig, out msg);
        }
        /// <summary>
        /// 支付升级
        /// </summary>
        /// <param name="orderPay"></param>
        /// <returns></returns>
        public bool PayUpgrade(OrderPay orderPay, string tradeNo)
        {
            string msg = "";
            BLLJIMP.Model.API.User.PayUpgrade payUpgrade = JsonConvert.DeserializeObject<BLLJIMP.Model.API.User.PayUpgrade>(orderPay.Ex1);
            orderPay.Trade_No = tradeNo;
            UserLevelConfig levelConfig = bll.QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", payUpgrade.toLevel.ToString());
            UserInfo currUser = bllUser.GetUserInfo(orderPay.UserId,orderPay.WebsiteOwner);
            if (!string.IsNullOrEmpty(levelConfig.CouponId))
            {
                BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
                bllCardCoupon.SendCardCouponsByCurrUserInfo(currUser, levelConfig.CouponId);
            }
            return bll.PayUpgradeTransfers(orderPay, payUpgrade, "", tradeNo, out msg);
        }
    }
}