using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// SignHistoryManage 的摘要说明
    /// </summary>
    public class SignHistoryManage : IHttpHandler, IRequiresSessionState
    {
        static BLLJIMP.BLL bll;
        public void ProcessRequest(HttpContext context)
        {
            bll = new BLLJIMP.BLL("");
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string Action = context.Request["Action"];
            string result = "false";
            switch (Action)
            {

                case "Delete":
                    result = Delete (context);
                    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
                case "getmorelist":
                    result = GetMoreList(context);
                    break;
            }
            context.Response.Write(result);


        }

       private static string GetMoreList(HttpContext context){
           if (context.Request["page"].Equals("1"))
           {
               return "";
               
           }
       System.Text.StringBuilder sb=new System.Text.StringBuilder();
          sb.AppendLine("<div class=\"rel sub-msg-item appmsgItem\" > ");
           sb.AppendLine("<span class=\"thumb\"> ");     
           sb.AppendLine("<a href=\"k\" target=\"_blank\" >");
           sb.AppendLine("<img src=\"/img/hb/hb6.jpg\" style=\"margin-right:10px;\" /> ");
           sb.AppendLine("</a>");    
      sb.AppendLine("</span>");   
        sb.AppendLine("<h4 class=\"msg-t1\">"); 

      sb.AppendLine("<a href=\"k\" target=\"_blank\" style=\"font-size: 2.5em;line-height: 1.5em;\" > ");  
  
        sb.AppendLine("gg</a>");  
        sb.AppendLine("<br />");
        sb.AppendLine("<div  style=\"font-size: 2.0em;line-height: 1.5em;\" >2013年10月10日 14:00</div>");
  
        
          sb.AppendLine("<div  style=\"font-size: 2.0em;line-height: 1.5em;\" >地址:j</div>");


    
  
       sb.AppendLine("</h4> "); 

  
       
   sb.AppendLine("</div>");
           return sb.ToString();

       
       }

        private static string GetAllByAny(HttpContext context)
        {

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];
            var searchCondition = string.Format("UserID='{0}'", Comm.DataLoadTool.GetCurrUserID());

            //if (!string.IsNullOrEmpty(searchtitle))
            //{
            //    searchCondition += " And MsgKeyword like '%" + searchtitle + "%'";
            //}

            List<UserSignHistory> list = bll.GetLit<UserSignHistory>(rows, page, searchCondition, "AddDate DESC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.UserSignHistory>(searchCondition);

            string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, list);

            return jsonResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {

            try
            {
                string ids = context.Request["id"];
                if (bll.Delete(new UserSignHistory(), string.Format("SignID in({0}) And UserID='{1}' ", ids, Comm.DataLoadTool.GetCurrUserID())) > 0)
                {
                     return "true";
                }
                return "false";
                
               

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
          

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