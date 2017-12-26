using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.WuBuHui.HRLove
{
    public partial class HRLoveInfo : System.Web.UI.Page
    {
        public GameFriendChain gameFriend = new GameFriendChain();
        public GameFriendChain gameFriendPrevious = new GameFriendChain();
        public GameFriendChain gameFriendNext1 = new GameFriendChain();
        public GameFriendChain gameFriendNext2 = new GameFriendChain();
        public string gameFriendPreviousUrl = string.Empty;
        public string gameFriendNext1Url = string.Empty;
        public string gameFriendNext2Url = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            int autoId = int.Parse(Request["AutoId"]);
            if (autoId != null)
            {
                BLLGameFriendChain bll = new BLLGameFriendChain();
                gameFriend = bll.Get<GameFriendChain>(string.Format("AutoId='{0}'", autoId));
                gameFriendPrevious = bll.Get<GameFriendChain>(string.Format("UserId='{0}'", gameFriend.PreviousUserId));
                gameFriendNext1 = bll.Get<GameFriendChain>(string.Format("UserId='{0}'", gameFriend.Next1UserId));
                gameFriendNext2 = bll.Get<GameFriendChain>(string.Format("UserId='{0}'", gameFriend.Next2UserId));


                if (gameFriendPrevious != null)
                {
                    
                
                gameFriendPreviousUrl = string.Format("/WuBuHui/HRLove/HRLoveInfo.aspx?AutoId={0}", gameFriendPrevious.AutoId);
                }

                if (gameFriendNext1 != null)
                {
                    gameFriendNext1Url = string.Format("/WuBuHui/HRLove/HRLoveInfo.aspx?AutoId={0}", gameFriendNext1.AutoId);
                }

                if (gameFriendNext2 != null)
                {
                    gameFriendNext2Url = string.Format("/WuBuHui/HRLove/HRLoveInfo.aspx?AutoId={0}", gameFriendNext2.AutoId);
                }
            }
        }
    }
}