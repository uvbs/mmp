using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.WanBang
{
    /// <summary>
    /// 万邦关爱PC端处理程序
    /// </summary>
    public class PC : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll=new BLL();
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
                    #region 基地管理
                    case "AddBaseInfo"://添加基地
                        result = AddBaseInfo(context);
                        break;
                    case "EditBaseInfo"://编辑基地信息
                        result = EditBaseInfo(context);
                        break;
                    case "DeleteBaseInfo"://删除基地信息
                        result = DeleteBaseInfo(context);
                        break;
                    case "QueryBaseInfo"://查询基地
                        result = QueryBaseInfo(context);
                        break;
                    #endregion

                    #region 企业管理
                    case "AddCompanyInfo"://添加
                        result = AddCompanyInfo(context);
                        break;
                    case "EditCompanyInfo"://编辑
                        result = EditCompanyInfo(context);
                        break;
                    case "DeleteCompanyInfo"://删除
                        result = DeleteCompanyInfo(context);
                        break;
                    case "QueryCompanyInfo"://查询
                        result = QueryCompanyInfo(context);
                        break;
                    #endregion

                    #region 项目管理
                    case "AddProjectInfo"://添加
                        result = AddProjectInfo(context);
                        break;
                    case "EditProjectInfo"://编辑
                        result = EditProjectInfo(context);
                        break;
                    case "DeleteProjectInfo"://删除
                        result = DeleteProjectInfo(context);
                        break;
                    case "QueryProjectInfo"://查询
                        result = QueryProjectInfo(context);
                        break;



                    #endregion

                    #region 对接项目管理
                    case "AddJointProject"://添加
                        result = AddJointProject(context);
                        break;
                    case "EditJointProject"://编辑
                        result = EditJointProject(context);
                        break;
                    case "DeleteJointProject"://删除
                        result = DeleteJointProject(context);
                        break;
                    case "QueryJointProject"://查询
                        result = QueryJointProject(context);
                        break;
                    #endregion

                    //case "ImportBaseData"://导入基地数据
                    //    result = ImportBaseData(context);
                    //    break;

                    //case "ImportCompanyData"://导入企业数据
                    //    result = ImportCompanyData(context);
                    //    break;

                    //case "ImportCompanyDataNew"://导入企业数据
                    //    result = ImportCompanyDataNew(context);
                    //    break;

                    //case "ImportProjectData"://导入企业数据
                    //    result = ImportProjectData(context);
                    //    break;
                    //case "ImportJointProject"://导入对接项目数据
                    //    result = ImportJointProject(context);
                    //    break;

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

        #region 基地管理模块
        /// <summary>
        /// 查询基地
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string QueryBaseInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string baseName = context.Request["BaseName"];
            string area = context.Request["Area"];

            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'",bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(baseName))
            {
                sbWhere.AppendFormat(" And BaseName like '%{0}%'", baseName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area = '{0}'", area);
            }


            int totalCount = bll.GetCount<WBBaseInfo>(sbWhere.ToString());
            List<WBBaseInfo> dataList = new List<WBBaseInfo>();
            dataList = bll.GetLit<WBBaseInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = dataList
     });


        }

        /// <summary>
        /// 删除基地信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DeleteBaseInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            return string.Format("成功删除 {0} 个基地信息", bll.Delete(new WBBaseInfo(), string.Format("AutoID in ({0}) And WebsiteOwner='{1}'", ids,bll.WebsiteOwner)));

        }
        /// <summary>
        /// 添加基地
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddBaseInfo(HttpContext context)
        {
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
            string userId = context.Request["UserId"];
            string password = context.Request["Password"];
            string isDisable = context.Request["IsDisable"];
            string introduction = context.Request["Introduction"];
            if (bll.GetCount<WBBaseInfo>(string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "用户名已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bll.GetCount<WBCompanyInfo>(string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "此用户名在企业列表中已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WBBaseInfo model = new WBBaseInfo();
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
            model.UserId = userId;
            model.Password = password;
            model.IsDisable = int.Parse(isDisable);
            model.Introduction = introduction;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加基地成功";
            }
            else
            {
                resp.Msg = "添加基地失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑基地信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EditBaseInfo(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
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
            string userId = context.Request["UserId"];
            string password = context.Request["Password"];
            string isDisable = context.Request["IsDisable"];
            string introduction = context.Request["Introduction"];
            if (bll.GetCount<WBBaseInfo>(string.Format("UserId='{0}' And AutoID!={1}", userId, autoId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "用户名已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bll.GetCount<WBCompanyInfo>(string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "此用户名在企业列表中已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WBBaseInfo model = bll.Get<WBBaseInfo>(string.Format("AutoID={0}", autoId));
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
            model.UserId = userId;
            model.Password = password;
            model.IsDisable = int.Parse(isDisable);
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

        #endregion

        #region 企业管理模块
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string QueryCompanyInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string companyName = context.Request["CompanyName"];
            string area = context.Request["Area"];

            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'",bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(companyName))
            {
                sbWhere.AppendFormat(" And CompanyName like '%{0}%'", companyName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area = '{0}'", area);
            }


            int totalCount = bll.GetCount<WBCompanyInfo>(sbWhere.ToString());
            List<WBCompanyInfo> dataList = new List<WBCompanyInfo>();
            dataList = bll.GetLit<WBCompanyInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = dataList
     });


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DeleteCompanyInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            return string.Format("成功删除 {0} 个企业信息", bll.Delete(new WBCompanyInfo(), string.Format("AutoID in ({0}) And WebsiteOwner='{1}'", ids,bll.WebsiteOwner)));

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddCompanyInfo(HttpContext context)
        {
            string companyName = context.Request["CompanyName"];
            string thumbnails = context.Request["Thumbnails"];
            string address = context.Request["Address"];
            string area = context.Request["Area"];
            string tel = context.Request["Tel"];
            string phone = context.Request["Phone"];
            string qq = context.Request["QQ"];
            string contacts = context.Request["Contacts"];
            string businessLicenseNumber = context.Request["BusinessLicenseNumber"];
            string helpCount = context.Request["HelpCount"];
            string userId = context.Request["UserId"];
            string password = context.Request["Password"];
            string isDisable = context.Request["IsDisable"];
            string introduction = context.Request["Introduction"];
            if (bll.GetCount<WBCompanyInfo>(string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "用户名已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bll.GetCount<WBCompanyInfo>(string.Format("CompanyName='{0}'", companyName)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "企业已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bll.GetCount<WBBaseInfo>(string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "此用户名在基地列表中已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WBCompanyInfo model = new WBCompanyInfo();
            model.CompanyName = companyName;
            model.Thumbnails = thumbnails;
            model.Address = address;
            model.Area = area;
            model.Tel = tel;
            model.Phone = phone;
            model.QQ = qq;
            model.Contacts = contacts;
            model.BusinessLicenseNumber = businessLicenseNumber;
            model.UserId = userId;
            model.Password = password;
            model.IsDisable = int.Parse(isDisable);
            model.Introduction = introduction;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加企业成功";
            }
            else
            {
                resp.Msg = "添加企业失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EditCompanyInfo(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            string companyName = context.Request["CompanyName"];
            string thumbnails = context.Request["Thumbnails"];
            string address = context.Request["Address"];
            string area = context.Request["Area"];
            string tel = context.Request["Tel"];
            string phone = context.Request["Phone"];
            string qq = context.Request["QQ"];
            string contacts = context.Request["Contacts"];
            string businessLicenseNumber = context.Request["BusinessLicenseNumber"];
            string userId = context.Request["UserId"];
            string password = context.Request["Password"];
            string isDisable = context.Request["IsDisable"];
            string introduction = context.Request["Introduction"];
            if (bll.GetCount<WBCompanyInfo>(string.Format("UserId='{0}' And AutoID!={1}", userId, autoId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "用户名已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bll.GetCount<WBBaseInfo>(string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "此用户名在基地列表中已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WBCompanyInfo model = bll.Get<WBCompanyInfo>(string.Format("AutoID={0}", autoId));
            model.CompanyName = companyName;
            model.Thumbnails = thumbnails;
            model.Address = address;
            model.Area = area;
            model.Tel = tel;
            model.Phone = phone;
            model.QQ = qq;
            model.Contacts = contacts;
            model.BusinessLicenseNumber = businessLicenseNumber;
            model.UserId = userId;
            model.Password = password;
            model.IsDisable = int.Parse(isDisable);
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

        #endregion

        #region 项目管理模块
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string QueryProjectInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string projectName = context.Request["ProjectName"];
            string area = context.Request["Area"];
            string category = context.Request["Category"];
            string status = context.Request["Status"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(projectName))
            {
                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", projectName);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area = '{0}'", area);
            }
            if (!string.IsNullOrEmpty(category))
            {
                sbWhere.AppendFormat(" And Category = '{0}'", category);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status = '{0}'", status);
            }
            int totalCount = bll.GetCount<WBProjectInfo>(sbWhere.ToString());
            List<WBProjectInfo> dataList = new List<WBProjectInfo>();
            dataList = bll.GetLit<WBProjectInfo>(pageSize, pageIndex, sbWhere.ToString(), "Status ASC,AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = dataList
     });


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DeleteProjectInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            return bll.Delete(new WBProjectInfo(), string.Format("AutoID in (0) And WebsiteOwner='{1}'", ids,bll.WebsiteOwner)).ToString();


        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddProjectInfo(HttpContext context)
        {
            string userId = context.Request["UserId"];
            string projectName = context.Request["ProjectName"];
            string thumbnails = context.Request["Thumbnails"];
            string area = context.Request["Area"];
            string category = context.Request["Category"];
            string logistics = context.Request["Logistics"];
            string projectCycle = context.Request["ProjectCycle"];
            string status = context.Request["Status"];
            string timeRequirement = context.Request["TimeRequirement"];
            string introduction = context.Request["Introduction"];

            var CompanyModel = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'", userId));
            if (CompanyModel == null)
            {
                resp.Status = 0;
                resp.Msg = "项目所属用户名不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            WBProjectInfo model = new WBProjectInfo();
            model.UserId = userId;
            model.ProjectName = projectName;
            model.Thumbnails = thumbnails;
            model.Area = area;
            model.Category = category;
            model.Logistics = int.Parse(logistics);
            model.ProjectCycle = int.Parse(projectCycle);
            model.Status = int.Parse(status);
            model.TimeRequirement = timeRequirement;
            model.Introduction = introduction;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bll.WebsiteOwner;

            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加项目成功";
            }
            else
            {
                resp.Msg = "添加项目失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EditProjectInfo(HttpContext context)
        {

            int autoId = int.Parse(context.Request["AutoID"]);
            string userId = context.Request["UserId"];
            string projectName = context.Request["ProjectName"];
            string thumbnails = context.Request["Thumbnails"];
            string area = context.Request["Area"];
            string category = context.Request["Category"];
            string logistics = context.Request["Logistics"];
            string projectCycle = context.Request["ProjectCycle"];
            string status = context.Request["Status"];
            string timeRequirement = context.Request["TimeRequirement"];
            string introduction = context.Request["Introduction"];
            var companyModel = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'", userId));
            if (companyModel == null)
            {
                resp.Status = 0;
                resp.Msg = "项目所属用户名不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            WBProjectInfo model = bll.Get<WBProjectInfo>(string.Format("AutoID={0}", autoId));
            model.UserId = userId;
            model.ProjectName = projectName;
            model.Thumbnails = thumbnails;
            model.Area = area;
            model.Category = category;
            model.Logistics = int.Parse(logistics);
            model.ProjectCycle = int.Parse(projectCycle);
            model.Status = int.Parse(status);
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
        /// 导入基地数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ImportBaseData(HttpContext context)
        {
            try
            {


                List<BLLJIMP.Model.WBBaseInfo> list = DatabaleToListBase(ExeclToDatabale("D:\\基地列表.xlsx"));
                if (bll.AddList<BLLJIMP.Model.WBBaseInfo>(list))
                {
                  resp.Status = -1;
                  resp.Msg ="导入成功!";

                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// DataTable转换 List<BLLJIMP.Model.WBBaseInfo>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BLLJIMP.Model.WBBaseInfo> DatabaleToListBase(System.Data.DataTable dt)
        {
            List<BLLJIMP.Model.WBBaseInfo> list = new List<BLLJIMP.Model.WBBaseInfo>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow item in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            var model = new BLLJIMP.Model.WBBaseInfo();
                                model.BaseName = item[0].ToString().Trim();
                                model.Address = item[1].ToString().Trim();
                                model.Area = item[2].ToString().Trim();
                                model.Contacts = item[3].ToString().Trim();
                                model.Tel =!string.IsNullOrEmpty(item[4].ToString().Trim())?("021"+item[4].ToString().Trim()):null ;
                                model.Phone = item[5].ToString().Trim();
                                model.Acreage = item[6].ToString().Trim();
                                model.HelpCount = int.Parse(item[7].ToString());
                                model.UserId = item[0].ToString().Trim();
                                model.Password="123";
                                model.InsertDate = DateTime.Now;
                                model.WebsiteOwner = bll.WebsiteOwner;
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// 导入企业数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ImportCompanyData(HttpContext context)
        {
            try
            {


                List<BLLJIMP.Model.WBCompanyInfo> list = DatabaleToListCompany(ExeclToDatabale("D:\\企业.xlsx"));
                if (bll.AddList<BLLJIMP.Model.WBCompanyInfo>(list))
                {
                    resp.Status = -1;
                    resp.Msg = "导入成功!";

                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// DataTable转换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BLLJIMP.Model.WBCompanyInfo> DatabaleToListCompany(System.Data.DataTable dt)
        {
            List<BLLJIMP.Model.WBCompanyInfo> list = new List<BLLJIMP.Model.WBCompanyInfo>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow item in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            var model = new BLLJIMP.Model.WBCompanyInfo();
                            model.CompanyName = item[0].ToString().Trim();
                            model.Contacts = item[1].ToString().Trim();
                            model.Phone = item[2].ToString().Trim();
                            model.UserId = item[0].ToString().Trim();
                            model.Password = "123";
                            model.Tel = !string.IsNullOrEmpty(item[3].ToString().Trim()) ? ("021" + item[3].ToString().Trim()) : null;
                            model.InsertDate = DateTime.Now;
                            model.WebsiteOwner = bll.WebsiteOwner;
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }


        ///// <summary>
        ///// 导入企业数据
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string ImportCompanyDataNew(HttpContext context)
        //{
        //    try
        //    {


        //        List<BLLJIMP.Model.WBCompanyInfo> list = DatabaleToListCompanyNew(ExeclToDatabale("D:\\微信后台新企业.xlsx"));
        //        if (bll.AddList<BLLJIMP.Model.WBCompanyInfo>(list))
        //        {
        //            resp.Status = -1;
        //            resp.Msg = "导入成功!";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = ex.Message;
        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        /// <summary>
        /// DataTable转换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BLLJIMP.Model.WBCompanyInfo> DatabaleToListCompanyNew(System.Data.DataTable dt)
        {
            List<BLLJIMP.Model.WBCompanyInfo> list = new List<BLLJIMP.Model.WBCompanyInfo>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow item in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            var model = new BLLJIMP.Model.WBCompanyInfo();
                            model.CompanyName = item[0].ToString().Trim();
                            model.Contacts = item[1].ToString().Trim();
                            model.Phone = item[2].ToString().Trim();
                            model.UserId = item[0].ToString().Trim();
                            model.Password = "123";
                            model.Tel = !string.IsNullOrEmpty(item[3].ToString().Trim()) ? ("021" + item[3].ToString().Trim()) : null;
                            model.Address = item[4].ToString().Trim();
                            model.InsertDate = DateTime.Now;
                            model.WebsiteOwner = bll.WebsiteOwner;
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// 导入项目数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ImportProjectData(HttpContext context)
        {
            try
            {


                List<BLLJIMP.Model.WBProjectInfo> list = DatabaleToListProject(ExeclToDatabale("D:\\项目.xlsx"));
                if (bll.AddList<BLLJIMP.Model.WBProjectInfo>(list))
                {
                    resp.Status = -1;
                    resp.Msg = "导入成功!";

                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// DataTable转换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BLLJIMP.Model.WBProjectInfo> DatabaleToListProject(System.Data.DataTable dt)
        {
            List<BLLJIMP.Model.WBProjectInfo> list = new List<BLLJIMP.Model.WBProjectInfo>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow item in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            var model = new BLLJIMP.Model.WBProjectInfo();
                            model.ProjectName = item[1].ToString().Trim();
                            model.UserId=item[0].ToString().Trim();
                            model.InsertDate = DateTime.Now;
                            model.Category="其它项目";
                            model.Status = 1;
                            model.WebsiteOwner ="10000care";
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        ////
        ///// <summary>
        ///// 导入对接数据
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string ImportJointProject(HttpContext context)
        //{
        //    try
        //    {


        //        List<BLLJIMP.Model.WBJointProjectInfo> list = DatabaleToListJointProject(ExeclToDatabale("D:\\对接项目.xlsx"));
        //        if (bll.AddList<BLLJIMP.Model.WBJointProjectInfo>(list))
        //        {
        //            resp.Status = -1;
        //            resp.Msg = "导入成功!";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = ex.Message;
        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        /// <summary>
        /// DataTable转换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BLLJIMP.Model.WBJointProjectInfo> DatabaleToListJointProject(System.Data.DataTable dt)
        {
            List<BLLJIMP.Model.WBJointProjectInfo> list = new List<BLLJIMP.Model.WBJointProjectInfo>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow item in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            var model = new BLLJIMP.Model.WBJointProjectInfo();
                            model.BaseName = item[0].ToString().Trim();
                            model.ProjectName = item[1].ToString().Trim();
                            model.CompanyName = item[2].ToString().Trim();
                            model.Thumbnails = "/img/hb/hb5.jpg";
                            model.InsertDate = DateTime.Now;
                            model.WebsiteOwner ="10000care";
                            if ((!string.IsNullOrEmpty(model.BaseName))&&(!string.IsNullOrEmpty(model.CompanyName))&&(!string.IsNullOrEmpty(model.ProjectName)))
                            {
                                list.Add(model);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }


        /// <summary>
        /// execl转换dataBale
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private System.Data.DataTable ExeclToDatabale(string filePath)
        {
            List<string> sheetNamelist = new List<string>();
            string strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", filePath);
            System.Data.OleDb.OleDbConnection myConn = new System.Data.OleDb.OleDbConnection(strConn);
            myConn.Open();

            string sheetName = "";

            System.Data.DataTable dt = myConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            for (int i =0; i < dt.Rows.Count; i++)
            {
                sheetName = dt.Rows[i]["TABLE_NAME"].ToString();

                sheetNamelist.Add(sheetName);
            }
            string currSheet = sheetNamelist[0].ToString();
            System.Data.OleDb.OleDbDataAdapter oda = new System.Data.OleDb.OleDbDataAdapter("select * from[" + currSheet + "]", myConn);
            System.Data.DataSet mySet = new System.Data.DataSet();
            oda.Fill(mySet, "tmp");
            myConn.Close();
            return mySet.Tables[0];
        }

        #endregion

        #region 对接项目管理模块
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string QueryJointProject(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string projectName = context.Request["ProjectName"];
            string companyName = context.Request["CompanyName"];
            string baseName = context.Request["BaseName"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(projectName))
            {
                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", projectName);
            }
            if (!string.IsNullOrEmpty(baseName))
            {
                sbWhere.AppendFormat(" And BaseName like '%{0}%'", baseName);
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                sbWhere.AppendFormat(" And CompanyName like '%{0}%'", companyName);
            }
            int totalCount = bll.GetCount<WBJointProjectInfo>(sbWhere.ToString());
            List<WBJointProjectInfo> dataList = new List<WBJointProjectInfo>();
            dataList = bll.GetLit<WBJointProjectInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = dataList
                });

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DeleteJointProject(HttpContext context)
        {
            string ids = context.Request["ids"];
            return bll.Delete(new WBJointProjectInfo(), string.Format("AutoID in (0) And WebsiteOwner='{1}'", ids, bll.WebsiteOwner)).ToString();


        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddJointProject(HttpContext context)
        {

            string projectName = context.Request["ProjectName"];
            string companyName = context.Request["CompanyName"];
            string baseName = context.Request["BaseName"];
            string thumbnails = context.Request["Thumbnails"];
            //var CompanyModel = bll.Get<WBCompanyInfo>(string.Format("CompanyName='{0}'", CompanyName));
            //if (CompanyModel == null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "企业不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            //var BasseModel = bll.Get<WBBaseInfo>(string.Format("BaseName='{0}'", BaseName));
            //if (BasseModel == null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "基地不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            //var ProjectModel = bll.Get<WBProjectInfo>(string.Format("ProjectName='{0}'", ProjectName));
            //if (ProjectModel == null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "项目不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            WBJointProjectInfo model = new WBJointProjectInfo();
            model.ProjectName = projectName;
            model.Thumbnails = thumbnails;
            model.CompanyName = companyName;
            model.BaseName = baseName;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bll.WebsiteOwner;

            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";
            }
            else
            {
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EditJointProject(HttpContext context)
        {

            int autoId = int.Parse(context.Request["AutoID"]);
            string projectName = context.Request["ProjectName"];
            string companyName = context.Request["CompanyName"];
            string baseName = context.Request["BaseName"];
            string thumbnails = context.Request["Thumbnails"];
            WBJointProjectInfo model = bll.Get<WBJointProjectInfo>(string.Format("AutoID={0} And WebsiteOwner='{1}'", autoId, bll.WebsiteOwner));
            //if (model==null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "对接项目不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            //var CompanyModel = bll.Get<WBCompanyInfo>(string.Format("CompanyName='{0}'", CompanyName));
            //if (CompanyModel == null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "企业不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            //var BasseModel = bll.Get<WBBaseInfo>(string.Format("BaseName='{0}'", BaseName));
            //if (BasseModel == null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "基地不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            //var ProjectModel = bll.Get<WBProjectInfo>(string.Format("ProjectName='{0}'", ProjectName));
            //if (ProjectModel == null)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "项目不存在";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            model.ProjectName = projectName;
            model.Thumbnails = thumbnails;
            model.CompanyName = companyName;
            model.BaseName = baseName;
            if (bll.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新成功";
            }
            else
            {
                resp.Msg = "更新失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }













  

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }







    }
}