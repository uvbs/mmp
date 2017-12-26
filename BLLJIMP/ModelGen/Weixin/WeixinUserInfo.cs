using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCJson;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 微信用户基本信息
    /// </summary>
    [Serializable]
    public class WeixinUserInfo
    {
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [JsonProperty("sex")]
        public int Sex { get; set; }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        [JsonProperty("headimgurl")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        [JsonProperty("privilege")]
        public List<string> Privilege { get; set; }
        /// <summary>
        /// UnionId
        /// </summary>
         [JsonProperty("unionid")]
        public string UnionID { get; set; }

        //openid	 用户的唯一标识
        //nickname	 用户昵称
        //sex	 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        //province	 用户个人资料填写的省份
        //city	 普通用户个人资料填写的城市
        //country	 国家，如中国为CN
        //headimgurl	 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        //privilege	 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
    }
}

