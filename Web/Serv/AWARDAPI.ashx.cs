using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.BLLJIMP.Model;
using ZCJson.Linq;
using NetDimension.Json;
using ZentCloud.BLLJIMP.Enums;
using CommonPlatform.Helper;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 刮奖API
    /// </summary>
    public class AWARDAPI : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        protected AshxResponse resp = new AshxResponse(); // 统一回复响应数据
        /// <summary>
        /// 刮奖BLL
        /// </summary>
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        WebsiteInfo websiteInfo = new WebsiteInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bll.IsLogin)
                {
                    currentUserInfo = bll.GetCurrentUserInfo();
                    websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
                }
                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "action is null";
                    result = Common.JSONHelper.ObjectToJson(resp);
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
        /// 抽奖
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Scratch(HttpContext context)
        {

            try
            {


                if (!bll.IsLogin)
                {
                    return "请先登录";

                }
                string id = context.Request["id"];
                if (string.IsNullOrEmpty(id))
                {
                    return "id必传";
                }

                /*
                 * 检查是否是签到抽奖，是则：
                 * 1.当前是否是在周日，不在周日则不能抽奖
                 * 2.如果在周日，检查是否一周内有签到记录，否则不能抽奖
                 * 
                 */

                var signInModel = new BLLJIMP.BLLSignIn().Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND Type='Sign' ", currentUserInfo.WebsiteOwner));
                if (signInModel !=null&& signInModel.LotteryId == id)
                {
                    var dt = DateTime.Now;
                    
                    if (dt.DayOfWeek != DayOfWeek.Sunday)
                    {
                        return "周日才可以抽奖";
                    }

                    //获取本周签到记录，倒退七天是否有七条
                    var signInCount = bll.GetCount<SignInLog>(string.Format("  cast(SignInDate as date) between '{0}' and '{1}' and  WebsiteOwner = '{2}' and UserID = '{3}' ",
                            dt.AddDays(-6).ToString("yyyy-MM-dd"),
                            dt.ToString("yyyy-MM-dd"),
                            currentUserInfo.WebsiteOwner,
                            currentUserInfo.UserID
                        ));

                    if (signInCount < 7)
                    {
                        return "一周连续签到才可以抽奖";
                    }

                }

                #region 检查是否可以继续
                ScratchJsonModel apiResult = new ScratchJsonModel();
                apiResult.awardName = "谢谢参与";
                WXLotteryV1 model = bll.Get<WXLotteryV1>(string.Format("LotteryID={0}", id));
                if (model == null)
                {
                    goto outoff;
                }
                apiResult.awardId = model.LotteryID;
                apiResult.isStart = model.Status == 1 ? true : false;
                if (model.StartTime != null)
                {
                    apiResult.startTime = model.StartTime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                    if (model.StartTime <= DateTime.Now)//已经到了开奖时间了
                    {
                        if (model.Status == 1)
                        {
                            apiResult.isStart = true;
                        }
                        else
                        {
                            apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.NotStart;
                            apiResult.awardName = "刮奖未开启";
                            apiResult.isStart = false;
                        }
                    }
                    else//开奖时间还没到
                    {
                        apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.NotStart;
                        apiResult.awardName = "未到刮奖时间";
                        apiResult.isStart = false;
                        goto outoff;

                    }

                }

                //检查结束时间
                if (model.EndTime != null)
                {
                    apiResult.startTime = model.EndTime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                    if (model.EndTime.Value <= DateTime.Now)
                    {
                        apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.IsEnd;
                        apiResult.awardName = "已结束";
                        apiResult.isStart = false;
                        goto outoff;
                    }
                }

                if (context.Request["hideAward"] != "1")
                {
                    apiResult.winRecord = bll.GetWXLotteryRecordList(model.LotteryID, currentUserInfo.UserID);
                }
                if (model.WinLimitType.Equals(0))
                {
                    var lotteryRecord = bll.GetWXLotteryRecordV1(currentUserInfo.UserID, model.LotteryID);
                    if (lotteryRecord != null)
                    {

                        if (context.Request["hideAward"] != "1")
                        {
                            WXLotteryLogV1 logWinLimit = new WXLotteryLogV1();
                            logWinLimit.LotteryId = model.LotteryID;
                            logWinLimit.UserId = currentUserInfo.UserID;
                            logWinLimit.InsertDate = DateTime.Now;
                            logWinLimit.IP = Common.MySpider.GetClientIP();
                            if (bll.AddWXLotteryLogV1(logWinLimit))//下一步 
                            {

                            }

                            apiResult.awardName = string.Format("{0}", lotteryRecord.WXAwardName);
                            apiResult.isAward = true;

                            //告诉前端你已经中过奖了
                            apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.LotteryHaveRecord; ;//LotteryHaveRecord
                        }
                        goto outoff;
                    }
                }

                if (!apiResult.isStart)
                {
                    apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.NotStart;
                    apiResult.awardName = "刮奖未开启";
                    apiResult.isStart = false;
                    goto outoff;
                }
                #endregion

                #region 内定中奖
                List<WXLotteryWinningDataV1> winData = bll.GetList<WXLotteryWinningDataV1>(string.Format("LotteryId={0}", model.LotteryID));
                if (winData.SingleOrDefault(p => p.UserId.Equals(currentUserInfo.UserID)) != null)
                {


                    //给默认中奖者中奖
                    WXLotteryRecordV1 record = new WXLotteryRecordV1();
                    record.InsertDate = DateTime.Now;
                    record.LotteryId = model.LotteryID;
                    record.Token = "0";
                    record.WXAwardsId = winData.Single(p => p.UserId.Equals(currentUserInfo.UserID)).WXAwardsId;
                    record.UserId = currentUserInfo.UserID;
                    record.IsGetPrize = 0;
                    if (bll.AddWXLotteryRecordV1(record))
                    {
                        apiResult.awardName = string.Format("{0}", winData.Single(p => p.UserId.Equals(currentUserInfo.UserID)).WXAwardName);
                        apiResult.isAward = true;


                    }

                    goto outoff;
                }
                //设置默认中奖 
                #endregion

                #region 外部积分读取
                var isSyncYike = new BLLJIMP.BLLCommRelation().ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, currentUserInfo.WebsiteOwner, "");

                Open.EZRproSDK.Client zrClient = new Open.EZRproSDK.Client();

                if (isSyncYike)
                {
                    var getBonusResp = zrClient.GetBonus(currentUserInfo.Ex1, currentUserInfo.Ex2, currentUserInfo.Phone);
                    if (getBonusResp != null)
                    {
                        currentUserInfo.TotalScore = getBonusResp.Bonus;
                    }
                    else
                    {
                        currentUserInfo.TotalScore = 0;
                    }

                }
                #endregion


                #region 使用宏巍积分

                Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                if (websiteInfo.IsUnionHongware == 1)
                {
                    var hongWareMemberInfo = hongWareClient.GetMemberInfo(currentUserInfo.WXOpenId);
                    if (hongWareMemberInfo.member != null)
                    {

                        currentUserInfo.TotalScore = hongWareMemberInfo.member.point;

                    }
                    else
                    {
                        currentUserInfo.TotalScore = 0;
                    }


                }
                #endregion

                string tempMsg = "";
                #region 抽奖限制 总共多少次或每天几次  用购买的刮奖次数除外

                #region 总共多少次
                if (model.LuckLimitType.Equals(0))//总共多少次
                {
                    int count = bll.GetWXLotteryLogCountV1(model.LotteryID, currentUserInfo.UserID);
                    if (count >= model.MaxCount)
                    {
                        //判读是否够积分
                        if (model.UsePoints > 0)
                        {
                            if (currentUserInfo.TotalScore >= model.UsePoints)
                            {
                                //扣积分
                                if (isSyncYike)
                                {
                                    var resp = zrClient.BonusUpdate(currentUserInfo.Ex2, -model.UsePoints, "抽奖减少积分");
                                }
                                else
                                {
                                    bllUser.AddUserScoreDetail(currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.Lottery), bllUser.WebsiteOwner, out tempMsg, -model.UsePoints, "抽奖减少积分", "", false);
                                }
                            }
                            else
                            {
                                if (currentUserInfo.LotteryCount == 0)
                                {
                                    apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.IntegralProblem;
                                    apiResult.awardName = "积分不足";
                                    apiResult.isStart = false;
                                    goto outoff;
                                }
                                else
                                {
                                    //剩余刮奖次数减1
                                    if (bll.Update(currentUserInfo, string.Format(" LotteryCount-=1"), string.Format("AutoId={0}", currentUserInfo.AutoID)) <= 0)
                                    {
                                        apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.IntegralProblem;
                                        apiResult.awardName = "谢谢参与";
                                        apiResult.isStart = false;
                                        goto outoff;
                                    }

                                }


                            }

                        }
                        else
                        {
                            apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.CountIsOver;
                            apiResult.awardName = "谢谢参与";
                            goto outoff;
                        }
                    }
                }
                #endregion

                #region 每天多少次
                else
                {
                    //判读一天有没有超出
                    int count = bll.GetWXLotteryLogCountV1(model.LotteryID, currentUserInfo.UserID, DateTime.Now);
                    if (count >= model.MaxCount)
                    {

                        //判读是否够积分
                        if (model.UsePoints > 0)
                        {
                            if (currentUserInfo.TotalScore >= model.UsePoints)
                            {
                                if (isSyncYike)
                                {
                                    var resp = zrClient.BonusUpdate(currentUserInfo.Ex2, -model.UsePoints, "抽奖减少积分");
                                }
                                else
                                {
                                    //扣积分
                                    bllUser.AddUserScoreDetail(currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.Lottery), bllUser.WebsiteOwner, out tempMsg, -model.UsePoints, "抽奖减少积分", "", false);
                                }
                            }
                            else
                            {
                                if (currentUserInfo.LotteryCount == 0)
                                {
                                    apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.IntegralProblem;
                                    apiResult.awardName = "积分不足";
                                    apiResult.isStart = false;
                                    goto outoff;
                                }
                                else
                                {
                                    //剩余刮奖次数减1
                                    if (bll.Update(currentUserInfo, string.Format(" LotteryCount-=1"), string.Format("AutoId={0}", currentUserInfo.AutoID)) <= 0)
                                    {
                                        apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.IntegralProblem;
                                        apiResult.awardName = "谢谢参与";
                                        apiResult.isStart = false;
                                        goto outoff;
                                    }

                                }




                            }

                        }
                        else
                        {
                            apiResult.errorCode = (int)BLLJIMP.Enums.APIErrCode.CountIsOver;
                            apiResult.awardName = "谢谢参与";
                            goto outoff;
                        }

                    }

                }
                #endregion

                #endregion

                WXLotteryLogV1 log = new WXLotteryLogV1();
                log.LotteryId = model.LotteryID;
                log.UserId = currentUserInfo.UserID;
                log.InsertDate = DateTime.Now;
                log.IP = Common.MySpider.GetClientIP();
                if (bll.AddWXLotteryLogV1(log))//下一步 
                {
                    #region 奖池生成
                    List<AwardModel> awardModelList = new List<AwardModel>();//奖池
                    List<WXAwardsV1> awardsList = bll.GetAwardsListV1(model.LotteryID).Where(p => p.WinCount < p.PrizeCount).ToList();//奖品列表 //已经中完的不参与
                    foreach (var item in awardsList)
                    {
                        for (int i = 1; i <= item.Probability; i++)
                        {
                            AwardModel m = new AwardModel();
                            m.AwardID = item.AutoID;
                            m.PrizeName = item.PrizeName;
                            m.AwardsType = item.AwardsType;
                            m.Img = item.Img;
                            m.Value = item.Value;
                            awardModelList.Add(m);
                        }
                    }
                    if (awardsList.Sum(p => p.Probability) < 100)//总中奖概率小于100 补足
                    {
                        for (int i = 1; i <= (100 - (awardsList.Sum(p => p.Probability))); i++)
                        {
                            AwardModel m = new AwardModel();
                            m.AwardID = 0;
                            m.PrizeName = "谢谢参与";
                            awardModelList.Add(m);
                        }
                    }
                    #endregion
                    //打乱数组顺序
                    awardModelList = GetRandomList<AwardModel>(awardModelList);
                    Random rand = new Random();
                    int index = rand.Next(0, awardModelList.Count);

                    #region 抽奖
                    if (awardModelList[index].AwardID > 0)//随机数中奖
                    {

                        string sqlUpdate = string.Format(" Update ZCJ_WXAwardsV1  Set WinCount+=1  where AutoID={0} And WinCount<PrizeCount ", awardModelList[index].AwardID);
                        if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sqlUpdate) > 0)
                        {
                            int token = rand.Next(1111, 9999);
                            //插入中奖记录
                            WXLotteryRecordV1 record = new WXLotteryRecordV1();
                            record.InsertDate = DateTime.Now;
                            record.LotteryId = model.LotteryID;
                            record.Token = token.ToString();
                            record.WXAwardsId = awardModelList[index].AwardID;
                            record.UserId = currentUserInfo.UserID;
                            record.IsGetPrize = 0;
                            if (bll.AddWXLotteryRecordV1(record))
                            {
                                apiResult.awardName = string.Format("{0}", awardModelList[index].PrizeName);
                                apiResult.isAward = true;
                                apiResult.awardsType = awardModelList[index].AwardsType;
                                apiResult.img = awardModelList[index].Img;
                                apiResult.value = awardModelList[index].Value;

                                if (apiResult.awardsType == 2)
                                {
                                    BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();

                                    var coupon = bllCardCoupon.GetCardCoupon(int.Parse(apiResult.value));
                                    apiResult.cardcoupon_name = coupon.Name;
                                    apiResult.cardcoupon_type = coupon.CardCouponType;
                                    apiResult.cardcoupon_start = coupon.ValidFrom == null ? "" : coupon.ValidFrom.Value.ToShortDateString();
                                    apiResult.cardcoupon_end = coupon.ValidTo == null ? "" : coupon.ValidTo.Value.ToShortDateString();
                                    if (coupon.ExpireTimeType == "1")
                                    {
                                        apiResult.cardcoupon_start = DateTime.Now.ToShortDateString();
                                        apiResult.cardcoupon_end = DateTime.Now.AddDays(Convert.ToInt32(coupon.ExpireDay)).ToShortDateString();
                                    }

                                }
                                if (apiResult.awardsType == 1 || apiResult.awardsType == 2)
                                {
                                    //中奖记录标记为已经领奖
                                    WXLotteryRecordV1 rec = bll.Get<WXLotteryRecordV1>(string.Format(" UserId='{0}' And LotteryId={1} And WXAwardsId={2} And Token='{3}'", record.UserId, record.LotteryId, record.WXAwardsId, record.Token));
                                    if (rec != null)
                                    {
                                        rec.IsGetPrize = 1;
                                        bll.Update(rec);
                                    }


                                }

                                goto outoff;
                            }
                            else
                            {
                                goto outoff;
                            }
                        }
                        else
                        {
                            goto outoff;
                        }

                        //}
                        //else
                        //{   //#region 虽然随机数中奖，但是奖项数量已经达到上限,重新随机抽奖
                        //    //if (reCount >= 5)
                        //    //{
                        //    goto outoff;
                        //    //}
                        //    //reCount++;
                        //    //goto start;
                        //}
                    }
                    else
                    {
                        //随机数未中奖

                    }
                    #endregion
                }
                outoff:
                if (!string.IsNullOrEmpty(context.Request["callback"]))
                {
                    return string.Format("{0}({1})", context.Request["callback"], Common.JSONHelper.ObjectToJson(apiResult));
                }
                return Common.JSONHelper.ObjectToJson(apiResult);
            }
            catch (Exception ex)
            {

                return ex.ToString();

            }
        }
        /// <summary>
        /// 领奖
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAward(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.Status = -1;
                resp.Msg = "请先登录";
                goto outoff;

            }

            string rid = context.Request["rid"];//奖项id

            if (string.IsNullOrEmpty(context.Request["id"]))
            {
                resp.Status = -1;
                resp.Msg = "编号不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(rid))
            {
                resp.Status = -1;
                resp.Msg = " rid 参数不能为空";
                goto outoff;
            }
            //var lotteryRecord = bll.GetWXLotteryRecordV1(currentUserInfo.UserID, int.Parse(context.Request["id"]),int.Parse(awardId));
            var lotteryRecord = bll.GetWXLotteryRecordByRecordIdV1(currentUserInfo.UserID, int.Parse(context.Request["id"]), int.Parse(rid));
            if (lotteryRecord == null)
            {
                resp.Status = -1;
                resp.Msg = "您没有中奖，不能领奖";
                goto outoff;

            }
            if (lotteryRecord.IsGetPrize.Equals(1))
            {
                resp.Status = -1;
                resp.Msg = "您已经领过奖";
                goto outoff;
            }
            lotteryRecord.IsGetPrize = 1;
            if (bll.Update(lotteryRecord))
            {
                resp.IsSuccess = true;
                resp.Status = 1;
                resp.Msg = "领奖成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "领奖失败，请重试";
            }
        outoff:
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                return string.Format("{0}({1})", context.Request["callback"], Common.JSONHelper.ObjectToJson(resp));
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 提交领奖信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitInfo(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.Status = -1;
                resp.Msg = "请先登录";
                goto outoff;

            }
            string id = context.Request["id"];
            string awardId = context.Request["aid"];//奖项id
            string recordId = context.Request["rid"];//中奖记录id
            string name = context.Request["name"];
            string phone = context.Request["phone"];
            if (string.IsNullOrEmpty(id))
            {
                resp.Status = -1;
                resp.Msg = " id 参数不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(awardId))
            {
                resp.Status = -1;
                resp.Msg = " awardId 参数不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(recordId))
            {
                resp.Status = -1;
                resp.Msg = " recordId 参数不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(name))
            {
                resp.Status = -1;
                resp.Msg = " name 参数不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.Status = -1;
                resp.Msg = " phone 参数不能为空";
                goto outoff;
            }
            var lotteryRecord = bll.GetWXLotteryRecordByRecordIdV1(currentUserInfo.UserID, int.Parse(id), int.Parse(recordId));
            if (lotteryRecord == null)
            {
                resp.Status = -1;
                resp.Msg = "您没有中奖，不能提交信息";
                goto outoff;

            }
            if (lotteryRecord.IsGetPrize.Equals(1))
            {
                resp.Status = -1;
                resp.Msg = "您已经领过奖";
                goto outoff;
            }
            lotteryRecord.Name = context.Request["name"];
            lotteryRecord.Phone = context.Request["phone"];
            if (bll.Update(lotteryRecord))
            {
                resp.IsSuccess = true;
                resp.Status = 1;
                resp.Msg = "提交信息成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "提交信息失败，请重试";
            }
        outoff:
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                return string.Format("{0}({1})", context.Request["callback"], Common.JSONHelper.ObjectToJson(resp));
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取中奖记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAwardRecord(HttpContext context)
        {
            AwardRecordApi result = new AwardRecordApi();
            List<LotteryRecordApi> list = new List<LotteryRecordApi>();
            if (string.IsNullOrEmpty(context.Request["id"]))
            {
                // resp.Msg = "编号不能为空";
                goto outOff;
            }
            if (string.IsNullOrEmpty(context.Request["pageindex"]))
            {
                //resp.Msg = "pageindex不能为空";
                goto outOff;
            }
            if (string.IsNullOrEmpty(context.Request["pagesize"]))
            {
                //resp.Msg = "pagesize不能为空";
                goto outOff;
            }
            int id = int.Parse(context.Request["id"]);
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            result.totalcount = bll.GetCount<WXLotteryRecordV1>(string.Format("LotteryId={0}", id));
            var source = bll.GetWXLotteryRecordV1(id, pageIndex, pageSize);

            foreach (var item in source)
            {
                LotteryRecordApi model = new LotteryRecordApi();
                model.awardname = item.WXAwardName;
                model.headimg = bllUser.GetUserDispalyAvatar(item.UserInfo);
                model.nickname = item.WXNickName;
                if (string.IsNullOrEmpty(model.nickname))
                {
                    model.nickname = "";
                }
                list.Add(model);
            }

        outOff:
            result.list = list;
            return Common.JSONHelper.ObjectToJson(result);

        }

        /// <summary>
        /// 获取抽奖信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetLottery(HttpContext context)
        {
            int id = Convert.ToInt32(context.Request["id"]);
            if (id == 0)
            {
                resp.IsSuccess = false;
                resp.Msg = "请传入正确的抽奖活动ID";
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.NoFollow;
                return JsonConvert.SerializeObject(resp);
            }

            if (!bll.IsLogin)
            {
                resp.IsSuccess = false;
                resp.Msg = "请先登录";
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return JsonConvert.SerializeObject(resp);
            }
            var model = bll.GetLottery(id);
            if (model == null)
            {
                resp.IsSuccess = false;
                resp.Msg = "未找到抽奖活动";
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.NoFollow;
                return JsonConvert.SerializeObject(resp);
            }

            /*
            
            返回抽奖活动基本信息
            返回当前用户抽奖记录
            返回当前用户中奖情况

            */

            List<WXLotteryLogV1> log = new List<WXLotteryLogV1>();
            List<WXLotteryRecordV1> winRecord = new List<WXLotteryRecordV1>();
            List<WXAwardsV1> awards = new List<WXAwardsV1>();

            //隐藏获奖记录，和奖项信息
            if (context.Request["hideAward"] != "1")
            {
                log = bll.GetUserLotteryLog(id, currentUserInfo.UserID);
                winRecord = bll.GetWXLotteryRecordList(id, currentUserInfo.UserID);
                awards = bll.GetAwardsListV1(id);
            }
            else
            {
                log = bll.GetUserLotteryLog(1, id, currentUserInfo.UserID);
            }

            //判断当前用户今天还能摇奖多少次
            var luckRest = 0;
            switch (model.LuckLimitType)
            {
                case 0://总共多少次
                    luckRest =model.MaxCount - log.Count;
                    break;
                case 1://每天多少次
                    var today = DateTime.Now;
                    var todayCount = log.Count(p => p.InsertDate.Year == today.Year && p.InsertDate.Month == today.Month && p.InsertDate.Day == today.Day);
                    luckRest = model.MaxCount - todayCount;
                    break;
                default:
                    break;
            }
            luckRest += currentUserInfo.LotteryCount;
            if (luckRest<0)
            {
                luckRest = 0;
            }


            string currAwardName = string.Empty;//当前中奖的名称
            int currIsCashed = 0;//当前是否已领奖
            int currIsSubmitInfo = 0;//当前是否已提交领奖信息
            WXLotteryRecordV1 lotteryRecord = new WXLotteryRecordV1();
            List<dynamic> recordList = new List<dynamic>();

            if (winRecord != null)
            {
                var wr = winRecord.Where(p => p.UserId == currentUserInfo.UserID).ToList();
                if (wr != null && wr.Count > 0)
                {
                    lotteryRecord = wr[0];//bll.GetWXLotteryRecordV1(currentUserInfo.UserID, model.LotteryID);

                    currAwardName = lotteryRecord.WXAwardName;
                    currIsCashed = bll.IsUserGetPrizeV1(currentUserInfo.UserID, model.LotteryID) ? 1 : 0;
                    if ((!string.IsNullOrEmpty(lotteryRecord.Name)) && (!string.IsNullOrEmpty(lotteryRecord.Phone)))
                    {
                        currIsSubmitInfo = 1;
                    }
                }

            }


            //var lotteryRecord = winRecord.Where(p => p.UserId == currentUserInfo.UserID).ToList();
            //List<dynamic> awardList = new List<dynamic>();
            //if (lotteryRecord.Count > 0)
            //{
            //    foreach (var item in lotteryRecord)
            //    {
            //        awardList.Add(new
            //        {
            //            cueeAwardName = item.WXAwardName,//当前中奖的名称
            //            currIsCashed = bll.IsUserGetPrizeV1(currentUserInfo.UserID, item.LotteryId) ? 1 : 0,
            //            currIsSubmitInfo = !string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Phone) ? 1 : 0
            //        });
            //    }
            //}

            MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
            detailInfo.MonitorPlanID = id;
            detailInfo.EventType = 0;
            detailInfo.EventBrowser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
            detailInfo.EventBrowserID = HttpContext.Current.Request.Browser.Id; ;
            if (HttpContext.Current.Request.Browser.Beta)
                detailInfo.EventBrowserIsBata = "测试版";
            else
                detailInfo.EventBrowserIsBata = "正式版";

            detailInfo.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;
            detailInfo.EventDate = DateTime.Now;
            if (HttpContext.Current.Request.Browser.Win16)
                detailInfo.EventSysByte = "16位系统";
            else
                if (HttpContext.Current.Request.Browser.Win32)
                    detailInfo.EventSysByte = "32位系统";
                else
                    detailInfo.EventSysByte = "64位系统";
            detailInfo.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
            detailInfo.SourceIP = ZentCloud.Common.MySpider.GetClientIP();
            detailInfo.IPLocation = ZentCloud.Common.MySpider.GetIPLocation(detailInfo.SourceIP);
            detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
            detailInfo.RequesSourcetUrl = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() : "";
            detailInfo.WebsiteOwner = bll.WebsiteOwner;
            detailInfo.ModuleType = "shake";
            if (bll.IsLogin)
            {
                detailInfo.EventUserID = bll.GetCurrUserID();
            }
            bll.Add(detailInfo);

            int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, id));
            int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, id));
            int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, id));

            bll.Update(new WXLotteryV1(), string.Format(" IP={0},PV={1},UV={2} ", ipCount, pvCount, uvCount), string.Format(" LotteryID={0} ", id));

            resp.IsSuccess = true;

            //判断活动是否已结束
            var awardGameOver = false;

            if (model.EndTime != null)
            {
                if (model.EndTime.Value < DateTime.Now)
                {
                    awardGameOver = true;
                }
            }

            if (model.Status == 0)
            {
                awardGameOver = true;
            }

            resp.Result = new
            {
                //抽奖活动基本信息
                name = model.LotteryName,
                content = model.LotteryContent,
                status = model.Status,
                startTime = Common.DateTimeHelper.DateTimeToStr(model.StartTime),
                endTime = Common.DateTimeHelper.DateTimeToStr(model.EndTime),
                shareImg = model.ShareImg,
                shareDesc = model.ShareDesc,
                log = log == null ? null : log.Select(p => new { time = p.InsertDate.ToString() }),
                winRecord = winRecord == null ? null : winRecord.Select(p => new
                {
                    rid = p.AutoID,
                    id = p.WXAwardsId,
                    token = p.Token,
                    name = p.WXAward.PrizeName,
                    time = p.InsertDateStr,
                    type = p.WXAward.AwardsType,
                    is_reveice = bll.IsUserGetPrizeByRecordIdV1(p.AutoID) ? 1 : 0,
                    org_obj = p.WXAward,
                    record_user_name = p.Name,
                    record_user_phone = p.Phone
                }),

                maxCount = model.MaxCount,
                luckLimitType = model.LuckLimitType,
                winLimitType = model.WinLimitType,
                usePoints = model.UsePoints,
                awards = awards,
                luckRest = luckRest,
                toolbarbutton = model.ToolbarButton,
                websiteownername = bll.GetWebsiteInfoModel().WebsiteName,
                currIsAward = lotteryRecord != null ? 1 : 0,
                currAwardName = currAwardName,
                currIsCashed = currIsCashed,
                currIsSubmitInfo = currIsSubmitInfo,
                isGetPrizeFromMobile = model.IsGetPrizeFromMobile,
                currUserName = currentUserInfo.TrueName,
                currUserPhone = currentUserInfo.Phone,
                awardGameOver = awardGameOver
            };
            return JsonConvert.SerializeObject(resp);
        }

        ///// <summary>
        ///// 检查是否领过奖品
        ///// </summary>
        ///// <param name="UserId">当前用户的id</param>
        ///// <param name="autoId">活动编号</param>
        ///// <returns>返回是否领奖成功</returns>
        //private bool IsUserGetPrizeV1(string UserId, int autoId)
        //{
        //    WXLotteryRecordV1 wxlRecord = bll.Get<WXLotteryRecordV1>(" UserId='" + UserId + "' And LotteryId=" + autoId);
        //    if (wxlRecord != null)
        //    {
        //        if (wxlRecord.IsGetPrize == 1)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}


        /// <summary>
        /// 获取抽奖信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetLotteryDetail(HttpContext context)
        {
            string id = context.Request["id"];
            var model = bll.Get<WXLotteryV1>(string.Format(" LotteryID={0}", id));
            if (model != null)
            {
                var data = new
                {
                    name = model.LotteryName,
                    img_url = bll.GetImgUrl(model.ThumbnailsPath),
                    share_img_url = bll.GetImgUrl(model.ShareImg),
                    share_desc = model.ShareDesc,
                    background_color = model.BackGroundColor,
                    content = model.LotteryContent
                };

                return ZentCloud.Common.JSONHelper.ObjectToJson(data);
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 1,
                errmsg = "false"
            });
        }

        /// <summary>
        /// 中奖记录api
        /// </summary>
        public class AwardRecordApi
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 中奖名单
            /// </summary>
            public List<LotteryRecordApi> list { get; set; }
        }
        /// <summary>
        /// 中奖名单api
        /// </summary>
        public class LotteryRecordApi
        {
            /// <summary>
            /// 头像
            /// </summary>
            public string headimg { get; set; }
            /// <summary>
            /// 昵称
            /// </summary>
            public string nickname { get; set; }
            /// <summary>
            /// 奖品名称
            /// </summary>
            public string awardname { get; set; }

        }

        /// <summary>
        /// 返回模型
        /// </summary>
        private class ScratchJsonModel
        {
            /// <summary>
            /// 抽奖编号
            /// </summary>
            public int awardId { get; set; }
            /// <summary>
            /// 奖项名称
            /// </summary>
            public string awardName { get; set; }
            /// <summary>
            /// 是否中奖
            /// </summary>
            public bool isAward { get; set; }
            /// <summary>
            /// 是否已经开始
            /// </summary>
            public bool isStart { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string startTime { get; set; }
            /// <summary>
            /// 获奖记录
            /// </summary>
            public List<BLLJIMP.Model.WXLotteryRecordV1> winRecord { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime { get; set; }

            /// <summary>
            /// 图片
            /// </summary>
            public string img { get; set; }
            /// <summary>
            /// 奖项类型：0为默认自定义，1积分，2优惠券
            /// </summary>
            public int awardsType { get; set; }
            /// <summary>
            /// 价值
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 优惠券名称
            /// </summary>
            public string cardcoupon_name { get; set; }
            /// <summary>
            /// 优惠券类型
            /// </summary>
            public string cardcoupon_type { get; set; }
            /// <summary>
            /// 优惠券生效时间
            /// </summary>
            public string cardcoupon_start { get; set; }
            /// <summary>
            /// 优惠券失效时间
            /// </summary>
            public string cardcoupon_end { get; set; }
            /// <summary>
            /// 错误码
            /// </summary>
            public int errorCode { get; set; }

        }

        /// <summary>
        /// 奖池
        /// </summary>
        private class AwardModel
        {
            /// <summary>
            /// 奖品ID
            /// </summary>
            public int AwardID { get; set; }
            /// <summary>
            /// 奖品名称
            /// </summary>
            public string PrizeName { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string Img { get; set; }
            /// <summary>
            /// 奖项类型：0为默认自定义，1积分，2优惠券
            /// </summary>
            public int AwardsType { get; set; }
            /// <summary>
            /// 价值
            /// </summary>
            public string Value { get; set; }

        }

        /// <summary>
        /// 数组随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
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