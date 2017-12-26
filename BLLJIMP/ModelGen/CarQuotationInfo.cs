using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 购车报价单
    /// </summary>
    public class CarQuotationInfo:ZCBLLEngine.ModelTable
    {
        public CarQuotationInfo()
        { }
        #region Model
        private int _quotationid;
        private string _userid;
        private int _carbrandid;
        private int _carseriescateid;
        private int _carseriesid;
        private int _carmodelid;
        private string _carcolor;
        private string _buytime;
        private string _licenseplate;
        private string _city;
        private string _district;
        private string _area;
        private string _preference;
        private string _carownername;
        private string _carownerphone;
        private DateTime _createtime;
        private int _status = 0;
        private string _activityid;
        private string _websiteowner;
        private string _sallermemo;
        private double? _guideprice;
        private double? _discountprice;
        private string _carimg;
        private string _carmodelname;
        private string _stockdescription;
        private int _nationalsalescount;
        private int _increase = 0;
        private int _isshopinsurance = 0;
        private double _licensingfees;
        private double _otherexpenses;
        private double _insurancecost;
        private double _purchasetaxcost;
        private double _totalprice;
        private string _operator;
        /// <summary>
        /// 
        /// </summary>
        public int QuotationId
        {
            set { _quotationid = value; }
            get { return _quotationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CarBrandId
        {
            set { _carbrandid = value; }
            get { return _carbrandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CarSeriesCateId
        {
            set { _carseriescateid = value; }
            get { return _carseriescateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CarSeriesId
        {
            set { _carseriesid = value; }
            get { return _carseriesid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CarModelId
        {
            set { _carmodelid = value; }
            get { return _carmodelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarColor
        {
            set { _carcolor = value; }
            get { return _carcolor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyTime
        {
            set { _buytime = value; }
            get { return _buytime; }
        }
        /// <summary>
        /// 牌照
        /// </summary>
        public string LicensePlate
        {
            set { _licenseplate = value; }
            get { return _licenseplate; }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 城区
        /// </summary>
        public string District
        {
            set { _district = value; }
            get { return _district; }
        }
        /// <summary>
        /// 地域
        /// </summary>
        public string Area
        {
            set { _area = value; }
            get { return _area; }
        }
        /// <summary>
        /// 购车偏好
        /// </summary>
        public string Preference
        {
            set { _preference = value; }
            get { return _preference; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarOwnerName
        {
            set { _carownername = value; }
            get { return _carownername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarOwnerPhone
        {
            set { _carownerphone = value; }
            get { return _carownerphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 报价单状态：0未处理，1进行中，2已过期，3已取消
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityId
        {
            set { _activityid = value; }
            get { return _activityid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WebSiteOwner
        {
            set { _websiteowner = value; }
            get { return _websiteowner; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string SallerMemo
        {
            set { _sallermemo = value; }
            get { return _sallermemo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? GuidePrice
        {
            set { _guideprice = value; }
            get { return _guideprice; }
        }
        /// <summary>
        /// 裸车优惠价
        /// </summary>
        public double? DiscountPrice
        {
            set { _discountprice = value; }
            get { return _discountprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarImg
        {
            set { _carimg = value; }
            get { return _carimg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarModelName
        {
            set { _carmodelname = value; }
            get { return _carmodelname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StockDescription
        {
            set { _stockdescription = value; }
            get { return _stockdescription; }
        }
        /// <summary>
        /// 全国销量
        /// </summary>
        public int NationalSalesCount
        {
            set { _nationalsalescount = value; }
            get { return _nationalsalescount; }
        }
        /// <summary>
        /// 增幅
        /// </summary>
        public int Increase
        {
            set { _increase = value; }
            get { return _increase; }
        }
        /// <summary>
        /// 是否需要店内保险
        /// </summary>
        public int IsShopInsurance
        {
            set { _isshopinsurance = value; }
            get { return _isshopinsurance; }
        }
        /// <summary>
        /// 上牌费
        /// </summary>
        public double LicensingFees
        {
            set { _licensingfees = value; }
            get { return _licensingfees; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double OtherExpenses
        {
            set { _otherexpenses = value; }
            get { return _otherexpenses; }
        }
        /// <summary>
        /// 保险预估费用
        /// </summary>
        public double InsuranceCost
        {
            set { _insurancecost = value; }
            get { return _insurancecost; }
        }
        /// <summary>
        /// 购置税预估
        /// </summary>
        public double PurchaseTaxCost
        {
            set { _purchasetaxcost = value; }
            get { return _purchasetaxcost; }
        }
        /// <summary>
        /// 总价
        /// </summary>
        public double TotalPrice
        {
            set { _totalprice = value; }
            get { return _totalprice; }
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator
        {
            set { _operator = value; }
            get { return _operator; }
        }
        /// <summary>
        /// 购车方式
        /// </summary>
        public string PurchaseWay { get; set; }
        /// <summary>
        /// 汽车品牌名称
        /// </summary>
        public string CarBrandName { get; set; }
        /// <summary>
        /// 车系名称
        /// </summary>
        public string CarSeriesName { get; set; }
        /// <summary>
        /// 操作时间（报价时间）
        /// </summary>
        public DateTime? OperateTime { get; set; }
        /// <summary>
        /// 取消人id
        /// </summary>
        public string CancelUserId { get; set; }
        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelTime { get; set; }

        #endregion Model

        #region ModelEx
        /// <summary>
        /// 显示适配车型
        /// </summary>
        public string ShowCarModel
        {
            get
            {
                string result = string.Empty;
                if (CarModelId != 0)
                {
                    result = new BLLCarLibrary().GetAllCarModelName(CarModelId);
                }
                return result;
            }
        }
        
        /// <summary>
        /// 状态字符说明
        /// </summary>
        public string StatusStr
        {
            get
            {
                string result = string.Empty;

                switch (Status)
                {
                    case 0:
                        result = "未处理";
                        break;
                    case 1:
                        result = "进行中";
                        break;
                    case 2:
                        result = "已过期";
                        break;
                    case 3:
                        result = "已取消";
                        break;
                }

                return result;
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public string UserInfo
        {
            get
            {
                BLLUser bllUser = new BLLUser();
                UserInfo result = new UserInfo();
                result = bllUser.GetUserInfo(UserId);

                return JsonConvert.SerializeObject(new
                {
                    WXNickname = result.WXNickname,
                    Name = bllUser.GetUserDispalyName(result),
                    Avatar = bllUser.GetUserDispalyAvatar(result)
                });
            }
        }

        #endregion
    }
}
