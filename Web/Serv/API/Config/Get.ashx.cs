using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Config
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuper = new BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();

        public void ProcessRequest(HttpContext context)
        {
            //string strPsmIds = "0";
            //List<long> psmIds = bllMenuper.GetUserAllPmsID(bllMenuper.GetCurrentUserInfo().UserID);
            //if (psmIds.Count() > 0) strPsmIds = MyStringHelper.ListToStr(psmIds, "'", ",");
            //List<string> strList = new List<string>();
            //var pmsList = bllMenuper.GetList<PermissionInfo>(string.Format(" PermissionKey>'' AND PermissionID in ({0})", strPsmIds));
            //if (pmsList != null) strList = pmsList.Select(p => p.PermissionKey).ToList();



            var websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            var companyWebsiteConfig=bll.Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'",bll.WebsiteOwner));

            if (companyWebsiteConfig==null)
	        {
		        companyWebsiteConfig=new CompanyWebsite_Config();
	        }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                #region 商城配置
                malll = new
                {
                    is_enable_account_amount_pay = websiteInfo.IsEnableAccountAmountPay == 1 ? true : false,//是否开启余额支付功能
                    account_amount_pay_showname = websiteInfo.AccountAmountPayShowName,//余额支付前端显示名称
                    groubuy_index_url = companyWebsiteConfig == null ? "" : companyWebsiteConfig.GroupBuyIndexUrl,//团购首页链接
                    score_show_name = string.IsNullOrEmpty(websiteInfo.ScorePayShowName) ? "积分" : websiteInfo.ScorePayShowName,
                    cardcoupon_show_name = string.IsNullOrEmpty(websiteInfo.CardCouponShowName) ? "优惠券" : websiteInfo.CardCouponShowName,
                    is_open_group = websiteInfo.IsOpenGroup==1?true:false,//是否允许用户开团
                    is_show_product_sale = websiteInfo.IsShowProductSaleCount==1?true:false,//是否显示商品销量
                    is_show_name_phone=websiteInfo.IsNeedMallOrderCreaterNamePhone==1?true:false,//是否需要姓名手机选项
                    rname=websiteInfo.NeedMallOrderCreaterNamePhoneRName,//自定义名称
                    is_show_stock=websiteInfo.IsShowStock==1?true:false,//是否显示库存
                    is_show_stock_value = websiteInfo.IsShowStockValue,// 库存对比value
                    order_cancel_minute=websiteInfo.OrderCancelMinute,//订单取消时间
                    is_customize_mall_head=websiteInfo.IsCustomizeMallHead,//商城自定义头部
                    customize_mall_head_config=websiteInfo.CustomizeMallHeadConfig,//商城自定义头部配置
                    score_pay_redio=websiteInfo.MallScorePayRatio,//积分支付比例
                    shop_cart_along_settlement=companyWebsiteConfig.ShopCartAlongSettlement,//购物车单独结算
                    is_store_since=companyWebsiteConfig.IsStoreSince,//是否开启门店自提
                    store_since_time=GetStoreSinceTime(companyWebsiteConfig.StoreSinceTimeJson),//门点自提时间段 小时
                    is_home_delivery=companyWebsiteConfig.IsHomeDelivery,//是否送货上门
                    earliest_delivery_time =companyWebsiteConfig.EarliestDeliveryTime,//最早送货时间  下单后几个小时
                    home_delivery_time = GetHomeDeliveryTime(companyWebsiteConfig.HomeDeliveryTimeJson),//送货上门时间段 小时
                    is_auto_assisn_order=companyWebsiteConfig.IsAutoAssignOrder,//是否自动分单
                    is_out_pay=companyWebsiteConfig.IsOutPay,//是否外部第三方支付
                    express_range = companyWebsiteConfig.ExpressRange,//快递发货：同城Y米以外
                    store_express_range = companyWebsiteConfig.StoreExpressRange,//门店自提 多少米以外
                    store_since_discount = companyWebsiteConfig.StoreSinceDiscount//自提优惠
                },
                #endregion

                #region 权限列表配置

                // permission_list = strList

                #endregion

                //is_union_hongware = websiteInfo.IsUnionHongware == 1 ? true : false,//是否绑定宏巍
                //前端无限再跳转到宏巍个人中心，直接返回false
                is_union_hongware = false,

                user_bind_url = websiteInfo.UserBindUrl,//用户绑定URL
                address_select_url = websiteInfo.AddressSelectUrl,//用户收货地址URL
                hongwei_orgcode = GetOrgCode(websiteInfo),
                wx_appid =websiteInfo.WeixinAppId,
                is_claim_mall_order_arrival_time = websiteInfo.IsClaimMallOrderArrivalTime,
                has_wx_pay = bllPay.IsWeixinPay(),
                has_ali_pay = bllPay.IsAliPay(),
                has_jd_pay = bllPay.IsJDPay(),
                mall_order_pay_success_url = websiteInfo.MallOrderPaySuccessUrl,
                login_page_config=websiteInfo.LoginPageConfig,
                is_disable_kefu=companyWebsiteConfig.IsDisableKefu,
                kefu_url=!string.IsNullOrEmpty(companyWebsiteConfig.KefuUrl)?companyWebsiteConfig.KefuUrl:"",
                kefu_image = !string.IsNullOrEmpty(companyWebsiteConfig.KefuImage) ? companyWebsiteConfig.KefuImage : ""
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }
        /// <summary>
        /// 获取OrgCode
        /// </summary>
        /// <param name="websiteInfo"></param>
        /// <returns></returns>
        private string GetOrgCode(WebsiteInfo websiteInfo)
        {

            if (websiteInfo.IsUnionHongware == 1)
            {
                if (!string.IsNullOrEmpty(websiteInfo.OrgCode))
                {
                    return websiteInfo.OrgCode;
                }
                else
                {
                    try
                    {
                        Open.HongWareSDK.Client client = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                        var result = client.GetOrgCode();
                        if (result.isSuccess)
                        {
                            websiteInfo.OrgCode = result.orgCode.orgCode;
                            if (bll.Update(websiteInfo))
                            {
                                return websiteInfo.OrgCode;
                            }

                        }
                    }
                    catch (Exception)
                    {


                    }

                    return "";
                }
            }
            return "";





        }

        /// <summary>
        /// 获取门店自提时间
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private dynamic GetStoreSinceTime(string json) {

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            List<string> list = new List<string>();
            JArray Jarry=JArray.Parse(json);
            DateTime dtNow = DateTime.Now;
            foreach (var item in Jarry)
            {
                //if ((dtNow.Hour >= Convert.ToInt32(item["from"].ToString()) && Convert.ToInt32(item["to"].ToString()) > dtNow.Hour) || Convert.ToInt32(item["from"].ToString()) >= dtNow.Hour)
                //{
                    list.Add(item["from"].ToString() + "-" + item["to"].ToString());
               // }
               
            }

            return list; 
        }
        /// <summary>
        /// 获取送货上门时间
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private dynamic GetHomeDeliveryTime(string json)
        {

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            List<string> list = new List<string>();
            JArray Jarry = JArray.Parse(json);
            DateTime dtNow = DateTime.Now;
            foreach (var item in Jarry)
            {
                //if ((dtNow.Hour >= Convert.ToInt32(item["from"].ToString()) && Convert.ToInt32(item["to"].ToString()) > dtNow.Hour) || Convert.ToInt32(item["from"].ToString()) >= dtNow.Hour)
                //{
                    list.Add(item["from"].ToString() + "-" + item["to"].ToString());
                //}

            }
            return list;
        }
    }
}