using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 省市区
    /// </summary>
    public class Areas : RespBase
    {
        /// <summary>
        /// 省市区集合
        /// </summary>
        public List<AreasModel> areas { get; set; }

    }
    /// <summary>
    /// 省市区模型
    /// </summary>
    public class AreasModel
    {
        /// <summary>
        /// 省市区代码
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 上级代码
        /// </summary>
        public string parent_id { get; set; }
        /// <summary>
        /// 省市区名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 省市区类型
        /// </summary>
        public string type { get; set; }

    }


}
