using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{

    /// <summary>
    /// 分销处理文件
    /// </summary>
    public class DistributionHandler : BaseHandler
    {
        /// <summary>
        ///分销 BLL
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 站点BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ApplyWithrawCash(HttpContext context)
        {
            string bankCardId = context.Request["BankCardId"];
            string amount = context.Request["Amount"];
            string type = context.Request["type"];//到账方式 0银行卡 1微信 2账户余额
            string msg = "";
            decimal mount = 0;

            CompanyWebsite_Config config = bllWebsite.GetCompanyWebsiteConfig();
            if (!decimal.TryParse(amount, out mount))
            {
                resp.Msg = "提现金额为整数";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (config.LowestAmount > 0)
            {
                if (mount < config.LowestAmount)
                {
                    resp.Msg = "最低提现金额为：" + config.LowestAmount;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            var currentUserInfo = bllUser.GetCurrentUserInfo();
            if (context.Request["ischannel"] != null && context.Request["ischannel"].ToString() == "1")
            {
                UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", currentUserInfo.UserID));
                if (channelUserInfo != null)
                {
                    currentUserInfo = channelUserInfo;
                    currentUserInfo.TrueName = channelUserInfo.ChannelName;
                }

            }
            if (bllDis.ApplyWithrawCash(currentUserInfo, bankCardId, amount, currentUserInfo.WebsiteOwner, int.Parse(type), out msg))
            {
                resp.Status = 1;
            }
            resp.Msg = msg;
            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 获取我的银行卡
        /// </summary>
        /// <returns></returns>
        private string GetMyBankCard(HttpContext context)
        {
            var currentUserInfo = bllDis.GetCurrentUserInfo();
            if (context.Request["ischannel"] != null && context.Request["ischannel"].ToString() == "1")
            {
                UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", currentUserInfo.UserID));
                if (channelUserInfo != null)
                {
                    currentUserInfo = channelUserInfo;
                }

            }
            string websiteOwner = bllDis.WebsiteOwner;
            var data = bllDis.GetList<BindBankCard>(string.Format("UserId='{0}'  And WebsiteOwner='{1}' ", currentUserInfo.UserID, websiteOwner));
            foreach (var item in data)
            {
                item.BankAccount = bllDis.HidePartialString(2, 4, item.BankAccount);
            }
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddBankCard(HttpContext context)
        {

            BindBankCard reqModel = bllDis.ConvertRequestToModel<BindBankCard>(new BindBankCard());
            reqModel.InsertDate = DateTime.Now;
            reqModel.UserId = bllDis.GetCurrentUserInfo().UserID;
            reqModel.WebsiteOwner = bllDis.WebsiteOwner;
            var currentUserInfo = bllDis.GetCurrentUserInfo();
            if (context.Request["ischannel"] != null && context.Request["ischannel"].ToString() == "1")
            {
                UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", currentUserInfo.UserID));
                if (channelUserInfo != null)
                {
                    reqModel.UserId = channelUserInfo.UserID;
                }

            }
            if (bllDis.Get<BindBankCard>(string.Format("UserId='{0}' And BankAccount='{1}' And WebsiteOwner='{2}' ", reqModel.UserId, reqModel.BankAccount, reqModel.WebsiteOwner)) != null)
            {
                resp.Msg = "银行卡号重复,请检查";
                goto outoff;
            }
            if (bllDis.Add(reqModel))
            {
                resp.Status = 1;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑银行卡
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditBankCard(HttpContext context)
        {

            BindBankCard reqModel = bllDis.ConvertRequestToModel<BindBankCard>(new BindBankCard());
            string websiteOwner = bllDis.WebsiteOwner;
            BindBankCard record = bllDis.Get<BindBankCard>(string.Format("AutoID={0} And UserId='{1}' And WebsiteOwner='{2}' ", reqModel.AutoID, bllUser.GetCurrUserID(), websiteOwner));
            record.BankAccount = reqModel.BankAccount;
            record.BankName = reqModel.BankName;
            record.AccountName = reqModel.AccountName;
            if (bllDis.Update(record))
            {
                resp.Status = 1;
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 删除银行卡
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelBankCard(HttpContext context)
        {
            string id = context.Request["AutoID"];
            if (bllDis.DeleteByKey<BindBankCard>("AutoID", id) > 0)
            {
                resp.Status = 1;
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 获取我的提现记录
        /// </summary>
        /// <returns></returns>
        private string QueryMyWithdrawRecord(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);//第几页
            int pageSize = int.Parse(context.Request["PageSize"]);//每页记录数
            string status = context.Request["Status"];
            var currentUserInfo = bllUser.GetCurrentUserInfo();
            if (context.Request["ischannel"] != null && context.Request["ischannel"].ToString() == "1")
            {
                UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", currentUserInfo.UserID));
                if (channelUserInfo != null)
                {
                    currentUserInfo = channelUserInfo;
                }

            }

            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("UserId='{0}'", currentUserInfo.UserID));
            if (status.Equals("1"))
            {
                sbWhere.AppendFormat("And Status=2");
            }
            else
            {
                sbWhere.AppendFormat("And Status!=2");
            }
            var data = bllDis.GetLit<WithdrawCash>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            foreach (var item in data)
            {
                item.BankAccount = bllDis.HidePartialString(2, 4, item.BankAccount);
                item.AccountBranchCity = null;
                item.AccountBranchName = null;
                item.AccountBranchProvince = null;
                item.AccountName = null;
                item.BankName = null;
                item.TrueName = null;
            }
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 获取我的下级会员
        /// </summary>
        /// <returns></returns>
        private string QueryMyMember(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);//第几页
            int pageSize = int.Parse(context.Request["PageSize"]);//每页记录数
            int level = int.Parse(context.Request["Level"]);
            string isPay = context.Request["ispay"];
            List<UserInfo> data = bllDis.GetDownUserList(currUserInfo.UserID, level);
            string autoId = context.Request["AutoId"];

            if (!string.IsNullOrEmpty(autoId) && (autoId != "0"))
            {
                data = bllDis.GetDownUserList(bllUser.GetUserInfoByAutoID(int.Parse(autoId)).UserID, level);
            }
            if (!string.IsNullOrEmpty(isPay))
            {
                if (isPay == "1")
                {
                    data = data.Where(p => p.DistributionSaleAmountLevel0 > 0).ToList();
                }
                else
                {
                    data = data.Where(p => p.DistributionSaleAmountLevel0 == 0).ToList();
                }

            }
            data = data.OrderByDescending(p => p.Regtime).ToList();
            var newData = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (var item in newData)
            {
                item.Regtime = bllDis.GetUserDistributionRegTime(item);

            }
            resp.ExObj = newData;
            resp.ExInt = data.Count;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取渠道的所有会员
        /// </summary>
        /// <returns></returns>
        private string QueryMyMemberChannel(HttpContext context)
        {
            try
            {


                int pageIndex = int.Parse(context.Request["PageIndex"]);//第几页
                int pageSize = int.Parse(context.Request["PageSize"]);//每页记录数
                List<UserInfo> data = new List<UserInfo>();
                string autoId = context.Request["AutoId"];
                string level = context.Request["level"];//是否显示直接会员
                if (!string.IsNullOrEmpty(autoId) && (currUserInfo.UserID == bllUser.WebsiteOwner || currUserInfo.UserType == 1) && (autoId != "0"))
                {
                    var user = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
                    if ((!string.IsNullOrEmpty(level)) && (level == "1"))
                    {
                        data = bllDis.GetChannelAllFirstLevelChildUser(user.UserID, user.WebsiteOwner);
                    }
                    else
                    {

                        data = bllDis.GetChannelAllChildUser(user.UserID);

                    }

                }
                else
                {
                    string channelUserId = bllDis.GetChannelUserId(currUserInfo);
                    data = bllDis.GetChannelAllChildUser(channelUserId);

                }
                var newData = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                for (int i = 0; i < newData.Count; i++)
                {
                    try
                    {
                        newData[i] = bllUser.GetUserInfo(newData[i].UserID);
                        newData[i].Regtime = bllDis.GetUserDistributionRegTime(newData[i]);

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
                resp.ExObj = newData;
                resp.ExInt = data.Count;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        /// <summary>
        /// 获取我的下级订单
        /// </summary>
        /// <returns></returns>
        private string QueryMyOrder(HttpContext context)
        {
            try
            {


                int pageIndex = int.Parse(context.Request["PageIndex"]);//第几页
                int pageSize = int.Parse(context.Request["PageSize"]);//每页记录数
                int level = int.Parse(context.Request["Level"]);//显示几级
                int status = int.Parse(context.Request["Status"]);//分销订单状态
                List<WXMallOrderInfo> sourceData = new List<WXMallOrderInfo>();
                switch (status)
                {
                    case 0:
                        sourceData = bllDis.GetOrderList(currUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.NotPay);
                        break;
                    case 1:
                        sourceData = bllDis.GetOrderList(currUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Paied);
                        break;
                    case 2:
                        sourceData = bllDis.GetOrderList(currUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Received);
                        break;
                    case 3:
                        sourceData = bllDis.GetOrderList(currUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Verified);
                        break;
                    case 4:
                        sourceData = bllDis.GetOrderList(currUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Withdraw);
                        break;
                    default:
                        break;
                }
                if (level == 0)
                {
                    sourceData = sourceData.Where(p => p.OrderUserID == currUserInfo.UserID).ToList();

                }
                else
                {
                    sourceData = sourceData.Where(p => p.OrderUserID != currUserInfo.UserID).ToList();

                }

                WebsiteInfo websiteInfo = bllDis.GetWebsiteInfoModelFromDataBase();
                List<OrderModel> orderList = new List<OrderModel>();
                sourceData = sourceData.OrderByDescending(p => p.InsertDate).ToList();
                var data = sourceData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                foreach (WXMallOrderInfo item in data)
                {

                    OrderModel order = new OrderModel();
                    #region 分销提成比例
                    #region 已分佣 提成比例及金额根据提成记录
                    if (item.DistributionStatus == 3)//已打佣金,分佣金额根据记录表去查
                    {
                        ProjectCommission projectCommission = bllDis.Get<ProjectCommission>(string.Format(" ProjectId='{0}' And UserId='{1}' And WebsiteOwner='{2}' And ProjectType='DistributionOnLine'", item.OrderID, currUserInfo.UserID, bllDis.WebsiteOwner));
                        if (projectCommission != null)
                        {
                            order.DistributionAmount = projectCommission.Amount;
                            order.DistributionRate = projectCommission.Rate.ToString();
                            order.TotalAmount = item.TotalAmount - item.Transport_Fee;
                            if (order.TotalAmount * (decimal.Parse(order.DistributionRate) / 100) != order.DistributionAmount)
                            {
                                order.DistributionRate = (Math.Round((order.DistributionAmount / order.TotalAmount), 3) * 100).ToString();
                            }

                        }
                        else
                        {
                            order.DistributionAmount = 0;
                            order.DistributionRate = "0";

                        }
                    }
                    #endregion

                    #region 还未分佣，根据预估分佣表
                    else
                    {

                        if (websiteInfo.IsDisabledCommission == 0)//分佣 
                        {

                            // UserLevelConfig userLevel = bllDis.GetUserLevel(currUserInfo);
                            //bool isFirst = bllDis.IsFirstOrder(item);

                            switch (level)
                            {
                                case 0:
                                    //decimal rate = (decimal)bllDis.GetDistributionRate(currUserInfo, 0, isFirst);//直销提成比例
                                    //order.DistributionAmount = bllDis.GetUserCommission(item, currUserInfo, 0);
                                    //order.DistributionRate = rate.ToString();
                                    ProjectCommissionEstimate esti = bllDis.Get<ProjectCommissionEstimate>(string.Format("ProjectId={0} And ProjectType='DistributionOnLine' And UserId='{1}' ", item.OrderID, currUserInfo.UserID));
                                    if (esti != null)
                                    {
                                        order.DistributionAmount = esti.Amount;
                                        order.DistributionRate = esti.Rate.ToString();
                                    }
                                    else
                                    {
                                        order.DistributionRate = "0";
                                    }
                                    break;
                                case 1:
                                   // var upUserLevel1 = bllDis.GetUpUser(item.OrderUserID, 1);
                                    //var orderUser = bllUser.GetUserInfo(item.OrderUserID);
                                   // decimal rateLevel1 = (decimal)bllDis.GetDistributionRate(currUserInfo, 1, isFirst);
                                    //order.DistributionAmount = bllDis.GetUserCommission(item, currUserInfo, 1);
                                   // order.DistributionRate = rateLevel1.ToString();

                                    ProjectCommissionEstimate estiUpLevel = bllDis.Get<ProjectCommissionEstimate>(string.Format("ProjectId={0} And ProjectType='DistributionOnLine' And UserId='{1}' ", item.OrderID, currUserInfo.UserID));
                                    if (estiUpLevel != null)
                                    {
                                        order.DistributionAmount = estiUpLevel.Amount;
                                        order.DistributionRate = estiUpLevel.Rate.ToString();
                                    }
                                    else
                                    {
                                        order.DistributionRate = "0";
                                    }
                                    break;
                                //case 2:
                                //    order.DistributionAmount = (decimal.Parse(userLevel.DistributionRateLevel2) / 100) * (item.TotalAmount - item.Transport_Fee);
                                //    order.DistributionRate = userLevel.DistributionRateLevel2;
                                //    break;
                                //case 3: 
                                //    order.DistributionAmount = (decimal.Parse(userLevel.DistributionRateLevel3) / 100) * item.TotalAmount;
                                //    order.DistributionRate = userLevel.DistributionRateLevel3;
                                //    break;
                                default:
                                    break;



                            }
                            //order.DistributionAmount = Math.Round(order.DistributionAmount, 2);




                        }
                        else//不分佣
                        {
                            order.DistributionAmount = 0;
                            order.DistributionRate = "0";

                        }


                    }
                    #endregion
                    #endregion

                    order.DistributionStatus = item.DistributionStatus;
                    if (item.IsRefund == 1)
                    {
                        order.DistributionStatus = -1;
                    }
                    order.InsertDate = item.InsertDate.ToString("yyyy-MM-dd");
                    order.OrderID = item.OrderID;
                    order.ProductCount = item.ProductCount;
                    order.TotalAmount = item.TotalAmount - item.Transport_Fee;
                    order.ProductList = new List<WXMallProductInfo>();
                    //order.WXNickName = bllUser.GetUserInfo(item.OrderUserID).WXNickname;
                    UserInfo orderUserInfo = bllUser.GetUserInfo(item.OrderUserID);
                    if (orderUserInfo != null)
                    {
                        order.TrueName = orderUserInfo.TrueName;
                        order.Phone = orderUserInfo.Phone;
                        order.AutoID = orderUserInfo.AutoID;
                        order.WXNickName = orderUserInfo.WXNickname;
                    }
                    foreach (var orderdetail in bllMall.GetOrderDetailsList(item.OrderID))
                    {
                        var productInfo = bllMall.GetProduct(orderdetail.PID);
                        if (productInfo != null)
                        {
                            productInfo.PDescription = null;
                            productInfo.CategoryId = null;
                            productInfo.IP = 0;
                            productInfo.UserID = null;
                            productInfo.WebsiteOwner = null;
                            productInfo.Price = (decimal)orderdetail.OrderPrice;
                            productInfo.Stock = orderdetail.TotalCount;
                            order.ProductList.Add(productInfo);
                        }

                    }

                    order.OrderType = item.OrderType;
                    if (!string.IsNullOrEmpty(order.Remark))
                    {
                        order.Remark = item.OrderMemo.Replace("购买选项:", "<br/>购买选项:<br/>").Replace("金额:", "<br/>金额:");

                    }
                    orderList.Add(order);

                }

                return Common.JSONHelper.ObjectToJson(orderList);
            }
            catch (Exception ex)
            {
                return ex.ToString();

            }
        }

        /// <summary>
        /// 获取渠道下级订单
        /// </summary>
        /// <returns></returns>
        private string QueryMyOrderChannel(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);//第几页
            int pageSize = int.Parse(context.Request["PageSize"]);//每页记录数
            int status = int.Parse(context.Request["Status"]);//分销订单状态
            string level = context.Request["level"];//1 直接销售
            string autoId = context.Request["autoId"];
            string channelUserId = bllDis.GetChannelUserId(currUserInfo);
            if (!string.IsNullOrEmpty(autoId) && (currUserInfo.UserID == bllUser.WebsiteOwner || currUserInfo.UserType == 1) && (autoId != "0"))
            {
                status = -1;
                pageSize = 20;
                channelUserId = bllUser.GetUserInfoByAutoID(int.Parse(autoId)).UserID;
            }
            var channelUserInfo = bllUser.GetUserInfo(channelUserId);
            List<WXMallOrderInfo> sourceData = new List<WXMallOrderInfo>();

            if (string.IsNullOrEmpty(level))
            {
                sourceData = bllDis.GetChannelAllOrder(channelUserId, bllUser.WebsiteOwner);
            }
            else if (level == "1")
            {
                sourceData = bllDis.GetChannelAllFirstLevelOrder(channelUserId, bllUser.WebsiteOwner);
            }
            if (status != -1)
            {
                sourceData = sourceData.Where(p => p.DistributionStatus == status).ToList();

            }
            WebsiteInfo websiteInfo = bllDis.GetWebsiteInfoModelFromDataBase();
            List<OrderModel> orderList = new List<OrderModel>();
            var data = sourceData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (WXMallOrderInfo item in data)
            {

                OrderModel order = new OrderModel();
                #region 分销提成比例
                #region 已分佣 提成比例及金额根据提成记录
                if (item.DistributionStatus == 3)//已打佣金,分佣金额根据记录表去查
                {
                    ProjectCommission projectCommission = bllDis.Get<ProjectCommission>(string.Format(" ProjectId='{0}' And UserId='{1}' And WebsiteOwner='{2}' ", item.OrderID, channelUserInfo.UserID, bllDis.WebsiteOwner));
                    if (projectCommission != null)
                    {
                        order.DistributionAmount = projectCommission.Amount;
                        order.DistributionRate = projectCommission.Rate.ToString();
                        order.TotalAmount = item.TotalAmount - item.Transport_Fee;
                        if (order.TotalAmount * (decimal.Parse(order.DistributionRate) / 100) != order.DistributionAmount)
                        {
                            order.DistributionRate = (Math.Round((order.DistributionAmount / order.TotalAmount), 3) * 100).ToString();
                        }
                    }
                    else
                    {
                        order.DistributionAmount = 0;
                        order.DistributionRate = "0";

                    }
                }
                #endregion

                #region 还未分佣，根据预估分佣表
                else
                {

                    if (websiteInfo.IsDisabledCommission == 0)
                    {
                        //decimal rate = (decimal)bllDis.GetDistributionRateChannel(channelUserInfo);
                        //order.DistributionAmount = (rate / 100) * (item.TotalAmount - item.Transport_Fee);
                        //order.DistributionRate = rate.ToString();
                        //order.DistributionAmount = Math.Round(order.DistributionAmount, 2);
                        //if (order.DistributionAmount < 0)
                        //{
                        //    order.DistributionAmount = 0;
                        //}
                        ProjectCommissionEstimate esti = bllDis.Get<ProjectCommissionEstimate>(string.Format("ProjectId={0} And ProjectType='DistributionOnLineChannel' And UserId='{1}' ", item.OrderID, channelUserInfo.UserID));
                        if (esti != null)
                        {
                            order.DistributionAmount = esti.Amount;
                            order.DistributionRate = esti.Rate.ToString();
                        }
                        else
                        {
                            order.DistributionRate = "0";
                        }


                    }
                    else
                    {
                        order.DistributionAmount = 0;
                        order.DistributionRate = "0";
                    }





                }
                #endregion
                #endregion

                order.DistributionStatus = item.DistributionStatus;
                if (item.IsRefund == 1)
                {
                    order.DistributionStatus = -1;
                }
                order.InsertDate = item.InsertDate.ToString("yyyy-MM-dd");
                order.OrderID = item.OrderID;
                order.ProductCount = item.ProductCount;
                order.TotalAmount = item.TotalAmount - item.Transport_Fee;
                order.ProductList = new List<WXMallProductInfo>();
                //order.WXNickName = bllUser.GetUserInfo(item.OrderUserID).WXNickname;
                UserInfo orderUserInfo = bllUser.GetUserInfo(item.OrderUserID);
                if (orderUserInfo != null)
                {
                    order.TrueName = orderUserInfo.TrueName;
                    order.Phone = orderUserInfo.Phone;
                    order.AutoID = orderUserInfo.AutoID;
                    order.WXNickName = orderUserInfo.WXNickname;
                }
                foreach (var orderdetail in bllMall.GetOrderDetailsList(item.OrderID))
                {
                    var productInfo = bllMall.GetProduct(orderdetail.PID);
                    if (productInfo != null)
                    {
                        productInfo.PDescription = null;
                        productInfo.CategoryId = null;
                        productInfo.IP = 0;
                        productInfo.UserID = null;
                        productInfo.WebsiteOwner = null;
                        productInfo.Price = (decimal)orderdetail.OrderPrice;
                        productInfo.Stock = orderdetail.TotalCount;
                        order.ProductList.Add(productInfo);
                    }

                }

                order.OrderType = item.OrderType;
                if (!string.IsNullOrEmpty(order.Remark))
                {
                    order.Remark = item.OrderMemo.Replace("购买选项:", "<br/>购买选项:<br/>").Replace("金额:", "<br/>金额:");

                }
                orderList.Add(order);

            }

            return Common.JSONHelper.ObjectToJson(orderList);
        }


        /// <summary>
        /// 我的订单模型
        /// </summary>
        public class OrderModel
        {
            /// <summary>
            /// 下单用户ID
            /// </summary>
            public int AutoID { get; set; }
            /// <summary>
            /// 下单用户的姓名
            /// </summary>
            public string TrueName { get; set; }
            /// <summary>
            /// 下单用户手机
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 微信昵称
            /// </summary>
            public string WXNickName { get; set; }
            /// <summary>
            /// 商品总数
            /// </summary>
            public int ProductCount { get; set; }
            /// <summary>
            /// 订单编号
            /// </summary>
            public string OrderID { get; set; }
            /// <summary>
            ///订单金额
            /// </summary>
            public decimal TotalAmount { get; set; }
            /// <summary>
            /// 订单日期
            /// </summary>
            public string InsertDate { get; set; }
            /// <summary>
            /// 分销订单状态
            /// </summary>
            public int DistributionStatus { get; set; }
            /// <summary>
            /// 提成比例
            /// </summary>
            public string DistributionRate { get; set; }
            /// <summary>
            /// 提成金额
            /// </summary>
            public decimal DistributionAmount { get; set; }
            /// <summary>
            /// 商品列表
            /// </summary>
            public List<WXMallProductInfo> ProductList { get; set; }
            /// <summary>
            /// 订单类型
            /// </summary>
            public int OrderType { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }


    }
}