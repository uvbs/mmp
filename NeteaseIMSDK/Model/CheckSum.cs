using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.Common;

namespace NeteaseIMSDK.Model
{
    public class CheckSum
    {
        public string AppKey { get; set; }

        public string AppSecret { get; set; }

        public string Nonce { get; set; }

        public string CurTime { get; set; }

        public string CheckSumResult {
            get
            {
                string result = string.Empty;

                result = SHA1.SHA1_Encrypt(AppSecret + Nonce + CurTime).ToLower();

                return result;
            }
        }

        public static CheckSum GetCheckSum(string appKey,string appSecret)
        {
            var result = new CheckSum()
            {
                AppKey = appKey,
                AppSecret = appSecret,
                Nonce = Guid.NewGuid().ToString(),
                CurTime = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString()
            };
            
            return result;
        }

    }
}
