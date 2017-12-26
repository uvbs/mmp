using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Socket
{
    [Serializable]
    public class User
    {
        public User()
        {
            connlogs = new List<Conn>();
            friends = new List<int>();
        }
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string ico { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime logintime { get; set; }
        /// <summary>
        /// 连接记录
        /// </summary>
        public List<Conn> connlogs { get; set; }
        /// <summary>
        /// 好友
        /// </summary>
        public List<int> friends { get; set; }
    }
}
