using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Company
{
    /// <summary>
    /// 企业名录
    /// </summary>
    public class Company : BaseHandler
    {

        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And ComPanyName like '%{0}%'", keyWord);
            }
            int totalCount = bll.GetCount<CompanyInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<CompanyInfo>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in sourceData
                       select new
                       {
                           company_id=p.CompanyId,
                           company_logo=bll.GetImgUrl(p.CompanyLogo),
                           company_name=p.ComPanyName,
                           company_tel=p.LinkTel,
                           company_natrue = p.Nature,
                           company_director = p.Director,
                           company_websiteurl = p.WebsiteUrl
                       };

            var data = new
            {
                totalcount = totalCount,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取公司详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string companyId=context.Request["company_id"];
            if (string.IsNullOrEmpty(companyId))
            {
                resp.errcode = 1;
                resp.errmsg = "company_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var companyInfo = bll.Get<CompanyInfo>(string.Format("CompanyId={0}",companyId));
            return ZentCloud.Common.JSONHelper.ObjectToJson(new {
                company_id =companyInfo.CompanyId,
                company_logo = bll.GetImgUrl(companyInfo.CompanyLogo),
                company_name = companyInfo.ComPanyName,
                company_tel = companyInfo.LinkTel,
                comoany_introduction=companyInfo.Introduction,
                company_natrue = companyInfo.Nature,
                company_director = companyInfo.Director,
                company_websiteurl = companyInfo.WebsiteUrl
            });

        }

    }
}