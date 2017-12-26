using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["autoid"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.errmsg = "autoid 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserInfo model = bllUser.GetUserInfoByAutoID(int.Parse(autoId), bllUser.WebsiteOwner);
            if (model == null)
            {
                resp.errmsg = "不存在该条信息";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            resp.isSuccess = true;
            resp.returnObj = new
            {
                user_id = model.UserID,
                wx_nick_name = model.WXNickname,
                wx_head_img_url = model.WXHeadimgurl,
                true_name = model.TrueName,
                user_phone = model.Phone,
                user_company = model.Company,
                user_email = model.Email,
                tag_name = model.TagName,
                total_score = model.TotalScore,
                user_position = model.Postion,
                distributionowner = model.DistributionOwner,
                access_level = model.AccessLevel,
                available_vote_count = model.AvailableVoteCount,
                credit_acount = model.CreditAcount,
                avatar = model.Avatar,
                salary = model.Salary,
                ex5 = model.Ex5,
                ex4 = model.Ex4,
                ex3 = model.Ex3,
                ex2 = model.Ex2,
                gender = model.Gender,
                district = model.District,
                birthday = DateTimeHelper.DateTimeToUnixTimestamp(model.Birthday),
                last_login_date = DateTimeHelper.DateTimeToUnixTimestamp(model.LastLoginDate),
                describe=model.Description,
                images=model.Images


            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}