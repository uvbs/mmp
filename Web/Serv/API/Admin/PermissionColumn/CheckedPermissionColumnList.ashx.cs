using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// CheckedPermissionColumnList 的摘要说明
    /// </summary>
    public class CheckedPermissionColumnList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermissionColumn bllPermissionColumn = new BLLPermission.BLLPermissionColumn();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string website_owner = context.Request["website_owner"];
            string show_hide = context.Request["show_hide"];
            string group_id = context.Request["group_id"];
            string max_level = context.Request["max_level"];
            bool showHide = false;
            bool.TryParse(show_hide, out showHide);
            int maxLevel = 3;
            if (string.IsNullOrWhiteSpace(max_level)) max_level = "3";
            int.TryParse(max_level, out maxLevel);

            List<BLLPermission.Model.PermissionColumn> list = bllPermissionColumn.GetColumnListByWebsiteOwner(website_owner, showHide);
            List<BLLPermission.Model.PermissionRelationInfo> relList = bllPermission.GetPermissionRelationList(group_id, 3);


            List<PermissionColumnModel> resultList = new List<PermissionColumnModel>();
            foreach (BLLPermission.Model.PermissionColumn item in list.Where(p => p.PermissionColumnPreID == 0))
            {
                PermissionColumnModel model = new PermissionColumnModel();
                model.col_id = item.PermissionColumnID;
                model.col_name = item.PermissionColumnName;
                model.col_checked = relList.Exists(pi => pi.PermissionID == item.PermissionColumnID);
                resultList.Add(GetTree(model, list, relList, item.PermissionColumnID, item.PermissionColumnBaseID, 1, maxLevel));
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
        private PermissionColumnModel GetTree(PermissionColumnModel model, List<BLLPermission.Model.PermissionColumn> list, List<BLLPermission.Model.PermissionRelationInfo> relList,
            long? parentID = 0, long? baseID=0, int nLevel = 1, int maxLevel = 3)
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
                        col_name = item.PermissionColumnName,
                        col_checked = relList.Exists(pi => pi.PermissionID == item.PermissionColumnID)
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, relList, item.PermissionColumnID, item.PermissionColumnBaseID, nLevel + 1, maxLevel);
                }
            }
            return model;
        }
    }
}