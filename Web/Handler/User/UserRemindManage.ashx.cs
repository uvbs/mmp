using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Handler.User
{
    /// <summary>
    /// UserRemindManage 的摘要说明
    /// </summary>
    public class UserRemindManage : IHttpHandler, IRequiresSessionState
    {


        static BLLJIMP.BLL bll;

        public void ProcessRequest(HttpContext context)
        {
            try
            {


                bll = new BLLJIMP.BLL("");
                context.Response.ContentType = "text/html";
                context.Response.Expires = 0;
                string Action = context.Request["Action"];
                string result = "false";
                switch (Action)
                {
                    case "Add":
                        result = Add(context);
                        break;
                    case "Edit":
                        result = Edit(context);
                        break;
                    case "Delete":
                        result = Delete(context);
                        break;
                    case "Query":
                        result = GetAllByAny(context);
                        break;
                    case "GetRemindList":
                        result = GetRemindList();
                        break;
                    case "UpdateRemindState":
                        result = UpdateRemindState(context);
                        break;
                    case "GetSingleRemindInfo":
                        result = GetSingleRemindInfo(context);
                        break;

                    case "GetRemindByTime":
                        result = GetRemindByTime();
                        break;
                    case "BatChangState":
                        result = BatChangState(context);
                        break;

                    case "GetWeiBoEventDetailsInfoRemind"://微博事件点击提醒
                        result = GetWeiBoEventDetailsInfoRemind(context);
                        break;
                    case "GetEmailEventDetailsInfoRemind":// 邮件打开提醒
                        result = GetEmailEventDetailsInfoRemind(context);
                        break;





                }
                context.Response.Write(result);
            }
            catch (Exception ex)
            {

                context.Response.Write(ex.Message);
                return;
            }

        }

        /// <summary>
        /// 获取用户分组
        /// </summary>
        private static string GetMemberGroupList()
        {

            var UserID = Comm.DataLoadTool.GetCurrUserID();
            string result = " <select id=\"ddlMemberGroup\" style=\"width:180px;\">";
            result += " <option value=\"\">无</option>";
            var GroupList = bll.GetList<MemberGroupInfo>(string.Format("UserID='{0}' and GroupType=1", UserID));
            foreach (MemberGroupInfo item in GroupList)
            {
                result += string.Format("<option value=\"{0}\">{1}</option>", item.GroupID, item.GroupName);
            }
            result += "</select>";
            return result.ToString();

        }

        /// <summary>
        /// 添加
        /// </summary>
        private static string Add(HttpContext context)
        {
            var model =GetModel(context);
            if (model.RemindTime<=DateTime.Now)
            {
                return "提醒时间须晚于当前时间";
            }
            model.RemindID =Convert.ToInt32(bll.GetGUID(BLLJIMP.TransacType.RemindAdd));
            model.UserID = Comm.DataLoadTool.GetCurrUserID();
            model.AddDate = DateTime.Now ;
            model.IsEnable = 1;
            model.IsRemind = 0;
            return bll.Add(model).ToString().ToLower();


        }


        /// <summary>
        /// 修改
        /// </summary>
        public static string Edit(HttpContext context)
        {
            var model =GetModel(context);
            if (model.IsEnable==1)
            {
             if (model.RemindTime<=DateTime.Now)
             {
                return "提醒时间须晚于当前时间";
              }

            }
            model.RemindID=Convert.ToInt32(context.Request["RemindID"]);
            model.IsRemind = 0;
            var remindinfo = bll.Get<UserRemind>(string.Format("RemindID='{0}' And UserID='{1}'", model.RemindID, Comm.DataLoadTool.GetCurrUserID()));
            if (remindinfo==null)
            {
                return "false";
            }
           return  bll.Update(model).ToString().ToLower();
    
        }


        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {
            string ids = context.Request["id"];
            var count = 0;
            count= bll.Delete(new UserRemind(), string.Format("RemindID in({0}) And UserID='{1}'", ids, Comm.DataLoadTool.GetCurrUserID()));
            if (count==ids.Split(',').Length)
            {
                return "true";
            }
            return "false";


        }

        /// <summary>
        /// 批量启用停用提醒
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string BatChangState(HttpContext context) {
            string ids = context.Request["Ids"];
            var state = context.Request["State"];
            var count = 0;
            count = bll.Update(new UserRemind(), string.Format("IsEnable='{0}' ",state), string.Format("RemindID in({0}) And UserID='{1}'", ids, Comm.DataLoadTool.GetCurrUserID()));
            if (count == ids.Split(',').Length)
            {
                return "true";
            }
            return "false";

        
        }





        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private static string GetAllByAny(HttpContext context)
        {

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            var searchCondition = string.Format("UserID='{0}'", Comm.DataLoadTool.GetCurrUserID());
            var searchtitle = context.Request["SearchTitle"];
            if (!string.IsNullOrEmpty(searchtitle))
            {
                searchCondition += " And Title like '%" + searchtitle + "%'";
            }
           
            List<UserRemind> list = bll.GetLit<UserRemind>(rows, page, searchCondition,"RemindTime DESC");
            int totalCount = bll.GetCount<UserRemind>(searchCondition);
            string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, list);
            return jsonResult;


        }

        /// <summary>
        /// 获取提醒列表
        /// </summary>
        /// <returns></returns>
        private static string GetRemindList() {
            var searchCondition = string.Format("UserID='{0}' And IsEnable=1 Order By RemindTime DESC", Comm.DataLoadTool.GetCurrUserID());
            List<UserRemind> list = bll.GetList<UserRemind>(searchCondition);
            StringBuilder strhtml = new StringBuilder();
            strhtml.Append("<div id=\"div_remind\">");
            
            foreach (var item in list)
            {
                strhtml.Append("<div group=\"remindgroup\"  style=\"float:left;margin-top:0px;margin-left:10px;width:190px;height:60px;overflow:hidden;\">");
                 strhtml.Append("主题:\t\t<span title="+ "主题:"+item.Title + ">" + SubString(item.Title, 5) + "</span>\t\t<a title=\"查看详细提醒\" style=\"color:blue;\" remindid=" + item.RemindID + " href=\"#\">详细</a>\t\t<span remindid=" + item.RemindID + " style=\"color:blue;cursor:hand;text-align:right\" title=\"不再提示\">不再提示</span></br>");
                 strhtml.Append("备注:\t\t<span title=" + "备注:"+item.Content + ">" + SubString(item.Content, 12) + "</span></br>");

                strhtml.Append("时间:\t\t" + item.RemindTime + "</br>");
                strhtml.Append("<hr>");
                 strhtml.Append("</div>");
            }

            strhtml.Append("</div>");
            return strhtml.ToString();

        }

        private static string GetSingleRemindInfo(HttpContext content) {
            var searchCondition = string.Format("UserID='{0}' And IsEnable=1 And RemindID='{1}'", Comm.DataLoadTool.GetCurrUserID(),content.Request["Id"]);
            var  model = bll.Get<UserRemind>(searchCondition);
            List<UserRemind> data=new List<UserRemind>();
            data.Add(model);
            return string.Format("{0}",ZentCloud.Common.JSONHelper.ListToJson<UserRemind>(data, ","));
        
        
        }
        /// <summary>
        /// 修改提醒状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string UpdateRemindState(HttpContext context)
        {
            var remindinfo = bll.Get<UserRemind>(string.Format("RemindID='{0}' And UserID='{1}'", context.Request["RemindID"], Comm.DataLoadTool.GetCurrUserID()));
            remindinfo.IsEnable =Convert.ToInt32(context.Request["IsEnable"]) ;
            var count= bll.Update(remindinfo, string.Format("IsEnable='{0}'", remindinfo.IsEnable), string.Format("RemindID='{0}'",remindinfo.RemindID));
            if (count>0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }


        /// <summary>
        /// 获取提醒信息
        /// </summary>
        /// <returns></returns>
        private static string GetRemindByTime()
        {
           
           
            var condition = string.Format("UserID='{0}' And IsEnable=1 And IsRemind=0 And DATEDIFF(MINUTE,RemindTime,GETDATE()) <=4 ", Comm.DataLoadTool.GetCurrUserID());
            var remindinfolist = bll.GetList<UserRemind>(condition);
            string strhtml = "";
            if (remindinfolist != null)
            {

                for (int i = 0; i < remindinfolist.Count; i++)
                {
                    strhtml += "主题:\t\t" + remindinfolist[i].Title + "</br>";
                    strhtml += "内容:\t\t" + remindinfolist[i].Content + "</br>";
                    strhtml += "时间:\t\t" + remindinfolist[i].RemindTime + "</br>";
                    remindinfolist[i].IsRemind = 1;
                    bll.Update(remindinfolist[i]);
                    if (i<remindinfolist.Count-1)
                    {
                        strhtml += "<hr/>";
                    }
                }
                return strhtml;

            }

            return "";


        }

        /// <summary>
        /// 获取传入的实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static UserRemind GetModel(HttpContext context)
        {
            return ZentCloud.Common.JSONHelper.JsonToModel<BLLJIMP.Model.UserRemind>(context.Request["JsonData"]);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <returns></returns>
        private static string SubString(string str,int length) {

            try
            {
             if (str.Length>length)
	        {
		        return str.Substring(0,length);
	        }


            }
            catch (Exception)
            {

                return str;
            }
             return str;
        
        }

        ///// <summary>
        ///// 微博事件收集提醒
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private static string GetWeiBoEventDetailsInfoRemind(HttpContext context) {
        //    var userInfo = Comm.DataLoadTool.GetCurrUserModel();
        //    var UserActivityList = bll.GetList<ActivityInfo>(string.Format("UserID='{0}'", userInfo.UserID));
        //    if (UserActivityList.Count>0)//用户有活动
        //    {
        //        string strUserActivityList = string.Empty;
        //        foreach (var item in UserActivityList)
        //        {
        //            strUserActivityList += item.ActivityID + ",";
        //        }
        //        strUserActivityList=strUserActivityList.TrimEnd(',');
        //        var WeiBoEventDetailsInfoList = bll.GetList<WeiBoEventDetailsInfo>(string.Format("ActivityID in ({0}) And DATEDIFF(MINUTE,EventDate,GETDATE()) <=3", strUserActivityList));
        //        if (WeiBoEventDetailsInfoList!=null)
        //        {
        //            string strhtml = "";
        //            //foreach (var item in WeiBoEventDetailsInfoList)
        //            //{
        //            //    var ActivityInfo=bll.Get<ActivityInfo>(string.Format("ActivityID={0}",item.ActivityID));
        //            //    strhtml += string.Format(@"活动: {0} 有新的点击!<a taget=""_blank"" title=""点击查看"" href=""/Weibo/EventDetails.aspx"">点击查看</a></br>微博UID:{1}</br>", ActivityInfo.ActivityName, item.ViewID);

        //            //    strhtml +=string.Format("时间:\t\t{0}</br>",item.EventDate) ;
        //            //    strhtml += "<hr/>";
        //            //}
        //            for (int i = 0; i < WeiBoEventDetailsInfoList.Count; i++)
        //            {
        //                var ActivityInfo = bll.Get<ActivityInfo>(string.Format("ActivityID={0}", WeiBoEventDetailsInfoList[i].ActivityID));
        //                strhtml += string.Format(@"活动: {0} 有新的点击!<a taget=""_blank"" title=""点击查看"" href=""/Weibo/EventDetails.aspx"">点击查看</a></br>微博UID:{1}</br>", ActivityInfo.ActivityName, WeiBoEventDetailsInfoList[i].ViewID);

        //                strhtml += string.Format("时间:\t\t{0}</br>", WeiBoEventDetailsInfoList[i].EventDate);
        //                if (i<WeiBoEventDetailsInfoList.Count-1)
        //                {
        //                     strhtml += "<hr/>";
        //                }
                       
                        
        //            }
        //            return strhtml;


        //        }



        //    }
        //    else//用户无活动
        //    {
               

        //    }
        //    return "";
        
        //}

        /// <summary>
        /// 微博事件收集提醒
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetWeiBoEventDetailsInfoRemind(HttpContext context)
        {
            var userInfo = Comm.DataLoadTool.GetCurrUserModel();
            var UserActivityList = bll.GetList<ActivityInfo>(string.Format("UserID='{0}'", userInfo.UserID));
            if (UserActivityList.Count > 0)//用户有活动
            {
                string strUserActivityList = string.Empty;
                foreach (var item in UserActivityList)
                {
                    strUserActivityList += item.ActivityID + ",";
                }
                strUserActivityList = strUserActivityList.TrimEnd(',');
                var WeiBoEventDetailsInfoList = bll.GetList<WeiBoEventDetailsInfo>(string.Format("ActivityID in ({0}) And DATEDIFF(MINUTE,EventDate,GETDATE()) <=3", strUserActivityList));
                if (WeiBoEventDetailsInfoList != null)
                {
                    StringBuilder strhtml =new StringBuilder("");
                    for (int i = 0; i < WeiBoEventDetailsInfoList.Count; i++)
                    {
                        var ActivityInfo = bll.Get<ActivityInfo>(string.Format("ActivityID={0}", WeiBoEventDetailsInfoList[i].ActivityID));
                        strhtml.Append(string.Format(@"活动: {0} 有新的点击!<a taget=""_blank"" title=""点击查看"" href=""/Weibo/EventDetails.aspx"">点击查看</a></br>微博UID:{1}</br>", ActivityInfo.ActivityName, WeiBoEventDetailsInfoList[i].ViewID));

                         strhtml.Append(string.Format("时间:\t\t{0}</br>", WeiBoEventDetailsInfoList[i].EventDate));
                        if (i < WeiBoEventDetailsInfoList.Count - 1)
                        {
                           strhtml.Append("<hr/>");
                        }


                    }
                    return strhtml.ToString();


                }



            }
            else//用户无活动
            {


            }
            return "";

        }

        ///// <summary>
        ///// 邮件打开事件提醒
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private static string GetEmailEventDetailsInfoRemind(HttpContext context)
        //{
        //    var userInfo = Comm.DataLoadTool.GetCurrUserModel();
        //    var UserEmailList = bll.GetList<EmailInfo>(string.Format("UserID='{0}'", userInfo.UserID));//用户所有邮件
        //    if (UserEmailList.Count > 0)//用户有邮件
        //    {

        //        string strIds = Common.StringHelper.ListToStr<string>(UserEmailList.Select(p => p.EmailID).ToList(), "'", ",");

        //        var EmailEventDetailsInfoList = bll.GetList<EmailEventDetailsInfo>(string.Format("EmailID in ({0}) And DATEDIFF(MINUTE,EventDate,GETDATE()) <=3 And EventType=1", strIds));
        //        if (EmailEventDetailsInfoList != null)
        //        {
        //            string strhtml = "";
        //            foreach (var item in EmailEventDetailsInfoList)
        //            {
        //                var EmailInfo = bll.Get<EmailInfo>(string.Format("EmailID='{0}'", item.EmailID));
        //                //if (item.EventType.Equals(0))
        //                //{
        //                //    strhtml += string.Format(@"邮件: {0} 有新的打开!<a title=""点击查看"" taget=""_blank"" href=""/EdM/Report/EmailEventDetails.aspx?EmailID={1}&EventType=0"">点击查看</a></br>邮箱:{2}</br>", EmailInfo.EmailName, EmailInfo.EmailID, item.EventEmail);

        //                //}
        //                //if(item.EventType.Equals(1))
        //                //{
        //                    strhtml += string.Format(@"邮件: {0} 有新的点击!<a title=""点击查看"" taget=""_blank"" href=""/EdM/Report/EmailEventDetails.aspx?EmailID={1}&EventType=1"">点击查看</a></br>邮箱:{2}</br>", EmailInfo.EmailName, EmailInfo.EmailID, item.EventEmail);

        //                //}
                      
        //                strhtml += string.Format("时间:\t\t{0}</br>", item.EventDate);
        //                strhtml += "<hr/>";
        //            }
        //            return strhtml;


        //        }



        //    }
        //    else
        //    {


        //    }
        //    return "";

        //}

        /// <summary>
        /// 邮件打开事件提醒
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetEmailEventDetailsInfoRemind(HttpContext context)
        {
            var userInfo = Comm.DataLoadTool.GetCurrUserModel();
            var UserEmailList = bll.GetList<EmailInfo>(string.Format("UserID='{0}'", userInfo.UserID));//用户所有邮件
            if (UserEmailList.Count > 0)//用户有邮件
            {

                string strIds = Common.StringHelper.ListToStr<string>(UserEmailList.Select(p => p.EmailID).ToList(), "'", ",");

                var EmailEventDetailsInfoList = bll.GetList<EmailEventDetailsInfo>(string.Format("EmailID in ({0}) And DATEDIFF(MINUTE,EventDate,GETDATE()) <=3 And EventType=1", strIds));
                if (EmailEventDetailsInfoList != null)
                {
                    StringBuilder strhtml =new StringBuilder("");
                    for (int i = 0; i < EmailEventDetailsInfoList.Count; i++)
                    {
                        var EmailInfo = bll.Get<EmailInfo>(string.Format("EmailID='{0}'", EmailEventDetailsInfoList[i].EmailID));

                        strhtml.Append(string.Format(@"邮件: {0} 有新的点击!<a title=""点击查看"" taget=""_blank"" href=""/EdM/Report/EmailEventDetails.aspx?EmailID={1}&EventType=1"">点击查看</a></br>邮箱:{2}</br>", EmailInfo.EmailName, EmailInfo.EmailID, EmailEventDetailsInfoList[i].EventEmail));

                        strhtml.Append(string.Format("时间:\t\t{0}</br>", EmailEventDetailsInfoList[i].EventDate));
                        if (i<EmailEventDetailsInfoList.Count-1)
                        {
                             strhtml.Append("<hr/>");
                        }
                       
                        
                    }
                    return strhtml.ToString() ;


                }



            }
            else
            {


            }
            return "";

        }




        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}