using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// GameApi 的摘要说明
    /// </summary>
    public class GameApi : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMessage resp = new ResponseMessage();
            try
            {
               
                if (string.IsNullOrEmpty(context.Request["activityid"]))
                {
                    resp.Status = -1;
                    resp.Message = "活动ID不能为空";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;

                }
                if (string.IsNullOrEmpty(context.Request["sortfield"]))
                {
                    resp.Status = -1;
                    resp.Message = "排序字段不能为空";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;

                }
                if (string.IsNullOrEmpty(context.Request["count"]))
                {
                    resp.Status = -1;
                    resp.Message = "获取数量不能为空";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;

                }
                BLLJIMP.BLLActivity bll = new BLLJIMP.BLLActivity("");

                int activityid;
                string sortfield = context.Request["sortfield"];
                int count;

                //检查有效性
                if (!int.TryParse(context.Request["activityid"], out activityid))
                {
                    resp.Status = -1;
                    resp.Message = "活动ID错误";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (!int.TryParse(context.Request["count"], out count))
                {
                    resp.Status = -1;
                    resp.Message = "获取数量错误";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;
                }



                //筛选活动id范围
                
                if (bll.GetCount<GameActivityQueryLimit>(string.Format("ActivityID={0}", activityid))>0)
                {
                    List<ActivityDataInfo> dataList = bll.GetLit<ActivityDataInfo>(count, 1, string.Format("ActivityID='{0}' And IsDelete !=1", activityid), string.Format(" CAST({0} as INTEGER) DESC", sortfield));

                    for (int i = 0; i < dataList.Count; i++)
                    {
                        dataList[i].WebsiteOwner = null;
                    }

                    context.Response.Write(Common.JSONHelper.ObjectToJson(dataList));
                    return;
                }
                else
                {
                    resp.Status = -1;
                    resp.Message = "不允许查询的活动";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;


                }

            }
            catch (Exception ex)
            {

                resp.Status = -1;
                resp.Message = ex.Message;
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }


            



        }


        public class ResponseMessage{
        
            public int Status{get;set;}
            public string Message{get;set;}
        
        
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