using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class WeixinSpread : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 微信推广码 16进制
        /// </summary>
        public string WeixinSpreadidHex {

            get {
                return Convert.ToString(_weixinspreadid, 16);
            
            
            }
        
        }
    }
}
