using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLPermission
{
    public class BLLMenuInfo :BLL
    {
        /// <summary>
        /// id查询菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public MenuInfo GetMenu(long menuId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" MenuID={0} ", menuId);
            return Get<MenuInfo>(sbSql.ToString());
        }
        /// <summary>
        /// id查询菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public MenuInfo GetMenu(long menuId, int menuType, string websiteOwner)
        {
            MenuInfo menu = GetMenu(menuId);
            if (menu.MenuType != menuType) return null;
            if (menuType == 1) return menu;

            if (!string.IsNullOrWhiteSpace(websiteOwner) && menu.WebsiteOwner != websiteOwner) return null;
            return menu;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="curUser"></param>
        /// <param name="pmsGroupId"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenusByUser(UserInfo curUser, string websiteOwner,bool showHide=false, int maxLevel = 3)
        {
            List<MenuInfo> Menus = new List<MenuInfo>();

            string showLevel = "3";
            if(curUser.UserType == 1)
            {
                showLevel = "1,2,3";
            }
            else if (curUser.UserID == websiteOwner)
            {
                showLevel = "2,3";
            }
            Menus = GetMenus(0, 2, 1, websiteOwner, showHide, maxLevel, showLevel);
            if (Menus.Count > 0) return Menus;

            Menus = GetMenus(0, 1, 1, websiteOwner, showHide, maxLevel, showLevel);
            return Menus;
        }
        
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="preID"></param>
        /// <param name="menuType"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="pmsGroupId"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenus(long preID, int menuType, int nowLevel, string websiteOwner, bool showHide, int maxLevel, string showLevel)
        {
            if (nowLevel > maxLevel) return new List<MenuInfo>();

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" PreID={0} ", preID);
            sbSql.AppendFormat(" AND MenuType={0} ", menuType);
            if (menuType != 1 && !string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(showLevel)) sbSql.AppendFormat(" AND ShowLevel In ({0}) ", showLevel);
            if (!showHide) sbSql.AppendFormat(" AND IsHide = 0 ");
            sbSql.AppendFormat(" Order by MenuSort ");
            List<MenuInfo> Menus = GetList<MenuInfo>(sbSql.ToString());
            if(Menus.Count > 0)
            {
                for (int i = 0; i < Menus.Count; i++)
                {
                    if (Menus[i].ChildMenus.Count > 0) continue;
                    List<MenuInfo> TempMenus = GetMenus(Menus[i].MenuID, menuType, nowLevel + 1, websiteOwner, showHide, maxLevel, showLevel);
                    if(TempMenus.Count > 0) Menus[i].ChildMenus.AddRange(TempMenus);
                }
            }
            return Menus;
        }

        /// <summary>
        /// 获取需要删除的菜单Ids
        /// </summary>
        /// <param name="menuIds">现有Ids</param>
        /// <param name="menuType">菜单类型</param>
        /// <param name="websiteOwner">站点</param>
        /// <param name="groupId">权限组</param>
        /// <returns></returns>
        public string GetNeedDelMenuIds(string menuIds, int menuType, string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" MenuType={0} ", menuType);
            if (!string.IsNullOrWhiteSpace(menuIds)) sbSql.AppendFormat(" AND MenuID Not In ({0})", menuIds);
            if (menuType != 1 && !string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            List<long> IdList = GetList<MenuInfo>(sbSql.ToString()).Select(p=>p.MenuID).ToList();
            if(IdList.Count > 0)
            {
                return Common.MyStringHelper.ListToStr(IdList, "", ",");
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="postMenus"></param>
        /// <param name="preId"></param>
        /// <param name="tran"></param>
        /// <param name="needDelMenuIds"></param>
        /// <returns></returns>
        public bool UpdateMenus(List<MenuInfo> postMenus, long preId, BLLTransaction tran, string needDelMenuIds = null)
        {
            foreach (MenuInfo item in postMenus)
            {
                if (item.IsNew)
                {
                    item.PreID = preId;
                    item.MenuID = Convert.ToInt64(GetGUID(TransacType.MenuAdd));
                    if (!Add(item, tran) || !UpdateMenus(item.ChildMenus, item.MenuID, tran))
                    {
                        throw new Exception("新增菜单失败！");
                    }
                }
                else
                {
                    item.PreID = preId;
                    if (!Update(item, tran) || !UpdateMenus(item.ChildMenus, item.MenuID, tran))
                    {
                        throw new Exception("修改菜单失败！");
                    }
                }
            }
            if(preId == 0 && !string.IsNullOrWhiteSpace(needDelMenuIds))
            {
                if(DeleteMultByKey<MenuInfo>("MenuID", needDelMenuIds, tran) == -1)
                {
                    throw new Exception("删除旧菜单失败！");
                }
            }
            return true;
        }

        /// <summary>
        /// 查询某关系ID的所有菜单关系
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        public List<MenuRelationInfo> GetAllMenuRelationList(string relationID, int? relationType)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" RelationID='{0}' ", relationID);
            if (relationType.HasValue)
            {
                sbsql.AppendFormat(" AND RelationType={0} ", relationType.Value);
            }
            return GetList<MenuRelationInfo>(sbsql.ToString());
        }
        /// <summary>
        /// 根据类型和关系ID查询菜单列表
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public List<MenuRelationInfo> GetMenuRelationList(string relationID, int? relationType)
        {
            return GetAllMenuRelationList(relationID, relationType);
        }
        //删除菜单关系
        public bool DeleteMenuRelation(string relationID, int? relationType)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" RelationID='{0}' ", relationID);
            if (relationType.HasValue)
            {
                sbsql.AppendFormat(" AND RelationType={0} ", relationType.Value);
            }
            return Delete(new MenuRelationInfo(), sbsql.ToString()) > 0;
        }

        /// <summary>
        /// 根据类型和关系ID查询菜单列表
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public List<MenuRelationInfo> GetMenuRelationListByRelationIds(string relationIDs, int? relationType)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" RelationID In ({0}) ", relationIDs);
            if (relationType.HasValue) sbsql.AppendFormat(" AND RelationType={0} ", relationType);
            return GetList<MenuRelationInfo>(sbsql.ToString());
        }
        /// <summary>
        /// 根据类型和关系ID组查询菜单列表
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public List<MenuRelationInfo> GetMenuRelationList(string relationID, List<int> relationTypes)
        {
            if (relationTypes.Count>0)
            {
                return GetAllMenuRelationList(relationID, null).Where(p => relationTypes.Contains(p.RelationType)).ToList();
            }
            else
            {
                return GetAllMenuRelationList(relationID, null);
            }
        }
        /// <summary>
        /// 根据类型和关系ID 菜单ID 查关系
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public MenuRelationInfo GetMenuRelation(string relationID, int relationType, long menuId)
        {
            return GetAllMenuRelationList(relationID, relationType).FirstOrDefault(p => p.MenuID == menuId);
        }

        /// <summary>
        /// 批量修改关系
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <param name="addList"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public bool UpdateMenuRelations(List<MenuRelationInfo> addList, List<MenuRelationInfo> delList,out string msg)
        {
            msg = "修改成功";
            BLLTransaction tran = new BLLTransaction();
            foreach (var item in delList)
            {
                if (Delete(item, tran)< 0)
                {
                    msg = "删除出错";
                    tran.Rollback();
                    return false;
                }
            }
            foreach (var item in addList)
            {
                if (!Add(item, tran))
                {
                    msg = "新增失败";
                    tran.Rollback();
                    return false;
                }
            }
            tran.Commit();
            return true;
        }

        /// <summary>
        /// 查询菜单IDs
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public string GetMenuStrByRelationID(string relationID, int relationType)
        {
            List<MenuRelationInfo> list = GetMenuRelationList(relationID, relationType);
            if (list.Count == 0) return "";
            return StringHelper.ListToStr(list.Select(p => p.MenuID).ToList(), "", ",");
        }

        /// <summary>
        /// 站点隐藏菜单
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int WebsiteHideMenu(string menuIds, string websiteOwner)
        {
            List<long> idlist = menuIds.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            List<MenuRelationInfo> oldMenuList = GetMenuRelationList(websiteOwner, 1);
            List<MenuRelationInfo> addList = new List<MenuRelationInfo>();
            foreach (var item in idlist)
            {
                if (!oldMenuList.Exists(p => p.MenuID == item))
                {
                    MenuRelationInfo temp = new MenuRelationInfo();
                    temp.MenuID = item;
                    temp.RelationID = websiteOwner;
                    temp.RelationType = 1;
                    addList.Add(temp);
                }
            }
            string msg = "";
            if (UpdateMenuRelations(addList, new List<MenuRelationInfo>(), out msg))
            {
                return addList.Count;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 站点显示菜单
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="websiteOwner"></param>
        public int WebsiteShowMenu(string menuIds, string websiteOwner)
        {
            List<long> idlist = menuIds.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            List<MenuRelationInfo> OldMenuList = GetMenuRelationList(websiteOwner, 1);
            List<MenuRelationInfo> delList = new List<MenuRelationInfo>();
            foreach (var item in idlist)
            {
                if (OldMenuList.Exists(p => p.MenuID == item))
                {
                    MenuRelationInfo temp = new MenuRelationInfo();
                    temp.MenuID = item;
                    temp.RelationID = websiteOwner;
                    temp.RelationType = 1;
                    delList.Add(temp);
                }
            }
            string msg = "";
            if (UpdateMenuRelations(new List<MenuRelationInfo>(), delList, out msg))
            {
                return delList.Count;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 加ex1关系
        /// relationType为2 显示级别
        /// relationType为3 排序
        /// relationType为4 名称
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="menuID"></param>
        /// <param name="ex1"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public int EditBaseMenuEx(string websiteOwner, long menuID, string ex, int relationType)
        {
            List<int> intRelationType = new List<int>(){2,3}; //数字关系
            List<int> stringRelationType = new List<int>(){4}; //字符串关系
            if(!intRelationType.Contains(relationType) && !stringRelationType.Contains(relationType)) return 0;

            MenuRelationInfo relationInfo = GetMenuRelation(websiteOwner, relationType, menuID);//显示级别关系
            int result = 0;
            MenuInfo menuInfo = new MenuInfo();

            if (string.IsNullOrWhiteSpace(ex))
            {
                if (relationInfo != null) result = Delete(relationInfo);
            }
            if (result == 0)
            {
                if (relationInfo != null)
                {
                    if (intRelationType.Contains(relationType) && relationInfo.Ex1 == Convert.ToInt32(ex)) return 0;
                    if (stringRelationType.Contains(relationType) && relationInfo.Ex2 == ex) return 0;
                }
                menuInfo = GetMenu(menuID);
                if (relationInfo == null && relationType == 2 && menuInfo.ShowLevel == Convert.ToInt32(ex)) return 0;
                if (relationInfo == null && relationType == 3 && menuInfo.MenuSort == Convert.ToInt32(ex)) return 0;
                if (relationInfo == null && relationType == 4 && menuInfo.NodeName == ex) return 0;

            }

            if (result == 0 && ((relationInfo != null && relationType == 2 && menuInfo.ShowLevel == Convert.ToInt32(ex)) ||
                (relationInfo != null && relationType ==3 && menuInfo.MenuSort == Convert.ToInt32(ex))||
                (relationInfo != null && relationType ==4 && menuInfo.NodeName == ex))) {
                    result = Delete(relationInfo);
            }
            if (result == 0 && ((relationInfo != null && relationType == 2 && menuInfo.ShowLevel != Convert.ToInt32(ex)) ||
                (relationInfo != null && relationType ==3 && menuInfo.MenuSort != Convert.ToInt32(ex))||
                (relationInfo != null && relationType ==4 && menuInfo.NodeName != ex))) {
                
                if (intRelationType.Contains(relationType)) relationInfo.Ex1 =Convert.ToInt32(ex);
                if (stringRelationType.Contains(relationType)) relationInfo.Ex2 = ex;
                result = Update(relationInfo) ? 1 : -1;
            }

            if (relationInfo == null && result ==0)
            {
                relationInfo = new MenuRelationInfo();
                relationInfo.RelationID = websiteOwner;
                relationInfo.MenuID = menuID;
                relationInfo.RelationType = relationType;
                if (intRelationType.Contains(relationType)) { 
                    relationInfo.Ex1 = Convert.ToInt32(ex); 
                }
                if (stringRelationType.Contains(relationType)) {
                    relationInfo.Ex2 = ex; 
                }
                result = Add(relationInfo)? 1 : -1;
            }
            if (relationType == 2 && result == 1)
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" PreID={0} ", menuID);
                sbSql.AppendFormat(" AND (WebsiteOwner='{0}' OR WebsiteOwner Is null) ", websiteOwner);
                List<MenuInfo> childList = GetList<MenuInfo>(sbSql.ToString());
                foreach (var item in childList)
                {
                    EditBaseMenuEx(websiteOwner, item.MenuID, ex, relationType);
                }
            }
            return 0;
        }
       
        /// <summary>
        /// 缓存站点列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<MenuInfo> BaseCacheGetMenuList(string websiteOwner)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" 1=1 ");
            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                sql.AppendFormat(" AND WebsiteOwner is null ");
            }
            else
            {
                sql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            }
            return GetList<MenuInfo>(sql.ToString());
        }

        public List<MenuInfo> GetWebsiteMenuList(string websiteOwner)
        {
            //自定义菜单
            List<MenuInfo> reList = new List<MenuInfo>();
            List<MenuInfo> list = BaseCacheGetMenuList(websiteOwner);
            reList.AddRange(list);
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                List<MenuInfo> tempList = BaseCacheGetMenuList(null);
                reList.AddRange(tempList);
            }
            //基础菜单
            return reList.OrderBy(p => p.PreID).ThenBy(p => p.MenuSort).ToList();
        }
        /// <summary>
        /// 获取菜单，过滤基本显示级别
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="showLevel"></param>
        /// <param name="onlyShowPreMenu"></param>
        /// <param name="showHideMenu"></param>
        /// <returns></returns>
        public List<MenuInfo> GetWebsiteMenuList(string websiteOwner, int showLevel = 3, bool onlyShowPreMenu = false
            , bool showHideMenu = false)
        {
            IEnumerable<MenuInfo> rList = GetWebsiteMenuList(websiteOwner)
                .Where(p => p.ShowLevel >= showLevel);

            if (onlyShowPreMenu)
            {
                rList = rList.Where(p => p.PreID == 0);
            }
            if (!showHideMenu)
            {
                rList = rList.Where(p => p.IsHide == 0);
            }
            return rList.ToList();
        }

        /// <summary>
        /// 检查菜单权限
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<MenuInfo> CheckPmsMenuRelationList(List<MenuInfo> menuList, string relationIds, string websiteOwner)
        {
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                List<MenuRelationInfo> relationHideList = GetMenuRelationList(websiteOwner, 1);
                List<MenuRelationInfo> relationShowLevelList = GetMenuRelationList(websiteOwner, 2);
                List<MenuRelationInfo> relationMenuSortlList = GetMenuRelationList(websiteOwner, 3);
                List<MenuRelationInfo> relationMenuNameList = GetMenuRelationList(websiteOwner, 4);
                foreach (var item in menuList)
                {
                    if (relationHideList.Exists(p => p.MenuID.Equals(item.MenuID))) item.IsHide = 1;
                    MenuRelationInfo tempShowLevelRelation = relationShowLevelList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempShowLevelRelation != null) item.ShowLevel = tempShowLevelRelation.Ex1;
                    MenuRelationInfo tempMenuSortRelation = relationMenuSortlList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempMenuSortRelation != null) item.MenuSort = tempMenuSortRelation.Ex1;
                    MenuRelationInfo tempMenuNameRelation = relationMenuNameList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempMenuNameRelation != null)
                    {
                        item.BaseName = item.NodeName;
                        item.NodeName = tempMenuNameRelation.Ex2;
                    }
                }
            }
            return menuList.OrderBy(p => p.PreID).ThenBy(p => p.MenuSort).ToList();
        }

        /// <summary>
        /// 检查自定义菜单关系
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<MenuInfo> CheckMenuRelationList(List<MenuInfo> menuList, string websiteOwner, string relationIds)
        {
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                List<MenuRelationInfo> relationPmsList = GetMenuRelationListByRelationIds(relationIds, 0);
                List<MenuRelationInfo> relationHideList = GetMenuRelationList(websiteOwner, 1);
                List<MenuRelationInfo> relationShowLevelList = GetMenuRelationList(websiteOwner, 2);
                List<MenuRelationInfo> relationMenuSortlList = GetMenuRelationList(websiteOwner, 3);
                List<MenuRelationInfo> relationMenuNameList = GetMenuRelationList(websiteOwner, 4);
                foreach (var item in menuList)
                {
                    if (!relationPmsList.Exists(p => p.MenuID.Equals(item.MenuID)) && item.WebsiteOwner != websiteOwner && item.ShowLevel != 1) {
                        item.Url = "/error/version.htm";
                    }
                    if (relationHideList.Exists(p => p.MenuID.Equals(item.MenuID))) item.IsHide = 1;
                    MenuRelationInfo tempShowLevelRelation = relationShowLevelList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempShowLevelRelation != null) item.ShowLevel = tempShowLevelRelation.Ex1;
                    MenuRelationInfo tempMenuSortRelation = relationMenuSortlList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempMenuSortRelation != null) item.MenuSort = tempMenuSortRelation.Ex1;
                    MenuRelationInfo tempMenuNameRelation = relationMenuNameList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempMenuNameRelation != null) {
                        item.BaseName = item.NodeName;
                        item.NodeName = tempMenuNameRelation.Ex2; 
                    }
                }
            }
            return menuList.OrderBy(p => p.PreID).ThenBy(p => p.MenuSort).ToList();
        }
        /// <summary>
        /// 检查自定义菜单关系
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<MenuInfo> NewCheckMenuRelationList(List<MenuInfo> menuList, string websiteOwner)
        {
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                List<MenuRelationInfo> relationHideList = GetMenuRelationList(websiteOwner, 1);
                List<MenuRelationInfo> relationShowLevelList = GetMenuRelationList(websiteOwner, 2);
                List<MenuRelationInfo> relationMenuSortlList = GetMenuRelationList(websiteOwner, 3);
                List<MenuRelationInfo> relationMenuNameList = GetMenuRelationList(websiteOwner, 4);
                foreach (var item in menuList)
                {
                    if (relationHideList.Exists(p => p.MenuID.Equals(item.MenuID))) item.IsHide = 1;
                    MenuRelationInfo tempShowLevelRelation = relationShowLevelList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempShowLevelRelation != null) item.ShowLevel = tempShowLevelRelation.Ex1;
                    MenuRelationInfo tempMenuSortRelation = relationMenuSortlList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempMenuSortRelation != null) item.MenuSort = tempMenuSortRelation.Ex1;
                    MenuRelationInfo tempMenuNameRelation = relationMenuNameList.FirstOrDefault(p => p.MenuID.Equals(item.MenuID));
                    if (tempMenuNameRelation != null)
                    {
                        item.BaseName = item.NodeName;
                        item.NodeName = tempMenuNameRelation.Ex2;
                    }
                }
            }
            return menuList.OrderBy(p => p.PreID).ThenBy(p => p.MenuSort).ToList();
        }
        /// <summary>
        /// 检查Index显示级别
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="showLevel"></param>
        /// <returns></returns>
        public List<MenuInfo> CheckUserMenuShowLevelList(List<MenuInfo> menuList, int showLevel)
        {
            return menuList.Where(p => p.ShowLevel >= showLevel).ToList();
        }

        public int DeleteMenu(string menuIds, string websiteOwner)
        {
            List<long> idlist = menuIds.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            int delCount = 0;
            DeleteMenu(idlist, websiteOwner, ref delCount);
            return delCount;
        }
        private void DeleteMenu(List<long> idlist, string websiteOwner, ref int delCount)
        {
            foreach (long Id in idlist)
            {
                List<MenuInfo> list = BaseCacheGetMenuList(websiteOwner).Where(p => p.PreID == Id).ToList();
                if (list.Count > 0)
                {
                    DeleteMenu(list.Select(p => p.MenuID).ToList(), websiteOwner, ref delCount);
                }
                Delete(new MenuInfo(), string.Format(" MenuID ={0}", Id));
                delCount++;
            }
        }
        /// <summary>
        /// 删除菜单(通过类型 站点 权限组)
        /// </summary>
        /// <param name="menuType"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int DeleteMenu(int menuType, string websiteOwner, string groupId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" MenuType={0} ", menuType);
            sql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(groupId)) sql.AppendFormat(" AND PmsGroupId='{0}' ", groupId);
            int r = Delete(new MenuInfo(), sql.ToString());
            return r;
        }

        /// <summary>
        /// 某关系ID的某关系是否存在
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        public bool ExistsMenuRelation(string relationID, int relationType, long? menuID = null)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" RelationID='{0}' ", relationID);
            sbsql.AppendFormat(" AND RelationType={0} ", relationType);
            if (menuID.HasValue) sbsql.AppendFormat(" AND MenuID={0} ", menuID.Value);
            return Get<MenuRelationInfo>(sbsql.ToString()) == null ? false : true;
        }
        /// <summary>
        /// 根据链接查询菜单列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenuListByPath(string path ,string websiteOwner)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" 1=1 ");
            sql.AppendFormat(" AND (WebsiteOwner='{0}' OR WebsiteOwner Is Null) ", websiteOwner);
            sql.AppendFormat(" AND Url='{0}' ", path);
            return GetList<MenuInfo>(sql.ToString());
        }
    }
}
