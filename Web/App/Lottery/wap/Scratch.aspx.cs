using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery.wap
{
    public partial class Scratch : System.Web.UI.Page
    {

        public WXLottery model;
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        /// <summary>
        /// 刮奖区域显示内容
        /// </summary>
        public string sbLottery = "谢谢参与";
        public string IsWin = "false"; //是否中奖
        public string IsGetPrize = "false";  //是否领奖
        public int UserLogCount;//抽奖记录数
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!bll.IsLogin())
                {
                    Response.Write("您尚未登录,不能刮奖");
                    Response.End();
                   
                }

                UserInfo CurrentUserInfo = Comm.DataLoadTool.GetCurrUserModel();
                model = bll.Get<WXLottery>(string.Format("AutoID={0}", Request["id"]));


                if (model == null)
                {
                    Response.End();
                }
                if (!string.IsNullOrEmpty(model.LotteryActivityID))//有关联的活动ID
                {
                    if (bll.GetCount<WXSignInInfo>(string.Format("SignInUserID='{0}' And JuActivityID={1}",CurrentUserInfo.UserID,model.LotteryActivityID))==0)
                    {
                        sbLottery = "请签到后再抽奖";
                        return;
                    }
                }

                //if (model.Status.Equals(0))
                //{

                //    sbLottery = "刮奖未开启";
                //    if (!string.IsNullOrEmpty(model.StopMessage))
                //    {
                //        sbLottery = model.StopMessage;
                //    }
                //    return;
                //}

                //#region 特殊处理
                
                //if ((!string.IsNullOrEmpty(WinList.SingleOrDefault(p => p.Key == CurrentUserInfo.WXOpenId).Value) && model.AutoID.Equals(9)))
                //{

                //    //先查看中奖记录
                //    WXLotteryRecord roc = bll.Get<WXLotteryRecord>(string.Format("LotteryId=9 And UserId='{0}'", CurrentUserInfo.UserID));
                //    if (roc != null)//已经中过奖
                //    {
                //        sbLottery = string.Format("恭喜!您获得了<br/>{0}</br>领取码:{1}", WinList.Single(p => p.Key == CurrentUserInfo.WXOpenId).Value, roc.Token);
                //        IsWin = "true";
                //        UserLogCount = 1;
                //        if (IsUserGetPrize(CurrentUserInfo.UserID, model.AutoID).Equals("true"))
                //        {

                //            IsGetPrize = "true";
                //        }
                //        return;
                //    }
                //    //
                //    //
                //    if (model.Status.Equals(0))
                //    {

                //        sbLottery = "刮奖未开启";
                //        if (!string.IsNullOrEmpty(model.StopMessage))
                //        {
                //            sbLottery = model.StopMessage;
                //        }
                //        return;
                //    }
                //    //
                //    //中奖
                //    Random r = new Random();
                //    WXLotteryRecord Record = new WXLotteryRecord();
                //    Record.InsertDate = DateTime.Now;
                //    Record.LotteryId = model.AutoID;
                //    Record.Token = r.Next(1, 9999).ToString();
                //    Record.WXAwardsId = 0;
                //    Record.UserId = CurrentUserInfo.UserID;
                //    Record.IsGetPrize = "0";
                //    if (bll.AddWXLotteryRecord(Record))
                //    {
                      
                //        sbLottery = string.Format("恭喜!您获得了<br/>{0}</br>领取码:{1}", WinList.Single(p => p.Key == CurrentUserInfo.WXOpenId).Value, Record.Token);
                //        IsWin = "true";
                //        return;
                //    }


                //}
              
                //#endregion


                if (!string.IsNullOrEmpty(model.NotWinMessage))
                {
                    sbLottery =model.NotWinMessage;
                }
                var LotteryRecord=bll.GetWXLotteryRecord(CurrentUserInfo.UserID,model.AutoID);
                if (LotteryRecord != null)
                {
                    sbLottery = string.Format("您已经获得了:<br/>{0}<br/>领取码:{1}", LotteryRecord.WXAwardName, LotteryRecord.Token);
                    UserLogCount = 1;
                    IsWin = "true";
                    if (IsUserGetPrize(CurrentUserInfo.UserID, model.AutoID).Equals("true"))
                    {

                        IsGetPrize = "true";
                    }
                    return;
                }


                //先插入刮奖日志
                //检查用户刮奖次数
                UserLogCount = bll.GetWXLotteryLogCount(model.AutoID, CurrentUserInfo.UserID);
                if (UserLogCount >= model.MaxCount)
                {
                    //sbLottery = "您的刮奖次数已经用完了";
                    return;
                }

                if (IsUserGetPrize(CurrentUserInfo.UserID, model.AutoID).Equals("true"))
                {
                    IsWin = "true";
                    sbLottery = string.Format("恭喜!您已经领过奖品！！！"); return;
                }
                if (model.Status.Equals(0))
                {

                    sbLottery = "刮奖未开启";
                    if (!string.IsNullOrEmpty(model.StopMessage))
                    {
                        sbLottery = model.StopMessage;
                    }
                    return;
                }

                WXLotteryLog log = new WXLotteryLog();
                log.LotteryId = model.AutoID;
                log.UserId = CurrentUserInfo.UserID;
                log.InsertDate = DateTime.Now;
                if (bll.AddWXLotteryLog(log))//下一步 
                {
                    //获取刮奖日志数
                    int TotalLogCount = bll.GetWXLotteryLogCount(model.AutoID);
                    List<WXLotteryWinningData> WinningDataList = bll.GetWXLotteryWinningDataList(model.AutoID);
                    if (WinningDataList.SingleOrDefault(p => p.WinningIndex.Equals(TotalLogCount)) != null)//中奖了
                    {
                        Random rand = new Random();
                        int token = rand.Next(1111, 9999);
                        WXLotteryWinningData WinningData = WinningDataList.Single(p => p.WinningIndex.Equals(TotalLogCount));
                        //插入中奖记录

                        WXLotteryRecord Record = new WXLotteryRecord();
                        Record.InsertDate = DateTime.Now;
                        Record.LotteryId = model.AutoID;
                        Record.Token = token.ToString();
                        Record.WXAwardsId = WinningData.WXAwardsId;
                        Record.UserId = CurrentUserInfo.UserID;
                        Record.IsGetPrize = "0";
                        if (bll.AddWXLotteryRecord(Record))
                        {
                            sbLottery = string.Format("恭喜!您获得了<br/>{0}</br>领取码:{1}", WinningData.WXAwardName, token);
                            IsWin = "true";
                        }


                    }


                }
            }
            catch (Exception ex)
            {

                sbLottery = ex.ToString();
            }
        }

        /// <summary>
        /// 检查是否领过奖品
        /// </summary>
        /// <param name="UserId">当前用户的id</param>
        /// <param name="autoId">活动编号</param>
        /// <returns>返回是否领奖成功</returns>
        private string IsUserGetPrize(string UserId, int autoId)
        {
            WXLotteryRecord wxlRecord = bll.Get<WXLotteryRecord>(" UserId='" + UserId + "' And LotteryId=" + autoId);
            if (wxlRecord != null)
            {
                if (wxlRecord.IsGetPrize.Trim() == "1")
                {
                    IsGetPrize = "true";
                }
            }
            return IsGetPrize;
        }



        //Dictionary<string, string> WinList = new Dictionary<string , string>()
        //{

        //    {"oh2CguF1EJND2tTCjGlZ_rYE3UXA","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    {"oh2CguKwp0D0n_RlGnuJIaaLuKiY","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    {"oh2CguH9FlJ88rZvDtnu0GE-Jxow","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    {"oh2CguK0iEAPPs6IqBeegkRb6xyk","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    {"oh2CguAmMvdmVED1ete8RvMGUtJM","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    //{"oh2CguBo4HQ1-Cn5uJ9B9ozRBD0o","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    {"oh2CguMRMNmqF70UN9ohWoxRqBEs","三星GALAXY T320 8.4英寸四核16G平板电脑1部"},
        //    {"oh2CguBHdFKC3XAbLDNNrLgMyYOk","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguPqEI_qbMdoPPksfw-6hVkc","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguB5kHw0McEMB0_6Wm8RIqQs","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguJS76EMaEUA-UGORGeXazm0","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguH80WMYYHC1ahP0_FNCBT70","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguG9sBeARgcZsyjOjrrTLAes","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguF_8_JfBmG0lloXz5P341TI","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguAjJytDAhjwxRwlLmGq79lU","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguDWWq34a_z3DuKvfeV6UuhI","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguIMvVAsDJDINGMVuP0_G-9g","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguJ_PaarcpOWo_8i_90GGITg","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguOBE7-vJVO_uNb3cVTJkPWU","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguBHuJ-Lj5qFvPXYbByGLH14","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguH3vNOnP_Dpv93KlxjsKwvA","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguHyN3uGGy44e42V_2EY7nCw","高级碳素纤维高尔夫球杆1根"},
        //    {"oh2CguM5eSs5T12df6RhfuXt6LYs","高级碳素纤维高尔夫球杆1根"},
            
            
            
        //};

    }
}