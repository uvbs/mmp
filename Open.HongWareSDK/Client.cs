using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCJson;
using System.Security.Cryptography;
using System.IO;
using ZentCloud.BLLJIMP.Model;
using Open.HongWareSDK.Entity;
using Newtonsoft.Json.Linq;

namespace Open.HongWareSDK
{
    /// <summary>
    /// 宏巍接口
    /// </summary>
    public class Client
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        public Client(string websiteOwner)
        {
            ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();
            WebsiteInfo websiteInfo = bll.Get<WebsiteInfo>(string.Format("WebsiteOwner='{0}'", websiteOwner));
            WebsiteOwner = websiteOwner;
            APIBaseUrl = "http://" + ZentCloud.Common.ConfigHelper.GetConfigString("HongWareApiDomain");
            //Nick = websiteInfo.ApiNick;
            //Name = websiteInfo.ApiName;
            AppId = websiteInfo.AuthorizerAppId;
            ShopName = websiteInfo.ShopName;
            OrgCode = websiteInfo.OrgCode;
            if (websiteInfo.IsUnionHongware == 1)
            {
                if (string.IsNullOrEmpty(OrgCode))
                {
                    var orgModel = GetOrgCode();
                    if (orgModel != null)
                    {
                        if (orgModel.orgCode != null)
                        {
                            OrgCode = orgModel.orgCode.orgCode;
                            if (!string.IsNullOrEmpty(OrgCode))
                            {
                                websiteInfo.OrgCode = OrgCode;
                                bll.Update(websiteInfo);
                            }
                        }
                    }

                }
            }




        }

        /// <summary>
        /// API 基本地址 http://www.host.com
        /// </summary>
        private string APIBaseUrl;
        /// <summary>
        /// 昵称
        /// </summary>
        private string Nick = "O2Omobile";
        /// <summary>
        /// 名字
        /// </summary>
        private string Name = "O2Omobile";
        /// <summary>
        /// 微信公众号AppId
        /// </summary>
        private string AppId = "";
        /// <summary>
        /// 宏巍微信公众号ShopName
        /// </summary>
        private string ShopName = "";
        /// <summary>
        /// 商家编码
        /// </summary>
        private string OrgCode = "";
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner;
        /// <summary>
        /// http请求
        /// </summary>
        ZentCloud.Common.HttpInterFace WebRequest = new ZentCloud.Common.HttpInterFace();
        /// <summary>
        /// 数据格式
        /// json/xml
        /// </summary>
        private string Format = "json";
        /// <summary>
        /// 门店
        /// </summary>
        private string Store = "";
        /// <summary>
        /// 操作人
        /// </summary>
        private string UserName = "V5mobile";
        /// <summary>
        /// 回调
        /// </summary>
        private string CallBack = "";
        /// <summary>
        /// SpId
        /// </summary>
        private string IsSpId = "00034393";
        /// <summary>
        /// SystemId
        /// </summary>
        public string SystemId = "5124";
        /// <summary>
        /// UseRangeId
        /// </summary>
        private string UseRangeId = "0000020790";
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="method">接口方法名</param>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        private string CreateSign(string method, string timeStamp)
        {
            return MD5(Base64Encode(Nick) + Base64Encode(method) + Base64Encode(timeStamp) + Base64Encode(Name) + Base64Encode(Format));
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ((int)ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <param name="inputCharset">编码 默认utf-8</param>
        /// <returns></returns>
        private string MD5(string content, string inputCharset = "UTF-8")
        {
            StringBuilder sbResult = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] byteArry = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(content));
            for (int i = 0; i < byteArry.Length; i++)
            {
                sbResult.Append(byteArry[i].ToString("x").PadLeft(2, '0'));
            }
            return sbResult.ToString();
        }

        /// <summary>
        /// Base 64编码
        /// </summary>
        /// <param name="content">原始内容</param>
        /// <param name="inputCharset">编码 默认UTF-8</param>
        /// <returns></returns>
        private string Base64Encode(string content, string inputCharset = "UTF-8")
        {

            byte[] byteArry = System.Text.Encoding.GetEncoding(inputCharset).GetBytes(content);
            return Convert.ToBase64String(byteArry);

        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\log\hongweilog.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
                }
            }
            catch { }
        }

        /// <summary>
        /// 请求接口
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="url">接口基本路径</param>
        /// <param name="method">接口方法名</param>
        /// <param name="par">应用级 参数键值对集合</param>
        /// <param name="postMethod">get/post</param>
        /// <returns></returns>
        private T GetCommand<T>(string url, string method, Dictionary<string, string> par, string postMethod = "get")
        {

            string resp = string.Empty;
            var timeStamp = GetTimeStamp();
            StringBuilder requestData = new StringBuilder();

            #region 系统级参数
            requestData.AppendFormat("nick={0}", Nick);
            requestData.AppendFormat("&name={0}", Name);
            requestData.AppendFormat("&method={0}", method);
            requestData.AppendFormat("&format={0}", Format);
            requestData.AppendFormat("&timestamp={0}", timeStamp);
            requestData.AppendFormat("&sign={0}", CreateSign(method, timeStamp));
            #endregion

            #region 应用级参数
            foreach (var item in par)
            {
                requestData.AppendFormat("&{0}={1}", item.Key, item.Value);

            }
            #endregion

            if (postMethod.Equals("get", StringComparison.OrdinalIgnoreCase) || method.Equals(""))
                resp = WebRequest.GetWebRequest(requestData.ToString(), url, Encoding.UTF8);
            else if (postMethod.Equals("post", StringComparison.OrdinalIgnoreCase))
                resp = WebRequest.PostWebRequest(requestData.ToString(), url, Encoding.UTF8);
            Tolog(requestData.ToString());
            Tolog(resp);
            return JsonConvert.DeserializeObject<T>(resp);

        }


        /// <summary>
        /// 请求接口
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="url">接口基本路径</param>
        /// <param name="method">接口方法名</param>
        /// <param name="par">应用级 参数键值对集合</param>
        /// <param name="postMethod">get/post</param>
        /// <returns></returns>
        private JToken GetCommand(string url, string method, Dictionary<string, string> par, string postMethod = "get")
        {

            string resp = string.Empty;
            var timeStamp = GetTimeStamp();
            StringBuilder requestData = new StringBuilder();

            #region 系统级参数
            requestData.AppendFormat("nick={0}", Nick);
            requestData.AppendFormat("&name={0}", Name);
            requestData.AppendFormat("&method={0}", method);
            requestData.AppendFormat("&format={0}", Format);
            requestData.AppendFormat("&timestamp={0}", timeStamp);
            requestData.AppendFormat("&sign={0}", CreateSign(method, timeStamp));
            #endregion

            #region 应用级参数
            foreach (var item in par)
            {
                requestData.AppendFormat("&{0}={1}", item.Key, item.Value);

            }
            #endregion

            if (postMethod.Equals("get", StringComparison.OrdinalIgnoreCase) || method.Equals(""))
                resp = WebRequest.GetWebRequest(requestData.ToString(), url, Encoding.UTF8);
            else if (postMethod.Equals("post", StringComparison.OrdinalIgnoreCase))
                resp = WebRequest.PostWebRequest(requestData.ToString(), url, Encoding.UTF8);
            Tolog(requestData.ToString());
            Tolog(resp);
            return JToken.Parse(resp);

        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns>会员信息</returns>
        public MemberInfo GetMemberInfo(string openId)
        {
            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("orgCode", OrgCode);
            par.Add("store", Store);
            par.Add("userName", UserName);
            par.Add("openID", openId);
            par.Add("appID", AppId);
            par.Add("callBack", CallBack);
            return GetCommand<MemberInfo>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberOpenidGet", "V5.mobile.member.openid.get", par, "get");


        }

        /// <summary>
        /// 获取会员收货地址列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public MemberAddress GetMemberAddressList(string phone, string openId)
        {
            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("orgCode", OrgCode);
            par.Add("store", Store);
            par.Add("userName", UserName);
            par.Add("memberMobile", phone);
            par.Add("openID", openId);
            par.Add("appID", AppId);
            par.Add("callBack", CallBack);
            return GetCommand<MemberAddress>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberAddressSearch", "V5.mobile.member.address.search", par, "get");


        }


        /// <summary>
        /// 修改会员积分
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="point">积分变动值</param>
        /// <returns></returns>
        public bool UpdateMemberScore(string phone, string openId, float point)
        {

            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("orgCode", OrgCode);
            par.Add("store", Store);
            par.Add("userName", UserName);
            par.Add("memberMobile", phone);
            par.Add("point", point.ToString());
            par.Add("openID", openId);
            par.Add("appID", AppId);
            par.Add("callBack", CallBack);
            return GetCommand<RespBase>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberPointModify", "V5.mobile.member.point.modify", par, "post").isSuccess;




        }
        ///// <summary>
        ///// 修改会员积分
        ///// </summary>
        ///// <param name="phone">手机号</param>
        ///// <param name="point">积分变动值</param>
        ///// <returns></returns>
        //public bool UpdateMemberScore(string phone, float point)
        //{

        //    Dictionary<string, string> par = new Dictionary<string, string>();
        //    par.Add("orgCode", OrgCode);
        //    par.Add("store", Store);
        //    par.Add("userName", UserName);
        //    par.Add("memberMobile", phone);
        //    par.Add("point", point.ToString());
        //    //par.Add("openID", openId);
        //    par.Add("appID", AppId);
        //    par.Add("callBack", CallBack);
        //    return GetCommand<RespBase>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberPointModify", "V5.mobile.member.point.modify", par, "post").isSuccess;




        //}

        /// <summary>
        /// 修改会员余额
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="balance">余额变动值</param>
        /// <returns></returns>
        public bool UpdateMemberBlance(string phone, string openId, float balance)
        {

            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("orgCode", OrgCode);
            par.Add("store", Store);
            par.Add("userName", UserName);
            par.Add("memberMobile", phone);
            par.Add("balance", balance.ToString());
            par.Add("openID", openId);
            par.Add("appID", AppId);
            par.Add("callBack", CallBack);
            return GetCommand<RespBase>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberBalanceModify", "V5.mobile.member.balance.modify", par, "post").isSuccess;

        }
        ///// <summary>
        ///// 修改会员余额
        ///// </summary>
        ///// <param name="phone">手机号</param>
        ///// <param name="balance">余额变动值</param>
        ///// <returns></returns>
        //public bool UpdateMemberBlance(string phone, float balance)
        //{

        //    Dictionary<string, string> par = new Dictionary<string, string>();
        //    par.Add("orgCode", OrgCode);
        //    par.Add("store", Store);
        //    par.Add("userName", UserName);
        //    par.Add("memberMobile", phone);
        //    par.Add("balance", balance.ToString());
        //    par.Add("appID", AppId);
        //    par.Add("callBack", CallBack);
        //    return GetCommand<RespBase>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberBalanceModify", "V5.mobile.member.balance.modify", par, "post").isSuccess;

        //}

        /// <summary>
        /// 订单通知
        /// </summary>
        /// <param name="openId">微信OpenId</param>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        public bool OrderNotice(string openId, string orderId)
        {

            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("openID", openId);
            par.Add("appID", AppId);
            par.Add("orgCode", OrgCode);
            par.Add("userName", UserName);
            par.Add("orderNumber", orderId);
            par.Add("shopName", ShopName);
            par.Add("callBack", CallBack);
            return GetCommand<RespBase>(APIBaseUrl + "/openApi/dyncHongware/mobile/wmallOrderNotice", "V5.mobile.wmall.order.notice", par, "get").isSuccess;

        }
        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool OrderPay(string orderId, string amount, string callBackUrl, out string payUrl, out string msg)
        {

            msg = "";
            payUrl = "";
            Dictionary<string, string> par = new Dictionary<string, string>();

            Nick = "heilan";
            Name = "heilan";
            APIBaseUrl = "http://172.18.9.5:8080";
            if (WebsiteOwner == "hailandev")
            {
                APIBaseUrl = "http://172.18.9.5:8180";
            }
            OrgCode = "172";
            var orderInfo = new
            {
                amount = amount,
                body = "订单号" + orderId,
                trade_no = orderId,
                client = "01",
                subject = "订单号" + orderId,
                pay_channel = "01",
                show_url = callBackUrl

            };

            par.Add("appID", AppId);
            par.Add("orgCode", OrgCode);
            par.Add("userName", UserName);
            par.Add("orderNumber", orderId);
            par.Add("shopName", ShopName);
            par.Add("callBack", CallBack);
            par.Add("store", "heilan");
            par.Add("op", "hongware");
            par.Add("orderInfo", ZentCloud.Common.JSONHelper.ObjectToJson(orderInfo));

            var result = GetCommand<Open.HongWareSDK.Entity.PayModel>(APIBaseUrl + "/openApi/dyncHongware/mobile/yimaPay", "V5.mobile.yima.pay", par, "post");
            if (result.isSuccess)
            {
                payUrl = result.data.url_all.Trim();
            }
            else
            {
                msg = result.map.errorMsg;
            }
            return result.isSuccess;

        }

        /// <summary>
        /// 会员权益接口
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Open.HongWareSDK.Entity.MemberRightModel MemberRight(List<ZentCloud.BLLJIMP.Model.API.Mall.SkuModel> skuList)
        {


            Open.HongWareSDK.Entity.MemberRightModel memberRight = new Entity.MemberRightModel();
            memberRight.isSuccess = true;
            memberRight.orders = new List<Entity.OrderModel>();

            Entity.OrderModel order = new Entity.OrderModel();
            order.orderItems = new List<Entity.OrderItem>();
            foreach (var item in skuList)
            {
                Entity.OrderItem orderItem = new Entity.OrderItem();
                orderItem.pro_RealPrice = (((decimal)item.price) * (decimal)1.0).ToString();
                orderItem.mem_realPrice = (((decimal)item.price) * (decimal)1.0).ToString();
                orderItem.birth_realPrice = (((decimal)item.price) * (decimal)1.0).ToString();
                orderItem.sku = item.sku_sn;
                orderItem.pro_dis = "0";
                orderItem.mem_dis = "0";
                orderItem.birth_dis = "0";
                order.orderItems.Add(orderItem);

            }
            memberRight.orders.Add(order);
            return memberRight;


            //Dictionary<string, string> par = new Dictionary<string, string>();
            //Nick = "heilan";
            //Name = "heilan";
            ////APIBaseUrl = "http://openapi.heilan.stage.shopware.cn";
            //APIBaseUrl = "http://sandbox.o2o.swapi.hongware.com";
            //OrgCode = "heilan";
            //List<object> orderList = new List<object>();
            //List<object> orderItems = new List<object>();
            //foreach (var sku in skuList)
            //{
            //    var orderItem = new
            //    {
            //        sku = sku.sku_sn,
            //        originPrice = sku.price,
            //        qty = sku.count

            //    };
            //    orderItems.Add(orderItem);
            //}
            //var order = new
            //{
            //    shopCode = "",
            //    mobile = "",
            //    saleDate = DateTime.Now.ToString(),
            //    totalcount = skuList.Sum(p => p.count),
            //    orderItems = orderItems


            //};
            //orderList.Add(order);
            //var orderObj = new
            //{

            //    orders = orderList

            //};
            //par.Add("appID", AppId);
            //par.Add("orgCode", OrgCode);
            //par.Add("userName", UserName);
            ////par.Add("orderNumber", orderId);
            ////par.Add("shopName", ShopName);
            //par.Add("callBack", CallBack);
            //par.Add("store", "heilan");
            //par.Add("op", "hongware");
            //par.Add("jsonstr", ZentCloud.Common.JSONHelper.ObjectToJson(orderObj));
            //return GetCommand<Open.HongWareSDK.Entity.MemberRightModel>(APIBaseUrl + "/openApi/dyncHongware/mobile/memberRights", "V5.mobile.member.rights", par, "post");


        }


        #region 翼码卡券

        /// <summary>
        /// 创建翼码卡券
        /// </summary>
        public bool CreateYimaCard(YimaCard model, out string msg, out string activityId)
        {
            msg = "";
            activityId = "";
            try
            {

                Dictionary<string, string> par = new Dictionary<string, string>();
                //Nick = "zeo";
                //Name = "zeo";
                //APIBaseUrl = "http://sandbox.swapi.hongware.com/openApi/dyncHongware";
                //OrgCode = "192";
             

                Nick = "heilan";
                Name = "heilan";
                APIBaseUrl = "http://openapi.heilan.stage.shopware.cn/openApi/dyncHongware";
                OrgCode = "172";

                StringBuilder xml = new StringBuilder();
                xml.AppendFormat("<?xml version=\"1.0\" encoding=\"GBK\"?>");
                xml.AppendFormat("<business_trans version=\"1.0\">");
                xml.AppendFormat("<request_type>activity_create_request</request_type>");
                xml.AppendFormat("<isspid>{0}</isspid>", IsSpId);
                xml.AppendFormat("<transaction_id>{0}</transaction_id>", model.transaction_id);
                xml.AppendFormat("<user_id>user01</user_id>");
                xml.AppendFormat("<system_id>{0}</system_id>", SystemId);
                xml.AppendFormat("<activity_create_request>");
                xml.AppendFormat("<activity_name>{0}</activity_name>", model.activity_name);
                xml.AppendFormat("<activity_short_name>{0}</activity_short_name>", model.activity_short_name);
                xml.AppendFormat("<use_range_id>{0}</use_range_id>", UseRangeId);
                xml.AppendFormat("<begin_time>{0}</begin_time>", model.begin_time);
                xml.AppendFormat("<end_time>{0}</end_time>", model.end_time);
                xml.AppendFormat("<card_type>{0}</card_type>", model.card_type);
                xml.AppendFormat("<codes>{0}</codes>", model.codes);
                xml.AppendFormat("<store_list>{0}</store_list>", model.store_list);
                xml.AppendFormat("<use_type>{0}</use_type>", model.use_type);
                xml.AppendFormat("<use_content>{0}</use_content>", model.use_content);
                xml.AppendFormat("<amt_flag>{0}</amt_flag>", model.amt_flag);
                xml.AppendFormat("<single_flag>{0}</single_flag>", model.single_flag);
                xml.AppendFormat("<channel_type>1</channel_type>");
                xml.AppendFormat("<discount_amt>{0}</discount_amt>", model.discount_amt);
                xml.AppendFormat("</activity_create_request>");
                xml.AppendFormat("</business_trans>");

            

                par.Add("appID", AppId);
                par.Add("orgCode", OrgCode);
                par.Add("userName", UserName);
                //par.Add("orderNumber", orderId);
                //par.Add("shopName", ShopName);
                par.Add("callBack", CallBack);
                par.Add("store", "heilan");
                par.Add("op", "hongware");
                par.Add("xml", System.Web.HttpUtility.UrlEncode(xml.ToString()));
                var strResult = GetCommand(APIBaseUrl + "/mobile/yimaActivityCreate", "V5.mobile.yima.activity.create", par, "post");

                if ((bool)strResult["isSuccess"] && (strResult["data"]["result"]["id"].ToString()=="0000"))//第一步制作活动
                {
                    #region 第二步制卡
                    string notifyUrl = "http://shop.eichitoo.com/serv/api/zentcloudopen/cardcoupon/callback.ashx";
                    if (WebsiteOwner == "hailandev")
                    {
                        notifyUrl = "http://test.eichitoo.com/serv/api/zentcloudopen/cardcoupon/callback.ashx";
                    }

                    activityId = strResult["data"]["activity_id"].ToString();
                    if (string.IsNullOrEmpty(activityId))
                    {
                        msg = "activity_id 生成失败";
                        return false;
                    }
                    xml.Clear();
                    xml.AppendFormat("<?xml version=\"1.0\" encoding=\"GBK\"?>");
                    xml.AppendFormat("<business_trans version=\"1.0\">");
                    xml.AppendFormat("<request_type>makecard_request</request_type>");
                    xml.AppendFormat("<isspid>{0}</isspid>", IsSpId);
                    xml.AppendFormat("<transaction_id>{0}</transaction_id>", model.transaction_id_makecard);
                    xml.AppendFormat("<user_id>{0}</user_id>", WebsiteOwner);
                    xml.AppendFormat("<system_id>{0}</system_id>", SystemId);
                    xml.AppendFormat("<makecard_request>");
                    xml.AppendFormat("<count>{0}</count>", model.count);
                    xml.AppendFormat("<begin_time>{0}</begin_time>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    xml.AppendFormat("<end_time>{0}</end_time>", model.end_time);
                    xml.AppendFormat("<activity_id>{0}</activity_id>", activityId);
                    xml.AppendFormat("<notify_url><![CDATA[{0}]]></notify_url>", notifyUrl);
                    xml.AppendFormat("</makecard_request>");
                    xml.AppendFormat("</business_trans>");
                    par["xml"] = System.Web.HttpUtility.UrlEncode(xml.ToString());
                    var cardResult = GetCommand(APIBaseUrl + "/mobile/yimaCardCreate", "V5.mobile.yima.card.create", par, "post");

                    if ((bool)cardResult["isSuccess"])
                    {

                        if (cardResult["data"]["result"]["id"].ToString() == "0000")
                        {
                            return true;
                        }
                        else
                        {
                            msg = cardResult["data"]["result"]["comment"].ToString();
                        }

                    }
                    else
                    {
                        msg = cardResult["map"]["errorMsg"].ToString();
                    } 
                    #endregion

                }
                else
                {
                    msg = strResult["map"]["errorMsg"].ToString();
                    msg = strResult["data"]["result"]["comment"].ToString();
                }
                return false;

            }
            catch (Exception ex)
            {

                msg = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 领取卡券
        /// </summary>
        public bool YimaGetCard(YimaGetCard model, out string msg, out string cardCode)
        {
            msg = "";
            cardCode = "";
            try
            {

                Dictionary<string, string> par = new Dictionary<string, string>();
                //Nick = "zeo";
                //Name = "zeo";
                ////APIBaseUrl = "http://openapi.heilan.stage.shopware.cn";
                ////APIBaseUrl = "http://openapi.heilan.stage.shopware.cn/openApi/dyncHongware";
                //APIBaseUrl = "http://sandbox.swapi.hongware.com/openApi/dyncHongware";
                //OrgCode = "192";


                Nick = "heilan";
                Name = "heilan";
                APIBaseUrl = "http://openapi.heilan.stage.shopware.cn/openApi/dyncHongware";
                OrgCode = "172";
                StringBuilder xml = new StringBuilder();

                xml.AppendFormat("<?xml version=\"1.0\" encoding=\"GBK\"?>");
                xml.AppendFormat("<business_trans version=\"1.0\">");
                xml.AppendFormat("<request_type>getcard_request</request_type>");
                xml.AppendFormat("<isspid>{0}</isspid>", IsSpId);
                xml.AppendFormat("<transaction_id>{0}</transaction_id>", model.transaction_id);
                xml.AppendFormat("<user_id>{0}</user_id>", WebsiteOwner);
                xml.AppendFormat("<system_id>{0}</system_id>", SystemId);
                xml.AppendFormat("<getcard_request>");
                xml.AppendFormat("<makecard_transid>{0}</makecard_transid>", model.makecard_transid);
                xml.AppendFormat("<start_number>{0}</start_number>", model.start_number);
                xml.AppendFormat("<count>{0}</count>", model.count);
                xml.AppendFormat("<activity_id>{0}</activity_id>", model.activity_id);
                xml.AppendFormat("</getcard_request>");
                xml.AppendFormat("</business_trans>");

                par.Add("appID", AppId);
                par.Add("orgCode", OrgCode);
                par.Add("userName", UserName);
                //par.Add("orderNumber", orderId);
                //par.Add("shopName", ShopName);
                par.Add("callBack", CallBack);
                par.Add("store", "heilan");
                par.Add("op", "hongware");
                par.Add("xml", System.Web.HttpUtility.UrlEncode(xml.ToString()));
                var strResult = GetCommand(APIBaseUrl + "/mobile/yimaCardGet", "V5.mobile.yima.card.get", par, "post");

                if ((bool)strResult["isSuccess"])
                {

                    var cardListStr = strResult["data"]["card_list"].ToString();
                    cardListStr = cardListStr.Replace(@"\", "");
                    JToken jTCad=JToken.Parse(cardListStr);
                    cardCode=jTCad[0]["card_no"].ToString();
                    return true;

                }
                else
                {
                    msg = strResult["map"]["errorMsg"].ToString();
                }

                return false;

            }
            catch (Exception ex)
            {

                msg = ex.ToString();
                return false;
            }
        }


        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <returns></returns>
        public bool YimaMyCardList(YimaQueryCard model, out List<string> cardCodeList, out string msg)
        {
            msg = "";
            cardCodeList = new List<string>();
            try
            {

                Dictionary<string, string> par = new Dictionary<string, string>();
                Nick = "heilan";
                Name = "heilan";
                APIBaseUrl = "http://openapi.heilan.stage.shopware.cn/openApi/dyncHongware";
                OrgCode = "172";

                StringBuilder xml = new StringBuilder();
                xml.AppendFormat("<?xml version=\"1.0\" encoding=\"GBK\"?>");
                xml.AppendFormat("<business_trans>");
                xml.AppendFormat("<request_type>card_query_request</request_type>");
                xml.AppendFormat("<isspid>{0}</isspid>", IsSpId);
                xml.AppendFormat("<pos_id>{0}</pos_id>", model.pos_id);
                xml.AppendFormat("<store_id>{0}</store_id>", model.store_id);
                xml.AppendFormat("<pos_seq>{0}</pos_seq>", model.pos_seq);
                xml.AppendFormat("<user_id>{0}</user_id>", WebsiteOwner);
                xml.AppendFormat("<system_id>{0}</system_id>", SystemId);
                xml.AppendFormat("<card_query_request>");
                xml.AppendFormat("<valid_info></valid_info>", model.valid_info);
                xml.AppendFormat("<goods_list></goods_list>", model.goods_list);
                xml.AppendFormat("</card_query_request>");
                xml.AppendFormat("</business_trans>");

                par.Add("appID", AppId);
                par.Add("orgCode", OrgCode);
                par.Add("userName", UserName);
                //par.Add("orderNumber", orderId);
                //par.Add("shopName", ShopName);
                par.Add("callBack", CallBack);
                par.Add("store", "heilan");
                par.Add("op", "hongware");
                par.Add("xml", System.Web.HttpUtility.UrlEncode(xml.ToString()));
                var strResult = GetCommand(APIBaseUrl + "/mobile/yimaCardQuery", "V5.mobile.yima.card.query", par, "post");

                if ((bool)strResult["isSuccess"])
                {

                    JArray jArry = (JArray)strResult["data"]["query_info"];

                    foreach (JToken item in jArry)
                    {
                        cardCodeList.Add(item["card_no"].ToString());

                    }


                    return true;

                }




            }
            catch (Exception ex)
            {

                msg = ex.ToString();
                return false;
            }


            return true;

        }


        /// <summary>
        /// 验证卡券列表
        /// </summary>
        /// <returns></returns>
        public bool YimaCardVerify(YimaVerifyCard model, out List<string> cardCodeList, out string msg)
        {
            msg = "";
            cardCodeList = new List<string>();
            try
            {

                Dictionary<string, string> par = new Dictionary<string, string>();
                Nick = "heilan";
                Name = "heilan";
                APIBaseUrl = "http://openapi.heilan.stage.shopware.cn/openApi/dyncHongware";
                OrgCode = "172";


                StringBuilder xml = new StringBuilder();
                xml.AppendFormat("<?xml version=\"1.0\" encoding=\"GBK\"?>");
                xml.AppendFormat("<business_trans>");
                xml.AppendFormat("<request_type>card_verify_request</request_type>");
                xml.AppendFormat("<isspid>{0}</isspid>", IsSpId);
                xml.AppendFormat("<pos_id>{0}</pos_id>", model.pos_id);
                xml.AppendFormat("<store_id>{0}</store_id>", model.store_id);
                xml.AppendFormat("<pos_seq>{0}</pos_seq>", model.pos_seq);
                xml.AppendFormat("<user_id>{0}</user_id>", WebsiteOwner);
                xml.AppendFormat("<system_id>{0}</system_id>", SystemId);
                xml.AppendFormat("<card_verify_request>");
                xml.AppendFormat("<valid_info>{0}</valid_info>", model.valid_info);
                xml.AppendFormat("<goods_list>{0}</goods_list>", model.goods_list);
                xml.AppendFormat("</card_verify_request>");
                xml.AppendFormat("</business_trans>");

                par.Add("appID", AppId);
                par.Add("orgCode", OrgCode);
                par.Add("userName", UserName);
                //par.Add("orderNumber", orderId);
                //par.Add("shopName", ShopName);
                par.Add("callBack", CallBack);
                par.Add("store", "heilan");
                par.Add("op", "hongware");
                par.Add("xml", System.Web.HttpUtility.UrlEncode(xml.ToString()));
                var strResult = GetCommand(APIBaseUrl + "/mobile/yimaCardVerify", "V5.mobile.yima.card.verify", par, "post");

                if ((bool)strResult["isSuccess"])
                {
                    //JToken jToken =strResult["data"];
                    JArray jArry = (JArray)strResult["data"]["query_info"];
                   
                    foreach (JToken item in jArry)
                    {
                        cardCodeList.Add(item["card_no"].ToString());

                    }


                    return true;

                }




            }
            catch (Exception ex)
            {

                msg = ex.ToString();
                return false;
            }


            return true;

        }


        ///// <summary>
        ///// 卡券通知
        ///// </summary>
        ///// <returns></returns>
        //public bool YimaCardNotice(YimaCardNotice model, out string msg)
        //{
        //    msg = "";
        //    try
        //    {

        //        Dictionary<string, string> par = new Dictionary<string, string>();
        //        Nick = "V5mobile";
        //        Name = "V5mobile";
        //        //APIBaseUrl = "http://openapi.heilan.stage.shopware.cn";
        //        APIBaseUrl = "http://sandbox.o2o.swapi.hongware.com";
        //        OrgCode = "heilan";
        //        StringBuilder xml = new StringBuilder();
        //        xml.AppendFormat("<?xml version=\"1.0\" encoding=\"GBK\"?>");
        //        xml.AppendFormat("<business_trans>");
        //        xml.AppendFormat("<request_type>card_callback_request</request_type>");
        //        xml.AppendFormat("<isspid>{0}</isspid>", IsSpId);
        //        xml.AppendFormat("<pos_id>{0}</pos_id>", model.pos_id);
        //        xml.AppendFormat("<store_id>{0}</store_id>", model.store_id);
        //        xml.AppendFormat("<pos_seq>{0}</pos_seq>", model.pos_seq);
        //        xml.AppendFormat("<user_id>{0}</user_id>", WebsiteOwner);
        //        xml.AppendFormat("<system_id>{0}</system_id>", SystemId);
        //        xml.AppendFormat("<card_callback_request>");
        //        xml.AppendFormat("<order_result>1</order_result>");
        //        xml.AppendFormat("<org_pos_seq>{0}</org_pos_seq>", model.org_pos_seq);
        //        xml.AppendFormat("</card_callback_request>");
        //        xml.AppendFormat("</business_trans>");


        //        par.Add("appID", AppId);
        //        par.Add("orgCode", OrgCode);
        //        par.Add("userName", UserName);
        //        //par.Add("orderNumber", orderId);
        //        //par.Add("shopName", ShopName);
        //        par.Add("callBack", CallBack);
        //        par.Add("store", "heilan");
        //        par.Add("op", "hongware");
        //        par.Add("xml", xml.ToString());
        //        var strResult = GetCommand<Open.HongWareSDK.Entity.RespCard>(APIBaseUrl + "/openApi/dyncHongware/mobile/Yima", "V5.mobile.yima.common", par, "post");

        //        if (strResult.isSuccess)
        //        {
        //            JToken jToken = JToken.Parse(strResult.data);
        //            if (jToken["business_trans"]["result"]["id"].ToString() == "0000")
        //            {
        //                return true;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        msg = ex.ToString();
        //        return false;
        //    }


        //    return false;

        //}


        /// <summary>
        /// 核销卡券
        /// </summary>
        /// <returns></returns>
        public bool YimaCardHexiao(string cardCode, out string msg)
        {
            msg = "";
            try
            {

                Dictionary<string, string> par = new Dictionary<string, string>();
                Nick = "V5mobile";
                Name = "V5mobile";
                //APIBaseUrl = "http://openapi.heilan.stage.shopware.cn";
                APIBaseUrl = "http://sandbox.o2o.swapi.hongware.com";
                OrgCode = "heilan";
                StringBuilder xml = new StringBuilder();
                xml.AppendFormat("<?xml version=\"1.0\" encoding=\"gb2312\"?>");
                xml.AppendFormat("<VerifyNotifyReq>");
                xml.AppendFormat("<ISSPID>{0}</ISSPID>", IsSpId);
                xml.AppendFormat("<CardNo>{0}</CardNo>", cardCode);
                xml.AppendFormat("<TransTime>{0}</TransTime>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                xml.AppendFormat("</VerifyNotifyReq>");

                par.Add("appID", AppId);
                par.Add("orgCode", OrgCode);
                par.Add("userName", UserName);
                //par.Add("orderNumber", orderId);
                //par.Add("shopName", ShopName);
                par.Add("callBack", CallBack);
                par.Add("store", "heilan");
                par.Add("op", "hongware");
                par.Add("xml", System.Web.HttpUtility.UrlEncode(xml.ToString()));
                var strResult = GetCommand(APIBaseUrl + "/openApi/dyncHongware/mobile/Yima", "V5.mobile.yima.common", par, "post");

                if ((bool)strResult["isSuccess"])
                {
                    JToken jToken =strResult["data"];
                    if (jToken["VerifyNotifyRes"]["Status"]["StatusCode"].ToString() == "0000")
                    {
                        return true;
                    }
                    else
                    {
                        msg = jToken["VerifyNotifyRes"]["Status"]["StatusText"].ToString();
                    }

                }

            }
            catch (Exception ex)
            {

                msg = ex.ToString();
                return false;
            }


            return false;

        } 
        #endregion


        /// <summary>
        /// 获取省市区信息
        /// </summary>
        /// <param name="id">省市区代码 默认1表示获取所有的省份跟直辖市</param>
        /// <returns></returns>
        public Entity.Areas GetAreaInfo(string id = "1")
        {
            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("orgCode", OrgCode);
            par.Add("id", id);
            par.Add("callBack", CallBack);
            Entity.Areas result = GetCommand<Entity.Areas>(APIBaseUrl + "/openApi/dyncHongware/mobile/areasGet", "V5.mobile.areas.get", par, "post");
            if (result.areas == null)
            {
                result.areas = new List<Entity.AreasModel>();
            }
            return result;
        }
        /// <summary>
        /// 获取OrgCode
        /// </summary>
        /// <returns></returns>
        public Entity.OrgCode GetOrgCode()
        {
            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("orgCode", "zko2o");
            par.Add("appID", AppId);
            par.Add("callBack", CallBack);
            return GetCommand<Entity.OrgCode>(APIBaseUrl + "/openApi/dyncHongware/mobile/orgCodeGet", "V5.mobile.orgcode.get", par, "get");

        }

        ///// <summary>
        ///// 获取所有省市区数据
        ///// </summary>
        ///// <returns></returns>
        //public List<Entity.AreasModel> GetAllAreas()
        //{
        //    ///返回结果
        //    List<Entity.AreasModel> data = new List<Entity.AreasModel>();
        //    foreach (var province in GetAreaInfo().areas)//循环省
        //    {
        //        province.parent_id = "1";
        //        province.type = "Province";
        //        data.Add(province);//把省份加入结果
        //        foreach (var city in GetAreaInfo(province.id).areas)//循环市
        //        {
        //            city.type = "City";
        //            data.Add(city);//把城市加入结果
        //            foreach (var dist in GetAreaInfo(city.id).areas)//循环区
        //            {
        //                dist.type = "District";
        //                data.Add(dist);//把区域加入结果

        //            }

        //        }


        //    }
        //    return data;

        //}



    }
}
