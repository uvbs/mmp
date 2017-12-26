using System;
using System.Collections.Generic;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Saller
{
    /// <summary>
    /// 查询商户列表
    /// </summary>
    public class List : UserBaseHandler
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
                var keyword = context.Request["keyword"];

                int totalCount = 0;

                var list = bll.GetUserList(pageSize, pageIndex, keyword, bll.WebsiteOwner, out totalCount, "5");

                List<dynamic> returnList = new List<dynamic>();

                resp.isSuccess = true;

                foreach (var item in list)
                {
                    returnList.Add(new
                    {
                        id = item.UserID,
                        companyName = item.Company,
                        province = item.Province,
                        city = item.City,
                        district = item.District,
                        address = item.Address

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