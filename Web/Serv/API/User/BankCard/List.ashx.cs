using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.BankCard
{
    /// <summary>
    /// List 的摘要说明 获取绑定银行卡列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            var list=bll.GetList<BLLJIMP.ModelGen.BankCard>(string.Format(" WebsiteOwner='{0}' AND UserID='{1}'",bll.WebsiteOwner,CurrentUserInfo.UserID));
            if (list.Count <= 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有绑定银行卡";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            resp.isSuccess = true;
            List<dynamic> returnList=new List<dynamic>();
            foreach (var item in list)
	        {
		        returnList.Add(new {
                    bank_card_id=item.BankCardID,
                    bank_name=item.BankName,
                    bank_card_number=item.BankCardNumber
                });
	        }
            resp.returnObj = new 
            {
                list=returnList
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            
        }
    }
}