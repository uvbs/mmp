using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web
{
    public partial class OpenForm : System.Web.UI.Page
    {

        //public class Model
        //{

        //    public int id { get; set; }
        //    public string text { get; set; }
        //    public List<Model> children { get; set; }


        //}

        //public void GetModel(Model model)
        //{


        //    if (model.children != null)
        //    {
        //        foreach (var item in model.children)
        //        {
        //            GetModel(item);

        //        }
        //    }


        //}
        protected void Page_Load(object sender, EventArgs e)
        {

            ///// <summary>
            ///// 驿氪  
            ///// </summary>
            // Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();

            //var yikeUser= yiKeClient.GetBonus("", "DS0002380", "13917267301");
            // if (yikeUser != null)
            // {

            //     var a = yikeUser.Bonus; 

            // }



            //yiKeClient.BonusUpdate("DS0000257", 24, string.Format("订单交易成功获得{0}积分", "24"));
            //BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
            //bllWeixin.SendTemplateMessageNotifyComm("o3zr-tgC2F1al3gFgXenAem1K1tg", "下单成功", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}", "117", 10.00, "0", "11"));
            //string orderid=Request["orderid"];
            //Response.Write(CreateOutOrderId(orderid));
            //double timeStamp = Math.Ceiling(new BLLJIMP.BLL("").GetTimeStamp(DateTime.Now));
            //var sign = CreateSign("jikuwifi", timeStamp.ToString(), "82f03c2b26180b845369190db2188b0cb00ae43cbf");
            //string data = Request["data"];
            //List<Model> result = ZentCloud.Common.JSONHelper.JsonToModel<List<Model>>(data);
            //foreach (var item in result)
            //{

            //    GetModel(item);



            //}





            //BLLJIMP.BllKuaidi100 bllKuaidi = new BLLJIMP.BllKuaidi100();
            //string msg = "";
            //bllKuaidi.Poll("yuantong", "700086960401", out msg);
            //bllKuaidi.Poll("yuantong", "600171951442", out msg);
            //bllKuaidi.Poll("yuantong", "700086960003", out msg);
            //bllKuaidi.Poll("yuantong", "100437527428", out msg);
            //bllKuaidi.Poll("yuantong", "718959422911", out msg);
            //List<string> successList = new List<string>();
            //var list = bllKuaidi.GetList<WXMallOrderInfo>(string.Format(" Websiteowner='mixblu' And ExpressCompanyCode!='' And ExpressNumber!=''"));
            //foreach (var item in list)
            //{
            //    string msg = "";
            //    if (bllKuaidi.Poll(item.ExpressCompanyCode, item.ExpressNumber, out msg))
            //    {
            //        successList.Add(item.ExpressNumber);
            //    } 
            //}


            //var result = successList;



            //BLLJIMP.BLL bll=new BLLJIMP.BLL();
            //Open.EZRproSDK.Client client = new Open.EZRproSDK.Client();
            //WXMallProductInfo productInfo = new WXMallProductInfo();
            //productInfo.IsOnSale = "1";
            //productInfo.ProductCode = "B15902008";
            //client.UpdateProductIsOnSale(productInfo);
            //WXMallRefund refud = bll.Get<WXMallRefund>(string.Format(" OrderDetailId=6439"));
            //refud.Status = 6;
            //var result=client.UpdateRefundStatus(refud);

            //BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
            //BLLJIMP.BllKuaidi100 bll = new BLLJIMP.BllKuaidi100();
            //var orderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='mixblu' And ExpressNumber!=''"));
            //foreach (var item in orderList)
            //{
            //    string msg = "";
            //    var result = bll.Poll(item.ExpressCompanyCode, item.ExpressNumber, out msg);
            //}

            //string msg = "";
            //var orderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='mixblu' And PayMentStatus=1"));
            //foreach (var order in orderList)
            //{
            //    var orderDetail = bllMall.GetOrderDetailsList(order.OrderID);

            //    orderDetail = CalcMaxRefundAmount(order.Product_Fee - order.UseScore, orderDetail);
            //    foreach (var item in orderDetail)
            //    {
            //        var result = bllMall.Update(item,string.Format("MaxRefundAmount={0}",item.MaxRefundAmount),string.Format(" AutoId={0}",item.AutoID));
            //    }


            //}

            //var result1 = orderList;



            //Open.EZRproSDK.Client client = new Open.EZRproSDK.Client();
            //foreach (var item in orderList)
            //{
            //    try
            //    {
            //        var uploadOrderResult = client.OrderUpload(item);
            //    }
            //    catch (Exception)
            //    {


            //    }


            //}        

            //string msg = "";
            //bll.Poll("zhongtong", "718959926051", out msg);
            //BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
            ////var list = bllMall.GetList<VoteObjectInfo>(string.Format(" VoteCount<=50 And VoteID=127 And Status=1"));
            ////foreach (var item in list)
            ////{

            ////    item.VoteCount += new Random().Next(1, 5);
            ////    var result = bllMall.Update(item);

            ////    System.Threading.Thread.Sleep(100);
            // //}
            //Open.EfastSDK.Client efast = new Open.EfastSDK.Client();
            ////var skuList = bllMall.GetList<ProductSku>(string.Format(" OutBarCode is not null And OutBarCode!=''"));
            //List<string> skuList = new List<string>();
            //skuList.Add("AD490278700S");
            //skuList.Add("AD490278700M");
            //skuList.Add("AD490278700L");
            //skuList.Add("AD49027870XL");
            //skuList.Add("T1480133800S");
            //skuList.Add("T1480133800M");
            //skuList.Add("T1480133800L");
            //skuList.Add("B158022017000M");
            //skuList.Add("B158022018000S");
            //skuList.Add("B158022018000M");
            //skuList.Add("B158022028000S");
            //skuList.Add("B158022028000M");
            //skuList.Add("B158022047000S");
            //skuList.Add("B158022047000M");
            //skuList.Add("B158022047000L");
            //skuList.Add("B158022077000S");
            //skuList.Add("B158022077000M");
            //skuList.Add("B158240049300S");
            //skuList.Add("B158240049300M");
            //skuList.Add("B158240049300L");
            //skuList.Add("B158240087000M");
            //skuList.Add("B158240087000L");
            //skuList.Add("B158270010800S");
            //skuList.Add("B158270010800M");
            //skuList.Add("B158270010800L");
            //skuList.Add("B158270119200S");
            //skuList.Add("B158270119200M");
            //skuList.Add("B158270119200L");
            //skuList.Add("B158270218000S");
            //skuList.Add("B158270218000M");
            //skuList.Add("B158270218000L");
            //skuList.Add("B158280050200S");
            //skuList.Add("B158280050200M");
            //skuList.Add("B158280050200L");
            //skuList.Add("B158280057000S");
            //skuList.Add("B158280057000M");
            //skuList.Add("B158280057000L");
            //skuList.Add("B1582900136026");
            //skuList.Add("B1582900136027");
            //skuList.Add("B1582900136028");
            //skuList.Add("B1582900136029");
            //skuList.Add("B1582900235026");
            //skuList.Add("B1582900235027");
            //skuList.Add("B1582900235028");
            //skuList.Add("B1582900235029");
            //List<string> skuNoExt = new List<string>();
            //  List<ProductSku> listSuccess = new List<ProductSku>();
            //  string shopIdStr = System.Configuration.ConfigurationManager.AppSettings["eFastShopId"];
            //  int shopId = int.Parse(shopIdStr);
            //  foreach (var skusn in skuList)
            //  {
            //      if (!string.IsNullOrEmpty(shopIdStr))
            //      {

            //          var eFastSku = efast.GetSkuStock(shopId, skusn);
            //          if (eFastSku != null)
            //          {
            //             // ProductSku skuInfo = bllMall.GetProductSkuBySkuSn(skuInfo.OutBarCode);
            //              //if (skuInfo != null)
            //              //{
            //                  //skuInfo.Stock = eFastSku.sl;
            //                  //if (bllMall.Update(skuInfo))//更新内部库存)
            //                  //{
            //                  //    listSuccess.Add(skuInfo);
            //                  //}
            //              //}



            //          }
            //          else
            //          {
            //              //skuInfo.Stock = 0;
            //              //bllMall.Update(skuInfo);
            //              //skuNoExt.Add(skuInfo.SkuId.ToString());


            //          }

            //      }

            //  }
            //  foreach (var item in listSuccess)
            //  {
            //      Response.Write("success" + item.OutBarCode);
            //      Response.Write("</br>");

            //  }

            //  Response.Write("----");
            //  foreach (var item in skuNoExt)
            //  {
            //      Response.Write("skuId:"+item);
            //      Response.Write("</br>");
            //  }

            //BLLJIMP.BLLWeixin bll = new BLLJIMP.BLLWeixin();
            //ZentCloud.BLLJIMP.BLLWeixin.TMTaskNotification template = new BLLJIMP.BLLWeixin.TMTaskNotification();
            //ZentCloud.BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
            //List<string> userIdList = new List<string>();
            //List<string> userIdFail = new List<string>();
            //userIdList.Add("WXUser20157281747364DZRE");//尹
            //userIdList.Add("WXUser201572916325024N7L");//翟
            //userIdList.Add("WXUser2015729191205WZDX2");//刘
            //userIdList.Add("WXUser2015728185644CXFZ5");//sys


            //#region 初赛批次
            ////////第一批
            ////userIdList.Add("WXUser201573200913LO4EJ   ");
            ////userIdList.Add("WXUser20157317543293DNS   ");
            ////userIdList.Add("WXUser2015771615084Q6IF   ");
            ////userIdList.Add("WXUser2015741734295WKUX   ");
            ////userIdList.Add("WXUser20157416440547U48   ");
            ////userIdList.Add("WXUser201579174601VNE82   ");
            ////userIdList.Add("WXUser201572110337FGZPM   ");
            ////userIdList.Add("WXUser201573153624X1ZSO   ");
            ////userIdList.Add("WXUser201575175900UO2IE   ");
            ////userIdList.Add("WXUser2015729114640J8JPG  ");
            ////userIdList.Add("WXUser201573114805Y0BEV   ");
            ////userIdList.Add("WXUser201572162225B8PP8   ");
            ////userIdList.Add("WXUser201573140009L4FDA   ");
            ////userIdList.Add("WXUser2015741339456VY0L   ");
            ////userIdList.Add("WXUser20157591716E02WL    ");
            ////userIdList.Add("WXUser201579180448QXLNM   ");
            ////userIdList.Add("WXUser201574164359AYC83   ");
            ////userIdList.Add("WXUser201574114412I4VJJ   ");
            ////userIdList.Add("WXUser2015729184727B4120  ");
            ////userIdList.Add("WXUser201577165005CLYOX   ");
            ////userIdList.Add("WXUser201574180749M1F75   ");
            ////userIdList.Add("WXUser201573125529FQ1LA   ");
            ////userIdList.Add("WXUser201577171028RDGXG   ");
            ////userIdList.Add("WXUser201572170841FUJ8F   ");
            ////userIdList.Add("WXUser201576195040FRDJ7   ");
            ////userIdList.Add("WXUser2015731754022AMGV   ");
            ////userIdList.Add("WXUser201575154549E2GKD   ");
            ////userIdList.Add("WXUser201572122648TISPB   ");
            ////userIdList.Add("WXUser20157283002NQK6Y    ");
            ////userIdList.Add("WXUser201577154244WEFKS   ");
            ////userIdList.Add("WXUser20157290916B36IU    ");
            ////userIdList.Add("WXUser201576133111JODDF   ");
            ////userIdList.Add("WXUser2015749131146E2R    ");
            ////userIdList.Add("WXUser20157616513707S56   ");
            ////userIdList.Add("WXUser201574104438NJWQZ   ");
            ////userIdList.Add("WXUser2015731352514JGO4   ");
            ////userIdList.Add("WXUser20157312075882UZM   ");
            //////第一批



            //////////第二批
            ////userIdList.Add("WXUser2015729114616KBEMB ");
            ////userIdList.Add("WXUser20157313461438CYZ  ");
            ////userIdList.Add("WXUser20157619500286OXA  ");
            ////userIdList.Add("WXUser2015729162405YWKCT ");
            ////userIdList.Add("WXUser20157495753K0JO5   ");
            ////userIdList.Add("WXUser201573124528XDQXQ  ");
            ////userIdList.Add("WXUser201578144553Q14BO  ");
            ////userIdList.Add("WXUser201573135534SFKJJ  ");
            ////userIdList.Add("WXUser201572163409931ZY  ");
            ////userIdList.Add("WXUser201573152650IR0BM  ");
            ////userIdList.Add("WXUser2015731647267M92L  ");
            ////userIdList.Add("WXUser2015751725529KDSC  ");
            ////userIdList.Add("WXUser201574900527C1HQ   ");
            ////userIdList.Add("WXUser2015759083348DJ2   ");
            ////userIdList.Add("WXUser20157317014160RJC  ");
            ////userIdList.Add("WXUser201576161240XBJ98  ");
            ////userIdList.Add("WXUser201573183458QSIEZ  ");
            ////userIdList.Add("WXUser201573200403XHT5W  ");
            ////userIdList.Add("WXUser201573114730MWP84  ");
            ////userIdList.Add("WXUser20157517590227XHK  ");
            ////userIdList.Add("WXUser201575175916OZ8RH  ");
            ////userIdList.Add("WXUser201577170731ZEO50  ");
            ////userIdList.Add("WXUser20157217084778OZH  ");
            ////userIdList.Add("WXUser201572221419XBMQW  ");
            ////userIdList.Add("WXUser20157717371685T3Y  ");
            ////userIdList.Add("WXUser20157693057QUFK7   ");
            ////userIdList.Add("WXUser2015761044491SSQT  ");
            ////userIdList.Add("WXUser2015731734379IH1V  ");
            ////userIdList.Add("WXUser2015741807137DKVK  ");
            ////userIdList.Add("WXUser201572183504KEAE1  ");
            ////userIdList.Add("WXUser201573115151FKF02  ");
            ////userIdList.Add("WXUser2015721205168T30Y  ");
            ////userIdList.Add("WXUser201573093748D9D7Z  ");
            ////userIdList.Add("WXUser20157482823TPQSC   ");
            ////userIdList.Add("WXUser201575213139FDTDO  ");
            ////userIdList.Add("WXUser201582711231P0R6   ");
            ////userIdList.Add("WXUser201573101351HVXFY  ");
            ////userIdList.Add("WXUser201573135525OXUFS  ");
            ////userIdList.Add("WXUser2015791648046HZJ1  ");
            ////userIdList.Add("WXUser2015729191205WZDX2 ");
            //////////第二批 
            //#endregion

            /////复赛
            //userIdList.Add("WXUser201572110337FGZPM ");
            //userIdList.Add("WXUser201582711231P0R6  ");
            //userIdList.Add("WXUser201572170841FUJ8F ");
            //userIdList.Add("WXUser201572163409931ZY ");
            //userIdList.Add("WXUser201573115151FKF02 ");
            //userIdList.Add("WXUser2015731352514JGO4 ");
            //userIdList.Add("WXUser2015731647267M92L ");
            //userIdList.Add("WXUser201573183458QSIEZ ");
            //userIdList.Add("WXUser20157482823TPQSC  ");
            //userIdList.Add("WXUser201574900527C1HQ  ");
            //userIdList.Add("WXUser201574104438NJWQZ ");
            //userIdList.Add("WXUser201575175916OZ8RH ");
            //userIdList.Add("WXUser201576133111JODDF ");
            //userIdList.Add("WXUser201577170731ZEO50 ");
            //userIdList.Add("WXUser201577154244WEFKS ");
            //userIdList.Add("WXUser201577165005CLYOX ");
            //userIdList.Add("WXUser201579174601VNE82 ");
            //userIdList.Add("WXUser2015729162405YWKCT");

            //int count = 0;
            //foreach (var item in userIdList)
            //{
            //    ZentCloud.BLLJIMP.Model.UserInfo userInfo = bllUser.GetUserInfo(item.TrimEnd(), "haima");
            //    //
            //    var uName = userInfo.TrueName;
            //    template.TemplateId = "jiAgcfLwmwObW9s_diNaR7eiUfc2wtXjNFCV-485bX0";
            //    template.Url = "http://mp.weixin.qq.com/s?__biz=MzIzODAwNjMzMQ==&mid=207662235&idx=1&sn=22d37935ffdfe1dca41f261d513db8d6#rd"; //第一批
            //    template.First = string.Format("您好，成都车展挑战赛已进入倒计时阶段。");
            //    //template.lineName = "您的出发城市-四川成都往返";
            //    //template.date = "点击查看";
            //    //template.orderNum = "---";
            //    //template.transactionNum = "---";
            //    template.Keyword1 = "----";
            //    template.Keyword2 = "进入行李准备阶段";
            //    template.Remark = string.Format("成都车展挑战赛即将开始，请随时关注微信平台发送的点对点信息。《建议个人携带物品》，请点击查看。");
            //    var result = bll.SendTemplateMessage(bll.GetAccessToken(), userInfo.WXOpenId, template);
            //    if (result.Contains("ok"))
            //    {
            //        count++;
            //    }
            //    else
            //    {
            //        userIdFail.Add(item.TrimEnd());
            //    }

            //}

            //Response.Write("successcount" + count);
            //Response.Write("<br/>faillog:<br/>");
            //foreach (var item in userIdFail)
            //{
            //    Response.Write(item);
            //    Response.Write("<br/>");
            //}


        }

        ///// <summary>
        ///// 生成外部订单号
        ///// </summary>
        ///// <returns></returns>
        //private string CreateOutOrderId(string orderId)
        //{

        //    if (orderId.Length > 10)
        //    {
        //        orderId = orderId.Substring(0, 10);
        //    }
        //    return string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddhhmmss"), orderId.PadLeft(10, '0'));

        //}

        ///// <summary>
        ///// 生成签名
        ///// </summary>
        ///// <param name="appId"></param>
        ///// <param name="timeStamp"></param>
        ///// <param name="appkey"></param>
        ///// <returns></returns>
        //public string CreateSign(string appId, string timeStamp, string appkey)
        //{
        //    string sign = ZentCloud.Common.SHA1.SHA1_Encrypt(string.Format("appid={0}&appkey={1}&timestamp={2}", appId, appkey, timeStamp)).ToUpper();
        //    return sign;

        //}


        protected void Button1_Click(object sender, EventArgs e)
        {BLLMall bllMall=new BLLMall();
            /// <summary>
            /// Efast
            /// </summary>
            Open.EfastSDK.Client efastClient = new Open.EfastSDK.Client();
            ProductSku skuInfo = bllMall.Get<ProductSku>(string.Format("SkuSN='{0}'", txtOrderId.Text));
            if (skuInfo==null)
            {
               txtOrderId.Text=string.Format("条码不存在");
               return;
            }
            var eFastSku = efastClient.GetSkuStock(9, skuInfo.OutBarCode);
            if (eFastSku != null)
            {

                if (eFastSku.sl != skuInfo.Stock)
                {
                    skuInfo.Stock = eFastSku.sl;

                    if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format("update ZCJ_ProductSku set Stock={0} where SkuId={1}", skuInfo.Stock, skuInfo.SkuId)) >= 0)
                    {
                        txtOrderId.Text=string.Format("同步成功,条码:{0}库存:{1}", skuInfo.OutBarCode, skuInfo.Stock);
                    }

                }
                else
                {
                   txtOrderId.Text=string.Format("库存一致,跳过 条码:{0}库存:{1}", skuInfo.OutBarCode, skuInfo.Stock);
                }

            }
            else
            {
               
               txtOrderId.Text="未查到";
               
            }
        }



        ///// <summary>
        ///// 计算最大退款金额
        ///// </summary>
        ///// <param name="totalAmount">总金额</param>
        ///// <param name="detailList">订单详情</param>
        ///// <returns></returns>
        //private List<WXMallOrderDetailsInfo> CalcMaxRefundAmount(decimal amount, List<WXMallOrderDetailsInfo> detailList)
        //{

        //    try
        //    {
        //        foreach (var orderDetail in detailList)
        //        {

        //            decimal rate = ((decimal)orderDetail.OrderPrice * orderDetail.TotalCount) / ((decimal)detailList.Sum(p => p.OrderPrice * p.TotalCount));//此sku所占的比例
        //            orderDetail.MaxRefundAmount = (amount * rate);

        //        }


        //    }
        //    catch (Exception)
        //    {


        //    }
        //    return detailList;
        //}


        ///// <summary>
        ///// 计算均摊价
        ///// </summary>
        ///// <param name="totalAmount">总金额</param>
        ///// <param name="detailList">订单详情</param>
        ///// <returns></returns>
        //private List<WXMallOrderDetailsInfo> CalcPaymentFt(decimal totalAmount, List<WXMallOrderDetailsInfo> detailList)
        //{

        //    try
        //    {
        //        //foreach (var orderDetail in detailList)
        //        //{

        //        //    decimal rate =((decimal)orderDetail.OrderPrice*orderDetail.TotalCount)/((decimal)detailList.Sum(p=>p.OrderPrice*p.TotalCount));//此sku所占的比例
        //        //    orderDetail.PaymentFt = (totalAmount * rate)/orderDetail.TotalCount;

        //        //}
        //        List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//不重复的商品列表
        //        foreach (var orderDetail in detailList)
        //        {
        //            var productInfo = bllMall.GetProduct(orderDetail.PID);
        //            if (productList.Where(p => p.PID == productInfo.PID).Count() == 0)
        //            {
        //                productList.Add(productInfo);
        //            }
        //        }
        //        foreach (var orderDetail in detailList)
        //        {
        //            var productQuotePrice = productList.Single(p => p.PID == orderDetail.PID).PreviousPrice;
        //            decimal rate = (decimal)productQuotePrice / productList.Sum(p => p.PreviousPrice);//此sku所占的比例
        //            orderDetail.PaymentFt = (totalAmount * rate);
        //        }

        //    }
        //    catch (Exception)
        //    {


        //    }
        //    return detailList;
        //}


    }
}
