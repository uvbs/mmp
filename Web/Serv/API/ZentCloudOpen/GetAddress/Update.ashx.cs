using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.GetAddress
{
    /// <summary>
    /// 更新自提点
    /// </summary>
    public class Update : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            bllMall.ToLog("进入 update 接口", "D:\\jikudevlog.txt");
            string getAddressId = HttpUtility.UrlDecode(context.Request["id"]);
            string getAddressName = HttpUtility.UrlDecode(context.Request["name"]);
            string getAddressLocation = context.Request["address"];
            string imgUrl = HttpUtility.UrlDecode(context.Request["imgurl"]);

            bllMall.ToLog(string.Format("进入 update 接口 imgUrl{0} getAddressId{1}  getAddressName {2} getAddressLocation{3}  ",
                imgUrl,
                getAddressId,
                getAddressName,
                getAddressLocation
                ), "D:\\jikudevlog.txt");

            if (string.IsNullOrEmpty(getAddressId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "id 参数必传";

                bllMall.ToLog("进入 update 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(getAddressName))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "name 参数必传";

                bllMall.ToLog("进入 update 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(getAddressLocation))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "address 参数必传";
                bllMall.ToLog("进入 update 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                
                return;
            }
            string msg = "";
            bool result = bllMall.UpdateGetAddress(getAddressId, getAddressName, getAddressLocation, out msg, imgUrl);
            if (result)
            {

                resp.status = true;
                resp.msg = "ok";
            }
            else
            {
                //resp.msg = msg;
                //resp.code = (int)APIErrCode.OperateFail;
                resp.status = bllMall.AddGetAddress(getAddressId, getAddressName, getAddressLocation, out msg, imgUrl);
                if (resp.status)
                {
                    resp.msg = "ok";
                }

            }

            bllMall.ToLog("进入 update 接口结束 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }


    }
}