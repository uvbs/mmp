using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Text;
namespace ZentCloud.JubitIMP.Web.Handler.WanBang
{
    /// <summary>
    /// WapHandler 的摘要说明
    /// </summary>
    public class Wap : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll=new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                 string action = context.Request["Action"];
                switch (action)
                {

                    case "Login"://登录
                        result = Login(context);
                        break;
                    case "GetBaseList"://获取基地列表
                        result = GetBaseList(context);
                        break;
                    case "GetCompanyList"://获取企业列表
                        result = GetCompanyList(context);
                        break;
                    case "GetProjectList"://获取项目列表
                        result = GetProjectList(context);
                        break;
                    case "GetJointProjectList"://获取对接项目列表
                        result = GetJointProjectList(context);
                        break;

                    case "UpdateBaseInfo"://更新基地资料
                        result = UpdateBaseInfo(context);
                        break;

                    case "UpdateCompanyInfo"://更新企业资料
                        result = UpdateCompanyInfo(context);
                        break;

                    case "GetMyProjectList"://获取我发布的项目列表
                        result = GetMyProjectList(context);
                        break;

                    case "AddAttentionInfo"://关注
                        result = AddAttentionInfo(context);
                        break;

                    case "CancelAttention"://取消关注
                        result = CancelAttention(context);
                        break;

                    case "AddProjectInfo"://发布项目
                        result = AddProjectInfo(context);
                        break;

                    case "EditProjectInfo"://更新项目
                        result = EditProjectInfo(context);
                        break;
                    case "EndProject"://结束项目
                        result = EndProject(context);
                        break;
                        

                    case "GetAttentionBaseList"://获取关注的基地列表
                        result = GetAttentionBaseList(context);
                        break;

                    case "GetAttentionCompanyList":///获取关注的企业列表
                        result = GetAttentionCompanyList(context);
                        break;

                    case "GetAttentionProjectList":///获取关注的项目列表
                        result = GetAttentionProjectList(context);
                        break;



                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }

            context.Response.Write(result);
        }
        /// <summary>
        /// 基地 企业登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Login(HttpContext context)
        {
            int userType = int.Parse(context.Request["UserType"]);
            string userName = context.Request["UserName"];
            string passWord = context.Request["Password"];
            switch (userType)
            {
                case 0://基地登录
                    var baseInfo = bll.Get<WBBaseInfo>(string.Format("UserId='{0}' And Password='{1}'", userName, passWord));
                    if (baseInfo == null)
                    {
                        resp.Msg = "用户名或密码错误";

                    }
                    else if (baseInfo.IsDisable.Equals(1))
                    {
                        resp.Msg = "您的账户已经被禁用,请联系管理员";
                    }
                    else if (baseInfo != null && baseInfo.IsDisable.Equals(0))
                    {
                        resp.Status = 1;
                        context.Session[SessionKey.WanBangUserID] = baseInfo.UserId;
                        context.Session[SessionKey.WanBangUserType] = userType;
                        resp.ExObj = baseInfo;
                    }
                    break;
                case 1://企业登录
                    var companyInfo = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}' And Password='{1}'", userName, passWord));
                    if (companyInfo == null)
                    {
                        resp.Msg = "用户名或密码错误";

                    }
                    else if (companyInfo.IsDisable.Equals(1))
                    {
                        resp.Msg = "您的账户已经被禁用,请联系管理员";
                    }
                    else if (companyInfo != null && companyInfo.IsDisable.Equals(0))
                    {
                        resp.Status = 1;
                        context.Session[SessionKey.WanBangUserID] = companyInfo.UserId;
                        context.Session[SessionKey.WanBangUserType] = userType;
                        resp.ExObj = companyInfo;
                    }
                    break;

            }

            return Common.JSONHelper.ObjectToJson(resp);




        }

        /// <summary>
        /// 获取基地列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetBaseList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string baseName = context.Request["BaseName"];
            string area = context.Request["Area"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" IsDisable=0 And WebsiteOwner='{0}'",bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(baseName))
            {
                sbWhere.AppendFormat(" And BaseName like '%{0}%'", baseName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area='{0}'", area);
            }

            totalCount = bll.GetCount<WBBaseInfo>(sbWhere.ToString());
            List<WBBaseInfo> data = bll.GetLit<WBBaseInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            for (int i = 0; i < data.Count; i++)
            {
                data[i].UserId = null;
                data[i].Password = null;
                data[i].Tel = null;
                data[i].Phone = null;
                data[i].QQ = null;
                //data[i].Acreage = null;
                data[i].Introduction = null;


            }
            resp.ExObj = data;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 获取企业列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetCompanyList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string companyName = context.Request["CompanyName"];
            string area = context.Request["Area"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" IsDisable=0 And WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(companyName))
            {
                sbWhere.AppendFormat(" And CompanyName like '%{0}%'", companyName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area='{0}'", area);
            }

            totalCount = bll.GetCount<WBCompanyInfo>(sbWhere.ToString());
            List<WBCompanyInfo> data = bll.GetLit<WBCompanyInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID ASC");
            for (int i = 0; i < data.Count; i++)
            {
                data[i].UserId = null;
                data[i].Password = null;
                data[i].Tel = null;
                data[i].Phone = null;
                data[i].QQ = null;
                //data[i].BusinessLicenseNumber = null;
                data[i].Introduction = null;

            }
            resp.ExObj = data;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }

       /// <summary>
        /// 获取对接项目列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetJointProjectList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string projectName = context.Request["Name"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(projectName))
            {
                sbWhere.AppendFormat(" And (ProjectName like '%{0}%' Or BaseName like '%{0}%' Or CompanyName like '%{0}%')", projectName);
            }
            totalCount = bll.GetCount<WBJointProjectInfo>(sbWhere.ToString());
            List<WBJointProjectInfo> data = bll.GetLit<WBJointProjectInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID ASC");
            resp.ExObj = data;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }


        /// <summary>
        /// 获取项目列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetProjectList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string projectName = context.Request["ProjectName"];
            string area = context.Request["Area"];
            string category = context.Request["Category"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" Status in(1,2) And WebsiteOwner='{0}' ",bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(projectName))
            {
                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", projectName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area='{0}'", area);
            }
            if (!string.IsNullOrEmpty(category))
            {
                sbWhere.AppendFormat(" And Category='{0}'", category);
            }
            totalCount = bll.GetCount<WBProjectInfo>(sbWhere.ToString());
            List<WBProjectInfo> data = bll.GetLit<WBProjectInfo>(pageSize, pageIndex, sbWhere.ToString(), " Status ASC,AutoID DESC");
            List<ProjectInfoModel> newData = new List<ProjectInfoModel>();
            foreach (var item in data)
            {

                ProjectInfoModel model = new ProjectInfoModel();
                model.AutoID = item.AutoID;
                model.ProjectName = item.ProjectName;
                model.Thumbnails = item.Thumbnails;
                model.Area = item.Area;
                model.Category = item.Category;
                model.ProjectCycle = item.ProjectCycle;
                model.Status = item.Status;
                model.InsertDate = string.Format("{0:f}",item.InsertDate);
                model.InsertDateStr = item.InsertDateStr;
                WBCompanyInfo CompanyInfo = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'",item.UserId));
                if (CompanyInfo!=null)
                {
                    model.CompanyName = CompanyInfo.CompanyName;

                }
                newData.Add(model);

            }
            resp.ExObj = newData;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 更新基地信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdateBaseInfo(HttpContext context)
        {

            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals("1"))
            {
                resp.Msg = "您不是基地用户，无法修改";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            string baseName = context.Request["BaseName"];
            string thumbnails = context.Request["Thumbnails"];
            string address = context.Request["Address"];
            string area = context.Request["Area"];
            string tel = context.Request["Tel"];
            string phone = context.Request["Phone"];
            string qq = context.Request["QQ"];
            string contacts = context.Request["Contacts"];
            string acreage = context.Request["Acreage"];
            string helpCount = context.Request["HelpCount"];
            string introduction = context.Request["Introduction"];

            WBBaseInfo model = bll.Get<WBBaseInfo>(string.Format("UserId='{0}'", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            model.BaseName = baseName;
            model.Thumbnails = thumbnails;
            model.Address = address;
            model.Area = area;
            model.Tel = tel;
            model.Phone = phone;
            model.QQ = qq;
            model.Contacts = contacts;
            model.Acreage = acreage;
            model.HelpCount = string.IsNullOrEmpty(helpCount) ? 0 : int.Parse(helpCount);
            model.Introduction = introduction;
            if (bll.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新基地信息成功";
            }
            else
            {
                resp.Msg = "更新基地信息失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 更新企业信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdateCompanyInfo(HttpContext context)
        {

            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals("0"))
            {
                resp.Msg = "您不是企业用户，无法修改";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            string baseName = context.Request["CompanyName"];
            string thumbnails = context.Request["Thumbnails"];
            string address = context.Request["Address"];
            string area = context.Request["Area"];
            string tel = context.Request["Tel"];
            string phone = context.Request["Phone"];
            string qq = context.Request["QQ"];
            string contacts = context.Request["Contacts"];
            string businessLicenseNumber = context.Request["BusinessLicenseNumber"];
            string introduction = context.Request["Introduction"];

            WBCompanyInfo model = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            model.CompanyName = baseName;
            model.Thumbnails = thumbnails;
            model.Address = address;
            model.Area = area;
            model.Tel = tel;
            model.Phone = phone;
            model.QQ = qq;
            model.Contacts = contacts;
            model.BusinessLicenseNumber = businessLicenseNumber;
            model.Introduction = introduction;
            if (bll.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新企业信息成功";
            }
            else
            {
                resp.Msg = "更新企业信息失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 获取我的项目列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetMyProjectList(HttpContext context)
        {
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals("0"))
            {
                return null;

            }
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string projectName = context.Request["ProjectName"];
            string area = context.Request["Area"];
            string category = context.Request["Category"];
            string status = context.Request["Status"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" UserId='{0}' And WebsiteOwner='{1}'", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString(),bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(projectName))
            {
                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", projectName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area='{0}'", area);
            }
            if (!string.IsNullOrEmpty(category))
            {
                sbWhere.AppendFormat(" And Category='{0}'", category);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status='{0}'", status);
            }
            totalCount = bll.GetCount<WBProjectInfo>(sbWhere.ToString());
            List<WBProjectInfo> data = bll.GetLit<WBProjectInfo>(pageSize, pageIndex, sbWhere.ToString(), " Status ASC,AutoID DESC");
            for (int i = 0; i < data.Count; i++)
            {
                
                data[i].TimeRequirement = null;
                data[i].Introduction = null;

            }
            resp.ExObj = data;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddAttentionInfo(HttpContext context)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            int attentionAutoId =int.Parse(context.Request["AttentionAutoID"]);
            string attentionType = context.Request["AttentionType"];
            if (bll.GetCount<WBAttentionInfo>(string.Format("UserId='{0}' And AttentionAutoID={1} And AttentionType={2}",HttpContext.Current.Session[SessionKey.WanBangUserID].ToString(),attentionAutoId,attentionType))>0)
            {
                resp.Msg = "已经关注过了";
                return Common.JSONHelper.ObjectToJson(resp);


            }
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals(attentionType))
            {
                switch (int.Parse(attentionType))
                {
                    case 0:
                        if (bll.Get<WBBaseInfo>(string.Format("AutoID={0}",attentionAutoId)).UserId.Equals(HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()))
                        {
                          resp.Msg = "不能关注自己";
                          return Common.JSONHelper.ObjectToJson(resp);

                        }
                        break;
                    case 1:
                        if (bll.Get<WBCompanyInfo>(string.Format("AutoID={0}", attentionAutoId)).UserId.Equals(HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()))
                        {
                            resp.Msg = "不能关注自己";
                            return Common.JSONHelper.ObjectToJson(resp);

                        }
                        break;

                }
            }
            WBAttentionInfo model = new WBAttentionInfo();
            model.UserId = HttpContext.Current.Session[SessionKey.WanBangUserID].ToString();
            model.AttentionAutoID = attentionAutoId;
            model.AttentionType = int.Parse(attentionType);
            model.InsertDate = DateTime.Now;
            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "关注成功";
            }
            else
            {
                resp.Msg = "关注失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        
        
        }


        
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string CancelAttention(HttpContext context)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            int autoId = int.Parse(context.Request["AutoID"]);
            string attentionType = context.Request["Attentiontype"];
            if (bll.Delete(new WBAttentionInfo(),string.Format("AttentionAutoID={0} And AttentionType={1} And UserId='{2}'",autoId,attentionType,HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()))>0)
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        
        
        }

        /// <summary>
        /// 添加项目信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddProjectInfo(HttpContext context)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            string projectName = context.Request["ProjectName"];
            string thumbnails = "/img/hb/hb5.jpg";
            string area = context.Request["Area"];
            string category = context.Request["Category"];
            string logistics = context.Request["Logistics"];
            string projectCycle = context.Request["ProjectCycle"];
            string timeRequirement = context.Request["TimeRequirement"];
            string introduction = context.Request["Introduction"];
            WBProjectInfo model = new WBProjectInfo();
            model.UserId = HttpContext.Current.Session[SessionKey.WanBangUserID].ToString();
            model.ProjectName = projectName;
            model.Thumbnails = thumbnails;
            model.Area = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString())).Area;
            model.Category = category;
            model.Logistics = int.Parse(logistics);
            model.ProjectCycle = int.Parse(projectCycle);
            model.Status =0;
            model.TimeRequirement = timeRequirement;
            model.Introduction = introduction;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "发布项目成功，请等待审核";
            }
            else
            {
                resp.Msg = "发布项目失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑项目信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EditProjectInfo(HttpContext context)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            int autoId = int.Parse(context.Request["AutoID"]);
            string projectName = context.Request["ProjectName"];
            string thumbnails = context.Request["Thumbnails"];
            //string Area = context.Request["Area"];
            string category = context.Request["Category"];
            string logistics = context.Request["Logistics"];
            string projectCycle = context.Request["ProjectCycle"];
            string timeRequirement = context.Request["TimeRequirement"];
            string introduction = context.Request["Introduction"];
            WBProjectInfo model = bll.Get<WBProjectInfo>(string.Format("AutoID={0} And UserId='{1}'", autoId, HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            if (model==null)
            {
                resp.Msg = "无效项目";
                return Common.JSONHelper.ObjectToJson(resp);
 
            }
            model.ProjectName = projectName;
            //model.Thumbnails = Thumbnails;
            //model.Area = Area;
            model.Category = category;
            model.Logistics = int.Parse(logistics);
            model.ProjectCycle = int.Parse(projectCycle);
            model.TimeRequirement = timeRequirement;
            model.Introduction = introduction;
            
            if (bll.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新项目信息成功";
            }
            else
            {
                resp.Msg = "更新项目信息失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 结束项目
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EndProject(HttpContext context)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            int autoId = int.Parse(context.Request["AutoID"]);

            WBProjectInfo model = bll.Get<WBProjectInfo>(string.Format("AutoID={0} And UserId='{1}'", autoId, HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            if (model==null)
            {
                resp.Msg = "无效项目";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            model.Status = 2;
            if (bll.Update(model))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 获取我收藏的基地列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetAttentionBaseList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);

            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" UserId='{0}' And AttentionType=0", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            totalCount = bll.GetCount<WBAttentionInfo>(sbWhere.ToString());
            List<WBAttentionInfo> data = bll.GetLit<WBAttentionInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            List<WBBaseInfo> NewData = new List<WBBaseInfo>();
            foreach (var item in data)
            {
                WBBaseInfo model = bll.Get <WBBaseInfo>(string.Format("AutoID={0}",item.AttentionAutoID));
                if (model!=null)
                {
                    model.UserId = null;
                    model.Password = null;
                    model.Introduction = null;
                    NewData.Add(model);
                }

            }
            resp.ExObj = NewData;
            resp.ExStr = "";
            int totalPage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 获取我收藏的企业列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetAttentionCompanyList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);

            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" UserId='{0}' And AttentionType=1", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            totalCount = bll.GetCount<WBAttentionInfo>(sbWhere.ToString());
            List<WBAttentionInfo> data = bll.GetLit<WBAttentionInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            List<WBCompanyInfo> NewData = new List<WBCompanyInfo>();
            foreach (var item in data)
            {
                WBCompanyInfo model = bll.Get<WBCompanyInfo>(string.Format("AutoID={0}", item.AttentionAutoID));
                if (model != null)
                {
                    model.UserId = null;
                    model.Password = null;
                    model.Introduction = null;
                    NewData.Add(model);
                }

            }
            resp.ExObj = NewData;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 获取我收藏的项目列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetAttentionProjectList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" UserId='{0}' And AttentionType=2", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));
            totalCount = bll.GetCount<WBAttentionInfo>(sbWhere.ToString());
            List<WBAttentionInfo> data = bll.GetLit<WBAttentionInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            List<WBProjectInfo> newData = new List<WBProjectInfo>();
            foreach (var item in data)
            {
                WBProjectInfo model = bll.Get<WBProjectInfo>(string.Format("AutoID={0}", item.AttentionAutoID));
                if (model != null)
                {
                    model.UserId = null;
                    model.WebsiteOwner = null;
                    newData.Add(model);
                    
                }

            }
            resp.ExObj = newData;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 项目实体
        /// </summary>
        public class ProjectInfoModel
        {
            /// <summary>
            /// 自动编号
            /// </summary>
            public int AutoID { get; set; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { get; set; }
            /// <summary>
            /// 项目缩略图
            /// </summary>
            public string Thumbnails { get; set; }
            /// <summary>
            /// 项目位置区县
            /// </summary>
            public string Area { get; set; }
            /// <summary>
            /// 项目分类
            /// </summary>
            public string Category { get; set; }

            /// <summary>
            /// 项目周期 0表示 临时(1个月以内) 1表示 短期(1-3个月) 2表示 中期(3-6个月) 3表示 长期(6-12个月)
            /// </summary>
            public int ProjectCycle { get; set; }
            /// <summary>
            /// 项目状态 0代表审核中 1代表征集中 2代表已结束
            /// </summary>
            public int Status { get; set; }
            /// <summary>
            /// 入库日期
            /// </summary>
            public string InsertDate { get; set; }
            /// <summary>
            /// 公司名称
            /// </summary>
            public string CompanyName { get; set; }

            public string InsertDateStr { get; set; }
        }

    }
}