using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CompanyWebsite.ToolBar
{
    /// <summary>
    /// UpdateGroupName 的摘要说明
    /// </summary>
    public class UpdateGroupName : BaseHandlerNeedLoginAdminNoAction
    {
        BLLCompanyWebSite bll = new BLLCompanyWebSite();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string groupName = context.Request["GroupName"];

            List<CompanyWebsite_ToolBar> oList = bll.GetMultListByKey<CompanyWebsite_ToolBar>("AutoID", ids);

            List<CompanyWebsite_ToolBar> addList = new List<CompanyWebsite_ToolBar>();
            List<CompanyWebsite_ToolBar> updateList = new List<CompanyWebsite_ToolBar>();
            BLLTransaction tran = new BLLTransaction();
            bool result = false;
            foreach (CompanyWebsite_ToolBar item in oList)
            {
                if (item.IsSystem == 1)
                {
                    item.IsSystem = 0;
                    item.WebsiteOwner = bll.WebsiteOwner;
                    item.KeyType = groupName;
                    item.BaseID = item.AutoID;
                    result = bll.Add(item, tran);
                }
                else
                {
                    item.KeyType = groupName;
                    result = bll.Update(item, tran);
                }
                if (!result)
                {
                    tran.Rollback();
                    break;
                }
            }
            if (result)
            {
                tran.Commit();
                apiResp.status = true;
                apiResp.msg = "编辑完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "编辑失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }

            bll.ContextResponse(context, apiResp);
        }
    }
}