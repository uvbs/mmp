using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 卡券
    /// </summary>
    public class CardCoupon : BaseHandlerNeedLoginAdmin
    {


        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCard = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 模块日志
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 微信卡券
        /// </summary>
        BLLWeixinCard bllWeixinCard = new BLLWeixinCard();
        /// <summary>
        /// 
        /// </summary>
        BLLWebSite bllWebsite = new BLLWebSite();
        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            string cardCouponType = context.Request["cardcoupon_type"];

            if (string.IsNullOrEmpty(cardCouponType))
            {
                cardCouponType = "-1";
            }
            int totalCount = 0;
            var sourceData = bllCard.GetCardCouponList(ConvertCardCouponType(int.Parse(cardCouponType)), pageIndex, pageSize, out totalCount, "");

            var list = from p in sourceData
                       select new
                       {
                           cardcoupon_id = p.CardId,
                           cardcoupon_name = p.Name,
                           cardcoupon_type = ConvertCardCouponType(p.CardCouponType),
                           valid_from = p.ValidFrom.ToString(),
                           valid_to = p.ExpireTimeType == "1" ? string.Format("领取后{0}天", p.ExpireDay) : p.ValidTo.ToString(),
                           img_url = bllCard.GetImgUrl(p.Logo),
                           discount = string.IsNullOrEmpty(p.Ex1) ? 0 : double.Parse(p.Ex1),
                           product_id = string.IsNullOrEmpty(p.Ex2) ? 0 : double.Parse(p.Ex2),
                           deductible_amount = string.IsNullOrEmpty(p.Ex3) ? 0 : double.Parse(p.Ex3),
                           freefreight_amount = string.IsNullOrEmpty(p.Ex4) ? 0 : double.Parse(p.Ex4),
                           buckle_amount = string.IsNullOrEmpty(p.Ex5) ? 0 : double.Parse(p.Ex5),
                           buckle_sub_amount = string.IsNullOrEmpty(p.Ex6) ? 0 : double.Parse(p.Ex6),
                           max_count = p.MaxCount,
                           send_count = p.SendCount,
                           used_count = p.UsedCount,
                           un_use_count = p.UnUseCount,
                           un_send_count = p.UnSendCount,
                           limit_type = p.Ex7,
                           product_tags = p.Ex8,
                           user_get_limit_type = p.GetLimitType,
                           is_can_use_shop = p.IsCanUseShop,
                           is_can_use_groupbuy = p.IsCanUseGroupbuy,
                           is_can_use_groupbuy_member = p.IsCanUseGroupbuyMember,
                           bind_channel_userid = p.BindChannelUserId,
                           expire_time_type = p.ExpireTimeType,
                           expire_day = p.ExpireDay,
                           bind_channel_name = string.IsNullOrEmpty(p.BindChannelUserId) ? "无" : bllUser.GetUserInfo(p.BindChannelUserId).ChannelName,
                           is_subscribe_give = p.IsSubscribeGive,
                           full_give = p.FullGive,
                           weixin_qrcode_url = p.WeixinQrCodeUrl,
                           weixin_card_id = p.WeixinCardId,
                           logo_url = p.Logo,
                           store_ids = p.StoreIds,
                           store_names = GetSotreNames(p.StoreIds),
                           limit_amount = p.LimitAmount,
                           limit_count = p.LimitCount,
                           categorys = p.Categorys,
                           is_pre_price = p.IsPrePrice
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = list,
                totalcount = totalCount,
                list = list

            });

        }
        /// <summary>
        /// 门店名称
        /// </summary>
        /// <param name="storeIds"></param>
        /// <returns></returns>
        private string GetSotreNames(string storeIds)
        {
            string storeName = "";
            if (!string.IsNullOrEmpty(storeIds))
            {
                foreach (var item in storeIds.Split(','))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        JuActivityInfo model = bllCard.Get<JuActivityInfo>(string.Format("JuActivityID={0}", item));
                        if (!string.IsNullOrEmpty(model.ActivityName))
                        {
                            storeName += string.Format("{0},", model.ActivityName);
                        }
                    }
                }
            }
            return storeName.TrimEnd(',');


        }

        /// <summary>
        /// 添加卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            string data = context.Request["data"];
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception)
            {
                resp.errcode = -1;
                resp.errmsg = "格式错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            #region 检查
            //检查
            if (string.IsNullOrEmpty(requestModel.cardcoupon_name))
            {
                resp.errcode = 1;
                resp.errmsg = "卡券名称必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.cardcoupon_name.Length > 9)
            {
                resp.errcode = 1;
                resp.errmsg = "卡券名称在9个字以内";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.expire_time_type))
            {
                if (requestModel.valid_from <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "生效开始时间必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (requestModel.valid_to <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "失效时间必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (requestModel.valid_to <= requestModel.valid_from)
                {
                    resp.errcode = 1;
                    resp.errmsg = "失效时间须晚于开始时间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }

            if (requestModel.cardcoupon_type == 0)//折扣券
            {
                if (double.Parse(requestModel.discount) <= 0 || double.Parse(requestModel.discount) >= 10)
                {
                    resp.errcode = 1;
                    resp.errmsg = "折扣券折扣在1-10之间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 1)//抵扣券
            {
                if (double.Parse(requestModel.deductible_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "抵扣券抵扣金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 2)//免邮券
            {
                if (double.Parse(requestModel.freefreight_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "免邮券金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 3)//满扣券
            {
                if (double.Parse(requestModel.buckle_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "满扣券满多少金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (double.Parse(requestModel.buckle_sub_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "满扣券减多少金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.product_id > 0 && (!string.IsNullOrEmpty(requestModel.product_tags)))
            {
                resp.errcode = 1;
                resp.errmsg = "商品ID和商品标签不能同时添加";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllCard.WebsiteOwner == "hailan" || bllCard.WebsiteOwner == "hailandev")
            {
                if (Convert.ToDecimal(requestModel.limit_amount) == 0 && Convert.ToDecimal(requestModel.limit_count) == 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "最小金额和最小订单数量不能同时为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
            }


            //检查 
            #endregion

            CardCoupons model = new CardCoupons();
            model.CardId = int.Parse(bllCard.GetGUID(TransacType.AddCardCoupon));
            model.Name = requestModel.cardcoupon_name;
            model.CardCouponType = ConvertCardCouponType(requestModel.cardcoupon_type);
            model.Ex1 = requestModel.discount.ToString();
            model.Ex2 = requestModel.product_id.ToString();
            model.Ex3 = requestModel.deductible_amount.ToString();
            model.Ex4 = requestModel.freefreight_amount.ToString();
            model.Ex5 = requestModel.buckle_amount.ToString();
            model.Ex6 = requestModel.buckle_sub_amount.ToString();
            model.Ex7 = requestModel.limit_type;
            model.Ex8 = requestModel.product_tags;

            model.InsertDate = DateTime.Now;
            model.WebSiteOwner = bllCard.WebsiteOwner;
            model.CreateUserId = currentUserInfo.UserID;
            model.GetLimitType = requestModel.user_get_limit_type;
            model.IsCanUseShop = requestModel.is_can_use_shop;
            model.IsCanUseGroupbuy = requestModel.is_can_use_groupbuy;
            model.IsCanUseGroupbuyMember = requestModel.is_can_use_groupbuy_member;
            model.BindChannelUserId = requestModel.bind_channel_userid;
            model.ExpireTimeType = requestModel.expire_time_type;
            model.ExpireDay = requestModel.expire_day;
            model.IsSubscribeGive = requestModel.is_subscribe_give;
            model.FullGive = requestModel.full_give;
            model.Logo = requestModel.logo_url;
            model.StoreIds = requestModel.store_ids;
            model.LimitAmount = Convert.ToDecimal(requestModel.limit_amount);
            model.LimitCount = Convert.ToInt32(requestModel.limit_count);
            model.Categorys = requestModel.categorys;
            model.IsPrePrice = Convert.ToInt32(requestModel.is_pre_price);
            if (!string.IsNullOrEmpty(requestModel.max_count))
            {
                model.MaxCount = int.Parse(requestModel.max_count);
            }
            //if (requestModel.valid_from > 0)
            //{
            model.ValidFrom = bllCard.GetTime(requestModel.valid_from);
            //}
            //if (requestModel.valid_to > 0)
            //{
            model.ValidTo = bllCard.GetTime(requestModel.valid_to);
            //}

            #region 生成翼码卡券
            Open.HongWareSDK.Client client = new Open.HongWareSDK.Client(bllCard.WebsiteOwner);
            Open.HongWareSDK.Entity.YimaCard yimaCard = new Open.HongWareSDK.Entity.YimaCard();

            yimaCard.transaction_id =DateTime.Now.ToString("yyyyMMddHHmmss");

            int randSec = new Random().Next(0, 60);
            yimaCard.transaction_id_makecard = string.Format("{0}{1}", client.SystemId, DateTime.Now.AddMinutes(randSec).ToString("yyyyMMddHHmm"));
            yimaCard.activity_name = model.Name;
            yimaCard.activity_short_name = model.Name;
            yimaCard.begin_time = Convert.ToDateTime(model.ValidFrom).ToString("yyyyMMddHHmmss");
            yimaCard.end_time = Convert.ToDateTime(model.ValidTo).ToString("yyyyMMddHHmmss");
            yimaCard.card_type = "1";
            if (!string.IsNullOrEmpty(model.Categorys))
            {
                yimaCard.card_type = "2";
                yimaCard.codes = model.Categorys;
            }
            yimaCard.store_list = model.StoreIds;
            if (model.LimitAmount > 0)
            {
                yimaCard.use_type = "1";
                yimaCard.use_content = (model.LimitAmount * 100).ToString("F0");
            }
            if (model.LimitCount > 0)
            {
                yimaCard.use_type = "2";
                yimaCard.use_content = model.LimitCount.ToString();
            }
            yimaCard.amt_flag = model.IsPrePrice.ToString();
            yimaCard.single_flag = "0";
            yimaCard.channel_type = "1";
            yimaCard.discount_amt = "0";
            switch (model.CardCouponType)
            {
                case "MallCardCoupon_Discount"://折扣券 无用
                    break;
                case "MallCardCoupon_Deductible"://抵扣券现金券
                    yimaCard.discount_amt = (decimal.Parse(model.Ex3) * 100).ToString("F0");
                    break;
                case "MallCardCoupon_FreeFreight"://免邮券 无用
                    break;
                case "MallCardCoupon_Buckle"://满减券
                    yimaCard.discount_amt = (decimal.Parse(model.Ex6) * 100).ToString("F0");
                    break;
                case "MallCardCoupon_BuckleGive":// 满送券
                    yimaCard.discount_amt = (decimal.Parse(model.Ex6) * 100).ToString("F0");
                    break;
                default:
                    break;
            }
            yimaCard.count = model.MaxCount.ToString();
            if (model.MaxCount==0)
            {
                yimaCard.count = int.MaxValue.ToString();
            }
            string msg = "";
            string yimaActivityId = "";
            if (!client.CreateYimaCard(yimaCard, out msg, out yimaActivityId))
            {
                resp.errcode = 1;
                resp.errmsg = "生成翼码卡券失败" + msg;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                model.YimaActivityId = yimaActivityId;
                model.YimaMakeCardTransId = yimaCard.transaction_id_makecard;
            }
            #endregion

            //#region 生成微信卡券
            //ZentCloud.BLLJIMP.Model.Weixin.WeixinCard weixinCard = new BLLJIMP.Model.Weixin.WeixinCard();
            //weixinCard.CardType = "GENERAL_COUPON";
            //weixinCard.CardName = model.Name;

            //#region Logo
            //weixinCard.LogoUrl = "";
            //CompanyWebsite_Config comConfig = bllWebsite.GetCompanyWebsiteConfig();
            //if (comConfig != null)
            //{
            //    if (!string.IsNullOrEmpty(comConfig.DistributionQRCodeIcon))
            //    {
            //        weixinCard.LogoUrl = comConfig.DistributionQRCodeIcon;

            //    }

            //}
            //if (string.IsNullOrEmpty(weixinCard.LogoUrl))
            //{
            //    weixinCard.LogoUrl = string.Format("http://{0}/logo.png", HttpContext.Current.Request.Url.Host);
            //}


            //#endregion
            //weixinCard.CodeType = "CODE_TYPE_QRCODE";
            //#region 品牌名称
            //weixinCard.BrandName = "";
            //var websiteInfo = bllCard.GetWebsiteInfoModelFromDataBase();
            //weixinCard.BrandName = websiteInfo.WebsiteName;
            //#endregion

            //weixinCard.Title = model.Name;
            //weixinCard.Color = "Color010";
            //weixinCard.Notice = "";
            //weixinCard.Description = "";
            //weixinCard.Quantity = model.MaxCount;
            //if (weixinCard.Quantity == 0)
            //{
            //    weixinCard.Quantity = 100000000;
            //}
            //weixinCard.Type = "DATE_TYPE_FIX_TIME_RANGE";
            //if (model.ExpireTimeType == "1")
            //{
            //    weixinCard.Type = "DATE_TYPE_FIX_TERM";
            //}
            //if (weixinCard.Type == "DATE_TYPE_FIX_TIME_RANGE")
            //{
            //    weixinCard.BeginTimeStamp = (int)(bllWeixinCard.GetTimeStamp((DateTime)model.ValidFrom) / 1000);
            //    weixinCard.EndTimeStamp = (int)(bllWeixinCard.GetTimeStamp((DateTime)model.ValidTo) / 1000);
            //}
            //else if (weixinCard.Type == "DATE_TYPE_FIX_TERM")
            //{
            //    weixinCard.FixedBeginTerm = 0;
            //    weixinCard.FixedTerm = int.Parse(model.ExpireDay);

            //}
            //weixinCard.Source = "";
            //weixinCard.UseLimit = 1;
            //weixinCard.GetLimit = 1;
            //weixinCard.CenterTitle = "立即使用";
            //weixinCard.CenterSubTitle = "";
            //weixinCard.CenterUrl = string.Format("http://{0}/customize/comeoncloud/Index.aspx?key=MallHome", HttpContext.Current.Request.Url.Host);
            //weixinCard.DefaultDetail = model.Name;
            //weixinCard.LocationIdList = new List<int>();
            //weixinCard.CanGiveFriend = true;
            //weixinCard.Image = model.Logo;
            //if (!string.IsNullOrEmpty(model.StoreIds))
            //{
            //    foreach (var item in model.StoreIds.Split(','))
            //    {
            //        if (!string.IsNullOrEmpty(item))
            //        {
            //            JuActivityInfo location = bllCard.Get<JuActivityInfo>(string.Format("JuActivityID={0}", item));
            //            if (location != null && (!string.IsNullOrEmpty(location.K30)))
            //            {
            //                weixinCard.LocationIdList.Add(int.Parse(location.K30));
            //            }

            //        }
            //    }
            //}
            //string weixinCardId = "";
            //if (bllWeixinCard.Create(weixinCard, out weixinCardId))
            //{
            //    model.WeixinCardId = weixinCardId;
            //    model.WeixinQrCodeUrl = bllWeixinCard.CreateQrCode(weixinCardId);

            //}
            //#endregion


            if (bllCard.Add(model))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 更新卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {

            string data = context.Request["data"];
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception)
            {
                resp.errcode = -1;
                resp.errmsg = "格式错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            #region 检查
            //检查
            if (string.IsNullOrEmpty(requestModel.cardcoupon_name))
            {
                resp.errcode = 1;
                resp.errmsg = "卡券名称必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.cardcoupon_name.Length > 9)
            {
                resp.errcode = 1;
                resp.errmsg = "卡券名称在9个字以内";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.expire_time_type))
            {
                if (requestModel.valid_from <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "生效开始时间必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (requestModel.valid_to <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "失效时间必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (requestModel.valid_to <= requestModel.valid_from)
                {
                    resp.errcode = 1;
                    resp.errmsg = "失效时间须晚于开始时间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 0)//折扣券
            {
                if (double.Parse(requestModel.discount) <= 0 || double.Parse(requestModel.discount) >= 10)
                {
                    resp.errcode = 1;
                    resp.errmsg = "折扣券折扣在1-10之间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 1)//抵扣券
            {
                if (double.Parse(requestModel.deductible_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "抵扣券抵扣金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 2)//免邮券
            {
                if (double.Parse(requestModel.freefreight_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "免邮券金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.cardcoupon_type == 3)//满扣券
            {
                if (double.Parse(requestModel.buckle_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "满扣券满多少金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (double.Parse(requestModel.buckle_sub_amount) <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "满扣券减多少金额必填";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (requestModel.product_id > 0 && (!string.IsNullOrEmpty(requestModel.product_tags)))
            {
                resp.errcode = 1;
                resp.errmsg = "商品ID和商品标签不能同时添加";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }



            //检查 
            #endregion

            CardCoupons model = bllCard.GetCardCoupon(requestModel.cardcoupon_id);
            if (model == null)
            {
                resp.errcode = 1;
                resp.errmsg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            model.Name = requestModel.cardcoupon_name;
            model.CardCouponType = ConvertCardCouponType(requestModel.cardcoupon_type);
            model.Ex1 = requestModel.discount.ToString();
            model.Ex2 = requestModel.product_id.ToString();
            model.Ex3 = requestModel.deductible_amount.ToString();
            model.Ex4 = requestModel.freefreight_amount.ToString();
            model.Ex5 = requestModel.buckle_amount.ToString();
            model.Ex6 = requestModel.buckle_sub_amount.ToString();
            model.Ex7 = requestModel.limit_type;
            model.Ex8 = requestModel.product_tags;
            model.GetLimitType = requestModel.user_get_limit_type;
            model.IsCanUseShop = requestModel.is_can_use_shop;
            model.IsCanUseGroupbuy = requestModel.is_can_use_groupbuy;
            model.IsCanUseGroupbuyMember = requestModel.is_can_use_groupbuy_member;
            model.BindChannelUserId = requestModel.bind_channel_userid;
            model.ExpireTimeType = requestModel.expire_time_type;
            model.ExpireDay = requestModel.expire_day;
            model.IsSubscribeGive = requestModel.is_subscribe_give;
            model.FullGive = requestModel.full_give;
            model.Logo = requestModel.logo_url;
            model.StoreIds = requestModel.store_ids;
            model.LimitAmount = Convert.ToDecimal(requestModel.limit_amount);
            model.LimitCount = Convert.ToInt32(requestModel.limit_count);
            model.Categorys = requestModel.categorys;
            model.IsPrePrice = Convert.ToInt32(requestModel.is_pre_price);
            if (!string.IsNullOrEmpty(requestModel.max_count))
            {
                model.MaxCount = int.Parse(requestModel.max_count);
            }
            //if (requestModel.valid_from > 0)
            //{
            model.ValidFrom = bllCard.GetTime(requestModel.valid_from);
            //}
            //if (requestModel.valid_to > 0)
            //{
            model.ValidTo = bllCard.GetTime(requestModel.valid_to);
            //}
            if (bllCard.Update(model))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string cardcouponIds = context.Request["cardcoupon_ids"];
            if (bllCard.Delete(new CardCoupons(), string.Format("CardId in({0})", cardcouponIds)) > 0)
            {
                bllCard.Delete(new MyCardCoupons(), string.Format("CardId in({0})", cardcouponIds));
                resp.errcode = 0;
                resp.errmsg = "ok";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除卡券[id=" + cardcouponIds + "]");
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取卡券详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {

            string cardCouponId = context.Request["cardcoupon_id"];
            if (string.IsNullOrEmpty(cardCouponId))
            {
                resp.errcode = 1;
                resp.errmsg = "卡券ID必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            CardCoupons model = bllCard.GetCardCoupon(int.Parse(cardCouponId));
            if (model == null)
            {
                resp.errcode = 1;
                resp.errmsg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            RequestModel respModel = new RequestModel();
            respModel.cardcoupon_id = model.CardId;
            respModel.cardcoupon_name = model.Name;
            respModel.cardcoupon_type = ConvertCardCouponType(model.CardCouponType);
            respModel.discount = string.IsNullOrEmpty(model.Ex1) ? "0" : model.Ex1;
            respModel.product_id = string.IsNullOrEmpty(model.Ex2) ? 0 : int.Parse(model.Ex2);
            respModel.deductible_amount = string.IsNullOrEmpty(model.Ex3) ? "0" : model.Ex3;
            respModel.freefreight_amount = string.IsNullOrEmpty(model.Ex4) ? "0" : model.Ex4;
            respModel.buckle_amount = string.IsNullOrEmpty(model.Ex5) ? "0" : model.Ex5;
            respModel.buckle_sub_amount = string.IsNullOrEmpty(model.Ex6) ? "0" : model.Ex6;
            respModel.valid_from = (model.ValidFrom == null) ? 0 : (long)bllCard.GetTimeStamp((DateTime)model.ValidFrom);
            respModel.valid_to = (model.ValidTo == null) ? 0 : (long)bllCard.GetTimeStamp((DateTime)model.ValidTo);
            respModel.max_count = model.MaxCount.ToString();
            respModel.limit_type = model.Ex7;
            respModel.product_tags = model.Ex8;
            respModel.user_get_limit_type = model.GetLimitType;
            respModel.is_can_use_shop = model.IsCanUseShop;
            respModel.is_can_use_groupbuy = model.IsCanUseGroupbuy;
            respModel.weixin_qrcode_url = model.WeixinQrCodeUrl;
            return ZentCloud.Common.JSONHelper.ObjectToJson(respModel);

        }
        /// <summary>
        /// 发送优惠券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Send(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];
            string phone = context.Request["phone"];
            string sendType = context.Request["send_type"];
            if (string.IsNullOrEmpty(cardCouponId))
            {
                resp.errcode = 1;
                resp.errmsg = "cardcoupon_id 必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(sendType))
            {
                resp.errcode = 1;
                resp.errmsg = "send_type 必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo userInfo = bllUser.GetUserInfo(phone, bllUser.WebsiteOwner);
            if (userInfo == null)
            {
                userInfo = bllUser.GetUserInfoByPhone(phone);
            }
            if (userInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "用户不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            CardCoupons cardCoupon = bllCard.GetCardCoupon(int.Parse(cardCouponId));
            if (cardCoupon == null)
            {
                resp.errcode = 1;
                resp.errmsg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
            {
                apiResp.msg = "卡券已过期";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            if (cardCoupon.MaxCount > 0)
            {
                if (bllCard.GetCardCouponSendCount(cardCoupon.CardId) >= cardCoupon.MaxCount)
                {
                    apiResp.msg = "卡券已经发放完了";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
            }
            switch (sendType)
            {
                case "0"://个人
                    MyCardCoupons model = new MyCardCoupons();
                    model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(11111, 99999).ToString());
                    model.CardCouponType = cardCoupon.CardCouponType;
                    model.CardId = cardCoupon.CardId;
                    model.InsertDate = DateTime.Now;
                    model.UserId = userInfo.UserID;
                    model.WebSiteOwner = bllCard.WebsiteOwner;
                    if (bllCard.Add(model))
                    {
                        resp.errmsg = "ok";
                        string title = "您收到一张优惠券";
                        string content = string.Format("{0}", cardCoupon.Name);
                        if (cardCoupon.ValidTo.HasValue)
                        {
                            content += string.Format("\\n{0}", ((DateTime)cardCoupon.ValidTo).ToString("yyyy-MM-dd"));
                        }

                        string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                        if (bllUser.WebsiteOwner == "jikuwifi")
                        {
                            redicturl = string.Format("http://{0}/customize/jikuwifi/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                        }
                        //redicturl = HttpUtility.UrlEncode(redicturl);
                        //string url = string.Format("http://{0}/customize/index.aspx?redirectUrl={1}", context.Request.Url.Host, redicturl);
                        bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, redicturl);

                    }
                    break;
                default:
                    resp.errcode = 1;
                    resp.errmsg = "不存在的 send_type";
                    break;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 发送优惠券 通过用户标签发送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SendByUserTags(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];
            string tags = context.Request["tags"];
            if (string.IsNullOrEmpty(cardCouponId))
            {

                apiResp.msg = "cardcoupon_id 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(tags))
            {
                apiResp.msg = "tags 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            CardCoupons cardCoupon = bllCard.GetCardCoupon(int.Parse(cardCouponId));
            if (cardCoupon == null)
            {
                apiResp.msg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            //if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
            //{
            //    apiResp.msg = "卡券已过期";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            //}
            if (cardCoupon.MaxCount > 0)
            {
                if (bllCard.GetCardCouponSendCount(cardCoupon.CardId) >= cardCoupon.MaxCount)
                {
                    apiResp.msg = "卡券已经发放完了";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
            }
            int successCount = 0;
            foreach (var tag in tags.Split(','))
            {
                List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format(" WebSiteOwner='{0}' And TagName like '%{1}%'", bllCard.WebsiteOwner, tag));
                foreach (var userInfo in userList)
                {
                    MyCardCoupons model = new MyCardCoupons();
                    model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllCard.GetGUID(BLLJIMP.TransacType.CommAdd));
                    model.CardCouponType = cardCoupon.CardCouponType;
                    model.CardId = cardCoupon.CardId;
                    model.InsertDate = DateTime.Now;
                    model.UserId = userInfo.UserID;
                    model.WebSiteOwner = bllCard.WebsiteOwner;
                    if (bllCard.Add(model))
                    {
                        successCount++;
                        string title = "您收到一张优惠券";
                        string content = string.Format("{0}", cardCoupon.Name);
                        if (cardCoupon.ValidTo.HasValue)
                        {
                            //content += string.Format("\\n{0}", ((DateTime)cardCoupon.ValidTo).ToString("yyyy-MM-dd"));
                        }
                        string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                        if (bllUser.WebsiteOwner == "jikuwifi")
                        {
                            redicturl = string.Format("http://{0}/customize/jikuwifi/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                        }
                        //redicturl = HttpUtility.UrlEncode(redicturl);
                        //string url = string.Format("http://{0}/customize/index.aspx?redirectUrl={1}", context.Request.Url.Host, redicturl);
                        bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, redicturl);


                    }
                }


            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                success_count = successCount
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }


        /// <summary>
        /// 发送优惠券 通过用户ID发送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SendByUserIds(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];
            string userIds = context.Request["user_ids"];
            if (string.IsNullOrEmpty(cardCouponId))
            {

                apiResp.msg = "cardcoupon_id 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(userIds))
            {
                apiResp.msg = "user_ids 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            CardCoupons cardCoupon = bllCard.GetCardCoupon(int.Parse(cardCouponId));
            if (cardCoupon == null)
            {
                apiResp.msg = "卡券不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            //if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
            //{
            //    apiResp.msg = "卡券已过期";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            //}
            if (cardCoupon.MaxCount > 0)
            {
                if (bllCard.GetCardCouponSendCount(cardCoupon.CardId) >= cardCoupon.MaxCount)
                {
                    apiResp.msg = "卡券已经发放完了";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
            }
            int successCount = 0;
            foreach (var userId in userIds.Split(','))
            {
                UserInfo userInfo = bllUser.GetUserInfo(userId, bllUser.WebsiteOwner);
                MyCardCoupons model = new MyCardCoupons();
                model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllCard.GetGUID(BLLJIMP.TransacType.CommAdd));
                model.CardCouponType = cardCoupon.CardCouponType;
                model.CardId = cardCoupon.CardId;
                model.InsertDate = DateTime.Now;
                model.UserId = userInfo.UserID;
                model.WebSiteOwner = bllCard.WebsiteOwner;
                if (bllCard.Add(model))
                {
                    successCount++;
                    string title = "您收到一张优惠券";
                    string content = string.Format("{0}", cardCoupon.Name);
                    if (cardCoupon.ValidTo.HasValue)
                    {
                        //content += string.Format("\\n{0}",((DateTime)cardCoupon.ValidTo).ToString("yyyy-MM-dd"));
                    }
                    string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                    if (bllUser.WebsiteOwner == "jikuwifi")
                    {
                        redicturl = string.Format("http://{0}/customize/jikuwifi/?v=1.0&ngroute=/mycoupons#/mycoupons", context.Request.Url.Host);
                    }
                    //redicturl = HttpUtility.UrlEncode(redicturl);
                    //string url = string.Format("http://{0}/customize/index.aspx?redirectUrl={1}", context.Request.Url.Host, redicturl);
                    bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, redicturl);

                }
            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                success_count = successCount
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }
        /// <summary>
        /// 获取优惠券发放记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SendRecordList(HttpContext context)
        {
            string cardCouponId = context.Request["cardcoupon_id"];
            if (string.IsNullOrEmpty(cardCouponId))
            {
                resp.errcode = 1;
                resp.errmsg = "cardcoupon_id 必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount = 0;
            var sourceData = bllCard.GetSendRecordList(int.Parse(cardCouponId), pageIndex, pageSize, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           cardcoupon_id = p.CardId,
                           cardcoupon_number = p.CardCouponNumber,
                           cardcoupon_type = ConvertCardCouponType(p.CardCouponType),
                           cardcoupon_send_time = bllCard.GetTimeStamp(p.InsertDate),
                           cardcoupon_use_time = p.UseDate != null ? bllCard.GetTimeStamp((DateTime)p.UseDate) : 0,
                           cardcoupon_status = p.Status,
                           cardcoupon_user_id = p.UserId,
                           cardcoupon_user_truename = bllUser.GetUserInfo(p.UserId) == null ? "" : bllUser.GetUserInfo(p.UserId).TrueName,
                           cardcoupon_user_phone = bllUser.GetUserInfo(p.UserId) == null ? "" : bllUser.GetUserInfo(p.UserId).Phone,
                           hexiao_channel = p.HexiaoChannel,
                           from_user_name = GetUserInfoName(p.FromUserId),
                           to_user_name = GetUserInfoName(p.ToUserId)
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list

            });

        }

        private string GetUserInfoName(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return "";
            }
            UserInfo userInfo = bllUser.GetUserInfo(userId);
            if (userInfo != null)
            {
                return string.Format("({0}{1})", bllUser.GetUserDispalyName(userInfo), userInfo.AutoID);
            }
            return "";
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
                case "MallCardCoupon_Discount"://折扣券
                    return 0;
                case "MallCardCoupon_Deductible"://抵扣券现金券
                    return 1;
                case "MallCardCoupon_FreeFreight"://免邮券
                    return 2;
                case "MallCardCoupon_Buckle"://满减券
                    return 3;
                case "MallCardCoupon_BuckleGive":// 满送券
                    return 4;
                default:
                    break;
            }
            return 0;


        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="cardCouponType"></param>
        /// <returns></returns>
        private string ConvertCardCouponType(int? cardCouponType)
        {

            switch (cardCouponType)
            {
                case 0:
                    return "MallCardCoupon_Discount";//折扣券
                case 1:
                    return "MallCardCoupon_Deductible";//抵扣券
                case 2:
                    return "MallCardCoupon_FreeFreight";//免邮券
                case 3:
                    return "MallCardCoupon_Buckle";//满减券
                case 4:
                    return "MallCardCoupon_BuckleGive";//满送券
                default:
                    break;
            }
            return "";


        }

        /// <summary>
        /// 请求模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 卡券ID
            /// </summary>
            public int cardcoupon_id { get; set; }
            /// <summary>
            /// 卡券名称
            /// </summary>
            public string cardcoupon_name { get; set; }
            /// <summary>
            /// logo
            /// </summary>
            public string logo_url { get; set; }
            /// <summary>
            /// 卡券类型
            ///0折扣券：凭折扣券对指定商品（全场）打折
            ///1抵扣券：支付时可以抵扣现金
            ///2免邮券：满一定金额包邮
            ///3满扣券：消费满一定金额减去一定金额
            ///4 满送券 满一定金额送优惠券
            /// </summary>
            public int cardcoupon_type { get; set; }
            /// <summary>
            /// 商品ID
            /// </summary>
            public int product_id { get; set; }
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
            /// 生效时间戳
            /// </summary>
            public long valid_from { get; set; }
            /// <summary>
            /// 失效时间戳
            /// </summary>
            public long valid_to { get; set; }

            /// <summary>
            /// 最多发放数量
            /// </summary>
            public string max_count { get; set; }

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
            /// 算法可用在参团下单购买
            /// </summary>
            public int is_can_use_groupbuy_member { get; set; }
            /// <summary>
            /// 绑定渠道UserID
            /// </summary>
            public string bind_channel_userid { get; set; }
            /// <summary>
            /// 过期类型
            /// 空 主卡券填的日期
            /// 1 从领取之日算的过期天数
            /// </summary>
            public string expire_time_type { get; set; }
            /// <summary>
            /// 过期类型
            /// 从领取之日算的过期天数 ExpireTimeType为1时有值
            /// </summary>
            public string expire_day { get; set; }
            /// <summary>
            /// 是否关注自动送
            /// </summary>
            public string is_subscribe_give { get; set; }
            /// <summary>
            /// 满多少元自动赠送
            /// </summary>
            public string full_give { get; set; }
            /// <summary>
            /// 微信二维码链接
            /// </summary>
            public string weixin_qrcode_url { get; set; }
            /// <summary>
            /// 门店id
            /// </summary>
            public string store_ids { get; set; }
            /// <summary>
            /// 最小使用金额
            /// </summary>
            public string limit_amount { get; set; }
            /// <summary>
            /// 最小购买数量
            /// </summary>
            public string limit_count { get; set; }
            /// <summary>
            /// 分类
            /// </summary>
            public string categorys { get; set; }
            /// <summary>
            /// 是否原价才能使用
            /// </summary>
            public string is_pre_price { get; set; }




        }

    }
}