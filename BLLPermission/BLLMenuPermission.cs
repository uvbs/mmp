using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLPermission.Model;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLPermission
{
    public static class MyEnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element))) { yield return element; }
            }
        }
    }


    public class BLLMenuPermission : BLL
    {

        /**********注意事项**********
         * 
         *  1.链接路径都为小写字母；
         *  2.ID返回为0的都是失败或者没有的；
         * 
         * 
         ****************************/

        public BLLMenuPermission(string userID)
            : base(userID)
        {

        }



        /// <summary>
        /// 根据权限键值代码获取权限
        /// </summary>
        /// <param name="pmsKey"></param>
        /// <returns></returns>
        public Model.PermissionInfo GetPmsByPmsKey(string pmsKey)
        {
            Model.PermissionInfo result;
            result = Get<Model.PermissionInfo>(string.Format(" PermissionKey = '{0}' ", pmsKey));
            return result;
        }

        /// <summary>
        /// 判断权限键值代码是否重复
        /// </summary>
        /// <param name="pmsKey"></param>
        /// <returns></returns>
        public bool IsRepeatPmsKey(string pmsKey)
        {
            Model.PermissionInfo model = GetPmsByPmsKey(pmsKey);
            if (model != null)
                return true;
            return false;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddPms(Model.PermissionInfo model, out string msg)
        {
            //if (string.IsNullOrWhiteSpace(model.PermissionKey))
            //{
            //    msg = "权限键值代码不能为空！";
            //    return false;
            //}

            //if (IsRepeatPmsKey(model.PermissionKey))
            //{
            //    msg = "权限键值代码重复！";
            //    return false;
            //}

            model.PermissionID = long.Parse(GetGUID(Common.TransacType.PermissionAdd));
            bool result = Add(model);

            if (result)
            {
                msg = "添加成功！";
            }
            else
            {
                msg = "添加失败！";
            }

            return result;
        }

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>所有权限集合</returns>
        public List<Model.PermissionInfo> GetUserAllPms(string userId)
        {
            List<Model.PermissionInfo> result = new List<Model.PermissionInfo>();

            List<long> pmsList = GetUserAllPmsID(userId);//获取所有权限ID

            if (pmsList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (long item in pmsList)
                {
                    sb.Append(item.ToString());
                    sb.Append(",");
                }

                result = GetList<Model.PermissionInfo>(string.Format(" PermissionID in ({0}) ", sb.ToString().TrimEnd(',')));

            }

            return result;
        }

        /// <summary>
        /// 获取用户所有权限ID
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>所有权限ID集合</returns>
        public List<long> GetUserAllPmsID(string userId)
        {
            UserInfo userInfo = Get<UserInfo>(string.Format(" UserID='{0}'", userId));
            if (userInfo == null)
            {
                return new List<long>();
            }
            if (userInfo.PermissionGroupID.HasValue)
            {
                ZentCloud.BLLPermission.Model.PermissionGroupInfo perGroupInfo = Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format(" GroupID={0}", userInfo.PermissionGroupID));
                if (perGroupInfo != null && perGroupInfo.GroupType == 3)//管理员权限跟站点所有者一致
                {
                    userId = userInfo.WebsiteOwner;
                }

            }
            List<long> result = new List<long>();
            //获取 用户-权限 列表
            result.AddRange(GetList<Model.PermissionRelationInfo>(
                     string.Format(" RelationID = '{0}' and RelationType = 1 ", userId)
                 ).Select(p => p.PermissionID));

            //获取 组-权限 列表
            //long groupID = GetPmsGroupIDByUser(userID);

            //if (groupID > 0)
            //{
            //    result.AddRange(GetList<Model.PermissionRelationInfo>(
            //        string.Format(" RelationID = '{0}' and RelationType = 0 ", groupID.ToString())
            //    ).Select(p => p.PermissionID));
            //}

            foreach (long item in GetPmsGroupIDByUser(userId))
            {
                result.AddRange(GetList<Model.PermissionRelationInfo>(
                    string.Format(" RelationID = '{0}' and RelationType = 0 ", item.ToString())
                ).Select(p => p.PermissionID));

                //检查栏目内权限
                foreach (PermissionRelationInfo citem in BaseCacheGetPermissionRelationList(3, item.ToString(), null))
                {
                    List<PermissionRelationInfo> list = BaseCacheGetPermissionRelationList(2, citem.PermissionID.ToString(), null);
                    if (list.Count > 0)
                    {
                        result.AddRange(list.Select(p => p.PermissionID));
                    }
                }
            }

            if(userId == userInfo.WebsiteOwner)
            {
                List<PermissionColumn> column_list = GetListByKey<PermissionColumn>("WebsiteOwner", userId);
                if (column_list.Count > 0)
                {
                    List<long> columnIdList = column_list.Select(p => p.PermissionColumnID).ToList();
                    List<long> columnIdList1 = column_list.Where(p => p.PermissionColumnBaseID > 0).Select(p => p.PermissionColumnBaseID).ToList();
                    columnIdList.AddRange(columnIdList1);
                    string columnIdStrs = MyStringHelper.ListToStr(columnIdList, "'", ",");

                    BLLPermission bllPer = new BLLPermission();
                    List<PermissionRelationInfo> columnPerList = bllPer.GetMultPermissionRelationList(columnIdStrs, 2);
                    if (columnPerList.Count > 0)
                    {
                        result.AddRange(columnPerList.Select(p => p.PermissionID));
                    }
                }
            }
            return result.Distinct().ToList();
        }

        /// <summary>
        /// 获取用户所有的菜单ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<long> GetUserMenuIDList(string userId)
        {
            //获取用户所有权限的菜单列表
            return GetUserAllPms(userId).Where(p => p.MenuID != null && p.MenuID != 0).Select(p => p.MenuID.Value).Distinct().ToList();
        }

        /// <summary>
        /// 获取用户所有菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Model.MenuInfo> GetUserMenuList(string userId)
        {
            List<Model.MenuInfo> result = new List<Model.MenuInfo>();

            List<long> menuIDList = GetUserMenuIDList(userId);

            if (menuIDList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (long item in menuIDList)
                {
                    sb.Append(item.ToString());
                    sb.Append(",");
                }

                result = GetList<Model.MenuInfo>(string.Format(" MenuID in ({0}) ", sb.ToString().TrimEnd(',')));
            }

            return result;
        }

        /// <summary>
        /// 获取用户菜单HTML
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string GetUserMenuTreeHtml(string userId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                List<Model.MenuInfo> menuLastList = GetUserMenuList(userId);//获取所有末端节点

                if (menuLastList.Count > 0)
                {
                    List<Model.MenuInfo> menuList = new List<Model.MenuInfo>();

                    foreach (Model.MenuInfo item in menuLastList)
                    {
                        menuList.AddRange(GetMenuSingelTree(item));
                    }//获取完整菜单列表

                    menuList = menuList.Where(p => !p.PreID.Equals(0) && p.IsHide != 1).DistinctBy(p => p.MenuID).OrderByDescending(p => p.MenuSort).ToList();//去一级模块和隐藏菜单、去重、排序

                    //获取菜单所有一级模块
                    List<Model.MenuInfo> tmpMenuList = GetList<Model.MenuInfo>(" PreID = 0 and IsHide = 0 ");

                    tmpMenuList = tmpMenuList.OrderByDescending(p => p.MenuSort).ToList();

                    foreach (Model.MenuInfo i in tmpMenuList)
                    {

                        //暂时特殊化将系统设置只能超级管理员有
                        if (i.NodeName == "系统设置")
                        {
                            UserInfo userModel = Get<UserInfo>(string.Format(" UserID = '{0}'", userId));
                            if (userModel.UserType != 1)
                                continue;
                        }

                        sb.AppendFormat(@"<div data-options=""iconCls:'{0}'"" title=""{1}""><div class=""easyui-panel"" fit=""true"" border=""false"">",
                            i.ICOCSS,
                            i.NodeName
                            );

                        List<Model.MenuInfo> tmpList = menuList.Where(p => p.PreID.Equals(i.MenuID)).ToList();

                        if (tmpList.Count.Equals(0))
                            sb.Append("<div style='with:100; color:red; text-align:center;'>请联系管理员开通</div>");

                        foreach (Model.MenuInfo j in tmpList)
                        {
                            sb.Append(GetSingelTreeHtml(j, menuList).ToString());
                        }

                        sb.Append("</div></div>");
                    }


                    //foreach (Model.MenuInfo item in menuList.Where(p => p.PreID.Equals(0)).ToList())
                    //{
                    //    sb.AppendFormat(@"<div data-options=""iconCls:'{0}'"" title=""{1}""><div class=""easyui-panel"" fit=""true"" border=""false"">{2}</div></div>",
                    //        item.ICOCSS,
                    //        item.NodeName,
                    //        GetSingelTreeHtml(item, menuList).ToString()
                    //        );

                    //    //sb.Append(
                    //    //    GetSingelTreeHtml(item, menuList).ToString()
                    //    //    );
                    //}

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return sb.ToString().Replace("</ul><ul class=\"easyui-tree\">", "");
        }


        /// <summary>
        /// 获取单个末端菜单的完整树菜单列表
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public List<Model.MenuInfo> GetMenuSingelTree(Model.MenuInfo menu)
        {
            List<Model.MenuInfo> result = new List<Model.MenuInfo>();
            if (menu.PreID.Equals(0))
                result.Add(menu);
            else
            {
                result.Add(menu);
                result.AddRange(GetMenuSingelTree(Get<Model.MenuInfo>(string.Format(" MenuID = {0} ", menu.PreID))));
            }

            return result;
        }

        /// <summary>
        /// 获取指定节点下的完整树
        /// </summary>
        /// <param name="menu">指定节点</param>
        /// <param name="menuList">菜单树列表</param>
        /// <returns></returns>
        public string GetSingelTreeHtml(Model.MenuInfo menu, List<Model.MenuInfo> menuList)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul class=\"easyui-tree\">");

            List<Model.MenuInfo> childList = menuList.Where(p => p.PreID.Equals(menu.MenuID)).ToList();

            if (childList.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(menu.Url))
                {
                    sb.AppendFormat(@"<li data-options=""iconCls:'{0}',state:'closed'""><span><a href=""javascript:;"" icon=""{0}"" rel=""{2}"">{1}</a></span>", menu.ICOCSS, menu.NodeName, menu.Url);//<a href=""javascript:;"" icon=""{0}"" rel=""{1}"">{2}</a>
                }
                else
                {
                    sb.AppendFormat(@"<li data-options=""iconCls:'{0}',state:'closed'""><span>{1}</span>", menu.ICOCSS, menu.NodeName);
                }

                foreach (Model.MenuInfo item in childList)
                {
                    sb.Append(GetSingelTreeHtml(item, menuList));
                }

                sb.Append("</li>");
            }
            else
            {
                sb.AppendFormat(@"<li data-options=""iconCls:'{0}'""><a href=""javascript:;"" icon=""{0}"" rel=""{1}"">{2}</a></li>",
                        menu.ICOCSS,
                        menu.Url,
                        menu.NodeName
                    );
            }

            sb.Append("</ul>");
            return sb.ToString();
        }
        /// <summary>
        /// 获取用户菜单HTML
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public string GetNewUserMenuTreeHtml(string userId, string websiteOwner)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                UserInfo curUser = BaseCacheGetUserInfo(userId);
                int ShowLevel = 3;
                if (curUser.UserType == 1)
                {
                    ShowLevel = 1;
                }
                else if (curUser.UserID == websiteOwner)
                {
                    ShowLevel = 2;
                }
                BLLMenuInfo bllMenu = new BLLMenuInfo();
                List<MenuInfo> list = bllMenu.GetWebsiteMenuList(websiteOwner, ShowLevel, false, true);

                string relationIds = "''";
                List<long> pmsGroupIdList = GetPmsGroupIDByUser(websiteOwner);
                if (pmsGroupIdList.Count > 0) relationIds = MyStringHelper.ListToStr(pmsGroupIdList, "'", ",");

                list = bllMenu.CheckMenuRelationList(list, websiteOwner, relationIds);
                list = bllMenu.CheckUserMenuShowLevelList(list, ShowLevel);
                list = list.Where(p => p.IsHide.Value == 0).ToList();

                List<MenuInfo> menuList = new List<MenuInfo>();
                menuList = list;

                int index = 0;
                foreach (MenuInfo item in menuList.Where(p => p.PreID == 0))
                {
                    //sb.AppendFormat("<li class=\"{0}\">", index > 0 ? "" : "active");
                    sb.AppendFormat("<li class=\"{0}\">", index > 0 ? "" : "");//默认不展开任何菜单

                    sb.AppendFormat("<a href=\"javascript:;\"><i class=\"{1}\"></i> <span class=\"nav-label\">{0}</span><span class=\"fa arrow\"></span></a>", item.NodeName, item.ICOCSS);

                    sb.AppendFormat(GetNewSingelTreeHtml(item, menuList));


                    sb.AppendFormat("</li>");
                    index++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sb.ToString();
        }
        /// <summary>
        /// 获取用户菜单HTML
        /// </summary>
        /// <param name="userId">账号</param>
        /// <param name="websiteOwner">所有者</param>
        /// <returns></returns>
        public string GetNewUserMenuTreeHtmlV2(string userId, string websiteOwner)
        {
            StringBuilder sbMenu = new StringBuilder();
            try
            {
                UserInfo userInfo = BaseCacheGetUserInfo(userId);
                int showLevel = 3;
                if (userInfo.UserType == 1)
                {
                    showLevel = 1;
                }
                else if (userInfo.UserID == websiteOwner)
                {
                    showLevel = 2;
                }
                BLLMenuInfo bllMenu = new BLLMenuInfo();
                List<MenuInfo> list = bllMenu.GetWebsiteMenuList(websiteOwner, 1, false, true);
                list = bllMenu.NewCheckMenuRelationList(list, websiteOwner);
                list = bllMenu.CheckUserMenuShowLevelList(list, showLevel);
                list = list.Where(p => p.IsHide.Value == 0).ToList();

                if (userInfo.UserType != 1)
                {
                    list = CheckPermissionColumnList(list, websiteOwner, userInfo);
                }

                List<MenuInfo> menuList = new List<MenuInfo>();
                menuList = list;

                int index = 0;
                foreach (MenuInfo item in menuList.Where(p => p.PreID == 0))
                {
                    //sbMenu.AppendFormat("<li class=\"{0}\">", index > 0 ? "" : "active");
                    sbMenu.AppendFormat("<li class=\"{0}\">", index > 0 ? "" : "");//默认不展开任何菜单

                    sbMenu.AppendFormat("<a href=\"javascript:;\"><i class=\"{1}\"></i> <span class=\"nav-label\">{0}</span><span class=\"fa arrow\"></span></a>", item.NodeName, item.ICOCSS);

                    sbMenu.AppendFormat(GetNewSingelTreeHtml(item, menuList));


                    sbMenu.AppendFormat("</li>");
                    index++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbMenu.ToString();
        }
        /// <summary>
        /// 检查栏目权限返回菜单
        /// </summary>
        /// <returns></returns>
        public List<MenuInfo> CheckPermissionColumnList(List<MenuInfo> list, string websiteOwner, UserInfo curUser)
        {
            List<MenuInfo> result = new List<MenuInfo>();

            if (list.Count == 0) return result;
            string relationIds = "''";
            List<long> pmsGroupIdList = GetPmsGroupIDByUser(curUser.UserID);
            List<PermissionRelationInfo> rel_column_list = new List<PermissionRelationInfo>();
            List<PermissionColumn> column_list = new List<PermissionColumn>();
            BLLPermission bllPermission = new BLLPermission();
            string columnRelationIds = "0";
            if (pmsGroupIdList.Count > 0)
            {
                relationIds = MyStringHelper.ListToStr(pmsGroupIdList, "'", ",");
                rel_column_list = bllPermission.GetMultPermissionRelationList(relationIds, 3);
                if (rel_column_list.Count > 0) { 
                    columnRelationIds = MyStringHelper.ListToStr(rel_column_list.Select(p => p.PermissionID).ToList(), "", ",");
                    column_list = bllPermission.GetMultListByKey<PermissionColumn>("PermissionColumnID", columnRelationIds);
                }
            }
            if (!string.IsNullOrWhiteSpace(websiteOwner) && curUser.UserID == websiteOwner)
            {
                List<PermissionColumn> column_list1 = bllPermission.GetListByKey<PermissionColumn>("WebsiteOwner", websiteOwner);
                column_list.AddRange(column_list1);
            }
            if (column_list.Count == 0) return result;

            List<long> columnId_list = column_list.Select(p => p.PermissionColumnID).ToList();
            List<long> columnId_list1 = column_list.Where(p => p.PermissionColumnBaseID > 0).Select(p => p.PermissionColumnBaseID).ToList();
            columnId_list.AddRange(columnId_list1);
            string columnRelationIdStrs = MyStringHelper.ListToStr(columnId_list, "'", ",");

            BLLMenuInfo bllMenu = new BLLMenuInfo();
            List<MenuRelationInfo> column_menu_list = bllMenu.GetMenuRelationListByRelationIds(columnRelationIdStrs, 5);
            if (column_menu_list.Count == 0) return result;

            List<long> menuId_list = column_menu_list.Select(p => p.MenuID).ToList();
            result = list.Where(p => menuId_list.Contains(p.MenuID)).ToList();
            return result;
        }
        /// <summary>
        /// 获取单个末端菜单的完整树菜单列表
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public List<MenuInfo> GetNewMenuSingelTree(MenuInfo menu, List<MenuInfo> menuList, List<PermissionInfo> userPermissionList, bool haveChildPms = false)
        {
            List<MenuInfo> result = new List<MenuInfo>();
            if (menu == null) return result;
            string path = menu.Url.ToLower();
            string url = path.Contains("?") ? path.Substring(0, path.IndexOf("?")) : path;
            url = url.Contains("#") ? url.Substring(0, url.IndexOf("#")) : url;
            if (haveChildPms || userPermissionList.Exists(p => p.Url.ToLower() == url))
            {
                haveChildPms = true;
                result.Add(menu);
            }
            if (!menu.PreID.Equals(0))
            {
                result.AddRange(GetNewMenuSingelTree(menuList.FirstOrDefault(p => p.MenuID == menu.PreID), menuList, userPermissionList, haveChildPms));
            }
            return result;
        }
        /// <summary>
        /// 获取指定节点下的完整树
        /// </summary>
        /// <param name="menu">指定节点</param>
        /// <param name="menuList">菜单树列表</param>
        /// <returns></returns>
        public string GetNewSingelTreeHtml(MenuInfo menu, List<MenuInfo> menuList)
        {
            StringBuilder sb = new StringBuilder();
            List<MenuInfo> childList = menuList.Where(p => p.PreID.Equals(menu.MenuID)).ToList();

            if (childList.Count > 0)
            {
                StringBuilder sbTemp = new StringBuilder();
                sb.Append("<ul class=\"nav nav-second-level collapse\">" );
                foreach (MenuInfo child in childList)
                {
                    List<MenuInfo> tempList = menuList.Where(p => p.PreID == child.MenuID).ToList();
                    if (tempList.Count > 0)
                    {
                        sb.AppendFormat("<li>");
                        sb.AppendFormat("<a href=\"javascript:;\">{0}</a>", child.NodeName);
                        sb.AppendFormat(GetNewSingelTreeHtml(child, menuList));
                        sb.AppendFormat("</li>");
                    }
                    else
                    {
                        sb.AppendFormat("<li{0}>", child.TargetBlank == 1 ? "" : string.Format(" data-rel=\"{0}\"", child.Url));
                        sb.AppendFormat("<a{1}>{0}</a>", child.NodeName, child.TargetBlank == 1 ? string.Format(" target=\"_blank\" href=\"{0}\"", child.Url) : " href=\"javascript:;\"");
                        sb.AppendFormat("</li>");
                    }
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 判断用户是否拥有指定页面路径权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckNewUserAndPath(string userId, int userType, string path)
        {
            string url = path.Contains("?") ? path.Substring(0, path.IndexOf("?")) : path;
            url = url.Contains("#") ? url.Substring(0, url.IndexOf("#")) : url;
            return GetUserPermission(userId, userType).Exists(p => p.Url.ToLower() == url.ToLower());
        }
        /// <summary>
        /// 用户的所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PermissionInfo> GetUserPermission(string userId, int userType)
        {
            if (userType == 1) return BaseCacheGetPermissionList();

            List<PermissionRelationInfo> userPermissionRelationList = BaseCacheGetPermissionRelationList(1, userId, null);
            List<UserPmsGroupRelationInfo> userPmsGroupRelationList = BaseCacheGetUserPmsGroupRelationList(userId);
            foreach (UserPmsGroupRelationInfo userPmsGroupRelation in userPmsGroupRelationList)
            {
                userPermissionRelationList.AddRange(BaseCacheGetPermissionRelationList(0, userPmsGroupRelation.GroupID.ToString(), null));
            }
            List<long> PmsIdList = userPermissionRelationList.Select(p => p.PermissionID).Distinct().ToList();
            return BaseCacheGetPermissionList().Where(p => PmsIdList.Contains(p.PermissionID)).ToList();
        }

        ///// <summary>
        ///// 获取用户所属权限组
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public long GetPmsGroupIDByUser(string userID)
        //{
        //    Model.UserInfo user = Get<Model.UserInfo>(string.Format(" UserID = '{0}' ", userID));

        //    if (user != null)
        //        if (user.PermissionGroupID != null)
        //            return user.PermissionGroupID.Value;

        //    return 0;
        //}

        /// <summary>
        /// 获取用户所属权限组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<long> GetPmsGroupIDByUser(string userId)
        {
            List<long> result = new List<long>();

            foreach (Model.UserPmsGroupRelationInfo item in BaseCacheGetUserPmsGroupRelationList(userId))
            {
                result.Add(item.GroupID);
            }
            return result;
        }

        /// <summary>
        /// 获取用户所属权限组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Model.PermissionGroupInfo> GetPmsGroupByUser(string userId)
        {
            List<Model.PermissionGroupInfo> result = new List<Model.PermissionGroupInfo>();

            List<long> pmsGroupIds = GetPmsGroupIDByUser(userId);

            if (pmsGroupIds.Count > 0)
            {
                result.AddRange(BaseCacheGetPermissionGroupList().Where(p => pmsGroupIds.Contains(p.GroupID)));
            }
            return result;
        }


        /// <summary>
        /// 设置一个用户所属的权限组
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool SetUserPmsGroup(string userId, long groupId)
        {
            Model.UserPmsGroupRelationInfo model = new Model.UserPmsGroupRelationInfo() { UserID = userId, GroupID = groupId };

            if (BaseCacheGetUserPmsGroupRelationList(userId).Exists(p => p.GroupID == groupId)) return false;

            return Add(model);
        }

        /// <summary>
        /// 批量设置用户权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupID">组ID列表</param>
        /// <param name="remoteOld">是否移除</param>
        /// <returns></returns>
        public int SetUserPmsGroup(string userId, List<long> groupIDList, bool remoteOld)
        {
            int result = 0;

            if (remoteOld)
                Delete(new Model.UserPmsGroupRelationInfo(), string.Format(" UserID = '{0}' ", userId));


            foreach (long groupID in groupIDList.Distinct())
            {
                if (SetUserPmsGroup(userId, groupID))
                    result++;
            }

            return result;
        }

        /// <summary>
        /// 设置一个权限所属的权限组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="pmsId"></param>
        /// <returns></returns>
        public bool SetPmsGroupPms(string groupId, long pmsId)
        {
            Model.PermissionRelationInfo model = new Model.PermissionRelationInfo() { RelationID = groupId, PermissionID = pmsId, RelationType = 0 };

            if (BaseCacheGetPermissionRelationList(0, groupId, null).Exists(p => p.PermissionID == pmsId)) return false;

            return Add(model);
        }

        /// <summary>
        /// 设置权限组权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="permissionIDList"></param>
        /// <param name="remoteOld"></param>
        /// <returns></returns>
        public int SetPmsGroupPms(long groupId, List<long> permissionIDList, bool remoteOld)
        {
            int result = 0;

            if (remoteOld)
                Delete(new Model.PermissionRelationInfo(), string.Format(" RelationID = '{0}' and RelationType = 0 ", groupId));


            foreach (long permissionID in permissionIDList.Distinct())
            {
                if (SetPmsGroupPms(groupId.ToString(), permissionID))
                    result++;
            }


            return result;
        }

        /// <summary>
        /// 获取指定权限组下的所有权限ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<long> GetPmsGroupPmsID(long groupId)
        {
            List<long> result = new List<long>();

            result = BaseCacheGetPermissionRelationList(0, groupId.ToString(), null).Select(p => p.PermissionID).ToList();

            return result;
        }

        /// <summary>
        /// 移除一个用户所属的权限组
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool RemoteUserPmsGroup(string userId, long groupId)
        {
            Model.UserPmsGroupRelationInfo model = new Model.UserPmsGroupRelationInfo() { UserID = userId, GroupID = groupId };
            if (!BaseCacheGetUserPmsGroupRelationList(userId).Exists(p => p.GroupID == groupId)) return false;
            if (Delete(model) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断用户是否拥有指定页面路径权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckUserAndPath(string userId, string path)
        {
            List<long> pmsIDList = GetPmsIDByUrlPath(path);//获取页面路径权限ID

            if (!pmsIDList.Count.Equals(0))
            {
                foreach (long pmsID in pmsIDList)
                {
                    if (CheckUserAndPms(userId, pmsID))
                        return true;
                }

            }

            return false;
        }
        /// <summary>
        /// 检查站点是否禁用权限
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="pmsId"></param>
        /// <returns></returns>
        public bool CheckWebsiteOwnerDisabled(string websiteOwner, long pmsId)
        {
            var result = BaseCacheGetPermissionRelationList(9, websiteOwner, pmsId.ToString());

            return result == null? false:result.Count() > 0;
        }
        /// <summary>
        /// 判断用户是否拥有指定权限ID权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pmsId"></param>
        /// <returns></returns>
        public bool CheckUserAndPms(string userId, long pmsId)
        {
            int result = BaseCacheGetPermissionRelationList(1, userId, null).Where(p => p.PermissionID == pmsId).Count();

            if (result > 0)
                return true;

            //long groupID = GetPmsGroupIDByUser(userID);

            //if (groupID > 0)
            //{
            //    return CheckGroupAndPms(groupID.ToString(), pmsID);
            //}

            foreach (long item in GetPmsGroupIDByUser(userId))
            {
                if (CheckGroupAndPms(item.ToString(), pmsId))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查用户和系统键值
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pkey"></param>
        /// <returns></returns>
        public bool CheckUserAndPmsKey(string userId, Enums.PermissionSysKey pkey,string websiteOwner="")
        {
            if (websiteOwner=="")
            {
                websiteOwner = WebsiteOwner;
            }
            UserInfo userInfo;
            if (string.IsNullOrEmpty(websiteOwner))
            {
                websiteOwner = WebsiteOwner;
            }
            if (userId == "jubit")
            {
                //userInfo = Get<UserInfo>(string.Format(" UserID='{0}' ", userId));
                return true;
            }
            else
            {
                userInfo = Get<UserInfo>(string.Format(" UserID='{0}' AND WebsiteOwner='{1}' ", userId, websiteOwner));
            }
            if (userInfo==null)
            {
                return false;
            }

            var key = CommonPlatform.Helper.EnumStringHelper.ToString(pkey);
            //先查出对应权限，不存在返回false
            var pms = GetPmsByPmsKey(key);
            if (pms == null) return false;
            //站点权限禁用时返回false
            if (CheckWebsiteOwnerDisabled(websiteOwner, pms.PermissionID)) return false;

            //判断是否是配置的管理员
            if (userInfo.PermissionGroupID.HasValue)
            {
                ZentCloud.BLLPermission.Model.PermissionGroupInfo perGroupInfo = Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format(" GroupID={0}", userInfo.PermissionGroupID));
                if (perGroupInfo != null && perGroupInfo.GroupType == 3)//管理员权限跟站点所有者一致
                {
                    userId = userInfo.WebsiteOwner;
                }
            }
            bool result = CheckUserAndPms(userId, pms.PermissionID);
            if (!result && userId == websiteOwner)
            {
                BLLPermission bllPer = new BLLPermission();
                List<PermissionColumn> column_list = bllPer.GetListByKey<PermissionColumn>("WebsiteOwner", websiteOwner);
                if (column_list.Count > 0)
                {
                    string columnIdStrs = MyStringHelper.ListToStr(column_list.Select(p => p.PermissionColumnID).ToList(), "'", ",");
                    List<PermissionRelationInfo> column_per_list = bllPer.GetMultPermissionRelationList(columnIdStrs, 2);
                    result = column_per_list.Exists(p => p.PermissionID == pms.PermissionID);
                }
            }
            return result;
        }

        /// <summary>
        /// 判断指定组是否拥有指定权限ID权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="pmsId"></param>
        /// <returns></returns>
        public bool CheckGroupAndPms(string groupId, long pmsId)
        {
            int result = BaseCacheGetPermissionRelationList(0, groupId, null).Where(p => p.PermissionID == pmsId).Count();

            if (result > 0)
                return true;

            //检查栏目内权限
            foreach (PermissionRelationInfo item in BaseCacheGetPermissionRelationList(3, groupId, null))
            {
                result = BaseCacheGetPermissionRelationList(2, item.PermissionID.ToString(), null).Where(p => p.PermissionID == pmsId).Count();
                if (result > 0)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 判断指定组是否拥有指定权限ID权限
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="pmsId"></param>
        /// <returns></returns>
        public bool CheckPerRelationByaccount(string userId, long pmsId)
        {
            int result = BaseCacheGetPermissionRelationList(1, userId, null).Where(p => p.PermissionID == pmsId).Count();

            if (result > 0)
                return true;

            return false;
        }
        /// <summary>
        /// 检查用户是否存在指定用户组里
        /// </summary>
        /// <param name="groupId">用户组</param>
        /// <param name="userId">用户</param>
        /// <returns></returns>
        public bool CheckGroupAndUser(long groupId, string userId)
        {
            return GetPmsGroupIDByUser(userId).Contains(groupId);
        }

        /// <summary>
        /// 根据页面路径查询获取权限ID
        /// </summary>
        /// <param name="path">页面路径</param>
        /// <returns></returns>
        public List<long> GetPmsIDByUrlPath(string path)
        {
            List<long> result = new List<long>();
            try
            {
                //List<Model.PermissionInfo> model =;

                foreach (PermissionInfo item in BaseCacheGetPermissionList().Where(p => p.Url.ToLower() == path.ToLower()))
                {
                    result.Add(item.PermissionID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool CopyBaseMenu(List<MenuInfo> menuList, string websiteOwner, int menuType, long preId, long newPreId, BLLTransaction tran)
        {
            List<MenuInfo> menus = menuList.Where(p => p.PreID == preId).ToList();
            if (menus.Count > 0)
            {
                foreach (var item in menus)
                {
                    MenuInfo Menu = new MenuInfo();
                    Menu.ICOCSS = item.ICOCSS;
                    Menu.MenuSort = item.MenuSort;
                    Menu.WebsiteOwner = websiteOwner;
                    Menu.PreID = newPreId;
                    Menu.ShowLevel = item.ShowLevel;
                    Menu.MenuType = menuType;
                    Menu.NodeName = item.NodeName;
                    Menu.Url = item.Url;
                    Menu.MenuID = Convert.ToInt64(GetGUID(Common.TransacType.MenuAdd));
                    if (!Add(Menu, tran))
                    {
                        return false;
                    }
                    else if (!CopyBaseMenu(menuList, websiteOwner, menuType, item.MenuID, Menu.MenuID, tran))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 缓存权限列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<PermissionInfo> BaseCacheGetPermissionList()
        {
            return GetList<PermissionInfo>();
        }
        /// <summary>
        /// 缓存权限列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        private List<PermissionGroupInfo> BaseCacheGetPermissionGroupList()
        {
            return GetList<PermissionGroupInfo>();
        }
        /// <summary>
        /// 缓存权限关系
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        private List<PermissionRelationInfo> BaseCacheGetPermissionRelationList(int relationType, string relationID, string permissionID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" RelationType={0} ", relationType);
            if (!string.IsNullOrWhiteSpace(relationID)) sql.AppendFormat(" AND RelationID ='{0}' ", relationID);
            if (!string.IsNullOrWhiteSpace(permissionID)) sql.AppendFormat(" AND PermissionID ='{0}' ", permissionID);
            return GetList<PermissionRelationInfo>(sql.ToString());
        }

        /// <summary>
        /// 缓存用户权限组
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<UserPmsGroupRelationInfo> BaseCacheGetUserPmsGroupRelationList(string userID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" UserID='{0}' ", userID);
            return GetList<UserPmsGroupRelationInfo>(sql.ToString());
        }
        /// <summary>
        /// 用户信息缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo BaseCacheGetUserInfo(string userId)
        {
            return Get<UserInfo>(string.Format(" UserID='{0}' ", userId));
        }

        public void UpdateChildShowLevel(long preId, int showLevel)
        {
            List<MenuInfo> childList = GetList<MenuInfo>(string.Format(" PreID={0} ", preId));
            foreach (var item in childList)
            {
                item.ShowLevel = showLevel;
                UpdateChildShowLevel(item.MenuID, showLevel);//修改子菜单
                Update(item);
            }
        }


        /// <summary>
        /// 判断用户是否拥有指定页面路径权限(新)
        /// </summary>
        /// <param name="userId">用户账号</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="path">路径</param>
        /// <param name="nAction">action</param>
        /// <returns></returns>
        public bool NewCheckUserAndPath(string userId, string websiteOwner, string path, string nAction)
        {
            BLLPermission bllPer = new BLLPermission();
            List<PermissionInfo> pmsList = bllPer.GetPermissionListByPath(path);//获取页面路径权限ID
            if (!string.IsNullOrWhiteSpace(nAction))
            {
                pmsList = pmsList.Where(p => string.IsNullOrWhiteSpace(p.PermissionAction) || p.PermissionAction == nAction).ToList();
            }
            if (pmsList.Count == 0) return false;
            List<long> pmsIDList = pmsList.Select(p => p.PermissionID).ToList();
            List<PermissionRelationInfo> rel_column_list = new List<PermissionRelationInfo>();
            List<PermissionColumn> column_list = new List<PermissionColumn>();
            List<long> pmsGroupIdList = GetPmsGroupIDByUser(userId);
            string groupIds = "''";
            if (pmsGroupIdList.Count > 0)
            {
                groupIds = MyStringHelper.ListToStr(pmsGroupIdList, "'", ",");
                rel_column_list = bllPer.GetMultPermissionRelationList(groupIds, 3);
                if(rel_column_list.Count > 0){
                    string columnRelationIds = MyStringHelper.ListToStr(rel_column_list.Select(p => p.PermissionID).ToList(), "", ",");
                    column_list = bllPer.GetMultListByKey<PermissionColumn>("PermissionColumnID", columnRelationIds);
                }
            } 
            if (!string.IsNullOrWhiteSpace(websiteOwner) && userId == websiteOwner)
            {
                List<PermissionColumn> column_list1 = bllPer.GetListByKey<PermissionColumn>("WebsiteOwner", websiteOwner);
                column_list.AddRange(column_list1);
            }
            if (column_list.Count == 0) return false;

            List<long> columnId_list = column_list.Select(p => p.PermissionColumnID).ToList();
            List<long> columnId_list1 = column_list.Where(p => p.PermissionColumnBaseID > 0).Select(p => p.PermissionColumnBaseID).ToList();
            columnId_list.AddRange(columnId_list1);
            string columnIdStrs = MyStringHelper.ListToStr(columnId_list, "'", ",");
            List<PermissionRelationInfo> column_per_list = bllPer.GetMultPermissionRelationList(columnIdStrs, 2);

            List<long> npmsIDList = column_per_list.Select(p => p.PermissionID).ToList();

            foreach (long pmsID in pmsIDList)
            {
                if (npmsIDList.Contains(pmsID)) return true;
            }
            return false;
        }
        /// <summary>
        /// 检查用户是否有权限组
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public bool CheckUserHasGroupIdInGroupIds(string userId, string groupIds)
        {
            if (string.IsNullOrWhiteSpace(groupIds)) return false;
            List<UserPmsGroupRelationInfo> list = GetListByKey<UserPmsGroupRelationInfo>("UserID", userId);
            List<string> userGroupIdList = list.Select(p => p.GroupID.ToString()).ToList();
            List<string> groupIdList = groupIds.Split(',').ToList();
            return userGroupIdList.Intersect(groupIdList).Count()>0;
        }
        public bool CheckUserHasGroupIdInGroupIds(UserInfo user, string groupIds)
        {
            if (user == null || user.AutoID == 0) return false;
            return CheckUserHasGroupIdInGroupIds(user.UserID, groupIds);
        }

    }
}

