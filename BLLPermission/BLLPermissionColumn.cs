using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;

namespace ZentCloud.BLLPermission
{
    public class BLLPermissionColumn:BLL
    {
        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="permissionColumnName"></param>
        /// <param name="permissionColumnPreID"></param>
        /// <param name="orderNum"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool AddColumn(string permissionColumnName, long permissionColumnPreID, int orderNum, string websiteOwner,long baseID=0,int hide=0)
        {
            PermissionColumn nPermissionColumn = new PermissionColumn();
            nPermissionColumn.PermissionColumnID = Convert.ToInt64(GetGUID(TransacType.PermissionColumn));
            nPermissionColumn.PermissionColumnName = permissionColumnName;
            nPermissionColumn.PermissionColumnPreID = permissionColumnPreID;
            nPermissionColumn.OrderNum = orderNum;
            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                nPermissionColumn.WebsiteOwner = null;
            }
            else
            {
                nPermissionColumn.WebsiteOwner = websiteOwner;
            }
            nPermissionColumn.PermissionColumnBaseID = baseID;
            nPermissionColumn.IsHide = hide;
            return Add(nPermissionColumn);
        }
        /// <summary>
        /// 编辑栏目
        /// </summary>
        /// <param name="permissionColumnName"></param>
        /// <param name="permissionColumnPreID"></param>
        /// <param name="orderNum"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool EditColumn(long permissionColumnID, string permissionColumnName, long permissionColumnPreID, int orderNum, string websiteOwner)
        {
            PermissionColumn nPermissionColumn = GetByKey<PermissionColumn>("PermissionColumnID", permissionColumnID.ToString());
            if (nPermissionColumn == null) throw new Exception("未找到该栏目");
            nPermissionColumn.PermissionColumnName = permissionColumnName;
            nPermissionColumn.PermissionColumnPreID = permissionColumnPreID;
            nPermissionColumn.OrderNum = orderNum;
            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                nPermissionColumn.WebsiteOwner = null;
            }
            else
            {
                nPermissionColumn.WebsiteOwner = websiteOwner;
            }
            return Update(nPermissionColumn);
        }
        /// <summary>
        /// 获取站点栏目
        /// </summary>
        /// <param name="row"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<PermissionColumn> GetColumnList(string websiteOwner, bool showHide = false)
        {
            return GetList<PermissionColumn>(int.MaxValue, GetColumnParams(websiteOwner, showHide), "OrderNum ASC");
        }
        /// <summary>
        /// 获取栏目总数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetColumnTotal(string websiteOwner, bool showHide = false)
        {
            return GetCount<PermissionColumn>(GetColumnParams(websiteOwner, showHide));
        }
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public string GetColumnParams(string websiteOwner, bool showHide)
        {
            StringBuilder sbSql = new StringBuilder();
            if(string.IsNullOrWhiteSpace(websiteOwner)){
                sbSql.AppendFormat("WebsiteOwner Is Null");
            }
            else{
                sbSql.AppendFormat("WebsiteOwner = '{0}'", websiteOwner);
            }
            if (!showHide)
            {
                sbSql.AppendFormat(" AND IsHide=0 ");
            }
            return sbSql.ToString();
        }
        /// <summary>
        /// 查询站点所有权限栏目（含自定义）
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="showHide"></param>
        /// <returns></returns>
        public List<PermissionColumn> GetAllColumnListByWebsiteOwner(string websiteOwner, bool showHide = false)
        {
            List<PermissionColumn> list = GetColumnList(websiteOwner, true);
            if (string.IsNullOrWhiteSpace(websiteOwner)) return list;

            List<long> baseIDlist = list.Where(p => p.PermissionColumnBaseID > 0).Select(p => p.PermissionColumnBaseID).ToList();

            List<PermissionColumn> tempList = GetColumnList(null, showHide);
            List<PermissionColumn> resultList = tempList.Where(p => !baseIDlist.Contains(p.PermissionColumnID)).ToList();
            resultList.AddRange(list);

            if (!showHide) resultList = resultList.Where(p => p.IsHide == 0).ToList();

            return resultList.OrderBy(p => p.OrderNum).ToList();
        }
        /// <summary>
        /// 查询站点的栏目（版本内的）
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="showHide"></param>
        /// <returns></returns>
        public List<PermissionColumn> GetColumnListByWebsiteOwner(string websiteOwner, bool showHide = false)
        {
            List<PermissionColumn> list = GetAllColumnListByWebsiteOwner(websiteOwner, showHide);

            BLLPermission bllPermission = new BLLPermission();
            List<UserPmsGroupRelationInfo> pmsGroupRelList = bllPermission.GetUserPmsGroupRelListByUserId(websiteOwner);
            if (pmsGroupRelList.Count == 0)
                return list.Where(p => p.WebsiteOwner == websiteOwner).ToList();

            string group_ids = ZentCloud.Common.MyStringHelper.ListToStr(pmsGroupRelList.Select(p => p.GroupID).ToList(), "", ",");
            List<PermissionRelationInfo> colList = bllPermission.GetMultPermissionRelationList(group_ids, 3);
            if (colList.Count == 0)
                return list.Where(p => p.WebsiteOwner == websiteOwner).ToList();

            List<long> colId_list = colList.Select(p => p.PermissionID).ToList();
            return list.Where(p => colId_list.Contains(p.PermissionColumnID) || p.WebsiteOwner ==websiteOwner).ToList();
        }
        //查询账户的栏目（账户内的）
        public List<PermissionColumn> GetColumnListByUser(string userId, string websiteOwner, bool showHide = false)
        {
            List<PermissionColumn> list = new List<PermissionColumn>();
            BLLPermission bllPermission = new BLLPermission();
            List<UserPmsGroupRelationInfo> pmsGroupRelList = bllPermission.GetUserPmsGroupRelListByUserId(userId);
            if (pmsGroupRelList.Count == 0) return list;

            string group_ids = ZentCloud.Common.MyStringHelper.ListToStr(pmsGroupRelList.Select(p=>p.GroupID).ToList(), "", ",");
            List<PermissionRelationInfo> colList = bllPermission.GetMultPermissionRelationList(group_ids, 3);
            if (colList.Count == 0) return list;

            List<long> colId_list = colList.Select(p=>p.PermissionID).ToList();
            list = GetAllColumnListByWebsiteOwner(websiteOwner, showHide);
            list = list.Where(p => colId_list.Contains(p.PermissionColumnID)).ToList();
            return list;
        }

        /// <summary>
        /// 获取选择List
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <param name="baseID"></param>
        /// <param name="nLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="blankStr"></param>
        public void GetItemList(ref List<ListItem> itemList, List<PermissionColumn> list, long? parentID, long? baseID, int nLevel = 1, int maxLevel = 3, string blankStr = "")
        {
            if (nLevel > maxLevel) return;
            List<PermissionColumn> nlist = list.Where(p => p.PermissionColumnPreID == parentID || (baseID != 0 && p.PermissionColumnPreID == baseID)).ToList();
            foreach (PermissionColumn item in nlist)
            {
                itemList.Add(new ListItem
                {
                    Text = ((blankStr != "") ? (blankStr + "└") : blankStr) + item.PermissionColumnName,
                    Value = item.PermissionColumnID.ToString()
                });
                GetItemList(ref itemList, list, item.PermissionColumnID, item.PermissionColumnBaseID, nLevel + 1, maxLevel, blankStr + "\u3000");
            }
        }
        
        /// <summary>
        /// 获取数据List
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <param name="baseID"></param>
        /// <param name="nLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="blankStr"></param>
        public void GetDataList(ref List<PermissionColumn> itemList, List<PermissionColumn> list, long? parentID, long? baseID, int nLevel = 1, int maxLevel = 3, string blankStr = "")
        {
            if (nLevel > maxLevel) return;
            List<PermissionColumn> nlist = list.Where(p => p.PermissionColumnPreID == parentID || (baseID != 0 && p.PermissionColumnPreID == baseID)).ToList();
            foreach (PermissionColumn item in nlist)
            {
                item.PermissionColumnName = ((blankStr != "") ? (blankStr + "└") : blankStr) + item.PermissionColumnName;
                itemList.Add(item);
                GetDataList(ref itemList, list, item.PermissionColumnID, item.PermissionColumnBaseID, nLevel + 1, maxLevel, blankStr + "\u3000");
            }
        }
    }
}
