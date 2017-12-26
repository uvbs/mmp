using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 车型颜色单个对象，存入数据库为车型颜色字段： [ {CarModelColorInfo}... ]
    /// </summary>
    public class CarModelColorInfo
    {
        /// <summary>
        /// 颜色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 颜色值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 对应的图片
        /// </summary>
        public string Img { get; set; }
    }
}
