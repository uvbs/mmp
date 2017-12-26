using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 检查是否登录
    /// </summary>
    public class IsLogin : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// yike 处理
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
        public void ProcessRequest(HttpContext context)
        {
            string result = "";
            if (bllUser.IsLogin)
            {
                var currentUserInfo = bllUser.GetCurrentUserInfo();
                string noYike = context.Request["no_yike"];
                #region mixblu检查
                if (noYike !="1" && bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                {
                    if ((!string.IsNullOrEmpty(currentUserInfo.Ex2)) && (!string.IsNullOrEmpty(currentUserInfo.Phone)))
                    {

                        result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                        {
                            is_login = true,
                            user_name = currentUserInfo.UserID,
                            id = currentUserInfo.AutoID


                        });
                    }
                    #region 去yike 拉取数据
                    else
                    {
                        try
                        {
                            string userInfoJson = yiKeClient.GetUserInfo(currentUserInfo.WXUnionID);
                            JToken token = JToken.Parse(userInfoJson);
                            if (token["Status"].ToString().ToLower() == "true")//有会员信息
                            {
                                var cardNum = token["Result"]["Code"].ToString();
                                var phone = token["Result"]["MobileNo"].ToString();
                                if (bllUser.Update(currentUserInfo, string.Format(" Phone='{0}',Ex2='{1}'", phone, cardNum), string.Format(" AutoId={0}", currentUserInfo.AutoID)) > 0)
                                {
                                    result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                                    {
                                        is_login = true,
                                        user_name = currentUserInfo.UserID,
                                        id = currentUserInfo.AutoID


                                    });
                                    context.Response.Write(result);
                                    return;

                                }


                            }
                        }
                        catch (Exception)
                        {


                        }
                        result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                        {
                            is_login = false,
                            user_name = ""


                        });
                    }
                    #endregion
                }
                #endregion

                #region 一般检查已登录
                else
                {
                    result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                    {
                        is_login = true,
                        user_name = currentUserInfo.UserID,
                        id = currentUserInfo.AutoID,
                        score = currentUserInfo.TotalScore
                    });
                }
                #endregion




            }
            else
            {
                result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    is_login = false,
                    user_name = "",
                    id=0

                });
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                result = string.Format("{0}({1})", context.Request["callback"], result);

            }
            else
            {
                //返回json数据
            }
            context.Response.Write(result);
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