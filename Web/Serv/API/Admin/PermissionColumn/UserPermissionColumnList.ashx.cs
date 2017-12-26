using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// UserPermissionColumnList 的摘要说明
    /// </summary>
    public class UserPermissionColumnList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermissionColumn bllPermissionColumn = new BLLPermission.BLLPermissionColumn();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string show_hide = context.Request["show_hide"];
            string max_level = context.Request["max_level"];
            bool showHide = false;
            bool.TryParse(show_hide, out showHide);
            int maxLevel = 3;
            if (string.IsNullOrWhiteSpace(max_level)) max_level = "3";
            int.TryParse(max_level, out maxLevel);

            List<BLLPermission.Model.PermissionColumn> list = new List<BLLPermission.Model.PermissionColumn>();
            if (currentUserInfo.UserType != 1 && currentUserInfo.UserID != bllPermissionColumn.WebsiteOwner)
            {
                list = bllPermissionColumn.GetColumnListByUser(currentUserInfo.UserID, bllPermissionColumn.WebsiteOwner, showHide);
            }
            else
            {
                list = bllPermissionColumn.GetColumnListByWebsiteOwner(bllPermissionColumn.WebsiteOwner, showHide);
            }
            List<PermissionColumnModel> resultList = new List<PermissionColumnModel>();
            foreach (BLLPermission.Model.PermissionColumn item in list.Where(p => p.PermissionColumnPreID == 0))
            {
                PermissionColumnModel model = new PermissionColumnModel();
                model.col_id = item.PermissionColumnID;
                model.col_name = item.PermissionColumnName;
                resultList.Add(GetTree(model, list, item.PermissionColumnID, item.PermissionColumnBaseID, 1, maxLevel));
            }
            apiResp.result = resultList;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllPermissionColumn.ContextResponse(context, apiResp);
        }
        /// <summary>
        /// 返回模型
        /// </summary>
        public class PermissionColumnModel
        {
            /// <summary>
            /// 栏目id
            /// </summary>
            public long col_id { get; set; }
            /// <summary>
            /// 栏目名称
            /// </summary>
            public string col_name { get; set; }
            /// <summary>
            /// 子节点
            /// </summary>
            public List<PermissionColumnModel> children { get; set; }
            /// <summary>
            /// 是否选中
            /// </summary>
            public bool col_checked { get; set; }
        }
        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private PermissionColumnModel GetTree(PermissionColumnModel model, List<BLLPermission.Model.PermissionColumn> list,
            long? parentID = 0, long? baseID = 0, int nLevel = 1, int maxLevel = 3)
        {
            if (model.children == null)
            {
                model.children = new List<PermissionColumnModel>();
            }
            if (nLevel < maxLevel)
            {
                foreach (BLLPermission.Model.PermissionColumn item in list.Where(p => p.PermissionColumnPreID == parentID || (baseID > 0 && p.PermissionColumnPreID == baseID)).OrderBy(p => p.OrderNum))
                {
                    PermissionColumnModel child = new PermissionColumnModel()
                    {
                        col_id = item.PermissionColumnID,
                        col_name = item.PermissionColumnName
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, item.PermissionColumnID, item.PermissionColumnBaseID, nLevel + 1, maxLevel);
                }
            }
            return model;
        }
    }
}