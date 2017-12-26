using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.ModelGen;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Stock
{
    /// <summary>
    /// Add 的摘要说明  添加货存记录
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string pId=context.Request["product_id"];
            string props=context.Request["props"];
            string count=context.Request["num"];
            string pName = context.Request["product_name"];

            if (string.IsNullOrEmpty(pId))
            {
                apiResp.msg = "必填参数为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context,apiResp);
                return;
            }


            UserInfo curUser = bllUser.GetCurrentUserInfo();

            ProductStock stock = new ProductStock();

            stock.ProductId =Convert.ToInt32(pId);
            stock.UserId = curUser.UserID;
            stock.PName = pName;
            stock.CreateDate = DateTime.Now;
            stock.Count = Convert.ToInt32(count);
            stock.WebsiteOwner = bllUser.WebsiteOwner;
            stock.Props = props;
            stock.WXNickname = curUser.WXNickname;
            stock.WXHeadimgurl = curUser.WXHeadimgurl;


            if (bllUser.Add(stock))
            {
                apiResp.msg = "成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context,apiResp);





        }

    }
}