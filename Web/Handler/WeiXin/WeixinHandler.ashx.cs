using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using System.Text;
using ZentCloud.BLLJIMP.Model.Weixin;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Handler.WeiXin
{
    /// <summary>
    /// 微信处理文件
    /// </summary>
    public class WeixinHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin("");
        /// <summary>
        /// 当前用户
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            string result = "false";
            try
            {
                this.currentUserInfo = bllWeixin.GetCurrentUserInfo();
                context.Response.ContentType = "text/plain";
                context.Response.Expires = 0;
                string action = context.Request["Action"];
                switch (action)
                {
                    case "AddWeixinMenu":
                        result = AddWeixinMenu(context);//添加微信菜单
                        break;
                    case "EditWeixinMenu":
                        result = EditWeixinMenu(context);//编辑微信菜单
                        break;
                    case "DeleteWeixinMenu":
                        result = DeleteWeixinMenu(context);//删除微信菜单
                        break;
                    case "QueryWeixinMenu":
                        result = QueryWeixinMenu(context);//获取微信菜单
                        break;
                    case "GetMenuSelectList"://获取微信自定义菜单
                        result = GetMenuSelectList(context);
                        break;

                    case "CreateWeixinClientMenu":
                        result = CreateWeixinClientMenu();//生成微信客户端菜单
                        break;

                    case "MoveMenu":
                        result = MoveMenu(context);//调整菜单顺序
                        break;

                    case "QueryJuActivity":
                        result = QueryJuActivity(context);//查询聚活动数据
                        break;

                    case "AddJuActivity":
                        result = AddJuActivity(context);//添加聚活动
                        break;
                    case "GetSingelJuActivity":
                        result = GetSingelJuActivity(context);//获取单条聚活动
                        break;
                    case "EditJuActivity":
                        result = EditJuActivity(context);//编辑聚活动
                        break;
                    case "DeleteJuActivity":
                        result = DeleteJuActivity(context);//删除聚活动
                        break;

                    case "QueryWXMember":
                        result = QueryWXMember(context);//查询会员注册数据
                        break;
                    case "EditWXMember":
                        result = EditWXMember(context);//编辑会员注册数据
                        break;
                    case "DeleteWXMember":
                        result = DeleteWXMember(context);//删除会员注册数据
                        break;
                    case "SetWXLogoImage":
                        result = SetWXLogoImage(context);//设置微信公众号Logo
                        break;
                    //case "SynchronousAllFollowers":
                    //    result = SynchronousAllFollowers();
                    //    break;

                }

            }
            catch (Exception ex)
            {

                result = ex.Message;
            }
            context.Response.Write(result);

        }

        /// <summary>
        /// 删除聚活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteJuActivity(HttpContext context)
        {
            string ids = context.Request["ids"];
            int result = bllWeixin.Delete(new JuActivityInfo(), string.Format(" JuActivityID in ({0})", ids));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了{0}个活动", result);
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取单条活动信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSingelJuActivity(HttpContext context)
        {
            int juActivityId = Convert.ToInt32(context.Request["JuActivityID"]);
            JuActivityInfo model = bllWeixin.Get<JuActivityInfo>("JuActivityID = " + juActivityId.ToString());

            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "活动不存在";
            }
            else
            {
                resp.Status = 1;
                resp.ExObj = model;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 编辑活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditJuActivity(HttpContext context)
        {
            int juActivityId = Convert.ToInt32(context.Request["JuActivityID"]);
            JuActivityInfo model = this.bllWeixin.Get<JuActivityInfo>("JuActivityID = " + juActivityId.ToString());

            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "活动不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            model.ActivityName = context.Request["ActivityName"];
            string activityStartDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityStartDate))
            {
                model.ActivityStartDate = DateTime.Parse(activityStartDate);
            }
            string activityEndDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityEndDate))
            {
                model.ActivityEndDate = DateTime.Parse(activityEndDate);
            }


            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];
            model.IsSignUpJubit = int.Parse(context.Request["IsSignUpJubit"]);
            model.SignUpActivityID = context.Request["SignUpActivityID"];
            model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            model.Sort = Convert.ToInt32(context.Request["Sort"]);
            model.IsFee = Convert.ToInt32(context.Request["IsFee"]);

            model.TopImgPath = context.Request["TopImgPath"];

            if (this.bllWeixin.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败!";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        //private string GetJuActivity(HttpContext context)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddJuActivity(HttpContext context)
        {
            JuActivityInfo model = new JuActivityInfo();
            model.UserID = this.currentUserInfo.UserID;
            model.JuActivityID = int.Parse(this.bllWeixin.GetGUID(BLLJIMP.TransacType.ActivityAdd));
            model.ActivityName = context.Request["ActivityName"];
            string activityStartDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityStartDate))
            {
                model.ActivityStartDate = DateTime.Parse(activityStartDate);
            }
            string activityEndDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityEndDate))
            {
                model.ActivityEndDate = DateTime.Parse(activityEndDate);
            }
            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];
            model.IsSignUpJubit = int.Parse(context.Request["IsSignUpJubit"]);
            model.SignUpActivityID = context.Request["SignUpActivityID"];
            model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            model.Sort = Convert.ToInt32(context.Request["Sort"]);
            model.IsFee = Convert.ToInt32(context.Request["IsFee"]);

            model.TopImgPath = context.Request["TopImgPath"];

            model.CreateDate = DateTime.Now;
            if (this.bllWeixin.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败!";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        private string QueryJuActivity(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            StringBuilder sbWhere = new StringBuilder();

            List<JuActivityInfo> dataList = this.bllWeixin.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), "Sort DESC,ActivityStartDate DESC");

            int totalCount = this.bllWeixin.GetCount<JuActivityInfo>(sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = dataList
     });
        }

        /// <summary>
        /// 获取菜单选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMenuSelectList(HttpContext context)
        {
            string result = string.Empty;
            result = new MySpider.MyCategories().GetSelectOptionHtml(bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}'", currentUserInfo.UserID)), "MenuID", "PreID", "NodeName", 0, "ddlPreMenu", "width:200px", "");
            return result.ToString();
        }

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWeixinMenu(HttpContext context)
        {


            string ids = context.Request["ids"];
            int result = bllWeixin.Delete(new WeixinMenu(), string.Format(" MenuID in ({0}) And UserID='{1}'", ids, currentUserInfo.UserID));
            result += bllWeixin.Delete(new WeixinMenu(), string.Format(" PreID in ({0}) And UserID='{1}'", ids, currentUserInfo.UserID));
            return result.ToString();

        }
        /// <summary>
        /// 添加微信自定义菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWeixinMenu(HttpContext context)
        {

            string jsonData = context.Request["JsonData"];
            WeixinMenu menuInfo = ZentCloud.Common.JSONHelper.JsonToModel<WeixinMenu>(jsonData);
            if (menuInfo.PreID == 0)//添加的是一级菜单
            {
                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID=0", currentUserInfo.UserID)) >= 3)
                {
                    return "最多可以添加3个一级菜单";
                }
            }
            else//添加是二级菜单
            {
                //var parentmenu = weixinBll.Get<WeixinMenu>(string.Format("MenuID='{0}'",menuInfo.PreID));
                var topMenu = bllWeixin.Get<WeixinMenu>(string.Format("MenuID='{0}'", menuInfo.PreID));
                if (topMenu != null)
                {
                    if (topMenu.PreID != 0)
                    {
                        return "只能添加二级菜单";
                    }

                }

                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID='{1}'", currentUserInfo.UserID, menuInfo.PreID)) >= 5)
                {
                    return "最多可以添加5个二级菜单";
                }

            }

            if (menuInfo.PreID == 0)//添加一级菜单
            {
                List<WeixinMenu> firstLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' And PreID=0", currentUserInfo.UserID)).OrderBy(p => p.MenuSort).ToList();
                if (firstLevelMenu.Count == 0)
                {
                    menuInfo.MenuSort = 1;
                }
                else
                {
                    menuInfo.MenuSort = firstLevelMenu[firstLevelMenu.Count - 1].MenuSort + 1;
                }


            }
            else//添加二级菜单
            {
                List<WeixinMenu> secondLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' And PreID={1}", currentUserInfo.UserID, menuInfo.PreID)).OrderBy(p => p.MenuSort).ToList();
                if (secondLevelMenu.Count == 0)
                {
                    menuInfo.MenuSort = 1;
                }
                else
                {
                    menuInfo.MenuSort = secondLevelMenu[secondLevelMenu.Count - 1].MenuSort + 1;
                }

            }

            menuInfo.MenuID = long.Parse(bllWeixin.GetGUID(ZentCloud.BLLJIMP.TransacType.WeixinMenuAdd));
            menuInfo.UserID = currentUserInfo.UserID;
            bool result = bllWeixin.Add(menuInfo);
            return result.ToString().ToLower();
        }

        private string EditWeixinMenu(HttpContext context)
        {


            string jsonData = context.Request["JsonData"];

            WeixinMenu menuInfo = ZentCloud.Common.JSONHelper.JsonToModel<WeixinMenu>(jsonData);

            WeixinMenu oldMenuInfo = bllWeixin.Get<WeixinMenu>(string.Format("MenuID={0} And UserID='{1}'", menuInfo.MenuID, currentUserInfo.UserID));
            if (menuInfo.PreID == 0)//上级是顶级菜单
            {
                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID=0", currentUserInfo.UserID)) >= 3 && (oldMenuInfo.PreID != 0) && (oldMenuInfo.PreID != menuInfo.PreID))
                {
                    return "一级菜单最多只能设置3个";
                }



            }
            else
            {


                var topMenu = bllWeixin.Get<WeixinMenu>(string.Format("MenuID='{0}'", menuInfo.PreID));
                if (topMenu != null)
                {
                    if (topMenu.PreID != 0)
                    {
                        return "只能设置二级菜单";
                    }

                }

                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID='{1}'", currentUserInfo.UserID, menuInfo.PreID)) >= 5 && (oldMenuInfo.PreID != menuInfo.PreID))
                {
                    return "最多可以设置5个二级菜单";
                }

                WeixinMenu lastSecondMenu = bllWeixin.Get<WeixinMenu>(string.Format("PreID='{0}' order by MenuSort DESC", menuInfo.PreID));
                if (lastSecondMenu != null)
                {
                    menuInfo.MenuSort = oldMenuInfo.MenuSort;
                }
                else
                {
                    menuInfo.MenuSort = 1;
                }


            }

            bool result = bllWeixin.Update(menuInfo);
            return result.ToString().ToLower();
        }

        /// <summary>
        /// 获取微信菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWeixinMenu(HttpContext context)
        {
            List<WeixinMenu> list;

            list = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}'", currentUserInfo.UserID));
            list = list.OrderBy(p => p.MenuSort).ToList();
            List<WeixinMenu> showList = new List<WeixinMenu>();

            MySpider.MyCategories m = new MySpider.MyCategories();

            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<WeixinMenu>("MenuID", "PreID", "NodeName", list), 0))
            {
                try
                {
                    WeixinMenu tmpModel = list.Where(p => p.MenuID.ToString().Equals(item.Value)).ToList()[0];
                    tmpModel.NodeName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }

            int totalCount = showList.Count;

            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = showList
                });
        }

        /// <summary>
        /// 生成微信客户端菜单
        /// </summary>
        /// <returns></returns>
        private string CreateWeixinClientMenu()
        {
            //if (currentUserInfo.WeixinIsEnableMenu == null || currentUserInfo.WeixinIsEnableMenu == 0)
            //{
            //    return "请先在 微信平台-公众号配置  页面中启用自定义菜单。";
            //}
            //if (string.IsNullOrEmpty(currentUserInfo.WeixinAppId) || string.IsNullOrEmpty(currentUserInfo.WeixinAppSecret))
            //{
            //    return "请先在 微信平台-公众号配置 页面中填写 AppId与 AppSecret";
            //}

            //

            //获取AccessToken
            string accessToken = bllWeixin.GetAccessToken();
            if (accessToken == string.Empty)
            {
                return "AppId 或AppSecret 不正确，请在 微信公众号-公众号接口配置 页面中检查";

            }
            //获取AccessToken

            List<WeixinMenu> lstFirstLevel = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID=0", currentUserInfo.UserID)).OrderBy(p => p.MenuSort).ToList();


            //构造菜单字符串
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("{\"button\":[");
            for (int i = 0; i < lstFirstLevel.Count; i++)
            {
                List<WeixinMenu> lstSendcondLevel = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID={1}", currentUserInfo.UserID, lstFirstLevel[i].MenuID)).OrderByDescending(p => p.MenuSort).ToList();
                sbMenu.Append("{");
                if (lstSendcondLevel.Count == 0)//无子菜单
                {

                    sbMenu.AppendFormat("\"type\":\"{0}\",", lstFirstLevel[i].Type.Trim());
                    sbMenu.AppendFormat("\"name\":\"{0}\",", lstFirstLevel[i].NodeName);
                    if (lstFirstLevel[i].Type.Trim().Equals("click"))
                    {
                        sbMenu.AppendFormat("\"key\":\"{0}\"", lstFirstLevel[i].KeyOrUrl);
                    }
                    else
                    {
                        sbMenu.AppendFormat("\"url\":\"{0}\"", lstFirstLevel[i].KeyOrUrl);
                    }

                }
                else//有子菜单
                {
                    sbMenu.AppendFormat("\"name\":\"{0}\",", lstFirstLevel[i].NodeName);
                    sbMenu.Append("\"sub_button\":[");

                    for (int j = 0; j < lstSendcondLevel.Count; j++)
                    {
                        sbMenu.Append("{");

                        sbMenu.AppendFormat("\"type\":\"{0}\",", lstSendcondLevel[j].Type.Trim());
                        sbMenu.AppendFormat("\"name\":\"{0}\",", lstSendcondLevel[j].NodeName);
                        if (lstSendcondLevel[j].Type.Trim().Equals("click"))
                        {
                            sbMenu.AppendFormat("\"key\":\"{0}\"", lstSendcondLevel[j].KeyOrUrl);
                        }
                        else
                        {
                            sbMenu.AppendFormat("\"url\":\"{0}\"", lstSendcondLevel[j].KeyOrUrl);
                        }

                        sbMenu.Append("}");
                        if (j < lstSendcondLevel.Count - 1)
                        {
                            sbMenu.Append(",");
                        }
                    }
                    sbMenu.Append("]");


                }
                sbMenu.Append("}");

                if (i < lstFirstLevel.Count - 1)
                {
                    sbMenu.Append(",");
                }

            }
            sbMenu.Append("]}");

            //构造菜单字符串

            WeixinAccessToken result = bllWeixin.CreateWeixinClientMenu(accessToken, sbMenu.ToString());
            return bllWeixin.GetCodeMessage(result.errcode);



        }

        /// <summary>
        /// 菜单排序
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MoveMenu(HttpContext context)
        {

            int menuID = int.Parse(context.Request["MenuID"]);//步骤ID
            string direction = context.Request["Direction"];//移动方向 up:上 down: 下
            WeixinMenu targetMenu = bllWeixin.Get<WeixinMenu>(string.Format("MenuID={0}", menuID));//要移动的菜单
            int index = 0;//菜单所在同级的顺序
            #region 移动一级菜单

            if (targetMenu.PreID.ToString().Equals("0"))//移动的是一级菜单
            {
                List<WeixinMenu> firstLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID=0", currentUserInfo.UserID));//一级菜单
                firstLevelMenu = firstLevelMenu.OrderBy(p => p.MenuSort).ToList();


                //修改menusort
                for (int i = 0; i < firstLevelMenu.Count; i++)
                {
                    if (firstLevelMenu[i].MenuID == targetMenu.MenuID)
                    {
                        index = i;
                        break;
                    }
                }
                int tagetMenuSort = (int)firstLevelMenu[index].MenuSort;
                if (direction.Equals("up"))//一级菜单上移
                {

                    if (index == 0)//一级菜单已经最靠前
                    {
                        return "选中菜单已经排最前";
                    }
                    else//一级菜单不是最靠前
                    {
                        //交换排序
                        int preMenuSort = (int)firstLevelMenu[index - 1].MenuSort;//上一条一级菜单排序
                        firstLevelMenu[index].MenuSort = preMenuSort;
                        firstLevelMenu[index - 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(firstLevelMenu[index]) && bllWeixin.Update(firstLevelMenu[index - 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }


                    }

                }
                else//一级菜单下移
                {
                    if (firstLevelMenu[firstLevelMenu.Count - 1].MenuID == targetMenu.MenuID)//一级菜单已经最靠后
                    {
                        return "选中菜单已经排最后";
                    }
                    else//一级菜单不是最靠后
                    {
                        //交换排序

                        int nextMenuSort = (int)firstLevelMenu[index + 1].MenuSort;//下一条一级菜单排序

                        firstLevelMenu[index].MenuSort = nextMenuSort;
                        firstLevelMenu[index + 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(firstLevelMenu[index]) && bllWeixin.Update(firstLevelMenu[index + 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }

                    }

                }
            }
            #endregion

            #region 移动二级菜单
            else//移动的是二级菜单
            {
                List<WeixinMenu> secondLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID={1}", currentUserInfo.UserID, targetMenu.PreID));//二级菜单
                secondLevelMenu = secondLevelMenu.OrderBy(p => p.MenuSort).ToList();

                //修改menusort
                for (int i = 0; i < secondLevelMenu.Count; i++)
                {
                    if (secondLevelMenu[i].MenuID == targetMenu.MenuID)
                    {
                        index = i;
                        break;
                    }
                }
                int tagetMenuSort = (int)secondLevelMenu[index].MenuSort;
                if (direction.Equals("up"))//二级菜单上移
                {

                    if (index == 0)//二级菜单已经最靠前
                    {
                        return "选中菜单已经排最前";
                    }
                    else//二级菜单不是最靠前
                    {
                        //交换排序
                        int preMenuSort = (int)secondLevelMenu[index - 1].MenuSort;//上一条二级菜单排序
                        secondLevelMenu[index].MenuSort = preMenuSort;
                        secondLevelMenu[index - 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(secondLevelMenu[index]) && bllWeixin.Update(secondLevelMenu[index - 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }


                    }

                }
                else//二级菜单下移
                {
                    if (secondLevelMenu[secondLevelMenu.Count - 1].MenuID == targetMenu.MenuID)//一级菜单已经最靠后
                    {
                        return "选中菜单已经排最后";
                    }
                    else//二级菜单不是最靠后
                    {
                        //交换排序

                        int nextMenuSort = (int)secondLevelMenu[index + 1].MenuSort;//下一条二级菜单排序

                        secondLevelMenu[index].MenuSort = nextMenuSort;
                        secondLevelMenu[index + 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(secondLevelMenu[index]) && bllWeixin.Update(secondLevelMenu[index + 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }

                    }

                }
            }
            #endregion


        }
        /// <summary>
        /// //查询会员注册数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private string QueryWXMember(HttpContext context)
        {


            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserID"];
            string name = context.Request["Name"];
            string openId = context.Request["OpenID"];
            string phone = context.Request["Phone"];

            StringBuilder strWhere = new StringBuilder(" 1=1");
            if (!currentUserInfo.UserType.Equals(1))
            {
                strWhere.AppendFormat(" And UserID ='{0}'", currentUserInfo.UserID);

            }
            if (!string.IsNullOrEmpty(userId))
            {
                strWhere.AppendFormat(" And UserID ='{0}'", userId);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                strWhere.AppendFormat(" And Name like '%{0}%'", name);
            }
            if (!string.IsNullOrWhiteSpace(openId))
            {
                strWhere.AppendFormat(" And WeixinOpenID like '%{0}%'", openId);
            }
            if (!string.IsNullOrEmpty(phone))
            {
                strWhere.AppendFormat(" And Phone ='{0}'", phone);
            }

            List<WXMemberInfo> dataList = this.bllWeixin.GetLit<WXMemberInfo>(pageSize, pageIndex, strWhere.ToString(), "MemberID DESC");
            int totalCount = this.bllWeixin.GetCount<WXMemberInfo>(strWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = dataList
                });


        }

        /// <summary>
        /// 删除微信注册会员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMember(HttpContext context)
        {

            string memberIds = context.Request["ids"];
            int count = bllWeixin.Delete(new WXMemberInfo(), string.Format(" MemberID in ({0})", memberIds));
            if (count == memberIds.Split(',').Length)
            {
                resp.Status = 1;
                resp.Msg = "删除成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败!";

            }
            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 编辑微信会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMember(HttpContext context)
        {

            string id = context.Request["id"];
            WXMemberInfo model = bllWeixin.Get<WXMemberInfo>(string.Format("MemberID={0}", id));
            model.Name = context.Request["Name"];
            model.Phone = context.Request["Phone"];
            model.Email = context.Request["Email"];
            model.Company = context.Request["Company"];
            model.Postion = context.Request["Postion"];
            if (bllWeixin.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "编辑成功!";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "编辑失败!";

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 设置公众号Logo
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetWXLogoImage(HttpContext context)
        {
            string filePath = context.Request["FilePath"];
            currentUserInfo.WXLogoImg = filePath;
            if (bllWeixin.Update(currentUserInfo, string.Format(" WXLogoImg='{0}'", currentUserInfo.WXLogoImg), string.Format(" AutoID={0}", currentUserInfo.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "设置公众号Logo图片成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "设置公众号Logo失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);


        }
        ///// <summary>
        ///// 同步微信粉丝信息
        ///// </summary>
        ///// <returns></returns>
        //public string SynchronousAllFollowers()
        //{

        //    return new BLLWeixin("").SynchronousAllFollowers(currentUserInfo.UserID, currentUserInfo.WeixinAppId, currentUserInfo.WeixinAppSecret).ToString();



        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}