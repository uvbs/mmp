using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.Common
{
    public class AMapHelper
    {
        /// <summary>
        /// 根据地址查询经纬度
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static GeoResult GetGeoByAddress(string address)
        {

            GeoResult result = new GeoResult();
            HttpInterFace request = new HttpInterFace();
            string respStr = request.GetWebRequest(string.Format("key={0}&address={1}", ConfigHelper.GetConfigString("AMapKeyService"), address), "http://restapi.amap.com/v3/geocode/geo", Encoding.UTF8);

            JToken JToken = JToken.Parse(respStr);
            if (JToken["status"].ToString() == "1" && (JToken["count"].ToString() != "0"))
            {
                result.status = true;
                result.longitude = Convert.ToDouble(JToken["geocodes"][0]["location"].ToString().Split(',')[0]);
                result.latitude = Convert.ToDouble(JToken["geocodes"][0]["location"].ToString().Split(',')[1]);

            }
            return result;

        }



      



        /// <summary>
        /// 结果模型
        /// </summary>
        public class GeoResult
        {
            /// <summary>
            /// 是否查询成功
            /// </summary>
            public bool status { get; set; }
            /// <summary>
            /// 经度
            /// </summary>
            public double longitude { get; set; }
            /// <summary>
            /// 纬度
            /// </summary>
            public double latitude { get; set; }

        }


    }
}
