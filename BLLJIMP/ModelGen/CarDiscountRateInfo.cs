using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 工时/配件 折扣率实体
    /// </summary>
    [Serializable]
    public class CarDiscountRateInfo:ZCBLLEngine.ModelTable
    {
        public CarDiscountRateInfo()
        { }
        #region Model
        private int _autoid;
        private string _sallerid;
        private byte _week = 0;
        private string _starttime;
        private string _endtime;
        private double _workhoursrate;
        private double _partsrate;
        private string _msg;
        private string _createuser;
        private DateTime? _createtime = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public int AutoId
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SallerId
        {
            set { _sallerid = value; }
            get { return _sallerid; }
        }
        /// <summary>
        /// 星期：0-6
        /// </summary>
        public byte Week
        {
            set { _week = value; }
            get { return _week; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double WorkhoursRate
        {
            set { _workhoursrate = value; }
            get { return _workhoursrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double PartsRate
        {
            set { _partsrate = value; }
            get { return _partsrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msg
        {
            set { _msg = value; }
            get { return _msg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUser
        {
            set { _createuser = value; }
            get { return _createuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public string WebsiteOwer { get; set; }
        #endregion Model


        public string WeekName {
            get
            {
                List<string> weekList = new List<string>()
                {
                    "星期日",
                    "星期一",
                    "星期二",
                    "星期三",
                    "星期四",
                    "星期五",
                    "星期六"

                };

                return weekList[Week];
            }
        }

        /// <summary>
        /// 时间段
        /// </summary>
        public string TimeInterval
        {
            get
            {
                return StartTime + "-" + EndTime;
            }
        }
    }
}
