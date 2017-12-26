using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.CrowdFund
{
    /// <summary>
    /// 众筹模块接口
    /// </summary>
    public class CrowdFund : BaseHandler
    {

        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 获取众筹活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string type = context.Request["crowdfund_type"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And Title like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And Type ={0}", type);
            }
            int totalCount = bll.GetCount<CrowdFundInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<CrowdFundInfo>(pageSize, pageIndex, sbWhere.ToString()," AutoID DESC ");
            var list = from p in sourceData
                       select new
                       {
                           crowdfund_id = p.CrowdFundID,
                           crowdfund_type = p.Type,
                           crowdfund_title = p.Title,
                           crowdfund_img_url = p.CoverImage,
                           crowdfund_pay_amount = p.TotalPayAmount,
                           crowdfund_pay_count = p.PayPersionCount,
                           crowdfund_percent = p.PayPercent
                       };

            var data = new
            {
                totalcount = totalCount,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取众筹活动详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string crowdfundId = context.Request["crowdfund_id"];
            if (string.IsNullOrEmpty(crowdfundId))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format("CrowdFundID={0}", crowdfundId));
            var sourceItemList = bll.GetList<CrowdFundItem>(string.Format("CrowdFundID={0}", crowdfundId));
            var itemList = from p in sourceItemList
                           select new
                           {
                               item_id = p.ItemId,
                               item_amount = p.Amount,
                               item_desc = p.Description,
                               item_productname=p.ProductName,
                               item_pay_amount = bll.GetCount<CrowdFundRecord>(string.Format(" ItemId={0} and Status=1",p.ItemId))
                           };

            var data = new
            {
                crowdfund_id = crowdFundInfo.AutoID,
                crowdfund_type = crowdFundInfo.Type,
                crowdfund_title = crowdFundInfo.Title,
                crowdfund_img_url = crowdFundInfo.CoverImage,
                crowdfund_amount = crowdFundInfo.FinancAmount,
                crowdfund_pay_amount = crowdFundInfo.TotalPayAmount,
                crowdfund_pay_count = crowdFundInfo.PayPersionCount,
                crowdfund_percent = crowdFundInfo.PayPercent,
                crowdfund_originator = crowdFundInfo.Originator,
                crowdfund_intro = crowdFundInfo.Introduction,
                crowdfund_remainingdays= crowdFundInfo.RemainingDays,
                item_list = itemList

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }
    }
}