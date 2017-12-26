using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Saller
{
    /// <summary>
    /// 商家评价列表
    /// </summary>
    public class SallerRateList : UserBaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                //if (!bll.IsLogin)
                //{
                //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                //    bll.ContextResponse(context, resp);
                //    return;
                //}

                var pageSize = Convert.ToInt32(context.Request["pageSize"]);
                var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
                var sallerId = context.Request["sallerId"];

                int totalCount = 0;

                var list = bllSaller.GetSallerRateList(sallerId, pageSize == 0 ? 20 : pageSize, pageIndex, out totalCount);

                List<dynamic> returnList = new List<dynamic>();

                resp.isSuccess = true;

                foreach (var item in list)
                {
                    var rateUser = bllSaller.GetUserInfo(item.MainId);

                    returnList.Add(new
                    {
                        userId = item.MainId,
                        userName = bllSaller.GetUserDispalyName(rateUser),
                        createTime = item.RelationTime.ToString(),
                        comment = item.Ex3,
                        reputationScore = item.Ex1,
                        serviceAttitudeScore = item.Ex2
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