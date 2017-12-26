using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {

            List<TableFieldMapping> formField = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, context.Request["mapping_type"]);
            formField = formField.Where(p => p.IsReadOnly == 0 && p.IsDelete == 0 && p.Field != "AutoID").ToList();

            if(formField.Count ==0) {
                apiResp.msg = "没有可编辑字段";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllTableFieldMap.ContextResponse(context, apiResp);
                return;
            }

            List<string> limitFields = new List<string>() { "UserID", "Phone", "WXOpenId" };

            #region 默认信息检查 姓名
            string autoID = context.Request["AutoID"];
            if(string.IsNullOrWhiteSpace(autoID) || autoID=="0"){
                apiResp.msg = "用户未找到";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllTableFieldMap.ContextResponse(context, apiResp);
                return;
            }
            UserInfo curUser = bllTableFieldMap.GetByKey<UserInfo>("AutoID", autoID);
            if (curUser == null)
            {
                apiResp.msg = "用户未找到";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllTableFieldMap.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            List<string> pms = new List<string>();

            #region 构造修改字段
            TableFieldMapping userIDField = formField.FirstOrDefault(p => p.Field.Equals("UserID"));
            if (userIDField != null)
            {
                string val = context.Request[userIDField.Field];
                if (!string.IsNullOrWhiteSpace(val))
                {
                    List<UserInfo> oUserList = bllTableFieldMap.GetColList<UserInfo>(int.MaxValue, 1, string.Format("UserID='{0}' And AutoID != {1} ", val, autoID), "AutoID,UserID");
                    if (oUserList.Count > 0)
                    {
                        apiResp.msg = "账号已被使用";
                        apiResp.code = (int)APIErrCode.OperateFail;
                        bllTableFieldMap.ContextResponse(context, apiResp);
                        return;
                    }
                    pms.Add(string.Format("{0}='{1}'", userIDField.Field, val));
                }
            }

            TableFieldMapping phoneField = formField.FirstOrDefault(p => p.Field.Equals("Phone"));
            if (phoneField != null)
            {
                string val = context.Request[phoneField.Field];
                if (!string.IsNullOrWhiteSpace(val))
                {
                    List<UserInfo> oUserList = bllTableFieldMap.GetColList<UserInfo>(int.MaxValue, 1, string.Format("Phone='{0}' And WebsiteOwner='{2}' And AutoID != {1} And IsSubAccount!='1'", val, autoID, bllTableFieldMap.WebsiteOwner), "AutoID,Phone");
                    if (oUserList.Count > 0)
                    {
                        apiResp.msg = "手机号已被使用";
                        apiResp.code = (int)APIErrCode.OperateFail;
                        bllTableFieldMap.ContextResponse(context, apiResp);
                        return;
                    }
                    pms.Add(string.Format("{0}='{1}'", phoneField.Field, val));
                }
            }

            TableFieldMapping wXOpenIdField = formField.FirstOrDefault(p => p.Field.Equals("WXOpenId"));
            if (wXOpenIdField != null)
            {
                string val = context.Request[wXOpenIdField.Field];
                if (!string.IsNullOrWhiteSpace(val))
                {
                    List<UserInfo> oUserList = bllTableFieldMap.GetColList<UserInfo>(int.MaxValue, 1, string.Format("WXOpenId='{0}' And WebsiteOwner='{2}' And AutoID != {1} ", val, autoID, bllTableFieldMap.WebsiteOwner), "AutoID,Phone");
                    if (oUserList.Count > 0)
                    {
                        apiResp.msg = "WXOpenId已被使用";
                        apiResp.code = (int)APIErrCode.OperateFail;
                        bllTableFieldMap.ContextResponse(context, apiResp);
                        return;
                    }
                    pms.Add(string.Format("{0}='{1}'", wXOpenIdField.Field, val));
                }
            }


            foreach (TableFieldMapping item in formField.Where(p=> !limitFields.Contains(p.Field)))
            {
                string val = context.Request[item.Field];
                if (string.IsNullOrWhiteSpace(val) && item.FieldIsNull == 1)
                {
                    apiResp.msg = item.MappingName + "不能为空";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllTableFieldMap.ContextResponse(context, apiResp);
                    return;
                }

                if (string.IsNullOrWhiteSpace(val))
                {
                    pms.Add(string.Format("{0}=Null", item.Field));
                }
                else{
                    if (!string.IsNullOrWhiteSpace(item.FormatValiFunc))
                    {
                        #region 检查数据格式
                        //检查数据格式
                        if (item.FormatValiFunc == "number")
                        {
                            if (!MyRegex.IsNumber(val))
                            {
                                apiResp.code = (int)APIErrCode.OperateFail;
                                apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                                bllTableFieldMap.ContextResponse(context, apiResp);
                                return;
                            }
                        }
                        if (item.FormatValiFunc == "phone")//email检查
                        {
                            if (!MyRegex.PhoneNumLogicJudge(val))
                            {
                                apiResp.code = (int)APIErrCode.OperateFail;
                                apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                                bllTableFieldMap.ContextResponse(context, apiResp);
                                return;
                            }
                        }
                        if (item.FormatValiFunc == "email")//email检查
                        {
                            if (!MyRegex.EmailLogicJudge(val))
                            {
                                apiResp.code = (int)APIErrCode.OperateFail;
                                apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                                bllTableFieldMap.ContextResponse(context, apiResp);
                                return;
                            }
                        }
                        if (item.FormatValiFunc == "url")//url检查
                        {
                            System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                            System.Text.RegularExpressions.Match match = regUrl.Match(val);
                            if (!match.Success)
                            {
                                apiResp.code = (int)APIErrCode.OperateFail;
                                apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                                bllTableFieldMap.ContextResponse(context, apiResp);
                                return;
                            }
                        }
                        #endregion
                    }
                    pms.Add(string.Format("{0}='{1}'", item.Field, val));
                }
            }
            #endregion

            if(bllTableFieldMap.Update(new UserInfo(),
                ZentCloud.Common.MyStringHelper.ListToStr(pms,"",","),
                string.Format("AutoID={0}", autoID)) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "编辑完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
                bllUser.AddUserScoreDetail(curUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.UpdateMyInfo),bllUser.WebsiteOwner, null, null);


                //
                TableFieldMapping tagNameField = formField.FirstOrDefault(p => p.Field.Equals("TagName"));
                if (tagNameField != null&&context.Request["TagName"]!=null)
                {
                    foreach (var tag in context.Request["TagName"].Split(','))
                    {
                        if (bllUser.GetCount<ZentCloud.BLLJIMP.Model.MemberTag>(string.Format(" WebsiteOwner='{0}' And TagName='{1}' And TagType='Member'", bllUser.WebsiteOwner, tag)) == 0)
                        {

                            ZentCloud.BLLJIMP.Model.MemberTag model = new BLLJIMP.Model.MemberTag();
                            model.CreateTime = DateTime.Now;
                            model.WebsiteOwner = bllUser.WebsiteOwner;
                            model.TagType = "Member";
                            model.TagName = tag;
                            model.Creator = currentUserInfo.UserID;
                            if (!bllUser.Add(model))
                            {
                                apiResp.msg = "新增标签失败";
                                apiResp.code = (int)APIErrCode.OperateFail;
                                bllTableFieldMap.ContextResponse(context, apiResp);
                            }

                        }
                    }
                }
                //



                

            }
            else
            {
                apiResp.msg = "编辑失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllTableFieldMap.ContextResponse(context, apiResp);
        }
    }
}