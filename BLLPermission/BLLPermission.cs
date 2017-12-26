using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLPermission
{
    public class BLLPermission : BLL
    {

        /// <summary>
        /// id查询权限组
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public PermissionGroupInfo GetPermissionGroup(string websiteOwner, long groupId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" GroupID = '{0}' ", groupId);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            return Get<PermissionGroupInfo>(sbSql.ToString());
        }
        /// <summary>
        /// 添加权限组
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <param name="relations"></param>
        /// <returns></returns>
        public long AddGroupInfo(PermissionGroupInfo groupInfo, List<long> pmsIds, BLLTransaction tran)
        {
            if (Get<PermissionGroupInfo>(string.Format(" GroupName='{0}' AND WebsiteOwner='{1}' "
                , groupInfo.GroupName, groupInfo.WebsiteOwner)) != null)
            {
                throw new Exception("权限组名称已被使用");
            }
            groupInfo.GroupID = Convert.ToInt64(GetGUID(TransacType.PermissionGroupAdd));
            if (!Add(groupInfo, tran))
            {
                throw new Exception("添加权限组失败");
            }
            foreach (long pmsId in pmsIds)
            {
                PermissionRelationInfo relation = new PermissionRelationInfo();
                relation.RelationID = groupInfo.GroupID.ToString();
                relation.PermissionID = pmsId;
                relation.RelationType = 0;
                if (!Add(relation, tran))
                {
                    throw new Exception("添加权限关系失败");
                }
            }
            return groupInfo.GroupID;
        }
        /// <summary>
        /// 添加权限组
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <param name="relations"></param>
        /// <returns></returns>
        public void UpdateGroupInfo(PermissionGroupInfo groupInfo, List<long> pmsIds, BLLTransaction tran)
        {
            PermissionGroupInfo tempOld = Get<PermissionGroupInfo>(string.Format(" GroupName='{0}' AND WebsiteOwner='{1}' "
                , groupInfo.GroupName, groupInfo.WebsiteOwner));
            if (tempOld != null && tempOld.GroupID != groupInfo.GroupID)
            {
                throw new Exception("权限组名称已被使用");
            }
            if (!Update(groupInfo, tran))
            {
                throw new Exception("编辑权限组失败");
            }
            List<PermissionRelationInfo> oldList = GetList<PermissionRelationInfo>(
                string.Format(" RelationType=0 AND RelationID='{0}' ", groupInfo.GroupID));

            foreach (PermissionRelationInfo item in oldList)
            {
                if (!pmsIds.Exists(p => p == item.PermissionID))
                {
                    if (Delete(item, tran) == -1)
                    {
                        throw new Exception("删除旧权限关系失败失败");
                    }
                }
            }
            foreach (long pmsId in pmsIds)
            {
                if (!oldList.Exists(p => p.PermissionID == pmsId))
                {
                    PermissionRelationInfo relation = new PermissionRelationInfo();
                    relation.RelationID = groupInfo.GroupID.ToString();
                    relation.PermissionID = pmsId;
                    relation.RelationType = 0;
                    if (!Add(relation, tran))
                    {
                        throw new Exception("添加新权限关系失败");
                    }
                }
            }
        }
        /// <summary>
        /// 拼接查询语句查询
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="groupIds"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        private string GetQueryWhere(string keyWord, string websiteOwner, string groupIds, int? groupType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner))
                sbWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(keyWord))
                sbWhere.AppendFormat(" AND GroupName like '%{0}%' ", keyWord);
            if (!string.IsNullOrWhiteSpace(groupIds))
                sbWhere.AppendFormat(" AND GroupID In ({0}) ", groupIds);
            if (groupType.HasValue)
                sbWhere.AppendFormat(" AND GroupType = {0} ", groupType.Value);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 查询权限组数量
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="groupIds"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        public int GetGroupCount(string keyWord, string websiteOwner, string groupIds, int? groupType)
        {
            return GetCount<PermissionGroupInfo>(GetQueryWhere(keyWord, websiteOwner, groupIds, groupType));
        }
        /// <summary>
        /// 查询权限组列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyWord"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="groupIds"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        public List<PermissionGroupInfo> GetGroupList(int pageSize, int pageIndex, string keyWord, string websiteOwner, string groupIds, int? groupType)
        {
            return GetLit<PermissionGroupInfo>(pageSize, pageIndex, GetQueryWhere(keyWord, websiteOwner, groupIds, groupType));
        }
        /// <summary>
        /// 查询权限组列表 (带数量返回)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyWord"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="total"></param>
        /// <param name="groupIds"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        public List<PermissionGroupInfo> GetGroupList(int pageSize, int pageIndex, string keyWord, string websiteOwner, out int total, string groupIds, int? groupType)
        {
            string strWhere = GetQueryWhere(keyWord, websiteOwner, groupIds, groupType);
            total = GetCount<PermissionGroupInfo>(strWhere);
            return GetLit<PermissionGroupInfo>(pageSize, pageIndex, strWhere);
        }

        /// <summary>
        /// 查询权限组权限关系
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public List<PermissionRelationInfo> GetPermissionRelationList(string relationID, int relationType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" RelationID = '{0}' ", relationID);
            sbWhere.AppendFormat(" AND RelationType = {0} ", relationType);
            return GetList<PermissionRelationInfo>(sbWhere.ToString());
        }

        //删除权限关系
        public bool DeletePermissionRelation(string relationID, int? relationType)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" RelationID='{0}' ", relationID);
            if (relationType.HasValue)
            {
                sbsql.AppendFormat(" AND RelationType={0} ", relationType.Value);
            }
            return Delete(new PermissionRelationInfo(), sbsql.ToString()) > 0;
        }

        //删除权限关系 通过权限ID
        public bool DeletePermissionRelationByPermission(string permissionID, int? relationType)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" PermissionID={0} ", permissionID);
            if (relationType.HasValue)
            {
                sbsql.AppendFormat(" AND RelationType={0} ", relationType.Value);
            }
            return Delete(new PermissionRelationInfo(), sbsql.ToString()) > 0;
        }
        /// <summary>
        /// 查询权限IDs
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public string GetPmsStrByRelationID(string relationID, int relationType)
        {
            List<PermissionRelationInfo> list = GetPermissionRelationList(relationID, relationType);
            if (list.Count == 0) return "";
            return StringHelper.ListToStr(list.Select(p => p.PermissionID).ToList(), "", ",");
        }


        /// <summary>
        /// 某关系ID的某关系是否存在
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        public bool ExistsRelation(string relationID, int relationType, long? permissionID = null)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" RelationID='{0}' ", relationID);
            sbsql.AppendFormat(" AND RelationType={0} ", relationType);
            if (permissionID.HasValue) sbsql.AppendFormat(" AND PermissionID={0} ", permissionID.Value);
            return Get<PermissionRelationInfo>(sbsql.ToString()) == null ? false : true;
        }

        /// <summary>
        /// 查询权限组权限关系
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public List<PermissionRelationInfo> GetMultPermissionRelationList(string relationIDs, int relationType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" RelationID In ({0}) ", relationIDs);
            sbWhere.AppendFormat(" AND RelationType = {0} ", relationType);
            return GetList<PermissionRelationInfo>(sbWhere.ToString());
        }

        /// <summary>
        /// 获取站点权限关系
        /// </summary>
        /// <returns></returns>
        public List<PermissionRelationInfo> GetWebsiteOwnerRelationList(string websiteOwner, int relationType)
        {
            BLLMenuPermission bllLMenuPermission = new BLLMenuPermission("");
            List<long> listRelation = bllLMenuPermission.BaseCacheGetUserPmsGroupRelationList(websiteOwner)
                .Select(p => p.GroupID).ToList();
            if (listRelation.Count == 0)
            {
                return new List<PermissionRelationInfo>();
            }
            string GroupIDs = ZentCloud.Common.MyStringHelper.ListToStr(listRelation, "'", ",");
            return GetMultPermissionRelationList(GroupIDs, relationType);
        }

        /// <summary>
        /// 根据链接查询菜单列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<PermissionInfo> GetPermissionListByPath(string path)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" 1=1 ");
            sql.AppendFormat(" AND Url='{0}' ", path);
            return GetList<PermissionInfo>(sql.ToString());
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
        public void GetDataList(ref List<PermissionGroupInfo> itemList, List<PermissionGroupInfo> list, long? parentID, int nLevel = 1, int maxLevel = 3, string blankStr = "")
        {
            if (nLevel > maxLevel) return;
            List<PermissionGroupInfo> nlist = list.Where(p => p.PreID == parentID).ToList();
            foreach (PermissionGroupInfo item in nlist)
            {
                item.GroupName = ((blankStr != "") ? (blankStr + "└") : blankStr) + item.GroupName;
                itemList.Add(item);
                GetDataList(ref itemList, list, item.GroupID, nLevel + 1, maxLevel, blankStr + "\u3000");
            }
        }
        /// <summary>
        /// 查询用户权限组ID列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PermissionGroupInfo> GetUserGroupList(string userId, string websiteOwner, int groupType)
        {
            BLLMenuPermission bllUserPmsGroupRelation = new BLLMenuPermission("");
            List<long> groupIDList = bllUserPmsGroupRelation.BaseCacheGetUserPmsGroupRelationList(userId)
                .Select(p => p.GroupID).ToList();
            if (groupIDList.Count >= 0)
            {
                string groupIDStrs = MyStringHelper.ListToStr(groupIDList, "'", ",");
                return GetGroupList(int.MaxValue, 1, null, websiteOwner, groupIDStrs, groupType);
            }
            return new List<PermissionGroupInfo>();
        }

        /// <summary>
        ///用户权限组
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<UserPmsGroupRelationInfo> GetUserPmsGroupRelList(string groupId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" GroupID={0} ", groupId);
            return GetList<UserPmsGroupRelationInfo>(sql.ToString());
        }
        /// <summary>
        ///用户权限组
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<UserPmsGroupRelationInfo> GetUserPmsGroupRelListByMultUserId(string userIds)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" UserID In ({0}) ", userIds);
            return GetList<UserPmsGroupRelationInfo>(sql.ToString());
        }
        /// <summary>
        ///用户权限组
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<UserPmsGroupRelationInfo> GetUserPmsGroupRelListByUserId(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" UserID ='{0}' ", userId);
            return GetList<UserPmsGroupRelationInfo>(sql.ToString());
        }
        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="groupList"></param>
        /// <param name="userPmsGroupRelList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetRoleName(List<PermissionGroupInfo> groupList, List<UserPmsGroupRelationInfo> userPmsGroupRelList, string userId)
        {
            List<string> roleList = new List<string>();
            List<UserPmsGroupRelationInfo> nRelList = userPmsGroupRelList.Where(p => p.UserID == userId).ToList();
            if (nRelList.Count == 0) return "";
            List<long> nGroupIdList = nRelList.Select(p => p.GroupID).ToList();
            List<PermissionGroupInfo> zGroupList = groupList.Where(p => nGroupIdList.Contains(p.GroupID)).ToList();
            if (zGroupList.Count == 0) return "";
            return ZentCloud.Common.MyStringHelper.ListToStr(zGroupList.Select(p => p.GroupName).Distinct().ToList(), "", ",");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <param name="nLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="blankStr"></param>
        public void GetGroupItemList(ref List<ListItem> itemList, List<PermissionGroupInfo> list, long? parentID, int nLevel = 1, int maxLevel = 3, string blankStr = "")
        {
            if (nLevel > maxLevel) return;
            List<PermissionGroupInfo> nlist = list.Where(p => p.PreID == parentID).ToList();
            foreach (PermissionGroupInfo item in nlist)
            {
                itemList.Add(new ListItem
                {
                    Text = ((blankStr != "") ? (blankStr + "└") : blankStr) + item.GroupName,
                    Value = item.GroupID.ToString()
                });
                GetGroupItemList(ref itemList, list, item.GroupID, nLevel + 1, maxLevel, blankStr + "\u3000");
            }
        }
        /// <summary>
        /// 所有子权限组ID
        /// </summary>
        /// <param name="groupList"></param>
        /// <param name="nGroupId"></param>
        /// <returns></returns>
        public List<long> GetAllChildGroupIdList(List<PermissionGroupInfo> groupList, long nGroupId)
        {
            List<long> groupIdList = new List<long>();
            foreach (var item in groupList.Where(p => p.PreID == nGroupId))
            {
                groupIdList.Add(item.GroupID);
                List<long> nChildIdList = GetAllChildGroupIdList(groupList, item.GroupID);
                if (nChildIdList.Count > 0)
                {
                    groupIdList.AddRange(nChildIdList);
                }
            }
            return groupIdList;
        }
        /// <summary>
        /// 检查是否启动新的权限检查
        /// </summary>
        /// <returns></returns>
        public bool IsActionPermissionV2(string websiteOwner)
        {
            return true;
            //string MapPath = System.Web.HttpContext.Current.Server.MapPath("/JsonConfig/PermissionV2Website.json");
            //if (!System.IO.File.Exists(MapPath)) return false;

            //string PermissionV2WebsiteJson = System.IO.File.ReadAllText(MapPath);
            //JObject PerJo = JObject.Parse(PermissionV2WebsiteJson);
            //if(PerJo["WebsiteOwner"] == null) return false;
            //JArray PerJa = JArray.FromObject(PerJo["WebsiteOwner"]);
            //return PerJa.Select(p => p.ToString()).Contains(websiteOwner);
        }
        /// <summary>
        /// 检查站点是否包含某个权限Key
        /// </summary>
        /// <param name="userId">账户</param>
        /// <param name="permissionKey">权限key </param>
        /// <returns></returns>
        public bool CheckPermissionKey(string userId, ZentCloud.BLLPermission.Enums.PermissionSysKey permissionKey)
        {
            string permissionKeyStr = EnumToString(permissionKey);
            BLLMenuPermission bllMenuper = new BLLMenuPermission("");
            List<PermissionInfo> pmsList = new List<PermissionInfo>();
            string strPsmIds = "0";
            List<long> psmIds = bllMenuper.GetUserAllPmsID(userId);
            if (psmIds.Count() > 0) strPsmIds = MyStringHelper.ListToStr(psmIds, "'", ",");
            pmsList = bllMenuper.GetList<PermissionInfo>(string.Format(" PermissionKey>'' AND PermissionID in ({0})", strPsmIds));
            //过滤掉已经禁止的权限
            var disPmsList = GetMultPermissionRelationList("'" + userId + "'", 9);
            pmsList = pmsList.Where(p => disPmsList.Count(dis => dis.PermissionID == p.PermissionID) == 0).ToList();
            if (pmsList != null)
            {
                return pmsList.Count(p => p.PermissionKey == permissionKeyStr) > 0;
            }
            return false;


        }

        /// <summary>
        /// 枚举转成String
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string EnumToString(object o)
        {
            Type t = o.GetType();
            string s = o.ToString();
            EnumDescriptionAttribute[] os = (EnumDescriptionAttribute[])t.GetField(s).GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (os != null && os.Length == 1)
            {
                return os[0].Text;
            }
            return s;
        }
        private class EnumDescriptionAttribute : Attribute
        {
            private string _text = "";
            public string Text
            {
                get { return this._text; }
            }
            public EnumDescriptionAttribute(string text)
            {
                _text = text;
            }

        }
    }
}
