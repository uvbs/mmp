using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    public static class MyEnumerableExtensions
    {
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element))) { yield return element; }
            }
        }
    }

    /// <summary>
    /// 卡券
    /// </summary>
    /// 
    public class CardCoupon : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLStoredValueCard bllStoredValue = new BLLJIMP.BLLStoredValueCard();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLWeixinCard bllWeixinCard = new BLLJIMP.BLLWeixinCard();
        /// <summary>
        /// 查询我的卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 20;
            pageSize = int.MaxValue;
            string cardcouponStatus = context.Request["cardcoupon_status"];
            string isCanUse = context.Request["is_can_use"];//可以正常使用的标识
            string amount = context.Request["amount"];//订单金额
            string isNotShowStoreValue = context.Request["is_notshow_storevalue"];//是否不显示储值卡
            // string skuIds = context.Request["sku_ids"];
            bool isSuccess = false;
            string msg = "";
            string couponName = "";
            int totalCount = 0;
            List<MyCardModel> list = new List<MyCardModel>();

            #region 优惠券
            var sourceData = bllCardCoupon.GetMyCardCoupons(currentUserInfo.UserID, pageIndex, pageSize, out totalCount, cardcouponStatus);
           
            foreach (var item in sourceData)
            {
                BLLJIMP.Model.CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(item.CardId);
                cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon, item);
                MyCardModel model = new MyCardModel();
                model.cardcoupon_id = item.AutoId;
                model.cardcoupon_number = item.CardCouponNumber;
                model.main_cardcoupon_id = item.CardId;
                
                model.cardcoupon_name = cardCoupon.Name;
                model.cardcoupon_type = ConvertCardCouponType(cardCoupon.CardCouponType);
                model.valid_from = cardCoupon.ValidFrom.ToString();
                model.valid_to = cardCoupon.ValidTo.ToString();
                model.img_url = bllMall.GetImgUrl(cardCoupon.Logo);
                model.cardcoupon_status = item.Status;
                model.cardcoupon_gettime = bllCardCoupon.GetTimeStamp(item.InsertDate);
                model.product_id = cardCoupon.Ex2;
                model.valid_from_timestamp = bllCardCoupon.GetTimeStamp((DateTime)cardCoupon.ValidFrom);
                model.valid_to_timestamp = bllCardCoupon.GetTimeStamp((DateTime)cardCoupon.ValidTo);
                model.discount = cardCoupon.Ex1;
                model.deductible_amount = cardCoupon.Ex3;
                model.freefreight_amount = cardCoupon.Ex4;
                model.buckle_amount = cardCoupon.Ex5;
                model.buckle_sub_amount = cardCoupon.Ex6;
                model.limit_type = cardCoupon.Ex7;
                model.product_tags = cardCoupon.Ex8;
                model.hexiao_channel = item.HexiaoChannel;
                model.is_can_use_shop = cardCoupon.IsCanUseShop;
                model.is_can_use_groupbuy = cardCoupon.IsCanUseGroupbuy;

                if (model.cardcoupon_type == 4)//满减券直接转成现金券
                {
                    model.buckle_amount = "";
                    model.buckle_sub_amount = "";
                    model.cardcoupon_type = 1;
                    model.deductible_amount = cardCoupon.Ex6;

                }

                #region 转赠信息
                model.is_can_give = bllCardCoupon.IsCanGiveCoupon(item, out msg);
                if (!string.IsNullOrEmpty(item.FromUserId))//赠送人信息
                {
                    model.from_user_info = new UserInfoModel();
                    UserInfo fromUserInfo = bllUser.GetUserInfo(item.FromUserId);
                    if (fromUserInfo != null)
                    {
                        model.from_user_info.head_img_url = bllUser.GetUserDispalyAvatar(fromUserInfo);
                        model.from_user_info.nick_name = bllUser.GetUserDispalyName(fromUserInfo);

                    }
                }
                if (!string.IsNullOrEmpty(item.ToUserId))//被赠送人信息
                {
                    model.to_user_info = new UserInfoModel();
                    UserInfo toUserInfo = bllUser.GetUserInfo(item.ToUserId);
                    if (toUserInfo != null)
                    {
                        model.to_user_info.head_img_url = bllUser.GetUserDispalyAvatar(toUserInfo);
                        model.to_user_info.nick_name = bllUser.GetUserDispalyName(toUserInfo);

                    }
                }
                #endregion

                if (isCanUse == "1")//可以使用
                {

                    if (model.cardcoupon_status == 0)
                    {
                        if (cardCoupon.ValidFrom != null && cardCoupon.ValidTo != null)
                        {
                            DateTime dtNow = DateTime.Now;
                            if (dtNow >= (DateTime)(cardCoupon.ValidFrom) && (dtNow <= (DateTime)(cardCoupon.ValidTo)))
                            {
                                list.Add(model);
                                //if (!string.IsNullOrEmpty(amount))
                                //{
                                //    if (bllMall.CalcDiscountAmount(item.AutoId.ToString(), decimal.Parse(amount), CurrentUserInfo.UserID, out isSuccess, out msg, out couponName) > 0)
                                //    {
                                //        list.Add(model);
                                //    }
                                //}
                                //else
                                //{
                                //    list.Add(model);
                                //}

                            }

                        }
                    }


                }
                else
                {
                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (bllMall.CalcDiscountAmount(item.AutoId.ToString(), decimal.Parse(amount), currentUserInfo.UserID, out isSuccess, out msg, out couponName) > 0)
                        {
                            list.Add(model);
                        }
                    }
                    else
                    {
                        list.Add(model);
                    }

                }

            }
            #endregion



            #region 储值卡转换成现金券
            if (string.IsNullOrEmpty(isNotShowStoreValue))
            {
                
                var storeValueCardList = bllStoredValue.GetCanUseStoredValueCardList(currentUserInfo.UserID);
                foreach (var item in storeValueCardList)
                {
                    try
                    {


                        BLLJIMP.Model.StoredValueCard cardCoupon = bllStoredValue.Get<StoredValueCard>(string.Format(" AutoId={0}", item.CardId));
                        MyCardModel model = new MyCardModel();
                        model.cardcoupon_id = item.AutoId;
                        model.main_cardcoupon_id = item.CardId;
                        model.cardcoupon_name = string.Format("{0}(储值卡余额{1}元)", cardCoupon.Name, bllMall.GetStoreValueCardCanUseAmount(item.AutoId.ToString(), item.UserId));
                        model.valid_from = DateTime.Now.ToString();
                        model.valid_to = cardCoupon.ValidTo.ToString();
                        // model.img_url = bllMall.GetImgUrl(cardCoupon.Logo);
                        model.cardcoupon_status = 0;
                        //model.cardcoupon_gettime = bllCardCoupon.GetTimeStamp(item.InsertDate);
                        //model.product_id = cardCoupon.Ex2;
                        model.valid_from_timestamp = bllCardCoupon.GetTimeStamp(DateTime.Now);
                        model.valid_to_timestamp = bllCardCoupon.GetTimeStamp((DateTime)cardCoupon.ValidTo);
                        model.discount = "";
                        //model.deductible_amount = cardCoupon.Ex3;
                        model.freefreight_amount = "";
                        model.buckle_amount = "";
                        model.buckle_sub_amount = "";
                        model.limit_type = "";
                        model.product_tags = "";
                        //model.hexiao_channel = item.HexiaoChannel;
                        model.is_can_use_shop = 1;
                        model.is_can_use_groupbuy = 1;
                        model.product_id = "";
                        model.buckle_amount = "";
                        model.buckle_sub_amount = "";
                        model.cardcoupon_type = 1;
                        model.deductible_amount = (item.Amount - bllStoredValue.GetUseRecordList(item.AutoId, item.UserId).Sum(p => p.UseAmount)).ToString();
                        model.is_store_card = 1;
                        list.Add(model);
                        totalCount++;

                    }
                    catch (Exception ex)
                    {

                       
                        continue;



                    }
                }
            #endregion

                #region 海澜处理

                if (bllCardCoupon.WebsiteOwner.Contains("hailan"))
                {
                    Open.HongWareSDK.Client client = new Open.HongWareSDK.Client(bllCardCoupon.WebsiteOwner);
                    Open.HongWareSDK.Entity.YimaVerifyCard yimaCard=new Open.HongWareSDK.Entity.YimaVerifyCard();
                    yimaCard.pos_seq=DateTime.Now.ToString("yyyyMMddHHmmss");

                    List<string> cardCodes=new List<string>();
                    client.YimaCardVerify(yimaCard, out cardCodes, out  msg);
                    if (cardCodes.Count>0)
                    {

                        foreach (var item in list)
                        {
                            if (cardCodes.Contains(item.yima_cardcode))
                            {
                                list.Remove(item);
                            }
                            
                        }

                    }
                    else
                    {
                        list = new List<MyCardModel>();//无卡券可用
                    }

                }
                #endregion

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = list.Count,
                list = list
               
            });
        }
        /// <summary>
        /// 月供宝会员升级优惠券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyCardList(HttpContext context)
        {
            int totalCount = 0;
            var sourceData = bllCardCoupon.GetMyCardCoupons(currentUserInfo.UserID, 1, 1000, out totalCount, "0");
            string v1CouponId = ZentCloud.Common.ConfigHelper.GetConfigString("YGBV1CouponId");
            string v2CouponId = ZentCloud.Common.ConfigHelper.GetConfigString("YGBV2CouponId");
            List<MyCardModel> list = new List<MyCardModel>();
            foreach (var item in sourceData)
            {
                BLLJIMP.Model.CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(item.CardId);
                MyCardModel model = new MyCardModel();
                if (item.CardId.ToString() == v1CouponId)
                {
                    model.cardcoupon_id = item.AutoId;
                    model.cardcoupon_number = item.CardCouponNumber;
                    model.main_cardcoupon_id = item.CardId;
                    model.cardcoupon_name = cardCoupon.Name;
                    model.img_url = bllMall.GetImgUrl(cardCoupon.Logo);
                    model.cardcoupon_status = item.Status;
                    model.main_cardcoupon_id = cardCoupon.CardId;
                    model.ex1 = "V1";
                    list.Add(model);
                }
                if (item.CardId.ToString() == v2CouponId)
                {
                    model.cardcoupon_id = item.AutoId;
                    model.cardcoupon_number = item.CardCouponNumber;
                    model.main_cardcoupon_id = item.CardId;
                    model.cardcoupon_name = cardCoupon.Name;
                    model.img_url = bllMall.GetImgUrl(cardCoupon.Logo);
                    model.cardcoupon_status = item.Status;
                    model.main_cardcoupon_id = cardCoupon.CardId;
                    model.ex1 = "V2";
                    list.Add(model);
                }
            }
            list=list.DistinctBy(p => p.ex1).ToList();
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = list.Count,
                list = list

            });

        }
       
        /// <summary>
        /// 计算优惠券优惠金额
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CalcDiscountAmount(HttpContext context)
        {

            string cardCouponId = context.Request["cardcoupon_id"];
            string data = context.Request["data"];
            //
            decimal discountAmount = 0;//优惠金额
            bool isSuccess = false;
            string msg = "";
            discountAmount = bllMall.CalcDiscountAmount(cardCouponId, data, currentUserInfo.UserID, out isSuccess, out msg);
            if (isSuccess)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    discount_amount = discountAmount

                });

            }
            else
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    errmsg = msg
                });
            }

            //


        }
        /// <summary>
        /// 领取卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReciveCardCoupon(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];
            CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(int.Parse(cardCouponId));
            if (cardCoupon == null)
            {
                resp.errcode = 1;
                resp.errmsg = "cardcoupon_id 不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon);
            var myCardCoupon = bllCardCoupon.GetMyCardCouponMainId(int.Parse(cardCouponId), currentUserInfo.UserID);
            if (myCardCoupon != null)
            {
                resp.errcode = 1;
                resp.errmsg = "已经领取过了";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
            {
                resp.errcode = 2;
                resp.errmsg = "卡券已过期";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (cardCoupon.MaxCount > 0)
            {
                if (bllCardCoupon.GetCardCouponSendCount(cardCoupon.CardId) >= cardCoupon.MaxCount)
                {
                    resp.errcode = 2;
                    resp.errmsg = "卡券已经领完";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }

            if (cardCoupon.GetLimitType != null)
            {
                if (cardCoupon.GetLimitType == "1" && !bllUser.IsDistributionMember(currentUserInfo, true))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyDistMember;
                    resp.errmsg = "只有分销员才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (cardCoupon.GetLimitType == "2" && bllUser.IsDistributionMember(currentUserInfo))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyNotDistMember;
                    resp.errmsg = "该券仅新用户（无购买历史）可以领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))
            {

                if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyChannel;
                    resp.errmsg = "只有指定渠道才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
                string channelUserId = bllDis.GetUserChannel(currentUserInfo);

                if (cardCoupon.BindChannelUserId != channelUserId)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyChannel;
                    resp.errmsg = "只有指定渠道才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }



            }



            MyCardCoupons model = new MyCardCoupons();
            model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllMall.GetGUID(BLLJIMP.TransacType.CommAdd));
            model.CardCouponType = cardCoupon.CardCouponType;
            model.CardId = cardCoupon.CardId;
            model.InsertDate = DateTime.Now;
            model.UserId = currentUserInfo.UserID;
            model.WebSiteOwner = bllCardCoupon.WebsiteOwner;
            if (bllCardCoupon.Add(model))
            {
                resp.errmsg = "ok";
                string title = "您收到了一张优惠券";
                string content = string.Format("{0}", cardCoupon.Name);

                if (cardCoupon.ValidTo.HasValue)
                {
                    content += string.Format("\\n{0}", ((DateTime)cardCoupon.ValidTo).ToString("yyyy-MM-dd"));
                }

                string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);

                if (bllUser.WebsiteOwner == "jikuwifi")
                {
                    url = string.Format("http://{0}/customize/jikuwifi/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                }

                bllWeixin.SendTemplateMessageNotifyComm(currentUserInfo, title, content, url);

                //#region 同时发放到微信卡包
                //if (!string.IsNullOrEmpty(cardCoupon.WeixinCardId) && (!string.IsNullOrEmpty(CurrentUserInfo.WXOpenId)))
                //{
                //    bllWeixinCard.SendByMass(cardCoupon.WeixinCardId, CurrentUserInfo.WXOpenId);

                //} 
                //#endregion
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "领取优惠券失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取主卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMainCardCoupon(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];//主卡券ID
            string myCardCouponId = context.Request["my_cardcoupon_id"];//我的卡券ID
            if (string.IsNullOrEmpty(cardCouponId))
            {
                resp.errcode = 1;
                resp.errmsg = "cardcoupon_id 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(int.Parse(cardCouponId));
            if (cardCoupon == null)
            {
                resp.errcode = 1;
                resp.errmsg = "cardcoupon_id 不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon);
            if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))
            {

                if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyChannel;
                    resp.errmsg = "只有指定渠道才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
                string channelUserId = bllDis.GetUserChannel(currentUserInfo);
                if (cardCoupon.BindChannelUserId != channelUserId)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyChannel;
                    resp.errmsg = "只有指定渠道才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }



            }

            MainCardModel model = new MainCardModel();
            model.cardcoupon_id = cardCoupon.CardId;
            model.cardcoupon_name = cardCoupon.Name;
            model.cardcoupon_type = ConvertCardCouponType(cardCoupon.CardCouponType);
            model.valid_from = cardCoupon.ValidFrom.ToString();
            model.valid_to = cardCoupon.ValidTo.ToString();
            model.img_url = bllMall.GetImgUrl(cardCoupon.Logo);
            model.discount = string.IsNullOrEmpty(cardCoupon.Ex1) ? 0 : double.Parse(cardCoupon.Ex1);
            model.product_id = string.IsNullOrEmpty(cardCoupon.Ex2) ? 0 : double.Parse(cardCoupon.Ex2);
            model.deductible_amount = string.IsNullOrEmpty(cardCoupon.Ex3) ? 0 : double.Parse(cardCoupon.Ex3);
            model.freefreight_amount = string.IsNullOrEmpty(cardCoupon.Ex4) ? 0 : double.Parse(cardCoupon.Ex4);
            model.buckle_amount = string.IsNullOrEmpty(cardCoupon.Ex5) ? 0 : double.Parse(cardCoupon.Ex5);
            model.buckle_sub_amount = string.IsNullOrEmpty(cardCoupon.Ex6) ? 0 : double.Parse(cardCoupon.Ex6);
            model.max_count = cardCoupon.MaxCount;
            model.send_count = cardCoupon.SendCount;
            model.un_send_count = cardCoupon.UnSendCount;
            model.is_recivece = bllCardCoupon.IsReciveCoupon(cardCoupon.CardId, currentUserInfo.UserID);
            model.valid_to_timestamp = bllCardCoupon.GetTimeStamp((DateTime)cardCoupon.ValidTo);
            model.limit_type = cardCoupon.Ex7;
            model.product_tags = cardCoupon.Ex8;
            model.user_get_limit_type = cardCoupon.GetLimitType;
            model.is_can_use_shop = cardCoupon.IsCanUseShop;
            model.is_can_use_groupbuy = cardCoupon.IsCanUseGroupbuy;
            model.expire_time_type = cardCoupon.ExpireTimeType;
            model.expire_day = cardCoupon.ExpireDay;
            model.weixin_card_id = cardCoupon.WeixinCardId==null?"":cardCoupon.WeixinCardId;
            if (!string.IsNullOrEmpty(myCardCouponId))
            {
                MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(myCardCouponId));
                if (myCardCoupon != null)
                {
                    UserInfo fromUserInfo = bllUser.GetUserInfo(myCardCoupon.UserId);
                    if (fromUserInfo != null)
                    {
                        model.from_user_info = new UserInfoModel();
                        model.from_user_info.head_img_url = bllUser.GetUserDispalyAvatar(fromUserInfo);
                        model.from_user_info.nick_name = bllUser.GetUserDispalyName(fromUserInfo);
                    }
                    if (!string.IsNullOrEmpty(myCardCoupon.ToUserId))
                    {
                        model.is_donation = true;
                    }
                    model.cardcoupon_number=myCardCoupon.CardCouponNumber;
                }
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(model);

        }
        /// <summary>
        /// 卡券转赠
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Give(HttpContext context)
        {
            string cardcouponId = context.Request["cardcoupon_id"];//我的卡券ID
            if (string.IsNullOrEmpty(cardcouponId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "cardcoupon_id 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            MyCardCoupons fromMyCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(cardcouponId));//要赠送的卡券信息
            CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(fromMyCardCoupon.CardId);
            if (fromMyCardCoupon == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (fromMyCardCoupon.UserId == currentUserInfo.UserID)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.NoFollow;
                apiResp.msg = "不能接收自己的卡券";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (!string.IsNullOrEmpty(fromMyCardCoupon.FromUserId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.NoFollow;
                apiResp.msg = "优惠券不能多次转赠";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (cardCoupon.GetLimitType != null)
            {
                if (cardCoupon.GetLimitType == "1" && !bllUser.IsDistributionMember(currentUserInfo, true))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyDistMember;
                    apiResp.msg = "只有分销员才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                if (cardCoupon.GetLimitType == "2" && bllUser.IsDistributionMember(currentUserInfo))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyNotDistMember;
                    apiResp.msg = "该券仅新用户（无购买历史）可以领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
            }
            if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))
            {

                if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyChannel;
                    apiResp.msg = "只有指定渠道才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
                string channelUserId = bllDis.GetUserChannel(currentUserInfo);
                if (cardCoupon.BindChannelUserId != channelUserId)
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.MallGetCardOnlyChannel;
                    apiResp.msg = "只有指定渠道才能领取";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

                }



            }
            string msg = "";
            if (!bllCardCoupon.IsCanGiveCoupon(fromMyCardCoupon, out msg))//无法转赠给他人
            {
                apiResp.msg = msg;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                MyCardCoupons model = new MyCardCoupons();
                model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllMall.GetGUID(BLLJIMP.TransacType.CommAdd));
                model.CardCouponType = fromMyCardCoupon.CardCouponType;
                model.CardId = fromMyCardCoupon.CardId;
                model.InsertDate = DateTime.Now;
                model.UserId = currentUserInfo.UserID;
                model.WebSiteOwner = currentUserInfo.WebsiteOwner;
                model.FromUserId = fromMyCardCoupon.UserId;
                if (!bllCardCoupon.Add(model))
                {
                    tran.Rollback();
                    apiResp.msg = "接收失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }

                fromMyCardCoupon.Status = 2;
                fromMyCardCoupon.ToUserId = currentUserInfo.UserID;
                if (!bllCardCoupon.Update(fromMyCardCoupon))
                {
                    tran.Rollback();
                    apiResp.msg = "接收失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }

               // CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(fromMyCardCoupon.CardId);
                //if (cardCoupon == null)
                //{
                //    tran.Rollback();
                //    apiResp.msg = "卡券不存在";
                //    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                //}
                UserInfo fromUserInfo = bllUser.GetUserInfo(fromMyCardCoupon.UserId);
                if (fromUserInfo == null)
                {
                    tran.Rollback();
                    apiResp.msg = "赠送用户不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                bllWeixin.SendTemplateMessageNotifyComm(fromUserInfo, "优惠券转赠通知", string.Format(" {0}已领取你转赠的优惠券{1}", bllUser.GetUserDispalyName(currentUserInfo), cardCoupon.Name));
                bllWeixin.SendTemplateMessageNotifyComm(currentUserInfo, "优惠券领取通知", string.Format(" 您已经领取来自{0}转赠的优惠券{1}", bllUser.GetUserDispalyName(fromUserInfo), cardCoupon.Name));
                tran.Commit();
               
                #region 分销关系建立
                BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
                if (bllMenuPermission.CheckUserAndPmsKey(currentUserInfo.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
                {

                    if (string.IsNullOrWhiteSpace(currentUserInfo.DistributionOwner))
                    {
                        
                        WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
                        if (websiteInfo.DistributionRelationBuildMallOrder == 1)
                        {


                            if (bllUser.IsDistributionMember(fromUserInfo) || (fromUserInfo.UserID == websiteInfo.WebsiteOwner))//上级符合分销员标准
                                {
                                    BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();

                                    var setUserDistributionOwnerResult = bllDis.SetUserDistributionOwner(currentUserInfo.UserID, fromUserInfo.UserID, currentUserInfo.WebsiteOwner);
                                

                                }

                            
                        }

                    }

                    


                }
                #endregion


                apiResp.status = true;
                apiResp.msg = "ok";



            }
            catch (Exception ex)
            {

                apiResp.msg = ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        /// <summary>
        /// 卡券核销
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Hexiao(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];//主卡券ID
            string hexiaoCode = context.Request["hexiao_code"];//核销码
            if (string.IsNullOrEmpty(cardCouponId))
            {
                apiResp.msg = "cardcoupon_id 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            if (string.IsNullOrEmpty(hexiaoCode))
            {
                apiResp.msg = "hexiao_code 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(int.Parse(cardCouponId));
            if (cardCoupon == null)
            {
                apiResp.msg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            MyCardCoupons myCardCoupon = bllCardCoupon.Get<MyCardCoupons>(string.Format(" CardId={0} And Status=0 And UserId='{1}'", cardCoupon.CardId, currentUserInfo.UserID));
            if (myCardCoupon == null)
            {
                apiResp.msg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (myCardCoupon.Status == 1)
            {
                apiResp.msg = "卡券已经使用";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            string hexiaoChanel = "";
            bool hexiaoResult = GetHexiaoChannel(hexiaoCode, out hexiaoChanel);
            if (!hexiaoResult)
            {
                apiResp.msg = "核销失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            try
            {
                myCardCoupon.HexiaoCode = hexiaoCode;
                myCardCoupon.HexiaoChannel = hexiaoChanel;
                myCardCoupon.Status = 1;
                myCardCoupon.UseDate = DateTime.Now;
                if (!bllCardCoupon.Update(myCardCoupon))
                {

                    apiResp.msg = "核销失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                else
                {
                    apiResp.status = true;
                    apiResp.msg = "ok";
                }

            }
            catch (Exception ex)
            {

                apiResp.msg = ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        /// <summary>
        /// 获取核销渠道
        /// </summary>
        /// <param name="hexiaoCode"></param>
        /// <param name="chanel"></param>
        /// <returns></returns>
        private bool GetHexiaoChannel(string hexiaoCode, out string chanel)
        {

            chanel = "";
            WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
            if (!string.IsNullOrEmpty(websiteInfo.HexiaoCode) && (hexiaoCode.ToLower() == websiteInfo.HexiaoCode.ToLower()))
            {
                chanel = "系统";
                return true;
            }
            UserInfo userInfo = bllUser.Get<UserInfo>(string.Format(" WebsiteOwner='{0}' And HexiaoCode='{1}'", bllUser.WebsiteOwner, hexiaoCode));
            if (userInfo != null)
            {
                chanel = bllUser.GetUserDispalyName(userInfo);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 我的卡券模型
        /// </summary>
        private class MyCardModel
        {
            /// <summary>
            /// 卡券编号
            /// </summary>
            public int cardcoupon_id { get; set; }
            /// <summary>
            /// 卡券号码
            /// </summary>
            public string cardcoupon_number { get; set; }

            /// <summary>
            /// 主卡编号
            /// </summary>
            public int main_cardcoupon_id { get; set; }
            /// <summary>
            /// 图片地址
            /// </summary>
            public string img_url { get; set; }
            /// <summary>
            ///卡券名称
            /// </summary>
            public string cardcoupon_name { get; set; }
            /// <summary>
            /// 卡券类型
            /// </summary>
            public int cardcoupon_type { get; set; }
            /// <summary>
            /// 是否是储值卡
            /// </summary>
            public int is_store_card { get; set; }
            /// <summary>
            /// 有效期开始
            /// </summary>
            public string valid_from { get; set; }
            /// <summary>
            ///有效期结束
            /// </summary>
            public string valid_to { get; set; }
            /// <summary>
            /// 状态 0 未使用 1已经使用
            /// </summary>
            public int cardcoupon_status { get; set; }

            /// <summary>
            /// 获取日期
            /// </summary>
            public double cardcoupon_gettime { get; set; }
            /// <summary>
            /// 商品ID
            /// </summary>
            public string product_id { get; set; }

            /// <summary>
            /// 有效期开始时间戳
            /// </summary>
            public double valid_from_timestamp { get; set; }
            /// <summary>
            ///有效期结束时间戳
            /// </summary>
            public double valid_to_timestamp { get; set; }
            /// <summary>
            /// 折扣（1-10）(折扣券)cardcoupon_type 等于0时必填
            /// </summary>
            public string discount { get; set; }
            /// <summary>
            /// 抵扣金额(抵扣券)cardcoupon_type 等于1时必填
            /// </summary>
            public string deductible_amount { get; set; }
            /// <summary>
            /// 满多少元包邮(免邮券)cardcoupon_type 等于2时必填
            /// </summary>
            public string freefreight_amount { get; set; }
            /// <summary>
            /// 满多少金额(满扣券)cardcoupon_type 等于3时必填
            /// </summary>
            public string buckle_amount { get; set; }
            /// <summary>
            /// 可减去多少金额（满扣券）cardcoupon_type 等于3时必填
            /// </summary>
            public string buckle_sub_amount { get; set; }
            /// <summary>
            /// 限制类型
            /// 空表示不限制
            /// 0 表示商品ID
            /// 1 表示商品标签
            /// </summary>
            public string limit_type { get; set; }
            /// <summary>
            /// 商品标签
            /// </summary>
            public string product_tags { get; set; }
            /// <summary>
            /// 是否可以赠送给他人
            /// </summary>
            public bool is_can_give { get; set; }
            /// <summary>
            /// 赠送人信息
            /// </summary>
            public UserInfoModel from_user_info { get; set; }
            /// <summary>
            /// 接收人信息
            /// </summary>
            public UserInfoModel to_user_info { get; set; }
            /// <summary>
            /// 核销渠道
            /// </summary>
            public string hexiao_channel { get; set; }

            /// <summary>
            /// 是否可用在普通下单购买
            /// </summary>
            public int is_can_use_shop { get; set; }

            /// <summary>
            /// 算法可用在团购下单购买
            /// </summary>
            public int is_can_use_groupbuy { get; set; }

            /// <summary>
            /// 区分V1 V2
            /// </summary>
            public string ex1 { get; set; }
            /// <summary>
            /// 翼码卡券
            /// </summary>
            public string yima_cardcode { get; set; }


        }
        /// <summary>
        /// 用户信息模型
        /// </summary>
        private class UserInfoModel
        {

            /// <summary>
            /// 头像
            /// </summary>
            public string head_img_url { get; set; }
            /// <summary>
            /// 昵称
            /// </summary>
            public string nick_name { get; set; }

        }
        /// <summary>
        /// 主卡券模型
        /// </summary>
        private class MainCardModel
        {
            /// <summary>
            /// 卡券编号
            /// </summary>
            public int cardcoupon_id { get; set; }
            /// <summary>
            /// 图片地址
            /// </summary>
            public string img_url { get; set; }
            /// <summary>
            ///卡券名称
            /// </summary>
            public string cardcoupon_name { get; set; }
            /// <summary>
            /// 有效期开始
            /// </summary>
            public string valid_from { get; set; }
            /// <summary>
            ///有效期结束
            /// </summary>
            public string valid_to { get; set; }
            /// <summary>
            /// 有效期结束
            /// </summary>
            public double valid_to_timestamp { get; set; }
            /// <summary>
            /// 卡券类型
            ///0折扣券：凭折扣券对指定商品（全场）打折
            ///1抵扣券：支付时可以抵扣现金
            ///2免邮券：满一定金额包邮
            ///3满扣券：消费满一定金额减去一定金额
            /// </summary>
            public double cardcoupon_type { get; set; }
            /// <summary>
            /// 商品ID
            /// </summary>
            public double product_id { get; set; }
            /// <summary>
            /// 折扣（1-10）(折扣券)cardcoupon_type 等于0时必填
            /// </summary>
            public double discount { get; set; }
            /// <summary>
            /// 抵扣金额(抵扣券)cardcoupon_type 等于1时必填
            /// </summary>
            public double deductible_amount { get; set; }
            /// <summary>
            /// 满多少元包邮(免邮券)cardcoupon_type 等于2时必填
            /// </summary>
            public double freefreight_amount { get; set; }
            /// <summary>
            /// 满多少金额(满扣券)cardcoupon_type 等于3时必填
            /// </summary>
            public double buckle_amount { get; set; }
            /// <summary>
            /// 可减去多少金额（满扣券）cardcoupon_type 等于3时必填
            /// </summary>
            public double buckle_sub_amount { get; set; }
            /// <summary>
            /// 是否已经领取 0未领 1已领
            /// </summary>
            public int is_recivece { get; set; }

            /// <summary>
            /// 发放总量
            /// </summary>
            public int max_count { get; set; }
            /// <summary>
            /// 已经发放数量
            /// </summary>
            public int send_count { get; set; }

            /// <summary>
            /// 剩余数量
            /// </summary>
            public int un_send_count { get; set; }
            /// <summary>
            /// 限制类型
            /// 空表示不限制
            /// 0 表示商品ID
            /// 1 表示商品标签
            /// </summary>
            public string limit_type { get; set; }
            /// <summary>
            /// 商品标签
            /// </summary>
            public string product_tags { get; set; }
            /// <summary>
            /// 赠送人信息
            /// </summary>
            public UserInfoModel from_user_info { get; set; }
            /// <summary>
            /// 是否已经转赠
            /// </summary>
            public bool is_donation { get; set; }
            /// <summary>
            /// GetLimitType 获取限制类型，1为只能分销会员领取
            /// </summary>
            public string user_get_limit_type { get; set; }

            /// <summary>
            /// 是否可用在普通下单购买
            /// </summary>
            public int is_can_use_shop { get; set; }

            /// <summary>
            /// 算法可用在团购下单购买
            /// </summary>
            public int is_can_use_groupbuy { get; set; }
            /// <summary>
            /// 过期类型
            /// </summary>
            public string  expire_time_type{get;set;}
            /// <summary>
            /// 领取后几天过期
            /// </summary>
            public string expire_day { get; set; }
            /// <summary>
            /// 微信卡券id
            /// </summary>
            public string weixin_card_id { get; set; }
            /// <summary>
            /// 优惠券编号
            /// </summary>
            public string cardcoupon_number { get; set; }

        }
        /// <summary>
        /// 卡券类型转换
        /// </summary>
        /// <param name="cardCouponType"></param>
        /// <returns></returns>
        private int ConvertCardCouponType(string cardCouponType)
        {

            switch (cardCouponType)
            {

                case "MallCardCoupon_Discount"://>折扣券：凭折扣券对指定商品（全场）打折
                    return 0;
                case "MallCardCoupon_Deductible"://抵扣券：支付时可以抵扣现金
                    return 1;
                case "MallCardCoupon_FreeFreight"://免邮券：满一定金额包邮
                    return 2;
                case "MallCardCoupon_Buckle"://满扣券：消费满一定金额减去一定金额
                    return 3;
                case "MallCardCoupon_BuckleGive"://满送券
                    return 4;
                default:
                    break;
            }
            return 0;


        }

    }
}