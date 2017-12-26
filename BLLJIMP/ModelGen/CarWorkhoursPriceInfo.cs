using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 商户车型工时表
    /// </summary>
    [Serializable]
    public class CarWorkhoursPriceInfo:ZCBLLEngine.ModelTable
    {
        public CarWorkhoursPriceInfo()
        { }
        #region Model
        private int _autoid;
        private string _sallerid;
        private int? _carbrandid;
        private int? _carseriescateid;
        private int? _carseriesid;
        private int? _carmodelid;
        private double _price;
        private string _msg;
        private string _createuser;
        private DateTime _createtime = DateTime.Now;
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
        /// 
        /// </summary>
        public int? CarBrandId
        {
            set { _carbrandid = value; }
            get { return _carbrandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarSeriesCateId
        {
            set { _carseriescateid = value; }
            get { return _carseriescateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarSeriesId
        {
            set { _carseriesid = value; }
            get { return _carseriesid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarModelId
        {
            set { _carmodelid = value; }
            get { return _carmodelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double Price
        {
            set { _price = value; }
            get { return _price; }
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
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public string CarBrandName { get; set; }
        public string CarSeriesCateName { get; set; }
        public string CarSeriesName { get; set; }
        public string CarModelName { get; set; }

        public string WebsiteOwer { get; set; }

        public string CarModelShowName { get; set; }

        #endregion Model
    }
}
