using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Payment.JDPay
{
    public class RSACoder
    {

        /// <summary>
        /// 私钥加密 .Net平台默认是使用公钥进行加密，私钥进行解密。私钥加密需要自己实现或者使用第三方dll
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] encryptByPrivateKey(String data, String key)
        {
            String priKey = key.Trim();
            String xmlPrivateKey = RSAPrivateKeyJava2DotNet(priKey);
            //加载私钥  
            RSACryptoServiceProvider privateRsa = new RSACryptoServiceProvider();
            privateRsa.FromXmlString(xmlPrivateKey);
            //转换密钥  
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetKeyPair(privateRsa);
            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");// 参数与Java中加密解密的参数一致       
            c.Init(true, keyPair.Private); //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥 
            byte[] DataToEncrypt = Encoding.UTF8.GetBytes(data);
            byte[] outBytes = c.DoFinal(DataToEncrypt);//加密  
            return outBytes;

        }


        /// <summary>
        /// 用公钥解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] decryptByPublicKey(String data, String key)
        {
            String pubKey = key.Trim();
            String xmlPublicKey = RSAPublicKeyJava2DotNet(pubKey);

            RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
            publicRsa.FromXmlString(xmlPublicKey);

            AsymmetricKeyParameter keyPair = DotNetUtilities.GetRsaPublicKey(publicRsa);
            //转换密钥  
            // AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(publicRsa);
            //

            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");// 参数与Java中加密解密的参数一致 

            //IBufferedCipher c = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            //IBufferedCipher c = CipherUtilities.GetCipher("RSA");
            c.Init(false, keyPair); //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥 
            byte[] dataToEncrypt = Convert.FromBase64String(data);

            //int blockSize = c.GetBlockSize();
            //byte[] raw = new byte[dataToEncrypt.Length]; 
            //int outputSize = 128;
            //int i = 0;
            //while (data.Length - i * blockSize > 0)
            //{
            //    if (data.Length - i * blockSize > blockSize)//处理数据长度大于一块的情况  

            //        c.DoFinal(dataToEncrypt, i * blockSize, blockSize, raw, i * outputSize);
                    
            //    else//处理数据长度小于一块的情况  

            //       // c.DoFinal(data, i * blockSize, data.Length - i * blockSize, raw, i * outputSize);
            //       return c.DoFinal(dataToEncrypt);//解密  

            //    i++;

            //}



            // byte[] outBytes = c.DoFinal(dataToEncrypt, 0, dataToEncrypt.Length);
            byte[] outBytes = c.DoFinal(dataToEncrypt);//解密  


            return outBytes;
        }


        /// <summary>
        /// 公钥解密（key公钥解密）- 必须对应私钥加密
        /// </summary>
        /// <param name="source">加密后的数据（密文）</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string DecryptByPublibKey(string source, string publicKey)
        {
            var publicInfoByte = Convert.FromBase64String(publicKey);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入 
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo.GetInstance(pubKeyObj));
            //开始解密
            IAsymmetricBlockCipher cipher = new RsaEngine();
            cipher.Init(false, pubKey);
            //解密已加密的数据
            byte[] encryptedData = Convert.FromBase64String(source);
            encryptedData = cipher.ProcessBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(encryptedData, 0, encryptedData.Length);
        }

        /// <summary>
        /// RSA私钥格式转换，java->.net
        /// </summary>
        /// <param name="privateKey">java生成的RSA私钥</param>
        /// <returns></returns>
        private static string RSAPrivateKeyJava2DotNet(string privateKey)
        {
            var privateKeyFromBase64String = Convert.FromBase64String(privateKey);

            var createKey = PrivateKeyFactory.CreateKey(privateKeyFromBase64String);

            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)createKey;

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA公钥格式转换，java->.net
        /// </summary>
        /// <param name="publicKey">java生成的公钥</param>
        /// <returns></returns>
        private static string RSAPublicKeyJava2DotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }
    }
}