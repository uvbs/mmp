using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Saller
{
    /// <summary>
    /// 获取商户详情
    /// </summary>
    public class Detail : UserBaseHandler
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

                var sallerId = context.Request["sallerId"];

                var data = bll.GetUserInfo(sallerId);

                resp.isSuccess = true;

                //计算商家信誉平均分
                //计算商家服务态度平均分

                var avg = bllSaller.GetSallerRateScoreAvg(sallerId);
                int rateTotalCount = 0;
                var rateList = bllSaller.GetSallerRateList(sallerId, 20, 1, out rateTotalCount);

                List<dynamic> rateListReturn = new List<dynamic>();

                foreach (var item in rateList)
                {
                    var rateUser = bllSaller.GetUserInfo(item.MainId);

                    rateListReturn.Add(new
                    {
                        userId = item.MainId,
                        userName = bllSaller.GetUserDispalyName(rateUser),
                        createTime = item.RelationTime.ToString(),
                        comment = item.Ex3,
                        reputationScore = item.Ex1,
                        serviceAttitudeScore = item.Ex2
                    });
                }

                List<dynamic> linkers = new List<dynamic>()
                {
                    new
                    {
                        type = "PreSaler",
                        name = data.Ex3,
                        position = data.Ex4,
                        phone = data.Ex5,
                        avatar = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port.ToString() + "/img/europejobsites.png"//data.ex6
                    }

                };//商家联系人，后期用扩展表实现，目前先根据字段构造

                
                resp.returnObj = new
                {
                    sallerId = data.UserID,
                    companyName = data.Company,
                    province = data.Province,
                    city = data.City,
                    district = data.District,
                    address = data.Address,

                    //trueName = data.TrueName,
                    //avatar = data.Avatar,
                    //postion = data.Postion,
                    //phone = data.Phone,

                    reputationScore = avg.ReputationScore,
                    serviceAttitudeScore = avg.ServiceAttitudeScore,

                    rateTotalCount = rateTotalCount,
                    rateList = rateListReturn,

                    linkers = linkers,

                    ex1 = data.Ex1,
                    ex2 = data.Ex2,
                    ex3 = data.Ex3,
                    ex4 = data.Ex4,
                    ex5 = data.Ex5,
                    ex6 = data.Ex6,
                    ex7 = data.Ex7
                    
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