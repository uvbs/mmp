using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;
using ZentCloud.BLLJIMP.ModelGen.API.cardcoupon;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Components.Coupon.Handler
{
    /// <summary>
    /// 卡券 API 
    /// </summary>
    public class API : IHttpHandler, IReadOnlySessionState
    {

        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 网站所有者
        /// </summary>
        //private string WebSiteOwner;
        /// <summary>
        /// 卡券业务逻辑
        /// </summary>
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        /// <summary>
        /// 基路径 形式如 http://dev.comeoncloud.net
        /// </summary>
       // private string basePath;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo CurrentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bllCardCoupon.IsLogin)
                {
                    CurrentUserInfo = bllCardCoupon.GetCurrentUserInfo();
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "尚未登录";
                    context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                //WebSiteOwner = bllCardCoupon.WebsiteOwner;
                //basePath = string.Format("http://{0}", context.Request.Url.Host);
                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {

                    resp.errmsg = "action not exist";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }

        }

        /// <summary>
        /// 获取主卡券信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getcouponinfo(HttpContext context)
        {
            string cardCouponType = context.Request["cardcoupontype"];
            int cardId = int.Parse(context.Request["cardid"]);
            switch (cardCouponType)
            {
                case "entranceticket"://门票
                    CardCoupons sourceData = bllCardCoupon.GetCardCoupon("EntranceTicket", cardId);
                    Cardcoupon_EntranceTicket model = new Cardcoupon_EntranceTicket();
                    model.card_id = sourceData.CardId;
                    model.card_name = sourceData.Name;
                    model.card_logo =bllCardCoupon.GetImgUrl(sourceData.Logo);
                    if (sourceData.ValidFrom != null)
                    {
                        model.card_validfrom = bllCardCoupon.GetTimeStamp((DateTime)sourceData.ValidFrom);

                    }
                    if (sourceData.ValidTo != null)
                    {
                        model.card_validto = bllCardCoupon.GetTimeStamp((DateTime)sourceData.ValidTo);

                    }
                    model.card_bigimg =bllCardCoupon.GetImgUrl(sourceData.Ex1);//Ex1 代表门票大图
                    model.card_detail = sourceData.Ex2;//Ex2 代表门票详情
                    if (model.card_detail != null && model.card_detail.Contains("/FileUpload/"))
                    {
                        model.card_detail = model.card_detail.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
                    }
                    return Common.JSONHelper.ObjectToJson(model);




                default:
                    break;
            }
            return "";


        }

        /// <summary>
        /// 接收卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string receivecoupon(HttpContext context)
        {
            string CardCouponType = context.Request["cardcoupontype"];
            int CardId = int.Parse(context.Request["cardid"]);
            string Msg = "";
            switch (CardCouponType)
            {
                #region 门票
                case "entranceticket"://门票
                    if (bllCardCoupon.ReciveCoupon(EnumCardCouponType.EntranceTicket, CardId, CurrentUserInfo.UserID, out Msg))
                    {
                        resp.errcode = 0;
                        resp.errmsg = Msg;
                    }
                    else
                    {
                        resp.errcode = 1;
                        resp.errmsg = Msg;
                    }
                    break;
                #endregion
                default:
                    break;
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 获取我的卡券列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getmycouponlist(HttpContext context)
        {
            string CardCouponType = context.Request["cardcoupontype"];
            int PageIndex = int.Parse(context.Request["pageindex"]);
            int PageSize = int.Parse(context.Request["pagesize"]);
            string Status = context.Request["status"];
            switch (CardCouponType)
            {
                #region 门票
                case "entranceticket"://门票
                    MyCardcouponList_EntranceTicket apimodel = new MyCardcouponList_EntranceTicket();
                    int TotalCount = 0;
                    var SourceData = bllCardCoupon.GetMyCardCoupons(EnumCardCouponType.EntranceTicket, CurrentUserInfo.UserID, PageIndex, PageSize, out TotalCount, "", " AutoID DESC",Status);
                    apimodel.totalcount = TotalCount;
                   
                    apimodel.list = new List<MyCardcoupon_EntranceTicket>();
                    foreach (var item in SourceData)
                    {
                        CardCoupons CardCoupon = bllCardCoupon.GetCardCoupon(EnumCardCouponType.EntranceTicket, item.CardId);
                        MyCardcoupon_EntranceTicket model = new MyCardcoupon_EntranceTicket();
                        model.card_bigimg =bllCardCoupon.GetImgUrl(CardCoupon.Ex1);
                        model.card_detail = CardCoupon.Ex2;
                        if (model.card_detail != null && model.card_detail.Contains("/FileUpload/"))
                        {
                            model.card_detail = model.card_detail.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
                        }
                        model.card_logo = bllCardCoupon.GetImgUrl(CardCoupon.Logo);
                        model.card_name = CardCoupon.Name;
                        model.card_number = item.CardCouponNumber;
                        if (CardCoupon.ValidFrom != null)
                        {
                            model.card_validfrom = bllCardCoupon.GetTimeStamp((DateTime)CardCoupon.ValidFrom);

                        }
                        if (CardCoupon.ValidTo != null)
                        {
                            model.card_validto = bllCardCoupon.GetTimeStamp((DateTime)CardCoupon.ValidTo);

                        }
                        model.companyname = CardCoupon.Ex3;//公司名称
                        model.id = item.AutoId;
                        model.status = item.Status;
                        if (item.Status==0)
                        {
                            if (bllCardCoupon.IsCardCouponExpire(EnumCardCouponType.EntranceTicket,item.CardId))
                            {
                                model.status = 2;//卡券已过期
                            }
                            
                        }

                        apimodel.list.Add(model);


                    }
                    return Common.JSONHelper.ObjectToJson(apimodel);
                #endregion


                default:
                    break;
            }
            return "";


        }

        /// <summary>
        /// 获取我的单个卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getmycoupon(HttpContext context)
        {
            string CardCouponType = context.Request["cardcoupontype"];
            int Id=int.Parse(context.Request["Id"]);
            switch (CardCouponType)
            {
                #region 门票
                case "entranceticket"://门票
                    var SourceData = bllCardCoupon.GetMyCardCoupon(Id, CurrentUserInfo.UserID);
                        CardCoupons CardCoupon = bllCardCoupon.GetCardCoupon(EnumCardCouponType.EntranceTicket, SourceData.CardId);
                        MyCardcoupon_EntranceTicket model = new MyCardcoupon_EntranceTicket();
                        model.card_bigimg = bllCardCoupon.GetImgUrl(CardCoupon.Ex1);
                        model.card_detail = CardCoupon.Ex2;
                        if (model.card_detail != null && model.card_detail.Contains("/FileUpload/"))
                        {
                            model.card_detail = model.card_detail.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
                        }
                        model.card_logo = bllCardCoupon.GetImgUrl(CardCoupon.Logo);
                        model.card_name = CardCoupon.Name;
                        model.card_number = SourceData.CardCouponNumber;
                        if (CardCoupon.ValidFrom != null)
                        {
                            model.card_validfrom = bllCardCoupon.GetTimeStamp((DateTime)CardCoupon.ValidFrom);

                        }
                        if (CardCoupon.ValidTo != null)
                        {
                            model.card_validto = bllCardCoupon.GetTimeStamp((DateTime)CardCoupon.ValidTo);

                        }
                        model.companyname = CardCoupon.Ex3;//公司名称
                        model.id = SourceData.AutoId;
                        model.status = SourceData.Status;
                        if (SourceData.Status == 0)
                        {
                            if (bllCardCoupon.IsCardCouponExpire(EnumCardCouponType.EntranceTicket, SourceData.CardId))
                            {
                                model.status = 2;//卡券已过期
                            }

                        }
                    return Common.JSONHelper.ObjectToJson(model);



                    
                    
                #endregion


               
            }
            return "";
    }

        

        /// <summary>
        /// 使用我的卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string usemycoupon(HttpContext context)
        {
            string CardCouponType = context.Request["cardcoupontype"];
            int id = int.Parse(context.Request["id"]);
            string Msg = "";
            switch (CardCouponType)
            {
                #region 门票
                case "entranceticket"://门票
                    if (bllCardCoupon.UseMyCardCoupons(EnumCardCouponType.EntranceTicket,CurrentUserInfo.UserID,id,out Msg))
                    {
                        resp.errcode = 0;
                        resp.errmsg = Msg;
                    }
                    else
                    {
                        resp.errcode = 1;
                        resp.errmsg = Msg;
                    }
                    break;
                #endregion
                default:
                    break;
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 检查是否已经领取卡券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string isreceivecoupon(HttpContext context)
        {
            string CardCouponType = context.Request["cardcoupontype"];
            int CardId = int.Parse(context.Request["cardid"]);
            switch (CardCouponType)
            {
                #region 门票
                case "entranceticket"://门票
                    if (bllCardCoupon.GetCardCoupon(EnumCardCouponType.EntranceTicket,CardId)==null)
                    {
                        resp.errcode =2;
                        resp.errmsg = "卡券不存在";
                        return Common.JSONHelper.ObjectToJson(resp);

                    }
                    if (bllCardCoupon.IsReciveCoupon(EnumCardCouponType.EntranceTicket,CardId,CurrentUserInfo.UserID))
                    {
                        resp.errcode =1;
                        resp.errmsg = "已领取";
                    }
                    else
                    {
                        resp.errcode =0;
                        resp.errmsg = "未领取";
                    }
                    break;
                #endregion
                default:
                    break;
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 获取我的信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getmyinfo(HttpContext context)
        {
            VoteObjectInfo voteObjectInfo=bllCardCoupon.Get<VoteObjectInfo>(string.Format("CreateUserId='{0}' And VoteID=120",CurrentUserInfo.UserID));
            return ZentCloud.Common.JSONHelper.ObjectToJson(new { 
            number=voteObjectInfo.Number,
            name=voteObjectInfo.VoteObjectName,
            rand=voteObjectInfo.Rank
            });
        
        }

        ///// <summary>
        ///// 检查是否已经使用卡券
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string isusecoupon(HttpContext context)
        //{
        //    string CardCouponType = context.Request["cardcoupontype"];
        //    int Id = int.Parse(context.Request["id"]);
        //    switch (CardCouponType)
        //    {
        //        #region 门票
        //        case "entranceticket"://门票
        //            //检查门票是否过期

        //            //

        //            if (bllCardCoupon.IsUseCoupon(EnumCardCouponType.EntranceTicket, Id, CurrentUserInfo.UserID))
        //            {
        //                resp.errcode = 1;
        //                resp.errmsg = "已领取";
        //            }
        //            else
        //            {
        //                resp.errcode = 0;
        //                resp.errmsg = "未领取";
        //            }
        //            break;
        //        #endregion
        //        default:
        //            break;
        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);


        //}

        /// <summary>
        /// 默认响应模型
        /// </summary>
        public class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

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