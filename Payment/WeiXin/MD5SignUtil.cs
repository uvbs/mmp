using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Payment.WeiXin
{

    /// <summary>
    /// MD5 签名类
    /// </summary>
    public class MD5SignUtil
    {
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String Sign(String content, String key)
        {
            String signStr = "";
            signStr = content + "&key=" + key;
            return Payment.WeiXin.MD5Util.GetMD5(signStr, "UTF-8").ToUpper();

        }


        /// <summary>
        /// 通知验签
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sign"></param>
        /// <param name="parnerKey"></param>
        /// <returns></returns>
        public static bool VerifySignature(String content, String sign,
                String parnerKey)
        {
            String signStr = content + "&key=" + parnerKey;
            String calculateSign = Payment.WeiXin.MD5Util.GetMD5(signStr,"UTF-8").ToUpper();
            String tenpaySign = sign.ToUpper();
            return (calculateSign == tenpaySign);
        }
    }

}
