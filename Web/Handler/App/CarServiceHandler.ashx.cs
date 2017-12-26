using System;
using System.Reflection;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ZCJson.Linq;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// CarServiceHandler 的摘要说明
    /// </summary>
    public class CarServiceHandler : IHttpHandler, IReadOnlySessionState
    {

        DefaultResponse resp = new DefaultResponse();
        BLLJIMP.BLLCarLibrary bll = new BLLJIMP.BLLCarLibrary();
        BLLJIMP.BLLArticleCategory bllCate = new BLLJIMP.BLLArticleCategory();
        BLLJIMP.BLLWeixin bllWX = new BLLJIMP.BLLWeixin();
        BLLJIMP.BLLSystemNotice bllNotice = new BLLJIMP.BLLSystemNotice();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLKeyValueData bllKV = new BLLJIMP.BLLKeyValueData();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        ///// <summary>
        ///// 网站所有者
        ///// </summary>
        //private string webSiteOwner;

        #region 入口
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bll.IsLogin)
                {
                    currentUserInfo = bll.GetCurrentUserInfo();
                    //webSiteOwner = bll.WebsiteOwner;
                    string action = context.Request["action"];
                    //利用反射找到未知的调用的方法
                    if (!string.IsNullOrEmpty(action))
                    {
                        MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                        result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                    }
                    else
                    {

                        resp.errmsg = "action not exist";
                        result = Common.JSONHelper.ObjectToJson(resp);
                    }

                }
                else
                {
                    resp.isSuccess = false;
                    resp.errmsg = "未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }

            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }
        }

        #endregion

        #region 车型库管理
        /// <summary>
        /// 获取品牌及车系
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RefreshBrandSeriesAPIData(HttpContext context)
        {
            var result = string.Empty;

            //查询当前用户的appkey
            //调用接口获取远程数据

            
            var baseUrl = "http://apis.haoservice.com/lifeservice/car/";
            var appKey = bllKV.GetDataVaule(BLLJIMP.Enums.KeyVauleDataType.HaoServiceAppKey, "GetCarSeries", bll.WebsiteOwner);
            var api = baseUrl + "GetSeries?key=" + appKey;
            var modelApi = baseUrl + "GetModel/?key=" + appKey + "&id=";

            var respStr = MySpider.MySpider.GetPageSourceForUTF8(api);

            if (!string.IsNullOrWhiteSpace(respStr))
            {
                //var respObj = JsonConvert.DeserializeObject<BLLJIMP.Model.HaoService.BrandResp>(respStr);

                var respObj = ZentCloud.Common.JSONHelper.JsonToModel<BLLJIMP.Model.HaoService.BrandResp>(respStr);

                var reason = respObj.reason;

                //保存品牌
                List<CarBrandInfo> brandList = new List<CarBrandInfo>();

                //保存车系分类
                List<CarSeriesCateInfo> seriesCateList = new List<CarSeriesCateInfo>();

                //保存车系类别
                List<CarSeriesInfo> seriesList = new List<CarSeriesInfo>();

                List<CarModelCateInfo> modelCateList = new List<CarModelCateInfo>();

                List<CarModelInfo> modelList = new List<CarModelInfo>();

                //dynamic resp = JsonConvert.DeserializeObject(respStr);
                var updator = this.currentUserInfo == null ? "" : this.currentUserInfo.UserID;
                foreach (var i in respObj.result)
                {
                    brandList.Add(new CarBrandInfo()
                    {
                        CarBrandId = i.I,
                        CarBrandName = i.N,
                        FirstLetter = i.L,
                        UpdateTime = DateTime.Now,
                        Updator = updator
                    });//保存品牌

                    foreach (var j in i.List)
                    {
                        seriesCateList.Add(new CarSeriesCateInfo()
                        {
                            CarBrandId = i.I,
                            CarSeriesCateId = j.I,
                            CarSeriesCateName = j.N,
                            UpdateTime = DateTime.Now,
                            Updator = updator
                        });//保存车系分类

                        foreach (var k in j.List)
                        {
                            seriesList.Add(new CarSeriesInfo()
                            {
                                CarBrandId = i.I,
                                CarSeriesCateId = j.I,
                                CarSeriesId = k.I,
                                CarSeriesName = k.N,
                                UpdateTime = DateTime.Now,
                                Updator = updator
                            });//保存车系类别
                        }
                    }
                }

                //保存到数据库，id不存在的则新增，否则跳过
                int totalCount = 0;

                //品牌数据不更新
                //totalCount += bll.UpdateBrandInfo(brandList);
                totalCount += bll.UpdateSeriesCateInfo(seriesCateList);
                totalCount += bll.UpdateSeriesInfo(seriesList);


                List<int> buyBrandIdList = new List<int>();
                List<int> serviceBrandList = new List<int>();
                List<int> allBrandIdList = new List<int>();

                bll.GetWebSiteBrandIdList(bll.WebsiteOwner, out buyBrandIdList, out serviceBrandList, out allBrandIdList);

                //获取车型数据
                var webSeriesList = seriesList.Where(p => allBrandIdList.Contains(p.CarBrandId));

                foreach (var series in webSeriesList)
                {
                    respStr = MySpider.MySpider.GetPageSourceForUTF8(modelApi + series.CarSeriesId.ToString());
                    dynamic apiData = JsonConvert.DeserializeObject(respStr);

                    if (apiData["reason"].ToString() == "Success")
                    {
                        dynamic apiResultData = apiData["result"]["List"];

                        foreach (var apiModelCate in apiResultData)
                        {
                            var cateId = int.Parse(apiModelCate["I"].ToString());
                            modelCateList.Add(new CarModelCateInfo()
                            {
                                CarSeriesId = series.CarSeriesId,
                                CarModelCateId = cateId,
                                CarModelCateName = apiModelCate["N"].ToString(),
                                UpdateTime = DateTime.Now,
                                Updator = updator
                            });

                            dynamic apiYearList = apiModelCate["List"];

                            foreach (var apiYearData in apiYearList)
                            {
                                var year = int.Parse(apiYearData["I"].ToString());

                                foreach (var apiCarModel in apiYearData["List"])
                                {
                                    var modelId = int.Parse(apiCarModel["I"].ToString());
                                    var modelName = apiCarModel["N"].ToString();
                                    var modelPrice = int.Parse(apiCarModel["P"].ToString());

                                    modelList.Add(new CarModelInfo()
                                    {
                                        CarSeriesCateId = series.CarSeriesCateId.Value,
                                        CarSeriesId = series.CarSeriesId,
                                        CarBrandId = series.CarBrandId,
                                        CarModelCateId = cateId,
                                        Year = year,
                                        CarModelId = modelId,
                                        CarModelName = modelName,
                                        GuidePrice = modelPrice,
                                        UpdateTime = DateTime.Now,
                                        Updator = updator
                                    });
                                }
                            }
                        }
                    }

                }

                totalCount += bll.UpdateModelCateInfo(modelCateList);
                totalCount += bll.UpdateModelInfo(modelList);

                resp.isSuccess = true;
                resp.returnValue = totalCount.ToString();

                result = MySpider.JSONHelper.ObjectToJson(resp);
            }

            return result;
        }

        /// <summary>
        /// 查询品牌数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryBrand(HttpContext context)
        {
            string firstLetter = context.Request["firstLetter"];
            var isMustInWebsite = Convert.ToInt32(context.Request["isMustInWebsite"]) == 1;

            int totalCount = 0,
                pageSize = Convert.ToInt32(context.Request["pageSize"]),
                pageIndex = Convert.ToInt32(context.Request["pageIndex"]);

            var dataList = bll.QueryBrand(out totalCount, bll.WebsiteOwner, pageSize, pageIndex, firstLetter, isMustInWebsite);

            return MySpider.JSONHelper.ObjectToJson(new
            {
                totalCount = totalCount,
                dataList = dataList
            });
        }

        private string GetSeriesCateList(HttpContext context)
        {
            var brandId = Convert.ToInt32(context.Request["brandId"]);

            var list = bll.GetSeriesCateList(brandId);

            return MySpider.JSONHelper.ObjectToJson(list);
        }

        private string GetSeriesList(HttpContext context)
        {
            var cateId = Convert.ToInt32(context.Request["cateId"]);
            var brandId = Convert.ToInt32(context.Request["brandId"]);
            var list = bll.GetSeriesList(cateId, brandId);

            return MySpider.JSONHelper.ObjectToJson(list);
        }

        private string GetModelCateList(HttpContext context)
        {
            var seriesId = Convert.ToInt32(context.Request["seriesId"]);

            var list = bll.GetModelCateList(seriesId);

            return MySpider.JSONHelper.ObjectToJson(list);
        }

        private string GetModelList(HttpContext context)
        {
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var modelCateId = Convert.ToInt32(context.Request["modelCateId"]);
            var seriesId = Convert.ToInt32(context.Request["seriesId"]);
            int totalCount = 0;

            var list = bll.GetModelList(out totalCount, pageSize, pageIndex, modelCateId, seriesId);

            return MySpider.JSONHelper.ObjectToJson(new
            {
                totalCount = totalCount,
                list = list
            });
        }

        /// <summary>
        /// 编辑品牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditBrand(HttpContext context)
        {
            string brandImg = context.Request["brandImg"];
            int brandId = Convert.ToInt32(context.Request["brandId"]),
                isBuy = Convert.ToInt32(context.Request["isBuy"]),
                isService = Convert.ToInt32(context.Request["isService"]);

            if (!string.IsNullOrWhiteSpace(brandImg))
            {
                bll.UpdateBrandImg(brandId, brandImg);
            }

            bll.RemoveWebSiteBrand(bll.WebsiteOwner, brandId, BLLJIMP.Enums.CommRelationType.BuyCarBrand);
            bll.RemoveWebSiteBrand(bll.WebsiteOwner, brandId, BLLJIMP.Enums.CommRelationType.ServiceCarBrand);

            if (isBuy.Equals(1))
            {
                bll.SetWebSiteBrand(bll.WebsiteOwner, brandId, BLLJIMP.Enums.CommRelationType.BuyCarBrand);
            }

            if (isService.Equals(1))
            {
                bll.SetWebSiteBrand(bll.WebsiteOwner, brandId, BLLJIMP.Enums.CommRelationType.ServiceCarBrand);
            }

            resp.isSuccess = true;

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        #endregion

        #region 配件管理

        private string AddParts(HttpContext context)
        {

            var partName = context.Request["partName"];
            var price = Convert.ToDouble(context.Request["price"]);
            var carBrandId = Convert.ToInt32(context.Request["carBrandId"]);
            var carSeriesCateId = Convert.ToInt32(context.Request["carSeriesCateId"]);
            var carSeriesId = Convert.ToInt32(context.Request["carSeriesId"]);

            var carModelIdStr = context.Request["carModelId"];

            var count = Convert.ToInt32(context.Request["count"]);

            var partsCateId = Convert.ToInt32(context.Request["partsCateId"]);
            var partsBrandId = Convert.ToInt32(context.Request["partsBrandId"]);

            var partsCateName = context.Request["partsCateName"];
            var partsBrandName = context.Request["partsBrandName"];
            var partsSpecs = context.Request["partsSpecs"];

            int totalCount = 0;
            foreach (var item in carModelIdStr.Split(','))
            {
                var carModelId = Convert.ToInt32(item);

                if (bll.AddPars(bll.WebsiteOwner, partName, price, carBrandId, carSeriesCateId,
                    carSeriesId, carModelId, count, currentUserInfo.UserID, partsCateId, partsCateName, partsBrandId, partsBrandName, partsSpecs))
                {
                    totalCount++;
                }

            }

            resp.returnValue = totalCount.ToString();
            resp.isSuccess = true;

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        private string EditParts(HttpContext context)
        {

            var partId = Convert.ToInt32(context.Request["partId"]);

            var partName = context.Request["partName"];
            var price = Convert.ToDouble(context.Request["price"]);
            var carBrandId = Convert.ToInt32(context.Request["carBrandId"]);
            var carSeriesCateId = Convert.ToInt32(context.Request["carSeriesCateId"]);
            var carSeriesId = Convert.ToInt32(context.Request["carSeriesId"]);
            var carModelId = Convert.ToInt32(context.Request["carModelId"]);
            var count = Convert.ToInt32(context.Request["count"]);

            var partsCateId = Convert.ToInt32(context.Request["partsCateId"]);
            var partsBrandId = Convert.ToInt32(context.Request["partsBrandId"]);

            var partsCateName = context.Request["partsCateName"];
            var partsBrandName = context.Request["partsBrandName"];
            var partsSpecs = context.Request["partsSpecs"];

            resp.isSuccess = bll.EditPart(partId, partName, price, carBrandId, carSeriesCateId, carSeriesId, carModelId, count, currentUserInfo.UserID, partsCateId, partsCateName, partsBrandId, partsBrandName, partsSpecs);

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        private string GetPartsList(HttpContext context)
        {
            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var carModelId = Convert.ToInt32(context.Request["carModelId"]);

            //int brandId = 0,int seriesCateId = 0,int seriesId = 0)

            var brandId = Convert.ToInt32(context.Request["brandId"]);
            var seriesCateId = Convert.ToInt32(context.Request["seriesCateId"]);
            var seriesId = Convert.ToInt32(context.Request["seriesId"]);

            int cateId = Convert.ToInt32(context.Request["cateId"]);
            int partsBrandId = Convert.ToInt32(context.Request["partsBrandId"]);
            var totalCount = 0;

            var list = bll.GetPartsList(out totalCount, pageSize, pageIndex, bll.WebsiteOwner, carModelId, brandId, seriesCateId, seriesId, cateId, partsBrandId);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CarModelId != null)
                {
                    list[i].ShowCarModel = bll.GetAllCarModelName(list[i].CarModelId.Value);
                }
            }

            return MySpider.JSONHelper.ObjectToJson
                (new
                {
                    totalCount = totalCount,
                    list = list
                }
                );
        }

        private string DeleteParts(HttpContext context)
        {
            string ids = context.Request["ids"];

            resp.returnValue = bll.DeletePartsById(ids).ToString();

            resp.isSuccess = true;

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        #endregion


        #region 服务管理


        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddServer(HttpContext context)
        {
            /*
                TODO：添加服务

                保存基本信息
                保存关联的配件信息：默认关联、商户指定关联
                            
            */

            var carBrandId = Convert.ToInt32(context.Request["carBrandId"]);
            var carSeriesCateId = Convert.ToInt32(context.Request["carSeriesCateId"]);
            var carSeriesId = Convert.ToInt32(context.Request["carSeriesId"]);
            var carModelId = Convert.ToInt32(context.Request["carModelId"]);
            var shopType = context.Request["shopType"];
            var serverType = context.Request["serverType"];
            var cateId = Convert.ToInt32(context.Request["cateId"]);
            var serverName = context.Request["serverName"];
            var workHours = Convert.ToInt32(context.Request["workHours"]);

            //关联的商户ids
            var sallers = context.Request["sallers"];

            //关联的默认配件
            var defParts = context.Request["defParts"];

            CarServerInfo model = new CarServerInfo()
            {
                ServerId = Convert.ToInt32(bll.GetGUID(BLLJIMP.TransacType.CommAdd)),
                CarBrandId = carBrandId,
                CarModelId = carModelId,
                CarSeriesCateId = carSeriesCateId,
                CarSeriesId = carSeriesId,
                CateId = cateId,
                CreateTime = DateTime.Now,
                CreateUser = currentUserInfo.UserID,
                ServerName = serverName,
                ServerType = serverType,
                ShopType = shopType,
                WebsiteOwner = bllUser.WebsiteOwner,
                WorkHours = workHours
            };

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {
                if (bll.Add(model, tran))
                {
                    //保存关联的商户id

                    List<CommRelationInfo> serverSallerList = new List<CommRelationInfo>();
                    List<CommRelationInfo> serverSallerPartsList = new List<CommRelationInfo>();

                    #region 读取商户及商户配件数据

                    if (!string.IsNullOrWhiteSpace(sallers) && sallers != "[]")
                    {

                        dynamic sallersObj = JsonConvert.DeserializeObject(sallers);

                        foreach (var i in sallersObj)
                        {
                            var sallerId = i["UserID"].ToString();

                            CommRelationInfo serverSaller = new CommRelationInfo()
                            {
                                RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerSaller),
                                MainId = model.ServerId.ToString(),
                                RelationId = sallerId,
                                RelationTime = DateTime.Now
                            };

                            var partsStr = i["parts"] == null ? "" : i["parts"].ToString();

                            if (!string.IsNullOrWhiteSpace(partsStr) && partsStr != "[]")
                            {
                                dynamic sallerPartsObj = JsonConvert.DeserializeObject(partsStr);

                                foreach (var j in sallerPartsObj)
                                {
                                    var partsId = j["partsId"].ToString();
                                    var count = j["count"].ToString();
                                    CommRelationInfo serverSallerParts = new CommRelationInfo()
                                    {
                                        RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsSaller),
                                        MainId = model.ServerId.ToString(),
                                        RelationId = partsId,
                                        ExpandId = sallerId,
                                        Ex1 = count,
                                        RelationTime = DateTime.Now
                                    };

                                    serverSallerPartsList.Add(serverSallerParts);
                                }
                            }

                            serverSallerList.Add(serverSaller);
                        }
                    }

                    #endregion

                    //保存关联的配件    
                    List<CommRelationInfo> serverDefPartsList = new List<CommRelationInfo>();

                    #region 读取默认配件数据
                    if (!string.IsNullOrWhiteSpace(defParts) && defParts != "[]")
                    {
                        dynamic serverDefPartsObj = JsonConvert.DeserializeObject(defParts);

                        foreach (var j in serverDefPartsObj)
                        {
                            var partsId = j["partsId"].ToString();
                            var count = j["count"].ToString();
                            CommRelationInfo serverSallerParts = new CommRelationInfo()
                            {
                                RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsDef),
                                MainId = model.ServerId.ToString(),
                                RelationId = partsId,
                                Ex1 = count,
                                RelationTime = DateTime.Now
                            };

                            serverSallerPartsList.Add(serverSallerParts);
                        }

                    }
                    #endregion


                    //保存到数据库
                    List<CommRelationInfo> allRelation = new List<CommRelationInfo>();
                    allRelation.AddRange(serverSallerList);
                    allRelation.AddRange(serverSallerPartsList);
                    allRelation.AddRange(serverDefPartsList);

                    foreach (var item in allRelation)
                    {
                        bll.Add(item, tran);
                    }

                    tran.Commit();
                    resp.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                tran.Rollback();
                throw ex;
            }

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        private string AddServerBatchCarModel(HttpContext context)
        {
            var carBrandId = Convert.ToInt32(context.Request["carBrandId"]);
            var carSeriesCateId = Convert.ToInt32(context.Request["carSeriesCateId"]);
            var carSeriesId = Convert.ToInt32(context.Request["carSeriesId"]);

            //var carModelId = Convert.ToInt32(context.Request["carModelId"]);
            var multiCarModel = context.Request["multiCarModel"];

            var shopType = context.Request["shopType"];
            var serverType = context.Request["serverType"];
            var cateId = Convert.ToInt32(context.Request["cateId"]);
            var serverName = context.Request["serverName"];
            var workHours = Convert.ToInt32(context.Request["workHours"]);

            //关联的商户ids
            var sallers = context.Request["sallers"];

            //关联的默认配件
            var defParts = context.Request["defParts"];

            resp.isSuccess = true;

            foreach (var carModelIdStr in multiCarModel.Split(','))
            {
                var carModelId = Convert.ToInt32(carModelIdStr);

                CarServerInfo model = new CarServerInfo()
                {
                    ServerId = Convert.ToInt32(bll.GetGUID(BLLJIMP.TransacType.CommAdd)),
                    CarBrandId = carBrandId,
                    CarModelId = carModelId,
                    CarSeriesCateId = carSeriesCateId,
                    CarSeriesId = carSeriesId,
                    CateId = cateId,
                    CreateTime = DateTime.Now,
                    CreateUser = currentUserInfo.UserID,
                    ServerName = serverName,
                    ServerType = serverType,
                    ShopType = shopType,
                    WebsiteOwner =bll.WebsiteOwner,
                    WorkHours = workHours
                };

                ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                try
                {
                    if (bll.Add(model, tran))
                    {
                        //保存关联的商户id

                        List<CommRelationInfo> serverSallerList = new List<CommRelationInfo>();
                        List<CommRelationInfo> serverSallerPartsList = new List<CommRelationInfo>();

                        #region 读取商户及商户配件数据

                        if (!string.IsNullOrWhiteSpace(sallers) && sallers != "[]")
                        {

                            dynamic sallersObj = JsonConvert.DeserializeObject(sallers);

                            foreach (var i in sallersObj)
                            {
                                var sallerId = i["UserID"].ToString();

                                CommRelationInfo serverSaller = new CommRelationInfo()
                                {
                                    RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerSaller),
                                    MainId = model.ServerId.ToString(),
                                    RelationId = sallerId,
                                    RelationTime = DateTime.Now
                                };

                                var partsStr = i["parts"] == null ? "" : i["parts"].ToString();

                                if (!string.IsNullOrWhiteSpace(partsStr) && partsStr != "[]")
                                {
                                    dynamic sallerPartsObj = JsonConvert.DeserializeObject(partsStr);

                                    foreach (var j in sallerPartsObj)
                                    {
                                        var partsId = j["partsId"].ToString();
                                        var count = j["count"].ToString();
                                        CommRelationInfo serverSallerParts = new CommRelationInfo()
                                        {
                                            RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsSaller),
                                            MainId = model.ServerId.ToString(),
                                            RelationId = partsId,
                                            ExpandId = sallerId,
                                            Ex1 = count,
                                            RelationTime = DateTime.Now
                                        };

                                        serverSallerPartsList.Add(serverSallerParts);
                                    }
                                }

                                serverSallerList.Add(serverSaller);
                            }
                        }

                        #endregion

                        //保存关联的配件    
                        List<CommRelationInfo> serverDefPartsList = new List<CommRelationInfo>();

                        #region 读取默认配件数据
                        if (!string.IsNullOrWhiteSpace(defParts) && defParts != "[]")
                        {
                            dynamic serverDefPartsObj = JsonConvert.DeserializeObject(defParts);

                            foreach (var j in serverDefPartsObj)
                            {
                                var partsId = j["partsId"].ToString();
                                var count = j["count"].ToString();
                                CommRelationInfo serverSallerParts = new CommRelationInfo()
                                {
                                    RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsDef),
                                    MainId = model.ServerId.ToString(),
                                    RelationId = partsId,
                                    Ex1 = count,
                                    RelationTime = DateTime.Now
                                };

                                serverSallerPartsList.Add(serverSallerParts);
                            }

                        }
                        #endregion

                        //保存到数据库
                        List<CommRelationInfo> allRelation = new List<CommRelationInfo>();
                        allRelation.AddRange(serverSallerList);
                        allRelation.AddRange(serverSallerPartsList);
                        allRelation.AddRange(serverDefPartsList);

                        foreach (var item in allRelation)
                        {
                            bll.Add(item, tran);
                        }

                        tran.Commit();

                    }
                }
                catch (Exception ex)
                {
                    resp.isSuccess = false;
                    tran.Rollback();
                    throw ex;
                }
            }

            return MySpider.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 编辑服务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditServer(HttpContext context)
        {
            /*
            TODO：编辑服务

            保存基本信息
            车型数据不给编辑
            保存关联的配件信息：移除关联，添加新关联，默认关联、商户指定关联

            */

            var serverId = Convert.ToInt32(context.Request["serverId"]);

            var carBrandId = Convert.ToInt32(context.Request["carBrandId"]);
            var carSeriesCateId = Convert.ToInt32(context.Request["carSeriesCateId"]);
            var carSeriesId = Convert.ToInt32(context.Request["carSeriesId"]);
            var carModelId = Convert.ToInt32(context.Request["carModelId"]);
            var shopType = context.Request["shopType"];
            var serverType = context.Request["serverType"];
            var cateId = Convert.ToInt32(context.Request["cateId"]);
            var serverName = context.Request["serverName"];
            var workHours = Convert.ToInt32(context.Request["workHours"]);

            //关联的商户ids
            var sallers = context.Request["sallers"];

            //关联的默认配件
            var defParts = context.Request["defParts"];

            CarServerInfo model = bll.GetServer(serverId);

            if (model == null)
            {
                resp.isSuccess = false;

                return MySpider.JSONHelper.ObjectToJson(resp);
            }

            model.CateId = cateId;
            model.ServerName = serverName;
            model.ServerType = serverType;
            model.ShopType = shopType;
            model.WorkHours = workHours;


            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {
                if (bll.Update(model, tran))
                {
                    //保存关联的商户id

                    List<CommRelationInfo> serverSallerList = new List<CommRelationInfo>();
                    List<CommRelationInfo> serverSallerPartsList = new List<CommRelationInfo>();

                    #region 读取商户及商户配件数据

                    if (!string.IsNullOrWhiteSpace(sallers) && sallers != "[]")
                    {

                        dynamic sallersObj = JsonConvert.DeserializeObject(sallers);

                        foreach (var i in sallersObj)
                        {
                            var sallerId = i["UserID"].ToString();

                            CommRelationInfo serverSaller = new CommRelationInfo()
                            {
                                RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerSaller),
                                MainId = model.ServerId.ToString(),
                                RelationId = sallerId,
                                RelationTime = DateTime.Now
                            };

                            var partsStr = i["parts"] == null ? "" : i["parts"].ToString();

                            if (!string.IsNullOrWhiteSpace(partsStr) && partsStr != "[]")
                            {
                                dynamic sallerPartsObj = JsonConvert.DeserializeObject(partsStr);

                                foreach (var j in sallerPartsObj)
                                {
                                    var partsId = j["partsId"].ToString();
                                    var count = j["count"].ToString();
                                    CommRelationInfo serverSallerParts = new CommRelationInfo()
                                    {
                                        RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsSaller),
                                        MainId = model.ServerId.ToString(),
                                        RelationId = partsId,
                                        ExpandId = sallerId,
                                        Ex1 = count,
                                        RelationTime = DateTime.Now
                                    };

                                    serverSallerPartsList.Add(serverSallerParts);
                                }
                            }

                            serverSallerList.Add(serverSaller);
                        }
                    }

                    #endregion

                    //保存关联的配件    
                    List<CommRelationInfo> serverDefPartsList = new List<CommRelationInfo>();

                    #region 读取默认配件数据
                    if (!string.IsNullOrWhiteSpace(defParts) && defParts != "[]")
                    {
                        dynamic serverDefPartsObj = JsonConvert.DeserializeObject(defParts);

                        foreach (var j in serverDefPartsObj)
                        {
                            var partsId = j["partsId"].ToString();
                            var count = j["count"].ToString();
                            CommRelationInfo serverSallerParts = new CommRelationInfo()
                            {
                                RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsDef),
                                MainId = model.ServerId.ToString(),
                                RelationId = partsId,
                                Ex1 = count,
                                RelationTime = DateTime.Now
                            };

                            serverSallerPartsList.Add(serverSallerParts);
                        }

                    }
                    #endregion

                    //清除原有关系数据
                    bll.Delete(new CommRelationInfo(), string.Format(" MainId = '{0}' AND RelationType IN ('{1}','{2}','{3}') ",
                            serverId,
                            CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsSaller),
                            CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsDef),
                            CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerSaller)
                        ), tran);

                    //保存到数据库
                    List<CommRelationInfo> allRelation = new List<CommRelationInfo>();
                    allRelation.AddRange(serverSallerList);
                    allRelation.AddRange(serverSallerPartsList);
                    allRelation.AddRange(serverDefPartsList);

                    foreach (var item in allRelation)
                    {
                        bll.Add(item, tran);
                    }

                    tran.Commit();
                    resp.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                tran.Rollback();
                throw ex;
            }

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取汽车服务列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetServerList(HttpContext context)
        {
            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var carBrandId = Convert.ToInt32(context.Request["carBrandId"]);
            var carSeriesCateId = Convert.ToInt32(context.Request["carSeriesCateId"]);
            var carSeriesId = Convert.ToInt32(context.Request["carSeriesId"]);
            var carModelId = Convert.ToInt32(context.Request["carModelId"]);
            var cateId = Convert.ToInt32(context.Request["cateId"]);

            var serverType = context.Request["serverType"];
            var shopType = context.Request["shopType"];

            var totalCount = 0;

            var list = bll.GetServerList(out totalCount, bllUser.WebsiteOwner, pageSize, pageIndex, cateId, carBrandId, carSeriesCateId, carSeriesId, carModelId, serverType, shopType);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CarModelId != 0)
                {
                    list[i].ShowCarModel = bll.GetAllCarModelName(list[i].CarModelId);
                }

                if (list[i].CateId != 0)
                {
                    var cate = bllCate.GetArticleCategory(list[i].CateId);
                    if (cate != null)
                    {
                        var preCate = bllCate.GetArticleCategory(cate.PreID);
                        list[i].ShowCate = string.Format("{0}{1}", preCate != null ? preCate.CategoryName + "/" : "", cate.CategoryName);
                    }
                }

            }

            return MySpider.JSONHelper.ObjectToJson
                (new
                {
                    totalCount = totalCount,
                    list = list
                }
                );
        }

        /// <summary>
        /// 获取服务关系数据列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetServerRelationList(HttpContext context)
        {
            var serverId = Convert.ToInt32(context.Request["serverId"]);
            //默认配件
            //关联商户及商户关联的配件

            List<CarServerParts> allRelation = bll.GetServerAllRelation(serverId);

            //构造数据结果

            List<dynamic> defList = new List<dynamic>();
            //默认配件
            foreach (var item in allRelation.Where(p => p.RelationType == CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsDef)))
            {
                //[{ count: 100, partsId: 1, parts: { PartName:'测试'}}]
                defList.Add(new
                {
                    count = item.Ex1,
                    partsId = item.RelationId,
                    parts = item.Parts
                }
                );
            }

            List<dynamic> sallers = new List<dynamic>();

            //关联商户及商户配件
            foreach (var item in allRelation.Where(p => p.RelationType == CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerSaller)))
            {
                //[{ UserID: 'jubit',parts:[{ count: 100, partsId: 1, parts: { PartName:'测试'}}] }, { UserID: 'hf' }]
                var sallerId = item.RelationId;

                List<dynamic> partsList = new List<dynamic>();

                foreach (var parts in allRelation.Where(p =>
                    p.RelationType == CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.CarServerPartsSaller)
                    && p.ExpandId == sallerId
                    ))
                {
                    partsList.Add(new
                    {
                        count = parts.Ex1,
                        partsId = parts.RelationId,
                        parts = parts.Parts
                    }
               );
                }

                sallers.Add(new
                {
                    UserID = sallerId,
                    parts = partsList
                }
                );

            }

            dynamic result = new
            {
                defParts = defList,
                sallers = sallers
            };

            return JsonConvert.SerializeObject(result);
        }

        #endregion


        /// <summary>
        /// 获取用户列表（自动完成选择）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserListAutocomplete(HttpContext context)
        {
            string result = string.Empty;
            string filterUserIds = context.Request["filterUsers"];
            string keyword = context.Request["keyword"];
            int userType = Convert.ToInt32(context.Request["userType"]);

            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

            //var userList = bllUser.GetAllUsers(" TOP 10 ", currentUserInfo.WebsiteOwner, string.Format("", keyword, userType));

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strWhere.AppendFormat(" AND Company LIKE '%{0}%' OR TrueName LIKE '%{0}%' OR UserID LIKE '%{0}%' ", keyword);
            }

            if (!string.IsNullOrWhiteSpace(filterUserIds))
            {
                strWhere.AppendFormat(" AND UserID NOT IN ({0}) ", Common.StringHelper.ListToStr<string>(
                        filterUserIds.Split(',').ToList(),
                        "'",
                        ","
                    ));
            }

            if (userType < 2)
            {
                userType = 2;
            }

            strWhere.AppendFormat(" AND UserType = {0} ", userType);

            var userList = bllUser.GetList<UserInfo>(10, strWhere.ToString(), " AutoId ");

            List<dynamic> resultList = new List<dynamic>();

            foreach (var item in userList)
            {
                resultList.Add(new
                {
                    UserID = item.UserID,
                    Company = item.Company,
                    TrueName = item.TrueName,
                    Phone = item.Phone,
                    Email = item.Email,
                    Province = item.Province,
                    City = item.City,
                    District = item.District,
                    Address = item.Address,
                    AddressArea = item.AddressArea
                });
            }

            return JsonConvert.SerializeObject(new { data = resultList });
        }

        #region 商户管理

        /// <summary>
        /// 添加商户车系关联
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddSallerAndCarSeries(HttpContext context)
        {
            //SallerCarSeries

            var data = context.Request["data"];
            var sallerId = context.Request["sallerId"];

            var relationKey = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.SallerCarSeries);

            //删除原有关系
            bll.Delete(
                    new BLLJIMP.Model.CommRelationInfo(),
                    string.Format(" RelationType = '{0}' AND MainId = '{1}' ", relationKey, sallerId)
                );

            //添加原有关系
            if (!string.IsNullOrWhiteSpace(data) && data != "[]")
            {
                List<CommRelationInfo> relationList = new List<CommRelationInfo>();

                dynamic dataList = JsonConvert.DeserializeObject(data);

                foreach (var item in dataList)
                {
                    //[{ brandId: 0, seriesCateId: 0, seriesId: 0, brandName: '', seriesCateName: '', seriesName: '' }]
                    relationList.Add(new CommRelationInfo()
                    {
                        MainId = sallerId,
                        RelationId = item["seriesId"].ToString(),
                        Ex1 = item["brandId"].ToString(),
                        Ex2 = item["brandName"].ToString(),
                        Ex3 = item["seriesCateId"].ToString(),
                        Ex4 = item["seriesCateName"].ToString(),
                        Ex5 = item["seriesName"].ToString(),
                        RelationType = relationKey,
                        RelationTime = DateTime.Now
                    });
                }

                //保存数据
                foreach (var item in relationList)
                {
                    bll.Add(item);
                }

            }

            resp.isSuccess = true;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取商户车系关联
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSallerAndCarSeries(HttpContext context)
        {
            var relationKey = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.CommRelationType.SallerCarSeries);
            var sallerId = context.Request["sallerId"];

            var list = bll.GetList<CommRelationInfo>(string.Format(" RelationType = '{0}'  AND  MainId = '{1}' ", relationKey, sallerId));

            //[{ brandId: 0, seriesCateId: 0, seriesId: 0, brandName: '', seriesCateName: '', seriesName: '' }]

            List<dynamic> resultList = new List<dynamic>();

            foreach (var item in list)
            {
                resultList.Add(new
                {
                    seriesId = item.RelationId,
                    brandId = item.Ex1,
                    brandName = item.Ex2,
                    seriesCateId = item.Ex3,
                    seriesCateName = item.Ex4,
                    seriesName = item.Ex5
                });
            }

            return JsonConvert.SerializeObject(resultList);

        }

        #endregion

        #region 订单管理

        private string GetCarServerOrders(HttpContext context)
        {
            string result = string.Empty;

            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var sallerId = context.Request["sallerId"];
            var status = context.Request["status"];
            int totalCount = 0;

            if (currentUserInfo.UserType == 5)
            {
                sallerId = currentUserInfo.UserID;
            }

            var dataList = bll.GetCarServerOrderList(out totalCount, pageSize, pageIndex, bllUser.WebsiteOwner, sallerId, "", "", status);

            resp.isSuccess = true;

            resp.returnObj = new
            {
                totalCount = totalCount,
                list = dataList
            };

            return JsonConvert.SerializeObject(resp);

        }

        //更改订单状态信息
        private string EditServerOrderStatus(HttpContext context)
        {
            string result = string.Empty;

            var ids = context.Request["ids"];//订单id集合，逗号分隔
            var status = Convert.ToInt32( context.Request["status"]);//0受理中，1已确认，2已完成，3已取消

            resp.isSuccess = bll.EditCarServerOrderStatus(ids, status) > 0;
            
            return JsonConvert.SerializeObject(resp);
        }

        private string GetCarQuotationList(HttpContext context)
        {
            string result = string.Empty;

            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var sallerId = context.Request["sallerId"];
            var status = context.Request["status"];

            int totalCount = 0;

            var dataList = bll.QueryCarQuotationInfo(out totalCount, pageSize, pageIndex, "", status);

            resp.isSuccess = true;

            resp.returnObj = new
            {
                totalCount = totalCount,
                list = dataList
            };

            return JsonConvert.SerializeObject(resp);

        }

        //EditCarQuotation
        private string EditCarQuotation(HttpContext context)
        {
            var dataStr = context.Request["data"];

            if (string.IsNullOrWhiteSpace(dataStr))
            {
                resp.isSuccess = false;
                return JsonConvert.SerializeObject(resp);
            }

            var data = JsonConvert.DeserializeObject<BLLJIMP.Model.CarQuotationInfo>(dataStr);

            var source = bll.Get<CarQuotationInfo>(string.Format(" QuotationId = {0} ", data.QuotationId));

            var oldStatus = source.Status;

            //只编辑报价信息 和报价状态
            source.GuidePrice = data.GuidePrice;
            source.StockDescription = data.StockDescription;
            source.NationalSalesCount = data.NationalSalesCount;
            source.Increase = data.Increase;
            source.DiscountPrice = data.DiscountPrice;
            source.IsShopInsurance = data.IsShopInsurance;
            source.Status = 1;
            source.SallerMemo = data.SallerMemo;
            source.LicensingFees = data.LicensingFees;
            source.OtherExpenses = data.OtherExpenses;
            source.InsuranceCost = data.InsuranceCost;
            source.PurchaseTaxCost = data.PurchaseTaxCost;
            source.TotalPrice = data.TotalPrice;
            source.ActivityId = data.ActivityId;

            resp.isSuccess = bll.Update(source);

            if (resp.isSuccess && oldStatus == 0)
            {
                string msg = string.Format("您提交的 [{0}] 询价单管理员已处理，请及时查看", bll.GetAllCarModelName(source.CarModelId));
                string url = string.Format("http://{0}/customize/pureCar/m/app.aspx?ngroute=/store/voucher/{1}#/store/voucher/{1}",
                                context.Request.Url.Host,
                                source.QuotationId
                             );

                //发送系统消息和微信模板消息
                bllNotice.SendSystemMessage("系统消息", msg, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage, BLLJIMP.BLLSystemNotice.SendType.Personal, source.UserId, url);

                var sourceUser = bllUser.GetUserInfo(source.UserId);

                if (!string.IsNullOrWhiteSpace(sourceUser.WXOpenId))
                {
                    string accessToken = bllWX.GetAccessToken(bllWX.WebsiteOwner);

                    JToken SendData = JToken.Parse("{}");
                    SendData["touser"] = sourceUser.WXOpenId;
                    SendData["K1"] = "";
                    SendData["K2"] = msg;
                    SendData["K3"] = DateTime.Now.ToString();
                    SendData["K4"] = "";
                    SendData["url"] = url;

                    var wxmsg = bllWX.SendTemplateMessage(accessToken, "7646", SendData);

                }

            }

            return JsonConvert.SerializeObject(resp);
        }

        private string EditCarModel(HttpContext context)
        {
            var dataStr = context.Request["data"];

            if (string.IsNullOrWhiteSpace(dataStr))
            {
                resp.isSuccess = false;
                return JsonConvert.SerializeObject(resp);
            }

            var data = JsonConvert.DeserializeObject<BLLJIMP.Model.CarModelInfo>(dataStr);

            var source = bll.Get<CarModelInfo>(string.Format(" CarModelId = {0} ", data.CarModelId));

            //只编辑报价信息 和报价状态
            source.GuidePrice = data.GuidePrice;
            source.CarModelName = data.CarModelName;
            source.Colors = data.Colors;
            source.Img = data.Img;

            resp.isSuccess = bll.Update(source);

            return JsonConvert.SerializeObject(resp);
        }


        #endregion


        #region 工时表、折扣率管理

        private string QueryCarWorkhoursPrice(HttpContext context)
        {
            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var sallerId = context.Request["sallerId"];

            int totalCount = 0;

            var list = bll.QueryCarWorkhoursPrice(out totalCount, pageIndex, pageSize, sallerId);

            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(list[i].CarModelShowName) && list[i].CarModelId != null && list[i].CarModelId != 0)
                {
                    list[i].CarModelShowName = bll.GetCarModelInfo(list[i].CarModelId.Value).ShowName;
                    bll.Update(list[i]);
                }
            }

            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalCount = totalCount,
                list = list
            };

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加工时表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCarWorkhoursPrice(HttpContext context)
        {
            var data = context.Request["data"];

            var model = JsonConvert.DeserializeObject<CarWorkhoursPriceInfo>(data);

            //判断商户是否存在

            var saller = bllUser.GetUserInfo(model.SallerId);

            if (saller == null)
            {
                resp.isSuccess = false;
                resp.errmsg = "商户不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            model.CreateUser = currentUserInfo.UserID;
            model.CreateTime = DateTime.Now;
            model.WebsiteOwer = bllUser.WebsiteOwner;
            
            resp.isSuccess = bll.Add(model);

            return Common.JSONHelper.ObjectToJson(resp);
        }
        
        private string EditCarWorkhoursPrice(HttpContext context)
        {
            var data = context.Request["data"];
            var model = JsonConvert.DeserializeObject<CarWorkhoursPriceInfo>(data);

            var source = bll.GetCarWorkhoursPriceInfo(model.AutoId);

            source.Msg = model.Msg;
            source.Price = model.Price;

            resp.isSuccess = bll.Update(source);

            return Common.JSONHelper.ObjectToJson(resp);
        }
        
        private string DeleteCarWorkhoursPrice(HttpContext context)
        {
            var ids = context.Request["ids"];

            resp.isSuccess = true;
            resp.returnObj = bll.DeleteCarWorkhoursPrice(ids);

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string QueryCarDiscountRate(HttpContext context)
        {
            var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(context.Request["pageSize"]);
            var sallerId = context.Request["sallerId"];

            int totalCount = 0;

            var list = bll.QueryCarDiscountRate(out totalCount, pageIndex, pageSize, sallerId);

            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalCount = totalCount,
                list = list
            };

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string AddCarDiscountRate(HttpContext context)
        {
            var data = context.Request["data"];

            var model = JsonConvert.DeserializeObject<CarDiscountRateInfo>(data);

            //判断商户是否存在

            var saller = bllUser.GetUserInfo(model.SallerId);

            if (saller == null)
            {
                resp.isSuccess = false;
                resp.errmsg = "商户不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            model.CreateUser = currentUserInfo.UserID;
            model.CreateTime = DateTime.Now;
            model.WebsiteOwer = bllUser.WebsiteOwner;

            resp.isSuccess = bll.Add(model);

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string EditCarDiscountRate(HttpContext context)
        {
            var data = context.Request["data"];
            var model = JsonConvert.DeserializeObject<CarDiscountRateInfo>(data);

            var source = bll.GetCarDiscountRateInfo(model.AutoId);

            source.Msg = model.Msg;
            source.PartsRate = model.PartsRate;
            source.WorkhoursRate = model.WorkhoursRate;
            source.StartTime = model.StartTime;
            source.EndTime = model.EndTime;
            source.Week = model.Week;

            resp.isSuccess = bll.Update(source);

            return Common.JSONHelper.ObjectToJson(resp);
        }
        
        private string DeleteCarDiscountRate(HttpContext context)
        {
            var ids = context.Request["ids"];

            resp.isSuccess = true;
            resp.returnObj = bll.DeleteCarDiscountRate(ids);

            return Common.JSONHelper.ObjectToJson(resp);
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}