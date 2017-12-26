using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLUser bllUser = new BLLUser();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["id"];
            string chk_friend = context.Request["chk_friend"];
            
            if (string.IsNullOrEmpty(autoId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserInfo model = bllUser.GetUserInfoByAutoID(int.Parse(autoId));

            if (model == null || (model.UserType!=1 &&model.WebsiteOwner!=bllUser.WebsiteOwner))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "用户不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            bool isFriend = false;
            if (chk_friend == "1")
            {
                UserInfo curUser = bllUser.GetCurrentUserInfo();
                if (curUser != null)
                {
                    if (model.AutoID == curUser.AutoID)
                    {
                        isFriend = true;
                    }
                    else
                    {
                        isFriend = bllCommRelation.ExistRelation(CommRelationType.Friend, model.AutoID.ToString(), curUser.AutoID.ToString());
                    }
                }
            }

            resp.isSuccess = true;
            resp.returnObj = new 
            {
                id = model.AutoID,
                user_name = model.UserID,
                nick_name = bllUser.GetUserDispalyName(model),
                avatar = model.Avatar,
                head_img_url = bllUser.GetUserDispalyAvatar(model),
                last_login_date = DateTimeHelper.DateTimeToUnixTimestamp(model.LastLoginDate),
                last_login_date_str = DateTimeHelper.DateTimeToString(model.LastLoginDate),
                birthday = DateTimeHelper.DateTimeToUnixTimestamp(model.Birthday),
                birthday_str = DateTimeHelper.DateTimeToString(model.Birthday),
                identification=model.Ex5,
                gender=model.Gender,
                describe=model.Description,
                city = model.City,
                province = model.Province,
                salary = model.Salary,
                imgs=model.Images,
                ex2=model.Ex2,
                ex3=model.Ex3,
                ex4 = model.Ex4,
                credit_acount = model.CreditAcount,
                distance=model.Distance,
                view = model.ViewType,
                phone = model.ViewType == 0 ? model.Phone : "",
                is_friend = isFriend,
                score = model.TotalScore,
                times = model.OnlineTimes
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}