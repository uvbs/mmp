using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.LiveChat.Detail
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLLiveChat bll = new BLLLiveChat();
        public void ProcessRequest(HttpContext context)
        {
            string roomId = context.Request["room_id"];
            var sourceData = bll.GetLiveChatDetailList(roomId);
            var data = from p in sourceData
                       select new
                       {
                           message = p.Message,
                           message_type = p.MessageType,
                           send_user_type=p.UserType,
                           send_user_name=p.UserType=="0"?bllUser.GetUserDispalyName(bllUser.GetUserInfoByAutoID(int.Parse(p.UserAutoId))):"",
                           send_user_head_img = p.UserType == "0" ? bllUser.GetUserDispalyAvatar(bllUser.GetUserInfoByAutoID(int.Parse(p.UserAutoId))) : "/img/icons/kefu.png"
                           //time = p.InsertDate.ToString("yyyy-MM-dd HH:mm"),
                       };

            var resp = new { 
            totalcount=data.Count(),
            list=data
            
            };
            bll.ContextResponse(context, resp);

        }

        ///// <summary>
        ///// 消息模型
        ///// </summary>
        //public class Message
        //{
        //    /// <summary>
        //    /// 消息主体
        //    /// </summary>
        //    public string message { get; set; }
        //    /// <summary>
        //    /// 消息类型
        //    /// system 系统提示
        //    /// text   文本
        //    /// </summary>
        //    public string message_type { get; set; }
        //    /// <summary>
        //    /// 发送用户类型
        //    /// -1系统
        //    /// 0 用户
        //    /// 1 客服
        //    /// </summary>
        //    public string send_user_type { get; set; }
        //    /// <summary>
        //    /// 发送用户显示名称
        //    /// </summary>
        //    public string send_user_name { get; set; }
        //    /// <summary>
        //    /// 发送用户显示头像
        //    /// </summary>
        //    public string send_user_head_img { get; set; }



        //}


    }
}