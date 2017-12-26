using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK
{
    /// <summary>
    /// 会员信息
    /// </summary>
    [Serializable]
    public class MemberInfo:RespBase
    {
        /// <summary>
        /// 会员信息
        /// </summary>
        public MemberModel member { get; set; }
        /// <summary>
        /// 默认收货地址信息
        /// </summary>
        public MemberAddressModel address { get; set; }

    }
    /// <summary>
    /// 会员信息模型
    /// </summary>
    public class MemberModel{
    
    /// <summary>
    /// 会员昵称
    /// </summary>
    public string memberName{get;set;}
    /// <summary>
    /// 手机号
    /// </summary>
    public string mobile { get; set; }
    /// <summary>
    /// 积分
    /// </summary>
    public float point { get; set; }
    /// <summary>
    /// 余额
    /// </summary>
    public float balance { get; set; }

    
    }



}
