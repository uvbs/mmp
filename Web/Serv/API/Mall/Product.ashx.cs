using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;
using Newtonsoft.Json;
using System.Threading;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product : BaseHandler
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// 文章
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLWXMallProduct bllWXMallProduct = new BLLJIMP.BLLWXMallProduct();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        /// <summary>
        /// 商品收藏
        /// </summary>
        Collect collect = new Collect();
        /// <summary>
        /// 获取商品列表 一般商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
          
            int accessLevel = 0;
            int totalCount = 0;
            var sourceData = bllMall.GetProductList(context, out totalCount, accessLevel,false);

            var hasCateName = context.Request["has_cate_name"];
            
            if (string.IsNullOrEmpty(hasCateName))
            {
                hasCateName = "";
            }

            string productIds = context.Request["product_ids"];//商品IDs
            string sortIds = context.Request["sort_ids"];//商品IDs排序
            string canRepeat = context.Request["can_repeat"];//商品IDs重复

            dynamic list;
            if (!string.IsNullOrWhiteSpace(productIds) && sortIds =="1")
            {
                List<dynamic> relist = new List<dynamic>();
                List<string> productIdList = productIds.Split(',').ToList();
                List<string> productInIdList = new List<string>();
                foreach (string productId in productIdList)
                {
                    if (canRepeat != "1" && productInIdList.Contains(productId)) continue;
                    WXMallProductInfo p = sourceData.FirstOrDefault(pi => pi.PID == productId);
                    if (p == null) continue;
                    productInIdList.Add(productId);
                    relist.Add(new
                    {
                        product_id = p.PID,
                        category_id = p.CategoryId,
                        category_name = hasCateName == "1" ? bllMall.GetWXMallCategoryName(p.CategoryId) : "",
                        title = p.PName,
                        summary = p.Summary,
                        access_level = p.AccessLevel,
                        quote_price = p.PreviousPrice,
                        price = bllMall.GetShowPrice(p),
                        score = p.Score,
                        img_url = bllMall.GetImgUrl(p.RecommendImg),
                        is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                        tags = p.Tags,
                        product_code = p.ProductCode,
                        sale_count = p.SaleCount,
                        review_count = p.ReviewCount,
                        totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID)),
                        buy_time = p.LimitBuyTime,
                        is_no_express = p.IsNoExpress,
                        min_price = p.MinPrice,
                        max_price = p.MaxPrice,
                        is_appointment = p.IsAppointment,
                        province=p.Province,
                        city=p.City,
                        district = p.District,
                        ex1 = p.Ex1,
                        group_buy_rule_list = string.IsNullOrWhiteSpace(p.GroupBuyRuleIds) ? null : from r in bllMall.GetProductGroupBuyRuleList(p.PID)
                                                                                                    select new
                                                                                                    {
                                                                                                        rule_id = r.RuleId,
                                                                                                        rule_name = r.RuleName,
                                                                                                        head_discount = r.HeadDiscount,
                                                                                                        head_price = Math.Round((decimal)p.Price * (decimal)(r.HeadDiscount / 10), 2),
                                                                                                        member_discount = r.MemberDiscount,
                                                                                                        member_price = Math.Round((decimal)p.Price * (decimal)(r.MemberDiscount / 10), 2),
                                                                                                        people_count = r.PeopleCount,
                                                                                                        expire_day = r.ExpireDay
                                                                                                    }
                    });

                }
                list = relist;
                totalCount = relist.Count;
            }
            else
            {
                list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           category_id = p.CategoryId,
                           category_name = hasCateName == "1" ? bllMall.GetWXMallCategoryName(p.CategoryId) : "",
                           title = p.PName,
                           summary = p.Summary,
                           access_level = p.AccessLevel,
                           quote_price = p.PreviousPrice,
                           price = bllMall.GetShowPrice(p),
                           score = p.Score,
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           tags = p.Tags,
                           product_code = p.ProductCode,
                           sale_count = p.SaleCount,
                           review_count = p.ReviewCount,
                           totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID)),
                           buy_time = p.LimitBuyTime,
                           is_no_express = p.IsNoExpress,
                           min_price = p.MinPrice,
                           max_price = p.MaxPrice,
                           is_appointment = p.IsAppointment,
                           province = p.Province,
                           city = p.City,
                           district = p.District,
                           ex1 = p.Ex1,
                           group_buy_rule_list = string.IsNullOrWhiteSpace(p.GroupBuyRuleIds) ? null : from r in bllMall.GetProductGroupBuyRuleList(p.PID)
                                                                                                       select new
                                                                                                       {
                                                                                                           rule_id = r.RuleId,
                                                                                                           rule_name = r.RuleName,
                                                                                                           head_discount = r.HeadDiscount,
                                                                                                           head_price = Math.Round((decimal)p.Price * (decimal)(r.HeadDiscount / 10), 2),
                                                                                                           member_discount = r.MemberDiscount,
                                                                                                           member_price = Math.Round((decimal)p.Price * (decimal)(r.MemberDiscount / 10), 2),
                                                                                                           people_count = r.PeopleCount,
                                                                                                           expire_day = r.ExpireDay
                                                                                                       }

                       };
            }

            var data = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            //return ZentCloud.Common.JSONHelper.ObjectToJson(data);
            return JsonConvert.SerializeObject(data);
        }


        /// <summary>
        /// 获取商品列表包含SKU信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ListSku(HttpContext context)
        {

            int totalCount = 0;
            var sourceData = bllMall.GetProductList(context, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           category_id = p.CategoryId,
                           title = p.PName,
                           quote_price = p.PreviousPrice,
                           price = p.Price,
                           base_price = p.BasePrice,
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           tags = p.Tags,
                           product_code = p.ProductCode,
                           skus = GetSkuList(int.Parse(p.PID)),
                       };

            var data = new
            {
                totalcount = totalCount,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        ///// <summary>
        ///// 获取限时特卖商品列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string PromotionList(HttpContext context)
        //{
        //    int totalCount = 0;
        //    var sourceData = bllMall.GetPromotionProductList(context, out totalCount);
        //    var list = from p in sourceData
        //               select new
        //               {
        //                   product_id = p.PID,
        //                   category_id = p.CategoryId,
        //                   title = p.PName,
        //                   quote_price = p.PreviousPrice,
        //                   price = p.PromotionPrice,
        //                   img_url = bllMall.GetImgUrl(p.RecommendImg),
        //                   is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
        //                   count = p.PromotionStock

        //               };

        //    var data = new
        //    {
        //        totalcount = totalCount,
        //        list = list,//列表

        //    };
        //    return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        //}

        ///// <summary>
        ///// 限时特卖日期列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string PromotinDateList(HttpContext context)
        //{
        //    var sourceData = bllMall.GetPromotionDateList();
        //    var list = from p in sourceData
        //               select new
        //               {
        //                  date=p,
        //                  weekday=bllMall.CaculateWeekDay(DateTime.Parse(p))
        //               };

        //    var data = new
        //    {
        //        totalcount = sourceData.Count,
        //        list = list,//列表

        //    };
        //    return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        //}

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            //bllMall.ToLog("进入get",@"d:\songhedev.txt");

            try
            {
                
                string productId = context.Request["product_id"];
                string supplierId=context.Request["supplier_id"];//供应商Id
                //bllMall.ToLog("productId:" + productId, @"d:\songhedev.txt");

                var productInfo = bllMall.GetProduct(productId);

                if (productInfo == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "商品ID不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                if (productInfo.MinPrice == null && productInfo.WebsiteOwner == "jikuwifi")
                {
                    bllWXMallProduct.UpdateProductPriceRange(productInfo.PID, bllWXMallProduct.WebsiteOwner);
                    productInfo = bllMall.GetProduct(productId, true);
                }

                if (productInfo.WebsiteOwner != bllMall.WebsiteOwner)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无权访问";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                //bllMall.ToLog("GetCurrentUserInfo", @"d:\songhedev.txt");

                UserInfo curUser = bllUser.GetUserInfoByCache(bllUser.GetCurrUserID(),bllUser.WebsiteOwner);//bllMall.GetCurrentUserInfo();

                //bllMall.ToLog("GetCurrentUserInfo end", @"d:\songhedev.txt");

                decimal currUserRebateScoreRate = 0;

                currUserRebateScoreRate = bllScore.GetUserRebateScoreRate(curUser != null ? curUser.UserID : null, bllMall.WebsiteOwner);



                #region 检查用户访问权限
                int userAccessLevel = curUser == null ? 0 : curUser.AccessLevel;
                if (productInfo.AccessLevel > 0 && userAccessLevel < productInfo.AccessLevel)
                { 
                    dynamic nresp = new
                    {
                        errcode = 1,
                        errmsg = "您的访问权限不足",
                        access_level = productInfo.AccessLevel
                    };
                    return ZentCloud.Common.JSONHelper.ObjectToJson(nresp);
                }
                #endregion
                var productSkuList = bllMall.GetProductSkuList(int.Parse(productId),true,supplierId);//源SKU 
                var skus = from p in productSkuList
                           select new
                           {
                               sku_id = p.SkuId,
                               properties = bllMall.GetProductProperties(p.SkuId),
                           show_properties = bllMall.GetProductShowProperties(p.SkuId),
                               price =
                                   productInfo.Score > 0 ?
                                   productInfo.Price : bllMall.GetSkuPrice(p),
                               count = bllMall.IsPromotionTime(p) ? p.PromotionStock : p.Stock,
                               score = productInfo.Score,
                               is_cashpay_only = productInfo.IsCashPayOnly,
                               rebate_score = bllScore.CalcProductSkuRebateScore(p.SkuId, currUserRebateScoreRate, null, 1, true),
                               sku_img = !string.IsNullOrEmpty(p.SkuImg) ? p.SkuImg : ""
                           };

                #region 展示图片列表
                List<string> showImgList = new List<string>();
                if (!string.IsNullOrEmpty(productInfo.ShowImage))
                {

                    foreach (var item in productInfo.ShowImage.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            showImgList.Add(bllMall.GetImgUrl(item));
                        }


                    }



                }
                if (showImgList.Count == 0)
                {
                    showImgList.Add(bllMall.GetImgUrl(productInfo.RecommendImg));
                }
                #endregion
                #region 文章内容
                string exContent1 = string.Empty;
                string exContent2 = string.Empty;
                string exContent3 = string.Empty;
                string exContent4 = string.Empty;
                string exContent5 = string.Empty;
                if (!string.IsNullOrEmpty(productInfo.ExArticleId_1))
                {
                    JuActivityInfo model1 = bllJuActivity.GetJuActivity(int.Parse(productInfo.ExArticleId_1), true, bllMall.WebsiteOwner);
                    if (model1 != null)
                    {
                        exContent1 = model1.ActivityDescription;
                    }
                }
                if (!string.IsNullOrEmpty(productInfo.ExArticleId_2))
                {
                    JuActivityInfo model2 = bllJuActivity.GetJuActivity(int.Parse(productInfo.ExArticleId_2), true, bllMall.WebsiteOwner);
                    if (model2 != null)
                    {
                        exContent2 = model2.ActivityDescription;
                    }
                }
                if (!string.IsNullOrEmpty(productInfo.ExArticleId_3))
                {
                    JuActivityInfo model3 = bllJuActivity.GetJuActivity(int.Parse(productInfo.ExArticleId_3), true, bllMall.WebsiteOwner);
                    if (model3 != null)
                    {
                        exContent3 = model3.ActivityDescription;
                    }
                }
                if (!string.IsNullOrEmpty(productInfo.ExArticleId_4))
                {
                    JuActivityInfo model4 = bllJuActivity.GetJuActivity(int.Parse(productInfo.ExArticleId_4), true, bllMall.WebsiteOwner);
                    if (model4 != null)
                    {
                        exContent4 = model4.ActivityDescription;
                    }
                }
                if (!string.IsNullOrEmpty(productInfo.ExArticleId_5))
                {
                    JuActivityInfo model5 = bllJuActivity.GetJuActivity(int.Parse(productInfo.ExArticleId_5), true, bllMall.WebsiteOwner);
                    if (model5 != null)
                    {
                        exContent5 = model5.ActivityDescription;
                    }
                }
                #endregion

                #region 价格配置
                var priceConfigListSource = bllMall.GetList<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}' ", bllMall.WebsiteOwner, productId));
                priceConfigListSource = priceConfigListSource.OrderBy(p => p.Date).ToList();
                var priceConfigList = from p in priceConfigListSource
                                      select new
                                      {

                                          id = p.AutoId,
                                          date = p.Date,
                                          price = p.Price

                                      };
                #endregion

                var storeInfo = new JuActivityInfo();
                if (!string.IsNullOrEmpty(supplierId))
                {

                     storeInfo = bllMall.Get<JuActivityInfo>(string.Format(" K5='{0}'",supplierId));
                    if (storeInfo==null)
                    {
                        storeInfo = new JuActivityInfo();
                    }

                }
                dynamic data = null;
                data = new
                {
                    product_id = productInfo.PID,
                    category_id = productInfo.CategoryId,
                    category_name = bllMall.GetWXMallCategoryName(productInfo.CategoryId),
                    title = productInfo.PName,
                    product_summary = productInfo.Summary,
                    quote_price = productInfo.PreviousPrice,
                    price = bllMall.GetShowPrice(productInfo),
                    base_price = productInfo.BasePrice,
                    score = productInfo.Score,
                    is_cashpay_only = productInfo.IsCashPayOnly,
                    img_url = bllMall.GetImgUrl(productInfo.RecommendImg),
                    is_onsale = (!string.IsNullOrEmpty(productInfo.IsOnSale) && productInfo.IsOnSale == "1") ? 1 : 0,
                    tags = productInfo.Tags,
                    is_collect = IsCollectProduct(int.Parse(productInfo.PID), productInfo.ArticleCategoryType),
                    show_img_list = showImgList,
                    is_promotion_product = productInfo.IsPromotionProduct,
                    promotion_price = bllMall.GetMinPrommotionPrice(int.Parse(productInfo.PID)),
                    promotion_start_time = productInfo.PromotionStartTime,
                    promotion_stop_time = productInfo.PromotionStopTime,
                    promotion_count = bllMall.GetProductTotalPromotionSaleStock(int.Parse(productInfo.PID)),
                    promotion_stock = bllMall.GetProductTotalStockPromotion(int.Parse(productInfo.PID)),
                    skus = skus,
                    product_desc = productInfo.PDescription,
                    pre_product_id = bllMall.GetPreProductId(int.Parse(productInfo.PID)),
                    next_product_id = bllMall.GetNextProductId(int.Parse(productInfo.PID)),
                    product_code = productInfo.ProductCode,
                    current_product_index = bllMall.GetCurrentProductIndex(int.Parse(productInfo.PID)),
                    total_product_count = bllMall.GetTotalProductCount(),
                    sale_count = productInfo.SaleCount,
                    relation_product_id = productInfo.RelationProductId,
                    totalcount = bllMall.GetProductTotalStock(int.Parse(productInfo.PID)),
                    group_buy_rule_list = from r in bllMall.GetProductGroupBuyRuleList(productInfo.PID)
                                          select new
                                          {
                                              rule_id = r.RuleId,
                                              rule_name = r.RuleName,
                                              head_discount = r.HeadDiscount,
                                              head_price = Math.Round((decimal)productInfo.Price * (decimal)(r.HeadDiscount / 10), 2),
                                              member_discount = r.MemberDiscount,
                                              member_price = Math.Round((decimal)productInfo.Price * (decimal)(r.MemberDiscount / 10), 2),
                                              people_count = r.PeopleCount,
                                              expire_day = r.ExpireDay
                                          },
                    collect_count = collect.GetProductCollectCount(productInfo.PID, "ProductCollect"),
                    ex_article_title_1 = productInfo.ExArticleTitle_1,
                    ex_article_title_2 = productInfo.ExArticleTitle_2,
                    ex_article_title_3 = productInfo.ExArticleTitle_3,
                    ex_article_title_4 = productInfo.ExArticleTitle_4,
                    ex_article_title_5 = productInfo.ExArticleTitle_5,
                    ex_article_id_1 = productInfo.ExArticleId_1,
                    ex_article_id_2 = productInfo.ExArticleId_2,
                    ex_article_id_3 = productInfo.ExArticleId_3,
                    ex_article_id_4 = productInfo.ExArticleId_4,
                    ex_article_id_5 = productInfo.ExArticleId_5,
                    ex_content_1 = exContent1,
                    ex_content_2 = exContent2,
                    ex_content_3 = exContent3,
                    ex_content_4 = exContent4,
                    ex_content_5 = exContent5,
                    access_level = productInfo.AccessLevel,
                    buy_time = productInfo.LimitBuyTime,
                    price_config_list = priceConfigList,
                    relevant_product_ids = productInfo.RelevantProductIds,
                    relevant_product = GetRelevantProduct(productInfo),
                    is_no_express = productInfo.IsNoExpress,
                    min_price = productInfo.MinPrice,
                    max_price = productInfo.MaxPrice,
                    review_score = productInfo.ReviewScore,
                    rebate_score = bllScore.CalcProductRebateScore(productInfo, currUserRebateScoreRate, 1, true),
                    is_appointment = productInfo.IsAppointment,
                    appointment_info = bllMall.GetProductAppointmentInfo(productInfo),//预购信息
                    address = productInfo.Address,
                    //property_data=propData//规格信息
                    ex2 = productInfo.Ex2,
                    ex3 = productInfo.Ex3,
                    ex4 = productInfo.Ex4,
                    ex5 = productInfo.Ex5,
                    ex6 = productInfo.Ex6,
                    ex7 = productInfo.Ex7,
                    ex8 = productInfo.Ex8,
                    ex9 = productInfo.Ex9,
                    ex10 = productInfo.Ex10,
                    ex11 = productInfo.Ex11,
                    ex12 = productInfo.Ex12,
                    ex13 = productInfo.Ex13,
                    ex14 = productInfo.Ex14,
                    ex15 = productInfo.Ex15,
                    ex16 = productInfo.Ex16,
                    ex17 = productInfo.Ex17,
                    ex18 = productInfo.Ex18,
                    ex19 = productInfo.Ex19,
                    supplier_name=storeInfo.ActivityName

                };
                
                #region 访问记录统计
                
                ShopDetailOpenStatisticsThread sParms = new ShopDetailOpenStatisticsThread()
                {
                    HttpContextCurrent = HttpContext.Current,
                    WebsiteOwner = bllMall.WebsiteOwner,
                    ProductId = int.Parse(productId),
                    EventUserID = bllMall.IsLogin? bllMall.GetCurrUserID():""
                };

                Thread t = new Thread(new ThreadStart(sParms.ShopDetailOpenStatistics));
                t.Start();
                
                #endregion
                
                return ZentCloud.Common.JSONHelper.ObjectToJson(data);
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
        }

        /// <summary>
        /// 访问统计记录线程处理
        /// </summary>
        public class ShopDetailOpenStatisticsThread
        {
            /// <summary>
            /// 
            /// </summary>
            public int ProductId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string EventUserID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public dynamic HttpContextCurrent { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string WebsiteOwner { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public void ShopDetailOpenStatistics()
            {

                MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();

                detailInfo.EventBrowser = HttpContextCurrent.Request.Browser == null ? "" : HttpContextCurrent.Request.Browser.ToString();
                detailInfo.EventBrowserID = HttpContextCurrent.Request.Browser.Id;
                if (HttpContextCurrent.Request.Browser.Beta)
                {
                    detailInfo.EventBrowserIsBata = "测试版";
                }
                else
                {
                    detailInfo.EventBrowserIsBata = "正式版";
                }

                detailInfo.EventBrowserVersion = HttpContextCurrent.Request.Browser.Version;
                detailInfo.EventDate = DateTime.Now;
                if (HttpContextCurrent.Request.Browser.Win16)
                    detailInfo.EventSysByte = "16位系统";
                else
                    if (HttpContextCurrent.Request.Browser.Win32)
                    detailInfo.EventSysByte = "32位系统";
                else
                    detailInfo.EventSysByte = "64位系统";

                detailInfo.EventSysPlatform = HttpContextCurrent.Request.Browser.Platform;
                detailInfo.SourceIP = ZentCloud.Common.MySpider.GetClientIP(HttpContextCurrent);
                detailInfo.IPLocation = ZentCloud.Common.MySpider.GetIPLocation(detailInfo.SourceIP, HttpContextCurrent);
                detailInfo.SourceUrl = HttpContextCurrent.Request.Url.ToString();
                detailInfo.RequesSourcetUrl = HttpContextCurrent.Request.UrlReferrer != null ? HttpContextCurrent.Request.UrlReferrer.ToString() : "";
                
                //统计转移到消息队列处理
                BLLJIMP.BLLMQ bllMq = new BLLJIMP.BLLMQ();

                var msgBody = new BLLJIMP.Model.MQ.ShopDetailOpenStatistics()
                {
                    ProductId = ProductId,
                    EventBrowser = detailInfo.EventBrowser,
                    EventBrowserID = detailInfo.EventBrowserID,
                    EventBrowserIsBata = detailInfo.EventBrowserIsBata,
                    EventBrowserVersion = detailInfo.EventBrowserVersion,
                    EventSysPlatform = detailInfo.EventSysPlatform,
                    EventUserID = EventUserID,
                    SourceIP = detailInfo.SourceIP,
                    RequesSourcetUrl = detailInfo.RequesSourcetUrl,
                    SourceUrl = detailInfo.SourceUrl,
                    SpreadUserId = detailInfo.SpreadUserID,
                    IPLocation = detailInfo.IPLocation,
                    EventSysByte = detailInfo.EventSysByte,
                    EventType = 0,
                    ModuleType = "product",
                    WebsiteOwner = WebsiteOwner,
                    EventDate = detailInfo.EventDate.Value
                };

                var mq = new BLLJIMP.Model.MQ.MessageInfo()
                {
                    Msg = JsonConvert.SerializeObject(msgBody),
                    MsgId = Guid.NewGuid().ToString(),
                    MsgType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.MQType.ShopDetailOpenStatistics),
                    WebsiteOwner = detailInfo.WebsiteOwner
                };

                bllMq.Publish(mq);
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetBaseInfo(HttpContext context)
        {
            string productId = context.Request["product_id"];
            var productInfo = bllMall.GetProduct(productId);
            if (productInfo == null)
            {
                resp.isSuccess = false;
                resp.errcode = 1;
                resp.errmsg = "商品ID不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            
            var data = new
            {
                product_id = productInfo.PID,
                category_id = productInfo.CategoryId,
                title = productInfo.PName,
                product_summary = productInfo.Summary,
                quote_price = productInfo.PreviousPrice,
                price = productInfo.Price,
                base_price = productInfo.BasePrice,
                score = productInfo.Score,
                is_cashpay_only = productInfo.IsCashPayOnly,
                is_onsale = (!string.IsNullOrEmpty(productInfo.IsOnSale) && productInfo.IsOnSale == "1") ? 1 : 0,
                tags = productInfo.Tags,
                is_promotion_product = productInfo.IsPromotionProduct,
                promotion_start_time = productInfo.PromotionStartTime,
                promotion_stop_time = productInfo.PromotionStopTime,
                product_desc = productInfo.PDescription,
                product_code = productInfo.ProductCode,
                sale_count = productInfo.SaleCount,
                relation_product_id = productInfo.RelationProductId,
                ex_article_title_1 = productInfo.ExArticleTitle_1,
                ex_article_title_2 = productInfo.ExArticleTitle_2,
                ex_article_title_3 = productInfo.ExArticleTitle_3,
                ex_article_title_4 = productInfo.ExArticleTitle_4,
                ex_article_title_5 = productInfo.ExArticleTitle_5,
                ex_article_id_1 = productInfo.ExArticleId_1,
                ex_article_id_2 = productInfo.ExArticleId_2,
                ex_article_id_3 = productInfo.ExArticleId_3,
                ex_article_id_4 = productInfo.ExArticleId_4,
                ex_article_id_5 = productInfo.ExArticleId_5,
                access_level = productInfo.AccessLevel,
                buy_time = productInfo.LimitBuyTime,
                relevant_product_ids = productInfo.RelevantProductIds,
                is_no_express = productInfo.IsNoExpress,
                min_price = productInfo.MinPrice,
                max_price = productInfo.MaxPrice,
                review_score = productInfo.ReviewScore,
                is_need_name_phone = productInfo.IsNeedNamePhone,
                supplier_name=!string.IsNullOrEmpty(productInfo.SupplierUserId)?(bllMall.GetSuppLierByUserId(productInfo.SupplierUserId,productInfo.WebsiteOwner)).Company:""
            };

            resp.isSuccess = true;
            resp.returnObj = data;
            
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        /// 是否已经收藏商品
        /// </summary>
        /// <returns></returns>
        private int IsCollectProduct(int productId,string type)
        {

            if (!bllMall.IsLogin)
            {
                return 0;
            }
            UserInfo currentUserInfo = bllMall.GetCurrentUserInfo();

            if (currentUserInfo == null)
            {
                return 0;
            }
            string strType = "ProductCollect";
            if (type == "Houses") strType = "HouseCollect";
            string strWhere = string.Format("MainId='{0}' And RelationId='{1}' And RelationType='{2}'", currentUserInfo.UserID, productId, strType);
            int totalCount = bllMall.GetCount<CommRelationInfo>(strWhere);
            if (totalCount >=1)
            {
                return 1;
            }
            return 0;


        }

        /// <summary>
        /// 获取前台展示SKU列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private object GetSkuList(int productId) {


            var productSkuList = bllMall.GetProductSkuList(productId);//源SKU 
            var skus = from p in productSkuList
                       select new
                       {
                           sku_id = p.SkuId,
                           properties = bllMall.GetProductProperties(p.SkuId),
                           price = p.Price,
                           base_price = p.BasePrice,
                           count =p.Stock,
                           sku_sn=p.SkuSN
                       };
            return skus;
        
        
        }

        /// <summary>
        /// 获取商品规格
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Property(HttpContext context)
        {

            string productId = context.Request["product_id"];
            //1:1:尺码:XS;2:2:颜色:红色
            var skuList = bllMall.GetProductSkuList(int.Parse(productId));
            List<string> propIdList = new List<string>();//获取商品拥有的所有属性ID集合
            Dictionary<int, string> propKeyValue = new Dictionary<int, string>();
            foreach (var sku in skuList)
            {

                if (!string.IsNullOrEmpty(sku.PropValueIdEx1) && !string.IsNullOrEmpty(sku.PropValueIdEx2))
                {
                    sku.Props = bllMall.GetProductProperties(sku.SkuId);//兼容efast同步
                }
                if (string.IsNullOrEmpty(sku.Props))
                {
                    return "";
                }
                foreach (var item in sku.Props.Split(';'))
                {
                    string propId = item.Split(':')[0];
                    if (!propIdList.Contains(propId))
                    {
                        propIdList.Add(propId);
                        string propName = item.Split(':')[2];
                        var  prop= bllMall.Get<ProductProperty>(string.Format("PropId={0}",propId));
                        if (prop!=null)
                        {
                            propName = prop.PropName;
                        }
                        propKeyValue.Add(int.Parse(propId), propName);

                    }
                }

            }

            List<PropModel> data = new List<PropModel>();
            foreach (var propId in propIdList)
            {
                PropModel model = new PropModel();
                model.name = propKeyValue[int.Parse(propId)];
                model.values = new List<Values>();
                foreach (var sku in skuList)
                {

                    foreach (var item in sku.Props.Split(';'))
                    {
                        string propIdNew = item.Split(':')[0];
                        string propValue = item.Split(':')[1];
                        string propValueName = item.Split(':')[3];
                        if (propId == propIdNew)
                        {
                            Values valueModel = new Values();
                            valueModel.p_id = int.Parse(propId);
                            valueModel.v_id = int.Parse(propValue);

                            BLLJIMP.Model.ProductPropertyValue propValueInfo = bllMall.GetProductPropertyValue(valueModel.v_id);
                            //valueModel.value = propValueName;
                            valueModel.value = propValueInfo.PropValue;
                            valueModel.sku_img = sku.SkuImg;
                            if (model.values.Where(p => p.v_id == valueModel.v_id).Count() == 0)
                            {
                                model.values.Add(valueModel);
                            }


                        }
                    }

                }

                data.Add(model);

            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {

                totalcount = data.Count,
                list = data

            });

        }

        /// <summary>
        /// 获取相关商品
        /// </summary>
        /// <param name="prodcutInfo"></param>
        /// <returns></returns>
        private dynamic GetRelevantProduct(WXMallProductInfo prodcutInfo) {

            if (!string.IsNullOrEmpty(prodcutInfo.RelevantProductIds))
            {

                string pids="'"+prodcutInfo.RelevantProductIds.Replace(",","','")+"'";
                var productList = bllMall.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' And PID in({1})",bllMall.WebsiteOwner,pids));
                var data = from p in productList
                           select new
                           {
                               product_id=p.PID,
                               title=p.PName,
                               price=p.Price,
                               img_url=p.RecommendImg


                           };

                return data;


            }
            return null;
        
        }

        /// <summary>
        ///   获取实际SKu信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTrueSkuInfo(HttpContext context)
        {
            string disType=context.Request["dis_type"];//折扣类型 0 商品折扣 1 会员折扣 2生日折扣
            if (string.IsNullOrEmpty(disType))
            {
                disType = "0";
            }
            string data=context.Request["data"];
            List<RequestSkuModel> requestSkuList =ZentCloud.Common.JSONHelper.JsonToModel<List<RequestSkuModel>>(data);
            Open.HongWareSDK.Client client = new Open.HongWareSDK.Client(bllMall.WebsiteOwner);

            List<BLLJIMP.Model.API.Mall.SkuModel> skuList=new List<BLLJIMP.Model.API.Mall.SkuModel>();
            foreach (var item in requestSkuList)
            {
                var sku = bllMall.GetProductSku(item.sku_id, true);
                BLLJIMP.Model.API.Mall.SkuModel model = new BLLJIMP.Model.API.Mall.SkuModel();
                model.price = item.price;
                model.count = item.num;
                model.sku_id = item.sku_id;
                model.sku_sn = sku.SkuSN;
                skuList.Add(model);


            }
            List<ResponseSkuModel> respSkuList = new List<ResponseSkuModel>();
           var result= client.MemberRight(skuList);
           if (result.isSuccess)
           {
               foreach (var orderItem in result.orders[0].orderItems)
               {

                   ResponseSkuModel model = new ResponseSkuModel();
                   var sku=skuList.Single(p => p.sku_sn == orderItem.sku);
                   model.sku_id = sku.sku_id;
                   model.num = sku.count;
                   model.pre_price =(decimal)sku.price;
                   
                   switch (disType)
                   {
                       case "0"://商品折扣
                           model.price = Convert.ToDecimal(orderItem.pro_RealPrice);
                           model.discount = orderItem.pro_dis;
                           break;
                       case "1"://会员折扣
                           model.price = Convert.ToDecimal(orderItem.mem_realPrice);
                           model.discount = orderItem.mem_dis;
                           break;
                       case "2"://生日折扣
                           model.price = Convert.ToDecimal(orderItem.birth_realPrice);
                           model.discount = orderItem.birth_dis;
                           break;
                       default:
                           break;
                   }

                   respSkuList.Add(model);


               }

           }

           
            return ZentCloud.Common.JSONHelper.ObjectToJson(respSkuList);


        }
       


           
        


        /// <summary>
        /// 请求的数据
        /// </summary>
        private class RequestSkuModel {
            /// <summary>
            /// sku编号
            /// </summary>
            public int sku_id { get; set; }
            /// <summary>
            /// 数量
            /// </summary>
            public int num { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal price { get; set; }
        
        }



        /// <summary>
        /// 响应的数据
        /// </summary>
        private class ResponseSkuModel {
            /// <summary>
            /// sku编号
            /// </summary>
            public int sku_id { get; set; }
            /// <summary>
            /// 正常价格
            /// </summary>
            public decimal pre_price { get; set; }
            /// <summary>
            /// 实价
            /// </summary>
            public decimal price{get;set;}
            /// <summary>
            /// 数量
            /// </summary>
            public int num { get; set; }
            /// <summary>
            /// 折扣
            /// </summary>
            public string discount { get; set; }
            /// <summary>
            /// 实际价格
            /// </summary>
           public decimal total_amount{get;set;}
        
        }


        /// <summary>
        /// 特征量模型
        /// </summary>
        private class PropModel
        {

            /// <summary>
            /// 属性
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public List<Values> values { get; set; }

        }
        /// <summary>
        /// 特征量值模型
        /// </summary>
        private class Values
        {
            /// <summary>
            /// 特征量ID
            /// </summary>
            public int p_id { get; set; }
            /// <summary>
            /// 特征量值ID
            /// </summary>
            public int v_id { get; set; }
            /// <summary>
            /// 特征量值
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// sku图片
            /// </summary>
            public string sku_img {get;set; }

        }




    }
}