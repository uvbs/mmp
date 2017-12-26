using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.GetAddress
{
    /// <summary>
    /// 更新自提点启用禁用状态
    /// </summary>
    public class UpdateEnableStatus : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            bllMall.ToLog("进入 UpdateEnableStatus 接口", "D:\\jikudevlog.txt");
            string getAddressIds = context.Request["ids"];//
            string isDisable=context.Request["is_disable"];//0 启用 1 禁用
            if (string.IsNullOrEmpty(getAddressIds))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "ids 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(isDisable))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "is_disable 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if ((isDisable!="0")&&(isDisable!="1"))
            {
                resp.msg = " is_disable 参数值不正确";
                resp.code = (int)APIErrCode.OperateFail;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            string msg = "";
            bool result = bllMall.UpdateGetAddressEnableStatus(getAddressIds,int.Parse(isDisable), out msg);
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
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }
    }
}