using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 键值对信息处理逻辑
    /// </summary>
    public class BLLKeyValueData : BLL
    {
        public BLLKeyValueData()
            : base()
        {

        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="PreKey">上级编码</param>
        /// <param name="WebsiteOwner">所属站点</param>
        /// <returns></returns>
        public List<Model.KeyVauleDataInfo> GetKeyVauleDataInfoList(string DataType, string PreKey, string WebsiteOwner)
        {
            List<Model.KeyVauleDataInfo> result = new List<Model.KeyVauleDataInfo>();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" DataType='{0}'", DataType);
            if (!string.IsNullOrWhiteSpace(PreKey)) strSql.AppendFormat("And PreKey='{0}'", PreKey);
            strSql.AppendFormat("And WebsiteOwner='{0}'", WebsiteOwner);
            strSql.AppendFormat(" ORDER BY AutoId");
            result = GetList<Model.KeyVauleDataInfo>(strSql.ToString());//DataType: area、city、province
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="DataType"></param>
        /// <param name="PreKey"></param>
        /// <param name="WebsiteOwner"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<KeyVauleDataInfo> GetKeyVauleDataInfoList(int pageSize, int pageIndex, string DataType, string PreKey, string WebsiteOwner, out int total)
        {
            List<KeyVauleDataInfo> result = new List<Model.KeyVauleDataInfo>();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" DataType='{0}'", DataType);
            if (!string.IsNullOrWhiteSpace(PreKey)) strSql.AppendFormat("And PreKey='{0}'", PreKey);
            strSql.AppendFormat("And WebsiteOwner='{0}'", WebsiteOwner);
            total = GetCount<KeyVauleDataInfo>(strSql.ToString());
            return GetLit<KeyVauleDataInfo>(pageSize, pageIndex, strSql.ToString(), "AutoId");
        }

        /// <summary>
        /// 获取所有省市区数据
        /// </summary>
        /// <returns></returns>
        public List<Model.KeyVauleDataInfo> GetAllLocationDataList()
        {

            //ToLog("进入 GetAllLocationDataList", "d:\\log\\areadebug.txt");
            List<Model.KeyVauleDataInfo> result = new List<Model.KeyVauleDataInfo>();

            var cacheKey = WebsiteOwner + ":area:" + CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CacheDataKey.AllLocationDataList);

            //var cacheData = Common.DataCache.GetCache(cacheKey);//读缓存

            var cacheData = RedisHelper.RedisHelper.StringGet(cacheKey);

            if (string.IsNullOrWhiteSpace(cacheData))
            {
                //ToLog("cacheData == null", "d:\\log\\areadebug.txt");
                var websiteInfo = GetWebsiteInfoModelFromDataBase();
                if (websiteInfo.IsUnionHongware == 0)
                {
                    result = GetList<Model.KeyVauleDataInfo>(" DataType IN ('District','City','Province') AND WebsiteOwner='Common' ORDER BY AutoId ");//DataType: area、city、province

                }
                else
                {
                    //result = GetList<Model.KeyVauleDataInfo>(string.Format(" DataType IN ('District','City','Province') AND WebsiteOwner='{0}' ORDER BY AutoId ", websiteInfo.WebsiteOwner));//使用站点自己的省市区
                    result = GetList<Model.KeyVauleDataInfo>(string.Format(" DataType IN ('District','City','Province') AND WebsiteOwner='hongwei' ORDER BY AutoId "));
                }

                RedisHelper.RedisHelper.StringSet(cacheKey, JsonConvert.SerializeObject(result));
                //Common.DataCache.SetCache(cacheKey, result);//写入缓存
            }
            else
            {
                //ToLog("cacheData != null", "d:\\log\\areadebug.txt");

                //result = (List<Model.KeyVauleDataInfo>)cacheData;

                result = JsonConvert.DeserializeObject<List<Model.KeyVauleDataInfo>>(cacheData);
                
            }

            return result;
        }
        /// <summary>
        /// 获取省
        /// </summary>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Model.KeyVauleDataInfo> GetProvinces(out int totalCount)
        {
            var websiteInfo = GetWebsiteInfoModelFromDataBase();

            //List<Model.KeyVauleDataInfo> result = GetAllLocationDataList();
            //totalCount = result.Where(p => p.DataType == "Province").Count();
            //return result.Where(p => p.DataType == "Province").ToList();
            
            List<BLLJIMP.Model.KeyVauleDataInfo> result;

            if (websiteInfo.IsUnionHongware == 1)
            {
                result = GetKeyVauleDataInfoList("Province", null, "hongwei");
            }
            else
            {
                result = GetKeyVauleDataInfoList("Province", null, "Common");
            }

            totalCount = result.Count;

            return result;
        }
        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="ProvinceKey"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Model.KeyVauleDataInfo> GetCitys(string ProvinceKey, out int totalCount)
        {
            //List<Model.KeyVauleDataInfo> result = GetAllLocationDataList();
            //totalCount = result.Where(p => p.DataType == "City" && p.PreKey == ProvinceKey).Count();
            //return result.Where(p => p.DataType == "City" && p.PreKey == ProvinceKey).ToList();

            var websiteInfo = GetWebsiteInfoModelFromDataBase();

            List<BLLJIMP.Model.KeyVauleDataInfo> sourceData;

            if (websiteInfo.IsUnionHongware == 1)
            {
                sourceData = GetKeyVauleDataInfoList("City", ProvinceKey, "hongwei");
            }
            else
            {
                sourceData = GetKeyVauleDataInfoList("City", ProvinceKey, "Common");
            }

            totalCount = sourceData.Count;

            return sourceData;

        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="CityKey"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Model.KeyVauleDataInfo> GetDistricts(string CityKey, out int totalCount)
        {
            //List<Model.KeyVauleDataInfo> result = GetAllLocationDataList();
            //totalCount = result.Where(p => p.DataType == "District" && p.PreKey == CityKey).Count();
            //return result.Where(p => p.DataType == "District" && p.PreKey == CityKey).ToList();

            var websiteInfo = GetWebsiteInfoModelFromDataBase();

            List<BLLJIMP.Model.KeyVauleDataInfo> sourceData;

            if (websiteInfo.IsUnionHongware == 1)
            {
                sourceData = GetKeyVauleDataInfoList("District", CityKey, "hongwei");
            }
            else
            {
                sourceData = GetKeyVauleDataInfoList("District", CityKey, "Common");
            }

            totalCount = sourceData.Count;

            return sourceData;

        }
        

        /// <summary>
        /// 获取KeyVauleData
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public KeyVauleDataInfo GetKeyData(int AutoId)
        {
            return Get<KeyVauleDataInfo>(string.Format("AutoId ={0}", AutoId));
        }
        /// <summary>
        /// 获取DataValue
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="DataKey"></param>
        /// <returns></returns>
        public string GetDataDefVaule(string DataType, string DataKey, string DefValue = "", string websiteOwner = "")
        {
            string where = string.Format(" DataType='{0}' AND DataKey='{1}'", DataType, DataKey);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) where = string.Format(" DataType='{0}' AND DataKey='{1}' AND WebsiteOwner='{2}'", DataType, DataKey, websiteOwner);
            Model.KeyVauleDataInfo data = Get<Model.KeyVauleDataInfo>(where);
            if (data == null) return DefValue;
            return data.DataValue;
        }
        /// <summary>
        /// 获取KeyVauleData
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public KeyVauleDataInfo GetKeyData(string DataType, string DataKey, string websiteOwner)
        {
            return Get<Model.KeyVauleDataInfo>(string.Format(" DataType='{0}' AND DataKey='{1}' AND WebsiteOwner = '{2}'", DataType, DataKey, websiteOwner));
        }


        /// <summary>
        /// 获取DataValue
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="DataKey"></param>
        /// <returns></returns>
        public string GetDataVaule(string DataType, string DataKey)
        {
            Model.KeyVauleDataInfo data = Get<Model.KeyVauleDataInfo>(string.Format(" DataType='{0}' AND DataKey='{1}'", DataType, DataKey));
            if (data == null) return "";
            return data.DataValue;
        }
        public string GetDataVaule(Enums.KeyVauleDataType dataType, string dataKey, string websiteOwner)
        {
            var key = CommonPlatform.Helper.EnumStringHelper.ToString(dataType);
            return GetDataVaule(key, dataKey, websiteOwner);
        }
        public string GetDataVaule(string DataType, string DataKey, string websiteOwner)
        {
            Model.KeyVauleDataInfo data = Get<Model.KeyVauleDataInfo>(string.Format(" DataType='{0}' AND DataKey='{1}' AND WebsiteOwner = '{2}'", DataType, DataKey, websiteOwner));
            if (data == null) return "";
            return data.DataValue;
        }

        public bool PutDataValue(Model.KeyVauleDataInfo vlueData)
        {
            Model.KeyVauleDataInfo data = Get<Model.KeyVauleDataInfo>(string.Format(" DataType='{0}' AND DataKey='{1}'", vlueData.DataType, vlueData.DataKey));
            if (data == null)
            {
                return Add(vlueData);
            }
            else
            {
                data.Creater = vlueData.Creater;
                data.CreateTime = vlueData.CreateTime;
                data.DataValue = vlueData.DataValue;
                data.PreKey = vlueData.PreKey;
                return Update(data);
            }
        }

        public bool DeleteDataVaule(string dataType, string dataKey, string websiteOwner)
        {
            return Delete(new KeyVauleDataInfo(), string.Format(" DataType='{0}' AND DataKey='{1}' AND WebsiteOwner='{2}' ", dataType, dataKey, websiteOwner)) > 0;
        }
        public bool DeleteDataVaule(string dataType, string dataKey, string preKey, string websiteOwner)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" DataType='{0}' ", dataType);
            if (!string.IsNullOrWhiteSpace(dataKey)) sbsql.AppendFormat(" AND DataKey='{0}' ", dataKey);
            if (!string.IsNullOrWhiteSpace(preKey)) sbsql.AppendFormat(" AND PreKey='{0}' ", preKey);
            sbsql.AppendFormat(" AND WebsiteOwner='{0}' ", WebsiteOwner);
            return Delete(new KeyVauleDataInfo(), sbsql.ToString()) > 0;
        }

        public bool DeleteDataVaule(string ids)
        {
            return Delete(new KeyVauleDataInfo(), string.Format(" AutoId in ({0})", ids)) > 0;
        }
        /// <summary>
        /// 获取KeyVauleData
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public Model.KeyVauleDataInfo GetKeyVauleData(string AutoId)
        {
            return Get<Model.KeyVauleDataInfo>(string.Format(" AutoId={0}", AutoId));
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="vlueData"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool PutDataValue(Model.KeyVauleDataInfo vlueData, BLLTransaction tran = null)
        {
            if (string.IsNullOrWhiteSpace(vlueData.DataValue)) return true;

            Model.KeyVauleDataInfo data = Get<Model.KeyVauleDataInfo>(string.Format(" DataType='{0}' AND DataKey='{1}' AND WebsiteOwner='{2}'  ",
                vlueData.DataType, vlueData.DataKey, vlueData.WebsiteOwner), tran);
            if (data != null) vlueData.AutoId = data.AutoId;
            if (vlueData.AutoId == 0)
            {
                return tran == null ? Add(vlueData) : Add(vlueData, tran);
            }
            else
            {
                return tran == null ? Update(vlueData) : Update(vlueData, tran);
            }
        }

        /// <summary>
        /// 获取商城所有配置
        /// </summary>
        /// <returns></returns>
        public string GetMallConfigList()
        {
            var sourceList = GetList<KeyVauleDataInfo>(string.Format(" WebsiteOwner='{0}' And DataType='MallConfig'", WebsiteOwner));
            WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase();
            CompanyWebsite_Config nWebsiteConfig = Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
            var apply_url = "/App/Member/Wap/PhoneVerify.aspx?referrer=" + System.Web.HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString());
            if (nWebsiteConfig != null && (nWebsiteConfig.MemberStandard == 2 || nWebsiteConfig.MemberStandard == 3))
            {
                apply_url = "/App/Member/Wap/CompleteUserInfo.aspx?referrer=" + System.Web.HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString());
            }
            UserInfo curUser = GetCurrentUserInfo();
            if (nWebsiteConfig==null) nWebsiteConfig = new CompanyWebsite_Config();
            var shopConfig = new 
            {
                mall_name = nWebsiteConfig.WebsiteTitle,
                mall_desc = nWebsiteConfig.WebsiteDescription,
                mall_logo = nWebsiteConfig.WebsiteImage,
                product_img_ratio1 = string.IsNullOrWhiteSpace(websiteInfo.ProductImgRatio1)? "600": websiteInfo.ProductImgRatio1,
                product_img_ratio2 = string.IsNullOrWhiteSpace(websiteInfo.ProductImgRatio2)? "600": websiteInfo.ProductImgRatio2,
                nav_group_name = websiteInfo.ShopNavGroupName,
                foottool = websiteInfo.ShopFoottool,
                slide_name = websiteInfo.ShopAdType,
                is_login = curUser != null ? true : false,
                is_show_oldprice = websiteInfo.IsShowOldPrice == 1? true : false,
                is_show_stock = websiteInfo.IsShowStock == 1 ? true : false,
                is_member = IsMember(),
                access_level = curUser!=null?curUser.AccessLevel:0,
                apply_url = apply_url,
                nopms_url = "/Error/NoPmsMobile.htm",
                website_version = new BLLWebSite().GetWebsiteVersion(WebsiteOwner),
                my_cardcoupons_title = nWebsiteConfig.MyCardCouponsTitle,
                mall_desc_top = websiteInfo.MallDescTop,
                mall_desc_bottom = websiteInfo.MallDescBottom
            };
           
            JObject config = new JObject();
            if (sourceList.Count>0)
            {
                try
                {
                    config["mallconfig"] = JObject.Parse(sourceList[0].DataValue);
                }
                catch (Exception)
                {
                }
            }

            JObject mallConfig = JObject.FromObject(shopConfig);
            config["mallconfignew"] = mallConfig;
            return config.ToString();

            //var list = from p in sourceList
            //           select new
            //           {

            //               key = p.DataKey,
            //               value = p.DataValue
            //           };
            //return list;

        }
        /// <summary>
        /// 获取分组广告比例
        /// </summary>
        /// <param name="slideType"></param>
        /// <returns></returns>
        public string GetSlideProportion(string slideType) {

            var model = Get<KeyVauleDataInfo>(string.Format(" WebSiteOwner='{0}' And DataType='SlideProportion' And DataKey='{1}'",WebsiteOwner,slideType));
            if (model!=null)
            {
                return model.DataValue;
            }
            return "";
        
        }
        /// <summary>
        /// 获取分组广告比例
        /// </summary>
        /// <param name="slideType">广告类型</param>
        /// <returns></returns>
        public KeyVauleDataInfo GetSlideProportionModel(string slideType)
        {

           return Get<KeyVauleDataInfo>(string.Format(" WebSiteOwner='{0}' And DataType='SlideProportion' And DataKey='{1}'", WebsiteOwner, slideType));
            

        }
        /// <summary>
        /// 添加分组广告比例
        /// </summary>
        /// <param name="slideType">广告类型</param>
        /// <returns></returns>
        public bool AddSlideProportion(string slideType,string proportion)
        {

            var modelRecord = GetSlideProportionModel(slideType);
            if (modelRecord != null)//更新
            {
               return UpdateSlideProportion(slideType, proportion);
            }
            else//添加
            {
                KeyVauleDataInfo model = new KeyVauleDataInfo();
                model.WebsiteOwner = WebsiteOwner;
                model.CreateTime = DateTime.Now;
                model.Creater = WebsiteOwner;
                model.DataType = "SlideProportion";
                model.DataKey = slideType;
                model.DataValue = proportion;
                if (Add(model))
                {
                    return true;

                }
            }
            return false;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="slideType"></param>
        /// <param name="proportion"></param>
        /// <returns></returns>
        public bool UpdateSlideProportion(string slideType, string proportion)
        {
            var model = GetSlideProportionModel(slideType);
            if (model!=null)
            {
                if (Update(new KeyVauleDataInfo(),string.Format(" DataValue='{0}'",proportion),string.Format(" AutoID={0}",model.AutoId))>0)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 获取分类 KeyValue数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<dynamic> GetCateKeyValueData(string type)
        {
            BLLArticleCategory bllCate = new BLLArticleCategory();
            int total = 0;
            List<ArticleCategory> cateList = bllCate.GetCateList(out total, type, null, null);
            List<KeyVauleDataInfo> keyValueList = GetListByKey<KeyVauleDataInfo>("DataType",type);

            List<dynamic> result = new List<dynamic>();

            foreach (ArticleCategory cate in cateList.Where(p=>p.PreID == 0))
            {
                List<ArticleCategory> ccateList = cateList.Where(p=>p.PreID == cate.AutoID).ToList();
                if(ccateList.Count ==0){
                    List<dynamic> keyDataList = new List<dynamic>();
                    foreach (KeyVauleDataInfo keyData in keyValueList.Where(p=>p.PreKey == cate.AutoID.ToString())){
                        keyDataList.Add(new{
                            data_key=keyData.DataName,
                            data_value = keyData.DataValue
                        });
                    }
                    result.Add(new
                    {
                        cate_id = cate.AutoID,
                        cate_name=cate.CategoryName,
                        child_cate_list = new List<dynamic>(),
                        key_value_list = keyDataList
                    });
                }
                else
                {
                    List<dynamic> ccList = new List<dynamic>();
                    foreach (ArticleCategory ccate in ccateList)
                    {
                        List<dynamic> keyDataList = new List<dynamic>();
                        foreach (KeyVauleDataInfo keyData in keyValueList.Where(p => p.PreKey == ccate.AutoID.ToString()))
                        {
                            keyDataList.Add(new
                            {
                                data_name = keyData.DataName,
                                data_value = keyData.DataValue
                            });
                        }
                        ccList.Add(new
                        {
                            cate_id = ccate.AutoID,
                            cate_name = ccate.CategoryName,
                            child_cate_list = new List<dynamic>(),
                            key_value_list = keyDataList
                        });
                    }
                    result.Add(new
                    {
                        cate_id = cate.AutoID,
                        cate_name = cate.CategoryName,
                        child_cate_list = ccList,
                        key_value_list = new List<dynamic>()
                    });
                }
            }
            return result;
        }
    }
}
