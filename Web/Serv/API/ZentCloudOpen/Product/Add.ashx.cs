using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product
{
    /// <summary>
    /// 添加商品
    /// </summary>
    public class Add : BaseHanderOpen
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {


            string data = context.Request["data"];
            //data = System.IO.File.ReadAllText("D:\\p.txt",System.Text.Encoding.UTF8);
            //商品模型
            ProcuctModel productRequestModel;
            //商品模型
            try
            {
                productRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<ProcuctModel>(data);

            }
            catch (Exception ex)
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = "json格式错误,请检查。错误信息:" + ex.Message;
                resp.result = "错误详细信息:" + ex.ToString();
                bllMall.ContextResponse(context, resp);
                return;

            }
            //
            //数据检查
            if (string.IsNullOrEmpty(productRequestModel.product_title))
            {

                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = "商品名称必填";
                bllMall.ContextResponse(context, resp);
                return;

            }
            if (productRequestModel.quote_price == 0)
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = "商品原价必填";
                bllMall.ContextResponse(context, resp);
                return;
            }

            //商品价格和积分必填一项            
            if (productRequestModel.price == 0 && productRequestModel.score == 0)
            {


                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = "商品价格和积分必填一项";
                bllMall.ContextResponse(context, resp);
                return;
            }

            if (productRequestModel.show_img_list.Count == 0)
            {

                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = "请上传商品图片";
                bllMall.ContextResponse(context, resp);
                return;
            }

            //#region 外部库存
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
            //                    resp.errcode = 1;
            //                    resp.errmsg = "商品条码不存在,请检查。";
            //                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
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
            //#endregion

            foreach (var sku in productRequestModel.skus)
            {
                if (sku.price <= 0)
                {

                    resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.msg = "价格不能小于0";
                    bllMall.ContextResponse(context, resp);
                    return;
                }
                if (!string.IsNullOrEmpty(sku.sku_sn))
                {
                    ProductSku productSku = bllMall.GetProductSkuBySkuSn(sku.sku_sn);
                    if (productSku != null)
                    {
                        resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        resp.msg = "sku_sn重复";
                        bllMall.ContextResponse(context, resp);
                        return;

                    }

                }


            }
            if (productRequestModel.unified_freight > 0 && productRequestModel.freight_template_id > 0)
            {

                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = "不能同时设置统一运费和运费模板";
                bllMall.ContextResponse(context, resp);
                return;
            }
            //if (bllMall.GetWebsiteInfoModelFromDataBase(bllMall.WebsiteOwner).RequiredSupplier == 1)
            //{
            //    if (string.IsNullOrEmpty(productRequestModel.supplier_userid))
            //    {
            //        resp.errcode = 1;
            //        resp.errmsg = "请选择商户";
            //        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //    }

            //}


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
                productModel.Price =Convert.ToDecimal(productRequestModel.price);
                productModel.BasePrice = Convert.ToDecimal(productRequestModel.base_price);
                productModel.CategoryId = productRequestModel.category_id;
                productModel.InsertDate = DateTime.Now;
                productModel.IsOnSale = productRequestModel.is_onsale.ToString();
                productModel.Stock = Convert.ToInt32(productRequestModel.totalcount);
                productModel.RecommendImg = productRequestModel.show_img_list[0];
                productModel.WebsiteOwner = bllMall.WebsiteOwner;
                productModel.Tags = productRequestModel.tags;
                productModel.Sort = Convert.ToInt32(productRequestModel.sort);
                productModel.UserID = bllMall.WebsiteOwner;
                productModel.PreviousPrice = Convert.ToDecimal(productRequestModel.quote_price);
                productModel.Summary = productRequestModel.product_summary;
                productModel.ProductCode = productRequestModel.product_sn;
                productModel.UnifiedFreight = Convert.ToInt32(productRequestModel.unified_freight);
                productModel.FreightTemplateId = Convert.ToInt32(productRequestModel.freight_template_id);
                productModel.LastUpdate = DateTime.Now;
                productModel.RelationProductId = productRequestModel.relation_product_id;
                productModel.AccessLevel = Convert.ToInt32(productRequestModel.access_Level);
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
                productModel.Score = Convert.ToInt32(productRequestModel.score);
                productModel.IsCashPayOnly = Convert.ToInt32(productRequestModel.is_cashpay_only);

                productModel.IsNoExpress = Convert.ToInt32(productRequestModel.is_no_express);

                productModel.RelevantProductIds = productRequestModel.relevant_product_ids;

                productModel.RebatePriceRate = Convert.ToDecimal(productRequestModel.rebate_price_rate);
                productModel.RebateScoreRate = Convert.ToDecimal(productRequestModel.rebate_score_rate);

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


                        resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        resp.msg = "预购开始时间,结束时间,发货时间必填";
                        bllMall.ContextResponse(context, resp);
                        return;
                    }

                    //if (Convert.ToDateTime(productModel.AppointmentStartTime) < DateTime.Now)
                    //{
                    //    resp.errcode = 1;
                    //    resp.errmsg = "预购开始时间需要晚于当前时间";
                    //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    //}
                    if (Convert.ToDateTime(productModel.AppointmentStartTime) >= Convert.ToDateTime(productModel.AppointmentEndTime))
                    {


                        resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        resp.msg = "预购结束时间需要晚于开始时间";
                        bllMall.ContextResponse(context, resp);
                        return;
                    }
                    if (Convert.ToDateTime(productModel.AppointmentDeliveryTime) < Convert.ToDateTime(productModel.AppointmentEndTime))
                    {


                        resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        resp.msg = "预购发货时间需要晚于结束时间";
                        bllMall.ContextResponse(context, resp);
                        return;
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
                //#region 楼盘字段
                //productModel.Address = productRequestModel.address;
                //productModel.Province = productRequestModel.province;
                //productModel.ProvinceCode = productRequestModel.province_code;
                //productModel.City = productRequestModel.city;
                //productModel.CityCode = productRequestModel.city_code;
                //productModel.District = productRequestModel.district;
                //productModel.DistrictCode = productRequestModel.district_code;
                //productModel.Ex1 = productRequestModel.ex1;
                //productModel.Ex2 = productRequestModel.ex2;
                //productModel.Ex3 = productRequestModel.ex3;
                //productModel.Ex4 = productRequestModel.ex4;
                //productModel.Ex5 = productRequestModel.ex5;
                //productModel.Ex6 = productRequestModel.ex6;
                //productModel.Ex7 = productRequestModel.ex7;
                //productModel.Ex8 = productRequestModel.ex8;
                //productModel.Ex9 = productRequestModel.ex9;
                //productModel.Ex10 = productRequestModel.ex10;
                //productModel.Ex11 = productRequestModel.ex11;
                //productModel.Ex12 = productRequestModel.ex12;
                //productModel.Ex13 = productRequestModel.ex13;
                //productModel.Ex14 = productRequestModel.ex14;
                //productModel.Ex15 = productRequestModel.ex15;
                //productModel.Ex16 = productRequestModel.ex16;
                //productModel.Ex17 = productRequestModel.ex17;
                //productModel.Ex18 = productRequestModel.ex18;
                //productModel.Ex19 = productRequestModel.ex19;
                //#endregion
                if (!bllMall.Add(productModel, tran))
                {
                    tran.Rollback();
                    resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.msg = "插入商品表失败";
                    bllMall.ContextResponse(context, resp);
                    return;

                }

                if (productRequestModel.skus != null && productRequestModel.skus.Count > 0)//没有sku 此商品为单品,加默认sku
                {
                    #region 增加sku
                    foreach (var sku in productRequestModel.skus)
                    {

                        if (!CheckSkuPropertyiesIsRepeat(sku.properties))
                        {
                            tran.Rollback();
                            resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                            resp.msg = "sku属性重复,请检查";
                            bllMall.ContextResponse(context, resp);
                            return;
                        }
                        //sku.properties = sku.properties.Replace("；", ";");
                        
                        if (!string.IsNullOrEmpty(sku.show_properties))
                        {
                            sku.show_properties = sku.show_properties.Replace("；", ";");
                        }
                        ProductSku productSku = new ProductSku();
                        productSku.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                        productSku.ProductId = int.Parse(productModel.PID);
                        productSku.InsertDate = DateTime.Now;
                        productSku.Price = Convert.ToDecimal(sku.price);
                        productSku.BasePrice = Convert.ToDecimal(sku.base_price);
                        productSku.Props = GetSkuPropertiesStr(sku.properties);
                        productSku.ShowProps = GetSkuShowProperties(sku.properties);
                        productSku.Stock = Convert.ToInt32(sku.count);
                        productSku.WebSiteOwner = bllMall.WebsiteOwner;
                        productSku.SkuSN = sku.sku_sn;
                        productSku.OutBarCode = sku.sku_sn;
                        productSku.ArticleCategoryType = "Mall";
                        productSku.Weight = !string.IsNullOrEmpty(sku.weight) ? decimal.Parse(sku.weight) : 0;
                        productSku.SkuImg = sku.skuimg;

                        //#region 课程检查
                        //if (productModel.ArticleCategoryType == "Course")
                        //{
                        //    if (!string.IsNullOrEmpty(sku.ex_questionnaire_id))
                        //    {
                        //        if (bllMall.GetCount<ProductSku>(string.Format("ExQuestionnaireID={0}", sku.ex_questionnaire_id)) > 0)
                        //        {
                        //            tran.Rollback();
                        //            resp.errcode = 1;
                        //            resp.errmsg = productSku.ShowProps + "  试卷已经被别的证书使用,请换另外一个试卷";
                        //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        //        }
                        //        productSku.ExQuestionnaireID = sku.ex_questionnaire_id;
                        //    }
                        //}
                        //#endregion

                        if (!bllMall.Add(productSku))
                        {
                            tran.Rollback();
                            resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                            resp.msg = "插入商品SKU表失败";
                            bllMall.ContextResponse(context, resp);
                            return;
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
                        resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        resp.msg = "插入商品SKU表失败";
                        bllMall.ContextResponse(context, resp);

                    }
                    #endregion


                }



                //#region 价格配置
                //if (productRequestModel.price_config_list != null && productRequestModel.price_config_list.Count > 0)
                //{
                //    foreach (var item in productRequestModel.price_config_list)
                //    {
                //        if (item.price <= 0)
                //        {
                //            tran.Rollback();
                //            resp.errcode = 1;
                //            resp.errmsg = string.Format("日期{0}价格需大于0", item.date);
                //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                //        }
                //        if (bllMall.GetCount<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}' And Date='{2}'", bllMall.WebsiteOwner, productModel.PID, item.date)) > 0)
                //        {
                //            tran.Rollback();
                //            resp.errcode = 1;
                //            resp.errmsg = "日期重复";
                //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                //        }
                //        ProductPriceConfig model = new ProductPriceConfig();
                //        model.WebsiteOwner = bllMall.WebsiteOwner;
                //        model.Date = item.date;
                //        model.Price = item.price;
                //        model.ProductId = productModel.PID;
                //        if (!bllMall.Add(model))
                //        {
                //            //tran.Rollback(); 
                //            resp.errcode = 1;
                //            resp.errmsg = "添加商品价格配置失败";
                //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                //        }


                //    }

                //}
                //#endregion

                tran.Commit();
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加商品[id=" + productModel.PID + "]");

                ////更新商品价格区间
                //bllWXMallProduct.UpdateProductPriceRange(productModel.PID, bllWXMallProduct.WebsiteOwner);

                ZentCloud.BLLJIMP.BLLRedis.ClearProductList(bllMall.WebsiteOwner);
                resp.status = true;
                resp.msg = "ok";
                resp.result = new
                {
                    product_id = productModel.PID
                };
                bllMall.ContextResponse(context, resp);

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.msg = ex.Message;
                resp.result = ex.ToString();
                bllMall.ContextResponse(context, resp);

            }
            bllMall.ContextResponse(context, resp);

        }

        /// <summary>
        /// 商品模型
        /// </summary>
        public class ProcuctModel
        {
            /// <summary>
            /// 商品编号
            /// </summary>
            public string product_id { get; set; }
            /// <summary>
            /// 商品编码
            /// </summary>
            public string product_sn { get; set; }
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
            public decimal? quote_price { get; set; }
            /// <summary>
            /// 现价
            /// </summary>
            public decimal? price { get; set; }
            /// <summary>
            /// 基础价
            /// </summary>
            public decimal? base_price { get; set; }
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
            public int? sort { get; set; }
            /// <summary>
            /// 总库存
            /// </summary>
            public int? totalcount { get; set; }
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
            public int? freight_template_id { get; set; }

            /// <summary>
            /// 统一运费
            /// </summary>
            public int? unified_freight { get; set; }

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
            public int? access_Level { get; set; }
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
            public int? score { get; set; }
            /// <summary>
            /// 金额部分是否仅现金支付 1仅现金支付
            /// </summary>
            public int? is_cashpay_only { get; set; }
            /// <summary>
            /// 是否需要物流
            /// </summary>
            public int? is_no_express { get; set; }
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
            public decimal? rebate_price_rate { get; set; }
            /// <summary>
            /// 参与返积分比例，默认值 100%
            /// </summary>
            public decimal? rebate_score_rate { get; set; }
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
            public int? sku_id { get; set; }
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
            public decimal? price { get; set; }
            /// <summary>
            /// 基础价
            /// </summary>
            public decimal? base_price { get; set; }

            /// <summary>
            /// sku 数量
            /// </summary>
            public int? count { get; set; }

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
                var property = bllMall.Get<BLLJIMP.Model.ProductProperty>(string.Format(" PropName='{0}' And WebsiteOwner='{1}'",item.property_name,bllMall.WebsiteOwner));
                if (property==null)
                {
                    property = new ProductProperty();
                    property.WebSiteOwner = bllMall.WebsiteOwner;
                    property.InsertDate = DateTime.Now;
                    property.Modified = DateTime.Now;
                    property.PropName = item.property_name;
                    property.PropID = int.Parse(bllMall.GetGUID(ZentCloud.BLLJIMP.TransacType.AddProductProperty));
                    bllMall.Add(property);


                }

                var propertyValue = bllMall.Get<BLLJIMP.Model.ProductPropertyValue>(string.Format("WebSiteOwner='{0}' And PropID={1} And PropValue='{2}'", bllMall.WebsiteOwner, property.PropID, item.property_value_name));
                if (propertyValue == null)
                {
                     propertyValue = new ProductPropertyValue();
                    propertyValue.PropValueId = int.Parse(bllMall.GetGUID(ZentCloud.BLLJIMP.TransacType.AddProductPropertyValue));
                    propertyValue.InsertDate = DateTime.Now;
                    propertyValue.PropID = property.PropID;
                    propertyValue.PropValue = item.property_value_name;
                    propertyValue.WebSiteOwner = bllMall.WebsiteOwner;
                    bllMall.Add(propertyValue);

                }

                sbStr.AppendFormat("{0}:{1}:{2}:{3};", property.PropID, propertyValue.PropValueId, item.property_name, item.property_value_name);
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