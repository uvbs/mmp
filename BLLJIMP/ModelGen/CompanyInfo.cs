using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 公司名录
	/// </summary>
	[Serializable]
	public partial class CompanyInfo : ZCBLLEngine.ModelTable
	{
		public CompanyInfo()
		{}
		#region Model
		private int? _companyid;
		private string _companyname;
		private string _summary;
		private string _address;
		private string _province;
		private string _city;
		private string _district;
		private string _area;
		private string _lat;
		private string _lng;
		private string _linkname;
		private string _linktel;
		private string _linkphone;
		private string _linkemail;
		private string _updater;
		private DateTime _updatetime;
		private int? _workinghoursdiscount;
		private int? _partsdiscount;
		private string _vipday;
		/// <summary>
		/// 公司编号
		/// </summary>
		public int? CompanyId
		{
			set{ _companyid=value;}
			get{return _companyid;}
		}
		/// <summary>
		/// 公司名称
		/// </summary>
		public string ComPanyName
		{
			set{ _companyname=value;}
			get{return _companyname;}
		}
		/// <summary>
		/// 简要介绍
		/// </summary>
		public string Summary
		{
			set{ _summary=value;}
			get{return _summary;}
		}
		/// <summary>
		/// 地址
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 省份
		/// </summary>
		public string Province
		{
			set{ _province=value;}
			get{return _province;}
		}
		/// <summary>
		/// 城市
		/// </summary>
		public string City
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 区
		/// </summary>
		public string District
		{
			set{ _district=value;}
			get{return _district;}
		}
		/// <summary>
		/// 具体地址
		/// </summary>
		public string Area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// 经度
		/// </summary>
		public string Lat
		{
			set{ _lat=value;}
			get{return _lat;}
		}
		/// <summary>
		/// 纬度
		/// </summary>
		public string Lng
		{
			set{ _lng=value;}
			get{return _lng;}
		}
		/// <summary>
		/// 联系人
		/// </summary>
		public string LinkName
		{
			set{ _linkname=value;}
			get{return _linkname;}
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		public string LinkTel
		{
			set{ _linktel=value;}
			get{return _linktel;}
		}
		/// <summary>
		///联系手机
		/// </summary>
		public string LinkPhone
		{
			set{ _linkphone=value;}
			get{return _linkphone;}
		}
		/// <summary>
		/// 联系Email
		/// </summary>
		public string LinkEmail
		{
			set{ _linkemail=value;}
			get{return _linkemail;}
		}
		/// <summary>
		/// 更新用户
		/// </summary>
		public string Updater
		{
			set{ _updater=value;}
			get{return _updater;}
		}
		/// <summary>
		/// 最后编辑时间
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WorkingHoursDiscount
		{
			set{ _workinghoursdiscount=value;}
			get{return _workinghoursdiscount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PartsDiscount
		{
			set{ _partsdiscount=value;}
			get{return _partsdiscount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string VIPDay
		{
			set{ _vipday=value;}
			get{return _vipday;}
		}
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 公司Logo
        /// </summary>
        public string CompanyLogo { get; set; }

        /// <summary>
        /// 企业介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 公司性质
        /// </summary>
        public string Nature { get; set; }
        /// <summary>
        /// 董事
        /// </summary>
        public string Director { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string WebsiteUrl { get; set; }
		#endregion Model

	}
}

