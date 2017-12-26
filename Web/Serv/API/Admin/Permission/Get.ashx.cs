using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    /// 获取当前用户所有的权限列表
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        BLLPermission.BLLMenuPermission bllMenuper = new BLLPermission.BLLMenuPermission("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (currentUserInfo.UserType == 7)
            {
                apiResp.msg = "查询完成";
                apiResp.status = true;
                apiResp.result = new string[] { "PMS_ONLYUPDATEPRODUCTSTOCK" };
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            string strPsmIds = "0";
            List<long> psmIds = new List<long>();
            List<PermissionInfo> pmsList = new List<PermissionInfo>();
            if (currentUserInfo.UserType != 1)
            {
                //bllUser.ToLog("PermissionInfo Get UserType != 1 ", @"D:\songhedev.txt");
                psmIds = bllMenuper.GetUserAllPmsID(currentUserInfo.UserID);
                //bllUser.ToLog("psmIds:" + JsonConvert.SerializeObject(psmIds), @"D:\songhedev.txt");
                if (psmIds.Count() > 0) strPsmIds = MyStringHelper.ListToStr(psmIds, "'", ",");
                pmsList = bllMenuper.GetList<PermissionInfo>(string.Format(" PermissionKey>'' AND PermissionID in ({0})", strPsmIds));
            }
            else
            {
                pmsList = bllMenuper.GetList<PermissionInfo>(string.Format(" PermissionKey>'' ", strPsmIds));

            }
            List<string> strList = new List<string>();
            if (pmsList.Count > 0) strList = pmsList.Select(p => p.PermissionKey).Distinct().ToList();
            apiResp.msg = "查询完成";
            apiResp.status = true;
            apiResp.result = strList;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}