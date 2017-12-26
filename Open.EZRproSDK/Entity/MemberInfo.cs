using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    [Serializable]
    public class MemberInfo
    {
        /// <summary>
        /// 线下会员卡号
        /// </summary>
        public string OldCode { get; set; }
        /// <summary>
        /// 线上会员卡号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 会员昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别，0=未知、1=男、2=女
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 生日(yyyy-MM-dd格式)
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 用户微信OpenID
        /// </summary>
        public string WxNo { get; set; }
        /// <summary>
        /// 用户微信UnionID
        /// </summary>
        public string WxUnionId { get; set; }
        /// <summary>
        /// 微博号
        /// </summary>
        public string WeibNo { get; set; }
        /// <summary>
        /// Qq号
        /// </summary>
        public string QqNo { get; set; }
        /// <summary>
        /// 淘宝号
        /// </summary>
        public string TbNo { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 开卡门店
        /// </summary>
        public string RegShop { get; set; }
        /// <summary>
        /// 开卡日期(yyyy-MM-dd格式)
        /// </summary>
        public string RegDate { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
