using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.DistributionOffLine
{
    /// <summary>
    /// SynchronousMember 的摘要说明  无用
    /// </summary>
    public class SynchronousMember : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLActivity bllActivity = new BLLActivity("");
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLUser bllUser = new BLLUser();
        BLLDistributionOffLine bllDistributionOffLine = new BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string activityId = context.Request["activity_id"];
            string orderBy = " InsertDate DESC";
            int totalCount = 0;
            List<ActivityDataInfo> activityList = bllActivity.GetActivityDataInfoList(activityId, orderBy, out totalCount);
            int count = 0;
            foreach (var item in activityList)
            {
                UserInfo userInfo = bllUser.GetUserInfoByOpenId(item.WeixinOpenID);
                if (userInfo == null) continue;
                bool isEdit = false;
                if (string.IsNullOrEmpty(userInfo.TrueName))
                {
                    isEdit = true;
                    userInfo.TrueName = item.Name;
                }
                if (string.IsNullOrEmpty(userInfo.Phone))
                {
                    isEdit = true;
                    userInfo.Phone = item.Phone;
                }
                if (string.IsNullOrEmpty(userInfo.Company))
                {
                    isEdit = true;
                    userInfo.Company = item.K2;
                }
                //if (string.IsNullOrEmpty(userInfo.Email)) userInfo.Email = item.K1;
                if (string.IsNullOrEmpty(userInfo.Postion))
                {
                    isEdit = true;
                    userInfo.Postion = item.K3;
                }
                if (isEdit)
                {
                    if (bllUser.Update(userInfo))
                    {
                        count++;
                    }
                }
                
            }
            apiResp.msg = "同步成功:" + count;
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));



        }
    }
}