using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class UserSignHistory : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _signid;
        private string _userid;
        private string _longitude;
        private string _latitude;
        private string _address;
        private DateTime? _date;
        /// <summary>
        /// 
        /// </summary>
        public int SignID
        {
            set { _signid = value; }
            get { return _signid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            set { _longitude = value; }
            get { return _longitude; }
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            set { _latitude = value; }
            get { return _latitude; }
        }
        /// <summary>
        /// 地点
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 签到日期
        /// </summary>
        public DateTime? AddDate
        {
            set { _date = value; }
            get { return _date; }
        }
        #endregion Model

    }
}
