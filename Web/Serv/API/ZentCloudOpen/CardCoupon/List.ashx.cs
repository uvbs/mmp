using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.CardCoupon
{
    /// <summary>
    /// Summary description for List
    /// </summary>
    public class List : BaseHanderOpen
    {

        public void ProcessRequest(HttpContext context)
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


            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 20;
            pageSize = int.MaxValue;
            string cardcouponStatus = context.Request["cardcoupon_status"];
            string isCanUse = "0";// context.Request["is_can_use"];//可以正常使用的标识
            string amount = context.Request["amount"];//订单金额
            string isNotShowStoreValue = context.Request["is_notshow_storevalue"];//是否不显示储值卡

            string openId = context.Request["open_id"];

            bool isSuccess = false;
            string msg = "";
            string couponName = "";
            int totalCount = 0;

            var currUser = bllUser.GetUserInfoByOpenId(openId);

            if (currUser == null)
            {
                resp.status = false;
                resp.msg = "openid找不到用户";
                context.Response.Write(JsonConvert.SerializeObject(resp));
                return;
            }

            List<MyCardModel> list = new List<MyCardModel>();

            #region 优惠券
            var sourceData = bllCardCoupon.GetMyCardCoupons(currUser.UserID, pageIndex, pageSize, out totalCount, cardcouponStatus);
            foreach (var item in sourceData)
            {
                BLLJIMP.Model.CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(item.CardId);
                cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon, item);
                MyCardModel model = new MyCardModel();
                model.cardcoupon_id = item.AutoId;
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
                model.weixin_card_id = cardCoupon.WeixinCardId;

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
                                //    if (bllMall.CalcDiscountAmount(item.AutoId.ToString(), decimal.Parse(amount), currUser.UserID, out isSuccess, out msg, out couponName) > 0)
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
                        if (bllMall.CalcDiscountAmount(item.AutoId.ToString(), decimal.Parse(amount), currUser.UserID, out isSuccess, out msg, out couponName) > 0)
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

                var storeValueCardList = bllStoredValue.GetCanUseStoredValueCardList(currUser.UserID);
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

            }

            resp.status = true;

            resp.result = new
            {
                totalcount = list.Count,
                list = list
            };

            context.Response.Write(JsonConvert.SerializeObject(resp));
        }

    }
}