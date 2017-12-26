using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ZentCloud.BLLJIMP;
using System.IO;

namespace Open.EZRproSDK
{
    public class Client
    {
        public readonly string APIUrl = ZentCloud.Common.ConfigHelper.GetConfigString("EZRproAPIUrl");
        public readonly string AppId = ZentCloud.Common.ConfigHelper.GetConfigString("EZRproAppId");
        public readonly string Token = ZentCloud.Common.ConfigHelper.GetConfigString("EZRproToken");
        readonly int CurrEfastShopId = ZentCloud.Common.ConfigHelper.GetConfigInt("eFastShopId");

        public string CreateSign(string appId,string timestamp,string token)
        {
            var sign = ZentCloud.Common.SHA1.SHA1_Encrypt(string.Format("AppId={0}&Timestamp={1}&Token={2}", appId, timestamp, token)).ToUpper();
            return sign;
        }
        public string CreateSign()
        {
            var timestamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(DateTime.Now);
            return CreateSign(AppId, timestamp, Token);
        }
        public string CreateSign(DateTime dt)
        {
            var timestamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(dt);
            return CreateSign(AppId, timestamp, Token);
        }
        public string CreateSign(string timestamp)
        {
            return CreateSign(AppId, timestamp, Token);
        }

        ////数据回传接收，更新或新增会员数据
        //public string CallBackMemberInfo(Entity.MemberCallBackReq memberResp)
        //{
        //    string result = string.Empty;
        //    var timestamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(DateTime.Now);
        //    var sign = ZentCloud.Common.SHA1.SHA1_Encrypt(string.Format("AppId={0}&Timestamp={1}&Token={2}", AppId, timestamp, Token));

        //    Entity.MemberInfo member = memberResp.Args;

        //    if (string.IsNullOrWhiteSpace(member.WxUnionId))
        //    {
        //        return JsonConvert.SerializeObject(new
        //        {
        //            Status = false,
        //            StatusCode = 0,
        //            Msg = "错误:WxUnionId为空",
        //            Timestamp = timestamp,
        //            Sign = sign,
        //            Result = 0
        //        });
                 
        //    }

        //    var inputSign = CreateSign(memberResp.AppId, memberResp.Timestamp, Token).ToUpper();
        //    if (inputSign != memberResp.Sign.ToUpper())
        //    {
        //        return JsonConvert.SerializeObject(new
        //        {
        //            Status = false,
        //            StatusCode = 0,
        //            Msg = "错误:签名错误",
        //            Timestamp = timestamp,
        //            Sign = sign,
        //            Result = 0
        //        });
        //    }

        //    BLLUser bllUser = new BLLUser();

        //    bool updateResult = false;
        //    if (member != null)
        //    {
        //        //根据WXUnionId 查找系统用户
        //        var userInfo = bllUser.GetUserInfoByWXUnionID(member.WxUnionId);

        //        //更新用户数据
        //        if (userInfo != null)
        //        {
        //            #region 更新用户信息
        //            if (bllUser.Update(
        //                                new ZentCloud.BLLJIMP.Model.UserInfo(),
        //                                string.Format(" Ex1='{0}',Ex2='{1}',WXNickname='{2}',Phone='{3}',TrueName='{4}',WXSex={5},Birthday='{6}',WXUnionID='{8}',WeiboID='{9}',QQ='{10}',TaobaoId='{11}',Email='{12}',Ex3='{13}',Ex4='{14}',WXProvince='{15}',WXCity='{16}',WXCountry='{17}',Password='{18}'",
        //                                    member.OldCode,
        //                                    member.Code,
        //                                    member.NickName,
        //                                    member.MobileNo,
        //                                    member.Name,
        //                                    member.Sex,
        //                                    member.Birthday,
        //                                    member.WxNo,//不更新
        //                                    member.WxUnionId,
        //                                    member.WeibNo,
        //                                    member.QqNo,
        //                                    member.TbNo,
        //                                    member.Email,
        //                                    member.RegShop,
        //                                    member.RegDate,
        //                                    member.Province,
        //                                    member.City,
        //                                    member.County,
        //                                    member.PassWord
        //                                ),
        //                                string.Format(" UserID = '{0}' ", userInfo.UserID)) > 0
        //                            )
        //            {
        //                updateResult = true;
        //            } 
        //            #endregion
        //        }
        //        else
        //        {
        //            #region 新增用户信息
        //            //新增用户信息
        //            userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
        //            //userInfo.UserID = string.Format("WXUser_yike_{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
        //            userInfo.UserID = string.Format("WXUser_yike_{0}", Guid.NewGuid().ToString());
        //            userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
        //            userInfo.UserType = 2;
        //            userInfo.WebsiteOwner = bllUser.WebsiteOwner;
        //            userInfo.Regtime = DateTime.Now;
        //            userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
        //            userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
        //            userInfo.LastLoginDate = DateTime.Now;
        //            userInfo.LoginTotalCount = 1;
                    
        //            userInfo.Ex1 = member.OldCode;//线下会员卡号
        //            userInfo.Ex2 = member.Code;//线上会员卡号
        //            userInfo.WXNickname = member.NickName;
        //            userInfo.Phone = member.MobileNo;
        //            userInfo.TrueName = member.Name;
        //            userInfo.WXSex = Convert.ToInt32(member.Sex);
        //            DateTime bd = DateTime.Now;
        //            if(DateTime.TryParse(member.Birthday, out bd))
        //            {
        //                userInfo.Birthday = bd;
        //            }

        //            /*
        //                TODO:新增的用户，如果是驿氪那边进入的而没经过我们这边，openId会跟我们这边的不一样不记录，UnionId记录，
        //                但是现有机制是，我们会根据新的openId创建新用户，可能会导致两个不同的OpenId出现相同的UnionId，后续考虑下怎么改
        //                目前先禁止没有经过我们这边就创建会员的数据进来
        //                测试可以允许注册进来
        //            */
        //            //userInfo.WXOpenId = member.WxNo;
        //            userInfo.WXUnionID = member.WxUnionId;
        //            userInfo.WeiboID = member.WeibNo;
        //            userInfo.QQ = member.QqNo;
        //            userInfo.TaobaoId = member.TbNo;
        //            userInfo.Email = member.Email;
        //            userInfo.Ex3 = member.RegShop;//开卡门店
        //            userInfo.Ex4 = member.RegDate;//开卡时间
        //            userInfo.WXProvince = member.Province;
        //            userInfo.WXCity = member.City;
        //            userInfo.WXCountry = member.County;

        //            userInfo.Password = member.PassWord;

        //            if (bllUser.Add(userInfo))
        //            {
        //                updateResult = true;
        //            }
        //            #endregion
        //        }
        //    }

        //    result = JsonConvert.SerializeObject(new
        //    {
        //        Status = updateResult,
        //        StatusCode = 200,
        //        Msg = updateResult? "成功":"失败",
        //        Timestamp = timestamp,
        //        Sign = sign,
        //        Result = 1
        //    });

        //    return result;
        //}

        public string CallBackMemberInfo(Entity.MemberCallBackReq memberResp)
        {
            Tolog("into bll CallBackMemberInfo");

            string result = string.Empty;
            var timestamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(DateTime.Now);
            var sign = ZentCloud.Common.SHA1.SHA1_Encrypt(string.Format("AppId={0}&Timestamp={1}&Token={2}", AppId, timestamp, Token));

            Tolog("CallBackMemberInfo - sign:" + sign);

            Tolog("CallBackMemberInfo - timestamp:" + timestamp);

            Entity.MemberInfo member = memberResp.Args;

            if (string.IsNullOrWhiteSpace(member.WxUnionId))
            {
                Tolog("CallBackMemberInfo - 错误:WxUnionId为空");

                return JsonConvert.SerializeObject(new
                {
                    Status = false,
                    StatusCode = 0,
                    Msg = "错误:WxUnionId为空",
                    Timestamp = timestamp,
                    Sign = sign,
                    Result = 0
                });

            }

            var inputSign = CreateSign(memberResp.AppId, memberResp.Timestamp, Token).ToUpper();

            Tolog("CreateSign ok");

            Tolog("inputSign:" + inputSign);
            //Tolog("memberResp:" + JsonConvert.SerializeObject(memberResp));
            Tolog("memberResp.Sign:" + memberResp.Sign);
            if (inputSign != memberResp.Sign.ToUpper())
            {
                Tolog("CallBackMemberInfo - 错误:签名错误");

                return JsonConvert.SerializeObject(new
                {
                    Status = false,
                    StatusCode = 0,
                    Msg = "错误:签名错误",
                    Timestamp = timestamp,
                    Sign = sign,
                    Result = 0
                });
            }

            BLLUser bllUser = new BLLUser();
            Tolog(" 开始处理会员信息 ");
            bool updateResult = false;
            if (member != null)
            {
                Tolog(" CallBackMemberInfo: member != null ");

                //根据WXUnionId 查找系统用户
                var userInfo = bllUser.GetUserInfoByWXUnionID(member.WxUnionId);


                //更新用户数据
                if (userInfo != null)
                {
                    Tolog("CallBackMemberInfo - 更新用户信息");


                    var strWhere = string.Format(" Ex1='{0}',Ex2='{1}',WXNickname='{2}',Phone='{3}',TrueName='{4}',WXSex='{5}',Birthday='{6}',WXUnionID='{8}',WeiboID='{9}',QQ='{10}',TaobaoId='{11}',Email='{12}',Ex3='{13}',Ex4='{14}',WXProvince='{15}',WXCity='{16}',WXCountry='{17}',Password='{18}'",
                                            member.OldCode,
                                            member.Code,
                                            member.NickName,
                                            member.MobileNo,
                                            member.Name,
                                            member.Sex,
                                            member.Birthday,
                                            member.WxNo,//不更新
                                            member.WxUnionId,
                                            member.WeibNo,
                                            member.QqNo,
                                            member.TbNo,
                                            member.Email,
                                            member.RegShop,
                                            member.RegDate,
                                            member.Province,
                                            member.City,
                                            member.County,
                                            member.PassWord
                                        );

                    Tolog("strWhere : " + strWhere);

                    Tolog("UserID : " + userInfo.UserID);

                    #region 更新用户信息
                    if (bllUser.Update(
                                        new ZentCloud.BLLJIMP.Model.UserInfo(),
                                        strWhere
                                        ,
                                        string.Format(" UserID = '{0}' ", userInfo.UserID)) > 0
                                    )
                    {
                        updateResult = true;
                        Tolog("CallBackMemberInfo - 更新用户信息 ok");
                    }
                    #endregion
                }
                else
                {
                    Tolog("CallBackMemberInfo - 新增用户信息");
                    #region 新增用户信息
                    //新增用户信息
                    userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                    userInfo.UserID = string.Format("WXUser_yike_{0}", Guid.NewGuid().ToString());
                    userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                    userInfo.UserType = 2;
                    userInfo.WebsiteOwner = bllUser.WebsiteOwner;
                    userInfo.Regtime = DateTime.Now;
                    userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                    userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                    userInfo.LastLoginDate = DateTime.Now;
                    userInfo.LoginTotalCount = 1;

                    userInfo.Ex1 = member.OldCode;//线下会员卡号
                    userInfo.Ex2 = member.Code;//线上会员卡号
                    userInfo.WXNickname = member.NickName;
                    userInfo.Phone = member.MobileNo;
                    userInfo.TrueName = member.Name;
                    userInfo.WXSex = Convert.ToInt32(member.Sex);
                    DateTime bd = DateTime.Now;
                    if (DateTime.TryParse(member.Birthday, out bd))
                    {
                        userInfo.Birthday = bd;
                    }

                    /*
                        TODO:新增的用户，如果是驿氪那边进入的而没经过我们这边，openId会跟我们这边的不一样不记录，UnionId记录，
                        但是现有机制是，我们会根据新的openId创建新用户，可能会导致两个不同的OpenId出现相同的UnionId，后续考虑下怎么改
                        目前先禁止没有经过我们这边就创建会员的数据进来
                        测试可以允许注册进来
                    */
                    //userInfo.WXOpenId = member.WxNo;
                    userInfo.WXUnionID = member.WxUnionId;
                    userInfo.WeiboID = member.WeibNo;
                    userInfo.QQ = member.QqNo;
                    userInfo.TaobaoId = member.TbNo;
                    userInfo.Email = member.Email;
                    userInfo.Ex3 = member.RegShop;//开卡门店
                    userInfo.Ex4 = member.RegDate;//开卡时间
                    userInfo.WXProvince = member.Province;
                    userInfo.WXCity = member.City;
                    userInfo.WXCountry = member.County;

                    userInfo.Password = member.PassWord;

                    if (bllUser.Add(userInfo))
                    {
                        Tolog("CallBackMemberInfo - 新增用户信息 ok");
                        updateResult = true;
                    }
                    #endregion
                }
            }

            Tolog("CallBackMemberInfo - 返回结果");

            result = JsonConvert.SerializeObject(new
            {
                Status = updateResult,
                StatusCode = 200,
                Msg = updateResult ? "成功" : "失败",
                Timestamp = timestamp,
                Sign = sign,
                Result = 1
            });

            Tolog("CallBackMemberInfo - 返回结果");

            return result;
        }

        public T GetCommand<T>(string argsJson, string url, string method = "get")
        {
            string resp = string.Empty;
            var timestamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(DateTime.Now);

            ZentCloud.Common.HttpInterFace http = new ZentCloud.Common.HttpInterFace();

            StringBuilder data = new StringBuilder();
            data.AppendFormat("AppId={0}", AppId);
            data.AppendFormat("&Timestamp={0}", timestamp);
            data.AppendFormat("&Sign={0}", CreateSign(timestamp));
            data.AppendFormat("&Args={0}", argsJson);

            Tolog(" 驿氪接口请求： " + data.ToString());

            if (method.Equals("get", StringComparison.OrdinalIgnoreCase) || method.Equals(""))
                resp = http.GetWebRequest(data.ToString(), url, Encoding.UTF8);
            else if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                resp = http.PostWebRequest(data.ToString(), url, Encoding.UTF8);

            Tolog(" 驿氪接口原始返回： " + resp);

            var respObj = JsonConvert.DeserializeObject<Entity.RespBase<T>>(resp);

            return respObj.Result;

            //result = respObj.Result;

            //if (result == "[]" || string.IsNullOrWhiteSpace(result))
            //{
            //    return default(T);
            //}
            //return JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argsJson"></param>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string GetCommand(string argsJson, string url, string method = "get")
        {
            string resp = string.Empty;
            var timestamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(DateTime.Now);

            ZentCloud.Common.HttpInterFace http = new ZentCloud.Common.HttpInterFace();

            StringBuilder data = new StringBuilder();
            data.AppendFormat("AppId={0}", AppId);
            data.AppendFormat("&Timestamp={0}", timestamp);
            data.AppendFormat("&Sign={0}", CreateSign(timestamp));
            data.AppendFormat("&Args={0}", argsJson);

            Tolog(" 驿氪接口请求： " + data.ToString());

            if (method.Equals("get", StringComparison.OrdinalIgnoreCase) || method.Equals(""))
                resp = http.GetWebRequest(data.ToString(), url, Encoding.UTF8);
            else if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                resp = http.PostWebRequest(data.ToString(), url, Encoding.UTF8);

            Tolog(" 驿氪接口原始返回： " + resp);

            return resp;

           


        }

        /// <summary>
        /// 获取积分
        /// </summary>
        /// <param name="oldCode">线下会员卡号</param>
        /// <param name="code">线上会员卡号</param>
        /// <param name="mobileNo">手机号</param>
        /// <returns></returns>
        public Entity.BonusGetResp GetBonus(string oldCode,string code,string mobileNo)
        {
            Entity.BonusGetResp result = null;

            var args = JsonConvert.SerializeObject(new
            {
                OldCode = "",
                Code = code,
                MobileNo = mobileNo
            });

            result = GetCommand<Entity.BonusGetResp>(args, APIUrl + "api/cvip/vipbonusget", "post");

            return result;

        }

        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="vipCode">会员线上卡号</param>
        /// <param name="transBonus">积分变动值，可支持负数</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public Entity.VipbonusUpdateResp BonusUpdate(string vipCode,int transBonus,string remark)
        {
            Entity.VipbonusUpdateResp result = null;

            var args = JsonConvert.SerializeObject(new
            {
                ShopCode = CurrEfastShopId,
                VipCode = vipCode,
                TransBonus = transBonus,
                TradeNo = "123",
                Remark = remark,
                DataOrigin = 1
            });

            result = GetCommand<Entity.VipbonusUpdateResp>(args, APIUrl + "api/cvip/vipbonusupdate", "post");

            return result;
        }
        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="vipCode">会员线上卡号</param>
        /// <param name="transBonus">积分变动值，可支持负数</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool BonusUpdateNew(string vipCode, int transBonus, string remark)
        {
           

            var args = JsonConvert.SerializeObject(new
            {
                ShopCode = CurrEfastShopId,
                VipCode = vipCode,
                TransBonus = transBonus,
                TradeNo = "123",
                Remark = remark,
                DataOrigin = 1
            });

           string result = GetCommand(args, APIUrl + "api/cvip/vipbonusupdate", "post");
           Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JToken.Parse(result);
           return token["Status"].ToString().ToLower() == "true";
            
        }
        /// <summary>
        /// 获取线下订单
        /// </summary>
        /// <param name="vipCode"></param>
        /// <param name="dataOrigin"></param>
        /// <returns></returns>
        public List<Entity.VipSaleGetResp> VipSaleGetResp(string vipCode,int dataOrigin = 0)
        {
            var args = JsonConvert.SerializeObject(new
            {
                VipCode = vipCode,
                DataOrigin = dataOrigin
            });
            return GetCommand<List<Entity.VipSaleGetResp>>(args, APIUrl + "api/csale/vipsaleget", "post");
        }
        /// <summary>
        /// 订单上传
        /// </summary>
        /// <param name="orderSrc"></param>
        /// <param name="saleType"></param>
        /// <returns></returns>
        public Entity.ReqBase OrderUpload(ZentCloud.BLLJIMP.Model.WXMallOrderInfo orderSrc,string saleType = "S")
        {
            Entity.ReqBase result = new Entity.ReqBase();

            BLLMall bllMall = new BLLMall();
            
            List<Entity.OrderInfo> orderList = new List<Entity.OrderInfo>();

            Entity.OrderInfo order = GeiMappingOrderInfo(orderSrc);
            
            orderList.Add(order);

            result = GetCommand<Entity.ReqBase>(JsonConvert.SerializeObject(orderList), APIUrl + "api/morder/ordadd", "post");

            return result;
        }
        /// <summary>
        /// 订单状态更改
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Entity.ReqBase ChangeStatus(string orderId, string status)
        {
            try
            {
                return GetCommand<Entity.ReqBase>(JsonConvert.SerializeObject(new
                {
                    ShopCode = CurrEfastShopId,
                    Code = orderId,
                    OrderStatus = GetOrderStatusCode(status),
                    StatusTime = DateTime.Now.ToString()
                }), APIUrl + "api/morder/ordstatusupdate", "post");

            }
            catch (Exception)
            {
                return null;
               
            }
        }

        public int GetOrderStatusCode(string statusStr)
        {
            int result = 2;

            switch (statusStr)
            {
                case "待付款":
                    result = 1;
                    break;
                case "待发货":
                    result = 2;
                    break;
                case "已发货":
                    result = 4;
                    break;
                case "交易成功":
                    result = 8;
                    break;
                case "已取消":
                    result = 0;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 根据本地订单映射出驿氪订单
        /// </summary>
        /// <param name="orderSrc"></param>
        /// <returns></returns>
        public Entity.OrderInfo GeiMappingOrderInfo(ZentCloud.BLLJIMP.Model.WXMallOrderInfo orderSrc)
        {
            BLLMall bllMall = new BLLMall();
            Entity.OrderInfo order = new Entity.OrderInfo();
            
            order.ShopCode = CurrEfastShopId.ToString();
            order.Code = orderSrc.OrderID;
            order.OrderTime = orderSrc.InsertDate.ToString();
            order.TotalQty = orderSrc.ProductCount;
            order.TotalMoney = 0;// (double)orderSrc.TotalAmount;
            order.IsPayed = orderSrc.PaymentStatus == 1;
            order.PayAmount = (double)orderSrc.TotalAmount;

            var userInfo = new BLLUser().GetUserInfo(orderSrc.OrderUserID);
            order.BuyerCode = userInfo.Ex2;

            order.OrderStatus = GetOrderStatusCode(orderSrc.Status);
            order.StatusTime = DateTime.Now.ToString();
            order.SellerId = orderSrc.SellerId;
            order.DataOrigin = 1;
            order.PayTime = DateTime.Now.ToString();
            order.ExpressFee = (double)orderSrc.Transport_Fee;

            order.RecvAddress = orderSrc.Address;
            order.RecvCity = orderSrc.ReceiverCity;
            order.RecvConsignee = orderSrc.Consignee;
            order.RecvCounty = orderSrc.ReceiverDist;
            order.RecvMobile = orderSrc.Phone;
            order.RecvProvince = orderSrc.ReceiverProvince;
            
            List<ZentCloud.BLLJIMP.Model.WXMallOrderDetailsInfo> detailList = bllMall.GetOrderDetailsList(orderSrc.OrderID);
            List<Entity.OrderDetail> dtls = new List<Entity.OrderDetail>();
            
            int saleProdQty = 0;
            foreach (var item in detailList)
            {
                Entity.OrderDetail dtl = new Entity.OrderDetail();
                saleProdQty++;
                
                dtl.BarCode = bllMall.GetEfastBarcode(item.SkuId.Value);
                dtl.PriceOriginal = (double)item.OrderPrice;
                dtl.PriceSell = (double)item.OrderPrice;
                dtl.Quantity = item.TotalCount;
                dtl.Amount = dtl.PriceSell * dtl.Quantity;
                dtl.IsGift = false;

                order.TotalMoney += dtl.Amount;

                dtls.Add(dtl);
            }
            
            order.Dtls = dtls;


            return order;
        }


        /// <summary>
        /// 上传退货单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Entity.ReqBase Refund(ZentCloud.BLLJIMP.Model.WXMallRefund refundInfo)
        {
            try
            {
                List<Open.EZRproSDK.Entity.RefundInfo> dtLs = new List<Open.EZRproSDK.Entity.RefundInfo>();
                Open.EZRproSDK.Entity.RefundInfo model = new Entity.RefundInfo();
                model.BarCode = refundInfo.SkuSn;
                model.ReturnMoney = refundInfo.RefundAmount;
                model.ReturnPrice = (double)refundInfo.RefundAmount;
                model.ReturnQuantity = 1;
                dtLs.Add(model);
                return GetCommand<Entity.ReqBase>(JsonConvert.SerializeObject(new
                {
                    ShopCode = CurrEfastShopId,
                    OrderCode =refundInfo.OrderId,
                    ReturnCode=refundInfo.OrderDetailId.ToString(),
                    ReturnTime = refundInfo.InsertDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ReturnReason =refundInfo.RefundReason,
                    Dtls = dtLs,
                }), APIUrl + "api/morder/ordreturn", "post");

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        /// <summary>
        /// 退货单更改
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Entity.ReqBase UpdateRefundStatus(ZentCloud.BLLJIMP.Model.WXMallRefund refundInfo)
        {
            try
            {
                return GetCommand<Entity.ReqBase>(JsonConvert.SerializeObject(new
                {
                    ShopCode = CurrEfastShopId,
                    ReturnCode = refundInfo.OrderDetailId.ToString(),
                    ReturnStatus=refundInfo.Status,
                    StatusTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    StatusRemark ="",
                }), APIUrl + "api/morder/OrdReturnStatus", "post");

            }
            catch (Exception)
            {
                return null;

            }
        }

        /// <summary>
        /// 更新商品上下架状态
        /// </summary>
        /// <param name="productInfo">商品信息</param>
        /// <returns></returns>
        public Entity.ReqBase UpdateProductIsOnSale(ZentCloud.BLLJIMP.Model.WXMallProductInfo productInfo)
        {


            try
            {
                Open.EZRproSDK.Entity.ProductSale model = new Entity.ProductSale();
                model.ItemNo = productInfo.ProductCode;
                model.IsOnSale = productInfo.IsOnSale;
                return GetCommand<Entity.ReqBase>(JsonConvert.SerializeObject(model), APIUrl + "api/mprod/prodonSale", "post");

            }
            catch (Exception)
            {
                return null;

            }
        }
        /// <summary>
        /// 会员信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetUserInfo(string unionId)
        {
            try
            {
                var argsJson = ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    WxUnionID = unionId
                });
                var timeStamp = ZentCloud.Common.DateTimeHelper.GetTimeIntStr(DateTime.Now);
                ZentCloud.Common.HttpInterFace http = new ZentCloud.Common.HttpInterFace();
                StringBuilder data = new StringBuilder();
                data.AppendFormat("AppId={0}", AppId);
                data.AppendFormat("&Timestamp={0}", timeStamp);
                data.AppendFormat("&Sign={0}", CreateSign(timeStamp));
                data.AppendFormat("&Args={0}", argsJson);
                return http.PostWebRequest(data.ToString(), APIUrl + "api/cvip/vipget", Encoding.UTF8);

                
               

            }
            catch (Exception)
            {
                return null;

            }
        }


        private void Tolog(string msg)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\log\EZRproSDK.txt", true, Encoding.GetEncoding("GB2312")))
            {
                sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
            }

        }

    }
}
