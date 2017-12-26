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
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    public partial class PayRegisterNotify : System.Web.UI.Page
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
                //xmlDoc.Load(@"D:\PayRegisterNotify20170613203329981.xml");
                xmlDoc.Save(string.Format("C:\\WXPay\\PayRegisterNotify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志

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
                BLLJIMP.Model.API.User.PayRegisterUser requestUser = JsonConvert.DeserializeObject<BLLJIMP.Model.API.User.PayRegisterUser>(orderPay.Ex1);
                UserLevelConfig levelConfig = bll.QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", requestUser.level.ToString());
                if(levelConfig == null){
                    Tolog("会员等级未找到");
                    Response.Write(failXml);
                    return;
                }
                //更新订单状态
                if (parametersAll["return_code"].Equals("SUCCESS") && parametersAll["result_code"].Equals("SUCCESS"))//交易成功
                {
                    UserInfo regUser = bllUser.GetUserInfoByPhone(requestUser.phone, orderPay.WebsiteOwner);
                    if (regUser != null && regUser.MemberLevel >= 10)
                    {
                        Tolog("该手机已注册会员");
                        Response.Write(failXml);
                        return;
                    }
                    if (regUser != null && regUser.MemberLevel > requestUser.level)
                    {
                        Tolog("该会员有更高级别");
                        Response.Write(failXml);
                        return;
                    }
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
                        regUser.IsDisable = 0;
                        regUser.RegUserID = null;
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
                        regUser.IsDisable = 0;
                        regUser.RegUserID = null;
                    }
                    string msg = "";
                    orderPay.Trade_No = parametersAll["transaction_id"];
                    //线上注册分佣
                    if (!bll.PayRegisterTransfers(regUser, orderPay, parametersAll["openid"], parametersAll["transaction_id"], levelConfig, out msg))
                    {
                        Tolog(msg);
                        Response.Write(failXml);
                        return;
                    }
                    //发送优惠券
                    if (!string.IsNullOrEmpty(levelConfig.CouponId))
                    {
                        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
                        bllCardCoupon.SendCardCouponsByCurrUserInfo(regUser,levelConfig.CouponId);
                    }

                    Response.Write(successXml);
                    return;
                }
                Tolog("返回信息有误");
                Response.Write(failXml);
            }
            catch (Exception ex)
            {
                Tolog("出错了：" + ex.ToString());
                Response.Write(failXml);

            } 
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
             //using (StreamWriter sw = new StreamWriter(@"D:\PayRegisterLog.txt", true, Encoding.GetEncoding("GB2312")))

            using (StreamWriter sw = new StreamWriter(@"D:\PayRegisterLog.txt", true, Encoding.GetEncoding("GB2312")))
            {
                sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
            }
        }
    }
}