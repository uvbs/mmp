using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 美帆主卡
    /// </summary>
    public class MeifanCard : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 卡id
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// 名称-英文
        /// </summary>
        public string CardNameEn { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string CardImg { get; set; }
        /// <summary>
        /// 类型
        ///个人
        ///personal
        ///家庭
        ///family
        ///船东卡
        ///chuandong
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 入会费
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal ServerAmount { get; set; }
        /// <summary>
        /// 有效期 (月)
        /// </summary>
        public int ValidMonth { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string Websiteowner { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 是否禁用
        /// 0启用1禁用
        /// </summary>
        public int IsDisable { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }
    }
}
