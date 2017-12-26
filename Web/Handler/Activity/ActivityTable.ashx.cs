using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Handler.Activity
{
    /// <summary>
    /// 报名字段管理 的摘要说明
    /// </summary>
    public class ActivityTable : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLActivity bllActivity=new BLLActivity("");
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 报名字段
        /// </summary>
        ActivityFieldMappingInfo activityFieldMap;
        /// <summary>
        /// 当前活动ID
        /// </summary>
        string activityID = "0";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentUserInfo = bllUser.GetCurrentUserInfo();
            string action = context.Request["Action"];
            //获取数据
            if (!string.IsNullOrWhiteSpace(context.Request["ActivityID"]))
            {
                this.activityID = context.Request["ActivityID"];
            }

            ActivityInfo activityInfo =bllActivity.Get<ActivityInfo>(string.Format(" ActivityID = '{0}'", activityID));
            if (activityInfo == null)
            {
                //context.Response.Redirect("/Home/Nopms.aspx");
                context.Response.End();
            }

            if (context.Request["JsonData"] != null)
                this.activityFieldMap = GetReqActivityInfo(context);


            string result = "true";
            try
            {
                switch (action)
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
                    case "EditDistinctKeys":
                        result = EditDistinctKeys(context);//设置自定义重复字段
                        break;
                    case "GetDistinctKeys":
                        result = GetDistinctKeys(context);//获取自定义重复字段
                        break;

                    case "SetFieldSort":
                        result = SetFieldSort(context);//设置排序
                        break;


                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            context.Response.Write(result);
        }

        private string GetAllByAny(HttpContext context)
        {
            List<ActivityFieldMappingInfo> list = this.bllActivity.GetActivityFieldMappingList(this.activityID);
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = list.Count,
        rows = list
    });
        }

        private string Delete(HttpContext context)
        {
            string ids=context.Request["id"];
            string[] idarry =ids.Split(',');
            int count = 0;

            count = bllActivity.Delete(new ActivityFieldMappingInfo(), string.Format("ActivityID='{0}' and ExFieldIndex in({1})", activityID, ids));
           if ((count==idarry.Length)&&count>0)
	       {
               if (bllActivity.Update(new ActivityInfo(),"DistinctKeys=''",string.Format("ActivityID='{0}'",activityID))>0)
               {
                   resp.Status = 1;
                   resp.Msg = "删除成功";

               }
               else
               {
                   resp.Status = 0;
                   resp.Msg = "删除失败";

               }
              

	        }
            else
            {
                resp.Status =0;
                resp.Msg = "删除失败";

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Edit(HttpContext context)
        {
            if (activityFieldMap.FieldType.Equals(1))
            {
                if (bllActivity.GetCount<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' and FieldType=1 And  ExFieldIndex!={1}", activityFieldMap.ActivityID,activityFieldMap.ExFieldIndex)) > 0)
                {
                    resp.Status = 0;
                    resp.Msg="只能设置一个微信推广字段";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (activityFieldMap.InputType != "text")
            {
                if (string.IsNullOrWhiteSpace(activityFieldMap.Options))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入选项,多个选项用逗号分隔";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    activityFieldMap.Options = activityFieldMap.Options.Replace("，", ",");
                }
            }
            if (bllActivity.Update(activityFieldMap))
	        {
		         resp.Status =1;
                 resp.Msg="修改成功";

	        }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            if (activityFieldMap.ExFieldIndex>=61)
            {
                resp.Status =0;
                resp.Msg = "最多能添加60个扩展字段";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (activityFieldMap.FieldType.Equals(1))
            {
                if (bllActivity.GetCount<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' and FieldType=1", activityFieldMap.ActivityID)) > 0)
                {
                   
                    resp.Status = 0;
                    resp.Msg = "只能添加一个微信推广字段";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                activityFieldMap.FieldIsNull =0 ;
            }
            if (activityFieldMap.InputType!="text")
            {
                if (string.IsNullOrWhiteSpace(activityFieldMap.Options))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入选项,多个选项用逗号分隔";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    activityFieldMap.Options = activityFieldMap.Options.Replace("，",",");
                }
            }
            if (bllActivity.Add(activityFieldMap))
            {
                   resp.Status = 1;
                   resp.Msg = "添加成功";

            }
            else
            {
                resp.Status =0;
                resp.Msg = "添加失败";

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 设置不允许重复性字段
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditDistinctKeys(HttpContext context) {
            string activityId = context.Request["ActivityID"];
            string distinctKeys = context.Request["DistinctKeys"];
            if (bllActivity.Update(new ActivityInfo(), string.Format(" DistinctKeys='{0}'", distinctKeys), string.Format("ActivityID={0}", activityId))>0)
            {
                resp.Status=1;
                resp.Msg="设置成功";
                
            }
            else
	        {
                resp.Status=0;
                resp.Msg="设置失败";

	        }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        
        
        }

        /// <summary>
        /// 获取自定义重复字段
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDistinctKeys(HttpContext context) {
            string activityId = context.Request["ActivityID"];
            ActivityInfo activityInfo=new ActivityInfo();
            if (currentUserInfo.UserType.Equals(1))
            {
                activityInfo=bllActivity.Get<ActivityInfo>(string.Format("ActivityID='{0}'",activityId));
            }
            else
	        {
                activityInfo=bllActivity.Get<ActivityInfo>(string.Format("ActivityID='{0}' And UserID='{1}'",activityId,currentUserInfo.UserID));
	        }
            return activityInfo.DistinctKeys == null ? "" : activityInfo.DistinctKeys;
        
        
        }
        /// <summary>
        /// 设置报名表排序字段
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetFieldSort(HttpContext context) {

            string activityId = context.Request["ActivityID"];
            string fieldSort = context.Request["FieldSort"];
            if (bllActivity.SetFieldSort(activityId, fieldSort))
            {
                resp.Status = 1;
                resp.Msg = "操作成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "操作失败";

            }
              return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            
        
        
        }

        private ActivityFieldMappingInfo GetReqActivityInfo(HttpContext context)
        {
            return ZentCloud.Common.JSONHelper.JsonToModel<ActivityFieldMappingInfo>(context.Request["JsonData"]);
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