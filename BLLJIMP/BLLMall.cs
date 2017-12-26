using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using System.Data;
using System.Web;
using System.IO;
using ZentCloud.BLLJIMP.Model.API.Mall;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 商城BLL
    /// </summary>
    public class BLLMall : BLL
    {
        /// <summary>
        /// 用户逻辑
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        /// <summary>
        ///卡券逻辑
        /// </summary>
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        /// <summary>
        /// Efast
        /// </summary>
        Open.EfastSDK.Client efast = new Open.EfastSDK.Client();
        /// <summary>
        /// 通用关系
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// 预约类型
        /// </summary>
        public static List<string> bookingList = new List<string>() { "MeetingRoom", "BookingTutor" };

        public BLLMall()
            : base()
        {

        }
        #region 收货地址模块

        /// <summary>
        /// 获取单个用户所有收货地址列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WXConsigneeAddress> GetConsigneeAddressList(string userId)
        {
            return GetList<WXConsigneeAddress>(string.Format("UserID='{0}' And WebsiteOwner='{1}'", userId, WebsiteOwner));
        }
        /// <summary>
        /// 获取用户收货地址列表
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public List<WXConsigneeAddress> GetConsigneeAddressList(string userId, string websiteOwner)
        {
            var websiteInfo = GetWebsiteInfoModelFromDataBase(websiteOwner);
            List<WXConsigneeAddress> outAddressList = new List<WXConsigneeAddress>();

            return GetList<WXConsigneeAddress>(string.Format("UserID='{0}' And WebsiteOwner='{1}'", userId, websiteOwner));

            //if (websiteInfo.IsUnionHongware == 0)
            //{
            //    return GetList<WXConsigneeAddress>(string.Format("UserID='{0}' And WebsiteOwner='{1}'", userId, websiteOwner));

            //}
            //else//使用宏巍的收货地址
            //{

            //    var userInfo = bllUser.GetUserInfo(userId);
            //    Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(userInfo.WebsiteOwner);
            //    var hongWareMemberInfo = hongWareClient.GetMemberInfo(userInfo.WXOpenId);
            //    if (hongWareMemberInfo.address != null)
            //    {

            //        //var addressList = hongWareClient.GetMemberAddressList(hongWareMemberInfo.member.mobile);
            //        //if (addressList.addresses != null)
            //        //{
            //        //     foreach (var item in addressList.addresses)
            //        //    {
            //        WXConsigneeAddress model = new WXConsigneeAddress();
            //        model.ConsigneeName = hongWareMemberInfo.address.name;
            //        model.Phone = hongWareMemberInfo.member.mobile;
            //        model.Address = hongWareMemberInfo.address.memberAddress;
            //        model.Province = hongWareMemberInfo.address.province;
            //        model.ProvinceCode = hongWareMemberInfo.address.provinceCode;
            //        model.City = hongWareMemberInfo.address.city;
            //        model.CityCode = hongWareMemberInfo.address.cityCode.ToString();
            //        model.Dist = hongWareMemberInfo.address.district;
            //        model.DistCode = hongWareMemberInfo.address.districtCode;
            //        model.ZipCode = "";
            //        model.IsDefault = "1";
            //        outAddressList.Add(model);
            //        //    }
            //        //}

            //    }


            //}

            //return outAddressList;


        }


        /// <summary>
        /// 添加收货地址
        /// </summary>
        /// <returns></returns>
        public bool AddConsigneeAddress(string userId, HttpContext context, out string msg)
        {

            string consigneeName = context.Request["ConsigneeName"];
            string address = context.Request["Address"];
            string phone = context.Request["Phone"];
            string isDefault = context.Request["IsDefault"];

            string province = context.Request["province"];
            string provinceCode = context.Request["province_code"];

            string city = context.Request["city"];
            string cityCode = context.Request["city_code"];

            string dist = context.Request["dist"];
            string distCode = context.Request["dist_code"];
            string zipCode = context.Request["zip_code"];
            //if (GetConsigneeAddressList(userId).Count >= 5)
            //{
            //    msg = "最多添加5个收货地址";
            //    return false;

            //}
            if (string.IsNullOrEmpty(userId))
            {
                msg = "用户名不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(consigneeName))
            {
                msg = "请输入收货人姓名";
                return false;
            }
            if (string.IsNullOrEmpty(address))
            {
                msg = "请输入收货地址";
                return false;
            }
            if (string.IsNullOrEmpty(phone))
            {
                msg = "请输入手机号";
                return false;
            }

            WXConsigneeAddress model = new WXConsigneeAddress();
            model.UserID = userId;
            model.ConsigneeName = consigneeName;
            model.Address = address;
            model.Phone = phone;
            model.IsDefault = isDefault;
            model.WebSiteOwner = WebsiteOwner;
            model.City = city;
            model.CityCode = cityCode;
            model.Province = province;
            model.ProvinceCode = provinceCode;
            model.Dist = dist;
            model.DistCode = distCode;
            model.ZipCode = zipCode;
            if (!string.IsNullOrEmpty(model.IsDefault))
            {
                if (model.IsDefault.Equals("1"))//默认地址 把其它地址更改为非默认
                {
                    Update(new WXConsigneeAddress(), "IsDefault='0'", string.Format("UserID='{0}'", userId));
                }
            }


            if (Add(model))
            {
                msg = "添加成功";
                //var websiteInfo = GetWebsiteInfoModelFromDataBase();

                //if (websiteInfo.IsSynchronizationData != null)
                //{
                //    if (websiteInfo.IsSynchronizationData.Value == 1)
                //    {
                //        UserInfo userModel = bllUser.GetUserInfo(userId);
                //        if (string.IsNullOrEmpty(userModel.TrueName))
                //        {
                //            userModel.TrueName = consigneeName;
                //        }
                //        if (string.IsNullOrEmpty(userModel.Phone))
                //        {
                //            userModel.Phone = phone;
                //        }
                //        bllUser.Update(userModel);
                //    }
                //}
                UserInfo currentUserInfo = GetCurrentUserInfo();
                if (string.IsNullOrWhiteSpace(currentUserInfo.TrueName))
                {
                    currentUserInfo.TrueName = consigneeName;
                    Update(currentUserInfo, string.Format(" TrueName = '{0}' ", consigneeName), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
                }

                if (string.IsNullOrWhiteSpace(currentUserInfo.Phone) && ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
                {
                    currentUserInfo.Phone = phone;
                    Update(currentUserInfo, string.Format(" Phone = '{0}' ", phone), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
                }

                return true;
            }
            else
            {
                msg = "添加失败";
                return false;
            }


        }

        /// <summary>
        /// 编辑收货地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool EditConsigneeAddress(string userId, HttpContext context, out string msg)
        {

            string consigneeName = context.Request["ConsigneeName"];
            string address = context.Request["Address"];
            string phone = context.Request["Phone"];
            string isDefault = context.Request["IsDefault"];
            string autoID = context.Request["AutoID"];
            string province = context.Request["province"];
            string provinceCode = context.Request["province_code"];

            string city = context.Request["city"];
            string cityCode = context.Request["city_code"];

            string dist = context.Request["dist"];
            string distCode = context.Request["dist_code"];
            string zipCode = context.Request["zip_code"];
            if (string.IsNullOrEmpty(userId))
            {
                msg = "用户名不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(consigneeName))
            {
                msg = "请输入收货人姓名";
                return false;
            }
            if (string.IsNullOrEmpty(address))
            {
                msg = "请输入收货地址";
                return false;
            }
            if (string.IsNullOrEmpty(phone))
            {
                msg = "请输入手机号";
                return false;
            }
            WXConsigneeAddress model = Get<WXConsigneeAddress>(string.Format("AutoID={0} And UserID='{1}'", autoID, userId));
            model.ConsigneeName = consigneeName;
            model.Address = address;
            model.Phone = phone;
            model.IsDefault = isDefault;
            model.City = city;
            model.CityCode = cityCode;
            model.Province = province;
            model.ProvinceCode = provinceCode;
            model.Dist = dist;
            model.DistCode = distCode;
            model.ZipCode = zipCode;
            if (!string.IsNullOrEmpty(model.IsDefault))
            {
                if (model.IsDefault.Equals("1"))//默认地址 把其它地址更改为非默认
                {
                    Update(new WXConsigneeAddress(), "IsDefault='0'", string.Format("UserID='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner));
                }
            }
            if (Update(model))
            {
                msg = "保存成功";
                var websiteInfo = GetWebsiteInfoModelFromDataBase();
                if (websiteInfo.IsSynchronizationData != null)
                {
                    if (websiteInfo.IsSynchronizationData.Value == 1)
                    {
                        UserInfo userModel = bllUser.GetUserInfo(userId);
                        if (string.IsNullOrEmpty(userModel.TrueName))
                        {
                            userModel.TrueName = consigneeName;
                        }
                        if (string.IsNullOrEmpty(userModel.Phone))
                        {
                            userModel.Phone = phone;
                        }
                        bllUser.Update(userModel);
                    }
                }
                return true;
            }
            else
            {
                msg = "保存失败";
                return false;
            }
        }

        /// <summary>
        /// 编辑收货地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool EditConsigneeAddress(string id, string userId, string consigneeName, string address, string phone, string isdefault, string province, string provincecode, string city, string citycode, string dist, string distcode, string zipcode, out string msg)
        {

            if (string.IsNullOrEmpty(userId))
            {
                msg = "用户名不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(consigneeName))
            {
                msg = "请输入收货人姓名";
                return false;
            }
            if (string.IsNullOrEmpty(address))
            {
                msg = "请输入收货地址";
                return false;
            }
            if (string.IsNullOrEmpty(phone))
            {
                msg = "请输入手机号";
                return false;
            }
            WXConsigneeAddress model = Get<WXConsigneeAddress>(string.Format("AutoID={0} And UserID='{1}'", id, userId));
            if (model == null)
            {
                msg = "收货地址不存在";
                return false;
            }
            model.ConsigneeName = consigneeName;
            model.Address = address;
            model.Phone = phone;
            model.IsDefault = isdefault;
            model.Province = province;
            model.ProvinceCode = provincecode;
            model.City = city;
            model.CityCode = citycode;
            model.Dist = dist;
            model.DistCode = distcode;
            model.ZipCode = zipcode;
            if (!string.IsNullOrEmpty(model.IsDefault))
            {
                if (model.IsDefault.Equals("1"))//默认地址 把其它地址更改为非默认
                {
                    Update(new WXConsigneeAddress(), "IsDefault='0'", string.Format("UserID='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner));
                }
            }
            if (Update(model))
            {
                msg = "保存成功";
                return true;
            }
            else
            {
                msg = "保存失败";
                return false;
            }
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteConsigneeAddress(string userId, string id)
        {
            int count = Delete(new WXConsigneeAddress(), string.Format("AutoID='{0}' And UserID='{1}'", id, userId));
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 获取单个收货地址信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXConsigneeAddress GetConsigneeAddress(string id)
        {

            return Get<WXConsigneeAddress>(string.Format("AutoID='{0}'", id));

        }


        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SetDefaultConsigneeAddress(string userId, string id)
        {

            Update(new WXConsigneeAddress(), "IsDefault='0'", string.Format("UserID='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner));

            return Update(new WXConsigneeAddress(), "IsDefault='1'", string.Format("UserID='{0}' And AutoID='{1}'", userId, id)) > 0;


        }
        #endregion


        #region 订单模块

        /// <summary>
        /// 订单是否已有分佣金记录
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool HasOrderCommission(string orderId)
        {
            return GetCount<ProjectCommission>(string.Format(" ProjectId='{0}' ", orderId)) > 0;
        }

        /// <summary>
        /// 根据订单号查询订单信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        public WXMallOrderInfo GetOrderInfo(string orderId)
        {
            //return Get<WXMallOrderInfo>(string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, WebsiteOwner));
            return Get<WXMallOrderInfo>(string.Format("OrderID='{0}'", orderId));

        }
        /// <summary>
        /// 根据外部订单号获取订单
        /// </summary>
        /// <param name="outOrderId"></param>
        /// <returns></returns>
        public WXMallOrderInfo GetOrderInfoByOutOrderId(string outOrderId)
        {

            return Get<WXMallOrderInfo>(string.Format("OutOrderId='{0}'", outOrderId));

        }


        /// <summary>
        /// 根据积分订单号查询订单信息 无用
        /// </summary>
        /// <param name="orderId">积分订单号</param>
        /// <returns></returns>
        public WXMallScoreOrderInfo GetScoreOrderInfo(string orderId)
        {
            return Get<WXMallScoreOrderInfo>(string.Format("OrderID='{0}' And WebSiteOwner='{1}'", orderId, WebsiteOwner));

        }


        /// <summary>
        /// 查询订单详细信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<WXMallOrderDetailsInfo> GetOrderDetailsList(string orderId)
        {
            return GetList<WXMallOrderDetailsInfo>(string.Format("OrderID='{0}'", orderId));
        }

        /// <summary>
        /// 查询积分订单详细信息 无用
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<WXMallScoreOrderDetailsInfo> GetScoreOrderDetailsList(string orderId)
        {

            return GetList<WXMallScoreOrderDetailsInfo>(string.Format("OrderID='{0}'", orderId));


        }


        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(string orderId, string status)
        {

            int count = Update(new WXMallOrderInfo(), string.Format("Status='{0}',LastUpdateTime=GETDATE()", status), string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, WebsiteOwner));
            if (count > 0)
            {
                if (status == "已发货")
                {
                    Update(new WXMallOrderInfo(), string.Format(" DeliveryTime=GETDATE()", status), string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, WebsiteOwner));

                }
                return true;
            }
            else
            {
                return false;
            }



        }

        /// <summary>
        /// 更新积分订单状态 无用
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool UpdateScoreOrderStatus(string orderId, string status)
        {
            int count = Update(new WXMallScoreOrderInfo(), string.Format("Status='{0}'", status), string.Format("OrderID='{0}'", orderId));
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        /// <summary>
        /// 查询微信退款状态
        /// </summary>
        /// <param name="refundNumber">订单号</param>
        /// <param name="refundId">微信退款单号</param>
        /// <returns></returns>
        public string GetWeiXinRefundStatus(string refundNumber, string refundId)
        {
            string msg = "";
            try
            {

                if (!string.IsNullOrEmpty(refundId))
                {

                    BllPay bllPay = new BllPay();
                    PayConfig payConfig = bllPay.GetPayConfig();
                    if (bllPay.QueryWeixinRefund(refundNumber, refundId, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg))
                    {

                    }
                    else
                    {
                        msg = "QUERY FAIL";
                    }

                }


            }
            catch (Exception ex)
            {

                msg = ex.Message;
            }
            return msg;



        }
        ///// <summary>
        ///// 获取用户最近一笔订单
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public WXMallOrderInfo GetLastOrderInfo(string userId)
        //{

        //    return Get<WXMallOrderInfo>(string.Format("OrderUserID='{0}' And WebsiteOwner='{1}' Order By InsertDate DESC", userId, WebsiteOwner));


        //}
        ///// <summary>
        ///// 获取最近一笔订单选择的门店 不使用
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>
        //public WXMallStores GetLastOrderStoreInfo(string userid)
        //{

        //    var lastorderInfo = GetLastOrderInfo(userid);
        //    if (lastorderInfo != null)
        //    {
        //        if (!string.IsNullOrEmpty(lastorderInfo.WxMallStoreId))
        //        {
        //            return GetStore(lastorderInfo.WxMallStoreId);
        //        }
        //    }
        //    return null;


        //}




        #region 订单状态管理
        /// <summary>
        /// 获取单个 订单状态信息
        /// </summary>
        /// <param name="orderStatusId"></param>
        /// <returns></returns>
        public WXMallOrderStatusInfo GetOrderStatus(int orderStatusId)
        {

            return Get<WXMallOrderStatusInfo>(string.Format(" AutoID={0}", orderStatusId));


        }

        /// <summary>
        /// 获取单个 订单状态信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXMallOrderStatusInfo GetOrderStatus(string orderStatus)
        {

            return Get<WXMallOrderStatusInfo>(string.Format(" OrderStatu='{0}' And WebsiteOwner='{1}'", orderStatus, WebsiteOwner));


        }
        /// <summary>
        /// 根据网站获取 订单状态列表
        /// </summary>
        /// <returns></returns>
        public List<WXMallOrderStatusInfo> GetOrderStatuList()
        {
            return GetList<WXMallOrderStatusInfo>(string.Format(" WebsiteOwner='{0}' And StatusType IS NULL Order By Sort DESC", WebsiteOwner));
        }
        /// <summary>
        /// 添加订单状态
        /// </summary>
        /// <returns></returns>
        public bool AddOrderStatus(string orderStatus, out string msg, int sort = 0)
        {
            msg = "";
            if (string.IsNullOrEmpty(orderStatus))
            {
                msg = "订单状态必填";
                return false;
            }
            if (GetOrderStatus(orderStatus) != null)
            {
                msg = "订单状态重复";
                return false;
            }

            WXMallOrderStatusInfo model = new WXMallOrderStatusInfo();
            model.OrderStatu = orderStatus;
            model.Sort = sort;
            model.WebsiteOwner = WebsiteOwner;
            if (Add(model))
            {
                return true;
            }
            else
            {
                msg = "添加订单状态失败";
                return false;
            }


        }
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderStatus(int orderStatusId, string orderStatus, out string msg, int sort = 0)
        {
            msg = "";
            if (string.IsNullOrEmpty(orderStatus))
            {
                msg = "订单状态必填";
                return false;
            }
            var model = GetOrderStatus(orderStatusId);
            if (model == null)
            {
                msg = "订单状态不存在";
                return false;
            }
            model.OrderStatu = orderStatus;
            model.Sort = sort;
            if (Update(model))
            {
                return true;
            }
            else
            {
                msg = "更新订单状态失败";
                return false;
            }


        }
        /// <summary>
        /// 删除订单状态
        /// </summary>
        /// <returns></returns>
        public bool DeleteOrderStatus(string orderStatusIds, out string msg)
        {
            msg = "";
            if (Delete(new WXMallOrderStatusInfo(), string.Format(" WebsiteOwner='{0}' And AutoID in({1})", WebsiteOwner, orderStatusIds)) == orderStatusIds.Split(',').Count())
            {
                return true;
            }
            msg = "删除失败";
            return false;
        }

        #endregion




        ///// <summary>
        ///// 获取订单列表
        ///// </summary>
        ///// <param name="totalCount"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="userId"></param>
        ///// <param name="status"></param>
        ///// <param name="orderFromTime"></param>
        ///// <param name="orderToTime"></param>
        ///// <param name="orderType"></param>
        ///// <param name="giftOrderType"></param>
        ///// <returns></returns>
        //public List<Model.WXMallOrderInfo> GetOrderList(out int totalCount, int pageSize, int pageIndex, string userId, string status = "", string orderFromTime = "", string orderToTime = "", string orderType = "", string giftOrderType = "")
        //{
        //    List<Model.WXMallOrderInfo> orderList = new List<WXMallOrderInfo>();

        //    StringBuilder strWhere = new StringBuilder(" 1=1 ");
        //    strWhere.AppendFormat(" AND OrderUserID = '{0}' ", userId);

        //    if (!string.IsNullOrWhiteSpace(status))
        //    {
        //        if (!status.Contains("'"))
        //        {
        //            status = ZentCloud.Common.StringHelper.ListToStr<string>(status.Split(',').ToList(), "'", ",");
        //        }
        //        strWhere.AppendFormat(" AND Status IN ({0})", status);
        //    }

        //    if (!string.IsNullOrEmpty(orderFromTime))
        //    {

        //        strWhere.AppendFormat(" AND InsertDate >= '{0}'", orderFromTime);
        //    }
        //    if (!string.IsNullOrEmpty(orderToTime))
        //    {

        //        strWhere.AppendFormat(" AND InsertDate <= '{0}'", orderToTime);
        //    }
        //    if (!string.IsNullOrEmpty(orderType))
        //    {

        //        strWhere.AppendFormat(" AND OrderType={0}", orderType);
        //    }
        //    if (!string.IsNullOrEmpty(giftOrderType))
        //    {
        //        if (giftOrderType=="0")//接收到的礼品订单
        //        {
        //            strWhere.AppendFormat(" AND OrderType=1 And ParentOrderId!=''", giftOrderType);
        //        }
        //        else if (giftOrderType=="1")//发出的礼品订单
        //        {
        //            strWhere.AppendFormat(" AND OrderType=1 And (ParentOrderId='' OR ParentOrderId IS NULL)", giftOrderType);
        //        }

        //    }

        //    totalCount = GetCount<Model.WXMallOrderInfo>(strWhere.ToString());
        //    orderList = GetLit<Model.WXMallOrderInfo>(pageSize, pageIndex, strWhere.ToString(), " InsertDate DESC");
        //    return orderList;
        //}


        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="totalCount">总数</param>
        /// <param name="status">订单状态</param>
        /// <param name="userId">下单用户</param>
        /// <param name="orderFromTime">订单时间开始</param>
        /// <param name="orderToTime">订单结束时间</param>
        /// <param name="orderType">订单类型订单类型 0 普通订单 1礼品订单</param>
        /// <param name="giftOrderType">//礼品订单类型 0收到的礼品订单 1 发出的礼品订单</param>
        /// <param name="lastUpdateFromTime">订单最后更新时间开始</param>
        /// <param name="lastUpdateToTime">订单最后更新时间结束</param>
        /// <param name="isHideReciveGiftOrder">是否隐藏接收的礼品订单</param>
        /// <param name="isHideMemberGroupBuy">是否隐藏拼团团员订单</param>
        /// <param name="groupBuyStatus">团购订单状态</param>
        /// <param name="isAppeal">是否申诉中的订单</param>
        /// <param name="isRefund">是否退款中的订单</param>
        /// <returns></returns>
        public List<Model.WXMallOrderInfo> GetOrderList(int pageSize, int pageIndex, string keyword, out int totalCount, string status = "", string userId = "", string orderFromTime = "", string orderToTime = "", string orderType = "", string giftOrderType = "", string lastUpdateFromTime = "", string lastUpdateToTime = "", string isHideReciveGiftOrder = "",
            string isHideMemberGroupBuy = "", string groupBuyStatus = "", string type = "", string orderIds = "", string isAppeal = "", string isRefund = "", string hasReview = "", bool isShowCourse = false, string isShowMain = "", string supplierUserId = "", string groupType = "", string isDisableCurrUser = "", string keyType = "", string sort = "", string channelUserId = "", string distributionOwner = "", string userAutoId = ""
            ,string refundStatus="")
        {
            List<Model.WXMallOrderInfo> orderList = new List<WXMallOrderInfo>();
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}' ", WebsiteOwner));

            if (isDisableCurrUser != "1" && !string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" AND OrderUserID = '{0}' ", userId);
            }
            if (!string.IsNullOrEmpty(userAutoId))
            {
                sbWhere.AppendFormat(" AND OrderUserID=(Select UserId from ZCJ_UserInfo Where AutoId={0}) ", userAutoId);
            }
            if (status == "待收货")
            {
                status = "已发货";
            }
            if ((!string.IsNullOrEmpty(status)) && status != "退款退货")
            {
                sbWhere.AppendFormat(" And Status='{0}'", status);
            }
            if (status == "退款退货")
            {
                sbWhere.AppendFormat(" And IsRefund=1 ");

                if (refundStatus == "已发货退款")
                {
                    sbWhere.AppendFormat(" And Status in ('已发货') ");
                }
                else if (refundStatus == "未发货退款")
                {
                    sbWhere.AppendFormat(" And Status='待发货' ");
                }
                else if (refundStatus == "待处理退款")
                {
                    sbWhere.AppendFormat(" AND OrderID in ( select OrderID from ZCJ_WXMallOrderDetailsInfo where  RefundStatus=0 ) ");
                }
                else if (refundStatus == "待买家发货")
                {
                    sbWhere.AppendFormat(" AND OrderID in ( select OrderID from ZCJ_WXMallOrderDetailsInfo where  RefundStatus=1) ");
                }
                else if (refundStatus == "待商家收货")
                {
                    sbWhere.AppendFormat(" AND OrderID in ( select OrderID from ZCJ_WXMallOrderDetailsInfo where  RefundStatus=3) ");
                }
                else if (refundStatus == "商家已拒绝")
                {
                    sbWhere.AppendFormat(" AND OrderID in ( select OrderID from ZCJ_WXMallOrderDetailsInfo where  RefundStatus=5) ");
                }
            }

            if (!string.IsNullOrEmpty(hasReview))
            {
                if (hasReview == "0")
                {
                    sbWhere.AppendFormat(" And (ReviewScore Is Null Or ReviewScore=0) ");
                }
                else if (hasReview == "1")
                {
                    sbWhere.AppendFormat(" And ReviewScore>0 ");
                }
            }



            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(keyType))
                {
                    switch (keyType)
                    {
                        case "0"://订单号
                            sbWhere.AppendFormat(" AND (OrderID like '%{0}%'  Or OutOrderId like '%{0}%' OR ParentOrderId like '%{0}%')", keyword);
                            break;
                        case "1"://商品名称
                            sbWhere.AppendFormat(" AND OrderID in (select orderid from ZCJ_WXMallOrderDetailsInfo where  ProductName like '%{0}%')", keyword);
                            break;
                        case "2"://商品编码
                            break;
                        case "3"://收货人姓名
                            sbWhere.AppendFormat(" AND Consignee like '{0}%' ", keyword);
                            break;
                        case "4"://收货人电话
                            sbWhere.AppendFormat(" AND Phone like '%{0}%'", keyword);
                            break;
                        case "5"://下单人姓名
                            sbWhere.AppendFormat(" AND ( OrderUserID in( select UserID from ZCJ_UserInfo where TrueName like '{0}%' and WebsiteOwner = '{1}')  )", keyword, WebsiteOwner);
                            break;
                        case "6"://下单人电话
                            sbWhere.AppendFormat(" AND ( OrderUserID in( select UserID from ZCJ_UserInfo where Phone like '%{0}%' and WebsiteOwner = '{1}')  )", keyword, WebsiteOwner);
                            break;
                        case "7"://微信昵称
                            sbWhere.AppendFormat(" AND ( OrderUserID in( select UserID from ZCJ_UserInfo where WXNickname like '{0}%' and WebsiteOwner = '{1}') )", keyword, WebsiteOwner);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    long orderIdKeyword = 0;

                    if (long.TryParse(keyword, out orderIdKeyword))
                    {
                        sbWhere.AppendFormat(" AND (OrderID like '%{0}%'  Or OutOrderId like '%{0}%')", keyword);
                    }
                    else
                    {
                        sbWhere.AppendFormat(" AND ( OrderUserID in( select UserID from ZCJ_UserInfo where TrueName like '{0}%' and WebsiteOwner = '{1}') or Consignee like '{0}%' )", keyword, WebsiteOwner);
                    }
                }


            }

            if (!string.IsNullOrEmpty(orderFromTime))
            {

                sbWhere.AppendFormat(" AND InsertDate >= '{0}'", orderFromTime);
            }
            if (!string.IsNullOrEmpty(orderToTime))
            {

                sbWhere.AppendFormat(" AND InsertDate <= '{0}'", orderToTime);
            }
            if (!string.IsNullOrEmpty(orderType))
            {
                if (isShowCourse && (orderType.Contains("0,2")) && (orderType != "2"))
                {
                    sbWhere.AppendFormat(" AND OrderType In(0,2,7)", orderType);
                }
                else
                {
                    sbWhere.AppendFormat(" AND OrderType in ({0})", orderType);
                    //if (orderType=="7")
                    //{
                    //    sbWhere.AppendFormat(" AND PaymentStatus=1 ");
                    //}
                }

            }

            if (!string.IsNullOrEmpty(giftOrderType))
            {
                if (giftOrderType == "0")//接收到的礼品订单
                {
                    sbWhere.AppendFormat(" AND OrderType=1 And ParentOrderId!=''", giftOrderType);
                }
                else if (giftOrderType == "1")//发出的礼品订单
                {
                    sbWhere.AppendFormat(" AND OrderType=1 And (ParentOrderId='' OR ParentOrderId IS NULL)", giftOrderType);
                }

            }
            if (!string.IsNullOrEmpty(lastUpdateFromTime))
            {

                sbWhere.AppendFormat(" AND LastUpdateTime>='{0}'", lastUpdateFromTime);
            }
            if (!string.IsNullOrEmpty(lastUpdateToTime))
            {

                sbWhere.AppendFormat(" AND LastUpdateTime<='{0}'", lastUpdateToTime);
            }
            if (!string.IsNullOrEmpty(isHideReciveGiftOrder))
            {
                if (isHideReciveGiftOrder == "1")
                {
                    // sbWhere.AppendFormat("  And ((ParentOrderId='' OR ParentOrderId IS NULL) Or (ParentOrderId=OrderId))", isHideReciveGiftOrder);

                }
            }
            if (!string.IsNullOrEmpty(isHideMemberGroupBuy))
            {
                if (isHideMemberGroupBuy == "1")
                {
                    sbWhere.AppendFormat("  And (GroupBuyParentOrderId='' OR GroupBuyParentOrderId IS NULL Or GroupBuyParentOrderId=OrderId)", isHideMemberGroupBuy);

                }
            }
            if (!string.IsNullOrEmpty(orderType))
            {
                if (orderType == "2")//拼团订单 只显示已经付款的订单
                {
                    sbWhere.AppendFormat("  And PaymentStatus=1 ");
                }
                else if (orderType == "0,2")
                {
                    sbWhere.AppendFormat("  And ((OrderType=2 and PaymentStatus=1) or (OrderType=0) Or OrderType=7) ");
                }
            }

            if (!string.IsNullOrEmpty(groupBuyStatus))
            {
                if (groupBuyStatus == "0")
                {
                    sbWhere.AppendFormat("  And ( GroupBuyStatus=0 or GroupBuyStatus = '' or GroupBuyStatus is null ) ", groupBuyStatus);
                }
                else if (groupBuyStatus == "3")
                {
                    //待退款的拼团订单
                    sbWhere.AppendFormat(" And GroupBuyStatus=2  And ((Ex1='' Or Ex1 IS NULL) Or (Select Count(*) from ZCJ_WXMallOrderInfo child where child.GroupBuyParentOrderId=OrderId And child.PaymentStatus=1 And (child.Ex1='' Or child.Ex1 IS NULL))>0) ");

                }
                else
                {
                    sbWhere.AppendFormat("  And GroupBuyStatus={0}", groupBuyStatus);

                }

            }
            if (string.IsNullOrEmpty(type) || type == "Mall")
            {
                sbWhere.AppendFormat("  And (ArticleCategoryType Is Null Or ArticleCategoryType='{0}')", "Mall");
            }
            else
            {
                sbWhere.AppendFormat("  And ArticleCategoryType='{0}'", type);
            }

            if (!string.IsNullOrWhiteSpace(orderIds))
            {
                sbWhere.AppendFormat("  And OrderID In ({0})", orderIds);
            }
            if (!string.IsNullOrWhiteSpace(isAppeal))
            {
                if (isAppeal == "1")
                {
                    sbWhere.AppendFormat("  And Ex8 ='{0}'", isAppeal);
                }

            }
            if (!string.IsNullOrWhiteSpace(isRefund))
            {
                if (isRefund == "1")
                {
                    sbWhere.AppendFormat("  And (Ex11 ='{0}' Or Ex18!='') And Status!='已取消'", isRefund);
                }
            }

            if (string.IsNullOrEmpty(isShowMain))
            {
                sbWhere.AppendFormat("  And ISNUll(IsMain,0)=0");
            }
            if (!string.IsNullOrEmpty(supplierUserId))
            {
                if (supplierUserId == "none")//无商户
                {
                    sbWhere.AppendFormat("  And (SupplierUserId='' Or SupplierUserId IS NULL)", "");
                }
                else
                {
                    sbWhere.AppendFormat("  And SupplierUserId='{0}'", supplierUserId);

                }
            }


            if (!string.IsNullOrEmpty(groupType))//区分团购订单里面的系统开团和用户开团
            {
                if (groupType == "1")//系统开团订单
                {
                    sbWhere.AppendFormat(" AND Ex10='{0}' ", groupType);
                }
                else if (groupType == "0")
                {
                    sbWhere.AppendFormat(" AND (Ex10 is null or Ex10='{0}') ", groupType);
                }
            }
            string orderBy = string.Empty;


            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "0":
                        orderBy = " InsertDate DESC ";
                        break;
                    case "1":
                        orderBy = " PayTime DESC ";
                        break;
                    case "2":
                        orderBy = " PayTime ASC ";
                        break;
                    case "3":
                        orderBy = " LastUpdateTime DESC,InsertDate Desc  ";//退款 排序按照申请先后次序
                        break;
                    default:
                        orderBy = " InsertDate DESC";
                        break;
                }
            }
            else
            {
                orderBy = " InsertDate DESC ";
            }

            if (!string.IsNullOrEmpty(channelUserId))
            {
                sbWhere.AppendFormat("  And ChannelUserId='{0}'", channelUserId);
            }
            if (!string.IsNullOrEmpty(distributionOwner))
            {

                sbWhere.AppendFormat("  And (OrderUserId In({0}) Or DistributionOwner In({0}))", ZentCloud.Common.StringHelper.AddSplitChar(distributionOwner));
            }


            totalCount = GetCount<Model.WXMallOrderInfo>(sbWhere.ToString());
            orderList = GetLit<Model.WXMallOrderInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            return orderList;
        }


        /// <summary>
        /// 获取订单详情列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<Model.WXMallOrderDetailsInfo> GetOrderDetail(string orderId)
        {
            return GetList<Model.WXMallOrderDetailsInfo>(string.Format(" OrderID = '{0}' ", orderId));

        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns></returns>
        public Model.WXMallOrderDetailsInfo GetOrderDetail(int orderDetailId)
        {
            return Get<Model.WXMallOrderDetailsInfo>(string.Format(" AutoID = '{0}' ", orderDetailId));

        }
        #endregion


        ///// <summary>
        ///// 获取 所在站点所有门店
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public List<WXMallStores> GetStoreList()
        //{//
        //    return GetList<WXMallStores>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));

        //}


        #region 商品模块

        /// <summary>
        /// 根据商品id查询单个商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public WXMallProductInfo GetProduct(string productId, bool isNoCache = false)
        {
            WXMallProductInfo result = null;

            if (isNoCache)
            {
                return Get<WXMallProductInfo>(string.Format("PID='{0}' ", productId));
            }

            try
            {
                var key = WebsiteOwner + ":PD:" + productId;

                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);

                if (string.IsNullOrWhiteSpace(cacheDataStr))
                {
                    result = Get<WXMallProductInfo>(string.Format("PID='{0}' ", productId));
                    RedisHelper.RedisHelper.StringSet(key, JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<WXMallProductInfo>(cacheDataStr);
                }
            }
            catch (Exception ex)
            {
                result = Get<WXMallProductInfo>(string.Format("PID='{0}' ", productId));
            }

            if (result == null)
            {
                result = new WXMallProductInfo();
            }

            return result;
        }
        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public WXMallProductInfo GetProduct(int productId)
        {
            return GetProduct(productId.ToString());
        }
        /// <summary>
        /// 根据商品CODE查询单个商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public WXMallProductInfo GetProductByProductCode(string productCode)
        {
            return Get<WXMallProductInfo>(string.Format("ProductCode='{0}' And WebsiteOwner='{1}'", productCode, WebsiteOwner));
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WXMallProductInfo> GetProductList(HttpContext context, out int totalCount, int accessLevel = 0, bool checkAccessLevel = true, bool isNoCache = false)
        {
            string productIds = context.Request["product_ids"];//商品IDs
            string keyWord = context.Request["keyword"];//关键字
            string categoryId = context.Request["categoryid"];
            string categoryIds = context.Request["category_id"];//分类ID新
            string sort = context.Request["sort"];//排序号
            string sort_tag = context.Request["sort_tag"];//排序标签
            string pageIndexStr = context.Request["pageindex"];//页码
            string pageSizeStr = context.Request["pagesize"];//页数
            string isSpecial = context.Request["is_special"];//无用
            string isNew = context.Request["is_new"];//无用
            string isHot = context.Request["is_hot"];//无用
            string isOnSale = context.Request["is_onsale"];//上下架
            string isPromotionProduct = context.Request["is_promotion_product"];//是否特卖商品
            string isRecommend = context.Request["is_recommend"];//无用
            string colorName = context.Request["color_name"];//颜色
            string sizeName = context.Request["size_name"];//尺码
            string tags = context.Request["tags"];//标签
            string propmotionStartTimeStampStr = context.Request["promotion_start_timestamp"];//特卖开始时间戳 旧的
            string propmotionStopTimeStampStr = context.Request["promotion_stop_timestamp"];//特卖结束时间戳   旧的
            string lastUpdateFromTime = context.Request["lastupdate_from_time"];//最后更新时间开始
            string lastUpdateToTime = context.Request["lastupdate_to_time"];//最后更新时间结束
            string isHaveRelationProductId = context.Request["is_have_relation_product_id"];//是否有关联商品ID
            string isGroupBuyProduct = context.Request["is_group_buy"];
            string startTime = context.Request["start_time"];//开始时间
            string endTime = context.Request["end_time"];//结束时间
            string page = context.Request["[page]"];
            string rows = context.Request["rows"];
            string isAppointment = context.Request["is_appointment"];//是否是预购商品
            string supplierUserId = context.Request["supplierUserId"];//供应商账号
            string roomType = context.Request["room_type"];//房型
            string startPrice = context.Request["s_price"]; //价格区间  开始
            string endPrice = context.Request["e_price"];//价格区间  结束
            string province = context.Request["province_key"];//省份
            string city = context.Request["city_key"];//城市
            string districts = context.Request["district_key"];//地区
            string ex19 = context.Request["ex19"];//楼盘分类  NewHouse  SecondHandHouse
            string supplierId = context.Request["supplier_id"];//供应商Id
           
            if (bllUser.IsLogin)
            {
                var currentUserInfo = bllUser.GetCurrentUserInfo();
                if (currentUserInfo.UserType == 7)
                {
                    supplierUserId = currentUserInfo.UserID;
                    var companyConfig = Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));
                    if (companyConfig != null && companyConfig.StockType == 1)
                    {
                        supplierUserId = "";
                        //supplierId = currentUserInfo.AutoID.ToString();
                    }

                }
            }
            var type = "Mall";
            if (!string.IsNullOrEmpty(context.Request["type"]))
            {
                type = context.Request["type"];
            }
            if (!string.IsNullOrEmpty(page))
            {
                pageIndexStr = page;
            }
            if (!string.IsNullOrEmpty(rows))
            {
                pageSizeStr = rows;
            }

            totalCount = 0;
            return GetProductList(keyWord, categoryId, categoryIds, sort,
             pageIndexStr, pageSizeStr, isSpecial, isNew, isHot, isOnSale, isPromotionProduct,
             isRecommend, colorName, sizeName, tags, propmotionStartTimeStampStr,
             propmotionStopTimeStampStr, lastUpdateFromTime, lastUpdateToTime, isHaveRelationProductId, isGroupBuyProduct,
            out  totalCount, accessLevel, checkAccessLevel, type, startTime, endTime, sort_tag, isAppointment, supplierUserId, productIds, roomType, startPrice, endPrice, province, city, districts, ex19, isNoCache, supplierId);
        }

        /// <summary>
        /// 获取商品列表 
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoryIds"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndexStr"></param>
        /// <param name="pageSizeStr"></param>
        /// <param name="isSpecial"></param>
        /// <param name="isNew"></param>
        /// <param name="isHot"></param>
        /// <param name="isOnSale"></param>
        /// <param name="isPromotionProduct"></param>
        /// <param name="isRecommend"></param>
        /// <param name="colorName"></param>
        /// <param name="sizeName"></param>
        /// <param name="tags"></param>
        /// <param name="propmotionStartTimeStampStr"></param>
        /// <param name="propmotionStopTimeStampStr"></param>
        /// <param name="lastUpdateFromTime"></param>
        /// <param name="lastUpdateToTime"></param>
        /// <param name="isHaveRelationProductId"></param>
        /// <param name="isGroupBuyProduct"></param>
        /// <param name="totalCount"></param>
        /// <param name="accessLevel"></param>
        /// <param name="checkAccessLevel"></param>
        /// <param name="type"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="sortTag"></param>
        /// <param name="isAppointment">是否是预购商品</param>
        /// <param name="supplierUserId">供应商账号</param>
        /// <param name="productIds">商品ID</param>
        /// <returns></returns>
        public List<WXMallProductInfo> GetProductList(string keyWord, string categoryId, string categoryIds, string sort,
            string pageIndexStr, string pageSizeStr, string isSpecial, string isNew, string isHot, string isOnSale, string isPromotionProduct,
            string isRecommend, string colorName, string sizeName, string tags, string propmotionStartTimeStampStr,
            string propmotionStopTimeStampStr, string lastUpdateFromTime, string lastUpdateToTime, string isHaveRelationProductId, string isGroupBuyProduct,
            out int totalCount, int accessLevel = 0, bool checkAccessLevel = true, string type = "Mall", string startTime = "", string endTime = "",
            string sortTag = "", string isAppointment = "", string supplierUserId = "", string productIds = "", string roomType = "", string startPrice = "", string endPrice = "", string province = "", string city = "", string districts = "", string ex19 = "", bool isNoCache = false, string supplierId = "")
        {
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And IsDelete=0 ", WebsiteOwner));

            double propmotionStartTimeStamp = 0;
            double propmotionStopTimeStamp = 0;
            int pageIndex = 1;
            int pageSize = 10;
            if (!string.IsNullOrEmpty(pageIndexStr))
            {
                pageIndex = int.Parse(pageIndexStr);
            }
            if (!string.IsNullOrEmpty(pageSizeStr))
            {
                pageSize = int.Parse(pageSizeStr);
            }
            else if (!string.IsNullOrWhiteSpace(productIds)) //id查询时数量
            {
                pageSize = productIds.Split(',').Count();
            }

            string orderBy = " Sort DESC,InsertDate DESC";
            if (!string.IsNullOrWhiteSpace(productIds))
            {
                productIds = "'" + productIds.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And PID In ({0})", productIds);
            }
            //sbWhere.AppendFormat(" And Exists (select 1 from [ZCJ_ProductSku] where [ProductId]=[ZCJ_WXMallProductInfo].[PID] )");
            if (checkAccessLevel)
            {
                sbWhere.AppendFormat(" And (AccessLevel<={0} OR AccessLevel Is Null)", accessLevel);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat(" And CategoryId='{0}'", categoryId);

            }

            if (!string.IsNullOrEmpty(categoryIds) && categoryIds != "0")
            {
                if (!categoryIds.Contains(","))
                {
                    try
                    {
                        string nCateIds = GetCateAndChildIds(int.Parse(categoryIds));
                        if (string.IsNullOrWhiteSpace(nCateIds))
                        {
                            sbWhere.AppendFormat(" And CategoryId in({0})", categoryIds);
                        }
                        else
                        {
                            sbWhere.AppendFormat(" And CategoryId in({0})", nCateIds);
                        }
                    }
                    catch (Exception)
                    {

                        sbWhere.AppendFormat(" And CategoryId in({0})", categoryIds);
                    }
                }
                else
                {
                    sbWhere.AppendFormat(" And CategoryId in({0})", categoryIds);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (PName like '%{0}%' Or ProductCode like '%{0}%' or Tags like '%{0}%' Or PID='{0}')", keyWord);
            }
            //if ((!string.IsNullOrEmpty(isSpecial)) && (isSpecial == "1"))
            //{
            //    sbWhere.AppendFormat(" And IsSpecial=1");
            //}
            //if ((!string.IsNullOrEmpty(isNew)) && (isNew == "1"))
            //{
            //    sbWhere.AppendFormat(" And IsNew=1");
            //}
            //if ((!string.IsNullOrEmpty(isHot)) && (isHot == "1"))
            //{
            //    sbWhere.AppendFormat(" And IsHot=1");
            //}
            //if ((!string.IsNullOrEmpty(isRecommend)) && (isRecommend == "1"))
            //{
            //    sbWhere.AppendFormat(" And IsRecommend=1");
            //}
            if (!string.IsNullOrEmpty(isOnSale))
            {
                sbWhere.AppendFormat(" And IsOnSale={0} ", isOnSale);
            }
            else
            {
                sbWhere.AppendFormat(" And IsOnSale=1 ");
            }
            if (!string.IsNullOrEmpty(isPromotionProduct))
            {
                sbWhere.AppendFormat(" And IsPromotionProduct={0} ", isPromotionProduct);
            }
            else
            {
                //sbWhere.AppendFormat(" And IsPromotionProduct=0 ");
            }

            if (!string.IsNullOrEmpty(propmotionStartTimeStampStr))
            {
                propmotionStartTimeStamp = double.Parse(propmotionStartTimeStampStr);
            }
            if (!string.IsNullOrEmpty(propmotionStopTimeStampStr))
            {
                propmotionStopTimeStamp = double.Parse(propmotionStopTimeStampStr);
            }

            if (propmotionStartTimeStamp > 0)
            {
                sbWhere.AppendFormat(" And PromotionStartTime >={0}", propmotionStartTimeStamp);
            }

            if (propmotionStopTimeStamp > 0)
            {
                sbWhere.AppendFormat(" And PromotionStopTime <={0}", propmotionStopTimeStamp);
            }
            if (!string.IsNullOrEmpty(lastUpdateFromTime))
            {
                sbWhere.AppendFormat(" And LastUpdate >='{0}'", lastUpdateFromTime);
            }
            if (!string.IsNullOrEmpty(lastUpdateToTime))
            {
                sbWhere.AppendFormat(" And LastUpdate <='{0}'", lastUpdateToTime);
            }
            if (!string.IsNullOrEmpty(tags))
            {
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < tags.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat("   ( Tags = '{0}' or Tags Like '{0},%'  or Tags Like '%,{0}' or Tags Like '%,{0},%' )  ", tags.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or ( Tags = '{0}' or Tags Like '{0},%'  or Tags Like '%,{0}' or Tags Like '%,{0},%' )  ", tags.Split(',')[i]);

                    }
                }

                sbWhere.AppendFormat(" ) ");
            }

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "price_asc":
                        orderBy = " Price ASC,Sort DESC,InsertDate DESC";
                        break;
                    case "price_desc":
                        orderBy = " Price DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "pv":
                        orderBy = " PV DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "pv_asc":
                        orderBy = " PV ASC,Sort DESC,InsertDate DESC";
                        break;
                    case "time_asc":
                        orderBy = " InsertDate ASC,Sort DESC";
                        break;
                    case "time_desc":
                        orderBy = " InsertDate DESC,Sort DESC";
                        break;
                    case "sales_volume":
                        orderBy = " SaleCount DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "sales_asc":
                        orderBy = " SaleCount ASC,Sort DESC,InsertDate DESC";
                        break;
                    case "sales_volume_onemonth":
                        orderBy = " SaleCountOneMonth DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "sales_volume_threemonth":
                        orderBy = " SaleCountThreeMonth DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "sales_volume_halfyear":
                        orderBy = " SaleCountHalfYear DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "sales_volume_oneyear":
                        orderBy = " SaleCountOneYear DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "sort_tag": //标签排序
                        if (!string.IsNullOrWhiteSpace(sortTag))
                        {
                            orderBy = " (case when ','+[Tags]+',' like '%," + sortTag + ",%' then 0 else 1 end) ASC,Sort DESC,InsertDate DESC";
                        }
                        break;
                    case "uv_desc":
                        orderBy = " UV DESC,Sort DESC,InsertDate DESC";
                        break;
                    case "uv_asc":
                        orderBy = " UV ASC,Sort DESC,InsertDate DESC";
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(colorName))
            {
                sbWhere.AppendFormat(" And PID in (Select ProductId from ZCJ_ProductSku where ShowProps like '%{0}%') ", colorName);
            }
            if (!string.IsNullOrEmpty(sizeName))
            {
                sbWhere.AppendFormat(" And PID in (Select ProductId from ZCJ_ProductSku where ShowProps like '%{0}%') ", sizeName);
            }
            if (!string.IsNullOrEmpty(isHaveRelationProductId))
            {
                sbWhere.AppendFormat(" And RelationProductId !='' And RelationProductId IS NOT NULL");
            }
            if (!string.IsNullOrEmpty(isGroupBuyProduct))
            {
                switch (isGroupBuyProduct)//是否是团购商品
                {
                    case "1":
                        sbWhere.AppendFormat(" And GroupBuyRuleIds <>'' ");

                        //是团购商品的时候，过滤掉可以用积分购买的商品
                        sbWhere.AppendFormat(" And ( Score = 0 or Score is null ) ");

                        break;
                    case "0":
                        //sbWhere.AppendFormat(" And (GroupBuyRuleIds ='' Or GroupBuyRuleIds IS NULL) ");

                        break;
                    default:
                        break;
                }
            }
            //if (isGroupBuyProduct == "3")//系统开团
            //{
            //    sbWhere.AppendFormat(" And GroupBuyType=1 ");
            //}
            //if (isGroupBuyProduct == "1")//用户开团
            //{
            //    sbWhere.AppendFormat(" And (GroupBuyType is null or GroupBuyType=0) ");
            //}

            if (string.IsNullOrWhiteSpace(type) || type == "Mall")
            {
                sbWhere.AppendFormat(" And (ArticleCategoryType Is Null Or ArticleCategoryType='Mall') ");
            }
            else
            {
                sbWhere.AppendFormat(" And ArticleCategoryType='{0}' ", type);
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                sbWhere.AppendFormat("   And (LimitBuyTime not  like '%{0}%' or  LimitBuyTime is null or LimitBuyTime='') ", startTime);
            }
            if (!string.IsNullOrEmpty(isAppointment))
            {
                sbWhere.AppendFormat(" And IsAppointment={0}", isAppointment);
            }
            if (!string.IsNullOrEmpty(supplierUserId))
            {
                var companyConfig = Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));
                if (companyConfig != null && companyConfig.StockType == 1)
                {
                    supplierUserId = "";
                }
                sbWhere.AppendFormat(" And SupplierUserId='{0}'", supplierUserId);
            }
            if (!string.IsNullOrEmpty(startPrice))
            {
                sbWhere.AppendFormat(" AND Price>{0}", decimal.Parse(startPrice));
            }
            if (!string.IsNullOrEmpty(endPrice))
            {
                sbWhere.AppendFormat(" AND Price<{0}", decimal.Parse(endPrice));
            }

            if (!string.IsNullOrEmpty(province))
            {
                sbWhere.AppendFormat(" AND ProvinceCode='{0}'", province);
            }

            if (!string.IsNullOrEmpty(city))
            {
                sbWhere.AppendFormat(" AND CityCode='{0}'", city);
            }

            if (!string.IsNullOrEmpty(districts))
            {
                districts = "'" + districts.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" AND DistrictCode in ({0})", districts);
            }

            if (!string.IsNullOrEmpty(roomType))//房型  
            {
                roomType = "'" + roomType.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And Ex18 in ({0})", roomType);
            }

            if (!string.IsNullOrWhiteSpace(ex19))
            {
                sbWhere.AppendFormat(" AND ex19='{0}'", ex19);
            }
            if (!string.IsNullOrEmpty(supplierId))
            {
                sbWhere.AppendFormat(" And PID in (Select ProductId from ZCJ_ProductSkuSupplier where SupplierId='{0}' And Stock>0) ", supplierId);
            }
            var result = new List<WXMallProductInfo>();
            try
            {
                var orginKey = string.Format("pageSize{0}pageIndex{1}{2}orderBy{3}", pageSize, pageIndex, sbWhere.ToString(), orderBy);//原始key
                var listKey = string.Format("{0}:PL:{1}", WebsiteOwner, Common.DEncrypt.GetMD5(orginKey));

                var redisDataStr = string.Empty;

                if (!isNoCache)
                {
                    redisDataStr = RedisHelper.RedisHelper.StringGet(listKey);
                }

                if (!string.IsNullOrWhiteSpace(redisDataStr))
                {
                    var redisData = JsonConvert.DeserializeObject<Model.API.BaseWXMallProductInfoList>(redisDataStr);
                    totalCount = redisData.TotalCount;
                    result = redisData.List;
                }
                else
                {
                    totalCount = GetCount<WXMallProductInfo>(sbWhere.ToString());
                    result = GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

                    var redisData = new Model.API.BaseWXMallProductInfoList()
                    {
                        TotalCount = totalCount,
                        List = result
                    };

                    RedisHelper.RedisHelper.StringSet(listKey, JsonConvert.SerializeObject(redisData));

                    if (!RedisHelper.RedisHelper.SetContains(WebsiteOwner + ":" + Common.SessionKey.ShopListKeys, listKey))
                        RedisHelper.RedisHelper.SetAdd(WebsiteOwner + ":" + Common.SessionKey.ShopListKeys, listKey);
                }
            }
            catch (Exception ex)
            {
                ToLog("获取商品列表异常，改成直接读取数据库：" + ex.Message, @"D:\log\redisdata.txt");
                totalCount = GetCount<WXMallProductInfo>(sbWhere.ToString());
                result = GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            }

            return result;

        }


        ///// <summary>
        ///// 获取商品列表-限时特卖 旧的
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public List<WXMallProductInfo> GetPromotionProductList(HttpContext context, out int totalCount)
        //{

        //    System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}'  And IsDelete=0 And IsPromotionProduct=1", WebsiteOwner));
        //    string promotionStartTimeStampStr = context.Request["promotion_start_timestamp"];//特卖开始时间戳
        //    string promotionStopTimeStampStr = context.Request["promotion_stop_timestamp"];//特卖结束时间戳

        //    double promotionStartTimeStamp = 0;
        //    double promotionStopTimeStamp = 0;
        //    string priceSort = context.Request["price_sort"];
        //    string discount_Sort = context.Request["discount_sort"];
        //    string pageIndexStr = context.Request["pageindex"];
        //    string pageSizeStr = context.Request["pagesize"];
        //    string haveStock = context.Request["is_havestock"];
        //    string keyWord = context.Request["keyword"];
        //    int pageIndex = 1;
        //    int pageSize = 10;
        //    if (!string.IsNullOrEmpty(pageIndexStr))
        //    {
        //        pageIndex = int.Parse(pageIndexStr);
        //    }
        //    if (!string.IsNullOrEmpty(pageSizeStr))
        //    {
        //        pageSize = int.Parse(pageSizeStr);
        //    }
        //    string orderBy = " Sort DESC,PID DESC";
        //    if (!string.IsNullOrEmpty(priceSort))
        //    {
        //        switch (priceSort)
        //        {
        //            case "price_asc":
        //                orderBy = " Price ASC";
        //                break;
        //            case "price_desc":
        //                orderBy = " Price DESC";
        //                break;

        //            default:
        //                break;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(discount_Sort))
        //    {
        //        sbWhere.AppendFormat(" And PreviousPrice>0");
        //        switch (discount_Sort)
        //        {
        //            case "discount_asc":
        //                orderBy = " (PromotionPrice/PreviousPrice) ASC";
        //                break;
        //            case "discount_desc":
        //                orderBy = " (PromotionPrice/PreviousPrice) DESC";
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(haveStock) && (haveStock == "1"))
        //    {
        //        sbWhere.AppendFormat(" And PromotionStock>0");
        //    }

        //    if (!string.IsNullOrEmpty(promotionStartTimeStampStr))
        //    {
        //        promotionStartTimeStamp = double.Parse(promotionStartTimeStampStr);
        //    }
        //    if (!string.IsNullOrEmpty(promotionStopTimeStampStr))
        //    {
        //        promotionStopTimeStamp = double.Parse(promotionStopTimeStampStr);
        //    }

        //    if (promotionStartTimeStamp > 0)
        //    {
        //        sbWhere.AppendFormat(" And PromotionStartTime >={0}", promotionStartTimeStamp);
        //    }
        //    if (promotionStopTimeStamp > 0)
        //    {
        //        sbWhere.AppendFormat(" And PromotionStopTime <={0}", promotionStopTimeStamp);
        //    }
        //    if (!string.IsNullOrEmpty(keyWord))
        //    {
        //        sbWhere.AppendFormat(" And (PName like '%{0}%' Or ProductCode like '%{0}%')", keyWord);
        //    }

        //    totalCount = GetCount<WXMallProductInfo>(sbWhere.ToString());
        //    return GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

        //}


        /// <summary>
        /// 获取商品列表-限时特卖V2 新的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<WXMallProductInfo> GetPromotionProductList(HttpContext context, out int totalCount)
        {
            string promotionActivityId = context.Request["promotion_activity_id"];//限时特卖活动ID
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;//页码
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;//页数
            pageSize = 10000;
            string keyWord = context.Request["keyword"];//关键字
            string categoryId = context.Request["category_id"];//分类Id
            string orderBy = " Sort DESC,PID DESC";//默认排序
            string priceSort = context.Request["price_sort"];//价格排序
            string discountSort = context.Request["discount_sort"];//折扣排序
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}'  And IsDelete=0 And IsPromotionProduct=1 And PromotionActivityId!=0 And PromotionActivityId!=''", WebsiteOwner));
            if (!string.IsNullOrEmpty(promotionActivityId))
            {
                sbWhere.AppendFormat(" And PromotionActivityId='{0}'", promotionActivityId);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat(" And CategoryId='{0}'", categoryId);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (PName like '%{0}%' Or ProductCode like '%{0}%')", keyWord);
            }
            if (!string.IsNullOrEmpty(priceSort))
            {
                switch (priceSort)
                {
                    case "price_asc":
                        orderBy = " Price ASC";//价格升序
                        break;
                    case "price_desc":
                        orderBy = " Price DESC";//价格降序
                        break;

                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(discountSort))
            {
                sbWhere.AppendFormat(" And PreviousPrice>0");
                switch (discountSort)
                {
                    case "discount_asc":
                        orderBy = " (Price/PreviousPrice) ASC";//折扣升序
                        break;
                    case "discount_desc":
                        orderBy = " (Price/PreviousPrice) DESC";//折扣降序
                        break;
                    default:
                        break;
                }
            }

            //totalCount = GetCount<WXMallProductInfo>(sbWhere.ToString());
            //return GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);


            var result = new List<WXMallProductInfo>();

            try
            {
                var orginKey = string.Format("pageSize{0}pageIndex{1}{2}orderBy{3}", pageSize, pageIndex, sbWhere.ToString(), orderBy);//原始key
                var listKey = string.Format("{0}:PL:{1}", WebsiteOwner, Common.DEncrypt.GetMD5(orginKey));

                var redisDataStr = string.Empty;

                if (!string.IsNullOrWhiteSpace(redisDataStr))
                {
                    var redisData = JsonConvert.DeserializeObject<Model.API.BaseWXMallProductInfoList>(redisDataStr);
                    totalCount = redisData.TotalCount;
                    result = redisData.List;
                }
                else
                {
                    totalCount = GetCount<WXMallProductInfo>(sbWhere.ToString());
                    result = GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

                    var redisData = new Model.API.BaseWXMallProductInfoList()
                    {
                        TotalCount = totalCount,
                        List = result
                    };

                    RedisHelper.RedisHelper.StringSet(listKey, JsonConvert.SerializeObject(redisData));

                    if (!RedisHelper.RedisHelper.SetContains(WebsiteOwner + ":" + Common.SessionKey.ShopListKeys, listKey))
                        RedisHelper.RedisHelper.SetAdd(WebsiteOwner + ":" + Common.SessionKey.ShopListKeys, listKey);
                }
            }
            catch (Exception ex)
            {
                ToLog("获取商品列表异常，改成直接读取数据库：" + ex.Message, @"D:\log\redisdata.txt");
                totalCount = GetCount<WXMallProductInfo>(sbWhere.ToString());
                result = GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            }

            return result;


        }


        /// <summary>
        ///获取商品总库存 根据sku统计
        /// </summary>
        /// <returns></returns>
        public int GetProductTotalStock(int productId)
        {
            var supplierId = "";
            var companyConfig = Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'",WebsiteOwner));
            if (companyConfig!=null&&companyConfig.StockType==1)
            {
                var currUserInfo=GetCurrentUserInfo();

                if (currUserInfo!=null&&currUserInfo.UserType == 7)
                {
                    supplierId = currUserInfo.AutoID.ToString();
                }
            }
            int totalCount = 0;
            var skuList = GetProductSkuList(productId, true, supplierId);
            if (skuList.Count > 0)
            {
                totalCount = skuList.Sum(p => p.Stock);
            }
            return totalCount;
        }

        /// <summary>
        ///获取商品限时特卖剩余总库存 根据sku统计
        /// </summary>
        /// <returns></returns>
        public int GetProductTotalStockPromotion(int productId)
        {
            int totalCount = 0;
            var skuList = GetProductSkuList(productId);
            if (skuList.Count > 0)
            {
                totalCount = skuList.Sum(p => p.PromotionStock);
            }
            return totalCount;
        }
        /// <summary>
        ///获取商品限时特卖 特卖总库存 
        /// </summary>
        /// <returns></returns>
        public int GetProductTotalPromotionSaleStock(int productId)
        {
            int totalCount = 0;
            var skuList = GetProductSkuList(productId);
            if (skuList.Count > 0)
            {
                totalCount = skuList.Sum(p => p.PromotionSaleStock);
            }
            return totalCount;
        }

        /// <summary>
        /// 获取最小的限时特卖价格
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public decimal GetMinPrommotionPrice(int productId)
        {
            decimal minPrice = 0;
            var skuList = GetProductSkuList(productId).Where(p => p.PromotionPrice > 0).ToList();
            if (skuList.Count > 0)
            {
                minPrice = skuList.Min(p => p.PromotionPrice);
            }
            return minPrice;


        }
        /// <summary>
        /// 获取显示价格
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public decimal GetShowPrice(WXMallProductInfo productInfo)
        {
            if (productInfo.PromotionActivityId != 0)
            {
                var dtNowStamp = GetTimeStamp(DateTime.Now);
                //if (skuInfo.PromotionPrice > 0 && skuInfo.PromotionStock > 0 && skuInfo.PromotionStartTime > 0 && skuInfo.PromotionStopTime > 0 && dtNowStamp >= skuInfo.PromotionStartTime && dtNowStamp < skuInfo.PromotionStopTime)
                //{
                //}
                if (GetCount<ProductSku>(string.Format(" PromotionPrice>0 And PromotionStock>0 And PromotionStartTime>0 And PromotionStopTime>0 And  PromotionStartTime<={0} And PromotionStopTime>{0} And ProductId={1}", dtNowStamp, productInfo.PID)) > 0)
                {
                    return GetMinPrommotionPrice(int.Parse(productInfo.PID));
                }

            }
            return productInfo.Price;
        }

        /// <summary>
        /// 根据积分商品id查询单个商品 无用
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public WXMallScoreProductInfo GetScoreProduct(int productId)
        {

            return Get<WXMallScoreProductInfo>(string.Format("AutoID={0}", productId));


        }
        /// <summary>
        /// 更新商品库存
        /// </summary>
        /// <param name="productId">商品id</param>
        /// <param name="buyCount">购买数量/取消数量</param>
        /// <returns></returns>
        public bool UpdateProductStock(int productId, int buyCount)
        {

            WXMallProductInfo model = GetProduct(productId.ToString());
            int count = model.Stock - buyCount;
            if (count < 0)
            {
                return false;
            }
            else
            {
                model.Stock = count;
                if (Update(model))
                {
                    BLLRedis.ClearProduct(WebsiteOwner, model.PID);
                    return true;
                }
                else
                {
                    return false;
                }

            }



        }

        /// <summary>
        /// 更新积分商品 库存 无用
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productCount"></param>
        /// <returns></returns>
        public bool UpdateScoreProductStock(int productId, int productCount)
        {

            WXMallScoreProductInfo model = GetScoreProduct(productId);
            int count = model.Stock - productCount;
            if (count < 0)
            {
                return false;
            }
            else
            {
                model.Stock = count;
                if (Update(model))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }



        }



        /// <summary>
        /// 获取商品销售数量
        /// </summary>
        /// <returns></returns>
        public int GetProductSaleCount(int productId)
        {

            string sql = string.Format("select sum(TotalCount) from ZCJ_WXMallOrderDetailsInfo where PID={0} And IsComplete =1; ", productId);
            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
            if (result != null)
            {
                return int.Parse(result.ToString());
            }
            return 0;

        }
        ///// <summary>
        ///// 更新IP访问量
        ///// </summary>
        ///// <param name="pid"></param>
        //public bool UpdateProductIP(string pid)
        //{

        //    WXMallProductInfo productInfo = GetProduct(pid);
        //    string strWhere = string.Format("/App/Cation/wap/mall/Showv1.aspx?action=show&pid={0}", pid);
        //    int ipCount = new BLL("").GetCount<WebAccessLogsInfo>("IP", string.Format("PageUrl like '%{0}'", strWhere));
        //    productInfo.IP = ipCount;
        //    return Update(productInfo);

        //}
        /// <summary>
        /// 更新PV访问量
        /// </summary>
        /// <param name="productId"></param>
        public bool UpdateWXMallProductPv(string productId)
        {
            WXMallProductInfo productInfo = GetProduct(productId);
            productInfo.PV++;
            return Update(productInfo);

        }

        /// <summary>
        /// 获取上一个商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetPreProductId(int productId)
        {
            int preProductId = 0;
            var preProduct = Get<WXMallProductInfo>(string.Format(" PID<{0} And IsDelete=0 And IsOnSale=1 And PID!={0}  And WebSiteOwner='{1}' Order by PID DESC", productId, WebsiteOwner));
            if (preProduct != null)
            {
                return int.Parse(preProduct.PID);
            }
            return preProductId;


        }
        /// <summary>
        /// 获取下一个商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetNextProductId(int productId)
        {
            int nextProductId = 0;
            var nextProduct = Get<WXMallProductInfo>(string.Format(" PID>{0} And IsDelete=0 And IsOnSale=1 And PID!={0} And WebSiteOwner='{1}' Order by PID ASC", productId, WebsiteOwner));
            if (nextProduct != null)
            {
                return int.Parse(nextProduct.PID);
            }
            return nextProductId;


        }
        /// <summary>
        /// 获取当前商品位置
        /// </summary>
        /// <returns></returns>
        public int GetCurrentProductIndex(int productId)
        {
            try
            {
                string sql = string.Format(" SELECT ROW_NUMBER() over (order by pid asc) as rowindex ,pid from ZCJ_WXMallProductInfo where IsDelete=0 And IsOnSale=1 And WebsiteOwner='{0}'", WebsiteOwner);
                DataTable dataTable = ZentCloud.ZCBLLEngine.BLLBase.Query(sql).Tables[0];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {

                    foreach (DataRow row in dataTable.Rows)
                    {

                        if (productId.ToString() == row[1].ToString())
                        {

                            return int.Parse(row[0].ToString());
                        }
                    }

                }
                return 0;
            }
            catch (Exception)
            {

                return 0;
            }


        }

        /// <summary>
        /// 获取商品总数量
        /// </summary>
        /// <returns></returns>
        public int GetTotalProductCount()
        {

            return GetCount<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' And IsDelete=0 And IsOnSale=1", WebsiteOwner));

        }

        /// <summary>
        /// 获取商品SKU
        /// </summary>
        /// <param name="skuSn">sku编码</param>
        public ProductSku GetProductSkuBySkuSn(string skuSn)
        {

            return Get<ProductSku>(string.Format(" WebSiteOwner='{0}' And SkuSN='{1}'", WebsiteOwner, skuSn));

        }


        /// <summary>
        /// 获取实际的SKU价格 如果是在特卖期间,则按特卖期间的价格计算
        /// </summary>
        /// <param name="skuInfo">sku信息</param>
        /// <returns></returns>
        public decimal GetSkuPrice(ProductSku skuInfo)
        {

            if (IsPromotionTime(skuInfo))
            {
                return skuInfo.PromotionPrice;
            }
            return skuInfo.Price;

        }



        /// <summary>
        /// 是否特卖期间
        /// </summary>
        /// <returns></returns>
        public bool IsPromotionTime(ProductSku skuInfo)
        {
            var dtNowStamp = GetTimeStamp(DateTime.Now);
            if (skuInfo.PromotionPrice > 0 && skuInfo.PromotionStock > 0 && skuInfo.PromotionStartTime > 0 && skuInfo.PromotionStopTime > 0 && dtNowStamp >= skuInfo.PromotionStartTime && dtNowStamp < skuInfo.PromotionStopTime)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取分类和所有子分类id
        /// </summary>
        /// <param name="preId"></param>
        /// <returns></returns>
        public string GetCateAndChildIds(int preId)
        {
            string result = string.Empty;
            string sql = string.Format("with a as(select AutoID from ZCJ_WXMallCategory where AutoID={0} union all select x.AutoID from ZCJ_WXMallCategory x,a where x.PreID=a.AutoID) select * from a", preId);
            var list = ZentCloud.ZCBLLEngine.BLLBase.Query<WXMallCategory>(sql);
            if (list.Count > 0)
            {
                result = string.Join(",", list.SelectMany(p => new List<int>() { (int)p.AutoID }));
            }
            return result;
        }

        /// <summary>
        /// 批量设置商品限制购买时间
        /// </summary>
        /// <param name="pids"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool UpdateProductBuyTime(string pids, string buyTimes)
        {
            var result = Update(new WXMallProductInfo(), string.Format(" LimitBuyTime='{0}' ", buyTimes), string.Format(" WebsiteOwner='{0}' AND PID in ({1})", WebsiteOwner, pids)) == pids.Split(',').Length;
            BLLRedis.ClearProductByIds(WebsiteOwner, pids, false);
            BLLRedis.ClearProductList(WebsiteOwner);
            return result;
        }

        /// <summary>
        /// 根据条件清除商品redis缓存
        /// </summary>
        /// <param name="where"></param>
        public void ClearProductListCacheByWhere(string where)
        {
            var productList = GetList<WXMallProductInfo>(where);
            if (productList != null && productList.Count > 0)
            {
                foreach (var item in productList)
                {
                    BLLRedis.ClearProduct(WebsiteOwner, item.PID, false);
                }
                BLLRedis.ClearProductList(WebsiteOwner);
            }
        }

        public void ClearProductListCacheByOrder(WXMallOrderInfo orderInfo)
        {
            List<string> productIds = new List<string>();

            foreach (var orderDetail in GetOrderDetailsList(orderInfo.OrderID).DistinctBy(p => p.PID))
            {
                productIds.Add(orderDetail.PID);
            }

            if (productIds.Count > 0)
            {
                BLLRedis.ClearProductByIds(orderInfo.WebsiteOwner, productIds, false);
                BLLRedis.ClearProductList(orderInfo.WebsiteOwner);
            }
        }

        #endregion

        #region 限时特卖模块

        /// <summary>
        /// 获取限时特卖活动
        /// </summary>
        /// <param name="promotionActivityId"></param>
        /// <returns></returns>
        public PromotionActivity GetPromotionActivity(int promotionActivityId)
        {

            return Get<PromotionActivity>(string.Format(" WebsiteOwner='{0}' And ActivityId={1}", WebsiteOwner, promotionActivityId));

        }



        #endregion


        #region 其它
        /// <summary>
        /// 设置经销商
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public bool SetAgent(int autoId)
        {
            return Update(new UserInfo(), string.Format("UserType=4"), string.Format("AutoId={0} And WebsiteOwner='{1}'", autoId, WebsiteOwner)) > 0 ? true : false;

        }


        ///// <summary>
        ///// 根据id获取门店信息 不使用
        ///// </summary>
        ///// <param name="?"></param>
        ///// <returns></returns>
        //public WXMallStores GetStore(string id)
        //{
        //    return Get<WXMallStores>(string.Format("AutoID={0}", id));

        //}


        #endregion


        #region 分类模块
        /// <summary>
        /// 获取某个 站点所有分类
        /// </summary>
        /// <returns></returns>
        public List<WXMallCategory> GetCategoryList()
        {

            return GetList<WXMallCategory>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));


        }
        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WXMallCategory> GetCategoryList(int pageIndex, int pageSize, string parentId, out int totalCount, string type = "", string keyWord = "")
        {

            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));
            if (!string.IsNullOrEmpty(parentId))
            {
                sbWhere.AppendFormat(" And PreID={0}", parentId);
            }
            if (string.IsNullOrEmpty(type) || type.ToLower().ToString() == "mall")
            {
                sbWhere.AppendFormat(" AND (Type ='Mall'  or Type is null) ");
            }
            else
            {
                sbWhere.AppendFormat(" AND Type ='{0}' ", type);
            }
            if (!string.IsNullOrEmpty(keyWord) && (keyWord != "undefined"))
            {
                sbWhere.AppendFormat(" AND CategoryName like '%{0}%' ", keyWord);
            }
            totalCount = GetCount<WXMallCategory>(sbWhere.ToString());
            return GetLit<WXMallCategory>(pageSize, pageIndex, sbWhere.ToString(), " Sort DESC ");

        }
        #endregion


        #region 配送员 外卖用 无用
        /// <summary>
        /// 获取某个 站点所有配送员
        /// </summary>
        /// <returns></returns>
        public List<WXMallDeliveryStaff> GetDeliveryStaffList()
        {

            return GetList<WXMallDeliveryStaff>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));


        }
        /// <summary>
        /// 获取单个配送员信息
        /// </summary>
        /// <returns></returns>
        public WXMallDeliveryStaff GetDeliveryStaff(int staffId)
        {

            return Get<WXMallDeliveryStaff>(string.Format("WebsiteOwner='{0}' And AutoID={1}", WebsiteOwner, staffId));


        }
        /// <summary>
        /// 获取单个配送员信息
        /// </summary>
        /// <returns></returns>
        public WXMallDeliveryStaff GetDeliveryStaff(string staffName)
        {

            return Get<WXMallDeliveryStaff>(string.Format("WebsiteOwner='{0}' And StaffName='{1}'", WebsiteOwner, staffName));


        }
        #endregion


        #region 积分记录模块
        ///// <summary>
        ///// 更新用户积分
        ///// </summary>
        ///// <param name="pid"></param>
        ///// <param name="buycount"></param>
        ///// <returns></returns>
        //public bool UpdateUserTotalScore(string userId, int score)
        //{
        //    UserInfo userInfo = bllUser.GetUserInfo(userId);
        //    double totalScore = userInfo.TotalScore - score;
        //    if (totalScore < 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        userInfo.TotalScore = totalScore;
        //        return Update(userInfo, string.Format("TotalScore={0}", totalScore), string.Format("AutoID={0}", userInfo.AutoID)) > 0;


        //    }



        //}

        /// <summary>
        /// 增加用户积分
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="buycount"></param>
        /// <returns></returns>
        public bool AddUserTotalScore(string userId, int score)
        {

            UserInfo userInfo = bllUser.GetUserInfo(userId);
            double totalScore = userInfo.TotalScore + score;
            if (totalScore < 0)
            {
                return false;
            }
            else
            {
                userInfo.TotalScore = totalScore;
                return Update(userInfo, string.Format("TotalScore={0}", totalScore), string.Format("AutoID={0}", userInfo.AutoID)) > 0;


            }



        }

        /// <summary>
        /// 获取用户积分记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WXMallScoreRecord> GetScoreRecordList(string userId, int pageIndex, int pageSize)
        {

            return GetLit<WXMallScoreRecord>(pageSize, pageIndex, string.Format("UserId='{0}'And WebsiteOwner='{1}'", userId, WebsiteOwner), " AutoID DESC");

        }

        /// <summary>
        /// 获取积分记录收入支出 总数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="intOut"></param>
        /// <returns></returns>
        public double GetScoreRecordTotalInOut(string userId, int intOut)
        {
            double resultd = 0;
            string sql = string.Format("select Sum(Score) from ZCJ_WXMallScoreRecord where UserId='{0}' And WebsiteOwner='{1}'", userId, WebsiteOwner);
            if (intOut == 0)//支出
            {
                sql += " And Score<0";
            }
            else if (intOut == 1)//收入
            {
                sql += " And Score>0";
            }

            string result = ZentCloud.ZCDALEngine.DbHelperSQL.Query(sql).Tables[0].Rows[0][0].ToString();
            if (!string.IsNullOrEmpty(result))
            {
                resultd = double.Parse(result);

            }
            return Math.Abs(resultd);


        }
        #endregion

        #region 物流模块 无用
        /// <summary>
        /// 获取单个配送方式
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public WXMallDelivery GetDelivery(int autoId)
        {
            return Get<WXMallDelivery>(string.Format("AutoId={0}", autoId));

        }
        /// <summary>
        /// 获取网站所有配送方式
        /// </summary>
        /// <returns></returns>
        public List<WXMallDelivery> GetDeliveryList()
        {

            return GetList<WXMallDelivery>(string.Format("WebsiteOwner='{0}' ORDER BY Sort ASC,AutoId ASC", WebsiteOwner));

        }
        #endregion


        #region 支付方式模块
        /// <summary>
        /// 获取单个支付方式
        /// </summary>
        /// <param name="autoid"></param>
        /// <returns></returns>
        public WXMallPaymentType GetPaymentType(int autoId)
        {
            return Get<WXMallPaymentType>(string.Format("AutoId={0}", autoId));

        }

        /// <summary>
        /// 获取网站所有支付方式
        /// </summary>
        /// <returns></returns>
        public List<WXMallPaymentType> GetPaymentTypeList()
        {
            return GetList<WXMallPaymentType>(string.Format("WebsiteOwner='{0}' And IsDisable=0 ORDER BY Sort ASC,AutoId ASC", WebsiteOwner));

        }
        #endregion


        #region 优惠券模块
        /// <summary>
        /// 添加优惠券 经销商添加
        /// </summary>
        /// <param name="userId">创建人用户名</param>
        /// <param name="disCount">折扣 0-10</param>
        /// <returns></returns>
        public bool AddCoupon(UserInfo userInfo, float disCount)
        {
            if (disCount <= 0 || disCount >= 10)
            {
                return false;
            }
            Coupon model = new Coupon();
            model.CouponNumber = string.Format("{0}{1}{2}{3}", userInfo.TagName, userInfo.GetUserAutoIDHex(), disCount * 10, new Random().Next(0, 99999));
            model.CouponType = 0;
            model.CreateUserId = userInfo.UserID;
            model.Discount = disCount;
            model.InsertDate = DateTime.Now;
            model.WebSiteOwner = WebsiteOwner;
            return Add(model);
        }
        /// <summary>
        /// 添加优惠券 系统添加
        /// </summary>
        /// <param name="disCount">折扣 0-10</param>
        /// <param name="productId">商品编号</param>
        /// <param name="startDate">生效日期</param>
        /// <param name="stopDate">失效日期</param>
        /// <returns></returns>
        public bool AddCoupon(float disCount, string productId, string startDate, string stopDate)
        {
            if (disCount <= 0 || disCount >= 10)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(productId))
            {
                var productInfo = GetProduct(productId);
                if (productInfo == null)
                {
                    return false;
                }
                if (productInfo.IsDelete.Equals(1))
                {
                    return false;

                }
                if (productInfo.IsOnSale.Equals(0))
                {
                    return false;
                }

            }
            DateTime dtStart = new DateTime();
            DateTime dtStop = new DateTime();
            Coupon model = new Coupon();
            model.CouponNumber = string.Format("TA{0}{1}", disCount * 10, new Random().Next(0, 99999));
            model.CouponType = 0;
            model.CreateUserId = GetCurrUserID();
            model.Discount = Math.Round(disCount, 1);
            model.InsertDate = DateTime.Now;
            model.WebSiteOwner = WebsiteOwner;
            if (!string.IsNullOrEmpty(startDate))
            {
                if (!DateTime.TryParse(startDate, out dtStart))
                {
                    return false;
                }
                model.StartDate = dtStart.ToString();
            }
            if (!string.IsNullOrEmpty(stopDate))
            {
                if (!DateTime.TryParse(stopDate, out dtStop))
                {
                    return false;
                }
                model.StopDate = dtStop.ToString();
            }
            if ((!string.IsNullOrEmpty(stopDate)) && (!string.IsNullOrEmpty(startDate)))
            {
                if (dtStop < dtStart)
                {
                    return false;
                }
            }
            return Add(model);
        }

        /// <summary>
        /// 查询单个优惠券
        /// </summary>
        /// <param name="couponNumber"></param>
        /// <returns></returns>
        public Coupon GetCoupon(string couponNumber)
        {
            return Get<Coupon>(string.Format("CouponNumber='{0}'", couponNumber));
        }


        /// <summary>
        ///删除优惠劵
        /// </summary>
        /// <param name="autoId">自动编号</param>
        /// <returns></returns>
        public bool DeleteCoupon(int autoId)
        {

            return Delete(new Coupon(), string.Format(" AutoId={0}", autoId)) > 0 ? true : false;

        }

        /// <summary>
        /// 查询优惠券
        /// </summary>
        /// <param name="pageIndex">查询第几页</param>
        /// <param name="pageSize">每页查询第几条</param>
        /// <param name="totalCount">总数</param>
        /// <param name="userId">用户</param>
        /// <param name="couponNumber">优惠券号码</param>
        /// <param name="couponType">优惠券类型</param>
        /// <returns></returns>
        public List<Coupon> GetCouponList(int pageIndex, int pageSize, out int totalCount, string userId = "", string couponNumber = "")
        {

            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}'", WebsiteOwner));
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And CreateUserId='{0}'", userId);
            }
            if (!string.IsNullOrEmpty(couponNumber))
            {
                sbWhere.AppendFormat(" And CouponNumber='{0}'", couponNumber);
            }

            totalCount = GetCount<Coupon>(sbWhere.ToString());
            return GetLit<Coupon>(pageSize, pageIndex, sbWhere.ToString(), " AutoId DESC");
        }

        /// <summary>
        /// 获取优惠金额（根据用户等级）
        /// </summary>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        public decimal GetDiscountAmount(UserInfo userInfo, decimal amount)
        {
            UserLevelConfig userLevel = bllUser.GetUserLevelByHistoryTotalScore(userInfo.HistoryTotalScore);
            if (userLevel != null)
            {
                return amount * (decimal)(userLevel.Discount / 10);
            }
            return amount;

        }
        /// <summary>
        /// 获取订单金额 根据优惠券
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="amount"></param>
        /// <param name="couponNumber"></param>
        /// <returns></returns>
        public decimal GetDiscountAmount(UserInfo userInfo, decimal amount, string couponNumber)
        {
            var couponModel = Get<Coupon>(string.Format(" CouponNumber='{0}'", couponNumber));
            if (couponModel != null)
            {
                return amount * (decimal)(couponModel.Discount / 10);
            }
            return amount;

        }

        /// <summary>
        /// 根据优惠券编号计算优惠金额
        /// </summary>
        /// <param name="cardCouponId">我的优惠券编号</param>
        /// <param name="orderData">订单数据</param>
        /// <param name="userId">用户名</param>
        /// <param name="isSuccess">true 成功 false 失败</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public decimal CalcDiscountAmount(string cardCouponId, string orderData, string userId, out bool isSuccess, out string msg, Enums.OrderType orderType = Enums.OrderType.Normal, string groupbuyType = "", decimal productFee = 0)
        {
            isSuccess = false;
            msg = "无法使用优惠券";

            var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(cardCouponId), userId);
            if (myCardCoupon == null || myCardCoupon.UserId != userId)
            {

                msg = "优惠券不存在";
                return 0;

            }
            if (myCardCoupon.Status == 1)
            {

                msg = "优惠券已经使用";
                return 0;

            }
            if (myCardCoupon.Status == 2)
            {

                msg = "优惠券已经转赠";
                return 0;

            }
            var cardCoupon = bllCardCoupon.GetCardCoupon(myCardCoupon.CardId);
            cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon, myCardCoupon);
            if (string.IsNullOrEmpty(cardCoupon.ExpireTimeType))//时间段
            {
                if (DateTime.Now < (DateTime)(cardCoupon.ValidFrom))
                {

                    msg = "优惠券开始时间未到";
                    return 0;

                }
                if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
                {

                    msg = "优惠券已经过期";
                    return 0;
                }
            }
            else
            {
                //检查过期时间


            }
            if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))//只有指定渠道才能使用
            {
                BLLDistribution bllDis = new BLLDistribution();
                string channelUserId = bllDis.GetUserChannel(bllDis.GetCurrentUserInfo());
                if (!string.IsNullOrEmpty(channelUserId))
                {
                    if (channelUserId != cardCoupon.BindChannelUserId)
                    {
                        msg = "只有指定渠道才可以使用";
                        return 0;
                    }
                }

            }
            if (orderType != Enums.OrderType.GroupBuy)
            {
                if (cardCoupon.IsCanUseShop == 0)
                {
                    msg = "该优惠券不能在商城使用";
                    return 0;
                }
            }

            if (orderType == Enums.OrderType.GroupBuy)
            {

                if (groupbuyType == "leader")
                {
                    if (cardCoupon.IsCanUseGroupbuy == 0)
                    {
                        msg = "该优惠券不能在开团使用";
                        return 0;
                    }
                }
                else if (groupbuyType == "member")
                {
                    if (cardCoupon.IsCanUseGroupbuyMember == 0)
                    {
                        msg = "该优惠券不能在参团使用";
                        return 0;
                    }
                }
                else
                {
                    msg = "没有找到开团类型";
                    return 0;
                }


            }

            //判断领取限制条件：
            if (cardCoupon.GetLimitType != null)
            {
                var currUser = GetCurrentUserInfo();

                if (cardCoupon.GetLimitType == "1" && !bllUser.IsDistributionMember(currUser, true))
                {
                    msg = "该券仅老用户（有购买历史）可以使用";
                    return 0;
                }
                if (cardCoupon.GetLimitType == "2" && bllUser.IsDistributionMember(currUser))
                {
                    msg = "该券仅新用户（无购买历史）可以使用";
                    return 0;
                }
            }

            decimal totalAmount = 0;
            decimal discountAmount = 0;

            if (orderType == Enums.OrderType.GroupBuy)
            {
                //团购，根据传入的商品总价计算，下面这个算法也应该是根据传入的计算，后面讨论下再做优化，先修复团购bug（应该是商品打折后的价格再算优惠券的折扣价）
                totalAmount = productFee;
            }
            else
            {
                OrderRequestModel orderRequestModel;
                try
                {
                    orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderRequestModel>(orderData);
                    foreach (var item in orderRequestModel.skus)
                    {
                        ProductSku sku = GetProductSku(item.sku_id);
                        totalAmount += GetSkuPrice(sku) * item.count;

                    }

                }
                catch (Exception ex)
                {
                    msg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                    return 0;

                }
            }

            switch (cardCoupon.CardCouponType)
            {
                case "MallCardCoupon_Discount"://商城卡券-折扣券 (凭折扣券对指定商品（全场）打折)
                    discountAmount = Math.Round(totalAmount - (totalAmount * (decimal.Parse(cardCoupon.Ex1) / 10)), 2);
                    isSuccess = true;
                    return discountAmount;
                case "MallCardCoupon_Deductible"://商城卡券-抵扣券 (支付时可以抵扣现金)
                    discountAmount = decimal.Parse(cardCoupon.Ex3);
                    isSuccess = true;
                    return discountAmount;
                case "MallCardCoupon_FreeFreight"://商城卡券-免邮券(满一定金额包邮)
                    if (totalAmount >= decimal.Parse(cardCoupon.Ex4))
                    {
                        //FreightModel freightModel = new FreightModel();
                        //freightModel.receiver_province_code = orderRequestModel.receiver_province_code;
                        //freightModel.receiver_city_code = orderRequestModel.receiver_city_code;
                        //freightModel.receiver_dist_code = orderRequestModel.receiver_dist_code;
                        //freightModel.skus = orderRequestModel.skus;
                        //string freightMsg = "";
                        //if (CalcFreight(freightModel, out discountAmount, out  freightMsg))
                        //{
                        //    isSuccess = true;

                        //}

                        //免邮金额已在前面一个下单步骤处理，这边不折扣
                        isSuccess = true;
                        return discountAmount;

                    }
                    else
                    {
                        msg = string.Format(" 应付金额满{0}元才可以使用该优惠券", cardCoupon.Ex4);
                    }
                    break;
                case "MallCardCoupon_Buckle"://商城卡券-满扣券(消费满一定金额减去一定金额)
                    if (totalAmount >= decimal.Parse(cardCoupon.Ex5))
                    {
                        isSuccess = true;
                        discountAmount = decimal.Parse(cardCoupon.Ex6);

                    }
                    else
                    {
                        msg = string.Format("订单金额需要满{0}元才能使用该优惠券", cardCoupon.Ex5);
                    }
                    break;
                case "MallCardCoupon_BuckleGive"://商城满送券
                    discountAmount = decimal.Parse(cardCoupon.Ex6);
                    isSuccess = true;
                    return discountAmount;
                    break;
                default:
                    break;
            }


            return discountAmount;

        }



        /// <summary>
        /// 计算优惠券可以优惠的金额
        /// </summary>
        /// <param name="cardCouponId"></param>
        /// <param name="totalAmount"></param>
        /// <param name="userId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public decimal CalcDiscountAmount(string cardCouponId, decimal totalAmount, string userId, out bool isSuccess, out string msg, out string couponName)
        {
            isSuccess = false;
            msg = "";
            couponName = "";
            var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(cardCouponId), userId);
            if (myCardCoupon == null || myCardCoupon.UserId != userId)
            {

                msg = "优惠券不存在";
                return 0;

            }
            if (myCardCoupon.Status == 1)
            {

                msg = "优惠券已经使用";
                return 0;

            }
            if (myCardCoupon.Status == 2)
            {

                msg = "优惠券已经转赠";
                return 0;

            }
            var cardCoupon = bllCardCoupon.GetCardCoupon(myCardCoupon.CardId);
            cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon, myCardCoupon);
            if (DateTime.Now < (DateTime)(cardCoupon.ValidFrom))
            {

                msg = "优惠券开始时间未到";
                return 0;

            }
            if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
            {

                msg = "优惠券已经过期";
                return 0;
            }
            couponName = cardCoupon.Name;

            decimal discountAmount = 0;
            switch (cardCoupon.CardCouponType)
            {
                case "MallCardCoupon_Discount"://商城卡券-折扣券 (凭折扣券对指定商品（全场）打折)
                    discountAmount = Math.Round(totalAmount - (totalAmount * (decimal.Parse(cardCoupon.Ex1) / 10)), 2);
                    isSuccess = true;
                    return discountAmount;
                case "MallCardCoupon_Deductible"://商城卡券-抵扣券 (支付时可以抵扣现金)
                    discountAmount = decimal.Parse(cardCoupon.Ex3);
                    isSuccess = true;
                    return discountAmount;
                case "MallCardCoupon_FreeFreight"://商城卡券-免邮券(满一定金额包邮)
                    msg = "暂不支持免邮券";
                    break;
                case "MallCardCoupon_Buckle"://商城卡券-满扣券(消费满一定金额减去一定金额)
                    if (totalAmount >= decimal.Parse(cardCoupon.Ex5))
                    {
                        isSuccess = true;
                        discountAmount = decimal.Parse(cardCoupon.Ex6);

                    }
                    else
                    {
                        msg = string.Format("订单金额需要满{0}元才能使用该优惠券", cardCoupon.Ex5);
                    }
                    break;
                default:
                    break;
            }


            return discountAmount;

        }


        /// <summary>
        /// 根据优惠券编号计算优惠金额 wifi使用
        /// </summary>
        /// <param name="cardCouponId">我的优惠券编号</param>
        /// <param name="orderData">订单数据</param>
        /// <param name="userId">用户名</param>
        /// <param name="isSuccess">true 成功 false 失败</param>
        /// <param name="totalAmount">租金总金额</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public decimal CalcDiscountAmountWifi(string cardCouponId, string orderData, string userId, out bool isSuccess, decimal totalAmount, out string msg)
        {
            isSuccess = false;
            msg = "不能使用该优惠券";

            var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(cardCouponId), userId);
            if (myCardCoupon == null || myCardCoupon.UserId != userId)
            {

                msg = "优惠券不存在";
                return 0;

            }
            if (myCardCoupon.Status == 1)
            {

                msg = "优惠券已经使用";
                return 0;

            }
            var cardCoupon = bllCardCoupon.GetCardCoupon(myCardCoupon.CardId);
            if (DateTime.Now < (DateTime)(cardCoupon.ValidFrom))
            {

                msg = "优惠券开始时间未到";
                return 0;

            }
            if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
            {

                msg = "优惠券已经过期";
                return 0;
            }

            decimal discountAmount = 0;//抵扣金额
            OrderRequestModel orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderRequestModel>(orderData); ;
            switch (cardCoupon.CardCouponType)
            {
                case "MallCardCoupon_Discount"://商城卡券-折扣券 (凭折扣券对指定商品（全场）打折)
                    discountAmount = Math.Round(totalAmount - (totalAmount * (decimal.Parse(cardCoupon.Ex1) / 10)), 2);
                    isSuccess = true;
                    break;
                case "MallCardCoupon_Deductible"://商城卡券-抵扣券 (支付时可以抵扣现金)
                    discountAmount = decimal.Parse(cardCoupon.Ex3);
                    isSuccess = true;
                    break;
                case "MallCardCoupon_FreeFreight"://商城卡券-免邮券(满一定金额包邮)
                    if (totalAmount >= decimal.Parse(cardCoupon.Ex4))
                    {
                        FreightModel freightModel = new FreightModel();
                        freightModel.receiver_province_code = orderRequestModel.receiver_province_code;
                        freightModel.receiver_city_code = orderRequestModel.receiver_city_code;
                        freightModel.receiver_dist_code = orderRequestModel.receiver_dist_code;
                        freightModel.skus = orderRequestModel.skus;
                        string freightMsg = "";
                        if (CalcFreight(freightModel, out discountAmount, out  freightMsg))
                        {
                            isSuccess = true;

                        }
                        //return discountAmount;


                    }
                    break;
                case "MallCardCoupon_Buckle"://商城卡券-满扣券(消费满一定金额减去一定金额)
                    if (totalAmount >= decimal.Parse(cardCoupon.Ex5))
                    {
                        isSuccess = true;
                        discountAmount = decimal.Parse(cardCoupon.Ex6);

                    }
                    else
                    {
                        msg = string.Format("租金金额需要满{0}元才能使用该优惠券", cardCoupon.Ex5);
                    }
                    break;
                default:
                    break;
            }

            if (discountAmount > totalAmount)
            {
                discountAmount = totalAmount;//最多只能抵扣租金金额
            }

            return discountAmount;


        }

        #endregion


        #region 商品SKU模块

        /// <summary>
        /// 获取单个SKU
        /// </summary>
        /// <param name="skuId">sku 编号</param>
        /// <param name="isNoCache">是否不读取缓存</param>
        /// <param name="supplierId">供应商Id</param>
        /// <returns></returns>
        public ProductSku GetProductSku(int skuId, bool isNoCache = false, string supplierId = "")
        {

            ProductSku result = new ProductSku();
            #region 数据库读取
            if (isNoCache)
            {
                if (string.IsNullOrEmpty(supplierId))
                {
                    result = Get<ProductSku>(string.Format(" SkuId={0}", skuId));
                }
                else
                {
                    //读取供应商自己的sku
                    var supplierSku = Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", skuId, supplierId));
                    if (supplierSku != null)
                    {
                        result = ConvertProductSku(supplierSku);
                    }
                }
                return result;

            }
            #endregion

            #region 缓存读取
            var key = string.Format("{0}:{1}:{2}", WebsiteOwner, Common.SessionKey.ProductSkuSingle, skuId);
            if (!string.IsNullOrEmpty(supplierId))
            {
                key = string.Format("{0}:{1}:{2}:{3}", WebsiteOwner, Common.SessionKey.ProductSkuSingle, skuId, supplierId);
            }
            try
            {
                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);

                if (string.IsNullOrWhiteSpace(cacheDataStr))
                {

                    if (string.IsNullOrEmpty(supplierId))
                    {
                        result = Get<ProductSku>(string.Format(" SkuId={0}", skuId));
                    }
                    else
                    {
                        //读取供应商自己的sku
                        var supplierSku = Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", skuId, supplierId));
                        if (supplierSku != null)
                        {
                            result = ConvertProductSku(supplierSku);
                        }
                    }
                    RedisHelper.RedisHelper.StringSet(key, JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<ProductSku>(cacheDataStr);
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(supplierId))
                {
                    result = Get<ProductSku>(string.Format(" SkuId={0}", skuId));
                }
                else
                {
                    //读取供应商自己的sku
                    var supplierSku = Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", skuId, supplierId));
                    if (supplierSku != null)
                    {
                        result = ConvertProductSku(supplierSku);
                    }
                }
            }
            return result;
            #endregion

        }

        /// <summary>
        /// 供应商sku 转换
        /// </summary>
        /// <param name="supplierSku"></param>
        /// <returns></returns>
        public ProductSku ConvertProductSku(ProductSkuSupplier supplierSku)
        {

            ProductSku sku = Get<ProductSku>(string.Format(" SkuId={0}", supplierSku.SkuId));
            sku.PromotionPrice=0;
            sku.PromotionStock= 0 ;
            sku.PromotionStartTime = 0;
            sku.PromotionStopTime =0;
            //sku.SkuId = supplierSku.SkuId;
            sku.Price = supplierSku.Price;
            sku.Stock = supplierSku.Stock;
            //sku.ProductId = supplierSku.ProductId;
            sku.BasePrice = supplierSku.BasePrice;
            return sku;

        }
        ///// <summary>
        ///// 获取sku 根据SKU Id
        ///// </summary>
        ///// <param name="skuId"></param>
        ///// <returns></returns>
        //public ProductSku GetSku(int skuId)
        //{
        //    return Get<ProductSku>(string.Format(" SkuId={0} ", skuId));
        //}

        ///// <summary>
        ///// 根据商品 sku 属性查询
        ///// </summary>
        ///// <param name="property"></param>
        ///// <returns></returns>
        //public ProductSku GetProductSku(string productId, string property)
        //{
        //    return Get<ProductSku>(string.Format(" Props='{0}' And WebSiteOwner='{1}'", property, WebsiteOwner));

        //}

        /// <summary>
        /// 获取某个商品所有SKU
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="supplierId">供应商Id</param>
        /// <returns></returns>
        public List<ProductSku> GetProductSkuList(int productId, bool isNoCache = false, string supplierId = "")
        {

            List<ProductSku> result = new List<ProductSku>(); ;
            #region 数据库读取
            if (isNoCache)
            {
                if (string.IsNullOrEmpty(supplierId))
                {
                    result = GetList<ProductSku>(string.Format(" ProductId={0}", productId));
                }
                else
                {
                    var supplierSkuList = GetList<ProductSkuSupplier>(string.Format(" ProductId={0} And SupplierId={1}", productId, supplierId));
                    foreach (var supplierSku in supplierSkuList)
                    {
                        result.Add(ConvertProductSku(supplierSku));

                    }
                }

                return result;

            }
            #endregion

            #region 缓存读取
            try
            {
                var key = string.Format("{0}:{1}:{2}", WebsiteOwner, Common.SessionKey.ProductSkus, productId);
                if (!string.IsNullOrEmpty(supplierId))
                {
                    key = string.Format("{0}:{1}:{2}:{3}", WebsiteOwner, Common.SessionKey.ProductSkus, productId, supplierId);
                }
                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);

                if (string.IsNullOrWhiteSpace(cacheDataStr))
                {

                    if (string.IsNullOrEmpty(supplierId))
                    {
                        result = GetList<ProductSku>(string.Format(" ProductId={0}", productId));
                    }
                    else
                    {

                        var supplierSkuList = GetList<ProductSkuSupplier>(string.Format(" ProductId={0} And SupplierId={1}", productId, supplierId));
                        foreach (var supplierSku in supplierSkuList)
                        {
                            result.Add(ConvertProductSku(supplierSku));

                        }
                    }

                    if (result != null && result.Count > 0)
                    {
                        RedisHelper.RedisHelper.StringSet(key, JsonConvert.SerializeObject(result));
                    }
                } 
                else
                {
                    result = JsonConvert.DeserializeObject<List<ProductSku>>(cacheDataStr);
                    if (result.Count == 0)
                    {
                        if (string.IsNullOrEmpty(supplierId))
                        {
                            result = GetList<ProductSku>(string.Format(" ProductId={0}", productId));
                        }
                        else
                        {

                            var supplierSkuList = GetList<ProductSkuSupplier>(string.Format(" ProductId={0} And SupplierId={1}", productId, supplierId));
                            foreach (var supplierSku in supplierSkuList)
                            {
                                result.Add(ConvertProductSku(supplierSku));

                            }

                        }
                        RedisHelper.RedisHelper.StringSet(key, JsonConvert.SerializeObject(result));
                    }
                }
            }
            catch (Exception ex)
            {

                if (string.IsNullOrEmpty(supplierId))
                {
                    result = GetList<ProductSku>(string.Format(" ProductId={0}", productId));
                }
                else
                {

                    var supplierSkuList = GetList<ProductSkuSupplier>(string.Format(" ProductId={0} And SupplierId={1}", productId, supplierId));
                    foreach (var supplierSku in supplierSkuList)
                    {
                        result.Add(ConvertProductSku(supplierSku));

                    }

                }
            }

            if (result == null)
            {
                result = new List<ProductSku>();
            }

            return result;
            #endregion

        }




        /// <summary>
        /// 更新单个SKU
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public bool UpdateProductSku(ProductSku model)
        {
            var result = Update(model);
            BLLRedis.ClearProductSkuSingle(WebsiteOwner, model.SkuId);
            return result;
        }

        /// <summary>
        /// 删除单个SKU
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public bool DeleteProductSku(ProductSku model)
        {
            var result = Delete(model) > 0;
            BLLRedis.ClearProductSkuSingle(WebsiteOwner, model.SkuId);
            return result;
        }

        /// <summary>
        /// 获取商品SKU属性 示例 1:1:尺码:S;2:5:颜色:蓝色
        /// </summary>
        /// <param name="skuId">sku编号</param>
        /// <returns></returns>
        public string GetProductProperties(int skuId)
        {
            ProductSku skuInfo = new ProductSku();
            try
            {
                
                skuInfo = GetProductSku(skuId);
                if (skuInfo == null)
                {
                    return "";
                }
                if (!string.IsNullOrEmpty(skuInfo.PropValueIdEx1) && !string.IsNullOrEmpty(skuInfo.PropValueIdEx2))
                {
                    //兼容efast 同步库存用 现已不使用
                    //1:1:尺码:S;2:5:颜色:蓝色
                    ProductProperty propInfoSize = GetProductProperty("尺码");
                    ProductProperty propInfoColor = GetProductProperty("颜色");
                    ProductPropertyValue propValueSize = GetProductPropertyValue(int.Parse(skuInfo.PropValueIdEx1));
                    ProductPropertyValue propValueColor = GetProductPropertyValue(int.Parse(skuInfo.PropValueIdEx2));
                    return string.Format("{0}:{1}:{2}:{3};{4}:{5}:{6}:{7}", propInfoSize.PropID, propValueSize.PropValueId, propInfoSize.PropName, propValueSize.PropValue, propInfoColor.PropID, propValueColor.PropValueId, propInfoColor.PropName, propValueColor.PropValue);
                }
                else
                {

                    StringBuilder sbProp = new StringBuilder();
                    foreach (var item in skuInfo.Props.Split(';'))
                    {
                        int propId = int.Parse(item.Split(':')[0]);
                        int propvalueId = int.Parse(item.Split(':')[1]);
                        BLLJIMP.Model.ProductProperty propInfo = GetProductProperty(propId);
                        BLLJIMP.Model.ProductPropertyValue propValueInfo = GetProductPropertyValue(propvalueId);
                        sbProp.AppendFormat("{0}:{1}:{2}:{3};", propInfo.PropID, propValueInfo.PropValueId, propInfo.PropName, propValueInfo.PropValue);

                    }
                    return sbProp.ToString().TrimEnd(';');
                    //return skuInfo.Props;


                }
            }
            catch (Exception)
            {
                //return "BLLMall.GetProductProperties Error";
                return skuInfo.Props;

            }


        }


        /// <summary>
        /// 获取商品SKU属性 示例 尺码:S:颜色:蓝色
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public string GetProductShowProperties(int skuId)
        {
            ProductSku skuInfo = new ProductSku();
            try
            {
                skuInfo = GetProductSku(skuId);
                if (skuInfo == null)
                {
                    return "";
                }
                if (!string.IsNullOrEmpty(skuInfo.PropValueIdEx1) && !string.IsNullOrEmpty(skuInfo.PropValueIdEx2))
                {
                    //尺码:S:颜色:蓝色
                    ProductProperty propInfoSize = GetProductProperty("尺码");
                    ProductProperty propInfoColor = GetProductProperty("颜色");
                    ProductPropertyValue propValueSize = GetProductPropertyValue(int.Parse(skuInfo.PropValueIdEx1));
                    ProductPropertyValue propValueColor = GetProductPropertyValue(int.Parse(skuInfo.PropValueIdEx2));
                    return string.Format("{0}:{1}:{2}:{3}", propInfoSize.PropName, propValueSize.PropValue, propInfoColor.PropName, propValueColor.PropValue);
                }
                else
                {
                    StringBuilder sbProp = new StringBuilder();
                    foreach (var item in skuInfo.Props.Split(';'))
                    {
                        int propId = int.Parse(item.Split(':')[0]);
                        int propvalueId = int.Parse(item.Split(':')[1]);
                        BLLJIMP.Model.ProductProperty propInfo = GetProductProperty(propId);
                        BLLJIMP.Model.ProductPropertyValue propValueInfo = GetProductPropertyValue(propvalueId);
                        sbProp.AppendFormat("{0}:{1};", propInfo.PropName, propValueInfo.PropValue);

                    }
                    return sbProp.ToString().TrimEnd(';');
                    //return skuInfo.ShowProps;

                }
            }
            catch (Exception)
            {
                //return "BLLMall.GetProductShowProperties Error";
                return skuInfo.ShowProps;
            }


        }
        /// <summary>
        /// 获取商品SKU属性 示例 尺码:S:颜色:蓝色
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public string GetProductStockProperties(string props)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(props)) return "";
                StringBuilder sbProp = new StringBuilder();
                foreach (var item in props.Split(';'))
                {
                    int propId = int.Parse(item.Split(':')[0]);
                    int propvalueId = int.Parse(item.Split(':')[1]);
                    BLLJIMP.Model.ProductProperty propInfo = GetProductProperty(propId);
                    BLLJIMP.Model.ProductPropertyValue propValueInfo = GetProductPropertyValue(propvalueId);
                    sbProp.AppendFormat("{0}:{1};", propInfo.PropName, propValueInfo.PropValue);
                }
                return sbProp.ToString().TrimEnd(';');
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 返回 每个商品的缺货记录
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public int GetProductStockByPID(string pid)
        {
            return Convert.ToInt32(GetSingle(string.Format(" Select sum([count]) from ZCJ_ProductStock where  WebsiteOwner='{0}' AND ProductId={1} ", bllCommRelation.WebsiteOwner, pid)));
        }

        /// <summary>
        ///  sku返还 取消订单,退货
        /// </summary>
        /// <param name="orderDetailList">订单明细</param>
        /// <returns></returns> 
        public bool ReturnProductSku(List<WXMallOrderDetailsInfo> orderDetailList)
        {

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                foreach (var orderDetail in orderDetailList)
                {

                    ProductSku sku = GetProductSku((int)orderDetail.SkuId);
                    if (Update(sku, string.Format(" Stock+={0}", orderDetail.TotalCount), string.Format(" SkuId={0}", sku.SkuId), tran) == 0)
                    {
                        tran.Rollback();
                        return false;

                    }

                    BLLRedis.ClearProductSkuSingle(WebsiteOwner, orderDetail.SkuId.Value);

                }
                tran.Commit();
                return true;

            }
            catch (Exception)
            {
                tran.Rollback();

            }
            return false;

        }


        #endregion


        #region 购物车模块

        /// <summary>
        /// 根据SKU获取购物车
        /// </summary>
        public ShoppingCart GetGetShoppingCartBySkuId(string userId, int skuId)
        {
            return Get<ShoppingCart>(string.Format(" UserId='{0}'And SkuId={1} And WebSiteOwner='{2}'", userId, skuId, WebsiteOwner));

        }

        /// <summary>
        /// 查询购物车列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ShoppingCart> GetShoppingCartList(string userId, bool isNoCache = false)
        {
            var key = string.Format("{0}:{1}:{2}", WebsiteOwner, Common.SessionKey.ShoppingCart, userId);

            List<ShoppingCart> result = null;

            if (isNoCache)
            {
                return GetList<ShoppingCart>(string.Format("UserId='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner));
            }

            try
            {
                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);

                if (string.IsNullOrWhiteSpace(cacheDataStr))
                {
                    result = GetList<ShoppingCart>(string.Format("UserId='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner));
                    RedisHelper.RedisHelper.StringSet(key, JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<List<ShoppingCart>>(cacheDataStr);
                }
            }
            catch (Exception ex)
            {
                result = GetList<ShoppingCart>(string.Format("UserId='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner));
            }

            if (result == null)
            {
                result = new List<ShoppingCart>();
            }

            return result;


        }
        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="skuId">SKu编号</param>
        /// <param name="count">数量</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool AddShoppingCart(string userId, int skuId, int count, out string msg, string supplierId = "")
        {
            //SKU
            ProductSku skuModel = GetProductSku(skuId);
            if (!string.IsNullOrEmpty(supplierId))
            {
                 var supplierSku = Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", skuId, supplierId));
                 skuModel = ConvertProductSku(supplierSku);
            }
            if (skuModel == null)
            {
                msg = "SKU不存在";
                return false;
            }
            //商品
            WXMallProductInfo productModel = GetProduct(skuModel.ProductId.ToString());
            msg = "false";
            ShoppingCart shoppingCartModel = GetGetShoppingCartBySkuId(userId, skuId);
            if (shoppingCartModel == null)// SKU从未添加到商品中，全新添加
            {

                if (productModel.IsOnSale == "0")
                {
                    msg = "已经下架";
                    return false;
                }
                if (count > skuModel.Stock)
                {
                    msg = "库存不足";
                    return false;
                }
                if (IsPromotionTime(skuModel))
                {
                    if (count > skuModel.PromotionStock)
                    {
                        msg = "特卖库存不足";
                        return false;
                    }


                }

                //if (productModel.IsPromotionProduct == 1)
                //{

                //    DateTime dtNow = DateTime.Now;
                //    if ((GetTimeStamp(dtNow) >= productModel.PromotionStartTime) && ((GetTimeStamp(dtNow) <= productModel.PromotionStopTime)))//限时特卖期间
                //    {

                //        if (count > productModel.PromotionStock)
                //        {
                //            msg = "限时特卖商品库存不足";
                //            return false;
                //        }
                //    }


                //}

                shoppingCartModel = new ShoppingCart();
                shoppingCartModel.CartId = int.Parse(GetGUID(TransacType.AddShoppingCart));
                shoppingCartModel.UserId = userId;
                shoppingCartModel.WebSiteOwner = WebsiteOwner;
                shoppingCartModel.Count = count;
                shoppingCartModel.DiscountFee = 0;//暂不使用
                shoppingCartModel.Freight = 0;//暂不使用
                shoppingCartModel.FreightTerms = "";//暂不使用
                shoppingCartModel.ImgUrl = productModel.RecommendImg;
                shoppingCartModel.InsertDate = DateTime.Now;
                shoppingCartModel.QuotePrice = productModel.PreviousPrice;
                //shoppingCartModel.Price = skuModel.Price;
                shoppingCartModel.Price = GetSkuPrice(skuModel);
                //if (productModel.IsPromotionProduct == 1)
                //{
                //    DateTime dtNow = DateTime.Now;
                //    if ((GetTimeStamp(dtNow) >= productModel.PromotionStartTime) && ((GetTimeStamp(dtNow) <= productModel.PromotionStopTime)))//限时特卖期间
                //    {
                //        shoppingCartModel.Price = productModel.PromotionPrice;//限时特卖期间价格
                //    }

                //}

                shoppingCartModel.ProductId = skuModel.ProductId;
                shoppingCartModel.SkuId = skuId;
                //shoppingCartModel.SkuPropertiesName = skuModel.ShowProps;
                shoppingCartModel.SkuPropertiesName = GetProductShowProperties(skuModel.SkuId);
                if (shoppingCartModel.SkuPropertiesName == null)
                {
                    shoppingCartModel.SkuPropertiesName = "";
                }
                shoppingCartModel.Title = productModel.PName;
                shoppingCartModel.TotalFee = shoppingCartModel.Price * count;
                shoppingCartModel.Tags = productModel.Tags;


                if (!string.IsNullOrEmpty(supplierId))
                {
                    var storeInfo = GetStoreInfo(supplierId);
                    shoppingCartModel.SupplierId = supplierId;
                    shoppingCartModel.SupplierName = storeInfo.ActivityName;
                    shoppingCartModel.StoreAddress = storeInfo.ActivityAddress;

                }
                else
                {
                    if (!string.IsNullOrEmpty(productModel.SupplierUserId))
                    {
                        var supplierUserInfo = GetSuppLierByUserId(productModel.SupplierUserId, WebsiteOwner);
                        if (supplierUserInfo != null)
                        {
                            shoppingCartModel.SupplierId = productModel.SupplierUserId;
                            shoppingCartModel.SupplierName = supplierUserInfo.Company; ;
                            shoppingCartModel.StoreAddress = supplierUserInfo.Address;
                        }

                    }
                }
                if (Add(shoppingCartModel))
                {
                    BLLRedis.ClearShoppingCart(WebsiteOwner, userId);
                    msg = "ok";
                    return true;
                }
                else
                {
                    msg = "添加购物车失败";
                    return false;
                }


            }
            else
            {
                //SKU 已经存在,更新购物车数量 
                //数量=原有+新增数量
                return UpdateShoppingCart(userId, skuId, count + shoppingCartModel.Count, out msg);

            }



        }

        /// <summary>
        /// 批量删除购物车 
        /// </summary>
        /// <param name="userId">当前用户</param>
        /// <param name="cartIds">购物车id集合，用逗号隔开</param>
        /// <returns></returns>
        public bool DeleteShoppingCart(string userId, string cartIds)
        {
            var result = Delete(new ShoppingCart(), string.Format(" UserId='{0}'And CartId in({1}) And WebSiteOwner='{2}'", userId, cartIds, WebsiteOwner)) == cartIds.Split(',').Length;
            BLLRedis.ClearShoppingCart(WebsiteOwner, userId);
            return result;

        }
        /// <summary>
        /// 修改购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skuId"></param>
        /// <param name="count"></param>
        /// <param name="msg"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public bool UpdateShoppingCart(string userId, int skuId, int count, out string msg,string supplierId="")
        {
            msg = "false";
            ProductSku skuModel = GetProductSku(skuId);
            if (!string.IsNullOrEmpty(supplierId))
            {
                var supplierSku = Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", skuId, supplierId));
                skuModel = ConvertProductSku(supplierSku);
            }
            if (skuModel == null)
            {
                msg = "SKU不存在";
                return false;
            }
            //商品
            WXMallProductInfo productModel = GetProduct(skuModel.ProductId.ToString());
            ShoppingCart shoppingCartModel = GetGetShoppingCartBySkuId(userId, skuId);
            if (productModel.IsOnSale == "0")
            {
                msg = "该商品已经下架";
                return false;
            }
            if (count > skuModel.Stock)
            {
                msg = "库存不足";
                return false;
            }
            if (IsPromotionTime(skuModel))
            {
                if (count > skuModel.PromotionStock)
                {
                    msg = "特卖库存不足";
                    return false;
                }


            }
            //if (productModel.IsPromotionProduct == 1)
            //{

            //    DateTime dtNow = DateTime.Now;
            //    if ((GetTimeStamp(dtNow) >= productModel.PromotionStartTime) && ((GetTimeStamp(dtNow) <= productModel.PromotionStopTime)))
            //    {
            //        if (count > productModel.PromotionStock)
            //        {
            //            msg = "限时特卖商品库存不足";
            //            return false;
            //        }
            //    }

            //}


            shoppingCartModel.Count = count;
            shoppingCartModel.TotalFee = shoppingCartModel.Price * count;
            if (Update(shoppingCartModel))
            {
                BLLRedis.ClearShoppingCart(WebsiteOwner, userId);
                msg = "ok";
                return true;
            }
            else
            {
                return false;

            }


        }

        /// <summary>
        ///清空购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ClearShoppingCart(string userId)
        {
            var result = Delete(new ShoppingCart(), string.Format(" UserId='{0}' And WebSiteOwner='{1}'", userId, WebsiteOwner)) > 0;
            BLLRedis.ClearShoppingCart(WebsiteOwner, userId);
            return result;

        }
        /// <summary>
        ///删除购物车(删除已经下单的商品)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteShoppingCart(string userId, List<SkuModel> skus)
        {
            try
            {
                var result = Delete(new ShoppingCart(), string.Format(" UserId='{0}' And WebSiteOwner='{1}' And SkuId in ({2}) ", userId, WebsiteOwner, string.Join(",", skus.SelectMany(p => new List<int>() { (int)p.sku_id })))) > 0;
                BLLRedis.ClearShoppingCart(WebsiteOwner, userId);
                return result;
            }
            catch (Exception)
            {

                return false;
            }


        }


        #endregion


        #region 特征量模块


        /// <summary>
        /// 根据商品属性名称 查询商品属性
        /// </summary>
        /// <returns></returns>
        public ProductProperty GetProductProperty(string propName)
        {
            return Get<ProductProperty>(string.Format(" PropName='{0}' And WebSiteOwner='{1}'", propName, WebsiteOwner));

        }


        /// <summary>
        /// 根据商品属性ID 查询商品属性
        /// </summary>
        /// <returns></returns>
        public ProductProperty GetProductProperty(int propId)
        {
            return Get<ProductProperty>(string.Format(" PropID='{0}' And WebSiteOwner='{1}'", propId, WebsiteOwner));

        }


        /// <summary>
        /// 获取所有属性列表
        /// </summary>
        /// <returns></returns>
        public List<ProductProperty> GetProductPropertyList()
        {

            return GetList<ProductProperty>(string.Format(" WebSiteOwner='{0}'", WebsiteOwner));

        }


        /// <summary>
        /// 添加商品特征量
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool AddProductProperty(string propName, out int propId, out string msg)
        {

            propId = 0;
            msg = "";
            if (GetProductProperty(propName) != null)
            {
                msg = "特征量已经存在";
                return false;
            }
            if (propName.Contains(":"))
            {
                msg = "特征量名称不能包含:";
                return false;
            }
            if (propName.Contains(";"))
            {
                msg = "特征量名称不能包含 ;";
                return false;
            }
            ProductProperty model = new ProductProperty();
            model.InsertDate = DateTime.Now;
            model.PropID = int.Parse(GetGUID(TransacType.AddProductProperty));
            model.PropName = propName;
            model.WebSiteOwner = WebsiteOwner;

            if (Add(model))
            {
                propId = model.PropID;
                msg = "ok";
                return true;
            }
            else
            {
                msg = "添加失败";
            }
            return false;

        }

        /// <summary>
        /// 添加商品特征量
        /// </summary>
        /// <param name="propId">特征量ID</param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool UpdateProductProperty(int propId, string propName, out string msg)
        {

            msg = "";
            ProductProperty model = GetProductProperty(propId);
            if (model == null)
            {
                msg = "特征量值不存在";
                return false;
            }
            if (propName.Contains(":"))
            {
                msg = "特征量名称不能包含:";
                return false;
            }
            if (propName.Contains(";"))
            {
                msg = "特征量名称不能包含 ;";
                return false;
            }
            model.PropName = propName;
            model.Modified = DateTime.Now;
            if (Update(model))
            {

                msg = "ok";
                return true;
            }
            else
            {
                msg = "添加失败";
            }
            return false;

        }


        /// <summary>
        /// 删除商品特征量
        /// </summary>
        /// <param name="propId">特征量ID</param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool DeleteProductProperty(string propIds)
        {
            int count = Delete(new ProductProperty(), string.Format(" PropID in ({0}) And WebSiteOwner='{1}'", propIds, WebsiteOwner));

            if (propIds.Split(',').Length == count)
            {
                return true;
            }
            return false;
        }







        #endregion


        #region 特征量值模块


        /// <summary>
        /// 获取商品特征量值列表
        /// </summary>
        /// <param name="propId"></param>
        /// <returns></returns>
        public List<ProductPropertyValue> GetProductPropertyValueList(int propId)
        {

            return GetList<ProductPropertyValue>(string.Format("PropID={0}", propId));


        }
        /// <summary>
        /// 获取商品特征量值列表 根据属性名称
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public List<ProductPropertyValue> GetProductPropValueList(string propName)
        {
            var propModel = GetProductProperty(propName);
            if (propModel == null)
            {
                return null;
            }
            return GetList<ProductPropertyValue>(string.Format("PropID={0}", propModel.PropID));

        }
        /// <summary>
        /// 添加商品特征量值
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool AddProductPropertyValue(int propId, string propValue, out int propertyValueId, out string msg)
        {
            propertyValueId = 0;
            msg = "";
            if (GetCount<ProductPropertyValue>(string.Format("WebSiteOwner='{0}' And PropID={1} And PropValue='{2}'", WebsiteOwner, propId, propValue)) > 0)
            {
                msg = "特征量值已经存在";
                return false;
            }
            if (propValue.Contains(":"))
            {
                msg = "特征量值不能包含:";
                return false;
            }
            if (propValue.Contains(";"))
            {
                msg = "特征量值不能包含 ;";
                return false;
            }
            ProductPropertyValue model = new ProductPropertyValue();
            model.PropValueId = int.Parse(GetGUID(TransacType.AddProductPropertyValue));
            model.InsertDate = DateTime.Now;
            model.PropID = propId;
            model.PropValue = propValue;
            model.WebSiteOwner = WebsiteOwner;
            model.PropValueId = int.Parse(GetGUID(TransacType.AddProductPropertyValue));
            if (Add(model))
            {
                propertyValueId = model.PropValueId;
                msg = "ok";
                return true;
            }
            else
            {
                msg = "添加失败";
            }
            return false;

        }

        /// <summary>
        ///更新商品特征量值
        /// </summary>
        /// <param name="propId">特征量ID</param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool UpdateProductPropertyValue(int propValueId, int propId, string propValue, out string msg)
        {

            msg = "";
            ProductPropertyValue model = GetProductPropertyValue(propValueId);
            if (model == null)
            {
                msg = "特征量值不存在";
                return false;
            }
            model.Modified = DateTime.Now;
            model.PropID = propId;
            model.PropValue = propValue;

            if (Update(model))
            {

                msg = "ok";
                return true;
            }
            else
            {
                msg = "添加失败";
            }
            return false;

        }


        /// <summary>
        /// 删除商品特征量值
        /// </summary>
        /// <param name="propId">特征量ID</param>
        /// <returns></returns>
        public bool DeleteProductPropertyValue(string propValueIds)
        {
            int count = Delete(new ProductPropertyValue(), string.Format(" PropValueId in ({0}) And WebSiteOwner='{1}'", propValueIds, WebsiteOwner));

            if (propValueIds.Split(',').Length == count)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取特征量值 根据特征量ID
        /// </summary>
        /// <param name="propValueId"></param>
        /// <returns></returns>
        public ProductPropertyValue GetProductPropertyValue(int propValueId)
        {

            return Get<ProductPropertyValue>(string.Format(" PropValueId={0}", propValueId));


        }

        #endregion


        #region 外部模块
        /// <summary>
        /// 获取EFast外部条码
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public string GetEfastBarcode(int skuId)
        {
            string result = string.Empty;

            result = GetProductSku(skuId).OutBarCode;

            return result;
        }


        /// <summary>
        /// 获取Sku库存 如果使用外部库存,则读取外部库存
        /// </summary>
        /// <param name="outBarcode"></param>
        /// <returns></returns>
        public int GetSkuCount(ProductSku skuInfo)
        {

            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
            {

                if (!string.IsNullOrEmpty(skuInfo.OutBarCode))
                {

                    string shopIdStr = System.Configuration.ConfigurationManager.AppSettings["eFastShopId"];
                    if (!string.IsNullOrEmpty(shopIdStr))
                    {
                        int shopId = int.Parse(shopIdStr);
                        var eFastSku = efast.GetSkuStock(shopId, skuInfo.OutBarCode);
                        if (eFastSku != null)
                        {

                            if (skuInfo.Stock != eFastSku.sl)
                            {
                                skuInfo.Stock = eFastSku.sl;
                                Update(skuInfo);//更新内部库存
                            }
                            return eFastSku.sl;
                        }

                    }

                }
            }
            return skuInfo.Stock;


        }

        #endregion


        #region 运费模块
        /// <summary>
        /// 获取所有运费模板信息
        /// </summary>
        /// <returns></returns>
        public List<FreightTemplate> GetFreightTemplateList()
        {

            return GetList<FreightTemplate>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));


        }
        /// <summary>
        /// 获取指定运费模板所有配送规则
        /// </summary>
        /// <returns></returns>
        public List<FreightTemplateRule> GetFreightTemplateRuleList(int templateId)
        {

            return GetList<FreightTemplateRule>(string.Format(" WebsiteOwner='{0}' And TemplateId={1}", WebsiteOwner, templateId));


        }




        /// <summary>
        /// 获取指定运费模板消息
        /// </summary>
        /// <returns></returns>
        public FreightTemplate GetFreightTemplate(int freightTemplateId)
        {

            return Get<FreightTemplate>(string.Format(" WebsiteOwner='{0}' And TemplateId={1}", WebsiteOwner, freightTemplateId));


        }

        /// <summary>
        /// 删除运费模板
        /// </summary>
        /// <param name="freightTemplateId">模板Id</param>
        /// <returns></returns>
        public bool DeleteFreightTemplate(int freightTemplateId)
        {

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (Delete(new FreightTemplate(), string.Format(" TemplateId ={0}", freightTemplateId), tran) <= 0)
                {
                    tran.Rollback();
                    return false;
                }

                if (Delete(new FreightTemplateRule(), string.Format(" TemplateId ={0}", freightTemplateId), tran) <= 0)
                {
                    tran.Rollback();
                    return false;
                }

                //更新商品表模板ID
                Update(new WXMallProductInfo(), string.Format(" FreightTemplateId=0"), string.Format(" FreightTemplateId={0} ", freightTemplateId));
                tran.Commit();

                var productList = GetList<WXMallProductInfo>(string.Format(" FreightTemplateId={0} ", freightTemplateId));

                if (productList != null && productList.Count > 0)
                {
                    foreach (var item in productList)
                    {
                        BLLRedis.ClearProduct(WebsiteOwner, item.PID);
                    }
                    BLLRedis.ClearProductList(WebsiteOwner);
                }

                return true;
            }
            catch (Exception)
            {
                tran.Rollback();
                return false;
            }



        }
        /// <summary>
        /// 删除运费模板规则
        /// </summary>
        /// <returns></returns>
        public bool DeleteFreightTemplateRule(int freightTemplateId)
        {

            if (Delete(new FreightTemplateRule(), string.Format(" TemplateId ={0}", freightTemplateId)) >= 0)
            {
                return true;
            }
            return false;

        }



        /// <summary>
        /// 运费计算 统一按数量 旧的 使用运费模板后不使用 无用
        /// </summary> 
        /// <returns></returns>
        public decimal CalcFreight(int totalCount)
        {

            int deliverId = 0;
            switch (HttpContext.Current.Request.Url.Host)
            {
                case "localhost":
                    deliverId = 9;//本地
                    break;
                case "dev1.comeoncloud.net"://开发
                    deliverId = 11;
                    break;
                case "mixblu.comeoncloud.net"://正式
                    deliverId = 37;
                    break;
                default:
                    break;
            }


            decimal freight = 0;
            WXMallDelivery delivery = GetDelivery(deliverId);
            if (delivery == null)
            {
                return 0;
            }
            if (totalCount <= delivery.InitialProductCount)
            {
                freight = delivery.InitialDeliveryMoney;
            }
            else
            {

                freight = delivery.InitialDeliveryMoney + Math.Ceiling((decimal)(totalCount - delivery.InitialProductCount) / delivery.AddProductCount) * delivery.AddMoney;

            }

            return freight;



        }

        ///// <summary>
        ///// 运费计算 按运费模板及统一运费计算
        ///// </summary>
        ///// <returns></returns>
        //public bool CalcFreight(FreightModel freightModel, out decimal freight, out string msg)
        //{
        //    //运费计算规则:
        //    //情况1 所选商品全部是统一邮费,则从中选择邮费最低的
        //    //情况2 不同或相同的商品，设置同一运费模板：按该模板设置的规则计算；
        //    //情况3 所有商品都设置了运费模板而且有多个模板 各个运费模板计算累加
        //    //情况4 既有运费模板又有统一运费 各个运费模板单独计算累加并加上最小的统一运费
        //    //参考 http://bbs.youzan.com/forum.php?mod=viewthread&tid=6150

        //    msg = "很抱歉，该地区暂不支持配送";
        //    freight = 0;//运费

        //    try
        //    {
        //        ///各个运费模板对应的商品数量
        //        List<FreightTemplateSkuMapping> freightTemplateSkuMapList = new List<FreightTemplateSkuMapping>();
        //        #region 获取所有下单商品跟使用到的运费模板
        //        List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//此订单包含的所有商品
        //        List<int> freightTemplateIdList = new List<int>();//此订单用到的所有运费模板ID
        //        foreach (var sku in freightModel.skus)
        //        {

        //            var skuInfo = GetProductSku(sku.sku_id);
        //            WXMallProductInfo productInfo = GetProduct(skuInfo.ProductId.ToString());
        //            productList.Add(productInfo);
        //            if (productInfo.FreightTemplateId > 0)//此商品设置了运费模板
        //            {
        //                if (!freightTemplateIdList.Contains(productInfo.FreightTemplateId))
        //                {
        //                    freightTemplateIdList.Add(productInfo.FreightTemplateId);
        //                }

        //                // 各个运费模板对应的商品数量
        //                FreightTemplateSkuMapping freightTemplateMap = new FreightTemplateSkuMapping();
        //                freightTemplateMap.SkuCount = sku.count;
        //                freightTemplateMap.TemplateId = productInfo.FreightTemplateId;
        //                freightTemplateSkuMapList.Add(freightTemplateMap);


        //            }



        //        }


        //        #endregion

        //        #region 获取所有用到的规则
        //        List<FreightTemplateRuleModel> allUseRuleList = new List<FreightTemplateRuleModel>();//此次订单用到的所有配送规则
        //        foreach (var templateId in freightTemplateIdList)
        //        {
        //            List<FreightTemplateRule> freightTemplateRuleList = GetFreightTemplateRuleList(templateId);
        //            foreach (var sourceRule in freightTemplateRuleList)
        //            {
        //                FreightTemplateRuleModel rule = new FreightTemplateRuleModel();
        //                rule.RuleId = sourceRule.AutoId;
        //                rule.TemplateId = sourceRule.TemplateId;
        //                rule.InitialProductCount = sourceRule.InitialProductCount;
        //                rule.InitialAmount = sourceRule.InitialAmount;
        //                rule.AddProductCount = sourceRule.AddProductCount;
        //                rule.AddAmount = sourceRule.AddAmount;
        //                rule.AreaCodeList = (!string.IsNullOrEmpty(sourceRule.AreaCodes) ? (sourceRule.AreaCodes.Split(',')) : (new string[0])).ToList();
        //                allUseRuleList.Add(rule);
        //            }

        //        }
        //        #endregion

        //        #region 邮费计算 (循环计算累加各个运费模板费用并加上统一邮费)
        //        foreach (var templateId in freightTemplateIdList)
        //        {

        //            var templateRuleList = allUseRuleList.Where(p => p.TemplateId == templateId);
        //            if (templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_dist_code.ToString())).Count() == 0 && templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_city_code.ToString())).Count() == 0 && templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_province_code.ToString())).Count() == 0)
        //            {

        //                return false;//不能配送到此地区

        //            }
        //            //计算使用此模板的商品数量
        //            int totalCount = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.SkuCount);

        //            ///区域代码匹配规则
        //            ///如果城市区域匹配，则使用城市区域的规则
        //            ///如果城市区域不匹配，则查找城市代码，看是否匹配
        //            ///如果城市不匹配，则查找省份代码
        //            ///省市区 先匹配区，再匹配市，再匹配省 
        //            var distRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_dist_code.ToString()));// 区对应规则
        //            var cityRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_city_code.ToString()));// 城市对应规则
        //            var provinceRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_province_code.ToString()));//省对应规则
        //            if (distRule != null)
        //            {

        //                freight += distRule.InitialAmount + Math.Ceiling((decimal)(totalCount - distRule.InitialProductCount) / distRule.AddProductCount) * distRule.AddAmount;


        //            }
        //            else if (cityRule != null)
        //            {
        //                freight += cityRule.InitialAmount + Math.Ceiling((decimal)(totalCount - cityRule.InitialProductCount) / cityRule.AddProductCount) * cityRule.AddAmount;


        //            }
        //            else if (provinceRule != null)
        //            {

        //                freight += provinceRule.InitialAmount + Math.Ceiling((decimal)(totalCount - provinceRule.InitialProductCount) / provinceRule.AddProductCount) * provinceRule.AddAmount;
        //            }

        //        }

        //        //累加最小的统一运费
        //        freight += productList.Min(p => p.UnifiedFreight);
        //        return true;
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {

        //        msg = ex.Message;
        //        return false;
        //    }




        //}



        /// <summary>
        /// 运费计算
        /// </summary>
        /// <param name="freightModel">运费模型</param>
        /// <param name="freight">总运费</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public bool CalcFreight(FreightModel freightModel, out decimal totalFreight, out string msg)
        {
            try
            {


                //1.拆分供应商
                //2.计算按数量计算的运费总和
                //3.计算按重量计算的运费总和
                //4. 按数量计算的运费总和加上按重量计算的运费总和 即为总运费

                #region 此次订单包含的所有商品及sku
                List<ProductSku> skuList = new List<ProductSku>();//此次订单包含的所有sku
                List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//此次订单包含的所有商品
                foreach (var item in freightModel.skus)
                {
                    ProductSku productSku = GetProductSku(item.sku_id);
                    skuList.Add(productSku);
                    WXMallProductInfo prodcutInfo = GetProduct(productSku.ProductId);
                    if (productList.Count(p => p.PID == productSku.ProductId.ToString()) == 0)
                    {
                        productList.Add(prodcutInfo);
                    }


                }
                #endregion

                #region 生成本次订单共有多少供应商
                //检查有几个供应商
                List<string> supplierUserIdList = new List<string>();//供应商列表
                foreach (var item in productList)
                {
                    if (string.IsNullOrEmpty(item.SupplierUserId))
                    {
                        item.SupplierUserId = "";
                    }
                    supplierUserIdList.Add(item.SupplierUserId);
                }
                if (supplierUserIdList.Count > 0)
                {
                    supplierUserIdList = supplierUserIdList.Distinct().ToList();
                }

                #endregion

                #region 生成供应商对应sku
                Dictionary<string, List<SkuModel>> dic = new Dictionary<string, List<SkuModel>>(); //key 供应商 value Sku集合
                foreach (var suppLierUserId in supplierUserIdList)
                {
                    var productListChild = productList.Where(p => p.SupplierUserId == suppLierUserId).ToList();
                    foreach (var item in productListChild)
                    {
                        List<SkuModel> list = new List<SkuModel>();
                        foreach (var sku in skuList)
                        {
                            if (sku.ProductId.ToString() == item.PID)
                            {
                                SkuModel model = new SkuModel();
                                model.sku_id = freightModel.skus.Single(p => p.sku_id == sku.SkuId).sku_id;
                                model.count = freightModel.skus.Single(p => p.sku_id == sku.SkuId).count;
                                list.Add(model);
                            }

                        }
                        if (!dic.ContainsKey(suppLierUserId))
                        {
                            dic.Add(suppLierUserId, list);
                        }
                        else
                        {
                            dic[suppLierUserId].AddRange(list);

                        }


                    }



                }
                #endregion

                totalFreight = 0;//总运费
                msg = "ok";
                foreach (var item in dic)
                {

                    //
                    decimal freightSupplier = 0;//每个供应商总运费
                    decimal freightCount = 0;//按数量计算的运费总和
                    decimal freightWeight = 0;//按重量计算的运费总和


                    FreightModel freightModelCount = new FreightModel();//按数量计算的运费模型
                    freightModelCount.receiver_province_code = freightModel.receiver_province_code;
                    freightModelCount.receiver_city_code = freightModel.receiver_city_code;
                    freightModelCount.receiver_dist_code = freightModel.receiver_dist_code;
                    freightModelCount.skus = new List<SkuModel>();


                    FreightModel freightModelWeight = new FreightModel();//按重量计算的运费模型
                    freightModelWeight.receiver_province_code = freightModel.receiver_province_code;
                    freightModelWeight.receiver_city_code = freightModel.receiver_city_code;
                    freightModelWeight.receiver_dist_code = freightModel.receiver_dist_code;
                    freightModelWeight.skus = new List<SkuModel>();


                    foreach (var itemSupplier in item.Value)
                    {
                        ProductSku productSku = skuList.Single(p => p.SkuId == itemSupplier.sku_id);
                        WXMallProductInfo prodcutInfo = productList.Single(p => p.PID == productSku.ProductId.ToString());
                        if (prodcutInfo.FreightTemplateId != 0)
                        {
                            var freightTemplate = GetFreightTemplate(prodcutInfo.FreightTemplateId);
                            switch (freightTemplate.CalcType)
                            {
                                case "count"://按数量计算
                                    freightModelCount.skus.Add(itemSupplier);
                                    break;
                                case "weight"://按重量计算
                                    freightModelWeight.skus.Add(itemSupplier);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                    #region 按数量计算运费

                    if (freightModelCount.skus.Count > 0)
                    {
                        if (!CalcFreightCount(freightModelCount, out freightCount, out msg))
                        {
                            return false;
                        }
                        else
                        {
                            freightSupplier += freightCount;
                        }
                    }

                    #endregion

                    #region 按重量计算运费

                    if (freightModelWeight.skus.Count > 0)
                    {
                        if (!CalcFreightWeight(freightModelWeight, out freightWeight, out msg))
                        {
                            return false;
                        }
                        else
                        {
                            freightSupplier += freightWeight;
                        }
                    }

                    #endregion



                    //

                    totalFreight += freightSupplier;

                }





                return true;

            }
            catch (Exception ex)
            {
                totalFreight = 0;
                msg = ex.ToString();
                return false;
            }
        }


        /// <summary>
        /// 运费计算 按数量
        /// </summary>
        /// <returns></returns>
        public bool CalcFreightCount(FreightModel freightModel, out decimal freight, out string msg)
        {
            //运费计算规则:
            //情况1 所选商品全部是统一邮费,则从中选择邮费最低的
            //情况2 不同或相同的商品，设置同一运费模板：按该模板设置的规则计算；
            //情况3 所有商品都设置了运费模板而且有多个模板 各个运费模板计算累加
            //情况4 既有运费模板又有统一运费 各个运费模板单独计算累加并加上最小的统一运费
            //参考 http://bbs.youzan.com/forum.php?mod=viewthread&tid=6150

            msg = "很抱歉，该地区暂不支持配送";
            freight = 0;//运费

            try
            {


                ///各个运费模板对应的商品数量
                List<FreightTemplateSkuMapping> freightTemplateSkuMapList = new List<FreightTemplateSkuMapping>();
                #region 获取所有下单商品跟使用到的运费模板
                List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//此订单包含的所有商品
                List<int> freightTemplateIdList = new List<int>();//此订单用到的所有运费模板ID
                foreach (var sku in freightModel.skus)
                {

                    var skuInfo = GetProductSku(sku.sku_id);
                    WXMallProductInfo productInfo = GetProduct(skuInfo.ProductId.ToString());
                    productList.Add(productInfo);
                    if (productInfo.FreightTemplateId > 0)//此商品设置了运费模板
                    {
                        if (!freightTemplateIdList.Contains(productInfo.FreightTemplateId))
                        {
                            freightTemplateIdList.Add(productInfo.FreightTemplateId);
                        }

                        // 各个运费模板对应的商品数量
                        FreightTemplateSkuMapping freightTemplateMap = new FreightTemplateSkuMapping();
                        freightTemplateMap.SkuCount = sku.count;
                        freightTemplateMap.TemplateId = productInfo.FreightTemplateId;
                        freightTemplateMap.Price = GetSkuPrice(GetProductSku(sku.sku_id));
                        freightTemplateSkuMapList.Add(freightTemplateMap);


                    }



                }


                #endregion

                #region 获取所有用到的规则
                List<FreightTemplateRuleModel> allUseRuleList = new List<FreightTemplateRuleModel>();//此次订单用到的所有配送规则
                WriteLog("freightTemplateIdListCount" + freightTemplateIdList.Count);
                foreach (var templateId in freightTemplateIdList)
                {
                    List<FreightTemplateRule> freightTemplateRuleList = GetFreightTemplateRuleList(templateId);
                    foreach (var sourceRule in freightTemplateRuleList)
                    {
                        FreightTemplateRuleModel rule = new FreightTemplateRuleModel();
                        rule.RuleId = sourceRule.AutoId;
                        rule.TemplateId = sourceRule.TemplateId;
                        rule.InitialProductCount = sourceRule.InitialProductCount;
                        rule.InitialAmount = sourceRule.InitialAmount;
                        rule.AddProductCount = sourceRule.AddProductCount;
                        rule.AddAmount = sourceRule.AddAmount;
                        rule.AreaCodeList = (!string.IsNullOrEmpty(sourceRule.AreaCodes) ? (sourceRule.AreaCodes.Split(',')) : (new string[0])).ToList();
                        allUseRuleList.Add(rule);
                    }

                }
                #endregion

                #region 邮费计算 (循环计算累加各个运费模板费用并加上统一邮费)
                string str = string.Empty;
                foreach (var templateId in freightTemplateIdList)
                {

                    var templateRuleList = allUseRuleList.Where(p => p.TemplateId == templateId);
                    if (templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_dist_code.ToString())).Count() == 0 && templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_city_code.ToString())).Count() == 0 && templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_province_code.ToString())).Count() == 0)
                    {

                        return false;//不能配送到此地区

                    }
                    var temp = GetFreightTemplate(templateId);


                    if (temp.FreightFreeLimitType == 1)
                    {
                        var skuCount = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.SkuCount);
                        if (skuCount >= temp.FreightFreeLimitValue)
                        {
                            str += "购买商品件数满" + (int)temp.FreightFreeLimitValue + "件包邮";
                            continue;
                        }


                    }

                    if (temp.FreightFreeLimitType == 2)
                    {
                        var priceSum = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.Price * p.SkuCount);
                        if (priceSum >= temp.FreightFreeLimitValue)
                        {
                            str += "满" + temp.FreightFreeLimitValue + "金额包邮";
                            continue;
                        }
                    }


                    //计算使用此模板的商品数量
                    int totalCount = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.SkuCount);


                    ///区域代码匹配规则
                    ///如果城市区域匹配，则使用城市区域的规则
                    ///如果城市区域不匹配，则查找城市代码，看是否匹配
                    ///如果城市不匹配，则查找省份代码
                    ///省市区 先匹配区，再匹配市，再匹配省 
                    var distRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_dist_code.ToString()));// 区对应规则
                    var cityRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_city_code.ToString()));// 城市对应规则
                    var provinceRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_province_code.ToString()));//省对应规则
                    if (distRule != null)
                    {
                        var cha = totalCount - distRule.InitialProductCount;
                        if (cha < 0)
                        {
                            cha = 0;
                        }
                        freight += distRule.InitialAmount + Math.Ceiling((decimal)(cha) / distRule.AddProductCount) * distRule.AddAmount;

                    }
                    else if (cityRule != null)
                    {
                        var cha = totalCount - cityRule.InitialProductCount;
                        if (cha < 0)
                        {
                            cha = 0;
                        }
                        freight += cityRule.InitialAmount + Math.Ceiling((decimal)(cha) / cityRule.AddProductCount) * cityRule.AddAmount;


                    }
                    else if (provinceRule != null)
                    {
                        var cha = totalCount - provinceRule.InitialProductCount;
                        if (cha < 0)
                        {
                            cha = 0;
                        }
                        freight += provinceRule.InitialAmount + Math.Ceiling((decimal)(cha) / provinceRule.AddProductCount) * provinceRule.AddAmount;
                    }

                }

                //累加最小的统一运费
                freight += productList.Min(p => p.UnifiedFreight);
                if (!string.IsNullOrEmpty(str)) msg = str;
                return true;
                #endregion

            }
            catch (Exception ex)
            {

                msg = ex.Message;
                return false;
            }




        }


        /// <summary>
        /// 运费计算 按重量
        /// </summary>
        /// <returns></returns>
        public bool CalcFreightWeight(FreightModel freightModel, out decimal freight, out string msg)
        {
            //运费计算规则:
            //情况1 所选商品全部是统一邮费,则从中选择邮费最低的
            //情况2 不同或相同的商品，设置同一运费模板：按该模板设置的规则计算；
            //情况3 所有商品都设置了运费模板而且有多个模板 各个运费模板计算累加
            //情况4 既有运费模板又有统一运费 各个运费模板单独计算累加并加上最小的统一运费
            //参考 http://bbs.youzan.com/forum.php?mod=viewthread&tid=6150

            msg = "很抱歉，该地区暂不支持配送";
            freight = 0;//运费

            try
            {


                ///各个运费模板对应的商品数量
                List<FreightTemplateSkuMapping> freightTemplateSkuMapList = new List<FreightTemplateSkuMapping>();
                #region 获取所有下单商品跟使用到的运费模板
                List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//此订单包含的所有商品
                List<int> freightTemplateIdList = new List<int>();//此订单用到的所有运费模板ID
                foreach (var sku in freightModel.skus)
                {

                    var skuInfo = GetProductSku(sku.sku_id);
                    WXMallProductInfo productInfo = GetProduct(skuInfo.ProductId.ToString());
                    productList.Add(productInfo);
                    if (productInfo.FreightTemplateId > 0)//此商品设置了运费模板
                    {
                        if (!freightTemplateIdList.Contains(productInfo.FreightTemplateId))
                        {
                            freightTemplateIdList.Add(productInfo.FreightTemplateId);
                        }

                        // 各个运费模板对应的商品数量
                        FreightTemplateSkuMapping freightTemplateMap = new FreightTemplateSkuMapping();
                        freightTemplateMap.SkuCount = sku.count;
                        freightTemplateMap.TemplateId = productInfo.FreightTemplateId;
                        freightTemplateMap.Weight = skuInfo.Weight;
                        freightTemplateMap.Price = GetSkuPrice(skuInfo);
                        if (skuInfo.Weight == 0 && productInfo.Weight > 0)
                        {
                            freightTemplateMap.Weight = productInfo.Weight;
                        }
                        freightTemplateSkuMapList.Add(freightTemplateMap);


                    }



                }


                #endregion

                #region 获取所有用到的规则
                List<FreightTemplateRuleModel> allUseRuleList = new List<FreightTemplateRuleModel>();//此次订单用到的所有配送规则
                foreach (var templateId in freightTemplateIdList)
                {
                    List<FreightTemplateRule> freightTemplateRuleList = GetFreightTemplateRuleList(templateId);
                    foreach (var sourceRule in freightTemplateRuleList)
                    {
                        FreightTemplateRuleModel rule = new FreightTemplateRuleModel();
                        rule.RuleId = sourceRule.AutoId;
                        rule.TemplateId = sourceRule.TemplateId;
                        rule.InitialProductCount = sourceRule.InitialProductCount;
                        rule.InitialAmount = sourceRule.InitialAmount;
                        rule.AddProductCount = sourceRule.AddProductCount;
                        rule.AddAmount = sourceRule.AddAmount;
                        rule.AreaCodeList = (!string.IsNullOrEmpty(sourceRule.AreaCodes) ? (sourceRule.AreaCodes.Split(',')) : (new string[0])).ToList();
                        allUseRuleList.Add(rule);
                    }

                }
                #endregion

                #region 邮费计算 (循环计算累加各个运费模板费用并加上统一邮费)
                string str = string.Empty;
                foreach (var templateId in freightTemplateIdList)
                {

                    var templateRuleList = allUseRuleList.Where(p => p.TemplateId == templateId);
                    if (templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_dist_code.ToString())).Count() == 0 && templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_city_code.ToString())).Count() == 0 && templateRuleList.Where(p => p.AreaCodeList.Contains(freightModel.receiver_province_code.ToString())).Count() == 0)
                    {
                        return false;//不能配送到此地区
                    }
                    var temp = GetFreightTemplate(templateId);
                    if (temp.FreightFreeLimitType == 1)
                    {
                        var weight = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.Weight);
                        if (weight >= temp.FreightFreeLimitValue)
                        {
                            str += "购买商品重量满" + temp.FreightFreeLimitValue + "kg包邮";
                            continue;
                        }
                    }

                    if (temp.FreightFreeLimitType == 2)
                    {
                        var priceSum = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.Price * p.SkuCount);
                        if (priceSum >= temp.FreightFreeLimitValue)
                        {
                            str += "满" + temp.FreightFreeLimitValue + "金额包邮";
                            continue;
                        }
                    }
                    //计算使用此模板的商品总重量
                    decimal totalWeight = freightTemplateSkuMapList.Where(p => p.TemplateId == templateId).Sum(p => p.SkuCount * p.Weight);

                    ///区域代码匹配规则
                    ///如果城市区域匹配，则使用城市区域的规则
                    ///如果城市区域不匹配，则查找城市代码，看是否匹配
                    ///如果城市不匹配，则查找省份代码
                    ///省市区 先匹配区，再匹配市，再匹配省 
                    var distRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_dist_code.ToString()));// 区对应规则
                    var cityRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_city_code.ToString()));// 城市对应规则
                    var provinceRule = templateRuleList.SingleOrDefault(p => p.AreaCodeList.Contains(freightModel.receiver_province_code.ToString()));//省对应规则
                    if (distRule != null)
                    {

                        decimal cha = totalWeight - distRule.InitialProductCount;
                        if (cha < 0)
                        {
                            cha = 0;
                        }
                        freight += distRule.InitialAmount + Math.Ceiling((decimal)(cha) / distRule.AddProductCount) * distRule.AddAmount;


                    }
                    else if (cityRule != null)
                    {
                        decimal cha = totalWeight - cityRule.InitialProductCount;
                        if (cha < 0)
                        {
                            cha = 0;
                        }
                        freight += cityRule.InitialAmount + Math.Ceiling((decimal)(cha) / cityRule.AddProductCount) * cityRule.AddAmount;


                    }
                    else if (provinceRule != null)
                    {
                        decimal cha = totalWeight - provinceRule.InitialProductCount;
                        if (cha < 0)
                        {
                            cha = 0;
                        }
                        freight += provinceRule.InitialAmount + Math.Ceiling((decimal)(cha) / provinceRule.AddProductCount) * provinceRule.AddAmount;
                    }

                }

                //累加最小的统一运费
                freight += productList.Min(p => p.UnifiedFreight);
                if (!string.IsNullOrEmpty(str)) msg = str;

                return true;
                #endregion

            }
            catch (Exception ex)
            {

                msg = ex.Message;
                return false;
            }




        }

        #endregion

        #region 缺货登记
        public List<ProductStock> GetProductStockList(int pageIndex, int pageSize, string pId, string pName, out int total, string userId = "", string sort = "")
        {
            StringBuilder sbSql = new StringBuilder(string.Format(" WebsiteOwner='{0}' ", WebsiteOwner));

            if (!string.IsNullOrEmpty(pId))
            {
                sbSql.AppendFormat(" AND ProductId={0} ", pId);
            }
            if (!string.IsNullOrEmpty(pName))
            {
                sbSql.AppendFormat(" AND PName like '%{0}%' ", pName);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            }
            string order = " AutoId DESC ";

            if (!string.IsNullOrEmpty(sort))
            {
                order = sort + " DESC ";
            }

            total = GetCount<ProductStock>(sbSql.ToString());

            return GetLit<ProductStock>(pageSize, pageIndex, sbSql.ToString(), order);

        }
        #endregion

        private void WriteLog(string msg)
        {

            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\log1.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                }
            }
            catch { }

        }

        /// <summary>
        /// 获取单个退款信息
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public WXMallRefund GetRefundInfoByOrderDetailId(int orderDetailId)
        {

            //return Get<WXMallRefund>(string.Format("OrderDetailId={0} And WebsiteOwner='{1}'", orderDetailId, WebsiteOwner));
            return Get<WXMallRefund>(string.Format("OrderDetailId={0} ", orderDetailId));
        }

        /// <summary>
        /// 获取退款状态
        /// 
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public string GetRefundStatus(WXMallOrderDetailsInfo orderDetail)
        {

            var orderInfo = GetOrderInfo(orderDetail.OrderID);

            if (!string.IsNullOrEmpty(orderDetail.RefundStatus))
            {
                return orderDetail.RefundStatus;

            }
            else
            {

                if (orderInfo.Status == "待发货" || orderInfo.Status == "已发货" || orderInfo.Status == "交易成功")
                {

                    return "-2";
                }

            }



            return "";
        }

        /// <summary>
        /// 检查订单是否可以分享礼品给朋友
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public bool IsCanShareGift(WXMallOrderInfo orderInfo)
        {
            if (orderInfo.OrderType == 1 && (string.IsNullOrEmpty(orderInfo.ParentOrderId)))
            {
                var parentOrderDetail = Get<WXMallOrderDetailsInfo>(string.Format(" OrderID='{0}'", orderInfo.OrderID));
                if (GetCount<WXMallOrderInfo>(string.Format(" ParentOrderId='{0}' And OrderID !='{0}' And OrderType=1", orderInfo.OrderID)) < parentOrderDetail.TotalCount)
                {
                    return true;
                }

            }
            return false;

        }


        #region 自提点模块

        /// <summary>
        /// 自提点列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="isDisable">启用禁用状态</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>自提点列表</returns>
        public List<GetAddress> GetAddressList(int pageIndex, int pageSize, string keyWord, out int totalCount, string isDisable = "")
        {

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));

            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And GetAddressName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(isDisable))
            {
                sbWhere.AppendFormat(" And IsDisable ={0}", isDisable);
            }
            totalCount = GetCount<GetAddress>(sbWhere.ToString());

            return GetLit<GetAddress>(pageSize, pageIndex, sbWhere.ToString());


        }

        /// <summary>
        /// 增加自提点
        /// </summary>
        /// <param name="getAddressId"></param>
        /// <param name="getAdressName"></param>
        /// <param name="getAddressLocation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddGetAddress(string getAddressId, string getAdressName, string getAddressLocation, out string msg, string imgUrl = "")
        {
            msg = "ok";
            GetAddress model = GetGetAddress(getAddressId);
            if (model != null)
            {
                msg = "自提点ID已经存在";
                return false;
            }
            if (GetCount<GetAddress>(string.Format(" WebSiteOwner='{0}' And GetAddressName='{1}'", WebsiteOwner, getAdressName)) > 0)
            {
                msg = "自提点已存在";
                return false;
            }


            model = new GetAddress();
            model.GetAddressId = getAddressId;
            model.GetAddressName = getAdressName;
            model.GetAddressLocation = getAddressLocation;
            model.WebSiteOwner = WebsiteOwner;
            model.ImgUrl = imgUrl;
            return Add(model);

        }
        /// <summary>
        /// 更新自提点
        /// </summary>
        /// <param name="getAddressId"></param>
        /// <param name="getAdressName"></param>
        /// <param name="getAdress"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateGetAddress(string getAddressId, string getAdressName, string getAddressLocation, out string msg, string imgUrl)
        {
            msg = "ok";
            GetAddress model = GetGetAddress(getAddressId);
            if (model == null)
            {
                msg = "自提点ID不存在";
                return false;
            }

            model.GetAddressName = getAdressName;
            model.GetAddressLocation = getAddressLocation;
            model.ImgUrl = imgUrl;
            return Update(model);

        }
        /// <summary>
        /// 获取单个自提点信息
        /// </summary>
        /// <param name="getAddressId"></param>
        /// <returns></returns>
        public GetAddress GetGetAddress(string getAddressId)
        {
            return Get<GetAddress>(string.Format(" GetAddressId='{0}'", getAddressId));

        }
        /// <summary>
        /// 删除自提点
        /// </summary>
        /// <param name="getAddressIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteGetAddress(string getAddressIds, out string msg)
        {
            msg = "ok";
            if (GetCount<GetAddress>(string.Format(" WebsiteOwner='{0}' GetAddressId in ({1})", WebsiteOwner, getAddressIds)) != getAddressIds.Split(',').Count())
            {
                msg = "自提点ID错误,请检查";
                return false;
            }
            if (Delete(new GetAddress(), string.Format(" WebsiteOwner='{0}' And GetAddressId in ({1})", WebsiteOwner, getAddressIds)) != getAddressIds.Split(',').Count())
            {
                msg = "删除失败";
                return false;
            }
            return true;

        }

        /// <summary>
        /// 更新自提点启用禁用状态
        /// </summary>
        /// <param name="getAddressIds"></param>
        /// <param name="enableStatus"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateGetAddressEnableStatus(string getAddressIds, int enableStatus, out string msg)
        {
            msg = "false";
            if (Update(new GetAddress(), string.Format(" IsDisable={0}", enableStatus), string.Format(" WebsiteOwner='{0}' And GetAddressId in ({1})", WebsiteOwner, getAddressIds)) > 0)
            {
                msg = "ok";
                return true;
            }
            return false;

        }
        #endregion

        ///// <summary>
        ///// 当前站点是否是分销商城 true 是false 否
        ///// </summary>
        ///// <returns></returns>
        //public bool IsDistributionMall
        //{

        //    get
        //    {
        //        if (GetWebsiteInfoModelFromDataBase().IsDistributionMall == 1)
        //        {
        //            return true;
        //        }
        //        return false;

        //    }


        //}
        #region 拼团
        /// <summary>
        /// 拼团规则列表
        /// </summary>
        /// <returns></returns>
        public List<ProductGroupBuyRule> GetProductGroupBuyRuleList()
        {

            return GetList<ProductGroupBuyRule>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
        }
        /// <summary>
        /// 单个规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public ProductGroupBuyRule GetProductGroupBuyRule(int ruleId)
        {

            return Get<ProductGroupBuyRule>(string.Format("WebsiteOwner='{0}' And RuleId={1}", WebsiteOwner, ruleId));

        }
        /// <summary>
        /// 单个规则根据数量
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ProductGroupBuyRule GetProductGroupBuyRuleByPeopleCount(int peopleCount)
        {

            return Get<ProductGroupBuyRule>(string.Format("WebsiteOwner='{0}' And PeopleCount={1}", WebsiteOwner, peopleCount));

        }
        /// <summary>
        ///添加 
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="headDiscount">团长折扣</param>
        /// <param name="memberDiscount">团员折扣</param>
        /// <param name="peopleCount">拼团人数</param>
        /// <param name="expireDay">过期天数</param>
        /// <param name="opeator">操作人</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public bool AddProductGroupBuyRule(string ruleName, decimal headDiscount, decimal memberDiscount, int peopleCount, int expireDay, string opeator, out string msg)
        {
            msg = "";
            if (GetProductGroupBuyRuleByPeopleCount(peopleCount) != null)
            {
                msg = "拼团人数重复";
                return false;
            }
            if (headDiscount < 0)
            {
                msg = "团长折扣需大于等于0";
                return false;
            }
            if (memberDiscount <= 0)
            {
                msg = "折扣需大于0";
                return false;
            }
            if (peopleCount <= 0)
            {
                msg = "拼团人数需大于0";
                return false;
            }
            if (expireDay <= 0)
            {
                msg = "过期天数需大于0";
                return false;
            }
            ProductGroupBuyRule model = new ProductGroupBuyRule();
            model.RuleName = ruleName;
            model.HeadDiscount = headDiscount;
            model.MemberDiscount = memberDiscount;
            model.ModifyDate = DateTime.Now;
            model.Operator = opeator;
            model.PeopleCount = peopleCount;
            model.RuleId = int.Parse(GetGUID(TransacType.CommAdd));
            model.WebsiteOwner = WebsiteOwner;
            model.ExpireDay = expireDay;
            if (string.IsNullOrEmpty(model.RuleName))
            {
                model.RuleName = peopleCount + "人团";
            }
            if (Add(model))
            {
                msg = "ok";
                return true;
            }
            else
            {
                msg = "操作失败";
                return false;
            }
        }
        /// <summary>
        ///编辑
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <param name="ruleName">规则名称</param>
        /// <param name="headDiscount">团长折扣</param>
        /// <param name="memberDiscount">团员折扣</param>
        /// <param name="peopleCount">拼团人数</param>
        /// <param name="expireDay">过期天数</param>
        /// <param name="opeator">操作人</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public bool UpdateProductGroupBuyRule(int ruleId, string ruleName, decimal headDiscount, decimal memberDiscount, int peopleCount, int expireDay, string opeator, out string msg)
        {
            msg = "";
            var model = GetProductGroupBuyRule(ruleId);
            if (model == null)
            {
                msg = "规则不存在";
                return false;
            }
            if (GetCount<ProductGroupBuyRule>(string.Format(" PeopleCount={0} And RuleId!={1} And WebsiteOwner='{2}'", peopleCount, ruleId, WebsiteOwner)) > 0)
            {
                msg = "拼团人数已经存在";
                return false;
            }
            if (headDiscount < 0)
            {
                msg = "团长折扣需大于等于0";
                return false;
            }
            if (memberDiscount <= 0)
            {
                msg = "折扣需大于0";
                return false;
            }
            if (peopleCount <= 0)
            {
                msg = "拼团人数需大于0";
                return false;
            }
            if (expireDay <= 0)
            {
                msg = "过期天数需大于0";
                return false;
            }
            model.RuleName = ruleName;
            model.HeadDiscount = headDiscount;
            model.MemberDiscount = memberDiscount;
            model.ModifyDate = DateTime.Now;
            model.Operator = opeator;
            model.PeopleCount = peopleCount;
            model.ExpireDay = expireDay;
            if (Update(model))
            {
                msg = "ok";
                return true;
            }
            else
            {
                msg = "操作失败";
                return false;
            }
        }
        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="ruleIds">规则ID列表，多个用逗号分隔</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteProductGroupBuyRule(string ruleIds, out string msg)
        {
            msg = "";
            if (Delete(new ProductGroupBuyRule(), string.Format("RuleId in({0}) And WebsiteOwner='{1}'", ruleIds, WebsiteOwner)) > 0)
            {
                foreach (var ruleId in ruleIds.Split(','))
                {
                    var strWhere = string.Format(" PATINDEX('{0}', GroupBuyRuleIds) >0", ruleId);
                    Update(new WXMallProductInfo(), string.Format(" GroupBuyRuleIds=replace(GroupBuyRuleIds,'{0}','') ", ruleId), strWhere);
                    ClearProductListCacheByWhere(strWhere);
                }
                msg = "ok";
                return true;
            }
            else
            {
                msg = "操作失败";
                return false;
            }
        }


        /// <summary>
        /// 更新商品拼团规则
        /// </summary>
        /// <param name="ruleIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateProductGroupBuyRule(string productId, string ruleIds, out string msg)
        {
            msg = "";
            if (Update(new WXMallProductInfo(), string.Format("GroupBuyRuleIds='{0}'", ruleIds), string.Format(" PID={0} And WebsiteOwner='{1}'", productId, WebsiteOwner)) > 0)
            {
                BLLRedis.ClearProduct(WebsiteOwner, productId);
                msg = "ok";
                return true;
            }
            else
            {
                msg = "操作失败";
                return false;
            }
        }

        /// <summary>
        /// 获取商品拥有的团购规则
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<ProductGroupBuyRule> GetProductGroupBuyRuleList(string productId)
        {
            List<ProductGroupBuyRule> list = new List<ProductGroupBuyRule>();
            WXMallProductInfo productInfo = GetProduct(productId);
            if (!string.IsNullOrEmpty(productInfo.GroupBuyRuleIds))
            {
                foreach (var ruleId in productInfo.GroupBuyRuleIds.Split(','))
                {
                    if (!string.IsNullOrEmpty(ruleId))
                    {
                        ProductGroupBuyRule rule = GetProductGroupBuyRule(int.Parse(ruleId));
                        if (rule != null)
                        {

                            list.Add(rule);
                        }
                    }


                }
            }



            return list;

        }

        #endregion


        /// <summary>
        /// 查询订单详细信息,根据商品ID，商品类型，开始时间，结束时间
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<WXMallOrderDetailsInfo> GetOrderDetailsList(string orderId, string pid, string type, DateTime? start, DateTime? end)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1 ", orderId);
            if (!string.IsNullOrWhiteSpace(orderId)) sbWhere.AppendFormat(" AND OrderID='{0}' ", orderId);
            if (!string.IsNullOrWhiteSpace(pid)) sbWhere.AppendFormat(" AND PID='{0}' ", pid);
            if (!string.IsNullOrWhiteSpace(type)) sbWhere.AppendFormat(" AND ArticleCategoryType='{0}' ", type);
            if (start.HasValue) sbWhere.AppendFormat(" AND StartDate>='{0}' ", start.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (end.HasValue) sbWhere.AppendFormat(" AND EndDate<='{0}' ", end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            return GetList<WXMallOrderDetailsInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <param name="cate_id"></param>
        /// <returns></returns>
        public string GetWXMallCategoryName(string cate_id)
        {
            if (string.IsNullOrWhiteSpace(cate_id)) return "";
            WXMallCategory nCate = GetColByKey<WXMallCategory>("AutoID", cate_id, "AutoID,CategoryName");
            if (nCate == null) return "";
            return nCate.CategoryName;
        }
        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <param name="cate_id"></param>
        /// <returns></returns>
        public string GetArticleCategoryName(string cate_id)
        {
            if (string.IsNullOrWhiteSpace(cate_id)) return "无";
            ArticleCategory nCate = GetByKey<ArticleCategory>("AutoID", cate_id);
            if (nCate == null) return "无";
            return nCate.CategoryName;
        }
        /// <summary>
        /// 查询商品默认Sku
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductSku GetProductSku(string productId)
        {
            return GetByKey<ProductSku>("ProductId", productId);
        }
        /// <summary>
        /// 查询商品默认SkuId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetProductSkuId(string productId)
        {
            ProductSku nsku = GetProductSku(productId);
            if (nsku == null) return 0;
            return nsku.SkuId;
        }

        /// <summary>
        /// 是否是限制购买时间段的商品
        /// </summary>
        /// <param name="productInfo">商品信息</param>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public bool IsLimitProductTime(WXMallProductInfo productInfo, string startDate, string endDate)
        {

            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            if (currentWebsiteInfo.IsEnableLimitProductBuyTime == 1)
            {

                if (!string.IsNullOrEmpty(productInfo.LimitBuyTime))
                {

                    List<DateTime> dateRange = new List<DateTime>();//时间范围
                    DateTime dtStart = DateTime.Parse(startDate);
                    DateTime dtEnd = DateTime.Parse(endDate);//2015-05-24,2016-05-27
                    for (int i = 1; i < (dtEnd - dtStart).TotalDays; i++)
                    {
                        dateRange.Add(dtStart.AddDays(i));//i=1 i<3
                    }
                    dateRange.Add(dtStart);
                    dateRange.Add(dtEnd);
                    foreach (var item in dateRange)
                    {
                        if (productInfo.LimitBuyTime.Contains(item.ToString("yyyy-MM-dd")))
                        {
                            return true;
                        }
                    }


                }
            }

            return false;
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="ids"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetColOrderListInStatus(string status, string ids, string colName, string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" OrderID In ({0}) AND Status In ({1}) AND WebsiteOwner='{2}'", ids, status, websiteOwner);
            return GetColList<WXMallOrderInfo>(int.MaxValue, 1, sbSql.ToString(), colName);
        }
        /// <summary>
        /// 是否启用余额支付
        /// </summary>
        /// <returns></returns>
        public bool IsEnableAccountAmountPay()
        {
            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            if (currentWebsiteInfo.IsEnableAccountAmountPay == 1)
            {
                return true;

            }
            return false;
        }
        /// <summary>
        /// 获取退款订单的数量
        /// </summary>
        /// <returns></returns>
        public int GetRefundOrderCount()
        {

            return GetCount<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And  IsRefund=1", WebsiteOwner));

        }

        /// <summary>
        /// 查询指定日期商品的价格
        /// </summary>
        /// <param name="sku">sku</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public decimal GetSkuPriceByDate(ProductSku sku, string date)
        {
            var productPriceConfigList = GetProductPriceConfigList(sku.ProductId, sku.WebSiteOwner);
            if (productPriceConfigList.Count > 0)
            {
                var skuPriceConfig = productPriceConfigList.Where(p => p.ProductId == sku.ProductId.ToString()).Where(p => p.Date == date).ToList();
                if (skuPriceConfig.Count() > 0)
                {
                    return skuPriceConfig.First().Price;

                }
            }

            return sku.Price;//默认价格
        }
        /// <summary>
        /// 获取商品价格配置
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<ProductPriceConfig> GetProductPriceConfigList(int productId, string websiteOwner)
        {

            return GetList<ProductPriceConfig>(string.Format(" ProductId={0} And WebsiteOwner='{1}'", productId, websiteOwner));


        }
        /// <summary>
        /// 获取商城统计列表
        /// </summary>
        /// <param name="pageSize">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="date">日期</param>
        /// <param name="sort">排序</param>
        /// <param name="totalCount">总数量</param>
        /// <returns></returns>
        public List<WXMallStatistics> GetWXMallStatisticsList(int pageSize, int pageIndex, string sdate, string edate, string sort, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(sdate))
            {
                sbWhere.AppendFormat(" AND Date>='{0}' ", DateTime.Parse(sdate).ToString("yyyy/MM/dd"));
            }
            if (!string.IsNullOrEmpty(edate))
            {
                sbWhere.AppendFormat(" AND Date<='{0}' ", DateTime.Parse(edate).ToString("yyyy/MM/dd"));
            }
            string orderBy = " AutoId DESC ";
            if (!string.IsNullOrEmpty(sort))
            {
                orderBy = sort + " ASC ";
            }
            totalCount = GetCount<WXMallStatistics>(sbWhere.ToString());
            return GetLit<WXMallStatistics>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
        }
        /// <summary>
        /// 导出商城统计
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsData(string sDate, string eDate)
        {
            DataTable dt = new DataTable();

            try
            {
                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat(" SELECT Date as 日期,");
                strSql.AppendFormat(" OrderCount as 成交笔数,");
                strSql.AppendFormat(" OrderProuductTotalCount as 成交件数,");
                strSql.AppendFormat(" OrderTotalAmount as 成交金额,");
                strSql.AppendFormat(" RefundProductTotalCount as 当日退货件数,");
                strSql.AppendFormat(" RefundTotalAmount as 当日退货金额,");
                strSql.AppendFormat(" PV as [商城PV(浏览量)],");
                strSql.AppendFormat(" UV as [商城UV(访客量)],");
                strSql.AppendFormat(" ProductTotalCount as 在线商品数,");
                strSql.AppendFormat(" ConvertRate as 转化率,");
                strSql.AppendFormat(" PerCustomerTransaction as 客单价,");
                strSql.AppendFormat(" ProcuctAveragePrice as 商品平均单价,");
                strSql.AppendFormat(" OrderTotalAmountMonth as 月累计,");
                strSql.AppendFormat(" TotalSales as 销售总额,");
                strSql.AppendFormat(" InvoiceAmount as 开票金额,");
                strSql.AppendFormat(" MerchantSettlemenTotalAmount as 商户结算总额");
                strSql.Append(" FROM ZCJ_WXMallStatistics ");
                strSql.AppendFormat(" Where WebsiteOwner='{0}' ", WebsiteOwner);
                if (!string.IsNullOrEmpty(sDate))
                {
                    strSql.AppendFormat(" AND Date>='{0}' ", DateTime.Parse(sDate).ToString("yyyy/MM/dd"));
                }
                if (!string.IsNullOrEmpty(eDate))
                {
                    strSql.AppendFormat(" AND Date<='{0}' ", DateTime.Parse(eDate).ToString("yyyy/MM/dd"));
                }
                strSql.Append(" order by AutoId DESC ");

                dt = Query(strSql.ToString()).Tables[0];


            }
            catch (Exception)
            {

                throw;
            }

            return dt;
        }



        /// <summary>
        /// 商城统计
        /// </summary>
        /// <returns></returns>
        public void Statistics()
        {

            DateTime dateTimeYesday = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            DateTime dateTimeToday = dateTimeYesday.AddDays(1).AddMilliseconds(-1);
            foreach (var website in GetList<WebsiteInfo>())
            {
                try
                {
                    if (GetCount<WXMallStatistics>(string.Format(" WebsiteOwner='{0}' And Date='{1}'", website.WebsiteOwner, dateTimeYesday.ToString("yyyy/MM/dd"))) == 0)
                    {

                        WXMallStatistics model = new WXMallStatistics();
                        model.Date = dateTimeYesday.ToString("yyyy/MM/dd");//统计昨天

                        #region 成交笔数
                        string sqlOrderCount = string.Format("Select count(*) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1  And OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') And IsNull(IsMain,0)=0", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderCount);
                        if (orderCount != null)
                        {
                            model.OrderCount = int.Parse(orderCount.ToString());

                        }
                        #endregion

                        #region 成交件数
                        string sqlOrderProuductTotalCount = string.Format(" Select Sum(TotalCount) from ZCJ_WXMallOrderDetailsInfo where OrderID in(Select OrderID from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') And IsNull(IsMain,0)=0)", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderProuductTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderProuductTotalCount);
                        if (orderProuductTotalCount != null)
                        {
                            model.OrderProuductTotalCount = int.Parse(orderProuductTotalCount.ToString());

                        }
                        #endregion

                        #region 成交金额
                        string sqlOrderTotalAmount = string.Format("Select sum(TotalAmount) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') And IsNull(IsMain,0)=0", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderTotalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderTotalAmount);
                        if (orderTotalAmount != null)
                        {
                            model.OrderTotalAmount = decimal.Parse(orderTotalAmount.ToString());

                        }
                        #endregion

                        #region 当日退货件数
                        string sqlRefundProductTotalCount = string.Format("    select SUM(TotalCount) from ZCJ_WXMallOrderDetailsInfo  where AutoID in( select OrderDetailId from ZCJ_WXMallRefund where WebsiteOwner='{0}'  And ( InsertDate Between '{1}' And '{2}'))", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var refundProductTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlRefundProductTotalCount);
                        if (refundProductTotalCount != null)
                        {
                            model.RefundProductTotalCount = int.Parse(refundProductTotalCount.ToString());

                        }
                        #endregion

                        #region 当日退货金额
                        string sqlRefundTotalAmount = string.Format("Select sum(RefundAmount) from ZCJ_WXMallRefund where WebsiteOwner='{0}'  And ( InsertDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var refundTotalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlRefundTotalAmount);
                        if (refundTotalAmount != null)
                        {
                            model.RefundTotalAmount = decimal.Parse(refundTotalAmount.ToString());

                        }
                        #endregion

                        #region Pv

                        string sqlPv = string.Format("  Select count(*) from ZCJ_MonitorEventDetailsInfo where ModuleType in('product') And WebsiteOwner='{0}'  and ( EventDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var pv = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlPv);
                        if (pv != null)
                        {
                            model.PV = int.Parse(pv.ToString());

                        }
                        #endregion

                        #region UV
                        string sqlUV = string.Format("  Select count(distinct(EventUserID)) from ZCJ_MonitorEventDetailsInfo where ModuleType in('product') And WebsiteOwner='{0}'  and ( EventDate Between '{1}' And '{2}') ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var uv = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlUV);
                        if (uv != null)
                        {
                            model.UV = int.Parse(uv.ToString());

                        }
                        #endregion

                        #region 在线商品数
                        string sqlProductTotalCount = string.Format("Select count(*) from ZCJ_WXMallProductInfo Where WebsiteOwner='{0}' And IsOnSale=1", website.WebsiteOwner);
                        var productTotalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlProductTotalCount);
                        if (productTotalCount != null)
                        {
                            model.ProductTotalCount = int.Parse(productTotalCount.ToString());

                        }
                        #endregion


                        #region 转化率
                        model.ConvertRate = "0%";
                        if (model.UV > 0)
                        {
                            var dou = (((double)model.OrderCount) / ((double)model.UV)) * 100;
                            model.ConvertRate = string.Format("{0}%", Math.Round(dou, 2));
                        }
                        #endregion

                        #region 客单价
                        model.PerCustomerTransaction = 0;
                        if (model.OrderCount > 0)
                        {
                            model.PerCustomerTransaction = model.OrderTotalAmount / model.OrderCount;
                        }
                        #endregion

                        #region 商品平均单价
                        string sqlProcuctAveragePrice = string.Format("SELECT AVG(Price) from ZCJ_ProductSku where WebsiteOwner='{0}'", website.WebsiteOwner);
                        var procuctAveragePrice = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlProcuctAveragePrice);
                        if (procuctAveragePrice != null)
                        {
                            model.ProcuctAveragePrice = decimal.Parse(procuctAveragePrice.ToString());

                        }
                        #endregion

                        #region 月累积销售额
                        string sqlOrderTotalAmountMonth = string.Format("Select sum(TotalAmount) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And  year([InsertDate]) =year('{1}') AND  month(InsertDate) = month('{1}')  And InsertDate<'{2}' And IsNull(IsMain,0)=0 ", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderTotalAmountMonth = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlOrderTotalAmountMonth);
                        if (orderTotalAmountMonth != null)
                        {
                            model.OrderTotalAmountMonth = decimal.Parse(orderTotalAmountMonth.ToString());

                        }
                        #endregion

                        #region 销售总额
                        string sqlTotalSales = string.Format("Select SUM(Product_Fee+Transport_Fee) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1  And OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') And IsNull(IsMain,0)=0", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderTotalSales = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalSales);
                        if (orderTotalSales != null)
                        {
                            model.TotalSales = decimal.Parse(orderTotalSales.ToString());
                        }
                        #endregion

                        #region 开票金额
                        string sqlInvoiceAmount = string.Format(" Select Sum(TotalCount*OrderPrice)-Sum(TotalCount*BasePrice) from ZCJ_WXMallOrderDetailsInfo where OrderID in(Select OrderID from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') And IsNull(IsMain,0)=0)", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderInvoiceAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlInvoiceAmount);
                        if (orderInvoiceAmount != null)
                        {
                            model.InvoiceAmount = decimal.Parse(orderInvoiceAmount.ToString());
                        }
                        #endregion


                        #region 商户结算总额
                        //基础家*数量
                        string sqlMerchantAmount = string.Format(" Select Sum(TotalCount*BasePrice) from ZCJ_WXMallOrderDetailsInfo where OrderID in(Select OrderID from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' And PaymentStatus=1 And  OrderType In(0,1,2) And ( InsertDate Between '{1}' And '{2}') And IsNull(IsMain,0)=0)", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderMerchantAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlMerchantAmount);
                        string sqlTransportFee = string.Format(" Select sum(Transport_Fee) from ZCJ_WXMallOrderInfo where WebsiteOwner='{0}' AND PaymentStatus=1 AND OrderType in (0,1,2) AND (InsertDate Between '{1}' AND '{2}') And IsNull(IsMain,0)=0", website.WebsiteOwner, dateTimeYesday, dateTimeToday);
                        var orderTransportFee = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTransportFee);
                        if (orderMerchantAmount != null && sqlTransportFee != null)
                        {
                            model.MerchantSettlemenTotalAmount = decimal.Parse(orderMerchantAmount.ToString()) + decimal.Parse(orderTransportFee.ToString());
                        }
                        #endregion

                        model.WebsiteOwner = website.WebsiteOwner;

                        model.InsertDate = DateTime.Now;

                        Add(model);
                    }
                }
                catch (Exception ex)
                {
                    continue;

                }

            }


        }

        /// <summary>
        /// 获取订单统计详细
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="taskId"></param>
        /// <param name="type"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WXMallStatisticsOrderDetail> GetWXMallStatisticsOrderDetail(int pageSize, int pageIndex, string taskId, string type, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(taskId))
            {
                sbWhere.AppendFormat(" AND TaskId='{0}' ", taskId);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND Type='{0}' ", type);
            }
            totalCount = GetCount<WXMallStatisticsOrderDetail>(sbWhere.ToString());
            return GetLit<WXMallStatisticsOrderDetail>(pageSize, pageIndex, sbWhere.ToString());
        }

        /// <summary>
        /// 更新销量
        /// </summary>
        /// <param name="orderInfo"></param>
        public void UpdateProductSaleCount(WXMallOrderInfo orderInfo)
        {
            List<string> productIds = new List<string>();
            foreach (var orderDetail in GetOrderDetailsList(orderInfo.OrderID).DistinctBy(p => p.PID))
            {
                int saleCount = GetProductSaleCount(int.Parse(orderDetail.PID));
                Update(new WXMallProductInfo(), string.Format("SaleCount={0}", saleCount), string.Format(" PID='{0}'", orderDetail.PID));
                productIds.Add(orderDetail.PID);
            }
            if (productIds.Count > 0)
            {
                BLLRedis.ClearProductByIds(orderInfo.WebsiteOwner, productIds, false);
                BLLRedis.ClearProductList(orderInfo.WebsiteOwner);
            }
        }
        /// <summary>
        /// 获取考试状态
        /// 0 尚未到考试时间
        /// 1 已经到考试时间,可以正常考试
        /// 2 已经考过了
        /// 3 缺考
        /// </summary>
        /// <returns></returns>
        public int GetExamStatus(WXMallOrderInfo orderInfo, Questionnaire examInfo)
        {

            QuestionnaireRecord record = Get<QuestionnaireRecord>(string.Format("QuestionnaireID={0} And UserId='{1}'", examInfo.QuestionnaireID, orderInfo.OrderUserID));
            if (record != null)//考过了
            {
                return 2;
            }
            //未考
            DateTime dtNow = DateTime.Now;
            if (dtNow < orderInfo.InsertDate.AddDays(15))//未到考试时间
            {
                return 0;

            }
            if (dtNow > orderInfo.InsertDate.AddDays(90))//缺考
            {
                return 3;

            }
            return 1;//考试期间


        }


        /// <summary>
        /// 获取考试信息
        /// </summary>
        /// <returns></returns>
        public ExamInfo GetExamInfo(WXMallOrderInfo orderInfo)
        {


            try
            {


                /// 0 尚未到考试时间
                /// 1 已经到考试时间,可以正常考试
                /// 2 已经考过了
                /// 3 缺考
                /// 4 未阅卷
                /// 5 已经阅卷
                Questionnaire examInfo = Get<Questionnaire>(string.Format("QuestionnaireID='{0}'", GetOrderDetailsList(orderInfo.OrderID)[0].ExQuestionnaireID));
                ExamInfo model = new ExamInfo();
                model.user_id = orderInfo.OrderUserID;
                model.exam_id = examInfo.QuestionnaireID;
                model.status = GetExamStatus(orderInfo, examInfo);
                if (model.status == 2)//考过了
                {
                    //检查有没有阅过卷
                    QuestionnaireRecord record = Get<QuestionnaireRecord>(string.Format("QuestionnaireID={0} And UserId='{1}'", examInfo.QuestionnaireID, orderInfo.OrderUserID));
                    if (string.IsNullOrEmpty(record.Result))
                    {
                        model.status = 4;

                    }
                    else
                    {
                        model.status = 5;
                    }

                }
                switch (model.status)
                {
                    case 0:
                        model.status_text = "尚未到考试时间";
                        break;
                    case 1:
                        model.status_text = "考试期间,未考试";
                        break;
                    case 2:
                        model.status_text = "已经提交考卷";
                        break;
                    case 3:
                        model.status_text = "缺考";
                        break;
                    case 4:
                        model.status_text = "未阅卷";
                        break;
                    case 5:
                        model.status_text = "已经阅卷";
                        break;
                    default:
                        break;
                }
                return model;

            }
            catch (Exception)
            {

                return null;
            }

        }
        /// <summary>
        /// 获取预购商品信息
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public ProductAppointmentInfo GetProductAppointmentInfo(WXMallProductInfo productInfo)
        {

            ProductAppointmentInfo model = new ProductAppointmentInfo();
            if (productInfo.IsAppointment == 1)
            {
                DateTime dtNow = DateTime.Now;
                if (dtNow < Convert.ToDateTime(productInfo.AppointmentStartTime))
                {
                    model.status = 0;//未开始
                }

                else if (dtNow > Convert.ToDateTime(productInfo.AppointmentEndTime))
                {

                    model.status = 2;//已经结束

                }
                else
                {
                    model.status = 1;//预购期间
                }
                model.appointment_start_time = productInfo.AppointmentStartTime.Replace("-", "/");
                model.appointment_end_time = productInfo.AppointmentEndTime.Replace("-", "/");
                model.appointment_delivery_time = productInfo.AppointmentDeliveryTime.Replace("-", "/");

                return model;
            }
            else
            {
                return null;
            }




        }

        /// <summary>
        /// 计算储值卡可以抵扣的金额
        /// </summary>
        /// <returns></returns>
        public decimal CalcDiscountAmountStoreValue(decimal orderAmount, string userId, string myStoreValueCardId)
        {
            BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();
            decimal disAmount = 0;
            StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And UserId='{1}'", myStoreValueCardId, userId));
            var storedUseRecordList = bllStoreValue.GetUseRecordList(myStoredValueCardRecord.AutoId, userId);//使用金额记录
            decimal useSum = storedUseRecordList.Sum(p => p.UseAmount);//已经使用的金额
            myStoredValueCardRecord.Amount -= useSum;
            if (myStoredValueCardRecord.Amount < 0)
            {
                myStoredValueCardRecord.Amount = 0;
            }
            if (myStoredValueCardRecord.Amount <= orderAmount)//可用金额小于订单金额,直接全部抵扣
            {
                disAmount = myStoredValueCardRecord.Amount;
            }
            else
            {
                disAmount = orderAmount;//可用金额大于订单金额,只抵扣订单金额
            }
            return disAmount;


        }

        /// <summary>
        /// 获取储值卡可用余额
        /// </summary>
        /// <param name="myStoreValueCardId"></param>
        /// <returns></returns>
        public decimal GetStoreValueCardCanUseAmount(string myStoreValueCardId, string userId)
        {
            BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();
            StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And UserId='{1}'", myStoreValueCardId, userId));
            var storedUseRecordList = bllStoreValue.GetUseRecordList(myStoredValueCardRecord.AutoId, userId);//使用金额记录
            decimal useSum = storedUseRecordList.Sum(p => p.UseAmount);//已经使用的金额
            myStoredValueCardRecord.Amount -= useSum;
            if (myStoredValueCardRecord.Amount < 0)
            {
                myStoredValueCardRecord.Amount = 0;
            }
            return myStoredValueCardRecord.Amount;

        }
        /// <summary>
        /// 获取供应商
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public UserInfo GetSuppLierByUserId(string userId, string websiteOwner)
        {

            UserInfo userInfo = new UserInfo();
            if (!string.IsNullOrEmpty(userId))
            {
                userInfo = bllUser.GetUserInfo(userId, websiteOwner);
                if (userInfo == null)
                {
                    userInfo = new UserInfo();
                }
            }
            if (string.IsNullOrEmpty(userInfo.Company))
            {
                userInfo.Company = "";
            }
            return userInfo;

        }
        /// <summary>
        /// 根据供应商Id查询供应商
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public UserInfo GetSuppLierByAutoId(int autoId)
        {

            UserInfo userInfo = new UserInfo();
            if (autoId > 0)
            {
                userInfo = bllUser.GetUserInfoByAutoID(autoId);
                if (userInfo == null)
                {
                    userInfo = new UserInfo();
                }
            }
            if (string.IsNullOrEmpty(userInfo.Company))
            {
                userInfo.Company = "";
            }
            return userInfo;

        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <returns></returns>
        public bool Refund(int orderDetailId, out string msg)
        {

            msg = "";
            WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase();
            Open.HongWareSDK.MemberInfo hongWareMemberInfo = null;
            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
            Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
            BllScore bllScore = new BllScore();
            BllPay bllPay = new BllPay();
            BLLWeixin bllWeixin = new BLLWeixin();
            BLLDistribution bllDistribution = new BLLDistribution();
            BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
            string aliBatchNo = string.Empty;
            UserInfo orderUserInfo = new UserInfo();//下单用户信息
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//主订单
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                WXMallRefund model = GetRefundInfoByOrderDetailId(orderDetailId);
                if (model.Status == 6)
                {

                    msg = "已经退过款了";
                    return false;
                }
                model.Status = 6;//商家退款
                WXMallOrderDetailsInfo orderDetail = GetOrderDetail(model.OrderDetailId);
                orderDetail.RefundStatus = "6";
                orderDetail.IsComplete = 0;
                if (!Update(orderDetail, tran))
                {
                    tran.Rollback();
                    msg = "操作失败";
                    return false;

                }
                if (Update(model, tran))
                {
                    msg = "ok";
                    //微信退款业务逻辑
                    ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.GetPayConfig();
                    orderInfo = GetOrderInfo(model.OrderId);
                    orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                    string weixinRefundId = "";
                    if (!string.IsNullOrEmpty(orderInfo.ParentOrderId) && orderInfo.OrderType == 1)//礼品订单退款
                    {
                        orderInfo = GetOrderInfo(orderInfo.ParentOrderId);

                    }
                    #region 退款
                    if (model.RefundAmount > 0)//需要退款
                    {
                        string orderIdRe = orderInfo.OrderID;
                        decimal orderTotalAmount = orderInfo.TotalAmount;
                        if (orderInfo.OrderType == 0 && (!string.IsNullOrEmpty(orderInfo.ParentOrderId)))
                        {
                            var parentOrderInfo = GetOrderInfo(orderInfo.ParentOrderId);
                            orderIdRe = parentOrderInfo.OrderID;//拆单用主订单
                            orderTotalAmount = parentOrderInfo.TotalAmount;
                        }
                        bool isSuccess = false;

                        #region 微信退款
                        if (orderInfo.PaymentType == 2)
                        {
                            //微信支付的退款
                            isSuccess = bllPay.WeixinRefund(orderIdRe, model.OrderDetailId.ToString(), orderTotalAmount, model.RefundAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId);

                            if ((!string.IsNullOrEmpty(msg)) && (msg.Contains("请使用可用余额退款")))
                            {
                                isSuccess = bllPay.WeixinRefundYuEr(orderIdRe, model.OrderDetailId.ToString(), orderTotalAmount, model.RefundAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId);
                            }
                        }
                        #endregion

                        #region 支付宝退款
                        else if (orderInfo.PaymentType == 1)
                        {
                            string payTranNo = orderInfo.PayTranNo;
                            if (orderInfo.OrderType == 0 && (!string.IsNullOrEmpty(orderInfo.ParentOrderId)))
                            {
                                var parentOrderInfo = GetOrderInfo(orderInfo.ParentOrderId);
                                payTranNo = parentOrderInfo.PayTranNo;
                            }
                            string batchNo = DateTime.Now.ToString("yyyyMMdd") + ((int)(GetTimeStamp(DateTime.Now) / 1000)).ToString();
                            string notifyUrl = string.Format("http://{0}/Alipay/NotifyRefund.aspx", System.Web.HttpContext.Current.Request.Url.Host);
                            string remark = string.Format("订单号{0}", orderInfo.OrderID);
                            //支付宝支付的退款
                            isSuccess = bllPay.AlipayRefund(payTranNo, batchNo, model.RefundAmount, notifyUrl, out msg, remark);
                            if (isSuccess)
                            {
                                aliBatchNo = batchNo;
                            }

                        }
                        #endregion

                        #region 京东支付
                        else if (orderInfo.PaymentType == 3)
                        {

                            isSuccess = bllPay.JDPayRefund(orderIdRe, model.OrderDetailId.ToString(), model.RefundAmount, "", "", out msg);

                        }
                        #endregion

                        if (!isSuccess)
                        {
                            tran.Rollback();
                            msg = msg + "退款失败";
                            return false;

                        }
                    }
                    #endregion

                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
                    }
                    #region 退积分
                    if (model.RefundScore > 0)//需要退还积分
                    {
                        orderUserInfo.TotalScore += (double)model.RefundScore;
                        if (bllUser.Update(orderUserInfo, string.Format(" TotalScore={0}", orderUserInfo.TotalScore), string.Format(" AutoID={0}", orderUserInfo.AutoID)) > 0)
                        {
                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.Score = (double)model.RefundScore;
                            scoreRecord.TotalScore = orderUserInfo.TotalScore;
                            scoreRecord.ScoreType = "OrderRefund";
                            scoreRecord.UserID = orderUserInfo.UserID;
                            scoreRecord.AddNote = "微商城-退款返还积分";
                            scoreRecord.RelationID = orderInfo.OrderID;
                            scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                            Add(scoreRecord);
                        }
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                        {
                            yikeClient.BonusUpdate(orderUserInfo.Ex2, (int)model.RefundScore, string.Format("订单{0}退款退还{1}积分", orderInfo.OrderID, model.RefundScore));
                        }

                        #region 宏巍加积分

                        if (websiteInfo.IsUnionHongware == 1)
                        {

                            if (hongWareMemberInfo.member != null)
                            {

                                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, (float)model.RefundScore))
                                {
                                    tran.Rollback();

                                    msg = "更新宏巍积分失败";
                                    return false;


                                }


                            }
                            else
                            {
                                tran.Rollback();
                                msg = "更新宏巍积分失败";
                                return false;
                            }


                        }
                        #endregion


                    }
                    #endregion

                    #region 退余额
                    if (model.RefundAccountAmount > 0)//需要退还余额
                    {
                        orderUserInfo.AccountAmount += model.RefundAccountAmount;
                        if (bllUser.Update(orderUserInfo, string.Format(" AccountAmount={0}", orderUserInfo.AccountAmount), string.Format(" AutoID={0}", orderUserInfo.AutoID)) > 0)
                        {
                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.Score = (double)model.RefundAccountAmount;
                            scoreRecord.TotalScore = (double)orderUserInfo.AccountAmount;
                            scoreRecord.UserID = orderUserInfo.UserID;
                            scoreRecord.AddNote = "微商城-退款余额返还";
                            scoreRecord.RelationID = orderInfo.OrderID;
                            scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                            scoreRecord.ScoreType = "AccountAmount";
                            Add(scoreRecord);
                        }

                        #region 宏巍加余额

                        if (websiteInfo.IsUnionHongware == 1)
                        {
                            if (hongWareMemberInfo.member != null)
                            {
                                if (!hongWareClient.UpdateMemberBlance(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, (float)model.RefundAccountAmount))
                                {
                                    tran.Rollback();
                                    msg = "更新宏巍余额失败";
                                    return false;

                                }


                            }
                            else
                            {
                                tran.Rollback();
                                msg = "更新宏巍余额失败";
                                return false;
                            }


                        }
                        #endregion


                    }
                    #endregion


                    //插入维权记录
                    WXMallRefundLog log = new WXMallRefundLog();
                    log.OrderDetailId = model.OrderDetailId;
                    log.Role = "商家";
                    log.Title = "确认收货并退款";
                    log.LogContent = string.Format("确认收货并退款");
                    log.InsertDate = DateTime.Now;
                    log.WebSiteOwner = orderInfo.WebsiteOwner;
                    Add(log);
                    //插入维权记录
                    tran.Commit();

                    if (!string.IsNullOrWhiteSpace(aliBatchNo))
                    {
                        Update(model, string.Format("OutRefundId='{0}'", aliBatchNo), string.Format("RefundId='{0}'", model.RefundId));
                    }

                    var orderDetailList = GetOrderDetailsList(model.OrderId);


                    #region 交易成功再退款扣除积分
                    if (orderInfo.Status == "交易成功")//交易成功以后退款
                    {
                        try
                        {

                            ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                            if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
                            {
                                int score = (int)(model.RefundAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                                {
                                    yikeClient.BonusUpdate(orderUserInfo.Ex2, -score, string.Format("退款扣除{0}积分", score));
                                }
                            }


                        }
                        catch (Exception)
                        {


                        }

                    }
                    #endregion

                    var totalCount = orderDetailList.Count(p => p.RefundStatus == "0")
                        + orderDetailList.Count(p => p.RefundStatus == "1")
                        + orderDetailList.Count(p => p.RefundStatus == "2")
                        + orderDetailList.Count(p => p.RefundStatus == "3")
                        + orderDetailList.Count(p => p.RefundStatus == "4")
                        + orderDetailList.Count(p => p.RefundStatus == "5");//退款中的数量
                    if (totalCount == 0)
                    {

                        orderInfo.IsRefund = 0;//此订单再无退款申请
                        Update(orderInfo);
                    }

                    #region 所有商品都退款 订单变成已取消
                    if (orderDetailList.Count(p => p.RefundStatus == "6") == orderDetailList.Count)
                    {

                        //修改订单为已经取消
                        orderInfo.Status = "已取消";
                        orderInfo.IsRefund = 0;
                        if (!Update(orderInfo))
                        {


                            msg = "修改订单状态失败";
                            return false;
                        }
                        model.WeiXinRefundId = weixinRefundId;
                        Update(model);


                        #region 驿氪订单状态同步
                        try
                        {


                            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                            {

                                //if (!string.IsNullOrEmpty(orderUserInfo.Phone))
                                //{


                                yikeClient.ChangeStatus(orderInfo.OrderID, orderInfo.Status);//更新yike订单状态
                                yikeClient.UpdateRefundStatus(model);//更新yike 退款状态
                                //}


                            }

                        }
                        catch (Exception)
                        {


                        }
                        #endregion


                        #region 代付返还
                        if (!string.IsNullOrEmpty(orderInfo.OtherUserId))
                        {
                            if (orderInfo.OtherUseAmount > 0)
                            {
                                UserInfo userInfo = bllUser.GetUserInfo(orderInfo.OtherUserId, orderInfo.WebsiteOwner);
                                if (Update(
                                    userInfo,
                                    string.Format(" AccountAmount+={0}", orderInfo.OtherUseAmount),
                                    string.Format(" AutoID={0}", userInfo.AutoID)) > 0
                                    )
                                {


                                }
                                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                                scoreRecord.AddTime = DateTime.Now;
                                scoreRecord.Score = Convert.ToDouble(orderInfo.OtherUseAmount);
                                scoreRecord.TotalScore = (double)userInfo.AccountAmount;
                                scoreRecord.UserID = userInfo.UserID;
                                scoreRecord.RelationID = orderInfo.OrderID;
                                scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                                scoreRecord.AddNote = "微商城-订单取消返还余额";
                                scoreRecord.ScoreType = "AccountAmount";
                                Add(scoreRecord);
                            }
                            if (Convert.ToInt32(orderInfo.OtherMyCouponCardId) > 0)
                            {
                                Delete(new StoredValueCardUseRecord(), string.Format("OrderId='{0}'", orderInfo.OrderID));

                            }




                        }
                        #endregion



                    }
                    #endregion


                    //try
                    //{
                    //    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                    //    {
                    //        var result = yikeClient.UpdateRefundStatus(model);
                    //    }
                    //}
                    //catch (Exception)
                    //{


                    //}


                    //}
                    List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();
                    detailList.Add(orderDetail);
                    ReturnProductSku(detailList);

                    #region 更新销量
                    UpdateProductSaleCount(orderInfo);
                    #endregion

                    bllDistribution.FlashChannelData(bllDistribution.WebsiteOwner);

                    #region 通知
                    bllWeixin.SendTemplateMessageNotifyComm(orderUserInfo, "退款成功", string.Format("您的订单:{0}已成功退款\\n退款金额：{1}元\\n退还积分:{2}\\n退还余额:{3}", orderInfo.OrderID, model.RefundAmount, model.RefundScore, model.RefundAccountAmount));
                    #region 给商城分销上级通知
                    try
                    {

                        //给上级通知
                        if (bllMenuPermission.CheckUserAndPmsKey(orderInfo.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
                        {


                            UserInfo upUserLevel1 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 1);//上一级用户
                            //UserInfo upUserLevel2 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 2);//上二级用户
                            //UserInfo upUserLevel3 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 3);//上三级用户                           
                            if (upUserLevel1 != null)
                            {
                                if (!string.IsNullOrEmpty(upUserLevel1.WXOpenId))
                                {
                                    string disName = bllUser.GetUserDispalyName(orderUserInfo);
                                    if (string.IsNullOrEmpty(disName))
                                    {
                                        disName = "";
                                    }
                                    bllWeixin.SendTemplateMessageNotifyComm(upUserLevel1, "佣金取消通知", string.Format("您的代言人{0}的订单:{1}已退款,佣金取消", disName, orderInfo.OrderID));

                                }
                            }

                            #region 所有已经退款,取消
                            if (orderDetailList.Count(p => p.RefundStatus == "6") == orderDetailList.Count)
                            {
                                #region 预估佣金取消
                                List<ProjectCommissionEstimate> list = GetList<ProjectCommissionEstimate>(string.Format("ProjectId='{0}'", orderInfo.OrderID));
                                foreach (var item in list)
                                {


                                    if (item.CommissionLevel == "0")
                                    {
                                        Update(new UserInfo(), string.Format(" HistoryDistributionOnLineTotalAmountEstimate-={0}", item.Amount), string.Format(" UserId='{0}'", item.UserId));

                                    }
                                    //    else if (item.CommissionLevel == "1")
                                    //{
                                    //    bllMall.Update(new UserInfo(), string.Format(" HistoryDistributionOnLineTotalAmountEstimate-={0},DistributionSaleAmountLevel1-={1}", item.Amount,item.ProjectAmount), string.Format(" UserId='{0}'", item.UserId));

                                //}
                                    else//渠道
                                    {
                                        Update(new UserInfo(), string.Format(" HistoryDistributionOnLineTotalAmountEstimate-={0},DistributionSaleAmountLevel1-={1}", item.Amount, item.ProjectAmount), string.Format(" UserId='{0}'", item.UserId));

                                    }


                                }
                                #endregion

                                #region 累计销售额取消
                                orderUserInfo.DistributionSaleAmountLevel0 -= orderInfo.TotalAmount;
                                if (orderUserInfo.DistributionSaleAmountLevel0 <= 0)
                                {
                                    orderUserInfo.DistributionSaleAmountLevel0 = 0;
                                }
                                Update(new UserInfo(), string.Format(" DistributionSaleAmountLevel0={0}", orderUserInfo.DistributionSaleAmountLevel0), string.Format(" UserId='{0}'", orderInfo.OrderUserID));
                                if (upUserLevel1 != null)
                                {

                                    upUserLevel1.DistributionSaleAmountLevel1 -= orderInfo.TotalAmount;
                                    if (upUserLevel1.DistributionSaleAmountLevel1 <= 0)
                                    {
                                        upUserLevel1.DistributionSaleAmountLevel1 = 0;
                                    }
                                    Update(new UserInfo(), string.Format(" DistributionSaleAmountLevel1={0}", upUserLevel1.DistributionSaleAmountLevel1), string.Format(" UserId='{0}'", upUserLevel1.UserID));


                                }
                                #endregion

                                #region 供应商渠道销售额取消
                                if (!string.IsNullOrEmpty(orderInfo.SupplierUserId))
                                {

                                    var suppLierInfo = bllUser.GetUserInfo(orderInfo.SupplierUserId, orderInfo.WebsiteOwner);
                                    if (suppLierInfo != null)
                                    {
                                        var channelUserInfo = bllUser.GetUserInfo(suppLierInfo.ParentChannel, orderInfo.WebsiteOwner);
                                        if (channelUserInfo != null)
                                        {

                                            Update(channelUserInfo, string.Format(" DistributionSaleAmountAll-={0}", orderInfo.TotalAmount), string.Format(" AutoId={0}", channelUserInfo.AutoID));
                                        }

                                    }

                                }
                                #endregion

                            }
                            else
                            {
                                bllCommRelation.ToLog("未配置分销" + orderInfo.WebsiteOwner);
                            }
                            //给上级通知 
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        bllCommRelation.ToLog("分销退款异常" + ex.ToString());

                    }

                    #endregion
                    #endregion
                    bllDistribution.CancelUpdateLevel(orderUserInfo);

                }
                else
                {
                    tran.Rollback();
                    msg = "操作失败";
                    return false;
                }


            }
            catch (Exception ex)
            {

                tran.Rollback();
                msg = ex.ToString();
                return false;
            }



            return true;
        }


        /// <summary>
        /// 获取商品库存阈值
        /// </summary>
        /// <returns></returns>
        public int GetProductStockThresholdCount(int value, string userId)
        {
            int count = 0;

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            sbSql.AppendFormat(" AND IsDelete=0 ");
            sbSql.AppendFormat(" AND IsOnSale=1 ");
            sbSql.AppendFormat(" And (ArticleCategoryType Is Null Or ArticleCategoryType='Mall')");

            if (!string.IsNullOrEmpty(userId))
            {
                sbSql.AppendFormat(" AND SupplierUserId='{0}' ", userId);
            }
            List<WXMallProductInfo> productList = GetList<WXMallProductInfo>(sbSql.ToString());

            foreach (WXMallProductInfo item in productList)
            {
                int stock = GetProductTotalStock(int.Parse(item.PID));

                if (stock <= value) count = count + 1;
            }

            return count;
        }
        /// <summary>
        /// 筛选状态个数
        /// </summary>
        /// <returns></returns>
        public int GetOrderStatusCount(string websiteOwner, string orderStatus, int orderType, string userId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            sbSql.AppendFormat(" AND OrderType={0} ", orderType);
            sbSql.AppendFormat(" AND IsNull(IsMain,0)=0 ");
            if (!string.IsNullOrEmpty(userId))
            {
                sbSql.AppendFormat(" AND SupplierUserId='{0}' ", userId);
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus == "退款退货")
                {
                    sbSql.AppendFormat(" AND IsRefund=1 ");
                }
                else
                {
                    sbSql.AppendFormat(" AND Status='{0}' ", orderStatus);
                }
            }
            return GetCount<WXMallOrderInfo>(sbSql.ToString());
        }
        /// <summary>
        /// 待供应商确认
        /// </summary>
        /// <returns></returns>
        public int SupplierIsConfirm(string supplierUserId)
        {
            return GetCount<SupplierSettlement>(string.Format(" WebsiteOwner='{0}' AND SupplierUserId='{1}'   AND Status='待供应商确认' ", WebsiteOwner, supplierUserId));
        }

        /// <summary>
        /// 统计订单及商品
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool StatisticsOrderAndProduct(TimingTask task)
        {

            if (StatisticsOrder(task))
            {
                if (StatisticsProduct(task))
                {
                    return true;
                }

            }
            return false;

        }

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <returns></returns>
        public bool StatisticsOrder(TimingTask task)
        {
            #region 筛选所有订单
            StringBuilder sbWhereAll = new StringBuilder();
            sbWhereAll.AppendFormat(" WebsiteOwner='{0}' And PaymentStatus=1 And Status!='已取消' And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0", task.WebsiteOwner, ((DateTime)task.FromDate).ToString(), ((DateTime)task.ToDate).ToString());
            if (!string.IsNullOrEmpty(task.ChannelUserId))
            {
                sbWhereAll.AppendFormat(" And ChannelUserId='{0}'", task.ChannelUserId);
            }
            if (!string.IsNullOrEmpty(task.DistributionUserId))
            {
                sbWhereAll.AppendFormat(" And (OrderUserId='{0}'Or DistributionOwner='{0}' )", task.DistributionUserId);
            }
            #endregion

            #region 筛选交易成功的订单
            StringBuilder sbWhere = new StringBuilder();//筛选条件

            sbWhere.AppendFormat(" WebsiteOwner='{0}' And PaymentStatus=1 And Status='交易成功'  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0", task.WebsiteOwner, ((DateTime)task.FromDate).ToString(), ((DateTime)task.ToDate).ToString());
            if (!string.IsNullOrEmpty(task.ChannelUserId))
            {
                sbWhere.AppendFormat(" And ChannelUserId='{0}'", task.ChannelUserId);
            }
            if (!string.IsNullOrEmpty(task.DistributionUserId))
            {
                sbWhere.AppendFormat(" And (OrderUserId='{0}'Or DistributionOwner='{0}' )", task.DistributionUserId);
            }
            #endregion


            List<WXMallOrderInfo> orderListAll = GetList<WXMallOrderInfo>(string.Format(" {0}", sbWhereAll.ToString()));//所有订单
            List<WXMallOrderInfo> orderListSuccess = GetList<WXMallOrderInfo>(string.Format(" {0}", sbWhere.ToString()));//所有交易成功订单
            List<WXMallStatisticsOrderDetail> statisticsOrderList = new List<WXMallStatisticsOrderDetail>();
            List<WXMallRefund> refundList = GetList<WXMallRefund>(string.Format("OrderId in(Select OrderId from ZCJ_WXMallOrderInfo where {0})", sbWhere.ToString()));//退款的

            List<WXMallOrderDetailsInfo> refundSuccessOrderDetailList = new List<WXMallOrderDetailsInfo>();//退款成功的订单明细

            foreach (var item in refundList.Where(p => p.Status == 6))
            {
                WXMallOrderDetailsInfo detail = GetOrderDetail(item.OrderDetailId);
                refundSuccessOrderDetailList.Add(detail);
            }

            WXMallStatisticsOrder model = new WXMallStatisticsOrder();
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = task.WebsiteOwner;
            model.TaskId = task.TaskId;
            model.FromDate = task.FromDate;
            model.ToDate = task.ToDate;

            #region 订单总金额
            string sqlTotalAmount = string.Format("Select Sum(TotalAmount) from ZCJ_WXMallOrderInfo where {0}", sbWhere.ToString());
            var totalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalAmount);
            if (totalAmount != null)
            {
                model.TotalAmount = decimal.Parse(totalAmount.ToString());

            }
            #endregion

            #region 订单总数量
            string sqlTotalCount = string.Format("Select Count(*) from ZCJ_WXMallOrderInfo where {0}", sbWhere.ToString());
            var totalCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalCount);
            if (totalCount != null)
            {
                model.TotalCount = int.Parse(totalCount.ToString());

            }
            #endregion

            #region 订单总基价
            string sqlBaseTotalAmount = string.Format("Select Sum(BasePrice*TotalCount) from ZCJ_WXMallOrderDetailsInfo where OrderID In(Select OrderId from ZCJ_WXMallOrderInfo where {0} )", sbWhere.ToString());
            var baseTotalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlBaseTotalAmount);
            if (baseTotalAmount != null)
            {
                model.BaseTotalAmount = decimal.Parse(baseTotalAmount.ToString());

            }
            model.BaseTotalAmount -= refundSuccessOrderDetailList.Sum(p => p.BasePrice * p.TotalCount);
            #endregion

            #region 订单总运费

            string sqlTotalTranFee = string.Format("Select Sum(Transport_Fee) from ZCJ_WXMallOrderInfo where {0}", sbWhere.ToString());
            var totalTranFee = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalTranFee);
            if (totalTranFee != null)
            {
                model.TotalTransportFee = decimal.Parse(totalTranFee.ToString());
            }
            #endregion

            #region 总优惠券抵扣
            string sqlCouponExAmount = string.Format("Select Sum(CardcouponDisAmount) from ZCJ_WXMallOrderInfo where {0} And IsNull(CouponType,0)=0", sbWhere.ToString());
            var couponExAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlCouponExAmount);
            if (couponExAmount != null)
            {
                model.TotalCouponExchangAmount = decimal.Parse(couponExAmount.ToString());
            }
            #endregion

            #region 总储值卡抵扣
            string sqlStoreCardExAmount = string.Format("Select Sum(CardcouponDisAmount) from ZCJ_WXMallOrderInfo where {0} And IsNull(CouponType,0)=1", sbWhere.ToString());
            var storeCardExAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlStoreCardExAmount);
            if (storeCardExAmount != null)
            {
                model.TotalStorecardExchangAmount = decimal.Parse(storeCardExAmount.ToString());
            }
            #endregion

            #region 总积分抵扣
            string sqlScoreExAmount = string.Format("Select Sum(ScoreExchangAmount) from ZCJ_WXMallOrderInfo where {0}", sbWhere.ToString());
            var scoreExAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlScoreExAmount);
            if (scoreExAmount != null)
            {
                model.TotalScoreExchangAmount = decimal.Parse(scoreExAmount.ToString());
            }
            #endregion

            #region 总余额抵扣
            string sqlAccountAmountExAmount = string.Format("Select Sum(UseAmount) from ZCJ_WXMallOrderInfo where {0}", sbWhere.ToString());
            var accountAmountExAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlAccountAmountExAmount);
            if (accountAmountExAmount != null)
            {
                model.TotalAccountAmountExchangAmount = decimal.Parse(accountAmountExAmount.ToString());
            }
            #endregion

            #region 总商品件数
            string sqlTotalProductCount = string.Format("Select Sum(TotalCount) from ZCJ_WXMallOrderDetailsInfo where OrderId in( Select OrderId from ZCJ_WXMallOrderInfo where {0})", sbWhere.ToString());
            var totalProductCount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalProductCount);
            if (totalProductCount != null)
            {
                model.TotalProductCount = int.Parse(totalProductCount.ToString());

            }
            model.TotalProductCount -= refundSuccessOrderDetailList.Sum(p => p.TotalCount);
            #endregion

            #region 总商品应付金额
            string sqlTotalProductFee = string.Format("Select Sum(Product_Fee) from ZCJ_WXMallOrderInfo where {0}", sbWhere.ToString());
            var totalProductFee = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalProductFee);
            if (totalProductFee != null)
            {
                model.TotalProductFee = decimal.Parse(totalProductFee.ToString());

            }
            //model.TotalProductFee -= refundOrderDetailList.Sum(p => (decimal)p.OrderPrice * p.TotalCount);
            #endregion

            #region 退款总金额
            model.TotalRefundAmount = refundList.Sum(p => p.RefundAmount);
            #endregion

            #region 利润
            model.Profit = model.TotalAmount - model.BaseTotalAmount - model.TotalTransportFee - model.TotalRefundAmount;

            #endregion

            #region 应该分佣订单数

            foreach (var item in orderListSuccess)
            {
                if (ShouldCommion(item))
                {
                    model.ShouldCommissionOrderCount++;
                    WXMallStatisticsOrderDetail detail = new WXMallStatisticsOrderDetail();
                    detail.TaskId = task.TaskId;
                    detail.Type = "ShouldCommission";
                    detail.OrderId = item.OrderID.ToString();
                    detail.WebsiteOwner = task.WebsiteOwner;
                    statisticsOrderList.Add(detail);


                }
            }



            #endregion

            #region 实际分佣订单数
            List<ProjectCommission> projectCommList = GetList<ProjectCommission>(string.Format("ProjectId in(Select OrderId from ZCJ_WXMallOrderInfo where {0})", sbWhere.ToString())).DistinctBy(p => p.ProjectId).ToList();
            model.RealCommissionOrderCount = projectCommList.Count;
            #region 明细
            foreach (var item in projectCommList)
            {
                WXMallStatisticsOrderDetail detail = new WXMallStatisticsOrderDetail();
                detail.TaskId = task.TaskId;
                detail.Type = "RealCommission";
                detail.OrderId = item.ProjectId.ToString();
                detail.WebsiteOwner = task.WebsiteOwner;
                statisticsOrderList.Add(detail);
            }
            #endregion
            #endregion

            #region 最后一个订单确认收货时间
            var last = orderListSuccess.OrderByDescending(p => p.ReceivingTime).FirstOrDefault();
            if (last != null)
            {
                model.LastReceivingTime = last.ReceivingTime.ToString();
            }

            #endregion

            #region 交易成功且在退款中的订单数
            model.RefundOrderCount = refundList.Count(p => p.Status != 6 && p.Status != 7);
            #region 明细
            foreach (var item in refundList.Where(p => p.Status != 6 && p.Status != 7))
            {
                WXMallStatisticsOrderDetail detail = new WXMallStatisticsOrderDetail();
                detail.TaskId = task.TaskId;
                detail.Type = "Refund";
                detail.OrderId = item.OrderId.ToString();
                detail.WebsiteOwner = task.WebsiteOwner;
                statisticsOrderList.Add(detail);
            }
            #endregion

            #endregion

            #region 待处理的订单数
            model.WaitProcessOrderCount = orderListAll.Count(p => p.Status != "交易成功");
            #region 明细
            foreach (var item in orderListAll.Where(p => p.Status != "交易成功"))
            {
                WXMallStatisticsOrderDetail detail = new WXMallStatisticsOrderDetail();
                detail.TaskId = task.TaskId;
                detail.Type = "WaitProcess";
                detail.OrderId = item.OrderID.ToString();
                detail.WebsiteOwner = task.WebsiteOwner;
                statisticsOrderList.Add(detail);
            }
            #endregion
            #endregion

            #region 所有交易成功订单明细
            foreach (var item in orderListSuccess)
            {
                WXMallStatisticsOrderDetail detail = new WXMallStatisticsOrderDetail();
                detail.TaskId = task.TaskId;
                detail.Type = "All";
                detail.OrderId = item.OrderID.ToString();
                detail.WebsiteOwner = task.WebsiteOwner;
                statisticsOrderList.Add(detail);
            }
            #endregion

            #region 记录明细
            if (statisticsOrderList.Count > 0)
            {
                AddList<WXMallStatisticsOrderDetail>(statisticsOrderList);
            }
            #endregion
            return Add(model);
        }

        /// <summary>
        /// 商品统计
        /// </summary>
        /// <returns></returns>
        public bool StatisticsProduct(TimingTask task)
        {


            WXMallStatisticsOrder statisOrder = Get<WXMallStatisticsOrder>(string.Format("TaskId='{0}'", task.TaskId));
            #region 筛选
            StringBuilder sbWhere = new StringBuilder();//筛选条件
            sbWhere.AppendFormat(" WebsiteOwner='{0}' And PaymentStatus=1 And Status='交易成功'  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0 ", task.WebsiteOwner, ((DateTime)task.FromDate).ToString(), ((DateTime)task.ToDate).ToString());
            if (!string.IsNullOrEmpty(task.ChannelUserId))
            {
                sbWhere.AppendFormat(" And ChannelUserId='{0}'", task.ChannelUserId);
            }
            if (!string.IsNullOrEmpty(task.DistributionUserId))
            {
                sbWhere.AppendFormat(" And (OrderUserId='{0}'Or DistributionOwner='{0}' )", task.DistributionUserId);
            }
            #endregion

            List<WXMallRefund> refundList = GetList<WXMallRefund>(string.Format("OrderId in(Select OrderId from ZCJ_WXMallOrderInfo where {0}) And Status=6", sbWhere.ToString()));//退款成功的
            List<WXMallOrderDetailsInfo> refundOrderDetailList = new List<WXMallOrderDetailsInfo>();//退款的订单明细
            foreach (var item in refundList)
            {
                WXMallOrderDetailsInfo detail = GetOrderDetail(item.OrderDetailId);
                refundOrderDetailList.Add(detail);
            }


            List<WXMallOrderDetailsInfo> orderDetailList = GetList<WXMallOrderDetailsInfo>(string.Format(" OrderId in(Select OrderId from ZCJ_WXMallOrderInfo where {0})", sbWhere.ToString()));
            foreach (var item in refundOrderDetailList)
            {
                orderDetailList.Remove(item);

            }

            List<WXMallOrderDetailsInfo> orderDetailListDistinct = orderDetailList.DistinctBy(p => p.SkuId).ToList();
            List<WXMallStatisticsProduct> list = new List<WXMallStatisticsProduct>();
            decimal totalProductFee = (decimal)orderDetailList.Sum(p => p.OrderPrice * p.TotalCount) - orderDetailList.Sum(p => p.BasePrice * p.TotalCount);
            decimal totalProductFee1 = (decimal)orderDetailList.Sum(p => p.OrderPrice * p.TotalCount);
            foreach (var item in orderDetailListDistinct)
            {

                WXMallStatisticsProduct model = new WXMallStatisticsProduct();
                model.InsertDate = DateTime.Now;
                model.WebsiteOwner = task.WebsiteOwner;
                model.TaskId = task.TaskId;
                model.FromDate = task.FromDate;
                model.ToDate = task.ToDate;
                model.ProductName = item.ProductName + " " + item.SkuShowProp;

                #region 商品总数量
                model.ProductTotalCount = orderDetailList.Where(p => p.SkuId == item.SkuId).Sum(p => p.TotalCount);
                #endregion

                #region 订单总数量
                model.OrderTotalCount = orderDetailList.Count(p => p.SkuId == item.SkuId);
                #endregion

                #region 订单总基价
                model.OrderBaseTotalAmount = orderDetailList.Where(p => p.SkuId == item.SkuId).Sum(p => p.BasePrice * p.TotalCount);
                #endregion


                #region 利润
                decimal singleProductFee = (decimal)orderDetailList.Where(p => p.SkuId == item.SkuId).Sum(p => p.OrderPrice * p.TotalCount) - model.OrderBaseTotalAmount;//单个sku毛利
                decimal rate = singleProductFee / totalProductFee;
                model.Profit = statisOrder.Profit * rate;
                #endregion

                #region 订单总金额
                decimal singleProductFee1 = (decimal)orderDetailList.Where(p => p.SkuId == item.SkuId).Sum(p => p.OrderPrice * p.TotalCount);
                decimal rate1 = singleProductFee1 / totalProductFee1;
                model.OrderTotalAmount = statisOrder.TotalAmount * rate1;
                #endregion


                model.OrderTotalOrderPrice = orderDetailList.Where(p => p.SkuId == item.SkuId).Sum(p => ((decimal)p.OrderPrice) * p.TotalCount);
                model.TotalRefundAmount = refundList.Where(p => p.OrderDetailId == item.AutoID).Sum(p => p.RefundAmount);
                list.Add(model);

            }
            if (list.Count > 0)
            {
                if (list.Sum(p => p.OrderTotalAmount) != statisOrder.TotalAmount)//订单金额有误差
                {
                    list[list.Count - 1].OrderTotalAmount += statisOrder.TotalAmount - list.Sum(p => p.OrderTotalAmount);//最后一种商品消除误差
                }
                if (list.Sum(p => p.Profit) != statisOrder.Profit)//利润金额有误差
                {
                    list[list.Count - 1].Profit += statisOrder.Profit - list.Sum(p => p.Profit);//最后一种商品消除误差
                }
            }
            foreach (var item in list)
            {
                Add(item);
            }

            return true;
        }

        /// <summary>
        /// 指示一个订单是否应该分佣
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public bool ShouldCommion(WXMallOrderInfo orderInfo)
        {
            ZentCloud.BLLPermission.BLLMenuPermission bllMenuPermission = new ZentCloud.BLLPermission.BLLMenuPermission("");
            if (bllMenuPermission.CheckUserAndPmsKey(orderInfo.WebsiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.OnlineDistribution, orderInfo.WebsiteOwner))
            {
                WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);
                if (websiteInfo.IsDisabledCommission == 0)
                {
                    UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                    if (bllUser.IsDistributionMember(orderUserInfo))
                    {
                        return true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(orderUserInfo.DistributionOwner))
                        {
                            UserInfo preUserInfo = bllUser.GetUserInfo(orderUserInfo.DistributionOwner, orderInfo.WebsiteOwner);
                            if (bllUser.IsDistributionMember(preUserInfo))
                            {
                                return true;
                            }
                        }
                    }


                }
            }

            return false;

        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderInfo">订单信息</param>
        /// <returns></returns>
        public bool CancelOrder(WXMallOrderInfo orderInfo, out string msg)
        {
            //WriteLog(orderInfo, "1");
            msg = "";
            var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            if (orderUserInfo == null)
            {
                msg = "用户不存在";
                return false;
            }
            // WriteLog(orderInfo, "2");

            var websiteInfo = GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);
            //WriteLog(orderInfo, "3");
            Open.HongWareSDK.MemberInfo hongWareMemberInfo = null;
            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
            if (websiteInfo.IsUnionHongware == 1)
            {
                hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
            }
            //WriteLog(orderInfo, "4");
            BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
            Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
            List<string> limitStatus = new List<string>() { "待发货", "已发货", "交易成功", "已取消", "预约成功", "预约失败" };
            if (limitStatus.Contains(orderInfo.Status))
            {
                msg = "只有状态为待付款,待发货的订单才可以取消";
                return false;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (Update(orderInfo, " Status='已取消'", string.Format(" OrderId='{0}'", orderInfo.OrderID), tran) <= 0)
                {
                    tran.Rollback();
                    msg = "更新订单状态失败";
                    return false;
                }

                //返还库存 BLLMall 中也有对应的方法返还SKU
                if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                {
                    List<WXMallOrderDetailsInfo> orderDetailList = GetOrderDetailsList(orderInfo.OrderID);
                    foreach (var orderDetail in orderDetailList)
                    {

                        if (orderDetail.SkuId != null)
                        {
                            ProductSku sku = GetProductSku((int)orderDetail.SkuId);
                            if (sku != null)
                            {
                                if (Update(sku, string.Format(" Stock+={0}", orderDetail.TotalCount), string.Format(" SkuId={0}", sku.SkuId), tran) == 0)
                                {
                                    tran.Rollback();
                                    msg = "修改sku库存失败";
                                    return false;

                                }
                            }
                        }


                    }
                    if (Update(new WXMallOrderDetailsInfo(), "IsComplete=0", string.Format(" OrderId='{0}'", orderInfo.OrderID), tran) <= 0)
                    {
                        tran.Rollback();
                        msg = "更新订单详情失败";
                        return false;
                    }


                }

                #region 积分返还
                if (orderInfo.UseScore > 0)//使用积分 积分返还
                {
                    //orderUserInfo.TotalScore += orderInfo.UseScore;
                    if (bllUser.Update(orderUserInfo,
                        string.Format(" TotalScore+={0}", orderInfo.UseScore),
                        string.Format(" AutoID={0}", orderUserInfo.AutoID),
                        tran
                        ) < 0)
                    {
                        tran.Rollback();
                        msg = "积分返还失败";
                        return false;
                    }
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = orderInfo.UseScore;
                    scoreRecord.TotalScore = orderUserInfo.TotalScore;
                    scoreRecord.ScoreType = "OrderCancel";
                    scoreRecord.UserID = orderUserInfo.UserID;
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                    if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                    {
                        scoreRecord.AddNote = "微商城-订单取消返还积分";
                    }
                    else
                    {
                        scoreRecord.AddNote = "预约-订单取消返还积分";
                    }
                    if (!Add(scoreRecord, tran))
                    {
                        msg = "添加积分记录失败";
                        tran.Rollback();
                        return false;
                    }



                    #region 宏巍加积分

                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        if (hongWareMemberInfo.member != null)
                        {

                            if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, orderInfo.UseScore))
                            {
                                tran.Rollback();
                                msg = "更新宏巍积分失败";
                                return false;

                            }


                        }


                    }
                    #endregion


                }
                #endregion

                //WriteLog(orderInfo, "5");

                #region 优惠券储值卡返还
                if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
                {
                    //WriteLog(orderInfo, "6");
                    var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), orderUserInfo.UserID);
                    //WriteLog(orderInfo, "7");
                    if (myCardCoupon != null)
                    {
                        if (myCardCoupon.Status == 1)
                        {
                            myCardCoupon.Status = 0;
                            if (!bllCardCoupon.Update(myCardCoupon, tran))
                            {

                                tran.Rollback();
                                msg = "优惠券更新失败";
                                return false;
                            }
                        }


                    }
                    else
                    {
                        Delete(new StoredValueCardUseRecord(), string.Format("OrderId='{0}'", orderInfo.OrderID), tran);
                    }

                }
                #endregion



                #region 账户余额返还

                if (orderInfo.UseAmount > 0)
                {
                    //orderUserInfo.AccountAmount += orderInfo.UseAmount;
                    if (Update(
                        orderUserInfo,
                        string.Format(" AccountAmount+={0}", orderInfo.UseAmount),
                        string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) < 0)
                    {
                        tran.Rollback();
                        msg = "更新用户余额失败";
                        return false;
                    }
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = (double)orderInfo.UseAmount;
                    scoreRecord.TotalScore = (double)orderUserInfo.AccountAmount;
                    scoreRecord.ScoreType = "AccountAmount";
                    scoreRecord.UserID = orderUserInfo.UserID;
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                    scoreRecord.AddNote = "微商城-订单取消返还余额";
                    if (!Add(scoreRecord, tran))
                    {
                        tran.Rollback();
                        msg = "插入余额记录失败";
                        return false;
                    }
                    #region 宏巍加余额

                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        if (hongWareMemberInfo.member != null)
                        {
                            if (!hongWareClient.UpdateMemberBlance(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, (float)orderInfo.UseAmount))
                            {
                                tran.Rollback();
                                msg = "更新宏巍余额失败";
                                return false;

                            }


                        }


                    }
                    #endregion




                }


                #endregion


                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, orderInfo.WebsiteOwner, ""))
                {
                    if (orderInfo.UseScore > 0)
                    {
                        yiKeClient.BonusUpdate(orderUserInfo.Ex2, orderInfo.UseScore, string.Format("订单:{0}取消返还{1}积分", orderInfo.OrderID, orderInfo.UseScore));

                    }
                    yiKeClient.ChangeStatus(orderInfo.OrderID, "已取消");

                }

                //冻结积分取消
                bllScore.CancelLockScoreByOrder(orderInfo.OrderID, "取消订单，积分取消", tran);

                #region 返还代付人余额、储值卡
                if (!string.IsNullOrEmpty(orderInfo.OtherUserId))
                {

                    orderUserInfo = bllUser.GetUserInfo(orderInfo.OtherUserId, orderInfo.WebsiteOwner);

                    if (orderInfo.OtherUseAmount > 0)
                    {
                        if (Update(
                           orderUserInfo,
                           string.Format(" AccountAmount+={0}", orderInfo.OtherUseAmount),
                           string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) < 0)
                        {
                            tran.Rollback();
                            msg = "更新用户余额失败";
                            return false;
                        }
                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        scoreRecord.AddTime = DateTime.Now;
                        scoreRecord.Score = (double)orderInfo.OtherUseAmount;
                        scoreRecord.TotalScore = (double)orderUserInfo.AccountAmount;
                        scoreRecord.ScoreType = "AccountAmount";
                        scoreRecord.UserID = orderUserInfo.UserID;
                        scoreRecord.RelationID = orderInfo.OrderID;
                        scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                        scoreRecord.AddNote = "微商城-订单取消返还余额";
                        if (!Add(scoreRecord, tran))
                        {
                            tran.Rollback();
                            msg = "插入余额记录失败";
                            return false;
                        }
                    }

                    if (Convert.ToInt32(orderInfo.OtherMyCouponCardId) > 0)
                    {
                        Delete(new StoredValueCardUseRecord(), string.Format("OrderId='{0}'", orderInfo.OrderID));
                    }


                }
                #endregion

                tran.Commit();
                Update(orderInfo, string.Format("Status='已取消'"), string.Format(" ParentOrderId='{0}'", orderInfo.OrderID));


                #region 累计销售额取消
                orderUserInfo.DistributionSaleAmountLevel0 -= orderInfo.TotalAmount;
                if (orderUserInfo.DistributionSaleAmountLevel0 <= 0)
                {
                    orderUserInfo.DistributionSaleAmountLevel0 = 0;
                }
                Update(new UserInfo(), string.Format(" DistributionSaleAmountLevel0={0}", orderUserInfo.DistributionSaleAmountLevel0), string.Format(" UserId='{0}'", orderInfo.OrderUserID));
                //WriteLog(orderInfo, "7");
                BLLDistribution bllDistribution = new BLLDistribution();
                UserInfo upUserLevel1 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 1);
                if (upUserLevel1 != null)
                {

                    upUserLevel1.DistributionSaleAmountLevel1 -= orderInfo.TotalAmount;
                    if (upUserLevel1.DistributionSaleAmountLevel1 <= 0)
                    {
                        upUserLevel1.DistributionSaleAmountLevel1 = 0;
                    }
                    Update(new UserInfo(), string.Format(" DistributionSaleAmountLevel1={0}", upUserLevel1.DistributionSaleAmountLevel1), string.Format(" UserId='{0}'", upUserLevel1.UserID));


                }
                #endregion

                ClearProductListCacheByOrder(orderInfo);

                msg = "ok";
                return true;


            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = ex.ToString();
                return false;

            }
            return false;

        }

        //public void WriteLog(WXMallOrderInfo orderInfo, string message)
        //{
        //    if (orderInfo.OrderID=="1724745")
        //    {
        //        try
        //        {
        //            using (StreamWriter sw = new StreamWriter("D:\\log\\cancellog.txt", true, Encoding.UTF8))
        //            {
        //                sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
        //            }

        //        }
        //        catch { }

        //    }


        //}
        /// <summary>
        /// 确认收货 交易成功
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool OrderSuccess(WXMallOrderInfo orderInfo, out string msg)
        {

            msg = "";
            var websiteInfo = GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);
            var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            BllScore bllScore = new BllScore();
            Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
            List<WXMallOrderDetailsInfo> orderDetailList;
            if (orderInfo.Status.Equals("交易成功"))
            {

                msg = "已经收过货了，请不要重复收货";
                return false;
            }
            if (!orderInfo.Status.Equals("已发货"))
            {
                msg = "只有状态是已发货的订单才能确认收货";
                return false;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {


                orderInfo.Status = "交易成功";
                orderInfo.ReceivingTime = DateTime.Now;
                orderInfo.DistributionStatus = 2;
                if (!Update(orderInfo, tran))
                {
                    tran.Rollback();
                    msg = "更新订单状态失败";
                    return false;

                }

                #region 交易成功加积分


                //TODO:这边改成增加的是增加冻结积分，到了分佣的时候才真正把积分加上，mixblu按照原规则走：确认收货就发积分、积分按应付金额来算，其他的则按新规则来
                if (websiteInfo.WebsiteOwner == "mixblu")
                {
                    #region mixblu加积分
                    //增加积分
                    ScoreConfig scoreConfig = bllScore.GetScoreConfig(orderInfo.WebsiteOwner);
                    int addScore = 0;
                    if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
                    {
                        addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                    }
                    if (addScore > 0)
                    {
                        //orderUserInfo.TotalScore += addScore;

                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        scoreRecord.AddTime = DateTime.Now;
                        scoreRecord.Score = addScore;
                        scoreRecord.TotalScore = orderUserInfo.TotalScore;
                        scoreRecord.ScoreType = "OrderSuccess";
                        scoreRecord.UserID = orderUserInfo.UserID;
                        scoreRecord.AddNote = "微商城-交易成功获得积分";
                        scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                        if (!Add(scoreRecord, tran))
                        {
                            tran.Rollback();
                            msg = "插入积分记录表失败";
                            return false;

                        }
                        if (bllUser.Update(orderUserInfo, string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore), string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) < 1)
                        {
                            tran.Rollback();
                            msg = "更新用户积分失败";
                            return false;
                        }

                        #region yike 加积分
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, orderInfo.WebsiteOwner, ""))
                        {

                            if ((!string.IsNullOrEmpty(orderUserInfo.Ex2)) && (!string.IsNullOrEmpty(orderUserInfo.Phone)))
                            {
                                try
                                {
                                    //驿氪同步
                                    yiKeClient.BonusUpdate(orderUserInfo.Ex2, addScore, string.Format("订单交易成功获得{0}积分", addScore));
                                    //驿氪同步
                                    yiKeClient.ChangeStatus(orderInfo.OrderID, orderInfo.Status);

                                }
                                catch (Exception)
                                {


                                }

                            }


                        }
                        #endregion

                        #region 宏巍加积分
                        if (websiteInfo.IsUnionHongware == 1)
                        {
                            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                            var hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
                            if (hongWareMemberInfo.member != null)
                            {
                                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, addScore))
                                {
                                    tran.Rollback();
                                    msg = "更新宏巍积分失败";
                                    return false;

                                }


                            }


                        }
                        #endregion

                    }
                    #endregion
                }
                else
                {

                }

                #endregion

                //

                //更新订单明细表状态
                orderDetailList = GetOrderDetailsList(orderInfo.OrderID);
                foreach (var orderDetail in orderDetailList)
                {
                    orderDetail.IsComplete = 1;
                    orderDetail.CompleteTime = DateTime.Now;
                    if (!Update(orderDetail))
                    {
                        tran.Rollback();
                        msg = "更新订单明细表失败";
                        return false;
                    }

                }




            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = ex.ToString();
                return false;

            }

            tran.Commit();
            #region 更新销量
            foreach (var orderDetail in orderDetailList)
            {
                int saleCount = GetProductSaleCount(int.Parse(orderDetail.PID));
                Update(new WXMallProductInfo(), string.Format("SaleCount={0}", saleCount), string.Format(" PID='{0}'", orderDetail.PID));

            }
            #endregion

            return true;

        }

        /// <summary>
        /// 已经结算结算单
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="supplierUserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="settlementId"></param>
        /// <param name="date"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<SupplierSettlement> SettlementList(string websiteOwner, string supplierUserId, int pageIndex, int pageSize, string status, string settlementId, string date, out int totalCount)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(websiteOwner))
            {
                sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            }
            if (!string.IsNullOrEmpty(supplierUserId))
            {
                sbWhere.AppendFormat(" And SupplierUserId='{0}'", supplierUserId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status='{0}'", status);
            }
            if (!string.IsNullOrEmpty(date))
            {
                sbWhere.AppendFormat(" And FromDate='{0}'", date);
            }
            if (!string.IsNullOrEmpty(settlementId))
            {
                sbWhere.AppendFormat(" And SettlementId='{0}'", settlementId);
            }

            totalCount = GetCount<SupplierSettlement>(sbWhere.ToString());
            return GetLit<SupplierSettlement>(pageSize, pageIndex, sbWhere.ToString(), "AutoId DESC");

        }

        /// <summary>
        /// 未结算订单
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="supplierUserId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="orderFromDate"></param>
        /// <param name="orderToDate"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<SupplierUnSettlement> UnSettlementList(string websiteOwner, int pageIndex, int pageSize, string supplierUserId, string orderId, string orderStatus, string orderFromDate, string orderToDate, out int totalCount)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(websiteOwner))
            {
                sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            }
            if (!string.IsNullOrEmpty(supplierUserId))
            {
                sbWhere.AppendFormat(" And SupplierUserId='{0}'", supplierUserId);
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                sbWhere.AppendFormat(" And OrderId='{0}'", orderId);
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus != "退款退货")
                {
                    sbWhere.AppendFormat(" And OrderStatus='{0}'", orderStatus);
                }
                else
                {
                    sbWhere.AppendFormat(" And IsRefund=1 ");
                }
                if (orderStatus == "交易成功")
                {
                    sbWhere.AppendFormat(" And IsRefund=0 ");
                }

            }
            if (!string.IsNullOrEmpty(orderFromDate))
            {
                sbWhere.AppendFormat(" And OrderDate>='{0}'", orderFromDate);
            }
            if (!string.IsNullOrEmpty(orderToDate))
            {
                sbWhere.AppendFormat(" And OrderDate<='{0}'", orderToDate);
            }

            totalCount = GetCount<SupplierUnSettlement>(sbWhere.ToString());
            return GetLit<SupplierUnSettlement>(pageSize, pageIndex, sbWhere.ToString(), "AutoId DESC");

        }
        /// <summary>
        /// 获取结算单
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="settlementId"></param>
        /// <param name="date"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<SupplierChannelSettlement> SupplierChannelSettlementList(string websiteOwner, string userId, int pageIndex, int pageSize, string status, string settlementId, string date, out int totalCount)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(websiteOwner))
            {
                sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId='{0}'", userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status='{0}'", status);
            }
            if (!string.IsNullOrEmpty(date))
            {
                sbWhere.AppendFormat(" And FromDate='{0}'", date);
            }
            if (!string.IsNullOrEmpty(settlementId))
            {
                sbWhere.AppendFormat(" And SettlementId='{0}'", settlementId);
            }

            totalCount = GetCount<SupplierChannelSettlement>(sbWhere.ToString());
            return GetLit<SupplierChannelSettlement>(pageSize, pageIndex, sbWhere.ToString(), "AutoId DESC");

        }



        /// <summary>
        /// 更新结算单
        /// </summary>
        /// <param name="settlementId"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public bool UpdateSupplierSettlement(string settlementId, string status, string remark, string img)
        {
            var model = Get<SupplierSettlement>(string.Format("SettlementId='{0}'", settlementId));
            model.Status = status;
            if (!string.IsNullOrEmpty(remark))
            {
                model.Remark = remark;
            }
            if (!string.IsNullOrEmpty(img))
            {
                model.ImgUrl = img;
            }
            return Update(model);

        }

        /// <summary>
        /// 更新供应商渠道结算单
        /// </summary>
        /// <param name="settlementId"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public bool UpdateSupplierChannelSettlement(string settlementId, string status, string remark, string img)
        {
            var model = Get<SupplierChannelSettlement>(string.Format("SettlementId='{0}'", settlementId));
            model.Status = status;
            if (!string.IsNullOrEmpty(remark))
            {
                model.Remark = remark;
            }
            if (!string.IsNullOrEmpty(img))
            {
                model.ImgUrl = img;
            }
            return Update(model);

        }
        /// <summary>
        /// 结算单详情
        /// </summary>
        /// <param name="settlementId">结算单号</param>
        /// <param name="orderId">订单号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<SupplierSettlementDetail> SettlementDetail(string settlementId, string orderId, int pageIndex, int pageSize, out int totalCount)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(settlementId))
            {
                sbWhere.AppendFormat(" SettlementId='{0}'", settlementId);
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                sbWhere.AppendFormat(" And OrderId='{0}'", orderId);
            }
            totalCount = GetCount<SupplierSettlementDetail>(sbWhere.ToString());
            return GetLit<SupplierSettlementDetail>(pageSize, pageIndex, sbWhere.ToString(), "OrderId ASC");

        }


        /// <summary>
        /// 供应商结算
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <param name="fromDate">开始日期</param>
        /// <param name="toDate">结束日期</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool SupplierSettlement(string websiteOwner, string fromDate, string toDate, out string msg)
        {
            msg = "";
            int totalSupplierCount;
            var supplierList = bllUser.GetSupplierList(websiteOwner, 1, int.MaxValue, "", "", out totalSupplierCount);//供应商
            foreach (var supplier in supplierList)
            {
                #region 可以结算的订单

                //条件 1交易成功 2.已经分佣 3.无退款
                StringBuilder sbWhere = new StringBuilder();//筛选条件
                sbWhere.AppendFormat(" WebsiteOwner='{0}' And TotalAmount>0 And PaymentStatus=1 And Status='交易成功'  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0 And SupplierUserId='{3}'   And OrderId Not In  (Select OrderID from ZCJ_WXMallRefund Where ZCJ_WXMallRefund.OrderId=OrderId)   And OrderId In (Select ProjectId from ZCJ_ProjectCommission Where ZCJ_ProjectCommission.ProjectId=OrderId)", websiteOwner, fromDate, toDate, supplier.UserID);


                string settlementId = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), supplier.AutoID);
                List<WXMallOrderInfo> orderList = GetList<WXMallOrderInfo>(sbWhere.ToString());
                List<SupplierSettlementDetail> suplierSettlementDetailList = new List<SupplierSettlementDetail>();


                List<WXMallOrderInfo> orderListCanSettHistory = GetList<WXMallOrderInfo>(string.Format(" IsCanSettlement='1' And WebsiteOwner='{0}' And SupplierUserId='{1}'", websiteOwner, supplier.UserID));//过往未结算的订单现在一起结算

                orderList.AddRange(orderListCanSettHistory);
                orderList = orderList.DistinctBy(p => p.OrderID).ToList();
                List<SupplierUnSettlement> suplierUnSettlementList = new List<SupplierUnSettlement>();//未结算订单
                foreach (var order in orderList)
                {

                    SupplierSettlementDetail settlementDetail = new SupplierSettlementDetail();
                    settlementDetail.InsertDate = DateTime.Now;
                    settlementDetail.WebsiteOwner = websiteOwner;
                    settlementDetail.OrderId = order.OrderID;
                    settlementDetail.SettlementId = settlementId;
                    List<WXMallOrderDetailsInfo> orderDetailList = GetList<WXMallOrderDetailsInfo>(string.Format("OrderId='{0}'", order.OrderID));
                    //List<WXMallRefund> refundList = GetList<WXMallRefund>(string.Format(" OrderId='{0}' And Status!=7",order.OrderID));
                    //foreach (var item in refundList)
                    //{
                    //    WXMallOrderDetailsInfo detail = GetOrderDetail(item.OrderDetailId);
                    //    if (orderDetailList.Contains(detail))
                    //    {
                    //        orderDetailList.Remove(detail);
                    //    }
                    //}
                    settlementDetail.BaseAmount = orderDetailList.Sum(p => p.BasePrice * p.TotalCount);
                    settlementDetail.TransportFee = order.Transport_Fee;
                    //settlementDetail.RefundAmount = refundList.Sum(p => p.RefundAmount);
                    settlementDetail.SettlementAmount = settlementDetail.BaseAmount + settlementDetail.TransportFee;
                    suplierSettlementDetailList.Add(settlementDetail);

                    Delete(new SupplierUnSettlement(), string.Format(" OrderId='{0}'", order.OrderID));//删除未结算单中对应的订单


                }
                SupplierSettlement supplierSettlementModel = new Model.SupplierSettlement();
                supplierSettlementModel.FromDate = Convert.ToDateTime(fromDate);
                supplierSettlementModel.ToDate = Convert.ToDateTime(toDate);
                supplierSettlementModel.WebsiteOwner = websiteOwner;
                supplierSettlementModel.InsertDate = DateTime.Now;
                supplierSettlementModel.SettlementId = settlementId;
                supplierSettlementModel.Status = "待供应商确认";
                supplierSettlementModel.SupplierName = supplier.Company;
                supplierSettlementModel.SupplierUserId = supplier.UserID;
                supplierSettlementModel.TotalBaseAmount = suplierSettlementDetailList.Sum(p => p.BaseAmount);
                supplierSettlementModel.TotalTransportFee = suplierSettlementDetailList.Sum(p => p.TransportFee);
                supplierSettlementModel.RefundTotalAmount = suplierSettlementDetailList.Sum(p => p.RefundAmount);
                supplierSettlementModel.SettlementTotalAmount = supplierSettlementModel.TotalBaseAmount + supplierSettlementModel.TotalTransportFee;
                supplierSettlementModel.SaleTotalAmount = orderList.Sum(p => p.TotalAmount);
                supplierSettlementModel.ServerTotalAmount = supplierSettlementModel.SaleTotalAmount - supplierSettlementModel.TotalBaseAmount;
                #endregion

                #region 未结算订单
                StringBuilder sbWhereUnSett = new StringBuilder();

                //sbWhereUnSett.AppendFormat(" WebsiteOwner='{0}' And TotalAmount>0 And PaymentStatus=1 And Status In('待发货','已发货')  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0 And SupplierUserId='{3}' And ( (Select Count(*) from ZCJ_WXMallRefund Where ZCJ_WXMallRefund.OrderId=OrderId)>0 ) And (Select Count(*) from ZCJ_ProjectCommission Where ZCJ_ProjectCommission.ProjectId=OrderId)=0 ", websiteOwner, fromDate, toDate, supplier.UserID);


                sbWhereUnSett.AppendFormat(" WebsiteOwner='{0}' And TotalAmount>0 And PaymentStatus=1 And Status In('待发货','已发货')  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0 And SupplierUserId='{3}' ", websiteOwner, fromDate, toDate, supplier.UserID);//待发货,已发货的订单

                StringBuilder sbWhereUnSett2 = new StringBuilder();

                sbWhereUnSett2.AppendFormat(" WebsiteOwner='{0}' And TotalAmount>0 And PaymentStatus=1 And Status In('待发货','已发货','交易成功')  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0 And SupplierUserId='{3}' And OrderId  In  (Select OrderID from ZCJ_WXMallRefund Where ZCJ_WXMallRefund.OrderId=OrderId)", websiteOwner, fromDate, toDate, supplier.UserID);//退款的订单

                StringBuilder sbWhereUnSett3 = new StringBuilder();

                sbWhereUnSett3.AppendFormat(" WebsiteOwner='{0}' And TotalAmount>0 And PaymentStatus=1 And Status In('交易成功')  And OrderType In(0,1,2) And ( InsertDate >= '{1}' And  InsertDate<='{2}') And IsNull(IsMain,0)=0 And SupplierUserId='{3}' And OrderId Not In (Select ProjectId from ZCJ_ProjectCommission Where ZCJ_ProjectCommission.ProjectId=OrderId)", websiteOwner, fromDate, toDate, supplier.UserID);//交易成功未分佣的订单

                List<WXMallOrderInfo> UnSettemorderList = GetList<WXMallOrderInfo>(sbWhereUnSett.ToString());
                List<WXMallOrderInfo> UnSettemorderList2 = GetList<WXMallOrderInfo>(sbWhereUnSett2.ToString());
                List<WXMallOrderInfo> UnSettemorderList3 = GetList<WXMallOrderInfo>(sbWhereUnSett3.ToString());

                UnSettemorderList.AddRange(UnSettemorderList2);
                UnSettemorderList.AddRange(UnSettemorderList3);
                UnSettemorderList = UnSettemorderList.DistinctBy(p => p.OrderID).ToList();
                foreach (var order in UnSettemorderList)
                {

                    List<WXMallOrderDetailsInfo> orderDetailList = GetList<WXMallOrderDetailsInfo>(string.Format("OrderId='{0}'", order.OrderID));
                    SupplierUnSettlement model = new SupplierUnSettlement();
                    model.WebsiteOwner = order.WebsiteOwner;
                    model.InsertDate = DateTime.Now;
                    model.BaseAmount = orderDetailList.Sum(p => p.BasePrice * p.TotalCount);
                    model.IsRefund = 0;
                    if (GetCount<WXMallRefund>(string.Format(" OrderId='{0}'", order.OrderID)) > 0)
                    {
                        model.IsRefund = 1;
                    }
                    model.OrderDate = order.InsertDate;
                    model.OrderId = order.OrderID;
                    model.OrderStatus = order.Status;
                    model.SaleAmount = order.TotalAmount;
                    model.ServerAmount = order.TotalAmount - model.BaseAmount;
                    model.SupplierName = supplier.Company;
                    model.SupplierUserId = supplier.UserID;
                    model.TransportFee = order.Transport_Fee;
                    model.SettlementAmount = model.BaseAmount = model.TransportFee;
                    suplierUnSettlementList.Add(model);


                }
                #endregion

                #region 提交事务
                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                try
                {

                    if (!AddList<SupplierSettlementDetail>(suplierSettlementDetailList))
                    {
                        tran.Rollback();
                    }
                    if (!(Add(supplierSettlementModel)))
                    {
                        tran.Rollback();
                    }
                    if (suplierUnSettlementList.Count > 0)
                    {

                        if (!AddList<SupplierUnSettlement>(suplierUnSettlementList))
                        {
                            tran.Rollback();
                        }
                    }

                    #region 结算单已经生成把历史可以结算订单的标识去掉
                    foreach (var order in orderListCanSettHistory)
                    {
                        if (Update(order, string.Format(" IsCanSettlement=''"), string.Format(" OrderId='{0}'", order.OrderID)) < 0)
                        {

                            tran.Rollback();
                            return false;
                        }
                    }
                    #endregion

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.ToString();
                    return false;

                }
                #endregion

            }
            return true;
        }


        /// <summary>
        /// 供应商渠道结算
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <param name="fromDate">开始日期</param>
        /// <param name="toDate">结束日期</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool SupplierChannelSettlement(string websiteOwner, string fromDate, string toDate, out string msg)
        {
            msg = "";
            int totalCount;
            var supplierChannelList = bllUser.GetSupplierChannelList(websiteOwner, 1, int.MaxValue, "", "", out totalCount);
            foreach (var item in supplierChannelList)
            {

                string settlementId = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), item.AutoID);

                ZentCloud.BLLJIMP.Model.SupplierChannelSettlement model = new Model.SupplierChannelSettlement();
                model.WebsiteOwner = websiteOwner;
                model.InsertDate = DateTime.Now;
                model.FromDate = Convert.ToDateTime(fromDate);
                model.ToDate = Convert.ToDateTime(toDate);
                model.SettlementId = settlementId;
                model.UserId = item.UserID;
                model.ChannelName = item.ChannelName;
                model.Status = "待商城确认";
                string sql = string.Format("select sum(Amount) from ZCJ_ProjectCommission where ProjectType='DistributionOnLineSupplierChannel' And WebsiteOwner='{0}' And UserId='{1}' And  InsertDate>='{2}' And InsertDate<='{3}'; ", websiteOwner, item.UserID, fromDate, toDate);
                var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
                if (result != null)
                {
                    model.SettlementTotalAmount = decimal.Parse(result.ToString());
                }
                Add(model);





            }
            return true;
        }

        /// <summary>
        /// 供应商结算
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool SupplierSettlementTask(TimingTask task)
        {


            foreach (var website in GetList<WebsiteInfo>(""))
            {
                string msg = "";
                SupplierSettlement(website.WebsiteOwner, ((DateTime)task.FromDate).ToString(), ((DateTime)task.ToDate).ToString(), out msg);

            }

            return true;
        }
        /// <summary>
        /// 供应商渠道结算
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool SupplierChannelSettlementTask(TimingTask task)
        {


            foreach (var website in GetList<WebsiteInfo>(""))
            {
                string msg = "";
                SupplierChannelSettlement(website.WebsiteOwner, ((DateTime)task.FromDate).ToString(), ((DateTime)task.ToDate).ToString(), out msg);

            }

            return true;
        }
        /// <summary>
        /// 自动好评
        /// </summary>
        public void TimingOrderAutoComment()
        {

            BLLReview bllReview = new BLLReview();

            foreach (var item in GetList<WebsiteInfo>())
            {

                if (item.IsOrderAutoComment == 0 || string.IsNullOrEmpty(item.OrderAutoCommentContent)) continue;

                var orderList = GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND Status='{1}' ", item.WebsiteOwner, "交易成功"));

                foreach (var order in orderList)
                {
                    if (!order.ReceivingTime.HasValue) continue;

                    var time = order.ReceivingTime.Value.AddDays(item.OrderAutoCommentDay);

                    if (order.ReceivingTime.Value.AddDays(item.OrderAutoCommentDay) > DateTime.Now) continue;

                    var orderDetailList = GetOrderDetail(order.OrderID);

                    foreach (var orderDetail in orderDetailList)
                    {
                        var reviewInfo = bllReview.Get<ReviewInfo>(string.Format(" WebsiteOwner='{0}' AND ForeignkeyId='{1}' AND Expand1='{2}' AND UserId='{3}' ", order.WebsiteOwner, orderDetail.OrderID, orderDetail.PID, order.OrderUserID));

                        if (reviewInfo != null) continue;

                        ReviewInfo review = new ReviewInfo();
                        review.AuditStatus = 1;
                        review.ReviewScore = 5;
                        review.ForeignkeyId = order.OrderID;
                        review.Expand1 = orderDetail.PID;
                        review.UserId = order.OrderUserID;
                        review.UserName = bllUser.GetUserDispalyName(order.OrderUserID);
                        review.ReviewContent = CommonPlatform.Helper.StringHandler.RandomStrArray(item.OrderAutoCommentContent, '；');
                        review.InsertDate = DateTime.Now;
                        review.ReviewType = "OrderComment";
                        review.ReviewTitle = "系统自动好评";
                        review.WebsiteOwner = order.WebsiteOwner;
                        review.ReviewMainId = int.Parse(bllReview.GetGUID(TransacType.CommAdd));
                        review.Ex2 = orderDetail.AutoID.ToString();
                        if (!Add(review)) continue;
                    }


                }
            }
        }

        /// <summary>
        /// 批量发货
        /// </summary>
        /// <param name="dt">上传excel转的表格</param>
        /// <param name="successCount">成功数量</param>
        /// <param name="failCount">失败数量</param>
        /// <param name="msg">提示信息</param>
        public void BatchDeliver(DataTable dt, out int successCount, out int failCount, out string msg)
        {

            successCount = 0;
            failCount = 0;
            msg = "";
            foreach (DataRow item in dt.Rows)
            {
                string orderId = item[0].ToString();
                string expressCompanyName = item[1].ToString();
                string expressNumber = item[2].ToString();
                if (!string.IsNullOrEmpty(orderId))
                {
                    var orderInfo = GetOrderInfo(orderId);
                    if (orderInfo != null && orderInfo.WebsiteOwner == WebsiteOwner)
                    {
                        var currentUserInfo = GetCurrentUserInfo();
                        if (currentUserInfo.UserType == 7 && (orderInfo.SupplierUserId != currentUserInfo.UserID))
                        {
                            failCount++;
                            msg += string.Format("【订单号:{0}发货失败,原因:订单号错误】", orderId);
                            continue;
                        }
                        if (orderInfo.PaymentStatus == 0)
                        {
                            failCount++;
                            msg += string.Format("【订单号:{0}发货失败,原因:未付款】", orderId);
                            continue;
                        }
                        if (orderInfo.Status == "待发货")
                        {
                            if (orderInfo.IsNoExpress == 0)
                            {
                                var expressInfo = Get<ZentCloud.BLLJIMP.ModelGen.ExpressInfo>(string.Format("ExpressCompanyName='{0}'", expressCompanyName));
                                if (expressInfo != null)
                                {
                                    if (!string.IsNullOrEmpty(expressNumber))
                                    {
                                        orderInfo.ExpressCompanyCode = expressInfo.ExpressCompanyCode;
                                        orderInfo.ExpressCompanyName = expressCompanyName;
                                        orderInfo.ExpressNumber = expressNumber;
                                        orderInfo.DeliveryTime = DateTime.Now;
                                        orderInfo.Status = "已发货";
                                        orderInfo.LastUpdateTime = DateTime.Now;

                                        if (Update(orderInfo))
                                        {
                                            successCount++;

                                        }
                                        else
                                        {
                                            failCount++;
                                            msg += string.Format("【订单号:{0}发货失败,原因:更新数据失败】", orderId);

                                        }
                                    }
                                    else
                                    {
                                        failCount++;
                                        msg += string.Format("【订单号:{0}发货失败,原因:未填写快递单号】", orderId);

                                    }

                                }
                                else
                                {
                                    failCount++;
                                    msg += string.Format("【订单号:{0}发货失败,原因:快递名称错误】", orderId);

                                }



                            }
                            else
                            {
                                failCount++;
                                msg += string.Format("【订单号:{0}发货失败,原因:无需物流】", orderId);

                            }
                        }
                        else
                        {
                            failCount++;
                            msg += string.Format("【订单号:{0}发货失败,原因:不是待发货状态】", orderId);
                        }


                    }
                    else
                    {
                        failCount++;
                        msg += string.Format("【订单号:{0}发货失败,原因:订单号错误】", orderId);

                    }

                }
                else
                {
                    failCount++;
                    msg += string.Format("【订单号:{0}发货失败,原因:订单号未填】", "无");

                }


            }



        }

        //private void WriteLog(string msg) {

        //    using (StreamWriter sw = new StreamWriter(@"D:\log\devlog.txt", true, Encoding.GetEncoding("gb2312")))
        //    {
        //        sw.WriteLine(string.Format("商品统计异常{0}", msg));
        //    }

        //}
        /// <summary>
        /// 手动添加订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool AddOrder(HttpContext context, out string msg)
        {

            msg = "添加失败";
            OrderManualModel orderRequestModel;
            try
            {
                orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderManualModel>(context.Request["data"]);
            }
            catch (Exception ex)
            {
                msg = "输入数据不完整,请检查";
                return false;

            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                WXMallOrderInfo orderInfo = new WXMallOrderInfo();

                List<string> orderSourceList = new List<string>();
                orderSourceList.Add("Eleme");
                orderSourceList.Add("Meituan");
                orderSourceList.Add("Baidu");
                orderSourceList.Add("DazhongDianPing");
                orderSourceList.Add("Comeoncloud");
                if (!orderSourceList.Contains(orderRequestModel.order_source))
                {
                    orderInfo.OrderType = 0;
                    orderInfo.SupplierUserId = orderRequestModel.order_source;
                    if (!string.IsNullOrEmpty(orderInfo.SupplierUserId))
                    {
                        var supplierInfo = bllUser.GetUserInfo(orderInfo.SupplierUserId, orderInfo.WebsiteOwner);
                        if (supplierInfo != null)
                        {
                            orderInfo.SupplierName = supplierInfo.Company;
                        }
                    }



                }
                else
                {
                    orderInfo.OrderType = 8;
                    orderInfo.TakeOutType = orderRequestModel.order_source;
                }
                orderInfo.OrderID = GetGUID(TransacType.CommAdd);
                orderInfo.Status = orderRequestModel.order_status;
                orderInfo.WebsiteOwner = WebsiteOwner;
                orderInfo.OrderUserID = GetCurrUserID();
                orderInfo.OutOrderId = orderRequestModel.out_order_id;
                orderInfo.InsertDate = Convert.ToDateTime(orderRequestModel.insert_date);
                orderInfo.PaymentType = orderRequestModel.pay_type;
                orderInfo.PaymentStatus = orderRequestModel.pay_status;
                if (!string.IsNullOrEmpty(orderRequestModel.pay_time))
                {
                    orderInfo.PayTime = Convert.ToDateTime(orderRequestModel.pay_time);
                }
                orderInfo.TotalAmount = orderRequestModel.total_amount;
                orderInfo.UseScore = orderRequestModel.use_score;
                orderInfo.UseAmount = orderRequestModel.use_amount;
                orderInfo.Ex1 = orderRequestModel.card_coupon_name;
                if (orderRequestModel.develive_type == "1")
                {
                    orderInfo.IsNoExpress = 1;
                }
                orderInfo.Address = orderRequestModel.receiver_address;
                orderInfo.Consignee = orderRequestModel.receiver_name;
                orderInfo.Phone = orderRequestModel.receiver_phone;

                orderInfo.CustomCreaterName = orderRequestModel.buy_name;
                orderInfo.CustomCreaterPhone = orderRequestModel.buy_phone;
                orderInfo.OrderMemo = orderRequestModel.buyer_memo;
                orderInfo.Transport_Fee = orderRequestModel.freight;



                List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();
                foreach (var item in orderRequestModel.product_list)
                {
                    WXMallOrderDetailsInfo detail = new WXMallOrderDetailsInfo();
                    detail.OrderID = orderInfo.OrderID;
                    detail.ProductImage = "";
                    detail.ProductName = item.product_name;
                    detail.TotalCount = item.product_count;
                    detail.OrderPrice = item.product_price;
                    detail.PID = "0";
                    detailList.Add(detail);

                }

                if (Add(orderInfo, tran) && AddList<WXMallOrderDetailsInfo>(detailList))
                {
                    tran.Commit();
                    return true;

                }


            }
            catch (Exception ex)
            {

                tran.Rollback();
                msg = ex.Message;
            }
            return false;


        }

        /// <summary>
        /// 获取绑定的门店
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetBindStoreAddress(UserInfo userInfo) {
            string address = "";

            if (!string.IsNullOrEmpty(userInfo.BindId))
            {
                JuActivityInfo juAc = Get<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' And K5='{1}'",WebsiteOwner,userInfo.BindId));
                if (juAc!=null)
                {
                    address = juAc.City;
                    address = juAc.ActivityAddress;
                    if (address.Length>=5)
                    {
                        address = address.Substring(0,5)+"...";
                    }
                }
            }


            return address;
        


        
        }
        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JuActivityInfo GetStoreInfo(string id) {

            var storeInfo = Get<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' And ArticleType='Outlets' And K5='{1}'", WebsiteOwner, id));
            if (storeInfo==null)
            {
                storeInfo = new JuActivityInfo();
            }
            return storeInfo;
        
        }


    }
}
