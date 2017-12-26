using CommonPlatform.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.WXTempmsg
{
    /// <summary>
    /// 修改微信模板
    /// </summary>
    public class Put : BaseHandlerNeedLoginAdminNoAction
    {

        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = JsonConvert.DeserializeObject<RequestModel>(context.Request["data"]);
            }
            catch (Exception ex)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
                bllKeyValueData.ContextResponse(context, resp);
                return;
            }
            KeyVauleDataInfo KeyVauleDataModel = new KeyVauleDataInfo();
            if (requestModel.id > 0)
            {
                KeyVauleDataModel = bllKeyValueData.GetByKey<KeyVauleDataInfo>("AutoId", requestModel.id.ToString());
                if (KeyVauleDataModel == null)
                {
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "原模板没有找到";
                    bllKeyValueData.ContextResponse(context, resp);
                    return;
                }
                if (KeyVauleDataModel.WebsiteOwner != bllKeyValueData.WebsiteOwner)
                {
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "原模板不是本站模板";
                    bllKeyValueData.ContextResponse(context, resp);
                    return;
                }
            }
            else
            {
                KeyVauleDataModel.Creater = currentUserInfo.UserID;
                KeyVauleDataModel.WebsiteOwner = bllKeyValueData.WebsiteOwner;
                KeyVauleDataModel.CreateTime = DateTime.Now;
                KeyVauleDataModel.PreKey = "0";
                KeyVauleDataModel.DataType = EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsg);
            }
            string OldKey = KeyVauleDataModel.DataKey;
            KeyVauleDataModel.DataKey = requestModel.data_key;
            KeyVauleDataModel.DataValue = requestModel.data_value;

            //微信模板Id变化则清除以前的字段数据
            if (!string.IsNullOrWhiteSpace(OldKey) && OldKey != KeyVauleDataModel.DataKey)
            {
                bllKeyValueData.DeleteDataVaule(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData), null, OldKey, bllKeyValueData.WebsiteOwner);
            }

            List<KeyVauleDataInfo> newFieldList = new List<KeyVauleDataInfo>();
            for (int i = 0; i < requestModel.child_list.Count; i++)
            {
                KeyVauleDataInfo newField = new KeyVauleDataInfo();
                newField.Creater = currentUserInfo.UserID;
                newField.WebsiteOwner = KeyVauleDataModel.WebsiteOwner;
                newField.CreateTime = DateTime.Now;
                newField.PreKey = KeyVauleDataModel.DataKey;
                newField.DataType = EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData);
                newField.DataKey = requestModel.child_list[i].data_key;
                newField.DataValue = requestModel.child_list[i].data_value;
                newField.OrderBy = i + 1;
                newFieldList.Add(newField);
            }
            List<KeyVauleDataInfo> oldFieldList = bllKeyValueData.GetKeyVauleDataInfoList(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData), KeyVauleDataModel.DataKey
                , KeyVauleDataModel.WebsiteOwner);

            List<KeyVauleDataInfo> deleteFieldList = new List<KeyVauleDataInfo>();
            List<KeyVauleDataInfo> editFieldList = new List<KeyVauleDataInfo>();
            List<KeyVauleDataInfo> addFieldList = new List<KeyVauleDataInfo>();

            foreach (KeyVauleDataInfo item in oldFieldList)
            {
                KeyVauleDataInfo temp = newFieldList.FirstOrDefault(p => p.DataType == item.DataType && p.DataKey == item.DataKey);
                if (temp == null)
                {
                    deleteFieldList.Add(item);
                }
                else
                {
                    item.DataValue = temp.DataValue;
                    item.OrderBy = temp.OrderBy;
                    editFieldList.Add(item);
                }
            }

            foreach (KeyVauleDataInfo item in newFieldList)
            {
                if (!oldFieldList.Exists(p => p.DataType == item.DataType && p.DataKey == item.DataKey))
                {
                    addFieldList.Add(item);
                }
            }


            BLLTransaction tran = new BLLTransaction();//事务
            try
            {
                if (KeyVauleDataModel.AutoId == 0)
                {
                    if (!bllKeyValueData.Add(KeyVauleDataModel, tran))
                    {
                        tran.Rollback();
                        resp.errmsg = "添加模板失败";
                        bllKeyValueData.ContextResponse(context, resp);
                        return;
                    }
                }
                else
                {
                    if (!bllKeyValueData.Update(KeyVauleDataModel, tran))
                    {
                        tran.Rollback();
                        resp.errcode = (int)APIErrCode.OperateFail;
                        resp.errmsg = "修改模板失败";
                        bllKeyValueData.ContextResponse(context, resp);
                        return;
                    }
                }

                if (deleteFieldList.Count > 0)
                {
                    string delIds = MyStringHelper.ListToStr(deleteFieldList.Select(p => p.AutoId).ToList(), "", ",");
                    if (bllKeyValueData.DeleteMultByKey<KeyVauleDataInfo>("AutoId", delIds) < 0)
                    {
                        tran.Rollback();
                        resp.errcode = (int)APIErrCode.OperateFail;
                        resp.errmsg = "删除旧字段失败";
                        bllKeyValueData.ContextResponse(context, resp);
                        return;
                    }
                }

                foreach (KeyVauleDataInfo item in editFieldList)//添加问题表
                {
                    if (!bllKeyValueData.Update(item, tran))
                    {
                        tran.Rollback();
                        resp.errcode = (int)APIErrCode.OperateFail;
                        resp.errmsg = "模板字段修改失败";
                        bllKeyValueData.ContextResponse(context, resp);
                        return;
                    }
                }
                foreach (KeyVauleDataInfo item in addFieldList)//添加问题表
                {
                    if (!bllKeyValueData.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.errcode = (int)APIErrCode.OperateFail;
                        resp.errmsg = "模板字段添加失败";
                        bllKeyValueData.ContextResponse(context, resp);
                        return;
                    }
                }
                tran.Commit();
                resp.isSuccess = true;
                resp.errcode = (int)APIErrCode.IsSuccess;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "提交失败，" + ex.Message;
            }
            bllKeyValueData.ContextResponse(context, resp);
        }
        public class RequestModel
        {
            /// <summary>
            /// id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 模板微信id
            /// </summary>
            public string data_key { get; set; }
            /// <summary>
            /// 模板名称
            /// </summary>
            public string data_value { get; set; }

            public List<RequestChildModel> child_list { get; set; }
        }

        public class RequestChildModel
        {
            /// <summary>
            /// 对应字段Key
            /// </summary>
            public string data_key { get; set; }
            /// <summary>
            /// 对应微信模板字段
            /// </summary>
            public string data_value { get; set; }
        }
    }
}