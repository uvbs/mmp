using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ZentCloud.Common
{
    /// <summary>
    /// Encrypt 的摘要说明。
    /// </summary>
    public class DEncrypt
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public DEncrypt()
        {
        }

        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        /// 使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "EOYOOCORP");
        }
        /// <summary>
        /// 使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, "EOYOOCORP", System.Text.Encoding.Default);
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string
        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(original);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }
        /// <summary>
        /// 使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }
        #endregion

        #region 使用 缺省密钥字符串 加密/解密/byte[]
        /// <summary>
        /// 使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("EOYOOCORP");
            return Decrypt(encrypted, key);
        }
        /// <summary>
        /// 使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original">原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("EOYOOCORP");
            return Encrypt(original, key);
        }
        #endregion

        #region  使用 给定密钥 加密/解密/byte[]

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }


        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        #endregion

        #region MD5类的使用
        /**/
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="input">要转换的字符串</param>
        /// <returns>转换后的MD5</returns>

        public static string GetMD5(string input, string encoding="")
        {
            MD5 md5 = MD5.Create();
            string result = "";
            if (string.IsNullOrWhiteSpace(encoding))
            {
                byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));
                for (int i = 0; i < data.Length; i++)
                {
                    result += data[i].ToString("x2");
                }
            }
            else
            {
                byte[] data = md5.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(input));
                for (int i = 0; i < data.Length; i++)
                {
                    result += data[i].ToString("x2");
                }
            }
            return result;
        }
        /**/
        /// <summary>
        /// MD5比较
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="data">比较的字符串</param>
        /// <returns>是否相同</returns>
        /// 
        public bool passWordCheck(string input, string data)
        {
            string hashInput = GetMD5(input);
            if (hashInput.Equals(data))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 臻云加密算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ZCEncrypt(string input)
        {
            /*  运算过程
             * 
             * 1.md5加密
             * 2.字符串反转；
             * 3.数字用9去减；
             * 4.3变成!，6变成#；
             * 5.转成大写；
             * 
             */

            string result = "";

            input = GetMD5(input);
            input = StringHelper.ReverseStr(input);

            foreach (var item in input.ToCharArray())
            {
                string tmpStr = item.ToString();
                int tmpInt = 0;
                if (int.TryParse(tmpStr, out tmpInt))
                {
                    tmpStr = (9 - tmpInt).ToString();
                }
                result += tmpStr;
            }

            result = result.Replace("3", "!").Replace("6", "#").ToUpper();

            return result;
        }
        /// <summary>
        /// 臻云加密数据，解密到MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ZCDecryptToMd5(string input)
        {
            /*  运算过程
             * 
             * 1.!变成3，#变成6；
             * 2.转成小写；
             * 3.数字用9去减；
             * 4.字符串反转；
             * 返回MD5
             * 
             */

            string result = "";
            input = input.Replace("!", "3").Replace("#", "6").ToLower();

            foreach (var item in input.ToCharArray())
            {
                string tmpStr = item.ToString();
                int tmpInt = 0;
                if (int.TryParse(tmpStr, out tmpInt))
                {
                    tmpStr = (9 - tmpInt).ToString();
                }
                result += tmpStr;
            }
            result = StringHelper.ReverseStr(result);
            return result;
        }

        //var key1 = DEncrypt.ZCDecryptToMd5("FB152!BFE2EC5B57CE2#BF9!A40480AA");
        //var key2 = DEncrypt.GetMD5("121.196.232.134");
    }
}
