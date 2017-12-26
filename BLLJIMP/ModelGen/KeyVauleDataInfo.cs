using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 键值对数据模型
    /// </summary>
    public partial  class KeyVauleDataInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PreKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OrderBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataName { get; set; }



    }
}
