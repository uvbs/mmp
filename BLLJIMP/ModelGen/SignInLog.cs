using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 签到记录
    /// </summary>
    [Serializable]
    public partial class SignInLog : ModelTable
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public long AutoID { get; set; }
        /// <summary>
        /// 地址ID
        /// </summary>
        public int AddressId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 距离(米)
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// 签到用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 状态 0签到失败 1签到成功     会员签到：1补签 0签到
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类型 [默认的是lbs签到   Sign:用户签到]
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 格式 20170106
        /// </summary>
        public string SignInDate { get; set; }

        /// <summary>
        /// 获得积分,补签扣除积分,补签日期
        /// </summary>
        public string Ex1 { get; set; }

    }
}
