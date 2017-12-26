using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 获取OrgCode
    /// </summary>
    public class OrgCode : RespBase
    {
        /// <summary>
        /// OrgCode
        /// </summary>
        public OrgCodeModel orgCode { get; set; }

    }

    public class OrgCodeModel {
        /// <summary>
        /// OrgCode
        /// </summary>
        public string orgCode { get; set; }
    
    }
}
