using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 新浪账户信息
    /// </summary>
    [Serializable]
    public class SinaAccountInfo : ZCBLLEngine.ModelTable
    {

        public SinaAccountInfo(){}

        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 新浪账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 新浪密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 站点拥有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 发送内容&text& 验证码
        /// </summary>
        public string Ntext { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }


    }
}
