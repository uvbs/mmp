using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLLiveChat : BLL
    {
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 获取聊天室详细内容
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public List<LiveChatDetail> GetLiveChatDetailList(string roomId)
        {
            return GetList<LiveChatDetail>(string.Format("RoomId={0} Order By InsertDate ASC", roomId));

        }

        /// <summary>
        /// 聊天室列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<LiveChatRoom> RoomList(string websiteOwner)
        {
            return GetList<LiveChatRoom>(string.Format("WebsiteOwner='{0}'", websiteOwner));
        }
        /// <summary>
        /// 是否客服在线
        /// </summary>
        /// <returns></returns>
        public bool IsKefuOnLine(string websiteOwner)
        {

            return GetCount<UserInfo>(string.Format("WebsiteOwner='{0}' And UserType!=0 And IsOnLine=1", websiteOwner)) > 0;

        }
        ///// <summary>
        ///// 客服是否已经加入
        ///// </summary>
        ///// <param name="roomId"></param>
        ///// <returns></returns>
        //public bool IsKefuJoin(string roomId)
        //{

        //    if (GetCount<LiveChatRoomUser>(string.Format("RoomId={0} And UserType=1", roomId)) > 0)
        //    {
        //        return true;
        //    }
        //    return false;

        //}

        ///// <summary>
        ///// 是否所有客服都离线
        ///// </summary>
        ///// <param name="websiteOwner"></param>
        ///// <returns></returns>
        //public bool IsAllKefuOffLine(string websiteOwner)
        //{
        //    //if (GetCount<UserInfo>(string.Format("WebsiteOwner='{0}' And IsOnLine=1 And WXOpenId In(Select WeiXinOpenID from ZCJ_WXKeFu Where WebsiteOwner='{0}')", websiteOwner)) == 0)

        //    if (GetCount<UserInfo>(string.Format("WebsiteOwner='{0}' And IsOnLine=1 And UserType!=2", websiteOwner)) == 0)
        //    {
        //        return true;
        //    }

        //    return false;

        //}

        ///// <summary>
        ///// 获取客服名字
        ///// </summary>
        ///// <param name="roomId"></param>
        ///// <returns></returns>
        //public string GetKefuName(string roomId) {

        //    string kefuName = "";
        //    var roomUser = Get<LiveChatRoomUser>(string.Format("RoomId={0} And UserType=1"));
        //    if (roomUser!=null)
        //    {
        //        return bllUser.GetUserDispalyName(bllUser.GetUserInfoByAutoID(int.Parse(roomUser.UserAutoId)));
        //    }
        //    return kefuName;

        
        //}





    }
}
