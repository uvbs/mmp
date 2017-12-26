using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 聊天室用户
    /// </summary>
    public class LiveChatRoomUser : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 房间id
        /// </summary>
        public string RoomId { get; set; }
        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserAutoId { get; set; }
        /// <summary>
        /// 用户类型
        /// 0 用户
        /// 1 客服
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// Socket SessionId
        /// </summary>
        public string SocketSessionId { get; set; }




    }
}
