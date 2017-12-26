using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 聊天室
    /// </summary>
    public class LiveChatRoom : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 房间id 同CreateUserAutoId
        /// </summary>
        public string RoomId { get; set; }
        /// <summary>
        /// 创建用户的账户id
        /// </summary>
        public string CreateUserAutoId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnReadCount { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 客服是否已经加入
        /// 0 未加入
        /// 1 己加入
        /// </summary>
        public int IsKefuJoin { get; set; }
        /// <summary>
        /// 用户是否在线
        /// 0 离线
        /// 1 在线
        /// </summary>
        public int UserIsOnLine { get; set; }
        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string UserShowName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadImg { get; set; }


    }
}
