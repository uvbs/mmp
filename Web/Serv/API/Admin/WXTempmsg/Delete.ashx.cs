using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.WXTempmsg
{
    /// <summary>
    ///添加微信模板
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrWhiteSpace(ids))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "ids不能为空";
                bllKeyValueData.ContextResponse(context, resp);
                return;
            }
            List<KeyVauleDataInfo> dataList = bllKeyValueData.GetListByKey<KeyVauleDataInfo>("AutoId", ids);
            try
            {
                foreach (var item in dataList)
                {
                    bllKeyValueData.DeleteDataVaule(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData), null, item.DataKey, item.WebsiteOwner);
                    bllKeyValueData.DeleteDataVaule(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsg), item.DataKey, item.WebsiteOwner);  
                }
                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            bllKeyValueData.ContextResponse(context, resp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}