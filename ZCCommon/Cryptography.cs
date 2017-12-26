using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
namespace Tencent
{
    public class Cryptography
    {
        public static UInt32 HostToNetworkOrder(UInt32 inval)
        {
            UInt32 outValue = 0;
            for (int i = 0; i < 4; i++)
                outValue = (outValue << 8) + ((inval >> (i * 8)) & 255);
            return outValue;
        }

        public static Int32 HostToNetworkOrder(Int32 inval)
        {
            Int32 outValue = 0;
            for (int i = 0; i < 4; i++)
                outValue = (outValue << 8) + ((inval >> (i * 8)) & 255);
            return outValue;
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="input">密文</param>
        /// <param name="encodingAESKey"></param>
        /// <returns></returns>
        /// 
        public static string AESDecrypt(String input, string encodingAESKey, ref string appId)
        {
            byte[] key;
            key = Convert.FromBase64String(encodingAESKey + "=");
            byte[] iv = new byte[16];
            Array.Copy(key, iv, 16);
            byte[] btmpMsg = AESDecrypt(input, iv, key);

            int len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);


            byte[] bMsg = new byte[len];
            byte[] bAppid = new byte[btmpMsg.Length - 20 - len];
            Array.Copy(btmpMsg, 20, bMsg, 0, len);
            Array.Copy(btmpMsg, 20+len , bAppid, 0, btmpMsg.Length - 20 - len);
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            appId = Encoding.UTF8.GetString(bAppid);

            
            return oriMsg;
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingAESKey"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static String AESEncrypt(String input, string encodingAESKey, string appId)
        {
            byte[] dey;
            dey = Convert.FromBase64String(encodingAESKey + "=");
            byte[] iv = new byte[16];
            Array.Copy(dey, iv, 16);
            string randCode = CreateRandCode(16);
            byte[] bRand = Encoding.UTF8.GetBytes(randCode);
            byte[] bAppid = Encoding.UTF8.GetBytes(appId);
            byte[] btmpMsg = Encoding.UTF8.GetBytes(input);
            byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
            byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppid.Length + btmpMsg.Length];

            Array.Copy(bRand, bMsg, bRand.Length);
            Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bAppid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bAppid.Length);
   
            return AESEncrypt(bMsg, iv, dey);

        }
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        private static string CreateRandCode(int codeLen)
        {
            string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
            {
                codeLen = 16;
            }
            string[] arr = codeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }
        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static String AESEncrypt(String input, byte[] iv, byte[] key)
        {
            var aes = new RijndaelManaged();
            //秘钥的大小，以位为单位
            aes.KeySize = 256;
            //支持的块大小
            aes.BlockSize = 128;
            //填充模式
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = key;
            aes.IV = iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }
            String output = Convert.ToBase64String(xBuff);
            return output;
        }
        /// <summary>
        /// 消息加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static String AESEncrypt(byte[] input, byte[] iv, byte[] key)
        {
            var aes = new RijndaelManaged();
            //秘钥的大小，以位为单位
            aes.KeySize = 256;
            //支持的块大小
            aes.BlockSize = 128;
            //填充模式
            //aes.Padding = PaddingMode.PKCS7;
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.CBC;
            aes.Key = key;
            aes.IV = iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            #region 自己进行PKCS7补位，用系统自己带的不行
            byte[] msg = new byte[input.Length + 32 - input.Length % 32];
            Array.Copy(input, msg, input.Length);
            byte[] pad = KCS7Encoder(input.Length);
            Array.Copy(pad, 0, msg, input.Length, pad.Length);
            #endregion

            #region 注释的也是一种方法，效果一样
            //ICryptoTransform transform = aes.CreateEncryptor();
            //byte[] xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);
            #endregion

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            String output = Convert.ToBase64String(xBuff);
            return output;
        }

        private static byte[] KCS7Encoder(int textLength)
        {
            int blockSize = 32;
            // 计算需要填充的位数
            int amountToPad = blockSize - (textLength % blockSize);
            if (amountToPad == 0)
            {
                amountToPad = blockSize;
            }
            // 获得补位所用的字符
            char padChr = chr(amountToPad);
            string tmp = "";
            for (int index = 0; index < amountToPad; index++)
            {
                tmp += padChr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }
        /**
         * 将数字转化成ASCII码对应的字符，用于对明文进行补码
         * 
         * @param a 需要转化的数字
         * @return 转化得到的字符
         */
        static char chr(int a)
        {

            byte target = (byte)(a & 0xFF);
            return (char)target;
        }
        /// <summary>
        /// 消息解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] AESDecrypt(String input, byte[] iv, byte[] key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = key;
            aes.IV = iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(input);
                    byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = Decode2(ms.ToArray());
            }
            return xBuff;
        }
        private static byte[] Decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
    }
}
