using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微商城-收货地址库列表
    /// </summary>
    public class WXConsigneeAddress : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID { get; set; }


        /// <summary>
        /// 收货人
        /// </summary>
        public string ConsigneeName { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 是否默认收货地址 1表示默认
        /// </summary>
        public string IsDefault { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 省份代码
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///城市代码
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 城市区域
        /// </summary>
        public string Dist { get; set; }
        /// <summary>
        /// 城市区域代码
        /// </summary>
        public string DistCode { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }


    }



}
