using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Account
{
    /// <summary>
    /// Add 的摘要说明  添加子账户
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #region 检查
            if (string.IsNullOrEmpty(requestModel.user_id))
            {
                resp.errmsg = "user_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.user_pwd))
            {
                resp.errmsg = "user_pwd 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.isdelete)||string.IsNullOrEmpty(requestModel.isexport))
            {
                resp.errmsg = "isdelete、isexport 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion
            UserInfo userModel = new UserInfo();
            userModel.UserID = requestModel.user_id;
            userModel.Password = requestModel.user_pwd;
            userModel.TrueName = requestModel.true_name;
            userModel.Company = requestModel.user_company;
            userModel.Phone = requestModel.user_phone;
            userModel.Postion = requestModel.user_postion;
            userModel.WXHeadimgurl = requestModel.wx_head_img_url;
            userModel.WebsiteOwner = bllUser.WebsiteOwner;
            userModel.UserType = 2;
            userModel.RegIP =ZentCloud.Common.MySpider.GetClientIP();
            userModel.Regtime = DateTime.Now;
            userModel.LoginTotalCount = 0;
            userModel.IsSubAccount = "1";
            userModel.VoteCount = requestModel.user_vote_count;
            userModel.LastLoginDate = DateTime.Now;
            if (bllUser.Exists(userModel, "UserID"))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                resp.errmsg = "用户名" + userModel.UserID + "已存在！";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(userModel.WebsiteOwner))
            {
                userModel.WebsiteOwner = userModel.UserID;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (this.bllUser.Add(userModel, tran))
                {
                    var group = new ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo()
                    {
                        UserID = userModel.UserID,
                        GroupID = 130273//管理员组
                    };
                    if (Convert.ToBoolean(requestModel.isdelete))
                    {
                        var relation = new ZentCloud.BLLPermission.Model.PermissionRelationInfo()
                        {
                            RelationID = userModel.UserID,
                            PermissionID = -1,
                            RelationType = 1
                        };
                        if (!bllUser.Add(relation, tran))
                        {
                            tran.Rollback();
                            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                            resp.errmsg = "权限关系添加失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                    }
                    if (Convert.ToBoolean(requestModel.isexport))
                    {
                        var relations = new ZentCloud.BLLPermission.Model.PermissionRelationInfo()
                        {
                            RelationID = userModel.UserID,
                            PermissionID = -2,
                            RelationType = 1
                        };
                        if (!bllUser.Add(relations, tran))
                        {
                            tran.Rollback();
                            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                            resp.errmsg = "权限关系添加失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                    }
                    if (bllUser.Add(group, tran))//添加权限组
                    {
                        tran.Commit();
                        resp.isSuccess = true;
                        resp.errmsg = "添加成功！";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    else
                    {
                        tran.Rollback();
                        resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        resp.errmsg = "添加用户组失败！";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }
                else
                {
                    tran.Rollback();
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "添加用户信息失败！";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
        }

        public class RequestModel 
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string user_id { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string user_pwd { get; set; }

            /// <summary>
            /// 公司
            /// </summary>
            public string user_company { get; set; }

            /// <summary>
            /// 职位
            /// </summary>
            public string user_postion { get; set; }

            /// <summary>
            /// 头像
            /// </summary>
            public string wx_head_img_url { get; set; }

            /// <summary>
            /// 手机
            /// </summary>
            public string user_phone { get; set; }

            /// <summary>
            /// 真实姓名
            /// </summary>
            public string true_name { get; set; }

            /// <summary>
            /// 票数
            /// </summary>
            public int user_vote_count { get; set; }

            /// <summary>
            /// 是否允许删除
            /// </summary>
            public string isdelete { get; set; }


            /// <summary>
            /// 是否允许导出
            /// </summary>
            public string isexport { get; set; }
        }
    }
}