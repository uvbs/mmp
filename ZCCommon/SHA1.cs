using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace ZentCloud.Common
{
	public class SHA1
	{
        public static string Hmac_sha1(string secret, string mk)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(secret);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(mk);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }
		public static string SHA1_Encrypt(string Source_String)
		{
			byte[] array = Encoding.UTF8.GetBytes(Source_String);
			HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
			array = hashAlgorithm.ComputeHash(array);
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}
		public static string Encryption(string original, string key)
		{

			return SHA1.Encryption(original, key);
		}
		public void DesEncrypt(string inFilePath, string outFilePath)
		{
			try
			{
				FileStream fileStream = new FileStream(inFilePath, FileMode.Open, FileAccess.Read);
				FileStream fileStream2 = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream2.SetLength(0L);
				byte[] buffer = new byte[100];
				long num = 0L;
				long length = fileStream.Length;
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = new CryptoStream(fileStream2, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
				while (num < length)
				{
					int num2 = fileStream.Read(buffer, 0, 100);
					cryptoStream.Write(buffer, 0, num2);
					num += (long)num2;
				}
				cryptoStream.Close();
				fileStream2.Close();
				fileStream.Close();
			}
			catch
			{
			}
		}
		public void DesEncrypt(string inFilePath, string outFilePath, string strEncrKey)
		{
			byte[] rgbIV = new byte[]
			{
				18,
				52,
				86,
				120,
				144,
				171,
				205,
				239
			};
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
				FileStream fileStream = new FileStream(inFilePath, FileMode.Open, FileAccess.Read);
				FileStream fileStream2 = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream2.SetLength(0L);
				byte[] buffer = new byte[100];
				long num = 0L;
				long length = fileStream.Length;
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = new CryptoStream(fileStream2, dESCryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
				while (num < length)
				{
					int num2 = fileStream.Read(buffer, 0, 100);
					cryptoStream.Write(buffer, 0, num2);
					num += (long)num2;
				}
				cryptoStream.Close();
				fileStream2.Close();
				fileStream.Close();
			}
			catch
			{
			}
		}
		public static byte[] makeSHA1(byte[] original)
		{
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			return sHA1CryptoServiceProvider.ComputeHash(original);
		}
	}
}
