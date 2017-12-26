using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Company
{
    /// <summary>
    /// 企业名录后台管理
    /// </summary>
    public class Company : BaseHandlerNeedLoginAdmin
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
                           company_id = p.CompanyId,
                           company_logo = bll.GetImgUrl(p.CompanyLogo),
                           company_name = p.ComPanyName,
                           company_tel = p.LinkTel,
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
            string companyId = context.Request["company_id"];
            if (string.IsNullOrEmpty(companyId))
            {
                resp.errcode = 1;
                resp.errmsg = "company_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var companyInfo = bll.Get<CompanyInfo>(string.Format("CompanyId={0}", companyId));
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                company_id = companyInfo.CompanyId,
                company_logo = bll.GetImgUrl(companyInfo.CompanyLogo),
                company_name = companyInfo.ComPanyName,
                company_tel = companyInfo.LinkTel,
                comoany_introduction = companyInfo.Introduction,
                company_natrue = companyInfo.Nature,
                company_director = companyInfo.Director,
                company_websiteurl = companyInfo.WebsiteUrl
            });

        }


        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.company_id >= 1)
            {

                resp.errcode = -1;
                resp.errmsg = "company_id固定为0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.company_name))
            {
                resp.errcode = -1;
                resp.errmsg = "company_name必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.company_logo))
            {
                resp.errcode = -1;
                resp.errmsg = "company_logo必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.comoany_introduction))
            {
                resp.errcode = -1;
                resp.errmsg = "comoany_introduction必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.company_tel))
            {
                resp.errcode = -1;
                resp.errmsg = "company_tel必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyInfo model = new CompanyInfo();
            model.CompanyId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
            model.CompanyLogo = requestModel.company_logo;
            model.ComPanyName = requestModel.company_name;
            model.LinkTel = requestModel.company_tel;
            model.Introduction = requestModel.comoany_introduction;
            model.WebsiteOwner = bll.WebsiteOwner;
            model.Nature = requestModel.company_natrue;
            model.Director = requestModel.company_director;
            model.WebsiteUrl = requestModel.company_websiteurl;
            if (bll.Add(model))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加失败";
            }


            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 更新公司信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.company_id <= 0)
            {
                resp.errcode = -1;
                resp.errmsg = "company_id参数错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.company_name))
            {
                resp.errcode = -1;
                resp.errmsg = "company_name必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.company_logo))
            {
                resp.errcode = -1;
                resp.errmsg = "company_logo必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.comoany_introduction))
            {
                resp.errcode = -1;
                resp.errmsg = "comoany_introduction必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.company_tel))
            {
                resp.errcode = -1;
                resp.errmsg = "company_tel必填,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyInfo model = bll.Get<CompanyInfo>(string.Format(" WebSiteOwner='{0}' And CompanyId={1}", bll.WebsiteOwner, requestModel.company_id));
            model.CompanyLogo = requestModel.company_logo;
            model.ComPanyName = requestModel.company_name;
            model.LinkTel = requestModel.company_tel;
            model.Introduction = requestModel.comoany_introduction;
            model.Nature = requestModel.company_natrue;
            model.Director = requestModel.company_director;
            model.WebsiteUrl = requestModel.company_websiteurl;
            if (bll.Update(model))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "编辑失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string companyIds = context.Request["company_ids"];
            if (string.IsNullOrEmpty(companyIds))
            {
                resp.errcode = -1;
                resp.errmsg = "company_ids 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bll.Delete(new CompanyInfo(), string.Format(" WebSiteOwner='{0}' And CompanyId in({1})", bll.WebsiteOwner, companyIds)) == companyIds.Split(',').Length)
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }


            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 请求模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 公司编号
            /// </summary>
            public int company_id { get; set; }
            /// <summary>
            /// 公司LOGO
            /// </summary>
            public string company_logo { get; set; }
            /// <summary>
            /// 公司名称
            /// </summary>
            public string company_name { get; set; }
            /// <summary>
            /// 公司联系电话
            /// </summary>
            public string company_tel { get; set; }
            /// <summary>
            /// 公司详细介绍
            /// </summary>
            public string comoany_introduction { get; set; }

            /// <summary>
            /// 公司性质
            /// </summary>
            public string company_natrue { get; set; }

            /// <summary>
            /// 公司董事
            /// </summary>
            public string company_director { get; set; }

            /// <summary>
            /// 公司网址
            /// </summary>
            public string company_websiteurl { get; set; }

        }


    }
}