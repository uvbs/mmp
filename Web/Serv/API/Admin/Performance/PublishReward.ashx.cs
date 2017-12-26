using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// PublishReward 的摘要说明
    /// </summary>
    public class PublishReward : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            int yearMonth = Convert.ToInt32(context.Request["yearmonth"]);
            string up_member = context.Request["up_member"];
            string member = context.Request["member"];
            string websiteOwner = bll.WebsiteOwner;
            string parentIds = "";
            string userIds = "";
            #region 查出userIds
            if (!string.IsNullOrWhiteSpace(member))
            {
                userIds = bllUser.GetSpreadUserIds(member, websiteOwner);
            }
            if (!string.IsNullOrWhiteSpace(up_member))
            {
                parentIds = bllUser.GetSpreadUserIds(up_member, websiteOwner);
            }
            #endregion 查出userIds
            string sqlWhere = bll.GetPerformanceParamString(userIds,parentIds, websiteOwner, yearMonth);

            int rcount = bll.Update(new TeamPerformance(), "Status=1", string.Format("{0} And {1}", sqlWhere, " IsNull([Status],0) =0"));
            if (rcount > 0)
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
                apiResp.msg = "发布完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.status = false;
                apiResp.msg = "发布失败";
            }
            bll.ContextResponse(context, apiResp);
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