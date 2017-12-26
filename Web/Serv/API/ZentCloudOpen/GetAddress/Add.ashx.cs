using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.GetAddress
{
    /// <summary>
    /// 增加自提点
    /// </summary>
    public class Add : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)

        {
            bllMall.ToLog("进入 add 接口", "D:\\jikudevlog.txt");

            string getAddressName = HttpUtility.UrlDecode(context.Request["name"]);
            string getAddressLocation= HttpUtility.UrlDecode(context.Request["address"]);
            string imgUrl = HttpUtility.UrlDecode(context.Request["imgurl"]);

            if (string.IsNullOrEmpty(getAddressName))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "name 参数必传";
                bllMall.ToLog("进入 add 接口异常:" + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(getAddressLocation))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "address 参数必传";
                bllMall.ToLog("进入 add 接口异常:" + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            string getAddressId = bllMall.GetGUID(BLLJIMP.TransacType.CommAdd);
            string msg = "";
            bool result= bllMall.AddGetAddress(getAddressId, getAddressName, getAddressLocation, out msg,imgUrl);
            if(result){

                resp.status = true;
                resp.msg = "ok";
                resp.result = new { 
                id=getAddressId
                };
            }
            else
            {
                resp.msg = msg;
                resp.code = (int)APIErrCode.OperateFail;
            }
            bllMall.ToLog("进入 add 接口结束 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }

        
    }
}