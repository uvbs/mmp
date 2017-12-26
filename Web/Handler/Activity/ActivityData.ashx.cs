using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Data;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.Handler.Activity
{
    /// <summary>
    /// 活动报名数据处理
    /// </summary>
    public class ActivityData : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJIMP.BLLActivity bllActivity = new BLLActivity("");
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJuActivity();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 
        /// </summary>
        BLLDistributionOffLine bllDistributionOffLine = new BLLDistributionOffLine();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLLog();
        /// <summary>
        /// 
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLDistribution bllDis = new BLLDistribution();
        /// <summary>
        /// BLL 站点
        /// </summary>
        BLLWebSite bllWebsite = new BLLWebSite();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            string result = "false";
            currentUserInfo = bllActivity.GetCurrentUserInfo();
            if (!bllActivity.IsLogin)
            {
                goto outoff;


            }
            if ((!currentUserInfo.UserID.Equals(bllActivity.WebsiteOwner)) && (!currentUserInfo.UserType.Equals(1)) && (currentUserInfo.IsSubAccount == "0"))
            {
                goto outoff;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            //利用反射找到未知的调用的方法
            if (!string.IsNullOrEmpty(action))
            {
                MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
            }
            else
            {
                result = "action not exist";
            }
        outoff:
            context.Response.Write(result);


        }
        /// <summary>
        /// 编辑报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditActivityData(HttpContext context)
        {
            string activityId = context.Request["ActivityID"];
            int uId = Convert.ToInt32(context.Request["UID"]);
            BLLJIMP.Model.ActivityDataInfo model = bllActivity.Get<BLLJIMP.Model.ActivityDataInfo>(string.Format(" ActivityID = '{0}' AND UID = {1} ", activityId, uId));
            BLLJIMP.Model.ActivityDataInfo reqModel = bllActivity.ConvertRequestToModel<BLLJIMP.Model.ActivityDataInfo>(new BLLJIMP.Model.ActivityDataInfo());
            WebsiteInfo websiteModel = bllWebsite.GetWebsiteInfo(bllWebsite.WebsiteOwner);
            reqModel.InsertDate = model.InsertDate;
            reqModel.IsDelete = model.IsDelete;
            reqModel.WeixinOpenID = model.WeixinOpenID;
            reqModel.SpreadUserID = model.SpreadUserID;
            reqModel.WebsiteOwner = model.WebsiteOwner;
            reqModel.MonitorPlanID = model.MonitorPlanID;

            reqModel.PaymentStatus = model.PaymentStatus;
            reqModel.OrderId = model.OrderId;
            reqModel.IsSignIn = model.IsSignIn;
            reqModel.UserId = model.UserId;
            reqModel.ActivityID = model.ActivityID;
            reqModel.Amount = model.Amount;
            reqModel.ArticleType = model.ArticleType;
            reqModel.CategoryId = model.CategoryId;
            reqModel.CouponName = model.CouponName;
            reqModel.Distance = model.Distance;
            reqModel.DistributionOffLineRecommendCode = model.DistributionOffLineRecommendCode;
            reqModel.DistributionOffLineRecommendName = model.DistributionOffLineRecommendName;
            reqModel.FromUserId = model.FromUserId;
            reqModel.GuaranteeCreditAcount = model.GuaranteeCreditAcount;
            reqModel.InsertDateStr = model.InsertDateStr;
            reqModel.IsFee = model.IsFee;
            reqModel.ItemAmount = model.ItemAmount;
            reqModel.ItemName = model.ItemName;
            reqModel.SpreadUserID = model.SpreadUserID;
            reqModel.Status = model.Status;
            reqModel.ToUserId = model.ToUserId;
            reqModel.UID = model.UID;
            reqModel.UseAmount = model.UseAmount;
            reqModel.UserLatitude = model.UserLatitude;
            reqModel.UserLongitude = model.UserLongitude;
            reqModel.UseScore = model.UseScore;

            if (
                websiteModel.IsSynchronizationData == 1 
                && 
                websiteModel.IsSynchronizationData!=null
                &&
                !string.IsNullOrWhiteSpace(model.UserId)
                )
            {
                UserInfo userModel = bllUser.GetUserInfo(model.UserId);
                if (userModel != null)
                {
                    userModel.Phone = model.Phone;
                    userModel.TrueName = model.Name;
                    bllWebsite.Update(userModel);
                }
            }

            if (bllActivity.Update(reqModel))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "编辑失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 手动添加报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddActivityData(HttpContext context)
        {
            //接收到的实体
            BLLJIMP.Model.ActivityDataInfo reqModel = bllActivity.ConvertRequestToModel<BLLJIMP.Model.ActivityDataInfo>(new BLLJIMP.Model.ActivityDataInfo());

            string activityId = context.Request["ActivityID"];
            var newActivityUId = 1001;
            var lastActivityDataInfo = bllActivity.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", activityId));
            if (lastActivityDataInfo != null)
            {
                newActivityUId = lastActivityDataInfo.UID + 1;
            }
            reqModel.ActivityID = activityId;
            reqModel.UID = newActivityUId;
            reqModel.InsertDate = DateTime.Now;
            reqModel.WebsiteOwner = bllActivity.WebsiteOwner;

            #region OLD
            //BLLJIMP.Model.ActivityDataInfo Model = new ActivityDataInfo();
            //Model.UserId = context.Request["UserId"];
            //Model.ActivityID = ActivityID;
            //Model.UID = NewActivityUID;
            //Model.InsertDate = DateTime.Now;
            //Model.Name = GetPostParm("Name");
            //Model.Phone = GetPostParm("Phone");
            //Model.K1 = GetPostParm("K1");
            //Model.K2 = GetPostParm("K2");
            //Model.K3 = GetPostParm("K3");
            //Model.K4 = GetPostParm("K4");
            //Model.K5 = GetPostParm("K5");
            //Model.K6 = GetPostParm("K6");
            //Model.K7 = GetPostParm("K7");
            //Model.K8 = GetPostParm("K8");
            //Model.K9 = GetPostParm("K9");
            //Model.K10 = GetPostParm("K10");
            //Model.K11 = GetPostParm("K11");
            //Model.K12 = GetPostParm("K12");
            //Model.K13 = GetPostParm("K13");
            //Model.K14 = GetPostParm("K14");
            //Model.K15 = GetPostParm("K15");
            //Model.K16 = GetPostParm("K16");
            //Model.K17 = GetPostParm("K17");
            //Model.K18 = GetPostParm("K18");
            //Model.K19 = GetPostParm("K19");
            //Model.K20 = GetPostParm("K20");

            //Model.K21 = GetPostParm("K21");
            //Model.K22 = GetPostParm("K22");
            //Model.K23 = GetPostParm("K23");
            //Model.K24 = GetPostParm("K24");
            //Model.K25 = GetPostParm("K25");
            //Model.K26 = GetPostParm("K26");
            //Model.K27 = GetPostParm("K27");
            //Model.K28 = GetPostParm("K28");
            //Model.K29 = GetPostParm("K29");
            //Model.K30 = GetPostParm("K30");

            //Model.K31 = GetPostParm("K31");
            //Model.K32 = GetPostParm("K32");
            //Model.K33 = GetPostParm("K33");
            //Model.K34 = GetPostParm("K34");
            //Model.K35 = GetPostParm("K35");
            //Model.K36 = GetPostParm("K36");
            //Model.K37 = GetPostParm("K37");
            //Model.K38 = GetPostParm("K38");
            //Model.K39 = GetPostParm("K39");
            //Model.K40 = GetPostParm("K40");

            //Model.K41 = GetPostParm("K41");
            //Model.K42 = GetPostParm("K42");
            //Model.K43 = GetPostParm("K43");
            //Model.K44 = GetPostParm("K44");
            //Model.K45 = GetPostParm("K45");
            //Model.K46 = GetPostParm("K46");
            //Model.K47 = GetPostParm("K47");
            //Model.K48 = GetPostParm("K48");
            //Model.K49 = GetPostParm("K49");
            //Model.K50 = GetPostParm("K50");

            //Model.K51 = GetPostParm("K51");
            //Model.K52 = GetPostParm("K52");
            //Model.K53 = GetPostParm("K53");
            //Model.K54 = GetPostParm("K54");
            //Model.K55 = GetPostParm("K55");
            //Model.K56 = GetPostParm("K56");
            //Model.K57 = GetPostParm("K57");
            //Model.K58 = GetPostParm("K58");
            //Model.K59 = GetPostParm("K59");
            //Model.K60 = GetPostParm("K60"); 
            #endregion

            UserInfo userInfo = bllUser.GetUserInfo(reqModel.UserId);
            if (!string.IsNullOrEmpty(reqModel.UserId))
            {

                if (userInfo == null)
                {
                    resp.Msg = "用户不存在,请检查";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    #region 自动补充信息
                    reqModel.WeixinOpenID = userInfo.WXOpenId;
                    reqModel.UserId = userInfo.UserID;
                    if (!string.IsNullOrEmpty(userInfo.TrueName))
                    {
                        reqModel.Name = userInfo.TrueName;
                    }
                    if (!string.IsNullOrEmpty(userInfo.Phone))
                    {
                        reqModel.Phone = userInfo.Phone;
                    }
                    var fieldMappingList = bllActivity.GetActivityFieldMappingList(reqModel.ActivityID);
                    Type type = reqModel.GetType();
                    if (!string.IsNullOrEmpty(userInfo.Company))
                    {

                        if (fieldMappingList.Where(p => p.MappingName.Contains("公司")).Count() > 0)
                        {

                            PropertyInfo propertyInfo = type.GetProperty("K" + fieldMappingList.Where(p => p.MappingName.Contains("公司")).First().ExFieldIndex.ToString());

                            propertyInfo.SetValue(reqModel, userInfo.Company, null);



                        }
                    }
                    if (!string.IsNullOrEmpty(userInfo.Postion))
                    {

                        if (fieldMappingList.Where(p => p.MappingName.Contains("职位")).Count() > 0)
                        {

                            PropertyInfo propertyInfo = type.GetProperty("K" + fieldMappingList.Where(p => p.MappingName.Contains("职位")).First().ExFieldIndex.ToString());

                            propertyInfo.SetValue(reqModel, userInfo.Postion, null);



                        }
                    }
                    if (!string.IsNullOrEmpty(userInfo.Email))
                    {

                        if (fieldMappingList.Where(p => p.MappingName.Contains("邮箱")).Count() > 0)
                        {

                            PropertyInfo propertyInfo = type.GetProperty("K" + fieldMappingList.Where(p => p.MappingName.Contains("邮箱")).First().ExFieldIndex.ToString());

                            propertyInfo.SetValue(reqModel, userInfo.Email, null);



                        }
                    }



                    #endregion

                }

            }
            if (string.IsNullOrEmpty(reqModel.Name))
            {
                resp.Msg = "该用户没有填写姓名,请填写姓名";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(reqModel.Phone))
            {
                resp.Msg = "该用户没有填写手机号,请填写手机号";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllActivity.Add(reqModel))
            {
                resp.Status = 1;
                #region 扣积分
                JuActivityInfo juActivityInfo = bllJuActivity.GetJuActivityByActivityID(activityId);
                if ((juActivityInfo != null) && (juActivityInfo.ActivityIntegral > 0))
                {
                    if (userInfo != null)
                    {
                        //userInfo.TotalScore -= juActivityInfo.ActivityIntegral;
                        if (bllUser.Update(userInfo, string.Format(" TotalScore-={0}", juActivityInfo.ActivityIntegral), string.Format(" AutoID={0}", userInfo.AutoID)) > 0)
                        {
                            ////积分记录
                            //BLLJIMP.Model.WBHScoreRecord record = new BLLJIMP.Model.WBHScoreRecord()
                            //{
                            //    InsertDate = DateTime.Now,
                            //    ScoreNum = "-" + juActivityInfo.ActivityIntegral.ToString(),
                            //    WebsiteOwner = bllUser.WebsiteOwner,
                            //    UserId = reqModel.UserId,
                            //    NameStr = "参加" + juActivityInfo.ActivityName,
                            //    Nums = "b55",
                            //    RecordType = "1",
                            //};
                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.Score = juActivityInfo.ActivityIntegral;
                            scoreRecord.ScoreType = "ActivityUse";
                            scoreRecord.UserID = userInfo.UserID;
                            scoreRecord.AddNote = "参加" + juActivityInfo.ActivityName + "使用" + juActivityInfo.ActivityIntegral + "积分";
                            scoreRecord.WebSiteOwner = userInfo.WebsiteOwner;
                            //bllUser.Add(record);
                            bllUser.Add(scoreRecord);
                        }
                        else
                        {
                            resp.Msg = "更新用户积分失败";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                    }

                }
                #endregion
            }
            else
            {
                resp.Msg = "添加失败";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 手动添加报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string autoIds = context.Request["Ids"];
            string activityId = context.Request["ActivityID"];
            int successCount = 0;
            foreach (var autoId in autoIds.Split(','))
            {
                UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
                if (bllActivity.GetCount<ActivityDataInfo>(string.Format(" ActivityID={0} And IsDelete=0 And UserId='{1}'",activityId,userInfo.UserID))>0)
                {
                    resp.Msg =string.Format("{0}已经报过名了,不需要重复报名",userInfo.TrueName);
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                ActivityDataInfo model = new ActivityDataInfo();
                var newActivityUId = 1001;
                var lastActivityDataInfo = bllActivity.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", activityId));
                if (lastActivityDataInfo != null)
                {
                    newActivityUId = lastActivityDataInfo.UID + 1;
                }
                model.ActivityID = activityId;
                model.UID = newActivityUId;
                model.InsertDate = DateTime.Now;
                model.WebsiteOwner = bllActivity.WebsiteOwner;
                model.SpreadUserID = "system";
                #region 自动补充信息
                model.WeixinOpenID = userInfo.WXOpenId;
                model.UserId = userInfo.UserID;
                if (!string.IsNullOrEmpty(userInfo.TrueName))
                {
                    model.Name = userInfo.TrueName;
                }
                else
                {
                    model.Name = "系统添加";
                }
                if (!string.IsNullOrEmpty(userInfo.Phone))
                {
                    model.Phone = userInfo.Phone;
                }
                if(string.IsNullOrEmpty(model.Phone))
                {
                    resp.Msg = string.Format("手机号不能为空");
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                var fieldMappingList = bllActivity.GetActivityFieldMappingList(activityId);
                Type type = model.GetType();
                if (!string.IsNullOrEmpty(userInfo.Company))
                {

                    if (fieldMappingList.Where(p => p.MappingName.Contains("公司")).Count() > 0)
                    {

                        PropertyInfo propertyInfo = type.GetProperty("K" + fieldMappingList.Where(p => p.MappingName.Contains("公司")).First().ExFieldIndex.ToString());

                        propertyInfo.SetValue(model, userInfo.Company, null);



                    }
                }
                if (!string.IsNullOrEmpty(userInfo.Postion))
                {

                    if (fieldMappingList.Where(p => p.MappingName.Contains("职位")).Count() > 0)
                    {

                        PropertyInfo propertyInfo = type.GetProperty("K" + fieldMappingList.Where(p => p.MappingName.Contains("职位")).First().ExFieldIndex.ToString());

                        propertyInfo.SetValue(model, userInfo.Postion, null);



                    }
                }
                if (!string.IsNullOrEmpty(userInfo.Email))
                {

                    if (fieldMappingList.Where(p => p.MappingName.Contains("邮箱")).Count() > 0)
                    {

                        PropertyInfo propertyInfo = type.GetProperty("K" + fieldMappingList.Where(p => p.MappingName.Contains("邮箱")).First().ExFieldIndex.ToString());

                        propertyInfo.SetValue(model, userInfo.Email, null);



                    }
                }



                #endregion


                if (bllActivity.Add(model))
                {
                    successCount++;
                    resp.Status = 1;

                }
                else
                {
                    resp.Msg = "添加失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }


            }
            resp.ExInt = successCount;
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityData(HttpContext context)
        {
            string activityId = context.Request["ActivityID"];
            int uId = Convert.ToInt32(context.Request["UID"]);
            BLLJIMP.Model.ActivityDataInfo model = bllActivity.Get<BLLJIMP.Model.ActivityDataInfo>(string.Format(" ActivityID = '{0}' AND UID = {1} ", activityId, uId));
            if (model != null)
                return Common.JSONHelper.ObjectToJson(model);
            else
                return "";
        }

        /// <summary>
        /// 查询报名数据
        /// </summary>
        private string Query(HttpContext context)
        {

            string activityId = context.Request["ActivityID"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            string linkName = context.Request["LinkName"];
            string status = context.Request["Status"];
            string source = context.Request["Source"];
            string paymentStatus = context.Request["PaymentStatus"];
            string dealStatus = context.Request["DealStatus"];
            string orderBy = " InsertDate DESC";

            var strWhere = string.Format("ActivityID='{0}' AND IsDelete = 0 ", activityId);

            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere += "And Name like '%" + keyWord + "%'";
            }

            if (!string.IsNullOrEmpty(linkName))
            {
                strWhere += string.Format(" AND SpreadUserID='{0}'", linkName);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                strWhere += string.Format(" AND Status in ({0}) ", status);
            }
            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                strWhere += string.Format(" AND PaymentStatus ={0}", paymentStatus);
            }
            if (!string.IsNullOrWhiteSpace(dealStatus))
            {
                strWhere += string.Format(" AND Status ={0}", dealStatus);
            }
            if (!string.IsNullOrWhiteSpace(source))
            {
                switch (source)
                {
                    case "0":
                        strWhere += string.Format(" AND (SpreadUserID='' or SpreadUserID is null) ", status);
                        break;
                    case "1":
                        strWhere += string.Format(" AND SpreadUserID!=''", status);
                        break;
                    default:
                        break;
                }
            }
            List<ActivityDataInfo> data = bllActivity.GetLit<ActivityDataInfo>(pageSize, pageIndex, strWhere, orderBy);
            var webSiteInfo = bllActivity.GetWebsiteInfoModelFromDataBase();
            for (int i = 0; i < data.Count; i++)
            {
                int rcode = 0;
                if (int.TryParse(data[i].DistributionOffLineRecommendCode, out rcode))
                {
                    var rUser = bllUser.GetUserInfoByAutoID(rcode);

                    if (rUser != null)
                    {
                        if (rUser.UserID == bllActivity.WebsiteOwner)
                        {
                            data[i].DistributionOffLineRecommendName = webSiteInfo.WebsiteName;
                        }
                        else
                        {
                            data[i].DistributionOffLineRecommendName = bllUser.GetUserDispalyName(rUser);

                            //如果是当前站点所有者，则显示站点的名称
                            if (bllActivity.WebsiteOwner == rUser.UserID)
                            {
                                data[i].DistributionOffLineRecommendName = bllActivity.GetWebsiteInfoModel().WebsiteName;
                            }

                        }

                    }
                }
            }

            int totalCount = bllActivity.GetCount<ActivityDataInfo>(strWhere);
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = data
            });

        }

        /// <summary>
        /// 删除
        /// </summary>
        private string Delete(HttpContext context)
        {
            string uids = context.Request["ids"];
            string activityId = context.Request["ActivityID"];
            if (bllActivity.Update(new ActivityDataInfo(), " IsDelete = 1 ", string.Format("UID in({0}) And ActivityID='{1}'", uids, activityId)) > 0)
            {
                foreach (var uid in uids.Split(','))//积分返还
                {

                    ActivityDataInfo model = bllActivity.GetActivityDataInfo(activityId, int.Parse(uid));
                    JuActivityInfo juActivityInfo = bllJuActivity.GetJuActivityByActivityID(model.ActivityID);
                    if ((juActivityInfo != null) && (juActivityInfo.ActivityIntegral > 0))
                    {
                        UserInfo userInfo = bllUser.GetUserInfoByOpenId(model.WeixinOpenID);
                        if (userInfo != null)
                        {
                            userInfo.TotalScore += juActivityInfo.ActivityIntegral;
                            if (bllUser.Update(new UserInfo(), string.Format(" TotalScore={0}", userInfo.TotalScore), string.Format(" AutoID={0}", userInfo.AutoID)) > 0)
                            {
                                //积分记录
                                BLLJIMP.Model.WBHScoreRecord record = new BLLJIMP.Model.WBHScoreRecord()
                                {
                                    InsertDate = DateTime.Now,
                                    ScoreNum = "+" + juActivityInfo.ActivityIntegral.ToString(),
                                    WebsiteOwner = bllUser.WebsiteOwner,
                                    UserId = userInfo.UserID,
                                    NameStr = "取消参加" + juActivityInfo.ActivityName,
                                    Nums = "b55",
                                    RecordType = "2",
                                };
                                bllUser.Add(record);
                                //积分记录
                            }
                            else
                            {
                                resp.Msg = "返还用户积分失败";
                                return Common.JSONHelper.ObjectToJson(resp);
                            }
                        }

                    }


                }
                resp.Status = 1;
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Activity, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除活动报名数据[报名id=" + uids + ",活动id=" + activityId + "]");
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        ///// <summary>
        /////发送短信
        ///// </summary>
        //private string SendSMS(HttpContext context)
        //{


        //    string PhoneList = context.Request["PhoneList"];
        //    //var SMSContent = context.Request["SMSContent"];
        //    //if (string.IsNullOrWhiteSpace(SMSContent))
        //    //{
        //    //    return "请输入短信内容";
        //    //}

        //    //var userInfo= DataLoadTool.GetCurrUserModel();
        //    //int count = 0;
        //    //foreach (var item in PhoneList.Split(','))
        //    //{
        //    //    string Parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membertrigger", userInfo.UserID, userInfo.Password, item, SMSContent);
        //    //    Common.HttpInterFace obj = new Common.HttpInterFace();
        //    //    string result = obj.PostWebRequest(Parm, "http://www.jubit.org/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));
        //    //    if (result=="0")
        //    //    {
        //    //        count++;
        //    //    }
        //    //    else
        //    //    {

        //    //    }

        //    //}
        //    //if (PhoneList.Split(',').Length==count)
        //    //{
        //    //    return "true";
        //    //}
        //    //else
        //    //{
        //    //    return "短信未全部发送成功，请检查余额或联系管理员";
        //    //}

        //    //BLLMember bllMember = new BLLMember("");
        //    //context.Session[SessionKey.PageRedirect] = "/Activity/ActivityData.aspx";
        //    //context.Session[SessionKey.PageCacheName] = "cache" + bllMember.GetGUID(TransacType.CacheGet);
        //    //Comm.DataCache.SetCache(context.Session[SessionKey.PageCacheName].ToString(), PhoneList);

        //    return "true";

        //}

        /// <summary>
        /// 下载报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void DownLoadActivityData(HttpContext context)
        {

            BLLPermission.BLLMenuPermission bllMenupermission = new BLLPermission.BLLMenuPermission("");
            bool isData = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);
            if (isData)
            {
                return;
            }
            string activityId = context.Request["ActivityID"];
            string exportType = context.Request["type"];
            if (!bllActivity.IsExistActivity(activityId))
            {
                return;
            }
            ////判断普通用户是否有相应权限
            //if (!bll.CheckActivityIDAndUser(ActivityID, cu.UserID) && userInfo.UserType != 1)
            //    return;
            #region 日志模块
     
            if (exportType == "DistributionOffLine")
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.DistributionOffLine,BLLJIMP.Enums.EnumLogTypeAction.Export, bllUser.GetCurrUserID(), "导出分销员审核数据");
            }
            else
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.DistributionOffLine, BLLJIMP.Enums.EnumLogTypeAction.Export, bllUser.GetCurrUserID(), "导出活动报名数据");
            }
            #endregion
            DataTable dataTable = bllActivity.QueryActivityData(activityId);
            string fileName = bllActivity.GetActivityInfoByActivityID(activityId).ActivityName;
            DataLoadTool.ExportDataTable(dataTable, string.Format("{0}_{1}_data.xls", fileName, DateTime.Now.ToString()));

        }
        /// <summary>
        /// 是否通过报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ActivityDataByIsPass(HttpContext context)
        {

            string activityId = context.Request["ActivityID"];
            string uId = context.Request["UID"];
            string remarks = context.Request["Remarks"];
            int status = int.Parse(context.Request["Status"]);
            string[] ids = uId.Split(',');
            ActivityDataInfo model = new ActivityDataInfo();
            foreach (var item in ids)
            {


                model = bllActivity.GetActivityDataInfo(activityId, int.Parse(item));
                model.Status = status;
                model.Remarks = remarks;
                UserInfo commendUserInfo = bllUser.GetUserInfoByAutoID(int.Parse(model.DistributionOffLineRecommendCode));
                if (commendUserInfo == null)
                {
                    continue;
                }
                #region 审核通过
                if (status == 1001)//审核通过
                {
                    UserInfo userInfo = bllUser.GetUserInfo(model.UserId);
                    userInfo.TrueName = model.Name;
                    if (string.IsNullOrEmpty(model.SpreadUserID))//不是微转发的才分配上级用户
                    {
                        userInfo.DistributionOffLinePreUserId = commendUserInfo.UserID;

                    }
                    else//微转发直接成为分销员
                    {
                        commendUserInfo = bllUser.GetUserInfo(model.SpreadUserID);
                        userInfo.DistributionOffLinePreUserId = commendUserInfo.UserID;
                    }
                    if (bllUser.Update(userInfo))
                    {
                        //申请通过向申请人和上级提醒通过申请

                        //申请人
                        //bllWeixin.SendTemplateMessageNotifyComm(userInfo.WXOpenId, "财富伙伴申请结果", "恭喜您已经通过审核！", string.Format("http://{0}/App/Distribution/m/index.aspx", context.Request.UserHostName));
                        if (string.IsNullOrEmpty(model.SpreadUserID))//不是微转发的才分配上级用户
                        {
                            //上级
                            bllWeixin.SendTemplateMessageNotifyComm(commendUserInfo, string.Format("恭喜您的会员“{0}”申请财富会员成功", model.Name), "请提醒他关注公众号并帮助他熟悉系统操作吧。", string.Format("http://{0}/App/Distribution/m/index.aspx", context.Request.Url.Host));
                        }

                    }

                    //if (!string.IsNullOrEmpty(model.SpreadUserID))//不是微转发的才分配上级用户
                    //{
                    //    model.Status = 4003;

                    //}

                }
                #endregion

                #region 审核不通过
                else if (status == 4001)
                {
                    UserInfo userInfo = bllUser.GetUserInfo(model.UserId);

                    //判断下，当他有下线的时候就不可以取消审核状态
                    if (bllDistributionOffLine.IsHaveLowerLevel(model.UserId))
                    {
                        continue;
                    }
                    else
                    {
                        userInfo.DistributionOffLinePreUserId = "";
                        bllUser.Update(userInfo);
                    }

                    //申请通过向申请人和上级提醒通过申请

                    //申请人
                    //bllWeixin.SendTemplateMessageNotifyComm(userInfo.WXOpenId, "财富伙伴申请结果", string.Format("审核未通过！\\{0}", remarks));

                    //上级

                    bllWeixin.SendTemplateMessageNotifyComm(commendUserInfo, string.Format("很抱歉您的会员“{0}”申请财富会员失败", model.Name), string.Format(" 原因：{0}\\n您可提醒他满足要求后重新申请。", remarks), string.Format("http://{0}/App/Distribution/m/index.aspx", context.Request.Url.Host));
                    if (!string.IsNullOrEmpty(model.SpreadUserID))
                    {
                        //微转发审核不通过，删除此记录 
                        bllActivity.Delete(model, string.Format("ActivityID={0} And UID={1}", model.ActivityID, model.UID));
                    }

                }
                #endregion

                if (!bllActivity.Update(model))
                {
                    continue;
                }
            }
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);
        }


        
        /// <summary>
        /// 批量设置交易完成状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDealStatus(HttpContext context)
        {

            string activityId = context.Request["ActivityID"];
            string uId = context.Request["UID"];
            string[] ids = uId.Split(',');
            ActivityDataInfo model = new ActivityDataInfo();
            foreach (var item in ids)
            {
                model = bllActivity.GetActivityDataInfo(activityId, int.Parse(item));
                if (model.Status ==1)
                {
                    continue;
                }
                model.Status = 1;
                if (!bllActivity.Update(model))
                {

                    continue;
                }
                else
                {

                    WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(model.OrderId);
                    if (orderInfo!=null)
                    {
                        orderInfo.Status = "交易成功";
                        orderInfo.DistributionStatus = 2;
                        string msg = "";
                        if (bllDis.Transfers(orderInfo, out msg))
                        {
                            orderInfo.DistributionStatus = 3;
                        }
                        bllMall.Update(orderInfo);
                    }

                }
            }
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        ///// <summary>
        ///// 获取参数
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //private string GetPostParm(string parm)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(parm))
        //            return HttpContext.Current.Request[parm];
        //    }
        //    catch { return ""; }
        //    return "";
        //}


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}