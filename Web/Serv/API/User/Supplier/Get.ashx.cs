using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Supplier
{
    /// <summary>
    /// 根据供应商id查询
    /// </summary>
    public class Get : BaseHandlerNoAction
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

           
            string id=context.Request["id"];
            var userInfo = bllUser.GetUserInfoByAutoID(int.Parse(id));
            if (userInfo!=null&&userInfo.UserType==7)
            {
                apiResp.status = true;
                apiResp.result = new
                {
                    id=userInfo.AutoID,
                    user_id=userInfo.UserID
                    //head_img_url=userInfo.WXHeadimgurl,
                    //image=userInfo.Images,
                    //ex1=userInfo.Ex1
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
            bllUser.ContextResponse(context,apiResp);



           
        }


    }
}