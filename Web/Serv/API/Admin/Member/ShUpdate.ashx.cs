using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ShUpdate 的摘要说明
    /// </summary>
    public class ShUpdate : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = bllUser.ConvertRequestToModel<RequestModel>(requestModel);
            }
            catch (Exception ex)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "参数错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo updateUser = bllUser.GetUserInfoByAutoID(requestModel.id);
            if (updateUser == null)
            {
                apiResp.msg = "会员未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            string remark = "修改会员信息：";
            bool updateName = false;
            string oldName = "";
            if (updateUser.TrueName != requestModel.name)
            {
                remark += string.Format(" 姓名[{0}-{1}]", updateUser.TrueName, requestModel.name);
                oldName = updateUser.TrueName;
                updateUser.TrueName = requestModel.name;
                updateName = true;
            }
            if (updateUser.Phone1 != requestModel.phone1)
            {
                remark += string.Format(" 联系手机[{0}-{1}]", updateUser.Phone1, requestModel.phone1);
                updateUser.Phone1 = requestModel.phone1;
            }
            if (updateUser.IdentityCard != requestModel.idcard)
            {
                remark += string.Format(" 身份证[{0}-{1}]", updateUser.IdentityCard, requestModel.idcard);
                updateUser.IdentityCard = requestModel.idcard;
            }
            if (updateUser.Province != requestModel.province || updateUser.City != requestModel.city
                || updateUser.District != requestModel.district || updateUser.Town != requestModel.town)
            {
                string[] li1 = new string[] { updateUser.Province, updateUser.City, updateUser.District, updateUser.Town };
                string[] li2 = new string[] { requestModel.province, requestModel.city, requestModel.district, requestModel.town };
                string ostr1 = string.Join(" ", li1.Where(p => !string.IsNullOrWhiteSpace(p)).ToList());
                string ostr2 = string.Join(" ", li2.Where(p => !string.IsNullOrWhiteSpace(p)).ToList());
                remark += string.Format(" 所在地[{0}-{1}]", ostr1, ostr2);
                updateUser.Province = requestModel.province;
                updateUser.ProvinceCode = requestModel.province_code;
                updateUser.City = requestModel.city;
                updateUser.CityCode = requestModel.city_code;
                updateUser.District = requestModel.district;
                updateUser.DistrictCode = requestModel.district_code;
                updateUser.Town = requestModel.town;
                updateUser.TownCode = requestModel.town_code;
            }
            int stock = requestModel.stock.HasValue ? requestModel.stock.Value : 0;
            if (updateUser.Stock != stock)
            {
                remark += string.Format(" 股权数[{0}-{1}]", updateUser.Stock, stock);
                updateUser.Stock = stock;
            }
            if (updateUser.Address != requestModel.address)
            {
                remark += string.Format(" 地址[{0}-{1}]", updateUser.Address, requestModel.address);
                updateUser.Address = requestModel.address;
            }
            List<string> oEx = new List<string>();
            if (!string.IsNullOrWhiteSpace(updateUser.Ex1)) oEx.Add(updateUser.Ex1);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex2)) oEx.Add(updateUser.Ex2);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex3)) oEx.Add(updateUser.Ex3);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex4)) oEx.Add(updateUser.Ex4);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex5)) oEx.Add(updateUser.Ex5);
            List<string> nEx = new List<string>();
            if (!string.IsNullOrWhiteSpace(requestModel.ex1)) nEx.Add(requestModel.ex1);
            if (!string.IsNullOrWhiteSpace(requestModel.ex2)) nEx.Add(requestModel.ex2);
            if (!string.IsNullOrWhiteSpace(requestModel.ex3)) nEx.Add(requestModel.ex3);
            if (!string.IsNullOrWhiteSpace(requestModel.ex4)) nEx.Add(requestModel.ex4);
            if (!string.IsNullOrWhiteSpace(requestModel.ex5)) nEx.Add(requestModel.ex5);
            string oExStr = ZentCloud.Common.MyStringHelper.ListToStr(oEx, "", ",");
            string nExStr = ZentCloud.Common.MyStringHelper.ListToStr(nEx, "", ",");
            if (oExStr != nExStr)
            {
                remark += string.Format(" 执照[{0} 改为{1}]", oExStr, nExStr);
                updateUser.Ex1 = string.IsNullOrWhiteSpace(requestModel.ex1) ? null : requestModel.ex1;
                updateUser.Ex2 = string.IsNullOrWhiteSpace(requestModel.ex2) ? null : requestModel.ex2;
                updateUser.Ex3 = string.IsNullOrWhiteSpace(requestModel.ex3) ? null : requestModel.ex3;
                updateUser.Ex4 = string.IsNullOrWhiteSpace(requestModel.ex4) ? null : requestModel.ex4;
                updateUser.Ex5 = string.IsNullOrWhiteSpace(requestModel.ex5) ? null : requestModel.ex5;
            }
            oEx = new List<string>();
            if (!string.IsNullOrWhiteSpace(updateUser.Ex6)) oEx.Add(updateUser.Ex6);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex7)) oEx.Add(updateUser.Ex7);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex8)) oEx.Add(updateUser.Ex8);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex9)) oEx.Add(updateUser.Ex9);
            if (!string.IsNullOrWhiteSpace(updateUser.Ex10)) oEx.Add(updateUser.Ex10);
            nEx = new List<string>();
            if (!string.IsNullOrWhiteSpace(requestModel.ex6)) nEx.Add(requestModel.ex6);
            if (!string.IsNullOrWhiteSpace(requestModel.ex7)) nEx.Add(requestModel.ex7);
            if (!string.IsNullOrWhiteSpace(requestModel.ex8)) nEx.Add(requestModel.ex8);
            if (!string.IsNullOrWhiteSpace(requestModel.ex9)) nEx.Add(requestModel.ex9);
            if (!string.IsNullOrWhiteSpace(requestModel.ex10)) nEx.Add(requestModel.ex10);
            oExStr = ZentCloud.Common.MyStringHelper.ListToStr(oEx, "", ",");
            nExStr = ZentCloud.Common.MyStringHelper.ListToStr(nEx, "", ",");
            if (oExStr != nExStr)
            {
                remark += string.Format(" 凭证[{0} 改为{1}]", oExStr, nExStr);
                updateUser.Ex6 = string.IsNullOrWhiteSpace(requestModel.ex6) ? null : requestModel.ex6;
                updateUser.Ex7 = string.IsNullOrWhiteSpace(requestModel.ex7) ? null : requestModel.ex7;
                updateUser.Ex8 = string.IsNullOrWhiteSpace(requestModel.ex8) ? null : requestModel.ex8;
                updateUser.Ex9 = string.IsNullOrWhiteSpace(requestModel.ex9) ? null : requestModel.ex9;
                updateUser.Ex10 = string.IsNullOrWhiteSpace(requestModel.ex10) ? null : requestModel.ex10;
            }
            if (bllUser.Update(updateUser))
            {
                if (remark != "修改会员信息：") bllLog.Add(EnumLogType.ShMember, EnumLogTypeAction.Update, currentUserInfo.UserID, remark, targetID: updateUser.UserID);
                if (updateName)
                {
                    //异步修改积分明细表
                    Thread th1 = new Thread(delegate()
                    {
                        bllUser.Update(new UserScoreDetailsInfo(),
                            string.Format("[AddNote] = REPLACE([AddNote],'{0}[{1}]','{2}[{1}]')", oldName, updateUser.Phone, updateUser.TrueName),
                            string.Format(" WebsiteOwner='{2}' And ScoreType='TotalAmount' And [AddNote] like '%{0}[{1}]%' ", oldName, updateUser.Phone, bllUser.WebsiteOwner));
                        bllUser.Update(new UserScoreDetailsInfo(),
                            string.Format("[AddNote] = REPLACE([AddNote],'{0}','{1}')", oldName, updateUser.TrueName),
                            string.Format(" WebsiteOwner='{1}' And ScoreType='TotalAmount' And [AddNote] like '%{0}%' ", oldName, bllUser.WebsiteOwner));
                    });
                    th1.Start();
                }
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "修改会员信息完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "修改会员信息失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }

        public class RequestModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string phone1 { get; set; }
            public string idcard { get; set; }
            public string province { get; set; }
            public string province_code { get; set; }
            public string city { get; set; }
            public string city_code { get; set; }
            public string district { get; set; }
            public string district_code { get; set; }
            public string town { get; set; }
            public string town_code { get; set; }
            public string address { get; set; }
            public int? stock { get; set; }
            public string ex1 { get; set; }
            public string ex2 { get; set; }
            public string ex3 { get; set; }
            public string ex4 { get; set; }
            public string ex5 { get; set; }
            public string ex6 { get; set; }
            public string ex7 { get; set; }
            public string ex8 { get; set; }
            public string ex9 { get; set; }
            public string ex10 { get; set; }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}