using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.WXTempmsg
{
    /// <summary>
    /// 微信模板列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int totalCount = 0;
            List<KeyVauleDataInfo> dataList = bllKeyValueData.GetKeyVauleDataInfoList(pageSize, pageIndex, EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsg), null, bllKeyValueData.WebsiteOwner, out totalCount);

            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalcount = totalCount,
                list = (from p in dataList
                        select new { 
                            id = p.AutoId,
                            data_key = p.DataKey,
                            data_value = p.DataValue
                        })
            };
            bllKeyValueData.ContextResponse(context, resp);
        }
    }
}