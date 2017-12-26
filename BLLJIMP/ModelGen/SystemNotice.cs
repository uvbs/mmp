using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class SystemNotice : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 编号
       /// </summary>
       public int AutoID { get; set; }

       /// <summary>
       /// 批次
       /// </summary>
       public string SerialNum { get; set; }


       /// <summary>
       /// 标题
       /// </summary>
       public string Title { get; set; }
       /// <summary>
       /// 内容
       /// </summary>
       public string Ncontent { get; set; }

       /// <summary>
       /// 接收用户
       /// </summary>
       public string UserId { get; set; }

       /// <summary>
       /// 消息类型
       /// </summary>
       public int NoticeType { get; set; }
       /// <summary>
       /// 发送时间
       /// </summary>
       public DateTime InsertTime { get; set; }
       public string InsertTimeString { get { return InsertTime.ToString(); } }

        /// <summary>
        /// 阅读时间,为空或null表示未读
        /// </summary>
       public DateTime ?Readtime { get; set; }
       public string ReadtimeString { get { return Readtime.ToString(); } }

        /// <summary>
        /// 站点
        /// </summary>
       public string WebsiteOwner { get; set; }

       /// <summary>
       /// 重定向URL
       /// </summary>
       public string RedirectUrl { get; set; }

       /// <summary>
       /// 发送类型 0:全部，1：分组，2： 个人
       /// </summary>
       public int SendType {get; set;}

       /// <summary>
       /// 接收列表 
       /// </summary>
       public string Receivers { get; set; }

       /// <summary>
       /// SendType 名称 
       /// </summary>
       public string SendTypeString
       {
           get
           {
               switch (SendType)
               {
                   case 0:
                       return "所有用户";
                   case 1:
                       return "标签列表";
                   case 2:
                       return "个人列表";
                   default:
                       return "未定义";
               }
           }
       }

       /// <summary>
       /// MessageType 名称
       /// </summary>
       public string NoticeTypeString
       {
           get
           {
               switch (NoticeType)
               {
                   case 0:
                       return "系统消息";
                   case 11:
                       return "话题提醒";
                   case 21:
                       return "问卷提醒";
                   default:
                       return "未定义的系统通知类型";
               }
           }
       }
    }
}
