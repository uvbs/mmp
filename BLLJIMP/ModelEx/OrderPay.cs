using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class OrderPay
    {

        /// <summary>
        /// 0微信 1支付宝
        /// </summary>
        public string PayTypeEnName { 
            get{
                if (PayType == 0) return "weixin";
                if (PayType == 1) return "alipay";
                if (PayType == 2) return "jdpay";
                return "";
            }
        }
        /// <summary>
        /// 0微信 1支付宝
        /// </summary>
        public string PayTypeCnName
        {
            get
            {
                if (PayType == 0) return "微信";
                if (PayType == 1) return "支付宝";
                if (PayType == 2) return "京东支付";
                return "其他";
            }
        }
    }
}
