using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 聊天室用户 后台登录用户 (实时提醒用)
    /// </summary>
    public class LiveChatAdminRoomUser : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserAutoId { get; set; }
        /// <summary>
        /// Socket SessionId
        /// </summary>
        public string SocketSessionId { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime InsertDate { get; set; }



    }
}
