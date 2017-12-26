using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCJson;

namespace ZentCloud.BLLJIMP.Model
{
    public class WeixinFollowersInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        ///微信Openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        ///性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
       /// <summary>
       /// 城市
       /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public string Subscribe_time { get; set; }
        /// <summary>
        /// 关注状态
        /// 1 已关注
        /// 0 取消关注
        /// </summary>
        public int IsWeixinFollower { get; set; }
        /// <summary>
        /// 取消关注时间
        /// </summary>
        public string UnSubscribeTime { get; set; }
        /// <summary>
        /// 用户组 无用
        /// </summary>
        public string UserPmsGroup {get;set;}

        /// <summary>
        /// 关注来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 上级用户
        /// </summary>
        public string ParentUserId { get; set; }
        /// <summary>
        /// 上级显示名字
        /// </summary>
        public string ParentShowName { get; set; }

        //public UserInfo UserInfo
        //{

        //    get {
        //      return   new BLLUser("").GetUserInfoByOpenId(OpenId);
               
        //    }


        //}

    }
}
