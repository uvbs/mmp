using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 美帆我的会员卡
    /// </summary>
    public class MeifanMyCard : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 卡id
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardNum { get; set; }
       
        /// <summary>
        /// 有效期 (月)
        /// </summary>
        public int ValidMonth { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime ValidDate { get; set; }
       
    }
}
