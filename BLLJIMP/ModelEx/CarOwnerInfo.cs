
using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// CarOwnerInfo
	/// </summary>
	[Serializable]
	public partial class CarOwnerInfo : ZCBLLEngine.ModelTable
	{
		public CarOwnerInfo()
		{}
		#region Model
		private string _userid;
		private string _carnumber;
		private string _vin;
		private string _drivinglicensetype;
		/// <summary>
		/// 
		/// </summary>
		public string UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CarModelId { get; set; }
		/// <summary>
		/// 车牌号
		/// </summary>
		public string CarNumber
		{
			set{ _carnumber=value;}
			get{return _carnumber;}
		}
		/// <summary>
		/// 车辆上牌时间
		/// </summary>
		public DateTime? CarNumberTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string VIN
		{
			set{ _vin=value;}
			get{return _vin;}
		}
		/// <summary>
		/// 驾照类型
		/// </summary>
		public string DrivingLicenseType
		{
			set{ _drivinglicensetype=value;}
			get{return _drivinglicensetype;}
		}
		/// <summary>
		/// 驾照领取时间
		/// </summary>
		public DateTime? DrivingLicenseTime { get; set; }

        public CarModelInfo CarModel { get; set; }

        #endregion Model

        /// <summary>
        /// 根据用户扩展信息获取车主信息
        /// </summary>
        /// <param name="userExp"></param>
        /// <returns></returns>
        public static CarOwnerInfo GetDataByUserExpand(UserExpand userExp)
        {
            CarOwnerInfo data = null;

            if (userExp == null) return null;

            data = new CarOwnerInfo();

            /*车主信息：
            ex1 我的车型、
            ex2 车牌号码、
            ex3 vin号、
            ex4 车辆上牌时间（yyyy-MM-dd）、
            ex5 驾照领取时间 (yyyy-MM-dd)、
            ex6 驾照类型
            */

            data.UserId = userExp.UserId;
            data.CarModelId = Convert.ToInt32(userExp.Ex1);

            data.CarNumber = userExp.Ex2;
            data.VIN = userExp.Ex3;
            data.DrivingLicenseType = userExp.Ex6;
            DateTime carNumberTime, drivingLicenseTime;

            if (DateTime.TryParse(userExp.Ex4, out carNumberTime))
            {
                data.CarNumberTime = carNumberTime;
            }

            if (DateTime.TryParse(userExp.Ex5, out drivingLicenseTime))
            {
                data.DrivingLicenseTime = drivingLicenseTime;
            }


            if (data.CarModelId > 0)
            {
                data.CarModel = new BLLCarLibrary().GetCarModelInfo(data.CarModelId);
            }

            return data;
        }

	}
}

