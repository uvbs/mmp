using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Security.Cryptography;
namespace Tencent
{
    /// <summary>
    /// 消息加解密类
    /// </summary>
    public class WXBizMsgCrypt
    {
        /// <summary>
        /// token
        /// </summary>
        string token;
        /// <summary>
        /// AESKey
        /// </summary>
        string encodingAESKey;
        /// <summary>
        /// 组件 CommpentAppId
        /// </summary>
        string appId;
        /// <summary>
        /// 错误码
        /// </summary>
        enum WXBizMsgCryptErrorCode
        {
            WXBizMsgCrypt_OK = 0,
            /// <summary>
            ///-40001 ： 签名验证错误
            /// </summary>
            WXBizMsgCrypt_ValidateSignature_Error = -40001,
            /// <summary>
            /// 40002 :  xml解析失败
            /// </summary>
            WXBizMsgCrypt_ParseXml_Error = -40002,
            /// <summary>
            /// 40003 :  sha加密生成签名失败
            /// </summary>
            WXBizMsgCrypt_ComputeSignature_Error = -40003,
            /// <summary>
            /// 40004 :  AESKey 非法
            /// </summary>
            WXBizMsgCrypt_IllegalAesKey = -40004,
            /// <summary>
            /// 40005 :  appid 校验错误
            /// </summary>
            WXBizMsgCrypt_ValidateAppid_Error = -40005,
            /// <summary>
            /// 40006 :  AES 加密失败
            /// </summary>
            WXBizMsgCrypt_EncryptAES_Error = -40006,
            /// <summary>
            /// 40007 ： AES 解密失败
            /// </summary>
            WXBizMsgCrypt_DecryptAES_Error = -40007,
            /// <summary>
            /// 40008 ： 解密后得到的buffer非法
            /// </summary>
            WXBizMsgCrypt_IllegalBuffer = -40008,
            /// <summary>
            /// 40009 :  base64加密异常
            /// </summary>
            WXBizMsgCrypt_EncodeBase64_Error = -40009,
            /// <summary>
            /// 40010 :  base64解密异常
            /// </summary>
            WXBizMsgCrypt_DecodeBase64_Error = -40010
        };

        //构造函数
        // @param sToken: 公众平台上，开发者设置的Token
        // @param sEncodingAESKey: 公众平台上，开发者设置的EncodingAESKey
        // @param sAppID: 公众帐号的appid
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sToken"> 公众平台上，开发者设置的Token</param>
        /// <param name="sEncodingAESKey">公众平台上，开发者设置的EncodingAESKey</param>
        /// <param name="sAppID">公众帐号的appid</param>
        public WXBizMsgCrypt(string sToken, string sEncodingAESKey, string sAppID)
        {
            token = sToken;
            appId = sAppID;
            encodingAESKey = sEncodingAESKey;
        }


        // 检验消息的真实性，并且获取解密后的明文
        // @param signature: 
        // @param timeStamp: 
        // @param nonce: 
        // @param postData: 
        // @param msg: 解密后的原文，当return返回0时有效
        // @return: 成功0，失败返回对应的错误码
        /// <summary>
        /// 解密消息
        /// </summary>
        /// <param name="signature">签名串，对应URL参数的msg_signature</param>
        /// <param name="timeStamp">时间戳，对应URL参数的timestamp</param>
        /// <param name="nonce">随机串，对应URL参数的nonce</param>
        /// <param name="postData">密文，对应POST请求的数据</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int DecryptMsg(string signature, string timeStamp, string nonce, string postData, ref string msg)
        {
            if (encodingAESKey.Length != 43)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
            }
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            string sEncryptMsg;
            try
            {
                doc.LoadXml(postData);
                root = doc.FirstChild;
                sEncryptMsg = root["Encrypt"].InnerText;
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ParseXml_Error;
            }
            //verify signature
            int ret = 0;
            ret = VerifySignature(token, timeStamp, nonce, sEncryptMsg, signature);
            if (ret != 0)
                return ret;
            //decrypt
            string cpid = "";
            try
            {
                msg = Cryptography.AESDecrypt(sEncryptMsg, encodingAESKey, ref cpid);
            }
            catch (FormatException)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error;
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecryptAES_Error;
            }
            if (cpid != appId)
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateAppid_Error;
            return 0;
        }

        //将企业号回复用户的消息加密打包
        // @param replyMsg: 企业号待回复用户的消息，xml格式的字符串
        // @param timeStamp: 时间戳，可以自己生成，也可以用URL参数的timestamp
        // @param nonce: 随机串，可以自己生成，也可以用URL参数的nonce
        // @param encryptMsg: 加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,
        //						当return返回0时有效
        // return：成功0，失败返回对应的错误码
        /// <summary>
        /// 加密消息
        /// </summary>
        /// <param name="replyMsg">企业号待回复用户的消息，xml格式的字符串</param>
        /// <param name="timeStamp">时间戳，可以自己生成，也可以用URL参数的timestamp</param>
        /// <param name="nonce">随机串，可以自己生成，也可以用URL参数的nonce</param>
        /// <param name="encryptMsg">密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串</param>
        /// <returns></returns>
        public int EncryptMsg(string replyMsg, string timeStamp, string nonce, ref string encryptMsg)
        {
            if (encodingAESKey.Length != 43)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
            }
            string raw = "";
            try
            {
                raw = Cryptography.AESEncrypt(replyMsg, encodingAESKey, appId);
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_EncryptAES_Error;
            }
            string msgSigature = "";
            int ret = 0;
            ret = GenarateSinature(token, timeStamp, nonce, raw, ref msgSigature);
            if (0 != ret)
                return ret;
            encryptMsg = "";

            string encryptLabelHead = "<Encrypt><![CDATA[";
            string encryptLabelTail = "]]></Encrypt>";
            string msgSigLabelHead = "<MsgSignature><![CDATA[";
            string msgSigLabelTail = "]]></MsgSignature>";
            string timeStampLabelHead = "<TimeStamp><![CDATA[";
            string timeStampLabelTail = "]]></TimeStamp>";
            string nonceLabelHead = "<Nonce><![CDATA[";
            string nonceLabelTail = "]]></Nonce>";

            encryptMsg = encryptMsg + "<xml>" + encryptLabelHead + raw + encryptLabelTail;
            encryptMsg = encryptMsg + msgSigLabelHead + msgSigature + msgSigLabelTail;
            encryptMsg = encryptMsg + timeStampLabelHead + timeStamp + timeStampLabelTail;
            encryptMsg = encryptMsg + nonceLabelHead + nonce + nonceLabelTail;
            encryptMsg += "</xml>";
            return 0;
        }

        public class DictionarySort : System.Collections.IComparer
        {
            /// <summary>
            /// 字典比较
            /// </summary>
            /// <param name="oLeft"></param>
            /// <param name="oRight"></param>
            /// <returns></returns>
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }
        //Verify Signature
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sToken"></param>
        /// <param name="sTimeStamp"></param>
        /// <param name="sNonce"></param>
        /// <param name="sMsgEncrypt"></param>
        /// <param name="sSigture"></param>
        /// <returns></returns>
        private static int VerifySignature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, string sSigture)
        {
            string hash = "";
            int ret = 0;
            ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
            if (ret != 0)
                return ret;
            //System.Console.WriteLine(hash);
            if (hash == sSigture)
                return 0;
            else
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateSignature_Error;
            }
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="msgEncrypt"></param>
        /// <param name="msgSignature"></param>
        /// <returns></returns>
        public static int GenarateSinature(string token, string timeStamp, string nonce, string msgEncrypt, ref string msgSignature)
        {
            ArrayList list = new ArrayList();
            list.Add(token);
            list.Add(timeStamp);
            list.Add(nonce);
            list.Add(msgEncrypt);
            list.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < list.Count; ++i)
            {
                raw += list[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ComputeSignature_Error;
            }
            msgSignature = hash;
            return 0;
        }
    }
}
