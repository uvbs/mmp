using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class MemberCallBackReq:ReqBase
    {
        /// <summary>
        /// 请求的数据实体，Json对象（单个对象或对象数组）
        /// </summary>
        public MemberInfo Args { get; set; }
    }
}
