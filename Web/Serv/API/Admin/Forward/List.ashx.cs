using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Forward
{
    /// <summary>
    /// List 的摘要说明 微问卷列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 转发
        /// </summary>
        BLLJIMP.BLLActivityForwardInfo bllForward = new BLLJIMP.BLLActivityForwardInfo();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 50;
            string keyWord=context.Request["keyword"];
            string type=context.Request["type"];
            string sort = context.Request["sort"];
            string order = context.Request["order"];
            int total=0;

            List<ActivityForwardInfo> list = bllForward.GetActivityForwardList(pageSize, pageIndex, keyWord, type, order,sort, out total);

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in list)
            {
                Questionnaire model = bllForward.Get<Questionnaire>(string.Format(" WebsiteOwner='{0}' AND QuestionnaireID={1}",bllForward.WebsiteOwner,item.ActivityId));
                int forwarNum = bllForward.GetCount<BLLJIMP.Model.MonitorLinkInfo>("  MonitorPlanID=" + model.QuestionnaireID);
                returnList.Add(new 
                {
                    activity_id = item.ActivityId,
                    activity_name = item.ActivityName,
                    insert_date = item.InsertDate,
                    pv = item.PV,
                    ip = model.IP,
                    uv = item.UV,
                    img_url = item.ThumbnailsPath,
                    userid = item.UserId,
                    forwarnum = forwarNum,
                   mid=item.ActivityId
                });
            }

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new {
                rows = returnList,
                total = total
            }));

        }

        
    }
}