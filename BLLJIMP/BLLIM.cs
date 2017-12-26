using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    public class BLLIM : BLL
    {
        NeteaseIMSDK.NeteaseIMSDK imSdk;
        public BLLIM()
        {
            var websiteInfo = GetWebsiteInfoModelFromDataBase();
            if (!string.IsNullOrWhiteSpace(websiteInfo.NIMAppKey) && !string.IsNullOrWhiteSpace(websiteInfo.NIMAppSecret))
            {
                imSdk = new NeteaseIMSDK.NeteaseIMSDK(websiteInfo.NIMAppKey, websiteInfo.NIMAppSecret);
            }
            else
            {
                throw new Exception("站点还未配置云信账号");
            }
        }
        public BLLIM(string nimAppKey, string nimAppSecret)
        {
            if (!string.IsNullOrWhiteSpace(nimAppKey) && !string.IsNullOrWhiteSpace(nimAppSecret))
            {
                imSdk = new NeteaseIMSDK.NeteaseIMSDK(nimAppKey, nimAppSecret);
            }
            else
            {
                throw new Exception("站点还未配置云信账号");
            }
        }
        
        /// <summary>
        /// 创建云信用户ID 并返回 Token
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="name"></param>
        /// <param name="props"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public string CreateUser(string accid, string name, string props, string icon)
        {
            NeteaseIMSDK.Model.CreateUserResp resp = imSdk.CreateUser(accid, name, props, icon);
            if(resp.Code!=200) throw new Exception(resp.Msg);
            return resp.Info.Token;
        }

        /// <summary>
        /// 云信用户token 并返回 Token
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="name"></param>
        /// <param name="props"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public string RefreshToken(string accid)
        {
            NeteaseIMSDK.Model.CreateUserResp resp = imSdk.RefreshToken(accid);
            if (resp.Code != 200) return "";
            return resp.Info.Token;
        }
    }
}
