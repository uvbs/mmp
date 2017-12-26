using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Supplier
{
    /// <summary>
    /// 根据供应商账号查询
    /// </summary>
    public class GetByUserId : BaseHandlerNoAction
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {


                string userId = context.Request["userId"];
                var userInfo = bllUser.GetUserInfo(userId, bllUser.WebsiteOwner);
                if (userInfo != null && userInfo.UserType == 7)
                {
                    apiResp.status = true;
                    apiResp.result = new
                    {
                        id = userInfo.AutoID,
                        user_id = userInfo.UserID,
                        head_img_url = userInfo.WXHeadimgurl,
                        image = userInfo.Images,
                        ex1 = userInfo.Ex1,
                        ex2=userInfo.Ex2,//供应商代码
                        ex3=userInfo.Ex3,
                        ex4=userInfo.Ex4,//提醒
                        company=userInfo.Company,
                        desc=userInfo.Description.Replace("\n","<br/>")
                    };


                }
                else
                {
                    apiResp.msg = "专柜码错误";
                }
            }
            catch (Exception)
            {

                apiResp.msg = "专柜码错误";
            }
            bllUser.ContextResponse(context, apiResp);




        }


    }
}