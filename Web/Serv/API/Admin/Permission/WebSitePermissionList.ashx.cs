using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    /// 站点权限列表
    /// </summary>
    public class WebSitePermissionList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            List<PermissionInfo> perList = bllMenuPermission.BaseCacheGetPermissionList();
            int CateCount = 0;
            List<BLLJIMP.Model.ArticleCategory> cateList = bllArticleCategory.GetCateList(out CateCount, "Permission", null, "Common");
            List<dynamic> result = new List<dynamic>();
            foreach (BLLJIMP.Model.ArticleCategory tCate in cateList)
            {
                List<PermissionInfo> tempPerList = perList.Where(p => p.PermissionCateId == tCate.AutoID).OrderBy(p => p.PermissionName).ToList();
                if (tempPerList.Count == 0) continue;
                result.Add(new
                {
                    cate_id = tCate.AutoID,
                    cate_name = tCate.CategoryName,
                    permission_list = from p in tempPerList
                                      select new
                                      {
                                          permission_id = p.PermissionID,
                                          permission_name = p.PermissionName,
                                          permission_checked = false
                                      }
                });
            }
            List<int> cateId_list = cateList.Select(p => p.AutoID).ToList();
            List<PermissionInfo> tempPerList1 = perList.Where(p => !cateId_list.Contains(p.PermissionCateId)).OrderBy(p => p.PermissionName).ToList();
            if (tempPerList1.Count > 0)
            {
                result.Add(new
                {
                    cate_id = 0,
                    cate_name = "未分类",
                    permission_list = from p in tempPerList1
                                      select new
                                      {
                                          permission_id = p.PermissionID,
                                          permission_name = p.PermissionName,
                                          permission_checked = false
                                      }
                });
            }

            apiResp.result = result;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMenuPermission.ContextResponse(context, apiResp);
        }

    }
}