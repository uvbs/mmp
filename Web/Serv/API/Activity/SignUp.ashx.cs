using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// SignUp 的摘要说明
    /// </summary>
    public class SignUp : BaseHandlerNoAction
    {
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();

        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 当前请求参数键值对
        /// </summary>
        Dictionary<string, string> dicPar;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (bll.IsLogin)
            {
                currentUserInfo = bll.GetCurrentUserInfo();
            }
            string activityId = context.Request["activity_id"];
            if (string.IsNullOrEmpty(activityId))
            {
                resp.errcode = 1;
                resp.errmsg = "activity_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            JuActivityInfo juInfo = bll.GetJuActivity(int.Parse(activityId), true);

            if (juInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "活动不存在!";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }
            #region 是否可以报名
            if (juInfo.ActivityStatus.Equals(1))
            {
                resp.errcode = 2;
                resp.errmsg = "活动已停止";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }
            if (juInfo.MaxSignUpTotalCount > 0)//检查报名人数
            {
                if (juInfo.SignUpTotalCount > (juInfo.MaxSignUpTotalCount - 1))
                {
                    resp.errcode = 3;
                    resp.errmsg = "报名人数已满";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;

                }

            }
            if (juInfo.ActivityIntegral > 0)
            {
                if (currentUserInfo.TotalScore < juInfo.ActivityIntegral)
                {
                    resp.errcode = 4;
                    resp.errmsg = "您的积分不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;

                }

            }
            if (juInfo.GuaranteeCreditAcount > 0)
            {
                if (currentUserInfo.CreditAcount < juInfo.GuaranteeCreditAcount)
                {
                    resp.errcode = 6;
                    resp.errmsg = "您的信用金不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

            }
            #endregion
            dicPar = bll.GetRequestParameter();
            //string weixinOpenID = null;
            string activityIdBySignUp = juInfo.SignUpActivityID;
            string spreadUserId = null;
            dicPar.TryGetValue("SpreadUserID", out spreadUserId);
            string strDistinctKeys = null;//检查重复的字段，多个字段用,分隔， //没有此参数默认用手机检查  
            dicPar.TryGetValue("DistinctKeys", out strDistinctKeys);
            string monitorPlanID = null;
            dicPar.TryGetValue("MonitorPlanID", out monitorPlanID);
            string name = null;
            dicPar.TryGetValue("Name", out name);
            string phone = null;
            dicPar.TryGetValue("Phone", out phone);
            ActivityInfo activity = bll.Get<ActivityInfo>(string.Format("ActivityID='{0}'", activityIdBySignUp));

            #region IP限制
            //获取用户IP;
            string userHostAddress = context.Request.UserHostAddress;
            var count = DataCache.GetCache(userHostAddress);
            if (count != null)
            {
                int newCount = int.Parse(count.ToString()) + 1;
                DataCache.SetCache(userHostAddress, newCount);
                int limitCount = 1000;
                if (activity != null)
                {

                    limitCount = activity.LimitCount;

                }
                if (newCount >= limitCount)
                {

                    resp.errcode = 5;
                    resp.errmsg = "您的提交过于频繁，请稍后再试";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            else
            {
                DataCache.SetCache(userHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
            }

            #endregion

            #region 活动权限验证
            if (juInfo == null)
            {
                resp.errcode = 6;
                resp.errmsg = "活动不存在!";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (juInfo.ActivityStatus.Equals(1))
            {
                resp.errcode = 7;
                resp.errmsg = "活动已关闭!";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (activity.IsDelete.Equals(1))
            {
                resp.errcode = 8;
                resp.errmsg = "活动已删除!";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion

            #region 判断必填项
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            {
                resp.errcode = 9;
                resp.errmsg = "姓名和手机不能为空!";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }

            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 10;
                resp.errmsg = "手机号码无效!";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            #endregion

            #region 检查自定义必填项
            List<ActivityFieldMappingInfo> listRequiredField = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And FieldIsNull=1", activity.ActivityID));
            if (listRequiredField.Count > 0)
            {
                foreach (var requiredField in listRequiredField)
                {
                    if (string.IsNullOrEmpty(dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", requiredField.ExFieldIndex))).Value))
                    {
                        resp.errcode = 11;
                        resp.errmsg = string.Format(" {0} 必填", requiredField.MappingName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }
                }
            }
            #endregion

            #region 检查数据格式
            //检查数据格式
            List<ActivityFieldMappingInfo> activityFieldMapping = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", activity.ActivityID));
            foreach (var item in activityFieldMapping)
            {
                        
                string value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                //检查数据格式
                if (item.FormatValiFunc == "email")//email检查
                {
                    if (!ZentCloud.Common.ValidatorHelper.EmailLogicJudge(value))
                    {
                        resp.errcode = 12;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }
                }
                if (item.FormatValiFunc == "url")//url检查
                {
                    System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                    System.Text.RegularExpressions.Match m = regUrl.Match(value);
                    if (!m.Success)
                    {
                        resp.errcode = 13;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }
                }
            }
            #endregion

            #region 检查是否已经报名
            if (!string.IsNullOrEmpty(strDistinctKeys))
            {

                if (!strDistinctKeys.Equals("none"))//自定义检查重复
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder("1=1 ");
                    string[] distinctKeys = strDistinctKeys.Split(',');
                    foreach (var item in distinctKeys)
                    {
                        sb.AppendFormat("And {0}='{1}' ", item, dicPar.Single(p => p.Key.Equals(item)).Value);
                    }
                    sb.Append("  and IsDelete = 0  ");
                    if (bll.GetCount<ActivityDataInfo>(sb.ToString()) > 0)
                    {

                        resp.errcode = 14;
                        resp.errmsg = "重复的报名!";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                }
                else//不检查重复
                {

                }
            }
            else//默认检查
            {
                if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}' and IsDelete = 0 ", activityIdBySignUp, phone)) > 0)
                {
                    resp.errcode = 15;
                    resp.errmsg = "已经报过名了!";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;


                }
            }



            #endregion


            var newActivityUID = 1001;
            var lastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", activityIdBySignUp));
            if (lastActivityDataInfo != null)
            {
                newActivityUID = lastActivityDataInfo.UID + 1;
            }
            ActivityDataInfo model = bll.ConvertRequestToModel<ActivityDataInfo>(new ActivityDataInfo());
            model.UID = newActivityUID;
            model.SpreadUserID = spreadUserId;
            model.ActivityID = activityIdBySignUp;
            if (juInfo.GuaranteeCreditAcount > 0)
            {
                if (model.GuaranteeCreditAcount < juInfo.GuaranteeCreditAcount)
                {
                    resp.errcode = 18;
                    resp.errmsg = string.Format( "担保信用金不能少于{0}!",Convert.ToDouble(juInfo.GuaranteeCreditAcount));
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            if (!string.IsNullOrEmpty(monitorPlanID))
            {
                model.MonitorPlanID = int.Parse(monitorPlanID);
            }
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.IsLogin)
            {
                UserInfo curUser = bll.GetCurrentUserInfo();
                model.UserId = curUser.UserID;
                model.WeixinOpenID = curUser.WXOpenId;
                if (context.Request["limit_userid_signupcount"] == "1")//限制每个登录账号只能报名一次
                {
                    if (bll.GetCount<ActivityDataInfo>(string.Format(" UserId='{0}' AND ActivityID={1} AND IsDelete=0 "
                        , model.UserId, juInfo.SignUpActivityID)) > 0)
                    {

                        resp.errcode = 14;
                        resp.errmsg = "重复的报名!";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }
                if (context.Request["limit_wxopenid_signupcount"] == "1")//限制每个微信只能报名一次
                {
                    if (bll.GetCount<ActivityDataInfo>(string.Format(" UserId='{0}' AND ActivityID={1} AND IsDelete=0 "
                        , model.WeixinOpenID, juInfo.SignUpActivityID)) > 0)
                    {
                        resp.errcode = 14;
                        resp.errmsg = "重复的报名!";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }
            }

            if (bll.Add(model))
            {
                bll.PlusNumericalCol("SignUpCount", juInfo.JuActivityID);//报名数+1

                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
                #region 当ActivityIntegral>0   扣积分
                if (juInfo.ActivityIntegral > 0)//扣积分
                {
                    currentUserInfo.TotalScore -= juInfo.ActivityIntegral;
                    if (bll.Update(currentUserInfo, string.Format("TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID)) <= 0)
                    {
                        resp.errcode = 16;
                        resp.errmsg = "扣除用户积分失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    else
                    {
                        //
                        BLLJIMP.Model.WBHScoreRecord scoreRecord = new BLLJIMP.Model.WBHScoreRecord();
                        scoreRecord.Nums = "b55";
                        scoreRecord.InsertDate = DateTime.Now;
                        scoreRecord.WebsiteOwner = bll.WebsiteOwner;
                        scoreRecord.UserId = currentUserInfo.UserID;
                        scoreRecord.RecordType = "2";
                        scoreRecord.NameStr = "参加活动:" + juInfo.ActivityName;
                        scoreRecord.ScoreNum = string.Format("-{0}", juInfo.ActivityIntegral);
                        if (!bll.Add(scoreRecord))
                        {
                            resp.errcode = 17;
                            resp.errmsg = "插入积分记录失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                    }
                }
                #endregion

                #region 当ActivityIntegral>0   扣信用金
                if (juInfo.GuaranteeCreditAcount > 0)//扣积分
                {
                    BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
                    bllUser.AddUserCreditAcountDetails(currentUserInfo.UserID, "ApplyCost", bllUser.WebsiteOwner, 0 - model.GuaranteeCreditAcount
                        , string.Format("报名【{0}】消耗{1}信用金", juInfo.ActivityName, Convert.ToDouble(model.GuaranteeCreditAcount)));
                }
                #endregion
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "报名失败，请重试或联系管理员!";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

    }
}