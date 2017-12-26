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
    /// 获取微信模板
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string AutoId = context.Request["id"];
            KeyVauleDataInfo KeyVauleDataModel = bllKeyValueData.GetByKey<KeyVauleDataInfo>("AutoId", AutoId);
            if (KeyVauleDataModel == null)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "模板没有找到";
                bllKeyValueData.ContextResponse(context, resp);
                return;
            }
            List<KeyVauleDataInfo> oldFieldList = bllKeyValueData.GetKeyVauleDataInfoList(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData), KeyVauleDataModel.DataKey
                , KeyVauleDataModel.WebsiteOwner);

            resp.isSuccess = true;
            resp.returnObj = new
            {
                id = KeyVauleDataModel.AutoId,
                data_key = KeyVauleDataModel.DataKey,
                data_value = KeyVauleDataModel.DataValue,
                child_list = (from p in oldFieldList
                        select new
                        {
                            data_key = p.DataKey,
                            data_value = p.DataValue
                        })
            };
            bllKeyValueData.ContextResponse(context, resp);
        }
    }
}