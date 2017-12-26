using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 新浪发送短信内容
    /// </summary>
    [Serializable]
    public class SinaSmsInfo : ZCBLLEngine.ModelTable
    {
        public SinaSmsInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public int SinaAccountId { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNum { get; set; }

        /// <summary>
        ///  插入时间
        /// </summary>
        public DateTime Insertdate { get; set; }

        /// <summary>
        /// 随机验证码
        /// </summary>
        public string RandomCode { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
