using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.QuotationInfo
{
    /// <summary>
    /// 获取我的报价单列表
    /// </summary>
    public class List : CarBaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }
                
                var pageSize = Convert.ToInt32(context.Request["pageSize"]);
                var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);

                int totalCount = 0;

                pageSize = pageSize == 0 ? 20 : pageSize;
                pageIndex = pageIndex == 0 ? 1 : pageIndex;

                var list = bll.QueryCarQuotationInfo(out totalCount, pageSize, pageIndex, bll.GetCurrUserID(), "");

                List<dynamic> returnList = new List<dynamic>();

                resp.isSuccess = true;

                foreach (var item in list)
                {
                    //TODO:判断如果活动已结束，而且当前用户也未报名，则显示状态为已过期

                    returnList.Add(new
                    {
                        quotationId = item.QuotationId,
                        status = item.Status,
                        createTime = item.CreateTime.ToString(),
                        carBrandName = item.CarBrandName,
                        carSeriesName = item.CarSeriesName,
                        carModelName = item.CarModelName,
                        carModelId = item.CarModelId
                    });
                }

                resp.returnObj = new
                {
                    totalCount = totalCount,
                    list = returnList
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

    }
}