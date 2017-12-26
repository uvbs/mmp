using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK
{
    /// <summary>
    /// 会员收货地址列表
    /// </summary>
    [Serializable]
    public class MemberAddress : RespBase
    {
        /// <summary>
        /// 会员收货地址列表
        /// </summary>
        public List<MemberAddressModel> addresses { get; set; }

    }
    /// <summary>
    /// 单个收货地址模型
    /// </summary>
    public class MemberAddressModel
    {
    
        /// <summary>
        /// 省份代码
        /// </summary>
        public string provinceCode { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 市代码
        /// </summary>
        public string cityCode { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 区代码
        /// </summary>
        public string districtCode { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string memberAddress { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string name { get; set; }

    
    }
}
