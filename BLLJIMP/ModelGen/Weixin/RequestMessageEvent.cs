using System;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    public class RequestMessageEvent : RequestMessageBase, IRequestMessageBase
    {
        /// <summary>
        /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件)
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 微信卡券Id
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 微信卡券码
        /// </summary>
        public string UserCardCode { get; set; }
        /// <summary>
        /// 朋友openid
        /// </summary>
        public string FriendUserName { get; set; }
        /// <summary>
        /// 是否来自朋友1是
        /// </summary>
        public string IsGiveByFriend { get; set; }
    }
}
