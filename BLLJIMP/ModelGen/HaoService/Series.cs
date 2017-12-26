using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.HaoService
{
    /// <summary>
    /// 车系
    /// </summary>
    public class Series
    {
        /// <summary>
        /// 车系分类id
        /// </summary>
        public int I { get; set; }
        /// <summary>
        /// 车系名称
        /// </summary>
        public string N { get; set; }

        public List<INLBase> List { get; set; }
    }
}
