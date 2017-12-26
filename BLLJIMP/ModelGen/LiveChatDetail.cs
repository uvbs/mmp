using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 聊天内容
    /// </summary>
    public class LiveChatDetail : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 房间id
        /// </summary>
        public string RoomId { get; set; }
        /// <summary>
        /// 用户账号id
        /// </summary>
        public string UserAutoId { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        ///// <summary>
        ///// 是否已读
        ///// </summary>
        //public int IsRead { get; set; }
        /// <summary>
        /// 用户类型
        /// 0 用户
        /// 1 客服
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadImg { get; set; }



    }
}
