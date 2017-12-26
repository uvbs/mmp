using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.LiveChat
{
    public partial class LiveChat : System.Web.UI.Page
    {
        BLLLiveChat bll = new BLLLiveChat();
        BLLUser bllUser = new BLLUser();
        public List<LiveChatDetail> RecordList = new List<LiveChatDetail>();
        /// <summary>
        /// WebSocket主机
        /// </summary>
        public string WebSocketHost = "";
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo WebsiteInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string roomId = Request["room_id"];
            var room = bll.Get<LiveChatRoom>(string.Format("RoomId={0}",roomId));

            RecordList = bll.GetLiveChatDetailList(room.RoomId);
            room.UnReadCount = 0;
            bll.Update(room);
            WebSocketHost = ZentCloud.Common.ConfigHelper.GetConfigString("WebSocketHost");
            WebsiteInfo = bll.GetWebsiteInfoModelFromDataBase(bll.WebsiteOwner);
        }
    }
}