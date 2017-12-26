using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// Sort 的摘要说明
    /// </summary>
    public class Sort : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        private string BatchSetSort(HttpContext context)
        {
            string sort = context.Request["sort"];
            string pids = context.Request["pids"];
            if (string.IsNullOrEmpty(sort))
            {
                apiResp.msg = "排序不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (string.IsNullOrEmpty(pids))
            {
                apiResp.msg = "商品ID不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            int count = bllMall.Update(new WXMallProductInfo(), string.Format(" Sort={0}", int.Parse(sort)), string.Format(" WebsiteOwner='{0}' AND PID IN ({1})", bllMall.WebsiteOwner, pids));
            if (count > 0)
            {
                BLLRedis.ClearProductByIds(bllMall.WebsiteOwner, pids, false);
                BLLRedis.ClearProductList(bllMall.WebsiteOwner);
                apiResp.msg = "操作完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "操作出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }
    }
}