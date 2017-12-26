using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.LiveChat.Room
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLLiveChat bll = new BLLLiveChat();
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            //string websiteowner = "hf";
             var sourceData = bll.RoomList(bll.WebsiteOwner);
             //var sourceData = bll.RoomList("hf");
            var data = from p in sourceData
                       select new
                       {
                           room_id = p.RoomId,
                           user_info = new
                           {
                               name = GetShowName(p.UserShowName),
                               head_img = p.UserHeadImg

                           },
                           is_kefu_join = p.IsKefuJoin==1?true:false,//客服是否已经加入
                           //kefu_name=bll.GetKefuName(p.RoomId),
                           is_online = p.UserIsOnLine,//是否在线
                           un_read_count = p.UnReadCount,//未读消息数量
                           last_update_time=p.UpdateTime,
                           last_update_timef=p.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
                           order_count=bllMall.GetCount<WXMallOrderInfo>(string.Format(" OrderUserId=(Select UserId from ZCj_UserInfo where AutoId={0}) ",p.CreateUserAutoId)),
                           user_auto_id=p.CreateUserAutoId,
                           view_count = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" EventUserID in(Select UserId from ZCJ_UserInfo where AutoId={0})", p.CreateUserAutoId))
                       };

            data = data.OrderByDescending(p => p.is_online).ThenByDescending(p => p.un_read_count).ThenByDescending(p => p.last_update_time).ToList();
            var resp = new
            {
                totalcount = data.Count(),
                wait_join_count = data.Count(p => p.is_kefu_join == false && p.is_online == 1),
                list = data

            };
            bll.ContextResponse(context, resp);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetShowName(string name) {

            if (name.Length<5)
            {
                return name;
            }
            return name.Substring(0,5)+"...";
        
        }

    }
}