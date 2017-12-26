using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using System.Xml;
using Payment.WeiXin;
using System.IO;
using System.Text;
using ZCJson.Linq;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    /// <summary>
    /// �̳�΢��֧��֪ͨ
    /// </summary>
    public partial class NotifyV2 : System.Web.UI.Page
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public WXMallOrderInfo orderInfo = new WXMallOrderInfo();
        /// <summary>
        /// �̳�BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// ֧��BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// ͨ�ù�ϵBLL
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// �û�BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// ΢��BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeiXin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// �˵�Ȩ��BLL
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// Efast BLL
        /// </summary>
        BLLJIMP.BLLEfast bllEfast = new BLLJIMP.BLLEfast();
        /// <summary>
        /// ���BLL
        /// </summary>
        Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// ����BLL
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// �̳Ƿ���BLL
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCard = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// �ɹ�xml
        /// </summary>
        private string successXml = "<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>";
        /// <summary>
        /// ʧ��xml
        /// </summary>
        private string failXml = "<xml><return_code><![CDATA[FAIL]]></return_code></xml>";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Tolog("����֧���ص�");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Request.InputStream);
                xmlDoc.Save(string.Format("C:\\WXPay\\Notify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//д����־
                //ȫ������
                Dictionary<string, string> parametersAll = new Dictionary<string, string>();
                foreach (XmlElement item in xmlDoc.DocumentElement.ChildNodes)
                {
                    string key = item.Name;
                    string value = item.InnerText;
                    if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(value)))
                    {
                        parametersAll.Add(key, value);

                    }


                }
                parametersAll = (from entry in parametersAll
                                 orderby entry.Key ascending
                                 select entry).ToDictionary(pair => pair.Key, pair => pair.Value);//ȫ����������
                orderInfo = bllMall.GetOrderInfo(parametersAll["out_trade_no"]);
                PayConfig payConfig = bllPay.GetPayConfig();
                if (!bllPay.VerifySignatureWx(parametersAll, payConfig.WXPartnerKey))//��֤ǩ��
                {
                    Tolog("��֤ǩ������");
                    Response.Write(failXml);
                    return;
                }
                if (orderInfo == null)
                {
                    Tolog("����δ�ҵ�");
                    Response.Write(failXml);
                    return;
                }
                if (orderInfo.PaymentStatus.Equals(1))
                {
                    //Tolog("��֧��");
                    Response.Write(successXml);
                    return;
                }
                orderInfo.PaymentType = 2;

                //���¶���״̬
                WXMallProductInfo tProductInfo = new WXMallProductInfo();
                if (parametersAll["return_code"].Equals("SUCCESS") && parametersAll["result_code"].Equals("SUCCESS"))//���׳ɹ�
                {
                    UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, bllUser.WebsiteOwner);//�µ��û���Ϣ
                    string hasOrderIDs = "";
                    int maxCount = 1;
                    //Tolog("׼�������¶���״̬");
                    if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                    {
                        //Tolog("Ԥ������");
                        #region ԤԼ�����޸�״̬
                        orderInfo.PaymentStatus = 1;
                        orderInfo.PayTime = DateTime.Now;
                        orderInfo.Status = "ԤԼ�ɹ�";

                        #region ����Ƿ���ԤԼ�ɹ��Ķ���
                        List<WXMallOrderDetailsInfo> tDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID, null, orderInfo.ArticleCategoryType, null, null);
                        List<WXMallOrderDetailsInfo> oDetailList = bllMall.GetOrderDetailsList(null, tDetailList[0].PID, orderInfo.ArticleCategoryType, tDetailList.Min(p => p.StartDate), tDetailList.Max(p => p.EndDate));
                        tProductInfo = bllMall.GetByKey<WXMallProductInfo>("PID", tDetailList[0].PID);
                        maxCount = tProductInfo.Stock;
                        List<string> hasOrderIDList = new List<string>();
                        foreach (var item in tDetailList)
                        {
                            List<WXMallOrderDetailsInfo> hasOrderDetailList = oDetailList.Where(p => !((item.StartDate >= p.EndDate && item.EndDate > p.EndDate) || (item.StartDate < p.StartDate && item.EndDate <= p.StartDate))).ToList();
                            if (hasOrderDetailList.Count >= maxCount)
                            {
                                hasOrderIDList.AddRange(hasOrderDetailList.Select(p => p.OrderID).Distinct());
                            }
                        }
                        hasOrderIDList = hasOrderIDList.Where(p => !p.Contains(orderInfo.OrderID)).ToList();
                        if (hasOrderIDList.Count > 0)
                        {
                            hasOrderIDList = hasOrderIDList.Distinct().ToList();
                            hasOrderIDs = MyStringHelper.ListToStr(hasOrderIDList, "'", ",");
                        }
                        #endregion ����Ƿ���ԤԼ�ɹ��Ķ���

                        #endregion ԤԼ�����޸�״̬
                    }
                    else
                    {
                        //Tolog("��ͨ����");
                        #region ԭ�����޸�״̬
                        orderInfo.PaymentStatus = 1;
                        orderInfo.Status = "������";
                        orderInfo.PayTime = DateTime.Now;
                        if (orderInfo.DeliveryType==1)
                        {
                            orderInfo.Status = "������";
                        }
                        Tolog("����״̬start");
                        //if (bllMall.GetWebsiteInfoModelFromDataBase().IsDistributionMall.Equals(1))
                        //{
                        orderInfo.GroupBuyStatus = "0";
                        orderInfo.DistributionStatus = 1;

                        //if (orderInfo.IsMain==1)
                        //{
                        //    bllMall.Update(orderInfo,string.Format(" DistributionStatus=1"),string.Format("ParentOrderId='{0}'",orderInfo.OrderID));
                        //}

                    

                        #region �����
                        if (orderInfo.OrderType == 4)
                        {
                            ActivityDataInfo data = bllMall.Get<ActivityDataInfo>(string.Format(" OrderId='{0}'", orderInfo.OrderID));
                            if (data != null)
                            {
                                bllMall.Update(data, string.Format(" PaymentStatus=1"), string.Format("  OrderId='{0}'", orderInfo.OrderID));
                            }
                        }
                        #endregion


                        bllMall.Update(orderInfo, string.Format("PaymentStatus=1,Status='������',PayTime=GETDATE(),DistributionStatus=1"), string.Format("ParentOrderId='{0}'", orderInfo.OrderID));


                        //}
                        #endregion ԭ�����޸�״̬

                        try
                        {
                            //���ý�����ˮ��
                            orderInfo.PayTranNo = parametersAll["transaction_id"];
                        }
                        catch (Exception ex)
                        {
                            Tolog("���ý�����ˮ��ʧ�ܣ�" + ex.Message);
                        }

                    }
                    bool result = false;

                    if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                    {
                        if (string.IsNullOrWhiteSpace(hasOrderIDs)) hasOrderIDs = "'0'";
                        result = bllMall.Update(new WXMallOrderInfo(),
                            string.Format("PaymentStatus={0},PayTime=GetDate(),Status='{1}'", 1, "ԤԼ�ɹ�"),
                            string.Format("OrderID='{0}' and WebsiteOwner='{4}' AND (select count(1) from [ZCJ_WXMallOrderInfo] where Status='{3}' and WebsiteOwner='{4}' and  OrderID IN({1}))<{2}",
                                orderInfo.OrderID, hasOrderIDs, maxCount, "ԤԼ�ɹ�", bllMall.WebsiteOwner)
                            ) > 0;
                        if (result)
                        {
                            // #region ���׳ɹ��ӻ���
                            //���ӻ��� ���۾۲���Ҫ��
                            //if (orderInfo.TotalAmount > 0)
                            //{
                            //    ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                            //    int addScore = 0;
                            //    if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
                            //    {
                            //        addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                            //    }
                            //    if (addScore > 0)
                            //    {
                            //        if (bllUser.Update(new UserInfo(), 
                            //            string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore),
                            //            string.Format(" UserID='{0}'", orderInfo.OrderUserID)) > 0)
                            //        {
                            //            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            //            scoreRecord.AddTime = DateTime.Now;
                            //            scoreRecord.Score = addScore;
                            //            scoreRecord.ScoreType = "OrderSuccess";
                            //            scoreRecord.UserID = orderInfo.OrderUserID;
                            //            scoreRecord.AddNote = "ԤԼ-���׳ɹ���û���";
                            //            bllMall.Add(scoreRecord);
                            //        }
                            //    }
                            //}
                            // #endregion

                            #region �޸�����ԤԼ����ΪԤԼʧ�� ��������

                            if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType) && !string.IsNullOrWhiteSpace(hasOrderIDs))
                            {
                                int tempCount = 0;
                                List<WXMallOrderInfo> tempList = bllMall.GetOrderList(0, 1, "", out tempCount, "ԤԼ�ɹ�", null, null, null,
                                        null, null, null, null, null, null, null, orderInfo.ArticleCategoryType, hasOrderIDs);
                                tempCount = tempCount + 1; //���ϵ�ǰ����������
                                if (tempCount >= maxCount)
                                {
                                    tempList = bllMall.GetColOrderListInStatus("'������','�����'", hasOrderIDs, "OrderID,OrderUserID,UseScore", bllMall.WebsiteOwner);
                                    if (tempList.Count > 0)
                                    {
                                        string stopOrderIds = MyStringHelper.ListToStr(tempList.Select(p => p.OrderID).ToList(), "'", ",");
                                        tempList = tempList.Where(p => p.UseScore > 0).ToList();
                                        foreach (var item in tempList)
                                        {
                                            orderUserInfo.TotalScore += item.UseScore;
                                            if (bllUser.Update(new UserInfo(), string.Format(" TotalScore+={0}", item.UseScore),
                                                string.Format(" UserID='{0}'", item.OrderUserID)) > 0)
                                            {
                                                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                                                scoreRecord.AddTime = DateTime.Now;
                                                scoreRecord.Score = item.UseScore;
                                                scoreRecord.TotalScore = orderUserInfo.TotalScore;
                                                scoreRecord.ScoreType = "OrderCancel";
                                                scoreRecord.UserID = item.OrderUserID;
                                                scoreRecord.RelationID = item.OrderID;
                                                scoreRecord.AddNote = "ԤԼ-����ʧ�ܷ�������";
                                                scoreRecord.WebSiteOwner = item.WebsiteOwner;
                                                bllMall.Add(scoreRecord);
                                            }
                                        }
                                        bllMall.Update(new WXMallOrderInfo(),
                                            string.Format("Status='{0}'", "ԤԼʧ��"),
                                            string.Format("OrderID In ({0}) and WebsiteOwner='{1}'", stopOrderIds, bllMall.WebsiteOwner));
                                    }
                                }
                                //Tolog("�����޸�����ԤԼΪԤԼʧ��");
                            }
                            #endregion

                        }
                    }
                    else
                    {
                        result = bllMall.Update(orderInfo);
                    }
                    if (result)
                    {
                        #region ƴ�Ŷ���
                        if (orderInfo.OrderType == 2)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId))
                                {
                                    var parentOrderInfo = bllMall.GetOrderInfo(orderInfo.GroupBuyParentOrderId);

                                    if (parentOrderInfo.Ex10 == "1")
                                    {
                                        if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' ", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                        {
                                            bllMall.Update(new WXMallOrderInfo(), string.Format("Status='��ȡ��'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                            parentOrderInfo.GroupBuyStatus = "1";
                                            bllMall.Update(parentOrderInfo);

                                        }
                                    }
                                    else
                                    {
                                        if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' Or OrderId='{0}'", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                        {
                                            bllMall.Update(new WXMallOrderInfo(), string.Format("Status='��ȡ��'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                            parentOrderInfo.GroupBuyStatus = "1";
                                            bllMall.Update(parentOrderInfo);

                                        }
                                    }

                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion
                        Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);

                        //Tolog("����״̬true");

                        #region Efastͬ��
                        //�ж���ǰվ���Ƿ���Ҫͬ������봺�efast
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                        {
                            try
                            {
                                Tolog("��ʼͬ��efast");

                                string outOrderId = string.Empty, msg = string.Empty;
                                var syncResult = bllEfast.CreateOrder(orderInfo.OrderID, out outOrderId, out msg);
                                if (syncResult)
                                {
                                    orderInfo.OutOrderId = outOrderId;
                                    bllMall.Update(orderInfo);
                                }
                                Tolog(string.Format("efast����ͬ�����:{0},�����ţ�{1}����ʾ��Ϣ��{2}", syncResult, outOrderId, msg));

                            }
                            catch (Exception ex)
                            {
                                Tolog("efast����ͬ���쳣��" + ex.Message);

                            }
                        }
                        #endregion

                        #region ���ͬ��
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                        {
                            try
                            {

                                Tolog("��ʼͬ�����");
                                //ͬ���ɹ����������

                                //UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                                //if ((!string.IsNullOrEmpty(orderUserInfo.Ex1)) && (!string.IsNullOrEmpty(orderUserInfo.Ex2)) && (!string.IsNullOrEmpty(orderUserInfo.Phone)))
                                //{
                                //    client.BonusUpdate(orderUserInfo.Ex2, -(orderInfo.UseScore), "�̳��µ�ʹ�û���" + orderInfo.UseScore);

                                //}
                                var uploadOrderResult = yikeClient.OrderUpload(orderInfo);

                                Tolog(string.Format("��봶���ͬ�������{0}", Common.JSONHelper.ObjectToJson(uploadOrderResult)));
                            }
                            catch (Exception ex)
                            {
                                Tolog("��봶���ͬ���쳣��" + ex.Message);

                            }
                        }
                        #endregion

                        #region ����ӻ���
                        try
                        {
                            bllUser.AddUserScoreDetail(orderInfo.OrderUserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.OrderPay), bllMall.WebsiteOwner, null, null);
                        }
                        catch (Exception)
                        { }

                        #endregion


                        #region ��Ϣ֪ͨ
                        if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                        {
                            try
                            {
                                var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;
                                string remark = "";
                                if (!string.IsNullOrEmpty(orderInfo.OrderMemo))
                                {
                                    remark = string.Format("�ͻ�����:{0}", orderInfo.OrderMemo);
                                }
                                bllWeiXin.SendTemplateMessageToKefu("���µĶ���", string.Format("������:{0}\\n�������:{1}Ԫ\\n�ջ���:{2}\\n�绰:{3}\\n��Ʒ:{4}\\n{5}", orderInfo.OrderID, Math.Round(orderInfo.TotalAmount, 2), orderInfo.Consignee, orderInfo.Phone, productName, remark));
                                if (orderInfo.OrderType != 4)//���ѵĻ������Ϣ
                                {
                                    if (orderInfo.WebsiteOwner != "jikuwifi")
                                    {
                                        string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", Request.Url.Host, orderInfo.OrderID);
                                        if (orderInfo.IsMain == 1)
                                        {
                                            url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderList#/orderList", Request.Url.Host);
                                        }
                                        bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, "�����ѳɹ�֧�������ǽ����췢�����뱣���ֻ���ͨ�ȴ������ʹ", string.Format("������:{0}\\n�������:{1}Ԫ\\n�ջ���:{2}\\n�绰:{3}\\n��Ʒ:{4}...\\n�鿴����", orderInfo.OrderID, Math.Round(orderInfo.TotalAmount, 2), orderInfo.Consignee, orderInfo.Phone, productName), url);

                                    }

                                }

                            }
                            catch (Exception)
                            {

                            }
                        }
                        else
                        {
                            try
                            {
                                bllWeiXin.SendTemplateMessageToKefu(orderInfo.Status, string.Format("ԤԼ:{2}\\n������:{0}\\n�������:{1}Ԫ\\nԤԼ��:{3}\\nԤԼ���ֻ�:{4}", orderInfo.OrderID, orderInfo.TotalAmount, tProductInfo.PName, orderUserInfo.TrueName, orderUserInfo.Phone));
                                bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, orderInfo.Status, string.Format("ԤԼ:{2}\\n������:{0}\\n�������:{1}Ԫ", orderInfo.OrderID, orderInfo.TotalAmount, tProductInfo.PName));
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion
                        WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));
                        #region �������
                        try
                        {
                            if (bllMenuPermission.CheckUserAndPmsKey(websiteInfo.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution, websiteInfo.WebsiteOwner))
                            {

                                if (string.IsNullOrWhiteSpace(orderUserInfo.DistributionOwner))
                                {

                                    if (websiteInfo.DistributionRelationBuildMallOrder == 1)
                                    {
                                        orderUserInfo.DistributionOwner = orderInfo.WebsiteOwner;
                                        bllMall.Update(orderUserInfo);

                                    }

                                }
                                bllDis.AutoUpdateLevel(orderInfo);
                                bllDis.TransfersEstimate(orderInfo);
                                bllDis.SendMessageToUser(orderInfo);
                                bllDis.UpdateDistributionSaleAmountUp(orderInfo);

                            }
                        }
                        catch (Exception ex)
                        {
                            Tolog("���÷���Ա�쳣��" + ex.Message + " �û�id��" + orderUserInfo.UserID);
                        }
                        #endregion


                        #region ��Ρ֪ͨ
                        try
                        {
                            if (websiteInfo.IsUnionHongware == 1)
                            {
                                hongWareClient.OrderNotice(orderUserInfo.WXOpenId, orderInfo.OrderID);

                            }
                        }
                        catch (Exception)
                        {


                        }

                        #endregion

                        bllCard.Give(orderInfo.TotalAmount, orderUserInfo);


                        string v1ProductId = Common.ConfigHelper.GetConfigString("YGBV1ProductId");
                        string v2ProductId = Common.ConfigHelper.GetConfigString("YGBV2ProductId");
                        string v1CouponId = Common.ConfigHelper.GetConfigString("YGBV1CouponId");
                        string v2CouponId = Common.ConfigHelper.GetConfigString("YGBV2CouponId");

                        List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                        foreach (var item in orderDetailList)
                        {
                            item.IsComplete = 1;
                            bllMall.Update(item);

                            #region ����ָ����Ʒ����ָ�����Ż�ȯ
                            if (!string.IsNullOrEmpty(v1ProductId) && !string.IsNullOrEmpty(v1CouponId)&&item.PID == v1ProductId)
                            {
                                bllCard.SendCardCouponsByCurrUserInfo(orderUserInfo, v1CouponId);
                            }

                            if (!string.IsNullOrEmpty(v2ProductId) && !string.IsNullOrEmpty(v2CouponId) && item.PID == v2ProductId)
                            {
                                bllCard.SendCardCouponsByCurrUserInfo(orderUserInfo, v2CouponId);
                            }
                            #endregion
                        }

                        //��������
                        bllMall.UpdateProductSaleCount(orderInfo);
                        Response.Write(successXml);
                        return;
                    }
                    else
                    {
                        Tolog("����״̬false");
                        Response.Write(failXml);
                        return;
                    }
                }
                Tolog("������Ϣ����");
                Response.Write(failXml);



            }
            catch (Exception ex)
            {
                Tolog("�����ˣ�" + ex.Message);
                Response.Write(failXml);

            }
        }
        /// <summary>
        /// ��־
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
                }
            }
            catch { }
        }
    }
}