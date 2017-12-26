using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 企业微网站 Bll
    /// </summary>
    public class BLLCompanyWebSite : BLL
    {
        //获取ToolBar使用分类
        public string GetToolBarUseTypeName(string useType)
        {
            string result = "";
            switch (useType)
            {
                case"foottool":
                    result = "底部导航";
                    break;
                case "tab":
                    result = "选项卡导航";
                    break;
                case "button":
                    result = "按钮导航";
                    break;
                case "headtool":
                    result = "顶部导航";
                    break;
                case "nav":
                    result = "商品导航";
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// 工具栏类型
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<string> GetToolBarUseTypeList(string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            List<CompanyWebsite_ToolBar> dataList = GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,UseType");
            return dataList.OrderBy(p => p.UseType).Select(p => p.UseType).Distinct().ToList();
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="useType"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        public List<CompanyWebsite_ToolBar> GetToolBarList(int rows, int page, string websiteOwner, string useType, string keyType, bool showAll, int? userType = null, bool? isSystem = null)
        {
            return GetLit<CompanyWebsite_ToolBar>(rows, page, GetWhereString(websiteOwner, useType, keyType,null,showAll,userType, isSystem), " PlayIndex ASC");
        }
        /// <summary>
        /// 查询导航分组
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="useType"></param>
        /// <returns></returns>
        public List<string> GetKeyTypeList(string websiteOwner, string useType){

            List<CompanyWebsite_ToolBar> list = GetToolBarList(int.MaxValue, 1, websiteOwner, useType, null, false);
            return list.OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();
        }
        /// <summary>
        /// 查询菜单数量
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="useType"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        public int GetToolBarCount(string websiteOwner, string useType, string keyType)
        {
            return GetCount<CompanyWebsite_ToolBar>(GetWhereString(websiteOwner, useType, keyType));
        }

        /// <summary>
        /// 查询所有子导航
        /// </summary>
        /// <param name="preID"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<int> GetChildToolBarIDList(int preID, string websiteOwner)
        {
            List<CompanyWebsite_ToolBar> list = GetLit<CompanyWebsite_ToolBar>(int.MaxValue, 1, GetWhereString(websiteOwner, null, null, preID));
            List<int> result = new List<int>();
            if(list.Count>0){
                result.AddRange(list.Select(p => p.AutoID).ToList());
                foreach (var item in list)
                {
                    result.AddRange(GetChildToolBarIDList(item.AutoID, websiteOwner));
                }
            }
            return result;
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="useType"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        private string GetWhereString(string websiteOwner, string useType, string keyType, int? preID = null, bool? showAll = true, int? userType = null, bool? isSystem = null)
        {
            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            }
            else
            {
                sbWhere.AppendFormat(" WebsiteOwner Is Null ", websiteOwner);
            }
            if (!string.IsNullOrWhiteSpace(useType)) sbWhere.AppendFormat(" AND UseType='{0}'", useType);
            if (!string.IsNullOrWhiteSpace(keyType)) sbWhere.AppendFormat(" AND KeyType='{0}'", keyType);
            if (preID.HasValue) sbWhere.AppendFormat(" AND PreID='{0}'", preID);
            if (!showAll.HasValue || !showAll.Value) sbWhere.AppendFormat(" AND IsShow='1'");
            if (isSystem.HasValue){
                if(isSystem.Value){
                    sbWhere.AppendFormat(" AND IsSystem=1 ");
                }
                else{
                    sbWhere.AppendFormat(" AND IsSystem=0 ");
                }
            }
            if (userType.HasValue && userType.Value != 1)
            {
                sbWhere.AppendFormat(" AND ( [IsSystem]=0 OR [PermissionGroup]='' OR EXISTS( SELECT 1 FROM ZCJ_UserPmsGroupRelationInfo WHERE UserID='{0}' and  ','+[PermissionGroup]+',' like  '%,'+ convert(varchar,GroupID)+',%')) ", websiteOwner); 
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 获取单个底部工具栏 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyWebsite_ToolBar GetCompanyWebsiteToolBarById(string id)
        {
            return Get<CompanyWebsite_ToolBar>(string.Format("AutoID={0}", id));
        }
        /// <summary>
        /// 获取通用导航
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        public List<CompanyWebsite_ToolBar> GetCommonToolBarList(string websiteOwner, string keyType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" (WebsiteOwner='{0}' Or WebsiteOwner Is Null) ", websiteOwner);
            sbWhere.AppendFormat(" And KeyType = '{0}' ", keyType);
            List<CompanyWebsite_ToolBar> dataListTemp = GetList<CompanyWebsite_ToolBar>(sbWhere.ToString());
            List<CompanyWebsite_ToolBar> dataList = dataListTemp.Where(p => p.IsSystem == 0 && p.WebsiteOwner==websiteOwner).ToList();
            List<int> nList = dataList.Select(p => p.BaseID).Distinct().ToList();
            foreach (CompanyWebsite_ToolBar item in dataListTemp.Where(p => p.IsSystem == 1 && !nList.Contains(p.AutoID))) {
                dataList.Add(item);
            }
            dataList = dataList.Where(p => p.IsShow.Equals("1")).OrderBy(p => p.PlayIndex).ToList();
            return dataList;
        }
        /// <summary>
        /// 检查导航权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="toolBar"></param>
        /// <returns></returns>
        public bool CheckHasPms(UserInfo user, CompanyWebsite_ToolBar toolBar)
        {
            BLLUser bllUser = new BLLUser();
            if (toolBar.VisibleSet == 1 && !bllUser.IsDistributionMember(user)) return false;

            if (toolBar.VisibleSet == 2)
            {
                if (user == null || user.AutoID == 0) return false;
                BLLPermission.BLLMenuPermission bllMpms = new BLLPermission.BLLMenuPermission("");
                if (!bllMpms.CheckUserHasGroupIdInGroupIds(user.UserID, toolBar.PermissionGroup)) return false;
            }
            return true;
        }
    }
}
