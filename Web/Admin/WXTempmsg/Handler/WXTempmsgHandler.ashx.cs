using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Admin.WXTempmsg.Handler
{
    /// <summary>
    /// 微信模板消息后台处理文件
    /// </summary>
    public class WXTempmsgHandler : IHttpHandler, IReadOnlySessionState
    {
        DefaultResponse resp = new DefaultResponse();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain"; context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllKeyValueData.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.errcode = (int)APIErrCode.UserIsNotLogin;
                    resp.errmsg = "用户未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string action = context.Request["Action"];
                switch (action)
                {
                    case "PostWXTempmsg":
                        result = PostWXTempmsg(context);
                        break;
                    case "GetWXTempmsg":
                        result = GetWXTempmsg(context);
                        break;
                    case "List":
                        result = List(context);
                        break;
                    case "Add":
                        result = Add(context);
                        break;
                    case "Update":
                        result = Update(context);
                        break;
                    case "Delete":
                        result = Delete(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }
         /// <summary>
        /// 提交模板数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string PostWXTempmsg(HttpContext context)
        {
            int autoId = Convert.ToInt32(context.Request["AutoId"]);
            KeyVauleDataInfo keyValue = new KeyVauleDataInfo();
            if (autoId > 0)
            {
                keyValue =bllKeyValueData.GetKeyData(autoId);
                if (keyValue == null)
                {
                    resp.errmsg = "原模板没有找到";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (keyValue.WebsiteOwner != bllKeyValueData.WebsiteOwner)
                {
                    resp.errmsg = "原模板不是本站模板";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            else
            {
                keyValue.Creater = currentUserInfo.UserID;
                keyValue.WebsiteOwner = bllKeyValueData.WebsiteOwner;
                keyValue.CreateTime = DateTime.Now;
                keyValue.PreKey = "0";
                keyValue.DataType = EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsg);
            }

            string oldKey = keyValue.DataKey;
            keyValue = bllKeyValueData.ConvertRequestToModel<KeyVauleDataInfo>(keyValue);
            //微信模板Id变化则清除以前的字段数据
            if (!string.IsNullOrWhiteSpace(oldKey) && oldKey != keyValue.DataKey)
            {
                bllKeyValueData.DeleteDataVaule(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData),null,oldKey,bllKeyValueData.WebsiteOwner);
            }

            string keyFieldsJson = context.Request["KeyFields"];
            List<KeyVauleDataInfo> newFieldList = Common.JSONHelper.JsonToModel<List<KeyVauleDataInfo>>(keyFieldsJson);//jSON 反序列化
            for (int i = 0; i < newFieldList.Count; i++)
            {
                newFieldList[i].DataType = EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData);
            }
            List<KeyVauleDataInfo> oldFieldList = bllKeyValueData.GetKeyVauleDataInfoList(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData), keyValue.DataKey
                , bllKeyValueData.WebsiteOwner);


            List<KeyVauleDataInfo> deleteFieldList = new List<KeyVauleDataInfo>();
            List<KeyVauleDataInfo> editFieldList = new List<KeyVauleDataInfo>();
            List<KeyVauleDataInfo> addFieldList = new List<KeyVauleDataInfo>();
            foreach (KeyVauleDataInfo item in oldFieldList)
	        {
                KeyVauleDataInfo temp = newFieldList.FirstOrDefault(p => p.DataType == item.DataType && p.DataKey == item.DataKey);
                if (temp == null) {
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
                    item.Creater = currentUserInfo.UserID;
                    item.WebsiteOwner = bllKeyValueData.WebsiteOwner;
                    item.CreateTime = DateTime.Now;
                    item.PreKey = keyValue.DataKey;
                    addFieldList.Add(item);
                }
            }

            if (deleteFieldList.Count > 0)
            {
                string delIds = Common.MyStringHelper.ListToStr(deleteFieldList.Select(p => p.AutoId).ToList(), "", ",");
                bllKeyValueData.DeleteDataVaule(delIds);
            }

            BLLTransaction tran = new BLLTransaction();//事务
            try
            {
                if (keyValue.AutoId == 0)
                {
                    if (!bllKeyValueData.Add(keyValue, tran))
                    {
                        resp.errmsg = "添加模板失败";
                        tran.Rollback();
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                else
                {
                    if (!bllKeyValueData.Update(keyValue, tran))
                    {
                        resp.errmsg = "修改模板失败";
                        tran.Rollback();
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }

                foreach (KeyVauleDataInfo item in editFieldList)//添加问题表
                {
                    if (!bllKeyValueData.Update(item, tran))
                    {
                        resp.errmsg = "模板字段修改失败";
                        tran.Rollback();
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                foreach (KeyVauleDataInfo item in addFieldList)//添加问题表
                {
                    if (!bllKeyValueData.Add(item, tran))
                    {
                        resp.errmsg = "模板字段添加失败";
                        tran.Rollback();
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                tran.Commit();
                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "提交失败，"+ex.Message;
                tran.Rollback();
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWXTempmsg(HttpContext context)
        {
            int autoId = Convert.ToInt32(context.Request["AutoId"]);
            KeyVauleDataInfo keyValue = bllKeyValueData.GetKeyData(autoId);
            if (keyValue == null)
            {
                resp.errmsg = "模板没有找到";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (keyValue.WebsiteOwner != bllKeyValueData.WebsiteOwner)
            {
                resp.errmsg = "不是本站模板";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            List<KeyVauleDataInfo> fieldList = bllKeyValueData.GetKeyVauleDataInfoList(EnumStringHelper.ToString(KeyVauleDataType.WXTmplmsgData), keyValue.DataKey
                , bllKeyValueData.WebsiteOwner);

            var fields = from p in fieldList
                         select new {
                             p.DataKey,
                             p.DataValue
                         };

            resp.returnObj = new
            {
                keyValue.DataValue,
                keyValue.DataKey,
                keyValue.OrderBy,
                KeyFields = fields
            };
            resp.isSuccess = true;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex=int.Parse(context.Request["page"]);
            int pageSize=int.Parse(context.Request["rows"]);
            int total=0;
            List<KeyVauleDataInfo> sourceList = bllKeyValueData.GetKeyVauleDataInfoList(pageSize, pageIndex, "WXTemplateMsg", "",  bllKeyValueData.WebsiteOwner, out  total);
            var list = from p in sourceList
                         select new
                         {
                             AutoID=p.AutoId,
                             TemplateName=p.DataKey,
                             TemplateID=p.DataValue,
                             TemplateType=p.PreKey
                         };

            return Common.JSONHelper.ObjectToJson(new { 
            
            total=total,
            rows=list
            
            });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            string templateName = context.Request["TemplateName"];
            string templateId = context.Request["TemplateId"];
            string templatyType = context.Request["TemplatyType"];
            KeyVauleDataInfo model = new KeyVauleDataInfo();
            model.WebsiteOwner = bllKeyValueData.WebsiteOwner;
            model.CreateTime = DateTime.Now;
            model.DataType = "WXTemplateMsg";
            model.DataKey = templateName;
            model.DataValue = templateId;
            model.PreKey = templatyType;

            if (bllKeyValueData.GetCount<KeyVauleDataInfo>(string.Format(" Websiteowner='{0}' And DataType='WXTemplateMsg' And PreKey='{1}'",bllKeyValueData.WebsiteOwner,model.PreKey))>0)
            {
                resp.errmsg = "类型重复";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            if (bllKeyValueData.Add(model))
            {
                resp.isSuccess = true;
               
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {

            string autoId = context.Request["AutoID"];
            string templateName = context.Request["TemplateName"];
            string templateId = context.Request["TemplateId"];
            string templatyType = context.Request["TemplatyType"];
            KeyVauleDataInfo model = bllKeyValueData.Get<KeyVauleDataInfo>(string.Format(" AutoID={0}",autoId));
            model.DataKey = templateName;
            model.DataValue = templateId;
            model.PreKey = templatyType;
            if (bllKeyValueData.Update(model, string.Format(" DataKey='{0}',DataValue='{1}',PreKey='{2}'",model.DataKey,model.DataValue,model.PreKey), string.Format(" AutoId={0}", model.AutoId)) > 0)
            {
                resp.isSuccess = true;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (bllKeyValueData.Delete(new KeyVauleDataInfo(),string.Format(" AutoId in({0}) And WebsiteOwner='{1}'",ids,bllKeyValueData.WebsiteOwner))==ids.Split(',').Length)
            {
                resp.isSuccess = true;
            }
            return Common.JSONHelper.ObjectToJson(resp);
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