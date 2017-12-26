using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.Common
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 计算本周起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateFirstDateOfWeek(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }
        /// <summary>
        /// 计算本周结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateLastDateOfWeek(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }
        /// <summary>
        /// 判断选择的日期是否是本周（根据系统当前时间决定的‘本周’比较而言）
        /// </summary>
        /// <param name="someDate"></param>
        /// <returns></returns>
        public static bool IsThisWeek(DateTime someDate)
        {
            //得到someDate对应的周一
            DateTime someMon = CalculateFirstDateOfWeek(someDate);
            //得到本周一
            DateTime nowMon = CalculateFirstDateOfWeek(DateTime.Now);
            TimeSpan ts = someMon - nowMon;
            if (ts.Days < 0)
                ts = -ts;//取正
            if (ts.Days >= 7)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///  美国时间格式转换(Wed Nov 04 00:00:00 +0800 2009)
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime TransUSDate(string strDate)
        {
            System.Globalization.CultureInfo cultureInfo =
            System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            string format = "ddd MMM d HH:mm:ss zz00 yyyy";
            DateTime dt = DateTime.ParseExact(strDate, format, cultureInfo); // 将字符串转换成日期
            return dt;
        }

        public static string GetTimeIntStr(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToStr(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString() : "";
        }
        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Convert.ToInt64((dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
        }
        
        /// <summary>
        /// 时间戳转换成日期
        /// </summary>
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            start = start.ToLocalTime();
            return start.AddMilliseconds(timestamp);
        }
        
        public static string DateTimeToString(DateTime dt)
        {
            return dt.ToString("yyyy-M-d H:mm:ss");
        }
        #region 8位时间转换
        public static int ToDateInt8ByDateTime(DateTime date)
        {
            return Convert.ToInt32(date.ToString("yyyyMMdd"));
        }

        public static int ToDateInt8ByString(string date)
        {
            return Convert.ToInt32(DateTime.Parse(date).ToString("yyyyMMdd"));
        }

        public static string ToStringByDateInt8(int date)
        {
            return DateTime.ParseExact(date.ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
        }

        public static DateTime ToDateTimeByDateInt8(int date)
        {
            return DateTime.ParseExact(date.ToString(), "yyyyMMdd", null);
        }
        #endregion 8位时间转换
    }
}
