//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.SessionState;
//using ZentCloud.BLLJIMP.Model;
//using System.Text;
//namespace ZentCloud.JubitIMP.Web.Handler.WangBang
//{
//    /// <summary>
//    /// WapHandler 的摘要说明
//    /// </summary>
//    public class WapHandler : IHttpHandler, IRequiresSessionState
//    {

//        AshxResponse resp = new AshxResponse();
//        BLLJIMP.BLL bll;
//        public void ProcessRequest(HttpContext context)
//        {
//            context.Response.ContentType = "text/plain";
//            context.Response.Expires = 0;
//            string result = "false";
//            try
//            {
//                bll = new BLLJIMP.BLL();
//                string Action = context.Request["Action"];
//                switch (Action)
//                {
                  
//                    case "Login"://添加基地
//                        result = Login(context);
//                        break;
//                    case "GetBaseList"://获取基地列表
//                        result = GetBaseList(context);
//                        break;
//                    case "GetCompanyList"://获取企业列表
//                        result = GetCompanyList(context);
//                        break;
//                    case "GetProjectList"://获取项目列表
//                        result = GetProjectList(context);
//                        break;

                   

      

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
//        /// <summary>
//        /// 基地 企业登录
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string Login(HttpContext context)
//        { 
//        int UserType=int.Parse(context.Request["UserType"]);
//        string UserName=context.Request["UserName"];
//        string PassWord=context.Request["Password"];
//        switch (UserType)
//        {   
//            case 0://基地登录
//                var baseInfo=bll.Get<WBBaseInfo>(string.Format("UserId='{0}' And Password='{1}'",UserName,PassWord));
//                if (baseInfo==null)
//                {
//                    resp.Msg = "用户名或密码错误";
                   
//                }
//                else if(baseInfo.IsDisable.Equals(1))
//                {
//                     resp.Msg = "您的账户已经被禁用,请联系管理员";
//                }
//                else if (baseInfo!=null&&baseInfo.IsDisable.Equals(0))
//                {
//                    resp.Status = 1;
//                    context.Session[Comm.SessionKey.WangBangUserID] = baseInfo.UserId;
//                    context.Session[Comm.SessionKey.WangBangUserType] = UserType;
//                }
//                break;
//            case 1://企业登录
//                var CompanyInfo=bll.Get<WBCompanyInfo>(string.Format("UserId='{0}' And Password='{1}'",UserName,PassWord));
//                if (CompanyInfo == null)
//                {
//                    resp.Msg = "用户名或密码错误";
                   
//                }
//                else if (CompanyInfo.IsDisable.Equals(1))
//                {
//                     resp.Msg = "您的账户已经被禁用,请联系管理员";
//                }
//                else if (CompanyInfo!= null && CompanyInfo.IsDisable.Equals(0))
//                {
//                    resp.Status = 1;
//                    context.Session[Comm.SessionKey.WangBangUserID] = CompanyInfo.UserId;
//                    context.Session[Comm.SessionKey.WangBangUserType] = UserType;
//                }
//                break;

//        }

//        return Common.JSONHelper.ObjectToJson(resp);
       
        
        
        
//        }

//        /// <summary>
//        /// 获取基地列表 
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string GetBaseList(HttpContext context)
//        {
         
//            int PageIndex = int.Parse(context.Request["PageIndex"]);
//            int PageSize = int.Parse(context.Request["PageSize"]);
//            string BaseName = context.Request["BaseName"];
//            string Area = context.Request["Area"];
//            int totalcount = 0;
//            StringBuilder sbWhere = new StringBuilder(" IsDisable=0");
//            if (!string.IsNullOrEmpty(BaseName))
//            {
//                sbWhere.AppendFormat(" And BaseName like '%{0}%'", BaseName);
//            }
//            if (!string.IsNullOrEmpty(Area))
//            {
//                sbWhere.AppendFormat(" And Area='{0}'", Area);
//            }

//            totalcount = bll.GetCount<WBBaseInfo>(sbWhere.ToString());
//            List<WBBaseInfo> data = bll.GetLit<WBBaseInfo>(PageSize, PageIndex, sbWhere.ToString(), " AutoID DESC");
//            for (int i = 0; i < data.Count; i++)
//            {
//                data[i].UserId = null;
//                data[i].Password = null;
//                data[i].Address = null;
//                data[i].Tel = null;
//                data[i].Phone = null;
//                data[i].QQ = null;
//                data[i].Acreage = null;
//                data[i].Introduction = null;
                

//            }
//            resp.ExObj = data;
//            resp.ExStr = "";
//            int totalpage = bll.GetTotalPage(totalcount, PageSize);
//            if ((totalpage > PageIndex) && (PageIndex.Equals(1)))
//            {
//                resp.ExStr = "1";//是否增加下一页按钮
//            }
//            return Common.JSONHelper.ObjectToJson(resp);



//        }

//        /// <summary>
//        /// 获取企业列表 
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string GetCompanyList(HttpContext context)
//        {

//            int PageIndex = int.Parse(context.Request["PageIndex"]);
//            int PageSize = int.Parse(context.Request["PageSize"]);
//            string CompanyName = context.Request["CompanyName"];
//            string Area = context.Request["Area"];
//            int totalcount = 0;
//            StringBuilder sbWhere = new StringBuilder(" IsDisable=0");
//            if (!string.IsNullOrEmpty(CompanyName))
//            {
//                sbWhere.AppendFormat(" And CompanyName like '%{0}%'", CompanyName);
//            }
//            if (!string.IsNullOrEmpty(Area))
//            {
//                sbWhere.AppendFormat(" And Area='{0}'", Area);
//            }

//            totalcount = bll.GetCount<WBCompanyInfo>(sbWhere.ToString());
//            List<WBCompanyInfo> data = bll.GetLit<WBCompanyInfo>(PageSize, PageIndex, sbWhere.ToString(), " AutoID DESC");
//            for (int i = 0; i < data.Count; i++)
//            {
//                data[i].UserId = null;
//                data[i].Password = null;
//                data[i].Address = null;
//                data[i].Tel = null;
//                data[i].Phone = null;
//                data[i].QQ = null;
//                data[i].BusinessLicenseNumber = null;
//                data[i].Introduction = null;

//            }
//            resp.ExObj = data;
//            resp.ExStr = "";
//            int totalpage = bll.GetTotalPage(totalcount, PageSize);
//            if ((totalpage > PageIndex) && (PageIndex.Equals(1)))
//            {
//                resp.ExStr = "1";//是否增加下一页按钮
//            }
//            return Common.JSONHelper.ObjectToJson(resp);



//        }

//        /// <summary>
//        /// 获取项目列表 
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public string GetProjectList(HttpContext context)
//        {

//            int PageIndex = int.Parse(context.Request["PageIndex"]);
//            int PageSize = int.Parse(context.Request["PageSize"]);
//            string ProjectName = context.Request["ProjectName"];
//            string Area = context.Request["Area"];
//            string Category = context.Request["Category"];
//            int totalcount = 0;
//            StringBuilder sbWhere = new StringBuilder(" Status=1");
//            if (!string.IsNullOrEmpty(ProjectName))
//            {
//                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", ProjectName);
//            }
//            if (!string.IsNullOrEmpty(Area))
//            {
//                sbWhere.AppendFormat(" And Area='{0}'", Area);
//            }
//            if (!string.IsNullOrEmpty(Category))
//            {
//                sbWhere.AppendFormat(" And Category='{0}'", Category);
//            }
//            totalcount = bll.GetCount<WBProjectInfo>(sbWhere.ToString());
//            List<WBProjectInfo> data = bll.GetLit<WBProjectInfo>(PageSize, PageIndex, sbWhere.ToString(), " AutoID DESC");
//            for (int i = 0; i < data.Count; i++)
//            {
//                data[i].UserId = null;
//                data[i].Logistics =0;
//                data[i].TimeRequirement = null;
//                data[i].Introduction = null;

//            }
//            resp.ExObj = data;
//            resp.ExStr = "";
//            int totalpage = bll.GetTotalPage(totalcount, PageSize);
//            if ((totalpage > PageIndex) && (PageIndex.Equals(1)))
//            {
//                resp.ExStr = "1";//是否增加下一页按钮
//            }
//            return Common.JSONHelper.ObjectToJson(resp);



//        }



//        public bool IsReusable
//        {
//            get
//            {
//                return false;
//            }
//        }
//    }
//}