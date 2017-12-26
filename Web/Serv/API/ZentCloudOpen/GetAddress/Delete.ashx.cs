using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.GetAddress
{
    /// <summary>
    /// 删除自提点
    /// </summary>
    public class Delete : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            bllMall.ToLog("进入 delete 接口", "D:\\jikudevlog.txt");

            //resp.code = (int)APIErrCode.OperateFail;
            //resp.msg = "暂时不支持删除";
            //context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //return;

            string getAddressIds = context.Request["ids"];
            if (string.IsNullOrEmpty(getAddressIds))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "ids 参数必传";
                bllMall.ToLog("进入 delete 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            string msg = "";
            bool result = bllMall.DeleteGetAddress(getAddressIds, out msg);
            if (result)
            {

                resp.status = true;
                resp.msg = "ok";
            }
            else
            {
                resp.msg = msg;
                resp.code = (int)APIErrCode.OperateFail;
            }
            bllMall.ToLog("进入 delete 接口结束 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }


    }
}