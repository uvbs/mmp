using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Outlets
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        BLLWebSite bllComopanyConfig = new BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            if (page==0)
            {
                page = 1;
            }
            if (rows == 0)
            {
                rows = 10;
            }
            string keyword = context.Request["keyword"];
            string cate_id = context.Request["cate_id"];
            bool show_hide = Convert.ToBoolean(context.Request["show_hide"]);
            string tags = context.Request["tags"];
            string k1 = context.Request["k1"];
            string k4 = context.Request["k4"];
            string k5 = context.Request["k5"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            int range = Convert.ToInt32(context.Request["range"]);
            int limitRange = Convert.ToInt32(context.Request["limit_range"]);
            string city=context.Request["city"];//城市
            var companyConfig = bllComopanyConfig.GetCompanyWebsiteConfig();
            if (!string.IsNullOrEmpty(companyConfig.OutletsSearchRange))
            {
                range = Convert.ToInt32(companyConfig.OutletsSearchRange);
                k5 = "1";
            }
            if (limitRange==0)//不限制搜索范围
            {
                range = int.MaxValue;
            }
            if (cate_id == "0") cate_id = "";
            if (longitude == "0") longitude = "";
            if (latitude == "0") latitude = "";

            int total = 0;
            List<JuActivityInfo> dataList = bllJuActivity.GetOutletsList(rows, page, cate_id, tags, keyword, show_hide,
                "JuActivityID,ActivityName,ActivityAddress,ThumbnailsPath,Sort,K4,K5,UserLongitude,UserLatitude,Province,City,District", out total,
                longitude, latitude, range, "range", bllJuActivity.WebsiteOwner, k1, k4,k5,city);

            var rData = from p in dataList
                        select new
                        {
                            id = p.JuActivityID,
                            title = p.ActivityName,
                            province=p.Province,
                            city=p.City,
                            district=p.District,
                            address = p.ActivityAddress,
                            img = p.ThumbnailsPath,
                            longitude=p.UserLongitude,
                            latitude=p.UserLatitude,
                            distance = p.Distance,
                            distance_formart=DistanceFormart(p.Distance.ToString()),
                            supplier_id=p.K5
                        };
            apiResp.result = new
            {
                totalcount = total,
                list = rData
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllJuActivity.ContextResponse(context, apiResp);
        }

        /// <summary>
        /// 格式化距离
        /// </summary>
        /// <returns></returns>
        public string DistanceFormart(string value) { 
        
        if (string.IsNullOrEmpty(value)) return "";
        decimal valueInt = decimal.Parse(value);
        if (valueInt < 0) return "";
        decimal m = valueInt * 1000;
        if (m < 10) return "<10m";
        if (m < 1000) return Math.Round(m,2) + "m";
        return Math.Round(valueInt * 100) / 100 + "km";
        
        }
        
    }
}