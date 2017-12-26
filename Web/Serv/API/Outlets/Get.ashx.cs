using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Outlets
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string supplierId=context.Request["supplier_id"];
            JuActivityInfo nInfo=new JuActivityInfo();
            if (!string.IsNullOrEmpty(id))
            {
                nInfo = bllJuActivity.GetByKey<JuActivityInfo>("JuActivityID", id, true);
            }
            if (!string.IsNullOrEmpty(supplierId))
            {
                nInfo = bllJuActivity.Get<JuActivityInfo>(string.Format("WebsiteOwner='{0}' And ArticleType='Outlets' And K5='{1}'",bllJuActivity.WebsiteOwner,supplierId));
            }
          
            if (nInfo == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "网点未找到";
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = new {
                id = nInfo.JuActivityID,
                title = nInfo.ActivityName,
                province = nInfo.Province,
                city = nInfo.City,
                district = nInfo.District,
                address = nInfo.ActivityAddress,
                img = nInfo.ThumbnailsPath,
                server_time = nInfo.ServerTimeMsg,
                server_msg = nInfo.ServicesMsg,
                longitude = nInfo.UserLongitude,
                latitude = nInfo.UserLatitude,
                k1 = nInfo.K1,
                k4 = nInfo.K4,
                tags = nInfo.Tags,
                cate_name = nInfo.CategoryName
            };
            bllJuActivity.ContextResponse(context, apiResp);
        }
    }
}