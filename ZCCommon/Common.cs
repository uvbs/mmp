using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace ZentCloud.Common
{
   public class Common
    {
       
        /// <summary>
        ///根据IP地址获取IP所在地
        /// </summary>
        public static string GetIPLocation(string ip)
        {
        
       return  IPLocation.IPLocation.IPLocate(HttpContext.Current.Server.MapPath("/FileUpload/IPLocation/qqwry.dat"), ip); 


        }
       
    }
}
