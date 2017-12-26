using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.CardCoupon
{
    /// <summary>
    /// Summary description for GetMainCardCoupon
    /// </summary>
    public class GetMainCardCoupon : BaseHanderOpen
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
            
            string cardCouponId = context.Request["cardcoupon_id"];//主卡券ID
            string myCardCouponId = context.Request["my_cardcoupon_id"];//我的卡券ID
            if (string.IsNullOrEmpty(cardCouponId))
            {
                resp.status = false;
                resp.msg = "cardcoupon_id 参数必传";
                context.Response.Write(JsonConvert.SerializeObject(resp));
                return ;
            }
            CardCoupons cardCoupon = bllCardCoupon.GetCardCouponByWXCardId(cardCouponId);
            if (cardCoupon == null)
            {
                cardCoupon = bllCardCoupon.GetCardCoupon(int.Parse(cardCouponId));
                if (cardCoupon==null)
                {
                    resp.status = false;
                    resp.msg = "cardcoupon_id 不存在";
                    context.Response.Write(JsonConvert.SerializeObject(resp));
                    return;

                }
            }
            cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon);
           
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
            //model.is_recivece = bllCardCoupon.IsReciveCoupon(cardCoupon.CardId, CurrentUserInfo.UserID);
            model.valid_to_timestamp = bllCardCoupon.GetTimeStamp((DateTime)cardCoupon.ValidTo);
            model.limit_type = cardCoupon.Ex7;
            model.product_tags = cardCoupon.Ex8;
            model.user_get_limit_type = cardCoupon.GetLimitType;
            model.is_can_use_shop = cardCoupon.IsCanUseShop;
            model.is_can_use_groupbuy = cardCoupon.IsCanUseGroupbuy;
            model.expire_time_type = cardCoupon.ExpireTimeType;
            model.expire_day = cardCoupon.ExpireDay;
            model.weixin_card_id = cardCoupon.WeixinCardId == null ? "" : cardCoupon.WeixinCardId;
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
                }
            }

            resp.status = true;
            resp.result = model;

            context.Response.Write(JsonConvert.SerializeObject(resp));
            
        }
    }
}