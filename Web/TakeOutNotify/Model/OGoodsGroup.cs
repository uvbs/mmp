using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.TakeOutNotify.Model
{
    public class OGoodsGroup
    {

        /// <summary>
        /// 分组名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分组类型 normal	普通商品  extra	配送费等额外信息   discount	折扣信息，红包，满减等
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 商品信息的列表
        /// </summary>
        public List<OGoodsItem> items { get; set; }
    }
}