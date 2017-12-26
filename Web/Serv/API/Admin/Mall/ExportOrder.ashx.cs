using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Text;
using System.Data;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 导出订单
    /// </summary>
    public class ExportOrder : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        ///// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardcoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 
        /// </summary>
        BLLStoredValueCard bllStoredValueCard = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner= '{0}' And IsNull(IsMain,0)=0", bllMall.WebsiteOwner);
            string fromDate = context.Request["from_date"];//开始日期
            string toDate = context.Request["to_date"];//结束日期
            string status = context.Request["status"];//订单状态
            string orderIds = context.Request["oids"];//订单号
            string userAutoIds = context.Request["user_aids"];//用户自动编号 
            string userTags = context.Request["user_tags"];//用户标签
            string isYuYueOrder = context.Request["is_yuyue"];//是否是会议室预约和资源预约订单
            string supplierUserId = context.Request["supplier_userid"];//供应商账号
            //订单类型 
            //0 商城订单 
            //1 礼品订单
            //2 团购订单 
            //3 无
            //4 活动订单
            //5 会议室预订 
            //6 导师预约
            string orderType = context.Request["order_type"];
            if (!string.IsNullOrEmpty(orderType))
            {
                if (orderType == "5")
                {
                    sbWhere.AppendFormat(" And ArticleCategoryType='MeetingRoom'");
                }
                else if (orderType == "6")
                {
                    sbWhere.AppendFormat(" And ArticleCategoryType='BookingTutor'");
                }
                else
                {
                    sbWhere.AppendFormat(" And OrderType={0}", orderType);
                }
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                sbWhere.AppendFormat(" And InsertDate>='{0}'", fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                sbWhere.AppendFormat(" And InsertDate<='{0}'", toDate);
            }
            if (orderType == "2" && !string.IsNullOrEmpty(status))
            {
                status = ConvertGroupbuyStatus(status);
                sbWhere.AppendFormat(" And GroupBuyStatus in({0})", status);
            }
            if (orderType == "2")
            {
                sbWhere.AppendFormat(" And PaymentStatus=1 ", status);
            }
            if ((!string.IsNullOrEmpty(status)) && (status != "退款退货") && orderType != "2")
            {
                status = "'" + status.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And Status in({0})", status);

            }
            if (status == "退款退货")
            {
                sbWhere.AppendFormat(" And IsRefund=1 ");
            }
            if (!string.IsNullOrEmpty(orderIds))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And OrderID in({0})  ", orderIds);

            }
            if (bllUser.IsSupplier(currentUserInfo))
            {
                sbWhere.AppendFormat(" And SupplierUserId ='{0}'  ", currentUserInfo.UserID);
            }
            if ((!string.IsNullOrEmpty(userAutoIds)) || (!string.IsNullOrEmpty(userTags)))
            {

                string userIds = "";
                if (!string.IsNullOrEmpty(userAutoIds))
                {
                    foreach (var userAutoId in userAutoIds.Split(','))
                    {
                        var userInfo = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
                        if (userInfo != null)
                        {
                            userIds += string.Format("'{0}',", userInfo.UserID);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(userTags))
                {

                    foreach (string tag in userTags.Split(','))
                    {

                        List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format(" Websiteowner='{0}' And TagName like '%{1}%'", bllUser.WebsiteOwner, tag));
                        foreach (var userInfo in userList)
                        {

                            userIds += string.Format("'{0}',", userInfo.UserID);

                        }

                    }



                }
                userIds = userIds.TrimEnd(',');
                sbWhere.AppendFormat(" And OrderUserId in({0})  ", userIds);

            }
            if (!string.IsNullOrEmpty(supplierUserId))
            {
                if (supplierUserId=="none")
                {
                    sbWhere.AppendFormat("  And (SupplierUserId='' Or SupplierUserId IS NULL)", "");
                }
                else
                {
                    sbWhere.AppendFormat(" And SupplierUserId ='{0}' ", supplierUserId);
                }
                
            }
            
            sbWhere.AppendFormat(" Order by InsertDate Desc ");

            List<WXMallOrderInfo> orderList = bllMall.GetList<WXMallOrderInfo>(sbWhere.ToString());
            //sbExport.Append("会员ID\t");
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1.Columns.Add("会员ID");
            dt1.Columns.Add("线上卡号");
            dt1.Columns.Add("OpenId");
            dt1.Columns.Add("订单时间");
            dt1.Columns.Add("父订单编号");
            dt1.Columns.Add("订单号");
            dt1.Columns.Add("订单状态");
            dt1.Columns.Add("商品编号");
            dt1.Columns.Add("商品编码");
            dt1.Columns.Add("商品名称");
            if (!string.IsNullOrEmpty(isYuYueOrder))
            {
                dt1.Columns.Add("预约时间");
            }
            dt1.Columns.Add("商品原价");
            dt1.Columns.Add("商品均摊价格");
            dt1.Columns.Add("商品单价");
            dt1.Columns.Add("商品规格");
            dt1.Columns.Add("商品条码");
            dt1.Columns.Add("商品数量");
            dt1.Columns.Add("商品总金额");
            dt1.Columns.Add("运费");
            dt1.Columns.Add("实付金额");
            dt1.Columns.Add("收货人姓名");
            dt1.Columns.Add("电话");
            dt1.Columns.Add("收货地址");
            dt1.Columns.Add("留言");
            dt1.Columns.Add("支付状态");
            dt1.Columns.Add("基础价");
            dt1.Columns.Add("使用积分");
            dt1.Columns.Add("使用余额");
            dt1.Columns.Add("优惠券名称");
            dt1.Columns.Add("优惠券ID");
            dt1.Columns.Add("退款");
            dt1.Columns.Add("主订单号");
            dt1.Columns.Add("商户名称");
            dt1.Columns.Add("商家备注");
            dt1.Columns.Add("快递公司");
            dt1.Columns.Add("快递单号");

            dt2.Columns.Add("订单时间");
            dt2.Columns.Add("订单号");
            dt2.Columns.Add("订单状态");
            dt2.Columns.Add("商品编号");
            dt2.Columns.Add("商品名称");
            dt2.Columns.Add("商品规格");
            dt2.Columns.Add("商品数量");
            dt2.Columns.Add("实付金额");
            dt2.Columns.Add("收货人姓名");
            dt2.Columns.Add("电话");
            dt2.Columns.Add("收货地址");
            dt2.Columns.Add("支付状态");
            dt2.Columns.Add("退款");
            dt2.Columns.Add("主订单号");
            dt2.Columns.Add("商户名称");
            dt2.Columns.Add("商家备注");
            dt2.Columns.Add("快递公司");
            dt2.Columns.Add("快递单号");

            for (int i = 0; i < orderList.Count; i++)
            {

                var userInfo = bllUser.GetUserInfo(orderList[i].OrderUserID);
                if (userInfo == null)
                {
                    userInfo = new UserInfo();
                }
                

                string cardId = string.Empty;
                string cardName = string.Empty;

                if (!string.IsNullOrEmpty(orderList[i].MyCouponCardId))
                {
                    
                    switch (orderList[i].CouponType)
                    {
                        
                        case 0:
                            MyCardCoupons myCardModel = bllCardcoupon.GetMyCardCoupon(Convert.ToInt32(orderList[i].MyCouponCardId));
                            if (myCardModel != null)
                            {
                                CardCoupons coupns = bllCardcoupon.GetCardCoupon(myCardModel.CardId);
                                cardId = coupns.CardId.ToString();
                                cardName = coupns.Name;
                            }
                            break;

                        case 1:
                            int cardIdInt = Convert.ToInt32(orderList[i].MyCouponCardId);

                            StoredValueCardRecord storedCard = bllStoredValueCard.GetStoredValueCardRecord(cardIdInt);

                            if (storedCard != null)
                            {
                                StoredValueCard cardModel = bllStoredValueCard.GetStoredValueCard(storedCard.CardId);
                                cardId = cardModel.AutoId.ToString();
                                cardName = cardModel.Name;
                            }
                            break;
                        default:
                            break;
                    }
                }
                foreach (var item in bllMall.GetOrderDetailsList(orderList[i].OrderID))
                {
                    if (orderList[i].IsRefund == 1 && string.IsNullOrEmpty(item.RefundStatus) && status == "退款退货") continue;
                    DataRow newRow = dt1.NewRow();
                    DataRow newRow1 = dt2.NewRow();
                    newRow["会员ID"] = userInfo.AutoID.ToString();
                    newRow["OpenId"] = userInfo.WXOpenId;
                    newRow["订单时间"] = orderList[i].InsertDate.ToString();
                    newRow1["订单时间"] = orderList[i].InsertDate.ToString();
                    WXMallProductInfo product = bllMall.GetProduct(item.PID);

                    switch (orderList[i].OrderType)
                    {
                        case 0:
                            newRow["父订单编号"] = "";
                            break;
                        case 1:
                            newRow["父订单编号"] = orderList[i].ParentOrderId;
                           
                            break;
                        case 2:
                            newRow["父订单编号"] = orderList[i].GroupBuyParentOrderId;
                            
                           
                            break;
                        default:
                            break;
                    }

                    newRow["订单号"] = orderList[i].OrderID;
                    newRow1["订单号"] = orderList[i].OrderID;

                    newRow["订单状态"] = orderList[i].Status;
                    newRow1["订单状态"] = orderList[i].Status;

                    newRow["商品编号"] = product == null ? "" : product.PID;
                    newRow1["商品编号"] = product == null ? "" : product.PID;
                    newRow["商品编码"] = product == null ? "" : product.ProductCode;
                    newRow["商品名称"] = product==null ? item.SkuShowProp : product.PName;
                    newRow1["商品名称"] = product == null ? item.SkuShowProp : product.PName;
                    if (!string.IsNullOrEmpty(isYuYueOrder))
                    {
                        if (isYuYueOrder == "MeetingRoom")
                        {
                            if (!string.IsNullOrEmpty(product.RelationProductId))
                            {
                                newRow["预约时间"] = item.StartDate.ToString("yyyy-MM-dd HH：mm") + "-" + item.EndDate.ToString("HH：mm");
                            }
                            else
                            {
                                newRow["预约时间"] = "";
                            }
                        }
                        else
                        {
                            newRow["预约时间"] = item.StartDate.ToString("yyyy-MM-dd HH：mm") + "-" + item.EndDate.ToString("HH：mm");
                        }
                    }
                    newRow["商品原价"] = product.PreviousPrice;
                    newRow["商品均摊价格"] = item.PaymentFt;
                    newRow["商品单价"] = item.OrderPrice;

                    if (item.SkuId.HasValue)
                    {
                        newRow["商品规格"] = item.SkuShowProp;
                        newRow1["商品规格"] = item.SkuShowProp;
                        newRow["商品条码"] = item.SkuId;
                    }
                    else
                    {
                        newRow["商品规格"] = "";
                        newRow1["商品规格"] = "";
                        newRow["商品条码"] = "";
                    }

                    newRow["商品数量"] = item.TotalCount;
                    newRow1["商品数量"] = item.TotalCount;
                    newRow["商品总金额"] = item.TotalCount * item.OrderPrice;
                    newRow["运费"] = orderList[i].Transport_Fee;
                    newRow["实付金额"] = orderList[i].TotalAmount;
                    newRow1["实付金额"] = orderList[i].TotalAmount;
                    newRow["收货人姓名"] = orderList[i].Consignee;
                    newRow1["收货人姓名"] = orderList[i].Consignee;
                    newRow["电话"] = orderList[i].Phone;
                    newRow1["电话"] = orderList[i].Phone;
                    newRow["收货地址"] = orderList[i].ReceiverProvince + orderList[i].ReceiverCity + orderList[i].ReceiverDist + orderList[i].Address;
                    newRow1["收货地址"] = orderList[i].ReceiverProvince + orderList[i].ReceiverCity + orderList[i].ReceiverDist + orderList[i].Address;
                    newRow["留言"] = orderList[i].OrderMemo;
                    newRow["支付状态"] = ConvertPaymentStatus(orderList[i].PaymentStatus);
                    newRow1["支付状态"] = ConvertPaymentStatus(orderList[i].PaymentStatus);
                    newRow["基础价"] = item.BasePrice;
                    newRow["使用积分"] = orderList[i].UseScore;
                    newRow["使用余额"] = orderList[i].UseAmount;
                    newRow["优惠券名称"] = cardName;
                    newRow["优惠券ID"] = cardId;

                    newRow["退款"] =ConvertRefundStatus(item.RefundStatus);
                    newRow1["退款"] =ConvertRefundStatus(item.RefundStatus);


                    newRow["主订单号"] = orderList[i].ParentOrderId;
                    newRow1["主订单号"] = orderList[i].ParentOrderId;


                    newRow["商户名称"] = orderList[i].SupplierName;
                    newRow1["商户名称"] = orderList[i].SupplierName;


                    newRow["商家备注"] = orderList[i].Ex21;
                    newRow1["商家备注"] = orderList[i].Ex21;

                    newRow["快递公司"] = orderList[i].ExpressCompanyName;
                    newRow1["快递公司"] = orderList[i].ExpressCompanyName;

                    newRow["快递单号"] = orderList[i].ExpressNumber;
                    newRow1["快递单号"] = orderList[i].ExpressNumber;

                    newRow["线上卡号"] = userInfo.Ex2;

                    dt1.Rows.Add(newRow);
                    dt2.Rows.Add(newRow1);
                }
                //if (orderList[i].OrderType == 2)//团购订单
                //{
                //    foreach (var item in bllMall.GetList<WXMallOrderInfo>(string.Format(" GroupBuyParentOrderId='{0}' And PaymentStatus=1", orderList[i].OrderID)))
                //    {
                //        foreach (var detail in bllMall.GetOrderDetailsList(item.OrderID))
                //        {
                //            DataRow nowRow1 = dt1.NewRow();
                //            DataRow nowRow2 = dt2.NewRow();
                //            WXMallProductInfo product = bllMall.GetProduct(detail.PID);
                //            nowRow1["订单时间"] = item.InsertDate.ToString();
                //            nowRow2["订单时间"] = item.InsertDate.ToString();
                //            nowRow1["父订单编号"] = item.GroupBuyParentOrderId;
                //            nowRow1["订单号"] = item.OrderID;
                //            nowRow2["订单号"] = item.OrderID;

                //            nowRow1["订单状态"] = item.Status;
                //            nowRow2["订单状态"] = item.Status;
                //            nowRow1["商品编号"] = product == null ? "" : product.PID;
                //            nowRow2["商品编号"] = product == null ? "" : product.PID;
                //            nowRow1["商品编码"] = product == null ? "" : product.ProductCode;
                //            nowRow1["商品名称"] = product == null ? "" : product.PName;
                //            nowRow2["商品名称"] = product == null ? "" : product.PName;

                //            nowRow1["商品原价"] = product.PreviousPrice;
                //            nowRow1["商品均摊价格"] = detail.PaymentFt;
                //            nowRow1["商品单价"] = detail.OrderPrice;
                //            if (detail.SkuId.HasValue)
                //            {
                //                nowRow1["商品规格"] = detail.SkuShowProp;
                //                nowRow2["商品规格"] = detail.SkuShowProp;
                //                nowRow1["商品条码"] = detail.SkuId;
                //            }
                //            else
                //            {
                //                nowRow1["商品规格"] = "";
                //                nowRow2["商品规格"] = "";
                //                nowRow1["商品条码"] = "";
                //            }
                //            nowRow1["商品数量"] = detail.TotalCount;
                //            nowRow2["商品数量"] = detail.TotalCount;
                //            nowRow1["商品总金额"] = detail.TotalCount * detail.OrderPrice;
                //            nowRow1["运费"] = item.Transport_Fee;
                //            nowRow1["实付金额"] = item.TotalAmount;
                //            nowRow2["实付金额"] = item.TotalAmount;
                //            nowRow1["收货人姓名"] = item.Consignee;
                //            nowRow2["收货人姓名"] = item.Consignee;
                //            nowRow1["电话"] = item.Phone;
                //            nowRow2["电话"] = item.Phone;
                //            nowRow1["收货地址"] = item.ReceiverProvince + item.ReceiverCity + item.ReceiverDist + item.Address;
                //            nowRow2["收货地址"] = item.ReceiverProvince + item.ReceiverCity + item.ReceiverDist + item.Address;
                //            nowRow1["留言"] = item.OrderMemo;
                //            nowRow1["支付状态"] = ConvertPaymentStatus(item.PaymentStatus);
                //            nowRow2["支付状态"] = ConvertPaymentStatus(item.PaymentStatus);

                //            nowRow1["基础价"] =detail.BasePrice;
                //            nowRow1["使用积分"] = orderList[i].UseScore;
                //            nowRow1["使用余额"] = orderList[i].UseAmount;
                //            nowRow1["优惠券名称"] = cardName;
                //            nowRow1["优惠券ID"] = cardId;
                //            nowRow1["退款"] =ConvertRefundStatus(detail.RefundStatus);
                //            nowRow2["退款"] = ConvertRefundStatus(detail.RefundStatus);
                //            dt1.Rows.Add(nowRow1);
                //            dt2.Rows.Add(nowRow2);
                //        }
                //    }
                //}

            }
            dt1.TableName = "订单列表";
            dt2.TableName = "精简订单列表";
            DataTable[] dt3 = { dt1, dt2 };
            DataLoadTool.ExportDataTable(dt3, string.Format("{0}_data.xls", DateTime.Now.ToString()));
        }
        /// <summary>
        /// 转换团购状态
        /// </summary>
        /// <param name="statusStr"></param>
        /// <returns></returns>
        private string ConvertGroupbuyStatus(string statusStr)
        {
            return statusStr.Replace("拼团中", "0").Replace("拼团成功", "1").Replace("拼团失败", "2");

        }
        /// <summary>
        /// 支付状态
        /// </summary>
        /// <param name="paymentStatus"></param>
        /// <returns></returns>
        private string ConvertPaymentStatus(int paymentStatus)
        {


            switch (paymentStatus)
            {
                case 0:
                    return "未付款";
                case 1:
                    return "已付款";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 退款状态
        /// </summary>
        /// <param name="refundStatus"></param>
        /// <returns></returns>
        private string ConvertRefundStatus(string refundStatus)
        {


            switch (refundStatus)
            {
                case "0":
                    return "退款中";
                case "1":
                    return "退款中";
                case "2":
                    return "退款中";
                case "3":
                    return "退款中";
                case "4":
                    return "退款中";
                case "5":
                    return "退款中";
                case "6":
                    return "退款完成";
                case "7":
                    return "退款关闭";
                default:
                    return "";
            }
        }


    }
}