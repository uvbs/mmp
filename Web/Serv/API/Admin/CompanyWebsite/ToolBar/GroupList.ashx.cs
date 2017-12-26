using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CompanyWebsite.ToolBar
{
    /// <summary>
    /// GroupList 的摘要说明
    /// </summary>
    public class GroupList : BaseHandlerNeedLoginNoAction
    {
        BLLCompanyWebSite bll = new BLLCompanyWebSite(); 
        public void ProcessRequest(HttpContext context)
        {
            //读取底部工具栏
            StringBuilder sbWhere = new StringBuilder();
            List<string> groupList = bll.GetKeyTypeList(bll.WebsiteOwner, "nav");
            apiResp.result = groupList;
            apiResp.status = true;
            apiResp.msg = "查询成功";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}