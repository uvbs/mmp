using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Permission
{
    /// <summary>
    /// 站点所有者权限列表
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuper = new BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            List<PermissionInfo> pmsList = new List<PermissionInfo>();
            List<string> strList = new List<string>();

            //if (CurrentUserInfo.UserID == "jubit")
            //{
            //    pmsList = bllMenuper.GetList<PermissionInfo>(" PermissionKey > '' ");
            //}
            //else {
            string strPsmIds = "0";
            List<long> psmIds = bllMenuper.GetUserAllPmsID(bllPms.WebsiteOwner);
            if (psmIds.Count() > 0) strPsmIds = MyStringHelper.ListToStr(psmIds, "'", ",");
                
            pmsList = bllMenuper.GetList<PermissionInfo>(string.Format(" PermissionKey>'' AND PermissionID in ({0})", strPsmIds));
            //}
            
            //过滤掉已经禁止的权限
            var disPmsList = bllPms.GetMultPermissionRelationList( "'" + bllPms.WebsiteOwner + "'", 9);

            pmsList = pmsList.Where(p => disPmsList.Count(dis => dis.PermissionID == p.PermissionID) == 0).ToList();
            
            if (pmsList != null) strList = pmsList.Select(p => p.PermissionKey).ToList();
            apiResp.msg = "ok";
            apiResp.status = true;
            apiResp.result = strList;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }
    }
}