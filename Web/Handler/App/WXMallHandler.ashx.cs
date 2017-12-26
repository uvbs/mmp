using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// 接收提交订单的商品ID和数量
    /// </summary>
    public class SubmitWxMallOrderPidsModel
    {
        //用来接收页面传过来的json实体{"pids":[["130568",3],["130567",1]]}
        private List<List<object>> _pids = new List<List<object>>();
        public List<List<object>> Pids
        {
            get
            {
                return _pids;
            }
            set
            {
                _pids = value;
            }
        }
    }

    /// <summary>
    /// 微商城 手机端-处理程序
    /// </summary>
    public class WXMallHandler : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 当前访问用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin("");
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 分销BLL
        /// </summary>
        BLLDistribution bllDis = new BLLDistribution();
        /// <summary>
        /// 通用关系BLL
        /// </summary>
        BLLCommRelation bllCommRela = new BLLCommRelation();
        /// <summary>
        /// 积分BLL
        /// </summary>
        BllScore bllScore = new BllScore();
        /// <summary>
        ///活动
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        /// <summary>
        /// 储值卡
        /// </summary>
        BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bllUser.IsLogin)
                {

                    currentUserInfo = bllMall.GetCurrentUserInfo();
                }
                string action = context.Request["Action"];
                switch (action)
                {

                    #region 商品模块
                    case "QueryProductsObjList":
                        result = QueryProductsObjList(context);
                        break;

                    case "GetProductObj":
                        result = GetProductObj(context);
                        break;
                    case "SubmitWxMallOrder":
                        result = SubmitWxMallOrder(context);
                        break;
                    case "SubmitWxMallOrderV1":
                        result = SubmitWxMallOrderV1(context);
                        break;
                    case "SubmitWxMallOrderBXGT":
                        result = SubmitWxMallOrderBXGT(context);
                        break;
                    case "SubmitWxMallScoreOrder":
                        result = SubmitWxMallScoreOrder(context);
                        break;
                    case "SubmitWxMallOrderTuao"://提交订单土澳网 
                        result = SubmitWxMallOrderTuao(context);
                        break;
                    case "UpdateProductIPPV":
                        result = UpdateProductIPPV(context);
                        break;

                    case "QueryScoreProductsObjList":
                        result = QueryScoreProductsObjList(context);
                        break;
                    case "QueryScoreRecord":
                        result = QueryScoreRecord(context);
                        break;

                    #endregion

                    #region 收货地址

                    case "QueryWXConsigneeAddress":
                        result = QueryWXConsigneeAddress(context);
                        break;
                    case "AddWXConsigneeAddress":
                        result = AddWXConsigneeAddress(context);
                        break;

                    case "EditWXConsigneeAddress":
                        result = EditWXConsigneeAddress(context);
                        break;

                    case "DeleteWXConsigneeAddress":
                        result = DeleteWXConsigneeAddress(context);
                        break;

                    #endregion

                    case "QueryWXMallStoresByDoMain"://查询域名门店地址
                        result = QueryWXMallStoresByDoMain(context);
                        break;


                    case "CancelOrder"://取消订单
                        result = CancelOrder(context);
                        break;
                    case "UpdateOrderComplete"://确定收货
                        result = UpdateOrderComplete(context);
                        break;
                    case "CancelScoreOrder"://取消积分订单
                        result = CancelScoreOrder(context);
                        break;

                    case "GiveScoreToOtherAccount"://赠送积分
                        result = GiveScoreToOtherAccount(context);
                        break;


                    case "CheckStock"://检查库存
                        result = CheckStock(context);
                        break;

                    case "GetOnLineType":
                        result = GetOnLineType(context);
                        break;

                    case "OnlineExchangeProdect":
                        result = OnlineExchangeProdect(context);
                        break;
                    case "CalcTransportFee"://计算运费
                        result = CalcTransportFee(context);
                        break;

                    case "AddCoupon"://添加优惠券 土澳
                        result = AddCoupon(context);
                        break;
                    case "GetMyCoupon"://获取我的优惠券
                        result = GetMyCoupon(context);
                        break;

                    case "AddProductCollect"://添加商品收藏
                        result = AddProductCollect(context);
                        break;
                    case "DeleteProductCollect"://删除商品收藏
                        result = DeleteProductCollect(context);
                        break;
                    case "GetMyProductCollect"://查询商品收藏
                        result = GetMyProductCollect(context);
                        break;
                    case "CalcActivityPayAmount"://计算应付金额
                        result = CalcActivityPayAmount(context);
                        break;
                    case "AddActivityOrder"://增加活动订单
                        result = AddActivityOrder(context);
                        break;
                    case "UpdateActivityOrder"://修改活动订单
                        result = UpdateActivityOrder(context);
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
        /// 获取分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOnLineType(HttpContext context)
        {

            List<WXMallScoreTypeInfo> data = bllMall.GetList<WXMallScoreTypeInfo>(string.Format(" WebSiteOwner='{0}'", bllUser.WebsiteOwner));
            if (data != null)
            {
                resp.Status = 0;
                resp.Msg = "成功";
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取单个商品详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetProductObj(HttpContext context)
        {
            string productId = context.Request["PID"];
            resp.ExObj = this.bllMall.Get<WXMallProductInfo>(string.Format(" PID = '{0}' ", productId));
            string sql = string.Format("select Sum(TotalCount) from ZCJ_WXMallOrderDetailsInfo as detail where PID={0} And exists(select 1 from ZCJ_WXMallOrderInfo as orderinfo where Status='交易成功' and orderinfo.OrderID in(detail.OrderID))", productId);
            object saleCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
            if (saleCount != null)
            {
                resp.ExInt = (int)saleCount;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 更新商品IP PV
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateProductIPPV(HttpContext context)
        {
            string pid = context.Request["PID"];
            bool count = bllMall.UpdateWXMallProductPv(pid);
            // bool resultip = bll.UpdateWXMallProductIP(pid);
            // bool resultip = true;
            return (count).ToString();
        }


        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryProductsObjList(HttpContext context)
        {

            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}' And IsOnSale='1' And IsDelete=0", bllMall.WebsiteOwner));
            string keyWord = context.Request["KeyWord"];
            string categoryId = context.Request["CategoryId"];
            string sort = context.Request["Sort"];
            string orderBy = "PID DESC";
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat(" And CategoryId='{0}'", categoryId);

            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And PName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceasc":
                        orderBy = " Price ASC";
                        break;
                    case "pricedesc":
                        orderBy = " Price DESC";
                        break;
                    case "pv":
                        orderBy = " PV DESC";
                        break;
                    default:
                        break;
                }
            }

            var data = this.bllMall.GetList<WXMallProductInfo>(int.MaxValue, sbWhere.ToString(), orderBy);
            for (int i = 0; i < data.Count; i++)
            {
                data[i].PDescription = null;
                data[i].WebsiteOwner = null;
                data[i].UserID = null;

            }
            if (bllMall.IsLogin)
            {
                resp.ExStr = currentUserInfo.AutoID.ToString();
            }
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }


        ///// <summary>
        ///// 获取积分商品列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryScoreProductsObjList(HttpContext context)
        //{

        //    int Online = Convert.ToInt32(context.Request["Line"]);
        //    string TypeId = context.Request["Typeid"];

        //    System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}' And IsOnSale='1' And IsDelete=0 And ScoreLine='{1}'", bll.WebsiteOwner, Online));
        //    if (!string.IsNullOrEmpty(TypeId))
        //    {
        //        sbWhere.AppendFormat(" AND TypeId={0}", TypeId);
        //    }
        //    var data = this.bll.GetList<WXMallScoreProductInfo>(sbWhere.ToString());
        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        data[i].PDescription = null;
        //        data[i].WebsiteOwner = null;
        //        data[i].UserID = null;

        //    }
        //    resp.ExObj = data;
        //    return Common.JSONHelper.ObjectToJson(resp);
        //}



        /// <summary>
        /// 获取积分商品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryScoreProductsObjList(HttpContext context)
        {

            int online = Convert.ToInt32(context.Request["Line"]);
            string typeId = context.Request["Typeid"];
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string categoryId = context.Request["CategoryId"];
            string orderByReq = context.Request["OrderBy"];
            string keyWord = context.Request["Title"];

            string orderBy = "Sort DESC";
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}' And IsOnSale='1' And IsDelete=0 And ScoreLine='{1}'", bllMall.WebsiteOwner, online));
            if (!string.IsNullOrEmpty(typeId))
            {
                sbWhere.AppendFormat(" AND TypeId={0}", typeId);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND PName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(orderByReq))
            {
                switch (orderByReq)
                {
                    case "score":
                        orderBy = "Sort DESC,DiscountScore ASC,Score ASC";
                        break;
                    case "pv":
                        orderBy = "Sort DESC,Pv DESC";
                        break;
                    case "time":
                        orderBy = "Sort DESC,InsertDate DESC";
                        break;
                    default:
                        break;
                }

            }
            var data = this.bllMall.GetLit<WXMallScoreProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            for (int i = 0; i < data.Count; i++)
            {
                data[i].PDescription = null;
                data[i].WebsiteOwner = null;
                data[i].UserID = null;

            }
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取积分记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryScoreRecord(HttpContext context)
        {

            var data = this.bllMall.GetScoreRecordList(currentUserInfo.UserID, int.Parse(context.Request["page"]), int.Parse(context.Request["rows"]));
            for (int i = 0; i < data.Count; i++)
            {
                data[i].AutoID = 0;
                data[i].WebsiteOwner = null;
                data[i].UserId = null;
            }
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }


        ///// <summary>
        ///// 提交订单 一般商品 20140924
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string SubmitWxMallOrder(HttpContext context)
        //{


        //    currUserInfo = DataLoadTool.GetCurrUserModel();
        //    string DeliveryId = context.Request["DeliveryId"];//配送方式 1代表门店自取 2代表快递 3代表无须物流 4代表外卖
        //    string pIdsStr = context.Request["PIds"];
        //    string consignee = context.Request["Consignee"];
        //    string phone = context.Request["Phone"];
        //    string address = context.Request["Address"];
        //    string orderMemo = context.Request["OrderMemo"];
        //    string wxMallStoreId = context.Request["wxMallStoreId"];//门店Id
        //    //string IsOnLinePay = context.Request["IsOnLinePay"];//是否在线支付 1在线支付 0线下支付
        //    string DeliveryTimeStr = context.Request["DeliveryTime"];//是否在线支付 1在线支付 0线下支付
        //    DateTime DtNow = DateTime.Now;//当前时间
        //    DateTime DeliveryTime = new DateTime();
        //    if (DeliveryId.Equals("1"))//门店自取
        //    {
        //        if (string.IsNullOrEmpty(wxMallStoreId))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "请选择一个门店!";
        //            return Common.JSONHelper.ObjectToJson(resp);

        //        }
        //        if (string.IsNullOrWhiteSpace(consignee) || string.IsNullOrWhiteSpace(phone))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "提货人姓名跟手机不能为空!";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }


        //    }
        //    if (DeliveryId.Equals("2"))//快递
        //    {
        //        if (string.IsNullOrWhiteSpace(consignee) || string.IsNullOrWhiteSpace(phone))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "收货人姓名跟手机不能为空!";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //    }
        //    if (DeliveryId.Equals("4"))//外卖
        //    {
        //        if (string.IsNullOrWhiteSpace(consignee) || string.IsNullOrWhiteSpace(phone))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "收货人姓名跟手机不能为空!";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //        DeliveryTime = Convert.ToDateTime(DeliveryTimeStr);
        //        int MinDeliveryDate = 0;
        //        MinDeliveryDate = DataLoadTool.GetWebsiteInfoModel().MinDeliveryDate;
        //        if (DeliveryTime <= DtNow.AddMinutes(MinDeliveryDate))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = string.Format("配送时间须晚于{0}", DtNow.AddMinutes(MinDeliveryDate).ToString("yyyy年MM月dd日HH点mm分"));
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }


        //    }

        //    SubmitWxMallOrderPidsModel pIdsModel = Common.JSONHelper.JsonToModel<SubmitWxMallOrderPidsModel>(pIdsStr);

        //    //生成基本信息
        //    WXMallOrderInfo orderModel = new WXMallOrderInfo();
        //    orderModel.OrderID = this.bll.GetGUID(TransacType.AddWXMallOrderInfo);
        //    orderModel.Consignee = consignee;
        //    orderModel.Phone = phone;
        //    orderModel.Address = address;
        //    orderModel.OrderMemo = orderMemo;
        //    orderModel.OrderUserID = currUserInfo.UserID;
        //    orderModel.WebsiteOwner = bll.WebsiteOwner;
        //    orderModel.WxMallStoreId = wxMallStoreId;
        //    orderModel.DeliveryId = DeliveryId;
        //    orderModel.Status = "待付款";
        //    if (DeliveryId.Equals("4"))//外卖
        //    {
        //        orderModel.DeliveryTime = DeliveryTime;
        //    }
        //    List<WXMallOrderDetailsInfo> details = new List<WXMallOrderDetailsInfo>();

        //    //生成订单详情
        //    foreach (var item in pIdsModel.Pids)
        //    {
        //        WXMallProductInfo ProductInfo = this.bll.Get<WXMallProductInfo>(string.Format(" PID = '{0}' ", (string)item[0]));
        //        WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
        //        detailModel.OrderID = orderModel.OrderID;
        //        detailModel.PID = (string)item[0];
        //        detailModel.TotalCount = (int)item[1];
        //        detailModel.OrderPrice = ProductInfo.Price;
        //        //
        //        if (ProductInfo.IsOnSale.Equals("0"))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = string.Format("{0}已经下架", ProductInfo.PName);
        //            return Common.JSONHelper.ObjectToJson(resp);

        //        }
        //        if (ProductInfo.Stock < detailModel.TotalCount)
        //        {
        //            resp.Status = 0;
        //            resp.Msg = string.Format("{0}库存不足", ProductInfo.PName);
        //            return Common.JSONHelper.ObjectToJson(resp);

        //        }
        //        //
        //        details.Add(detailModel);
        //        orderModel.CategoryId = ProductInfo.CategoryId;

        //    }


        //    //相关计算
        //    orderModel.TotalAmount = details.Sum(p => p.OrderPrice * p.TotalCount).Value;

        //    //保存数据
        //    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
        //    try
        //    {

        //        if (!this.bll.Add(orderModel, tran))
        //        {
        //            tran.Rollback();
        //            resp.Status = 0;
        //            resp.Msg = "提交失败";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //        //bll.AddList(details);
        //        foreach (var item in details)
        //        {

        //            if (!this.bll.Add(item, tran))
        //            {
        //                tran.Rollback();
        //                resp.Status = -1;
        //                resp.Msg = "提交失败";
        //                return Common.JSONHelper.ObjectToJson(resp);
        //            }
        //            else//更新商品库存
        //            {

        //                bll.UpdateProductStock(int.Parse(item.PID), item.TotalCount);


        //            }
        //        }

        //        tran.Commit();

        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        resp.Status = -1;
        //        resp.Msg = "异常:" + ex.Message;
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    //try
        //    //{

        //    //补足用户信息：如果原用户没有真实姓名和地址，则补充上去
        //    //if (string.IsNullOrWhiteSpace(this.currUserInfo.TrueName))
        //    this.currUserInfo.TrueName = consignee;
        //    //if (string.IsNullOrWhiteSpace(this.currUserInfo.Address))
        //    this.currUserInfo.Address = address;

        //    // if (string.IsNullOrWhiteSpace(this.currUserInfo.Phone))
        //    this.currUserInfo.Phone = phone;

        //    this.bll.Update(this.currUserInfo);
        //    //}
        //    //catch { }

        //    //返回
        //    resp.ExStr = orderModel.OrderID;
        //    resp.Status = 1;
        //    resp.Msg = "订单提交成功!";

        //    try
        //    {
        //        BLLWeixin bllWeixin = new BLLWeixin("");
        //        System.Text.StringBuilder Message = new System.Text.StringBuilder();
        //        Message.AppendFormat("您的订单 {0} 已确认收到，您可进入个人中心-我的订单页面随时关注订单状态\n", orderModel.OrderID);

        //        Message.AppendFormat("订单总金额:￥{0}\n", orderModel.TotalAmount);
        //        Message.AppendFormat("下单时间:{0}\n", orderModel.InsertDate);
        //        Message.AppendFormat("收货人:{0}", orderModel.Consignee);
        //        var accesstokenmodel = bllWeixin.GetAccessToken(currWebSiteUserInfo.WeixinAppId, currWebSiteUserInfo.WeixinAppSecret);
        //        if (!string.IsNullOrEmpty(accesstokenmodel.access_token))
        //        {
        //            bllWeixin.SendMessageKeFu(accesstokenmodel.access_token, bllWeixin.CreateTextStrMessage(currUserInfo.WXOpenId, Message.ToString()));
        //        }



        //    }
        //    catch
        //    {


        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);


        //}
        /// <summary>
        /// 提交订单 V1 普通商城
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitWxMallOrder(HttpContext context)
        {
            string pIds = context.Request["PIds"];
            string consignee = context.Request["Consignee"];
            string phone = context.Request["Phone"];
            string address = context.Request["Address"];
            string orderMemo = context.Request["OrderMemo"];
            string deliveryAutoId = context.Request["DeliveryAutoId"];
            string paymenttypeAutoId = context.Request["PaymenttypeAutoId"];
            string recommendId = context.Request["SID"];//推荐人用户编号 AutoID
            WebsiteInfo webSiteInfo = bllMall.GetWebsiteInfoModel();
            if (webSiteInfo.IsDistributionMall == 1)
            {

                if (!string.IsNullOrEmpty(recommendId))//有推荐人
                {

                    UserInfo recommondUserInfo = bllUser.GetUserInfoByAutoID(int.Parse(recommendId));
                    if ((recommondUserInfo != null) && (!currentUserInfo.UserID.Equals(recommondUserInfo.UserID)))
                    {
                        if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                        {
                            currentUserInfo.DistributionOwner = recommondUserInfo.UserID;
                            if (bllUser.Update(currentUserInfo, string.Format(" DistributionOwner='{0}'", currentUserInfo.DistributionOwner), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 1)
                            {
                                resp.Msg = "设置分销上级失败";
                                return Common.JSONHelper.ObjectToJson(resp);
                            }



                        }
                        else
                        {

                        }


                    }
                    else
                    {
                        if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                        {
                            currentUserInfo.DistributionOwner = bllMall.WebsiteOwner;
                            if (bllUser.Update(currentUserInfo, string.Format(" DistributionOwner='{0}'", currentUserInfo.DistributionOwner), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 1)
                            {
                                resp.Msg = "设置分销上级失败";
                                return Common.JSONHelper.ObjectToJson(resp);

                            }

                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                    {
                        currentUserInfo.DistributionOwner = bllMall.WebsiteOwner;
                        if (bllUser.Update(currentUserInfo, string.Format(" DistributionOwner='{0}'", currentUserInfo.DistributionOwner), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 1)
                        {
                            resp.Msg = "设置分销上级失败";
                            return Common.JSONHelper.ObjectToJson(resp);

                        }

                    }


                }


            }


            decimal productFee = 0;
            decimal transportFee = 0;
            int productCount = 0;//商品数量
            WXMallDelivery delivery = bllMall.GetDelivery(int.Parse(deliveryAutoId));
            WXMallPaymentType paymentType = bllMall.GetPaymentType(int.Parse(paymenttypeAutoId));
            if (delivery == null)
            {

                resp.Msg = "配送方式无效!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (paymentType == null)
            {

                resp.Msg = "支付方式无效!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrWhiteSpace(consignee) || string.IsNullOrWhiteSpace(phone))
            {

                resp.Msg = "收货人姓名跟手机不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            SubmitWxMallOrderPidsModel pIdsModel = Common.JSONHelper.JsonToModel<SubmitWxMallOrderPidsModel>(pIds);
            //生成基本信息
            WXMallOrderInfo orderModel = new WXMallOrderInfo();
            orderModel.OrderID = this.bllMall.GetGUID(TransacType.AddWXMallOrderInfo);
            orderModel.Consignee = consignee;
            orderModel.Phone = phone;
            orderModel.Address = address;
            orderModel.OrderMemo = orderMemo;
            orderModel.OrderUserID = currentUserInfo.UserID;
            orderModel.WebsiteOwner = bllMall.WebsiteOwner;
            orderModel.Status = "待付款";
            orderModel.DeliveryAutoId = deliveryAutoId;
            orderModel.PaymentTypeAutoId = paymenttypeAutoId;
            orderModel.PaymentStatus = 0;
            orderModel.DeliveryType = (int)delivery.DeliveryType;
            orderModel.PaymentType = paymentType.PaymentType;
            if (webSiteInfo.IsDistributionMall == 1)
            {
                orderModel.DistributionRateLevel1 = (decimal)webSiteInfo.DistributionRateLevel1;
                int orderCount = bllMall.GetCount<WXMallOrderInfo>(string.Format(" OrderUserID='{0}' And WebsiteOwner='{1}' And (PaymentStatus=1 or Status='已付款' or DistributionStatus>=1) ", currentUserInfo.UserID, bllMall.WebsiteOwner));
                if (orderCount >= 1)//会员再次购买
                {
                    orderModel.DistributionRateLevel1 = (decimal)webSiteInfo.DistributionMemberRateLevel1;
                }
            }
            List<WXMallOrderDetailsInfo> details = new List<WXMallOrderDetailsInfo>();
            //生成订单详情
            foreach (var item in pIdsModel.Pids)
            {
                WXMallProductInfo productInfo = this.bllMall.GetProduct((string)item[0]);
                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.OrderID = orderModel.OrderID;
                detailModel.PID = (string)item[0];
                detailModel.TotalCount = (int)item[1];
                detailModel.OrderPrice = productInfo.Price;
                //
                if (productInfo.IsOnSale.Equals("0"))
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (productInfo.Stock < detailModel.TotalCount)
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}库存不足", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //
                details.Add(detailModel);
                orderModel.CategoryId = productInfo.CategoryId;

            }
            productFee = details.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用
            productCount = details.Sum(p => p.TotalCount);//商品数量
            if (delivery.DeliveryType.Equals(0))//有配送费用
            {
                if (productCount <= delivery.InitialProductCount)
                {
                    transportFee = delivery.InitialDeliveryMoney;
                }
                else
                {

                    transportFee = delivery.InitialDeliveryMoney + Math.Ceiling((decimal)(productCount - delivery.InitialProductCount) / delivery.AddProductCount) * delivery.AddMoney;

                }


            }

            orderModel.Transport_Fee = transportFee;
            //相关计算
            orderModel.Product_Fee = productFee;
            orderModel.TotalAmount = orderModel.Product_Fee + orderModel.Transport_Fee;
            //保存数据
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderModel, tran))
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                foreach (var item in details)
                {

                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    else//更新商品库存
                    {

                        bllMall.UpdateProductStock(int.Parse(item.PID), item.TotalCount);


                    }
                }

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = "异常:" + ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //try
            //{
            currentUserInfo = bllMall.GetCurrentUserInfo();
            //补足用户信息：如果原用户没有真实姓名和地址，则补充上去
            //if (string.IsNullOrWhiteSpace(this.currUserInfo.TrueName))
            this.currentUserInfo.TrueName = consignee;
            //if (string.IsNullOrWhiteSpace(this.currUserInfo.Address))
            this.currentUserInfo.Address = address;

            // if (string.IsNullOrWhiteSpace(this.currUserInfo.Phone))
            this.currentUserInfo.Phone = phone;

            this.bllMall.Update(this.currentUserInfo, string.Format(" TrueName='{0}',Address='{1}',Phone='{2}'", currentUserInfo.TrueName, currentUserInfo.Address, currentUserInfo.Phone), string.Format(" AutoID={0}", currentUserInfo.AutoID));
            //}
            //catch { }

            //返回
            resp.ExStr = orderModel.OrderID;
            resp.Status = 1;
            resp.Msg = "订单提交成功!";

            try
            {
                System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
                sbMessage.AppendFormat("您的订单 {0} 已确认收到，您可进入个人中心-我的订单页面随时关注订单状态\n", orderModel.OrderID);
                sbMessage.AppendFormat("订单总金额:￥{0}\n", orderModel.TotalAmount);
                sbMessage.AppendFormat("下单时间:{0}\n", orderModel.InsertDate);
                sbMessage.AppendFormat("收货人:{0}", orderModel.Consignee);
                string accessToken = bllWeixin.GetAccessToken();
                if (accessToken != string.Empty)
                {
                    bllWeixin.SendKeFuMessageText(accessToken, currentUserInfo.WXOpenId, sbMessage.ToString());
                }

            }
            catch
            {


            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 提交订单 外卖类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitWxMallOrderV1(HttpContext context)
        {


            string pIds = context.Request["PIds"];
            string consignee = context.Request["Consignee"];
            string phone = context.Request["Phone"];
            string address = context.Request["Address"];
            string orderMemo = context.Request["OrderMemo"];
            string deliveryTimeStr = context.Request["DeliveryTime"];//配送时间
            string deliveryAutoId = context.Request["DeliveryAutoId"];
            string paymenttypeAutoId = context.Request["PaymenttypeAutoId"];
            DateTime dtNow = DateTime.Now;//当前时间
            DateTime deliveryTime = new DateTime();
            decimal productFee = 0;
            decimal transportFee = 0;
            int productCount = 0;//商品数量
            WXMallDelivery delivery = bllMall.GetDelivery(int.Parse(deliveryAutoId));
            WXMallPaymentType paymentType = bllMall.GetPaymentType(int.Parse(paymenttypeAutoId));
            if (delivery == null)
            {
                resp.Status = 0;
                resp.Msg = "配送方式无效!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (paymentType == null)
            {
                resp.Status = 0;
                resp.Msg = "支付方式无效!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrWhiteSpace(consignee) || string.IsNullOrWhiteSpace(phone))
            {
                resp.Status = 0;
                resp.Msg = "收货人姓名跟手机不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            deliveryTime = Convert.ToDateTime(deliveryTimeStr);
            int minDeliveryDate = 0;
            minDeliveryDate = DataLoadTool.GetWebsiteInfoModel().MinDeliveryDate;
            if (deliveryTime <= dtNow.AddMinutes(minDeliveryDate))
            {
                resp.Status = 0;
                resp.Msg = string.Format("配送时间须晚于{0}", dtNow.AddMinutes(minDeliveryDate).ToString("yyyy年MM月dd日HH点mm分"));
                return Common.JSONHelper.ObjectToJson(resp);
            }
            SubmitWxMallOrderPidsModel pIdsModel = Common.JSONHelper.JsonToModel<SubmitWxMallOrderPidsModel>(pIds);

            //生成基本信息
            WXMallOrderInfo orderModel = new WXMallOrderInfo();
            orderModel.OrderID = this.bllMall.GetGUID(TransacType.AddWXMallOrderInfo);
            orderModel.Consignee = consignee;
            orderModel.Phone = phone;
            orderModel.Address = address;
            orderModel.OrderMemo = orderMemo;
            orderModel.OrderUserID = currentUserInfo.UserID;
            orderModel.WebsiteOwner = bllMall.WebsiteOwner;
            orderModel.Status = "待付款";
            orderModel.DeliveryTime = deliveryTime;
            orderModel.DeliveryAutoId = deliveryAutoId;
            orderModel.PaymentTypeAutoId = paymenttypeAutoId;
            orderModel.PaymentStatus = 0;
            orderModel.DeliveryType = (int)delivery.DeliveryType;
            orderModel.PaymentType = paymentType.PaymentType;
            List<WXMallOrderDetailsInfo> details = new List<WXMallOrderDetailsInfo>();
            //生成订单详情
            foreach (var item in pIdsModel.Pids)
            {
                WXMallProductInfo productInfo = bllMall.GetProduct((string)item[0]);
                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.OrderID = orderModel.OrderID;
                detailModel.PID = (string)item[0];
                detailModel.TotalCount = (int)item[1];
                detailModel.OrderPrice = productInfo.Price;
                //
                if (productInfo.IsOnSale.Equals("0"))
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (productInfo.Stock < detailModel.TotalCount)
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}库存不足", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //
                details.Add(detailModel);
                orderModel.CategoryId = productInfo.CategoryId;

            }
            productFee = details.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用
            productCount = details.Sum(p => p.TotalCount);//商品数量
            if (delivery.DeliveryType.Equals(0))//有配送费用
            {
                if (productCount <= delivery.InitialProductCount)
                {
                    transportFee = delivery.InitialDeliveryMoney;
                }
                else
                {

                    transportFee = delivery.InitialDeliveryMoney + Math.Ceiling((decimal)(productCount - delivery.InitialProductCount) / delivery.AddProductCount) * delivery.AddMoney;

                }


            }
            orderModel.Transport_Fee = transportFee;
            //相关计算
            orderModel.Product_Fee = productFee;
            orderModel.TotalAmount = orderModel.Product_Fee + orderModel.Transport_Fee;
            //保存数据
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderModel, tran))
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                foreach (var item in details)
                {

                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    else//更新商品库存
                    {

                        bllMall.UpdateProductStock(int.Parse(item.PID), item.TotalCount);


                    }
                }

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = "异常:" + ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //try
            //{

            //补足用户信息：如果原用户没有真实姓名和地址，则补充上去
            //if (string.IsNullOrWhiteSpace(this.currUserInfo.TrueName))
            this.currentUserInfo.TrueName = consignee;
            //if (string.IsNullOrWhiteSpace(this.currUserInfo.Address))
            this.currentUserInfo.Address = address;

            // if (string.IsNullOrWhiteSpace(this.currUserInfo.Phone))
            this.currentUserInfo.Phone = phone;

            this.bllMall.Update(this.currentUserInfo, string.Format(" TrueName='{0}',Address='{1}',Phone='{2}'", currentUserInfo.TrueName, currentUserInfo.Address, currentUserInfo.Phone), string.Format(" AutoID={0}", currentUserInfo.AutoID));
            //}
            //catch { }

            //返回
            resp.ExStr = orderModel.OrderID;
            resp.Status = 1;
            resp.Msg = "订单提交成功!";

            try
            {
                System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
                sbMessage.AppendFormat("您的订单 {0} 已确认收到，您可进入个人中心-我的订单页面随时关注订单状态\n", orderModel.OrderID);

                sbMessage.AppendFormat("订单总金额:￥{0}\n", orderModel.TotalAmount);
                sbMessage.AppendFormat("下单时间:{0}\n", orderModel.InsertDate);
                sbMessage.AppendFormat("收货人:{0}", orderModel.Consignee);
                var accessToken = bllWeixin.GetAccessToken();
                if (accessToken != string.Empty)
                {
                    bllWeixin.SendKeFuMessageText(accessToken, currentUserInfo.WXOpenId, sbMessage.ToString());
                }



            }
            catch
            {


            }
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 提交订单 包笑公堂
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitWxMallOrderBXGT(HttpContext context)
        {


            string productId = context.Request["ProductId"];
            int productCount = string.IsNullOrEmpty(context.Request["Count"]) ? 1 : int.Parse(context.Request["Count"]);
            string name = context.Request["Name"];
            string phone = context.Request["Phone"];
            string address = context.Request["Address"];
            string showAddress = context.Request["ShowAddress"];
            string showTime = context.Request["ShowTime"];
            string price = context.Request["Price"];
            string deliveryAutoId = context.Request["DeliveryAutoId"];
            string paymentTypeAutoId = context.Request["PaymenttypeAutoId"];
            string remark = string.Format("演出地点:{0}<br/>演出时间:{1}<br/>型号:{2}", showAddress, showTime, price);
            decimal productFee = 0;
            decimal transportFee = 0;
            WXMallDelivery delivery = bllMall.GetDelivery(int.Parse(deliveryAutoId));
            WXMallPaymentType paymentType = bllMall.GetPaymentType(int.Parse(paymentTypeAutoId));
            if (delivery == null)
            {
                resp.Status = 0;
                resp.Msg = "配送方式无效!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (paymentType == null)
            {
                resp.Status = 0;
                resp.Msg = "支付方式无效!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrEmpty(address))
            {
                resp.Status = 0;
                resp.Msg = "收货人姓名,手机,地址不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //生成基本信息
            WXMallOrderInfo orderModel = new WXMallOrderInfo();
            orderModel.OrderID = this.bllMall.GetGUID(TransacType.AddWXMallOrderInfo);
            orderModel.Consignee = name;
            orderModel.Phone = phone;
            orderModel.Address = address;
            orderModel.OrderMemo = remark;
            orderModel.OrderUserID = currentUserInfo.UserID;
            orderModel.WebsiteOwner = bllMall.WebsiteOwner;
            orderModel.Status = "待付款";
            orderModel.DeliveryAutoId = deliveryAutoId;
            orderModel.PaymentTypeAutoId = paymentTypeAutoId;
            orderModel.PaymentStatus = 0;
            orderModel.DeliveryType = (int)delivery.DeliveryType;
            orderModel.PaymentType = paymentType.PaymentType;
            List<WXMallOrderDetailsInfo> details = new List<WXMallOrderDetailsInfo>();
            //生成订单详情
            WXMallProductInfo productInfo = bllMall.GetProduct(productId);
            WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
            detailModel.OrderID = orderModel.OrderID;
            detailModel.PID = productId;
            detailModel.TotalCount = productCount;
            detailModel.OrderPrice = decimal.Parse(price);
            if (productInfo.IsOnSale.Equals("0"))
            {
                resp.Status = 0;
                resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (productInfo.Stock < detailModel.TotalCount)
            {
                resp.Status = 0;
                resp.Msg = string.Format("{0}库存不足", productInfo.PName);
                return Common.JSONHelper.ObjectToJson(resp);

            }
            //
            details.Add(detailModel);
            orderModel.CategoryId = productInfo.CategoryId;


            productFee = details.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用
            productCount = details.Sum(p => p.TotalCount);//商品数量
            orderModel.Transport_Fee = transportFee;
            //相关计算
            orderModel.Product_Fee = productFee;
            orderModel.TotalAmount = orderModel.Product_Fee + orderModel.Transport_Fee;
            //保存数据
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderModel, tran))
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                foreach (var item in details)
                {

                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    else//更新商品库存
                    {

                        bllMall.UpdateProductStock(int.Parse(item.PID), item.TotalCount);


                    }
                }

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //try
            //{

            //补足用户信息：如果原用户没有真实姓名和地址，则补充上去
            //if (string.IsNullOrWhiteSpace(this.currUserInfo.TrueName))
            this.currentUserInfo.TrueName = name;
            //if (string.IsNullOrWhiteSpace(this.currUserInfo.Address))
            this.currentUserInfo.Address = address;

            // if (string.IsNullOrWhiteSpace(this.currUserInfo.Phone))
            this.currentUserInfo.Phone = phone;

            //this.bll.Update(this.currUserInfo);
            //}
            //catch { }
            //返回
            resp.ExStr = orderModel.OrderID;
            resp.Status = 1;
            resp.Msg = "订单提交成功!";
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 提交订单 土澳网
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitWxMallOrderTuao(HttpContext context)
        {
            string pIds = context.Request["PIds"];//商品id 跟数量集合
            string addressId = context.Request["AddressId"];//收货地址ID
            string couponNumber = context.Request["CouponNumber"];//优惠券号码

            decimal productFee = 0;//商品费用
            //decimal Transport_Fee = 0;
            int productCount = 0;//商品数量
            if (string.IsNullOrEmpty(addressId))
            {
                resp.Msg = "请选择一个收货地址!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WXConsigneeAddress addressInfo = bllMall.GetConsigneeAddress(addressId);//收货地址
            if (addressInfo == null || (!addressInfo.UserID.Equals(currentUserInfo.UserID)))
            {
                resp.Msg = "收货地址无效";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            var coupon = bllMall.GetCoupon(couponNumber);//优惠券
            #region 检查优惠券是否有效
            if (!string.IsNullOrEmpty(couponNumber))
            {

                if (coupon == null)
                {
                    resp.Msg = "优惠券号码无效,请检查!";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (!string.IsNullOrEmpty(coupon.StartDate))
                {
                    if (Convert.ToDateTime(coupon.StartDate) > DateTime.Now)
                    {
                        resp.Msg = "优惠券还未到使用日期";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                if (!string.IsNullOrEmpty(coupon.StopDate))
                {
                    if (Convert.ToDateTime(coupon.StopDate) < DateTime.Now)
                    {
                        resp.Msg = "优惠券已过期";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }


            }
            #endregion

            SubmitWxMallOrderPidsModel pidsModel = Common.JSONHelper.JsonToModel<SubmitWxMallOrderPidsModel>(pIds);
            //生成基本信息
            WXMallOrderInfo orderModel = new WXMallOrderInfo();
            orderModel.OrderID = this.bllMall.GetGUID(TransacType.AddWXMallOrderInfo);
            orderModel.Consignee = addressInfo.ConsigneeName;
            orderModel.Phone = addressInfo.Phone;
            orderModel.Address = addressInfo.Address;
            //OrderModel.OrderMemo = OrderMemo;
            orderModel.OrderUserID = currentUserInfo.UserID;
            orderModel.WebsiteOwner = bllMall.WebsiteOwner;
            orderModel.Status = "待付款";
            orderModel.PaymentStatus = 0;
            orderModel.PaymentType = 2;
            orderModel.MyCouponCardId = couponNumber;
            List<WXMallOrderDetailsInfo> orderDetails = new List<WXMallOrderDetailsInfo>();
            //生成订单详情
            foreach (var item in pidsModel.Pids)
            {
                WXMallProductInfo productInfo = bllMall.GetProduct((string)item[0]);
                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.OrderID = orderModel.OrderID;
                detailModel.PID = (string)item[0];
                detailModel.TotalCount = (int)item[1];
                detailModel.OrderPrice = productInfo.Price;
                //
                if (productInfo.IsOnSale.Equals("0"))
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (productInfo.Stock < detailModel.TotalCount)
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}库存不足", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //
                //优惠券
                if (coupon != null)
                {
                    if (!string.IsNullOrEmpty(coupon.ProductId))
                    {
                        if (productInfo.PID.Equals(coupon.ProductId))
                        {
                            //指定商品折扣
                            detailModel.OrderPrice = productInfo.Price * ((decimal)coupon.Discount / 10);

                        }

                    }

                }
                //优惠券
                orderDetails.Add(detailModel);
                orderModel.CategoryId = productInfo.CategoryId;



            }
            productFee = orderDetails.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用
            productCount = orderDetails.Sum(p => p.TotalCount);//商品数量

            orderModel.Transport_Fee = 0;
            //相关计算
            orderModel.Product_Fee = productFee;
            orderModel.TotalAmount = orderModel.Product_Fee + orderModel.Transport_Fee;
            //检查当前用户等级是否有折扣
            orderModel.TotalAmount = bllMall.GetDiscountAmount(currentUserInfo, orderModel.TotalAmount);
            ///检查当前用户等级是否有折扣

            //检查是否有优惠券
            if (!string.IsNullOrEmpty(couponNumber))
            {
                if (string.IsNullOrEmpty(coupon.ProductId))//对全部商品打折
                {
                    orderModel.TotalAmount = bllMall.GetDiscountAmount(currentUserInfo, orderModel.TotalAmount, couponNumber);
                }



            }
            //检查是否有优惠券
            //保存数据
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderModel, tran))
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                foreach (var item in orderDetails)
                {

                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    //更新商品库存


                    resp.ExObj = bllMall.UpdateProductStock(int.Parse(item.PID), item.TotalCount);



                }

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = "异常:" + ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //返回
            resp.ExStr = orderModel.OrderID;
            resp.Status = 1;
            resp.Msg = "订单提交成功!";


            System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
            sbMessage.AppendFormat("您的订单 {0} 已确认收到，您可进入个人中心-我的订单页面随时关注订单状态\n", orderModel.OrderID);
            sbMessage.AppendFormat("订单总金额:￥{0}\n", orderModel.TotalAmount);
            sbMessage.AppendFormat("下单时间:{0}\n", orderModel.InsertDate);
            sbMessage.AppendFormat("收货人:{0}", orderModel.Consignee);
            string accessToken = bllWeixin.GetAccessToken();
            if (accessToken != string.Empty)
            {
                bllWeixin.SendKeFuMessageText(accessToken, currentUserInfo.WXOpenId, sbMessage.ToString());
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }





        /// <summary>
        /// 检查订单 CheckStock 库存
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CheckStock(HttpContext context)
        {
            string pids = context.Request["PIds"];
            SubmitWxMallOrderPidsModel pIdsModel = Common.JSONHelper.JsonToModel<SubmitWxMallOrderPidsModel>(pids);
            foreach (var item in pIdsModel.Pids)
            {
                WXMallProductInfo productInfo = bllMall.GetProduct((string)item[0]);
                if (productInfo.IsOnSale.Equals("0"))
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (Convert.ToInt32(item[1]) > productInfo.Stock)
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0} 库存为{1}", productInfo.PName, productInfo.Stock);
                    return Common.JSONHelper.ObjectToJson(resp);

                }


            }
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 提交订单 积分商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitWxMallScoreOrder(HttpContext context)
        {
            int pId = int.Parse(context.Request["PID"]);
            string consignee = context.Request["Consignee"];
            string phone = context.Request["Phone"];
            string address = context.Request["Address"];
            string orderMemo = context.Request["OrderMemo"];
            string province = context.Request["Province"];
            string city = context.Request["City"];
            string district = context.Request["District"];
            int count = int.Parse(context.Request["Count"]);
            if (string.IsNullOrWhiteSpace(consignee) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address))
            {
                resp.Status = 0;
                resp.Msg = "收货人姓名,手机,地址不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(province) || string.IsNullOrEmpty(city) | string.IsNullOrEmpty(district) || province.Equals("null"))
            {
                resp.Status = 0;
                resp.Msg = "省市区不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (count <= 0)
            {
                resp.Status = 0;
                resp.Msg = "数量须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            WXMallScoreProductInfo productInfo = this.bllMall.GetScoreProduct(pId);
            if (productInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "积分商品不存在!";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (productInfo.DiscountScore > 0)
            {
                productInfo.Score = productInfo.DiscountScore;
            }
            if (currentUserInfo.TotalScore < (productInfo.Score * count))
            {
                resp.Status = 0;
                resp.Msg = "您的积分不足,无法兑换";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            //生成基本信息
            WXMallScoreOrderInfo orderModel = new WXMallScoreOrderInfo();
            orderModel.OrderID = this.bllMall.GetGUID(TransacType.AddWXMallOrderInfo);
            orderModel.Consignee = consignee;
            orderModel.Phone = phone;
            orderModel.Address = province + city + district + address;
            orderModel.OrderMemo = orderMemo;
            orderModel.OrderUserID = currentUserInfo.UserID;
            orderModel.WebsiteOwner = bllMall.WebsiteOwner;
            orderModel.Status = "待付款";

            List<WXMallScoreOrderDetailsInfo> orderDetails = new List<WXMallScoreOrderDetailsInfo>();
            WXMallScoreOrderDetailsInfo detailModel = new WXMallScoreOrderDetailsInfo();
            detailModel.OrderID = orderModel.OrderID;
            detailModel.PID = pId.ToString();
            detailModel.TotalCount = count;
            detailModel.OrderPrice = productInfo.Score;
            if (productInfo.IsOnSale.Equals("0"))
            {
                resp.Status = 0;
                resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (productInfo.Stock < detailModel.TotalCount)
            {
                resp.Status = 0;
                resp.Msg = string.Format("{0}库存不足", productInfo.PName);
                return Common.JSONHelper.ObjectToJson(resp);

            }
            //
            orderDetails.Add(detailModel);
            //相关计算
            orderModel.TotalAmount = orderDetails.Sum(p => p.OrderPrice * p.TotalCount).Value;
            //保存数据
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderModel, tran))
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                foreach (var item in orderDetails)
                {

                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    else
                    {


                    }
                }
                #region 更新库存
                productInfo.Stock -= count;
                if (bllMall.Update(productInfo, string.Format(" Stock={0}", productInfo.Stock), string.Format(" AutoID={0}", productInfo.AutoID), tran) <= 0)
                {

                    tran.Rollback();
                    resp.Status = -1;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);


                }
                #endregion

                #region 更新用户积分

                BLLUserScore bllUserScore = new BLLUserScore(currentUserInfo.UserID);
                BLLUserScore.UserScore userScore = bllUserScore.GetDefinedUserScore(BLLUserScore.UserScoreType.ExchangeGoodInScoreMall);
                userScore.Score = Convert.ToDouble(orderModel.TotalAmount) * -1;
                if (0 == bllUserScore.UpdateUserScoreWithWXTMNotify(userScore, bllWeixin.GetAccessToken()))
                {
                    tran.Rollback();
                    resp.Status = -1;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                #endregion
                WXMallScoreRecord scoreRecord = new WXMallScoreRecord();
                scoreRecord.InsertDate = DateTime.Now;
                scoreRecord.OrderID = orderModel.OrderID;
                scoreRecord.Remark = "积分商城-购物";
                scoreRecord.Score = -(Convert.ToInt32(orderModel.TotalAmount));
                scoreRecord.Type = 2;
                scoreRecord.UserId = currentUserInfo.UserID;
                scoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                if (!bllMall.Add(scoreRecord, tran))
                {
                    tran.Rollback();
                    resp.Status = -1;
                    resp.Msg = "提交失败";
                    return Common.JSONHelper.ObjectToJson(resp);

                }

                tran.Commit();






            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }



            currentUserInfo = bllUser.GetUserInfo(orderModel.OrderUserID);
            currentUserInfo.Province = province;
            currentUserInfo.City = city;
            currentUserInfo.District = district;
            currentUserInfo.Address = address;
            currentUserInfo.Phone = phone;

            bllMall.Update(this.currentUserInfo, string.Format(" Province='{0}',City='{1}',District='{2}',Address='{3}'", currentUserInfo.Province, currentUserInfo.City, currentUserInfo.District, currentUserInfo.Address), string.Format(" AutoID={0}", currentUserInfo.AutoID));


            //返回
            resp.ExStr = orderModel.OrderID;
            resp.Status = 1;
            resp.Msg = "订单提交成功!";

            try
            {

                string accessToken = bllWeixin.GetAccessToken();
                System.Text.StringBuilder sbMsg = new System.Text.StringBuilder();
                sbMsg.AppendFormat("您的订单 {0} 已确认收到，您可进入个人中心-我的订单页面随时关注订单状态\n", orderModel.OrderID);

                sbMsg.AppendFormat("订单积分:{0}积分\n", (int)orderModel.TotalAmount);
                sbMsg.AppendFormat("下单时间:{0}\n", orderModel.InsertDate);
                sbMsg.AppendFormat("收货人:{0}\n", orderModel.Consignee);
                sbMsg.AppendFormat("提示:取消订单不退还积分", orderModel.Consignee);
                if (accessToken != string.Empty)
                {
                    bllWeixin.SendKeFuMessageText(accessToken, currentUserInfo.WXOpenId, sbMsg.ToString());
                }


                ////提醒客服
                //if (!string.IsNullOrEmpty(currentWebSiteUserInfo.WeiXinKeFuOpenId))
                //{
                //    BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
                //    notificaiton.First = "有新的积分订单";
                //    notificaiton.Keyword1 = "订单号:" + orderModel.OrderID;
                //    notificaiton.Keyword2 = "积分订单";
                //    notificaiton.Remark = string.Format("商品:{0}收货人:{1}", productInfo.PName, orderModel.Consignee);
                //    bllWeixin.SendTemplateMessage(accessToken, currentWebSiteUserInfo.WeiXinKeFuOpenId, notificaiton);
                //}
                ////提醒客服
            }
            catch
            {


            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 线下兑换商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string OnlineExchangeProdect(HttpContext context)
        {
            try
            {
                int pid = int.Parse(context.Request["PID"]);
                string consignee = currentUserInfo.TrueName;
                string phone = currentUserInfo.Phone;
                string address = currentUserInfo.Address;
                int count = int.Parse(context.Request["Count"]);
                WXMallScoreProductInfo productInfo = this.bllMall.GetScoreProduct(pid);
                if (productInfo == null)
                {
                    resp.Status = 0;
                    resp.Msg = "积分商品不存在!";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (currentUserInfo.TotalScore < (productInfo.Score * count))
                {
                    resp.Status = 0;
                    resp.Msg = "您的积分不足,无法兑换";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //生成基本信息
                WXMallScoreOrderInfo orderModel = new WXMallScoreOrderInfo();
                orderModel.OrderID = this.bllMall.GetGUID(TransacType.AddWXMallOrderInfo);
                orderModel.Consignee = consignee;
                orderModel.Phone = phone;
                orderModel.Address = address;
                orderModel.OrderMemo = "";
                orderModel.OrderUserID = currentUserInfo.UserID;
                orderModel.WebsiteOwner = bllMall.WebsiteOwner;
                orderModel.Status = "交易完成";
                List<WXMallScoreOrderDetailsInfo> orderDetails = new List<WXMallScoreOrderDetailsInfo>();
                WXMallScoreOrderDetailsInfo detailModel = new WXMallScoreOrderDetailsInfo();
                detailModel.OrderID = orderModel.OrderID;
                detailModel.PID = pid.ToString();
                detailModel.TotalCount = count;
                detailModel.OrderPrice = productInfo.Score;
                if (productInfo.IsOnSale.Equals("0"))
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}已经下架", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (productInfo.Stock < detailModel.TotalCount)
                {
                    resp.Status = 0;
                    resp.Msg = string.Format("{0}库存不足", productInfo.PName);
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //
                orderDetails.Add(detailModel);

                //相关计算
                orderModel.TotalAmount = orderDetails.Sum(p => p.OrderPrice * p.TotalCount).Value;
                //保存数据
                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                try
                {

                    if (!this.bllMall.Add(orderModel, tran))
                    {
                        tran.Rollback();
                        resp.Status = 0;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }

                    foreach (var item in orderDetails)
                    {

                        if (!this.bllMall.Add(item, tran))
                        {
                            tran.Rollback();
                            resp.Status = -1;
                            resp.Msg = "提交失败";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                        else
                        {


                        }
                    }
                    #region 更新库存
                    productInfo.Stock -= count;
                    if (!bllMall.Update(productInfo, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);


                    }
                    #endregion

                    #region 更新用户积分
                    currentUserInfo.TotalScore -= Convert.ToDouble(orderModel.TotalAmount);
                    if (bllMall.Update(currentUserInfo, string.Format(" TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 1)
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);

                    }
                    #endregion
                    WXMallScoreRecord scoreRecord = new WXMallScoreRecord();
                    scoreRecord.InsertDate = DateTime.Now;
                    scoreRecord.OrderID = orderModel.OrderID;
                    scoreRecord.Remark = "积分商城-购物";
                    scoreRecord.Score = -(Convert.ToInt32(orderModel.TotalAmount));
                    scoreRecord.Type = 2;
                    scoreRecord.UserId = currentUserInfo.UserID;
                    scoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                    if (!bllMall.Add(scoreRecord, tran))
                    {
                        tran.Rollback();
                        resp.Status = -1;
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);

                    }
                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resp.Status = -1;
                    resp.Msg = ex.Message;
                    return Common.JSONHelper.ObjectToJson(resp);
                }


                //补足用户信息：如果原用户没有真实姓名和地址，则补充上去
                // if (string.IsNullOrWhiteSpace(this.currUserInfo.TrueName))
                this.currentUserInfo.TrueName = consignee;
                //if (string.IsNullOrWhiteSpace(this.currUserInfo.Address))
                this.currentUserInfo.Address = address;

                // if (string.IsNullOrWhiteSpace(this.currUserInfo.Phone))
                this.currentUserInfo.Phone = phone;

                this.bllMall.Update(this.currentUserInfo, string.Format(" TrueName='{0}',Address='{1}',Phone='{2}'", currentUserInfo.TrueName, currentUserInfo.Address, currentUserInfo.Phone), string.Format(" AutoID={0}", currentUserInfo.AutoID));


                //返回
                resp.ExStr = orderModel.OrderID;
                resp.Status = 1;
                resp.Msg = "订单提交成功!";
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员！";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 获取所有收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXConsigneeAddress(HttpContext context)
        {

            var data = this.bllMall.GetConsigneeAddressList(currentUserInfo.UserID);
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 添加收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXConsigneeAddress(HttpContext context)
        {
            string msg = "";
            if (bllMall.AddConsigneeAddress(currentUserInfo.UserID, context, out msg))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Status = 0;
            }
            resp.Msg = msg;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 编辑收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXConsigneeAddress(HttpContext context)
        {
            string Msg = "";
            if (bllMall.EditConsigneeAddress(currentUserInfo.UserID, context, out Msg))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Status = 0;
            }
            resp.Msg = Msg;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXConsigneeAddress(HttpContext context)
        {

            if (bllMall.DeleteConsigneeAddress(currentUserInfo.UserID, context.Request["id"]))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Status = 0;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 根据域名查询门店列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallStoresByDoMain(HttpContext context)
        {

            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}'", bllMall.WebsiteOwner));
            var data = this.bllMall.GetList<WXMallStores>(sbWhere.ToString());
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CancelOrder(HttpContext context)
        {
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(context.Request["orderid"]);
            if (orderInfo == null)
            {
                resp.Status = -1;
                resp.Msg = "不存在的订单";
                goto outoff;
            }
            if ((!orderInfo.OrderUserID.Equals(currentUserInfo.UserID)) || (!orderInfo.WebsiteOwner.Equals(bllMall.WebsiteOwner)))
            {
                resp.Status = -1;
                resp.Msg = "拒绝修改";
                goto outoff;

            }
            //if (!orderInfo.Status.Equals("待付款"))
            //{
            //    resp.Status = -1;
            //    resp.Msg = string.Format("订单状态为 {0} 不允许修改", orderInfo.Status);
            //    goto outoff;

            //}

            if (bllMall.UpdateOrderStatus(context.Request["orderid"], "已取消"))
            {
                resp.Status = 1;
                //库存返还
                foreach (var item in bllMall.GetOrderDetailsList(orderInfo.OrderID))
                {
                    bllMall.UpdateProductStock(int.Parse(item.PID), (-item.TotalCount));
                }


            }
            else
            {
                resp.Status = 0;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateOrderComplete(HttpContext context)
        {
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(context.Request["orderid"]);
            if (orderInfo == null)
            {
                resp.Status = -1;
                resp.Msg = "不存在的订单";
                goto outoff;
            }
            if ((!orderInfo.OrderUserID.Equals(currentUserInfo.UserID)) || (!orderInfo.WebsiteOwner.Equals(bllMall.WebsiteOwner)))
            {
                resp.Status = -1;
                resp.Msg = "拒绝修改";
                goto outoff;

            }
            if (!orderInfo.Status.Equals("已发货"))
            {
                resp.Status = -1;
                resp.Msg = string.Format("订单状态不是已发货状态 {0} 不允许修改", orderInfo.Status);
                goto outoff;


            }
            if (bllMall.UpdateOrderStatus(context.Request["orderid"], "交易成功"))
            {
                resp.Status = 1;

                #region 交易成功加积分
                //更新会员积分记录积分
                int addScore = (int)Math.Ceiling(orderInfo.TotalAmount);//原始获得的积分
                bllMall.AddUserTotalScore(orderInfo.OrderUserID, addScore);
                //插入积分记录

                WXMallScoreRecord scoreRecord = new WXMallScoreRecord();
                scoreRecord.InsertDate = DateTime.Now;
                scoreRecord.Remark = "微商城-购物";
                scoreRecord.Score = addScore;
                scoreRecord.UserId = orderInfo.OrderUserID;
                scoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                scoreRecord.OrderID = orderInfo.OrderID;
                scoreRecord.Type = 1;
                bllMall.Add(scoreRecord);


                //if (orderInfo.DeliveryTime != null)
                //{
                //    TimeSpan timeSpan = (Convert.ToDateTime(orderInfo.DeliveryTime)) - Convert.ToDateTime(orderInfo.InsertDate);
                //    if (timeSpan.Days >= 1)
                //    {
                //        addScore *= 2;
                //    }

                //    bllMall.AddUserTotalScore(orderInfo.OrderUserID, addScore);

                //    //插入积分记录
                //    WXMallScoreRecord ScoreRecord = new WXMallScoreRecord();
                //    ScoreRecord.InsertDate = DateTime.Now;
                //    ScoreRecord.Remark = "微商城-购物";
                //    ScoreRecord.Score = addScore;
                //    ScoreRecord.UserId = orderInfo.OrderUserID;
                //    ScoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                //    ScoreRecord.OrderID = orderInfo.OrderID;
                //    ScoreRecord.Type = 1;
                //    bllMall.Add(ScoreRecord);


                //    //检查是否有积分奖励
                //    var ScoreConfig = new BllScore().GetScoreConfig();
                //    if (ScoreConfig != null)
                //    {
                //        if (ScoreConfig.OrderDate != null && ScoreConfig.OrderDateTotalAmount != null && ScoreConfig.OrderScore != null)
                //        {
                //            if ((Convert.ToDateTime(ScoreConfig.OrderDate) - DateTime.Now).Days.Equals(0))
                //            {
                //                if (orderInfo.TotalAmount >= ScoreConfig.OrderDateTotalAmount)
                //                {
                //                    //插入积分记录
                //                    WXMallScoreRecord jiangLiScoreRecord = new WXMallScoreRecord();
                //                    jiangLiScoreRecord.InsertDate = DateTime.Now;
                //                    jiangLiScoreRecord.Remark = "积分额外奖励";
                //                    jiangLiScoreRecord.Score = (int)ScoreConfig.OrderScore;
                //                    jiangLiScoreRecord.UserId = orderInfo.OrderUserID;
                //                    jiangLiScoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                //                    jiangLiScoreRecord.OrderID = orderInfo.OrderID;
                //                    jiangLiScoreRecord.Type = 1;
                //                    bllMall.Add(jiangLiScoreRecord);
                //                    bllMall.AddUserTotalScore(orderInfo.OrderUserID, jiangLiScoreRecord.Score);
                //                }
                //            }
                //        }
                //    }

                //    //




                //}

                #endregion

                //#region 分销订单打款
                //if (bllMall.IsDistributionMall)//当前是分销商城
                //{
                //    int status = 3;
                //    if (orderInfo.DistributionStatus.Equals(status))
                //    {
                //        resp.Msg = "订单状态与原订单状态不能相同";
                //        goto outoff;
                //    }
                //    int count = bllMall.Update(new WXMallOrderInfo(), string.Format(" DistributionStatus={0}", status), string.Format(" OrderID='{0}' And WebsiteOwner='{1}'", orderInfo.OrderID, bllMall.WebsiteOwner));
                //    if (count > 0)
                //    {
                //        goto outoff;
                //        //if (!bllDis.UpdateDistributionOrderComplete(orderInfo))
                //        //{
                //        //    bllMall.Update(orderInfo);//订单状态还原
                //        //    resp.Msg = "给上级用户充值失败";
                //        //    goto outoff;
                //        //}
                //        //else
                //        //{
                //        //    resp.Status = 1;

                //        //}
                //    }
                //    else
                //    {

                //        resp.Msg = "更新分销订单状态失败";
                //    }

                //}
                //#endregion
            }
            else
            {
                resp.Status = 0;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 取消积分订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CancelScoreOrder(HttpContext context)
        {
            WXMallScoreOrderInfo orderInfo = bllMall.GetScoreOrderInfo(context.Request["orderid"]);
            if (orderInfo == null)
            {
                resp.Status = -1;
                resp.Msg = "不存在的订单";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if ((!orderInfo.OrderUserID.Equals(currentUserInfo.UserID)) || (!orderInfo.WebsiteOwner.Equals(bllMall.WebsiteOwner)))
            {
                resp.Status = -1;
                resp.Msg = "拒绝修改";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!orderInfo.Status.Equals("待付款"))
            {
                resp.Status = -1;
                resp.Msg = string.Format("订单状态为 {0} 不允许修改", orderInfo.Status);
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (bllMall.UpdateScoreOrderStatus(context.Request["orderid"], "已取消"))
            {
                //返还积分
                //if (bllUser.WebsiteOwner.Equals("wubuhui"))
                //{
                //    currUserInfo.TotalScore += (double)Math.Floor(orderInfo.TotalAmount/2);//五步会取消只返还50%;
                //}
                //else
                //{
                currentUserInfo.TotalScore += (int)orderInfo.TotalAmount;
                //}


                bllMall.Update(currentUserInfo, string.Format(" TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID));
                foreach (var item in bllMall.GetScoreOrderDetailsList(orderInfo.OrderID))
                {
                    bllMall.UpdateScoreProductStock(int.Parse(item.PID), (-item.TotalCount));

                }
                //if (bllMall.WebsiteOwner.Equals("wubuhui"))
                //{
                //    //提醒
                //    WBHScoreRecord scoreRecord = new WBHScoreRecord();
                //    scoreRecord.NameStr = "取消积分订单";
                //    scoreRecord.ScoreNum = "+" + Math.Floor(orderInfo.TotalAmount).ToString();
                //    scoreRecord.InsertDate = DateTime.Now;
                //    scoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                //    scoreRecord.Nums = "b55";
                //    scoreRecord.UserId = orderInfo.OrderUserID;
                //    scoreRecord.RecordType = "2";
                //    bllMall.Add(scoreRecord);
                //    //


                //    //发送微信模板消息，订单状态通知
                //    BLLWeixin.TMOderStatusUpdateNotification notificaiton = new BLLWeixin.TMOderStatusUpdateNotification();
                //    notificaiton.Url = string.Format("http://{0}/WuBuHui/Score/Score.aspx", context.Request.Url.Host);
                //    notificaiton.First = "您已取消积分订单";
                //    notificaiton.OrderSn = orderInfo.OrderID;
                //    notificaiton.OrderStatus = "取消订单";
                //    notificaiton.Remark = "点击详情进入订单中心查看";
                //    bllWeixin.SendTemplateMessage(bllWeixin.GetAccessToken(), currentUserInfo.WXOpenId, notificaiton);

                //}
                resp.Status = 1;
                resp.Msg = "操作成功！";


            }
            else
            {
                resp.Status = 0;
                resp.Msg = "操作失败！";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 赠送积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GiveScoreToOtherAccount(HttpContext context)
        {
            string targetAccount = context.Request["TargetAccount"];
            string score = context.Request["Score"];
            int scoreInt = 0;
            if (!int.TryParse(score, out scoreInt))
            {
                resp.Status = 0;
                resp.Msg = "积分需大于0";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (scoreInt <= 0)
            {
                resp.Status = 0;
                resp.Msg = "积分需大于0";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo tagetUserInfo = bllUser.GetUserInfo(targetAccount, bllMall.WebsiteOwner);
            if (tagetUserInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "不存在的账号";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (currentUserInfo.UserID.Equals(tagetUserInfo.UserID))
            {
                resp.Status = 0;
                resp.Msg = "您不能给自己赠送积分";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (currentUserInfo.TotalScore < scoreInt)
            {
                resp.Status = 0;
                resp.Msg = "您的积分不足";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            currentUserInfo.TotalScore -= scoreInt;
            tagetUserInfo.TotalScore += scoreInt;
            //开始事务
            if (bllMall.Update(currentUserInfo, string.Format(" TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID), tran) < 1)
            {
                tran.Rollback();
                resp.Status = 0;
                resp.Msg = "操作失败";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (bllMall.Update(tagetUserInfo, string.Format(" TotalScore={0}", tagetUserInfo.TotalScore), string.Format(" AutoID={0}", tagetUserInfo.AutoID), tran) < 1)
            {
                tran.Rollback();
                resp.Status = 0;
                resp.Msg = "操作失败";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            WXMallScoreRecord recordCurrUser = new WXMallScoreRecord();
            recordCurrUser.InsertDate = DateTime.Now;
            recordCurrUser.Remark = "积分赠送";
            recordCurrUser.Score = -scoreInt;
            recordCurrUser.Type = 3;
            recordCurrUser.UserId = currentUserInfo.UserID;
            recordCurrUser.WebsiteOwner = bllMall.WebsiteOwner;

            WXMallScoreRecord recordTargetUser = new WXMallScoreRecord();
            recordTargetUser.InsertDate = DateTime.Now;
            recordTargetUser.Remark = "积分赠送";
            recordTargetUser.Score = scoreInt;
            recordTargetUser.Type = 3;
            recordTargetUser.UserId = tagetUserInfo.UserID;
            recordTargetUser.WebsiteOwner = bllMall.WebsiteOwner;
            if (!bllMall.Add(recordCurrUser, tran))
            {
                tran.Rollback();
                resp.Status = 0;
                resp.Msg = "操作失败";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!bllMall.Add(recordTargetUser, tran))
            {
                tran.Rollback();
                resp.Status = 0;
                resp.Msg = "操作失败";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            tran.Commit();
            resp.Status = 1;
            resp.Msg = "操作成功";
            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 获取物流费用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CalcTransportFee(HttpContext context)
        {
            string pIds = context.Request["PIds"];
            string deliveryAutoId = context.Request["DeliveryAutoId"];
            SubmitWxMallOrderPidsModel pIdsModel = Common.JSONHelper.JsonToModel<SubmitWxMallOrderPidsModel>(pIds);
            WXMallDelivery delivery = bllMall.GetDelivery(int.Parse(deliveryAutoId));
            int totalProductCount = 0;
            decimal totalTransportFee = 0;
            foreach (var item in pIdsModel.Pids)
            {

                totalProductCount += Convert.ToInt32(item[1]);

            }
            if (delivery.DeliveryType.Equals(0))//计算快递费用
            {
                if (totalProductCount <= delivery.InitialProductCount)
                {
                    totalTransportFee = delivery.InitialDeliveryMoney;
                }
                else
                {

                    totalTransportFee = delivery.InitialDeliveryMoney + Math.Ceiling((decimal)(totalProductCount - delivery.InitialProductCount) / delivery.AddProductCount) * delivery.AddMoney;

                }
            }
            resp.Status = 1;
            resp.ExStr = totalTransportFee.ToString();
            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 添加优惠券 土澳
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCoupon(HttpContext context)
        {
            string discountStr = context.Request["Discount"];
            float discountF = float.Parse(discountStr);
            if (discountF > 0 && discountF < 10)
            {
                if (string.IsNullOrEmpty(currentUserInfo.TagName) || (!currentUserInfo.TagName.StartsWith("JXS")))
                {

                    resp.Msg = "经销商才可以添加优惠券";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                resp.Status = bllMall.AddCoupon(currentUserInfo, discountF) == true ? 1 : 0;

            }
            else
            {
                resp.Msg = "折扣在0-10之间不包含0与10";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 查询经销商发放的优惠券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyCoupon(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            int totalCount;
            resp.ExObj = bllMall.GetCouponList(pageIndex, pageSize, out totalCount, currentUserInfo.UserID);
            resp.ExInt = totalCount;
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 添加商品收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddProductCollect(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            var productInfo = bllMall.GetProduct(context.Request["PId"]);
            if (productInfo == null || (!productInfo.WebsiteOwner.Equals(currentUserInfo.WebsiteOwner)))
            {
                resp.Msg = "商品不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //检查是否已经收藏
            if (bllCommRela.ExistRelation(BLLJIMP.Enums.CommRelationType.ProductCollect, currentUserInfo.UserID, productInfo.PID))
            {
                resp.Msg = "商品已收藏";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //检查是否已经收藏

            if (bllCommRela.AddCommRelation(BLLJIMP.Enums.CommRelationType.ProductCollect, currentUserInfo.UserID, productInfo.PID))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "收藏失败,请重试";
            }


            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 删除商品收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteProductCollect(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.Msg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (bllCommRela.DelCommRelation(BLLJIMP.Enums.CommRelationType.ProductCollect, currentUserInfo.UserID, context.Request["PId"]))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "删除失败,请重试";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取我的商品收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyProductCollect(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            var productList = new List<WXMallProductInfo>();
            var relationData = bllCommRela.GetLit<CommRelationInfo>(pageSize, pageIndex, string.Format("MainId='{0}' And RelationType='ProductCollect'", currentUserInfo.UserID), "AutoId Desc");
            foreach (var item in relationData)
            {
                var productInfo = bllMall.GetProduct(item.RelationId);
                if (productInfo != null)
                {
                    productInfo.PDescription = null;
                    productList.Add(productInfo);

                }
            }
            resp.ExObj = productList;
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 添加活动订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddActivityOrder(HttpContext context)
        {
            string activityId = context.Request["ActivityId"];//真实活动ID
            string itemId = context.Request["ItemId"];//选项ID
            string useScore = context.Request["UseScore"];//使用积分
            string myCouponId = context.Request["MyCouponId"];//我的优惠券
            string phone = context.Request["Phone"];//手机
            string name = context.Request["Name"];//姓名
            string useAmountStr = context.Request["UseAmount"];//使用余额

            int useScoreInt = 0;
            decimal useAmount = 0;
            if (!string.IsNullOrEmpty(useScore))
            {
                useScoreInt = int.Parse(useScore);
            }

            if (!string.IsNullOrEmpty(useAmountStr))
            {
                useAmount = decimal.Parse(useAmountStr);
            }

            CrowdFundItem item = bllUser.Get<CrowdFundItem>(string.Format(" ItemId='{0}'", itemId));
            if (item == null)
            {
                resp.Msg = "请选择选项";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivity(item.CrowdFundID);
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            //orderInfo.Address = orderRequestModel.receiver_address;
            orderInfo.Consignee = name;
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.OrderUserID = currentUserInfo.UserID;
            orderInfo.Phone = phone;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            //orderInfo.Transport_Fee = 0;
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            //orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            //orderInfo.ReceiverProvince = orderRequestModel.receiver_province;
            //orderInfo.ReceiverProvinceCode = orderRequestModel.receiver_province_code.ToString();
            //orderInfo.ReceiverCity = orderRequestModel.receiver_city;
            //orderInfo.ReceiverCityCode = orderRequestModel.receiver_city_code.ToString();
            //orderInfo.ReceiverDist = orderRequestModel.receiver_dist;
            //orderInfo.ReceiverDistCode = orderRequestModel.receiver_dist_code.ToString();
            //orderInfo.ZipCode = orderRequestModel.receiver_zip;
            orderInfo.MyCouponCardId = myCouponId;
            orderInfo.UseScore = useScoreInt;
            orderInfo.UseAmount = useAmount;
            orderInfo.Status = "待付款";
            orderInfo.ArticleCategoryType = "Mall";
            orderInfo.OrderType = 4;
            //orderInfo.UseAmount = orderRequestModel.use_amount;
            orderInfo.Ex1 = juActivityInfo.ActivityName;
            orderInfo.Ex2 = item.ProductName;
            orderInfo.Ex3 = item.Amount.ToString();
            orderInfo.OrderMemo = string.Format("活动名称:{0}购买选项:{1}选项金额:{2}", juActivityInfo.ActivityName, item.ProductName, item.Amount);

            #region 优惠券计算

            StoredValueCardRecord storeValueCardRecord = new StoredValueCardRecord();
            decimal discountAmount = 0;//优惠券优惠金额
            string msg = "";
            string couponName = "";
            if (!string.IsNullOrEmpty(myCouponId))//有优惠券
            {

                bool isSuccess;
                //discountAmount = bllMall.CalcDiscountAmount(myCouponId, item.Amount, currentUserInfo.UserID, out isSuccess, out msg, out couponName);//优惠券优惠金额
                //if (!isSuccess)
                //{
                //    resp.Msg = msg;
                //    return Common.JSONHelper.ObjectToJson(resp);
                //}
                //
                MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(myCouponId), currentUserInfo.UserID);
                if (myCardCoupon != null)//优惠券
                {
                    discountAmount = bllMall.CalcDiscountAmount(myCouponId, item.Amount, currentUserInfo.UserID, out isSuccess, out msg, out couponName);//优惠券优惠金额
                    if (!isSuccess)
                    {
                        resp.Msg = msg;
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                else//储值卡
                {
                    ////检查是否使用了积分余额
                    //if (useScoreInt > 0 || useAmount > 0)
                    //{
                    //    resp.Msg = "使用储值卡时,不可再用积分或余额抵扣,请取消使用积分或余额抵扣";
                    //    return Common.JSONHelper.ObjectToJson(resp);
                    //}
                    storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", myCouponId));
                    discountAmount = bllMall.CalcDiscountAmountStoreValue(item.Amount, storeValueCardRecord.UserId, myCouponId);
                }
                //



            }

            #endregion

            #region 积分计算
            decimal scoreExchangeAmount = 0;///积分抵扣的金额
            //积分计算
            if (useScoreInt > 0)
            {
                if (currentUserInfo.TotalScore < useScoreInt)
                {
                    //resp.Status = 1;
                    resp.Msg = "积分不足";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                if (scoreConfig != null && scoreConfig.ExchangeAmount > 0)
                {
                    scoreExchangeAmount = Math.Round(useScoreInt / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);

                }


            }

            //积分计算 
            #endregion

            orderInfo.TotalAmount = item.Amount;//选项金额
            orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额
            orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
            orderInfo.TotalAmount -= useAmount;//余额抵扣金额
            if (orderInfo.TotalAmount < 0)
            {
                orderInfo.TotalAmount = 0;

            }
            if (orderInfo.TotalAmount == 0)
            {
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;
                orderInfo.Status = "待发货";


            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderInfo, tran))
                {
                    tran.Rollback();

                    resp.Msg = "提交失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }



                #region 积分抵扣
                //积分扣除
                if (useScoreInt > 0)
                {
                    currentUserInfo.TotalScore -= useScoreInt;
                    if (bllMall.Update(currentUserInfo, string.Format(" TotalScore-={0}", useScoreInt), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        resp.Msg = "更新用户积分失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    //积分记录
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -useScoreInt;
                    scoreRecord.TotalScore = currentUserInfo.TotalScore;
                    scoreRecord.ScoreType = "OrderSubmit";
                    scoreRecord.UserID = currentUserInfo.UserID;
                    scoreRecord.AddNote = "微商城-下单使用积分";
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        resp.Msg = "插入积分记录失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }



                }




                //积分扣除 
                #endregion

                #region 更新优惠券使用状态
                //优惠券
                if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))//有优惠券且已经成功使用
                {
                    MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), currentUserInfo.UserID);
                    if (myCardCoupon!=null)//优惠券
                    {
                        myCardCoupon.UseDate = DateTime.Now;
                        myCardCoupon.Status = 1;
                        if (!bllCardCoupon.Update(myCardCoupon, tran))
                        {
                            tran.Rollback();
                            resp.Msg = "更新优惠券状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                    }
                    else//储值卡
                    {

                        StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And (UserId='{1}' Or ToUserId='{1}')", myCouponId, currentUserInfo.UserID));
                        myStoredValueCardRecord.UseDate = DateTime.Now;
                        myStoredValueCardRecord.Status = 9;
                        if (!bllStoreValue.Update(myStoredValueCardRecord, tran))
                        {
                            tran.Rollback();
                            resp.Msg = "更新储值卡状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        StoredValueCardUseRecord storeValueCardUseRecord = new StoredValueCardUseRecord();
                        storeValueCardUseRecord.UseAmount = discountAmount;
                        storeValueCardUseRecord.CardId = myStoredValueCardRecord.CardId;
                        storeValueCardUseRecord.Remark = string.Format("活动报名,{0}使用{1}元", juActivityInfo.ActivityName, Math.Round(discountAmount, 2));
                        storeValueCardUseRecord.UseDate = DateTime.Now;
                        storeValueCardUseRecord.UserId = storeValueCardRecord.UserId;
                        storeValueCardUseRecord.WebsiteOwner = orderInfo.WebsiteOwner;
                        storeValueCardUseRecord.MyCardId = int.Parse(myCouponId);
                        storeValueCardUseRecord.OrderId = orderInfo.OrderID;
                        storeValueCardUseRecord.UseUserId = currentUserInfo.UserID;
                        if (!bllStoreValue.Add(storeValueCardUseRecord, tran))
                        {
                            tran.Rollback();
                            resp.Msg = "更新储值卡状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }


                    }


                }

                //优惠券 
                #endregion

                #region 余额抵扣

                if (orderInfo.UseAmount > 0 && bllMall.IsEnableAccountAmountPay())
                {
                    currentUserInfo.AccountAmount -= orderInfo.UseAmount;
                    if (bllMall.Update(currentUserInfo, string.Format(" AccountAmount={0}", currentUserInfo.AccountAmount), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        resp.Msg = "更新用户余额失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }


                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = (double)orderInfo.UseAmount;
                    scoreRecord.TotalScore = (double)currentUserInfo.AccountAmount;
                    scoreRecord.UserID = currentUserInfo.UserID;
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                    scoreRecord.AddNote = "活动订单-下单消耗余额";
                    scoreRecord.ScoreType = "AccountAmount";
                    if (!bllMall.Add(scoreRecord))
                    {

                    }

                    UserCreditAcountDetails record = new UserCreditAcountDetails();
                    record.WebsiteOwner = bllUser.WebsiteOwner;
                    record.Operator = currentUserInfo.UserID;
                    record.UserID = currentUserInfo.UserID;
                    record.CreditAcount = -orderInfo.UseAmount;
                    record.SysType = "AccountAmount";
                    record.AddTime = DateTime.Now;
                    record.AddNote = "账户余额变动-" + orderInfo.UseAmount;
                    if (!bllMall.Add(record))
                    {
                        tran.Rollback();

                        resp.Msg = "插入余额记录失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                }


                #endregion

                #region 更新报名数据订单号
                ActivityDataInfo data = bllUser.Get<ActivityDataInfo>(string.Format(" ActivityId='{0}' And (UserId='{1}' Or WeixinOpenID='{2}') And IsDelete=0 Order By InsertDate DESC", activityId, currentUserInfo.UserID, currentUserInfo.WXOpenId));
                if (data == null)
                {
                    tran.Rollback();
                    resp.Msg = "您还未报名";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                data.OrderId = orderInfo.OrderID;
                data.IsFee = 1;
                data.ItemName = item.ProductName;
                data.ItemAmount = item.Amount;
                data.CouponName = couponName;
                data.UseScore = useScoreInt;
                data.UseAmount = useAmount;
                data.Amount = orderInfo.TotalAmount;
                if (orderInfo.TotalAmount == 0)
                {
                    data.PaymentStatus = 1;

                }
                if (!bllUser.Update(data))
                {
                    tran.Rollback();
                    resp.Msg = "更新报名数据失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                //更新报名数据订单号 
                #endregion

                tran.Commit();//提交订单事务



            }
            catch (Exception ex)
            {
                //回滚事物
                tran.Rollback();
                resp.Msg = "提交订单失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            resp.Status = 1;
            resp.ExStr = orderInfo.OrderID;
            resp.ExObj = orderInfo.TotalAmount;
            resp.Msg = "ok";
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 修改活动订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateActivityOrder(HttpContext context)
        {
            bool isReNewOrder = false;

            string activityId = context.Request["ActivityId"];//真实活动ID
            string itemId = context.Request["ItemId"];//购买选项
            string myCouponId = context.Request["MyCouponId"];//我的优惠券
            string useScore = context.Request["UseScore"];//使用积分
            string useAmountStr = context.Request["UseAmount"];//使用余额
            string orderId = context.Request["OrderId"];//订单号

            int useScoreInt = 0;//使用积分
            decimal useAmount = 0;//使用余额

            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);

            WXMallOrderInfo oldOrderInfo = orderInfo;

            if (orderInfo == null)
            {
                //订单为空，则重新下单
                orderInfo = new WXMallOrderInfo();
                orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);

                orderInfo.Consignee = currentUserInfo.TrueName;
                orderInfo.InsertDate = DateTime.Now;
                orderInfo.OrderUserID = currentUserInfo.UserID;
                orderInfo.Phone = currentUserInfo.Phone;
                orderInfo.WebsiteOwner = bllMall.WebsiteOwner;

                oldOrderInfo = orderInfo;
                isReNewOrder = true;
            }
            else if (orderInfo.OrderUserID != currentUserInfo.UserID)
            {

                resp.Msg = "订单号无效";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }


            if (!string.IsNullOrEmpty(useScore))
            {
                useScoreInt = int.Parse(useScore);
            }
            if (!string.IsNullOrEmpty(useAmountStr))
            {
                useAmount = decimal.Parse(useAmountStr);
            }
            CrowdFundItem item = bllUser.Get<CrowdFundItem>(string.Format(" ItemId='{0}'", itemId));
            if (item == null)
            {
                resp.Msg = "请选择选项";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivity(item.CrowdFundID);

            //orderInfo.Address = orderRequestModel.receiver_address;
            //orderInfo.Consignee = name;
            //orderInfo.InsertDate = DateTime.Now;
            //orderInfo.OrderUserID = currentUserInfo.UserID;
            //orderInfo.Phone = phone;
            //orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            //orderInfo.Transport_Fee = 0;
            //orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            //orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            //orderInfo.ReceiverProvince = orderRequestModel.receiver_province;
            //orderInfo.ReceiverProvinceCode = orderRequestModel.receiver_province_code.ToString();
            //orderInfo.ReceiverCity = orderRequestModel.receiver_city;
            //orderInfo.ReceiverCityCode = orderRequestModel.receiver_city_code.ToString();
            //orderInfo.ReceiverDist = orderRequestModel.receiver_dist;
            //orderInfo.ReceiverDistCode = orderRequestModel.receiver_dist_code.ToString();
            //orderInfo.ZipCode = orderRequestModel.receiver_zip;
            orderInfo.MyCouponCardId = myCouponId;
            orderInfo.UseScore = useScoreInt;
            orderInfo.UseAmount = useAmount;
            orderInfo.Status = "待付款";
            orderInfo.ArticleCategoryType = "Mall";
            orderInfo.OrderType = 4;
            //orderInfo.UseAmount = orderRequestModel.use_amount;
            orderInfo.Ex1 = juActivityInfo.ActivityName;
            orderInfo.Ex2 = item.ProductName;
            orderInfo.Ex3 = item.Amount.ToString();
            orderInfo.OrderMemo = string.Format("活动名称:{0}购买选项:{1}选项金额:{2}", juActivityInfo.ActivityName, item.ProductName, item.Amount);

            #region 优惠券计算
            StoredValueCardRecord storeValueCardRecord = new StoredValueCardRecord();
            decimal discountAmount = 0;//优惠金额
            string msg = "";
            string couponName = "";
            if (!string.IsNullOrEmpty(myCouponId))//有优惠券
            {

                bool isSuccess;
                MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(myCouponId), currentUserInfo.UserID);
                if (myCardCoupon != null)//优惠券
                {
                    discountAmount = bllMall.CalcDiscountAmount(myCouponId, item.Amount, currentUserInfo.UserID, out isSuccess, out msg, out couponName);//优惠券优惠金额
                    if (!isSuccess)
                    {
                        resp.Msg = msg;
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                else//储值卡
                {
                    ////检查是否使用了积分余额
                    //if (useScoreInt > 0 || useAmount > 0)
                    //{
                    //    resp.Msg = "使用储值卡时,不可再用积分或余额抵扣,请取消使用积分或余额抵扣";
                    //    return Common.JSONHelper.ObjectToJson(resp);
                    //}

                    storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", myCouponId));
                    discountAmount = bllMall.CalcDiscountAmountStoreValue(item.Amount, storeValueCardRecord.UserId, myCouponId);
                }
                //

            }

            #endregion

            #region 积分计算
            decimal scoreExchangeAmount = 0;///积分抵扣的金额
            //积分计算
            if (useScoreInt > 0)
            {
                if (currentUserInfo.TotalScore < useScoreInt)
                {
                    //resp.Status = 1;
                    resp.Msg = "积分不足";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                if (scoreConfig != null && scoreConfig.ExchangeAmount > 0)
                {
                    scoreExchangeAmount = Math.Round(useScoreInt / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);
                }

            }



            //积分计算 
            #endregion

            orderInfo.TotalAmount = item.Amount;//选项金额
            orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额
            orderInfo.TotalAmount -= scoreExchangeAmount;//积分抵扣金额
            orderInfo.TotalAmount -= useAmount;//使用余额
            if (orderInfo.TotalAmount < 0)
            {
                orderInfo.TotalAmount = 0;

            }
            if (orderInfo.TotalAmount == 0)
            {
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;
                orderInfo.Status = "待发货";
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (isReNewOrder)
                {
                    if (!this.bllMall.Add(orderInfo, tran))
                    {
                        tran.Rollback();
                        resp.Msg = "提交失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                else
                {
                    if (!this.bllMall.Update(orderInfo, tran))
                    {
                        tran.Rollback();
                        resp.Msg = "提交失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                }

                #region 积分抵扣
                //积分扣除
                if (useScoreInt > 0)
                {
                    currentUserInfo.TotalScore -= useScoreInt;
                    if (bllMall.Update(currentUserInfo, string.Format(" TotalScore-={0}", useScoreInt), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        resp.Msg = "更新用户积分失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    //积分记录
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -useScoreInt;
                    scoreRecord.TotalScore = currentUserInfo.TotalScore;
                    scoreRecord.ScoreType = "OrderSubmit";
                    scoreRecord.UserID = currentUserInfo.UserID;
                    scoreRecord.AddNote = "微商城-下单使用积分";
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        resp.Msg = "插入积分记录失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }



                }




                //积分扣除 
                #endregion

                #region 更新优惠券使用状态
                //优惠券
                if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))//有优惠券且已经成功使用
                {
                    MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), currentUserInfo.UserID);
                    if (myCardCoupon != null)//优惠券
                    {
                        myCardCoupon.UseDate = DateTime.Now;
                        myCardCoupon.Status = 1;
                        if (!bllCardCoupon.Update(myCardCoupon, tran))
                        {
                            tran.Rollback();
                            resp.Msg = "更新优惠券状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                    }
                    else//储值卡
                    {

                        StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And (UserId='{1}' Or ToUserId='{1}')", myCouponId, currentUserInfo.UserID));
                        myStoredValueCardRecord.UseDate = DateTime.Now;
                        myStoredValueCardRecord.Status = 9;
                        if (!bllStoreValue.Update(myStoredValueCardRecord, tran))
                        {
                            tran.Rollback();
                            resp.Msg = "更新储值卡状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        StoredValueCardUseRecord storeValueCardUseRecord = new StoredValueCardUseRecord();
                        storeValueCardUseRecord.UseAmount = discountAmount;
                        storeValueCardUseRecord.CardId = myStoredValueCardRecord.CardId;
                        storeValueCardUseRecord.Remark = string.Format("活动报名,{0}使用{1}元", juActivityInfo.ActivityName, Math.Round(discountAmount, 2));
                        storeValueCardUseRecord.UseDate = DateTime.Now;
                        storeValueCardUseRecord.UserId = storeValueCardRecord.UserId;
                        storeValueCardUseRecord.WebsiteOwner = orderInfo.WebsiteOwner;
                        storeValueCardUseRecord.MyCardId = int.Parse(myCouponId);
                        storeValueCardUseRecord.OrderId = orderInfo.OrderID;
                        storeValueCardUseRecord.UseUserId = currentUserInfo.UserID;
                        if (!bllStoreValue.Add(storeValueCardUseRecord, tran))
                        {
                            tran.Rollback();
                            resp.Msg = "更新储值卡状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }


                    }

                }

                //优惠券 
                #endregion

                #region 余额抵扣

                if (orderInfo.UseAmount > 0 && bllMall.IsEnableAccountAmountPay())
                {
                    currentUserInfo.AccountAmount -= orderInfo.UseAmount;
                    if (bllMall.Update(currentUserInfo, string.Format(" AccountAmount={0}", currentUserInfo.AccountAmount), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        resp.Msg = "更新用户余额失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -(double)orderInfo.UseAmount;
                    scoreRecord.TotalScore = (double)currentUserInfo.AccountAmount;
                    scoreRecord.UserID = currentUserInfo.UserID;
                    scoreRecord.AddNote = "账户余额变动-下单使用余额";
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                    scoreRecord.ScoreType = "AccountAmount";
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        resp.Msg = "插入余额记录失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    UserCreditAcountDetails record = new UserCreditAcountDetails();
                    record.WebsiteOwner = bllUser.WebsiteOwner;
                    record.Operator = currentUserInfo.UserID;
                    record.UserID = currentUserInfo.UserID;
                    record.CreditAcount = -orderInfo.UseAmount;
                    record.SysType = "AccountAmount";
                    record.AddTime = DateTime.Now;
                    record.AddNote = "账户余额变动-" + orderInfo.UseAmount;
                    if (!bllMall.Add(record))
                    {
                        tran.Rollback();

                        resp.Msg = "插入余额记录失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }



                }


                #endregion


                #region 更新报名数据订单号
                ActivityDataInfo data = bllUser.Get<ActivityDataInfo>(string.Format(" ActivityId='{0}' And UserId='{1}' And IsDelete=0 Order By InsertDate DESC", activityId, currentUserInfo.UserID));
                if (data == null)
                {
                    tran.Rollback();
                    resp.Msg = "您还未报名";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                data.OrderId = orderInfo.OrderID;
                data.IsFee = 1;
                data.ItemName = item.ProductName;
                data.ItemAmount = item.Amount;
                data.CouponName = couponName;
                data.UseScore = useScoreInt;
                data.UseAmount = useAmount;
                data.Amount = orderInfo.TotalAmount;
                if (orderInfo.TotalAmount == 0)
                {
                    data.PaymentStatus = 1;
                }
                if (!bllUser.Update(data))
                {
                    tran.Rollback();
                    resp.Msg = "更新报名数据失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                //更新报名数据订单号 
                #endregion

                tran.Commit();//提交订单事务

                if (!isReNewOrder)
                {
                    //重新创建订单的，无需返还

                    #region 原积分返还
                    if (oldOrderInfo.UseScore > 0)//
                    {
                        bllUser.Update(currentUserInfo, string.Format(" TotalScore+={0}", oldOrderInfo.UseScore), string.Format(" AutoId={0}", currentUserInfo.AutoID));
                    }
                    #endregion

                    #region 原余额返还
                    if (oldOrderInfo.UseAmount > 0)//原余额返还
                    {
                        bllUser.Update(currentUserInfo, string.Format(" AccountAmount+={0}", oldOrderInfo.UseAmount), string.Format(" AutoId={0}", currentUserInfo.AutoID));
                    }
                    #endregion

                    #region 原优惠券返还
                    if (!string.IsNullOrEmpty(oldOrderInfo.MyCouponCardId))
                    {

                        var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(oldOrderInfo.MyCouponCardId), currentUserInfo.UserID);
                        if (myCardCoupon != null && myCardCoupon.Status == 1)
                        {
                            myCardCoupon.Status = 0;
                            if (!bllCardCoupon.Update(myCardCoupon))
                            {


                                resp.Msg = "返还优惠券失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                            }

                        }

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                //回滚事物
                tran.Rollback();
                resp.Msg = "提交订单失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            resp.Status = 1;
            resp.ExStr = orderInfo.OrderID;
            resp.ExObj = orderInfo.TotalAmount;
            resp.Msg = "ok";
            return Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        /// 计算应付金额
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CalcActivityPayAmount(HttpContext context)
        {
            string itemId = context.Request["ItemId"];//选项ID
            string useScore = context.Request["UseScore"];//使用积分
            string myCouponId = context.Request["MyCouponId"];//优惠券编号
            string useAmountStr = context.Request["UseAmount"];//使用余额

            decimal totalAmount = 0;//应付金额
            int useScoreInt = 0;
            decimal useAmount = 0;

            CrowdFundItem item = bllUser.Get<CrowdFundItem>(string.Format(" ItemId='{0}'", itemId));

            #region 积分计算
            decimal scoreExchangeAmount = 0;///积分抵扣的金额
            //积分计算
            if (!string.IsNullOrEmpty(useScore))
            {

                useScoreInt = int.Parse(useScore);
                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                if (scoreConfig != null & scoreConfig.ExchangeAmount == 0)
                {
                    scoreExchangeAmount = 0;
                }
                else
                {
                    scoreExchangeAmount = Math.Round(useScoreInt / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);
                }




            }

            //积分计算 
            #endregion

            if (!string.IsNullOrEmpty(useAmountStr))
            {
                useAmount = decimal.Parse(useAmountStr);
            }
            StoredValueCardRecord storeValueCardRecord =new StoredValueCardRecord();
            bool isSuccess;
            string msg = "";
            string couponName = "";
            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(myCouponId))
            {
                MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(myCouponId), currentUserInfo.UserID);
                if (myCardCoupon != null)//优惠券
                {
                    discountAmount = bllMall.CalcDiscountAmount(myCouponId, item.Amount, currentUserInfo.UserID, out isSuccess, out msg, out couponName);//优惠券优惠金额
                    if (!isSuccess)
                    {
                        resp.Msg = msg;
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                }
                else//储值卡
                {
                    ////检查是否使用了积分余额
                    //if (useScoreInt>0|| useAmount>0)
                    //{
                    //    resp.Msg = "使用储值卡时,不可再用积分或余额抵扣,请取消使用积分或余额抵扣";
                    //    return Common.JSONHelper.ObjectToJson(resp);
                    //}
                    storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", myCouponId));
                    discountAmount = bllMall.CalcDiscountAmountStoreValue(item.Amount,storeValueCardRecord.UserId, myCouponId);
                }


            }
            totalAmount = item.Amount;
            totalAmount -= discountAmount;
            totalAmount -= scoreExchangeAmount;
            totalAmount -= useAmount;
            if (totalAmount < 0)
            {
                totalAmount = 0;
            }
            resp.Status = 1;
            resp.ExStr = totalAmount.ToString();
            resp.Msg = "ok";
            return Common.JSONHelper.ObjectToJson(resp);

        }

        ///// <summary>
        ///// 计算可以使用的最多积分
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string CalcActivityMaxUseScore(HttpContext context)
        //{
        //    ScoreConfig scoreConfig = bllScore.GetScoreConfig();
        //    if (scoreConfig.ExchangeScore==0||scoreConfig.ExchangeAmount==0)
        //    {
        //            resp.Msg = "未配置积分兑换比例";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    string itemId = context.Request["ItemId"];
        //    string myCouponId = context.Request["MyCouponId"];
        //    decimal maxUseScore=0;
        //    decimal totalAmount = 0;//金额
        //    CrowdFundItem item = bllUser.Get<CrowdFundItem>(string.Format(" ItemId='{0}'", itemId));
        //    bool isSuccess;
        //    string msg = "";
        //    decimal discountAmount = 0;
        //    if (!string.IsNullOrEmpty(myCouponId))
        //    {
        //        discountAmount = bllMall.CalcDiscountAmount(myCouponId, item.Amount, currentUserInfo.UserID, out isSuccess, out msg);//优惠券优惠金额
        //        if (!isSuccess)
        //        {
        //            resp.Msg = msg;
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //    }
        //    totalAmount = item.Amount;
        //    totalAmount -= discountAmount;


        //   maxUseScore = Math.Round(totalAmount / (scoreConfig.ExchangeAmount / scoreConfig.ExchangeScore), 2);

        //   if (currentUserInfo.TotalScore>=(double)maxUseScore)
        //   {
        //       resp.ExStr = maxUseScore.ToString();
        //   }
        //   else
        //   {
        //       resp.ExStr = currentUserInfo.TotalScore.ToString();
        //   }
        //    resp.Status = 1;
        //    resp.Msg = "ok";
        //    return Common.JSONHelper.ObjectToJson(resp);

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