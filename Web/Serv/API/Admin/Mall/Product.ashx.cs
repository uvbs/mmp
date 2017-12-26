using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{


    /// <summary>
    /// 商品
    /// </summary>
    public class Product : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        ///yike
        /// </summary>
        Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// 模块日志
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuPerm = new BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// 
        /// </summary>
        ZentCloud.BLLPermission.BLLPermission bllPer = new BLLPermission.BLLPermission();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLWXMallProduct bllWXMallProduct = new BLLJIMP.BLLWXMallProduct();



        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            try
            {
                string iSthreshold = context.Request["is_threshold"];//库存紧急商品
                int totalCount = 0;
                var sourceData = bllMall.GetProductList(context, out totalCount, 0, false);
                bool isProductPV = bllMenuPerm.CheckUserAndPmsKey(bllLog.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.IsShowProductPv);
                var list = from p in sourceData
                           select new
                           {
                               product_id = p.PID,
                               category_id = p.CategoryId,
                               category_name = bllMall.GetWXMallCategoryName(p.CategoryId),
                               product_title = p.PName,
                               quote_price = p.PreviousPrice,
                               //price = p.Price,
                               price = bllMall.GetShowPrice(p),
                               score = p.Score,
                               img_url = bllMall.GetImgUrl(p.RecommendImg),
                               is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                               pv = !isProductPV ? 0 : p.PV,
                               ip = p.IP,
                               uv = p.UV,
                               tags = p.Tags,
                               sale_count = p.SaleCount,
                               sort = p.Sort,
                               create_time = bllMall.GetTimeStamp(p.InsertDate),
                               totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID)),
                               is_promotion_product = p.IsPromotionProduct,
                               product_code = p.ProductCode,
                               access_level = p.AccessLevel,
                               limit_buy_time = p.LimitBuyTime,
                               supplier_name = bllMall.GetSuppLierByUserId(p.SupplierUserId, p.WebsiteOwner).Company,
                               //product_type_id=p.ProductTypeId,
                               //is_product_pv=isProductPV,
                               record_product_count = bllMall.GetProductStockByPID(p.PID),//缺货登记
                               province = p.Province,
                               city = p.City,
                               ex19 = p.Ex19,//楼盘分类  新房(NewHouse)  二手房(SecondHandHouse)
                               group_buy_rule_list = from r in bllMall.GetProductGroupBuyRuleList(p.PID)
                                                     select new
                                                     {
                                                         rule_id = r.RuleId,
                                                         rule_name = r.RuleName,
                                                         head_discount = r.HeadDiscount,
                                                         member_discount = r.MemberDiscount,
                                                         people_count = r.PeopleCount,
                                                         expire_day = r.ExpireDay
                                                     },
                               review_count = p.ReviewCount
                           };

                if (iSthreshold == "1")
                {
                    WebsiteInfo website = bllMall.GetWebsiteInfoModelFromDataBase();
                    list = list.Where(p => p.totalcount <= website.ProductStockThreshold).ToList();
                    totalCount = list.Count();
                }
                var data = new
                {
                    totalcount = totalCount,
                    list = list,//列表

                };
                return ZentCloud.Common.JSONHelper.ObjectToJson(data);
                //return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ListByPids(HttpContext context)
        {
            string pids = context.Request["pids"];

            List<WXMallProductInfo> plist = bllMall.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' AND PID in ({1}) ", bllMall.WebsiteOwner, pids));
            var list = from p in plist
                       select new
                       {
                           product_id = p.PID,
                           category_name = bllMall.GetWXMallCategoryName(p.CategoryId),
                           product_title = p.PName,
                       };
            var data = new
            {
                totalcount = plist.Count,
                list = list,//列表
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {


            if (currentUserInfo.UserType == 7)
            {
                var companyConfig = bllMall.Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
                if (companyConfig != null && companyConfig.StockType == 1)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无权添加商品";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

            }
            string data = context.Request["data"];
            //商品模型
            ProcuctModel productRequestModel;
            //商品模型
            try
            {
                productRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<ProcuctModel>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            //
            //数据检查
            if (string.IsNullOrEmpty(productRequestModel.product_title))
            {
                resp.errcode = 1;
                resp.errmsg = "商品名称必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (productRequestModel.quote_price == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "商品原价必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //商品价格和积分必填一项            
            if (productRequestModel.price == 0 && productRequestModel.score == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "商品价格和积分必填一项";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            if (productRequestModel.show_img_list.Count == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "请上传商品图片";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            #region 外部库存
            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
            {
                Open.EfastSDK.Client efast = new Open.EfastSDK.Client();
                string shopIdStr = System.Configuration.ConfigurationManager.AppSettings["eFastShopId"];
                int shopId = int.Parse(shopIdStr);
                foreach (var sku in productRequestModel.skus)
                {
                    if (!string.IsNullOrEmpty(sku.sku_sn))
                    {
                        if (!string.IsNullOrEmpty(shopIdStr))
                        {
                            var eFastSku = efast.GetSkuStock(shopId, sku.sku_sn);
                            if (eFastSku != null)
                            {
                                sku.count = eFastSku.sl;

                            }
                            else
                            {
                                resp.errcode = 1;
                                resp.errmsg = "商品条码不存在,请检查。";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }

                        }

                    }
                    else
                    {
                        //resp.errcode = 1;
                        //resp.errmsg = "商品条码必填,请检查。";
                        //return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                }

            }
            #endregion

            foreach (var sku in productRequestModel.skus)
            {
                if (sku.price <= 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "价格不能小于0";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (!string.IsNullOrEmpty(sku.sku_sn))
                {
                    ProductSku productSku = bllMall.GetProductSkuBySkuSn(sku.sku_sn);
                    if (productSku != null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "商家编码重复";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                }


            }
            if (productRequestModel.unified_freight > 0 && productRequestModel.freight_template_id > 0)
            {
                resp.errcode = 1;
                resp.errmsg = "不能同时设置统一运费和运费模板";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.GetWebsiteInfoModelFromDataBase(bllMall.WebsiteOwner).RequiredSupplier == 1)
            {
                if (string.IsNullOrEmpty(productRequestModel.supplier_userid))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请选择商户";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

            }


            //if (productRequestModel.score > 0 && (productRequestModel.is_cashpay_only == 1))
            //{

            //    resp.errcode = 1;
            //    resp.errmsg = "商品设置为仅现金支付时不能设置积分";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //}

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                WXMallProductInfo productModel = new WXMallProductInfo();
                productModel.PID = bllMall.GetGUID(BLLJIMP.TransacType.AddWXMallProductID);
                productModel.PName = productRequestModel.product_title;
                productModel.PDescription = productRequestModel.product_desc;
                productModel.Price = productRequestModel.price;
                productModel.BasePrice = productRequestModel.base_price;
                productModel.CategoryId = productRequestModel.category_id;
                productModel.InsertDate = DateTime.Now;
                productModel.IsOnSale = productRequestModel.is_onsale.ToString();
                productModel.Stock = productRequestModel.totalcount;
                productModel.RecommendImg = productRequestModel.show_img_list[0];
                productModel.WebsiteOwner = bllMall.WebsiteOwner;
                productModel.Tags = productRequestModel.tags;
                productModel.Sort = productRequestModel.sort;
                productModel.UserID = currentUserInfo.UserID;
                productModel.PreviousPrice = productRequestModel.quote_price;
                productModel.Summary = productRequestModel.product_summary;
                productModel.ProductCode = productRequestModel.product_code;
                productModel.UnifiedFreight = productRequestModel.unified_freight;
                productModel.FreightTemplateId = productRequestModel.freight_template_id;
                productModel.LastUpdate = DateTime.Now;
                productModel.RelationProductId = productRequestModel.relation_product_id;
                productModel.AccessLevel = productRequestModel.access_Level;
                productModel.ExArticleId_1 = productRequestModel.ex_article_id_1;
                productModel.ExArticleId_2 = productRequestModel.ex_article_id_2;
                productModel.ExArticleId_3 = productRequestModel.ex_article_id_3;
                productModel.ExArticleId_4 = productRequestModel.ex_article_id_4;
                productModel.ExArticleId_5 = productRequestModel.ex_article_id_5;
                productModel.ExArticleTitle_1 = productRequestModel.ex_article_title_1;
                productModel.ExArticleTitle_2 = productRequestModel.ex_article_title_2;
                productModel.ExArticleTitle_3 = productRequestModel.ex_article_title_3;
                productModel.ExArticleTitle_4 = productRequestModel.ex_article_title_4;
                productModel.ExArticleTitle_5 = productRequestModel.ex_article_title_5;
                if (!string.IsNullOrEmpty(productRequestModel.type))
                {
                    productModel.ArticleCategoryType = productRequestModel.type;
                }
                else
                {
                    productModel.ArticleCategoryType = "Mall";
                }
                productModel.Score = productRequestModel.score;
                productModel.IsCashPayOnly = productRequestModel.is_cashpay_only;

                productModel.IsNoExpress = productRequestModel.is_no_express;

                productModel.RelevantProductIds = productRequestModel.relevant_product_ids;

                productModel.RebatePriceRate = productRequestModel.rebate_price_rate;
                productModel.RebateScoreRate = productRequestModel.rebate_score_rate;

                productModel.TabExTitle1 = productRequestModel.tab_ex_title1;
                productModel.TabExTitle2 = productRequestModel.tab_ex_title2;
                productModel.TabExTitle3 = productRequestModel.tab_ex_title3;
                productModel.TabExTitle4 = productRequestModel.tab_ex_title4;
                productModel.TabExTitle5 = productRequestModel.tab_ex_title5;

                productModel.TabExContent1 = productRequestModel.tab_ex_content1;
                productModel.TabExContent2 = productRequestModel.tab_ex_content2;
                productModel.TabExContent3 = productRequestModel.tab_ex_content3;
                productModel.TabExContent4 = productRequestModel.tab_ex_content4;
                productModel.TabExContent5 = productRequestModel.tab_ex_content5;
                productModel.IsAppointment = !string.IsNullOrEmpty(productRequestModel.is_appointment) ? int.Parse(productRequestModel.is_appointment) : 0;
                productModel.AppointmentStartTime = productRequestModel.appointment_start_time;
                productModel.AppointmentEndTime = productRequestModel.appointment_end_time;
                productModel.AppointmentDeliveryTime = productRequestModel.appointment_delivery_time;
                productModel.Weight = !string.IsNullOrEmpty(productRequestModel.weight) ? decimal.Parse(productRequestModel.weight) : 0;
                productModel.SupplierUserId = productRequestModel.supplier_userid;
                #region 预购信息检查
                if (productModel.IsAppointment == 1)
                {

                    if (string.IsNullOrEmpty(productModel.AppointmentStartTime) || string.IsNullOrEmpty(productModel.AppointmentEndTime) || string.IsNullOrEmpty(productModel.AppointmentDeliveryTime))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "预购开始时间,结束时间,发货时间必填";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    //if (Convert.ToDateTime(productModel.AppointmentStartTime) < DateTime.Now)
                    //{
                    //    resp.errcode = 1;
                    //    resp.errmsg = "预购开始时间需要晚于当前时间";
                    //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    //}
                    if (Convert.ToDateTime(productModel.AppointmentStartTime) >= Convert.ToDateTime(productModel.AppointmentEndTime))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "预购结束时间需要晚于开始时间";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    if (Convert.ToDateTime(productModel.AppointmentDeliveryTime) < Convert.ToDateTime(productModel.AppointmentEndTime))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "预购发货时间需要晚于结束时间";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }


                }
                #endregion
                //如果商品无需物流，则运费模板设置为空
                if (productModel.IsNoExpress == 1)
                {
                    productModel.FreightTemplateId = 0;
                    productModel.IsNeedNamePhone = !string.IsNullOrEmpty(productRequestModel.is_need_name_phone) ? int.Parse(productRequestModel.is_need_name_phone) : 1;
                }
                else
                {
                    productModel.IsNeedNamePhone = 0;
                }

                string showImageStr = "";
                for (int i = 0; i < productRequestModel.show_img_list.Count; i++)
                {
                    showImageStr += productRequestModel.show_img_list[i] + ",";
                }
                showImageStr = showImageStr.TrimEnd(',');
                productModel.ShowImage = showImageStr;
                #region 楼盘字段
                productModel.Address = productRequestModel.address;
                productModel.Province = productRequestModel.province;
                productModel.ProvinceCode = productRequestModel.province_code;
                productModel.City = productRequestModel.city;
                productModel.CityCode = productRequestModel.city_code;
                productModel.District = productRequestModel.district;
                productModel.DistrictCode = productRequestModel.district_code;
                productModel.Ex1 = productRequestModel.ex1;
                productModel.Ex2 = productRequestModel.ex2;
                productModel.Ex3 = productRequestModel.ex3;
                productModel.Ex4 = productRequestModel.ex4;
                productModel.Ex5 = productRequestModel.ex5;
                productModel.Ex6 = productRequestModel.ex6;
                productModel.Ex7 = productRequestModel.ex7;
                productModel.Ex8 = productRequestModel.ex8;
                productModel.Ex9 = productRequestModel.ex9;
                productModel.Ex10 = productRequestModel.ex10;
                productModel.Ex11 = productRequestModel.ex11;
                productModel.Ex12 = productRequestModel.ex12;
                productModel.Ex13 = productRequestModel.ex13;
                productModel.Ex14 = productRequestModel.ex14;
                productModel.Ex15 = productRequestModel.ex15;
                productModel.Ex16 = productRequestModel.ex16;
                productModel.Ex17 = productRequestModel.ex17;
                productModel.Ex18 = productRequestModel.ex18;
                productModel.Ex19 = productRequestModel.ex19;
                #endregion
                if (!bllMall.Add(productModel, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "插入商品表失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                if (productRequestModel.skus != null && productRequestModel.skus.Count > 0)//没有sku 此商品为单品,加默认sku
                {
                    #region 增加sku
                    foreach (var sku in productRequestModel.skus)
                    {

                        if (!CheckSkuPropertyiesIsRepeat(sku.properties))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "sku属性重复,请检查";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        //sku.properties = sku.properties.Replace("；", ";");
                        sku.show_properties = sku.show_properties.Replace("；", ";");
                        ProductSku productSku = new ProductSku();
                        productSku.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                        productSku.ProductId = int.Parse(productModel.PID);
                        productSku.InsertDate = DateTime.Now;
                        productSku.Price = sku.price;
                        productSku.BasePrice = sku.base_price;
                        productSku.Props = GetSkuPropertiesStr(sku.properties);
                        productSku.ShowProps = GetSkuShowProperties(sku.properties);
                        productSku.Stock = sku.count;
                        productSku.WebSiteOwner = bllMall.WebsiteOwner;
                        productSku.SkuSN = sku.sku_sn;
                        productSku.OutBarCode = sku.sku_sn;
                        productSku.ArticleCategoryType = "Mall";
                        productSku.Weight = !string.IsNullOrEmpty(sku.weight) ? decimal.Parse(sku.weight) : 0;
                        productSku.SkuImg = sku.skuimg;

                        #region 课程检查
                        if (productModel.ArticleCategoryType == "Course")
                        {
                            if (!string.IsNullOrEmpty(sku.ex_questionnaire_id))
                            {
                                if (bllMall.GetCount<ProductSku>(string.Format("ExQuestionnaireID={0}", sku.ex_questionnaire_id)) > 0)
                                {
                                    tran.Rollback();
                                    resp.errcode = 1;
                                    resp.errmsg = productSku.ShowProps + "  试卷已经被别的证书使用,请换另外一个试卷";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }
                                productSku.ExQuestionnaireID = sku.ex_questionnaire_id;
                            }
                        }
                        #endregion

                        if (!bllMall.Add(productSku))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "插入商品SKU表失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                        }


                    }
                    #endregion
                }
                else
                {

                    #region 增加系统默认sku
                    //增加系统默认sku
                    ProductSku productSkuDefault = new ProductSku();//
                    productSkuDefault.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                    productSkuDefault.ProductId = int.Parse(productModel.PID);
                    productSkuDefault.InsertDate = DateTime.Now;
                    productSkuDefault.Price = productModel.Price;
                    productSkuDefault.Props = "";
                    productSkuDefault.ShowProps = "";
                    productSkuDefault.Stock = productModel.Stock;
                    productSkuDefault.WebSiteOwner = bllMall.WebsiteOwner;
                    productSkuDefault.SkuSN = productModel.ProductCode;
                    productSkuDefault.OutBarCode = productModel.ProductCode;
                    productSkuDefault.ArticleCategoryType = "Mall";
                    productSkuDefault.Weight = productModel.Weight;
                    if (!bllMall.Add(productSkuDefault))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "插入商品SKU表失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }
                    #endregion


                }



                #region 价格配置
                if (productRequestModel.price_config_list != null && productRequestModel.price_config_list.Count > 0)
                {
                    foreach (var item in productRequestModel.price_config_list)
                    {
                        if (item.price <= 0)
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = string.Format("日期{0}价格需大于0", item.date);
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        if (bllMall.GetCount<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}' And Date='{2}'", bllMall.WebsiteOwner, productModel.PID, item.date)) > 0)
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "日期重复";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        ProductPriceConfig model = new ProductPriceConfig();
                        model.WebsiteOwner = bllMall.WebsiteOwner;
                        model.Date = item.date;
                        model.Price = item.price;
                        model.ProductId = productModel.PID;
                        if (!bllMall.Add(model))
                        {
                            //tran.Rollback(); 
                            resp.errcode = 1;
                            resp.errmsg = "添加商品价格配置失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                        }


                    }

                }
                #endregion

                tran.Commit();
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加商品[id=" + productModel.PID + "]");

                //更新商品价格区间
                bllWXMallProduct.UpdateProductPriceRange(productModel.PID, bllWXMallProduct.WebsiteOwner);

                BLLRedis.ClearProductList(bllMall.WebsiteOwner);

                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "ok",
                    product_id = productModel.PID
                });


            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = 1;
                resp.errmsg = ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }         //


        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string data = context.Request["data"];
            //商品模型
            ProcuctModel productRequestModel;
            //商品模型
            try
            {
                productRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<ProcuctModel>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            WXMallProductInfo productModel = bllMall.GetProduct(productRequestModel.product_id.ToString());
            #region 供应商仅支持更新商品库存
            if (currentUserInfo.UserType == 7)
            {

                var companyConfig = bllMall.Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
                if (companyConfig != null && companyConfig.StockType == 1)
                {
                    #region 同一商品不同门店不同库存
                    productModel.Stock = productRequestModel.totalcount;
                    bllMall.Update(productModel);
                    List<ProductSku> oldProductSku = new List<ProductSku>();
                    if (productRequestModel.skus != null)//非单品
                    {
                        oldProductSku = bllMall.GetProductSkuList(int.Parse(productModel.PID),false).Where(p => string.IsNullOrEmpty(p.Props)).ToList();
                        if (oldProductSku.Count > 0)
                        {
                            SkuModel defaultModel=new SkuModel();
                            defaultModel.sku_id=oldProductSku[0].SkuId;
                            defaultModel.count = productRequestModel.totalcount;
                            defaultModel.price = productRequestModel.price;
                            defaultModel.base_price = oldProductSku[0].BasePrice;
                            productRequestModel.skus.Add(defaultModel);

                        }
                        foreach (var sku in productRequestModel.skus.Where(p => p.sku_id > 0))
                        {

                            ProductSkuSupplier productSku = bllMall.Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", sku.sku_id, currentUserInfo.AutoID));
                            if (productSku == null)
                            {
                                productSku = new ProductSkuSupplier();
                                productSku.SkuId = sku.sku_id;
                                productSku.Price = sku.price;
                                productSku.Stock = sku.count;
                                productSku.WebSiteOwner = bllMall.WebsiteOwner;
                                productSku.InsertDate = DateTime.Now;
                                productSku.ProductId = int.Parse(productModel.PID);
                                productSku.BasePrice = sku.base_price;
                                productSku.SupplierId = currentUserInfo.AutoID.ToString();
                                if (!bllMall.Add(productSku))
                                {
                                    resp.errcode = 1;
                                    resp.errmsg = "添加库存失败";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }
                            }
                            else
                            {
                                //productSku.SkuId = sku.sku_id;
                                productSku.Price = sku.price;
                                productSku.Stock = sku.count;
                                productSku.WebSiteOwner = bllMall.WebsiteOwner;
                                //productSku.InsertDate = DateTime.Now;
                                //productSku.ProductId = int.Parse(productModel.PID);
                                productSku.BasePrice = sku.base_price;
                                productSku.SupplierId = currentUserInfo.AutoID.ToString();
                                if (!bllMall.Update(productSku))
                                {
                                    BLLRedis.ClearProduct(productModel.WebsiteOwner, productModel.PID);
                                    resp.errcode = 1;
                                    resp.errmsg = "更新库存失败";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }
                            }


                        }



                    }
                    #endregion

                }
                else
                {
                    #region 独立库存
                    productModel.Stock = productRequestModel.totalcount;
                    bllMall.Update(productModel);
                    //原有的sku
                    List<ProductSku> oldProductSku = new List<ProductSku>();
                    if (productRequestModel.skus != null)//非单品
                    {
                        oldProductSku = bllMall.GetProductSkuList(int.Parse(productModel.PID)).Where(p => string.IsNullOrEmpty(p.Props)).ToList();
                        if (oldProductSku.Count > 0)
                        {
                            oldProductSku[0].Stock = productModel.Stock;
                            bllMall.Update(oldProductSku[0]);
                        }
                        //修改原有sku
                        foreach (var sku in productRequestModel.skus)
                        {

                            if (sku.sku_id == 0)//sku id为0增加sku
                            {


                            }
                            else
                            {
                                //修改原有sku
                                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                                if (productSku == null)
                                {

                                    resp.errcode = 1;
                                    resp.errmsg = "sku_id不存在，请检查";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }
                                if (!CheckSkuPropertyiesIsRepeat(sku.properties))
                                {

                                    resp.errcode = 1;
                                    resp.errmsg = "sku属性重复,请检查";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }
                                if (sku.price < 0)
                                {

                                    resp.errcode = 1;
                                    resp.errmsg = "价格不能小于0";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }
                                productSku.Stock = sku.count;
                                productSku.Modified = DateTime.Now;
                                if (!bllMall.Update(productSku))
                                {

                                    //清除商品和商品列表缓存
                                    BLLRedis.ClearProduct(productModel.WebsiteOwner, productModel.PID);

                                    resp.errcode = 1;
                                    resp.errmsg = "更新商品SKU表失败";
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                                }
                                else
                                {

                                }
                            }

                        }

                    }
                    #endregion

                }
                resp.errcode = 0;
                resp.errmsg = "ok";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            #endregion

            //
            //数据检查
            if (string.IsNullOrEmpty(productRequestModel.product_title))
            {
                resp.errcode = 1;
                resp.errmsg = "商品名称必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //商品价格和积分必填一项            
            if (productRequestModel.price == 0 && productRequestModel.score == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "商品价格和积分必填一项";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (productRequestModel.show_img_list.Count == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "请上传商品图片";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (productRequestModel.unified_freight > 0 && productRequestModel.freight_template_id > 0)
            {
                resp.errcode = 1;
                resp.errmsg = "不能同时设置统一运费和运费模板";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.GetWebsiteInfoModelFromDataBase(bllMall.WebsiteOwner).RequiredSupplier == 1)
            {
                if (string.IsNullOrEmpty(productRequestModel.supplier_userid))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请选择商户";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {


                string oldCateId = string.Empty;

                if (productModel == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "商品不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                oldCateId = productModel.CategoryId;

                productModel.PName = productRequestModel.product_title;
                productModel.PDescription = productRequestModel.product_desc;
                productModel.Price = productRequestModel.price;
                productModel.BasePrice = productRequestModel.base_price;
                productModel.CategoryId = productRequestModel.category_id;
                productModel.IsOnSale = productRequestModel.is_onsale.ToString();
                productModel.Stock = productRequestModel.totalcount;
                productModel.RecommendImg = productRequestModel.show_img_list[0];
                productModel.Tags = productRequestModel.tags;
                productModel.Sort = productRequestModel.sort;
                productModel.PreviousPrice = productRequestModel.quote_price;
                productModel.Summary = productRequestModel.product_summary;
                productModel.ProductCode = productRequestModel.product_code;
                productModel.UnifiedFreight = productRequestModel.unified_freight;
                productModel.FreightTemplateId = productRequestModel.freight_template_id;
                productModel.LastUpdate = DateTime.Now;
                productModel.RelationProductId = productRequestModel.relation_product_id;
                productModel.AccessLevel = productRequestModel.access_Level;
                productModel.ExArticleId_1 = productRequestModel.ex_article_id_1;
                productModel.ExArticleId_2 = productRequestModel.ex_article_id_2;
                productModel.ExArticleId_3 = productRequestModel.ex_article_id_3;
                productModel.ExArticleId_4 = productRequestModel.ex_article_id_4;
                productModel.ExArticleId_5 = productRequestModel.ex_article_id_5;
                productModel.ExArticleTitle_1 = productRequestModel.ex_article_title_1;
                productModel.ExArticleTitle_2 = productRequestModel.ex_article_title_2;
                productModel.ExArticleTitle_3 = productRequestModel.ex_article_title_3;
                productModel.ExArticleTitle_4 = productRequestModel.ex_article_title_4;
                productModel.ExArticleTitle_5 = productRequestModel.ex_article_title_5;
                //productModel.ProductTypeId = productRequestModel.product_type_id;
                productModel.Score = productRequestModel.score;
                productModel.IsCashPayOnly = productRequestModel.is_cashpay_only;
                productModel.IsNoExpress = productRequestModel.is_no_express;
                productModel.RelevantProductIds = productRequestModel.relevant_product_ids;
                productModel.TabExTitle1 = productRequestModel.tab_ex_title1;
                productModel.TabExTitle2 = productRequestModel.tab_ex_title2;
                productModel.TabExTitle3 = productRequestModel.tab_ex_title3;
                productModel.TabExTitle4 = productRequestModel.tab_ex_title4;
                productModel.TabExTitle5 = productRequestModel.tab_ex_title5;

                productModel.RebatePriceRate = productRequestModel.rebate_price_rate;
                productModel.RebateScoreRate = productRequestModel.rebate_score_rate;

                productModel.TabExContent1 = productRequestModel.tab_ex_content1;
                productModel.TabExContent2 = productRequestModel.tab_ex_content2;
                productModel.TabExContent3 = productRequestModel.tab_ex_content3;
                productModel.TabExContent4 = productRequestModel.tab_ex_content4;
                productModel.TabExContent5 = productRequestModel.tab_ex_content5;
                productModel.ArticleCategoryType = productRequestModel.type;
                productModel.IsAppointment = !string.IsNullOrEmpty(productRequestModel.is_appointment) ? int.Parse(productRequestModel.is_appointment) : 0;
                productModel.AppointmentStartTime = productRequestModel.appointment_start_time;
                productModel.AppointmentEndTime = productRequestModel.appointment_end_time;
                productModel.AppointmentDeliveryTime = productRequestModel.appointment_delivery_time;
                productModel.Weight = !string.IsNullOrEmpty(productRequestModel.weight) ? decimal.Parse(productRequestModel.weight) : 0;
                productModel.SupplierUserId = productRequestModel.supplier_userid;
                productModel.Address = productRequestModel.address;
                productModel.Province = productRequestModel.province;
                productModel.ProvinceCode = productRequestModel.province_code;
                productModel.City = productRequestModel.city;
                productModel.CityCode = productRequestModel.city_code;
                productModel.District = productRequestModel.district;
                productModel.DistrictCode = productRequestModel.district_code;
                productModel.Ex1 = productRequestModel.ex1;
                productModel.Ex2 = productRequestModel.ex2;
                productModel.Ex3 = productRequestModel.ex3;
                productModel.Ex4 = productRequestModel.ex4;
                productModel.Ex5 = productRequestModel.ex5;
                productModel.Ex6 = productRequestModel.ex6;
                productModel.Ex7 = productRequestModel.ex7;
                productModel.Ex8 = productRequestModel.ex8;
                productModel.Ex9 = productRequestModel.ex9;
                productModel.Ex10 = productRequestModel.ex10;
                productModel.Ex11 = productRequestModel.ex11;
                productModel.Ex12 = productRequestModel.ex12;
                productModel.Ex13 = productRequestModel.ex13;
                productModel.Ex14 = productRequestModel.ex14;
                productModel.Ex15 = productRequestModel.ex15;
                productModel.Ex16 = productRequestModel.ex16;
                productModel.Ex17 = productRequestModel.ex17;
                productModel.Ex18 = productRequestModel.ex18;
                productModel.Ex19 = productRequestModel.ex19;
                #region 预购信息检查
                if (productModel.IsAppointment == 1)
                {

                    if (string.IsNullOrEmpty(productModel.AppointmentStartTime) || string.IsNullOrEmpty(productModel.AppointmentEndTime) || string.IsNullOrEmpty(productModel.AppointmentDeliveryTime))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "预购开始时间,结束时间,发货时间必填";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    //if (Convert.ToDateTime(productModel.AppointmentStartTime) < DateTime.Now)
                    //{
                    //    resp.errcode = 1;
                    //    resp.errmsg = "预购开始时间需要晚于当前时间";
                    //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    //}
                    if (Convert.ToDateTime(productModel.AppointmentStartTime) >= Convert.ToDateTime(productModel.AppointmentEndTime))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "预购结束时间需要晚于开始时间";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    if (Convert.ToDateTime(productModel.AppointmentDeliveryTime) < Convert.ToDateTime(productModel.AppointmentEndTime))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "预购发货时间需要晚于结束时间";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }


                }
                #endregion
                //如果商品无需物流，则运费模板设置为空
                if (productModel.IsNoExpress == 1)
                {
                    productModel.FreightTemplateId = 0;
                    productModel.IsNeedNamePhone = !string.IsNullOrEmpty(productRequestModel.is_need_name_phone) ? int.Parse(productRequestModel.is_need_name_phone) : 1;


                }
                else
                {
                    productModel.IsNeedNamePhone = 0;
                }

                string showImageStr = "";
                for (int i = 0; i < productRequestModel.show_img_list.Count; i++)
                {
                    showImageStr += productRequestModel.show_img_list[i] + ",";
                }

                showImageStr = showImageStr.TrimEnd(',');
                productModel.ShowImage = showImageStr;
                if (!bllMall.Update(productModel, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "插入商品表失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                }
                //原有的sku
                List<ProductSku> oldProductSku = new List<ProductSku>();
                if (productRequestModel.skus != null)//非单品
                {
                    oldProductSku = bllMall.GetProductSkuList(int.Parse(productModel.PID));
                    if (oldProductSku.Count != productRequestModel.skus.Where(p => p.sku_id != 0).Count())
                    {
                        //数量不一致，说明有删除操作
                        //筛选出被删除的 sku
                        var delSkuList = from req in oldProductSku
                                         where !(from old in productRequestModel.skus
                                                 select old.sku_id).Contains(req.SkuId)
                                         select req;

                        //
                        delSkuList = delSkuList.Where(p => p.Props != null && p.Props != "");

                        oldProductSku = (from req in oldProductSku
                                         where !(from old in delSkuList
                                                 select old.SkuId).Contains(req.SkuId)
                                         select req).ToList();

                        if (delSkuList.Count() > 0)
                        {
                            string delSkuIds = string.Format(" SkuId in ({0})", string.Join(",", delSkuList.SelectMany(p => new List<int>() { (int)p.SkuId })));
                            if (bllMall.GetCount<WXMallOrderDetailsInfo>(delSkuIds) > 0)
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "商品规格已经有订单,因此不能删除";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                            if (bllMall.Delete(new ProductSku(), delSkuIds, tran) != delSkuList.Count())
                            {

                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "删除商品SKU失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                        }
                        //
                    }

                    //if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                    //{
                    //    Open.EfastSDK.Client efast = new Open.EfastSDK.Client();
                    //    string shopIdStr = System.Configuration.ConfigurationManager.AppSettings["eFastShopId"];
                    //    int shopId = int.Parse(shopIdStr);
                    //    foreach (var sku in productRequestModel.skus)
                    //    {
                    //        if (!string.IsNullOrEmpty(sku.sku_sn))
                    //        {
                    //            if (!string.IsNullOrEmpty(shopIdStr))
                    //            {
                    //                var eFastSku = efast.GetSkuStock(shopId, sku.sku_sn);
                    //                if (eFastSku != null)
                    //                {
                    //                    sku.count = eFastSku.sl;

                    //                }
                    //                else
                    //                {
                    //                //    resp.errcode = 1;
                    //                //    resp.errmsg = "商品条码不存在,请检查。";
                    //                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    //                }

                    //            }

                    //        }
                    //        else
                    //        {
                    //            //resp.errcode = 1;
                    //            //resp.errmsg = "商品条码必填,请检查。";
                    //            //return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    //        }

                    //    }

                    //}
                    //修改原有sku
                    foreach (var sku in productRequestModel.skus)
                    {
                        if (sku.price <= 0)
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "价格不能小于0";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        if (sku.sku_id == 0)//sku id为0增加sku
                        {

                            if (!CheckSkuPropertyiesIsRepeat(sku.properties))
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "sku属性重复,请检查";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }

                            ProductSku productSku = new ProductSku();
                            productSku.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                            productSku.ProductId = int.Parse(productModel.PID);
                            productSku.InsertDate = DateTime.Now;
                            productSku.Price = sku.price;
                            productSku.BasePrice = sku.base_price;
                            productSku.Props = GetSkuPropertiesStr(sku.properties);
                            productSku.ShowProps = GetSkuShowProperties(sku.properties);
                            productSku.Stock = sku.count;
                            productSku.WebSiteOwner = bllMall.WebsiteOwner;
                            productSku.SkuSN = sku.sku_sn;
                            productSku.OutBarCode = sku.sku_sn;
                            productSku.Weight = !string.IsNullOrEmpty(sku.weight) ? decimal.Parse(sku.weight) : 0;
                            productSku.SkuImg = sku.skuimg;
                            #region 课程检查
                            if (productModel.ArticleCategoryType == "Course")
                            {

                                if (!string.IsNullOrEmpty(sku.ex_questionnaire_id))
                                {

                                    if (bllMall.GetCount<ProductSku>(string.Format("ExQuestionnaireID={0}", sku.ex_questionnaire_id)) > 0)
                                    {
                                        tran.Rollback();
                                        resp.errcode = 1;
                                        resp.errmsg = productSku.ShowProps + " 试卷已经被别的证书使用,请换另外一个试卷";
                                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                    }
                                    productSku.ExQuestionnaireID = sku.ex_questionnaire_id;
                                }
                            }
                            #endregion
                            if (!bllMall.Add(productSku, tran))
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "插入商品SKU表失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                            }
                        }
                        else
                        {
                            //修改原有sku
                            ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                            ProductSku productSkuOld = new ProductSku();
                            productSkuOld.ExQuestionnaireID = productSku.ExQuestionnaireID;
                            if (productSku == null)
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "sku_id不存在，请检查";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                            if (!CheckSkuPropertyiesIsRepeat(sku.properties))
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "sku属性重复,请检查";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                            if (sku.price < 0)
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "价格不能小于0";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                            productSku.Price = sku.price;
                            productSku.BasePrice = sku.base_price;
                            productSku.Props = GetSkuPropertiesStr(sku.properties); //属性组合不能修改
                            productSku.ShowProps = sku.show_properties;
                            productSku.Stock = sku.count;
                            productSku.SkuSN = sku.sku_sn;
                            productSku.OutBarCode = sku.sku_sn;
                            productSku.Modified = DateTime.Now;
                            productSku.Weight = !string.IsNullOrEmpty(sku.weight) ? decimal.Parse(sku.weight) : 0;
                            productSku.SkuImg = sku.skuimg;
                            #region 课程检查
                            if (productModel.ArticleCategoryType == "Course")
                            {
                                //if (string.IsNullOrEmpty(sku.ex_questionnaire_id))
                                //{

                                //    tran.Rollback();
                                //    resp.errcode = 1;
                                //    resp.errmsg = "请为每一个证书选择一个试卷";
                                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                //}
                                if (!string.IsNullOrEmpty(sku.ex_questionnaire_id))
                                {

                                    if (bllMall.GetCount<ProductSku>(string.Format("ExQuestionnaireID={0} And SkuId!={1}", sku.ex_questionnaire_id, productSku.SkuId)) > 0)
                                    {
                                        tran.Rollback();
                                        resp.errcode = 1;
                                        resp.errmsg = productSku.ShowProps + " 试卷已经被别的证书使用,请换另外一个试卷";
                                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                    }
                                    productSku.ExQuestionnaireID = sku.ex_questionnaire_id;
                                }
                                if (!string.IsNullOrEmpty(productSkuOld.ExQuestionnaireID))
                                {

                                    if (productSkuOld.ExQuestionnaireID != productSku.ExQuestionnaireID)
                                    {
                                        //检查是否还有未批改的试卷
                                        if (bllMall.GetCount<QuestionnaireRecord>(string.Format("QuestionnaireID={0}  And Result='' ", productSkuOld.ExQuestionnaireID)) > 0)
                                        {
                                            tran.Rollback();
                                            resp.errcode = 1;
                                            resp.errmsg = productSku.ShowProps + " 还有未批改的试卷,请先批改";
                                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                        }
                                        //检查是否还有未批改的试卷
                                    }

                                }





                            }
                            #endregion
                            if (!bllMall.Update(productSku, tran))
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "更新商品SKU表失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(productSku.ExQuestionnaireID))
                                {
                                    //更新订单表
                                    bllMall.Update(new WXMallOrderDetailsInfo(), string.Format("ExQuestionnaireID='{0}'", productSku.ExQuestionnaireID), string.Format("SkuId={0}", productSku.SkuId));
                                    //更新订单表


                                }
                            }
                        }

                    }

                }


                if (productRequestModel.skus.Count == 0)//带过来的sku 为增加
                {
                    if (oldProductSku.Count == 0)
                    {
                        #region 增加系统默认sku
                        //增加系统默认sku
                        ProductSku productSkuDefault = new ProductSku();//
                        productSkuDefault.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                        productSkuDefault.ProductId = int.Parse(productModel.PID);
                        productSkuDefault.InsertDate = DateTime.Now;
                        productSkuDefault.Price = productModel.Price;
                        productSkuDefault.BasePrice = productModel.BasePrice;
                        productSkuDefault.Props = "";
                        productSkuDefault.ShowProps = "";
                        productSkuDefault.Stock = productModel.Stock;
                        productSkuDefault.WebSiteOwner = bllMall.WebsiteOwner;
                        // productSku.SkuSN = "";
                        productSkuDefault.SkuSN = productModel.ProductCode;
                        productSkuDefault.OutBarCode = productModel.ProductCode;
                        productSkuDefault.Weight = productModel.Weight;
                        if (!bllMall.Add(productSkuDefault, tran))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "插入商品SKU表失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        #endregion
                    }
                    else
                    {
                        //修改原有默认sku
                        oldProductSku[0].Price = productRequestModel.price;
                        oldProductSku[0].BasePrice = productRequestModel.base_price;
                        oldProductSku[0].Stock = productRequestModel.totalcount;
                        oldProductSku[0].Modified = DateTime.Now;
                        oldProductSku[0].SkuSN = productModel.ProductCode;
                        oldProductSku[0].OutBarCode = productModel.ProductCode;
                        oldProductSku[0].Weight = productModel.Weight;
                        if (!bllMall.Update(oldProductSku[0], tran))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "更新默认商品SKU表失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                        }
                    }
                }
                else
                {
                    var defaultSku = oldProductSku.Where(p => p.Props == null || p.Props == "").ToList();
                    if (defaultSku.Count() > 0)
                    {

                        bllMall.Delete(defaultSku[0], tran);

                    }

                }

                #region 价格配置

                #region 检查是否需要删除价格配置
                List<ProductPriceConfig> priceConfigList = bllMall.GetList<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}'", bllMall.WebsiteOwner, productModel.PID));
                if (priceConfigList.Count > 0)
                {
                    if (productRequestModel.price_config_list.Where(p => p.id > 0).Count() != priceConfigList.Count)
                    {
                        //
                        //数量不一致，说明有删除操作

                        var delProductConfig = from req in priceConfigList
                                               where !(from old in productRequestModel.price_config_list
                                                       select old.id).Contains(req.AutoId)
                                               select req;

                        //


                        if (delProductConfig.Count() > 0)
                        {
                            string delIds = string.Format(" AutoId in ({0})", string.Join(",", delProductConfig.SelectMany(p => new List<int>() { p.AutoId })));
                            if (bllMall.Delete(new ProductPriceConfig(), delIds) != delProductConfig.Count())
                            {

                                resp.errcode = 1;
                                resp.errmsg = "删除商品价格配置失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                        }
                        //


                    }



                }
                #endregion

                if (productRequestModel.price_config_list != null && productRequestModel.price_config_list.Count > 0)
                {
                    foreach (var item in productRequestModel.price_config_list)
                    {
                        if (item.price <= 0)
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = string.Format("日期{0}价格需大于0", item.date);
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }

                        if (item.id == 0)//添加
                        {
                            if (bllMall.GetCount<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}' And Date='{2}'", bllMall.WebsiteOwner, productModel.PID, item.date)) > 0)
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "日期重复";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                            ProductPriceConfig model = new ProductPriceConfig();
                            model.WebsiteOwner = bllMall.WebsiteOwner;
                            model.Date = item.date;
                            model.Price = item.price;
                            model.ProductId = productModel.PID;
                            if (!bllMall.Add(model))
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "添加商品价格配置失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                            }
                        }
                        else//编辑
                        {
                            if (bllMall.GetCount<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}' And Date='{2}' And AutoId!={3}", bllMall.WebsiteOwner, productModel.PID, item.date, item.id)) > 0)
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "日期重复";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                            ProductPriceConfig model = bllMall.Get<ProductPriceConfig>(string.Format(" AutoId={0} And WebsiteOwner='{1}'", item.id, bllMall.WebsiteOwner));
                            model.Date = item.date;
                            model.Price = item.price;
                            if (!bllMall.Update(model))
                            {
                                tran.Rollback();
                                resp.errcode = 1;
                                resp.errmsg = "更新商品价格配置失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }

                        }




                    }

                }





                #endregion


                tran.Commit();


                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                {
                    yikeClient.UpdateProductIsOnSale(productModel);
                }
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "修改商品[id=" + productModel.PID + "]");

                bllWXMallProduct.UpdateProductPriceRange(productModel.PID, bllWXMallProduct.WebsiteOwner);

                //原分类价格区间变动
                if (oldCateId != productModel.CategoryId)
                {
                    int oldCateIdInt = 0;
                    if (Int32.TryParse(oldCateId, out oldCateIdInt))
                    {
                        bllWXMallProduct.UpdateCateProductPriceRange(oldCateIdInt, bllWXMallProduct.WebsiteOwner);
                    }
                }

                //清除商品和商品列表缓存
                BLLRedis.ClearProduct(productModel.WebsiteOwner, productModel.PID);

                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "ok"

                });


            }
            catch (Exception ex)
            {
                //tran.Rollback();
                resp.errcode = 1;
                resp.errmsg = ex.ToString();
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

        }


        /// <summary>
        /// 更新商品信息[按传入的字段更新]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateField(HttpContext context)
        {

            string field = context.Request["field"];
            string value = context.Request["value"];
            string productIds = context.Request["product_ids"];
            if (string.IsNullOrEmpty(field))
            {
                resp.errcode = 1;
                resp.errmsg = "更新字段不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            //if (string.IsNullOrEmpty(value))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "value参数不能为空";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //}
            if (string.IsNullOrEmpty(productIds))
            {
                resp.errcode = 1;
                resp.errmsg = "product_ids不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            string strUpdateField = "";
            switch (field)
            {
                case "is_onsale"://更新上下架
                    strUpdateField = string.Format(" IsOnSale='{0}'", value);
                    break;
                case "is_delete"://删除
                    strUpdateField = string.Format(" IsDelete='{0}'", value);
                    break;
                case "sort"://排序号
                    strUpdateField = string.Format(" Sort='{0}'", value);
                    break;
                case "category_id"://分类
                    strUpdateField = string.Format(" CategoryId='{0}'", value);
                    break;
                case "freighttemplate_id"://运费模块
                    strUpdateField = string.Format(" FreightTemplateId={0}", int.Parse(value));
                    break;
                case "supplier_userid"://供应商
                    strUpdateField = string.Format(" SupplierUserId='{0}'", value);
                    break;
                default:
                    break;
            }
            if (strUpdateField != "")
            {
                if (bllMall.Update(new WXMallProductInfo(), strUpdateField, string.Format(" PID in({0}) And WebsiteOwner='{1}'", productIds, bllMall.WebsiteOwner)) == productIds.Split(',').Count())
                {
                    #region yike 同步
                    if (field == "is_onsale")
                    {
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                        {
                            foreach (var productId in productIds.Split(','))
                            {


                                ZentCloud.BLLJIMP.Model.WXMallProductInfo productInfo = bllMall.GetProduct(productId);
                                yikeClient.UpdateProductIsOnSale(productInfo);

                            }

                        }
                    }
                    #endregion

                    BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, productIds, false);
                    BLLRedis.ClearProductList(bllMall.WebsiteOwner);

                    resp.errcode = 0;
                    resp.errmsg = "ok";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "fail";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
            }

            resp.errcode = 1;
            resp.errmsg = "未更新任何字段";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string productIds = context.Request["product_ids"];
            int resultCount = bllMall.Update(new WXMallProductInfo(), string.Format(" IsDelete=1"), string.Format(" PID in ({0}) And WebsiteOwner='{1}'", productIds, bllMall.WebsiteOwner));
            if (resultCount == productIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除商品[id=" + productIds + "]");

                //删除成功，更新下前6位的分类价格区间
                int totalCount = 0;
                var cateList = bllMall.GetCategoryList(1, 6, "", out totalCount);

                if (cateList != null)
                {
                    foreach (var item in cateList)
                    {
                        bllWXMallProduct.UpdateCateProductPriceRange(item.AutoID, bllWXMallProduct.WebsiteOwner);
                    }
                }

                BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, productIds, false);
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取单个商品信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            try
            {


                string productId = context.Request["product_id"];
                var productInfo = bllMall.GetProduct(productId);
                if (productInfo == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "商品ID不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (productInfo.WebsiteOwner != bllMall.WebsiteOwner)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无权访问";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }


                string supplierId = "";
                if (currentUserInfo.UserType == 7)
                {
                    var companyConfig = bllMall.Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
                    if (companyConfig != null && companyConfig.StockType == 1)
                    {
                        supplierId = currentUserInfo.AutoID.ToString();
                    }

                }
                var skuSourceList = bllMall.GetProductSkuList(int.Parse(productId), true,supplierId);//源SKU 

                if (skuSourceList.Count==0)
                {
                    skuSourceList = bllMall.GetProductSkuList(int.Parse(productId), true);//源SKU 
                    
                }
                var defaultSku = skuSourceList.Where(p => string.IsNullOrEmpty(p.Props) && string.IsNullOrEmpty(p.PropValueIdEx1) && string.IsNullOrEmpty(p.PropValueIdEx2)).ToList();//默认sku
                if (defaultSku.Count > 0)
                {
                    skuSourceList.Remove(defaultSku[0]);
                }
                //skuSourceList = skuSourceList.OrderBy(p => p.PropValueIdEx1).ToList();
                //skuSourceList = skuSourceList.OrderBy(p => p.Props).ToList();

                var skus = from p in skuSourceList
                           select new
                           {
                               sku_id = p.SkuId,
                               properties = GetSkuProperties(bllMall.GetProductProperties(p.SkuId)),
                               show_properties = bllMall.GetProductShowProperties(p.SkuId),
                               price = p.Price,
                               base_price = p.BasePrice,
                               count = p.Stock,
                               sku_sn = p.SkuSN,
                               ex_questionnaire_id = p.ExQuestionnaireID,
                               weight = p.Weight,
                               skuimg = p.SkuImg
                           };
                skus = from s in skus where s.properties != null select s;
                
                if (skus.Count() > 0)
                {
                    try
                    {
                        var properties = skus.FirstOrDefault().properties;
                        if (properties != null &&properties.Count>0)
                        {
                            var tempSkus = skus.OrderBy(p => p.properties.FirstOrDefault(pi => pi.property_id == properties[0].property_id).property_value_id);

                            for (int i = 1; i < properties.Count; i++)
                            {
                                var pid = properties[i].property_id;
                                tempSkus = tempSkus.ThenBy(p => p.properties.FirstOrDefault(pi => pi.property_id == pid).property_value_id);
                            }
                            skus = tempSkus;
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }


                }

               




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

                //if (!string.IsNullOrEmpty(sourceData.ShowImage1))
                //{
                //    showImgList.Add(bllMall.GetImgUrl(sourceData.ShowImage1));
                //}
                //if (!string.IsNullOrEmpty(sourceData.ShowImage2))
                //{
                //    showImgList.Add(bllMall.GetImgUrl(sourceData.ShowImage2));
                //}
                //if (!string.IsNullOrEmpty(sourceData.ShowImage3))
                //{
                //    showImgList.Add(bllMall.GetImgUrl(sourceData.ShowImage3));
                //}
                //if (!string.IsNullOrEmpty(sourceData.ShowImage4))
                //{
                //    showImgList.Add(bllMall.GetImgUrl(sourceData.ShowImage4));
                //}
                //if (!string.IsNullOrEmpty(sourceData.ShowImage5))
                //{
                //    showImgList.Add(bllMall.GetImgUrl(sourceData.ShowImage5));
                //}

                #endregion

                #region 价格配置
                var priceConfigListSource = bllMall.GetList<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}'", bllMall.WebsiteOwner, productId));
                priceConfigListSource = priceConfigListSource.OrderBy(p => p.Date).ToList();
                var priceConfigList = from p in priceConfigListSource
                                      select new
                                      {

                                          id = p.AutoId,
                                          date = p.Date,
                                          price = p.Price

                                      };
                #endregion

                var data = new
                {
                    product_id = productInfo.PID,
                    product_title = productInfo.PName,
                    product_code = productInfo.ProductCode,
                    product_summary = productInfo.Summary,
                    is_onsale = (!string.IsNullOrEmpty(productInfo.IsOnSale) && productInfo.IsOnSale == "1") ? 1 : 0,
                    category_id = productInfo.CategoryId,
                    quote_price = productInfo.PreviousPrice,
                    price = productInfo.Price,
                    base_price = productInfo.BasePrice,
                    img_url = bllMall.GetImgUrl(productInfo.RecommendImg),
                    show_img_list = showImgList,
                    skus = skus,
                    have_property_list = GetHaveProperty(int.Parse(productInfo.PID)),
                    product_desc = productInfo.PDescription,
                    tags = productInfo.Tags,
                    sort = productInfo.Sort,
                    totalcount = bllMall.GetProductTotalStock(int.Parse(productInfo.PID)),
                    freight_template_id = productInfo.FreightTemplateId,//运费模板Id
                    unified_freight = productInfo.UnifiedFreight,//统一运费
                    relation_product_id = productInfo.RelationProductId,//关联商品ID
                    access_level = productInfo.AccessLevel,//访问等级
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
                    buy_time = productInfo.LimitBuyTime,
                    price_config_list = priceConfigList,
                    score = productInfo.Score,
                    is_cashpay_only = productInfo.IsCashPayOnly,
                    is_no_express = productInfo.IsNoExpress,
                    relevant_product_ids = productInfo.RelevantProductIds,
                    relevant_product = GetRelevantProduct(productInfo),
                    is_need_name_phone = productInfo.IsNeedNamePhone,
                    rebate_price_rate = productInfo.RebatePriceRate,
                    rebate_score_rate = productInfo.RebateScoreRate,
                    tab_ex_title1 = productInfo.TabExTitle1,
                    tab_ex_title2 = productInfo.TabExTitle2,
                    tab_ex_title3 = productInfo.TabExTitle3,
                    tab_ex_title4 = productInfo.TabExTitle4,
                    tab_ex_title5 = productInfo.TabExTitle5,
                    tab_ex_content1 = productInfo.TabExContent1,
                    tab_ex_content2 = productInfo.TabExContent2,
                    tab_ex_content3 = productInfo.TabExContent3,
                    tab_ex_content4 = productInfo.TabExContent4,
                    tab_ex_content5 = productInfo.TabExContent5,
                    type = productInfo.ArticleCategoryType,
                    is_appointment = productInfo.IsAppointment,
                    appointment_start_time = productInfo.AppointmentStartTime,
                    appointment_end_time = productInfo.AppointmentEndTime,
                    appointment_delivery_time = productInfo.AppointmentDeliveryTime,
                    weight = productInfo.Weight,
                    supplier_userid = productInfo.SupplierUserId,
                    address = productInfo.Address,
                    province = productInfo.Province,
                    province_code = productInfo.ProvinceCode,
                    city = productInfo.City,
                    city_code = productInfo.CityCode,
                    district = productInfo.District,
                    district_code = productInfo.DistrictCode,
                    ex1 = productInfo.Ex1,
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
                    ex19 = productInfo.Ex19

                };

                return ZentCloud.Common.JSONHelper.ObjectToJson(data);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        /// <summary>
        /// 批量设置访问等级
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string setAccessLevel(HttpContext context)
        {
            string ids = context.Request["pid"];
            string accessLevel = context.Request["access_level"];
            if (string.IsNullOrEmpty(ids))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "商品ID不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(accessLevel))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "access_level参数为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            int count = bllMall.Update(new WXMallProductInfo(), string.Format(" AccessLevel={0} ", Convert.ToInt32(accessLevel)), string.Format(" Websiteowner='{0}' AND  PID  in ({1})", bllMall.WebsiteOwner, ids));
            if (count > 0)
            {
                BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, ids, false);
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);

                apiResp.status = true;
                apiResp.msg = "操作成功";
            }
            else
            {
                apiResp.msg = "操作出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }
        /// <summary>
        /// 按商品分类批量设置访问级别
        /// </summary>
        /// <returns></returns>
        private string setAccessLevelByCategory(HttpContext context)
        {
            string categoryId = context.Request["category_id"];
            string accessLevel = context.Request["access_level"];

            if (string.IsNullOrEmpty(categoryId))
            {
                apiResp.msg = "商品分类ID为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(accessLevel))
            {
                apiResp.msg = "访问等级为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }

            string preIds = bllMall.GetCateAndChildIds(int.Parse(categoryId));
            string strWhere = string.Format(" WebsiteOwner='{0}' AND CategoryId in ({1})", bllMall.WebsiteOwner, preIds);

            if (bllMall.Update(new WXMallProductInfo(), string.Format(" AccessLevel={0} ", accessLevel), strWhere) > 0)
            {
                //刷新相关商品id及列表
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);
                //先直接这里处理，一般会操作比较少，未来操作多数据大再优化到线程
                var productList = bllMall.GetList<WXMallProductInfo>(strWhere);

                foreach (var item in productList)
                {
                    BLLRedis.ClearProduct(bllMall.WebsiteOwner, item.PID, false);
                }

                apiResp.msg = "设置访问级别成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "设置访问级别失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }

        /// <summary>
        /// 设置商品不能购买的日期段
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateBuyTime(HttpContext context)
        {
            string pIds = context.Request["pids"];
            string limitbuyTimes = context.Request["times"];
            if (string.IsNullOrEmpty(pIds))
            {
                apiResp.msg = "pids 参数不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            //if (string.IsNullOrEmpty(LimitbuyTimes))
            //{
            //    apiResp.msg = "时间参数为空,请检查";
            //    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            //}
            if (!string.IsNullOrEmpty(limitbuyTimes))
            {
                foreach (var item in limitbuyTimes.Split(','))
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        apiResp.msg = "请检查 times参数,多个日期用,分隔";
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                    }
                    else
                    {
                        //if (DateTime.Parse(item) < DateTime.Now)
                        //{
                        //    apiResp.msg = "日期需要大于现在日期,请重新选择日期";
                        //    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        //    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                        //}
                    }
                }
            }

            if (bllMall.UpdateProductBuyTime(pIds, limitbuyTimes))
            {

                apiResp.msg = "ok";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "设置购买时间失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }
        /// <summary>
        /// 更新商品标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateProductTag(HttpContext context)
        {
            string productIds = context.Request["pids"];
            string tagsName = context.Request["tags"];
            if (string.IsNullOrEmpty(productIds))
            {
                apiResp.msg = "商品id不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(tagsName))
            {
                apiResp.msg = "标签名称不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            int count = bllMall.Update(new WXMallProductInfo(), string.Format(" Tags='{0}' ", tagsName), string.Format(" WebsiteOwner='{0}' AND PID in ({1})", bllMall.WebsiteOwner, productIds));
            if (count > 0)
            {

                BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, productIds, false);
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);

                apiResp.msg = "操作完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "操作出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }
        /// <summary>
        /// 在原来的商品标签基础上增加标签
        /// </summary>
        /// <returns></returns>
        private string UpdateProductTagByAdd(HttpContext context)
        {
            string productIds = context.Request["pids"];
            string tagsName = context.Request["tags"];
            if (string.IsNullOrEmpty(productIds))
            {
                apiResp.msg = "商品id不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(tagsName))
            {
                apiResp.msg = "标签名称不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            List<string> tags = tagsName.Split(',').Select(p => p.Trim()).ToList();
            if (tags.Count > 0)
            {
                List<WXMallProductInfo> pList = bllMall.GetMultListByKey<WXMallProductInfo>("PID", productIds);
                for (int i = 0; i < pList.Count; i++)
                {
                    List<string> arrayList = new List<string>();
                    if (!string.IsNullOrEmpty(pList[i].Tags))
                    {
                        arrayList = pList[i].Tags.Split(',').Select(p => p.Trim()).ToList();
                        arrayList = arrayList.Where(p => !tags.Contains(p)).ToList();
                    }
                    arrayList.AddRange(tags);
                    string nTageName = "";
                    if (arrayList.Count > 0)
                    {
                        nTageName = ZentCloud.Common.MyStringHelper.ListToStr(arrayList, "", ",");
                    }
                    pList[i].Tags = nTageName;
                    bllMall.Update(new WXMallProductInfo(), string.Format(" Tags='{0}' ", pList[i].Tags), string.Format(" PID={0} ", pList[i].PID));
                }

                BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, productIds, false);
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);
            }
            apiResp.msg = "操作完成";
            apiResp.status = true;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }

        /// <summary>
        /// 在原来的商品基础上删除标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateProductTagByDelete(HttpContext context)
        {
            string productIds = context.Request["pids"];
            string tagsName = context.Request["tags"];
            if (string.IsNullOrEmpty(productIds))
            {
                apiResp.msg = "商品id不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(tagsName))
            {
                apiResp.msg = "标签名称不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            List<string> tags = tagsName.Split(',').Select(p => p.Trim()).ToList();
            if (tags.Count > 0)
            {
                List<WXMallProductInfo> pList = bllMall.GetMultListByKey<WXMallProductInfo>("PID", productIds);
                for (int i = 0; i < pList.Count; i++)
                {
                    if (string.IsNullOrEmpty(pList[i].Tags)) continue;
                    List<string> arrayList = pList[i].Tags.Split(',').Select(p => p.Trim()).ToList();
                    arrayList = arrayList.Where(p => !tags.Contains(p)).ToList();
                    string nTageName = "";
                    if (arrayList.Count > 0)
                    {
                        nTageName = ZentCloud.Common.MyStringHelper.ListToStr(arrayList, "", ",");
                    }
                    pList[i].Tags = nTageName;
                    bllMall.Update(new WXMallProductInfo(), string.Format(" Tags='{0}' ", pList[i].Tags), string.Format(" PID = {0} ", pList[i].PID));
                }
                BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, productIds, false);
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);
            }
            apiResp.msg = "操作完成";
            apiResp.status = true;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }

        /// <summary>
        /// 任务中心
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        private string GetProductThresholdCount(HttpContext context)
        {
            string userId = string.Empty;

            if (bllMall.GetCurrentUserInfo().UserType == 7)
            {
                userId = bllMall.GetCurrUserID();
            }

            WebsiteInfo website = bllMall.GetWebsiteInfoModelFromDataBase();

            //紧急库存
            int count = bllMall.GetProductStockThresholdCount(website.ProductStockThreshold, userId);


            //普通商品 待发货订单
            int deliveryCount = bllMall.GetOrderStatusCount(bllMall.WebsiteOwner, "待发货", 0, userId);


            //普通商品 待退款订单
            int refundCount = bllMall.GetOrderStatusCount(bllMall.WebsiteOwner, "退款退货", 0, userId);

            //供应商是否已确认
            int isCancel = bllMall.SupplierIsConfirm(bllMall.GetCurrUserID());
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                product_count = count,
                order_delivery_count = deliveryCount,
                order_refund_count = refundCount,
                is_confirm_supplier = isCancel
            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }


        /// <summary>
        /// 商品模型
        /// </summary>
        public class ProcuctModel
        {
            /// <summary>
            /// 商品编号
            /// </summary>
            public int product_id { get; set; }
            /// <summary>
            /// 商品编码
            /// </summary>
            public string product_code { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string product_title { get; set; }
            /// <summary>
            /// 分类ID
            /// </summary>
            public string category_id { get; set; }
            /// <summary>
            /// 吊牌价格
            /// </summary>
            public decimal quote_price { get; set; }
            /// <summary>
            /// 现价
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// 基础价
            /// </summary>
            public decimal base_price { get; set; }
            /// <summary>
            ///在否上架 1上架 0下架
            /// </summary>
            public int is_onsale { get; set; }
            /// <summary>
            /// 限时特卖开始时间
            /// </summary>
            public string promotion_start_time { get; set; }
            /// <summary>
            /// 限时特卖结束
            /// </summary>
            public string promotion_stop_time { get; set; }
            /// <summary>
            /// 商品介绍
            /// </summary>
            public string product_desc { get; set; }

            /// <summary>
            /// 标签
            /// </summary>
            public string tags { get; set; }
            /// <summary>
            /// 排序号
            /// </summary>
            public int sort { get; set; }
            /// <summary>
            /// 总库存
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 商品简介
            /// </summary>
            public string product_summary { get; set; }
            /// <summary>
            /// 展示图片
            /// </summary>
            public List<string> show_img_list { get; set; }

            /// <summary>
            /// 运费模板ID
            /// </summary>
            public int freight_template_id { get; set; }

            /// <summary>
            /// 统一运费
            /// </summary>
            public int unified_freight { get; set; }

            /// <summary>
            /// 关联商品ID
            /// </summary>
            public string relation_product_id { get; set; }
            /// <summary>
            /// sku 集合
            /// </summary>
            public List<SkuModel> skus { get; set; }

            /// <summary>
            /// 访问等级
            /// </summary>
            public int access_Level { get; set; }
            /// <summary>
            /// 商品类型Id
            /// </summary>
            public string product_type_id { get; set; }
            /// <summary>
            /// 扩展字段
            /// </summary>
            public string ex_article_id_1 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_title_1 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_id_2 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_title_2 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_id_3 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_title_3 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_id_4 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_title_4 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_id_5 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ex_article_title_5 { get; set; }
            /// <summary>
            /// 商品配置列表
            /// </summary>
            public List<ProductConfigModel> price_config_list { get; set; }
            /// <summary>
            /// 相关商品id,多个商品id用逗号分隔
            /// </summary>
            public string relevant_product_ids { get; set; }
            /// <summary>
            /// 积分
            /// </summary>
            public int score { get; set; }
            /// <summary>
            /// 金额部分是否仅现金支付 1仅现金支付
            /// </summary>
            public int is_cashpay_only { get; set; }
            /// <summary>
            /// 是否需要物流
            /// </summary>
            public int is_no_express { get; set; }
            /// <summary>
            /// 商品类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 需要姓名手机
            /// </summary>
            public string is_need_name_phone { get; set; }
            /// <summary>
            /// 参与返利比例，默认值 100%
            /// </summary>
            public decimal rebate_price_rate { get; set; }
            /// <summary>
            /// 参与返积分比例，默认值 100%
            /// </summary>
            public decimal rebate_score_rate { get; set; }
            /// <summary>
            /// tab1 标题
            /// </summary>
            public string tab_ex_title1 { get; set; }
            /// <summary>
            /// tab2 标题
            /// </summary>
            public string tab_ex_title2 { get; set; }
            /// <summary>
            /// tab3 标题
            /// </summary>
            public string tab_ex_title3 { get; set; }
            /// <summary>
            /// tab4 标题
            /// </summary>
            public string tab_ex_title4 { get; set; }
            /// <summary>
            /// tab5 标题
            /// </summary>
            public string tab_ex_title5 { get; set; }
            /// <summary>
            /// tab1 内容
            /// </summary>
            public string tab_ex_content1 { get; set; }
            /// <summary>
            /// tab2 内容
            /// </summary>
            public string tab_ex_content2 { get; set; }
            /// <summary>
            /// tab3 内容
            /// </summary>
            public string tab_ex_content3 { get; set; }
            /// <summary>
            /// tab4 内容
            /// </summary>
            public string tab_ex_content4 { get; set; }
            /// <summary>
            /// tab5 内容
            /// </summary>
            public string tab_ex_content5 { get; set; }
            /// <summary>
            /// 是否预购商品
            /// 1 预购商品
            /// 0 正常商品
            /// </summary>
            public string is_appointment { get; set; }
            /// <summary>
            /// 预购开始时间
            /// </summary>
            public string appointment_start_time { get; set; }
            /// <summary>
            /// 预购结束时间
            /// </summary>
            public string appointment_end_time { get; set; }
            /// <summary>
            /// 预购发货时间
            /// </summary>
            public string appointment_delivery_time { get; set; }
            /// <summary>
            /// 重量 Kg
            /// </summary>
            public string weight { get; set; }
            /// <summary>
            /// 供应商账号
            /// </summary>
            public string supplier_userid { get; set; }
            /// <summary>
            /// Houses:楼盘地址
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// 省份
            /// </summary>
            public string province { get; set; }
            /// <summary>
            /// 省份代码
            /// </summary>
            public string province_code { get; set; }
            /// <summary>
            /// 城市
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// 城市代码
            /// </summary>
            public string city_code { get; set; }
            /// <summary>
            /// 地区
            /// </summary>
            public string district { get; set; }
            /// <summary>
            /// 地区代码
            /// </summary>
            public string district_code { get; set; }
            /// <summary>
            /// 楼盘(Houses):建筑面积
            /// </summary>
            public string ex1 { get; set; }
            /// <summary>
            /// 楼盘(Houses):开盘时间
            /// </summary>
            public string ex2 { get; set; }
            /// <summary>
            /// 楼盘(Houses):楼盘类型(现房,期房)
            /// </summary>
            public string ex3 { get; set; }
            /// <summary>
            /// 楼盘(Houses):附近学校
            /// </summary>
            public string ex4 { get; set; }
            /// <summary>
            /// 楼盘(Houses):交房时间
            /// </summary>
            public string ex5 { get; set; }
            /// <summary>
            /// 楼盘(Houses):装修情况
            /// </summary>
            public string ex6 { get; set; }
            /// <summary>
            /// 楼盘(Houses):开发商
            /// </summary>
            public string ex7 { get; set; }
            /// <summary>
            /// 楼盘(Houses):产权年限
            /// </summary>
            public string ex8 { get; set; }
            /// <summary>
            /// 楼盘(Houses):物业公司
            /// </summary>
            public string ex9 { get; set; }
            /// <summary>
            /// 楼盘(Houses):建筑类型
            /// </summary>
            public string ex10 { get; set; }
            /// <summary>
            /// 楼盘(Houses):车位比例
            /// </summary>
            public string ex11 { get; set; }
            /// <summary>
            /// 楼盘(Houses):规划户数
            /// </summary>
            public string ex12 { get; set; }
            /// <summary>
            /// 楼盘(Houses):绿化率
            /// </summary>
            public string ex13 { get; set; }
            /// <summary>
            /// 楼盘(Houses):会员佣金
            /// </summary>
            public string ex14 { get; set; }
            /// <summary>
            /// 楼盘(Houses):会员优惠
            /// </summary>
            public string ex15 { get; set; }
            /// <summary>
            /// 楼盘(Houses):非会员优惠
            /// </summary>
            public string ex16 { get; set; }
            /// <summary>
            /// 楼盘(Houses):楼盘位置
            /// </summary>
            public string ex17 { get; set; }
            /// <summary>
            /// 楼盘(Houses):房型
            /// </summary>
            public string ex18 { get; set; }
            /// <summary>
            /// 楼盘(Houses):分类
            /// </summary>
            public string ex19 { get; set; }

        }





        /// <summary>
        /// SKU 模型
        /// </summary>
        public class SkuModel
        {
            /// <summary>
            /// sku 编号
            /// </summary>
            public int sku_id { get; set; }
            ///// <summary>
            ///// sku 属性集合 旧
            ///// </summary>
            //public string properties { get; set; }
            /// <summary>
            /// sku 属性集合 新
            /// </summary>
            public List<SkuProperties> properties { get; set; }
            /// <summary>
            /// 展示属性
            /// </summary>
            public string show_properties { get; set; }
            /// <summary>
            /// SKU 编码
            /// </summary>
            public string sku_sn { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// 基础价
            /// </summary>
            public decimal base_price { get; set; }

            /// <summary>
            /// sku 数量
            /// </summary>
            public int count { get; set; }

            /// <summary>
            /// 扩展试卷id
            /// </summary>
            public string ex_questionnaire_id { get; set; }
            /// <summary>
            /// 重量 Kg
            /// </summary>
            public string weight { get; set; }

            /// <summary>
            /// 图片 sku
            /// </summary>
            public string skuimg { get; set; }


        }

        /// <summary>
        /// sku属性模型
        /// </summary>
        public class SkuProperties
        {
            /// <summary>
            /// 特征量ID
            /// </summary>
            public int property_id { get; set; }
            /// <summary>
            /// 特征量名称
            /// </summary>
            public string property_name { get; set; }
            /// <summary>
            /// 特征量值ID
            /// </summary>
            public int property_value_id { get; set; }
            /// <summary>
            /// 特征量值名称
            /// </summary>
            public string property_value_name { get; set; }


        }

        /// <summary>
        /// 转换sku组合成字符串 返回示例 1:2:尺码:M;2:6:颜色:马其色 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetSkuPropertiesStr(List<SkuProperties> list)
        {
            System.Text.StringBuilder sbStr = new System.Text.StringBuilder();
            foreach (var item in list)
            {
                sbStr.AppendFormat("{0}:{1}:{2}:{3};", item.property_id, item.property_value_id, item.property_name, item.property_value_name);
            }
            return sbStr.ToString().TrimEnd(';');

        }

        /// <summary>
        /// 转换sku组合展示字符串 返回示例 尺码:M;颜色:马其色
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetSkuShowProperties(List<SkuProperties> list)
        {
            System.Text.StringBuilder sbStr = new System.Text.StringBuilder();
            foreach (var item in list)
            {
                sbStr.AppendFormat("{0}:{1};", item.property_name, item.property_value_name);
            }
            return sbStr.ToString().TrimEnd(';');

        }
        /// <summary>
        /// 转换sku属性字符串成对象 
        /// </summary>
        /// <param name="skuPropStr">示例 1:2:尺码:M;2:6:颜色:马其色 </param>
        /// <returns></returns>
        private List<SkuProperties> GetSkuProperties(string skuPropStr)
        {
            List<SkuProperties> list = new List<SkuProperties>();
            try
            {


                foreach (var item in skuPropStr.Split(';'))
                {
                    SkuProperties model = new SkuProperties();
                    string[] propArry = item.Split(':');
                    BLLJIMP.Model.ProductProperty property = bllMall.GetProductProperty(int.Parse(propArry[0]));
                    BLLJIMP.Model.ProductPropertyValue propValue = bllMall.GetProductPropertyValue(int.Parse(propArry[1]));
                    model.property_id = property.PropID;
                    model.property_name = property.PropName.Trim();
                    model.property_value_id = propValue.PropValueId;
                    model.property_value_name = propValue.PropValue.Trim();
                    list.Add(model);

                }
                return list;
            }
            catch (Exception ex)
            {

                return list;
            }

        }



        /// <summary>
        /// 检查SKU属性是否重复
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckSkuPropertyiesIsRepeat(List<SkuProperties> list)
        {

            var distinctCountProperty = (from p in list select p.property_id).Distinct().ToList().Count;
            if (distinctCountProperty != list.Count)
            {
                return false;

            }
            var distinctCountPropertyValue = (from p in list select p.property_value_id).Distinct().ToList().Count;
            if (distinctCountPropertyValue != list.Count)
            {
                return false;

            }

            return true;

        }

        /// <summary>
        /// 获取商品拥有的属性集合 如颜色 有几种颜色,尺寸有几种尺寸
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private List<PropertyModel> GetHaveProperty(int productId)
        {
            try
            {
                var skuList = bllMall.GetProductSkuList(productId);
                List<string> propIdList = new List<string>();//获取商品拥有的所有属性ID集合
                Dictionary<int, string> propKeyValue = new Dictionary<int, string>();
                foreach (var sku in skuList)
                {

                    if (!string.IsNullOrEmpty(sku.PropValueIdEx1) && !string.IsNullOrEmpty(sku.PropValueIdEx2))
                    {
                        sku.Props = bllMall.GetProductProperties(sku.SkuId);//兼容efast同步 无用
                    }

                    foreach (var item in sku.Props.Split(';'))
                    {
                        string propId = item.Split(':')[0];
                        if (!propIdList.Contains(propId))
                        {
                            propIdList.Add(propId);
                            string propName = item.Split(':')[2];
                            propKeyValue.Add(int.Parse(propId), propName);

                        }
                    }

                }


                List<PropertyModel> data = new List<PropertyModel>();
                foreach (var propId in propIdList)
                {
                    PropertyModel model = new PropertyModel();
                    Property prop = new Property();
                    prop.property_id = int.Parse(propId);
                    prop.property_name = propKeyValue[int.Parse(propId)].Trim();
                    model.property = prop;
                    model.property_value_list = new List<PropertyValueModel>();
                    foreach (var sku in skuList)
                    {

                        foreach (var item in sku.Props.Split(';'))
                        {
                            string propIdNew = item.Split(':')[0];
                            string propValue = item.Split(':')[1];
                            string propValueName = item.Split(':')[3];


                            if (propId == propIdNew)
                            {
                                PropertyValueModel valueModel = new PropertyValueModel();
                                valueModel.property_value_id = int.Parse(propValue);

                                BLLJIMP.Model.ProductPropertyValue propValueInfo = bllMall.GetProductPropertyValue(int.Parse(propValue));
                                //valueModel.property_value_name = propValueName.Trim();
                                valueModel.property_value_name = propValueInfo.PropValue.Trim();
                                if (model.property_value_list.Where(p => p.property_value_id == valueModel.property_value_id).Count() == 0)
                                {
                                    model.property_value_list.Add(valueModel);
                                }


                            }
                        }

                    }

                    data.Add(model);

                }


                return data;
            }
            catch (Exception)
            {
                return new List<PropertyModel>();

            }

        }


        /// <summary>
        /// 获取相关商品
        /// </summary>
        /// <param name="prodcutInfo"></param>
        /// <returns></returns>
        private dynamic GetRelevantProduct(WXMallProductInfo prodcutInfo)
        {

            if (!string.IsNullOrEmpty(prodcutInfo.RelevantProductIds))
            {

                string pids = "'" + prodcutInfo.RelevantProductIds.Replace(",", "','") + "'";
                var productList = bllMall.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' And PID in({1})", bllMall.WebsiteOwner, pids));
                var data = from p in productList
                           select new
                           {
                               product_id = p.PID,
                               title = p.PName,
                               price = p.Price,
                               img_url = p.RecommendImg


                           };

                return data;


            }
            return null;

        }

        /// <summary>
        /// 商品拥有的不重复的属性集合
        /// </summary>
        private class PropertyModel
        {
            // /// <summary>
            // /// 属性ID
            // /// </summary>
            //public int property_id { get; set; }
            // /// <summary>
            // /// 属性
            // /// </summary>
            //public string property_name { get; set; }
            public Property property { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public List<PropertyValueModel> property_value_list { get; set; }

        }

        private class Property
        {
            /// <summary>
            /// 属性ID
            /// </summary>
            public int property_id { get; set; }
            /// <summary>
            /// 属性
            /// </summary>
            public string property_name { get; set; }



        }



        /// <summary>
        /// 特征量值
        /// </summary>
        private class PropertyValueModel
        {
            /// <summary>
            /// 特征量值ID
            /// </summary>
            public int property_value_id { get; set; }
            /// <summary>
            /// 特征量值
            /// </summary>
            public string property_value_name { get; set; }

        }

        /// <summary>
        /// 商品价格配置模型
        /// </summary>
        public class ProductConfigModel
        {

            /// <summary>
            /// ID
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 日期 格式2016/05/30
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal price { get; set; }


        }


    }
}