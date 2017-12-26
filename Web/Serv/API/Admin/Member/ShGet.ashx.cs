using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ShGet 的摘要说明
    /// </summary>
    public class ShGet : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bllUser.WebsiteOwner;
            UserInfo u = bllUser.GetColByKey<UserInfo>("AutoID", id, "AutoID,UserID,Phone1,IdentityCard,Province"+
                ",ProvinceCode,City,CityCode,District,DistrictCode,Town,TownCode,MemberLevel,MemberStartTime,Address"+
                ",Ex2,Ex3,Ex4,Ex5,Ex6,Ex7,Ex8,Ex9,Ex10,TotalAmount,AccountAmountEstimate,AccumulationFund", websiteOwner: websiteOwner);

            List<BindBankCard> cards = bllUser.GetListByKey<BindBankCard>("UserId",u.UserID);

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询列表完成";
            apiResp.result = new{
                id = u.AutoID,
                province = u.Province,
                province_code = u.ProvinceCode,
                city = u.City,
                city_code = u.CityCode,
                district = u.District,
                district_code = u.DistrictCode,
                town = u.Town,
                town_code = u.TownCode,
                address = u.Address,
                ex2 = u.Ex2,
                ex3 = u.Ex3,
                ex4 = u.Ex4,
                ex5 = u.Ex5,
                ex6 = u.Ex6,
                ex7 = u.Ex7,
                ex8 = u.Ex8,
                ex9 = u.Ex9,
                ex10 = u.Ex10,
                amout = u.TotalAmount,
                estimate = u.AccountAmountEstimate,
                fund = u.AccumulationFund,
                phone1 = u.Phone1,
                idcard = u.IdentityCard,
                member_stime = u.MemberStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                bank_cards = from p in cards
                            select new
                            {
                                id = p.AutoID,
                                account_name = p.AccountName,
                                bank_account = p.BankAccount,
                                bank_name = p.BankName
                            }
            };
            bllUser.ContextResponse(context, apiResp);
        }
    }
}