using System;
using System.Reflection;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Admin.CarManage
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        DefaultResponse resp = new DefaultResponse();
        BLLJIMP.BLLCarLibrary bll = new BLLJIMP.BLLCarLibrary();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 网站所有者
        /// </summary>
        private string webSiteOwner;

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
                }
                webSiteOwner = bll.WebsiteOwner;
                string Action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(Action))
                {
                    MethodInfo method = this.GetType().GetMethod(Action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {

                    resp.errmsg = "action not exist";
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

            BLLJIMP.BLLKeyValueData bllKV = new BLLJIMP.BLLKeyValueData();
            var baseUrl = "http://apis.haoservice.com/lifeservice/car/";
            var appKey = bllKV.GetDataVaule(BLLJIMP.Enums.KeyVauleDataType.HaoServiceAppKey, "GetCarSeries", this.webSiteOwner);
            var api = baseUrl + "GetSeries?key=" + appKey;
            var modelApi = baseUrl + "GetModel/?key=" + appKey + "&id=";

            var respStr = MySpider.MySpider.GetPageSourceForUTF8(api);

            if (!string.IsNullOrWhiteSpace(respStr))
            {
                //var respObj = JsonConvert.DeserializeObject<BLLJIMP.Model.HaoService.BrandResp>(respStr);

                 var respObj=ZentCloud.Common.JSONHelper.JsonToModel<BLLJIMP.Model.HaoService.BrandResp>(respStr);
 
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
                var updator =  this.currentUserInfo == null ? "" : this.currentUserInfo.UserID;
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

                bll.GetWebSiteBrandIdList(this.webSiteOwner, out buyBrandIdList, out serviceBrandList, out allBrandIdList);

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

            var dataList = bll.QueryBrand(out totalCount, this.webSiteOwner, pageSize, pageIndex, firstLetter,isMustInWebsite);

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

            bll.RemoveWebSiteBrand(this.webSiteOwner, brandId, BLLJIMP.Enums.CommRelationType.BuyCarBrand);
            bll.RemoveWebSiteBrand(this.webSiteOwner, brandId, BLLJIMP.Enums.CommRelationType.ServiceCarBrand);

            if (isBuy.Equals(1))
            {
                bll.SetWebSiteBrand(this.webSiteOwner, brandId, BLLJIMP.Enums.CommRelationType.BuyCarBrand);
            }

            if (isService.Equals(1))
            {
                bll.SetWebSiteBrand(this.webSiteOwner, brandId, BLLJIMP.Enums.CommRelationType.ServiceCarBrand);
            }
            
            resp.isSuccess = true;

            return MySpider.JSONHelper.ObjectToJson(resp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}