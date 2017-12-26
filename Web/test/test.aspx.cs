using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;
using NetDimension.Json;
using StackExchange.Redis;
using System.Text;
using CommonPlatform.Helper;
using System.Drawing;
using System.Net;
using ZentCloud.BLLJIMP.Model.Weixin;
using TakeOutSDK.Eleme;
using TakeOutSDK.Eleme.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;
namespace ZentCloud.JubitIMP.Web.test
{
    public partial class test : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        BLLJIMP.BLLWebSite bllWebSite = new BLLJIMP.BLLWebSite();
        BLLJIMP.BLLComponent bllComponent = new BLLJIMP.BLLComponent();
        BLLJIMP.BLLDistributionOffLine bllDistributionOffLine = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        BLLWeixinOpen bllWeixinOpen = new BLLWeixinOpen();
        BLLWeixin bllWeiXin = new BLLWeixin();
        BLLUser bllUser = new BLLUser();
        BLLDistribution bllDis = new BLLDistribution();
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJuActivity();
        string websiteOwner = "";
        BLLRedis bllRedis = new BLLRedis();
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// yike
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
        BLLWeixinCard bllWeixinCard = new BLLWeixinCard();
        
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!bll.IsLogin)
            //{
            //    Response.Write("未登录");
            //}
            //else
            //{
            //    Response.Write(bll.GetCurrUserID());
            //}
            ////Response.End();
            Open.HongWareSDK.Client cl = new Open.HongWareSDK.Client("hf");
            //Open.HongWareSDK.Entity.YimaGetCard model=new Open.HongWareSDK.Entity.YimaGetCard();
            //model.activity_id = "17080454993";
            //model.makecard_transid = "5124201708041653";
            //model.transaction_id = DateTime.Now.ToString("yyyyMMddHHmmss");
            //model.start_number = "2";
            //model.count = "1";
            //string msg="";
            //string cardCode = "";
            //cl.YimaGetCard(model, out msg, out cardCode);
            //Open.HongWareSDK.Entity.YimaVerifyCard model=new Open.HongWareSDK.Entity.YimaVerifyCard();
            //model.goods_list = "";
            //model.pos_id = "";
            //model.pos_seq = DateTime.Now.ToString("yyyyMMddHHmmss");
            //model.store_id = "";
            //model.valid_info = "6609566330826508";
            //model.goods_list = "商品";
            //List<string> cardStr=new List<string>();
            //string msg = "";
            //cl.YimaCardVerify(model, out cardStr, out msg);

            Open.HongWareSDK.Entity.YimaQueryCard model = new Open.HongWareSDK.Entity.YimaQueryCard();
            model.goods_list = "";
            model.pos_seq=
            model.valid_info = "6609566330826508";
            List<string> cardCodeList = new List<string>();
            string msg = DateTime.Now.ToString("yyyyMMddHHmmss");
            cl.YimaMyCardList(model, out cardCodeList, out msg);
            //List<SkuModel> skuList = new List<SkuModel>() ;
            //SkuModel skuModel = new SkuModel();
            //skuModel.count = 1;
            //skuModel.price = 99;
            //skuModel.sku_sn = "sn487456";
            //skuList.Add(skuModel);
            //cl.MemberRight(skuList);

            //cl.CreateYimaCard();
            //string payUrl = "";
            //cl.OrderPay("1456890", 1, "baidu.com", out  payUrl);
           //var a= (int)(77.17 / (10 / 1));
            //yiKeClient.BonusUpdate("DS0045761", -7, "订单号2267743手动扣7积分");
            //yiKeClient.BonusUpdate("DS0045761", -37, "订单号2267773手动扣37积分");
            //yiKeClient.BonusUpdate("DS0045761", -14, "订单号2267776手动扣14积分");
           //Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson( ZentCloud.Common.AMapHelper.GetGeoByAddress(Request["address"])));
            
            //try
            //{

            //    string dir = "ScoreStatis";
            //    string fileType = "excel";
            //   var s= AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket(""), dir, AliOss.OssHelper.GetBucket(""), fileType, "study", "D:\\WebSite\\Sandbox\\FileUpload\\ScoreStatis\\0be96398-5dff-43f7-8fe9-f1fff80d7057_201706281533积分统计.xls");
            //   Response.Write(s);
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.ToString());
            //}
            


            //RedisHelper.RedisHelper.SetAdd("testsetIds",Guid.NewGuid().ToString());
            
            //var testsetIds = RedisHelper.RedisHelper.SetMembers("testsetIds");

            //foreach (var item in testsetIds)
            //{
            //    Response.Write(item.ToString() + "<br>");
            //}

            //RedisHelper.RedisHelper.KeyDelete("testsetIds");

            //Open.HongWareSDK.Client c = new Open.HongWareSDK.Client("yonder");
            //var x = c.GetMemberInfo(Guid.NewGuid().ToString());
            //Response.Write(JsonConvert.SerializeObject(x));
            //int activityCount = 0;
            //int activityData = 0;
            //foreach (var item in bll.GetList<MeifanActivity>(string.Format("")))
            //{
            //    JuActivityInfo model = new JuActivityInfo();
            //    model.JuActivityID = int.Parse(item.ActivityId);
            //    model.ActivityName = item.ActivityName;
            //    model.ThumbnailsPath = item.ActivityImg;
            //    model.Summary = item.Summary;
            //    model.ArticleType = item.ActivityType;
            //    model.CreateDate = item.InsertDate;
            //    model.BeginDate = item.BeginDate;
            //    model.EndDate = item.EndDate;
            //    model.IsFee = item.IsNeedPay;
            //    model.ActivityAddress = item.Address;
            //    model.ActivityDescription = item.Description;
            //    model.WebsiteOwner = item.Websiteowner;
            //    model.IsDelete = item.IsDelete;
            //    model.IsPublish = item.IsPublish;
            //    model.Remark = item.Remark;
            //    model.MainPoints = item.MainPoints;
            //    model.UserID = "meifan";
            //    if (bll.Add(model))
            //    {
            //        activityCount++;
            //    }



            //}
            //int uid=0;
            //foreach (var item in bll.GetList<MeifanActivityData>(""))
            //{
            //    ActivityDataInfo model = new ActivityDataInfo();
            //    model.ActivityID = item.ActivityId;
            //    model.UID = uid;
            //    model.Name = item.Name;
            //    model.Phone = item.Phone;
            //    model.InsertDate = item.InsertDate;
            //    model.WebsiteOwner = "meifan";
            //    model.UserId = item.UserId;
            //    model.ActivityType = item.ActivityType;
            //    model.OrderId = item.OrderId;
            //    model.ActivityName = item.ActivityName;
            //    model.PaymentStatus = item.IsPay;
            //    model.Sex = item.Sex;
            //    model.Email = item.Email;
            //    model.BirthDay = item.BirthDay;
            //    model.DateRange = item.DateRange;
            //    model.GroupType = item.GroupType;
            //    model.IsMember = item.IsMember;
            //    if (!string.IsNullOrEmpty(item.Amount))
            //    {
            //         model.Amount = decimal.Parse(item.Amount);
            //    }
            //    model.Remarks = item.Remark;
            //    model.InsertDate = item.InsertDate;
            //    model.UserRemark = item.UserRemark;
            //    model.ActivityType = item.ActivityType;
            //    model.WebsiteOwner = "meifan";
            //    if (bll.Add(model))
            //    {
            //        activityData++;
            //        uid++;
            //    }

            //}

            //Response.Write(activityCount);
            //Response.Write("<br/>");
            //Response.Write(activityData);

            //'2386209',
            //'2386221',
            //'2386221',
            //'2386231',
            //'2386231',
            //'2386386',
            //'2386386',
            //'2386386',
            //'2386783',
            //'2386783',
            //'2386783',
            //'2386783',
            //'2386821',
            //'2386821',
            //'2386912',
            //'2386912',
            //'2386912',
            //'2386783',
            //'2387073',
            //'2387073',
            //'2387073',
            //'2387073',
            //'2387073',
            //'2387205',
            //'2387205',
            //'2386783',
            //'2387205',
            //'2386821',
            //'2387292',
            //'2387292',
            //'2387292',
            //'2386912',
            //'2387292',
            //'2387320',
            //'2387320',
            //'2387320',
            //'2387320',
            //'2387320',
            //'2386783',
            //'2387386',
            //'2386821',
            //'2387386',
            //'2387386',
            //'2387386',
            //'2387386',
            //'2386912',
            //'2387292',
            //'2386783',
            //'2386821',
            //'2387386',
            //'2386912',
            //'2387292',
            //'2387292',
            //'2386783',
            //'2386821',
            //'2386912',
            //'2387292',
            //'2388184',
            //'2388184',
            //'2388184',
            //'2388184',
            //'2388184',
            //'2387292',
            //'2388184',
            //'2388184',
            //'2388184',
            //'2388184',
            //'2388184',
            //'2388570',
            //'2388570',
            //'2388570',
            //'2388570',
            //'2388570',
            //'2388570',
            //'2388570',
            //'2388726',
            //'2388726',
            //'2388726',
            //'2388726',
            //'2388726',
            //'2388570',
            //'2388726',
            //'2388570',
            //'2388726',
            //'2388872',
            //'2388872',
            //'2388872',
            //'2388872',
            //'2388872',
            //'2388726',
            //'2388872',
            //'2389070',
            //'2389070',
            //'2389070',
            //'2389070',
            //'2389070',
            //'2388570',
            //'2389127',
            //'2389127',
            //'2389127',
            //'2389127',
            //'2389150',
            //'2389150',
            //'2389157',
            //'2389150',
            //'2389157',
            //'2389157',
            //'2389157',
            //'2389127',
            //'2389219',
            //'2389219',
            //'2389157',
            //'2389219',
            //'2389219',
            //'2389219',
            //'2388726',
            //'2389282',
            //'2389282',
            //'2389282',
            //'2389282',
            //'2388872',
            //'2389305',
            //'2389305',
            //'2389305',
            //'2389305',
            //'2389282',
            //'2389305',
            //'2389184',
            //'2389184',
            //'2389184',
            //'2389184',
            //'2389070',
            //'2389184',
            //'2389375',
            //'2389375',
            //'2389328',
            //'2389375',
            //'2389328',
            //'2389328',
            //'2389375',
            //'2389328',
            //'2389375',
            //'2389328',
            //'2389446',
            //'2389446',
            //'2389446',
            //'2389446'



































            //new BLLMQ().Connect();

            //  int successCount = 0;
            //  int failCount = 0;
            //  string msg = "";
            //new BLLMall().BatchDeliver(DataLoadTool.NPOIHelper.Import("D:\\template .xls"),out successCount,out failCount,out msg);

            //var a = 0;

            //Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client("hf");


            //var hongWareMemberInfo = hongWareClient.GetMemberInfo("oXV9ruBQFoloDFRwrF98WqxaMxSg");
            //if (hongWareMemberInfo.member != null)
            //{
            //    if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, "oXV9ruBQFoloDFRwrF98WqxaMxSg", 1))
            //    {


            //    }


            //}
            //foreach (var item in bll.GetList<LiveChatRoom>(""))
            //{
            //    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(item.RoomId));
            //    item.UserShowName = bllUser.GetUserDispalyName(userInfo);
            //    item.UserHeadImg = bllUser.GetUserDispalyAvatar(userInfo);
            //    bllUser.Update(item);


            //}
            //foreach (var item in bll.GetList<LiveChatDetail>(" UserType=0"))
            //{
            //    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(item.RoomId));
            //    item.UserHeadImg = bllUser.GetUserDispalyAvatar(userInfo);
            //    bllUser.Update(item, string.Format("UserHeadImg='{0}'", item.UserHeadImg), string.Format("UserAutoId={0}", item.UserAutoId));


            //}
            //BLLSMS bllsms = new BLLSMS("");
            ////bllsms.SendSms("hf", "18521562432", "手机验证码：293935");
            //int deposit=0;
            //string msg = "";
            //bllsms.GetSmsDeposit("hftest", out deposit, out msg);
            //var store = bllWeixinCard.GetStoreById("468741430"); 
            //var storeList = bllWeixinCard.GetStoreList();
            //BllKuaidi100 bllK = new BllKuaidi100();
            //bllK.Query("yunda", "12025267406770");

            //WeixinStore model = new WeixinStore();
            //model.telephone = "021-6026989";
            //model.poi_id = "";
            //string msg = "468741430";
            //bllWeixinCard.UpdateStore(model, out msg);
            //WXMallOrderInfo orderInfo = new WXMallOrderInfo();
            //orderInfo.WebsiteOwner = "songhe";
            //orderInfo.OrderUserID = "WXUserfffac211-8053-4538-8442-9a7a4deedc40";
            //orderInfo.TotalAmount = 150;
            //bllDis.AutoUpdateLevel(orderInfo);
            //BLLSMS bllSms = new BLLSMS("");
            //int des = 0;
            //string msg = "";
            //var o = bllSms.GetSmsDeposit("songhe", out des, out msg);
            // string msg1 = "";
            //var point = bllSms.SmsRecharge("hf", "1", 1,out msg1);
            // var b = point;
            //BllPay bllPay = new BllPay();
            //bllPay.GetAliPayRequestMobile("4598", 1, "pushunongye@163.com", "2088611124608113", "4zrb15ss7eapgmc4t0kip5w1e25zlc2c", "http://comeoncloud.comeoncloud.net/notify.aspx");
            //DateTime dtLastMonthFirstDay = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1);
            //dtLastMonthFirstDay = Convert.ToDateTime(dtLastMonthFirstDay.ToString("yyyy-MM-dd"));
            ////string fromDate = dtLastMonthStart.ToString();//上个月第一天

            //DateTime dtLastMonthLastDay = DateTime.Now.AddDays(1 - DateTime.Now.Day);
            //dtLastMonthLastDay = Convert.ToDateTime(dtLastMonthLastDay.ToString("yyyy-MM-dd")).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            ////
            //int s = 2;
            //BLLMall bllMall = new BLLMall();
            //string msg;
            //bllMall.SupplierSettlement("hf", "2016-02-01", "2017-02-28", out msg);
            //string cardId = "";
            //bllWeixinCard.Create("至云之家", out cardId);
            //bllWeixinCard.Consume("606659401644");
            //bllWeixinCard.SendByMass("p99IZtxeq7bi7Df3jG17JPmEOBAA", "o99IZtxVsEpSanbBmiW_1_IVL4-0");
            //var r= bllWeixinCard.CreateQrCode("p99IZtxeq7bi7Df3jG17JPmEOBAA");
            //bll.StatisticsProduct(bll.Get<TimingTask>(string.Format("autoId=379")));
            //var filePath = bllDis.CreateUserDistributionImage("oWFcfxBoQJ2n1fDtHbS00mMTMCgI", "songhe");
            //string mediaIdUpload = bllWeixin.UploadFileToWeixinModel(bllWeixin.GetAccessToken("songhe"), "image", filePath).media_id;
            //if (!string.IsNullOrEmpty(mediaIdUpload))
            //{
            //    bllWeixin.SendKeFuMessageImage(bllWeixin.GetAccessToken("songhe"), "oWFcfxBoQJ2n1fDtHbS00mMTMCgI", mediaIdUpload);
            //}

            ////string a = DateTime.Now.ToString("yyyyMMdd");

            //string msg = "";
            //bllPay.AlipayRefund("2017020721001004830269248522", "20170210" + ((int)(bll.GetTimeStamp(DateTime.Now) / 1000)).ToString() + ((int)(bll.GetTimeStamp(DateTime.Now) / 1000)).ToString() + ((int)(bll.GetTimeStamp(DateTime.Now) / 1000)).ToString(), (decimal)0.01, "http://songhedev.comeoncloud.com.cn/Alipay/NotifyRefund.aspx", out msg);
            //Response.Write(msg);
            //return;
            //int count=0;
            //int failCount = 0;
            //foreach (var orderInfo in bll.GetList<WXMallOrderInfo>(string.Format(" PaymentStatus=1 And InsertDate>='2016-12-29 23:30' And InsertDate<='2016-12-30 10:30'")))
            //{
            //    try
            //    {
            //        bllDis.TransfersEstimate(orderInfo);
            //        bllDis.SendMessageToPreUser(orderInfo);
            //        count++;
            //    }
            //    catch (Exception)
            //    {
            //        failCount++;
            //        continue;
            //    }

            //}
            //Response.Write("scuccessCount"+count);
            //Response.Write("failCount"+failCount);
            //Response.End();

            //BLLMall bllMall=new BLLMall();
            //bllDis.GetUserCommission(bllMall.GetOrderInfo("140268"), bll.GetCurrentUserInfo(), 1);
            //string filePath = "/FileUpload/QRCodePage/" + Guid.NewGuid() + ".jpg";
            //filePath = filePath.Replace("/", "\\");
            // string fileServerPath = string.Format("D:\\WebSite\\CommonPlatform{0}", filePath);//本地磁盘路径
            // string fileServerPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            //WebsiteToImageHelper websiteToImage = new WebsiteToImageHelper("http://hudieyugou.comeoncloud.net/App/Cation/Wap/Mall/Distribution/QCodePage.aspx?autoid=1929793");
            //Bitmap bMap = websiteToImage.Generate();

            //bMap.Save("D:\\a.jpg");


            //int successCount = 0;
            //bool needUpdate;
            //foreach (var item in bll.GetList<VoteObjectInfo>(string.Format("VoteId=149")))
            //{
            //     needUpdate = false;
            //    if (item.VoteObjectHeadImage.StartsWith("/FileUpload"))
            //    {
            //        item.VoteObjectHeadImage = AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket("dali"), AliOss.OssHelper.GetBaseDir("dali"), "dali", "image", item.VoteObjectHeadImage);
            //        needUpdate = true;
            //    }
            //    if (item.Ex1.StartsWith("/FileUpload"))
            //    {
            //        item.Ex1 = AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket("dali"), AliOss.OssHelper.GetBaseDir("dali"), "dali", "image", item.Ex1);
            //        needUpdate = true;
            //    }
            //    if (item.ShowImage1.StartsWith("/FileUpload"))
            //    {
            //        item.ShowImage1 = AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket("dali"), AliOss.OssHelper.GetBaseDir("dali"), "dali", "image", item.ShowImage1);
            //        needUpdate = true;
            //    }
            //    if (item.ShowImage2.StartsWith("/FileUpload"))
            //    {
            //        item.ShowImage2 = AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket("dali"), AliOss.OssHelper.GetBaseDir("dali"), "dali", "image", item.ShowImage2);
            //        needUpdate = true;
            //    }
            //    if (item.ShowImage3.StartsWith("/FileUpload"))
            //    {
            //        item.ShowImage3 = AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket("dali"), AliOss.OssHelper.GetBaseDir("dali"), "dali", "image", item.ShowImage3);
            //        needUpdate = true;
            //    }
            //    if (item.ShowImage4.StartsWith("/FileUpload"))
            //    {
            //        item.ShowImage4 = AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket("dali"), AliOss.OssHelper.GetBaseDir("dali"), "dali", "image", item.ShowImage4);
            //        needUpdate = true;
            //    }
            //    if (needUpdate)
            //    {
            //        if (bll.Update(item))
            //        {
            //            successCount++;
            //        }
            //    }



            //}
            //Response.Write(successCount);
            //Response.End();


            //Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus("", "DS0004481", "13702694131");
            //Response.Write(Request.UserAgent);
            //string url = "http://comeoncloud.comeoncloud.net/App/Cation/Wap/Mall/Distribution/QCodePage.aspx?autoid=7344";


            //string fileServerPath = string.Format("D:\\a.jpg");//本地磁盘路径
            //// string fileServerPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            //WebsiteToImageHelper websiteToImage = new WebsiteToImageHelper(url);
            //Bitmap bMap = websiteToImage.Generate();
            //bMap.Save(fileServerPath);







            //var sign= ZentCloud.Common.SHA1.SHA1_Encrypt("appid=jikuwifi&appkey=82f03c2b26180b845369190db2188b0cb00ae43cbf&timestamp=1477906856000");

            //int successCount = 0;
            //var allChannelList = bllUser.GetList<UserInfo>(string.Format(" PermissionGroupID=4 And WebsiteOwner in('study')"));
            //foreach (var channelUserInfo in allChannelList)
            //{
            //    channelUserInfo.DistributionDownUserCountLevel1 = bllDis.GetChannelAllFirstLevelChildUser(channelUserInfo.UserID, "study").Count;//直接会员数量
            //    channelUserInfo.DistributionDownUserCountAll = bllDis.GetChannelAllChildUser(channelUserInfo.UserID).Count();//所有会员数量
            //    channelUserInfo.DistributionSaleAmountLevel1 = bllDis.GetChannelAllFirstLevelOrder(channelUserInfo.UserID, "study").Sum(s => s.TotalAmount);
            //    channelUserInfo.DistributionSaleAmountAll = bllDis.GetChannelAllOrder(channelUserInfo.UserID).Sum(s => s.TotalAmount);
            //    if (bll.Update(channelUserInfo))
            //    {
            //        successCount++;
            //    }

            //}
            //Response.Write(successCount);
            //return;

            // bllDis.UpdateUpUserCount(bllUser.GetUserInfo("WXUser201510141814488ZI89"));
            //string msg="";
            //bllDis.TransfersEstimate(bll.Get<WXMallOrderInfo>(string.Format("OrderId='194696'")));
            // bllDis.UpdateDistributionSaleAmountUp(bll.Get<WXMallOrderInfo>(string.Format("OrderId='194696'")));
            //string msg = "";
            //bllDis.Transfers(bll.Get<WXMallOrderInfo>(string.Format("OrderId='194696'")),out msg);
            //bllDis.UpdateDistributionSaleAmountUp(bll.Get<WXMallOrderInfo>(string.Format("OrderId='194696'")));
            //int successCount = 0;
            //foreach (var model in bll.GetList<WXMallStatistics>(string.Format("WebsiteOwner='hf' ")))
            //{

            //    #region 成交笔数
            //    string sqlOrderCount = string.Format("Select count(*) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1  And OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') ", model.WebsiteOwner, model.Date, Convert.ToDateTime(model.Date).AddDays(1).AddMilliseconds(-1).ToString());
            //    var orderCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderCount);
            //    if (orderCount != null)
            //    {
            //        model.OrderCount = int.Parse(orderCount.ToString());

            //    }
            //    #endregion
            //    #region 成交件数
            //    string sqlOrderProuductTotalCount = string.Format(" Select Sum(TotalCount) from ZCJ_WXMallOrderDetailsInfo where OrderID in(Select OrderID from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}'))", model.WebsiteOwner, model.Date, Convert.ToDateTime(model.Date).AddDays(1).AddMilliseconds(-1).ToString());
            //    var orderProuductTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderProuductTotalCount);
            //    if (orderProuductTotalCount != null)
            //    {
            //        model.OrderProuductTotalCount = int.Parse(orderProuductTotalCount.ToString());

            //    }
            //    #endregion

            //    #region 成交金额
            //    string sqlOrderTotalAmount = string.Format("Select sum(TotalAmount) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') ", model.WebsiteOwner, model.Date, Convert.ToDateTime(model.Date).AddDays(1).AddMilliseconds(-1).ToString());
            //    var orderTotalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderTotalAmount);
            //    if (orderTotalAmount != null)
            //    {
            //        model.OrderTotalAmount = decimal.Parse(orderTotalAmount.ToString());

            //    }
            //    #endregion

            //    if (bll.Update(model, string.Format("OrderCount={0},OrderProuductTotalCount={1},OrderTotalAmount={2}", model.OrderCount, model.OrderProuductTotalCount, model.OrderTotalAmount), string.Format("AutoId={0}", model.AutoId)) > 0)
            //    {
            //        successCount++;
            //    }


            //}
            //Response.Write(successCount);
            //return;








            //BllKuaidi100 bllKuaidi100 = new BllKuaidi100();
            //foreach (var orderInfo in bll.GetList<WXMallOrderInfo>(string.Format("WebsiteOwner='lanyueliang'")))
            //{
            //    string msg;
            //   bllKuaidi100.Poll(orderInfo.ExpressCompanyCode, orderInfo.ExpressNumber, out  msg);
            //   Response.Write(msg+orderInfo.OrderID);

            //}

            //return;
            //Response.Write( bllWeixin.GetAccessToken(bll.WebsiteOwner));

            //int successCount = 0;
            //foreach (var item in bll.GetList<WXMallOrderInfo>(string.Format("WebsiteOwner='lanyueliang' And PayMentStatus=1 And Status!='已取消'")))
            //{
            //    try
            //    {
            //        bllDis.TransfersEstimate(item);
            //        successCount++;
            //    }
            //    catch (Exception)
            //    {
            //        continue;

            //    }

            //}
            //Response.Write(successCount);
            //return;

            //WebsiteInfo w = bll.Get<WebsiteInfo>("websiteowner='hf'");
            //DateTime dt = DateTime.Now;
            //var m = bllRedis.GetModuleFilterInfoList();
            //TimeSpan ts = DateTime.Now - dt;



            //Response.Write(ts.TotalMilliseconds);
            //return;
            // Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();

            //bll.Update(new WebsiteInfo(),"hexiaoCode='syscode'"," websiteowner='hf'");
            //var successCount = 0;
            //var allWebsite = RedisHelper.RedisHelper.HashGetAll("WebsiteInfo");
            //foreach (var item in allWebsite)
            //{
            //    var website = RedisHelper.RedisHelper.HashGet<WebsiteInfo>(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, item.Name);
            //    Response.Write(website.WebsiteOwner+" "+website.MallStatisticsLimitDate.ToString());
            //    Response.Write("<br/>");
            //    //if (RedisHelper.RedisHelper.HashDelete(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, item.Name))
            //    //{
            //    //    successCount++;
            //    //}


            //}
            //try

            //{
            //    var website = RedisHelper.RedisHelper.HashGet<WebsiteDomainInfo>(RedisHelper.Enums.RedisKeyEnum.WebsiteDomainInfo, "comeoncloud.comeoncloud.net");
            //}
            //catch (Exception ex)
            //{

            //    Response.Write(ex.ToString());
            //}


            //foreach (var item in RedisHelper.RedisHelper.HashGetAll("WebsiteDomainInfo"))
            //{


            //    Response.Write(RedisHelper.RedisHelper.HashGet<WebsiteDomainInfo>(RedisHelper.Enums.RedisKeyEnum.WebsiteDomainInfo, item.Name).WebsiteDomain);
            //    Response.Write("<br/>");

            //}


            //Response.Write(successCount);
            //Response.End();



            //WebsiteInfo websiteInfo;

            //DateTime dtBegin = DateTime.Now;
            //for (int i = 1; i <= 1500; i++)
            //{
            //    websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            //}
            //websiteInfo = bll.GetWebsiteInfoModelFromDataBase("hf");;

            //TimeSpan ts = DateTime.Now - dtBegin;

            //Response.Write(ts.TotalMilliseconds);
            //return;
            //var mi = ts.TotalSeconds;
            //var keyEx = RedisHelper.RedisHelper.HashExists("websiteInfo","hf");
            //var keyDel = RedisHelper.RedisHelper.HashDelete("websiteInfo", "hf");

            //List<JuActivityInfo> websiteInfoRedi0s = RedisHelper.RedisHelper.HashGet<List<JuActivityInfo>>(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, "0");
            //var w = bll.GetWebsiteInfoModelFromDataBase();
            //var setresult = RedisHelper.RedisHelper.HashSet(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, "hf", w);
            //WebsiteInfo websiteInfoRedis = RedisHelper.RedisHelper.HashGet<WebsiteInfo>(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, "hf");

            //Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus("DS0005475", "", "18071719091");

            //var result=  yiKeClient.BonusUpdate("DS0005475", 42, string.Format("订单取消返还{0}积分", 42));
            // var a = result;
            //int successCount = 0;
            //int updateCount = 0;
            //string ex = "";
            //int index = 0;
            //foreach (var item in bllWeiXin.GetList<WebsiteInfo>(" WebsiteOwner!='wjgj'"))
            //{
            //    index++;
            //    try
            //    {

            //        try
            //        {
            //            UserInfo websiteOwnerInfo = bllUser.GetUserInfo(item.WebsiteOwner, item.WebsiteOwner);
            //            if (websiteOwnerInfo == null)
            //            {
            //                continue;

            //            }
            //        }
            //        catch (Exception)
            //        {
            //            continue;

            //        }
            //        //if (!string.IsNullOrEmpty(websiteOwnerInfo.SubscribeKeyWord))//原有
            //        //{

            //        //    if (bll.Update(new WeixinReplyRuleInfo(), "MsgKeyWord='关注自动回复'", string.Format(" UserId='{0}' And MsgKeyWord='{1}'", item.WebsiteOwner, websiteOwnerInfo.UserID)) > 0)
            //        //    {
            //        //        updateCount++;
            //        //    }


            //        //}
            //        //else if (bll.GetCount<WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' And RuleType = 4 ", item.WebsiteOwner)) > 0)//原有
            //        //{
            //        //    if (bll.Update(new WeixinReplyRuleInfo(), "MsgKeyWord='消息自动回复'", string.Format(" UserId='{0}' And RuleType = 4", item.WebsiteOwner)) > 0)
            //        //    {
            //        //        updateCount++;
            //        //    }
            //        //}

            //        if (bll.GetCount<WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' And MsgKeyword ='关注自动回复'", item.WebsiteOwner)) == 0)
            //        {
            //            #region 关注自动回复
            //            var replyModel1 = new WeixinReplyRuleInfo();
            //            replyModel1.MsgKeyword = "关注自动回复";
            //            replyModel1.MatchType = "全文匹配";
            //            replyModel1.ReplyContent = "欢迎关注";
            //            replyModel1.ReceiveType = "text";
            //            replyModel1.ReplyType = "text";
            //            replyModel1.CreateDate = DateTime.Now;
            //            replyModel1.RuleType = 1;
            //            replyModel1.UID = bll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
            //            replyModel1.UserID = item.WebsiteOwner;
            //            if (bll.Add(replyModel1))
            //            {
            //                successCount++;
            //            }
            //            #endregion
            //        }


            //        if (bll.GetCount<WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' And MsgKeyword ='消息自动回复'", item.WebsiteOwner)) == 0)
            //        {
            //            #region 消息自动回复
            //            var replyModel2 = new WeixinReplyRuleInfo();
            //            replyModel2.MsgKeyword = "消息自动回复";
            //            replyModel2.MatchType = "全文匹配";
            //            replyModel2.ReplyContent = "";
            //            replyModel2.ReceiveType = "text";
            //            replyModel2.ReplyType = "text";
            //            replyModel2.CreateDate = DateTime.Now;
            //            replyModel2.RuleType = 1;
            //            replyModel2.UID = bll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
            //            replyModel2.UserID = item.WebsiteOwner;
            //            if (bll.Add(replyModel2))
            //            {
            //                successCount++;
            //            }

            //            #endregion
            //        }
            //    }
            //    catch (Exception exm)
            //    {
            //        ex += item.WebsiteOwner + " " + exm.ToString();
            //        continue;
            //    }







            //}
            //Response.Write("AddCount" + successCount);
            //Response.Write("UpdateCount" + updateCount);
            //Response.Write("ex"+ex);
            //Response.Write("index"+index);

            //Response.End();




            //int successCount = 0;
            //DateTime dateTimeYesday = DateTime.Parse("2016-06-01");
            //DateTime dateTimeEnd = DateTime.Parse("2016-09-30");
            //for (int i = 1; i <= (dateTimeEnd-dateTimeYesday).Days; i++)
            //{

            //    DateTime dateTimeToday = dateTimeYesday.AddDays(1).AddMilliseconds(-1);
            //    foreach (var website in bll.GetList<WebsiteInfo>(string.Format("WebsiteOwner='comeoncloud'")))
            //    {
            //        try
            //        {

            //            if (bll.GetCount<WXMallStatistics>(string.Format(" WebsiteOwner='{0}' And Date='{1}'", website.WebsiteOwner, dateTimeYesday.ToString("yyyy/MM/dd"))) == 0)
            //            {
            //                WXMallStatistics model = new WXMallStatistics();
            //                model.Date = dateTimeYesday.ToString("yyyy/MM/dd");//统计昨天

            //                #region 成交笔数
            //                string sqlOrderCount = string.Format("Select count(*) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //                var orderCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderCount);
            //                if (orderCount != null)
            //                {
            //                    model.OrderCount = int.Parse(orderCount.ToString());

            //                }
            //                #endregion

            //                #region 成交件数
            //                string sqlOrderProuductTotalCount = string.Format(" Select Sum(TotalCount) from ZCJ_WXMallOrderDetailsInfo where OrderID in(Select OrderID from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}'))", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //                var orderProuductTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderProuductTotalCount);
            //                if (orderProuductTotalCount != null)
            //                {
            //                    model.OrderProuductTotalCount = int.Parse(orderProuductTotalCount.ToString());

            //                }
            //                #endregion

            //                #region 成交金额
            //                string sqlOrderTotalAmount = string.Format("Select sum(TotalAmount) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //                var orderTotalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderTotalAmount);
            //                if (orderTotalAmount != null)
            //                {
            //                    model.OrderTotalAmount = decimal.Parse(orderTotalAmount.ToString());

            //                }
            //                #endregion

            //                #region 当日退货件数
            //                string sqlRefundProductTotalCount = string.Format("    select SUM(TotalCount) from ZCJ_WXMallOrderDetailsInfo  where AutoID in( select OrderDetailId from ZCJ_WXMallRefund where WebsiteOwner='{0}'  And ( InsertDate Between '{1}' And '{2}'))", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //                var refundProductTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlRefundProductTotalCount);
            //                if (refundProductTotalCount != null)
            //                {
            //                    model.RefundProductTotalCount = int.Parse(refundProductTotalCount.ToString());

            //                }
            //                #endregion

            //                #region 当日退货金额
            //                string sqlRefundTotalAmount = string.Format("Select sum(RefundAmount) from ZCJ_WXMallRefund where WebsiteOwner='{0}'  And ( InsertDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //                var refundTotalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlRefundTotalAmount);
            //    //                if (refundTotalAmount != null)
            //    //                {
            //    //                    model.RefundTotalAmount = decimal.Parse(refundTotalAmount.ToString());

            //    //                }
            //    //                #endregion

            //    //                #region Pv

            //    //                string sqlPv = string.Format("  Select count(*) from ZCJ_MonitorEventDetailsInfo where ModuleType in('product') And WebsiteOwner='{0}'  and ( EventDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //    //                var pv = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlPv);
            //    //                if (pv != null)
            //    //                {
            //    //                    model.PV = int.Parse(pv.ToString());

            //    //                }
            //    //                #endregion

            //    //                #region UV
            //    //                string sqlUV = string.Format("  Select count(distinct(EventUserID)) from ZCJ_MonitorEventDetailsInfo where ModuleType in('product') And WebsiteOwner='{0}'  and ( EventDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //    //                var uv = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlUV);
            //    //                if (uv != null)
            //    //                {
            //    //                    model.UV = int.Parse(uv.ToString());

            //    //                }
            //    //                #endregion

            //    //                #region 在线商品数
            //    //                string sqlProductTotalCount = string.Format("Select count(*) from ZCJ_WXMallProductInfo Where WebsiteOwner='{0}' And IsOnSale=1", website.WebsiteOwner);
            //    //                var productTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlProductTotalCount);
            //    //                if (productTotalCount != null)
            //    //                {
            //    //                    model.ProductTotalCount = int.Parse(productTotalCount.ToString());

            //    //                }
            //    //                #endregion


            //    //                #region 转化率
            //    //                model.ConvertRate = "0%";
            //    //                if (model.UV > 0)
            //    //                {
            //    //                    var dou = (((double)model.OrderCount) / ((double)model.UV)) * 100;
            //    //                    model.ConvertRate = string.Format("{0}%", Math.Round(dou, 2));
            //    //                }
            //    //                #endregion

            //    //                #region 客单价
            //    //                model.PerCustomerTransaction = 0;
            //    //                if (model.OrderCount > 0)
            //    //                {
            //    //                    model.PerCustomerTransaction = model.OrderTotalAmount / model.OrderCount;
            //    //                }
            //    //                #endregion

            //    //                #region 商品平均单价
            //    //                string sqlProcuctAveragePrice = string.Format("SELECT AVG(Price) from ZCJ_ProductSku where WebsiteOwner='{0}'", website.WebsiteOwner);
            //    //                var procuctAveragePrice = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlProcuctAveragePrice);
            //    //                if (procuctAveragePrice != null)
            //    //                {
            //    //                    model.ProcuctAveragePrice = decimal.Parse(procuctAveragePrice.ToString());

            //    //                }
            //    //                #endregion

            //    //                #region 月累积销售额
            //    //                string sqlOrderTotalAmountMonth = string.Format("Select sum(TotalAmount) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And OrderType In(0,1,2) And ( month(InsertDate) = month('{1}') And InsertDate<='{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
            //    //                var orderTotalAmountMonth = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderTotalAmountMonth);
            //    //                if (orderTotalAmountMonth != null)
            //    //                {
            //    //                    model.OrderTotalAmountMonth = decimal.Parse(orderTotalAmountMonth.ToString());

            //    //                }
            //    //                #endregion

            //    //                model.WebsiteOwner = website.WebsiteOwner;
            //    //                model.InsertDate = DateTime.Now;
            //    //                if (bll.Add(model))
            //    //                {
            //    //                    successCount++;
            //    //                }

            //    //            }


            //    //        }
            //    //        catch (Exception ex)
            //    //        {
            //    //            using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("gb2312")))
            //    //            {
            //    //                sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), ex.ToString() + website.WebsiteOwner));
            //    //            }
            //    //            continue;

            //    //        }

            //    //    }
            //    //    dateTimeYesday = dateTimeYesday.AddDays(1);

            //    //}

            //    //Response.Write(successCount);
            //    //return;
            //    // var a= (decimal)(Convert.ToDouble((98.12568).ToString("0.00")));



            //    //int icount = 0;
            //    //foreach (var item in bll.GetList<JuActivityInfo>(string.Format(" RelationArticles !=''")))
            //    //{

            //    //    item.RelationArticles = item.RelationArticles.Replace("'",null);
            //    //    if (bll.Update(item, string.Format("RelationArticles='{0}'", item.RelationArticles),string.Format(" JuActivityId={0}",item.JuActivityID))>0)
            //    //    {
            //    //        icount++;
            //    //    }

            //    //}

            //    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //    //foreach (var item in bll.GetList<JuActivityInfo>(string.Format(" RelationArticles !=''")))
            //    //{

            //    //    sb.AppendFormat("update ZCJ_JuActivityInfo set RelationArticles='{0}' where JuActivityID={1} ;", item.RelationArticles, item.JuActivityID);
            //    //    sb.AppendLine("");
            //    //}
            //    //using (StreamWriter sw = new StreamWriter(@"D:\sql.txt", true, Encoding.GetEncoding("gb2312")))
            //    //{
            //    //    sw.WriteLine(string.Format("{0}",sb.ToString()));
            //    //}
            //    // Response.Write(icount);
            //    // return;

            //    //var x = RedisHelper.RedisHelper.Get<List<ModuleFilterInfo>>("WXModuleFilter");
            //    //if(x == null)
            //    //{

            //    //}


            //    //var x = RedisHelper.RedisHelper.Get<List<ModuleFilterInfo>>("WXModuleFilter");
            //    //if(x == null)
            //    //{

            //    //}
            //    // var result= bllWeiXin.UploadImageToWeixin(bllWeiXin.GetAccessToken(),"image","D:\\img.png");
            //    //ConfigurationOptions config = ConfigurationOptions.Parse("127.0.0.1:6379,allowAdmin=true");
            //    //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);
            //    //IDatabase db = redis.GetDatabase();
            //    //db.HashSet("hashsetkey", "key", 0);
            //    //db.HashSet("hashsetkey", "tempkey", "tempkeyvalue");
            //    //var rank = db.KeyRandom();
            //    //var dd = db.HashScan("hashsetkey");
            //    //foreach (var item in dd)
            //    //{

            //    //}
            //    //var zz = db.HashGetAll("hashsetkey");
            //    //var r1 = db.HashDelete("hashsetkey", "tempkey");
            //    //var r2 = db.HashExists("hashsetkey", "key");
            //    //string r3 = db.HashGet("hashsetkey", "key").ToString();
            //    //var r7 = db.HashGet("hashsetkey", new RedisValue[] { "key", "tempkey" });
            //    //var r4 = db.HashDecrement("hashsetkey", "key", 1);
            //    //var r5 = db.HashIncrement("hashsetkey", "key", 1);
            //    //var r6 = db.HashKeys("hashsetkey");
            //    //var hsetsrsult = db.HashValues("hashsetkey");
            //    //db.HashScan("hashsetkey", "te*");
            //    //db.HashSet("hashsetkey", new HashEntry[] { new HashEntry("key", 0) });
            //    ////#endregion


            //    //#region List操作 List 就是String数组
            //    //db.ListRightPush("listkey", "listvalue");
            //    //db.ListLeftPush("listkey", "listvalue");
            //    //db.ListLeftPush("listkey", "mylistvalue");
            //    //var l1 = db.ListGetByIndex("listkey", 0);
            //    //var l2 = db.ListRange("listkey").Where(p => p.ToString().Contains("my"));
            //    ////db.ListTrim("listkey", 0, 0);
            //    ////var l3=db.LockQuery("listkey");
            //    //var l4 = db.ListRange("listkey");
            //    //var l5 = db.ListRemove("listkey", "0");
            //    //db.ListSetByIndex("listkey", 0, "listvalue0");

            //    //#endregion

            //    //#region 集合操作 String数组 集合中不能出现重复的数据
            //    //db.SetAdd("setkey", "setvalue");
            //    //db.SetAdd("setkey", "setvalue");
            //    //var s1 = db.SetAdd("setkey", new RedisValue[] { "a", 0 });
            //    //var s2 = db.SetCombine(SetOperation.Intersect, new RedisKey[] { "a", "b" });
            //    //var s3 = db.SetMembers("setkey");
            //    //db.SetCombineAndStore(SetOperation.Intersect, "setkeynew", new RedisKey[] { "a", "b" });
            //    //db.SetContains("setkey", "setvalue");
            //    //db.SetMove("setkey", "setkeynew", "0");
            //    //db.SetRandomMember("setkey");
            //    //db.SetRemove("setkey", 0);

            //    //#endregion

            //    //#region 有序集合操作 String数组 集合中不能出现重复的数据且有序
            //    //db.SortedSetAdd("sortsetkey", "sortsetvalue", 0);
            //    ////var z1=db.SortS
            //    //#endregion


            //    //System.Net.EndPoint[] endpoints = redis.GetEndPoints();
            //    //redis.PreserveAsyncOrder = false;
            //    //db.Publish("chanel", "msg");
            //    //ISubscriber sub = redis.GetSubscriber();
            //    //sub.Subscribe("messages", (channel, message) =>
            //    //{
            //    //    Console.WriteLine((string)message);
            //    //});
            //    //string myname = db.StringGet("myname");

            //    //db.StringSet("myname", "黄忠鸿");
            //    //var ss = db.StringGetAsync("myname");
            //    //var de = db.DebugObject("myname");

            //    ////db.StringSet("activity",ZentCloud.Common.JSONHelper.ObjectToJson(bllJuActivity.GetLit<JuActivityInfo>(100,1,"")));

            //    ////list

            //    //#region Hash操作
            //    //HashEntry[] hashsetEf = 
            //    //{ 
            //    //new HashEntry("key", "value")

            //    //};

            //    //HashEntry c = new HashEntry("a", "b");


            //    // db.HashSet("hashsetkey", hashsetEf);




            //    //list

            //    //DateTime d1 = DateTime.Now;
            //    //var ccccz = db.StringGet("activity");

            //    //var tims = DateTime.Now - d1;




            //    //RedisValue[] vlu = {

            //    //"a",
            //    //"b"

            //    //};
            //    //db.StringAppend("", "");
            //    //db.SetAdd("key", "value");

            //    //var zxzz = db.ListRightPush("ccccc", vlu);
            //    //var zxxxx = db.Publish("a", "b");
            //    //var ew = zxxxx.ToString();
            //    // var result= bllWeiXin.UploadImageToWeixin(bllWeiXin.GetAccessToken(),"image","D:\\img.png");

            //    // websiteOwner = txtWebsiteOwner.Text;
            //    // bllDis.PageScreenshot("7984",Request.Url.Authority);
            //    //var result=  bllWeiXin.BatchGetMaterial(bll.WebsiteOwner, "news", 0, 20);
            //    // bllWeiXin.SynWeixinNews(bll.WebsiteOwner);
            //    //var result = "{\"item\":[{\"media_id\":\"GiYmleeG0NngThLKmvq_Nh2r07MX_cSO5BVSt0AJ9Nk\",\"content\":{\"news_item\":[{\"title\":\"易起秀微信后台\",\"author\":\"\",\"digest\":\"易起秀微信后台\",\"content\":\"<p>易起秀微信后台<\\/p>\",\"content_source_url\":\"http:\\/\\/create.maka.im\\/display\\/preview\\/mb?id=O122C5NR\",\"thumb_media_id\":\"kGqtk0k4CT6c_M7fq5d6-fRtadQv9C_WFXc969xFclY\",\"show_cover_pic\":1,\"url\":\"http:\\/\\/mp.weixin.qq.com\\/s?__biz=MzA3MTkyMjkzNQ==&mid=204784291&idx=1&sn=fbec126e40163d8180567b5a4851eb51#rd\",\"thumb_url\":\"http:\\/\\/mmbiz.qpic.cn\\/mmbiz\\/3atia1EIHTb9g2qZNMZ7VpCaNfGXXy3Q9TGWjou8u31gWhOhGt4oWBEk7zkgkE45PrOKV3Dia2raMgwBNxhbRUAg\\/0\"}],\"create_time\":1429709547,\"update_time\":1429709622},\"update_time\":1429709622},{\"media_id\":\"kGqtk0k4CT6c_M7fq5d6-YVacjpmZV3rKpGJUMhK-p4\",\"content\":{\"news_item\":[{\"title\":\"title\",\"author\":\"author\",\"digest\":\"正文\",\"content\":\"<p>正文<\\/p>\",\"content_source_url\":\"http:\\/\\/comeoncloud.comeoncloud.net\",\"thumb_media_id\":\"kGqtk0k4CT6c_M7fq5d6-fRtadQv9C_WFXc969xFclY\",\"show_cover_pic\":1,\"url\":\"http:\\/\\/mp.weixin.qq.com\\/s?__biz=MzA3MTkyMjkzNQ==&mid=203012908&idx=1&sn=bce826135ab2425c975bf547efa46767#rd\",\"thumb_url\":\"http:\\/\\/mmbiz.qpic.cn\\/mmbiz\\/3atia1EIHTbibZzyxO1wx6pibEoVZf4OVD2oibDhear38ONUjlLnpYt4kiaPqROFgReRFAjoKmeaiaTAwnJvShlBAIuQ\\/0\"},{\"title\":\"title\",\"author\":\"author\",\"digest\":\"正文\",\"content\":\"<p>正文<\\/p>\",\"content_source_url\":\"http:\\/\\/www.baidu.com\",\"thumb_media_id\":\"kGqtk0k4CT6c_M7fq5d6-fRtadQv9C_WFXc969xFclY\",\"show_cover_pic\":1,\"url\":\"http:\\/\\/mp.weixin.qq.com\\/s?__biz=MzA3MTkyMjkzNQ==&mid=203012908&idx=2&sn=d2dcc2e4a9095e2b001813cbfac6b04a#rd\",\"thumb_url\":\"http:\\/\\/mmbiz.qpic.cn\\/mmbiz\\/3atia1EIHTbibZzyxO1wx6pibEoVZf4OVD2oibDhear38ONUjlLnpYt4kiaPqROFgReRFAjoKmeaiaTAwnJvShlBAIuQ\\/0\"}],\"create_time\":1422928332,\"update_time\":1422928425},\"update_time\":1422928425}],\"total_count\":2,\"item_count\":2}";

            //    //JToken token = JToken.Parse(result);

            //    //foreach (var item in token["item"])
            //    //{
            //    //    foreach (var newsItem in item["content"]["news_item"])
            //    //    {
            //    //        var title=newsItem["title"];//标题
            //    //        var digest = newsItem["digest"];//图片
            //    //        var thumbUrl = newsItem["thumb_url"];//缩略图
            //    //        var url=newsItem["url"];//链接

            //    //    }
            //    //}

            //    //bllDis.SynDistributionCount("hf");
            //    //Session.Clear();

            //    //JToken token = JToken.Parse(new StreamReader("D:\\log.txt",System.Text.Encoding.UTF8).ReadLine());
            //    //var usname = token["authorizer_info"]["user_name"].ToString();
            //    //var servertype= token["authorizer_info"]["service_type_info"]["id"].ToString();
            //    //var vertype = token["authorizer_info"]["verify_type_info"]["id"].ToString();
            //    ////100000845500
            //    //string orderid = "845500";
            //    //var left = orderid.PadLeft(11, '0');
            //    //orderid = string.Format("1{0}", orderid.PadLeft(11, '0'));
            //    ////string a = "http://dev8.comeoncloud.net/145/chtml?id=1&from=2";
            //    //string b = HttpUtility.UrlEncode(a);
            //    //Session.Clear();
            //    //string reUrl = Request["redirecturl"];

            //    //var acctoken= bllWeixinOpen.GetComponentAccessToken();
            //    //Response.Write(acctoken);
            //    // return;

            //    //bllWeiXin.CreateWeixinClientMenu(acctoken, "{\"button\":[{  \"type\":\"click\", \"name\":\"今日歌曲\", \"key\":\"V1001_TODAY_MUSIC\" }]}");	

            //    //var currentWebsiteInfo = bllWeiXin.Get<ZentCloud.BLLJIMP.Model.WebsiteInfo>(string.Format("WebsiteOwner='{0}'", "hf"));
            //    // var amount = 630;
            //    //var p1 = (amount * (((decimal)371) / (amount)));
            //    //var p2 = (amount * (((decimal)209) / (amount)));
            //    //var p3 = (amount * (((decimal)52) / (amount)));
            //    //var total = p1 + p2 + p3;


            //    //var amount = 447;
            //    //var p1 = (amount * (((decimal)199) / (632)));
            //    //var p2 = (amount * (((decimal)99) / (632)));
            //    //var p3 = (amount * (((decimal)199) / (632)));
            //    //var total = p1 + p2 + p3;


            //    //decimal rate = ;//此sku所占的比例

            //    //var p1=630*(((decimal)371) / (decimal)(632));
            //    //var p2 = 630 * (((decimal)209) / (decimal)(632));
            //    //var p3 = 630* (((decimal)52) / (decimal)(632));

            //    //var total = p1 + p2 + p3;

            //    //var p1 = 447 * (((decimal)199) / (decimal)(497));
            //    //var p2 = 447 * (((decimal)99) / (decimal)(497));
            //    //var p3 = 447 * (((decimal)199) / (decimal)(497));

            //    //var total = p1 + p2 + p3;

            //    //var tot = 178.98 + 178.98 + 89.04;
            //    //string data="{ \"authorization_info\": { \"authorizer_appid\": \"wxf8b4f85f3a794e77\", \"authorizer_access_token\": \"QXjUqNqfYVH0yBE1iI_7vuN_9gQbpjfK7hYwJ3P7xOa88a89-Aga5x1NMYJyB8G2yKt1KCl0nPC3W9GJzw0Zzq_dBxc8pxIGUNi_bFes0qM\", \"expires_in\": 7200, \"authorizer_refresh_token\": \"dTo-YCXPL4llX-u1W1pPpnp8Hgm4wpJtlR6iV0doKdY\", \"func_info\": [ { \"funcscope_category\": { \"id\": 1 } }, { \"funcscope_category\": { \"id\": 2 } }, { \"funcscope_category\": { \"id\": 3 } } ] } }";
            //    //ZentCloud.BLLJIMP.BLLWeixinOpen.AuthorizationInfoModel obj = ZentCloud.Common.JSONHelper.JsonToModel<ZentCloud.BLLJIMP.BLLWeixinOpen.AuthorizationInfoModel>(data);


            //    //obj.authorization_info.func_info[0].funcscope_category.id;
            //    //Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();

            //    //Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus("DS0000020", "DS0000020", "18516505529");
            //    //if (yikeUser != null)
            //    //{

            //    //}
            //    //var token= bllWeixinOpen.GetApiComponentToken("123456","516516","fdsfsdf");  
            //    //EZ001088
            //    //Open.EZRproSDK.Client client = new Open.EZRproSDK.Client();
            //    //Response.Write(client.CreateSign("mixblu", "20150807154158", "Mblu123Tkn98").ToUpper());
            //    //Response.Write("<br />积分：" + client.GetBonus("EZ001088", "EZ001088", "18721029082").BonusTotal);
            //    //Response.Write(UrlTest(Request.Url.ToString(), new List<string>() {"a","b" }));


            //    //BLLJIMP.Model.WXMallOrderInfo order = bll.Get<BLLJIMP.Model.WXMallOrderInfo>(" OrderID = 140268 ");//bll.GetOrderInfoByOrderID("140268");
            //    //client.OrderUpload(order);

            //    //Open.EfastSDK.Client clientEfast = new Open.EfastSDK.Client();
            //    //clientEfast.GetGoodsList();

            //    // Open.EfastSDK.Client client = new Open.EfastSDK.Client();

            //    //Response.Write(  client.CreateOrder());
            //    //BllScore bllScore = new BllScore();
            //    //bllScore.ScoreRank("", "");

            //    //公众平台上开发者设置的token, appID, EncodingAESKey
            //    //string sToken = "QDG6eK";
            //    //string sAppID = "wx5823bf96d3bd56c7";
            //    //string sEncodingAESKey = "jWmYm7qr5nMoAUwZRjGtBxmz3KA1tkAj3ykkR6q2B2C";

            //    //Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

            //    /* 1. 对用户回复的数据进行解密。
            //    * 用户回复消息或者点击事件响应时，企业会收到回调消息，假设企业收到的推送消息：
            //    * 	POST /cgi-bin/wxpush? msg_signature=477715d11cdb4164915debcba66cb864d751f3e6&timestamp=1409659813&nonce=1372623149 HTTP/1.1
            //       Host: qy.weixin.qq.com
            //       Content-Length: 613
            //    *
            //    * 	<xml>
            //           <ToUserName><![CDATA[wx5823bf96d3bd56c7]]></ToUserName>
            //           <Encrypt><![CDATA[RypEvHKD8QQKFhvQ6QleEB4J58tiPdvo+rtK1I9qca6aM/wvqnLSV5zEPeusUiX5L5X/0lWfrf0QADHHhGd3QczcdCUpj911L3vg3W/sYYvuJTs3TUUkSUXxaccAS0qhxchrRYt66wiSpGLYL42aM6A8dTT+6k4aSknmPj48kzJs8qLjvd4Xgpue06DOdnLxAUHzM6+kDZ+HMZfJYuR+LtwGc2hgf5gsijff0ekUNXZiqATP7PF5mZxZ3Izoun1s4zG4LUMnvw2r+KqCKIw+3IQH03v+BCA9nMELNqbSf6tiWSrXJB3LAVGUcallcrw8V2t9EL4EhzJWrQUax5wLVMNS0+rUPA3k22Ncx4XXZS9o0MBH27Bo6BpNelZpS+/uh9KsNlY6bHCmJU9p8g7m3fVKn28H3KDYA5Pl/T8Z1ptDAVe0lXdQ2YoyyH2uyPIGHBZZIs2pDBS8R07+qN+E7Q==]]></Encrypt>
            //       </xml>
            //    */
            //    //string sReqMsgSig = "477715d11cdb4164915debcba66cb864d751f3e6";
            //    //string sReqTimeStamp = "1409659813";
            //    //string sReqNonce = "1372623149";
            //    //string sReqData = "<xml><ToUserName><![CDATA[wx5823bf96d3bd56c7]]></ToUserName><Encrypt><![CDATA[RypEvHKD8QQKFhvQ6QleEB4J58tiPdvo+rtK1I9qca6aM/wvqnLSV5zEPeusUiX5L5X/0lWfrf0QADHHhGd3QczcdCUpj911L3vg3W/sYYvuJTs3TUUkSUXxaccAS0qhxchrRYt66wiSpGLYL42aM6A8dTT+6k4aSknmPj48kzJs8qLjvd4Xgpue06DOdnLxAUHzM6+kDZ+HMZfJYuR+LtwGc2hgf5gsijff0ekUNXZiqATP7PF5mZxZ3Izoun1s4zG4LUMnvw2r+KqCKIw+3IQH03v+BCA9nMELNqbSf6tiWSrXJB3LAVGUcallcrw8V2t9EL4EhzJWrQUax5wLVMNS0+rUPA3k22Ncx4XXZS9o0MBH27Bo6BpNelZpS+/uh9KsNlY6bHCmJU9p8g7m3fVKn28H3KDYA5Pl/T8Z1ptDAVe0lXdQ2YoyyH2uyPIGHBZZIs2pDBS8R07+qN+E7Q==]]></Encrypt></xml>";
            //    //string sMsg = "";  //解析之后的明文
            //    //int ret = 0;
            //    //ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            //    //if (ret != 0)
            //    //{
            //    //    System.Console.WriteLine("ERR: Decrypt fail, ret: " + ret);
            //    //    return;
            //    //}
            //    //System.Console.WriteLine(sMsg);


            //    /*
            //     * 2. 企业回复用户消息也需要加密和拼接xml字符串。
            //     * 假设企业需要回复用户的消息为：
            //     * 		<xml>
            //     * 		<ToUserName><![CDATA[mycreate]]></ToUserName>
            //     * 		<FromUserName><![CDATA[wx5823bf96d3bd56c7]]></FromUserName>
            //     * 		<CreateTime>1348831860</CreateTime>
            //            <MsgType><![CDATA[text]]></MsgType>
            //     *      <Content><![CDATA[this is a test]]></Content>
            //     *      <MsgId>1234567890123456</MsgId>
            //     *      </xml>
            //     * 生成xml格式的加密消息过程为：
            //     */
            //    //string sRespData = "<xml><ToUserName><![CDATA[mycreate]]></ToUserName><FromUserName><![CDATA[wx582测试一下中文的情况，消息长度是按字节来算的396d3bd56c7]]></FromUserName><CreateTime>1348831860</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[this is a test]]></Content><MsgId>1234567890123456</MsgId></xml>";
            //    //string sEncryptMsg = ""; //xml格式的密文
            //    //ret = wxcpt.EncryptMsg(sRespData, sReqTimeStamp, sReqNonce, ref sEncryptMsg);
            //    //System.Console.WriteLine("sEncryptMsg");
            //    //System.Console.WriteLine(sEncryptMsg);

            //    ///*测试：
            //    // * 将sEncryptMsg解密看看是否是原文
            //    // * */
            //    //XmlDocument doc = new XmlDocument();
            //    //doc.LoadXml(sEncryptMsg);
            //    //XmlNode root = doc.FirstChild;
            //    //string sig = root["MsgSignature"].InnerText;
            //    //string enc = root["Encrypt"].InnerText;
            //    //string timestamp = root["TimeStamp"].InnerText;
            //    //string nonce = root["Nonce"].InnerText;
            //    //string stmp = "";
            //    //ret = wxcpt.DecryptMsg(sig, timestamp, nonce, sEncryptMsg, ref stmp);
            //    //System.Console.WriteLine("stemp");
            //    //System.Console.WriteLine(stmp + ret);
            //    //return;
            //}
            //public string UrlTest(string url, List<string> parmList)
            //{
            //    string result = url;
            //    var arr = url.Split('?');
            //    if (arr.Length > 1)
            //    {
            //        var arr2 = arr.GetValue(1).ToString().Split('&');

            //        for (int i = 0; i < arr2.Length; i++)
            //        {
            //            for (int j = 0; j < parmList.Count; j++)
            //            {
            //                if (arr2.GetValue(i).ToString().IndexOf(parmList[j] + "=") == 0)
            //                {
            //                    result = result.Replace(arr2.GetValue(i).ToString(), "");
            //                }
            //            }

            //        }

            //        result = result.Replace("&&", "&").Replace("?&", "?").TrimEnd(new char[] { '&', '?' });
            //    }


            //    return result;
            //}

            //protected void Button1_Click(object sender, EventArgs e)
            //{
            //    Common.DataCache.ClearCacheAll();
            //}

            //protected void Button2_Click(object sender, EventArgs e)
            //{
            //    Common.DataCache.ClearCacheStartsWith(TextBox1.Text.Trim());
            //}

            //protected void btnScoreRule_Click(object sender, EventArgs e)
            //{
            //    List<WebsiteInfo> websiteList = bll.GetList<WebsiteInfo>();
            //    foreach (var item in websiteList)
            //    {
            //        string websiteOwner = item.WebsiteOwner;

            //        #region 添加默认积分规则

            //        if (bll.GetCount<ScoreDefineInfo>(string.Format(" Websiteowner='{0}' And ScoreType='OrderPay'", websiteOwner)) <= 0)
            //        {

            //            ScoreDefineInfo scoreDefineInfo1 = new ScoreDefineInfo();
            //            scoreDefineInfo1.ScoreId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            scoreDefineInfo1.Score = 0;
            //            scoreDefineInfo1.ScoreType = "OrderPay";
            //            scoreDefineInfo1.Name = "订单付款";
            //            scoreDefineInfo1.Description = "订单付款";
            //            scoreDefineInfo1.WebsiteOwner = websiteOwner;
            //            scoreDefineInfo1.CreateUserId = "system";
            //            scoreDefineInfo1.InsertTime = DateTime.Now;
            //            scoreDefineInfo1.IsHide = 0;
            //            scoreDefineInfo1.DayLimit = -1;
            //            scoreDefineInfo1.TotalLimit = -1;
            //            scoreDefineInfo1.OrderNum = 0;
            //            scoreDefineInfo1.Ex1 = "0";
            //            bll.Add(scoreDefineInfo1);

            //        }
            //        if (bll.GetCount<ScoreDefineInfo>(string.Format(" Websiteowner='{0}' And ScoreType='ReadArticle'", websiteOwner)) <= 0)
            //        {


            //            ScoreDefineInfo scoreDefineInfo2 = new ScoreDefineInfo();
            //            scoreDefineInfo2.ScoreId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            scoreDefineInfo2.Score = 0;
            //            scoreDefineInfo2.ScoreType = "ReadArticle";
            //            scoreDefineInfo2.Name = "阅读文章";
            //            scoreDefineInfo2.Description = "阅读文章";
            //            scoreDefineInfo2.WebsiteOwner = websiteOwner;
            //            scoreDefineInfo2.CreateUserId = "system";
            //            scoreDefineInfo2.InsertTime = DateTime.Now;
            //            scoreDefineInfo2.IsHide = 0;
            //            scoreDefineInfo2.DayLimit = -1;
            //            scoreDefineInfo2.TotalLimit = -1;
            //            scoreDefineInfo2.OrderNum = 0;
            //            scoreDefineInfo2.Ex1 = "0";
            //            bll.Add(scoreDefineInfo2);

            //        }


            //        if (bll.GetCount<ScoreDefineInfo>(string.Format(" Websiteowner='{0}' And ScoreType='ReadCategory'", websiteOwner)) <= 0)
            //        {

            //            ScoreDefineInfo scoreDefineInfo3 = new ScoreDefineInfo();
            //            scoreDefineInfo3.ScoreId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            scoreDefineInfo3.Score = 0;
            //            scoreDefineInfo3.ScoreType = "ReadCategory";
            //            scoreDefineInfo3.Name = "阅读分类";
            //            scoreDefineInfo3.Description = "阅读分类";
            //            scoreDefineInfo3.WebsiteOwner = websiteOwner;
            //            scoreDefineInfo3.CreateUserId = "system";
            //            scoreDefineInfo3.InsertTime = DateTime.Now;
            //            scoreDefineInfo3.IsHide = 0;
            //            scoreDefineInfo3.DayLimit = -1;
            //            scoreDefineInfo3.TotalLimit = -1;
            //            scoreDefineInfo3.OrderNum = 0;
            //            scoreDefineInfo3.Ex1 = "0";
            //            bll.Add(scoreDefineInfo3);
            //        }

            //        if (bll.GetCount<ScoreDefineInfo>(string.Format(" Websiteowner='{0}' And ScoreType='ShareArticle'", websiteOwner)) <= 0)
            //        {

            //            ScoreDefineInfo scoreDefineInfo4 = new ScoreDefineInfo();
            //            scoreDefineInfo4.ScoreId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            scoreDefineInfo4.Score = 0;
            //            scoreDefineInfo4.ScoreType = "ShareArticle";
            //            scoreDefineInfo4.Name = "分享文章";
            //            scoreDefineInfo4.Description = "分享文章";
            //            scoreDefineInfo4.WebsiteOwner = websiteOwner;
            //            scoreDefineInfo4.CreateUserId = "system";

            //            scoreDefineInfo4.InsertTime = DateTime.Now;
            //            scoreDefineInfo4.IsHide = 0;
            //            scoreDefineInfo4.DayLimit = -1;
            //            scoreDefineInfo4.TotalLimit = -1;
            //            scoreDefineInfo4.OrderNum = 0;
            //            scoreDefineInfo4.Ex1 = "0";
            //            bll.Add(scoreDefineInfo4);
            //        }

            //        if (bll.GetCount<ScoreDefineInfo>(string.Format(" Websiteowner='{0}' And ScoreType='Signin'", websiteOwner)) <= 0)
            //        {

            //            ScoreDefineInfo scoreDefineInfo5 = new ScoreDefineInfo();
            //            scoreDefineInfo5.ScoreId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            scoreDefineInfo5.Score = 0;
            //            scoreDefineInfo5.ScoreType = "Signin";
            //            scoreDefineInfo5.Name = "签到";
            //            scoreDefineInfo5.Description = "签到";
            //            scoreDefineInfo5.WebsiteOwner = websiteOwner;
            //            scoreDefineInfo5.CreateUserId = "system";

            //            scoreDefineInfo5.InsertTime = DateTime.Now;
            //            scoreDefineInfo5.IsHide = 0;
            //            scoreDefineInfo5.DayLimit = 1;
            //            scoreDefineInfo5.TotalLimit = -1;
            //            scoreDefineInfo5.OrderNum = 0;
            //            scoreDefineInfo5.Ex1 = "0";
            //            bll.Add(scoreDefineInfo5);
            //        }



            //        if (bll.GetCount<ScoreDefineInfo>(string.Format(" Websiteowner='{0}' And ScoreType='UpdateMyInfo'", websiteOwner)) <= 0)
            //        {

            //            ScoreDefineInfo scoreDefineInfo6 = new ScoreDefineInfo();
            //            scoreDefineInfo6.ScoreId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            scoreDefineInfo6.Score = 0;
            //            scoreDefineInfo6.ScoreType = "UpdateMyInfo";
            //            scoreDefineInfo6.Name = "完善个人资料";
            //            scoreDefineInfo6.Description = "完善个人资料";
            //            scoreDefineInfo6.WebsiteOwner = websiteOwner;
            //            scoreDefineInfo6.CreateUserId = "system";
            //            scoreDefineInfo6.InsertTime = DateTime.Now;
            //            scoreDefineInfo6.IsHide = 0;
            //            scoreDefineInfo6.DayLimit = 1;
            //            scoreDefineInfo6.TotalLimit = 1;
            //            scoreDefineInfo6.OrderNum = 0;
            //            scoreDefineInfo6.Ex1 = "0";
            //            bll.Add(scoreDefineInfo6);
            //        }

            //        #endregion


            //    }


            //}

            //protected void btnAddSysComponent_Click(object sender, EventArgs e)
            //{
            //    List<string> websiteList = new List<string>();
            //    string websiteOwner = TextBox2.Text.Trim();
            //    if (!CheckBox1.Checked)
            //    {
            //        if (websiteOwner == "")
            //        {
            //            ZentCloud.Common.WebMessageBox.Show(this, "请输入站点");
            //            return;
            //        }

            //        WebsiteInfo website = bllWebSite.GetWebsiteInfo(websiteOwner);
            //        if (website == null)
            //        {
            //            ZentCloud.Common.WebMessageBox.Show(this, "站点不存在");
            //            return;
            //        }
            //        websiteList.Add(websiteOwner);
            //    }
            //    else
            //    {
            //        websiteList = bll.GetList<WebsiteInfo>().Select(p => p.WebsiteOwner).ToList();
            //    }
            //    foreach (string website in websiteList)
            //    {
            //        #region 添加默认导航（商城个人中心-我的订单 商城个人中心-其他 商城底部导航等）
            //        //string json_SysToolBar_Path = ZentCloud.Common.ConfigHelper.GetConfigString("json_SysToolBar");
            //        //if (!string.IsNullOrWhiteSpace(json_SysToolBar_Path))
            //        //{
            //        //    string json_SysToolBarStr = File.ReadAllText(this.Server.MapPath(json_SysToolBar_Path));
            //        //    JObject json_SysToolBar = JObject.Parse(json_SysToolBarStr);
            //        //    bllWebSite.AddSysToolBars(website, json_SysToolBar);
            //        //}

            //        #endregion
            //        #region 添加默认组件（个人中心 商城首页等）
            //        string json_SysComponent_Path = ZentCloud.Common.ConfigHelper.GetConfigString("json_SysComponent");
            //        if (!string.IsNullOrWhiteSpace(json_SysComponent_Path))
            //        {
            //            string json_SysComponentStr = File.ReadAllText(this.Server.MapPath(json_SysComponent_Path));
            //            JObject json_SysComponent = JObject.Parse(json_SysComponentStr);
            //            bllComponent.AddSysComponents(website, json_SysComponent);
            //        }
            //        #endregion
            //    }
            //}

            //protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
            //{
            //    if (CheckBox1.Checked)
            //    {
            //        TextBox2.Text = "";
            //    }
            //    TextBox2.Enabled = !CheckBox1.Checked;
            //}

            //protected void Button4_Click(object sender, EventArgs e)
            //{
            //    List<WXBroadcastHistory> wxbList = new List<WXBroadcastHistory>();

            //    wxbList = bll.GetList<WXBroadcastHistory>(" broadcasttype is not null ");

            //    List<string> ids = new List<string>() {
            //        "2016021502033215DK9Q",
            //        "201602150901513BS9F7",
            //        "201602150905051IL3DQ",
            //        "201602151002082A6BE7",
            //        "20160215100421EV7R6X",
            //        "20160215100458SSCJLZ",
            //        "20160215102514I071GM",
            //        "20160215102652CUC79R",
            //        "20160215102902RJ5B6B",
            //        "20160215103424XWHK27",
            //        "20160215103715Q8D496",
            //        "20160215104254CBBOOY",
            //        "20160215105002KW4BM0",
            //        "20160215120912ELVOMI",
            //        "201602151215113OUHXV",
            //        "20160215121828KGZ543",
            //        "20160215121903D9D8SH",
            //        "201602151221362CKOSX"
            //    };

            //    foreach (var item in ids)
            //    {
            //        List<WXBroadcastHistory> data = wxbList.Where(p => p.SerialNum == item).ToList();

            //        SMSPlanInfo plan = new SMSPlanInfo();
            //        plan.ChargeCount = 0;
            //        plan.PlanID = item;
            //        plan.SubmitDate = DateTime.Now;
            //        plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WXTemplateMsg_Notify;
            //        plan.SenderID = "jubit";
            //        plan.SendContent = data[0].Msg;
            //        plan.SendFrom = "发送模板通知消息";
            //        plan.SubmitCount = data.Count;
            //        plan.SubmitDate = DateTime.Now;
            //        plan.Title = data[0].Title;
            //        plan.Url = data[0].Url;
            //        plan.UsePipe = "none";
            //        plan.ProcStatus = 1;
            //        plan.WebsiteOwner = data[0].WebsiteOwner;
            //        plan.SuccessCount = data.Count(p => p.Status == 1);

            //        if (bll.GetCount<SMSPlanInfo>(string.Format(" PlanID = '{0}' ", plan.PlanID)) == 0)
            //        {
            //            bll.Add(plan);
            //        }

            //    }

            //}

            //protected void Button5_Click(object sender, EventArgs e)
            //{


            //    //获取当前站点的分销活动
            //    string activityId = bllDistributionOffLine.GetDistributionOffLineApplyActivityID();


            //    //取出所有报名的人
            //    //bll.GetLit<ActivityDataInfo> 

            //    //更新用户信息

            //    Response.Write("同步完成");

            //}

            //protected void Button6_Click(object sender, EventArgs e)
            //{
            //    BLLMall bllMall = new BLLMall();
            //    int successCount = 0;
            //    List<WXMallProductInfo> productList = bll.GetList<WXMallProductInfo>(string.Format("websiteowner='{0}'", txtWebsiteOwner.Text));
            //    foreach (var item in productList)
            //    {

            //        List<ProductSku> productSkuList = bllMall.GetProductSkuList(int.Parse(item.PID));
            //        if (productSkuList.Count == 0)
            //        {

            //            ProductSku model = new ProductSku();
            //            model.WebSiteOwner = txtWebsiteOwner.Text;
            //            model.InsertDate = DateTime.Now;
            //            model.Modified = DateTime.Now;
            //            model.Price = item.Price;
            //            model.ProductId = int.Parse(item.PID);
            //            model.Props = "";
            //            model.ShowProps = "";
            //            model.SkuId = int.Parse(bll.GetGUID(TransacType.CommAdd));
            //            model.Stock = item.Stock;
            //            if (bll.Add(model))
            //            {
            //                successCount++;
            //            }
            //        }


            //    }
            //    Response.Write(successCount);
            //}

            //protected void Button7_Click(object sender, EventArgs e)
            //{
            //    string json = "{\"OldCode\":null,\"Code\":\"DS0004509\",\"NickName\":\"梓琪\",\"MobileNo\":\"18335273151\",\"Name\":null,\"Sex\":null,\"PassWord\":null,\"Birthday\":null,\"WxNo\":\"oHavmjk-NuzE5WlJYRHqX_GxypVs\",\"WxUnionId\":\"oI8jGwRZIzfan4-WDjc0LOs-TPNk\",\"WeibNo\":\"\",\"QqNo\":\"\",\"TbNo\":\"\",\"Email\":\"\",\"RegShop\":\"00001\",\"RegDate\":\"2016-03-18\",\"Province\":\"山西省\",\"City\":\"大同市\",\"County\":\"矿区\"}";
            //    Open.EZRproSDK.Entity.MemberCallBackReq info = new Open.EZRproSDK.Entity.MemberCallBackReq();
            //    info.Args = JsonConvert.DeserializeObject<Open.EZRproSDK.Entity.MemberInfo>(json);
            //}

            //protected void Button8_Click(object sender, EventArgs e)
            //{
            //    //string strSql = "SELECT * FROM ZCJ_ActivityForwardInfo where WebsiteOwner='" + websiteOwner + "' ";
            //    //List<ActivityForwardInfo> ForwardList = ZentCloud.ZCBLLEngine.BLLBase.Query<ActivityForwardInfo>(strSql);
            //    int count = 0;
            //    foreach (ActivityForwardInfo item in bll.GetList<ActivityForwardInfo>(""))
            //    {
            //        JuActivityInfo juactivityModel = bllJuActivity.GetJuActivity(int.Parse(item.ActivityId));
            //        if (juactivityModel == null) continue;
            //        count += bll.Update(new ActivityForwardInfo(), string.Format(" PV={0},UV={1} ", juactivityModel.PV, juactivityModel.UV), string.Format(" ActivityId='{0}' ", item.ActivityId));
            //    }
            //    Response.Write("更新了" + count + "条数据.");
            //}

            //protected void Button9_Click(object sender, EventArgs e)
            //{
            //    //string strSql = "SELECT * FROM ZCJ_MonitorLinkInfo where WebsiteOwner='"+websiteOwner+"' ";
            //    //List<MonitorLinkInfo> LinkInfoList = ZentCloud.ZCBLLEngine.BLLBase.Query<MonitorLinkInfo>(strSql);
            //    int count = 0;
            //    // MonitorLinkInfo linkModel = bll.Get<MonitorLinkInfo>(string.Format(" MonitorPlanID={0} AND LinkName='{1}' ", monitorPlanID, spreadUserId));
            //    //int signupCount = bll.GetCount<ActivityDataInfo>(string.Format("MonitorPlanID={0} And SpreadUserID='{1}' And IsDelete=0", int.Parse(monitorPlanID), spreadUserId));
            //    //if (linkModel != null)
            //    //{
            //    //    linkModel.ActivitySignUpCount = signupCount;
            //    //     bll.Update(linkModel);
            //    // }
            //    //int count = GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND DistributionOwner='{2}'", WebsiteOwner, linkInfo.ActivityId, linkInfo.LinkName));
            //    //int uv = GetCount<MonitorEventDetailsInfo>("EventUserID",string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND SpreadUserID='{2}'", WebsiteOwner, linkInfo.MonitorPlanID, linkInfo.LinkName));
            //    //linkInfo.UV = uv;
            //    //linkInfo.PowderCount = count;
            //    foreach (var item in bll.GetList<MonitorLinkInfo>(""))
            //    {
            //        //报名数量
            //        int signupCount = bll.GetCount<ActivityDataInfo>(string.Format("MonitorPlanID={0} And SpreadUserID='{1}' And IsDelete=0", item.MonitorPlanID, item.LinkName));
            //        //微信阅读人数
            //        int UVCount = bll.GetCount<MonitorEventDetailsInfo>("EventUserID", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND SpreadUserID='{2}'", item.WebsiteOwner, item.MonitorPlanID, item.LinkName));
            //        //吸粉数量
            //        int powderCount = bll.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND DistributionOwner='{2}'", item.WebsiteOwner, item.ActivityId, item.LinkName));

            //        item.ActivitySignUpCount = signupCount;
            //        item.UV = UVCount;
            //        item.PowderCount = powderCount;
            //        count += bll.Update(new MonitorLinkInfo(), string.Format(" ActivitySignUpCount={0},UV={1},PowderCount={2} ", signupCount, UVCount, powderCount), string.Format(" LinkID={0} ", item.LinkID));
            //    }
            //    Response.Write("更新了" + count + "条数据.");
            //}

            //protected void Button10_Click(object sender, EventArgs e)
            //{
            //    if (Request.Cookies["test_cookie"] != null)
            //    {
            //        var test_cookie = Request.Cookies["test_cookie"].Value;
            //        Response.Write("test_cookie:" + test_cookie);
            //    }

            //    HttpCookie cookie = new HttpCookie("test_cookie");
            //    cookie.Value = TextBox2.Text;
            //    cookie.Expires = DateTime.MaxValue;

            //    Response.Cookies.Add(cookie);

            //    RedisHelper.RedisHelper redis = new RedisHelper.RedisHelper();
            //    //redis.Test();


            //}
            //protected void Button11_Click(object sender, EventArgs e)
            //{

            //    RedisHelper.RedisHelper redis = new RedisHelper.RedisHelper();
            //    // redis.Test1();


            //}

            //protected void Unnamed1_Click(object sender, EventArgs e)
            //{
            //    BLLJIMP.BLLUserStatistics kk = new BLLUserStatistics();
            //    kk.AddUserStatistics();
            //}

            //protected void Button10_Click1(object sender, EventArgs e)
            //{
            //    string websiteOwner = TextBox3.Text.Trim();
            //    string yearMonthString = TextBox4.Text.Trim();
            //    int yearMonth = int.Parse(yearMonthString);
            //    bllDis.BuildMonthPerformance(yearMonth, websiteOwner);
            //}

            //protected void Unnamed2_Click(object sender, EventArgs e)
            //{
            //    //bllDis.PageScreenshot("o99IZtxrESF18vDLG4gdLZbdhYlQ", this.Request.Url.Authority);
            //    BLLMall bllMall = new BLLMall();
            //    bllMall.Statistics();
            //}

            //protected void Button11_Click1(object sender, EventArgs e)
            //{
            //    BLLMall bllMall = new BLLMall();
            //    bllMall.TimingOrderAutoComment();
            //}




        }


       

        protected void Button1_Click(object sender, EventArgs e)
        {
            var website = bll.GetWebsiteInfoModelFromDataBase();
            var orderInfo = string.Empty;
            if (!string.IsNullOrEmpty(website.ElemeAccessToken) && website.ElemeTokenLastUpdateDate.Value.AddDays(1) > DateTime.Now)
            {
                OrderHelper orderHelper = new OrderHelper(website.ElemeAccessToken,website.ElemeAppKey,website.ElemeAppSecret);
                orderInfo=orderHelper.GetOrder("1205751603879834806");
            }
            else
            {
                Authorize model = new Authorize(website.ElemeAppKey, website.ElemeAppSecret);
                //AuthorizeResponse toKenModel = model.GetToken();
                //website.ElemeAccessToken = toKenModel.access_token;
                website.ElemeTokenLastUpdateDate = DateTime.Now;
                bll.Update(website);
                OrderHelper orderHelper = new OrderHelper(website.ElemeAccessToken, website.ElemeAppKey, website.ElemeAppSecret);
                orderInfo=orderHelper.GetOrder("1205751603879834806");
            }
            RespOnseModel elemeOrder = JsonConvert.DeserializeObject<RespOnseModel>(orderInfo);

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            WXMallOrderInfo mallOrder = new WXMallOrderInfo();
            OOrder oorder = elemeOrder.result;
            mallOrder.OrderID = oorder.id;
            mallOrder.Address = oorder.address;
            mallOrder.OrderType = 8;
            mallOrder.TakeOutType = "eleme";
            if (!string.IsNullOrEmpty(oorder.createdAt))
            {
                mallOrder.InsertDate = GetDate(oorder.createdAt);
            }
            if (!string.IsNullOrEmpty(oorder.activeAt))
            {
                mallOrder.PayTime = GetDate(oorder.activeAt);
            }
            mallOrder.Ex1 = oorder.deliverFee.ToString();//配送费
            if (!string.IsNullOrEmpty(oorder.deliverTime))
            {
                mallOrder.Ex2 = GetDate(oorder.deliverTime).ToString();//预计送达时间
            }
            mallOrder.OrderMemo = oorder.description;
            mallOrder.Ex3 = oorder.invoice;
            mallOrder.Ex4 = oorder.book.ToString();
            mallOrder.Ex5 = oorder.onlinePaid.ToString();
            List<string> phoneList = oorder.phoneList;
            if (phoneList.Count > 0)
            {
                for (int i = 0; i < phoneList.Count; i++)
                {
                    if (i == 0) mallOrder.Phone = phoneList[i];
                    mallOrder.Ex6 = phoneList[i] + ",";
                }
                mallOrder.Ex6 = mallOrder.Ex6.Substring(mallOrder.Ex6.Length - 1, 1);
            }
            mallOrder.Ex7 = oorder.shopId.ToString();
            mallOrder.Ex8 = oorder.openId;
            mallOrder.Ex9 = oorder.shopName;
            mallOrder.OrderUserID = oorder.userId.ToString();
            mallOrder.TotalAmount =(decimal)oorder.totalPrice;
            mallOrder.Ex10 = oorder.originalPrice.ToString();
            mallOrder.Consignee = oorder.consignee;
            mallOrder.Ex11 = oorder.deliveryGeo;
            mallOrder.Ex12 = oorder.deliveryPoiAddress;
            mallOrder.Ex13 = oorder.invoiced.ToString();
            mallOrder.Ex14 = oorder.income.ToString();
            mallOrder.Ex15 = oorder.serviceRate.ToString();
            mallOrder.Ex16 = oorder.serviceFee.ToString();
            mallOrder.Ex17 = oorder.hongbao.ToString();
            mallOrder.Ex18 = oorder.packageFee.ToString();
            mallOrder.Ex19 = oorder.activityTotal.ToString();
            mallOrder.Ex20 = oorder.shopPart.ToString();
            mallOrder.Ex21 = oorder.elemePart.ToString();
            mallOrder.WebsiteOwner = bll.WebsiteOwner;
            if (mallOrder.TotalAmount > 0) mallOrder.PaymentStatus = 1;
            mallOrder.Ex22  = oorder.downgraded.ToString();
            mallOrder.OutRefundStatus = oorder.refundStatus;
            if (!bll.Add(mallOrder, tran))
            {
                tran.Rollback();
            }
            List<OGoodsGroup> goodsGroupList = oorder.groups;
            foreach (var item in goodsGroupList)
            {
                WXMallOrderDetailsInfo oorderDetail = new WXMallOrderDetailsInfo();
                oorderDetail.Ex1 = item.name;
                oorderDetail.Ex2 = item.type;
                oorderDetail.OrderID = mallOrder.OrderID;
                List<OGoodsItem> goodItems = item.items;
                foreach (var good in goodItems)
                {
                    oorderDetail.PID = good.id.ToString();
                    oorderDetail.Ex9 = good.skuId.ToString();
                    oorderDetail.ProductName = good.name;
                    oorderDetail.Ex4 = good.categoryId.ToString();
                    oorderDetail.OrderPrice =(decimal)good.price;
                    oorderDetail.TotalCount = good.quantity;
                    oorderDetail.TotalPrice = (decimal)good.total;
                    oorderDetail.Ex7 = good.extendCode;
                    oorderDetail.Ex8 = good.barCode;
                    if (good.weight.HasValue) oorderDetail.Wegith = (decimal)good.weight;
                    oorderDetail.Unit = "元";
                    if (good.newSpecs.Count > 0)
                    {
                        List<OGroupItemSpec> specList = good.newSpecs;
                        oorderDetail.Ex5=ZentCloud.Common.JSONHelper.ListToJson<OGroupItemSpec>(specList);
                    }
                    if (good.attributes.Count > 0)
                    {
                        List<OGroupItemAttribute> attrList = good.attributes;
                        oorderDetail.Ex6 = ZentCloud.Common.JSONHelper.ListToJson<OGroupItemAttribute>(attrList);
                    }
                    if (!bll.Add(oorderDetail, tran))
                    {
                        tran.Rollback();
                    }

                }

            }

            
            
        }

        public DateTime GetDate(string date)
        {
            var time = date.Replace("T", " ");
            return DateTime.Parse(time);
        }

        public class RespOnseModel
        {
            public string id { get; set; }

            public OOrder result { get; set; }

            public errorOrder error { get; set; }
        }
        public class OOrder
        {
            /// <summary>
            /// 送餐地址
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// 下单时间
            /// </summary>
            public string createdAt { get; set; }
            /// <summary>
            /// 订单生效时间
            /// </summary>
            public string activeAt { get; set; }
            /// <summary>
            /// 配送费
            /// </summary>
            public double? deliverFee { get; set; }
            /// <summary>
            /// 预计送达时间
            /// </summary>
            public string deliverTime { get; set; }
            /// <summary>
            /// 订单备注
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 订单详细类目的列表
            /// </summary>
            public List<OGoodsGroup> groups { get; set; }
            /// <summary>
            /// 发票抬头
            /// </summary>
            public string invoice { get; set; }
            /// <summary>
            /// 是否预订单
            /// </summary>
            public bool book { get; set; }
            /// <summary>
            /// 是否在线支付
            /// </summary>
            public bool onlinePaid { get; set; }
            /// <summary>
            /// 订单id
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 顾客联系电话
            /// </summary>
            public List<string> phoneList { get; set; }
            /// <summary>
            /// 店铺id
            /// </summary>
            public long shopId { get; set; }
            /// <summary>
            /// 店铺绑定的外部ID
            /// </summary>
            public string openId { get; set; }
            /// <summary>
            /// 店铺名称
            /// </summary>
            public string shopName { get; set; }
            /// <summary>
            /// 店铺当日订单流水号
            /// </summary>
            public int daySn { get; set; }
            /// <summary>
            /// 订单状态  pending未生效订单unprocessed未处理订单refunding退单处理中valid已处理的有效订单invalid无效订单settled已完成订单
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 退款状态noRefund未申请退单applied用户申请退单rejected店铺拒绝退单arbitrating客服仲裁中failed退单失败successful退单成功
            /// </summary>
            public string refundStatus { get; set; }
            /// <summary>
            /// 下单用户id
            /// </summary>
            public int userId { get; set; }
            /// <summary>
            /// 订单总价，用户实际支付的金额，单位：元
            /// </summary>
            public double? totalPrice { get; set; }
            /// <summary>
            /// 订单原始价格
            /// </summary>
            public double? originalPrice { get; set; }
            /// <summary>
            /// 单收货人姓名
            /// </summary>
            public string consignee { get; set; }
            /// <summary>
            /// 订单收货地址经纬度
            /// </summary>
            public string deliveryGeo { get; set; }
            /// <summary>
            /// 送餐地址
            /// </summary>
            public string deliveryPoiAddress { get; set; }
            /// <summary>
            /// 顾客是否需要发票
            /// </summary>
            public bool invoiced { get; set; }
            /// <summary>
            /// 店铺实收
            /// </summary>
            public double? income { get; set; }
            /// <summary>
            /// 饿了么服务费率
            /// </summary>
            public double? serviceRate { get; set; }
            /// <summary>
            /// 饿了么服务费
            /// </summary>
            public double? serviceFee { get; set; }
            /// <summary>
            /// 订单中的红包金额
            /// </summary>
            public double? hongbao { get; set; }
            /// <summary>
            /// 餐盒费
            /// </summary>
            public double? packageFee { get; set; }
            /// <summary>
            /// 订单活动总额
            /// </summary>
            public double? activityTotal { get; set; }
            /// <summary>
            /// 店铺承担活动费用
            /// </summary>
            public double? shopPart { get; set; }
            /// <summary>
            /// 饿了么承担活动费用
            /// </summary>
            public double? elemePart { get; set; }
            /// <summary>
            /// 降级标识
            /// </summary>
            public bool downgraded { get; set; }

        }

        public class errorOrder
        {
            public string code { get; set; }
            public string message { get; set; }
        }

        public class OGoodsGroup
        {
            /// <summary>
            /// 分组名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 分组类型 normal	普通商品  extra	配送费等额外信息   discount	折扣信息，红包，满减等
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 商品信息的列表
            /// </summary>
            public List<OGoodsItem> items { get; set; }

        }

        public class OGoodsItem
        {
            /// <summary>
            /// 规格Id
            /// </summary>
            public long id { get; set; }
            /// <summary>
            /// SkuId
            /// </summary>
            public long skuId { get; set; }
            /// <summary>
            ///商品名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 订单中商品项的标识(注意，此处不是商品分类Id)
            /// 2配送费  11食物活动  12餐厅活动  13	红包抵扣  15商家代金券抵扣  102	餐盒费  103	饿配送会员减免配送费   108最低消费   200限时抢购   300饿了么会员免配送费
            /// </summary>
            public long categoryId { get; set; }
            /// <summary>
            /// 商品单价
            /// </summary>
            public double? price { get; set; }
            /// <summary>
            /// 商品数量
            /// </summary>
            public int quantity { get; set; }
            /// <summary>
            /// 总价
            /// </summary>
            public double? total { get; set; }
            /// <summary>
            /// 多规格
            /// </summary>
            public List<OGroupItemSpec> newSpecs { get; set; }
            /// <summary>
            /// 多属性
            /// </summary>
            public List<OGroupItemAttribute> attributes { get; set; }
            /// <summary>
            /// 商品扩展码
            /// </summary>
            public string extendCode { get; set; }
            /// <summary>
            ///商品条形码
            /// </summary>
            public string barCode { get; set; }
            /// <summary>
            /// 商品重量(单位克)
            /// </summary>
            public double? weight { get; set; }

        }

        public class OGroupItemSpec
        {
            /// <summary>
            /// 规格名称   "杯型"
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 规格值   "大杯"
            /// </summary>
            public string value { get; set; }
        }
        public class OGroupItemAttribute
        {

            /// <summary>
            /// 属性名称   "加冰"
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 属性名称值   "少冰"
            /// </summary>
            public string value { get; set; }
        }
    } 
}