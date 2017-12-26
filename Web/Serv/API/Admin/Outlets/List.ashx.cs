using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Outlets
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();

        public void ProcessRequest(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            string cate_id = context.Request["cate_id"];
            string k1 = context.Request["k1"];
            string k4 = context.Request["k4"];
            string k5 = context.Request["k5"];
            bool show_hide = Convert.ToBoolean(context.Request["show_hide"]);
            string tags = context.Request["tags"];
            if (cate_id == "0") cate_id = "";

            int total = 0;
            List<JuActivityInfo> dataList = bllJuActivity.GetOutletsList(rows,page,cate_id,tags,keyword,show_hide,
                "JuActivityID,ActivityName,ActivityAddress,CategoryId,ThumbnailsPath,Sort,K1,K4,Province,City,District", out total,
                null, null, null, null, bllJuActivity.WebsiteOwner, k1, k4,k5,"");

            var rData = from p in dataList
                        select new  
                        {
                            id = p.JuActivityID,
                            title = p.ActivityName,
                            cate_name = p.CategoryName,
                            address = p.ActivityAddress,
                            img = p.ThumbnailsPath,
                            k1 = p.K1,
                            k2=p.K2,
                            k3=p.K3,
                            k4 = p.K4,
                            province=p.Province,
                            city=p.City,
                            district=p.District,
                            //server_time = p.ServerTimeMsg,
                            //server_msg = p.ServicesMsg,
                            //tags = p.Tags,
                            distance = p.Distance
                        };
            apiResp.result = new {
                totalcount = total,
                list = rData
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllJuActivity.ContextResponse(context, apiResp);
        }
    }
}