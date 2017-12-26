using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// 角色列表接口
    /// </summary>
    public class RoleList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["searchReq"];

            List<BLLPermission.Model.PermissionGroupInfo> list = bllPms.GetGroupList(pageSize, pageIndex,keyWord,bllPms.WebsiteOwner,null,2);
            list.AddRange(bllPms.GetGroupList(pageSize, pageIndex, keyWord, "common", null, 3));
            list.AddRange(bllPms.GetGroupList(pageSize, pageIndex, keyWord, "common", null, 4));
            List<BLLPermission.Model.PermissionGroupInfo> dataList = new List<BLLPermission.Model.PermissionGroupInfo>();

            if (currentUserInfo.UserType != 1 && currentUserInfo.UserID != bllPms.WebsiteOwner)
            {
                List<BLLPermission.Model.PermissionGroupInfo> groupList = bllPms.GetUserGroupList(currentUserInfo.UserID, bllPms.WebsiteOwner, 2);
                if (groupList.Count > 0)
                {
                    foreach (BLLPermission.Model.PermissionGroupInfo item in groupList)
                    {
                        if (dataList.FirstOrDefault(p => p.GroupID == item.GroupID) != null) continue;
                        bllPms.GetDataList(ref dataList, list, item.GroupID, 1, 9);
                    }
                }
            }
            else
            {
                bllPms.GetDataList(ref dataList, list, 0, 1, 9);
            }
            int total = dataList.Count;
            if (dataList.Count > pageSize)
            {
                dataList = dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            var data = new
            {
                total = total,
                rows = dataList//列表
            };
            bllPms.ContextResponse(context,data);
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