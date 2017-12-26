using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.GetAddress
{
    /// <summary>
    /// 获取单个自提点
    /// </summary>
    public class Get : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            string getAddressId = context.Request["id"];
            if (string.IsNullOrEmpty(getAddressId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var  model = bllMall.GetGetAddress(getAddressId);
            if (model!=null)
            {

                resp.status = true;
                resp.msg = "ok";
                resp.result = new
                {
                    id = model.GetAddressId,
                    name = model.GetAddressName,
                    address = model.GetAddressLocation,
                    is_disable = model.IsDisable,
                    imgurl = model.ImgUrl
                };
            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "自提点不存在";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }



    }
}