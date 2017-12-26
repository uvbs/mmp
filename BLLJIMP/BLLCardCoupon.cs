using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 卡券业务逻辑
    /// </summary>
    public class BLLCardCoupon : BLL
    {

        /// <summary>
        /// 添加主卡券
        /// </summary>
        /// <param name="cardCoupons">主卡券实体</param>
        /// <returns></returns>
        public bool AddCardCoupon(CardCoupons cardCoupons)
        {
            cardCoupons.CardId = int.Parse(GetGUID(TransacType.AddCardCoupon));
            //if (ExistCardId(cardCoupons.CardId))
            //{
            //    return false;
            //}
            return Add(cardCoupons);

        }
        /// <summary>
        /// 卡券类型
        /// </summary>
        /// <param name="cardCoupons">主卡券实体</param>
        /// <param name="cardCouponType">卡券类型</param>
        /// <returns></returns>
        public bool AddCardCoupon(CardCoupons cardCoupons, EnumCardCouponType cardCouponType)
        {
            //if (ExistCardId(cardCoupons.CardId))
            //{
            //    return false;
            //}
            cardCoupons.CardCouponType = CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType);
            return Add(cardCoupons);

        }


        /// <summary>
        /// 是否存在主卡券编号
        /// </summary>
        /// <param name="cardId"></param>
        public bool ExistCardId(int cardId)
        {
            return Exists(new CardCoupons(), "CardId");

        }

        /// <summary>
        /// 获取 站点下主卡券列表
        /// </summary>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public List<CardCoupons> GetCardCouponList(EnumCardCouponType cardCouponType, int pageIndex, int pageSize, string where = "", string order = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  CardCouponType='{0}' And WebSiteOwner='{1}'", CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType), WebsiteOwner);
            if (!string.IsNullOrEmpty(where))
            {
                sbWhere.AppendFormat("And {0}", where);
            }
            if (!string.IsNullOrEmpty(order))
            {
                return GetLit<CardCoupons>(pageSize, pageIndex, sbWhere.ToString(), order);
            }
            return GetLit<CardCoupons>(pageSize, pageIndex, sbWhere.ToString());
        }



        /// <summary>
        /// 获取 站点下主卡券列表
        /// </summary>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public List<CardCoupons> GetCardCouponList(string cardCouponType, int pageIndex, int pageSize, out int totalCount, string order = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebSiteOwner='{0}'", WebsiteOwner);
            if (!string.IsNullOrEmpty(cardCouponType))
            {
                sbWhere.AppendFormat(" And CardCouponType='{0}'", cardCouponType);
            }
            totalCount = GetCount<CardCoupons>(sbWhere.ToString());
            if (!string.IsNullOrEmpty(order))
            {
                return GetLit<CardCoupons>(pageSize, pageIndex, sbWhere.ToString(), order);
            }
            return GetLit<CardCoupons>(pageSize, pageIndex, sbWhere.ToString(), " CardId DESC");
        }


        /// <summary>
        /// 获取卡券发放记录
        /// </summary>
        /// <param name="cardCoupononId">卡券ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="totalCount">总数量</param>
        /// <returns></returns>
        public List<MyCardCoupons> GetSendRecordList(int cardCoupononId, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebSiteOwner='{0}' And CardId={1}", WebsiteOwner, cardCoupononId);
            totalCount = GetCount<MyCardCoupons>(sbWhere.ToString());
            return GetLit<MyCardCoupons>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
        }

        /// <summary>
        /// 获取单个主卡券
        /// </summary>
        /// <param name="cardId">卡券编号</param>
        /// <returns></returns>
        public CardCoupons GetCardCoupon(EnumCardCouponType cardCouponType, int cardId)
        {

            return Get<CardCoupons>(string.Format(" CardId={0} And CardCouponType='{1}'", cardId, CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType)));
        }

        /// <summary>
        /// 获取单个主卡券
        /// </summary>
        /// <param name="cardId">卡券类型</param>
        /// <returns></returns>
        public CardCoupons GetCardCoupon(string cardCouponType, int cardId)
        {
            return Get<CardCoupons>(string.Format(" CardId={0} And CardCouponType='{1}'", cardId, cardCouponType));
        }
        /// <summary>
        /// 获取单个主卡券
        /// </summary>
        /// <param name="cardId">卡券类型</param>
        /// <returns></returns>
        public CardCoupons GetCardCoupon(int cardId)
        {
            return Get<CardCoupons>(string.Format(" CardId={0}", cardId));
        }
        /// <summary>
        /// 获取单个主卡券 根据微信卡券id
        /// </summary>
        /// <param name="wxCardId">微信卡券id</param>
        /// <returns></returns>
        public CardCoupons GetCardCouponByWXCardId(string  wxCardId)
        {
            return Get<CardCoupons>(string.Format(" WeixinCardId='{0}'", wxCardId));
        }
        /// <summary>
        /// 查询我的卡券
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="where">关键字查询</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public List<MyCardCoupons> GetMyCardCoupons(EnumCardCouponType cardCouponType, string userId, int pageIndex, int pageSize, out int totalCount, string where = "", string orderBy = "", string status = "")
        {
            totalCount = 0;
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  CardCouponType='{0}' And WebSiteOwner='{1}' And UserId='{2}'", CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType), WebsiteOwner, userId);
            //if (!string.IsNullOrEmpty(where))
            //{
            //    sbWhere.AppendFormat("And {0}", where);
            //}
            switch (status)
            {
                case "0":
                    sbWhere.AppendFormat(" And Status=0");
                    break;
                case "1":
                    sbWhere.AppendFormat(" And Status=1");
                    break;
                case "2":
                    sbWhere.AppendFormat(" And CardId in (Select CardId from ZCJ_CardCoupons where DateDiff(s,ValidTo,getdate())>0 ) ");
                    break;
                default:
                    break;
            }

            totalCount = GetCount<MyCardCoupons>(sbWhere.ToString());
            if (!string.IsNullOrEmpty(orderBy))
            {
                return GetLit<MyCardCoupons>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

            }
            return GetLit<MyCardCoupons>(pageSize, pageIndex, sbWhere.ToString());

        }
        /// <summary>
        /// 获取卡券数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="isCanUse"></param>
        /// <param name="cardCouponType"></param>
        /// <returns></returns>
        public int GetCardCouponCount(string websiteOwner, string userId = "", string status = "", string isCanUse = "", string cardCouponType = "")
        {
            //StringBuilder sbWhere = new StringBuilder();
            //sbWhere.AppendFormat(" WebSiteOwner='{0}' ", WebsiteOwner);
            //if (!string.IsNullOrEmpty(userId)) sbWhere.AppendFormat(" And UserId='{0}'", userId);
            //if (!string.IsNullOrEmpty(status)) sbWhere.AppendFormat(" And Status={0}", status);
            //if (!string.IsNullOrEmpty(cardCouponType)) sbWhere.AppendFormat(" And CardCouponType='{0}'", cardCouponType);
            //if (!string.IsNullOrEmpty(isCanUse) && isCanUse == "1")
            //{
            //    sbWhere.AppendFormat(" And (ToUserId Is NULL Or ToUserId='') ");
            //    sbWhere.AppendFormat(" And Status=0 ");
            //    string curtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    sbWhere.AppendFormat(" And EXISTS( ");
            //    sbWhere.AppendFormat(" SELECT 1 FROM ZCJ_CardCoupons WHERE ZCJ_MyCardCoupons.CardId = CardId ");
            //    sbWhere.AppendFormat(" And (ValidFrom Is Null Or ValidFrom < '{0}') ", curtime);
            //    sbWhere.AppendFormat(" And (ValidTo Is Null Or ValidTo > '{0}') ", curtime);

            //    sbWhere.AppendFormat(" ) ");
            //}
            //else if (!string.IsNullOrEmpty(status))
            //{
            //    sbWhere.AppendFormat(" And Status={0}", status);
            //}
            //return GetCount<MyCardCoupons>(sbWhere.ToString());



            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("SELECT COUNT(*)  FROM [dbo].[ZCJ_MyCardCoupons] ");
            sbWhere.AppendFormat("LEFT   JOIN [ZCJ_CardCoupons]  on [ZCJ_CardCoupons].[CardId] =[ZCJ_MyCardCoupons].[CardId] ");
            sbWhere.AppendFormat(" Where ");
            sbWhere.AppendFormat("[ZCJ_MyCardCoupons] .WebSiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrEmpty(userId))
            {

                sbWhere.AppendFormat(" And UserId='{0}'", userId);

            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status={0}", status);
            }
            if (!string.IsNullOrEmpty(cardCouponType))
            {
                sbWhere.AppendFormat(" And CardCouponType='{0}'", cardCouponType);
            }
            if (!string.IsNullOrEmpty(isCanUse) && isCanUse == "1")//可用的卡券
            {

                string curtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sbWhere.AppendFormat(" And Status=0  ");//未使用
                sbWhere.AppendFormat(" And (ToUserId Is NULL Or ToUserId='')");//无转赠
                sbWhere.AppendFormat(" And ("); //组合 主卡券指定的时间段+领取后未过期的

                //主卡券指定的时间段
                sbWhere.AppendFormat("(");
                sbWhere.AppendFormat("(ExpireTimeType='' or ExpireTimeType='0') And (ValidFrom Is Null Or ValidFrom < '{0}')", curtime);
                sbWhere.AppendFormat(" And (ValidTo Is Null Or ValidTo > '{0}')  ", curtime);
                sbWhere.AppendFormat(") ");
                //主卡券指定的时间段

                // 领取后几天未过期
                sbWhere.AppendFormat(" Or(");
                sbWhere.AppendFormat("(ExpireTimeType='1' And dateadd(day,Convert(int,ExpireDay),[ZCJ_MyCardCoupons] .InsertDate)>'{0}' )", curtime);
                sbWhere.AppendFormat(")");
                // 领取后几天未过期

                sbWhere.AppendFormat(")");

            }
            return (int)ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sbWhere.ToString());

        }

        /// <summary>
        /// 查询我的卡券
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="totalCount">总数</param>
        /// <param name="status">使用状态</param>
        /// <returns></returns>
        public List<MyCardCoupons> GetMyCardCoupons(string userId, int pageIndex, int pageSize, out int totalCount, string status = "", string isCanUse = "")
        {
            totalCount = 0;
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebSiteOwner='{0}' And UserId='{1}'", WebsiteOwner, userId);

            switch (status)
            {
                case "0":
                    sbWhere.AppendFormat(" And Status=0");
                    break;
                case "1":
                    sbWhere.AppendFormat(" And Status=1");
                    break;

                default:
                    break;
            }


            //switch (isCanUse)
            //{

            //    case "1":
            //        //sbWhere.AppendFormat(" And exists(select 'x' from ZCJ_CardCoupons where getdate()>=ValidFrom And getdate()<=ValidTo And ()) ");
            //        break;

            //    default:
            //        break;
            //}





            totalCount = GetCount<MyCardCoupons>(sbWhere.ToString());

            return GetLit<MyCardCoupons>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC ");

        }
        /// <summary>
        /// 获取我的单个卡券信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MyCardCoupons GetMyCardCoupon(int id, string userId)
        {

            return Get<MyCardCoupons>(string.Format(" AutoId={0} And UserId='{1}'", id, userId));


        }
        /// <summary>
        /// 获取单个卡券信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MyCardCoupons GetMyCardCoupon(int id)
        {

            return Get<MyCardCoupons>(string.Format(" AutoId={0} ", id));


        }
        /// <summary>
        /// 获取我的单个卡券信息根据主卡券ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MyCardCoupons GetMyCardCouponMainId(int cardCouponId, string userId)
        {

            return Get<MyCardCoupons>(string.Format(" CardId={0} And UserId='{1}'", cardCouponId, userId));


        }
        /// <summary>
        /// 获取已经发放的卡券数量
        /// </summary>
        /// <param name="cardCouponId"></param>
        /// <returns></returns>
        public int GetCardCouponSendCount(int cardCouponId)
        {
            return GetCount<MyCardCoupons>(string.Format(" CardId={0}", cardCouponId));

        }




        /// <summary>
        /// 使用我的卡券
        /// </summary>
        /// <param name="autoId">卡券自动编号（不是卡券号码）</param>
        /// <param name="msg">返回消息</param>
        /// <returns></returns>
        public bool UseMyCardCoupons(EnumCardCouponType cardCouponType, string userId, int autoId, out string msg)
        {
            msg = "";
            MyCardCoupons myCardCoupons = Get<MyCardCoupons>(string.Format("CardCouponType='{0}' And UserId='{1}' And AutoId={2}", CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType), userId, autoId));

            if (myCardCoupons == null)
            {
                msg = "卡券不存在";
                return false;

            }
            CardCoupons cardCoupons = GetCardCoupon(CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType), myCardCoupons.CardId);
            if (cardCoupons == null)
            {
                msg = "卡券不存在";
                return false;
            }
            if (myCardCoupons.Status == 1)
            {
                msg = "卡券已使用过了";
                return false;
            }
            if (cardCoupons.ValidFrom != null)
            {
                if (DateTime.Now < (DateTime)(cardCoupons.ValidFrom))
                {
                    msg = string.Format("请在{0}后使用", cardCoupons.ValidFrom.ToString());
                    return false;
                }
            }
            if (cardCoupons.ValidTo != null)
            {
                if (DateTime.Now > (DateTime)(cardCoupons.ValidTo))
                {
                    msg = "卡券已过期";
                    return false;
                }
            }
            myCardCoupons.Status = 1;
            myCardCoupons.UseDate = DateTime.Now;
            if (Update(myCardCoupons))
            {
                msg = "成功使用";
                return true;
            }
            return false;



        }


        /// <summary>
        /// 是否已经领取了卡券
        /// </summary>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="cardId">主卡券编号</param>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public bool IsReciveCoupon(EnumCardCouponType cardCouponType, int cardId, string userId)
        {


            int count = GetCount<MyCardCoupons>(string.Format("CardId={0} And CardCouponType='{1}' And UserId='{2}'", cardId, CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType), userId));
            if (count > 0)
            {
                return true;
            }

            return false;



        }
        /// <summary>
        /// 是否已经领取了卡券
        /// </summary>
        /// <param name="cardId">主卡券编号</param>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public int IsReciveCoupon(int cardId, string userId)
        {

            int count = GetCount<MyCardCoupons>(string.Format("CardId={0}  And UserId='{1}'", cardId, userId));
            if (count > 0)
            {
                return 1;
            }

            return 0;



        }

        /// <summary>
        /// 是否使用了卡券
        /// </summary>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="cardId">券编号</param>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public bool IsUseCoupon(EnumCardCouponType cardCouponType, int cardId, string userId)
        {
            int count = GetCount<MyCardCoupons>(string.Format("CardId={0} And CardCouponType='{1}' And UserId='{2}' And Status=1", cardId, CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType), userId));
            if (count > 0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 接收门票信息
        /// </summary>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="cardId">卡券编号</param>
        /// <param name="userId">用户名</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public bool ReciveCoupon(EnumCardCouponType cardCouponType, int cardId, string userId, out string msg)
        {
            msg = "";
            if (IsCardCouponExpire(cardCouponType, cardId))
            {
                msg = "卡券已经过期";
                return false;
            }
            if (IsReciveCoupon(cardCouponType, cardId, userId))
            {
                msg = "已经领取过了";
                return false;
            }
            switch (cardCouponType)
            {

                case EnumCardCouponType.EntranceTicket:
                    MyCardCoupons Model = new MyCardCoupons();
                    Model.CardCouponType = CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType);
                    Model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(11111, 99999).ToString());
                    Model.CardId = cardId;
                    Model.InsertDate = DateTime.Now;
                    Model.Status = 0;
                    Model.UserId = userId;
                    Model.WebSiteOwner = WebsiteOwner;
                    if (Add(Model))
                    {
                        msg = "领取成功";
                        return true;
                    }
                    else
                    {
                        msg = "领取失败";
                        return false;
                    }
                default:
                    break;
            }



            return false;

        }

        /// <summary>
        /// 接收门票信息 多个门票
        /// </summary>
        /// <param name="cardCouponType">卡券类型</param>
        /// <param name="cardId">卡券编号</param>
        /// <param name="userId">用户名</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public bool ReciveCoupons(EnumCardCouponType cardCouponType, int cardId, string userId, out string msg)
        {
            msg = "";
            if (IsCardCouponExpire(cardCouponType, cardId))
            {
                msg = "卡券已经过期";
                return false;
            }
            switch (cardCouponType)
            {

                case EnumCardCouponType.EntranceTicket:
                    MyCardCoupons model = new MyCardCoupons();
                    model.CardCouponType = CommonPlatform.Helper.EnumStringHelper.ToString(cardCouponType);
                    model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(11111, 99999).ToString());
                    model.CardId = cardId;
                    model.InsertDate = DateTime.Now;
                    model.Status = 0;
                    model.UserId = userId;
                    model.WebSiteOwner = WebsiteOwner;
                    if (Add(model))
                    {
                        msg = "领取成功";
                        return true;
                    }
                    else
                    {
                        msg = "领取失败";
                        return false;
                    }
                default:
                    break;
            }



            return false;

        }


        /// <summary>
        /// 检查卡券是否已经过期
        /// </summary>
        /// <param name="cardCouponType"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public bool IsCardCouponExpire(EnumCardCouponType cardCouponType, int cardId)
        {

            CardCoupons model = GetCardCoupon(cardCouponType, cardId);
            if (model.ValidTo != null)
            {
                if ((DateTime)model.ValidTo < DateTime.Now)
                {
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// 发优惠券给指定用户
        /// </summary>
        /// <param name="cardCouponId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SendCardCoupon(int cardCouponId, string userId)
        {
            CardCoupons cardCoupon = GetCardCoupon(cardCouponId);

            MyCardCoupons model = new MyCardCoupons();
            model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(11111, 99999).ToString() + GetGUID(TransacType.CommAdd));
            model.CardCouponType = cardCoupon.CardCouponType;
            model.CardId = cardCoupon.CardId;
            model.InsertDate = DateTime.Now;
            model.UserId = userId;
            model.WebSiteOwner = WebsiteOwner;

            return Add(model);
        }

        /// <summary>
        /// 是否已经领过卡券 0 未领 1已领
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mainCardId"></param>
        /// <returns></returns>
        public int IsReciveceCardCoupon(string userId, string cardCouponId)
        {

            if (GetCount<MyCardCoupons>(string.Format(" UserId='{0}' And CardId='{1}'", userId, cardCouponId)) > 0)
            {
                return 1;
            }
            return 0;

        }
        /// <summary>
        /// 检查优惠券是否可以赠送给他人
        /// </summary>
        /// <param name="myCoupon">我的优惠券</param>
        /// <returns></returns>
        public bool IsCanGiveCoupon(MyCardCoupons myCoupon, out string msg)
        {
            msg = "";
            if (!IsCanUseCoupon(myCoupon, out msg))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(myCoupon.FromUserId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(myCoupon.ToUserId))
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// 是否可以使用优惠券
        /// </summary>
        /// <param name="myCoupon"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsCanUseCoupon(MyCardCoupons myCoupon, out string msg)
        {
            msg = "";
            if (myCoupon.Status == 0)
            {
                CardCoupons cardCoupon = GetCoupon(myCoupon.CardId);
                cardCoupon = ConvertExpireTime(cardCoupon);
                if (cardCoupon.ValidFrom != null && cardCoupon.ValidTo != null)
                {
                    DateTime dtNow = DateTime.Now;
                    if (dtNow >= (DateTime)(cardCoupon.ValidFrom) && (dtNow <= (DateTime)(cardCoupon.ValidTo)))
                    {
                        return true;
                    }
                    else
                    {
                        msg = "优惠券已过期";
                    }

                }
            }
            else if (myCoupon.Status == 1)
            {

                msg = "优惠券已经使用";
            }
            else if (myCoupon.Status == 2)
            {
                msg = "优惠券已经转赠";
            }

            return false;
        }

        /// <summary>
        /// 查询优惠券
        /// </summary>
        /// <param name="cardCouponId"></param>
        /// <returns></returns>
        public CardCoupons GetCoupon(int cardId)
        {
            return Get<CardCoupons>(string.Format("CardId={0}", cardId));
        }

        /// <summary>
        /// 赠送满送券
        /// </summary>
        public void Give(decimal totalAmount, UserInfo userInfo)
        {

            try
            {

                BLLWeixin bllWeixin = new BLLWeixin();
                BLLWebsiteDomainInfo bllDomain = new BLLWebsiteDomainInfo();
                List<CardCoupons> cardCouponList = GetList<CardCoupons>(string.Format("WebsiteOwner='{0}' And CardCouponType='MallCardCoupon_BuckleGive'", userInfo.WebsiteOwner)).OrderBy(p => p.Ex5).ToList();
                foreach (var cardCoupon in cardCouponList)
                {
                    if (totalAmount >= decimal.Parse(cardCoupon.Ex5))
                    {
                        if (cardCoupon.MaxCount > 0)
                        {
                            int sendCount = GetCount<MyCardCoupons>(string.Format(" CardID={0}", cardCoupon.CardId));
                            if (sendCount >= cardCoupon.MaxCount)
                            {
                                break;
                            }

                        }
                        //给用户发满送券
                        MyCardCoupons model = new MyCardCoupons();
                        model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), GetGUID(BLLJIMP.TransacType.CommAdd));
                        model.CardCouponType = cardCoupon.CardCouponType;
                        model.CardId = cardCoupon.CardId;
                        model.InsertDate = DateTime.Now;
                        model.UserId = userInfo.UserID;
                        model.WebSiteOwner = userInfo.WebsiteOwner;
                        if (Add(model))
                        {
                            string title = "您收到一张优惠券";
                            string content = string.Format("{0}", cardCoupon.Name);
                            string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", bllDomain.GetWebsiteDoMain(userInfo.WebsiteOwner));
                            bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, redicturl);

                        }


                        //给用户发满送券
                        //break;

                    }

                }


                #region New
                List<CardCoupons> cardCouponListNew = GetList<CardCoupons>(string.Format("WebsiteOwner='{0}' And FullGive!=''", userInfo.WebsiteOwner));

                foreach (var cardCoupon in cardCouponListNew)
                {
                    if (totalAmount >= decimal.Parse(cardCoupon.FullGive))
                    {
                        if (cardCoupon.MaxCount > 0)
                        {
                            int sendCount = GetCount<MyCardCoupons>(string.Format(" CardID={0}", cardCoupon.CardId));
                            if (sendCount >= cardCoupon.MaxCount)
                            {

                                break;
                            }

                        }
                        //给用户发券
                        MyCardCoupons model = new MyCardCoupons();
                        model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), GetGUID(BLLJIMP.TransacType.CommAdd));
                        model.CardCouponType = cardCoupon.CardCouponType;
                        model.CardId = cardCoupon.CardId;
                        model.InsertDate = DateTime.Now;
                        model.UserId = userInfo.UserID;
                        model.WebSiteOwner = userInfo.WebsiteOwner;
                        if (Add(model))
                        {
                            string title = "您收到一张优惠券";
                            string content = string.Format("{0}", cardCoupon.Name);
                            string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", bllDomain.GetWebsiteDoMain(userInfo.WebsiteOwner));
                            bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, redicturl);

                        }
                        else
                        {

                        }


                        //给用户发券
                        //break;

                    }
                    else
                    {

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {



            }


        }

        public void SendCardCouponsByCurrUserInfo(UserInfo currUserInfo,string cardId)
        {
            try
            {
                BLLWeixin bllWeixin = new BLLWeixin();

                BLLWebsiteDomainInfo bllDomain = new BLLWebsiteDomainInfo();

                CardCoupons cardCoupon = Get<CardCoupons>(string.Format(" WebsiteOwner='{0}' AND CardId={1} ", currUserInfo.WebsiteOwner, int.Parse(cardId)));

                MyCardCoupons model = new MyCardCoupons();
                model.CardCouponNumber = GetGUID(BLLJIMP.TransacType.CommAdd);
                model.CardCouponType = cardCoupon.CardCouponType;
                model.CardId = cardCoupon.CardId;
                model.InsertDate = DateTime.Now;
                model.UserId = currUserInfo.UserID;
                model.WebSiteOwner = currUserInfo.WebsiteOwner;
                if (Add(model))
                {
                    string title = "您收到一张优惠券";
                    string content = string.Format("{0}", cardCoupon.Name);
                    string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", bllDomain.GetWebsiteDoMain(currUserInfo.WebsiteOwner));
                    bllWeixin.SendTemplateMessageNotifyComm(currUserInfo, title, content, redicturl);
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 转换卡券生效时间段
        /// </summary>
        /// <param name="mainCardCoupon">主卡券</param>
        /// <param name="myCardCoupon">我的优惠券</param>
        /// <returns></returns>
        public CardCoupons ConvertExpireTime(CardCoupons mainCardCoupon, MyCardCoupons myCardCoupon = null)
        {

            try
            {
                if (myCardCoupon == null)
                {
                    if (mainCardCoupon.ExpireTimeType == "1")
                    {
                        mainCardCoupon.ValidFrom = DateTime.Now;
                        mainCardCoupon.ValidTo = DateTime.Now.AddDays(int.Parse(mainCardCoupon.ExpireDay));
                    }
                    return mainCardCoupon;
                }
                if (mainCardCoupon.ExpireTimeType == "1" && myCardCoupon != null)//指定天数过期 
                {
                    if (!string.IsNullOrEmpty(myCardCoupon.FromUserId))//转赠的优惠券过期时间跟赠送人的卡券过期时间相同
                    {
                        myCardCoupon = GetMyCardCouponMainId(mainCardCoupon.CardId, myCardCoupon.FromUserId);//查找到转赠人的卡券

                    }
                    mainCardCoupon.ValidFrom = myCardCoupon.InsertDate;
                    mainCardCoupon.ValidTo = myCardCoupon.InsertDate.AddDays(int.Parse(mainCardCoupon.ExpireDay));

                }

            }
            catch (Exception)
            {


            }
            return mainCardCoupon;

        }

        /// <summary>
        /// 关注自动送券
        /// </summary>
        public void SubscribeGive(UserInfo userInfo)
        {

            try
            {
                BLLDistribution bllDis = new BLLDistribution();
                BLLWeixin bllWeixin = new BLLWeixin();
                BLLWebsiteDomainInfo bllDomain = new BLLWebsiteDomainInfo();
                List<CardCoupons> cardCouponList = GetList<CardCoupons>(string.Format("WebsiteOwner='{0}' And IsSubscribeGive='1'", userInfo.WebsiteOwner));
                foreach (var cardCoupon in cardCouponList)
                {
                    if (GetCount<MyCardCoupons>(string.Format("CardId='{0}' And UserId='{1}'", cardCoupon.CardId, userInfo.UserID)) > 0)
                    {
                        continue;//已经发过了
                    }
                    if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))
                    {

                        if (bllDis.GetUserChannel(userInfo) != cardCoupon.BindChannelUserId)
                        {
                            continue;
                        }
                    }
                    if (cardCoupon.MaxCount > 0)
                    {
                        int sendCount = GetCount<MyCardCoupons>(string.Format(" CardID={0}", cardCoupon.CardId));
                        if (sendCount >= cardCoupon.MaxCount)
                        {
                            continue;
                        }

                    }


                    //给用户发满送券
                    MyCardCoupons model = new MyCardCoupons();
                    model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), GetGUID(BLLJIMP.TransacType.CommAdd));
                    model.CardCouponType = cardCoupon.CardCouponType;
                    model.CardId = cardCoupon.CardId;
                    model.InsertDate = DateTime.Now;
                    model.UserId = userInfo.UserID;
                    model.WebSiteOwner = userInfo.WebsiteOwner;
                    if (Add(model))
                    {
                        string title = "您收到一张优惠券";
                        string content = string.Format("{0}", cardCoupon.Name);
                        string redicturl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons", bllDomain.GetWebsiteDoMain(userInfo.WebsiteOwner));
                        bllWeixin.SendTemplateMessageNotifyComm(userInfo, title, content, redicturl);


                    }



                }
            }
            catch (Exception)
            {


            }


        }

    }
}
