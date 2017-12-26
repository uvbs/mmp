using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.Common
{
    public class GeolocationHelper
    {
        /// <summary>
        /// 计算坐标距离（米）
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double ComputeDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double radius = 6378137.0;//距离 1度=6378137米

            double radLatBegin = lat1 * Math.PI /180.0 ;
            double radLatEnd = lat2 * Math.PI/180.0 ;
            double radLatDiff = radLatBegin - radLatEnd; //纬度球面距离
            
            double radLngBegin = lon1 * Math.PI /180.0 ;
            double radLngEnd = lon2 * Math.PI/180.0 ;
            double radLngDiff = radLngBegin-radLngEnd;  //经度球面距离

            //距离
            double distance = 2 * Math.Asin(Math.Sqrt(
                Math.Pow(Math.Sin(radLatDiff/2), 2)
                +Math.Cos(radLatBegin)*Math.Cos(radLatEnd)*Math.Pow(Math.Sin(radLngDiff/2), 2)
                ));
            return distance*radius;
        }



    }
}
