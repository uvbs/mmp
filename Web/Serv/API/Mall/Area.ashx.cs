using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 省市区
    /// </summary>
    public class Area :BaseHandler
    {
        /// <summary>
        /// 通用关系
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();

        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Provinces(HttpContext context)
        {
            int totalCount = 0;
            var sourceData = bllKeyValue.GetProvinces(out totalCount);
            if (bllKeyValue.WebsiteOwner == "huadaojia")
            {
                sourceData=sourceData.Where(p => p.DataValue == "上海").ToList();
            }
            var list = from p in sourceData
                       select new
                       {

                           code = p.DataKey,
                           name = p.DataValue,
                           pcode = p.PreKey,
                           level = 1

                       };

            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 根据省份代码获取城市列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Cities(HttpContext context)
        {

            string provinceCode = context.Request["province_code"];
            int totalCount = 0;
            var sourceData = bllKeyValue.GetCitys(provinceCode, out totalCount);
            var list = from p in sourceData
                       select new
                       {

                           code = p.DataKey,
                           name = p.DataValue,
                           pcode = p.PreKey,
                           level = 2

                       };

            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }


        /// <summary>
        /// 根据城市代码获取区域列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Districts(HttpContext context)
        {

            string cityCode = context.Request["city_code"];
            int totalCount = 0;
            var sourceData = bllKeyValue.GetDistricts(cityCode, out totalCount);
            if (sourceData.Count<=0)
            {
                ZentCloud.BLLJIMP.Model.KeyVauleDataInfo model = new BLLJIMP.Model.KeyVauleDataInfo();
                model.DataKey = "-1";
                model.DataValue = "无区域";
                sourceData.Add(model);

            }
            var list = from p in sourceData
                       select new
                       {

                           code = p.DataKey,
                           name = p.DataValue,
                           pcode = p.PreKey,
                           level = 3
                       };
            if (sourceData.Count <= 0)
            {
                ZentCloud.BLLJIMP.Model.KeyVauleDataInfo model = new BLLJIMP.Model.KeyVauleDataInfo();
                model.DataKey = "-1";
                model.DataValue = "无区域";
                sourceData.Add(model);

            }
            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 根据城区获取区域
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Areas(HttpContext context)
        {
            var websiteInfo = bllKeyValue.GetWebsiteInfoModelFromDataBase();

            string districtCode = context.Request["district_code"];
            
            List<BLLJIMP.Model.KeyVauleDataInfo> sourceData;

            if (websiteInfo.IsUnionHongware == 1)
            {
                sourceData = bllKeyValue.GetKeyVauleDataInfoList("District", districtCode, "hongwei");
            }
            else
            {
                sourceData = bllKeyValue.GetKeyVauleDataInfoList("District", districtCode, "Common");
            }


            var list = from p in sourceData
                       select new
                       {

                           code = p.DataKey,
                           name = p.DataValue,
                           pcode = p.PreKey,
                           level = 4

                       };

            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取所有省市区数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string All(HttpContext context)
        {

            var sourceData = bllKeyValue.GetAllLocationDataList();
            var rootData = sourceData.Where(p => p.DataType == "Province");
            List<AreaModel> list = new List<AreaModel>();
            foreach (var item in rootData)
            {
                var model = new AreaModel();
                model.code = item.DataKey;
                model.name = item.DataValue;
                model.type = item.DataType;
                model.parent_code = item.PreKey;
                list.Add(GetTree(model, sourceData, model.code));
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(list);

        }


        /// <summary>
        /// 返回模型
        /// </summary>
        public class AreaModel
        {
            /// <summary>
            /// 上级代码
            /// </summary>
            public string parent_code { get; set; }
            /// <summary>
            /// 省市区代码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 省市区名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// Province 省
            /// City 市
            /// District 区
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 子节点
            /// </summary>
            public List<AreaModel> children { get; set; }


        }
        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private AreaModel GetTree(AreaModel model, IList<BLLJIMP.Model.KeyVauleDataInfo> list, string parentID = "")
        {
            var query = list.Where(m => m.PreKey == parentID);
            if (query.Any())
            {
                if (model.children == null)
                {
                    model.children = new List<AreaModel>();
                }
                foreach (var item in query)
                {
                    AreaModel child = new AreaModel()
                    {
                        code = item.DataKey,
                        name = item.DataValue,
                        type = item.DataType,
                        parent_code = item.PreKey
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, item.DataKey);
                }
            }
            return model;
        }


    }
}