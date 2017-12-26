using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// 积分规则
    /// </summary>
    public class ScoreDefineListHandler : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser("");
        BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
        BLLKeyValueData bllkeyValueData = new BLLKeyValueData();
        UserInfo currentUserInfo;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.Status = -1;
                    resp.Msg = "未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string action = context.Request["Action"];
                switch (action)
                {
                    case "getDefineList":
                        result = getDefineList(context);
                        break;
                    case "putDefine":
                        result = PutDefine(context);
                        break;
                    case "delDefine":
                        result = DelDefine(context);
                        break;
                    case "EditRechargeConfig":
                        result = EditRechargeConfig(context);
                        break;
                    case "getRechargePriceList":
                        result = getRechargePriceList(context);
                        break;
                    case "AddRechargePrice":
                        result = AddRechargePrice(context);
                        break;
                    case "UpdateRechargePrice":
                        result = UpdateRechargePrice(context);
                        break;
                    case "DeleteRechargePrice":
                        result = DeleteRechargePrice(context);
                        break;
                    case "GetDefineListEx":
                        result = GetDefineListEx(context);
                        break;
                    case "AddEditDefineEx":
                        result = AddEditDefineEx(context);
                        break;
                    case "DelDefineEx":
                        result = DelDefineEx(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }
            context.Response.Write(result);
        }
        /// <summary>
        ///所有积分规则
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getDefineList(HttpContext context)
        {
            List<ScoreDefineInfo> scoreDefineInfoList = bllScoreDefine.GetScoreDefineList(bllUser.WebsiteOwner,null);
            return Common.JSONHelper.ObjectToJson(new {
                rows = scoreDefineInfoList,
                total = scoreDefineInfoList.Count
            });
        }
        /// <summary>
        /// 获取扩展规则设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDefineListEx(HttpContext context)
        {
            List<ScoreDefineInfoExt> scoreDefineInfoExList = bllScoreDefine.GetScoreDefineExList(int.Parse(context.Request["ScoreId"]));
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = scoreDefineInfoExList,
                total = scoreDefineInfoExList.Count
            });
        }
        /// <summary>
        /// 添加编辑积分规则
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string PutDefine(HttpContext context)
        {
            int scoreId = int.Parse(context.Request["score_id"]);
            int score = int.Parse(context.Request["score"]);
            int limit = int.Parse(context.Request["limit"]);
            int totalLimit = int.Parse(context.Request["total_limit"]);
            string summary = context.Request["summary"];
            string type = context.Request["type"];
            int hide = int.Parse(context.Request["hide"]);
            int order = int.Parse(context.Request["order"]);
            string ex1 = context.Request["ex1"];
            string scoreEvent=context.Request["score_event"];
            string baseRateValue = context.Request["base_rate_value"];
            string baseRateScore= context.Request["base_rate_score"];

            KeyVauleDataInfo scoreDefineData = bllkeyValueData.GetKeyData("ScoreDefineType", type, bllkeyValueData.WebsiteOwner);
            if (scoreDefineData == null)
            {
                scoreDefineData = bllkeyValueData.GetKeyData("ScoreDefineType", type, "Common");
                if (scoreDefineData == null)
                {
                    resp.Status = (int)APIErrCode.OperateFail;
                    resp.Msg = "规则类型暂不支持";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            ScoreDefineInfo scoreDefineInfo = new ScoreDefineInfo();
            scoreDefineInfo.ScoreId = scoreId;
            scoreDefineInfo.Score = score;
            scoreDefineInfo.DayLimit = limit;
            scoreDefineInfo.TotalLimit = totalLimit;
            scoreDefineInfo.Description = summary;
            scoreDefineInfo.WebsiteOwner = bllUser.WebsiteOwner;
            scoreDefineInfo.CreateUserId = this.currentUserInfo.UserID;
            scoreDefineInfo.IsHide = hide;
            scoreDefineInfo.InsertTime = DateTime.Now;
            scoreDefineInfo.OrderNum = order;
            scoreDefineInfo.Name = scoreDefineData.DataValue;
            scoreDefineInfo.ScoreType = scoreDefineData.DataKey;
            scoreDefineInfo.Ex1 = ex1;
            scoreDefineInfo.ScoreEvent = scoreEvent;
            if (!string.IsNullOrEmpty(baseRateValue))
            {
                scoreDefineInfo.BaseRateValue = decimal.Parse(baseRateValue);
            }
            if (!string.IsNullOrEmpty(baseRateScore))
            {
                 scoreDefineInfo.BaseRateScore = decimal.Parse(baseRateScore);
            }
            if (bllScoreDefine.PutScoreDefine(scoreDefineInfo))
            {
                resp.Status = 1;
                resp.Msg = "提交成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "提交失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        
        /// <summary>
        /// 添加编辑积分扩展规则
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddEditDefineEx(HttpContext context)
        {
            int id = int.Parse(context.Request["id"]);
            int scoreId = int.Parse(context.Request["score_id"]);
            string beginTime = context.Request["begin_time"];
            string endTime = context.Request["end_time"];
            string rateValue = context.Request["rate_value"];
            string rateScore = context.Request["rate_score"];
            ScoreDefineInfoExt model = new ScoreDefineInfoExt();
            if (id==0)//添加
            {


                model.WebsiteOwner = bllkeyValueData.WebsiteOwner;
                model.ScoreId = scoreId;
                model.BeginTime = DateTime.Parse(beginTime);
                model.EndTime = DateTime.Parse(endTime);
                model.RateValue = decimal.Parse(rateValue);
                model.RateScore = decimal.Parse(rateScore);

                if (bllScoreDefine.Add(model))
                {
                    resp.Status = 1;
                }



            }
            else
            {
                model = bllScoreDefine.Get<ScoreDefineInfoExt>(string.Format(" AutoId={0} And WebsiteOwner='{1}'", id, bllkeyValueData.WebsiteOwner));
                model.BeginTime = DateTime.Parse(beginTime);
                model.EndTime = DateTime.Parse(endTime);
                model.RateValue = decimal.Parse(rateValue);
                model.RateScore = decimal.Parse(rateScore);

                if (bllScoreDefine.Update(model))
                {
                    resp.Status = 1;
                }
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 删除积分规则
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelDefine(HttpContext context)
        {
            int scoreId = int.Parse(context.Request["scoreId"]);
            if (bllScoreDefine.DeleteScoreDefine(scoreId, bllScoreDefine.WebsiteOwner))
            {
                resp.Status = 1;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 删除积分扩展规则
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelDefineEx(HttpContext context)
        {
            int id = int.Parse(context.Request["id"]);
            if (bllScoreDefine.DeleteScoreDefineEx(id, bllScoreDefine.WebsiteOwner))
            {
                resp.Status = 1;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        private string EditRechargeConfig(HttpContext context)
        {

            string recharge = context.Request["Recharge"];
            string vipPrice = context.Request["VIPPrice"];
            string sendNoticePrice = context.Request["SendNoticePrice"];
            string minScore = context.Request["MinScore"];
            string minWithdrawCashScore = context.Request["MinWithdrawCashScore"];
            string vipPrice0 = context.Request["VIPPrice0"];
            string  vipDatelong = context.Request["VIPDatelong"];
            string  vipInterestID = context.Request["VIPInterestID"];
            string vipInterestDescription = context.Request["VIPInterestDescription"];
            string websiteOwner = bllkeyValueData.WebsiteOwner;
            KeyVauleDataInfo rechargeKeyValue = new KeyVauleDataInfo("Recharge", "100", recharge, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo sendNoticePriceValue = new KeyVauleDataInfo("SendNoticePrice", "1", sendNoticePrice, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo minScoreValue = new KeyVauleDataInfo("MinScore", "1", minScore, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo minWithdrawCashScoreValue = new KeyVauleDataInfo("MinWithdrawCashScore", "1", minWithdrawCashScore, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo VIPPriceKeyValue = new KeyVauleDataInfo("VIPPrice", "1", vipPrice, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo VIPPrice0KeyValue = new KeyVauleDataInfo("VIPPrice", "0", vipPrice0, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo VIPDatelongKeyValue = new KeyVauleDataInfo("VIPDatelong", "1", vipDatelong, websiteOwner, null, currentUserInfo.UserID);
            KeyVauleDataInfo VIPInterestIDKeyValue = new KeyVauleDataInfo("VIPInterestID", "1", vipInterestID, websiteOwner, null, currentUserInfo.UserID);
            JuActivityInfo juAct = new JuActivityInfo();
            BLLJuActivity bllJuAct = new BLLJuActivity();
            if (!string.IsNullOrWhiteSpace(vipInterestID) && vipInterestID != "0")
            {
                juAct = bllJuAct.GetJuActivity(Convert.ToInt32(vipInterestID));
                juAct.ActivityDescription = vipInterestDescription;
                if (!bllJuAct.PutArticle(juAct))
                {
                    resp.Status = -1;
                    resp.Msg = "提交VIP权益失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            else if (!string.IsNullOrWhiteSpace(vipInterestDescription))
            {
                vipInterestID = bllJuAct.GetGUID(TransacType.CommAdd);
                VIPInterestIDKeyValue.DataValue = vipInterestID;

                juAct.JuActivityID = Convert.ToInt32(vipInterestID);
                juAct.ActivityName = "VIP权益";
                juAct.ArticleType = "ConfigContent";
                juAct.UserID = currentUserInfo.UserID;
                juAct.WebsiteOwner = websiteOwner;
                juAct.CreateDate = DateTime.Now;
                juAct.ActivityDescription = vipInterestDescription;
                if (!bllJuAct.Add(juAct))
                {
                    resp.Status = -1;
                    resp.Msg = "提交VIP权益失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }

            BLLTransaction tran = new BLLTransaction();
            if (bllkeyValueData.PutDataValue(rechargeKeyValue, tran)
                && bllkeyValueData.PutDataValue(sendNoticePriceValue, tran)
                && bllkeyValueData.PutDataValue(minScoreValue, tran)
                && bllkeyValueData.PutDataValue(minWithdrawCashScoreValue, tran)
                && bllkeyValueData.PutDataValue(VIPPriceKeyValue, tran)
                && bllkeyValueData.PutDataValue(VIPPrice0KeyValue, tran)
                && bllkeyValueData.PutDataValue(VIPDatelongKeyValue, tran)
                && bllkeyValueData.PutDataValue(VIPInterestIDKeyValue, tran))
            {
                tran.Commit();
                resp.Status = 1;
                resp.Msg = "提交成功";
            }
            else
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = "提交失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string getRechargePriceList(HttpContext context)
        {
            List<KeyVauleDataInfo> rechargePriceList = bllkeyValueData.GetKeyVauleDataInfoList("RechargePrice", null, bllUser.WebsiteOwner);
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = rechargePriceList,
                total = rechargePriceList.Count
            });
        }
        private string AddRechargePrice(HttpContext context)
        {
            string dataValue = context.Request["DataValue"];
            string orderNum = context.Request["OrderNum"];
            KeyVauleDataInfo rechargePriceKeyValue = new KeyVauleDataInfo("RechargePrice", bllkeyValueData.GetGUID(TransacType.CommAdd), dataValue
                , bllkeyValueData.WebsiteOwner, null, currentUserInfo.UserID, orderNum);
            if (bllkeyValueData.Add(rechargePriceKeyValue))
            {
                resp.Status = 1;
                resp.Msg = "新增成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "新增失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string UpdateRechargePrice(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string dataValue = context.Request["DataValue"];
            string orderNum = context.Request["OrderNum"];
            KeyVauleDataInfo rechargePriceKeyValue = bllkeyValueData.GetKeyVauleData(autoId);
            if (rechargePriceKeyValue == null)
            {
                resp.Status = -1;
                resp.Msg = "找不到记录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            rechargePriceKeyValue.DataValue = dataValue;
            rechargePriceKeyValue.OrderBy = Convert.ToInt32(orderNum);
            if (bllkeyValueData.Update(rechargePriceKeyValue))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string DeleteRechargePrice(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bllkeyValueData.DeleteDataVaule(ids))
            {
                resp.Status = 1;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败";
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