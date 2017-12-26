using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class WBAttentionInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 用户名 对应WBBaseInfo UserID 或 WBCompanyInfo UserID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 被关注对象编号 对应WBBaseInfo AutoID 或 WBCompanyInfo AutoID 或WBProjectInfo AutoID
        /// </summary>
        public int AttentionAutoID { get; set; }
        /// <summary>
        /// 0代表关注基地 1代表关注企业 3代表关注项目
        /// </summary>
        public int AttentionType { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime InsertDate { get; set; }
    }
}
