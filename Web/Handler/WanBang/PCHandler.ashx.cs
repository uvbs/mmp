//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.SessionState;
//using ZentCloud.BLLJIMP;
//using System.Text;
//using ZentCloud.BLLJIMP.Model;

//namespace ZentCloud.JubitIMP.Web.Handler.WanBang
//{
//    /// <summary>
//    /// 万邦关爱PC端处理程序
//    /// </summary>
//    public class PCHandler : IHttpHandler, IRequiresSessionState
//    {
//        AshxResponse resp = new AshxResponse();
//        BLLJIMP.BLL bll;
//        /// <summary>
//        /// 当前用户信息
//        /// </summary>
//        ZentCloud.BLLJIMP.Model.UserInfo userInfo;
//        public void ProcessRequest(HttpContext context)
//        {
//            context.Response.ContentType = "text/plain";
//            context.Response.Expires = 0;
//            string result = "false";

//            try
//            {
//                bll = new BLLJIMP.BLL();
//                this.userInfo = Comm.DataLoadTool.GetCurrUserModel();
//                string Action = context.Request["Action"];
//                switch (Action)
//                {
//                    #region 基地管理
//                    case "AddBaseInfo"://添加基地
//                        result = AddBaseInfo(context);
//                        break;
//                    case "EditBaseInfo"://编辑基地信息
//                        result = EditBaseInfo(context);
//                        break;
//                    case "DeleteBaseInfo"://删除基地信息
//                        result = DeleteBaseInfo(context);
//                        break;
//                    case "QueryBaseInfo"://查询基地
//                        result = QueryBaseInfo(context);
//                        break; 
//                    #endregion

//                    #region 企业管理
//                    case "AddCompanyInfo"://添加
//                        result = AddCompanyInfo(context);
//                        break;
//                    case "EditCompanyInfo"://编辑
//                        result = EditCompanyInfo(context);
//                        break;
//                    case "DeleteCompanyInfo"://删除
//                        result = DeleteCompanyInfo(context);
//                        break;
//                    case "QueryCompanyInfo"://查询
//                        result = QueryCompanyInfo(context);
//                        break; 
//                    #endregion

//                    #region 项目管理
//                    case "AddProjectInfo"://添加
//                        result = AddProjectInfo(context);
//                        break;
//                    case "EditProjectInfo"://编辑
//                        result = EditProjectInfo(context);
//                        break;
//                    case "DeleteProjectInfo"://删除
//                        result = DeleteProjectInfo(context);
//                        break;
//                    case "QueryProjectInfo"://查询
//                        result = QueryProjectInfo(context);
//                        break; 
//                    #endregion

//                }
//            }
//            catch (Exception ex)
//            {
//                resp.Status = -1;
//                resp.Msg = ex.Message;
//                result = Common.JSONHelper.ObjectToJson(resp);

//            }

//            context.Response.Write(result);
//        }

//        #region 基地管理模块
//        /// <summary>
//        /// 查询基地
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string QueryBaseInfo(HttpContext context)
//        {

//            int page = Convert.ToInt32(context.Request["page"]);
//            int rows = Convert.ToInt32(context.Request["rows"]);
//            string BaseName = context.Request["BaseName"];
//            string Area = context.Request["Area"];

//            StringBuilder sbWhere = new StringBuilder(string.Format("1=1"));
//            if (!string.IsNullOrEmpty(BaseName))
//            {
//                sbWhere.AppendFormat(" And BaseName like '%{0}%'", BaseName);
//            }
//            if (!string.IsNullOrEmpty(Area))
//            {
//                sbWhere.AppendFormat(" And Area = '{0}'", Area);
//            }


//            int totalcount = bll.GetCount<WBBaseInfo>(sbWhere.ToString());
//            List<WBBaseInfo> dataList = new List<WBBaseInfo>();
//            dataList = bll.GetLit<WBBaseInfo>(rows, page, sbWhere.ToString(), "AutoID DESC");
//            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalcount, dataList);
//            return jsonResult;


//        }

//        /// <summary>
//        /// 删除基地信息
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string DeleteBaseInfo(HttpContext context)
//        {
//            string ids = context.Request["ids"];
//            return string.Format("成功删除 {0} 个基地信息", bll.Delete(new WBBaseInfo(), string.Format("AutoID in ({0})", ids)));

//        }
//        /// <summary>
//        /// 添加基地
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string AddBaseInfo(HttpContext context)
//        {
//            string BaseName = context.Request["BaseName"];
//            string Thumbnails = context.Request["Thumbnails"];
//            string Address = context.Request["Address"];
//            string Area = context.Request["Area"];
//            string Tel = context.Request["Tel"];
//            string Phone = context.Request["Phone"];
//            string QQ = context.Request["QQ"];
//            string Contacts = context.Request["Contacts"];
//            string Acreage = context.Request["Acreage"];
//            string HelpCount = context.Request["HelpCount"];
//            string UserId = context.Request["UserId"];
//            string Password = context.Request["Password"];
//            string IsDisable = context.Request["IsDisable"];
//            string Introduction = context.Request["Introduction"];
//            if (bll.GetCount<WBBaseInfo>(string.Format("UserId='{0}'", UserId))>0)
//            {
//                resp.Status = 0;
//                resp.Msg = "用户名已经存在";
//                return Common.JSONHelper.ObjectToJson(resp);
//            }
//            WBBaseInfo model = new WBBaseInfo();
//            model.BaseName = BaseName;
//            model.Thumbnails = Thumbnails;
//            model.Address = Address;
//            model.Area = Area;
//            model.Tel = Tel;
//            model.Phone = Phone;
//            model.QQ = QQ;
//            model.Contacts = Contacts;
//            model.Acreage = Acreage;
//            model.HelpCount = string.IsNullOrEmpty(HelpCount) ? 0 : int.Parse(HelpCount);
//            model.UserId = UserId;
//            model.Password = Password;
//            model.IsDisable =int.Parse(IsDisable);
//            model.Introduction = Introduction;
//            model.InsertDate = DateTime.Now;
//            if (bll.Add(model))
//            {
//                resp.Status = 1;
//                resp.Msg = "添加基地成功";
//            }
//            else
//            {
//                resp.Msg = "添加基地失败";
//            }
//            return Common.JSONHelper.ObjectToJson(resp);


//        }

//        /// <summary>
//        /// 编辑基地信息
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string EditBaseInfo(HttpContext context)
//        {
//            int AutoID=int.Parse(context.Request["AutoID"]);
//            string BaseName = context.Request["BaseName"];
//            string Thumbnails = context.Request["Thumbnails"];
//            string Address = context.Request["Address"];
//            string Area = context.Request["Area"];
//            string Tel = context.Request["Tel"];
//            string Phone = context.Request["Phone"];
//            string QQ = context.Request["QQ"];
//            string Contacts = context.Request["Contacts"];
//            string Acreage = context.Request["Acreage"];
//            string HelpCount = context.Request["HelpCount"];
//            string UserId = context.Request["UserId"];
//            string Password = context.Request["Password"];
//            string IsDisable = context.Request["IsDisable"];
//            string Introduction = context.Request["Introduction"];
//            if (bll.GetCount<WBBaseInfo>(string.Format("UserId='{0}' And AutoID!={1}", UserId,AutoID)) >0)
//            {
//                resp.Status = 0;
//                resp.Msg = "用户名已经存在";
//                return Common.JSONHelper.ObjectToJson(resp);
//            }
//            WBBaseInfo model = bll.Get<WBBaseInfo>(string.Format("AutoID={0}",AutoID));
//            model.BaseName = BaseName;
//            model.Thumbnails = Thumbnails;
//            model.Address = Address;
//            model.Area = Area;
//            model.Tel = Tel;
//            model.Phone = Phone;
//            model.QQ = QQ;
//            model.Contacts = Contacts;
//            model.Acreage = Acreage;
//            model.HelpCount = string.IsNullOrEmpty(HelpCount) ? 0 : int.Parse(HelpCount);
//            model.UserId = UserId;
//            model.Password = Password;
//            model.IsDisable = int.Parse(IsDisable);
//            model.Introduction = Introduction;
//            if (bll.Update(model))
//            {
//                resp.Status = 1;
//                resp.Msg = "更新基地信息成功";
//            }
//            else
//            {
//                resp.Msg = "更新基地信息失败";
//            }
//            return Common.JSONHelper.ObjectToJson(resp);


//        }

//        #endregion

//        #region 企业管理模块
//        /// <summary>
//        /// 查询
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string QueryCompanyInfo(HttpContext context)
//        {

//            int page = Convert.ToInt32(context.Request["page"]);
//            int rows = Convert.ToInt32(context.Request["rows"]);
//            string CompanyName = context.Request["CompanyName"];
//            string Area = context.Request["Area"];

//            StringBuilder sbWhere = new StringBuilder(string.Format("1=1"));
//            if (!string.IsNullOrEmpty(CompanyName))
//            {
//                sbWhere.AppendFormat(" And CompanyName like '%{0}%'", CompanyName);
//            }
//            if (!string.IsNullOrEmpty(Area))
//            {
//                sbWhere.AppendFormat(" And Area = '{0}'", Area);
//            }


//            int totalcount = bll.GetCount<WBCompanyInfo>(sbWhere.ToString());
//            List<WBCompanyInfo> dataList = new List<WBCompanyInfo>();
//            dataList = bll.GetLit<WBCompanyInfo>(rows, page, sbWhere.ToString(), "AutoID DESC");
//            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalcount, dataList);
//            return jsonResult;


//        }

//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string DeleteCompanyInfo(HttpContext context)
//        {
//            string ids = context.Request["ids"];
//            return string.Format("成功删除 {0} 个企业信息", bll.Delete(new WBCompanyInfo(), string.Format("AutoID in ({0})", ids)));

//        }
//        /// <summary>
//        /// 添加
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string AddCompanyInfo(HttpContext context)
//        {
//            string CompanyName = context.Request["CompanyName"];
//            string Thumbnails = context.Request["Thumbnails"];
//            string Address = context.Request["Address"];
//            string Area = context.Request["Area"];
//            string Tel = context.Request["Tel"];
//            string Phone = context.Request["Phone"];
//            string QQ = context.Request["QQ"];
//            string Contacts = context.Request["Contacts"];
//            string BusinessLicenseNumber = context.Request["BusinessLicenseNumber"];
//            string HelpCount = context.Request["HelpCount"];
//            string UserId = context.Request["UserId"];
//            string Password = context.Request["Password"];
//            string IsDisable = context.Request["IsDisable"];
//            string Introduction = context.Request["Introduction"];
//            if (bll.GetCount<WBCompanyInfo>(string.Format("UserId='{0}'", UserId)) > 0)
//            {
//                resp.Status = 0;
//                resp.Msg = "用户名已经存在";
//                return Common.JSONHelper.ObjectToJson(resp);
//            }
//            WBCompanyInfo model = new WBCompanyInfo();
//            model.CompanyName = CompanyName;
//            model.Thumbnails = Thumbnails;
//            model.Address = Address;
//            model.Area = Area;
//            model.Tel = Tel;
//            model.Phone = Phone;
//            model.QQ = QQ;
//            model.Contacts = Contacts;
//            model.BusinessLicenseNumber = BusinessLicenseNumber;
//            model.UserId = UserId;
//            model.Password = Password;
//            model.IsDisable = int.Parse(IsDisable);
//            model.Introduction = Introduction;
//            model.InsertDate = DateTime.Now;
//            if (bll.Add(model))
//            {
//                resp.Status = 1;
//                resp.Msg = "添加企业成功";
//            }
//            else
//            {
//                resp.Msg = "添加企业失败";
//            }
//            return Common.JSONHelper.ObjectToJson(resp);


//        }

//        /// <summary>
//        /// 编辑
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string EditCompanyInfo(HttpContext context)
//        {
//            int AutoID = int.Parse(context.Request["AutoID"]);
//            string CompanyName = context.Request["CompanyName"];
//            string Thumbnails = context.Request["Thumbnails"];
//            string Address = context.Request["Address"];
//            string Area = context.Request["Area"];
//            string Tel = context.Request["Tel"];
//            string Phone = context.Request["Phone"];
//            string QQ = context.Request["QQ"];
//            string Contacts = context.Request["Contacts"];
//            string BusinessLicenseNumber = context.Request["BusinessLicenseNumber"];
//            string UserId = context.Request["UserId"];
//            string Password = context.Request["Password"];
//            string IsDisable = context.Request["IsDisable"];
//            string Introduction = context.Request["Introduction"];
//            if (bll.GetCount<WBCompanyInfo>(string.Format("UserId='{0}' And AutoID!={1}", UserId,AutoID)) >0)
//            {
//                resp.Status = 0;
//                resp.Msg = "用户名已经存在";
//                return Common.JSONHelper.ObjectToJson(resp);
//            }
//            WBCompanyInfo model = bll.Get<WBCompanyInfo>(string.Format("AutoID={0}", AutoID));
//            model.CompanyName = CompanyName;
//            model.Thumbnails = Thumbnails;
//            model.Address = Address;
//            model.Area = Area;
//            model.Tel = Tel;
//            model.Phone = Phone;
//            model.QQ = QQ;
//            model.Contacts = Contacts;
//            model.BusinessLicenseNumber = BusinessLicenseNumber;
//            model.UserId = UserId;
//            model.Password = Password;
//            model.IsDisable = int.Parse(IsDisable);
//            model.Introduction = Introduction;
//            if (bll.Update(model))
//            {
//                resp.Status = 1;
//                resp.Msg = "更新企业信息成功";
//            }
//            else
//            {
//                resp.Msg = "更新企业信息失败";
//            }
//            return Common.JSONHelper.ObjectToJson(resp);


//        }

//        #endregion

//        #region 项目管理模块
//        /// <summary>
//        /// 查询
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string QueryProjectInfo(HttpContext context)
//        {

//            int page = Convert.ToInt32(context.Request["page"]);
//            int rows = Convert.ToInt32(context.Request["rows"]);
//            string ProjectName = context.Request["ProjectName"];
//            string Area = context.Request["Area"];
//            string Category = context.Request["Category"];
//            string Status = context.Request["Status"];
//            StringBuilder sbWhere = new StringBuilder(string.Format("1=1"));
//            if (!string.IsNullOrEmpty(ProjectName))
//            {
//                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", ProjectName);
//            }
//            if (!string.IsNullOrEmpty(Area))
//            {
//                sbWhere.AppendFormat(" And Area = '{0}'", Area);
//            }
//            if (!string.IsNullOrEmpty(Category))
//            {
//                sbWhere.AppendFormat(" And Category = '{0}'", Category);
//            }
//            if (!string.IsNullOrEmpty(Status))
//            {
//                sbWhere.AppendFormat(" And Status = '{0}'", Status);
//            }
//            int totalcount = bll.GetCount<WBProjectInfo>(sbWhere.ToString());
//            List<WBProjectInfo> dataList = new List<WBProjectInfo>();
//            dataList = bll.GetLit<WBProjectInfo>(rows, page, sbWhere.ToString(), "Status ASC,AutoID DESC");
//            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalcount, dataList);
//            return jsonResult;


//        }

//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string DeleteProjectInfo(HttpContext context)
//        {
//            string ids = context.Request["ids"];
//            return bll.Delete(new WBProjectInfo(), string.Format("AutoID in (0)", ids)).ToString();


//        }
//        /// <summary>
//        /// 添加
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string AddProjectInfo(HttpContext context)
//        {
//            string UserId = context.Request["UserId"];
//            string ProjectName = context.Request["ProjectName"];
//            string Thumbnails = context.Request["Thumbnails"];
//            string Area = context.Request["Area"];
//            string Category = context.Request["Category"];
//            string Logistics = context.Request["Logistics"];
//            string ProjectCycle = context.Request["ProjectCycle"];
//            string Status = context.Request["Status"];
//            string TimeRequirement = context.Request["TimeRequirement"];
//            string Introduction = context.Request["Introduction"];

//            var CompanyModel = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'",UserId));
//            if (CompanyModel==null)
//            {
//                resp.Status =0;
//                resp.Msg = "项目所属用户名不存在";
//                return Common.JSONHelper.ObjectToJson(resp);

//            }

//            WBProjectInfo model = new WBProjectInfo();
//            model.UserId = UserId;
//            model.ProjectName = ProjectName;
//            model.Thumbnails = Thumbnails;
//            model.Area = Area;
//            model.Category = Category;
//            model.Logistics = int.Parse(Logistics);
//            model.ProjectCycle = int.Parse(ProjectCycle);
//            model.Status = int.Parse(Status);
//            model.TimeRequirement = TimeRequirement;
//            model.Introduction = Introduction;
//            model.InsertDate = DateTime.Now;


//            if (bll.Add(model))
//            {
//                resp.Status = 1;
//                resp.Msg = "添加项目成功";
//            }
//            else
//            {
//                resp.Msg = "添加项目失败";
//            }
//            return Common.JSONHelper.ObjectToJson(resp);


//        }

//        /// <summary>
//        /// 编辑
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string EditProjectInfo(HttpContext context)
//        {

//            int AutoID = int.Parse(context.Request["AutoID"]);
//            string UserId = context.Request["UserId"];
//            string ProjectName = context.Request["ProjectName"];
//            string Thumbnails = context.Request["Thumbnails"];
//            string Area = context.Request["Area"];
//            string Category = context.Request["Category"];
//            string Logistics = context.Request["Logistics"];
//            string ProjectCycle = context.Request["ProjectCycle"];
//            string Status = context.Request["Status"];
//            string TimeRequirement = context.Request["TimeRequirement"];
//            string Introduction = context.Request["Introduction"];
//            var CompanyModel = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'", UserId));
//            if (CompanyModel == null)
//            {
//                resp.Status = 0;
//                resp.Msg = "项目所属用户名不存在";
//                return Common.JSONHelper.ObjectToJson(resp);

//            }
//            WBProjectInfo model = bll.Get<WBProjectInfo>(string.Format("AutoID={0}",AutoID));
//            model.UserId = UserId;
//            model.ProjectName = ProjectName;
//            model.Thumbnails = Thumbnails;
//            model.Area = Area;
//            model.Category = Category;
//            model.Logistics = int.Parse(Logistics);
//            model.ProjectCycle = int.Parse(ProjectCycle);
//            model.Status = int.Parse(Status);
//            model.TimeRequirement = TimeRequirement;
//            model.Introduction = Introduction;
//            if (bll.Update(model))
//            {
//                resp.Status = 1;
//                resp.Msg = "更新项目信息成功";
//            }
//            else
//            {
//                resp.Msg = "更新项目信息失败";
//            }
//            return Common.JSONHelper.ObjectToJson(resp);


//        }

//        #endregion

//        public bool IsReusable
//        {
//            get
//            {
//                return false;
//            }
//        }

      





//    }
//}