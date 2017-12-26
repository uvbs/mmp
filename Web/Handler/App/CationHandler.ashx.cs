using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Weixin;
using ZentCloud.BLLPermission;
using ZentCloud.BLLJIMP.ModelGen.Weixin;
using AliOss;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections;



namespace ZentCloud.JubitIMP.Web.Handler.App
{
    public static class MyEnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element))) { yield return element; }
            }
        }
    }

    /// <summary>
    ///  Big Handler
    /// </summary>
    public class CationHandler : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 基BLL
        /// </summary>
        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 微信 BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 权限 BLL
        /// </summary>
        ZentCloud.BLLPermission.BLLMenuPermission pmsBll = new BLLMenuPermission("");
        /// <summary>
        /// 网站 BLL
        /// </summary>
        BLLWebSite bllWebsite = new BLLWebSite();
        /// <summary>
        /// 商城 BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 线上分销BLL
        /// </summary>
        BLLDistribution bllDis = new BLLDistribution();
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLDistributionOffLine bllDisOffLine = new BLLDistributionOffLine();
        /// <summary>
        /// 投票BLL
        /// </summary>
        BLLVote bllVote = new BLLVote();
        /// <summary>
        /// 刮奖活动BLL
        /// </summary>
        BllLottery bllLottery = new BllLottery();
        /// <summary>
        /// 积分BLL
        /// </summary>
        BllScore bllScore = new BllScore();
        /// <summary>
        /// 系统通知
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        /// <summary>
        /// 企业微网站
        /// </summary>
        BLLCompanyWebSite bllConpanyWebsite = new BLLCompanyWebSite();
        /// <summary>
        /// 卡券逻辑
        /// </summary>
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        /// <summary>
        /// 支付逻辑
        /// </summary>
        BllPay bllPay = new BllPay();
        /// <summary>
        /// 短信BLL
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// yike 
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();

        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
        /// <summary>
        /// 当前站点所有者信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        /// <summary>
        /// 模块日志
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLLog();
        /// <summary>
        /// 当前站点信息
        /// </summary>
        WebsiteInfo currentWebSiteInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                this.currentUserInfo = bllBase.GetCurrentUserInfo();
                this.currWebSiteUserInfo = bllBase.GetCurrWebSiteUserInfo();
                currentWebSiteInfo = bllBase.GetWebsiteInfoModel();
                string action = context.Request["Action"];
                switch (action)
                {
                    #region 活动文章相关模块

                    case "AddJuActivity"://添加文章活动等
                        result = AddJuActivity(context);
                        break;

                    case "EditJuActivity"://编辑文章活动等
                        result = EditJuActivity(context);
                        break;

                    case "QueryJuActivityForWeb"://查询活动 pc
                        result = QueryJuActivityForWeb(context);
                        break;
                    case "ImportMpArticle"://
                        result = ImportMpArticle(context);
                        break;


                    //case "QueryJuActivityForWap"://查询活动 wap old
                    //    result = QueryJuActivityForWap(context);
                    //    break;

                    case "QueryArticleForWap"://查询文章活动 wap
                        result = QueryArticleForWap(context);
                        break;

                    case "UpdateArticleSortIndex"://修改文章活动排序号
                        result = UpdateArticleSortIndex(context);
                        break;

                    case "UpdateAccessLevel"://修改文章活动访问级别
                        result = UpdateAccessLevel(context);
                        break;

                    case "DeleteJuActivity":
                        result = DeleteJuActivity(context);
                        break;

                    case "GetArticleSpreadTree"://文章转发树
                        result = GetArticleSpreadTree(context);
                        break;



                    //case "QueryArticleListForWap":// 文章活动课程 移动设备访问
                    //    result = QueryArticleListForWap(context);
                    //    break;

                    //case "QueryJuMaster":
                    //    result = QueryJuMaster(context);
                    //    break;

                    case "GetSingelJuActivity":
                        result = GetSingelJuActivity(context);
                        break;


                    //case "QueryJuMasterForWeb"://查询讲师
                    //    result = QueryJuMasterForWeb(context);
                    //    break;

                    //case "EditJuMasterInfo"://修改讲师
                    //    result = EditJuMasterInfo(context);
                    //    break;

                    //case "AddJuMasterInfo"://添加讲师
                    //    result = AddJuMasterInfo(context);
                    //    break;
                    //case "DeleteJuMasterInfo"://删除讲师
                    //    result = DeleteJuMasterInfo(context);
                    //    break;

                    case "QueryWXSignInData":
                        result = QueryWXSignInData(context);
                        break;

                    case "DeleteWXSignInData":
                        result = DeleteWXSignInData(context);
                        break;
                    case "BatchSetArticleCategory":
                        result = BatchSetArticleCategory(context);
                        break;




                    #endregion

                    #region 用户管理相关模块
                    //查询 站点会员
                    case "QueryWebsiteUser":
                        result = QueryWebsiteUser(context);
                        break;
                    case "QueryWebsiteUserDistributionOnLine"://商城分销会员
                        result = QueryWebsiteUserDistributionOnLine(context);
                        break;
                    #region 渠道
                    case "QueryChannel"://渠道
                        result = QueryChannel(context);
                        break;
                    case "QueryAllChannel"://所有渠道
                        result = QueryAllChannel(context);
                        break;
                    case "QueryChildChannel"://分销员
                        result = QueryChildChannel(context);
                        break;
                    case "AddChannel"://
                        result = AddChannel(context);
                        break;
                    case "EditChannel"://
                        result = EditChannel(context);
                        break;
                    case "DeleteChannel"://
                        result = DeleteChannel(context);
                        break;
                    case "FlashChannelData"://刷新渠道数据
                        result = FlashChannelData(context);
                        break;

                    case "SetWeixinMgr"://设置微信管理员
                        result = SetWeixinMgr(context);
                        break;
                    case "SetWeixinMgrAndSetFirstDistributionLevel"://设置微信管理员且添加二维码
                        result = SetWeixinMgrAndSetFirstDistributionLevel(context);
                        break;

                    case "SetFirstLevelDistribution"://从己有用户添加二维码
                        result = SetFirstLevelDistribution(context);
                        break;
                    case "DeleteFirstLevelDistribution"://删除二维码
                        result = DeleteFirstLevelDistribution(context);
                        break;

                    case "AddFirstLevelDistribution"://增加渠道二维码
                        result = AddFirstLevelDistribution(context);
                        break;
                    case "EditFirstLevelDistribution"://编辑渠道二维码
                        result = EditFirstLevelDistribution(context);
                        break;
                    #endregion

                    #region 供应商渠道
                    case "QuerySupplierChannel"://渠道
                        result = QuerySupplierChannel(context);
                        break;
                    case "AddSupplierChannel"://
                        result = AddSupplierChannel(context);
                        break;
                    case "EditSupplierChannel"://
                        result = EditSupplierChannel(context);
                        break;
                    case "DeleteSupplierChannel"://
                        result = DeleteSupplierChannel(context);
                        break;
                    case "QueryChildChannelSupplier"://
                        result = QueryChildChannelSupplier(context);
                        break;
                    case "AddChildChannelSupplier"://
                        result = AddChildChannelSupplier(context);
                        break;
                    case "DeleteChildChannelSupplier"://
                        result = DeleteChildChannelSupplier(context);
                        break;
                    #endregion
                    case "UpdateDistributionOnLinePreUser"://修改商城分销会员上级
                        result = UpdateDistributionOnLinePreUser(context);
                        break;
                    case "SynDistribution"://同步分销上级人数
                        result = SynDistribution(context);
                        break;
                    case "SynDistributionSaleAmount"://同步销售额
                        result = SynDistributionSaleAmount(context);
                        break;
                    case "CleanUser"://会员清洗
                        result = CleanUser(context);
                        break;


                    //case "QueryWebsiteUserByTrueName":
                    //    result = QueryWebsiteUserByTrueName(context);
                    //    break;
                    //case "QueryWebsiteUserByTrueNameOrWX":
                    //    result = QueryWebsiteUserByTrueNameOrWX(context);
                    //    break;
                    //case "QueryWebsiteUserByDistribution":
                    //    result = QueryWebsiteUserByDistribution(context);
                    //    break;
                    //case "SetHFUserPmsGroup":
                    //    result = SetHFUserPmsGroup(context);
                    //    break;
                    case "SetUserAccessLevel":
                        result = SetUserAccessLevel(context);
                        break;
                    case "SendTemplateMsg":
                        result = SendTemplateMsg(context);
                        break;
                    case "SendTemplateMsgByTag":
                        result = SendTemplateMsgByTag(context);
                        break;
                    case "SendTemplateMsgByFans":
                        result = SendTemplateMsgByFans(context);
                        break;
                    case "SendTemplateMsgByAllFans":
                        result = SendTemplateMsgByAllFans(context);
                        break;
                    case "UpdateUserPwd":
                        result = UpdateUserPwd(context);
                        break;
                    case "SynMemberInfo":
                        result = SynMemberInfo(context);
                        break;

                    #endregion

                    #region 报名相关模块
                    case "QueryActivityData":
                        result = QueryActivityData(context);
                        break;
                    case "DeleteActivityData":
                        result = DeleteActivityData(context);
                        break;
                    #endregion

                    #region 问答模块
                    //case "QueryJuMasterFeedBack":
                    //    result = QueryJuMasterFeedBack(context);
                    //    break;

                    //case "QueryJuMasterFeedBackDialogue":
                    //    result = QueryJuMasterFeedBackDialogue(context);
                    //    break;

                    //case "QueryJuMasterFeedBackForPCGrid":
                    //    result = QueryJuMasterFeedBackForPCGrid(context);
                    //    break;

                    //case "QueryJuMasterFeedBackDialogueForPCGrid":
                    //    result = QueryJuMasterFeedBackDialogueForPCGrid(context);
                    //    break;

                    //case "AddJuMasterFeedBack":
                    //    result = AddJuMasterFeedBack(context);
                    //    break;
                    //case "AddJuMasterFeedBackDialog":
                    //    result = AddJuMasterFeedBackDialog(context);
                    //    break;

                    //case "DeleteJuMasterFeedBack":
                    //    result = DeleteJuMasterFeedBack(context);
                    //    break;

                    //case "DeleteJuMasterFeedBackDialogue":
                    //    result = DeleteJuMasterFeedBackDialogue(context);
                    //    break;

                    #endregion

                    #region 微信菜单配置模块
                    case "AddWeixinMenu":
                        result = AddWeixinMenu(context);//添加微信菜单
                        break;
                    case "EditWeixinMenu":
                        result = EditWeixinMenu(context);//编辑微信菜单
                        break;
                    case "DeleteWeixinMenu":
                        result = DeleteWeixinMenu(context);//删除微信菜单
                        break;
                    case "QueryWeixinMenu":
                        result = QueryWeixinMenu(context);//获取微信菜单
                        break;
                    case "GetMenuSelectList"://获取微信自定义菜单
                        result = GetMenuSelectList(context);
                        break;

                    case "CreateWeixinClientMenu":
                        result = CreateWeixinClientMenu();//生成微信客户端菜单
                        break;

                    case "MoveMenu":
                        result = MoveMenu(context);//调整菜单顺序
                        break;
                    #endregion

                    #region 站点模块相关
                    case "QueryWebsiteInfo":
                        result = QueryWebsiteInfo(context);
                        break;

                    case "AddWebsite":
                        result = AddWebsite(context);
                        break;

                    case "DeleteWebsite":
                        result = DeleteWebsite(context);
                        break;

                    case "EditWebsite":
                        result = EditWebsite(context);
                        break;

                    case "QueryWebSiteDomain":
                        result = QueryWebSiteDomain(context);
                        break;

                    case "AddWebSiteDomain":
                        result = AddWebSiteDomain(context);
                        break;

                    case "EditWebSiteDomain":
                        result = EditWebSiteDomain(context);
                        break;

                    case "DeleteWebSiteDomain":
                        result = DeleteWebSiteDomain(context);
                        break;

                    #endregion

                    #region 用户管理模块
                    case "QuerySysUserInfo":
                        result = QuerySysUserInfo(context);
                        break;

                    case "AddSysUser":
                        result = AddSysUser(context);
                        break;

                    case "EditSysUser":
                        result = EditSysUser(context);
                        break;
                    case "SuperEditSysUser":
                        result = SuperEditSysUser(context);
                        break;

                    case "EditWebSiteMember":
                        result = EditWebSiteMember(context);
                        break;

                    case "AddScore":
                        result = AddScore(context);
                        break;

                    case "SetSysUserPms":
                        result = SetSysUserPms(context);
                        break;

                    case "GetUserAllPmsGroup":
                        result = GetUserAllPmsGroup(context);
                        break;

                    case "DisableUser":
                        result = DisableUser(context);
                        break;

                    #endregion

                    #region 站点管理模块
                    case "UpdateWebSiteInfo"://修改站点配置 超级管理员
                        result = UpdateWebSiteInfo(context);
                        break;

                    case "UpdateWebSiteInfoPersonal"://修改站点配置个人
                        result = UpdateWebSiteInfoPersonal(context);
                        break;

                    case "QueryWebSiteInfo":
                        result = QueryWebSiteInfo(context);
                        break;

                    #endregion

                    #region 微信自动回复模块


                    #region 文本回复模块
                    case "AddTextReply":
                        result = AddTextReply(context);
                        break;
                    case "EditTextReply":
                        result = EditTextReply(context);
                        break;
                    case "DeleteTextReply":
                        result = DeleteTextReply(context);
                        break;
                    case "QueryTextReply":
                        result = QueryTextReply(context);
                        break;
                    #endregion


                    #region 图文回复
                    case "AddNewsReply":
                        result = AddNewsReply(context);
                        break;
                    case "EditNewsReply":
                        result = EditNewsReply(context);
                        break;
                    case "GetNewsReplyImageList":
                        result = GetNewsReplyImageList(context);
                        break;
                    case "DeleteNewsReply":
                        result = DeleteNewsReply(context);
                        break;
                    case "QuerySource":
                        result = GetSourceNotAdd(context);
                        break;
                    case "QueryNewsReply":
                        result = QueryNewsReply(context);
                        break;
                    case "GetSourceImageList"://图文回复图片列表
                        result = GetSourceImageList(context);
                        break;



                    #endregion

                    #region 图文回复素材图片
                    case "AddNewsReplyImg":
                        result = AddNewsReplyImg(context);
                        break;
                    case "EditNewsReplyImg":
                        result = EditNewsReplyImg(context);
                        break;
                    case "DeleteNewsReplyImg":
                        result = DeleteNewsReplyImg(context);
                        break;
                    case "QueryNewsReplyImg":
                        result = QueryNewsReplyImg(context);
                        break;
                    case "BroadcastImageText":
                        result = BroadcastImageText(context); ;
                        break;

                    case "SendMassMessageNews"://群发图文
                        result = SendMassMessageNews(context); ;
                        break;
                    case "SendMassMessageNewsPreview"://群发图文预览
                        result = SendMassMessageNewsPreview(context); ;
                        break;


                    #endregion




                    #endregion


                    //case "SendKeFuMsgKuanQiao"://宽桥手机核名申请成功 发送客服消息到用户
                    //    result = SendKeFuMsgKuanQiao(context);
                    //    break;


                    case "SetPubConfig"://配置微信接口
                        result = SetPubConfig(context);
                        break;

                    case "QueryMsgDetails":
                        result = QueryMsgDetails(context);
                        break;

                    case "GetMsgDetails":
                        result = GetMsgDetails(context);
                        break;

                    //case "SynchronousAllFollowers":
                    //    result = SynchronousAllFollowers();//同步微信粉丝信息
                    //    break;

                    case "QueryWeixinFollowersInfo"://查询粉丝
                        result = QueryWeixinFollowersInfo(context);
                        break;
                    case "UpdateAllFollowersInfo":
                        result = UpdateAllFollowersInfo();//更新微信粉丝信息
                        break;
                    case "SynWeixinNews":
                        result = SynWeixinNews();//同步微信素材
                        break;

                    case "SetKeFuConfig"://配置客服
                        result = SetKeFuConfig(context);
                        break;

                    case "UpdateWXMallConfig"://商城配置
                        result = UpdateWXMallConfig(context);
                        break;



                    #region 商城模块

                    #region 商品管理模块

                    case "QueryWXMallProductInfo"://查询商品
                        result = QueryWXMallProductInfo(context);
                        break;


                    case "DeleteWXMallProductInfo"://删除商品
                        result = DeleteWXMallProductInfo(context);
                        break;

                    case "AddWXMallProductInfo"://添加商品
                        result = AddWXMallProductInfo(context);
                        break;

                    case "EditWXMallProductInfo"://编辑商品
                        result = EditWXMallProductInfo(context);
                        break;

                    //case "QueryProductsObjList":
                    //    result = QueryProductsObjList(context);
                    //    break;
                    ////GetProductObj
                    //case "GetProductObj":
                    //    result = GetProductObj(context);
                    //    break;
                    ////SubmitWxMallOrder
                    //case "SubmitWxMallOrder":
                    //    result = SubmitWxMallOrder(context);
                    //    break;

                    //case "QianWeiStockStatistics"://库存统计
                    //    result = QianWeiStockStatistics(context);
                    //    break;
                    //case "UpdateProductStock"://更新库存
                    //    result = UpdateProductStock(context);
                    //    break;

                    #endregion


                    #region 积分商品管理模块

                    case "QueryWXMallScoreProductInfo"://查询商品
                        result = QueryWXMallScoreProductInfo(context);
                        break;

                    case "DeleteWXMallScoreProductInfo"://删除商品
                        result = DeleteWXMallScoreProductInfo(context);
                        break;

                    case "AddWXMallScoreProductInfo"://添加商品
                        result = AddWXMallScoreProductInfo(context);
                        break;

                    case "EditWXMallScoreProductInfo"://编辑积分商品
                        result = EditWXMallScoreProductInfo(context);
                        break;


                    case "UpdateScoreProductSortIndex"://修改排序号
                        result = UpdateScoreProductSortIndex(context);
                        break;

                    #endregion

                    #region 一般订单管理模块
                    case "QueryWXMallOrderInfo"://查询订单
                        result = QueryWXMallOrderInfo(context);
                        break;
                    case "DeleteWXMallOrderInfo"://删除订单
                        result = DeleteWXMallOrderInfo(context);
                        break;

                    case "ExportOrder"://导出订单
                        ExportOrder(context);
                        break;
                    case "UpdateOrderStatus"://更新订单状态 单条记录
                        result = UpdateOrderStatus(context);
                        break;

                    case "UpdateOrderStatusBatch"://更新订单状态 批量修改
                        result = UpdateOrderStatusBatch(context);
                        break;

                    //case "GetOrderRemindByTime"://一般订单提醒
                    //    result = GetOrderRemindByTime(context);
                    //    break;

                    case "UpdateDistributionOrderStatus"://更新分销订单状态 单条记录
                        result = UpdateDistributionOrderStatus(context);
                        break;
                    //case "UpdateDistributionOrderStatusBatch"://更新分销订单状态 多条记录
                    //    result = UpdateDistributionOrderStatusBatch(context);
                    //    break;
                    case "GetChildDistribution"://获取分销树
                        result = GetChildDistribution(context);
                        break;

                    case "QueryDistributionOrder"://获取分销订单
                        result = QueryDistributionOrder(context);
                        break;

                    case "EfastSynWXMallOrderInfo"://efast订单同步
                        result = EfastSynWXMallOrderInfo(context);
                        break;



                    #endregion

                    #region 积分订单管理模块
                    case "QueryWXMallScoreOrderInfo"://查询订单
                        result = QueryWXMallScoreOrderInfo(context);
                        break;
                    case "DeleteWXMallScoreOrderInfo"://删除订单
                        result = DeleteWXMallScoreOrderInfo(context);
                        break;

                    case "UpdateScoreOrderStatus"://更新订单状态
                        result = UpdateScoreOrderStatus(context);
                        break;
                    case "UpdateScoreOrderRemark"://更新订单备注
                        result = UpdateScoreOrderRemark(context);
                        break;

                    #endregion

                    #region 微商城门店管理
                    case "QueryWXMallStores":
                        result = QueryWXMallStores(context);
                        break;
                    case "AddWXMallStore":
                        result = AddWXMallStore(context);
                        break;
                    case "EditWXMallStore":
                        result = EditWXMallStore(context);
                        break;
                    case "DeleteWXMallStore":
                        result = DeleteWXMallStore(context);
                        break;

                    #endregion

                    #region 微商城分类管理
                    case "QueryWXMallCategory":
                        result = QueryWXMallCategory(context);
                        break;
                    case "AddWXMallCategory":
                        result = AddWXMallCategory(context);
                        break;
                    case "EditWXMallCategory":
                        result = EditWXMallCategory(context);
                        break;
                    case "DeleteWXMallCategory":
                        result = DeleteWXMallCategory(context);
                        break;

                    case "GetWXMallCategorySelectList"://获取文章分类
                        result = GetWXMallCategorySelectList(context);
                        break;

                    #endregion

                    #region 微商城订单状态管理
                    case "QueryWXMallOrderStatu":
                        result = QueryWXMallOrderStatu(context);
                        break;
                    case "AddWXMallOrderStatu":
                        result = AddWXMallOrderStatu(context);
                        break;
                    case "EditWXMallOrderStatu":
                        result = EditWXMallOrderStatu(context);
                        break;
                    case "DeleteWXMallOrderStatu":
                        result = DeleteWXMallOrderStatu(context);
                        break;

                    #endregion
                    #endregion

                    //#region 行业模板管理


                    //case "QueryIndustryTemplate"://查询模板
                    //    result = QueryIndustryTemplate(context);
                    //    break;
                    //case "AddIndustryTemplate"://添加模板
                    //    result = AddIndustryTemplate(context);
                    //    break;

                    //case "EditIndustryTemplate"://编辑模板
                    //    result = EditIndustryTemplate(context);
                    //    break;

                    //case "DeleteIndustryTemplate"://删除模板
                    //    result = DeleteIndustryTemplate(context);
                    //    break;


                    //#endregion


                    case "GetAddVImageWap"://微信加V 移动设备端
                        result = GetAddVImageWap(context);
                        break;


                    #region 监测平台

                    //#region 任务管理
                    ////监测任务
                    //case "AddPlan":
                    //    result = AddPlan(context);//添加监测任务
                    //    break;
                    //case "EditPlan":
                    //    result = EditPlan(context);//编辑监测任务
                    //    break;
                    //case "DeletePlan":
                    //    result = DeletePlan(context);//删除任务
                    //    break;
                    //case "QueryPlan"://查询监测任务
                    //    result = QueryPlan(context);
                    //    break;

                    //case "BatChangPlanStatus"://设置任务状态
                    //    result = BatChangPlanStatus(context);
                    //    break;

                    ////监测任务 
                    //#endregion 

                    #endregion

                    #region 客服列表管理
                    case "QueryKuFuList"://微信客服列表
                        result = QueryKuFuList(context);
                        break;
                    case "AddKeFu":
                        result = AddKeFu(context);
                        break;
                    case "EditKeFu":
                        result = EditKeFu(context);
                        break;
                    case "DeleteKeFu":
                        result = DeleteKeFu(context);
                        break;

                    #endregion


                    #region 文章分类管理
                    case "QueryArticleCategory":
                        result = QueryArticleCategory(context);
                        break;
                    case "AddArticleCategory":
                        result = AddArticleCategory(context);
                        break;
                    case "EditArticleCategory":
                        result = EditArticleCategory(context);
                        break;
                    case "DeleteArticleCategory":
                        result = DeleteArticleCategory(context);
                        break;

                    case "GetCategorySelectList"://获取文章分类
                        result = GetCategorySelectList(context);
                        break;

                    #endregion


                    #region 刮奖管理
                    case "QueryWXLottery":
                        result = QueryWXLottery(context);
                        break;
                    case "AddWXLottery":
                        result = AddWXLottery(context);
                        break;
                    case "EditWXLottery":
                        result = EditWXLottery(context);
                        break;
                    case "DeleteWXLottery":
                        result = DeleteWXLottery(context);
                        break;
                    case "ResetWXLottery":
                        result = ResetWXLottery(context);
                        break;

                    case "QueryWXLotteryRecord":
                        result = QueryWXLotteryRecord(context);
                        break;


                    #region 刮奖V1
                    case "QueryWXLotteryV1":
                        result = QueryWXLotteryV1(context);
                        break;
                    case "AddWXLotteryV1":
                        result = AddWXLotteryV1(context);
                        break;
                    case "EditWXLotteryV1":
                        result = EditWXLotteryV1(context);
                        break;
                    case "DeleteWXLotteryV1":
                        result = DeleteWXLotteryV1(context);
                        break;
                    case "ResetWXLotteryV1":
                        result = ResetWXLotteryV1(context);
                        break;

                    case "QueryWXLotteryRecordV1":
                        result = QueryWXLotteryRecordV1(context);
                        break;
                    case "QueryWXLotteryWinDataV1":
                        result = QueryWXLotteryWinDataV1(context);
                        break;
                    case "AddWinData":
                        result = AddWinData(context);
                        break;
                    case "EditWinData":
                        result = EditWinData(context);
                        break;
                    case "DeleteWinData":
                        result = DeleteWinData(context);
                        break;

                    case "QueryAwardsSelect":
                        result = QueryAwardsSelect(context);
                        break;
                    case "UpdateIsGetPrize":
                        result = UpdateIsGetPrize(context);
                        break;

                    #endregion


                    #region 奖项设置
                    case "QueryWXAwards":
                        result = QueryWXAwards(context);
                        break;

                    case "AddWXAwards":
                        result = AddWXAwards(context);
                        break;
                    case "EditWXAwards":
                        result = EditWXAwards(context);
                        break;
                    case "DeleteWXAwards":
                        result = DeleteWXAwards(context);
                        break;
                    case "GetWxAwardListByLotteryId":
                        result = GetWxAwardListByLotteryId(context);
                        break;


                    #endregion


                    #region 中奖设置
                    case "QueryWinningData":
                        result = QueryWinningData(context);
                        break;

                    case "AddWinningData":
                        result = AddWinningData(context);
                        break;
                    case "EditWinningData":
                        result = EditWinningData(context);
                        break;
                    case "DeleteWinningData":
                        result = DeleteWinningData(context);
                        break;

                    #endregion


                    #endregion


                    #region 微网站

                    case "UpdateCompanyWebsiteConfig":
                        result = UpdateCompanyWebsiteConfig(context);
                        break;


                    #region 幻灯片 管理
                    case "QueryCompanyWebsiteProjector":
                        result = QueryCompanyWebsiteProjector(context);
                        break;
                    case "AddCompanyWebsiteProjector":
                        result = AddCompanyWebsiteProjector(context);
                        break;
                    case "EditCompanyWebsiteProjector":
                        result = EditCompanyWebsiteProjector(context);
                        break;
                    case "DeleteCompanyWebsiteProjector":
                        result = DeleteCompanyWebsiteProjector(context);
                        break;


                    #endregion

                    #region 底部工具栏 管理
                    case "QueryCompanyWebsiteToolBar":
                        result = QueryCompanyWebsiteToolBar(context);
                        break;
                    case "QueryCompanyWebsiteToolBarPreSelect":
                        result = QueryCompanyWebsiteToolBarPreSelect(context);
                        break;
                    case "AddCompanyWebsiteToolBar":
                        result = AddCompanyWebsiteToolBar(context);
                        break;
                    case "EditCompanyWebsiteToolBar":
                        result = EditCompanyWebsiteToolBar(context);
                        break;
                    case "DeleteCompanyWebsiteToolBar":
                        result = DeleteCompanyWebsiteToolBar(context);
                        break;
                    case "UpdateSortIndex":
                        result = UpdateSortIndex(context);
                        break;


                    #endregion

                    #region 导航管理
                    case "QueryCompanyWebsiteNavigate":
                        result = QueryCompanyWebsiteNavigate(context);
                        break;
                    case "AddCompanyWebsiteNavigate":
                        result = AddCompanyWebsiteNavigate(context);
                        break;
                    case "EditCompanyWebsiteNavigate":
                        result = EditCompanyWebsiteNavigate(context);
                        break;
                    case "DeleteCompanyWebsiteNavigate":
                        result = DeleteCompanyWebsiteNavigate(context);
                        break;


                    #endregion

                    #endregion

                    //#region 游戏活动限制管理
                    //case "QueryGameActivityQueryLimit":
                    //    result = QueryGameActivityQueryLimit(context);
                    //    break;
                    //case "AddGameActivityQueryLimit":
                    //    result = AddGameActivityQueryLimit(context);
                    //    break;
                    //case "EditGameActivityQueryLimit":
                    //    result = EditGameActivityQueryLimit(context);
                    //    break;
                    //case "DeleteGameActivityQueryLimit":
                    //    result = DeleteGameActivityQueryLimit(context);
                    //    break;

                    //#endregion

                    #region 企业网站模板管理


                    case "QueryCompanyWebsiteTemplate"://查询模板
                        result = QueryCompanyWebsiteTemplate(context);
                        break;
                    case "AddCompanyWebsiteTemplate"://添加模板
                        result = AddCompanyWebsiteTemplate(context);
                        break;

                    case "EditCompanyWebsiteTemplate"://编辑模板
                        result = EditCompanyWebsiteTemplate(context);
                        break;

                    case "DeleteCompanyWebsiteTemplate"://删除模板
                        result = DeleteCompanyWebsiteTemplate(context);
                        break;

                    case "UpdateCompanyWebSiteTemplate"://
                        result = UpdateCompanyWebSiteTemplate(context);
                        break;


                    #endregion

                    case "SetPwd":
                        result = SetPwd(context);
                        break;

                    //设置默认数据
                    case "DefaultData":
                        result = DefaultData(context);
                        break;

                    #region 游戏

                    #region 游戏任务管理
                    case "QueryGamePlan":
                        result = QueryGamePlan(context);
                        break;
                    case "AddGamePlan":
                        result = AddGamePlan(context);
                        break;
                    case "DeleteGamePlan":
                        result = DeleteGamePlan(context);
                        break;

                    #endregion


                    #region 后台游戏管理 管理员
                    case "QueryGameInfo"://查询
                        result = QueryGameInfo(context);
                        break;
                    case "AddGameInfo"://添加
                        result = AddGameInfo(context);
                        break;

                    case "EditGameInfo"://编辑
                        result = EditGameInfo(context);
                        break;

                    case "DeleteGameInfo"://删除
                        result = DeleteGameInfo(context);
                        break;


                    #endregion


                    #region 游戏监测
                    case "QueryGameEventDetail"://
                        result = QueryGameEventDetail(context);
                        break;
                    case "QueryGameEventDetailClick"://
                        result = QueryGameEventDetailClick(context);
                        break;
                    #endregion


                    #endregion

                    #region 子账户管理模块
                    case "QuerySubAccount":
                        result = QuerySubAccount(context);
                        break;

                    case "AddSubAccount":
                        result = AddSubAccount(context);
                        break;

                    case "EditSubAccount":
                        result = EditSubAccount(context);
                        break;





                    #endregion

                    #region 投票管理
                    case "QueryVoteInfo":
                        result = QueryVoteInfo(context);
                        break;
                    case "AddVoteInfo":
                        result = AddVoteInfo(context);
                        break;
                    case "EditVoteInfo":
                        result = EditVoteInfo(context);
                        break;
                    case "DeleteVoteInfo":
                        result = DeleteVoteInfo(context);
                        break;

                    #endregion

                    #region 投票对象管理
                    case "QueryVoteObjectInfo":
                        result = QueryVoteObjectInfo(context);
                        break;
                    case "AddVoteObjectInfo":
                        result = AddVoteObjectInfo(context);
                        break;
                    case "EditVoteObjectInfo":
                        result = EditVoteObjectInfo(context);
                        break;
                    case "DeleteVoteObjectInfo":
                        result = DeleteVoteObjectInfo(context);
                        break;
                    case "QueryVoteOrderInfo"://查询投票购票记录
                        result = QueryVoteOrderInfo(context);
                        break;

                    case "QueryVoteLogInfo"://查询投票记录
                        result = QueryVoteLogInfo(context);
                        break;
                    case "TransformImage"://旋转图片
                        result = TransformImage(context);
                        break;

                    #endregion

                    #region 投票[PK模式]
                    case "AddVoteGroupInfo":
                        result = AddVoteGroupInfo(context);
                        break;
                    case "QueryVoteGroupInfo":
                        result = QueryVoteGroupInfo(context);
                        break;
                    case "DeleteVoteGroupInfo":
                        result = DeleteVoteGroupInfo(context);
                        break;
                    case "EditVoteGroupInfo":
                        result = EditVoteGroupInfo(context);
                        break;
                    case "DeleteVoteGroupInfoByMembers":
                        result = DeleteVoteGroupInfoByMembers(context);
                        break;
                    case "AddVoteGroupByMember":
                        result = AddVoteGroupByMember(context);
                        break;
                    #endregion

                    #region 充值设置
                    case "QueryVoteRecharge":
                        result = QueryVoteRecharge(context);
                        break;
                    case "AddVoteRecharge":
                        result = AddVoteRecharge(context);
                        break;
                    case "EditVoteRecharge":
                        result = EditVoteRecharge(context);
                        break;
                    case "DeleteVoteRecharge":
                        result = DeleteVoteRecharge(context);
                        break;

                    #endregion


                    #region 微商城配送员管理
                    case "QueryWXMallDeliveryStaff":
                        result = QueryWXMallDeliveryStaff(context);
                        break;
                    case "AddWXMallDeliveryStaff":
                        result = AddWXMallDeliveryStaff(context);
                        break;
                    case "EditWXMallDeliveryStaff":
                        result = EditWXMallDeliveryStaff(context);
                        break;
                    case "DeleteWXMallDeliveryStaff":
                        result = DeleteWXMallDeliveryStaff(context);
                        break;

                    #endregion

                    #region 积分配置
                    case "UpdateScoreConfig":
                        result = UpdateScoreConfig(context);
                        break;
                    #endregion
                    case "ActivityForwardInfo":
                        result = ActivityForward(context);
                        break;


                    #region 支付配置
                    case "SetAlipayConfig":
                        result = SetAlipayConfig(context);
                        break;
                    case "SavePayConfig":
                        result = SavePayConfig(context);
                        break;

                    #endregion


                    #region 配送方式管理
                    case "QueryWXMallDelivery":
                        result = QueryWXMallDelivery(context);
                        break;
                    case "AddWXMallDelivery":
                        result = AddWXMallDelivery(context);
                        break;
                    case "EditWXMallDelivery":
                        result = EditWXMallDelivery(context);
                        break;
                    case "DeleteWXMallDelivery":
                        result = DeleteWXMallDelivery(context);
                        break;

                    #endregion

                    #region 支付方式管理
                    case "QueryWXMallPaymentType":
                        result = QueryWXMallPaymentType(context);
                        break;
                    case "AddWXMallPaymentType":
                        result = AddWXMallPaymentType(context);
                        break;
                    case "EditWXMallPaymentType":
                        result = EditWXMallPaymentType(context);
                        break;
                    case "DeleteWXMallPaymentType":
                        result = DeleteWXMallPaymentType(context);
                        break;
                    case "UpdateWXMallPaymentTypeStatu":
                        result = UpdateWXMallPaymentTypeStatu(context);
                        break;
                    #endregion



                    #region 问卷管理
                    case "QueryQuestionnaire":
                        result = QueryQuestionnaire(context);
                        break;
                    case "AddQuestionnaire":
                        result = AddQuestionnaire(context);//添加问卷
                        break;

                    case "DeleteQuestionnaire":
                        result = DeleteQuestionnaire(context);
                        break;

                    case "QueryQuestionnaireRecord":
                        result = QueryQuestionnaireRecord(context);
                        break;


                    case "DeleteQuestionnaireRecord":
                        result = DeleteQuestionnaireRecord(context);
                        break;

                    case "EditQuestionnaire":
                        result = EditQuestionnaire(context);//编辑问卷
                        break;

                    case "SaveExamResult":
                        result = SaveExamResult(context);
                        break;
                    #endregion

                    case "SaveTutor"://设置用户为导师
                        result = SaveTutor(context);
                        break;
                    case "SavaArticleTutor"://设置文章所属导师
                        result = SavaArticleTutor(context);
                        break;

                    #region 会员标签管理
                    case "QueryMemberTag":
                        result = QueryMemberTag(context);
                        break;
                    case "AddMemberTag":
                        result = AddMemberTag(context);
                        break;
                    case "EditMemberTag":
                        result = EditMemberTag(context);
                        break;
                    case "DeleteMemberTag":
                        result = DeleteMemberTag(context);
                        break;
                    case "UpdateUserTagName":
                        result = UpdateUserTagName(context);
                        break;
                    case "UpdateUserTagNameByAddTag":
                        result = UpdateUserTagNameByAddTag(context);
                        break;
                    case "UpdateUserTagNameByDeleteTag":
                        result = UpdateUserTagNameByDeleteTag(context);
                        break;

                    #endregion

                    #region 关注管理
                    case "QueryAttention":
                        result = QueryAttention(context);
                        break;
                    #endregion


                    #region 系统通知
                    case "GetSystemNotice":
                        result = GetSystemNotice(context);
                        break;
                    case "DelSystemNotice":
                        result = DelSystemNotice(context);
                        break;
                    case "AddSystemNotice":
                        result = AddSystemNotice(context);
                        break;

                    #endregion


                    case "SetWXQiyeConfig"://配置微信接口
                        result = SetWXQiyeConfig(context);
                        break;

                    case "GetTimingTasks"://获取定时任务
                        result = GetTimingTasks(context);
                        break;

                    case "DelTimingTasks"://删除定时任务
                        result = DelTimingTasks(context);
                        break;
                    case "AddScoreStatisticsTask"://添加积分统计任务
                        result = AddScoreStatisticsTask(context);
                        break;

                    #region 用户等级配置管理
                    case "QueryUserLevelConfig":
                        result = QueryUserLevelConfig(context);
                        break;
                    case "AddUserLevelConfig":
                        result = AddUserLevelConfig(context);
                        break;
                    case "EditUserLevelConfig":
                        result = EditUserLevelConfig(context);
                        break;
                    case "DeleteUserLevelConfig":
                        result = DeleteUserLevelConfig(context);
                        break;

                    #endregion


                    #region 积分分类管理
                    case "QueryScoreTypeInfos":
                        result = QueryScoreTypeInfos(context);
                        break;
                    case "DeleteScoreTypeInfos":
                        result = DeleteScoreTypeInfos(context);
                        break;
                    case "ADScoreTypeInfo":
                        result = ADScoreTypeInfo(context);
                        break;
                    case "GetScoreTypeInfo":
                        result = GetScoreTypeInfo(context);
                        break;

                    #endregion


                    #region 分销提现管理

                    case "QueryWithrawCash":
                        result = QueryWithrawCash(context);
                        break;
                    case "UpdateWithrawCashStatus":
                        result = UpdateWithrawCashStatus(context);
                        break;
                    case "SetDistributionOwner":
                        result = SetDistributionOwner(context);
                        break;



                    #endregion


                    #region 群发图文素材管理
                    case "QueryWXMassArticle":
                        result = QueryWXMassArticle(context);
                        break;
                    case "AddWXMassArticle":
                        result = AddWXMassArticle(context);
                        break;
                    case "EditWXMassArticle":
                        result = EditWXMassArticle(context);
                        break;
                    case "DeleteWXMassArticle":
                        result = DeleteWXMassArticle(context);
                        break;

                    #endregion

                    case "UpdateWXMassArticleSortIndex"://修改文章活动排序号
                        result = UpdateWXMassArticleSortIndex(context);
                        break;


                    #region 幻灯片
                    case "QuerySlide"://查询幻灯片
                        result = QuerySlide(context);
                        break;
                    case "AddSlide"://添加幻灯片
                        result = AddSlide(context);
                        break;
                    case "EditSlide"://编辑幻灯片
                        result = EditSlide(context);
                        break;
                    case "DeleteSlide"://删除幻灯片
                        result = DeleteSlide(context);
                        break;
                    #endregion


                    #region 导航
                    case "QueryNavigation"://查询
                        result = QueryNavigation(context);
                        break;
                    case "AddNavigation"://添加
                        result = AddNavigation(context);
                        break;
                    case "EditNavigation"://编辑
                        result = EditNavigation(context);
                        break;
                    case "DeleteNavigation"://删除
                        result = DeleteNavigation(context);
                        break;

                    case "GeNavigationTree"://获取导航菜单
                        result = GeNavigationTree(context);
                        break;
                    #endregion



                    #region 关键字过滤
                    case "QueryFilterWord"://查询
                        result = QueryFilterWord(context);
                        break;
                    case "AddFilterWord"://添加
                        result = AddFilterWord(context);
                        break;
                    case "EditFilterWord"://编辑
                        result = EditFilterWord(context);
                        break;
                    case "DeleteFilterWord"://删除
                        result = DeleteFilterWord(context);
                        break;
                    #endregion

                    case "QueryKeyValueData"://查询键值对数据
                        result = QueryKeyValueData(context);
                        break;
                    case "SetAgent"://设置用户为代理商
                        result = SetAgent(context);
                        break;
                    case "SetChannel"://设置用户为渠道
                        result = SetChannel(context);
                        break;
                    case "QueryCoupon"://查询优惠券
                        result = QueryCoupon(context);
                        break;
                    case "AddCoupon"://添加优惠券
                        result = AddCoupon(context);
                        break;

                    case "QueryCouponV2"://查询优惠券
                        result = QueryCouponV2(context);
                        break;
                    case "AddCouponV2"://添加优惠券
                        result = AddCouponV2(context);
                        break;
                    case "SendCouponV2"://发放优惠券
                        result = SendCouponV2(context);
                        break;
                    case "DeleteCouponV2"://删除优惠券
                        result = DeleteCouponV2(context);
                        break;

                    #region 回收站
                    case "QueryJuActivityByDelete":
                        result = QueryJuActivityByDelete(context);//查询被删除的活动或文章
                        break;
                    case "RecoverJuActivity"://还原活动和文章
                        result = RecoverJuActivity(context);
                        break;

                    case "QueryWXMallProductInfoByDelete":
                        result = QueryWXMallProductInfoByDelete(context);//查询被删除的商品
                        break;
                    case "RestoreWXMallProductInfo":
                        result = RestoreWXMallProductInfo(context);//还原商品
                        break;
                    #endregion

                    case "UpdateDistributionMallConfig":
                        result = UpdateDistributionMallConfig(context);//配置商场分销
                        break;

                    case "QuerySendWxMsgList":
                        result = QuerySendWxMsgList(context);
                        break;

                    case "QuerySendWxMsgPlan":
                        result = QuerySendWxMsgPlan(context);
                        break;

                    case "GetDistributionWxQrcodeLimitUrl":
                        result = GetDistributionWxQrcodeLimitUrl(context);
                        break;

                    case "SetEmployee":
                        result = SetEmployee(context);
                        break;

                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                resp.ExStr = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);

            }

            context.Response.Write(result);
        }

        private string QueryKeyValueData(HttpContext context)
        {
            var dataType = context.Request["dataType"];

            var sqlWhere = new StringBuilder(" 1=1 ");

            sqlWhere.AppendFormat(" AND DataType in ({0}) ", Common.StringHelper.ListToStr<string>(dataType.Split(',').ToList(), "'", ","));

            var dataList = this.bllBase.GetList<KeyVauleDataInfo>(sqlWhere.ToString());

            return MySpider.JSONHelper.ObjectToJson(dataList);
        }

        /// <summary>
        /// 给导师设置文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavaArticleTutor(HttpContext context)
        {
            string autoId = context.Request["ids"];
            string userId = context.Request["userId"];
            int updateResultCount = bllJuActivity.Update(new BLLJIMP.Model.JuActivityInfo(), string.Format(" UserID='{0}'", userId), string.Format(" JuActivityID in ({0})", autoId));
            if (updateResultCount > 0)
            {
                BLLJIMP.Model.TutorInfo tutorInfo = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", userId));
                tutorInfo.WZNums = bllJuActivity.GetCount<JuActivityInfo>(string.Format(" UserID='{0}' And IsDelete=0 And IsHide=0 And WebsiteOwner='{1}' ", userId, bllBase.WebsiteOwner));
                bllJuActivity.Update(tutorInfo);
                resp.Status = 0;
                resp.Msg = "导师配置成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "导师配置失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 设置导师
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveTutor(HttpContext context)
        {
            string userId = context.Request["AutoId"];

            BLLJIMP.Model.UserInfo userInfo = bllUser.GetUserInfo(userId);
            if (userInfo != null)
            {
                BLLJIMP.Model.TutorInfo tutorInfo = new TutorInfo()
                {
                    TutorName = userInfo.TrueName,
                    TutorExplain = "",
                    TutorAnswers = 0,
                    TutorImg = userInfo.WXHeadimgurl,
                    TutorQuestions = 0,
                    RDataTime = DateTime.Now,
                    UserId = userInfo.UserID,
                    websiteOwner = bllBase.WebsiteOwner,
                    TradeStr = "0",
                    ProfessionalStr = "0"
                };
                if (bllUser.Add(tutorInfo))
                {
                    resp.Status = -1;
                    resp.Msg = "设置成功";
                }
                else
                {

                }

            }
            else
            {
                resp.Status = -1;
                resp.Msg = "系统错误，请联系管理员";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 添加到转发列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ActivityForward(HttpContext context)
        {

            string ids = context.Request["ids"]; //获取活动的编号
            string forwardType = context.Request["forward_type"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条记录进行操作！";
            }
            else
            {
                string[] activityIds = ids.Split(',');
                ////检查是否有推广人   0  有推广人 不等于0 没有推广人
                string count = "1";
                //foreach (string id in activityId)
                //{
                //    BLLJIMP.Model.JuActivityInfo jafmInfo1 = juActivityBll.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID='" + id + "'");
                //    BLLJIMP.Model.ActivityFieldMappingInfo afmInfo = juActivityBll.Get<BLLJIMP.Model.ActivityFieldMappingInfo>(" ActivityID='" + jafmInfo1.SignUpActivityID + "' AND MappingName='推广人'");
                //    if (afmInfo != null) Count = "0";
                //    else
                //    {
                //        Count = id; break;
                //    }
                //}

                //if (Count != "0")
                //{
                //    BLLJIMP.Model.JuActivityInfo jactivityInfo = juActivityBll.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID='" + Count + "'");
                //    resp.Status = -1;
                //    resp.Msg = "活动名称： ‘" + jactivityInfo.ActivityName + "’没有推广人，请您添加！！！";
                //    goto OutOf; //没有goto语句跳出不在执行
                //}


                //判断数据是否有重新添加  字段 Count   0  有重复数据 不等于0 没有
                //Count = "1";
                foreach (string id in activityIds)
                {
                    BLLJIMP.Model.JuActivityInfo juactInfo2 = bllJuActivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID='" + id + "'");
                    BLLJIMP.Model.ActivityForwardInfo activityForwardInfo = bllJuActivity.Get<BLLJIMP.Model.ActivityForwardInfo>(" ActivityID='" + juactInfo2.JuActivityID + "'");
                    if (activityForwardInfo == null) count = "0";
                    else
                    {
                        count = id;
                        break;
                    }
                }

                ToLog("ActivityForward1");

                // 字段 Count  字段 Count   0  有重复数据 不等于0 没有
                if (count != "0")
                {
                    BLLJIMP.Model.JuActivityInfo juActivityInfo = bllJuActivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID='" + count + "'");
                    resp.Status = -1;
                    resp.Msg = "活动名称： ‘" + juActivityInfo.ActivityName + "’已经转发，请您重新选择！";
                    goto OutOf; //没有goto语句跳出不在执行
                }

                ToLog("创建集合装载转发活动数据,ids:" + ids);
                //创建集合装载转发活动数据
                List<BLLJIMP.Model.ActivityForwardInfo> afIndfoList = new List<ActivityForwardInfo>();

                foreach (string id in activityIds)
                {

                    BLLJIMP.Model.JuActivityInfo jafInfo = bllJuActivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID='" + id + "' ");
                    //ToLog("构造数据：" + JsonConvert.SerializeObject(jafInfo));

                    int uv = 0, pv = 0;

                    try
                    {
                        uv = bllJuActivity.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" MonitorPlanID={0} ", jafInfo.MonitorPlanID));
                        pv = bllJuActivity.GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID={0} ", jafInfo.MonitorPlanID));
                    }
                    catch { }

                    afIndfoList.Add(
                        new ActivityForwardInfo
                        {
                            ActivityId = jafInfo.JuActivityID.ToString(),
                            ActivityName = jafInfo.ActivityName,
                            WebsiteOwner = bllBase.WebsiteOwner,
                            InsertDate = DateTime.Now,
                            ReadNum = jafInfo.PV,
                            UserId = currentUserInfo.UserID,
                            ThumbnailsPath = jafInfo.ThumbnailsPath,
                            ForwardType = forwardType,
                            UV = uv,
                            PV = pv
                        });



                }
                ToLog("插入数据");
                if (bllJuActivity.AddList<BLLJIMP.Model.ActivityForwardInfo>(afIndfoList))
                {
                    resp.Msg = "添加成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "添加失败";
                }
                ToLog("ActivityForward2");
            }

        OutOf:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        #region 初始化网站设置数据
        /// <summary>
        /// 设置默认数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DefaultData(HttpContext context)
        {

            DeleteOldData();
            bllWebsite.Add(SetConfigModelData());
            bllWebsite.AddList<BLLJIMP.Model.CompanyWebsite_Navigate>(SetNavigateModelData());
            bllWebsite.AddList<BLLJIMP.Model.CompanyWebsite_Projector>(SetProjectorModelData());
            bllWebsite.AddList<BLLJIMP.Model.CompanyWebsite_ToolBar>(ToolBarModelData());

            resp.Msg = "初始化成功！！！";
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除旧数据
        /// </summary>
        /// <returns></returns>
        private bool DeleteOldData()
        {

            bllWebsite.Delete(bllWebsite.Get<BLLJIMP.Model.CompanyWebsite_Config>(" WebsiteOwner='" + bllBase.WebsiteOwner + "'"));
            bllWebsite.Delete(bllWebsite.Get<BLLJIMP.Model.CompanyWebsite_Navigate>(" WebsiteOwner='" + bllBase.WebsiteOwner + "'"), " WebsiteOwner='" + bllBase.WebsiteOwner + "'");
            bllWebsite.Delete(bllWebsite.Get<BLLJIMP.Model.CompanyWebsite_Projector>(" WebsiteOwner='" + bllBase.WebsiteOwner + "'"), " WebsiteOwner='" + bllBase.WebsiteOwner + "'");
            bllWebsite.Delete(bllWebsite.Get<BLLJIMP.Model.CompanyWebsite_ToolBar>(" WebsiteOwner='" + bllBase.WebsiteOwner + "'"), " WebsiteOwner='" + bllBase.WebsiteOwner + "'");

            return true;
        }

        /// <summary>
        /// 设置网站配置信息
        /// </summary>
        /// <returns></returns>
        private BLLJIMP.Model.CompanyWebsite_Config SetConfigModelData()
        {
            BLLJIMP.Model.CompanyWebsite_Config defaultConfigModel = new CompanyWebsite_Config
            {
                WebsiteTitle = "公司名称或简称",
                Copyright = "解释归属权",
                WebsiteOwner = bllBase.WebsiteOwner,
                WebsiteImage = "/FileUpload/CompanyWebsite/log.jpg",
                WebsiteDescription = "公司口号、经营理念或服务宗旨等"
            };
            return defaultConfigModel;
        }

        /// <summary>
        /// 设置模块信息
        /// </summary>
        /// <returns></returns>
        private List<BLLJIMP.Model.CompanyWebsite_Navigate> SetNavigateModelData()
        {

            List<BLLJIMP.Model.CompanyWebsite_Navigate> navigateModelList = new List<CompanyWebsite_Navigate>
            {
                new BLLJIMP.Model.CompanyWebsite_Navigate{ NavigateName="模块1",NavigateDescription="",PlayIndex=0,NavigateImage="/FileUpload/CompanyWebsite/ab4d1f0f-3221-44ff-a437-19f7b7c90c00.jpg",IsShow="1",NavigateType="链接",NavigateTypeValue="",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_Navigate{ NavigateName="模块2",NavigateDescription="",PlayIndex=0,NavigateImage="/FileUpload/CompanyWebsite/ab4d1f0f-3221-44ff-a437-19f7b7c90c00.jpg",IsShow="1",NavigateType="链接",NavigateTypeValue="",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_Navigate{ NavigateName="模块3",NavigateDescription="",PlayIndex=0,NavigateImage="/FileUpload/CompanyWebsite/ab4d1f0f-3221-44ff-a437-19f7b7c90c00.jpg",IsShow="1",NavigateType="链接",NavigateTypeValue="",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_Navigate{ NavigateName="模块4",NavigateDescription="",PlayIndex=0,NavigateImage="/FileUpload/CompanyWebsite/ab4d1f0f-3221-44ff-a437-19f7b7c90c00.jpg",IsShow="1",NavigateType="链接",NavigateTypeValue="",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_Navigate{ NavigateName="模块5",NavigateDescription="",PlayIndex=0,NavigateImage="/FileUpload/CompanyWebsite/ab4d1f0f-3221-44ff-a437-19f7b7c90c00.jpg",IsShow="1",NavigateType="链接",NavigateTypeValue="",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_Navigate{ NavigateName="模块6",NavigateDescription="",PlayIndex=0,NavigateImage="/FileUpload/CompanyWebsite/ab4d1f0f-3221-44ff-a437-19f7b7c90c00.jpg",IsShow="1",NavigateType="链接",NavigateTypeValue="",WebsiteOwner=bllBase.WebsiteOwner},
            };
            return navigateModelList;
        }

        /// <summary>
        /// 设置幻灯片
        /// </summary>
        /// <returns></returns>
        private List<BLLJIMP.Model.CompanyWebsite_Projector> SetProjectorModelData()
        {
            List<BLLJIMP.Model.CompanyWebsite_Projector> projectorModelList = new List<CompanyWebsite_Projector>
            {
                new BLLJIMP.Model.CompanyWebsite_Projector{ ProjectorName="幻灯片1",ProjectorDescription="",PlayIndex=1,ProjectorImage="/FileUpload/CompanyWebsite/70be1785-be73-46a5-8c84-dc4adbf37cf5.jpg",IsShow="1",ProjectorType="链接",ProjectorTypeValue="http://www.qq.com",WebsiteOwner=bllBase.WebsiteOwner },
                new BLLJIMP.Model.CompanyWebsite_Projector{ ProjectorName="幻灯片2",ProjectorDescription="",PlayIndex=1,ProjectorImage="/FileUpload/CompanyWebsite/70be1785-be73-46a5-8c84-dc4adbf37cf5.jpg",IsShow="1",ProjectorType="链接",ProjectorTypeValue="http://www.qq.com",WebsiteOwner=bllBase.WebsiteOwner },
                new BLLJIMP.Model.CompanyWebsite_Projector{ ProjectorName="幻灯片3",ProjectorDescription="",PlayIndex=1,ProjectorImage="/FileUpload/CompanyWebsite/70be1785-be73-46a5-8c84-dc4adbf37cf5.jpg",IsShow="1",ProjectorType="链接",ProjectorTypeValue="http://www.qq.com",WebsiteOwner=bllBase.WebsiteOwner }
            };
            return projectorModelList;
        }

        /// <summary>
        /// 设置底部工具栏
        /// </summary>
        private List<BLLJIMP.Model.CompanyWebsite_ToolBar> ToolBarModelData()
        {
            List<BLLJIMP.Model.CompanyWebsite_ToolBar> toolBarModelList = new List<CompanyWebsite_ToolBar> 
            {
                new BLLJIMP.Model.CompanyWebsite_ToolBar{ ToolBarName="主页",ToolBarDescription="",ToolBarImage="ico_home",PlayIndex=0, ToolBarType="链接", ToolBarTypeValue="/web/index.aspx", IsShow="1",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_ToolBar{ ToolBarName="联系我们",ToolBarDescription="",ToolBarImage="ico_phone",PlayIndex=1, ToolBarType="电话", ToolBarTypeValue="",IsShow="1",WebsiteOwner=bllBase.WebsiteOwner},
                new BLLJIMP.Model.CompanyWebsite_ToolBar{ ToolBarName="加入我们",ToolBarDescription="", ToolBarImage="ico_note", PlayIndex=2, ToolBarType="短信", ToolBarTypeValue="",IsShow="1",WebsiteOwner=bllBase.WebsiteOwner}
            };
            return toolBarModelList;
        }
        #endregion


        /// <summary>
        /// 获取指定对话消息
        /// </summary>
        /// <param name="context"></param>
        /// <returns>返回消息主记录及图文消息记录</returns>
        private string GetMsgDetails(HttpContext context)
        {
            string msgId = context.Request["msgId"];
            if (string.IsNullOrWhiteSpace(msgId))
            {
                resp.Status = -1;
                resp.Msg = "传入ID不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            WeixinMsgDetails msgModel = new WeixinMsgDetails();
            List<WeixinMsgDetailsImgsInfo> msgNewsList = new List<WeixinMsgDetailsImgsInfo>();

            msgModel = this.bllWeixin.Get<WeixinMsgDetails>(string.Format(" UID = '{0}' ", msgId));

            if (msgModel == null)
            {
                resp.Status = -2;
                resp.Msg = "消息记录不存在!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            msgNewsList = this.bllWeixin.GetList<WeixinMsgDetailsImgsInfo>(string.Format(" MsgID = '{0}' ", msgId));

            List<object> result = new List<object>();
            result.Add(msgModel);


            if (msgNewsList != null)
                result.Add(msgNewsList);

            resp.Status = 1;
            resp.ExObj = result;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询消息对话
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryMsgDetails(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string loadType = context.Request["loadType"];
            List<WeixinMsgDetails> data = new List<WeixinMsgDetails>();
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");

            sbWhere.AppendFormat(" AND  WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);

            switch (loadType)
            {
                case "all":
                    break;
                case "today"://今天
                    sbWhere.AppendFormat(" and DATEDIFF(DAY,ReceiveDate,GETDATE()) = 0 ");
                    break;
                case "yestoday"://昨天
                    sbWhere.AppendFormat(" and DATEDIFF(DAY,ReceiveDate,GETDATE()) = 1 ");
                    break;
                case "beforeyestody"://前天
                    sbWhere.AppendFormat(" and DATEDIFF(DAY,ReceiveDate,GETDATE()) = 2 ");
                    break;
                default:
                    break;
            }

            data = this.bllWeixin.GetLit<WeixinMsgDetails>(pageSize, pageIndex, sbWhere.ToString(), "UID DESC");

            for (int i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i].WXNickname))
                {
                    UserInfo userInfo = this.bllWeixin.GetWeixinInfoToUserInfo(currWebSiteUserInfo.UserID, this.currWebSiteUserInfo.WeixinAppId, this.currWebSiteUserInfo.WeixinAppSecret, data[i].ReceiveOpenID);
                    data[i].WXNickname = userInfo.WXNickname;
                    data[i].WXHeadimgurlLocal = userInfo.WXHeadimgurlLocal;
                }

            }

            resp.ExObj = data;
            resp.ExInt = this.bllWeixin.GetTotalPage(this.bllWeixin.GetCount<WeixinMsgDetails>(sbWhere.ToString()), pageSize);

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWebSiteInfo(HttpContext context)
        {
            currentWebSiteInfo = bllBase.GetWebsiteInfoModelFromDataBase();
            return Common.JSONHelper.ObjectToJson(currentWebSiteInfo);
        }
        /// <summary>
        /// 更新站点信息超级管理员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWebSiteInfo(HttpContext context)
        {
            currentWebSiteInfo.ArticleCate1 = context.Request["ArticleCate1"];
            currentWebSiteInfo.ArticleCate2 = context.Request["ArticleCate2"];
            currentWebSiteInfo.ArticleCate3 = context.Request["ArticleCate3"];
            currentWebSiteInfo.ArticleCate4 = context.Request["ArticleCate4"];
            currentWebSiteInfo.ArticleCate5 = context.Request["ArticleCate5"];

            currentWebSiteInfo.CourseCate1 = context.Request["CourseCate1"];
            currentWebSiteInfo.CourseCate2 = context.Request["CourseCate2"];

            currentWebSiteInfo.CourseManageMenuRName = context.Request["CourseManageMenuRName"];
            currentWebSiteInfo.ArticleManageMenuRName = context.Request["ArticleManageMenuRName"];
            currentWebSiteInfo.MasterManageMenuRName = context.Request["MasterManageMenuRName"];
            currentWebSiteInfo.QuestionManageMenuRName = context.Request["QuestionManageMenuRName"];
            currentWebSiteInfo.ActivityManageMenuRName = context.Request["ActivityManageMenuRName"];
            currentWebSiteInfo.SignUpCourseMenuRName = context.Request["SignUpCourseMenuRName"];

            currentWebSiteInfo.WebsiteName = context.Request["WebsiteName"];
            currentWebSiteInfo.UserManageMenuRName = context.Request["UserManageMenuRName"];
            currentWebSiteInfo.MallMenuRName = context.Request["MallMenuRName"];
            currentWebSiteInfo.WebSiteStatisticsMenuRName = context.Request["WebSiteStatisticsMenuRName"];
            currentWebSiteInfo.AddVMenuRName = context.Request["AddVMenuRName"];
            currentWebSiteInfo.MonitorMenuRName = context.Request["MonitorMenuRName"];
            currentWebSiteInfo.ArticleHeadCode = context.Request["ArticleHeadCode"];
            currentWebSiteInfo.ArticleBottomCode = context.Request["ArticleBottomCode"];
            if (this.bllJuActivity.Update(currentWebSiteInfo))
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
            }
            else
            {
                resp.Status = 1;
                resp.Msg = "更新失败！";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 更新站点信息 网站所有者
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWebSiteInfoPersonal(HttpContext context)
        {
            currentWebSiteInfo.WebsiteName = context.Request["WebsiteName"];
            currentWebSiteInfo.ArticleHeadCode = context.Request["ArticleHeadCode"];
            currentWebSiteInfo.ArticleBottomCode = context.Request["ArticleBottomCode"];
            currentWebSiteInfo.WebsiteLogo = context.Request["WebsiteLogo"];
            currentWebSiteInfo.IsHideAdminLogoAndTop = Convert.ToInt32(context.Request["IsHideAdminLogoAndTop"]);

            if (this.bllJuActivity.Update(currentWebSiteInfo))
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.System, BLLJIMP.Enums.EnumLogTypeAction.Config, bllLog.GetCurrUserID(), "全局设置[" + bllLog.GetCurrUserID() + "]");
            }
            else
            {
                resp.Status = 1;
                resp.Msg = "更新失败！";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 启用禁用账户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisableUser(HttpContext context)
        {
            string userIdsStr = context.Request["userIds"];
            int isDisable = Convert.ToInt32(context.Request["disableValue"]);
            int updateCount = this.bllUser.Update(new UserInfo(), string.Format(" IsDisable = {0} ", isDisable), string.Format(" UserID in ({0}) ", userIdsStr));
            if (updateCount > 0)
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
        /// 获取指定用户权限组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserAllPmsGroup(HttpContext context)
        {
            string userId = context.Request["inputUserID"];
            List<long> groupIDList = this.pmsBll.GetPmsGroupIDByUser(userId);

            resp.Status = 1;
            resp.Msg = Common.StringHelper.ListToStr<long>(groupIDList, "", ",");

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetSysUserPms(HttpContext context)
        {
            string userId = context.Request["UserID"];
            string groupIdListStr = context.Request["Ids"];
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(groupIdListStr))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            List<long> groupIDList = new List<long>();

            string[] arr = { "," };

            groupIDList = groupIdListStr.Split(arr, StringSplitOptions.RemoveEmptyEntries).Select(p => long.Parse(p)).ToList();

            int result = this.pmsBll.SetUserPmsGroup(userId, groupIDList, true);

            resp.Status = result;
            resp.Msg = "设置成功!";

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 编辑用户 超级管理员使用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditSysUser(HttpContext context)
        {
            string userId = context.Request["UserID"];
            UserInfo userModel = this.bllUser.GetUserInfo(userId);
            if (userModel == null)
            {
                resp.Status = -3;
                resp.Msg = "用户不存在！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            userModel.TrueName = context.Request["TrueName"];
            if (this.currentUserInfo.UserType == 1)
                userModel.WebsiteOwner = string.IsNullOrWhiteSpace(context.Request["WebsiteOwner"]) ? bllBase.WebsiteOwner : context.Request["WebsiteOwner"];
            else
                userModel.WebsiteOwner = bllBase.WebsiteOwner;
            userModel.Company = context.Request["Company"];
            userModel.Phone = context.Request["Phone"];
            userModel.Phone3 = context.Request["Phone3"];
            userModel.Postion = context.Request["Postion"];
            userModel.WeixinIsAdvancedAuthenticate = Convert.ToInt32(context.Request["WeixinIsAdvancedAuthenticate"]);


            userModel.Address = context.Request["Address"];

            userModel.Province = context.Request["Province"];
            userModel.City = context.Request["City"];
            userModel.District = context.Request["District"];

            userModel.Ex1 = context.Request["Ex1"];
            userModel.Ex2 = context.Request["Ex2"];
            userModel.Ex3 = context.Request["Ex3"];
            userModel.Ex4 = context.Request["Ex4"];
            userModel.Ex5 = context.Request["Ex5"];
            userModel.Ex6 = context.Request["Ex6"];
            userModel.Ex7 = context.Request["Ex7"];
            userModel.Ex8 = context.Request["Ex8"];
            userModel.Ex9 = context.Request["Ex9"];
            userModel.Ex10 = context.Request["Ex10"];

            if (string.IsNullOrWhiteSpace(userModel.UserID) || string.IsNullOrWhiteSpace(userModel.Password))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllUser.Update(userModel, string.Format(" TrueName='{0}',WebsiteOwner='{1}',Company='{2}',Phone='{3}',Phone3='{4}',Postion='{5}',WeixinIsAdvancedAuthenticate={6},Address='{7}',Province='{8}',City='{9}',District='{10}',Ex1='{11}',Ex2='{12}',Ex3='{13}',Ex4='{14}',Ex5='{15}',Ex6='{16}',Ex7='{17}',Ex8='{18}',Ex9='{19}',Ex10='{20}'",
                userModel.TrueName,
                userModel.WebsiteOwner,
                userModel.Company,
                userModel.Phone,
                userModel.Phone3,
                userModel.Postion,
                userModel.WeixinIsAdvancedAuthenticate,
                userModel.Address,
                userModel.Province,
                userModel.City,
                userModel.District,
                userModel.Ex1,
                userModel.Ex2,
                userModel.Ex3,
                userModel.Ex4,
                userModel.Ex5,
                userModel.Ex6,
                userModel.Ex7,
                userModel.Ex8,
                userModel.Ex9,
                userModel.Ex10

                ), string.Format(" AutoID={0}", userModel.AutoID)) == 1)
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
                var relAutoRegNewWxUser = bllCommRelation.GetRelationInfo(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");
                bool hWXAuthPageMustLogin = bllCommRelation.ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                if (userModel.WeixinIsAdvancedAuthenticate == 0)
                {
                    if (relAutoRegNewWxUser != null) bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");

                    int wxAuthPageMustLogin = Convert.ToInt32(context.Request["WXAuthPageMustLogin"]);
                    if (wxAuthPageMustLogin == 1 && !hWXAuthPageMustLogin)
                    {
                        bllCommRelation.AddCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                    }
                    else if (wxAuthPageMustLogin == 0 && hWXAuthPageMustLogin)
                    {
                        bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                    }
                }
                else
                {
                    if (hWXAuthPageMustLogin) bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");

                    int autoRegNewWxUser = Convert.ToInt32(context.Request["AutoRegNewWxUser"]);
                    if (autoRegNewWxUser == 1 || autoRegNewWxUser == 2)
                    {
                        if (relAutoRegNewWxUser != null)
                        {
                            bllCommRelation.ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "", (autoRegNewWxUser == 2 ? "1" : ""));
                        }
                        else
                        {
                            bllCommRelation.AddCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "", (autoRegNewWxUser == 2 ? "1" : ""));
                        }
                    }
                    else if (autoRegNewWxUser == 0 && relAutoRegNewWxUser != null)
                    {
                        bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");
                    }
                }
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }

        /// <summary>
        /// 编辑用户 超级管理员使用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SuperEditSysUser(HttpContext context)
        {
            string userId = context.Request["UserID"];

            if (currentUserInfo.UserType != 1)
            {
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.NoPms;
                resp.Msg = "无权更改";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            UserInfo userModel = this.bllUser.GetUserInfo(userId, userId);

            if (userModel == null)
            {
                resp.Status = -3;
                resp.Msg = "用户不存在！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            userModel.TrueName = context.Request["TrueName"];
            if (this.currentUserInfo.UserType == 1)
                userModel.WebsiteOwner = string.IsNullOrWhiteSpace(context.Request["WebsiteOwner"]) ? bllBase.WebsiteOwner : context.Request["WebsiteOwner"];
            else
                userModel.WebsiteOwner = bllBase.WebsiteOwner;
            userModel.Company = context.Request["Company"];
            userModel.Phone = context.Request["Phone"];
            userModel.Phone3 = context.Request["Phone3"];
            userModel.Postion = context.Request["Postion"];
            userModel.WeixinIsAdvancedAuthenticate = Convert.ToInt32(context.Request["WeixinIsAdvancedAuthenticate"]);


            userModel.Address = context.Request["Address"];

            userModel.Province = context.Request["Province"];
            userModel.City = context.Request["City"];
            userModel.District = context.Request["District"];

            userModel.Ex1 = context.Request["Ex1"];
            userModel.Ex2 = context.Request["Ex2"];
            userModel.Ex3 = context.Request["Ex3"];
            userModel.Ex4 = context.Request["Ex4"];
            userModel.Ex5 = context.Request["Ex5"];
            userModel.Ex6 = context.Request["Ex6"];
            userModel.Ex7 = context.Request["Ex7"];
            userModel.Ex8 = context.Request["Ex8"];
            userModel.Ex9 = context.Request["Ex9"];
            userModel.Ex10 = context.Request["Ex10"];

            if (string.IsNullOrWhiteSpace(userModel.UserID) || string.IsNullOrWhiteSpace(userModel.Password))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllUser.Update(userModel, string.Format(" TrueName='{0}',WebsiteOwner='{1}',Company='{2}',Phone='{3}',Phone3='{4}',Postion='{5}',WeixinIsAdvancedAuthenticate={6},Address='{7}',Province='{8}',City='{9}',District='{10}',Ex1='{11}',Ex2='{12}',Ex3='{13}',Ex4='{14}',Ex5='{15}',Ex6='{16}',Ex7='{17}',Ex8='{18}',Ex9='{19}',Ex10='{20}'",
                userModel.TrueName,
                userModel.WebsiteOwner,
                userModel.Company,
                userModel.Phone,
                userModel.Phone3,
                userModel.Postion,
                userModel.WeixinIsAdvancedAuthenticate,
                userModel.Address,
                userModel.Province,
                userModel.City,
                userModel.District,
                userModel.Ex1,
                userModel.Ex2,
                userModel.Ex3,
                userModel.Ex4,
                userModel.Ex5,
                userModel.Ex6,
                userModel.Ex7,
                userModel.Ex8,
                userModel.Ex9,
                userModel.Ex10

                ), string.Format(" AutoID={0}", userModel.AutoID)) == 1)
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
                var isNotAutoRegNewWxUser = bllCommRelation.GetRelationInfo(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");
                bool isMustPageLogin = bllCommRelation.ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                if (userModel.WeixinIsAdvancedAuthenticate == 0)
                {
                    #region 没有高级授权
                    if (isNotAutoRegNewWxUser != null) bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");

                    int wxAuthPageMustLogin = Convert.ToInt32(context.Request["WXAuthPageMustLogin"]);
                    if (wxAuthPageMustLogin == 1 && !isMustPageLogin)
                    {
                        bllCommRelation.AddCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                    }
                    else if (wxAuthPageMustLogin == 0 && isMustPageLogin)
                    {
                        bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                    }
                    #endregion
                }
                else
                {
                    #region 有高级授权
                    if (isMustPageLogin) bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");

                    /*
                     * 
                     TODO: AutoRegNewWxUser 是否是下面的意思？
                        0 自动注册
                        1 不自动注册 会跳到注册页
                        2 不自动注册  不跳到注册页          
                    
                    但是读取判断的地方很简单 就原来的 WebsiteIsNotAutoRegNewWxUser 加上 ExpandId = 1，这边却写得很复杂
                    
                    bllCommRelation里面的 GetIsNotAutoRegNewWxUser 返回的 0 1 2 又是代表什么意思？
                                                   
                     */
                    int autoRegNewWxUser = Convert.ToInt32(context.Request["AutoRegNewWxUser"]);
                    if (autoRegNewWxUser == 1 || autoRegNewWxUser == 2)
                    {
                        if (isNotAutoRegNewWxUser != null)
                        {
                            isNotAutoRegNewWxUser.ExpandId = (autoRegNewWxUser == 2 ? "1" : "");
                            bllCommRelation.Update(isNotAutoRegNewWxUser);
                        }
                        else
                        {
                            bllCommRelation.AddCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "", (autoRegNewWxUser == 2 ? "1" : ""));
                        }
                    }
                    else if (autoRegNewWxUser == 0 && isNotAutoRegNewWxUser != null)
                    {
                        bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");
                    }
                    #endregion
                }
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }


        /// <summary>
        /// 编辑用户 站点管理员使用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWebSiteMember(HttpContext context)
        {
            string userId = context.Request["UserID"];
            string totalScoreStr = context.Request["TotalScore"];
            string wxName = context.Request["WeixinName"];
            string trueName = context.Request["TrueName"];
            string phone = context.Request["Phone"];
            string company = context.Request["Company"];
            string availableVoteCountStr = context.Request["AvailableVoteCount"];
            double totalScoreD = 0;
            int availableVoteCountInt = 0;
            UserInfo userModel = this.bllUser.GetUserInfo(userId, bllBase.WebsiteOwner);
            if (userModel == null)
            {
                resp.Status = -3;
                resp.Msg = "用户不存在！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(totalScoreStr))
            {
                if (!double.TryParse(totalScoreStr, out totalScoreD))
                {
                    resp.Status = 0;
                    resp.Msg = "积分不正确！";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }

            //if (bllBase.WebsiteOwner.Equals("wubuhui"))
            //{
            //    int tempScore = (int)(totalScoreD - userModel.TotalScore);
            //    if (tempScore != 0)
            //    {
            //        //加入积分记录
            //        BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
            //        {
            //            InsertDate = DateTime.Now,
            //            ScoreNum = tempScore.ToString(),
            //            WebsiteOwner = bllBase.WebsiteOwner,
            //            UserId = userModel.UserID,
            //            NameStr = "系统积分",
            //            Nums = "b55",
            //            RecordType = tempScore > 0 ? "2" : "1",
            //        };
            //        bllBase.Add(srInfo);
            //        //加入积分记录

            //    }

            //}

            if (!string.IsNullOrEmpty(availableVoteCountStr))
            {
                if (!int.TryParse(availableVoteCountStr, out availableVoteCountInt))
                {
                    resp.Status = 0;
                    resp.Msg = "票数不正确！";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }

            //if (AvailableVoteCountInt != 0)
            //{
            userModel.AvailableVoteCount = availableVoteCountInt;
            //}
            //if (TotalScoreD != 0)
            //{
            userModel.TotalScore = totalScoreD;
            userModel.WXNickname = wxName;
            userModel.TrueName = trueName;
            userModel.Phone = phone;
            userModel.Company = company;
            //}
            if (this.bllUser.Update(userModel, string.Format(" AvailableVoteCount={0},TotalScore={1},WXNickname='{2}',TrueName='{3}',Phone='{4}',Company='{5}'", userModel.AvailableVoteCount, userModel.TotalScore, userModel.WXNickname, userModel.TrueName, userModel.Phone, userModel.Company), string.Format(" AutoID={0}", userModel.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }

        /// <summary>
        /// 添加或减少积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddScore(HttpContext context)
        {
            string autoIds = context.Request["ids"];
            string addScore = context.Request["AddScore"];
            string moduleName = context.Request["module"];
            string descript = context.Request["Descript"];
            double addScoreD = 0;
            string module = "积分";
            if (string.IsNullOrEmpty(autoIds))
            {
                resp.Status = 0;
                resp.Msg = "请检查参数！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(moduleName))
            {
                module = moduleName;
            }
            if (!string.IsNullOrEmpty(addScore))
            {
                if (!double.TryParse(addScore, out addScoreD))
                {
                    resp.Status = 0;
                    resp.Msg = module + "不正确！";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }

            string[] ids = autoIds.Split(',');
            foreach (var item in ids)
            {
                UserInfo userModel = bllUser.GetUserInfoByAutoID(int.Parse(item), bllUser.WebsiteOwner);
                if (userModel == null)
                {
                    resp.Status = -3;
                    resp.Msg = "用户不存在！";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                double newScore = userModel.TotalScore;
                string str = string.Empty;
                if ((userModel.TotalScore + addScoreD) < 0)
                {
                    userModel.TotalScore = 0;
                    str = "-";
                }
                else
                {
                    userModel.TotalScore += addScoreD;
                    newScore = addScoreD;
                    str = "+";
                }

                //允许负积分
                //double newScore = addScoreD;
                //string str = string.Empty;
                //if ((userModel.TotalScore + addScoreD) < 0)
                //{
                //    userModel.TotalScore += addScoreD;
                //    str = "-";
                //}
                //else
                //{
                //    userModel.TotalScore += addScoreD;
                //    str = "+";
                //}



                if (currentWebSiteInfo.IsUnionHongware == 1)
                {
                    Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(currentWebSiteInfo.WebsiteOwner);
                    if (!hongWareClient.UpdateMemberScore(userModel.Phone, userModel.WXOpenId, (float)newScore))
                    {
                        resp.Status = 0;
                        resp.Msg = "更新宏巍积分失败！";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }

                }
                if (bllUser.Update(userModel, string.Format(" TotalScore={0}", userModel.TotalScore), string.Format(" AutoID={0}", item)) > 0)
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Update, bllUser.GetCurrUserID(), "[" + bllUser.GetCurrUserID() + "]修改了用户[" + userModel.UserID + "]积分[" + str + "" + newScore + "]");
                    resp.Status = 1;
                    resp.Msg = "更新成功！";

                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.UserID = userModel.UserID;
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.TotalScore = userModel.TotalScore;

                    //--------不允许负积分-----------
                    if (str == "-")
                        scoreRecord.Score = -newScore;
                    else
                        scoreRecord.Score = newScore;
                    //-------------------------------

                    //--------允许负积分-----------
                    //scoreRecord.Score = newScore;
                    //-----------------------------

                    scoreRecord.ScoreType = "AdminSubmit";
                    scoreRecord.AddNote = descript;
                    scoreRecord.TotalScore = userModel.TotalScore;
                    scoreRecord.RelationID = bllMall.GetCurrUserID();
                    scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                    bllBase.Add(scoreRecord);

                    if (!string.IsNullOrEmpty(moduleName) && moduleName == "淘股币")
                    {
                        SystemNotice systemNoticeGet = new SystemNotice();
                        systemNoticeGet.Title = descript;
                        systemNoticeGet.Ncontent = descript;
                        systemNoticeGet.NoticeType = (int)BLLSystemNotice.NoticeType.GetReward;
                        systemNoticeGet.InsertTime = DateTime.Now;
                        systemNoticeGet.WebsiteOwner = bllSystemNotice.WebsiteOwner;
                        systemNoticeGet.SendType = 2;
                        systemNoticeGet.UserId = userModel.UserID;
                        systemNoticeGet.Receivers = userModel.UserID;
                        bllSystemNotice.Add(systemNoticeGet);
                    }
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "更新失败！";
                }
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddSysUser(HttpContext context)
        {
            var userType = Convert.ToInt32(context.Request["UserType"]);
            UserInfo userModel = new UserInfo();
            userModel.UserID = context.Request["UserID"];
            userModel.Password = context.Request["Pwd"];
            userModel.TrueName = context.Request["TrueName"];
            if (this.currentUserInfo.UserType == 1)
                userModel.WebsiteOwner = string.IsNullOrWhiteSpace(context.Request["WebsiteOwner"]) ? bllBase.WebsiteOwner : context.Request["WebsiteOwner"];
            else
                userModel.WebsiteOwner = bllBase.WebsiteOwner;

            userModel.Company = context.Request["Company"];
            userModel.Phone = context.Request["Phone"];
            userModel.Phone3 = context.Request["Phone3"];
            userModel.Postion = context.Request["Postion"];

            if (userType >= 2)
            {
                userModel.UserType = userType;
            }
            else
            {
                userModel.UserType = 2;
            }

            var isSubAccount = context.Request["isSubAccount"];

            if (isSubAccount != null && isSubAccount == "1")
            {
                userModel.IsSubAccount = "1";
            }

            userModel.Address = context.Request["Address"];

            userModel.Province = context.Request["Province"];
            userModel.City = context.Request["City"];
            userModel.District = context.Request["District"];

            userModel.Ex1 = context.Request["Ex1"];
            userModel.Ex2 = context.Request["Ex2"];
            userModel.Ex3 = context.Request["Ex3"];
            userModel.Ex4 = context.Request["Ex4"];
            userModel.Ex5 = context.Request["Ex5"];
            userModel.Ex6 = context.Request["Ex6"];
            userModel.Ex7 = context.Request["Ex7"];
            userModel.Ex8 = context.Request["Ex8"];
            userModel.Ex9 = context.Request["Ex9"];
            userModel.Ex10 = context.Request["Ex10"];

            userModel.RegIP = Common.MySpider.GetClientIP();
            userModel.Regtime = DateTime.Now;
            userModel.LoginTotalCount = 0;

            userModel.LastLoginDate = DateTime.Now;
            userModel.WeixinIsAdvancedAuthenticate = Convert.ToInt32(context.Request["WeixinIsAdvancedAuthenticate"]);
            if (string.IsNullOrWhiteSpace(userModel.UserID) || string.IsNullOrWhiteSpace(userModel.Password))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllUser.Exists(userModel, "UserID"))
            {
                resp.Status = -2;
                resp.Msg = "用户名" + userModel.UserID + "已存在！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (string.IsNullOrEmpty(userModel.WebsiteOwner))
            {
                //resp.Status = -2;
                //resp.Msg ="请输入网站所有者!";
                //return Common.JSONHelper.ObjectToJson(resp);
                userModel.WebsiteOwner = userModel.UserID;

            }
            //if (userBll.GetUserInfo(userModel.WebsiteOwner)==null)
            //{
            //    resp.Status = -2;
            //    resp.Msg = "网站所有者用户名不在，请检查!";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}


            if (this.bllUser.Add(userModel))
            {
                resp.Status = 1;
                resp.Msg = "添加成功！";

                var relAutoRegNewWxUser = bllCommRelation.GetRelationInfo(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");
                bool hWXAuthPageMustLogin = bllCommRelation.ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                if (userModel.WeixinIsAdvancedAuthenticate == 0)
                {
                    if (relAutoRegNewWxUser != null) bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");

                    int wxAuthPageMustLogin = Convert.ToInt32(context.Request["WXAuthPageMustLogin"]);
                    if (wxAuthPageMustLogin == 1 && !hWXAuthPageMustLogin)
                    {
                        bllCommRelation.AddCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                    }
                    else if (wxAuthPageMustLogin == 0 && hWXAuthPageMustLogin)
                    {
                        bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");
                    }
                }
                else
                {
                    if (hWXAuthPageMustLogin) bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, userModel.UserID, "");

                    int autoRegNewWxUser = Convert.ToInt32(context.Request["AutoRegNewWxUser"]);
                    if (autoRegNewWxUser == 1 || autoRegNewWxUser == 2)
                    {
                        if (relAutoRegNewWxUser != null)
                        {
                            bllCommRelation.ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "", (autoRegNewWxUser == 2 ? "1" : ""));
                        }
                        else
                        {
                            bllCommRelation.AddCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "", (autoRegNewWxUser == 2 ? "1" : ""));
                        }
                    }
                    else if (autoRegNewWxUser == 0 && relAutoRegNewWxUser != null)
                    {
                        bllCommRelation.DelCommRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, userModel.UserID, "");
                    }
                }

                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QuerySysUserInfo(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string isShowAll = context.Request["ShowAll"];
            string userId = context.Request["UserId"];
            StringBuilder sbWhere = new StringBuilder("1=1");
            if (isShowAll == "0")
            {
                sbWhere.AppendFormat(" And UserId =WebSiteOwner ", userId);
            }
            //sbWhere.AppendFormat(" WebSiteOwner='{0}' ", bllJuActivity.WebsiteOwner);//bllJuActivity.WebsiteOwner
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserID ='{0}' ", userId);
            }
            int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            List<UserInfo> data = bllUser.GetColList<UserInfo>(pageSize, pageIndex, sbWhere.ToString()
                , "AutoID desc", "AutoID,UserID,TrueName,WXNickname,Company,Phone,Phone3,Postion,WebsiteOwner,WeixinIsAdvancedAuthenticate");
            var users = from p in data
                        select new
                        {
                            p.AutoID,
                            p.UserID,
                            p.TrueName,
                            p.WXNickname,
                            p.Company,
                            p.Phone,
                            p.Phone3,
                            p.Postion,
                            p.WebsiteOwner,
                            p.WeixinIsAdvancedAuthenticate,
                            AutoRegNewWxUser = bllCommRelation.GetIsNotAutoRegNewWxUser(p.UserID),
                            WXAuthPageMustLogin = bllCommRelation.ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, p.UserID, "")
                        };
            var result = new
            {
                total = totalCount,
                rows = users
            };
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 添加站点
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWebsite(HttpContext context)
        {

            WebsiteInfo model = new WebsiteInfo();
            model.WebsiteName = context.Request["WebsiteName"];
            model.WebsiteOwner = context.Request["WebsiteOwner"];
            model.WebsiteDescription = context.Request["WebsiteDescription"];
            model.MaxSubAccountCount = int.Parse(context.Request["MaxSubAccountCount"]);
            model.LogLimitDay = int.Parse(context.Request["LogLimitDay"]);
            int isEnableLimitProductBuyTime = int.Parse(context.Request["IsEnableLimitProductBuyTime"]);
            int isEnableAmountPay = int.Parse(context.Request["IsEnableAmountPay"]);
            string accountAmountPayShowName = context.Request["AccountAmountPayShowName"];
            string totalAmountShowName = context.Request["TotalAmountShowName"];
            if (!string.IsNullOrWhiteSpace(context.Request["WebsiteExpirationDate"]))
            {
                model.WebsiteExpirationDate = Convert.ToDateTime(context.Request["WebsiteExpirationDate"]);
            }
            int distributionMemberStandardsHaveParent = !string.IsNullOrEmpty(context.Request["DistributionMemberStandardsHaveParent"]) ? int.Parse(context.Request["DistributionMemberStandardsHaveParent"]) : 0;//分销会员标准 有上级
            int distributionMemberStandardsHavePay = !string.IsNullOrEmpty(context.Request["DistributionMemberStandardsHavePay"]) ? int.Parse(context.Request["DistributionMemberStandardsHavePay"]) : 0;//分销会员标准 有付款的订单
            int distributionMemberStandardsHaveSuccessOrder = !string.IsNullOrEmpty(context.Request["DistributionMemberStandardsHaveSuccessOrder"]) ? int.Parse(context.Request["DistributionMemberStandardsHaveSuccessOrder"]) : 0;//分销会员标准 有交易完成的订单

            int distributionRelationBuildQrCode = !string.IsNullOrEmpty(context.Request["DistributionRelationBuildQrCode"]) ? int.Parse(context.Request["DistributionRelationBuildQrCode"]) : 0;//分销关系规则建立 二维码
            int distributionRelationBuildSpreadActivity = !string.IsNullOrEmpty(context.Request["DistributionRelationBuildSpreadActivity"]) ? int.Parse(context.Request["DistributionRelationBuildSpreadActivity"]) : 0;////分销关系规则建立 转发活动
            int distributionRelationBuildMallOrder = !string.IsNullOrEmpty(context.Request["DistributionRelationBuildMallOrder"]) ? int.Parse(context.Request["DistributionRelationBuildMallOrder"]) : 0;//分销关系规则建立 商城下单
            int isNeedDistributionRecommendCode = !string.IsNullOrEmpty(context.Request["IsNeedDistributionRecommendCode"]) ? int.Parse(context.Request["IsNeedDistributionRecommendCode"]) : 1;//是否需要分销推荐码
            int isSynchronizationData = !string.IsNullOrEmpty(context.Request["IsSynchronizationData"]) ? int.Parse(context.Request["IsSynchronizationData"]) : 1;//是否自动同步数据
            int mallStatisticsLimitDate = !string.IsNullOrEmpty(context.Request["MallStatisticsLimitDate"]) ? int.Parse(context.Request["MallStatisticsLimitDate"]) : 30;//商城统计限制天数
            int distributionLimitLevel = !string.IsNullOrEmpty(context.Request["DistributionLimitLevel"]) ? int.Parse(context.Request["DistributionLimitLevel"]) : 1;
            int distributionGetWay = Convert.ToInt32(context.Request["DistributionGetWay"]);
            int isDisabledCommission = !string.IsNullOrEmpty(context.Request["IsDisabledCommission"]) ? int.Parse(context.Request["IsDisabledCommission"]) : 0;
            decimal autoUpdateLevelMinAmout = !string.IsNullOrEmpty(context.Request["AutoUpdateLevelMinAmout"]) ? decimal.Parse(context.Request["AutoUpdateLevelMinAmout"]) : 0;
            string autoUpdateLevelId = context.Request["AutoUpdateLevelId"];

            int requiredSupplier = !string.IsNullOrEmpty(context.Request["RequiredSupplier"]) ? int.Parse(context.Request["RequiredSupplier"]) : 0;// 添加编辑商品 供应商必填

            model.IsEnableLimitProductBuyTime = isEnableLimitProductBuyTime;
            model.IsEnableAccountAmountPay = isEnableAmountPay;
            model.AccountAmountPayShowName = accountAmountPayShowName;
            model.TotalAmountShowName = totalAmountShowName;
            model.Creater = this.currentUserInfo.UserID;
            model.CreateDate = DateTime.Now;
            model.DistributionMemberStandardsHaveParent = distributionMemberStandardsHaveParent;
            model.DistributionMemberStandardsHavePay = distributionMemberStandardsHavePay;
            model.DistributionMemberStandardsHaveSuccessOrder = distributionMemberStandardsHaveSuccessOrder;
            model.DistributionRelationBuildQrCode = distributionRelationBuildQrCode;
            model.DistributionRelationBuildSpreadActivity = distributionRelationBuildSpreadActivity;
            model.DistributionRelationBuildMallOrder = distributionRelationBuildMallOrder;
            model.IsNeedDistributionRecommendCode = isNeedDistributionRecommendCode;
            model.WhiteIP = context.Request["WhiteIP"];
            model.IsSynchronizationData = isSynchronizationData;
            model.WeiXinBindDomain = context.Request["WeiXinBindDomain"];//微信绑定域名
            model.MallStatisticsLimitDate = mallStatisticsLimitDate;
            model.SmsSignature = model.WebsiteName;
            model.DistributionLimitLevel = distributionLimitLevel;
            model.DistributionGetWay = distributionGetWay;
            model.ScorePayShowName = context.Request["ScorePayShowName"];
            model.CardCouponShowName = context.Request["CardCouponShowName"];
            model.IsDisabledCommission = isDisabledCommission;
            model.ComeoncloudOpenAppKey = context.Request["ComeoncloudOpenAppKey"];
            model.AutoUpdateLevelMinAmout = autoUpdateLevelMinAmout;
            model.AutoUpdateLevelId = autoUpdateLevelId;
            model.LoginPageConfig = context.Request["LoginPageConfig"];
            model.RequiredSupplier = requiredSupplier;
            //判断指定用户是否已经创建了站点
            if (this.bllUser.Exists(model, "WebsiteOwner"))
            {
                resp.Status = 0;
                resp.Msg = "用户" + model.WebsiteOwner + "已经创建了站点！";
                goto outoff;
            }

            if (bllUser.GetUserInfo(model.WebsiteOwner, model.WebsiteOwner) == null)
            {
                resp.Status = 0;
                resp.Msg = "站点所有者用户名不存在";
                goto outoff;

            }
            #region
            //string templateId = context.Request["TemplateId"];
            //if ((!string.IsNullOrEmpty(templateId)) && (!templateId.Equals("0")))//有模板
            //{
            //    IndustryTemplate template = juActivityBll.Get<IndustryTemplate>(string.Format("AutoID='{0}'", templateId));
            //    model.CourseManageMenuRName = template.CourseManageMenuRName;
            //    model.ArticleManageMenuRName = template.ArticleManageMenuRName; ;
            //    model.ActivityManageMenuRName = template.ActivityManageMenuRName;
            //    model.MasterManageMenuRName = template.MasterManageMenuRName;
            //    model.QuestionManageMenuRName = template.QuestionManageMenuRName;
            //    model.UserManageMenuRName = template.UserManageMenuRName;
            //    model.SignUpCourseMenuRName = template.SignUpCourseMenuRName;
            //    model.MallMenuRName = template.MallMenuRName;
            //    model.WebSiteStatisticsMenuRName = template.WebSiteStatisticsMenuRName;
            //    model.ArticleCate1 = template.ArticleCate1;
            //    model.ArticleCate2 = template.ArticleCate2;
            //    model.ArticleCate3 = template.ArticleCate3;
            //    model.ArticleCate4 = template.ArticleCate4;
            //    model.ArticleCate5 = template.ArticleCate5;
            //    model.CourseCate1 = template.CourseCate1;
            //    model.CourseCate2 = template.CourseCate2;
            //    model.AddVMenuRName = template.AddVMenuRName;

            //}

            //#region 自动创建报名活动

            ////自动创建报名活动
            //ActivityInfo signUpActivityModel = new ActivityInfo();
            //signUpActivityModel.ActivityID = this.juActivityBll.GetGUID(TransacType.ActivityAdd);
            //signUpActivityModel.UserID = model.WebsiteOwner;
            //signUpActivityModel.ActivityName = "站点" + model.WebsiteName + "的网上报名活动";
            //signUpActivityModel.ActivityStatus = 1;
            //signUpActivityModel.LimitCount = 100;

            ////设置自动生成的ID
            //model.SignUpActivityID = signUpActivityModel.ActivityID;

            //if (!this.juActivityBll.Add(signUpActivityModel))
            //{
            //    resp.Status = 0;
            //    resp.Msg = "添加失败!";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}


            ////添加默认字段
            ////添加自定义字段
            //List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>();

            //fieldData = new List<ActivityFieldMappingInfo>()
            //            {
            //                new ActivityFieldMappingInfo()
            //                { 
            //                    ActivityID = model.SignUpActivityID, 
            //                    ExFieldIndex = 1, 
            //                    FieldIsDefauld = 0,
            //                    FieldType = 0,
            //                    FormatValiFunc = "email",
            //                    MappingName = "邮箱"
            //                },
            //                new ActivityFieldMappingInfo()
            //                { 
            //                    ActivityID = model.SignUpActivityID, 
            //                    ExFieldIndex = 2, 
            //                    FieldIsDefauld = 0,
            //                    FieldType = 0,
            //                    MappingName = "公司"
            //                },
            //                new ActivityFieldMappingInfo()
            //                { 
            //                    ActivityID = model.SignUpActivityID, 
            //                    ExFieldIndex = 3, 
            //                    FieldIsDefauld = 0,
            //                    FieldType = 0,
            //                    MappingName = "职位"
            //                }
            //            };

            //if (!this.juActivityBll.AddList(fieldData))
            //{
            //    resp.Status = 0;
            //    resp.Msg = "添加报名字段失败!";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //};

            //#endregion


            //model.SignUpActivityID = signUpActivityModel.ActivityID;

            #endregion

            if (this.bllBase.Add(model))
            {
                #region 添加默认导航（商城个人中心-我的订单 商城个人中心-其他 商城底部导航等）
                //string json_SysToolBar_Path = ZentCloud.Common.ConfigHelper.GetConfigString("json_SysToolBar");
                //if (!string.IsNullOrWhiteSpace(json_SysToolBar_Path))
                //{
                //    string json_SysToolBarStr = File.ReadAllText(context.Server.MapPath(json_SysToolBar_Path));
                //    JObject json_SysToolBar = JObject.Parse(json_SysToolBarStr);
                //    bllWebsite.AddSysToolBars(model.WebsiteOwner, json_SysToolBar);
                //}

                #endregion
                #region 添加默认组件（个人中心 商城首页等）
                string jsonSysComponentPath = ZentCloud.Common.ConfigHelper.GetConfigString("json_SysComponent");
                if (!string.IsNullOrWhiteSpace(jsonSysComponentPath))
                {
                    string jsonSysComponentStr = File.ReadAllText(context.Server.MapPath(jsonSysComponentPath));
                    JObject jsonSysComponent = JObject.Parse(jsonSysComponentStr);
                    BLLComponent bllComponent = new BLLComponent();
                    bllComponent.AddSysComponents(model.WebsiteOwner, jsonSysComponent);
                }
                #endregion

                #region 添加默认订单状态
                List<WXMallOrderStatusInfo> orderStatusList = new List<WXMallOrderStatusInfo>();
                //添加默认订单状态
                WXMallOrderStatusInfo orderstatus1 = new WXMallOrderStatusInfo();
                orderstatus1.OrderStatu = "待付款";
                orderstatus1.WebsiteOwner = model.WebsiteOwner;
                orderstatus1.Sort = 5;

                WXMallOrderStatusInfo orderstatus2 = new WXMallOrderStatusInfo();
                orderstatus2.OrderStatu = "待发货";
                orderstatus2.WebsiteOwner = model.WebsiteOwner;
                orderstatus2.Sort = 4;

                WXMallOrderStatusInfo orderstatus3 = new WXMallOrderStatusInfo();
                orderstatus3.OrderStatu = "已发货";
                orderstatus3.WebsiteOwner = model.WebsiteOwner;
                orderstatus3.Sort = 3;

                WXMallOrderStatusInfo orderstatus4 = new WXMallOrderStatusInfo();
                orderstatus4.OrderStatu = "交易成功";
                orderstatus4.WebsiteOwner = model.WebsiteOwner;
                orderstatus4.Sort = 2;

                WXMallOrderStatusInfo orderstatus5 = new WXMallOrderStatusInfo();
                orderstatus5.OrderStatu = "已取消";
                orderstatus5.WebsiteOwner = model.WebsiteOwner;
                orderstatus5.Sort = 1;

                WXMallOrderStatusInfo orderstatus6 = new WXMallOrderStatusInfo();
                orderstatus6.OrderStatu = "退款退货";
                orderstatus6.WebsiteOwner = model.WebsiteOwner;
                orderstatus6.Sort = 0;

                orderStatusList.Add(orderstatus1);
                orderStatusList.Add(orderstatus2);
                orderStatusList.Add(orderstatus3);
                orderStatusList.Add(orderstatus4);
                orderStatusList.Add(orderstatus5);
                orderStatusList.Add(orderstatus6);
                if (!bllBase.AddList(orderStatusList))
                {
                    resp.Status = 0;
                    resp.Msg = "添加网站成功，但是添加默认订单状态失败";
                    goto outoff;

                }
                // 
                #endregion

                #region 添加默认积分规则

                List<ScoreDefineInfo> scoreDefineList = new List<ScoreDefineInfo>();
                ScoreDefineInfo scoreDefineInfo1 = new ScoreDefineInfo();
                scoreDefineInfo1.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo1.Score = 0;
                scoreDefineInfo1.ScoreType = "OrderPay";
                scoreDefineInfo1.Name = "订单付款";
                scoreDefineInfo1.Description = "订单付款";
                scoreDefineInfo1.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo1.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo1.InsertTime = DateTime.Now;
                scoreDefineInfo1.IsHide = 0;
                scoreDefineInfo1.DayLimit = -1;
                scoreDefineInfo1.TotalLimit = -1;
                scoreDefineInfo1.OrderNum = 0;
                scoreDefineInfo1.Ex1 = "0";

                ScoreDefineInfo scoreDefineInfo2 = new ScoreDefineInfo();
                scoreDefineInfo2.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo2.Score = 0;
                scoreDefineInfo2.ScoreType = "ReadArticle";
                scoreDefineInfo2.Name = "阅读文章";
                scoreDefineInfo2.Description = "阅读文章";
                scoreDefineInfo2.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo2.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo2.InsertTime = DateTime.Now;
                scoreDefineInfo2.IsHide = 0;
                scoreDefineInfo2.DayLimit = -1;
                scoreDefineInfo2.TotalLimit = -1;
                scoreDefineInfo2.OrderNum = 0;
                scoreDefineInfo2.Ex1 = "0";

                ScoreDefineInfo scoreDefineInfo3 = new ScoreDefineInfo();
                scoreDefineInfo3.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo3.Score = 0;
                scoreDefineInfo3.ScoreType = "ReadCategory";
                scoreDefineInfo3.Name = "阅读分类";
                scoreDefineInfo3.Description = "阅读分类";
                scoreDefineInfo3.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo3.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo3.InsertTime = DateTime.Now;
                scoreDefineInfo3.IsHide = 0;
                scoreDefineInfo3.DayLimit = -1;
                scoreDefineInfo3.TotalLimit = -1;
                scoreDefineInfo3.OrderNum = 0;
                scoreDefineInfo3.Ex1 = "0";

                ScoreDefineInfo scoreDefineInfo4 = new ScoreDefineInfo();
                scoreDefineInfo4.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo4.Score = 0;
                scoreDefineInfo4.ScoreType = "ShareArticle";
                scoreDefineInfo4.Name = "分享文章";
                scoreDefineInfo4.Description = "分享文章";
                scoreDefineInfo4.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo4.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo4.InsertTime = DateTime.Now;
                scoreDefineInfo4.IsHide = 0;
                scoreDefineInfo4.DayLimit = -1;
                scoreDefineInfo4.TotalLimit = -1;
                scoreDefineInfo4.OrderNum = 0;
                scoreDefineInfo4.Ex1 = "0";


                ScoreDefineInfo scoreDefineInfo5 = new ScoreDefineInfo();
                scoreDefineInfo5.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo5.Score = 0;
                scoreDefineInfo5.ScoreType = "Signin";
                scoreDefineInfo5.Name = "活动签到";
                scoreDefineInfo5.Description = "活动签到";
                scoreDefineInfo5.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo5.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo5.InsertTime = DateTime.Now;
                scoreDefineInfo5.IsHide = 0;
                scoreDefineInfo5.DayLimit = 1;
                scoreDefineInfo5.TotalLimit = -1;
                scoreDefineInfo5.OrderNum = 0;
                scoreDefineInfo5.Ex1 = "0";

                ScoreDefineInfo scoreDefineInfo6 = new ScoreDefineInfo();
                scoreDefineInfo6.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo6.Score = 0;
                scoreDefineInfo6.ScoreType = "UpdateMyInfo";
                scoreDefineInfo6.Name = "完善个人资料";
                scoreDefineInfo6.Description = "完善个人资料";
                scoreDefineInfo6.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo6.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo6.InsertTime = DateTime.Now;
                scoreDefineInfo6.IsHide = 0;
                scoreDefineInfo6.DayLimit = 1;
                scoreDefineInfo6.TotalLimit = 1;
                scoreDefineInfo6.OrderNum = 0;
                scoreDefineInfo6.Ex1 = "0";

                ScoreDefineInfo scoreDefineInfo7 = new ScoreDefineInfo();
                scoreDefineInfo7.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo7.Score = 0;
                scoreDefineInfo7.ScoreType = "AnswerQuestions";
                scoreDefineInfo7.Name = "回答话题";
                scoreDefineInfo7.Description = "回答话题";
                scoreDefineInfo7.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo7.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo7.InsertTime = DateTime.Now;
                scoreDefineInfo7.IsHide = 0;
                scoreDefineInfo7.DayLimit = -1;
                scoreDefineInfo7.TotalLimit = -1;
                scoreDefineInfo7.OrderNum = 0;
                scoreDefineInfo7.Ex1 = "0";

                ScoreDefineInfo scoreDefineInfo8 = new ScoreDefineInfo();
                scoreDefineInfo8.ScoreId = int.Parse(bllBase.GetGUID(TransacType.CommAdd));
                scoreDefineInfo8.Score = 0;
                scoreDefineInfo8.ScoreType = "LBSSignIn";
                scoreDefineInfo8.Name = "LBS签到";
                scoreDefineInfo8.Description = "LBS签到";
                scoreDefineInfo8.WebsiteOwner = model.WebsiteOwner;
                scoreDefineInfo8.CreateUserId = currentUserInfo.UserID;
                scoreDefineInfo8.InsertTime = DateTime.Now;
                scoreDefineInfo8.IsHide = 0;
                scoreDefineInfo8.DayLimit = 1;
                scoreDefineInfo8.TotalLimit = -1;
                scoreDefineInfo8.OrderNum = 0;
                scoreDefineInfo8.Ex1 = "0";

                scoreDefineList.Add(scoreDefineInfo1);
                scoreDefineList.Add(scoreDefineInfo2);
                scoreDefineList.Add(scoreDefineInfo3);
                scoreDefineList.Add(scoreDefineInfo4);
                scoreDefineList.Add(scoreDefineInfo5);
                scoreDefineList.Add(scoreDefineInfo6);
                scoreDefineList.Add(scoreDefineInfo7);
                scoreDefineList.Add(scoreDefineInfo8);
                bllBase.AddList(scoreDefineList);

                #endregion


                #region 移动商机
                #region 项目字段
                List<TableFieldMapping> projectFieldList = new List<TableFieldMapping>();

                TableFieldMapping projectField1 = new TableFieldMapping();
                projectField1.TableName = "ZCJ_Project";
                projectField1.WebSiteOwner = model.WebsiteOwner;
                projectField1.Field = "ProjectName";
                projectField1.MappingName = "商机名称";
                projectField1.FieldIsNull = 0;
                projectField1.Sort = 3;

                TableFieldMapping projectField2 = new TableFieldMapping();
                projectField2.TableName = "ZCJ_Project";
                projectField2.WebSiteOwner = model.WebsiteOwner;
                projectField2.Field = "Contact";
                projectField2.MappingName = "联系人";
                projectField2.FieldIsNull = 0;
                projectField2.Sort = 2;

                TableFieldMapping projectField3 = new TableFieldMapping();
                projectField3.TableName = "ZCJ_Project";
                projectField3.WebSiteOwner = model.WebsiteOwner;
                projectField3.Field = "Phone";
                projectField3.MappingName = "手机号";
                projectField3.FieldIsNull = 0;
                projectField3.Sort = 1;

                projectFieldList.Add(projectField1);
                projectFieldList.Add(projectField2);
                projectFieldList.Add(projectField3);

                bllBase.AddList<TableFieldMapping>(projectFieldList);
                #endregion

                #region 项目状态字段
                List<WXMallOrderStatusInfo> projectStatusList = new List<WXMallOrderStatusInfo>();
                WXMallOrderStatusInfo projectStatus1 = new WXMallOrderStatusInfo();
                projectStatus1.OrderStatu = "待审核";
                projectStatus1.StatusAction = "";
                projectStatus1.Sort = 6;
                projectStatus1.WebsiteOwner = model.WebsiteOwner;
                projectStatus1.StatusType = "DistributionOffLine";

                WXMallOrderStatusInfo projectStatus2 = new WXMallOrderStatusInfo();
                projectStatus2.OrderStatu = "审核通过";
                projectStatus2.StatusAction = "";
                projectStatus2.Sort = 5;
                projectStatus2.WebsiteOwner = model.WebsiteOwner;
                projectStatus2.StatusType = "DistributionOffLine";

                WXMallOrderStatusInfo projectStatus3 = new WXMallOrderStatusInfo();
                projectStatus3.OrderStatu = "审核不通过";
                projectStatus3.StatusAction = "";
                projectStatus3.Sort = 4;
                projectStatus3.WebsiteOwner = model.WebsiteOwner;
                projectStatus3.StatusType = "DistributionOffLine";

                WXMallOrderStatusInfo projectStatus4 = new WXMallOrderStatusInfo();
                projectStatus4.OrderStatu = "已立项跟进";
                projectStatus4.StatusAction = "";
                projectStatus4.Sort = 3;
                projectStatus4.WebsiteOwner = model.WebsiteOwner;
                projectStatus4.StatusType = "DistributionOffLine";

                WXMallOrderStatusInfo projectStatus5 = new WXMallOrderStatusInfo();
                projectStatus5.OrderStatu = "已签约";
                projectStatus5.StatusAction = "";
                projectStatus5.Sort = 2;
                projectStatus5.WebsiteOwner = model.WebsiteOwner;
                projectStatus5.StatusType = "DistributionOffLine";

                WXMallOrderStatusInfo projectStatus6 = new WXMallOrderStatusInfo();
                projectStatus6.OrderStatu = "已完成";
                projectStatus6.StatusAction = "DistributionOffLineCommission";
                projectStatus6.Sort = 1;
                projectStatus6.WebsiteOwner = model.WebsiteOwner;
                projectStatus6.StatusType = "DistributionOffLine";

                projectStatusList.Add(projectStatus1);
                projectStatusList.Add(projectStatus2);
                projectStatusList.Add(projectStatus3);
                projectStatusList.Add(projectStatus4);
                projectStatusList.Add(projectStatus5);
                projectStatusList.Add(projectStatus6);

                bllBase.AddList<WXMallOrderStatusInfo>(projectStatusList);
                #endregion

                #endregion

                #region 公众号自动回复

                #region 关注自动回复
                var replyModel1 = new WeixinReplyRuleInfo();
                replyModel1.MsgKeyword = "关注自动回复";
                replyModel1.MatchType = "全文匹配";
                replyModel1.ReplyContent = "欢迎关注";
                replyModel1.ReceiveType = "text";
                replyModel1.ReplyType = "text";
                replyModel1.CreateDate = DateTime.Now;
                replyModel1.RuleType = 1;
                replyModel1.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
                replyModel1.UserID = model.WebsiteOwner;
                bllBase.Add(replyModel1);
                #endregion

                #region 消息自动回复
                var replyModel2 = new WeixinReplyRuleInfo();
                replyModel2.MsgKeyword = "消息自动回复";
                replyModel2.MatchType = "全文匹配";
                replyModel2.ReplyContent = "自动回复";
                replyModel2.ReceiveType = "text";
                replyModel2.ReplyType = "text";
                replyModel2.CreateDate = DateTime.Now;
                replyModel2.RuleType = 1;
                replyModel2.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
                replyModel2.UserID = model.WebsiteOwner;
                bllBase.Add(replyModel2);
                #endregion


                #endregion


                resp.Status = 1;
                resp.Msg = "添加成功!";
                goto outoff;
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败!";
                goto outoff;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 编辑站点
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWebsite(HttpContext context)
        {
            string inputWebsiteOwner = context.Request["WebsiteOwner"];
            string websiteName = context.Request["WebsiteName"];
            string websiteDescription = context.Request["WebsiteDescription"];
            string websiteExpirationDate = context.Request["WebsiteExpirationDate"];
            int logLimitDay = int.Parse(context.Request["LogLimitDay"]);
            int isEnableLimitProductBuyTime = int.Parse(context.Request["IsEnableLimitProductBuyTime"]);
            int isEnableAmountPay = int.Parse(context.Request["IsEnableAmountPay"]);
            string accountAmountPayShowName = context.Request["AccountAmountPayShowName"];
            string totalAmountShowName = context.Request["TotalAmountShowName"];
            int distributionMemberStandardsHaveParent = !string.IsNullOrEmpty(context.Request["DistributionMemberStandardsHaveParent"]) ? int.Parse(context.Request["DistributionMemberStandardsHaveParent"]) : 0;//分销会员标准 有上级
            int distributionMemberStandardsHavePay = !string.IsNullOrEmpty(context.Request["DistributionMemberStandardsHavePay"]) ? int.Parse(context.Request["DistributionMemberStandardsHavePay"]) : 0;//分销会员标准 有付款的订单
            int distributionMemberStandardsHaveSuccessOrder = !string.IsNullOrEmpty(context.Request["DistributionMemberStandardsHaveSuccessOrder"]) ? int.Parse(context.Request["DistributionMemberStandardsHaveSuccessOrder"]) : 0;//分销会员标准 有交易完成的订单

            int distributionRelationBuildQrCode = !string.IsNullOrEmpty(context.Request["DistributionRelationBuildQrCode"]) ? int.Parse(context.Request["DistributionRelationBuildQrCode"]) : 0;//分销关系规则建立 二维码
            int distributionRelationBuildSpreadActivity = !string.IsNullOrEmpty(context.Request["DistributionRelationBuildSpreadActivity"]) ? int.Parse(context.Request["DistributionRelationBuildSpreadActivity"]) : 0;////分销关系规则建立 转发活动
            int distributionRelationBuildMallOrder = !string.IsNullOrEmpty(context.Request["DistributionRelationBuildMallOrder"]) ? int.Parse(context.Request["DistributionRelationBuildMallOrder"]) : 0;//分销关系规则建立 商城下单
            int isNeedDistributionRecommendCode = !string.IsNullOrEmpty(context.Request["IsNeedDistributionRecommendCode"]) ? int.Parse(context.Request["IsNeedDistributionRecommendCode"]) : 0;//是否需要分销推荐码
            int isSynchronizationData = !string.IsNullOrEmpty(context.Request["IsSynchronizationData"]) ? int.Parse(context.Request["IsSynchronizationData"]) : 1;//是否自动同步数据
            int mallStatisticsLimitDate = !string.IsNullOrEmpty(context.Request["MallStatisticsLimitDate"]) ? int.Parse(context.Request["MallStatisticsLimitDate"]) : 30;
            int isDisabledCommission = !string.IsNullOrEmpty(context.Request["IsDisabledCommission"]) ? int.Parse(context.Request["IsDisabledCommission"]) : 0;

            string weiXinBindDomain = context.Request["WeiXinBindDomain"];
            string channelShowName = context.Request["ChannelShowName"];
            int distributionLimitLevel = !string.IsNullOrEmpty(context.Request["DistributionLimitLevel"]) ? int.Parse(context.Request["DistributionLimitLevel"]) : 1;
            int distributionGetWay = Convert.ToInt32(context.Request["DistributionGetWay"]);
            decimal autoUpdateLevelMinAmout = !string.IsNullOrEmpty(context.Request["AutoUpdateLevelMinAmout"]) ? decimal.Parse(context.Request["AutoUpdateLevelMinAmout"]) : 0;
            string autoUpdateLevelId = context.Request["AutoUpdateLevelId"];
            string loginPageConfig = context.Request["LoginPageConfig"];
            int isOpenElemeOrderSynchronous = !string.IsNullOrEmpty(context.Request["IsOpenElemeOrderSynchronous"]) ? int.Parse(context.Request["IsOpenElemeOrderSynchronous"]) : 0;
            string memberMgrBtn = context.Request["MemberMgrBtn"];

            int requiredSupplier = !string.IsNullOrEmpty(context.Request["RequiredSupplier"]) ? int.Parse(context.Request["RequiredSupplier"]) : 0;// 添加编辑商品 供应商必填


            if (string.IsNullOrWhiteSpace(inputWebsiteOwner))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WebsiteInfo model = bllBase.Get<WebsiteInfo>(string.Format(" WebsiteOwner = '{0}' ", inputWebsiteOwner));
            if (model == null)
            {
                resp.Status = -1;
                resp.Msg = "没有找到该站点！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            model.WebsiteName = websiteName;
            model.WebsiteDescription = websiteDescription;
            model.LogLimitDay = logLimitDay;
            model.IsEnableLimitProductBuyTime = isEnableLimitProductBuyTime;
            model.IsEnableAccountAmountPay = isEnableAmountPay;
            model.AccountAmountPayShowName = accountAmountPayShowName;
            model.TotalAmountShowName = totalAmountShowName;
            model.CardCouponShowName = context.Request["CardCouponShowName"];
            if (!string.IsNullOrWhiteSpace(websiteExpirationDate))
            {
                model.WebsiteExpirationDate = Convert.ToDateTime(websiteExpirationDate);
            }
            model.MaxSubAccountCount = int.Parse(context.Request["MaxSubAccountCount"]);
            model.DistributionMemberStandardsHaveParent = distributionMemberStandardsHaveParent;
            model.DistributionMemberStandardsHavePay = distributionMemberStandardsHavePay;
            model.DistributionMemberStandardsHaveSuccessOrder = distributionMemberStandardsHaveSuccessOrder;
            model.DistributionRelationBuildQrCode = distributionRelationBuildQrCode;
            model.DistributionRelationBuildSpreadActivity = distributionRelationBuildSpreadActivity;
            model.DistributionRelationBuildMallOrder = distributionRelationBuildMallOrder;
            model.IsNeedDistributionRecommendCode = isNeedDistributionRecommendCode;
            model.WhiteIP = context.Request["WhiteIP"];
            model.IsSynchronizationData = isSynchronizationData;
            model.WeiXinBindDomain = weiXinBindDomain;
            model.MallStatisticsLimitDate = mallStatisticsLimitDate;
            model.ChannelShowName = channelShowName;
            model.DistributionLimitLevel = distributionLimitLevel;
            model.DistributionGetWay = distributionGetWay;
            model.IsDisabledCommission = isDisabledCommission;
            model.ComeoncloudOpenAppKey = context.Request["ComeoncloudOpenAppKey"];
            model.AutoUpdateLevelMinAmout = autoUpdateLevelMinAmout;
            model.AutoUpdateLevelId = autoUpdateLevelId;
            model.LoginPageConfig = loginPageConfig;
            model.IsOpenElemeOrderSynchronous = isOpenElemeOrderSynchronous;
            model.MemberMgrBtn = memberMgrBtn;
            model.RequiredSupplier = requiredSupplier;
            if (bllBase.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }


        }
        /// <summary>
        /// 删除站点
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWebsite(HttpContext context)
        {
            string inputWebsiteOwner = context.Request["inputWebsiteOwner"];

            if (string.IsNullOrWhiteSpace(inputWebsiteOwner))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //删除域名
            int domainCount = this.bllJuActivity.Delete(new WebsiteDomainInfo(), string.Format(" WebsiteOwner IN ({0}) ", inputWebsiteOwner));

            //删除站点
            int count = this.bllJuActivity.Delete(new WebsiteInfo(), string.Format(" WebsiteOwner IN ({0}) ", inputWebsiteOwner));

            resp.Status = 0;
            resp.Msg = "已成功删除数据" + count.ToString() + "行";
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 删除站点域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWebSiteDomain(HttpContext context)
        {
            string domain = context.Request["domain"];

            if (string.IsNullOrWhiteSpace(domain))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            int count = this.bllJuActivity.Delete(new WebsiteDomainInfo(), string.Format(" WebsiteDomain IN ({0}) ", domain));

            resp.Status = 0;
            resp.Msg = "已成功删除数据" + count.ToString() + "行";
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 编辑站点域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWebSiteDomain(HttpContext context)
        {
            string oldDomain = context.Request["oldDomain"];
            string newDomain = context.Request["newDomain"];

            if (string.IsNullOrWhiteSpace(oldDomain) || string.IsNullOrWhiteSpace(newDomain))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (oldDomain.Equals(newDomain))
            {
                resp.Status = -3;
                resp.Msg = "域名没有做任何修改！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllJuActivity.GetCount<WebsiteDomainInfo>(string.Format(" WebsiteDomain = '{0}' ", newDomain)) > 0)
            {
                resp.Status = -2;
                resp.Msg = "该域名已经被当前站点或者其他站点添加过，请检查是否填写有误！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllJuActivity.Update(new WebsiteDomainInfo(), string.Format(" WebsiteDomain = '{0}' ", newDomain), string.Format(" WebsiteDomain = '{0}' ", oldDomain)) > 0)
            {

                var websiteDomainInfo = bllBase.Get<WebsiteDomainInfo>(string.Format(" WebsiteDomain = '{0}'", newDomain));
                if (websiteDomainInfo != null)
                {
                    RedisHelper.RedisHelper.HashSetSerialize(RedisHelper.Enums.RedisKeyEnum.WebsiteDomainInfo, newDomain, websiteDomainInfo);

                }
                resp.Status = 1;
                resp.Msg = "修改成功！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }
        /// <summary>
        /// 增加站点域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWebSiteDomain(HttpContext context)
        {
            string domain = context.Request["domain"];
            string inputWebsiteOwner = context.Request["inputWebsiteOwner"];

            if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(inputWebsiteOwner))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            WebsiteDomainInfo domainModel = new WebsiteDomainInfo();
            domainModel.WebsiteDomain = domain;
            domainModel.WebsiteOwner = inputWebsiteOwner;

            if (this.bllJuActivity.Exists(domainModel, "WebsiteDomain"))
            {
                resp.Status = -2;
                resp.Msg = "该域名已经被当前站点或者其他站点添加过，请检查是否填写有误！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllJuActivity.Add(domainModel))
            {

                //
                //添加默认配置

                //
                //添加订单状态
                #region 添加默认订单状态
                if (bllMall.GetOrderStatuList().Count <= 0)
                {
                    WXMallOrderStatusInfo orderStatus1 = new WXMallOrderStatusInfo();
                    orderStatus1.OrderStatu = "待付款";
                    orderStatus1.Sort = 0;
                    orderStatus1.WebsiteOwner = bllMall.WebsiteOwner;
                    orderStatus1.OrderMessage = "";

                    WXMallOrderStatusInfo orderStatus2 = new WXMallOrderStatusInfo();
                    orderStatus2.OrderStatu = "待发货";
                    orderStatus2.Sort = 1;
                    orderStatus2.WebsiteOwner = bllMall.WebsiteOwner;
                    orderStatus2.OrderMessage = "";

                    WXMallOrderStatusInfo orderStatus3 = new WXMallOrderStatusInfo();
                    orderStatus3.OrderStatu = "已发货";
                    orderStatus3.Sort = 2;
                    orderStatus3.WebsiteOwner = bllMall.WebsiteOwner;
                    orderStatus3.OrderMessage = "";

                    WXMallOrderStatusInfo orderStatus4 = new WXMallOrderStatusInfo();
                    orderStatus4.OrderStatu = "交易成功";
                    orderStatus4.Sort = 3;
                    orderStatus4.WebsiteOwner = bllMall.WebsiteOwner;
                    orderStatus4.OrderMessage = "";

                    bllMall.Add(orderStatus1);
                    bllMall.Add(orderStatus2);
                    bllMall.Add(orderStatus3);
                    bllMall.Add(orderStatus4);

                }
                #endregion

                resp.Status = 1;
                resp.Msg = "添加成功！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }
        /// <summary>
        /// 查询站点域名列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWebSiteDomain(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string inputWebsiteOwner = context.Request["inputWebsiteOwner"];
            int totalCount = this.bllJuActivity.GetCount<WebsiteDomainInfo>(string.Format("WebsiteOwner = '{0}' ", inputWebsiteOwner));
            List<WebsiteDomainInfo> dataList = this.bllJuActivity.GetList<WebsiteDomainInfo>(string.Format(" WebsiteOwner = '{0}' ", inputWebsiteOwner));
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});

        }
        /// <summary>
        /// 查询域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWebsiteInfo(HttpContext context)
        {
            if (this.currentUserInfo.UserType != 1)
            {
                resp.Status = -5;
                resp.Msg = "没有操作权限!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string websiteName = context.Request["WebSiteName"];
            string version = context.Request["Version"];
            int totalCount = 0;
            List<WebsiteInfo> dataList = bllWebsite.GetWebsiteInfoList(pageIndex, pageSize, websiteName, version, " CreateDate DESC ", out totalCount);
            foreach (var item in dataList)
            {
                item.Version = bllWebsite.GetWebsiteVersion(item.WebsiteOwner);
                List<BLLPermission.Model.PermissionRelationInfo> relOList = bllPermission.GetPermissionRelationList(item.WebsiteOwner, 9);
                if (relOList.Count > 0)
                {
                    item.DisablePermissions = ZentCloud.Common.MyStringHelper.ListToStr(relOList.Select(p => p.PermissionID).Distinct().ToList(), "", ",");
                }
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = dataList
            });
        }

        /// <summary>
        /// 接口配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetPubConfig(HttpContext context)
        {
            string weixinAPIUrl = string.Format("http://{0}/Weixin/OAuthPage.aspx?u={1}",
                               context.Request.Url.Host,
                               ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(bllBase.WebsiteOwner)
                           );
            string weixinToken = context.Request["WeixinToken"];
            string weixinAppId = context.Request["WeixinAppId"];
            string weixinAppSecret = context.Request["WeixinAppSecret"];
            string wellcomeReplyContent = context.Request["WellcomeReplyContent"];
            string subscribeKeyWord = context.Request["SubscribeKeyWord"];
            UserInfo websiteOwnerUserInfo = this.bllUser.GetUserInfo(bllUser.WebsiteOwner);
            websiteOwnerUserInfo.WeixinAPIUrl = weixinAPIUrl;
            websiteOwnerUserInfo.WeixinToken = weixinToken;
            websiteOwnerUserInfo.WeixinIsEnableMenu = 1;
            websiteOwnerUserInfo.WeixinAppId = weixinAppId;
            websiteOwnerUserInfo.WeixinAppSecret = weixinAppSecret;
            websiteOwnerUserInfo.SubscribeKeyWord = subscribeKeyWord;
            var result = this.bllUser.Update(websiteOwnerUserInfo, string.Format("WeixinAPIUrl='{0}',WeixinToken='{1}',WeixinIsEnableMenu=1,WeixinAppId='{2}',WeixinAppSecret='{3}',SubscribeKeyWord='{4}'", websiteOwnerUserInfo.WeixinAPIUrl, websiteOwnerUserInfo.WeixinToken, websiteOwnerUserInfo.WeixinAppId, websiteOwnerUserInfo.WeixinAppSecret, websiteOwnerUserInfo.SubscribeKeyWord), string.Format("AutoID={0}", websiteOwnerUserInfo.AutoID));
            if (result <= 0)
            {
                resp.Msg = "更新失败";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo menuModel = bllWeixin.Get<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 4 ", websiteOwnerUserInfo.UserID));

            //if (menuModel != null)
            //{
            //menuModel.MsgKeyword = "m";
            //menuModel.ReplyContent = wellcomeReplyContent;
            //this.bllWeixin.Update(menuModel);
            // }
            //else
            //{
            //menuModel = new WeixinReplyRuleInfo();
            //menuModel.ReplyContent = wellcomeReplyContent;
            //menuModel.ReplyType = "text";
            //menuModel.ReceiveType = "text";
            //menuModel.MatchType = "全文匹配";
            //menuModel.UserID = websiteOwnerUserInfo.UserID;
            //menuModel.UID = this.bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
            //menuModel.RuleType = 4;
            //menuModel.MsgKeyword = "m";
            //menuModel.CreateDate = DateTime.Now;
            //this.bllWeixin.Add(menuModel);
            //}

            resp.Status = 1;
            resp.Msg = "配置成功!";


            return Common.JSONHelper.ObjectToJson(resp);

        }


        #region 微信菜单模块


        /// <summary>
        /// 获取菜单选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMenuSelectList(HttpContext context)
        {
            string result = string.Empty;
            result = new MySpider.MyCategories().GetSelectOptionHtml(bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}'", bllBase.WebsiteOwner)), "MenuID", "PreID", "NodeName", 0, "ddlPreMenu", "width:200px", "");
            return result.ToString();
        }

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWeixinMenu(HttpContext context)
        {

            string ids = context.Request["ids"];
            int result = bllWeixin.Delete(new WeixinMenu(), string.Format(" MenuID in ({0}) And UserID='{1}'", ids, bllBase.WebsiteOwner));
            result += bllWeixin.Delete(new WeixinMenu(), string.Format(" PreID in ({0}) And UserID='{1}'", ids, bllBase.WebsiteOwner));
            return result.ToString();

        }
        /// <summary>
        /// 增加微信自定义菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWeixinMenu(HttpContext context)
        {
            string jsonData = context.Request["JsonData"];
            WeixinMenu menuInfo = ZentCloud.Common.JSONHelper.JsonToModel<WeixinMenu>(jsonData);
            //if ((menuInfo.Type == "view") && (!menuInfo.KeyOrUrl.StartsWith("http://") && !menuInfo.KeyOrUrl.StartsWith("https://")))
            //{

            //    return "链接格式不正确";

            //}
            if (menuInfo.PreID == 0)//添加的是一级菜单
            {
                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID=0", bllBase.WebsiteOwner)) >= 3)
                {
                    return "最多可以添加3个一级菜单";
                }
            }
            else//添加是二级菜单
            {
                var topMenu = bllWeixin.Get<WeixinMenu>(string.Format("MenuID='{0}'", menuInfo.PreID));
                if (topMenu != null)
                {
                    if (topMenu.PreID != 0)
                    {
                        return "只能添加二级菜单";
                    }

                }

                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID='{1}'", bllBase.WebsiteOwner, menuInfo.PreID)) >= 5)
                {
                    return "最多可以添加5个二级菜单";
                }

            }

            if (menuInfo.PreID == 0)//添加一级菜单
            {
                List<WeixinMenu> firstLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' And PreID=0", bllBase.WebsiteOwner)).OrderBy(p => p.MenuSort).ToList();
                if (firstLevelMenu.Count == 0)
                {
                    menuInfo.MenuSort = 1;
                }
                else
                {
                    menuInfo.MenuSort = firstLevelMenu[firstLevelMenu.Count - 1].MenuSort + 1;
                }


            }
            else//添加二级菜单
            {
                List<WeixinMenu> secondLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' And PreID={1}", bllBase.WebsiteOwner, menuInfo.PreID)).OrderBy(p => p.MenuSort).ToList();
                if (secondLevelMenu.Count == 0)
                {
                    menuInfo.MenuSort = 1;
                }
                else
                {
                    menuInfo.MenuSort = secondLevelMenu[secondLevelMenu.Count - 1].MenuSort + 1;
                }

            }

            menuInfo.MenuID = long.Parse(bllWeixin.GetGUID(ZentCloud.BLLJIMP.TransacType.WeixinMenuAdd));
            menuInfo.UserID = bllBase.WebsiteOwner;
            bool result = bllWeixin.Add(menuInfo);
            return result.ToString().ToLower();
        }
        /// <summary>
        /// 编辑微信自定义菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWeixinMenu(HttpContext context)
        {
            string jsonData = context.Request["JsonData"];
            WeixinMenu menuInfo = ZentCloud.Common.JSONHelper.JsonToModel<WeixinMenu>(jsonData);
            //if ((menuInfo.Type == "view") && (!menuInfo.KeyOrUrl.StartsWith("http://")&&!menuInfo.KeyOrUrl.StartsWith("https://")))
            //{

            //    return "链接格式不正确";

            //}
            WeixinMenu oldMenuInfo = bllWeixin.Get<WeixinMenu>(string.Format("MenuID={0} And UserID='{1}'", menuInfo.MenuID, bllBase.WebsiteOwner));
            if (menuInfo.PreID == 0)//上级是顶级菜单
            {
                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID=0", bllBase.WebsiteOwner)) >= 3 && (oldMenuInfo.PreID != 0) && (oldMenuInfo.PreID != menuInfo.PreID))
                {
                    return "一级菜单最多只能设置3个";
                }



            }
            else
            {


                var topMenu = bllWeixin.Get<WeixinMenu>(string.Format("MenuID='{0}'", menuInfo.PreID));
                if (topMenu != null)
                {
                    if (topMenu.PreID != 0)
                    {
                        return "只能设置二级菜单";
                    }

                }

                if (bllWeixin.GetCount<WeixinMenu>(string.Format("UserID='{0}'and PreID='{1}'", bllBase.WebsiteOwner, menuInfo.PreID)) >= 5 && (oldMenuInfo.PreID != menuInfo.PreID))
                {
                    return "最多可以设置5个二级菜单";
                }

                //WeixinMenu lastsecondmenu = weixinBll.Get<WeixinMenu>(string.Format("PreID='{0}' order by MenuSort DESC", menuInfo.PreID));
                //if (lastsecondmenu != null)
                //{
                //    menuInfo.MenuSort = oldmenuInfo.MenuSort;
                //}
                //else
                //{
                //    menuInfo.MenuSort = 1;
                //}



            }
            menuInfo.UserID = bllBase.WebsiteOwner;
            bool result = bllWeixin.Update(menuInfo);
            return result.ToString().ToLower();
        }

        /// <summary>
        /// 获取微信菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWeixinMenu(HttpContext context)
        {
            List<WeixinMenu> list;
            list = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}'", bllBase.WebsiteOwner));
            list = list.OrderBy(p => p.MenuSort).ToList();
            List<WeixinMenu> showList = new List<WeixinMenu>();

            MySpider.MyCategories m = new MySpider.MyCategories();

            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<WeixinMenu>("MenuID", "PreID", "NodeName", list), 0))
            {
                try
                {
                    WeixinMenu tmpModel = (WeixinMenu)list.Where(p => p.MenuID.ToString().Equals(item.Value)).ToList()[0].Clone();
                    tmpModel.NodeName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }

            int totalCount = showList.Count;

            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = showList
     });
        }

        /// <summary>
        /// 生成微信客户端菜单
        /// </summary>
        /// <returns></returns>
        private string CreateWeixinClientMenu()
        {
            //UserInfo websiteOwnerUserInfo = this.bllUser.GetUserInfo(bllBase.WebsiteOwner);
            //if (string.IsNullOrEmpty(websiteOwnerUserInfo.WeixinAppId) || string.IsNullOrEmpty(websiteOwnerUserInfo.WeixinAppSecret))
            //{
            //    return "未配置 AppId与 AppSecret";
            //}

            //

            //获取AccessToken
            string accessToken = bllWeixin.GetAccessToken();
            if (accessToken == string.Empty)
            {
                return "获取accesstoken失败";

            }
            //获取AccessToken


            List<WeixinMenu> firstLevelList = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID=0", bllBase.WebsiteOwner)).OrderBy(p => p.MenuSort).ToList();

            //构造菜单字符串
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append("{\"button\":[");
            for (int i = 0; i < firstLevelList.Count; i++)
            {
                List<WeixinMenu> sendcondLevelList = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID={1}", bllBase.WebsiteOwner, firstLevelList[i].MenuID)).OrderByDescending(p => p.MenuSort).ToList();
                sbMenu.Append("{");
                if (sendcondLevelList.Count == 0)//无子菜单
                {

                    sbMenu.AppendFormat("\"type\":\"{0}\",", firstLevelList[i].Type.Trim());
                    sbMenu.AppendFormat("\"name\":\"{0}\",", firstLevelList[i].NodeName);
                    if (firstLevelList[i].Type.Trim().Equals("click"))
                    {
                        sbMenu.AppendFormat("\"key\":\"{0}\"", firstLevelList[i].KeyOrUrl);
                    }
                    else
                    {
                        sbMenu.AppendFormat("\"url\":\"{0}\"", firstLevelList[i].KeyOrUrl);
                    }

                }
                else//有子菜单
                {
                    sbMenu.AppendFormat("\"name\":\"{0}\",", firstLevelList[i].NodeName);
                    sbMenu.Append("\"sub_button\":[");

                    for (int j = 0; j < sendcondLevelList.Count; j++)
                    {
                        sbMenu.Append("{");

                        sbMenu.AppendFormat("\"type\":\"{0}\",", sendcondLevelList[j].Type.Trim());
                        sbMenu.AppendFormat("\"name\":\"{0}\",", sendcondLevelList[j].NodeName);
                        if (sendcondLevelList[j].Type.Trim().Equals("click"))
                        {
                            sbMenu.AppendFormat("\"key\":\"{0}\"", sendcondLevelList[j].KeyOrUrl);
                        }
                        else
                        {
                            sbMenu.AppendFormat("\"url\":\"{0}\"", sendcondLevelList[j].KeyOrUrl);
                        }

                        sbMenu.Append("}");
                        if (j < sendcondLevelList.Count - 1)
                        {
                            sbMenu.Append(",");
                        }
                    }
                    sbMenu.Append("]");


                }
                sbMenu.Append("}");

                if (i < firstLevelList.Count - 1)
                {
                    sbMenu.Append(",");
                }
            }
            sbMenu.Append("]}");

            //构造菜单字符串

            WeixinAccessToken result = bllWeixin.CreateWeixinClientMenu(accessToken, sbMenu.ToString());
            return bllWeixin.GetCodeMessage(result.errcode);
        }

        /// <summary>
        /// 菜单排序
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MoveMenu(HttpContext context)
        {

            int menuId = int.Parse(context.Request["MenuID"]);//步骤ID
            string direction = context.Request["Direction"];//移动方向 up:上 down: 下
            WeixinMenu targetMenu = bllWeixin.Get<WeixinMenu>(string.Format("MenuID={0}", menuId));//要移动的菜单
            int index = 0;//菜单所在同级的顺序
            #region 移动一级菜单

            if (targetMenu.PreID.ToString().Equals("0"))//移动的是一级菜单
            {
                List<WeixinMenu> firstLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID=0", bllBase.WebsiteOwner));//一级菜单
                firstLevelMenu = firstLevelMenu.OrderBy(p => p.MenuSort).ToList();


                //修改menusort
                for (int i = 0; i < firstLevelMenu.Count; i++)
                {
                    if (firstLevelMenu[i].MenuID == targetMenu.MenuID)
                    {
                        index = i;
                        break;
                    }
                }
                int tagetMenuSort = (int)firstLevelMenu[index].MenuSort;
                if (direction.Equals("up"))//一级菜单上移
                {

                    if (index == 0)//一级菜单已经最靠前
                    {
                        return "选中菜单已经排最前";
                    }
                    else//一级菜单不是最靠前
                    {
                        //交换排序
                        int preMenuSort = (int)firstLevelMenu[index - 1].MenuSort;//上一条一级菜单排序
                        firstLevelMenu[index].MenuSort = preMenuSort;
                        firstLevelMenu[index - 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(firstLevelMenu[index]) && bllWeixin.Update(firstLevelMenu[index - 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }


                    }

                }
                else//一级菜单下移
                {
                    if (firstLevelMenu[firstLevelMenu.Count - 1].MenuID == targetMenu.MenuID)//一级菜单已经最靠后
                    {
                        return "选中菜单已经排最后";
                    }
                    else//一级菜单不是最靠后
                    {
                        //交换排序

                        int nextMenuSort = (int)firstLevelMenu[index + 1].MenuSort;//下一条一级菜单排序

                        firstLevelMenu[index].MenuSort = nextMenuSort;
                        firstLevelMenu[index + 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(firstLevelMenu[index]) && bllWeixin.Update(firstLevelMenu[index + 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }

                    }

                }
            }
            #endregion

            #region 移动二级菜单
            else//移动的是二级菜单
            {
                List<WeixinMenu> secondLevelMenu = bllWeixin.GetList<WeixinMenu>(string.Format("UserID='{0}' and PreID={1}", bllBase.WebsiteOwner, targetMenu.PreID));//二级菜单
                secondLevelMenu = secondLevelMenu.OrderBy(p => p.MenuSort).ToList();

                //修改menusort
                for (int i = 0; i < secondLevelMenu.Count; i++)
                {
                    if (secondLevelMenu[i].MenuID == targetMenu.MenuID)
                    {
                        index = i;
                        break;
                    }
                }
                int tagetMenuSort = (int)secondLevelMenu[index].MenuSort;
                if (direction.Equals("up"))//二级菜单上移
                {

                    if (index == 0)//二级菜单已经最靠前
                    {
                        return "选中菜单已经排最前";
                    }
                    else//二级菜单不是最靠前
                    {
                        //交换排序
                        int preMenuSort = (int)secondLevelMenu[index - 1].MenuSort;//上一条二级菜单排序
                        secondLevelMenu[index].MenuSort = preMenuSort;
                        secondLevelMenu[index - 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(secondLevelMenu[index]) && bllWeixin.Update(secondLevelMenu[index - 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }


                    }

                }
                else//二级菜单下移
                {
                    if (secondLevelMenu[secondLevelMenu.Count - 1].MenuID == targetMenu.MenuID)//一级菜单已经最靠后
                    {
                        return "选中菜单已经排最后";
                    }
                    else//二级菜单不是最靠后
                    {
                        //交换排序

                        int nextMenusort = (int)secondLevelMenu[index + 1].MenuSort;//下一条二级菜单排序

                        secondLevelMenu[index].MenuSort = nextMenusort;
                        secondLevelMenu[index + 1].MenuSort = tagetMenuSort;
                        if (bllWeixin.Update(secondLevelMenu[index]) && bllWeixin.Update(secondLevelMenu[index + 1]))
                        {
                            return "true";
                        }
                        else
                        {
                            return "操作失败";
                        }

                    }

                }
            }
            #endregion


        }

        #endregion


        ///// <summary>
        ///// 删除问题:连同回复一起删除
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string DeleteJuMasterFeedBack(HttpContext context)
        //{

        //    string ids = context.Request["ids"];
        //    //删除问题
        //    int count = juActivityBll.Delete(new JuMasterFeedBack(), string.Format("FeedBackID in({0})", ids));

        //    if (count > 0)
        //    {
        //        //删除回复(暂时不check)
        //        int countReply = juActivityBll.Delete(new JuMasterFeedBackDialogue(), string.Format("FeedBackID in({0})", ids));

        //        resp.Status = 1;
        //        resp.Msg = string.Format("成功删除{0}条数据", count);

        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "删除失败";

        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}


        ///// <summary>
        ///// 删除回复
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string DeleteJuMasterFeedBackDialogue(HttpContext context)
        //{
        //    string ids = context.Request["ids"];
        //    //删除问题
        //    int count = juActivityBll.Delete(new JuMasterFeedBackDialogue(), string.Format("DialogueID in({0})", ids));
        //    if (count > 0)
        //    {
        //        resp.Status = 1;
        //        resp.Msg = string.Format("成功删除{0}条数据", count);
        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "删除失败";
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}


        //private string QueryJuMasterFeedBackDialogueForPCGrid(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);

        //    int FeedBackID = Convert.ToInt32(context.Request["FeedBackID"]);

        //    StringBuilder strWhere = new StringBuilder();

        //    strWhere.AppendFormat(" FeedBackID = {0} ", FeedBackID);

        //    List<JuMasterFeedBackDialogue> dataList = this.juActivityBll.GetLit<JuMasterFeedBackDialogue>(rows, page, strWhere.ToString(), " SubmitDate DESC ");

        //    return Common.JSONHelper.ListToEasyUIJson(this.juActivityBll.GetCount<JuMasterFeedBackDialogue>(strWhere.ToString()), dataList);
        //}

        //private string QueryJuMasterFeedBackForPCGrid(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    StringBuilder strWhere = new StringBuilder(" 1=1 ");

        //    strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);

        //    List<JuMasterFeedBack> dataList = this.juActivityBll.GetLit<JuMasterFeedBack>(rows, page, strWhere.ToString(), " SubmitDate DESC ");

        //    return Common.JSONHelper.ListToEasyUIJson(this.juActivityBll.GetCount<JuMasterFeedBack>(strWhere.ToString()), dataList);
        //}

        ///// <summary>
        ///// 提交问题信息
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddJuMasterFeedBack(HttpContext context)
        //{
        //    try
        //    {
        //        JuMasterFeedBack model = new JuMasterFeedBack();

        //        model.MasterID = context.Request["MasterID"];
        //        model.FeedBackContent = context.Request["FeedBackContent"];

        //        //return model.FeedBackContent;

        //        model.SubmitDate = DateTime.Now;
        //        model.SubmitIP = Common.MySpider.GetClientIP();
        //        model.ProcessStatus = "未处理";
        //        model.UserID = this.CurrentUserInfo.UserID;
        //        model.WebsiteOwner = bllBase.WebsiteOwner;

        //        if (string.IsNullOrWhiteSpace(model.FeedBackContent))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "请输入内容！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }

        //        //判断该用户是否重复提交问题
        //        if (this.juActivityBll.Exists(model, new List<string>() { "MasterID", "UserID", "FeedBackContent" }))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "该问题已提交，请耐心等待专家回复！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }

        //        model.FeedBackID = int.Parse(this.juActivityBll.GetGUID(BLLJIMP.TransacType.AddJuMasterFeedBackInfo));

        //        if (juActivityBll.Add(model))
        //        {
        //            resp.Status = 1;
        //            resp.Msg = "添加成功！";
        //        }
        //        else
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "添加失败！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "特殊异常：" + ex.Message;
        //    }

        //    if (resp.Status == 1)
        //    {
        //        //添加问答积分
        //        this.userBll.AddUserScore(CurrentUserInfo.UserID, 2, "发表问题", "问与答");
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);

        //}

        ///// <summary>
        ///// 问题回复
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddJuMasterFeedBackDialog(HttpContext context)
        //{
        //    BLLJIMP.Model.JuMasterFeedBackDialogue model = new JuMasterFeedBackDialogue();
        //    model.DialogueID = int.Parse(this.juActivityBll.GetGUID(BLLJIMP.TransacType.AddJuMasterFeedBackDialogue));

        //    model.UserID = this.CurrentUserInfo.UserID;
        //    model.FeedBackID = int.Parse(context.Request["FeedBackID"]);
        //    model.DialogueContent = context.Request["DialogueContent"];
        //    model.SubmitDate = DateTime.Now;
        //    model.SubmitIP = Common.MySpider.GetClientIP();
        //    ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

        //    try
        //    {
        //        if (this.juActivityBll.Add(model, tran))
        //        {
        //            if (juActivityBll.Update(new JuMasterFeedBack(), " ProcessStatus='已回复'", string.Format("FeedBackID={0}", model.FeedBackID), tran) > 0)
        //            {
        //                tran.Commit();
        //                resp.Status = 1;
        //                resp.Msg = "添加成功!";

        //            }
        //            else
        //            {
        //                tran.Rollback();
        //                resp.Status = 0;
        //                resp.Msg = "添加失败!";

        //            }
        //        }
        //        else
        //        {
        //            tran.Rollback();
        //            resp.Status = 0;
        //            resp.Msg = "添加失败!";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        resp.Status = 0;
        //        resp.Msg = ex.Message;


        //    }

        //    if (resp.Status == 1)
        //    {
        //        //添加问答积分
        //        this.userBll.AddUserScore(CurrentUserInfo.UserID, 2, "回复问题", "问与答");
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        ///// <summary>
        ///// 查询问题对话详细
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterFeedBackDialogue(HttpContext context)
        //{
        //    string feedbackid = context.Request["FeedBackID"];
        //    StringBuilder sbWhere = new StringBuilder(string.Format("FeedBackID='{0}'", feedbackid));
        //    sbWhere.Append(" Order by FeedBackID DESC");
        //    // sbWhere.AppendFormat("MasterID='{0}'", MasterID);
        //    List<JuMasterFeedBackDialogue> dataList = new List<JuMasterFeedBackDialogue>();
        //    dataList = juActivityBll.GetList<JuMasterFeedBackDialogue>(sbWhere.ToString());
        //    StringBuilder sb = new StringBuilder();
        //    JuMasterFeedBack feedbackInfo = juActivityBll.Get<JuMasterFeedBack>(string.Format("FeedBackID='{0}'", feedbackid));

        //    UserInfo feedbackUser = this.userBll.GetUserInfo(feedbackInfo.UserID);

        //    sb.AppendLine("<div style=\"border-radius:5px;border: 1px solid #CCC;margin-top:10px;\">");
        //    sb.AppendLine("<div style=\"border-radius:5px;font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");
        //    sb.AppendFormat("<img src=\"{2}\" height=\"25px\" width=\"25px\" style=\"border-radius:50px;\"  />{0} 发表于 {1}", feedbackUser.WXNickname, feedbackInfo.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"), feedbackUser.WXHeadimgurlLocal);
        //    sb.AppendLine("</div>");
        //    sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");
        //    sb.Append(feedbackInfo.FeedBackContent);
        //    sb.AppendLine("</div>");
        //    sb.AppendLine("</div>");

        //    return sb + ConverHtmlFormateMasterFeedBackDialog(dataList);

        //}

        ///// <summary>
        ///// 构造问题对话详细Html
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMasterFeedBackDialog(List<JuMasterFeedBackDialogue> dataList)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        sb.AppendFormat("<div style=\"border-radius:5px;border: 1px solid #CCC;margin-top:10px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" >", item.FeedBackID);
        //        sb.AppendLine("  <div style=\"border-radius:5px;font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");

        //        UserInfo feedBackUser = this.userBll.GetUserInfo(item.UserID);

        //        sb.AppendFormat("<img src=\"{2}\" height=\"25px\" width=\"25px\" style=\"border-radius:50px;\"  />{0} 回复 {1}", feedBackUser.WXNickname, item.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"), feedBackUser.WXHeadimgurlLocal);

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");

        //        sb.Append(item.DialogueContent);

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("</div>");
        //    }
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// 查询鸿风问答列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterFeedBack(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    string FeedBackStatus = context.Request["FeedBackStatus"];
        //    StringBuilder sbWhere = new StringBuilder(" 1=1 ");

        //    string justMe = context.Request["justMe"];

        //    if (justMe == "1")
        //    {
        //        sbWhere.AppendFormat(" And UserID='{0}'", this.CurrentUserInfo.UserID);
        //    }

        //    if (!string.IsNullOrEmpty(FeedBackStatus))
        //    {
        //        if (FeedBackStatus.Equals("0"))
        //        {
        //            FeedBackStatus = "未处理";
        //        }
        //        if (FeedBackStatus.Equals("1"))
        //        {
        //            FeedBackStatus = "已回复";
        //        }

        //        sbWhere.AppendFormat(" And ProcessStatus='{0}'", FeedBackStatus);

        //    }

        //    sbWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);

        //    int count = juActivityBll.GetCount<JuMasterFeedBack>(sbWhere.ToString());
        //    int totalcount = this.juActivityBll.GetTotalPage(count, rows);

        //    List<JuMasterFeedBack> dataList = new List<JuMasterFeedBack>();
        //    dataList = juActivityBll.GetLit<JuMasterFeedBack>(rows, page, sbWhere.ToString(), "FeedBackID DESC");

        //    return ConverHtmlFormateMasterFeedBack(dataList, rows, ((totalcount > page) && page == 1));

        //}


        ///// <summary>
        ///// 构造问答列表html
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMasterFeedBack(List<JuMasterFeedBack> dataList, int rows, bool isShowBtnNext = false)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        UserInfo fUser = new BLLUser("").GetUserInfo(item.UserID);

        //        if (DataLoadTool.CheckCurrUserPms(ZentCloud.BLLPermission.PermissionKey.Pms_Hongfeng_Wap_QuestionDialog))//判断是否有回复权限
        //        {
        //            if (DataLoadTool.CheckCurrUserPms(ZentCloud.BLLPermission.PermissionKey.Pms_Hongfeng_Wap_QuestionDialog_OnlyOwn))//判断是不是只能回复自己的，并且该问题是自己的
        //            {
        //                if (fUser.UserID == this.CurrentUserInfo.UserID)
        //                {
        //                    sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px; border-radius:5px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/App/Cation/Wap/QuestionDialog.aspx?feedbackid={0}\" onclick=\"GotoRel(this)\" >", item.FeedBackID);
        //                }
        //                else
        //                {
        //                    sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px; border-radius:5px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" >");
        //                }
        //            }
        //            else
        //            {
        //                sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px; border-radius:5px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/App/Cation/Wap/QuestionDialog.aspx?feedbackid={0}\" onclick=\"GotoRel(this)\" >", item.FeedBackID);
        //            }
        //        }
        //        else
        //        {
        //            sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px; border-radius:5px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" >");
        //        }

        //        sb.AppendLine("  <div style=\"border-radius:5px;font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");

        //        sb.AppendFormat("<img src=\"{2}\" height=\"25px\" width=\"25px\" style=\"border-radius:50px;\"  />{0} 发表于 {1}", fUser.WXNickname, item.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"), fUser.WXHeadimgurlLocal);

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");

        //        sb.Append(item.FeedBackContent);
        //        // 请问因学习需要可以自带笔记本电脑在图书馆无线上网查资料吗?（总馆和分馆都行吗）
        //        sb.AppendLine("</div>");


        //        //列表显示所有人的回复

        //        List<JuMasterFeedBackDialogue> feedBackDialogueList = this.juActivityBll.GetList<JuMasterFeedBackDialogue>(string.Format("FeedBackID={0} Order by SubmitDate ASC", item.FeedBackID));

        //        foreach (var feedBackDialogue in feedBackDialogueList)
        //        {
        //            if (feedBackDialogue != null)
        //            {
        //                UserInfo zUser = new BLLUser("").GetUserInfo(feedBackDialogue.UserID);

        //                sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;text-align: left;font-weight: bold;font-size: 12px;color:#930;\"> ");
        //                sb.AppendFormat("<img src=\"{1}\" height=\"25px\" width=\"25px\" style=\"border-radius:50px;\"  />{2}回复 {0}", feedBackDialogue.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"), zUser.WXHeadimgurlLocal, zUser.WXNickname);
        //                //管理员回复 2013-10-21 13:12:25
        //                sb.AppendLine("</div>");
        //                sb.AppendLine("<div style=\"margin-left:5px;margin-top:10px;font-size: 12px;color: #666666;line-height: 18px;\">");
        //                sb.Append(feedBackDialogue.DialogueContent);
        //                //可以的。
        //                sb.AppendLine("</div>");
        //            }
        //        }

        //        sb.AppendLine("</div>");


        //    }
        //    if (isShowBtnNext)
        //    {
        //        sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\"></div>");
        //        sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);

        //    }


        //    return sb.ToString();


        //}

        /// <summary>
        /// 删除报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteActivityData(HttpContext context)
        {
            string ids = context.Request["id"];
            var activityId = context.Request["ActivityID"];
            if (this.bllJuActivity.Update(new ActivityDataInfo(), " IsDelete = 1 ", string.Format("UID in({0}) And ActivityID='{1}'", ids, activityId)) > 0)
            {
                return "true";
            }

            return "false";
        }

        /// <summary>
        /// 查询报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryActivityData(HttpContext context)
        {
            string activityId = context.Request["ActivityID"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            var strWhere = string.Format("ActivityID='{0}' AND IsDelete = 0 ", activityId);
            //string pmsGroup = context.Request["pmsGroup"];

            //if (!string.IsNullOrEmpty(pmsGroup))
            //{
            //    Dictionary<string, int> pmsGroupKeyValue = this.juActivityBll.GetHFPmsGroupMatch();
            //    string groupIdStr = "";
            //    foreach (var item in pmsGroup.Split(','))
            //    {
            //        int groupId = 0;
            //        if (pmsGroupKeyValue.TryGetValue(item, out groupId))
            //        {
            //            groupIdStr += "," + groupId.ToString();
            //        }
            //    }
            //    if (!string.IsNullOrWhiteSpace(groupIdStr))
            //    {
            //        searchCondition += string.Format(" AND WeixinOpenID IN (select b.WXOpenId from ZCJ_UserPmsGroupRelationInfo a,ZCJ_UserInfo b where GroupID in({0}) and a.UserID = b.UserID)", groupIdStr.Trim(','));
            //    }
            //}
            List<ActivityDataInfo> list = this.bllJuActivity.GetLit<ActivityDataInfo>(pageSize, pageIndex, strWhere, "UID DESC");
            int totalCount = this.bllJuActivity.GetCount<ActivityDataInfo>(strWhere);
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = list
                });
        }

        ///// <summary>
        ///// 设置权限组
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string SetHFUserPmsGroup(HttpContext context)
        //{
        //    if (!currIsHFAdmin())
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "无权进行该操作!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    string userIds = context.Request["userIds"];
        //    string pmsGroupIdStr = context.Request["pmsGroupId"];

        //    if (string.IsNullOrWhiteSpace(userIds) || string.IsNullOrWhiteSpace(pmsGroupIdStr))
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "用户及权限组不能为空!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    long pmsGroupId = Convert.ToInt32(pmsGroupIdStr, 16);

        //    if (pmsGroupId == 0)
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "权限组ID出错!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }


        //    List<string> userIdList = userIds.Split(',').ToList();

        //    List<long> hfPmsIdList = new List<long>() { 130273, 130334, 130335, 130388 };

        //    Dictionary<string, string> userPmsOld = new Dictionary<string, string>();

        //    foreach (var item in userIdList)
        //    {
        //        UserInfo upUser = this.userBll.GetUserInfo(item);
        //        userPmsOld.Add(item, upUser.HFUserPmsGroup);
        //    }

        //    //先将鸿风三权限移除，然后重新配置权限
        //    foreach (var item in hfPmsIdList)
        //    {


        //        this.juActivityBll.Delete(new UserPmsGroupRelationInfo(), string.Format("GroupID = {0} and UserID in ({1})",
        //           item,
        //           Common.StringHelper.ListToStr<string>(userIdList, "'", ",")
        //       ));

        //    }

        //    List<UserPmsGroupRelationInfo> userPmsGroupList = new List<UserPmsGroupRelationInfo>();

        //    foreach (var item in userIdList)
        //    {
        //        UserPmsGroupRelationInfo model = new UserPmsGroupRelationInfo();

        //        model.UserID = item;
        //        model.GroupID = pmsGroupId;

        //        userPmsGroupList.Add(model);
        //    }

        //    if (this.juActivityBll.AddList<UserPmsGroupRelationInfo>(userPmsGroupList))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "更新成功!";


        //        Dictionary<string, string> userPmsNew = new Dictionary<string, string>();

        //        foreach (var item in userIdList)
        //        {
        //            UserInfo upUser = this.userBll.GetUserInfo(item);
        //            //如何是游客转换成其他角色则记录下转正时间
        //            string oldPms = userPmsOld[item];
        //            if (oldPms == "游客" && oldPms != upUser.HFUserPmsGroup)
        //            {
        //                upUser.ToHFUserDate = DateTime.Now;
        //                this.userBll.Update(upUser);
        //            }

        //            //更新鸿风角色字段到数据库
        //            upUser.HFPmsGroupStr = upUser.HFUserPmsGroup;
        //            this.userBll.Update(upUser);
        //        }


        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "更新失败!";

        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);

        //}
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateUserPwd(HttpContext context)
        {
            string userPwd = context.Request["Password"];
            string autoId = context.Request["id"];
            UserInfo model = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
            if (model == null)
            {
                resp.Msg = "用户不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            model.Password = userPwd;
            if (bllUser.UpdatePassword(model))
            {
                resp.Msg = "修改成功";
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "修改失败";
                resp.Status = 0;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询站点用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string
            QueryWebsiteUser(HttpContext context)
        {

            #region 旧的
            //if (!currIsHFAdmin())
            //    return "无权限!";

            //int page = Convert.ToInt32(context.Request["page"]);
            //int rows = Convert.ToInt32(context.Request["rows"]);

            //string pmsGroup = context.Request["pmsGroup"];

            //StringBuilder strWhere = new StringBuilder();

            ////用户判断条件：有微信昵称、有姓名的。
            //strWhere.Append(" ((TrueName <> '' and TrueName is not null ) or (WXNickname <> '' and WXNickname is not null))");

            //strWhere.AppendFormat("and WebsiteOwner = '{0}' ", this.websiteOwner);

            ////根据权限组ID查询用户

            //if (!string.IsNullOrEmpty(pmsGroup))
            //{
            //    Dictionary<string, int> pmsGroupKeyValue = this.juActivityBll.GetHFPmsGroupMatch();
            //    string groupIdStr = "";
            //    foreach (var item in pmsGroup.Split(','))
            //    {
            //        int groupId = 0;
            //        if (pmsGroupKeyValue.TryGetValue(item, out groupId))
            //        {
            //            groupIdStr += "," + groupId.ToString();
            //        }
            //    }
            //    if (!string.IsNullOrWhiteSpace(groupIdStr))
            //    {
            //        strWhere.AppendFormat(" AND UserID IN ( SELECT userid FROM ZCJ_UserPmsGroupRelationInfo WHERE GroupID in ({0}))", groupIdStr.Trim(','));
            //    }
            //}

            ////case 130273:
            ////           return "管理员";
            ////       case 130334:
            ////           return "游客";
            ////       case 130335:
            ////           return "正式学员";
            ////       case 130388:
            ////           return "教师";

            //List<ZentCloud.BLLJIMP.Model.UserInfo> userList = this.juActivityBll.GetLit<ZentCloud.BLLJIMP.Model.UserInfo>(rows, page, strWhere.ToString(), " CHARINDEX(SUBSTRING(HFPmsGroupStr,1,1),'教管正游'),Regtime DESC ");

            //return Common.JSONHelper.ListToEasyUIJson(this.juActivityBll.GetCount<ZentCloud.BLLJIMP.Model.UserInfo>(strWhere.ToString()), userList);

            #endregion

            #region 新的
            //if (!currIsHFAdmin())
            //    return "无权限!";

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["KeyWord"];
            string tagName = context.Request["TagName"];

            string haveTrueName = context.Request["HaveTrueName"];
            string haveWxNickNameAndTrueName = context.Request["HaveWxNickNameAndTrueName"];

            string isFans = context.Request["IsFans"];//是否是粉丝
            string isReg = context.Request["IsReg"];//是否是会员
            string isDisOnLineUser = context.Request["IsDisOnLineUser"];//是否商城分销会员
            string isDisOffLineUser = context.Request["IsDisOffLineUser"];//是否业务分销会员
            string isPhoneReg = context.Request["IsPhoneReg"];//是否手机验证会员
            string isName = context.Request["isName"];
            string isPhone = context.Request["isPhone"];
            string isEmail = context.Request["isEmail"];
            string isWxnickName = context.Request["isWxnickName"];
            string isMember = context.Request["isMember"];
            string userAutoId = context.Request["autoId"];//用户AutoId
            string isWeixinUser = context.Request["isWeixinUser"];//是否微信用户
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner = '{0}' And UserId !='{1}' ", bllBase.WebsiteOwner, bllBase.WebsiteOwner);

            if (!string.IsNullOrEmpty(userAutoId))//用户自动编号
            {
                sbWhere.AppendFormat(" And AutoId={0} ", userAutoId);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                //sbWhere.AppendFormat("And ( UserID  like'%{0}%' ", keyWord);
                sbWhere.AppendFormat("And ( ", keyWord);
                sbWhere.AppendFormat(" TrueName  like'{0}%' ", keyWord);
                sbWhere.AppendFormat("OR Company  like'{0}%' ", keyWord);
                //sbWhere.AppendFormat("OR Postion  like'{0}%' ", keyWord);
                sbWhere.AppendFormat("OR Phone  like'{0}%' ", keyWord);
                sbWhere.AppendFormat("OR WXNickName  like'%{0}%' )", keyWord);
                //sbWhere.AppendFormat("OR Email  like'{0}%' )", keyWord);
            }

            #region 标签搜索
            if (!string.IsNullOrEmpty(tagName))
            {

                string[] tagNameArray = tagName.Split(',');
                sbWhere.AppendFormat(" AND( ");
                for (int i = 0; i < tagNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tagNameArray[i]))
                    {
                        if (i > 0)
                        {
                            sbWhere.AppendFormat(" OR TagName like '%{0}%' ", tagNameArray[i]);
                        }
                        else
                        {
                            sbWhere.AppendFormat(" TagName like '%{0}%' ", tagNameArray[i]);
                        }

                    }
                }
                sbWhere.AppendFormat(") ");
            }
            #endregion


            if (!string.IsNullOrEmpty(haveTrueName))
            {
                sbWhere.AppendFormat(" AND TrueName !=''");
            }
            if (!string.IsNullOrEmpty(haveWxNickNameAndTrueName))
            {
                sbWhere.AppendFormat(" AND TrueName !='' And WXNickname !=''");
            }
            //if (!string.IsNullOrEmpty(isDistributionUser))
            //{
            //    sbWhere.AppendFormat(" AND DistributionOwner is not null And DistributionOwner !=''");

            //}

            //if (!string.IsNullOrEmpty(isDistributionOffLineUser))
            //{
            //    sbWhere.AppendFormat(" AND DistributionOffLinePreUserId is not null And DistributionOffLinePreUserId !=''");

            //}

            //if ((!string.IsNullOrEmpty(isFans))||(!string.IsNullOrEmpty(isReg))||(!string.IsNullOrEmpty(isPhoneReg))||(!string.IsNullOrEmpty(isDisOffLineUser))||(!string.IsNullOrEmpty(isDisOnLineUser)))
            //{
            //用户类型筛选
            //sbWhere.AppendFormat("And ( 1=1 ");

            if (!string.IsNullOrEmpty(isDisOffLineUser))//业务分销会员
            {
                sbWhere.AppendFormat(" And DistributionOffLinePreUserId is not null And DistributionOffLinePreUserId !=''");

            }
            if (!string.IsNullOrEmpty(isDisOnLineUser))//商城分销会员
            {
                sbWhere.AppendFormat(" And DistributionOwner is not null And DistributionOwner !=''");

            }
            if (!string.IsNullOrEmpty(isFans))//粉丝
            {
                //sbWhere.AppendFormat(" And (WXNickname is not null And WXNickname!='') ");
                sbWhere.AppendFormat(" And IsWeixinFollower=1 ");

            }
            if (!string.IsNullOrEmpty(isReg))//注册会员
            {
                sbWhere.AppendFormat(" And (TrueName>'' or WXNickname>'')");
            }
            if (!string.IsNullOrEmpty(isPhoneReg))//是否手机认证会员
            {
                sbWhere.AppendFormat(" And (Phone is not null And Phone!='' And IsPhoneVerify=1)");
            }
            if (!string.IsNullOrEmpty(isName))//姓名
            {
                sbWhere.AppendFormat(" And TrueName>''  ");
            }
            if (!string.IsNullOrEmpty(isPhone))//手机
            {
                sbWhere.AppendFormat(" And Phone>'' ");
            }
            if (!string.IsNullOrEmpty(isEmail))//邮箱
            {
                sbWhere.AppendFormat(" And Email>'' ");
            }
            if (!string.IsNullOrEmpty(isWxnickName))//昵称
            {
                sbWhere.AppendFormat(" And WxnickName>'' ");
            }
            if (!string.IsNullOrEmpty(isMember))//昵称
            {
                BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
                CompanyWebsite_Config nWebsiteConfig = bllTableFieldMap.GetByKey<CompanyWebsite_Config>("WebsiteOwner", bllTableFieldMap.WebsiteOwner);
                int memberStandard = nWebsiteConfig == null ? 0 : nWebsiteConfig.MemberStandard;
                string memberStandardFields = "";
                if (memberStandard == 2 || memberStandard == 3)
                {
                    List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo");
                    if (listFieldList.Count > 0) memberStandardFields = ZentCloud.Common.MyStringHelper.ListToStr(listFieldList.Select(p => p.Field).ToList(), "", ",");
                }
                List<string> memberStandardFieldList = new List<string>();
                if (!string.IsNullOrWhiteSpace(memberStandardFields))
                {
                    memberStandardFieldList = memberStandardFields.Split(',').Where(p => p.Trim().Equals("")).ToList();
                }
                if (memberStandard == 1)
                {
                    sbWhere.AppendFormat(" AND Phone > '' ");
                    sbWhere.AppendFormat(" AND IsPhoneVerify = 1 ");
                }
                else if (memberStandard == 2 || memberStandard == 3)
                {
                    sbWhere.AppendFormat(" AND IsPhoneVerify = 1 ");
                    if (memberStandard == 3)
                        sbWhere.AppendFormat(" AND MemberApplyStatus = 9 ");

                    foreach (string field in memberStandardFieldList)
                    {
                        sbWhere.AppendFormat(" AND {0} > '' ", field);
                    }
                }
                sbWhere.AppendFormat(" And AccessLevel>0 ");
            }

            if (!string.IsNullOrEmpty(isWeixinUser))
            {
                if (isWeixinUser == "1")
                {
                    sbWhere.AppendFormat(" And UserId like 'WX%' ");
                }
                if (isWeixinUser == "-1")
                {
                    //And (IsFirstLevelDistribution='' Or IsFirstLevelDistribution Is Null)
                    sbWhere.AppendFormat(" And UserId Not like 'WX%' And (PermissionGroupID Is NULL) ");
                }

            }
            List<ZentCloud.BLLJIMP.Model.UserInfo> userList = this.bllUser.GetColList<ZentCloud.BLLJIMP.Model.UserInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC",
                "AutoID,UserID,WXNickname,WXHeadimgurl,TrueName,Phone,Company,Postion,Email,TagName,TotalScore,DistributionOwner,UserType,AccessLevel,AvailableVoteCount,WXOpenId,AccountAmount");
            int tCount = this.bllUser.GetCount<ZentCloud.BLLJIMP.Model.UserInfo>(sbWhere.ToString());
            var users = from p in userList
                        select new
                        {
                            p.AutoID,
                            p.UserID,
                            p.WXNickname,
                            p.WXHeadimgurlLocal,
                            p.TrueName,
                            p.Phone,
                            p.Company,
                            p.Postion,
                            p.Email,
                            p.TagName,
                            p.TotalScore,
                            p.DistributionOwner,
                            p.UserType,
                            p.AccessLevel,
                            p.AvailableVoteCount,
                            p.WXOpenId,
                            p.AccountAmount
                        };
            var result = new
            {
                total = tCount,
                rows = users
            };
            return JsonConvert.SerializeObject(result);
            #endregion

        }

        /// <summary>
        /// 查询商城分销用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWebsiteUserDistributionOnLine(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["KeyWord"];
            string sort = context.Request["sort"];//排序字段
            string order = context.Request["order"]; //asc 
            string tagName = context.Request["tagName"];//标签
            string recommendUserIds = context.Request["RecommendUserIds"];//推荐人UserId
            string payStatus = context.Request["payStatus"];//付款状态
            string addSystem = context.Request["AddSystemUser"];//是否添加秕



            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner = '{0}' And DistributionOwner !=''", bllUser.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And ( TrueName like '{0}%' OR WXNickname  like '{0}%' ) ", keyWord);
            }

            #region 标签搜索
            if (!string.IsNullOrEmpty(tagName))
            {
                List<string> tagNameArray = tagName.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                sbWhere.AppendFormat(" AND( ");
                for (int i = 0; i < tagNameArray.Count; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" TagName like '%{0}%' ", tagNameArray[i]);
                    }
                    else
                    {
                        sbWhere.AppendFormat(" OR TagName like '%{0}%' ", tagNameArray[i]);
                    }
                }
                sbWhere.AppendFormat(") ");
            }
            #endregion

            if (!string.IsNullOrEmpty(recommendUserIds))
            {
                recommendUserIds = "'" + recommendUserIds.Replace(",", "','") + "'";

                sbWhere.AppendFormat(" And DistributionOwner in({0})", recommendUserIds);
            }
            if (!string.IsNullOrEmpty(payStatus))
            {
                if (payStatus == "1")//已消费
                {
                    sbWhere.AppendFormat(" And DistributionSaleAmountLevel0>0 ");
                    // sbWhere.AppendFormat(" And ((select Count(*) from ZCJ_WXMallOrderInfo Where OrderUserId=UserId And PayMentStatus=1 And  OrderType in(0,1,2) And IsRefund!=1 And Status!='已取消' And TotalAmount>0)>0)");
                }
                else//未消费
                {
                    sbWhere.AppendFormat(" And DistributionSaleAmountLevel0=0 ");
                    // sbWhere.AppendFormat(" And ((select Count(*) from ZCJ_WXMallOrderInfo Where OrderUserId=UserId And PayMentStatus=1 And  OrderType in(0,1,2) And IsRefund!=1 And Status!='已取消' And TotalAmount>0)=0)");
                }
            }


            var webSite = bllWebsite.GetWebsiteInfoModelFromDataBase();
            if (webSite.DistributionGetWay == 1)
            {
                sbWhere.AppendFormat(" AND MemberLevel>0 ");
            }



            string orderBy = " DistributionDownUserCountLevel1 DESC";
            if (!string.IsNullOrEmpty(sort) && (!string.IsNullOrEmpty(order)))
            {
                orderBy = sort + " " + order;
                if (sort == "CanUseAmount")
                {
                    orderBy = "( TotalAmount-FrozenAmount)" + " " + order;
                }
                if (sort == "SalesQuota")//累计销售
                {
                    orderBy = "( DistributionSaleAmountLevel0+DistributionSaleAmountLevel1)" + " " + order;
                }

            }



            int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            //var sourceUserList = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            var sourceUserList = bllUser.GetColList<UserInfo>(pageSize, pageIndex, sbWhere.ToString() + " order by" + orderBy,
            "AutoID,UserID,WXNickname,WXHeadimgurl,TrueName,Phone,TagName,HistoryDistributionOnLineTotalAmountEstimate,DistributionDownUserCountLevel1,DistributionSaleAmountLevel0,DistributionSaleAmountLevel1,TotalAmount,FrozenAmount,DistributionOwner,MemberLevel,HexiaoCode");
            if (!string.IsNullOrEmpty(addSystem))
            {
                UserInfo systemUserInfo = new UserInfo();
                systemUserInfo.WXNickname = "系统";
                systemUserInfo.UserID = bllBase.WebsiteOwner;
                systemUserInfo.TrueName = "系统";
                systemUserInfo.Phone = "系统";
                sourceUserList.Add(systemUserInfo);
                sourceUserList.Insert(0, systemUserInfo);
            }
            sourceUserList = sourceUserList.DistinctBy(p => p.AutoID).ToList();
            var userList = from p in sourceUserList
                           select new
                           {
                               p.AutoID,
                               p.UserID,
                               p.WXNickname,
                               WXHeadimgurl = !string.IsNullOrEmpty(p.WXHeadimgurl) ? p.WXHeadimgurl : "/img/persion.png",
                               p.TrueName,
                               p.Phone,
                               p.TagName,
                               CanUseAmount = bllDis.GetUserCanUseAmount(p),
                               DistributionOnLineRecomendUserInfo = bllUser.GetUserInfo(p.DistributionOwner),
                               p.DistributionDownUserCountLevel1,//会员数
                               p.DistributionSaleAmountLevel1,//会员消费额
                               p.DistributionSaleAmountLevel0,//自己销售额
                               SalesQuota = p.DistributionSaleAmountLevel0 + p.DistributionSaleAmountLevel1,//累计销售
                               p.HistoryDistributionOnLineTotalAmountEstimate,//累计奖励（预估）
                               OverCanUseAmount = Math.Round(bllDis.GetUserWithdrawTotalAmount(p), 2), //已提现奖励
                               MemberLevel = p.MemberLevel > 0 ? bllUser.GetMemberLevelName(p.MemberLevel, "DistributionOnLine") : "",
                               p.HexiaoCode
                           };
            var result = new
            {
                total = totalCount,
                rows = userList
            };
            return JsonConvert.SerializeObject(result);


        }



        #region 渠道
        /// <summary>
        /// 渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryChannel(HttpContext context)
        {

            //int pageIndex = Convert.ToInt32(context.Request["page"]);
            //int pageSize = Convert.ToInt32(context.Request["rows"]);
            string permissionGroupId = bllDis.GetChannelPermissionGroupId();//渠道用户组
            if (string.IsNullOrEmpty(permissionGroupId))
            {
                permissionGroupId = "0";
            }
            string keyWord = context.Request["KeyWord"];
            string sort = context.Request["sort"];//排序字段
            string order = context.Request["order"]; //asc 
            // string isDistributionOwner = context.Request["isDistributionOwner"];//判断是分销员还是渠道
            //string tagName = context.Request["tagName"];//标签
            //string recommendUserIds = context.Request["RecommendUserIds"];//推荐人UserId
            //string parentChannel=context.Request["parentChannel"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner = '{0}' And PermissionGroupID={1} ", bllUser.WebsiteOwner, permissionGroupId));

            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And  ChannelName like '%{0}%' ", keyWord);
            }
            //if (!string.IsNullOrEmpty(isDistributionOwner) && isDistributionOwner == "1")
            //{
            //    sbWhere.AppendFormat(" And PermissionGroupID>'' ");
            //}
            //if (!string.IsNullOrEmpty(isDistributionOwner) && isDistributionOwner == "2")
            //{
            //    sbWhere.AppendFormat(" And PermissionGroupID is null ");
            //}
            //#region 标签搜索
            //if (!string.IsNullOrEmpty(tagName))
            //{
            //    List<string> tagNameArray = tagName.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            //    sbWhere.AppendFormat(" AND( ");
            //    for (int i = 0; i < tagNameArray.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            sbWhere.AppendFormat(" TagName like '%{0}%' ", tagNameArray[i]);
            //        }
            //        else
            //        {
            //            sbWhere.AppendFormat(" OR TagName like '%{0}%' ", tagNameArray[i]);
            //        }
            //    }
            //    sbWhere.AppendFormat(") ");
            //}
            //#endregion
            //if (!string.IsNullOrEmpty(recommendUserIds))
            //{
            //    recommendUserIds = "'" + recommendUserIds.Replace(",", "','") + "'";

            //    sbWhere.AppendFormat(" And DistributionOwner in({0})", recommendUserIds);
            //}
            //string orderBy = " DistributionDownUserCountLevel1 DESC";
            //if (!string.IsNullOrEmpty(sort) && (!string.IsNullOrEmpty(order)))
            //{
            //    orderBy = sort + " " + order;
            //    if (sort == "CanUseAmount")
            //    {
            //        orderBy = "( TotalAmount-FrozenAmount)" + " " + order;
            //    }
            //}
            int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            var sourceUserList = bllUser.GetList<UserInfo>(sbWhere.ToString());

            List<UserInfo> showList = new List<UserInfo>();
            ZentCloud.Common.MyCategoriesV2 myCategories = new ZentCloud.Common.MyCategoriesV2();
            List<ZentCloud.Common.Model.MyCategoryV2Model> myCategoryModel = myCategories.GetCommCateModelList("UserID", "ParentChannel", "ChannelName", sourceUserList.ToList());
            var it = myCategories.GetCateListItem(myCategoryModel, "");
            foreach (ListItem item in myCategories.GetCateListItem(myCategoryModel, ""))
            {
                try
                {


                    UserInfo userInfo = sourceUserList.First(p => p.UserID.Equals(item.Value));
                    userInfo.ChannelName = item.Text;
                    showList.Add(userInfo);
                }
                catch { continue; }
            }
            var userList = from p in showList
                           select new
                           {
                               p.AutoID,
                               p.UserID,
                               p.ChannelName,
                               p.Description,
                               p.WXNickname,
                               p.WXHeadimgurlLocal,
                               p.TrueName,
                               p.Phone,
                               p.Company,
                               p.Postion,
                               p.Email,
                               p.TagName,
                               p.TotalScore,
                               p.DistributionOwner,
                               p.UserType,
                               p.AccessLevel,
                               p.AvailableVoteCount,
                               p.WXOpenId,
                               p.AccountAmount,
                               p.TotalAmount,
                               CanUseAmount = bllDis.GetUserCanUseAmount(p),
                               //p.HistoryDistributionOnLineTotalAmount,
                               //p.DistributionOnLineRecomendUserInfo,
                               FirstLevelDistributionCount = bllUser.GetCount<UserInfo>(string.Format("WebsiteOwner='{0}' And ParentChannel='{1}' And IsFirstLevelDistribution='1'", bllUser.WebsiteOwner, p.UserID)),//二维码数量
                               p.DistributionDownUserCountLevel1,//直接会员数量
                               p.DistributionDownUserCountAll,//所有会员数量
                               p.DistributionSaleAmountLevel1,//直接销售
                               p.DistributionSaleAmountAll,//所有销售
                               //DistributionDownUserCountLevel1 = bllDis.GetChannelAllFirstLevelChildUser(p.UserID).Count,//直接会员数量
                               // DistributionDownUserCountAll = bllDis.GetChannelAllChildUser(p.UserID).Count(),//所有会员数量
                               //p.DistributionDownUserCountLevel2,
                               //p.DistributionDownUserCountLevel3,
                               //DistributionSaleAmountLevel1 = bllDis.GetChannelAllFirstLevelOrder(p.UserID).Sum(s => s.TotalAmount),//直接销售额
                               //DistributionSaleAmountAll = bllDis.GetChannelAllOrder(p.UserID).Sum(s => s.TotalAmount),//累计销售
                               //p.DistributionSaleAmountLevel2,
                               //p.DistributionSaleAmountLevel3,
                               //p.HexiaoCode
                               //DistributionSaleAmountLevel0 = GetDistributionSaleAmountLevel0(p),
                               //SalesQuota = Math.Round(bllDis.GetUserSalesQuota(p), 2),//累计销售
                               p.HistoryDistributionOnLineTotalAmountEstimate,//累计奖励（预估）
                               //OverCanUseAmount = Math.Round(bllDis.GetUserWithdrawTotalAmount(p), 2), //已提现奖励
                               OverCanUseAmount = 0, //已提现奖励
                               //ParentChannelUserInfo=bllUser.GetUserInfo(p.ParentChannel),
                               p.ChannelLevelId,
                               MgrUserInfo = bllUser.GetUserInfo(p.MgrUserId),
                               p.ParentChannel,
                               LevelName = bllBase.Get<UserLevelConfig>(string.Format("AutoId={0}", p.ChannelLevelId)) != null ? bllBase.Get<UserLevelConfig>(string.Format("AutoId={0}", p.ChannelLevelId)).LevelString : ""
                           };


            #region 排序
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "DistributionDownUserCountAll"://所有会员排序
                        if (order == "asc")
                        {
                            userList = userList.OrderBy(p => p.DistributionDownUserCountAll).ToList();

                        }
                        if (order == "desc")
                        {
                            userList = userList.OrderByDescending(p => p.DistributionDownUserCountAll).ToList();
                        }
                        break;
                    case "FirstLevelDistributionCount"://二维码数量排序
                        if (order == "asc")
                        {
                            userList = userList.OrderBy(p => p.FirstLevelDistributionCount).ToList();

                        }
                        if (order == "desc")
                        {
                            userList = userList.OrderByDescending(p => p.FirstLevelDistributionCount).ToList();
                        }
                        break;
                    case "DistributionDownUserCountLevel1"://渠道下二维码直接会员数
                        if (order == "asc")
                        {
                            userList = userList.OrderBy(p => p.DistributionDownUserCountLevel1).ToList();

                        }
                        if (order == "desc")
                        {
                            userList = userList.OrderByDescending(p => p.DistributionDownUserCountLevel1).ToList();
                        }
                        break;
                    case "DistributionSaleAmountLevel1"://渠道直接销售
                        if (order == "asc")
                        {
                            userList = userList.OrderBy(p => p.DistributionSaleAmountLevel1).ToList();

                        }
                        if (order == "desc")
                        {
                            userList = userList.OrderByDescending(p => p.DistributionSaleAmountLevel1).ToList();
                        }
                        break;
                    case "DistributionSaleAmountAll"://渠道累计销售
                        if (order == "asc")
                        {
                            userList = userList.OrderBy(p => p.DistributionSaleAmountAll).ToList();

                        }
                        if (order == "desc")
                        {
                            userList = userList.OrderByDescending(p => p.DistributionSaleAmountAll).ToList();
                        }
                        break;
                    default:
                        break;
                }

            }
            #endregion
            var result = new
            {
                total = totalCount,
                rows = userList
            };
            return JsonConvert.SerializeObject(result);






        }

        /// <summary>
        /// 渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryAllChannel(HttpContext context)
        {
            string permissionGroupId = bllDis.GetChannelPermissionGroupId();//渠道用户组
            if (string.IsNullOrEmpty(permissionGroupId))
            {
                permissionGroupId = "0";
            }
            //StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner = '{0}' And PermissionGroupID={1} ", bllUser.WebsiteOwner, permissionGroupId));
            //var sourceUserList = bllUser.GetList<UserInfo>(sbWhere.ToString());
            //return JsonConvert.SerializeObject(sourceUserList);

            var allChannelList = bllUser.GetList<UserInfo>(string.Format(" WebsiteOwner = '{0}' And PermissionGroupID={1} ", bllUser.WebsiteOwner, permissionGroupId));
            ZentCloud.Common.MyCategoriesV2 myCategories = new ZentCloud.Common.MyCategoriesV2();
            List<ZentCloud.Common.Model.MyCategoryV2Model> myCategoryModel = myCategories.GetCommCateModelList("UserID", "ParentChannel", "ChannelName", allChannelList);
            var itemList = myCategories.GetCateListItem(myCategoryModel, "");
            var list = from p in itemList
                       select new
                       {
                           p.Text,
                           p.Value
                       };

            return JsonConvert.SerializeObject(list);


        }

        /// <summary>
        /// 渠道下的第一层分销员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryChildChannel(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["page"]);
            int pageSize = int.Parse(context.Request["rows"]);
            string keyWord = context.Request["KeyWord"];
            string parentChannel = context.Request["parentChannel"];
            string parentChannels = context.Request["parentChannels"];
            string sort = context.Request["sort"];
            string order = context.Request["order"];
            StringBuilder sbWhere = new StringBuilder();

            sbWhere.AppendFormat("WebsiteOwner='{0}'  And IsFirstLevelDistribution='1'", bllUser.WebsiteOwner);

            if (!string.IsNullOrEmpty(parentChannel))
            {
                sbWhere.AppendFormat(" And  ParentChannel='{0}'", parentChannel);
            }
            if (!string.IsNullOrEmpty(parentChannels))
            {
                sbWhere.AppendFormat(" And  ParentChannel='{0}'", parentChannels);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (TrueName like '%{0}%' Or Cast(AutoID AS Nvarchar(20))='{0}')", keyWord);
            }

            string orderBy = "AutoId DESC";

            if (!string.IsNullOrEmpty(sort))
            {
                orderBy = sort + " " + order;
            }
            int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            //var sourceUserList = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            var sourceUserList = bllUser.GetColList<UserInfo>(pageSize, pageIndex, sbWhere.ToString() + " order by" + orderBy,
            "AutoID,UserID,TrueName,WXNickName,DistributionDownUserCountLevel1,DistributionDownUserCountAll,DistributionSaleAmountLevel1,DistributionSaleAmountAll,TotalAmount,FrozenAmount,ParentChannel");

            var userList = from p in sourceUserList
                           select new
                           {
                               p.AutoID,
                               p.UserID,
                               //p.WXNickname,
                               //p.WXHeadimgurlLocal,
                               TrueName = bllUser.GetUserDispalyName(p),
                               //p.Phone,
                               //p.Company,
                               //p.Postion,
                               //p.Email,
                               //p.TagName,
                               //p.TotalScore,
                               //p.DistributionOwner,
                               //p.UserType,
                               //p.AccessLevel,
                               //p.AvailableVoteCount,
                               //p.WXOpenId,
                               //p.AccountAmount,
                               //p.TotalAmount,
                               p.ParentChannel,
                               CanUseAmount = Math.Round(bllDis.GetUserCanUseAmount(p)),
                               //p.HistoryDistributionOnLineTotalAmount,
                               //p.DistributionOnLineRecomendUserInfo,
                               ParentChannelName = bllUser.GetUserInfo(p.ParentChannel).ChannelName,
                               p.DistributionDownUserCountLevel1,//一级会员数
                               p.DistributionDownUserCountAll,//所有会员数
                               //DistributionDownUserCountAll = bllDis.GetAllDownUsersList(p.UserID).Where(a => a.UserID != p.UserID).Count(),//所有会员数
                               p.DistributionSaleAmountLevel1,//一级销售额
                               p.DistributionSaleAmountAll//所有销售额
                               //DistributionSaleAmountAll = bllDis.GetDisSaleAmount(p.UserID)//所有销售额
                               //p.DistributionDownUserCountLevel2,
                               //p.DistributionDownUserCountLevel3,
                               //DistributionSaleAmountLevel1=bllDis.GetAllDownUsesrList(p.UserID).Where(u=>p.UserID!=u.UserID).ToList().Count,
                               //p.DistributionSaleAmountLevel2,
                               //p.DistributionSaleAmountLevel3,
                               //p.HexiaoCode
                               //DistributionSaleAmountLevel0 = GetDistributionSaleAmountLevel0(p),
                               // SalesQuota = Math.Round(bllDis.GetUserSalesQuota(p), 2),//累计销售
                               // HistoryDistributionOnLineTotalAmountEstimate = p.HistoryDistributionOnLineTotalAmountEstimate,//累计奖励（预估）
                               // OverCanUseAmount = Math.Round(bllDis.GetUserWithdrawTootalAmount(p), 2) //已提现奖励
                           };

            //#region 排序
            //if (!string.IsNullOrEmpty(sort))
            //{
            //    switch (sort)
            //    {
            //        case "DistributionDownUserCountLevel1"://一级会员排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionDownUserCountLevel1).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionDownUserCountLevel1).ToList();
            //            }
            //            break;
            //        case "DistributionDownUserCountAll"://所有会员数排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionDownUserCountAll).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionDownUserCountAll).ToList();
            //            }
            //            break;
            //        case "DistributionSaleAmountLevel1"://一级销售额排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionSaleAmountLevel1).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionSaleAmountLevel1).ToList();
            //            }
            //            break;
            //        case "DistributionSaleAmountAll"://所有销售额排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionSaleAmountAll).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionSaleAmountAll).ToList();
            //            }
            //            break;
            //        default:
            //            break;
            //    }

            //} 
            //#endregion

            var result = new
            {
                total = totalCount,
                rows = userList
            };
            return JsonConvert.SerializeObject(result);


        }






        /// <summary>
        /// 添加渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddChannel(HttpContext context)
        {

            string channelName = context.Request["ChannelName"];
            string description = context.Request["Description"];
            string parentChannel = context.Request["ParentChannel"];
            string trueName = context.Request["TrueName"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            string channelLevelId = context.Request["ChannelLevelId"];

            UserInfo userInfo = new UserInfo();
            userInfo.ChannelName = channelName;
            userInfo.Description = description;
            userInfo.UserID = string.Format("{0}_Channel_{1}", bllUser.WebsiteOwner, bllUser.GetGUID(TransacType.CommAdd));
            userInfo.Password = "";
            userInfo.ParentChannel = parentChannel;
            userInfo.TrueName = trueName;
            userInfo.Company = company;
            userInfo.Postion = position;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.WebsiteOwner = bllUser.WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.Birthday = DateTime.Now;
            userInfo.ChannelLevelId = channelLevelId;
            if (string.IsNullOrEmpty(userInfo.ChannelName))
            {
                resp.Msg = "渠道名称必填";
                return JsonConvert.SerializeObject(resp);
            }
            string permissionGroupId = bllDis.GetChannelPermissionGroupId();//渠道用户组
            if (string.IsNullOrEmpty(permissionGroupId))
            {
                resp.Msg = "系统中无渠道角色";
                return JsonConvert.SerializeObject(resp);
            }
            else
            {
                userInfo.PermissionGroupID = long.Parse(permissionGroupId);
            }
            if (string.IsNullOrEmpty(userInfo.UserID))
            {
                resp.Msg = "账户名必填";
                return JsonConvert.SerializeObject(resp);
            }

            if (userInfo.UserID == userInfo.ParentChannel)
            {
                resp.Msg = "账户名不能与上级渠道相同 ";
                return JsonConvert.SerializeObject(resp);
            }
            if (string.IsNullOrEmpty(channelLevelId))
            {
                resp.Msg = "请选择等级";
                return JsonConvert.SerializeObject(resp);
            }


            if (bllUser.Add(userInfo))
            {
                resp.IsSuccess = true;

            }
            else
            {
                resp.Msg = "添加失败";
            }

            return JsonConvert.SerializeObject(resp);


        }


        /// <summary>
        /// 编辑渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditChannel(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string channelName = context.Request["ChannelName"];
            string description = context.Request["Description"];
            string parentChannel = context.Request["ParentChannel"];
            string trueName = context.Request["TrueName"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            string channelLevelId = context.Request["ChannelLevelId"];
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
            //userInfo.Password = passWord;
            userInfo.ChannelName = channelName;
            userInfo.Description = description;
            userInfo.ParentChannel = parentChannel;
            userInfo.TrueName = trueName;
            userInfo.Company = company;
            userInfo.Postion = position;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.ChannelLevelId = channelLevelId;

            if (string.IsNullOrEmpty(userInfo.ChannelName))
            {
                resp.Msg = "渠道名称必填";
                return JsonConvert.SerializeObject(resp);
            }
            if (string.IsNullOrEmpty(channelLevelId))
            {
                resp.Msg = "请选择等级";
                return JsonConvert.SerializeObject(resp);
            }
            if (userInfo.UserID == userInfo.ParentChannel)
            {
                resp.Msg = "账户名不能与上级渠道相同 ";
                return JsonConvert.SerializeObject(resp);
            }

            if (bllUser.Update(userInfo))
            {
                resp.IsSuccess = true;

            }
            else
            {
                resp.Msg = "编辑失败";
            }

            return JsonConvert.SerializeObject(resp);


        }

        /// <summary>
        ///设置微信管理员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteChannel(HttpContext context)
        {

            string userId = context.Request["userId"];
            if (bllUser.Update(new UserInfo(), string.Format("PermissionGroupID =null"), string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.IsSuccess = true;
            }
            return JsonConvert.SerializeObject(resp);

        }

        /// <summary>
        ///刷新渠道数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FlashChannelData(HttpContext context)
        {

            TimingTask task = new TimingTask();
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = 1;
            task.TaskInfo = "刷新渠道数据";
            task.TaskType = 11;
            task.ScheduleDate = DateTime.Now;

            if (bllDis.Add(task))
            {
                resp.IsSuccess = true;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }
        /// <summary>
        ///设置微信管理员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetWeixinMgr(HttpContext context)
        {

            string userId = context.Request["userId"];
            string mgrUserId = context.Request["mgrUserId"];
            UserInfo mgrUserInfo = bllUser.GetUserInfo(mgrUserId);
            //if (string.IsNullOrEmpty(mgrUserInfo.Channel))
            //{
            //    resp.Msg = "该微信用户还没有分配渠道，请先扫码成为该渠道的下级";
            //    return JsonConvert.SerializeObject(resp);

            //}
            if (bllUser.GetCount<UserInfo>(string.Format(" MgrUserId='{0}'", mgrUserId)) > 0)
            {
                resp.Msg = "该微信用户已经在管理其它渠道,同一个微信用户不能设置为多个渠道的管理员";
                return JsonConvert.SerializeObject(resp);
            }
            //bool isCan = false;
            //UserInfo channelUserInfo = bllUser.GetUserInfo(mgrUserInfo.Channel);
            //if (channelUserInfo.UserID == mgrUserInfo.Channel)
            //{
            //    isCan = true;
            //}
            //else
            //{
            //    do
            //    {
            //        if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
            //        {
            //            break;
            //        }
            //        channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel);
            //        if (channelUserInfo.UserID == mgrUserInfo.Channel)
            //        {
            //            isCan = true;
            //            break;
            //        }

            //    } while (channelUserInfo != null);
            //}

            //if (!isCan)
            //{
            //    resp.Msg = "该微信用户不属于该渠道下,因此不能分配此管理员";
            //    return JsonConvert.SerializeObject(resp);
            //}


            if (bllUser.Update(new UserInfo(), string.Format(" MgrUserId='{0}'", mgrUserId), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, userId)) == 1)
            {
                resp.IsSuccess = true;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }

        /// <summary>
        ///设置微信管理员且添加二维码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetWeixinMgrAndSetFirstDistributionLevel(HttpContext context)
        {

            string channelUserId = context.Request["userId"];
            string mgrUserId = context.Request["mgrUserId"];
            UserInfo mgrUserInfo = bllUser.GetUserInfo(mgrUserId);
            if (bllUser.GetCount<UserInfo>(string.Format(" MgrUserId='{0}'", mgrUserId)) > 0)
            {
                resp.Msg = "该微信用户已经在管理其它渠道,同一个微信用户不能设置为多个渠道的管理员";
                return JsonConvert.SerializeObject(resp);
            }
            //bool isCan = false;
            //UserInfo channelUserInfo = bllUser.GetUserInfo(mgrUserInfo.Channel);
            //if (channelUserInfo.UserID == mgrUserInfo.Channel)
            //{
            //    isCan = true;
            //}
            //else
            //{
            //    do
            //    {
            //        if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
            //        {
            //            break;
            //        }
            //        channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel);
            //        if (channelUserInfo.UserID == mgrUserInfo.Channel)
            //        {
            //            isCan = true;
            //            break;
            //        }

            //    } while (channelUserInfo != null);
            //}

            //if (!isCan)
            //{
            //    resp.Msg = "该微信用户不属于该渠道下,因此不能分配此管理员";
            //    return JsonConvert.SerializeObject(resp);
            //}


            if (bllUser.Update(new UserInfo(), string.Format(" MgrUserId='{0}'", mgrUserId), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, channelUserId)) == 1)
            {
                resp.IsSuccess = true;
                #region 设置管理员为渠道二维码
                var userInfo = bllUser.GetUserInfo(mgrUserId, bllUser.WebsiteOwner);
                string sqlSet = string.Format(" IsFirstLevelDistribution='1', ParentChannel='{0}',Channel='{0}'", channelUserId);
                if (string.IsNullOrEmpty(userInfo.DistributionOwner))
                {
                    sqlSet += string.Format(",DistributionOwner='{0}'", bllUser.WebsiteOwner);
                }
                if (bllUser.Update(new UserInfo(), sqlSet, string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, mgrUserId)) == 1)
                {
                    resp.IsSuccess = true;
                    var downUserList = bllDis.GetDownUserList(userInfo.UserID).Where(p => p.IsFirstLevelDistribution != "1");
                    foreach (var user in downUserList)
                    {
                        bllUser.Update(user, string.Format("Channel='{0}'", channelUserId), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, user.UserID));

                    }

                }
                else
                {
                    resp.Msg = "操作失败";
                }
                #endregion
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }

        /// <summary>
        ///设置渠道下的第一层分销员 从已有用户添加二维码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetFirstLevelDistribution(HttpContext context)
        {

            string channelUserId = context.Request["ParentChannel"];
            string userId = context.Request["UserId"];
            var userInfo = bllUser.GetUserInfo(userId, bllUser.WebsiteOwner);
            if (!bllUser.IsDistributionMember(userInfo))
            {
                resp.Msg = "该用户不是分销员,不能添加为二维码";
                return JsonConvert.SerializeObject(resp);
            }
            string sqlSet = string.Format(" IsFirstLevelDistribution='1', ParentChannel='{0}',Channel='{0}'", channelUserId);
            if (string.IsNullOrEmpty(userInfo.DistributionOwner))
            {
                sqlSet += string.Format(",DistributionOwner='{0}'", bllUser.WebsiteOwner);
            }
            if (bllUser.Update(new UserInfo(), sqlSet, string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, userId)) == 1)
            {
                resp.IsSuccess = true;
                var downUserList = bllDis.GetDownUserList(userInfo.UserID).Where(p => p.IsFirstLevelDistribution != "1");
                foreach (var user in downUserList)
                {
                    bllUser.Update(user, string.Format("Channel='{0}'", channelUserId), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, user.UserID));

                }

            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }
        /// <summary>
        /// 删除第一层二维码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteFirstLevelDistribution(HttpContext context)
        {

            string userId = context.Request["UserId"];
            //UserInfo userInfo = bllUser.GetUserInfo(userId);
            if (bllUser.Update(new UserInfo(), string.Format(" IsFirstLevelDistribution='0', ParentChannel=''"), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, userId)) == 1)
            {
                resp.IsSuccess = true;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }

        /// <summary>
        /// 添加渠道下的第一层分销员 二维码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddFirstLevelDistribution(HttpContext context)
        {

            string parentChannel = context.Request["ParentChannel"];
            string parentChannels = context.Request["ParentChannels"];
            string trueName = context.Request["TrueName"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];

            UserInfo userInfo = new UserInfo();
            userInfo.UserID = string.Format("{0}_FirstLevelDistribution_{1}", bllUser.WebsiteOwner, bllUser.GetGUID(TransacType.CommAdd));
            userInfo.Password = "";
            userInfo.TrueName = trueName;
            userInfo.Company = company;
            userInfo.Postion = position;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.WebsiteOwner = bllUser.WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.Birthday = DateTime.Now;
            userInfo.ParentChannel = parentChannel;
            userInfo.Channel = parentChannel;
            userInfo.IsFirstLevelDistribution = "1";
            userInfo.DistributionOwner = userInfo.WebsiteOwner;
            if (!string.IsNullOrEmpty(parentChannels))
            {
                userInfo.ParentChannel = parentChannels;
                userInfo.Channel = parentChannels;
            }
            if (string.IsNullOrEmpty(userInfo.TrueName))
            {
                resp.Msg = "名称必填";
                return JsonConvert.SerializeObject(resp);
            }
            if (string.IsNullOrEmpty(userInfo.ParentChannel))
            {
                resp.Msg = "渠道必选";
                return JsonConvert.SerializeObject(resp);
            }
            if (bllUser.Add(userInfo))
            {
                resp.IsSuccess = true;

            }
            else
            {
                resp.Msg = "添加失败";
            }
            return JsonConvert.SerializeObject(resp);

        }

        /// <summary>
        /// 编辑渠道下的第一层分销员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditFirstLevelDistribution(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string parentChannel = context.Request["ParentChannel"];
            string parentChannels = context.Request["ParentChannels"];
            string trueName = context.Request["TrueName"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
            userInfo.TrueName = trueName;
            userInfo.Company = company;
            userInfo.Postion = position;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.ParentChannel = parentChannel;
            userInfo.Channel = parentChannel;
            if (string.IsNullOrEmpty(userInfo.TrueName))
            {
                resp.Msg = "名称必填";
                return JsonConvert.SerializeObject(resp);
            }
            if (!string.IsNullOrEmpty(parentChannels))
            {
                userInfo.ParentChannel = parentChannels;
                userInfo.Channel = parentChannels;
            }
            if (bllUser.Update(userInfo))
            {

                var downUserList = bllDis.GetDownUserList(userInfo.UserID).Where(p => p.IsFirstLevelDistribution != "1");
                foreach (var user in downUserList)
                {
                    bllUser.Update(user, string.Format("Channel='{0}'", userInfo.Channel), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, user.UserID));

                }
                resp.IsSuccess = true;

            }
            else
            {
                resp.Msg = "添加失败";
            }
            return JsonConvert.SerializeObject(resp);

        }


        /// <summary>
        /// 修改分销上级
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDistributionOnLinePreUser(HttpContext context)
        {

            string autoIds = context.Request["autoIds"];
            string preUserId = context.Request["preUserId"];

            string errorMsg = string.Empty;

            resp.Status = bllDis.UpdatePreUserId(autoIds, preUserId, out errorMsg) == true ? 1 : 0;

            resp.Msg = errorMsg;

            return Common.JSONHelper.ObjectToJson(resp);

        }
        #endregion

        #region 供应商渠道
        /// <summary>
        /// 供应商渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QuerySupplierChannel(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            string keyWord = context.Request["KeyWord"];
            string sort = context.Request["sort"];//排序字段
            string order = context.Request["order"]; //asc 

            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner = '{0}' And UserType=8 ", bllUser.WebsiteOwner));

            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And  ChannelName like '%{0}%' ", keyWord);
            }
            string orderBy = "AutoID ASC";
            int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            var sourceUserList = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

            var userList = from p in sourceUserList
                           select new
                           {
                               p.AutoID,
                               p.UserID,
                               p.ChannelName,
                               p.Description,
                               p.WXNickname,
                               p.WXHeadimgurlLocal,
                               p.TrueName,
                               p.Phone,
                               p.Company,
                               p.Postion,
                               p.Email,
                               p.TagName,
                               p.TotalScore,
                               p.DistributionOwner,
                               p.UserType,
                               p.AccessLevel,
                               p.AvailableVoteCount,
                               p.WXOpenId,
                               p.AccountAmount,
                               p.TotalAmount,
                               CanUseAmount = bllDis.GetUserCanUseAmount(p),
                               //p.HistoryDistributionOnLineTotalAmount,
                               //p.DistributionOnLineRecomendUserInfo,
                               FirstLevelDistributionCount = bllUser.GetCount<UserInfo>(string.Format("WebsiteOwner='{0}' And ParentChannel='{1}' And IsFirstLevelDistribution='1'", bllUser.WebsiteOwner, p.UserID)),//供应商数量
                               // p.DistributionDownUserCountLevel1,//直接会员数量
                               // p.DistributionDownUserCountAll,//所有会员数量
                               //p.DistributionSaleAmountLevel1,//直接销售
                               p.DistributionSaleAmountAll,//所有销售
                               //DistributionDownUserCountLevel1 = bllDis.GetChannelAllFirstLevelChildUser(p.UserID).Count,//直接会员数量
                               // DistributionDownUserCountAll = bllDis.GetChannelAllChildUser(p.UserID).Count(),//所有会员数量
                               //p.DistributionDownUserCountLevel2,
                               //p.DistributionDownUserCountLevel3,
                               //DistributionSaleAmountLevel1 = bllDis.GetChannelAllFirstLevelOrder(p.UserID).Sum(s => s.TotalAmount),//直接销售额
                               //DistributionSaleAmountAll = bllDis.GetChannelAllOrder(p.UserID).Sum(s => s.TotalAmount),//累计销售
                               //p.DistributionSaleAmountLevel2,
                               //p.DistributionSaleAmountLevel3,
                               //p.HexiaoCode
                               //DistributionSaleAmountLevel0 = GetDistributionSaleAmountLevel0(p),
                               //SalesQuota = Math.Round(bllDis.GetUserSalesQuota(p), 2),//累计销售
                               p.HistoryDistributionOnLineTotalAmountEstimate,//累计奖励（预估）
                               //OverCanUseAmount = Math.Round(bllDis.GetUserWithdrawTotalAmount(p), 2), //已提现奖励
                               OverCanUseAmount = 0, //已提现奖励
                               //ParentChannelUserInfo=bllUser.GetUserInfo(p.ParentChannel),
                               p.ChannelLevelId,
                               //MgrUserInfo = bllUser.GetUserInfo(p.MgrUserId),
                               // p.ParentChannel,
                               LevelName = bllBase.Get<UserLevelConfig>(string.Format("AutoId={0}", p.ChannelLevelId)) != null ? bllBase.Get<UserLevelConfig>(string.Format("AutoId={0}", p.ChannelLevelId)).LevelString : ""
                           };


            //#region 排序
            //if (!string.IsNullOrEmpty(sort))
            //{
            //    switch (sort)
            //    {
            //        //case "DistributionDownUserCountAll"://所有会员排序
            //        //    if (order == "asc")
            //        //    {
            //        //        userList = userList.OrderBy(p => p.DistributionDownUserCountAll).ToList();

            //        //    }
            //        //    if (order == "desc")
            //        //    {
            //        //        userList = userList.OrderByDescending(p => p.DistributionDownUserCountAll).ToList();
            //        //    }
            //        //    break;
            //        //case "FirstLevelDistributionCount"://二维码数量排序
            //        //    if (order == "asc")
            //        //    {
            //        //        userList = userList.OrderBy(p => p.FirstLevelDistributionCount).ToList();

            //        //    }
            //        //    if (order == "desc")
            //        //    {
            //        //        userList = userList.OrderByDescending(p => p.FirstLevelDistributionCount).ToList();
            //        //    }
            //        //    break;
            //        //case "DistributionDownUserCountLevel1"://渠道下二维码直接会员数
            //        //    if (order == "asc")
            //        //    {
            //        //        userList = userList.OrderBy(p => p.DistributionDownUserCountLevel1).ToList();

            //        //    }
            //        //    if (order == "desc")
            //        //    {
            //        //        userList = userList.OrderByDescending(p => p.DistributionDownUserCountLevel1).ToList();
            //        //    }
            //        //    break;
            //        //case "DistributionSaleAmountLevel1"://渠道直接销售
            //        //    if (order == "asc")
            //        //    {
            //        //        userList = userList.OrderBy(p => p.DistributionSaleAmountLevel1).ToList();

            //        //    }
            //        //    if (order == "desc")
            //        //    {
            //        //        userList = userList.OrderByDescending(p => p.DistributionSaleAmountLevel1).ToList();
            //        //    }
            //        //    break;
            //        case "DistributionSaleAmountAll"://渠道累计销售
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionSaleAmountAll).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionSaleAmountAll).ToList();
            //            }
            //            break;
            //        default:
            //            break;
            //    }

            //}
            //#endregion
            var result = new
            {
                total = totalCount,
                rows = userList
            };
            return JsonConvert.SerializeObject(result);






        }


        /// <summary>
        /// 渠道下的供应商
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryChildChannelSupplier(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["page"]);
            int pageSize = int.Parse(context.Request["rows"]);
            string keyWord = context.Request["KeyWord"];
            string parentChannel = context.Request["parentChannel"];
            string parentChannels = context.Request["parentChannels"];
            string sort = context.Request["sort"];
            string order = context.Request["order"];
            StringBuilder sbWhere = new StringBuilder();

            sbWhere.AppendFormat("WebsiteOwner='{0}'  And IsFirstLevelDistribution='1'", bllUser.WebsiteOwner);

            if (!string.IsNullOrEmpty(parentChannel))
            {
                sbWhere.AppendFormat(" And  ParentChannel='{0}'", parentChannel);
            }
            if (!string.IsNullOrEmpty(parentChannels))
            {
                sbWhere.AppendFormat(" And  ParentChannel='{0}'", parentChannels);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (Company like '%{0}%' Or Cast(AutoID AS Nvarchar(20))='{0}')", keyWord);
            }

            string orderBy = "AutoId DESC";

            if (!string.IsNullOrEmpty(sort))
            {
                orderBy = sort + " " + order;
            }
            int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            //var sourceUserList = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            var sourceUserList = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString() + " order by" + orderBy);


            var userList = from p in sourceUserList
                           select new
                           {
                               p.AutoID,
                               p.UserID,
                               //p.WXNickname,
                               //p.WXHeadimgurlLocal,
                               //TrueName = bllUser.GetUserDispalyName(p),
                               //p.Phone,
                               p.Company,
                               //p.Postion,
                               //p.Email,
                               //p.TagName,
                               //p.TotalScore,
                               //p.DistributionOwner,
                               //p.UserType,
                               //p.AccessLevel,
                               //p.AvailableVoteCount,
                               //p.WXOpenId,
                               //p.AccountAmount,
                               //p.TotalAmount,
                               p.ParentChannel,
                               CanUseAmount = Math.Round(bllDis.GetUserCanUseAmount(p)),
                               //p.HistoryDistributionOnLineTotalAmount,
                               //p.DistributionOnLineRecomendUserInfo,
                               ParentChannelName = bllUser.GetUserInfo(p.ParentChannel).ChannelName,
                               //p.DistributionDownUserCountLevel1,//一级会员数
                               //p.DistributionDownUserCountAll,//所有会员数
                               ////DistributionDownUserCountAll = bllDis.GetAllDownUsersList(p.UserID).Where(a => a.UserID != p.UserID).Count(),//所有会员数
                               //p.DistributionSaleAmountLevel1,//一级销售额
                               //p.DistributionSaleAmountAll//所有销售额
                               //DistributionSaleAmountAll = bllDis.GetDisSaleAmount(p.UserID)//所有销售额
                               //p.DistributionDownUserCountLevel2,
                               //p.DistributionDownUserCountLevel3,
                               //DistributionSaleAmountLevel1=bllDis.GetAllDownUsesrList(p.UserID).Where(u=>p.UserID!=u.UserID).ToList().Count,
                               //p.DistributionSaleAmountLevel2,
                               //p.DistributionSaleAmountLevel3,
                               //p.HexiaoCode
                               //DistributionSaleAmountLevel0 = GetDistributionSaleAmountLevel0(p),
                               // SalesQuota = Math.Round(bllDis.GetUserSalesQuota(p), 2),//累计销售
                               // HistoryDistributionOnLineTotalAmountEstimate = p.HistoryDistributionOnLineTotalAmountEstimate,//累计奖励（预估）
                               // OverCanUseAmount = Math.Round(bllDis.GetUserWithdrawTootalAmount(p), 2) //已提现奖励
                           };

            //#region 排序
            //if (!string.IsNullOrEmpty(sort))
            //{
            //    switch (sort)
            //    {
            //        case "DistributionDownUserCountLevel1"://一级会员排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionDownUserCountLevel1).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionDownUserCountLevel1).ToList();
            //            }
            //            break;
            //        case "DistributionDownUserCountAll"://所有会员数排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionDownUserCountAll).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionDownUserCountAll).ToList();
            //            }
            //            break;
            //        case "DistributionSaleAmountLevel1"://一级销售额排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionSaleAmountLevel1).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionSaleAmountLevel1).ToList();
            //            }
            //            break;
            //        case "DistributionSaleAmountAll"://所有销售额排序
            //            if (order == "asc")
            //            {
            //                userList = userList.OrderBy(p => p.DistributionSaleAmountAll).ToList();

            //            }
            //            if (order == "desc")
            //            {
            //                userList = userList.OrderByDescending(p => p.DistributionSaleAmountAll).ToList();
            //            }
            //            break;
            //        default:
            //            break;
            //    }

            //} 
            //#endregion

            var result = new
            {
                total = totalCount,
                rows = userList
            };
            return JsonConvert.SerializeObject(result);


        }



        /// <summary>
        /// 添加供应商渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddSupplierChannel(HttpContext context)
        {

            string channelName = context.Request["ChannelName"];
            string description = context.Request["Description"];
            string parentChannel = context.Request["ParentChannel"];
            string trueName = context.Request["TrueName"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            string channelLevelId = context.Request["ChannelLevelId"];

            UserInfo userInfo = new UserInfo();
            userInfo.ChannelName = channelName;
            userInfo.Description = description;
            userInfo.UserID = string.Format("{0}_Channel_{1}", bllUser.WebsiteOwner, bllUser.GetGUID(TransacType.CommAdd));
            userInfo.Password = "";
            userInfo.ParentChannel = parentChannel;
            userInfo.TrueName = trueName;
            userInfo.Company = company;
            userInfo.Postion = position;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.WebsiteOwner = bllUser.WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.Birthday = DateTime.Now;
            userInfo.ChannelLevelId = channelLevelId;
            userInfo.UserType = 8;
            if (string.IsNullOrEmpty(userInfo.ChannelName))
            {
                resp.Msg = "渠道名称必填";
                return JsonConvert.SerializeObject(resp);
            }


            if (userInfo.UserID == userInfo.ParentChannel)
            {
                resp.Msg = "账户名不能与上级渠道相同 ";
                return JsonConvert.SerializeObject(resp);
            }
            if (string.IsNullOrEmpty(channelLevelId))
            {
                resp.Msg = "请选择等级";
                return JsonConvert.SerializeObject(resp);
            }


            if (bllUser.Add(userInfo))
            {
                resp.IsSuccess = true;

            }
            else
            {
                resp.Msg = "添加失败";
            }

            return JsonConvert.SerializeObject(resp);


        }


        /// <summary>
        /// 编辑供应商渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditSupplierChannel(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string channelName = context.Request["ChannelName"];
            string description = context.Request["Description"];
            string parentChannel = context.Request["ParentChannel"];
            string trueName = context.Request["TrueName"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            string channelLevelId = context.Request["ChannelLevelId"];
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
            //userInfo.Password = passWord;
            userInfo.ChannelName = channelName;
            userInfo.Description = description;
            userInfo.ParentChannel = parentChannel;
            userInfo.TrueName = trueName;
            userInfo.Company = company;
            userInfo.Postion = position;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.ChannelLevelId = channelLevelId;

            if (string.IsNullOrEmpty(userInfo.ChannelName))
            {
                resp.Msg = "渠道名称必填";
                return JsonConvert.SerializeObject(resp);
            }
            if (string.IsNullOrEmpty(channelLevelId))
            {
                resp.Msg = "请选择等级";
                return JsonConvert.SerializeObject(resp);
            }
            if (userInfo.UserID == userInfo.ParentChannel)
            {
                resp.Msg = "账户名不能与上级渠道相同 ";
                return JsonConvert.SerializeObject(resp);
            }

            if (bllUser.Update(userInfo))
            {
                resp.IsSuccess = true;

            }
            else
            {
                resp.Msg = "编辑失败";
            }

            return JsonConvert.SerializeObject(resp);


        }

        /// <summary>
        ///删除供应商渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteSupplierChannel(HttpContext context)
        {

            string userId = context.Request["userId"];
            if (bllUser.Delete(new UserInfo(), string.Format("UserId='{0}'", userId)) > 0)
            {
                resp.IsSuccess = true;
            }
            return JsonConvert.SerializeObject(resp);

        }


        /// <summary>
        ///添加渠道下的供应商
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddChildChannelSupplier(HttpContext context)
        {

            string channelUserId = context.Request["ParentChannel"];
            string userId = context.Request["UserId"];
            var userInfo = bllUser.GetUserInfo(userId, bllUser.WebsiteOwner);
            string sqlSet = string.Format(" IsFirstLevelDistribution='1', ParentChannel='{0}'", channelUserId);
            if (string.IsNullOrEmpty(userInfo.DistributionOwner))
            {
                sqlSet += string.Format(",DistributionOwner='{0}'", bllUser.WebsiteOwner);
            }
            if (bllUser.Update(new UserInfo(), sqlSet, string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, userId)) == 1)
            {
                resp.IsSuccess = true;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }
        /// <summary>
        /// 删除供应商渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteChildChannelSupplier(HttpContext context)
        {

            string userId = context.Request["UserId"];
            //UserInfo userInfo = bllUser.GetUserInfo(userId);
            if (bllUser.Update(new UserInfo(), string.Format(" IsFirstLevelDistribution='0', ParentChannel=''"), string.Format(" WebsiteOwner='{0}' And UserId='{1}'", bllUser.WebsiteOwner, userId)) == 1)
            {
                resp.IsSuccess = true;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }



        #endregion




        /// <summary>
        /// 同时分销上下级人数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SynDistribution(HttpContext context)
        {

            TimingTask task = new TimingTask();
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = 1;
            task.TaskInfo = "同步分销会员下级人数";
            task.TaskType = 5;
            task.ScheduleDate = DateTime.Now;
            if (bllUser.Add(task))
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 同步分销销售额
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SynDistributionSaleAmount(HttpContext context)
        {

            TimingTask task = new TimingTask();
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = 1;
            task.TaskInfo = "同步分销销售额";
            task.TaskType = 7;
            task.ScheduleDate = DateTime.Now;
            if (bllUser.Add(task))
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 清洗会员数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CleanUser(HttpContext context)
        {

            TimingTask task = new TimingTask();
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = 1;
            task.TaskInfo = "会员清洗";
            task.TaskType = 8;
            task.ScheduleDate = DateTime.Now;
            if (bllUser.Add(task))
            {
                resp.Status = 1;
                resp.Msg = "已添加到任务,数据将稍后更新";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        //private string QueryWebsiteUserByTrueName(HttpContext context)
        //{
        //    //if ((!CurrentUserInfo.UserType.Equals(1))&&(!CurrentUserInfo.UserID.Equals(bllBase.WebsiteOwner)))
        //    //    return "无权限!";

        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);

        //    StringBuilder strWhere = new StringBuilder();
        //    strWhere.AppendFormat("WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);

        //    strWhere.AppendFormat(" AND TrueName is not null");
        //    List<ZentCloud.BLLJIMP.Model.UserInfo> userList = this.bllJuActivity.GetColList<ZentCloud.BLLJIMP.Model.UserInfo>(rows, page, strWhere.ToString(), "AutoID DESC",
        //        "AutoID,UserID,WXNickname,WXHeadimgurl,TrueName,Phone,Company,Postion,Email,TagName,TotalScore,DistributionOwner,UserType,AccessLevel,AvailableVoteCount");
        //    int tCount = this.bllJuActivity.GetCount<ZentCloud.BLLJIMP.Model.UserInfo>(strWhere.ToString());
        //    var users = from p in userList
        //                select new
        //                {
        //                    p.AutoID,
        //                    p.UserID,
        //                    p.WXNickname,
        //                    p.WXHeadimgurlLocal,
        //                    p.TrueName,
        //                    p.Phone,
        //                    p.Company,
        //                    p.Postion,
        //                    p.Email,
        //                    p.TagName,
        //                    p.TotalScore,
        //                    p.DistributionOwner,
        //                    p.UserType,
        //                    p.AccessLevel,
        //                    p.AvailableVoteCount
        //                };
        //    var result = new
        //    {
        //        total = tCount,
        //        rows = userList
        //    };


        //    return JsonConvert.SerializeObject(result);
        //    //return Common.JSONHelper.ObjectToJson(result);


        //}

        ///// <summary>
        ///// 查询站点用户户
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryWebsiteUserByTrueNameOrWX(HttpContext context)
        //{
        //    //if (!CurrentUserInfo.UserType.Equals(1))
        //    //    return "无权限!";

        //    int pageIndex = Convert.ToInt32(context.Request["page"]);
        //    int pageSize = Convert.ToInt32(context.Request["rows"]);

        //    StringBuilder sbWhere = new StringBuilder();
        //    sbWhere.AppendFormat("WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);
        //    sbWhere.AppendFormat(" AND TrueName is not null And WXNickname is not null");
        //    List<ZentCloud.BLLJIMP.Model.UserInfo> userList = this.bllUser.GetColList<ZentCloud.BLLJIMP.Model.UserInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC",
        //        "AutoID,UserID,WXNickname,WXHeadimgurl,TrueName,Phone,Company,Postion,Email,TagName,TotalScore,DistributionOwner,UserType,AccessLevel,AvailableVoteCount");
        //    int tCount = this.bllJuActivity.GetCount<ZentCloud.BLLJIMP.Model.UserInfo>(sbWhere.ToString());
        //    var users = from p in userList
        //                select new
        //                {
        //                    p.AutoID,
        //                    p.UserID,
        //                    p.WXNickname,
        //                    p.WXHeadimgurlLocal,
        //                    p.TrueName,
        //                    p.Phone,
        //                    p.Company,
        //                    p.Postion,
        //                    p.Email,
        //                    p.TagName,
        //                    p.TotalScore,
        //                    p.DistributionOwner,
        //                    p.UserType,
        //                    p.AccessLevel,
        //                    p.AvailableVoteCount
        //                };
        //    var result = new
        //    {
        //        total = tCount,
        //        rows = users
        //    };
        //    return JsonConvert.SerializeObject(result);
        //}


        // <summary>
        // 只显示分销会员
        // </summary>
        // <param name="context"></param>
        ///// <returns></returns>
        //    private string QueryWebsiteUserByDistribution(HttpContext context)
        //    {

        //        int pageIndex = Convert.ToInt32(context.Request["page"]);
        //        int pageSize = Convert.ToInt32(context.Request["rows"]);
        //        StringBuilder sbWhere = new StringBuilder();
        //        sbWhere.AppendFormat("WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);
        //        sbWhere.AppendFormat(" AND DistributionOwner is not null And DistributionOwner !=''");
        //        List<ZentCloud.BLLJIMP.Model.UserInfo> userList = this.bllUser.GetLit<ZentCloud.BLLJIMP.Model.UserInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");

        //        return Common.JSONHelper.ObjectToJson(
        //new
        //{
        //    total = this.bllUser.GetCount<ZentCloud.BLLJIMP.Model.UserInfo>(sbWhere.ToString()),
        //    rows = userList
        //});


        //    }

        // <summary>
        // 设置用户权限访问级别
        // </summary>
        // <param name="context"></param>
        ///// <returns></returns>
        private string SetUserAccessLevel(HttpContext context)
        {
            int accessLevel = int.Parse(context.Request["AccessLevel"]);
            string userAutoIds = context.Request["UserAutoIds"];
            if (bllUser.Update(new UserInfo(), string.Format("AccessLevel={0}", accessLevel), string.Format(" AutoId in ({0})", userAutoIds)) == userAutoIds.Split(',').Length)
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "设置会员等级[id=" + userAutoIds + ",等级=" + accessLevel + "]");
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "设置会员等级失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }


        // <summary>
        // 发送模板消息
        // </summary>
        // <param name="context"></param>
        ///// <returns></returns>
        private string SendTemplateMsg(HttpContext context)
        {
            #region 已经注释
            //string templateType = context.Request["TemplateType"];
            //string title = context.Request["Title"];
            //string content = context.Request["Content"];
            //string url = context.Request["Url"];
            //string userAutoIds = context.Request["UserAutoIds"];
            //int successCount = 0;
            ////string serialNum = DateTime.Now.ToString("yyyyMMddhhmmss") + Common.Rand.Str(6);
            //switch (templateType)
            //{
            //    case "notify":

            //        //记录发送批次
            //        SMSPlanInfo plan = new SMSPlanInfo();
            //        plan.ChargeCount = 0;
            //        plan.PlanID = bllBase.GetGUID(TransacType.CommAdd);
            //        plan.SubmitDate = DateTime.Now;
            //        plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WXTemplateMsg_Notify;
            //        plan.SenderID = bllBase.GetCurrUserID();
            //        plan.SendContent = content;
            //        plan.SendFrom = "发送模板通知消息";
            //        plan.SubmitCount = 0;
            //        plan.SubmitDate = DateTime.Now;
            //        plan.Title = title;
            //        plan.Url = url;
            //        plan.UsePipe = "none";
            //        plan.ProcStatus = 1;
            //        plan.WebsiteOwner = bllBase.WebsiteOwner;

            //        foreach (string autoId in userAutoIds.Split(','))
            //        {

            //            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
            //            if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.WXOpenId)))
            //            {
            //                plan.SubmitCount += 1;
            //                if (bllWeixin.SendTemplateMessageNotifyComm(userInfo.WXOpenId, title, content, url, CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify), plan.PlanID, userInfo.UserID))
            //                {
            //                    successCount++;
            //                }

            //            }
            //        }

            //        plan.SuccessCount = successCount;
            //        bllBase.Add(plan);

            //        resp.Status = 1;
            //        resp.Msg = string.Format("成功向{0}人发送了消息", successCount);
            //        break;
            //    default:
            //        resp.Msg = "暂时不支持该通知类型";
            //        break;
            //}
            //return Common.JSONHelper.ObjectToJson(resp);
            #endregion

            string title = context.Request["Title"];
            string content = Common.StringHelper.GetReplaceStr(context.Request["Content"]);
            string url = context.Request["Url"];
            string userAutoIds = context.Request["UserAutoIds"];
            string sendTo = context.Request["SendTo"];
            TimingTask task = new TimingTask();
            task.TaskId = bllBase.GetGUID(TransacType.CommAdd);
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.ScheduleDate = DateTime.Now;
            task.Receivers = userAutoIds;
            task.Status = 1;
            task.TaskType = 4;
            task.TaskInfo = string.Format("群发模板消息\t标题:{0}内容:{1}链接:{2}", title, content, url);
            task.Title = title;
            task.MsgContent = content;
            task.Url = url;
            ZentCloud.ZCBLLEngine.BLLTransaction trans = new ZCBLLEngine.BLLTransaction();
            if (!bllUser.Add(task, trans))
            {
                trans.Rollback();
                resp.Status = -1;
                resp.Msg = "操作出错.";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            SMSPlanInfo plan = new SMSPlanInfo();
            plan.ChargeCount = 0;
            plan.PlanID = task.TaskId;
            plan.SubmitDate = DateTime.Now;
            if (sendTo == "1")
            {
                plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.AppMsg;
            }
            else if (sendTo == "2")
            {
                plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.AppAndWx;
            }
            else
            {
                plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WXTemplateMsg_Notify;
            }
            plan.SendContent = task.MsgContent;
            plan.SendFrom = "发送模板通知消息";
            plan.SubmitDate = DateTime.Now;
            plan.Title = task.Title;
            plan.Url = task.Url;
            plan.UsePipe = "none";
            plan.ProcStatus = 1;
            plan.WebsiteOwner = task.WebsiteOwner;

            string[] ids = userAutoIds.Split(',');
            int totalCount = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrEmpty(ids[i])) continue;
                UserInfo userModel = bllUser.GetUserInfoByAutoID(int.Parse(ids[i]));
                if (userModel == null || string.IsNullOrEmpty(userModel.WXOpenId)) continue;
                WXBroadcastHistory historyModel = new WXBroadcastHistory();
                historyModel.OpenId = userModel.WXOpenId;
                historyModel.UserId = userModel.UserID;
                historyModel.Title = title;
                historyModel.Msg = content;
                historyModel.Url = url;
                historyModel.SerialNum = plan.PlanID;
                historyModel.InsertDate = DateTime.Now;
                historyModel.Status = -1;
                historyModel.StatusMsg = "";
                historyModel.WebsiteOwner = bllUser.WebsiteOwner;
                if (sendTo == "1")
                {
                    historyModel.BroadcastType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.AppMsg);
                }
                else if (sendTo == "2")
                {
                    historyModel.BroadcastType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.AppAndWx);
                }
                else
                {
                    historyModel.BroadcastType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify);
                }
                if (!bllUser.Add(historyModel, trans))
                {
                    trans.Rollback();
                    resp.Msg = "操作出错";
                    resp.Status = -1;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                totalCount++;
            }
            plan.SubmitCount = totalCount;
            if (!bllUser.Add(plan, trans))
            {
                trans.Rollback();
                resp.Msg = "操作出错.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            trans.Commit();
            resp.Status = 1;
            resp.Msg = string.Format("已添加到任务");
            return Common.JSONHelper.ObjectToJson(resp);

        }

        // <summary>
        // 发送模板消息
        // </summary>
        // <param name="context"></param>
        ///// <returns></returns>
        private string SendTemplateMsgByTag(HttpContext context)
        {
            #region 已经注释
            //string templateType = context.Request["TemplateType"];
            //string title = context.Request["Title"];
            //string content = context.Request["Content"];
            //string url = context.Request["Url"];
            //string tags = context.Request["Tags"];
            //int successCount = 0;
            ////string serialNum = DateTime.Now.ToString("yyyyMMddhhmmss") + Common.Rand.Str(6);
            //switch (templateType)
            //{
            //    case "notify":

            //        //记录发送批次
            //        SMSPlanInfo plan = new SMSPlanInfo();
            //        plan.ChargeCount = 0;
            //        plan.PlanID = bllBase.GetGUID(TransacType.CommAdd);
            //        plan.SubmitDate = DateTime.Now;
            //        plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WXTemplateMsg_Notify;
            //        plan.SenderID = bllBase.GetCurrUserID();
            //        plan.SendContent = content;
            //        plan.SendFrom = "发送模板通知消息";
            //        plan.SubmitCount = 0;
            //        plan.SubmitDate = DateTime.Now;
            //        plan.Title = title;
            //        plan.Url = url;
            //        plan.UsePipe = "none";
            //        plan.ProcStatus = 1;
            //        plan.WebsiteOwner = bllBase.WebsiteOwner;

            //        foreach (string tag in tags.Split(','))
            //        {

            //            List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format(" Websiteowner='{0}' And TagName like '%{1}%'", bllUser.WebsiteOwner, tag));
            //            plan.SubmitCount += userList.Count;
            //            foreach (var userInfo in userList)
            //            {
            //                if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.WXOpenId)))
            //                {
            //                    if (bllWeixin.SendTemplateMessageNotifyComm(userInfo.WXOpenId, title, content, url, CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify), plan.PlanID, userInfo.UserID))
            //                    {
            //                        successCount++;
            //                    }

            //                }
            //            }

            //        }
            //        plan.SuccessCount = successCount;
            //        bllBase.Add(plan);

            //        resp.Status = 1;
            //        resp.Msg = string.Format("成功向{0}人发送了消息", successCount);
            //        break;
            //    default:
            //        resp.Msg = "暂时不支持该通知类型";
            //        break;
            //}
            //return Common.JSONHelper.ObjectToJson(resp);
            #endregion

            string title = context.Request["Title"];
            string content = Common.StringHelper.GetReplaceStr(context.Request["Content"]);
            string url = context.Request["Url"];
            string tags = context.Request["Tags"];
            TimingTask task = new TimingTask();
            task.TaskId = bllBase.GetGUID(TransacType.CommAdd);
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.ScheduleDate = DateTime.Now;
            task.Tags = tags;
            task.Status = 1;
            task.TaskType = 4;
            task.TaskInfo = string.Format("群发模板消息\t标题:{0}内容:{1}链接:{2}", title, content, url);
            task.Title = title;
            task.MsgContent = content;
            task.Url = url;
            ZentCloud.ZCBLLEngine.BLLTransaction trans = new ZCBLLEngine.BLLTransaction();
            if (!bllUser.Add(task, trans))
            {
                trans.Rollback();
                resp.Status = -1;
                resp.Msg = "操作出错.";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            SMSPlanInfo plan = new SMSPlanInfo();
            plan.ChargeCount = 0;
            plan.PlanID = task.TaskId;
            plan.SubmitDate = DateTime.Now;
            plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WXTemplateMsg_Notify;
            plan.SendContent = task.MsgContent;
            plan.SendFrom = "发送模板通知消息";
            plan.SubmitDate = DateTime.Now;
            plan.Title = task.Title;
            plan.Url = task.Url;
            plan.UsePipe = "none";
            plan.ProcStatus = 1;
            plan.WebsiteOwner = task.WebsiteOwner;

            int totalCount = 0;
            foreach (string tag in task.Tags.Split(','))
            {
                List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format(" Websiteowner='{0}' And TagName like '%{1}%'", task.WebsiteOwner, tag));

                foreach (var userInfo in userList)
                {
                    if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.WXOpenId)))
                    {
                        WXBroadcastHistory historyModel = new WXBroadcastHistory();
                        historyModel.OpenId = userInfo.WXOpenId;
                        historyModel.UserId = userInfo.UserID;
                        historyModel.Title = title;
                        historyModel.Msg = content;
                        historyModel.Url = url;
                        historyModel.SerialNum = plan.PlanID;
                        historyModel.InsertDate = DateTime.Now;
                        historyModel.Status = -1;
                        historyModel.StatusMsg = "";
                        historyModel.WebsiteOwner = bllUser.WebsiteOwner;
                        historyModel.BroadcastType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify);
                        if (!bllUser.Add(historyModel, trans))
                        {
                            trans.Rollback();
                            resp.Msg = "操作出错";
                            resp.Status = -1;
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                        totalCount++;
                    }
                }

            }
            plan.SubmitCount = totalCount;
            if (!bllUser.Add(plan, trans))
            {
                trans.Rollback();
                resp.Msg = "操作出错.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            trans.Commit();
            resp.Status = 1;
            resp.Msg = string.Format("已添加到任务");
            return Common.JSONHelper.ObjectToJson(resp);
        }


        // <summary>
        // 发送模板消息
        // </summary>
        // <param name="context"></param>
        ///// <returns></returns>
        private string SendTemplateMsgByFans(HttpContext context)
        {


            string title = context.Request["Title"];
            string content = Common.StringHelper.GetReplaceStr(context.Request["Content"]);
            string url = context.Request["Url"];
            string autoIds = context.Request["AutoIds"];
            string userAutoIds = "";
            foreach (var item in autoIds.Split(','))
            {
                var flower = bllUser.Get<WeixinFollowersInfo>(string.Format("AutoId={0}", item));
                UserInfo user = bllUser.GetUserInfoByOpenId(flower.OpenId);
                if (user != null)
                {
                    userAutoIds += user.AutoID.ToString() + ",";
                }
            }
            userAutoIds = userAutoIds.TrimEnd(',');

            TimingTask task = new TimingTask();
            task.TaskId = bllBase.GetGUID(TransacType.CommAdd);
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.ScheduleDate = DateTime.Now;
            task.Receivers = userAutoIds;
            task.Status = 1;
            task.TaskType = 4;
            task.TaskInfo = string.Format("群发模板消息\t标题:{0}内容:{1}链接:{2}", title, content, url);
            task.Title = title;
            task.MsgContent = content;
            task.Url = url;
            ZentCloud.ZCBLLEngine.BLLTransaction trans = new ZCBLLEngine.BLLTransaction();
            if (!bllUser.Add(task, trans))
            {
                trans.Rollback();
                resp.Status = -1;
                resp.Msg = "操作出错.";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            SMSPlanInfo plan = new SMSPlanInfo();
            plan.ChargeCount = 0;
            plan.PlanID = task.TaskId;
            plan.SubmitDate = DateTime.Now;
            plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WXTemplateMsg_Notify;
            plan.SendContent = task.MsgContent;
            plan.SendFrom = "发送模板通知消息";
            plan.SubmitDate = DateTime.Now;
            plan.Title = task.Title;
            plan.Url = task.Url;
            plan.UsePipe = "none";
            plan.ProcStatus = 1;
            plan.WebsiteOwner = task.WebsiteOwner;

            string[] ids = userAutoIds.Split(',');
            int totalCount = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrEmpty(ids[i])) continue;
                UserInfo userModel = bllUser.GetUserInfoByAutoID(int.Parse(ids[i]));
                if (userModel == null || string.IsNullOrEmpty(userModel.WXOpenId)) continue;
                WXBroadcastHistory historyModel = new WXBroadcastHistory();
                historyModel.OpenId = userModel.WXOpenId;
                historyModel.UserId = userModel.UserID;
                historyModel.Title = title;
                historyModel.Msg = content;
                historyModel.Url = url;
                historyModel.SerialNum = plan.PlanID;
                historyModel.InsertDate = DateTime.Now;
                historyModel.Status = -1;
                historyModel.StatusMsg = "";
                historyModel.WebsiteOwner = bllUser.WebsiteOwner;
                historyModel.BroadcastType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify);
                if (!bllUser.Add(historyModel, trans))
                {
                    trans.Rollback();
                    resp.Msg = "操作出错";
                    resp.Status = -1;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                totalCount++;
            }
            plan.SubmitCount = totalCount;
            if (!bllUser.Add(plan, trans))
            {
                trans.Rollback();
                resp.Msg = "操作出错.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            trans.Commit();
            resp.Status = 1;
            resp.Msg = string.Format("已添加到任务");
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 群发模板消息(全部已关注的粉丝)
        /// </summary>
        /// <returns></returns>
        private string SendTemplateMsgByAllFans(HttpContext context)
        {
            string title = context.Request["Title"];
            string content = Common.StringHelper.GetReplaceStr(context.Request["Content"]);
            string url = context.Request["Url"];
            TimingTask task = new TimingTask();
            task.TaskId = bllBase.GetGUID(TransacType.CommAdd);
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.ScheduleDate = DateTime.Now;
            task.Status = 1;
            task.TaskType = 10;
            task.TaskInfo = string.Format("群发模板消息\t标题:{0}内容:{1}链接:{2}", title, content, url);
            task.Title = title;
            task.MsgContent = content;
            task.Url = url;
            if (bllUser.Add(task))
            {
                resp.Status = 1;
                resp.Msg = string.Format("已添加到任务");
            }
            else
            {
                resp.Msg = "操作出错.";
                resp.Status = -1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 同步会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SynMemberInfo(HttpContext context)
        {
            int successCount = 0;
            var orderList = bllUser.GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And Consignee!='' And Phone!=''", bllUser.WebsiteOwner));
            var signDataList = bllUser.GetList<ActivityDataInfo>(string.Format(" WebsiteOwner='{0}' And WeixinOpenID!=''", bllUser.WebsiteOwner));
            foreach (var order in orderList)
            {
                var orderUserInfo = bllUser.GetUserInfo(order.OrderUserID);
                if (orderUserInfo == null)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(orderUserInfo.TrueName) || string.IsNullOrEmpty(orderUserInfo.Phone))
                {
                    if (string.IsNullOrEmpty(orderUserInfo.TrueName))
                    {
                        orderUserInfo.TrueName = order.Consignee;
                    }
                    if (string.IsNullOrEmpty(orderUserInfo.Phone))
                    {
                        orderUserInfo.Phone = order.Phone;
                    }

                    if (bllUser.Update(orderUserInfo, string.Format(" TrueName='{0}',Phone='{1}'", orderUserInfo.TrueName, orderUserInfo.Phone), string.Format(" AutoId={0}", orderUserInfo.AutoID)) > 0)
                    {
                        successCount++;


                    }


                }


            }
            foreach (var data in signDataList)
            {
                var userInfo = bllUser.GetUserInfoByOpenId(data.WeixinOpenID);
                if (userInfo == null)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(userInfo.TrueName) || string.IsNullOrEmpty(userInfo.Phone))
                {
                    if (string.IsNullOrEmpty(userInfo.TrueName))
                    {
                        userInfo.TrueName = data.Name;
                    }
                    if (string.IsNullOrEmpty(userInfo.Phone))
                    {
                        userInfo.Phone = data.Phone;
                    }

                    if (bllUser.Update(userInfo, string.Format(" TrueName='{0}',Phone='{1}'", userInfo.TrueName, userInfo.Phone), string.Format(" AutoId={0}", userInfo.AutoID)) > 0)
                    {
                        successCount++;


                    }


                }


            }
            resp.Status = 1;
            resp.Msg = string.Format("同步完成,本次同步共更新了{0}位会员信息", successCount);
            return Common.JSONHelper.ObjectToJson(resp);

        }

        ///// <summary>
        ///// 移动设备访问文章活动课程列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryArticleListForWap(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    List<JuActivityInfo> dataList = new List<JuActivityInfo>();

        //    string RecommendCate = context.Request["RecommendCate"];
        //    string ArticleTypeEx1 = context.Request["ArticleTypeEx1"];

        //    int totalCount = 0;

        //    dataList = juActivityBll.QueryJuActivityData("search", out totalCount, null, null, RecommendCate, null, null, page, rows, null, null, "", bllBase.WebsiteOwner, ArticleTypeEx1);
        //    int totalpage = this.juActivityBll.GetTotalPage(totalCount, rows);

        //    return ConverHtmlFormateArticleListForWap(dataList, rows, ((totalpage > page) && page == 1), page);//

        //}
        /// <summary>
        /// 添加文章活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddJuActivity(HttpContext context)
        {
            int status = 0;
            string msg = string.Empty;
            object exObj = new object();
            string exStr = string.Empty;

            List<JuActivityFiles> files = new List<JuActivityFiles>();
            string file_list = context.Request["file_list"];
            if (!string.IsNullOrWhiteSpace(file_list))
            {
                List<Serv.API.Admin.Policy.Add.FileModel> nFiles = new List<Serv.API.Admin.Policy.Add.FileModel>();
                nFiles = ZentCloud.Common.JSONHelper.JsonToModel<List<Serv.API.Admin.Policy.Add.FileModel>>(file_list);
                foreach (var item in nFiles)
                {
                    files.Add(new JuActivityFiles()
                    {
                        AddDate = DateTime.Now,
                        FileClass = item.file_class,
                        FileName = item.file_name,
                        FilePath = item.file_path,
                        UserID = currentUserInfo.UserID
                    });
                }
            }

            this.bllJuActivity.AddJuActivity(context, this.currentUserInfo.UserID, bllBase.WebsiteOwner, ref status, ref msg, ref exObj, ref exStr, files);


            resp.ExObj = exObj;
            resp.Status = status;
            resp.Msg = msg;
            resp.ExStr = exStr;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 编辑文章活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditJuActivity(HttpContext context)
        {
            int status = 0;
            string msg = string.Empty;
            object exObj = new object();
            string exStr = string.Empty;

            List<JuActivityFiles> files = new List<JuActivityFiles>();
            string file_list = context.Request["file_list"];
            string noDeleteFileIds = "0";
            if (!string.IsNullOrWhiteSpace(file_list))
            {
                List<Serv.API.Admin.Policy.Add.FileModel> nFiles = new List<Serv.API.Admin.Policy.Add.FileModel>();
                nFiles = ZentCloud.Common.JSONHelper.JsonToModel<List<Serv.API.Admin.Policy.Add.FileModel>>(file_list);
                foreach (var item in nFiles.Where(p => p.id == 0))
                {
                    files.Add(new JuActivityFiles()
                    {
                        AddDate = DateTime.Now,
                        FileClass = item.file_class,
                        FileName = item.file_name,
                        FilePath = item.file_path,
                        UserID = currentUserInfo.UserID
                    });
                }
                if (nFiles.Where(p => p.id != 0).Count() > 0) noDeleteFileIds = ZentCloud.Common.MyStringHelper.ListToStr(nFiles.Where(p => p.id != 0).Select(p => p.id).ToList(), "", ",");

            }
            this.bllJuActivity.EditJuActivity(context, currentUserInfo.UserType.Equals(1) ? "" : this.currentUserInfo.UserID, bllBase.WebsiteOwner, ref status, ref msg, ref exObj, ref exStr, files, noDeleteFileIds);

            resp.Status = status;
            resp.Msg = msg;
            resp.ExObj = exObj;
            resp.ExStr = exStr;

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取单个文章活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSingelJuActivity(HttpContext context)
        {
            int juActivityId = Convert.ToInt32(context.Request["JuActivityID"]);
            BLLJIMP.Model.JuActivityInfo model = bllJuActivity.GetJuActivity(juActivityId);
            if (this.currentUserInfo.UserType != 1)
            {
                model = this.bllJuActivity.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" JuActivityID = {0} ", juActivityId.ToString(), string.Format(" AND UserID = '{0}' ", this.currentUserInfo.UserID)));
            }
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "活动不存在，或无权修改他人活动！";
            }
            else
            {
                resp.Status = 1;
                resp.ExObj = model;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        ///查询聚活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityForWeb(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserID"];
            string isToJubitActivity = context.Request["IsToJubitActivity"];
            string activityName = context.Request["ActivityName"];
            string isSignUpJubit = context.Request["IsSignUpJubit"];
            string articleType = context.Request["ArticleType"];
            string recommendCate = context.Request["RecommendCate"];
            string articleTypeEx1 = context.Request["ArticleTypeEx1"];
            string categoryId = context.Request["CategoryId"];
            int isShowHide = Convert.ToInt32(context.Request["isShowHide"]);
            int isShowSummary = Convert.ToInt32(context.Request["isShowSummary"]);
            if (categoryId == "0")
            {
                categoryId = "";
            }

            int totalCount = 0;
            string column = string.Format(@" JuActivityID,ActivityName,ArticleType,ThumbnailsPath,ActivityStartDate,ActivityEndDate,UserID,ActivityAddress,
                    SignUpActivityID,IsHide,Sort,SignUpCount,MonitorPlanID,AccessLevel,UV,PV,IP,CategoryId,CommentCount,PraiseCount,
                    FavoriteCount,RewardTotal,CreateDate,K1,K2,K3 {0}"
                , isShowSummary == 1 ? ",Summary" : "");
            List<JuActivityInfo> dataList = this.bllJuActivity.QueryJuActivityData(
                null, out totalCount, null, null, recommendCate, null, activityName, pageIndex, pageSize,
                "", null, articleType, bllBase.WebsiteOwner, articleTypeEx1, categoryId, "", "", "", "", "", false, "", false, true, false, "", null, false, column);

            BLLReview bllReview = new BLLReview();
            BLLShareMonitor bllShareMonitor = new BLLShareMonitor();
            bool isShowPv = pmsBll.CheckUserAndPmsKey(bllBase.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.IsShowArticlePv);
            List<UserInfo> uList = new List<UserInfo>();
            if (dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    UserInfo author = uList.FirstOrDefault(p => p.UserID == dataList[i].UserID);
                    if (author == null)
                    {
                        author = bllUser.GetUserInfo(dataList[i].UserID, bllUser.WebsiteOwner);
                        if (author == null)
                        {
                            author = bllUser.GetUserInfo("jubit", bllUser.WebsiteOwner);
                        }
                        uList.Add(author);
                    }
                    string userNickname = bllUser.GetUserDispalyName(author);
                    dataList[i].UserNickname = string.IsNullOrWhiteSpace(userNickname) ? dataList[i].UserID : userNickname;

                    if (dataList[i].ActivityEndDate != null)
                    {
                        if (DateTime.Now >= (DateTime)dataList[i].ActivityEndDate)
                        {
                            dataList[i].IsHide = 1;
                        }
                    }
                    //权限key控制是否显示pv
                    if (!isShowPv)
                    {
                        dataList[i].PV = 0;
                    }

                    var shareMonitorPlan = bllShareMonitor.GetMonitorByFK(dataList[i].JuActivityID.ToString(), dataList[i].ArticleType);

                    if (shareMonitorPlan != null)
                    {
                        dataList[i].ShareMonitorId = shareMonitorPlan.MonitorId;
                    }

                }
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = dataList
            });
        }

        /// <summary>
        ///导入公众号文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ImportMpArticle(HttpContext context)
        {
            string url = context.Request["url"];
            return bllWeixin.DownLoadMpArticle(url);
            //if (!string.IsNullOrEmpty(html))
            //{
            //    resp.IsSuccess = true;
            //    resp.ExObj = html;
            //}
            //return Common.JSONHelper.ObjectToJson(resp);
        }
        ///// <summary>
        /////查询活动 移动设备 old
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuActivityForWap(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    string IsToJubitActivity = context.Request["IsToJubitActivity"];
        //    string ActivityName = context.Request["ActivityName"];
        //    string IsSignUpJubit = context.Request["IsSignUpJubit"];
        //    string ArticleType = context.Request["ArticleType"];
        //    string RecommendCate = context.Request["RecommendCate"];
        //    string ArticleTypeEx1 = context.Request["ArticleTypeEx1"];
        //    int totalCount = 0;
        //    List<JuActivityInfo> dataList = this.juActivityBll.QueryJuActivityData(
        //        null, out totalCount, null, null, RecommendCate, null, ActivityName, page, rows,
        //        "",
        //        null, ArticleType, bllBase.WebsiteOwner, ArticleTypeEx1);
        //    int totalpage = this.juActivityBll.GetTotalPage(totalCount, rows);
        //    return ConverHtmlFormateArticleListForWap(dataList, rows, ((totalpage > page) && page == 1), page);//

        //}



        /// <summary>
        /// 查询文章活动 移动设备
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryArticleForWap(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string IsToJubitActivity = context.Request["IsToJubitActivity"];
            string activityName = context.Request["ActivityName"];
            string isSignUpJubit = context.Request["IsSignUpJubit"];
            string articleType = context.Request["ArticleType"];
            string recommendCate = context.Request["RecommendCate"];
            string articleTypeEx1 = context.Request["ArticleTypeEx1"];
            int totalCount = 0;
            List<JuActivityInfo> dataList = this.bllJuActivity.QueryJuActivityData(
                null, out totalCount, null, null, recommendCate, null, activityName, pageIndex, pageSize,
                currentUserInfo.UserID,
                null, articleType, bllBase.WebsiteOwner, articleTypeEx1);
            int totalpage = this.bllJuActivity.GetTotalPage(totalCount, pageSize);
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].ActivityDescription = null;

            }
            if (((totalpage > pageIndex) && pageIndex == 1))
            {
                resp.ExInt = 1;
            }
            if (dataList.Count > 0)
            {
                resp.Status = 1;
            }
            resp.ExObj = string.Format("[{0}]", ZentCloud.Common.JSONHelper.ListToJson<JuActivityInfo>(dataList, ","));
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除文章活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteJuActivity(HttpContext context)
        {
            BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
            bool isData = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);
            if (isData)
            {
                resp.Status = 0;
                resp.Msg = "权限不足";
            }
            string ids = context.Request["ids"];
            string type = context.Request["type"];
            int result = this.bllJuActivity.Update(new JuActivityInfo(), " IsDelete = 1 ", string.Format(" JuActivityID in ({0})  And WebsiteOwner='{1}' ", ids, bllBase.WebsiteOwner));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了{0}条数据", result);
            if (type == "article")
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Article, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllUser.GetCurrUserID(), "删除文章[id=" + ids + "]");
            }
            else if (type == "Outlets")
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Article, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllUser.GetCurrUserID(), "删除服务网点[id=" + ids + "]");
            }
            else
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Activity, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllUser.GetCurrUserID(), "删除活动[id=" + ids + "]");
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 更新文章活动排序号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateArticleSortIndex(HttpContext context)
        {
            string articleId = context.Request["ArticleID"];
            string sortIndex = context.Request["SortIndex"];
            int count = this.bllJuActivity.Update(new JuActivityInfo(), string.Format("Sort={0},LastUpdateDate=GETDATE()", sortIndex), string.Format(" JuActivityID ='{0}'", articleId));
            if (count.Equals(1))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 批量设置文章活动访问级别
        /// </summary>
        /// <returns></returns>
        private string UpdateAccessLevel(HttpContext context)
        {
            string accessLevel = context.Request["AccessLevel"];
            string juActivityIds = context.Request["JuActivityID"];
            int count = this.bllJuActivity.Update(new JuActivityInfo(), string.Format("AccessLevel={0}", Convert.ToInt32(accessLevel)), string.Format(" JuActivityID  in ({0})", juActivityIds));
            if (count > 0)
            {
                resp.Status = 1;
                resp.Msg = "保存成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }




        /// <summary>
        /// 文章转发树
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleSpreadTree(HttpContext context)
        {
            string articleId = context.Request["articleId"];
            string exSpreadUserId = context.Request["Ex_SpreadUserID"];
            string exShareTimestamp = context.Request["Ex_ShareTimestamp"];
            string id = context.Request["id"];

            var articleIdHex = Convert.ToString(int.Parse(articleId), 16);//文章活动ID十六进制
            string pageUrl = string.Format("http://{0}/{1}/Share.chtml", context.Request.Url.Host, articleIdHex);
            var nodeList = bllJuActivity.GetList<WebAccessLogsInfo>(string.Format(string.Format("Ex_PreSpreadUserID='{0}' And Ex_PreShareTimestamp='{1}' And PageUrl like '{2}%' And AutoID!='{3}' Order by AccessDate ASC", exSpreadUserId, exShareTimestamp, pageUrl, id)));//根节点
            if (nodeList.Count > 0)
            {
                nodeList = nodeList.DistinctBy(p => p.Ex_ShareTimestamp).ToList();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("[");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var item = nodeList[i];




                    var subList = bllJuActivity.GetList<WebAccessLogsInfo>(string.Format("Ex_PreSpreadUserID='{0}' And Ex_PreShareTimestamp='{1}' And AutoID!='{2}' Order by AccessDate ASC ", item.Ex_SpreadUserID, item.Ex_ShareTimestamp, id));

                    subList = subList.DistinctBy(p => p.Ex_ShareTimestamp).ToList();
                    int count = subList.Count;

                    var isParent = false;
                    if (count > 0)
                    {
                        isParent = true;
                    }
                    string wxNickName = "无昵称";
                    string wxHeadImg = "/zTree/css/zTreeStyle/img/diy/user.png";
                    string icon = "/zTree/css/zTreeStyle/img/diy/user.png";

                    var userInfo = bllUser.GetUserInfo(string.Format("UserID='{0}'", item.Ex_SpreadUserID));
                    if (userInfo != null)
                    {
                        if (!string.IsNullOrEmpty(userInfo.WXNickname))
                        {
                            wxNickName = userInfo.WXNickname;
                        }
                        if (!string.IsNullOrEmpty(userInfo.WXHeadimgurlLocal))
                        {
                            wxHeadImg = userInfo.WXHeadimgurlLocal;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(userInfo.WXOpenId))
                            {
                                //拉取用户信息并更新数据库

                                ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo weixinInfo = bllWeixin.GetWeixinUserInfo(currWebSiteUserInfo.UserID, currWebSiteUserInfo.WeixinAppId, currWebSiteUserInfo.WeixinAppSecret, userInfo.WXOpenId);
                                if (weixinInfo != null)
                                {
                                    if (!string.IsNullOrEmpty(weixinInfo.NickName))
                                    {
                                        userInfo.WXNickname = weixinInfo.NickName;
                                    }
                                    if (!string.IsNullOrEmpty(weixinInfo.HeadImgUrl))
                                    {
                                        userInfo.WXHeadimgurl = weixinInfo.HeadImgUrl;
                                    }
                                    bllUser.Update(userInfo, string.Format(" WXNickname='{0}',WXHeadimgurl='{1}'", userInfo.WXNickname, userInfo.WXHeadimgurl), string.Format(" AutoID={0}", userInfo.AutoID));

                                }

                            }

                        }


                    }
                    string tip = string.Format("<img src='{0}' align='absmiddle' width='100px' height='100px'/><br/>{1}<br/>被<span style='color:red;'>{2}</span>次转发", wxHeadImg, wxNickName, count);
                    var title = string.Format("<span style='color:blue;'>{0}</span>  <span style='color:red;'>{1}</span>转发 [{2}]", wxNickName, count, string.Format("{0:f}", item.AccessDate));

                    sb.Append("{");
                    sb.AppendFormat("name: \"{0}\", id: \"{1}\", count:{2}, times: 1, isParent:\"{3}\",Ex_SpreadUserID:\"{4}\",Ex_ShareTimestamp:\"{5}\",open:true,icon:\"{6}\",tip:\"{7}\"", title, item.AutoID, "1", isParent.ToString().ToLower(), item.Ex_SpreadUserID, item.Ex_ShareTimestamp, icon, tip);
                    sb.Append("}");

                    if (i < nodeList.Count - 1)//追加分隔符
                    {
                        sb.Append(",");
                    }

                }
                sb.Append("]");
                return sb.ToString();
            }
            else
            {
                return "";
            }





        }


        ///// <summary>
        ///// 专家库查询
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMaster(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    List<JuMasterInfo> dataList = new List<JuMasterInfo>();


        //    StringBuilder strWhere = new StringBuilder(" 1=1 ");

        //    strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);

        //    int totalcount = juActivityBll.GetCount<JuMasterInfo>(strWhere.ToString());
        //    int totalpage = this.juActivityBll.GetTotalPage(totalcount, rows);
        //    dataList = juActivityBll.GetLit<JuMasterInfo>(rows, page, strWhere.ToString());
        //    return ConverHtmlFormateMaster(dataList, rows, ((totalpage > page) && page == 1), page);










        //}
        ///// <summary>
        ///// 专家库查询pc端
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterForWeb(HttpContext context)
        //{

        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    string masterName = context.Request["MasterName"];
        //    StringBuilder sbCondtion = new StringBuilder(" 1=1 ");

        //    if (!currIsHFAdmin())
        //    {
        //        sbCondtion.AppendFormat(" AND AddUserID='{0}'", CurrentUserInfo.UserID);
        //    }
        //    if (!string.IsNullOrEmpty(masterName))
        //    {
        //        sbCondtion.AppendFormat(" And MasterName like '%{0}%'", masterName);
        //    }

        //    sbCondtion.AppendFormat(" AND WebsiteOwner = '{0}' ", bllBase.WebsiteOwner);


        //    List<ZentCloud.BLLJIMP.Model.JuMasterInfo> list = juActivityBll.GetLit<ZentCloud.BLLJIMP.Model.JuMasterInfo>(rows, page, sbCondtion.ToString(), "InserDate DESC");

        //    int totalCount = juActivityBll.GetCount<ZentCloud.BLLJIMP.Model.JuMasterInfo>(sbCondtion.ToString());
        //    string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalCount, list);
        //    return jsonResult;

        //}


        //private bool currIsHFAdmin()
        //{
        //    return DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Hongfeng_Admin);
        //}

        ///// <summary>
        ///// 修改讲师资料
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string EditJuMasterInfo(HttpContext context)
        //{
        //    JuMasterInfo model = new JuMasterInfo();
        //    model.MasterID = context.Request["MasterID"];
        //    model.MasterName = context.Request["MasterName"];
        //    model.Gender = context.Request["Gender"];
        //    model.Title = context.Request["Title"];
        //    model.Summary = context.Request["Summary"];
        //    model.IntroductionContent = context.Request["IntroductionContent"];
        //    model.HeadImg = context.Request["HeadImg"];

        //    JuMasterInfo masterInfo = juActivityBll.Get<JuMasterInfo>(string.Format("MasterID='{0}'", model.MasterID));
        //    if (masterInfo != null)//编辑
        //    {
        //        if (masterInfo.AddUserID == CurrentUserInfo.UserID)
        //        {
        //            masterInfo.MasterName = model.MasterName;
        //            masterInfo.Gender = model.Gender;
        //            masterInfo.Company = model.Company;
        //            masterInfo.Title = model.Title;
        //            masterInfo.Summary = model.Summary;
        //            masterInfo.IntroductionContent = model.IntroductionContent;
        //            masterInfo.HeadImg = model.HeadImg;
        //            if (juActivityBll.Update(masterInfo))
        //            {

        //                resp.Status = 1;
        //                resp.Msg = "保存成功！";


        //            }
        //            else
        //            {
        //                resp.Status = 0;
        //                resp.Msg = "保存失败";

        //            }
        //        }
        //        else
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "无权修改";

        //        }





        //    }
        //    else//添加
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "不存在的讲师";


        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);


        //}

        ///// <summary>
        ///// 添加讲师资料
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddJuMasterInfo(HttpContext context)
        //{
        //    JuMasterInfo model = new JuMasterInfo();
        //    model.MasterName = context.Request["MasterName"];
        //    model.Gender = context.Request["Gender"];
        //    model.Title = context.Request["Title"];
        //    model.Summary = context.Request["Summary"];
        //    model.IntroductionContent = context.Request["IntroductionContent"];
        //    model.HeadImg = context.Request["HeadImg"];
        //    model.MasterID = juActivityBll.GetGUID(BLLJIMP.TransacType.AddJuMasterID);
        //    model.AddUserID = CurrentUserInfo.UserID;
        //    model.WebsiteOwner = bllBase.WebsiteOwner;
        //    if (juActivityBll.Add(model))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "添加成功！";

        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "保存失败";

        //    }



        //    return Common.JSONHelper.ObjectToJson(resp);


        //}


        ///// <summary>
        ///// 删除讲师资料
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string DeleteJuMasterInfo(HttpContext context)
        //{

        //    string ids = context.Request["ids"];
        //    int count = juActivityBll.Delete(new JuMasterInfo(), string.Format("{0} MasterID in({1})", currIsHFAdmin() ? "" : string.Format(" AddUserID='{0}' and ", CurrentUserInfo.UserID), ids));
        //    if (count > 0)
        //    {
        //        resp.Status = 1;
        //        resp.Msg = string.Format("成功删除{0}条数据", count);

        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "删除失败";

        //    }



        //    return Common.JSONHelper.ObjectToJson(resp);


        //}

        /// <summary>
        /// 删除微信签到数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXSignInData(HttpContext context)
        {
            string ids = context.Request["ids"];

            return this.bllJuActivity.Delete(new WXSignInInfo(), string.Format(" AutoID in ({0}) ", ids)).ToString();

        }
        /// <summary>
        /// 批量设置分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatchSetArticleCategory(HttpContext context)
        {
            string ids = context.Request["ids"];
            string categoryId = context.Request["CategroyId"];
            int count = this.bllJuActivity.Update(new JuActivityInfo(), string.Format(" CategoryId='{0}'", categoryId), string.Format(" JuActivityID in ({0}) ", ids));
            if (count == ids.Split(',').Length)
            {
                resp.Status = 1;
                resp.Msg = "设置分类成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "设置分类失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 查询微信签到数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXSignInData(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int juActivityId = Convert.ToInt32(context.Request["jid"]);

            StringBuilder sbWhere = new StringBuilder();

            sbWhere.AppendFormat(" JuActivityID = {0} ", juActivityId);

            int totalCount = 0;
            List<WXSignInInfo> dataList = new List<WXSignInInfo>();
            dataList = this.bllJuActivity.GetLit<WXSignInInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            totalCount = this.bllJuActivity.GetCount<WXSignInInfo>(sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }


        ///// <summary>
        ///// 专家库查询格式化列表移动设备html
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMaster(List<JuMasterInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        #region old
        //        //sb.AppendLine("<div >");
        //        //sb.AppendFormat("<div style=\"margin-left:5px;margin-top:5px;margin-bottom:5px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" onclick=\"window.location.href='MasterDetails.aspx?masterId={0}';\" >", item.MasterID);
        //        //sb.AppendLine("<table style=\"width:100%;\">");
        //        //sb.AppendLine("<tbody>");
        //        ////sb.AppendFormat("<tr rel=\"/JuActivity/Wap/JuMasterDetails.aspx?masterid={0}\" onclick=\"GotoRel(this)\"  >", item.MasterID);
        //        //sb.AppendFormat("<tr>");
        //        //sb.AppendLine(" <td valign=\"top\" style=\"width:90px;\" >");
        //        //sb.AppendFormat("<img src=\"{0}\" style=\"width:75.75px;height:100px;\" />", item.HeadImg);
        //        //sb.AppendLine("</td>");
        //        //sb.AppendLine("<td>");
        //        //sb.AppendLine(" <div>");
        //        //sb.AppendLine("<label style=\"font-weight:bold;font-size: 20px;font-weight:700;font-family: Helvetica,Arial,sans-serif;\">");
        //        //sb.AppendLine(item.MasterName);
        //        //sb.AppendLine("</label>");

        //        //sb.AppendLine("</div>");
        //        //sb.AppendLine("<div style=\"margin-top:5px;color:#8E8E8E;\">");
        //        //sb.AppendLine("<img src=\"/JuActivity/Wap/image/master/postion.gif\" width=\"12px\" height=\"12px\">");
        //        //sb.AppendLine(item.Title);

        //        //sb.AppendLine("</div>");

        //        //sb.AppendLine(" <div style=\"font-size:100%;line-height:100%;color:#8E8E8E;margin-top:5px;font-family: Helvetica,Arial,sans-serif;margin-top:5px;\" >");
        //        //sb.AppendLine("<img src=\"/JuActivity/Wap/image/master/company.gif\" width=\"12px\" height=\"12px\">");
        //        //sb.AppendLine(item.Company);
        //        //sb.AppendLine("</div>");

        //        //sb.AppendLine("</td>");
        //        //sb.AppendLine("</tr>");

        //        //sb.AppendLine("</tbody>");

        //        //sb.AppendLine("</table>");

        //        //sb.AppendLine(" <div style=\"font-family: Helvetica,Arial,sans-serif;color:#8E8E8E;margin-left:5px;\">");
        //        //sb.AppendLine("<img src=\"/JuActivity/Wap/image/master/summary.gif\" width=\"12px\" height=\"12px\">");
        //        //sb.AppendLine("简介: ");
        //        //string sum = item.Summary;
        //        //if (sum != null)
        //        //{
        //        //    if (sum.Length > 81)
        //        //    {
        //        //        sum = sum.Substring(0, 80) + "...";
        //        //    }
        //        //}
        //        //sb.AppendLine(sum);
        //        //sb.AppendLine("</div>");
        //        //sb.AppendLine("</div>");
        //        //sb.AppendLine("</div>");
        //        //sb.AppendLine("<hr noshade size=1 align=center width=100%"); 
        //        #endregion

        //        sb.AppendLine("<li>");
        //        sb.AppendFormat("<a href=\"MasterDetails.aspx?masterId={0}\">", item.MasterID);
        //        sb.AppendFormat("<img src=\"{0}\" >", item.HeadImg);
        //        sb.AppendFormat("<h2>{0}<span>{1}</span></h2>", item.MasterName, item.Title);
        //        sb.AppendLine("<div class=\"article\">");
        //        sb.AppendFormat("<p>简介:{0}</p>", juActivityBll.ClearHtmlTag(item.Summary, 80));
        //        sb.AppendLine("</div>");
        //        sb.AppendLine("</a>");
        //        sb.AppendLine("</li>");



        //    }
        //    if (isShowBtnNext)
        //    {
        //        sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
        //        sb.AppendFormat("<li id=\"btnNext\" style=\"text-align:center;font-size: 12px;line-height: 18px;margin-bottom:50px;\"><div class=\"article\"><a>点击显示下{0}条</a></div></li>", rows);
        //    }
        //    return sb.ToString();
        //}


        ///// <summary>
        ///// 格式化文章活动课程移动设备html
        ///// </summary>
        ///// <param name="memberIDHex"></param>
        ///// <param name="dataList"></param>
        ///// <param name="rows"></param>
        ///// <param name="isShowBtnNext"></param>
        ///// <param name="pageIndex"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateArticleListForWap(List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        //{

        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count.Equals(0))
        //    {
        //        return "";
        //    }

        //    foreach (var item in dataList)
        //    {
        //        #region old
        //        //sb.AppendFormat("<li data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"div\" data-icon=\"arrow-r\" data-iconpos=\"right\" data-theme=\"c\" class=\"ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-li-has-thumb ui-btn-up-c\">");
        //        //sb.AppendLine("<div class=\"ui-btn-inner ui-li\">");
        //        //sb.AppendLine("<div class=\"ui-btn-text\">");
        //        //sb.AppendFormat("<a href=\"/{0}/details.chtml\" data-ajax=\"false\" class=\"ui-link-inherit\">", item.JuActivityIDHex);
        //        //sb.AppendFormat("<img src=\"{0}\" width=\"80\" height=\"80\" class=\"ui-li-thumb\">  ", item.ThumbnailsPath);
        //        //sb.AppendLine("<h2 class=\"ui-li-heading\">");
        //        //sb.AppendLine(item.ActivityName);
        //        //sb.AppendLine("</h2>");
        //        //if (!string.IsNullOrWhiteSpace(item.ActivityLecturer))
        //        //    sb.AppendFormat("<p class=\"ui-li-desc\">讲师： {0}</p> ", item.ActivityLecturer);
        //        //if (item.ActivityStartDate != null)
        //        //    sb.AppendFormat("<p class=\"ui-li-desc\">开课时间： {0}</p> ", item.ActivityStartDate.Value.ToString());

        //        //sb.AppendLine("</a>");
        //        //sb.AppendLine("</div>");
        //        //sb.AppendLine("<span class=\"ui-icon ui-icon-arrow-r ui-icon-shadow\">&nbsp;</span>");
        //        //sb.AppendLine("</div>");
        //        //sb.AppendLine(" </li>"); 
        //        #endregion

        //        sb.AppendLine("<li>");
        //        sb.AppendFormat("<a href=\"/{0}/details.chtml\">", item.JuActivityIDHex);
        //        sb.AppendFormat("<img src=\"{0}\" >", item.ThumbnailsPath);
        //        sb.AppendFormat("<h2>{0}</h2>", juActivityBll.ClearHtmlTag(item.ActivityName, 15));
        //        sb.AppendLine("<div class=\"article\">");
        //        if (string.IsNullOrEmpty(item.ActivityLecturer))
        //        {
        //            sb.AppendFormat("<p>{0}</p>", juActivityBll.ClearHtmlTag(item.ActivityDescription, 60));
        //        }
        //        if (!string.IsNullOrWhiteSpace(item.ActivityLecturer))
        //        {
        //            sb.AppendFormat("<p>讲师： {0}</p> ", item.ActivityLecturer);
        //        }
        //        if ((item.ActivityStartDate != null))
        //        {
        //            if (item.ArticleType.ToLower().Equals("activity"))
        //            {
        //                sb.AppendFormat("<p>{0:f}</p> ", item.ActivityStartDate);
        //            }
        //            else
        //            {
        //                sb.AppendFormat("<p>开课时间： {0:f}</p> ", item.ActivityStartDate);
        //            }

        //        }
        //        sb.AppendLine("</div>");
        //        sb.AppendLine("</a>");
        //        sb.AppendLine("</li>");

        //    }
        //    if (isShowBtnNext)
        //    {
        //        sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
        //        //sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 显示下{0}条</div>", rows);

        //        sb.AppendFormat("<li id=\"btnNext\" style=\"text-align:center;font-size: 12px;line-height: 18px;margin-bottom:50px;\"><div class=\"article\"><a>点击显示下{0}条</a></div></li>", rows);
        //    }
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// 同步微信粉丝信息
        ///// </summary>
        ///// <returns></returns>
        //private string SynchronousAllFollowers()
        //{

        //    return bllWeixin.SynchronousAllFollowers(currWebSiteUserInfo.UserID, currWebSiteUserInfo.WeixinAppId, currWebSiteUserInfo.WeixinAppSecret).ToString();


        //}
        /// <summary>
        /// 更新微信粉丝数量
        /// </summary>
        /// <returns></returns>
        private string UpdateAllFollowersInfo()
        {
            TimingTask task = new TimingTask();
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = 1;
            task.TaskInfo = "同步粉丝";
            task.TaskType = 3;
            task.ScheduleDate = DateTime.Now;
            if (bllUser.Add(task))
            {
                return "已经添加到任务中";
            }
            else
            {
                return "添加任务未成功";
            }
            // return bllWeixin.UpdateAllFollowersInfo(currWebSiteUserInfo.UserID).ToString();

        }

        /// <summary>
        /// 同步微信素材
        /// </summary>
        /// <returns></returns>
        private string SynWeixinNews()
        {

            return bllWeixin.SynWeixinNews(bllWeixin.WebsiteOwner).ToString();


        }

        /// <summary>
        /// 查询粉丝
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWeixinFollowersInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["KeyWord"];
            string isFollower = context.Request["IsFollower"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And NickName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(isFollower))
            {
                sbWhere.AppendFormat(" And IsWeixinFollower = {0} ", isFollower);
            }
            #region 注释
            //string pmsGroup = context.Request["pmsGroup"];
            //if (!string.IsNullOrEmpty(pmsGroup))
            //{

            //    for (int i = 0; i < pmsGroup.Split(',').Length; i++)
            //    {
            //        if (i == 0)
            //        {
            //            searchCondition.AppendFormat(" And (UserPmsGroup='{0}'", pmsGroup.Split(',')[i]);
            //        }
            //        else
            //        {
            //            searchCondition.AppendFormat(" Or UserPmsGroup='{0}'", pmsGroup.Split(',')[i]);
            //        }


            //    }
            //    searchCondition.AppendLine(")");

            //}
            #endregion
            List<WeixinFollowersInfo> list = bllWeixin.GetLit<WeixinFollowersInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");

            int totalCount = bllWeixin.GetCount<WeixinFollowersInfo>(sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = list
            });

        }

        /// <summary>
        /// 配置客服
        /// </summary>
        /// <returns></returns>
        private string SetKeFuConfig(HttpContext context)
        {

            string kefuOpenId = context.Request["KeFuOpenID"];
            if (string.IsNullOrEmpty(kefuOpenId))
            {
                resp.Status = 1;
                resp.Msg = "客服微信号OpenId不能为空";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            currWebSiteUserInfo.WeiXinKeFuOpenId = kefuOpenId;
            if (bllJuActivity.Update(currWebSiteUserInfo, string.Format(" WeiXinKeFuOpenId='{0}'", currWebSiteUserInfo.WeiXinKeFuOpenId), string.Format(" AutoID={0}", currWebSiteUserInfo.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "设置成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "设置失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        /// 商城配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWXMallConfig(HttpContext context)
        {

            currentWebSiteInfo = bllBase.GetWebsiteInfoModelFromDataBase();

            currentWebSiteInfo.SumbitOrderPromptInformation = context.Request["SumbitOrderPromptInformation"];
            currentWebSiteInfo.MallTemplateId = string.IsNullOrEmpty(context.Request["MallTemplateId"]) ? 0 : int.Parse(context.Request["MallTemplateId"]);
            currentWebSiteInfo.WXMallName = context.Request["WXMallName"];
            currentWebSiteInfo.MallType = context.Request["MallType"];
            currentWebSiteInfo.WXMallMemberCardMessage = context.Request["WXMallMemberCardMessage"];
            currentWebSiteInfo.WXMallBannerImage = context.Request["WXMallBannerImage"];
            currentWebSiteInfo.ShopDescription = context.Request["ShopDescription"];
            currentWebSiteInfo.ShopAdType = context.Request["ShopAdType"];
            currentWebSiteInfo.ShopNavGroupName = context.Request["ShopNavGroupName"];
            currentWebSiteInfo.ShopFoottool = context.Request["ShopFoottool"];
            currentWebSiteInfo.ProductImgRatio1 = context.Request["ProductImgRatio1"];
            currentWebSiteInfo.ProductImgRatio2 = context.Request["ProductImgRatio2"];
            currentWebSiteInfo.IsShowOldPrice = Convert.ToInt32(context.Request["IsShowOldPrice"]);
            currentWebSiteInfo.IsShowStock = Convert.ToInt32(context.Request["IsShowStock"]);
            currentWebSiteInfo.ThemeColor = context.Request["ThemeColor"];
            currentWebSiteInfo.IsRebateScoreMustAllCash = Convert.ToInt32(context.Request["IsRebateScoreMustAllCash"]);
            currentWebSiteInfo.IsOrderRebateScoreByMallOrder = Convert.ToInt32(context.Request["IsOrderRebateScoreByMallOrder"]);
            currentWebSiteInfo.IsOrderRebateScoreByCreateGroupBuy = Convert.ToInt32(context.Request["IsOrderRebateScoreByCreateGroupBuy"]);
            currentWebSiteInfo.IsOrderRebateScoreByJoinGroupBuy = Convert.ToInt32(context.Request["IsOrderRebateScoreByJoinGroupBuy"]);
            currentWebSiteInfo.RebateScoreGetIntType = Convert.ToInt32(context.Request["RebateScoreGetIntType"]);
            currentWebSiteInfo.IsOpenGroup = Convert.ToInt32(context.Request["IsOpenGroup"]);
            currentWebSiteInfo.IsClaimMallOrderArrivalTime = Convert.ToInt32(context.Request["IsClaimMallOrderArrivalTime"]);
            currentWebSiteInfo.ProductStockThreshold = Convert.ToInt32(context.Request["ProductStockThreshold"]);
            currentWebSiteInfo.IsShowProductSaleCount = Convert.ToInt32(context.Request["IsShowProductSaleCount"]);
            currentWebSiteInfo.IsNeedMallOrderCreaterNamePhone = Convert.ToInt32(context.Request["IsNeedMallOrderCreaterNamePhone"]);
            currentWebSiteInfo.NeedMallOrderCreaterNamePhoneRName = context.Request["NeedMallOrderCreaterNamePhoneRName"];
            currentWebSiteInfo.IsShowStockValue = Convert.ToInt32(context.Request["IsShowStockValue"]);
            currentWebSiteInfo.MallDescTop = context.Request["MallDescTop"];
            currentWebSiteInfo.MallDescBottom = context.Request["MallDescBottom"];
            currentWebSiteInfo.IsOrderAutoComment = Convert.ToInt32(context.Request["IsOrderAutoComment"]);
            currentWebSiteInfo.OrderCancelMinute = context.Request["OrderCancelMinute"];
            currentWebSiteInfo.OrderAutoCommentDay = Convert.ToInt32(context.Request["OrderAutoCommentDay"]);
            currentWebSiteInfo.OrderAutoCommentContent = context.Request["OrderAutoCommentContent"];
            currentWebSiteInfo.IsCustomizeMallHead = Convert.ToInt32(context.Request["IsCustomizeMallHead"]);
            currentWebSiteInfo.CustomizeMallHeadConfig = context.Request["CustomizeMallHeadConfig"];
            currentWebSiteInfo.MallScorePayRatio = context.Request["MallScorePayRatio"];
            string orderAmount = context.Request["OrderAmount"];
            string orderScore = context.Request["OrderScore"];
            string exchangeScore = context.Request["ExchangeScore"];
            string exchangeAmount = context.Request["ExchangeAmount"];
            string minDeliveryDate = context.Request["MinDeliveryDate"];
            int isAutoCloseRefund = Convert.ToInt32(context.Request["IsAutoCloseRefund"]);
            int autoCloseRefundDay = Convert.ToInt32(context.Request["AutoCloseRefundDay"]);


            int stockType = Convert.ToInt32(context.Request["StockType"]);//库存模式
            int isAutoAssignOrder = Convert.ToInt32(context.Request["IsAutoAssignOrder"]);//是否自动分单
            string autoAssignOrderRange = context.Request["AutoAssignOrderRange"];

            int shopCartAlongSettlement = Convert.ToInt32(context.Request["ShopCartAlongSettlement"]);//是否开启购物车独立结算
            int isStoreSince = Convert.ToInt32(context.Request["IsStoreSince"]);//是否开启门店自提
            string storeSinceTimeJson = context.Request["StoreSinceTimeJson"];//门店自提时间段

            int isHomeDelivery = Convert.ToInt32(context.Request["IsHomeDelivery"]);//是否送货上们
            string earliestDeliveryTime = context.Request["EarliestDeliveryTime"];//送货上门最早时间
            string homeDeliveryTimeJson = context.Request["HomeDeliveryTimeJson"]; //送货上门时间段

            int storeExpressRange = Convert.ToInt32(context.Request["StoreExpressRange"]);
            int expressRange = Convert.ToInt32(context.Request["ExpressRange"]);
            int storeSinceDiscount = Convert.ToInt32(context.Request["StoreSinceDiscount"]);
            if (!string.IsNullOrEmpty(minDeliveryDate))
            {
                currentWebSiteInfo.MinDeliveryDate = int.Parse(minDeliveryDate);
            }
            if (bllWebsite.Update(currentWebSiteInfo))
            {
                resp.Status = 1;
                resp.Msg = "保存成功！";
                if (!bllScore.UpdateScoreConfig(orderAmount, orderScore, exchangeScore, exchangeAmount))
                {

                    resp.Msg = "积分设置失败！";
                }


                //
                CompanyWebsite_Config model = bllWebsite.GetCompanyWebsiteConfig();

                if (model.AutoID == 0)//还没有配置
                {
                    //model.WebsiteTitle = websiteTitle;
                    //model.Copyright = copyright;
                    //model.WebsiteOwner = WebsiteOwner;
                    //model.WebsiteImage = websiteimg;
                    //model.WebsiteDescription = websitedescription;
                    //model.ShopNavGroupName = groupName;
                    //model.ShopAdType = shopAdType;
                    //model.BottomToolbars = buttomtoolbar;
                    //model.MemberStandard = Convert.ToInt32(memberStandard);
                    //model.HaveComment = Convert.ToInt32(haveComment);
                    //model.MemberStandardDescription = memberStandardDescription;
                    //model.MyCardCouponsTitle = myCardCouponsTitle;
                    //model.WeixinAccountNickName = weixinAccountNickName;
                    //model.DistributionQRCodeIcon = distributionQRCodeIcon;
                    //model.ArticleToolBarGrous = articleToolBarGrous;
                    //model.ActivityToolBarGrous = activityToolBarGrous;
                    //model.GroupBuyIndexUrl = groupBuyIndexUrl;
                    //model.NoPermissionsPage = int.Parse(noPermissionsPage);
                    //model.PersonalCenterLink = personalCenterLink;
                    //model.LowestAmount = lowestAmount;
                    //model.Tel = tel;
                    //model.QQ = qq;
                    //model.IsDisableKefu = isDisableKefu;
                    //model.KefuImage = kefuImage;
                    //model.KefuUrl = kefuUrl;
                    //model.KefuOnLineReply = kefuOnLineReply;
                    //model.KefuOffLineReply = kefuOffLineReply;
                    //model.IsEnableCustomizeLoginPage = Convert.ToInt32(isEnableCustomizeLoginPage);
                    //model.LoginConfigJson = loginConfigJson;
                    //model.OutletsSearchRange = outletsSearchRange;
                    model.IsAutoCloseRefund = isAutoCloseRefund;
                    model.AutoCloseRefundDay = autoCloseRefundDay;

                    model.StockType = stockType;
                    model.IsAutoAssignOrder = isAutoAssignOrder;
                    model.AutoAssignOrderRange = autoAssignOrderRange;
                    model.ShopCartAlongSettlement = shopCartAlongSettlement;
                    model.IsStoreSince = isStoreSince;
                    model.StoreSinceTimeJson = storeSinceTimeJson;
                    model.IsHomeDelivery = isHomeDelivery;
                    model.EarliestDeliveryTime = earliestDeliveryTime;
                    model.HomeDeliveryTimeJson = homeDeliveryTimeJson;
                    model.ExpressRange = expressRange;
                    model.StoreExpressRange = storeExpressRange;
                    model.StoreSinceDiscount = storeSinceDiscount;
                    model.WebsiteOwner = bllWebsite.WebsiteOwner;
                    if (!bllWebsite.Add(model))
                    {
                        resp.Msg = "添加 CompanyWebsite_Config失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    } 

                }
                else
                {
                    
                    //model.WebsiteTitle = websiteTitle;
                    //model.Copyright = copyright;
                    //model.WebsiteImage = websiteimg;
                    //model.WebsiteDescription = websitedescription;
                    //model.ShopNavGroupName = groupName;
                    //model.ShopAdType = shopAdType;
                    //model.BottomToolbars = buttomtoolbar;
                    //model.MemberStandard = Convert.ToInt32(memberStandard);
                    //model.HaveComment = Convert.ToInt32(haveComment);
                    //model.MemberStandardDescription = memberStandardDescription;
                    //model.MyCardCouponsTitle = myCardCouponsTitle;
                    //model.WeixinAccountNickName = weixinAccountNickName;
                    //model.DistributionQRCodeIcon = distributionQRCodeIcon;
                    //model.ArticleToolBarGrous = articleToolBarGrous;
                    //model.ActivityToolBarGrous = activityToolBarGrous;
                    //model.GroupBuyIndexUrl = groupBuyIndexUrl;
                    //model.NoPermissionsPage = int.Parse(noPermissionsPage);
                    //model.PersonalCenterLink = personalCenterLink;
                    //model.LowestAmount = lowestAmount;
                    //model.Tel = tel;
                    //model.QQ = qq;
                    //model.IsDisableKefu = isDisableKefu;
                    //model.KefuImage = kefuImage;
                    //model.KefuUrl = kefuUrl;
                    //model.KefuOnLineReply = kefuOnLineReply;
                    //model.KefuOffLineReply = kefuOffLineReply;
                    //model.IsEnableCustomizeLoginPage = Convert.ToInt32(isEnableCustomizeLoginPage);
                    //model.LoginConfigJson = loginConfigJson;
                    //model.OutletsSearchRange = outletsSearchRange;
                    model.IsAutoCloseRefund = isAutoCloseRefund;
                    model.AutoCloseRefundDay = autoCloseRefundDay;
                    model.StockType = stockType;
                    model.IsAutoAssignOrder = isAutoAssignOrder;
                    model.AutoAssignOrderRange = autoAssignOrderRange;
                    model.ShopCartAlongSettlement = shopCartAlongSettlement;
                    model.IsStoreSince = isStoreSince;
                    model.StoreSinceTimeJson = storeSinceTimeJson;
                    model.IsHomeDelivery = isHomeDelivery;
                    model.EarliestDeliveryTime = earliestDeliveryTime;
                    model.HomeDeliveryTimeJson = homeDeliveryTimeJson;
                    model.ExpressRange = expressRange;
                    model.StoreExpressRange = storeExpressRange;
                    model.StoreSinceDiscount = storeSinceDiscount;
                    if (!bllWebsite.Update(model))
                    {
                        resp.Msg = "修改 CompanyWebsite_Config失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    
                }
                //


                //if (bllMall.Update(new CompanyWebsite_Config(), string.Format(" IsAutoCloseRefund={0},AutoCloseRefundDay={1}", isAutoCloseRefund, autoCloseRefundDay), string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner)) < 0)
                //{

                //    resp.Msg = "退款设置失败！";
                //}

                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Config, bllLog.GetCurrUserID(), "商城配置[" + bllLog.GetCurrUserID() + "]");

            }
            else
            {

                resp.Msg = "保存失败！";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }



        //商城模块
        #region 商品管理模块
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallProductInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["PName"];
            string categoryId = context.Request["CategoryId"];
            string isOnSale = context.Request["IsOnSale"];

            //string StoreId = context.Request["StoreId"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'  And IsDelete=0", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And PName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(categoryId) && (!categoryId.Equals("0")))
            {
                sbWhere.AppendFormat(" And CategoryId = '{0}'", categoryId);
            }
            if (!string.IsNullOrEmpty(isOnSale))
            {
                sbWhere.AppendFormat(" And IsOnSale = '{0}'", isOnSale);
            }


            //if (!string.IsNullOrEmpty(StoreId))
            //{
            //    sbWhere.AppendFormat(" And StoreId = '{0}'", StoreId);
            //}

            int totalCount = bllJuActivity.GetCount<WXMallProductInfo>(sbWhere.ToString());
            //int pagecount = this.juActivityBll.GetTotalPage(count, rows);
            List<WXMallProductInfo> dataList = new List<WXMallProductInfo>();
            dataList = bllJuActivity.GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].PDescription = null;

            }
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });


        }



        ///// <summary>
        ///// 前味库存统计 已无用
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QianWeiStockStatistics(HttpContext context)
        //{
        //    string CategoryId = context.Request["CategoryId"];
        //    StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And IsOnSale=1 And IsDelete=0 ", bllBase.WebsiteOwner));
        //    if (!string.IsNullOrEmpty(CategoryId))
        //    {
        //        sbWhere.AppendFormat(" And CategoryId = '{0}'", CategoryId);
        //    }
        //    int totalcount = bllJuActivity.GetCount<WXMallProductInfo>(sbWhere.ToString());
        //    List<WXMallProductInfo> dataList = new List<WXMallProductInfo>();
        //    dataList = bllJuActivity.GetList<WXMallProductInfo>(sbWhere.ToString());
        //    for (int i = 0; i < dataList.Count; i++)
        //    {
        //        dataList[i].PDescription = null;

        //    }
        //    string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalcount, dataList);
        //    return jsonResult;


        //}

        ///// <summary>
        ///// 更新库存
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string UpdateProductStock(HttpContext context)
        //{
        //    int ProductID = int.Parse(context.Request["ProductID"]);
        //    int UpdateCount = int.Parse(context.Request["UpdateCount"]);
        //    WXMallProductInfo model = bllMall.Get<WXMallProductInfo>(string.Format(" PID={0}", ProductID));
        //    if ((!model.WebsiteOwner.Equals(currentUserInfo.WebsiteOwner)) && (!currentUserInfo.UserType.Equals(1)))
        //    {
        //        resp.Msg = "拒绝访问";
        //        return Common.JSONHelper.ObjectToJson(resp);


        //    }
        //    model.Stock += UpdateCount;
        //    if (model.Stock < 0)
        //    {
        //        resp.Msg = "库存不能小于0";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    if (bllMall.Update(model))
        //    {
        //        resp.Status = 1;
        //    }
        //    else
        //    {
        //        resp.Msg = "更新失败";
        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);


        //}
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallProductInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            //检查权限，检查是否有订单相关
            return bllMall.Update(new WXMallProductInfo(), " IsDelete=1", string.Format("PID in ({0}) And WebsiteOwner='{1}' ", ids, bllBase.WebsiteOwner)).ToString();


        }
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallProductInfo(HttpContext context)
        {
            string productName = context.Request["PName"];//商品名称
            string productDesc = context.Request["PDescription"];//商品详情
            string previousPrice = context.Request["PreviousPrice"];//原价 吊牌价
            string price = context.Request["Price"];//现价
            string recommendImg = context.Request["RecommendImg"];//主图
            string categoryId = context.Request["CategoryId"];//分类ID
            string isOnSale = context.Request["IsOnSale"];//是否上架
            string stock = context.Request["Stock"];//库存
            string isNew = context.Request["IsNew"];//是否最新上架
            string isHot = context.Request["IsHot"];//是否热卖
            string isSpecial = context.Request["IsSpecial"];//是否特价
            string isRecommend = context.Request["IsRecommend"];//是否推荐
            string promotionStartTime = context.Request["PromotionStartTime"];//限时特卖开始时间
            string promotionStopTime = context.Request["PromotionStopTime"];//限时特卖结束时间
            //string StoreId = context.Request["StoreId"];
            decimal priceD = 0;
            int stockInt = 0;
            //检查输入
            if (string.IsNullOrEmpty(productName))
            {
                resp.Status = 0;
                resp.Msg = "商品名称不能为空";
                return Common.JSONHelper.ObjectToJson(resp);


            }
            if (!string.IsNullOrEmpty(price))
            {
                if (!Decimal.TryParse(price, out priceD))
                {
                    resp.Status = 0;
                    resp.Msg = "商品价格不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (priceD < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "商品价格须大于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }

            //if (PriceD <= 0)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "商品价格须大于0";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            if (!string.IsNullOrEmpty(stock))
            {
                if (!int.TryParse(stock, out stockInt))
                {
                    resp.Status = 0;
                    resp.Msg = "库存不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }
            WXMallProductInfo model = new WXMallProductInfo();
            model.PID = bllMall.GetGUID(TransacType.AddWXMallProductID);
            model.PName = productName;
            if (!string.IsNullOrEmpty(previousPrice))
            {
                model.PreviousPrice = decimal.Parse(previousPrice);

            }

            model.Price = priceD;


            model.UserID = currentUserInfo.UserID;
            model.PDescription = productDesc;
            model.RecommendImg = recommendImg;
            model.InsertDate = DateTime.Now;
            model.CategoryId = categoryId;
            model.IsOnSale = isOnSale;
            model.IsDelete = 0;
            model.Stock = stockInt;
            //model.StoreId = StoreId;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.ShowImage1 = context.Request["ShowImage1"];
            model.ShowImage2 = context.Request["ShowImage2"];
            model.ShowImage3 = context.Request["ShowImage3"];
            model.ShowImage4 = context.Request["ShowImage4"];
            model.ShowImage5 = context.Request["ShowImage5"];
            //if (!string.IsNullOrEmpty(isNew))
            //{
            //    model.IsNew = int.Parse(isNew);
            //}
            //if (!string.IsNullOrEmpty(isHot))
            //{
            //    model.IsHot = int.Parse(isHot);
            //}
            //if (!string.IsNullOrEmpty(isSpecial))
            //{
            //    model.IsSpecial = int.Parse(isSpecial);
            //}
            //if (!string.IsNullOrEmpty(isRecommend))
            //{
            //    model.IsRecommend = int.Parse(isRecommend);
            //}

            if (bllMall.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加商品成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加商品失败";


            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallProductInfo(HttpContext context)
        {
            string productId = context.Request["PID"];
            string productName = context.Request["PName"];
            string productDesc = context.Request["PDescription"];
            string previousPrice = context.Request["PreviousPrice"];
            string price = context.Request["Price"];
            string recommendImg = context.Request["RecommendImg"];
            string categoryId = context.Request["CategoryId"];
            string isOnSale = context.Request["IsOnSale"];
            string stock = context.Request["Stock"];
            //string StoreId = context.Request["StoreId"];
            string isNew = context.Request["IsNew"];//是否最新上架
            string isHot = context.Request["IsHot"];//是否热卖
            string isSpecial = context.Request["IsSpecial"];//是否特价
            string isRecommend = context.Request["IsRecommend"];//是否推荐

            string promotionStartTime = context.Request["PromotionStartTime"];//限时特卖开始时间
            string promotionStopTime = context.Request["PromotionStopTime"];//限时特卖结束时间
            int stockInt = 0;
            decimal priceD = 0;
            //检查输入
            if (string.IsNullOrEmpty(productName))
            {
                resp.Status = 0;
                resp.Msg = "商品名称不能为空";
                return Common.JSONHelper.ObjectToJson(resp);


            }
            if (!string.IsNullOrEmpty(price))
            {
                if (!Decimal.TryParse(price, out priceD))
                {
                    resp.Status = 0;
                    resp.Msg = "商品价格不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (priceD < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "商品价格须大于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }



            if (!string.IsNullOrEmpty(stock))
            {
                if (!int.TryParse(stock, out stockInt))
                {
                    resp.Status = 0;
                    resp.Msg = "库存不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }
            WXMallProductInfo model = bllMall.GetProduct(productId);
            model.PID = productId;
            model.PName = productName;
            if (!string.IsNullOrEmpty(previousPrice))
            {
                model.PreviousPrice = decimal.Parse(previousPrice);

            }

            model.Price = priceD;
            //model.UserID = userInfo.UserID;
            model.PDescription = productDesc;
            model.RecommendImg = recommendImg;
            //model.InsertDate = DateTime.Now;
            model.CategoryId = categoryId;
            model.IsOnSale = isOnSale;
            model.Stock = stockInt;
            //model.StoreId = StoreId;
            model.ShowImage1 = context.Request["ShowImage1"];
            model.ShowImage2 = context.Request["ShowImage2"];
            model.ShowImage3 = context.Request["ShowImage3"];
            model.ShowImage4 = context.Request["ShowImage4"];
            model.ShowImage5 = context.Request["ShowImage5"];
            //if (!string.IsNullOrEmpty(isNew))
            //{
            //    model.IsNew = int.Parse(isNew);
            //}
            //if (!string.IsNullOrEmpty(isHot))
            //{
            //    model.IsHot = int.Parse(isHot);
            //}
            //if (!string.IsNullOrEmpty(isSpecial))
            //{
            //    model.IsSpecial = int.Parse(isSpecial);
            //}
            //if (!string.IsNullOrEmpty(isRecommend))
            //{
            //    model.IsRecommend = int.Parse(isRecommend);
            //}


            if (bllMall.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";


            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        #endregion

        #region 积分商品管理模块
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallScoreProductInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["PName"];
            string categoryId = context.Request["CategoryId"];
            string isOnSale = context.Request["IsOnSale"];
            string onLine = context.Request["OnLine"];
            string typeId = context.Request["TypeId"];//分类
            //string StoreId = context.Request["StoreId"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'  And IsDelete=0", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And PName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat(" And CategoryId = '{0}'", categoryId);
            }
            if (!string.IsNullOrEmpty(isOnSale))
            {
                sbWhere.AppendFormat(" And IsOnSale = '{0}'", isOnSale);
            }
            if (!string.IsNullOrEmpty(onLine))
            {
                sbWhere.AppendFormat(" And ScoreLine = '{0}'", onLine);
            }
            if (!string.IsNullOrEmpty(typeId))
            {
                sbWhere.AppendFormat(" And TypeId = {0}", typeId);
            }

            //if (!string.IsNullOrEmpty(StoreId))
            //{
            //    sbWhere.AppendFormat(" And StoreId = '{0}'", StoreId);
            //}

            int totalCount = bllMall.GetCount<WXMallScoreProductInfo>(sbWhere.ToString());

            List<WXMallScoreProductInfo> dataList = new List<WXMallScoreProductInfo>();
            dataList = bllMall.GetLit<WXMallScoreProductInfo>(pageSize, pageIndex, sbWhere.ToString(), "Sort DESC,InsertDate DESC");
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].PDescription = null;

            }
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallScoreProductInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            //检查权限，检查是否有订单相关
            return bllMall.Update(new WXMallScoreProductInfo(), " IsDelete=1", string.Format("AutoID in ({0}) And WebsiteOwner='{1}' ", ids, bllBase.WebsiteOwner)).ToString();


        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallScoreProductInfo(HttpContext context)
        {
            WXMallScoreProductInfo model = new WXMallScoreProductInfo();
            string pName = context.Request["PName"];
            string pDescription = context.Request["PDescription"];
            string score = context.Request["Score"];
            string recommendImg = context.Request["RecommendImg"];
            string isOnSale = context.Request["IsOnSale"];
            string stock = context.Request["Stock"];
            string onLine = context.Request["OnLine"];
            string typeId = context.Request["TypeId"];
            //string StoreId = context.Request["StoreId"];
            string discountScore = context.Request["DiscountScore"];
            int scoreD = 0;
            int stockInt = 0;
            int discountScoreInt = 0;
            //检查输入
            if (string.IsNullOrEmpty(pName))
            {
                resp.Msg = "积分商品名称不能为空";
                goto outoff;


            }

            if (!int.TryParse(score, out scoreD))
            {
                resp.Msg = "商品积分不正确";
                goto outoff;

            }
            if (scoreD < 0)
            {
                resp.Msg = "商品所需积分须大于0";
                goto outoff;

            }
            if (!string.IsNullOrEmpty(discountScore))
            {
                if (!int.TryParse(discountScore, out discountScoreInt))
                {
                    resp.Msg = "打折积分需是整数";
                    goto outoff;
                }
                else
                {
                    if (discountScoreInt < 0)
                    {
                        resp.Msg = "打折积分需大于0";
                        goto outoff;
                    }
                    model.DiscountScore = discountScoreInt;
                }


            }

            if (!string.IsNullOrEmpty(stock))
            {
                if (!int.TryParse(stock, out stockInt))
                {
                    resp.Msg = "库存不正确";
                    goto outoff;

                }
            }
            model.PName = pName;
            model.Score = scoreD;
            model.UserID = currentUserInfo.UserID;
            model.PDescription = pDescription;
            model.RecommendImg = recommendImg;
            model.InsertDate = DateTime.Now;
            model.IsOnSale = isOnSale;
            model.IsDelete = 0;
            model.Stock = stockInt;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.ScoreLine = onLine;
            model.TypeId = Convert.ToInt32(typeId);
            model.ShowImage1 = context.Request["ShowImage1"];
            model.ShowImage2 = context.Request["ShowImage2"];
            model.ShowImage3 = context.Request["ShowImage3"];
            model.ShowImage4 = context.Request["ShowImage4"];
            model.ShowImage5 = context.Request["ShowImage5"];

            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";


            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallScoreProductInfo(HttpContext context)
        {
            WXMallScoreProductInfo model = new WXMallScoreProductInfo();
            string autoId = context.Request["AutoID"];
            string pName = context.Request["PName"];
            string pDescription = context.Request["PDescription"];
            string score = context.Request["Score"];
            string recommendImg = context.Request["RecommendImg"];
            string isOnSale = context.Request["IsOnSale"];
            string stock = context.Request["Stock"];
            string onLine = context.Request["OnLine"];
            string typeId = context.Request["TypeId"];
            string discountScore = context.Request["DiscountScore"];
            string sort = context.Request["Sort"];

            int stockInt = 0;
            int scoreD = 0;
            int discountScoreInt = 0;
            //检查输入
            if (string.IsNullOrEmpty(pName))
            {
                resp.Msg = "名称不能为空";
                goto outoff;
            }

            if (!int.TryParse(score, out scoreD))
            {
                resp.Status = 0;
                resp.Msg = "商品积分不正确";
                goto outoff;
            }
            if (scoreD < 0)
            {
                resp.Msg = "商品积分须大于0";
                goto outoff;

            }

            if (!string.IsNullOrEmpty(discountScore))
            {
                if (!int.TryParse(discountScore, out discountScoreInt))
                {
                    resp.Msg = "打折积分需是整数";
                    goto outoff;
                }
                else
                {
                    if (discountScoreInt < 0)
                    {
                        resp.Msg = "打折积分需大于0";
                        goto outoff;
                    }
                    model.DiscountScore = discountScoreInt;
                }


            }
            if (!string.IsNullOrEmpty(stock))
            {
                if (!int.TryParse(stock, out stockInt))
                {
                    resp.Msg = "库存不正确";
                    goto outoff;

                }
            }
            model.AutoID = int.Parse(autoId);
            model.PName = pName;
            model.Score = scoreD;
            //model.UserID = userInfo.UserID;
            model.PDescription = pDescription;
            model.RecommendImg = recommendImg;
            //model.InsertDate = DateTime.Now;
            model.IsOnSale = isOnSale;
            model.Stock = stockInt;
            //model.StoreId = StoreId;
            model.ScoreLine = onLine;
            if (!string.IsNullOrEmpty(typeId))
            {
                model.TypeId = Convert.ToInt32(typeId);

            }
            model.ShowImage1 = context.Request["ShowImage1"];
            model.ShowImage2 = context.Request["ShowImage2"];
            model.ShowImage3 = context.Request["ShowImage3"];
            model.ShowImage4 = context.Request["ShowImage4"];
            model.ShowImage5 = context.Request["ShowImage5"];
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = int.Parse(sort);
            }
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";


            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 更新积分商品排序号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateScoreProductSortIndex(HttpContext context)
        {
            string id = context.Request["id"];
            string sortIndex = context.Request["SortIndex"];
            int count = this.bllMall.Update(new WXMallScoreProductInfo(), string.Format("Sort={0}", sortIndex), string.Format(" AutoID ={0}", id));
            if (count.Equals(1))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        #endregion

        #region 一般订单管理
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallOrderInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string orderId = context.Request["OrderID"];
            string fromDate = context.Request["FromDate"];
            string toDate = context.Request["ToDate"];
            string orderStatus = context.Request["OrderStatus"];
            //string StoreId = context.Request["StoreId"];
            string categoryId = context.Request["CategoryId"];
            string paymentStatus = context.Request["PaymentStatus"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' ", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(orderId))
            {
                sbWhere.AppendFormat("And (OrderID='{0}' Or OrderUserID like'%{0}%' Or Consignee like'%{0}%' )", orderId);
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                sbWhere.AppendFormat("And Status in({0})", orderStatus);
            }
            //if (!string.IsNullOrEmpty(StoreId))
            //{
            //    sbWhere.AppendFormat("And WxMallStoreId = '{0}'", StoreId);
            //}
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                sbWhere.AppendFormat("And PaymentStatus = {0}", paymentStatus);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat("And CategoryId= '{0}'", categoryId);
            }
            if ((!string.IsNullOrEmpty(fromDate)) && (string.IsNullOrEmpty(toDate)))//大于开始时间
            {
                sbWhere.AppendFormat("And InsertDate>='{0}'", Convert.ToDateTime(fromDate));
            }
            if ((string.IsNullOrEmpty(fromDate)) && (!string.IsNullOrEmpty(toDate)))//小于结束时间
            {
                sbWhere.AppendFormat("And InsertDate<='{0}'", Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59));
            }
            if ((!string.IsNullOrEmpty(fromDate)) && (!string.IsNullOrEmpty(toDate)))//大于开始时间小于结束时间
            {
                sbWhere.AppendFormat("And InsertDate>='{0}' And  InsertDate<='{1}'", Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59));
            }

            int totalCount = bllMall.GetCount<WXMallOrderInfo>(sbWhere.ToString());
            List<WXMallOrderInfo> dataList = new List<WXMallOrderInfo>();
            dataList = bllMall.GetLit<WXMallOrderInfo>(pageSize, pageIndex, sbWhere.ToString(), "OrderID DESC");
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });

        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallOrderInfo(HttpContext context)
        {
            resp.Msg = "暂时不支持删除订单!";
            return Common.JSONHelper.ObjectToJson(resp);
            //string orderId = context.Request["oid"];
            //if (bllMall.GetCount<WXMallOrderInfo>(string.Format("OrderID in ({0}) And PaymentStatus=1", orderId)) > 0)
            //{
            //    resp.Msg = "已经付款的订单不能删除!";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            //int count = bllMall.Delete(new WXMallOrderInfo(), string.Format("OrderID in ({0}) And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
            //if (count > 0)
            //{
            //    bllMall.Delete(new WXMallOrderDetailsInfo(), string.Format("OrderID in ({0})", orderId));
            //    resp.Msg = "成功删除了" + count.ToString() + "个订单";
            //    resp.Status = 1;
            //}
            //return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 更新订单状态 单条记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateOrderStatus(HttpContext context)
        {

            string orderId = context.Request["OrderID"];
            string expressCompanyCode = context.Request["ExpressCompanyCode"];//快递公司代码
            string expressCompanyName = context.Request["ExpressCompanyName"];//快递公司名称
            string expressNumber = context.Request["ExpressNumber"];//运单号码
            string status = context.Request["Status"];
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.Msg = "无效的订单";
                goto outoff;
            }
            int updateCount = bllMall.Update(new WXMallOrderInfo(), string.Format("Status='{0}',ExpressCompanyCode='{1}',ExpressCompanyName='{2}',ExpressNumber='{3}'", status, expressCompanyCode, expressCompanyName, expressNumber), string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
            if (updateCount > 0)
            {
                orderInfo.Status = status;
                resp.Status = 1;
                try
                {

                    #region 交易成功加积分 1元=1积分
                    if (status.Contains("交易完成") || status.Contains("交易成功"))
                    {
                        //更新会员积分记录积分
                        int addScore = (int)Math.Ceiling(orderInfo.TotalAmount);//原始获得的积分
                        //if (orderInfo.DeliveryTime != null)
                        //{
                        //TimeSpan timeSpan = (Convert.ToDateTime(orderInfo.DeliveryTime)) - Convert.ToDateTime(orderInfo.InsertDate);
                        //if (timeSpan.Days >= 1)
                        //{
                        //    AddScore *= 2;
                        //}

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
                        //检查是否有积分奖励
                        //var ScoreConfig = new BllScore().GetScoreConfig();
                        //if (ScoreConfig != null)
                        //{
                        //    if (ScoreConfig.OrderDate != null && ScoreConfig.OrderDateTotalAmount != null && ScoreConfig.OrderScore != null)
                        //    {
                        //        if ((Convert.ToDateTime(ScoreConfig.OrderDate) - DateTime.Now).Days.Equals(0))
                        //        {
                        //            if (orderInfo.TotalAmount >= ScoreConfig.OrderDateTotalAmount)
                        //            {
                        //                //插入积分记录
                        //                WXMallScoreRecord JiangLiScoreRecord = new WXMallScoreRecord();
                        //                JiangLiScoreRecord.InsertDate = DateTime.Now;
                        //                JiangLiScoreRecord.Remark = "积分额外奖励";
                        //                JiangLiScoreRecord.Score = (int)ScoreConfig.OrderScore;
                        //                JiangLiScoreRecord.UserId = orderInfo.OrderUserID;
                        //                JiangLiScoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                        //                JiangLiScoreRecord.OrderID = orderInfo.OrderID;
                        //                JiangLiScoreRecord.Type = 1;
                        //                bllMall.Add(JiangLiScoreRecord);
                        //                bllMall.AddUserTotalScore(orderInfo.OrderUserID, JiangLiScoreRecord.Score);
                        //            }
                        //        }
                        //    }
                        //}

                        //


                        //}


                    }


                    #endregion

                    #region 发送通知消息
                    System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
                    WXMallOrderStatusInfo statusInfo = bllMall.GetOrderStatus(status);
                    UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);//订单用户信息
                    if (statusInfo != null)
                    {
                        if (statusInfo.OrderMessage.Contains("$ORDERID$"))
                        {
                            statusInfo.OrderMessage = statusInfo.OrderMessage.Replace("$ORDERID$", orderInfo.OrderID);
                        }
                        sbMessage.AppendFormat(statusInfo.OrderMessage);
                        //if (Status.Equals("已配送"))
                        //{
                        //    WXMallDeliveryStaff StaffInfo = bllMall.GetSingleWXMallDeliveryStaff(int.Parse(DeliverStaffId));
                        //    Message.Clear();
                        //    Message.AppendFormat("您的订单 {0} 已经开始配送\n[{1}]\n", orderInfo.OrderID, context.Request["Status"]);
                        //    Message.AppendFormat("配送员: {0} \n", StaffInfo.StaffName);
                        //    Message.AppendFormat("手机号:{0} \n", StaffInfo.StaffPhone);
                        //    orderInfo.DeliveryStaff = StaffInfo.StaffName;
                        //    bllMall.Update(orderInfo);

                        //}

                    }
                    else
                    {
                        sbMessage.AppendFormat("您的订单 {0} 状态已修改为\n[{1}]", orderInfo.OrderID, status);
                    }
                    string accessToken = bllWeixin.GetAccessToken();
                    if (!string.IsNullOrEmpty(accessToken))
                    {

                        if (orderUserInfo != null)
                        {
                            bllWeixin.SendKeFuMessageText(accessToken, orderUserInfo.WXOpenId, sbMessage.ToString());
                        }
                    }
                    #endregion

                    #region 更新分销订单状态
                    if (currentWebSiteInfo.IsDistributionMall.Equals(1))
                    {
                        if (orderInfo.Status.Equals("已付款") || orderInfo.Status.Equals("已发货"))
                        {
                            if (orderInfo.DistributionStatus.Equals(0))
                            {
                                //更新分销订单状态为已经付款
                                int count = bllMall.Update(new WXMallOrderInfo(), "DistributionStatus=1", string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
                                if (count == 0)
                                {
                                    resp.Msg = "更新分销订单为已付款失败";
                                    goto outoff;
                                }

                            }

                        }
                    }
                    #endregion

                    //#region 驿氪同步
                    //if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                    //{

                    //    if ((!string.IsNullOrEmpty(orderUserInfo.Ex1)) && (!string.IsNullOrEmpty(orderUserInfo.Ex2)) && (!string.IsNullOrEmpty(orderUserInfo.Phone)))
                    //    {

                    //        //驿氪同步
                    //        Open.EZRproSDK.Client client = new Open.EZRproSDK.Client();
                    //        client.ChangeStatus(orderInfo.OrderID, status);
                    //        //驿氪同步
                    //    }


                    //}
                    //#endregion



                }
                catch
                {


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
        /// 更新订单状态 批量修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateOrderStatusBatch(HttpContext context)
        {

            foreach (var orderId in context.Request["orderids"].Split(','))
            {
                string status = context.Request["Status"];
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
                int updateCount = bllMall.Update(new WXMallOrderInfo(), string.Format("Status='{0}'", status), string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
                if (updateCount > 0)
                {
                    orderInfo.Status = status;
                    resp.Status = 1;
                    //try
                    //{

                    #region 交易成功加积分
                    if (status.Contains("交易完成") || status.Contains("交易成功"))
                    {
                        //更新会员积分记录积分
                        int addScore = (int)Math.Ceiling(orderInfo.TotalAmount);//原始获得的积分
                        //if (OrderInfo.DeliveryTime != null)
                        //{
                        //TimeSpan timeSpan = (Convert.ToDateTime(OrderInfo.DeliveryTime)) - Convert.ToDateTime(OrderInfo.InsertDate);
                        //if (timeSpan.Days >= 1)
                        //{
                        //    AddScore *= 2;
                        //}

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


                        ////检查是否有积分奖励
                        //var ScoreConfig = new BllScore().GetScoreConfig();
                        //if (ScoreConfig != null)
                        //{
                        //    if (ScoreConfig.OrderDate != null && ScoreConfig.OrderDateTotalAmount != null && ScoreConfig.OrderScore != null)
                        //    {
                        //        if ((Convert.ToDateTime(ScoreConfig.OrderDate) - DateTime.Now).Days.Equals(0))
                        //        {
                        //            if (OrderInfo.TotalAmount >= ScoreConfig.OrderDateTotalAmount)
                        //            {
                        //                //插入积分记录
                        //                WXMallScoreRecord JiangLiScoreRecord = new WXMallScoreRecord();
                        //                JiangLiScoreRecord.InsertDate = DateTime.Now;
                        //                JiangLiScoreRecord.Remark = "积分额外奖励";
                        //                JiangLiScoreRecord.Score = (int)ScoreConfig.OrderScore;
                        //                JiangLiScoreRecord.UserId = OrderInfo.OrderUserID;
                        //                JiangLiScoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                        //                JiangLiScoreRecord.OrderID = OrderInfo.OrderID;
                        //                JiangLiScoreRecord.Type = 1;
                        //                bllMall.Add(JiangLiScoreRecord);
                        //                bllMall.AddUserTotalScore(OrderInfo.OrderUserID, JiangLiScoreRecord.Score);

                        //            }
                        //        }
                        //    }
                        //}

                        //


                    }




                    //}
                    #endregion

                    #region 发送通知消息
                    System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
                    WXMallOrderStatusInfo statusInfo = bllMall.GetOrderStatus(status);
                    if (statusInfo != null)
                    {
                        if (statusInfo.OrderMessage.Contains("$ORDERID$"))
                        {
                            statusInfo.OrderMessage = statusInfo.OrderMessage.Replace("$ORDERID$", orderInfo.OrderID);
                        }
                        sbMessage.AppendFormat(statusInfo.OrderMessage);
                        //if (Status.Equals("已配送"))
                        //{
                        //    WXMallDeliveryStaff StaffInfo = bllMall.GetSingleWXMallDeliveryStaff(int.Parse(DeliverStaffId));
                        //    Message.Clear();
                        //    Message.AppendFormat("您的订单 {0} 已经开始配送\n[{1}]\n", orderInfo.OrderID, context.Request["Status"]);
                        //    Message.AppendFormat("配送员: {0} \n", StaffInfo.StaffName);
                        //    Message.AppendFormat("手机号:{0} \n", StaffInfo.StaffPhone);
                        //    orderInfo.DeliveryStaff = StaffInfo.StaffName;
                        //    bllMall.Update(orderInfo);

                        //}

                    }
                    else
                    {
                        sbMessage.AppendFormat("您的订单 {0} 状态已修改为\n[{1}]", orderInfo.OrderID, status);
                    }
                    string accessToken = bllWeixin.GetAccessToken(currWebSiteUserInfo.UserID);
                    if (accessToken != string.Empty)
                    {
                        bllWeixin.SendKeFuMessageText(accessToken, bllUser.GetUserInfo(orderInfo.OrderUserID).WXOpenId, sbMessage.ToString());
                    }
                    #endregion

                    #region 分销订单状态
                    if (currentWebSiteInfo.IsDistributionMall.Equals(1))
                    {
                        if (orderInfo.Status.Equals("已付款") || orderInfo.Status.Equals("已发货"))
                        {
                            if (orderInfo.DistributionStatus.Equals(0))
                            {
                                //更新分销订单状态为已经付款
                                int count = bllMall.Update(new WXMallOrderInfo(), "DistributionStatus=1", string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
                                if (count == 0)
                                {
                                    resp.Msg = "更新分销订单为已付款失败";
                                    goto outoff;
                                }

                            }

                        }
                    }
                    #endregion

                    //}
                    //catch
                    //{


                    //}

                }
                else
                {
                    resp.Status = 0;
                    goto outoff;
                }
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        /// 更新分销订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDistributionOrderStatus(HttpContext context)
        {


            string orderId = context.Request["OrderID"];
            int status = int.Parse(context.Request["Status"]);
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo.DistributionStatus.Equals(status))
            {
                resp.Msg = "订单状态与原订单状态不能相同";
                goto outoff;
            }
            int updateCount = bllMall.Update(new WXMallOrderInfo(), string.Format(" DistributionStatus={0}", status), string.Format(" OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
            if (updateCount > 0)
            {

                if (status.Equals(3))//已审核 ,给上级用户账户打款
                {

                    //if (!bllDis.UpdateDistributionOrderComplete(orderInfo))
                    //{
                    //    bllMall.Update(orderInfo);//订单状态还原
                    //    resp.Msg = "给上级用户充值失败";
                    goto outoff;
                    //}
                    //else
                    //{
                    //    resp.Status = 1;

                    //}

                }


                resp.Status = 1;
            }
            else
            {

                resp.Msg = "更新分销订单状态失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        ///// <summary>
        ///// 更新分销订单状态 批量修改
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string UpdateDistributionOrderStatusBatch(HttpContext context)
        //{
        //    int status = int.Parse(context.Request["Status"]);
        //    foreach (var orderId in context.Request["orderids"].Split(','))
        //    {
        //        WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
        //        if (orderInfo.DistributionStatus.Equals(status))
        //        {
        //            resp.Msg = "订单状态与原订单状态不能相同";
        //            goto outoff;
        //        }
        //    }
        //    foreach (var orderId in context.Request["orderids"].Split(','))
        //    {
        //        WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
        //        int updateResultCount = bllMall.Update(new WXMallOrderInfo(), string.Format(" DistributionStatus={0}", status), string.Format(" OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
        //        if (updateResultCount > 0)
        //        {
        //            if (status.Equals(3))//已审核 ,给上级用户账户打款
        //            {

        //                if (!bllDis.UpdateDistributionOrderComplete(orderInfo))
        //                {
        //                    bllMall.Update(orderInfo);//订单状态还原
        //                    resp.Msg = "给上级用户充值失败";
        //                    goto outoff;
        //                }
        //                else
        //                {
        //                    resp.Status = 1;

        //                }
        //            }


        //            resp.Status = 1;
        //        }
        //        else
        //        {

        //            resp.Msg = "更新订单状态失败";
        //        }
        //    }
        //outoff:
        //    return Common.JSONHelper.ObjectToJson(resp);

        //}


        /// <summary>
        /// 获取下级分销用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetChildDistribution(HttpContext context)
        {
            string userId = context.Request["id"];
            string is_member = context.Request["is_member"];
            string hide_order = context.Request["hide_order"];

            //List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format("DistributionOwner='{0}'", id));
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat(" Select AutoID,UserID,TrueName,WXNickname,WXHeadimgurl ,DistributionDownUserCountLevel1, ");
            sbSQL.AppendFormat(" DistributionDownUserCountLevel2,DistributionDownUserCountLevel3,DistributionSaleAmountLevel1, ");
            sbSQL.AppendFormat(" DistributionSaleAmountLevel2,DistributionSaleAmountLevel3,HistoryDistributionOnLineTotalAmount, ");
            sbSQL.AppendFormat(" TotalAmount,FrozenAmount from ZCJ_UserInfo Where ");
            sbSQL.AppendFormat(" WebsiteOwner='{0}' ", bllUser.WebsiteOwner);
            sbSQL.AppendFormat(" And DistributionOwner='{0}' ", userId, bllUser.WebsiteOwner);
            if (is_member == "1") sbSQL.AppendFormat(" And MemberLevel>0 ");

            List<UserInfo> userList = ZentCloud.ZCBLLEngine.BLLBase.Query<UserInfo>(string.Format("Select AutoID,UserID,TrueName,WXNickname,WXHeadimgurl ,DistributionDownUserCountLevel1,DistributionDownUserCountLevel2,DistributionDownUserCountLevel3,DistributionSaleAmountLevel1,DistributionSaleAmountLevel2,DistributionSaleAmountLevel3,HistoryDistributionOnLineTotalAmount,TotalAmount,FrozenAmount from ZCJ_UserInfo Where WebsiteOwner='{0}' And DistributionOwner='{1}'", bllUser.WebsiteOwner, userId));
            if (userList.Count > 0)
            {

                System.Text.StringBuilder sbJson = new System.Text.StringBuilder();
                sbJson.Append("[");
                for (int i = 0; i < userList.Count; i++)
                {
                    var item = userList[i];
                    var isParent = false;
                    if (bllUser.GetCount<UserInfo>(string.Format("DistributionOwner='{0}'", item.UserID)) > 0)
                    {
                        isParent = true;
                    }
                    string showName = item.UserID;
                    string headImg = "/Plugins/zTree/css/zTreeStyle/img/diy/user.png";
                    string icon = "/Plugins/zTree/css/zTreeStyle/img/diy/user.png";

                    if (!string.IsNullOrEmpty(item.WXNickname))
                    {
                        showName = item.WXNickname.Replace("\"", "").Replace("'", "");
                    }
                    if (!string.IsNullOrEmpty(item.TrueName))
                    {
                        showName = item.TrueName;
                    }
                    if (!string.IsNullOrEmpty(item.WXHeadimgurl))
                    {
                        headImg = item.WXHeadimgurl;
                    }
                    string tip = string.Format("<img src='{0}' align='absmiddle' width='100px' height='100px'/><br/>{1}<br/>会员<span style='color:red;'>&nbsp;{2}</span><br/>销售额<span style='color:red;'>&nbsp;{5}</span><br/>累计佣金 <span style='color:red;'>&nbsp;{8}</span><br/>可提现佣金<span style='color:red;'>&nbsp;{9}</span>", headImg, showName, item.DistributionDownUserCountLevel1, item.DistributionDownUserCountLevel2, item.DistributionDownUserCountLevel3, item.DistributionSaleAmountLevel1 + item.DistributionSaleAmountLevel0, item.DistributionSaleAmountLevel2, item.DistributionSaleAmountLevel3, item.HistoryDistributionOnLineTotalAmount, bllDis.GetUserCanUseAmount(item));

                    var title = string.Format("<span style='color:blue;'>{0}</span>", showName);
                    if (hide_order != "1") title += string.Format("&nbsp;<a href='DistributionOrder.aspx?uid={0}' target='_blank'>查看销售订单</a>", item.AutoID);
                    sbJson.Append("{");
                    sbJson.AppendFormat("name: \"{0}\", id: \"{1}\", count:{2}, times: 1, isParent:\"{3}\",open:false,icon:\"{4}\",tip:\"{5}\"", title, item.UserID, "1", isParent.ToString().ToLower(), icon, tip);
                    sbJson.Append("}");

                    if (i < userList.Count - 1)//追加分隔符
                    {
                        sbJson.Append(",");
                    }

                }
                sbJson.Append("]");

                return sbJson.ToString();
            }
            else
            {
                return "";
            }



        }



        /// <summary>
        /// 获取分销订单
        /// </summary>
        /// <returns></returns>
        private string QueryDistributionOrder(HttpContext context)
        {
            if ((!currentUserInfo.UserType.Equals(1)) && (!currentUserInfo.UserID.Equals(bllUser.WebsiteOwner)) && (currentUserInfo.IsSubAccount != "1"))
            {
                return null;
            }
            int pageIndex = int.Parse(context.Request["PageIndex"]);//第几页
            int pageSize = int.Parse(context.Request["PageSize"]);//每页记录数
            int level = int.Parse(context.Request["Level"]);
            int uid = int.Parse(context.Request["Uid"]);

            UserInfo targetUserInfo = bllUser.GetUserInfoByAutoID(uid);
            List<WXMallOrderInfo> sourceData = new List<WXMallOrderInfo>();
            sourceData.AddRange(bllDis.GetOrderList(targetUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.NotPay));
            sourceData.AddRange(bllDis.GetOrderList(targetUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Paied));
            sourceData.AddRange(bllDis.GetOrderList(targetUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Received));
            sourceData.AddRange(bllDis.GetOrderList(targetUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Verified));
            sourceData.AddRange(bllDis.GetOrderList(targetUserInfo.UserID, level, BLLJIMP.BLLDistribution.DistributionStatus.Withdraw));
            if (level == 0)
            {
                sourceData = sourceData.Where(p => p.OrderUserID == targetUserInfo.UserID).ToList();

            }
            else
            {
                sourceData = sourceData.Where(p => p.OrderUserID != targetUserInfo.UserID).ToList();

            }
            WebsiteInfo websiteInfo = bllBase.GetWebsiteInfoModelFromDataBase();
            sourceData = sourceData.OrderByDescending(p => p.InsertDate).ToList();
            List<DistributionOnLineOrder> orderList = new List<DistributionOnLineOrder>();
            var data = sourceData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (WXMallOrderInfo item in data)
            {
                DistributionOnLineOrder order = new DistributionOnLineOrder();

                #region 分销提成比例
                #region 已分佣 提成比例及金额根据提成记录
                if (item.DistributionStatus == 3)//已打佣金,分佣金额根据记录表去查
                {

                    ProjectCommission projectCommission = bllDis.Get<ProjectCommission>(string.Format(" ProjectId='{0}' And UserId='{1}' And WebsiteOwner='{2}' And ProjectType='DistributionOnLine'", item.OrderID, targetUserInfo.UserID, bllDis.WebsiteOwner));
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
                        //UserLevelConfig userLevel = bllDis.GetUserLevel(targetUserInfo);
                        // bool isFirst = bllDis.IsFirstOrder(item);
                        switch (level)
                        {
                            case 0:
                                //if (isFirst)
                                //{
                                //    order.DistributionAmount = (decimal.Parse(userLevel.DistributionRateLevel0First) / 100) * (item.TotalAmount - item.Transport_Fee);
                                //    order.DistributionRate = userLevel.DistributionRateLevel0First;

                                //}
                                //else
                                //{
                                //    order.DistributionAmount = (decimal.Parse(userLevel.DistributionRateLevel0) / 100) * (item.TotalAmount - item.Transport_Fee);
                                //    order.DistributionRate = userLevel.DistributionRateLevel0;


                                //}
                                //isFirst = bllDis.IsFirstOrder(targetUserInfo.UserID);
                                //decimal rate = (decimal)bllDis.GetDistributionRate(targetUserInfo, 0, isFirst);//直销提成比例
                                //order.DistributionAmount = bllDis.GetUserCommission(item, targetUserInfo, 0);
                                //order.DistributionRate = rate.ToString();

                                ProjectCommissionEstimate esti = bllDis.Get<ProjectCommissionEstimate>(string.Format("ProjectId={0} And ProjectType='DistributionOnLine' And UserId='{1}' ", item.OrderID, targetUserInfo.UserID));
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
                                //if (isFirst)
                                //{
                                //    order.DistributionAmount = (decimal.Parse(userLevel.DistributionRateLevel1First) / 100) * (item.TotalAmount - item.Transport_Fee);
                                //    order.DistributionRate = userLevel.DistributionRateLevel1First;

                                //}
                                //else
                                //{
                                //    order.DistributionAmount = (decimal.Parse(userLevel.DistributionRateLevel1) / 100) * (item.TotalAmount - item.Transport_Fee);
                                //    order.DistributionRate = userLevel.DistributionRateLevel1;

                                //}
                                var upUserLevel1 = bllDis.GetUpUser(targetUserInfo.UserID, 1);
                                //decimal rateLevel1 = (decimal)bllDis.GetDistributionRate(upUserLevel1, 1, isFirst);
                                //order.DistributionAmount = bllDis.GetUserCommission(item, targetUserInfo, 1);
                                //order.DistributionRate = rateLevel1.ToString();

                                ProjectCommissionEstimate estiUpLevel = bllDis.Get<ProjectCommissionEstimate>(string.Format("ProjectId={0} And ProjectType='DistributionOnLine' And UserId='{1}' ", item.OrderID, upUserLevel1.UserID));
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
                        //if (order.DistributionAmount < 0)
                        //{
                        //    order.DistributionAmount = 0;
                        //}




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
                UserInfo orderUserInfo = bllUser.GetUserInfo(item.OrderUserID);
                if (orderUserInfo != null)
                {
                    order.TrueName = orderUserInfo.TrueName;
                    order.Phone = orderUserInfo.Phone;
                    order.AutoID = orderUserInfo.AutoID;
                    order.WXNickName = orderUserInfo.WXNickname;
                }
                order.ProductList = new List<WXMallProductInfo>();
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
        ///同步efast订单
        /// </summary>
        /// <returns></returns>
        private string EfastSynWXMallOrderInfo(HttpContext context)
        {
            string orderId = context.Request["orderId"];
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "订单号不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (orderInfo.PaymentStatus == 0)
            {
                resp.Status = 0;
                resp.Msg = "订单未付款";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(orderInfo.OutOrderId))
            {
                resp.Status = 0;
                resp.Msg = "订单已经同步";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            BLLJIMP.BLLEfast bllEfast = new BLLJIMP.BLLEfast();
            string outOrderId = string.Empty, msg = string.Empty;
            var syncResult = bllEfast.CreateOrder(orderInfo.OrderID, out outOrderId, out msg);
            if (syncResult)
            {
                orderInfo.OutOrderId = outOrderId;
                bllMall.Update(orderInfo);
                resp.Status = 1;
                resp.Msg = "成功同步,外部订单号" + outOrderId;

            }
            else
            {
                resp.Status = 1;
                resp.Msg = msg;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 分销订单模型
        /// </summary>
        public class DistributionOnLineOrder
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

        ///// <summary>
        ///// 一般订单提醒
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string GetOrderRemindByTime(HttpContext context)
        //{

        //    DateTime dtNow = DateTime.Now;
        //    var data = bllMall.GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And InsertDate >='{1}' And InsertDate<='{2}' And Status='等待处理'", bllBase.WebsiteOwner, dtNow.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm"), dtNow.AddMinutes(1).ToString("yyyy-MM-dd HH:mm")));
        //    if (data.Count > 0)
        //    {
        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            data[i].Address = null;
        //            data[i].Consignee = null;
        //            data[i].DeliveryStaff = null;
        //            data[i].DeliveryTime = null;
        //            data[i].OrderMemo = null;
        //            data[i].OrderUserID = null;
        //            data[i].Phone = null;
        //            data[i].Status = null;
        //            data[i].WebsiteOwner = null;
        //            data[i].WxMallStoreId = null;

        //        }
        //        resp.ExInt = 1;
        //        resp.ExObj = data;
        //    }
        //    else
        //    {
        //        resp.ExInt = 0;
        //    }

        //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        //}




        ///// <summary>
        ///// 导出订单
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public void ExportOrder(HttpContext context) {

        //     string orderids=context.Request["oids"];
        //     BLLMall bllMall=new BLLMall();
        //     StringBuilder sb=new StringBuilder();
        //     List<WXMallOrderInfo> orderList = bllMall.GetList<WXMallOrderInfo>(string.Format("OrderID in({0})", orderids));
        //     foreach (var order in orderList)
        //     {
        //         switch (order.DeliveryId)
        //         {
        //             case "1":
        //                 sb.Append("订单编号\t");
        //                 sb.Append("商品总数量\t");
        //                 sb.Append("商品总金额\t");
        //                 sb.Append("下单时间\t");
        //                 sb.Append("订单状态\t");
        //                 sb.Append("配送方式\t");
        //                 sb.Append("门店名称:\t");
        //                 sb.Append("门店地址:\t");
        //                 sb.Append("提货人姓名:\t");
        //                 sb.Append("电话:\t");
        //                 sb.Append("备注:\t");
        //                 sb.Append("\n");
        //                 sb.Append(order.OrderID+"\t");
        //                 sb.Append(order.ProductCount + "\t");
        //                 sb.Append(order.TotalAmount + "\t");
        //                 sb.Append(order.InsertDate.ToString() + "\t");
        //                 sb.Append(order.Status + "\t");
        //                 sb.Append("门店自提\t");
        //                 WXMallStores store = bllMall.GetWXMallStoreById(order.WxMallStoreId);
        //                 sb.Append(store.StoreName == null ?"": store.StoreName + "\t");
        //                 sb.Append(store.StoreAddress == null ? "" : store.StoreAddress + "\t");
        //                 sb.Append(order.Consignee + "\t");
        //                 sb.Append(order.Phone + "\t");
        //                 sb.Append(order.OrderMemo + "\t");
        //                 sb.Append("\n");




        //                 break;
        //             case "2":
        //                 sb.Append("订单编号\t");
        //                 sb.Append("商品总数量\t");
        //                 sb.Append("商品总金额\t");
        //                 sb.Append("下单时间\t");
        //                 sb.Append("订单状态\t");
        //                 sb.Append("配送方式\t");
        //                 sb.Append("收货人姓名:\t");
        //                 sb.Append("电话:\t");
        //                 sb.Append("收货地址:\t");
        //                 sb.Append("备注:\t");
        //                 sb.Append("\n");
        //                 sb.Append(order.OrderID+"\t");
        //                 sb.Append(order.ProductCount + "\t");
        //                 sb.Append(order.TotalAmount + "\t");
        //                 sb.Append(order.InsertDate.ToString() + "\t");
        //                 sb.Append(order.Status + "\t");
        //                 sb.Append("快递\t");
        //                 sb.Append(order.Consignee + "\t");
        //                 sb.Append(order.Phone + "\t");
        //                 sb.Append(order.Address + "\t");
        //                 sb.Append(order.OrderMemo + "\t");
        //                 sb.Append("\n");


        //                 break;
        //             case "3":
        //                 break;
        //             default:
        //                 break;




        //         }
        //         sb.Append("\n");
        //         sb.Append("商品清单\t");
        //         sb.Append("\n");
        //         sb.Append("订单编号\t");
        //         sb.Append("商品编号\t");
        //         sb.Append("商品名称\t");
        //         sb.Append("商品单价\t");
        //         sb.Append("商品数量\t");
        //         //sb.Append("金额\t");
        //         sb.Append("\n");

        //         foreach (var item in bllMall.GetOrderDetailsList(order.OrderID))
        //         {
        //             WXMallProductInfo product = bllMall.GetWXMallProductByProductId(item.PID);
        //             sb.Append(order.OrderID+"\t");
        //             sb.Append(product.PID == null ? "" : product.PID + "\t");
        //             sb.Append(product.PName == null ? "" : product.PName + "\t");
        //             sb.Append(item.OrderPrice+"\t");
        //             sb.Append(item.TotalCount + "\t");
        //             //sb.Append((item.TotalCount * item.OrderPrice) + "\t");
        //             sb.Append("\n");

        //         }
        //         sb.Append("\n");
        //         sb.Append("\t\t\t");
        //         sb.Append("商品总数量:\t");
        //         sb.Append(order.ProductCount);
        //         sb.Append("\n");
        //         sb.Append("\t\t\t");
        //         sb.Append("总计金额:\t");
        //         sb.Append(order.TotalAmount);
        //         sb.Append("\n");

        //         sb.Append("\n");
        //         sb.Append("\n");
        //         sb.Append("\n");
        //         sb.Append("\n");
        //         sb.Append("\n");


        //     }

        //     HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        //     HttpContext.Current.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename=订单_{0}.xls",DateTime.Now.ToString()));
        //     HttpContext.Current.Response.Write(sb);
        //     HttpContext.Current.Response.End();




        //}


        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void ExportOrder(HttpContext context)
        {

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner= '{0}'", bllBase.WebsiteOwner);
            string fromDate = context.Request["from_date"];//开始日期
            string toDate = context.Request["to_date"];//结束日期
            string status = context.Request["status"];//订单状态
            string orderIds = context.Request["oids"];//订单号
            if (!string.IsNullOrEmpty(fromDate))
            {
                sbWhere.AppendFormat(" And InsertDate>='{0}'", fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                sbWhere.AppendFormat(" And InsertDate<='{0}'", toDate);
            }
            if (!string.IsNullOrEmpty(status))
            {
                status = "'" + status.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And Status in({0})", status);

            }
            if (!string.IsNullOrEmpty(orderIds))
            {
                sbWhere.AppendFormat(" And OrderID in({0})  ", orderIds);

            }

            List<WXMallOrderInfo> orderList = bllMall.GetList<WXMallOrderInfo>(sbWhere.ToString());
            sbWhere.Append("订单编号\t");
            sbWhere.Append("商品编号\t");
            sbWhere.Append("商品名称\t");
            sbWhere.Append("商品单价\t");
            sbWhere.Append("商品数量\t");
            sbWhere.Append("金额\t");
            sbWhere.Append("门店名称\t");
            sbWhere.Append("收货人姓名\t");
            sbWhere.Append("电话\t");
            sbWhere.Append("地址\t");
            sbWhere.Append("\n");
            foreach (var order in orderList)
            {
                #region OLD
                //switch (order.DeliveryId)
                //{
                //    case "1":
                //        sb.Append("订单编号\t");
                //        sb.Append("商品总数量\t");
                //        sb.Append("商品总金额\t");
                //        sb.Append("下单时间\t");
                //        sb.Append("订单状态\t");
                //        sb.Append("配送方式\t");
                //        sb.Append("门店名称:\t");
                //        sb.Append("门店地址:\t");
                //        sb.Append("提货人姓名:\t");
                //        sb.Append("电话:\t");
                //        sb.Append("备注:\t");
                //        sb.Append("\n");
                //        sb.Append(order.OrderID + "\t");
                //        sb.Append(order.ProductCount + "\t");
                //        sb.Append(order.TotalAmount + "\t");
                //        sb.Append(order.InsertDate.ToString() + "\t");
                //        sb.Append(order.Status + "\t");
                //        sb.Append("门店自提\t");
                //        WXMallStores store = bllMall.GetWXMallStoreById(order.WxMallStoreId);
                //        sb.Append(store.StoreName == null ? "" : store.StoreName + "\t");
                //        sb.Append(store.StoreAddress == null ? "" : store.StoreAddress + "\t");
                //        sb.Append(order.Consignee + "\t");
                //        sb.Append(order.Phone + "\t");
                //        sb.Append(order.OrderMemo + "\t");
                //        sb.Append("\n");




                //        break;
                //    case "2":
                //        sb.Append("订单编号\t");
                //        sb.Append("商品总数量\t");
                //        sb.Append("商品总金额\t");
                //        sb.Append("下单时间\t");
                //        sb.Append("订单状态\t");
                //        sb.Append("配送方式\t");
                //        sb.Append("收货人姓名:\t");
                //        sb.Append("电话:\t");
                //        sb.Append("收货地址:\t");
                //        sb.Append("备注:\t");
                //        sb.Append("\n");
                //        sb.Append(order.OrderID + "\t");
                //        sb.Append(order.ProductCount + "\t");
                //        sb.Append(order.TotalAmount + "\t");
                //        sb.Append(order.InsertDate.ToString() + "\t");
                //        sb.Append(order.Status + "\t");
                //        sb.Append("快递\t");
                //        sb.Append(order.Consignee + "\t");
                //        sb.Append(order.Phone + "\t");
                //        sb.Append(order.Address + "\t");
                //        sb.Append(order.OrderMemo + "\t");
                //        sb.Append("\n");


                //        break;
                //    case "3":
                //        break;
                //    default:
                //        break;




                //}
                //sb.Append("\n");
                //sb.Append("商品清单\t");
                //sb.Append("\n"); 
                #endregion


                foreach (var item in bllMall.GetOrderDetailsList(order.OrderID))
                {
                    WXMallProductInfo product = bllMall.GetProduct(item.PID);
                    sbWhere.Append(order.OrderID + "\t");
                    sbWhere.Append(product == null ? "\t" : product.PID + "\t");
                    sbWhere.Append(product == null ? "\t" : product.PName + "\t");
                    sbWhere.Append(item.OrderPrice + "\t");
                    sbWhere.Append(item.TotalCount + "\t");
                    sbWhere.Append((item.TotalCount * item.OrderPrice) + "\t");
                    sbWhere.Append(order.Consignee + "\t");
                    sbWhere.Append(order.Phone + "\t");
                    sbWhere.Append(order.Address + "\t");
                    sbWhere.Append("\n");
                }
            }

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename=订单_{0}.xls", DateTime.Now.ToString()));
            HttpContext.Current.Response.Write(sbWhere);
            HttpContext.Current.Response.End();




        }
        #endregion


        #region 积分订单管理
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallScoreOrderInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string orderId = context.Request["OrderID"];
            string fromDate = context.Request["FromDate"];
            string toDate = context.Request["ToDate"];
            string orderStatus = context.Request["OrderStatus"];
            string storeId = context.Request["StoreId"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' And IsDelete=0 ", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(orderId))
            {
                sbWhere.AppendFormat("And OrderID='{0}'", orderId);
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                sbWhere.AppendFormat("And Status = '{0}'", orderStatus);
            }
            if ((!string.IsNullOrEmpty(fromDate)) && (string.IsNullOrEmpty(toDate)))//大于开始时间
            {
                sbWhere.AppendFormat("And InsertDate>='{0}'", Convert.ToDateTime(fromDate));
            }
            if ((string.IsNullOrEmpty(fromDate)) && (!string.IsNullOrEmpty(toDate)))//小于结束时间
            {
                sbWhere.AppendFormat("And InsertDate<'{0}'", Convert.ToDateTime(toDate).AddDays(1));
            }
            if ((!string.IsNullOrEmpty(fromDate)) && (!string.IsNullOrEmpty(toDate)))//大于开始时间小于结束时间
            {
                sbWhere.AppendFormat("And InsertDate>='{0}' And  InsertDate<'{1}'", Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate).AddDays(1));
            }
            int totalCount = bllMall.GetCount<WXMallScoreOrderInfo>(sbWhere.ToString());
            List<WXMallScoreOrderInfo> dataList = bllJuActivity.GetLit<WXMallScoreOrderInfo>(pageSize, pageIndex, sbWhere.ToString(), "OrderID DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});



        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallScoreOrderInfo(HttpContext context)
        {

            string orderId = context.Request["oid"];
            //int count= juActivityBll.Delete(new WXMallScoreOrderInfo(), string.Format("OrderID in({0}) And WebsiteOwner='{1}'", oid, websiteOwner));
            //juActivityBll.Delete(new WXMallScoreOrderDetailsInfo(), string.Format("OrderID in({0}) And WebsiteOwner='{1}'", oid, websiteOwner));
            int updateCount = bllJuActivity.Update(new WXMallScoreOrderInfo(), "IsDelete=1", string.Format("OrderID in ({0})", orderId));
            return updateCount.ToString();


        }


        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateScoreOrderStatus(HttpContext context)
        {

            string orderId = context.Request["OrderID"];
            //string DeliverStaffId = context.Request["DeliverStaffId"];//配送员ID
            string status = context.Request["Status"];
            WXMallScoreOrderInfo orderInfo = bllMall.GetScoreOrderInfo(orderId);
            UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
            int updateCount = bllMall.Update(new WXMallScoreOrderInfo(), string.Format("Status='{0}'", status), string.Format("OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllBase.WebsiteOwner));
            if (updateCount > 0)
            {
                resp.Status = 1;

                System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
                WXMallOrderStatusInfo statusInfo = bllMall.GetOrderStatus(status);
                if (statusInfo != null)
                {
                    if (statusInfo.OrderMessage.Contains("$ORDERID$"))
                    {
                        statusInfo.OrderMessage = statusInfo.OrderMessage.Replace("$ORDERID$", orderInfo.OrderID);
                    }
                    sbMessage.AppendFormat(statusInfo.OrderMessage);
                    //if (Status.Equals("已配送"))
                    //{
                    //    WXMallDeliveryStaff StaffInfo = bllMall.GetSingleWXMallDeliveryStaff(int.Parse(DeliverStaffId));
                    //    Message.Clear();
                    //    Message.AppendFormat("您的订单 {0} 已经开始配送\n[{1}]\n", OrderInfo.OrderID, context.Request["Status"]);
                    //    Message.AppendFormat("配送员: {0} \n", StaffInfo.StaffName);
                    //    Message.AppendFormat("手机号:{0} \n", StaffInfo.StaffPhone);
                    //}
                    //else
                    //{
                    //    //Message.AppendFormat("您的订单 {0} 状态已修改为\n[{1}]", orderInfo.OrderID, context.Request["Status"]);
                    //}

                }
                else
                {
                    sbMessage.AppendFormat("您的订单 {0} 状态已修改为\n[{1}]", orderInfo.OrderID, context.Request["Status"]);
                }

                string accessToken = bllWeixin.GetAccessToken();
                if (accessToken != string.Empty)
                {
                    bllWeixin.SendKeFuMessageText(accessToken, orderUserInfo.WXOpenId, sbMessage.ToString());
                }

                string first = "订单状态变更通知";//通知标题
                string remark = "点击详情进入订单中心查看";//通知备注
                if (status.Equals("已取消") && (!orderInfo.Status.Equals("已取消")))
                {
                    // //退分-加积分记录
                    // ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                    // //if (bllBase.WebsiteOwner.Equals("wubuhui"))
                    // //{

                    // //    First = "取消积分订单";
                    // //    //Remark = "取消后只返还50%积分";
                    // //    //五步会只返回50%
                    // //    OrderUserInfo.TotalScore += (double)Math.Floor(orderInfo.TotalAmount / 2);//积分返还 50%;

                    // //}
                    // //else
                    // //{
                    //     OrderUserInfo.TotalScore += (double)Math.Floor(orderInfo.TotalAmount);//积分返还 100%;

                    //// }
                    // if (bllBase.Update(OrderUserInfo, string.Format(" TotalScore={0}", OrderUserInfo.TotalScore), string.Format(" AutoID={0}", OrderUserInfo.AutoID), tran) < 1)
                    // {
                    //     tran.Rollback();
                    //     goto outoff;
                    // }




                    // WBHScoreRecord scoreRecord = new WBHScoreRecord();
                    // scoreRecord.NameStr = "取消积分订单";
                    // //if (bllBase.WebsiteOwner.Equals("wubuhui"))
                    // //{
                    // //    scoreRecord.ScoreNum = "+" + Math.Floor(orderInfo.TotalAmount / 2).ToString();

                    // //}
                    // //else
                    // //{
                    //     scoreRecord.ScoreNum = "+" + Math.Floor(orderInfo.TotalAmount).ToString();

                    // //}
                    // scoreRecord.InsertDate = DateTime.Now;
                    // scoreRecord.WebsiteOwner = bllUser.WebsiteOwner;
                    // scoreRecord.Nums = "b55";
                    // scoreRecord.UserId = OrderUserInfo.UserID;
                    // scoreRecord.RecordType = "2";
                    // if (!bllBase.Add(scoreRecord, tran))
                    // {
                    //     tran.Rollback();
                    //     goto outoff;
                    // }
                    // tran.Commit();
                    //退分-加积分记录
                    remark = "提示:取消订单不退还积分";
                    //返还库存
                    List<WXMallScoreOrderDetailsInfo> orderDetailList = bllBase.GetList<WXMallScoreOrderDetailsInfo>(string.Format("OrderID='{0}'", orderInfo.OrderID));
                    StringBuilder sbStock = new StringBuilder();
                    foreach (var item in orderDetailList)
                    {
                        sbStock.AppendFormat("update ZCJ_WXMallScoreProductInfo set Stock+={0} where  AutoID={1}", item.TotalCount, item.PID);
                    }

                    int teturnCount = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbStock.ToString());


                }
                //
                //发送微信模板消息，订单状态通知
                BLLWeixin.TMOderStatusUpdateNotification notificaiton = new BLLWeixin.TMOderStatusUpdateNotification();
                if (bllBase.WebsiteOwner.Equals("wubuhui"))
                {
                    notificaiton.Url = string.Format("http://{0}/WuBuHui/Score/Score.aspx", context.Request.Url.Host);
                }
                notificaiton.First = first;
                notificaiton.OrderSn = orderInfo.OrderID;
                notificaiton.OrderStatus = context.Request["Status"];
                notificaiton.Remark = remark;
                bllWeixin.SendTemplateMessage(accessToken, orderUserInfo.WXOpenId, notificaiton);


            }
            else
            {
                resp.Status = 0;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 更新积分订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateScoreOrderRemark(HttpContext context)
        {

            string orderId = context.Request["OrderID"];
            string remarks = context.Request["Remarks"];
            WXMallScoreOrderInfo scoreOrder = bllMall.GetScoreOrderInfo(orderId);
            scoreOrder.Remarks = remarks;
            if (bllMall.Update(scoreOrder))
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        #endregion


        #region 文本回复模块
        /// <summary>
        /// 添加
        /// </summary>
        private string AddTextReply(HttpContext context)
        {


            var keyword = context.Request["MsgKeyword"];
            var matchType = context.Request["MatchType"];
            if (!bllWeixin.CheckUserKeyword(bllBase.WebsiteOwner, keyword))
            {
                return "关键字重复";
            }
            var model = new WeixinReplyRuleInfo();
            model.MsgKeyword = keyword;
            model.MatchType = matchType;
            model.ReplyContent = context.Request["ReplyContent"];
            model.ReceiveType = "text";
            model.ReplyType = "text";
            model.CreateDate = DateTime.Now;
            model.RuleType = 1;
            model.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
            model.UserID = bllBase.WebsiteOwner;
            return bllWeixin.Add(model).ToString().ToLower();




        }


        /// <summary>
        /// 修改
        /// </summary>
        private string EditTextReply(HttpContext context)
        {
            var uid = context.Request["UID"];
            var keyword = context.Request["MsgKeyword"];
            var matchType = context.Request["MatchType"];
            var type = context.Request["type"];
            var oldInfo = bllWeixin.Get<WeixinReplyRuleInfo>(string.Format("UID={0}", uid));
            if (oldInfo.MsgKeyword != keyword)//对比关键字已经改变
            {
                //关键字改变,检查关键字是否重复
                if (!bllWeixin.CheckUserKeyword(bllMall.WebsiteOwner, keyword))
                {
                    return "关键字重复";
                }

            }

            var model = new WeixinReplyRuleInfo();
            model.UID = uid;
            model.MsgKeyword = keyword;
            model.MatchType = matchType;
            model.ReplyContent = context.Request["ReplyContent"];
            model.ReceiveType = type;
            model.ReplyType = type;
            model.CreateDate = DateTime.Now;
            model.RuleType = 1;
            model.UserID = bllUser.WebsiteOwner;
            return bllWeixin.Update(model).ToString().ToLower();
        }


        /// <summary>
        /// 删除
        /// </summary>
        private string DeleteTextReply(HttpContext context)
        {


            string Ids = context.Request["id"];
            bllWeixin.Delete(new WeixinReplyRuleInfo(), string.Format("UID in({0}) ", Ids));
            return "true";




        }


        /// <summary>
        /// 查询文本自动回复关键词
        /// </summary>
        private string QueryTextReply(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["SearchTitle"];
            var strWhere = string.Format("UserID='{0}'  And ReplyType='text'And RuleType=1", bllBase.WebsiteOwner);

            if (!string.IsNullOrEmpty(keyword))
            {
                strWhere += " And MsgKeyword like '%" + keyword + "%'";
            }
            List<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo> data = bllWeixin.GetLit<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(pageSize, pageIndex, strWhere, "UID");
            int totalCount = bllWeixin.GetCount<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(strWhere);
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = data
});

        }
        #endregion

        #region 图文回复模块
        /// <summary>
        /// 添加图文
        /// </summary>
        private string AddNewsReply(HttpContext context)
        {
            var keyword = context.Request["MsgKeyword"];
            var matchType = context.Request["MatchType"];
            if (!bllWeixin.CheckUserKeyword(bllUser.WebsiteOwner, keyword))
            {
                return "关键字重复";
            }
            var model = new WeixinReplyRuleInfo();
            model.MsgKeyword = keyword;
            model.MatchType = matchType;
            model.ReceiveType = "news";
            model.ReplyType = "news";
            model.CreateDate = DateTime.Now;
            model.RuleType = 1;
            var sourceIds = context.Request["SourceIds"];
            model.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
            model.UserID = bllUser.WebsiteOwner;
            WeixinReplyRuleImgsInfo ruleImgsInfo;
            if (bllWeixin.Add(model))//规则表添加成功，往图文表插入
            {
                if (!string.IsNullOrEmpty(sourceIds))
                {
                    foreach (var item in sourceIds.Split(','))
                    {
                        var sourceInfo = bllWeixin.Get<WeixinMsgSourceInfo>(string.Format("SourceID='{0}'", item));
                        ruleImgsInfo = new WeixinReplyRuleImgsInfo();
                        ruleImgsInfo.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleImgAdd);
                        ruleImgsInfo.RuleID = model.UID;
                        ruleImgsInfo.Title = sourceInfo.Title;
                        ruleImgsInfo.Description = sourceInfo.Description;
                        ruleImgsInfo.PicUrl = sourceInfo.PicUrl;
                        ruleImgsInfo.Url = sourceInfo.Url;
                        bllWeixin.Add(ruleImgsInfo);
                    }
                    return "true";
                }
            }
            return "false";
        }

        /// <summary>
        /// 编辑图文自动回复
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditNewsReply(HttpContext context)
        {
            var uid = context.Request["UID"];
            var keyword = context.Request["MsgKeyword"];
            var matchType = context.Request["MatchType"];
            var type = context.Request["type"];
            var content = context.Request["ReplyContent"];
            var indexs = context.Request["index"];
            var oldInfo = bllWeixin.Get<WeixinReplyRuleInfo>(string.Format("UID={0}", uid));
            if (oldInfo.MsgKeyword != keyword)//对比关键字已经改变
            {
                //关键字改变,检查关键字是否重复
                if (!bllWeixin.CheckUserKeyword(bllBase.WebsiteOwner, keyword))
                {
                    return "关键字重复";
                }

            }

            var sourceType = context.Request["SourceType"];
            var sourceIds = context.Request["SourceIds"];

            var model = new WeixinReplyRuleInfo();
            model.UID = uid;
            model.MsgKeyword = keyword;
            model.MatchType = matchType;
            model.ReceiveType = type;
            model.ReplyType = type;
            model.RuleType = 1;
            model.UID = context.Request["UID"];
            model.UserID = bllBase.WebsiteOwner;
            if (!string.IsNullOrEmpty(content))
            {
                model.ReplyContent = content;
            }
            if (bllWeixin.Update(model))//规则表更新成功，更新图文表信息
            {

                if (!string.IsNullOrEmpty(type) && type == "text")
                {
                    bllWeixin.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}'", uid));
                }
                else
                {
                    if (sourceType.Equals("imagelist"))//原有的图文表
                    {
                        //更新图文列表
                        bllWeixin.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}' And UID Not IN ({1})", uid, sourceIds));
                        string[] kIndex = indexs.Split(',');
                        string[] ids = sourceIds.Split(',');
                        for (int i = 0; i < ids.Count(); i++)
                        {
                            WeixinReplyRuleImgsInfo ruleImgsInfo = bllBase.Get<WeixinReplyRuleImgsInfo>(string.Format(" RuleID='{0}' AND UID={1} ", uid, ids[i]));
                            if (ruleImgsInfo != null)
                            {
                                bllBase.Update(new WeixinReplyRuleImgsInfo(), string.Format(" OrderIndex={0}", kIndex[i]), string.Format(" RuleID='{0}' AND UID={1} ", uid, ids[i]));
                            }
                        }
                        return "true";
                    }
                    else
                    {
                        //删除旧图片
                        bllWeixin.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}'", uid));
                        //添加新图片 
                        string[] kIndex = indexs.Split(',');
                        string[] ids = sourceIds.Split(',');
                        for (int i = 0; i < ids.Count(); i++)
                        {
                            var sourceInfo = bllWeixin.Get<WeixinMsgSourceInfo>(string.Format("SourceID='{0}'", ids[i]));
                            WeixinReplyRuleImgsInfo ruleImgsInfo = new WeixinReplyRuleImgsInfo();
                            ruleImgsInfo.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleImgAdd);
                            ruleImgsInfo.RuleID = model.UID;
                            ruleImgsInfo.Title = sourceInfo.Title;
                            ruleImgsInfo.Description = sourceInfo.Description;
                            ruleImgsInfo.PicUrl = sourceInfo.PicUrl;
                            ruleImgsInfo.Url = sourceInfo.Url;
                            ruleImgsInfo.OrderIndex = int.Parse(kIndex[i]);
                            bllWeixin.Add(ruleImgsInfo);
                        }
                        //foreach (var item in sourceIds.Split(','))
                        //{
                        //    var sourceInfo = bllWeixin.Get<WeixinMsgSourceInfo>(string.Format("SourceID='{0}'", item));
                        //    WeixinReplyRuleImgsInfo ruleImgsInfo = new WeixinReplyRuleImgsInfo();
                        //    ruleImgsInfo.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleImgAdd);
                        //    ruleImgsInfo.RuleID = model.UID;
                        //    ruleImgsInfo.Title = sourceInfo.Title;
                        //    ruleImgsInfo.Description = sourceInfo.Description;
                        //    ruleImgsInfo.PicUrl = sourceInfo.PicUrl;
                        //    ruleImgsInfo.Url = sourceInfo.Url;
                        //    ruleImgsInfo.OrderIndex = 0;
                        //    bllWeixin.Add(ruleImgsInfo);
                        //}
                    }
                }
                return "true";
                #region
                //if (!string.IsNullOrEmpty(sourceIds))
                //{
                //    if (sourceType.Equals("imagelist"))//原有的图文表
                //    {
                //        //更新图文列表
                //        bllWeixin.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}' And UID Not IN ({1})", uid, sourceIds));
                //        return "true";
                //    }
                //    else if (sourceType.Equals("source"))//从素材表中选择
                //    {

                //        //删除旧图片
                //        bllWeixin.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}'", uid));
                //        //添加新图片 
                //        foreach (var item in sourceIds.Split(','))
                //        {
                //            var sourceInfo = bllWeixin.Get<WeixinMsgSourceInfo>(string.Format("SourceID='{0}'", item));
                //            WeixinReplyRuleImgsInfo  ruleImgsInfo = new WeixinReplyRuleImgsInfo();
                //            ruleImgsInfo.UID = bllWeixin.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleImgAdd);
                //            ruleImgsInfo.RuleID = model.UID;
                //            ruleImgsInfo.Title = sourceInfo.Title;
                //            ruleImgsInfo.Description = sourceInfo.Description;
                //            ruleImgsInfo.PicUrl = sourceInfo.PicUrl;
                //            ruleImgsInfo.Url = sourceInfo.Url;
                //            bllWeixin.Add(ruleImgsInfo);
                //        }
                //        return "true";
                //    }
                //}
                //else
                //{
                //    return "true";
                //}
                #endregion
            }
            else
            {
                return "更新规则表失败";
            }
        }


        /// <summary>
        /// 删除图文
        /// </summary>
        private string DeleteNewsReply(HttpContext context)
        {


            string ids = context.Request["id"];
            bllWeixin.Delete(new WeixinReplyRuleInfo(), string.Format("UID in({0}) ", ids));
            bllWeixin.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID in({0}) ", ids));
            return "true";




        }

        /// <summary>
        /// 查询图文及文本自动回复
        /// </summary>
        private string QueryNewsReply(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["SearchTitle"];
            string type = context.Request["type"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("UserID='{0}' AND RuleType=1", bllBase.WebsiteOwner);
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND ReplyType='{0}'", type);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And MsgKeyword like '%{0}%'", keyword);
            }
            List<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo> dataList = bllWeixin.GetLit<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(pageSize, pageIndex, sbWhere.ToString(), " ReplyType DESC ");
            int totalCount = bllWeixin.GetCount<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });


        }

        /// <summary>
        /// 获取规则图文列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSourceImageList(HttpContext context)
        {
            var data = bllJuActivity.GetList<WeixinReplyRuleImgsInfo>(string.Format("RuleID='{0}'", context.Request["UID"]));
            data = data.OrderBy(p => p.OrderIndex).ToList();
            if (data.Count > 0)
            {
                return string.Format("[{0}]", ZentCloud.Common.JSONHelper.ListToJson<WeixinReplyRuleImgsInfo>(data, ","));

            }
            return "";
        }


        /// <summary>
        /// 获取未添加的素材
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSourceNotAdd(HttpContext context)
        {


            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["SearchTitle"];
            var sbWhere = string.Format("UserID='{0}'", bllBase.WebsiteOwner);
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere += " and Title like '%" + keyword + "%'";
            }
            List<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo> list = bllWeixin.GetLit<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(pageSize, pageIndex, sbWhere, "SourceID");

            int totalCount = bllWeixin.GetCount<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(sbWhere);
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = list
            });
        }

        /// <summary>
        /// 获取规则图文列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNewsReplyImageList(HttpContext context)
        {
            var list = bllWeixin.GetList<WeixinReplyRuleImgsInfo>(string.Format("RuleID='{0}'", context.Request["UID"]));
            if (list.Count > 0)
            {
                return string.Format("[{0}]", ZentCloud.Common.JSONHelper.ListToJson<WeixinReplyRuleImgsInfo>(list, ","));

            }
            return "";
        }


        #region 图文图片模块

        /// <summary>
        /// 添加图文图片
        /// </summary>
        private string AddNewsReplyImg(HttpContext context)
        {


            var linkUrl = context.Request["LinkUrl"];
            var picUrl = context.Request["PicUrl"];
            string match = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            Regex reg = new Regex(match);
            if (!string.IsNullOrEmpty(linkUrl))
            {
                if (!reg.IsMatch(linkUrl))
                {
                    return "请输入正确的网址,格式如 http://www.baidu.com";
                }

            }


            WeixinMsgSourceInfo model = new WeixinMsgSourceInfo();
            model.UserID = bllBase.WebsiteOwner;
            model.Title = Common.StringHelper.GetReplaceStr(context.Request["SourceName"]);
            model.PicUrl = picUrl;
            model.Url = linkUrl;
            model.SourceID = bllWeixin.GetGUID(ZentCloud.BLLJIMP.TransacType.WeixinSourceAdd);
            model.Description = Common.StringHelper.GetReplaceStr(context.Request["Description"]);
            return bllWeixin.Add(model).ToString().ToLower();


        }

        /// <summary>
        /// 修改图文图片
        /// </summary>
        private string EditNewsReplyImg(HttpContext context)
        {

            //验证网址
            var linkUrl = context.Request["LinkUrl"];
            if (!string.IsNullOrEmpty(linkUrl))
            {
                string match = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                Regex reg = new Regex(match);
                if (!reg.IsMatch(linkUrl))
                {
                    return "请输入正确的网址,格式如 http://www.baidu.com";
                }

            }
            WeixinMsgSourceInfo model = new WeixinMsgSourceInfo();
            model.UserID = bllBase.WebsiteOwner;
            model.Title = Common.StringHelper.GetReplaceStr(context.Request["SourceName"]);
            model.PicUrl = context.Request["PicUrl"];
            model.Url = context.Request["LinkUrl"];
            model.SourceID = context.Request["SourceId"];
            model.Description = Common.StringHelper.GetReplaceStr(context.Request["Description"]);
            return bllWeixin.Update(model).ToString().ToLower();

        }

        /// <summary>
        /// 删除图文图片
        /// </summary>
        private string DeleteNewsReplyImg(HttpContext context)
        {
            string ids = context.Request["id"];
            if (bllWeixin.Delete(new WeixinMsgSourceInfo(), string.Format("SourceID in({0}) ", ids)) > 0)
                return "true";

            return "false";
        }

        /// <summary>
        /// 群发图文,客服接口 返回发送成功的openid列表
        /// </summary>
        private string BroadcastImageText(HttpContext context)
        {
            string ids = context.Request["id"];
            string isTimingChecked = context.Request["isTiming"];
            string time = context.Request["time"];
            TimingTask task = new TimingTask();
            task.TaskType = (int)BLLTimingTask.TaskType.SendImageTextMessage;
            task.TaskInfo = ids;
            task.ReceiverType = (int)BLLTimingTask.ReceiverType.All;
            task.WebsiteOwner = bllBase.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = (int)BLLTimingTask.TaskStatus.Waiting;
            if (isTimingChecked == "checked")   //定时发送
            {
                task.ScheduleDate = DateTime.Parse(time);

            }
            else     //立即发送
            {
                //List<BLLWeixin.WeiXinArticle> articleList = new List<BLLWeixin.WeiXinArticle>();
                //string[] idarray = ids.Split(',');
                //foreach (string id in idarray)
                //{
                //    WeixinMsgSourceInfo msg = bllWeixin.Get<WeixinMsgSourceInfo>(string.Format("SourceID={0}", id));
                //    articleList.Add(new BLLWeixin.WeiXinArticle()
                //                {
                //                    Title = msg.Title,
                //                    Description = msg.Description,
                //                    Url = msg.Url,
                //                    PicUrl = msg.PicUrl
                //                });
                //}
                //ReturnValue rv = bllWeixin.BroadcastKeFuMessageImageText(bllBase.WebsiteOwner, articleList, ids);
                //return rv.Msg;
                task.ScheduleDate = DateTime.Now;


            }
            if (bllWeixin.Add(task))
            {
                return "群发已加入队列！";
            }
            return "加入任务失败";
        }



        /// <summary>
        /// 群发图文 群发接口
        /// </summary>
        private string SendMassMessageNews(HttpContext context)
        {


            string ids = context.Request["ids"];
            string isTimingChecked = context.Request["isTiming"];
            string time = context.Request["time"];
            List<UploadArticle> articleList = new List<UploadArticle>();
            string[] idArray = ids.Split(',');
            if (isTimingChecked == "checked")   //定时发送
            {
                TimingTask task = new TimingTask();
                task.TaskType = (int)BLLTimingTask.TaskType.SendMassMessage;
                task.TaskInfo = ids;
                task.ReceiverType = (int)BLLTimingTask.ReceiverType.All;
                task.WebsiteOwner = bllBase.WebsiteOwner;
                task.ScheduleDate = DateTime.Parse(time);
                task.InsertDate = DateTime.Now;
                task.Status = (int)BLLTimingTask.TaskStatus.Waiting;
                bllWeixin.Add(task);
                resp.Msg = "定时群发已加入队列！";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else//立即发送
            {
                string accessToken = bllWeixin.GetAccessToken();
                foreach (string id in idArray)
                {
                    WXMassArticle msg = bllWeixin.Get<WXMassArticle>(string.Format("AutoID={0}", id));
                    msg.ThumbImage = context.Server.MapPath(msg.ThumbImage);//本地磁盘路径
                    string mediaId = bllWeixin.UploadFileToWeixinModel(accessToken, "image", msg.ThumbImage).media_id;
                    var model = new UploadArticle();
                    model.thumb_media_id = mediaId;
                    model.title = msg.Title;
                    model.content_source_url = msg.Content_Source_Url;
                    model.content = msg.Content;
                    model.digest = msg.Digest;
                    model.author = msg.Author;
                    model.show_cover_pic = "0";
                    //if (model.content.Contains("/FileUpload/"))
                    //{
                    //    model.content = model.content.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
                    //}
                    articleList.Add(model);

                }
                var upLoadObj = new
                {

                    articles = articleList

                };
                JToken jt = JToken.FromObject(upLoadObj);
                string sendMsg = jt.ToString();
                string sendMediaId = bllWeixin.UploadNews(accessToken, sendMsg).media_id;
                //string returnResult = bllWeixin.SendMassMessageMpNewsPreview(accessToken, sendMediaId, "o99IZtxVsEpSanbBmiW_1_IVL4-0");
                string returnResult = bllWeixin.SendMassMessageMpNews(accessToken, sendMediaId);
                var returnModel = bllWeixin.GetErrorMessageModel(returnResult);
                resp.Msg = bllWeixin.GetCodeMessage(returnModel.errcode);
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }


        }

        /// <summary>
        /// 群发图文预览
        /// </summary>
        private string SendMassMessageNewsPreview(HttpContext context)
        {


            string ids = context.Request["ids"];
            string userAutoId = context.Request["userAutoId"];
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
            List<UploadArticle> articleList = new List<UploadArticle>();
            string[] idArray = ids.Split(',');

            string accessToken = bllWeixin.GetAccessToken();
            foreach (string id in idArray)
            {
                WXMassArticle msg = bllWeixin.Get<WXMassArticle>(string.Format("AutoID={0}", id));
                msg.ThumbImage = context.Server.MapPath(msg.ThumbImage);//本地磁盘路径
                string mediaId = bllWeixin.UploadFileToWeixinModel(accessToken, "image", msg.ThumbImage).media_id;
                var model = new UploadArticle();
                model.thumb_media_id = mediaId;
                model.title = msg.Title;
                model.content_source_url = msg.Content_Source_Url;
                model.content = msg.Content;
                model.digest = msg.Digest;
                model.author = msg.Author;
                model.show_cover_pic = "0";
                articleList.Add(model);

            }
            var upLoadObj = new
            {

                articles = articleList

            };
            JToken jt = JToken.FromObject(upLoadObj);
            string sendMsg = jt.ToString();
            string sendMediaId = bllWeixin.UploadNews(accessToken, sendMsg).media_id;
            string returnResult = bllWeixin.SendMassMessageMpNewsPreview(accessToken, sendMediaId, userInfo.WXOpenId);
            var returnModel = bllWeixin.GetErrorMessageModel(returnResult);
            resp.Msg = bllWeixin.GetCodeMessage(returnModel.errcode);
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);




        }


        /// <summary>
        /// 查询图文图片
        /// </summary>
        private string QueryNewsReplyImg(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["SearchTitle"];
            var sbWhere = string.Format("UserID='{0}'", bllBase.WebsiteOwner);
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere += " AND Title like '%" + keyword + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo> dataList = bllWeixin.GetLit<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(pageSize, pageIndex, sbWhere, "SourceID DESC");

            int totalCount = bllWeixin.GetCount<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(sbWhere);

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });

        }
        #endregion


        #region 行业模板管理模块

        ///// <summary>
        ///// 添加模板
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddIndustryTemplate(HttpContext context)
        //{
        //    string IndustryTemplateName = context.Request["IndustryTemplateName"];
        //    string WebsiteName = context.Request["WebsiteName"];
        //    string WebsiteDescription = context.Request["WebsiteDescription"];
        //    string WebsiteLogo = context.Request["WebsiteLogo"];
        //    string CourseManageMenuRName = context.Request["CourseManageMenuRName"];
        //    string ArticleManageMenuRName = context.Request["ArticleManageMenuRName"];
        //    string MasterManageMenuRName = context.Request["MasterManageMenuRName"];
        //    string QuestionManageMenuRName = context.Request["QuestionManageMenuRName"];
        //    string UserManageMenuRName = context.Request["UserManageMenuRName"];
        //    string SignUpCourseMenuRName = context.Request["SignUpCourseMenuRName"];
        //    string ActivityManageMenuRName = context.Request["ActivityManageMenuRName"];
        //    string MallMenuRName = context.Request["MallMenuRName"];
        //    string WebSiteStatisticsMenuRName = context.Request["WebSiteStatisticsMenuRName"];
        //    string CourseCate1 = context.Request["CourseCate1"];
        //    string CourseCate2 = context.Request["CourseCate2"];
        //    string ArticleCate1 = context.Request["ArticleCate1"];
        //    string ArticleCate2 = context.Request["ArticleCate2"];
        //    string ArticleCate3 = context.Request["ArticleCate3"];
        //    string ArticleCate4 = context.Request["ArticleCate4"];
        //    string ArticleCate5 = context.Request["ArticleCate5"];
        //    string AddVMenuRName = context.Request["AddVMenuRName"];
        //    if (string.IsNullOrEmpty(IndustryTemplateName))
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "请输入模板名称";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    IndustryTemplate model = new IndustryTemplate();
        //    model.IndustryTemplateName = IndustryTemplateName;
        //    model.WebsiteName = WebsiteName;
        //    model.WebsiteDescription = WebsiteDescription;
        //    model.WebsiteLogo = WebsiteLogo;
        //    model.CreateDate = DateTime.Now;
        //    model.CourseManageMenuRName = CourseManageMenuRName;
        //    model.ArticleManageMenuRName = ArticleManageMenuRName;
        //    model.MasterManageMenuRName = MasterManageMenuRName;
        //    model.QuestionManageMenuRName = QuestionManageMenuRName;
        //    model.UserManageMenuRName = UserManageMenuRName;
        //    model.SignUpCourseMenuRName = SignUpCourseMenuRName;
        //    model.ActivityManageMenuRName = ActivityManageMenuRName;
        //    model.MallMenuRName = MallMenuRName;
        //    model.WebSiteStatisticsMenuRName = WebSiteStatisticsMenuRName;
        //    model.CourseCate1 = CourseCate1;
        //    model.CourseCate2 = CourseCate2;
        //    model.ArticleCate1 = ArticleCate1;
        //    model.ArticleCate2 = ArticleCate2;
        //    model.ArticleCate3 = ArticleCate3;
        //    model.ArticleCate4 = ArticleCate4;
        //    model.ArticleCate5 = ArticleCate5;
        //    model.AddVMenuRName = AddVMenuRName;
        //    if (bllJuActivity.Add(model))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "添加成功";


        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "添加失败";

        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);




        //}

        ///// <summary>
        ///// 编辑模板
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string EditIndustryTemplate(HttpContext context)
        //{
        //    string AutoID = context.Request["AutoID"];
        //    string IndustryTemplateName = context.Request["IndustryTemplateName"];
        //    string WebsiteName = context.Request["WebsiteName"];
        //    string WebsiteDescription = context.Request["WebsiteDescription"];
        //    string WebsiteLogo = context.Request["WebsiteLogo"];
        //    string CourseManageMenuRName = context.Request["CourseManageMenuRName"];
        //    string ArticleManageMenuRName = context.Request["ArticleManageMenuRName"];
        //    string MasterManageMenuRName = context.Request["MasterManageMenuRName"];
        //    string QuestionManageMenuRName = context.Request["QuestionManageMenuRName"];
        //    string UserManageMenuRName = context.Request["UserManageMenuRName"];
        //    string SignUpCourseMenuRName = context.Request["SignUpCourseMenuRName"];
        //    string ActivityManageMenuRName = context.Request["ActivityManageMenuRName"];
        //    string MallMenuRName = context.Request["MallMenuRName"];
        //    string WebSiteStatisticsMenuRName = context.Request["WebSiteStatisticsMenuRName"];
        //    string CourseCate1 = context.Request["CourseCate1"];
        //    string CourseCate2 = context.Request["CourseCate2"];
        //    string ArticleCate1 = context.Request["ArticleCate1"];
        //    string ArticleCate2 = context.Request["ArticleCate2"];
        //    string ArticleCate3 = context.Request["ArticleCate3"];
        //    string ArticleCate4 = context.Request["ArticleCate4"];
        //    string ArticleCate5 = context.Request["ArticleCate5"];
        //    string AddVMenuRName = context.Request["AddVMenuRName"];
        //    if (string.IsNullOrEmpty(IndustryTemplateName))
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "请输入模板名称";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    IndustryTemplate model = new IndustryTemplate();
        //    model.AutoID = int.Parse(AutoID);
        //    model.IndustryTemplateName = IndustryTemplateName;
        //    model.WebsiteName = WebsiteName;
        //    model.WebsiteDescription = WebsiteDescription;
        //    model.WebsiteLogo = WebsiteLogo;
        //    model.CreateDate = DateTime.Now;
        //    model.CourseManageMenuRName = CourseManageMenuRName;
        //    model.ArticleManageMenuRName = ArticleManageMenuRName;
        //    model.MasterManageMenuRName = MasterManageMenuRName;
        //    model.QuestionManageMenuRName = QuestionManageMenuRName;
        //    model.UserManageMenuRName = UserManageMenuRName;
        //    model.SignUpCourseMenuRName = SignUpCourseMenuRName;
        //    model.ActivityManageMenuRName = ActivityManageMenuRName;
        //    model.MallMenuRName = MallMenuRName;
        //    model.WebSiteStatisticsMenuRName = WebSiteStatisticsMenuRName;
        //    model.CourseCate1 = CourseCate1;
        //    model.CourseCate2 = CourseCate2;
        //    model.ArticleCate1 = ArticleCate1;
        //    model.ArticleCate2 = ArticleCate2;
        //    model.ArticleCate3 = ArticleCate3;
        //    model.ArticleCate4 = ArticleCate4;
        //    model.ArticleCate5 = ArticleCate5;
        //    model.AddVMenuRName = context.Request["AddVMenuRName"];
        //    if (bllJuActivity.Update(model))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "更新成功";


        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "更新失败";

        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);




        //}
        ///// <summary>
        ///// 删除模板
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string DeleteIndustryTemplate(HttpContext context)
        //{

        //    string ids = context.Request["Ids"];
        //    int count = bllJuActivity.Delete(new IndustryTemplate(), string.Format("AutoID in ({0})", ids));
        //    return string.Format("成功删除了{0}条数据", count);

        //}


        ///// <summary>
        ///// 查询模板
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryIndustryTemplate(HttpContext context)
        //{


        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    string TemplateName = context.Request["TemplateName"];
        //    StringBuilder sbWhere = new StringBuilder();
        //    if (!string.IsNullOrEmpty(TemplateName))
        //    {
        //        sbWhere.AppendFormat("And TemplateName='{0}'", TemplateName);
        //    }
        //    int count = bllJuActivity.GetCount<IndustryTemplate>(sbWhere.ToString());

        //    List<IndustryTemplate> dataList = new List<IndustryTemplate>();
        //    dataList = bllJuActivity.GetLit<IndustryTemplate>(rows, page, sbWhere.ToString(), "AutoID DESC");
        //    string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(count, dataList);
        //    return jsonResult;



        //}



        #endregion


        /// <summary>
        /// 获取加V分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAddVImageWap(HttpContext context)
        {

            string dir = context.Request["dir"];//加载加V分类
            if (string.IsNullOrEmpty(currentUserInfo.WXHeadimgurlLocal))
            {
                return "获取不到头像，请点击'更新头像'";

            }
            string imgOrgPath = context.Server.MapPath(currentUserInfo.WXHeadimgurlLocal);
            //string imgOrgPath = context.Server.MapPath("/FileUpload/ImageMapping/b2df5bd9-1700-4ee5-878a-af7d7783e38c.jpg");
            string imgBorderPath = context.Server.MapPath("/FileUpload/WXADDV/border/" + Guid.NewGuid().ToString() + ".jpg");
            List<string> imgWatermarkPathList = new List<string>();
            string[] arrFiles = System.IO.Directory.GetFiles(context.Server.MapPath(string.Format("/img/WXADDV/{0}/{1}/", bllBase.WebsiteOwner, dir)));
            if (arrFiles.Length > 0)
                imgWatermarkPathList = arrFiles.ToList();

            List<string> imgVList = new List<string>();
            List<string> fileNameList = new List<string>(); //文件名列表
            ZentCloud.Common.ImgWatermarkHelper im = new ZentCloud.Common.ImgWatermarkHelper();
            im.ImgAddBord(imgOrgPath, imgBorderPath);
            StringBuilder strHtml = new StringBuilder();
            foreach (var item in imgWatermarkPathList)
            {

                string imgVstr = "/FileUpload/WXADDV/" + Guid.NewGuid().ToString() + ".jpg";
                string imgVstrLocal = context.Server.MapPath(imgVstr);
                im.SaveWatermark(imgBorderPath, item, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.RigthBottom, 0, imgVstrLocal, 0.3f);//, 0.25f
                imgVList.Add(imgVstr);
                fileNameList.Add(item.Split('\\')[item.Split('\\').Length - 1].Replace(".png", null));

            }
            for (int i = 0; i < imgVList.Count; i++)
            {
                strHtml.AppendFormat("<div style=\"width: 120px; float: left; padding: 5px;text-align:center;\"> <img alt=\"\" width=\"120px\" src=\"{0}\" /><br/><label>{1}</label> </div>", imgVList[i], GetAddVMapingKeyByValue(string.Format("V_{0}_{1}", dir, fileNameList[i])));

            }
            return strHtml.ToString();


        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetAddVMapingKeyByValue(string value)
        {
            WXAddVMaping addvMap = bllUser.Get<WXAddVMaping>(string.Format("AddVValue='{0}' And WebsiteOwner='{1}'", value, bllBase.WebsiteOwner));
            if (addvMap != null)
            {
                return addvMap.AddVKey;
            }
            return "";

        }





        #endregion

        ///// <summary>
        ///// 宽桥企业帮 申请企业核名 成功后发送客服消息
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string SendKeFuMsgKuanQiao(HttpContext context)
        //{
        //    BLLJIMP.BLLWeixin weixinBll = new BLLJIMP.BLLWeixin("");
        //    string accessToken = weixinBll.GetAccessToken(CurrWebSiteUserInfo.UserID);
        //    if (accessToken != string.Empty)
        //    {
        //        string msg =  "申请提交成功!请稍后将投资人身份证正反面图片发送到微信公众号 宽桥企业帮";
        //        if (weixinBll.SendKeFuMessageText(accessToken, CurrentUserInfo.WXOpenId, msg).Contains("ok"))
        //        {
        //            return "1";
        //        }


        //    }
        //    return "0";


        //}



        #region 客服列表
        /// <summary>
        /// 查询客服列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryKuFuList(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            int totalCount = bllBase.GetCount<WXKeFu>(sbWhere.ToString());
            List<WXKeFu> dataList = this.bllBase.GetLit<WXKeFu>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加客服信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddKeFu(HttpContext context)
        {
            string trueName = context.Request["TrueName"];
            string phone = context.Request["Phone"];
            string weixinOpenId = context.Request["WeiXinOpenID"];
            if (string.IsNullOrEmpty(trueName))
            {
                resp.Status = 0;
                resp.Msg = "请输入姓名";
                Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(weixinOpenId))
            {
                resp.Status = 0;
                resp.Msg = "请输入OpenId";
                Common.JSONHelper.ObjectToJson(resp);
            }
            WXKeFu userInfo = bllWeixin.GetKeFu(weixinOpenId, bllWeixin.WebsiteOwner);
            if (userInfo != null)
            {
                resp.Status = 0;
                resp.Msg = "重复添加";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WXKeFu model = new WXKeFu();
            model.TrueName = trueName;
            model.Phone = phone;
            model.WeiXinOpenID = weixinOpenId;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";
                resp.ExStr = bllWeixin.GetKeFuMaxID().ToString();
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑客服信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditKeFu(HttpContext context)
        {
            string trueName = context.Request["TrueName"];
            string phone = context.Request["Phone"];
            string weixinOpenId = context.Request["WeiXinOpenID"];
            int autoID = int.Parse(context.Request["AutoID"]);
            if (string.IsNullOrEmpty(trueName))
            {
                resp.Status = 0;
                resp.Msg = "请输入姓名";
                Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(weixinOpenId))
            {
                resp.Status = 0;
                resp.Msg = "请输入OpenId";
                Common.JSONHelper.ObjectToJson(resp);
            }
            var model = bllJuActivity.Get<WXKeFu>(string.Format("AutoID={0}", autoID));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "客服不存在";
                Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                Common.JSONHelper.ObjectToJson(resp);

            }
            model.TrueName = trueName;
            model.Phone = phone;
            model.WeiXinOpenID = weixinOpenId;
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除客服信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteKeFu(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXKeFu model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<WXKeFu>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int Count = bllJuActivity.Delete(new WXKeFu(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", Count);

        }
        #endregion


        #region 文章分类管理
        /// <summary>
        /// 查询文章分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryArticleCategory(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //string categoryName = context.Request["CategoryName"];
            string categoryType = context.Request["CategoryType"];

            int cateRootId = Convert.ToInt32(context.Request["cateRootId"]);
            string websiteowner = context.Request["websiteowner"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bllBase.WebsiteOwner;

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", websiteowner));
            //if (!string.IsNullOrEmpty(categoryName))
            //{
            //    sbWhere.AppendFormat(" And CategoryName like '%{0}%'", categoryName);
            //}
            if (!string.IsNullOrEmpty(categoryType))
            {
                sbWhere.AppendFormat(" And CategoryType ='{0}'", categoryType);
            }
            //if(cateRootId > 0)
            //{
            //    strWhere.AppendFormat(" And PreID ='{0}'", cateRootId);
            //}
            List<ArticleCategory> data;
            data = bllWeixin.GetList<ArticleCategory>(sbWhere.ToString());
            data = data.OrderBy(p => p.Sort).ThenBy(p => p.AutoID).ToList();
            List<ArticleCategory> showList = new List<ArticleCategory>();

            Common.MyCategories m = new Common.MyCategories();

            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<ArticleCategory>("AutoID", "PreID", "CategoryName", data), cateRootId))
            {
                //try
                //{
                ArticleCategory tmpModel = (ArticleCategory)data.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0].Clone();
                tmpModel.CategoryName = item.Text;
                showList.Add(tmpModel);
                //}
                //catch { }
            }
            int total = showList.Count;
            showList = showList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Common.JSONHelper.ObjectToJson(new
            {
                total = total,
                rows = showList
            });
            //

        }


        /// <summary>
        /// 添加文章分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddArticleCategory(HttpContext context)
        {
            string categoryName = context.Request["CategoryName"],
                    categoryType = context.Request["CategoryType"],
                    preID = context.Request["PreID"],
                    sort = context.Request["Sort"],
                    sysType = context.Request["SysType"],
                    summary = context.Request["Summary"],
                    imgSrc = context.Request["ImgSrc"],
                    websiteowner = context.Request["websiteowner"];

            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bllBase.WebsiteOwner;


            if (string.IsNullOrEmpty(categoryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入分类名称";
                Common.JSONHelper.ObjectToJson(resp);
            }


            ArticleCategory model = new ArticleCategory();
            model.CategoryName = categoryName;
            model.CategoryType = categoryType;
            model.WebsiteOwner = websiteowner;
            model.PreID = string.IsNullOrEmpty(preID) ? 0 : int.Parse(preID);
            model.Sort = string.IsNullOrEmpty(sort) ? 0 : int.Parse(sort);
            model.SysType = Convert.ToInt32(sysType);
            model.Summary = summary;
            model.ImgSrc = imgSrc;
            if (bllJuActivity.GetCount<ArticleCategory>(string.Format(" WebsiteOwner='{0}' And CategoryName='{1}' And CategoryType='{2}'", websiteowner, model.CategoryName, model.CategoryType)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "分类名称已存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑文章分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditArticleCategory(HttpContext context)
        {
            string categoryName = context.Request["CategoryName"],
                   preId = context.Request["PreID"],
                   sort = context.Request["Sort"],
                   sysType = context.Request["SysType"],
                    imgSrc = context.Request["ImgSrc"],
                    summary = context.Request["Summary"],
                    websiteowner = context.Request["websiteowner"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bllBase.WebsiteOwner;
            int autoId = int.Parse(context.Request["AutoID"]);
            if (string.IsNullOrEmpty(categoryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入分类名称";
                Common.JSONHelper.ObjectToJson(resp);
            }

            var model = bllJuActivity.Get<ArticleCategory>(string.Format("AutoID={0}", autoId));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "分类不存在";
                Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(websiteowner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                Common.JSONHelper.ObjectToJson(resp);

            }
            model.CategoryName = categoryName;
            model.PreID = string.IsNullOrEmpty(preId) ? 0 : int.Parse(preId);
            model.Sort = string.IsNullOrEmpty(sort) ? 0 : int.Parse(sort);
            model.SysType = Convert.ToInt32(sysType);
            model.Summary = summary;
            model.ImgSrc = imgSrc;
            if (bllJuActivity.Update(model))
            {

                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除文章分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteArticleCategory(HttpContext context)
        {
            string ids = context.Request["ids"],
                    websiteowner = context.Request["websiteowner"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bllBase.WebsiteOwner;
            ArticleCategory model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<ArticleCategory>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(websiteowner)))
                {
                    return "无权删除";

                }

            }
            int count = bllJuActivity.Delete(new ArticleCategory(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }


        /// <summary>
        /// 获取分类选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCategorySelectList(HttpContext context)
        {
            int cateRootId = Convert.ToInt32(context.Request["cateRootId"]);
            int maxDepth = Convert.ToInt32(context.Request["maxDepth"]);
            string websiteowner = context.Request["websiteowner"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bllBase.WebsiteOwner;

            if (maxDepth == 0)
            {
                maxDepth = 10000;
            }

            return new Common.MyCategories().GetSelectOptionHtml(
                    bllWeixin.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", websiteowner, context.Request["CategoryType"])),
                    "AutoID",
                    "PreID",
                    "CategoryName",
                    cateRootId,
                    "ddlPreMenu",
                    "width:200px",
                    "",
                    "0",
                    "",
                    maxDepth
                );
        }

        #endregion


        #region 微商城分类管理
        /// <summary>
        /// 查询微商城商品分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallCategory(HttpContext context)
        {

            //int page = Convert.ToInt32(context.Request["page"]);
            //int rows = Convert.ToInt32(context.Request["rows"]);
            //string categoryName = context.Request["CategoryName"];

            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            //if (!string.IsNullOrEmpty(categoryName))
            //{
            //    sbWhere.AppendFormat(" And CategoryName like '%{0}%'", categoryName);
            //}


            //int totalCount = this.juActivityBll.GetCount<WXMallCategory>(sbWhere.ToString());
            //List<WXMallCategory> dataList = this.juActivityBll.GetLit<WXMallCategory>(rows, page, sbWhere.ToString());

            //return Common.JSONHelper.ListToEasyUIJson(totalCount, dataList);


            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string type = context.Request["type"];
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = int.MaxValue;
            }

            //string categoryName = context.Request["CategoryName"];
            string categoryName = context.Request["CategoryName"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));

            if (!string.IsNullOrEmpty(categoryName))
            {
                sbWhere.AppendFormat(" And CategoryName ='{0}'", categoryName);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And Type ='{0}'", type);
            }
            List<WXMallCategory> dataList;
            dataList = bllWeixin.GetList<WXMallCategory>(sbWhere.ToString());
            dataList = dataList.OrderBy(p => p.AutoID).ToList();
            List<WXMallCategory> showList = new List<WXMallCategory>();
            MySpider.MyCategories m = new MySpider.MyCategories();
            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<WXMallCategory>("AutoID", "PreID", "CategoryName", dataList), 0))
            {
                try
                {
                    WXMallCategory tmpModel = (WXMallCategory)dataList.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0].Clone();
                    tmpModel.CategoryName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }

            showList = showList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Common.JSONHelper.ObjectToJson(
new
{
    total = showList.Count,
    rows = showList
});


        }


        /// <summary>
        /// 添加微商城商品分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallCategory(HttpContext context)
        {
            string categoryName = context.Request["CategoryName"];
            string description = context.Request["Description"];
            int preID = int.Parse(context.Request["PreID"]);
            string categoryImg = context.Request["CategoryImg"];
            string type = context.Request["Type"];
            if (string.IsNullOrEmpty(categoryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            WXMallCategory model = new WXMallCategory();
            model.CategoryName = categoryName;
            model.Description = description;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.PreID = preID;
            model.CategoryImg = categoryImg;
            model.Type = type;
            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑微商城商品分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallCategory(HttpContext context)
        {


            string categoryName = context.Request["CategoryName"];
            int autoId = int.Parse(context.Request["AutoID"]);
            string description = context.Request["Description"];
            int preId = int.Parse(context.Request["PreID"]);
            string categoryImg = context.Request["CategoryImg"];
            string type = context.Request["Type"];
            if (string.IsNullOrEmpty(categoryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入分类名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            var model = bllMall.Get<WXMallCategory>(string.Format("AutoID={0}", autoId));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "分类不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.CategoryName = categoryName;
            model.Description = description;
            model.PreID = preId;
            model.CategoryImg = categoryImg;
            model.Type = type;
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除微商城分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallCategory(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXMallCategory model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<WXMallCategory>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllJuActivity.Delete(new WXMallCategory(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }


        /// <summary>
        /// 获取商品分类选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWXMallCategorySelectList(HttpContext context)
        {
            return new MySpider.MyCategories().GetSelectOptionHtml(bllMall.GetCategoryList().Where(p => p.Type.Equals(context.Request["type"])).ToList(), "AutoID", "PreID", "CategoryName", 0, "ddlPreMenu", "width:200px", "");

        }

        #endregion

        #region 微商城订单状态管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallOrderStatu(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            int totalCount = this.bllMall.GetCount<WXMallOrderStatusInfo>(sbWhere.ToString());
            List<WXMallOrderStatusInfo> dataList = this.bllMall.GetLit<WXMallOrderStatusInfo>(pageSize, pageIndex, sbWhere.ToString(), " Sort DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallOrderStatu(HttpContext context)
        {
            string orderStatu = context.Request["OrderStatu"];
            string orderMessage = context.Request["OrderMessage"];
            string sort = context.Request["Sort"];
            if (string.IsNullOrEmpty(orderStatu))
            {
                resp.Status = 0;
                resp.Msg = "请输入订单状态";
                Common.JSONHelper.ObjectToJson(resp);
            }

            WXMallOrderStatusInfo model = new WXMallOrderStatusInfo();
            model.OrderStatu = orderStatu;
            model.OrderMessage = orderMessage;
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = int.Parse(sort);
            }

            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallOrderStatu(HttpContext context)
        {


            string orderStatu = context.Request["OrderStatu"];
            string orderMessage = context.Request["OrderMessage"];
            string sort = context.Request["Sort"];
            int autoId = int.Parse(context.Request["AutoID"]);
            if (string.IsNullOrEmpty(orderStatu))
            {
                resp.Status = 0;
                resp.Msg = "请输入订单状态";
                Common.JSONHelper.ObjectToJson(resp);
            }

            var model = bllJuActivity.Get<WXMallOrderStatusInfo>(string.Format("AutoID={0}", autoId));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "状态不存在";
                Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                Common.JSONHelper.ObjectToJson(resp);

            }
            model.OrderStatu = orderStatu;
            model.OrderMessage = orderMessage;
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = int.Parse(sort);
            }

            if (bllMall.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallOrderStatu(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXMallOrderStatusInfo model;
            foreach (var item in ids.Split(','))
            {
                model = bllMall.Get<WXMallOrderStatusInfo>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int resultCount = bllMall.Delete(new WXMallOrderStatusInfo(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", resultCount);

        }

        #endregion

        #region 刮奖活动

        /// <summary>
        /// 查询刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXLottery(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string lotteryName = context.Request["LotteryName"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(lotteryName))
            {
                sbWhere.AppendFormat(" And LotteryName like '%{0}%'", lotteryName);
            }
            int totalCount = this.bllJuActivity.GetCount<WXLottery>(sbWhere.ToString());
            List<WXLottery> dataList = this.bllJuActivity.GetLit<WXLottery>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }

        /// <summary>
        /// 查询中奖结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXLotteryRecord(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int lotteryId = int.Parse(context.Request["LotteryId"]);
            string token = context.Request["Token"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" LotteryId={0}", lotteryId));
            if (!string.IsNullOrEmpty(token))
            {
                sbWhere.AppendFormat(" And Token ='{0}'", token);
            }
            int totalCount = this.bllLottery.GetCount<WXLotteryRecord>(sbWhere.ToString());
            List<WXLotteryRecord> dataList = this.bllLottery.GetLit<WXLotteryRecord>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXLottery(HttpContext context)
        {
            string lotteryName = context.Request["LotteryName"];
            string lotteryTitle = context.Request["LotteryTitle"];
            string thumbnailsPath = context.Request["ThumbnailsPath"];
            string scratchUpAreaContent = context.Request["ScratchUpAreaContent"];
            string scratchDownAreaContent = context.Request["ScratchDownAreaContent"];
            int status = int.Parse(context.Request["Status"]);
            int maxCount = int.Parse(context.Request["MaxCount"]);
            string prizeSet = context.Request["PrizeSet"];
            string lotteryActivityId = context.Request["LotteryActivityID"];
            if (string.IsNullOrEmpty(lotteryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入活动名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            WXLottery model = new WXLottery();
            model.LotteryName = lotteryName;
            model.LotteryTitle = lotteryTitle;
            model.ThumbnailsPath = thumbnailsPath;
            model.ScratchUpAreaContent = scratchUpAreaContent;
            model.ScratchDownAreaContent = scratchDownAreaContent;
            model.Status = status;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.PrizeSet = prizeSet;
            model.MaxCount = maxCount;
            model.LotteryActivityID = lotteryActivityId;
            if (bllLottery.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXLottery(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            string lotteryName = context.Request["LotteryName"];
            string lotteryTitle = context.Request["LotteryTitle"];
            string thumbnailsPath = context.Request["ThumbnailsPath"];
            string scratchUpAreaContent = context.Request["ScratchUpAreaContent"];
            string scratchDownAreaContent = context.Request["ScratchDownAreaContent"];
            int status = int.Parse(context.Request["Status"]);
            string prizeSet = context.Request["PrizeSet"];
            int maxCount = int.Parse(context.Request["MaxCount"]);
            string lotteryActivityId = context.Request["LotteryActivityID"];
            if (string.IsNullOrEmpty(lotteryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入活动名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }


            WXLottery model = bllLottery.Get<WXLottery>(string.Format("AutoID={0}", autoId));
            model.LotteryName = lotteryName;
            model.LotteryTitle = lotteryTitle;
            model.ThumbnailsPath = thumbnailsPath;
            model.ScratchUpAreaContent = scratchUpAreaContent;
            model.ScratchDownAreaContent = scratchDownAreaContent;
            model.Status = status;
            model.PrizeSet = prizeSet;
            model.MaxCount = maxCount;
            model.LotteryActivityID = lotteryActivityId;
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }




        /// <summary>
        /// 删除刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXLottery(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXLottery model;
            foreach (var item in ids.Split(','))
            {
                model = bllLottery.Get<WXLottery>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            //关联删除
            int count = bllLottery.Delete(new WXLottery(), string.Format("AutoID in ({0})", ids));//删除刮奖表
            bllLottery.Delete(new WXLotteryLog(), string.Format("LotteryId in ({0})", ids));//删除刮奖记录表
            bllLottery.Delete(new WXLotteryRecord(), string.Format("LotteryId in ({0})", ids));//删除中奖记录表
            bllLottery.Delete(new WXAwards(), string.Format("LotteryId in ({0})", ids));//删除奖品表
            bllLottery.Delete(new WXLotteryWinningData(), string.Format("LotteryId in ({0})", ids));//删除中奖设置表
            return string.Format("成功删除了 {0} 条数据", count);

        }

        /// <summary>
        /// 重置刮奖
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ResetWXLottery(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXLottery model;
            foreach (var item in ids.Split(','))
            {
                model = bllLottery.Get<WXLottery>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权操作";

                }

            }
            bllLottery.Delete(new WXLotteryLog(), string.Format("LotteryId in ({0})", ids));
            bllLottery.Delete(new WXLotteryRecord(), string.Format("LotteryId in ({0})", ids));

            return string.Format("已成功重置");

        }

        #region 奖项设置

        /// <summary>
        /// 查询奖项设置列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXAwards(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string lotteryId = context.Request["LotteryId"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" LotteryId='{0}'", lotteryId));
            int totalCount = this.bllLottery.GetCount<WXAwards>(sbWhere.ToString());
            List<WXAwards> dataList = this.bllLottery.GetLit<WXAwards>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }
        /// <summary>
        /// 查询奖项设置列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWxAwardListByLotteryId(HttpContext context)
        {

            string lotteryId = context.Request["LotteryId"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" LotteryId='{0}'", lotteryId));
            List<WXAwards> dataList = this.bllLottery.GetList<WXAwards>(sbWhere.ToString());
            resp.ExObj = dataList;
            return Common.JSONHelper.ObjectToJson(resp);


        }



        /// <summary>
        /// 添加奖项设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXAwards(HttpContext context)
        {
            int lotteryId = int.Parse(context.Request["LotteryId"]);
            string prizeName = context.Request["PrizeName"];
            int prizeCount = int.Parse(context.Request["PrizeCount"]);

            WXAwards model = new WXAwards();
            model.LotteryId = lotteryId;
            model.PrizeName = prizeName;
            model.PrizeCount = prizeCount;
            if (bllLottery.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);





        }

        /// <summary>
        /// 编辑奖项设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXAwards(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            string lotteryId = context.Request["LotteryId"];
            string prizeName = context.Request["PrizeName"];
            int prizeCount = int.Parse(context.Request["PrizeCount"]);
            WXAwards model = bllLottery.Get<WXAwards>(string.Format("AutoID={0}", autoId));
            model.PrizeName = prizeName;
            model.PrizeCount = prizeCount;
            if (bllLottery.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "编辑成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "编辑失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);





        }

        /// <summary>
        /// 删除奖项设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXAwards(HttpContext context)
        {

            string ids = context.Request["ids"];
            int count = bllLottery.Delete(new WXAwards(), string.Format("AutoID in ({0})", ids));
            resp.Msg = string.Format("成功删除 {0} 条数据", count);
            return Common.JSONHelper.ObjectToJson(resp);



        }




        #endregion


        #region 中奖设置

        /// <summary>
        /// 查询中奖设置列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWinningData(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string lotteryId = context.Request["LotteryId"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" LotteryId='{0}'", lotteryId));
            int totalCount = this.bllLottery.GetCount<WXLotteryWinningData>(sbWhere.ToString());
            List<WXLotteryWinningData> dataList = this.bllJuActivity.GetLit<WXLotteryWinningData>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }

        /// <summary>
        /// 添加中奖设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWinningData(HttpContext context)
        {
            int lotteryId = int.Parse(context.Request["LotteryId"]);
            int winningIndex = int.Parse(context.Request["WinningIndex"]);
            int wxAwardsId = int.Parse(context.Request["WXAwardsId"]);

            WXLotteryWinningData model = new WXLotteryWinningData();
            model.LotteryId = lotteryId;
            model.WinningIndex = winningIndex;
            model.WXAwardsId = wxAwardsId;

            if (bllLottery.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);





        }

        /// <summary>
        /// 编辑中奖设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWinningData(HttpContext context)
        {

            string autoId = context.Request["AutoID"];
            int lotteryId = int.Parse(context.Request["LotteryId"]);
            int winningIndex = int.Parse(context.Request["WinningIndex"]);
            int wXAwardsId = int.Parse(context.Request["WXAwardsId"]);
            WXLotteryWinningData model = bllJuActivity.Get<WXLotteryWinningData>(string.Format("AutoID={0}", autoId));
            model.WinningIndex = winningIndex;
            model.WXAwardsId = wXAwardsId;
            if (bllLottery.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "编辑成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "编辑失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除中奖设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWinningData(HttpContext context)
        {

            string ids = context.Request["ids"];
            int count = bllLottery.Delete(new WXLotteryWinningData(), string.Format("AutoID in ({0})", ids));
            resp.Msg = string.Format("成功删除 {0} 条数据", count);
            return Common.JSONHelper.ObjectToJson(resp);


        }




        #endregion

        #endregion

        #region 刮奖活动V1

        /// <summary>
        /// 查询刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXLotteryV1(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string lotteryName = context.Request["LotteryName"];
            string lotteryType = context.Request["LotteryType"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' ", bllBase.WebsiteOwner));

            if (lotteryType == "scratch")
            {
                sbWhere.AppendFormat(" And (LotteryType='{0}'  or LotteryType is null) ", lotteryType);
            }
            else if (lotteryType == "all")
            {
                sbWhere.AppendFormat(" AND (LotteryType in ('shake','scratch') or LotteryType is null) ");
            }
            else
            {
                sbWhere.AppendFormat(" AND LotteryType='{0}' ", lotteryType);
            }

            if (!string.IsNullOrEmpty(lotteryName))
            {
                sbWhere.AppendFormat(" And LotteryName like '%{0}%'", lotteryName);
            }
            int totalCount = bllLottery.GetCount<WXLotteryV1>(sbWhere.ToString());
            List<WXLotteryV1> dataList = this.bllLottery.GetLit<WXLotteryV1>(pageSize, pageIndex, sbWhere.ToString(), " Status DESC, LotteryID DESC");
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].Awards = bllLottery.GetAwardsListV1(dataList[i].LotteryID);
            }
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });
        }

        /// <summary>
        /// 查询中奖结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXLotteryRecordV1(HttpContext context)
        {
            try
            {


                int pageIndex = Convert.ToInt32(context.Request["page"]);
                int pageSize = Convert.ToInt32(context.Request["rows"]);
                int lotteryId = int.Parse(context.Request["LotteryId"]);
                string userId = context.Request["UserId"];
                string awardId = context.Request["AwardId"];
                System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" LotteryId={0}", lotteryId));
                if (!string.IsNullOrEmpty(userId))
                {
                    sbWhere.AppendFormat(" And UserId ='{0}'", userId);
                }
                if (!string.IsNullOrEmpty(awardId))
                {
                    sbWhere.AppendFormat(" And WXAwardsId ={0}", awardId);
                }
                int totalCount = bllLottery.GetCount<WXLotteryRecordV1>(sbWhere.ToString());
                List<WXLotteryRecordV1> data = bllLottery.GetLit<WXLotteryRecordV1>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Phone))
                    {
                        UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                        if (userInfo != null)
                        {
                            if (string.IsNullOrEmpty(item.Name))
                            {
                                item.Name = userInfo.TrueName;
                            }
                            if (string.IsNullOrEmpty(item.Phone))
                            {
                                item.Phone = userInfo.Phone;
                            }
                        }

                    }
                }
                return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = data
                });
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

        }




        /// <summary>
        /// 查询默认中奖名单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXLotteryWinDataV1(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int lotteryId = int.Parse(context.Request["LotteryID"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" LotteryID={0}", lotteryId));
            int totalCount = this.bllLottery.GetCount<WXLotteryWinningDataV1>(sbWhere.ToString());
            List<WXLotteryWinningDataV1> dataList = this.bllLottery.GetLit<WXLotteryWinningDataV1>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });


        }
        /// <summary>
        /// 添加中奖名单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWinData(HttpContext context)
        {

            int lotteryId = int.Parse(context.Request["LotteryID"]);
            string userId = context.Request["UserID"];
            int awardId = int.Parse(context.Request["AwardId"]);
            if (bllUser.GetUserInfo(userId) == null)
            {
                resp.Msg = "用户名不存在，请检查";
                goto outoff;
            }
            if (bllLottery.GetCount<WXLotteryWinningDataV1>(string.Format("LotteryId={0} And UserId='{1}'", lotteryId, userId)) > 0)
            {
                resp.Msg = "该用户名已经在中奖名单中";
                goto outoff;
            }

            if (bllLottery.GetCount<WXAwardsV1>(string.Format("AutoID={0}", awardId)) - (bllBase.GetCount<WXLotteryWinningDataV1>(string.Format("WXAwardsId={0}", awardId)) + 1) < 0)
            {
                resp.Msg = "默认中奖奖项之和超过了该奖项的数量";
                goto outoff;
            }

            WXLotteryWinningDataV1 model = new WXLotteryWinningDataV1();
            model.LotteryId = lotteryId;
            model.UserId = userId;
            model.WXAwardsId = awardId;
            if (bllBase.Add(model))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "添加失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 编辑中奖名单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWinData(HttpContext context)
        {

            int autoId = int.Parse(context.Request["AutoID"]);
            string userId = context.Request["UserID"];
            int awardId = int.Parse(context.Request["AwardId"]);

            if (bllUser.GetUserInfo(userId) == null)
            {
                resp.Msg = "用户名不存在，请检查";
                goto outoff;
            }

            WXLotteryWinningDataV1 model = bllBase.Get<WXLotteryWinningDataV1>(string.Format("AutoID={0}", autoId));
            model.UserId = userId;
            model.WXAwardsId = awardId;
            if (bllBase.Update(model))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "更新失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 删除中奖名单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWinData(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bllLottery.Delete(new WXLotteryWinningDataV1(), string.Format("AutoID in({0})", ids)) == ids.Split(',').Length)
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "删除失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        ///  //标记为已领奖
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateIsGetPrize(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bllLottery.Update(new WXLotteryRecordV1(), " IsGetPrize=1", string.Format("AutoID in({0})", ids)) == ids.Split(',').Length)
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "更新失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 查询某个刮奖的所有奖项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryAwardsSelect(HttpContext context)
        {
            var lotteryId = Convert.ToInt32(context.Request["LotteryID"]);

            var awards = bllLottery.GetAwardsListV1(lotteryId); //bllBase.GetList<WXAwardsV1>(string.Format("LotteryId={0}", lotteryId));

            resp.IsSuccess = true;
            resp.Result = awards;
            resp.ExObj = awards;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private class WXLotteryModel
        {
            public string ThumbnailsPath { get; set; }
            public string LotteryName { get; set; }

            public string LotteryContent { get; set; }
            public int Status { get; set; }
            public int MaxCount { get; set; }
            public string BackGroundColor { get; set; }
            public string ShareImg { get; set; }
            public string ShareDesc { get; set; }
            public DateTime? StartTime { get; set; }
            public int IsGetPrizeFromMobile { get; set; }
            public List<BLLJIMP.Model.WXAwardsV1> Awards { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime? EndTime { get; set; }
            /// <summary>
            /// 上限类型：默认0每用户多少次，1为每天多少次
            /// </summary>
            public int LuckLimitType { get; set; }

            /// <summary>
            /// 每次消耗积分
            /// </summary>
            public string UsePoints { get; set; }
            public int WinLimitType { get; set; }
            /// <summary>
            /// 底部工具栏
            /// </summary>
            public string ToolbarButton { get; set; }

            /// <summary>
            /// 活动类型
            /// </summary>
            public string LotteryType { get; set; }
        }

        /// <summary>
        /// 添加刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXLotteryV1(HttpContext context)
        {

            WXLotteryModel requestModel = Common.JSONHelper.JsonToModel<WXLotteryModel>(context.Request["JsonData"]);
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (string.IsNullOrEmpty(requestModel.LotteryName))
                {
                    resp.Msg = "请输入活动名称";
                    goto outoff;

                }
                if (requestModel.Awards.Count <= 0)
                {
                    resp.Msg = "至少添加一个奖项";
                    goto outoff;
                }
                foreach (var item in requestModel.Awards)
                {
                    if (string.IsNullOrEmpty(item.PrizeName))
                    {
                        resp.Msg = "奖项名称不能为空";
                        goto outoff;
                    }
                    //if (item.PrizeCount <= 0)
                    //{
                    //    resp.Msg = string.Format("{0}奖项数量不能小于0", item.PrizeName);
                    //    goto outoff;
                    //}
                    //if (item.Probability <= 0)
                    //{
                    //    resp.Msg = string.Format("{0}中奖比例不能小于0", item.PrizeName);
                    //    goto outoff;
                    //}

                }

                if (requestModel.Awards.Sum(p => p.Probability) > 100)
                {
                    resp.Msg = "中奖比例之和不能大于100";
                    goto outoff;
                }

                WXLotteryV1 lotteryModel = new WXLotteryV1();
                lotteryModel.InsertDate = DateTime.Now;
                lotteryModel.LotteryID = int.Parse(bllBase.GetGUID(TransacType.DialogueID));
                lotteryModel.BackGroundColor = requestModel.BackGroundColor;
                lotteryModel.LotteryContent = requestModel.LotteryContent;
                lotteryModel.LotteryName = requestModel.LotteryName;
                lotteryModel.MaxCount = requestModel.MaxCount;
                lotteryModel.Status = requestModel.Status;
                lotteryModel.ThumbnailsPath = requestModel.ThumbnailsPath;
                lotteryModel.WebsiteOwner = bllBase.WebsiteOwner;
                lotteryModel.StartTime = requestModel.StartTime;
                lotteryModel.ShareImg = requestModel.ShareImg;
                lotteryModel.ShareDesc = requestModel.ShareDesc;
                lotteryModel.IsGetPrizeFromMobile = requestModel.IsGetPrizeFromMobile;
                lotteryModel.ToolbarButton = requestModel.ToolbarButton;
                lotteryModel.EndTime = requestModel.EndTime;
                lotteryModel.LuckLimitType = requestModel.LuckLimitType;

                if (!string.IsNullOrEmpty(requestModel.UsePoints))
                {
                    lotteryModel.UsePoints = Convert.ToInt32(requestModel.UsePoints);
                }
                lotteryModel.WinLimitType = requestModel.WinLimitType;
                lotteryModel.LotteryType = requestModel.LotteryType;
                lotteryModel.ToolbarButton = requestModel.ToolbarButton;
                if (!bllBase.Add(lotteryModel, tran))
                {
                    tran.Rollback();
                    resp.Msg = "添加活动失败";
                    goto outoff;
                }

                foreach (var item in requestModel.Awards)
                {
                    WXAwardsV1 award = new WXAwardsV1();
                    award.LotteryId = lotteryModel.LotteryID;
                    award.PrizeCount = item.PrizeCount;
                    award.PrizeName = item.PrizeName;
                    award.Probability = item.Probability;
                    award.Img = item.Img;
                    award.AwardsType = item.AwardsType;
                    award.Value = item.Value;
                    award.Description = item.Description;
                    if (award.AwardsType == 1)
                    {
                        int value;
                        if (!int.TryParse(award.Value, out value))
                        {
                            resp.Msg = "奖品请输入数字";
                            goto outoff;
                        }
                    }
                    if (!bllBase.Add(award, tran))
                    {
                        tran.Rollback();
                        resp.Msg = "添加奖项失败";
                        goto outoff;
                    }

                }
                tran.Commit();
                resp.Status = 1;
                if (lotteryModel.LotteryType == "scratch")
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加刮刮奖[id=" + lotteryModel.LotteryID + "]");
                }
                else
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加摇一摇[id=" + lotteryModel.LotteryID + "]");
                }
            }
            catch (Exception ex)
            {

                tran.Rollback();
                resp.Msg = ex.Message;

            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXLotteryV1(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            WXLotteryV1 lotteryModel = bllBase.Get<WXLotteryV1>(string.Format("LotteryID={0}", autoId));
            WXLotteryModel requestModel = Common.JSONHelper.JsonToModel<WXLotteryModel>(context.Request["JsonData"]);

            if (string.IsNullOrEmpty(requestModel.LotteryName))
            {
                resp.Msg = "请输入活动名称";
                goto outoff;

            }
            if (requestModel.Awards.Count <= 0)
            {
                resp.Msg = "至少添加一个奖项";
                goto outoff;
            }
            foreach (var item in requestModel.Awards)
            {
                if (string.IsNullOrEmpty(item.PrizeName))
                {
                    resp.Msg = "奖项名称不能为空";
                    goto outoff;
                }
                //if (item.PrizeCount <= 0)
                //{
                //    resp.Msg = string.Format("{0}奖项数量不能小于0", item.PrizeName);
                //    goto outoff;
                //}
                //if (item.Probability <= 0)
                //{
                //    resp.Msg = string.Format("{0}中奖比例不能小于0", item.PrizeName);
                //    goto outoff;
                //}

            }
            if (requestModel.Awards.Sum(p => p.Probability) > 100)
            {
                resp.Msg = "中奖比例之和不能大于100";
                goto outoff;
            }
            #region 检查是否删除了奖项
            List<WXAwardsV1> oldAwardList = bllBase.GetList<WXAwardsV1>(string.Format("LotteryId={0}", lotteryModel.LotteryID));//旧奖项
            if (requestModel.Awards.Where(p => p.AutoID > 0).Count() < oldAwardList.Count)//有删除的奖项
            {

                foreach (var item in oldAwardList)
                {
                    if (requestModel.Awards.Where(p => p.AutoID > 0).Where(p => p.AutoID == item.AutoID).Count() == 0)//该奖项被删除了
                    {
                        //检查该奖项是否有中奖记录，有的话不可以删除
                        int recordCount = bllBase.GetCount<WXLotteryRecordV1>(string.Format("WXAwardsId={0}", item.AutoID));
                        if (recordCount > 0)
                        {
                            resp.Msg = string.Format("{0}已经有人中奖,不能删除", item.PrizeName);
                            goto outoff;
                        }
                        else
                        {
                            //删除该奖项
                            int result = bllBase.Delete(item);

                        }


                    }
                }

            }
            #endregion

            lotteryModel.BackGroundColor = requestModel.BackGroundColor;
            lotteryModel.LotteryContent = requestModel.LotteryContent;
            lotteryModel.LotteryName = requestModel.LotteryName;
            lotteryModel.MaxCount = requestModel.MaxCount;
            lotteryModel.Status = requestModel.Status;
            lotteryModel.StartTime = requestModel.StartTime;
            lotteryModel.ThumbnailsPath = requestModel.ThumbnailsPath;
            lotteryModel.ShareImg = requestModel.ShareImg;
            lotteryModel.ShareDesc = requestModel.ShareDesc;
            lotteryModel.IsGetPrizeFromMobile = requestModel.IsGetPrizeFromMobile;
            lotteryModel.EndTime = requestModel.EndTime;
            lotteryModel.LuckLimitType = requestModel.LuckLimitType;
            lotteryModel.ToolbarButton = requestModel.ToolbarButton;
            if (!string.IsNullOrEmpty(requestModel.UsePoints))
            {
                lotteryModel.UsePoints = Convert.ToInt32(requestModel.UsePoints);
            }
            lotteryModel.WinLimitType = requestModel.WinLimitType;
            lotteryModel.ToolbarButton = requestModel.ToolbarButton;
            if (!bllBase.Update(lotteryModel))
            {
                resp.Msg = "更新活动失败";
                goto outoff;
            }
            foreach (var item in requestModel.Awards.Where(p => p.AutoID > 0))//更新的奖项
            {
                WXAwardsV1 award = bllBase.Get<WXAwardsV1>(string.Format("AutoID={0}", item.AutoID));
                award.PrizeCount = item.PrizeCount;
                award.PrizeName = item.PrizeName;
                award.Probability = item.Probability;
                award.Img = item.Img;
                award.AwardsType = item.AwardsType;
                award.Value = item.Value;
                award.Description = item.Description;
                if (award.AwardsType == 1)
                {
                    int value;
                    if (!int.TryParse(award.Value, out value))
                    {
                        resp.Msg = "奖品请输入数字";
                        goto outoff;
                    }
                }

                string sqlUpdate = string.Format(" Update ZCJ_WXAwardsV1 Set PrizeCount={0},PrizeName='{1}',Probability='{2}',Img='{3}',AwardsType='{4}',Value='{5}',Description='{6}' Where AutoID={7}", award.PrizeCount, award.PrizeName, award.Probability, award.Img, award.AwardsType, award.Value, award.Description, award.AutoID);
                if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sqlUpdate) <= 0)
                {

                    resp.Msg = "更新奖项失败";
                    goto outoff;
                }

            }
            foreach (var item in requestModel.Awards.Where(p => p.AutoID == 0))//添加的奖项
            {
                WXAwardsV1 award = new WXAwardsV1();
                award.LotteryId = lotteryModel.LotteryID;
                award.PrizeCount = item.PrizeCount;
                award.PrizeName = item.PrizeName;
                award.Probability = item.Probability;
                award.Img = item.Img;
                award.AwardsType = item.AwardsType;
                award.Value = item.Value;
                award.Description = item.Description;
                if (!bllBase.Add(award))
                {
                    resp.Msg = "添加奖项失败";
                    goto outoff;
                }

            }
            resp.Status = 1;
            if (lotteryModel.LotteryType == "scratch")
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "修改刮刮奖[id=" + lotteryModel.LotteryID + "]");
            }
            else
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "修改摇一摇[id=" + lotteryModel.LotteryID + "]");
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// 删除刮奖活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXLotteryV1(HttpContext context)
        {
            string ids = context.Request["ids"];
            string LotteryType = context.Request["LotteryType"];
            WXLotteryV1 model;
            foreach (var item in ids.Split(','))
            {
                model = bllLottery.Get<WXLotteryV1>(string.Format("LotteryID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    resp.Msg = "无权删除";
                    goto outoff;

                }

            }
            //关联删除
            int resultCount = bllLottery.Delete(new WXLotteryV1(), string.Format("LotteryID in ({0})", ids));//删除刮奖表

            bllLottery.Delete(new WXLotteryLogV1(), string.Format("LotteryId in ({0})", ids));//删除刮奖记录表
            bllLottery.Delete(new WXLotteryRecordV1(), string.Format("LotteryId in ({0})", ids));//删除中奖记录表
            bllLottery.Delete(new WXAwardsV1(), string.Format("LotteryId in ({0})", ids));//删除奖品表

            bllLottery.Delete(new WXLotteryWinningDataV1(), string.Format("LotteryId in ({0})", ids));//删除默认中奖名单
            if (resultCount > 0)
            {
                resp.Status = 1;
                if (LotteryType == "scratch")
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除刮刮奖[id=" + ids + "]");
                }
                else
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除摇一摇[id=" + ids + "]");
                }
            }
            resp.Msg = string.Format("成功删除了 {0} 条数据", resultCount);
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 重置刮奖
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ResetWXLotteryV1(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXLotteryV1 model;
            foreach (var item in ids.Split(','))
            {
                model = bllLottery.Get<WXLotteryV1>(string.Format("LotteryID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    resp.Msg = "无权操作";
                    goto outoff;
                }

            }


            bllLottery.Delete(new WXLotteryLogV1(), string.Format("LotteryId in ({0})", ids));
            bllLottery.Delete(new WXLotteryRecordV1(), string.Format("LotteryId in ({0})", ids));
            ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format(" Update ZCJ_WXAwardsV1 Set WinCount=0 Where LotteryId  in({0})", ids));
            resp.Msg = "已成功重置";
            goto outoff;
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        #endregion


        #region 微商城门店管理
        /// <summary>
        /// 查询微商城门店列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallStores(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string storeName = context.Request["StoreName"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(storeName))
            {
                sbWhere.AppendFormat(" And StoreName like '%{0}%'", storeName);
            }


            int totalCount = this.bllMall.GetCount<WXMallStores>(sbWhere.ToString());
            List<WXMallStores> dataList = this.bllMall.GetLit<WXMallStores>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });


        }


        /// <summary>
        /// 添加微商城商门店信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallStore(HttpContext context)
        {
            string storeName = context.Request["StoreName"];
            string storeAddress = context.Request["StoreAddress"];
            string isDefaultStore = context.Request["IsDefaultStore"];
            if (string.IsNullOrEmpty(storeName))
            {
                resp.Status = 0;
                resp.Msg = "请输入门店名称";
                Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(storeAddress))
            {
                resp.Status = 0;
                resp.Msg = "请输入门店地址";
                Common.JSONHelper.ObjectToJson(resp);
            }
            if (isDefaultStore.Equals("1"))//修改其它门店为非默认
            {
                bllJuActivity.Update(new WXMallStores(), "IsDefaultStore='0'", string.Format("WebsiteOwner='{0}'", bllBase.WebsiteOwner));

            }
            WXMallStores model = new WXMallStores();
            model.StoreName = storeName;
            model.StoreAddress = storeAddress;
            model.IsDefaultStore = isDefaultStore;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllMall.Add(model))
            {

                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑微商城门店信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallStore(HttpContext context)
        {


            string storeName = context.Request["StoreName"];
            string storeAddress = context.Request["StoreAddress"];
            int autoId = int.Parse(context.Request["AutoID"]);
            string isDefaultStore = context.Request["IsDefaultStore"];
            if (string.IsNullOrEmpty(storeName))
            {
                resp.Status = 0;
                resp.Msg = "请输入门店名称";
                Common.JSONHelper.ObjectToJson(resp);
            }

            var model = bllMall.Get<WXMallStores>(string.Format("AutoID={0}", autoId));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "门店不存在";
                Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                Common.JSONHelper.ObjectToJson(resp);

            }
            model.StoreName = storeName;
            model.StoreAddress = storeAddress;
            model.IsDefaultStore = isDefaultStore;
            if (bllMall.Update(model))
            {
                if (isDefaultStore.Equals("1"))//修改其它门店为非默认
                {
                    bllMall.Update(new WXMallStores(), "IsDefaultStore='0'", string.Format("WebsiteOwner='{0}' And AutoID !='{1}'", bllBase.WebsiteOwner, autoId));

                }

                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除微商城门店信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallStore(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXMallStores model;
            foreach (var item in ids.Split(','))
            {
                model = bllMall.Get<WXMallStores>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllJuActivity.Delete(new WXMallStores(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion



        //#region 限制查询的 游戏活动
        ///// <summary>
        ///// 查询
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryGameActivityQueryLimit(HttpContext context)
        //{

        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    string Param = context.Request["Param"];

        //    StringBuilder sbWhere = new StringBuilder(" 1=1");
        //    if (!string.IsNullOrEmpty(Param))
        //    {
        //        sbWhere.AppendFormat(" And ( ActivityID like '%{0}%' Or ActivityName like '%{0}%')", Param);
        //    }
        //    int totalCount = this.juActivityBll.GetCount<GameActivityQueryLimit>(sbWhere.ToString());
        //    List<GameActivityQueryLimit> dataList = this.juActivityBll.GetLit<GameActivityQueryLimit>(rows, page, sbWhere.ToString());

        //    return Common.JSONHelper.ListToEasyUIJson(totalCount, dataList);


        //}


        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddGameActivityQueryLimit(HttpContext context)
        //{
        //    string GameActivityName = context.Request["GameActivityName"];
        //    int GameActivityID = int.Parse(context.Request["GameActivityID"]);

        //    if (new BLLActivity("").GetActivityInfoByActivityID(GameActivityID.ToString()) == null)
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "活动ID不存在";
        //        return Common.JSONHelper.ObjectToJson(resp);


        //    }


        //    GameActivityQueryLimit model = new GameActivityQueryLimit();
        //    model.ActivityID = GameActivityID;
        //    model.ActivityName = GameActivityName;

        //    if (juActivityBll.Add(model))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "添加成功";

        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "添加失败";
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);


        //}

        ///// <summary>
        ///// 编辑
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string EditGameActivityQueryLimit(HttpContext context)
        //{

        //    int AutoID = int.Parse(context.Request["AutoID"]);
        //    string GameActivityName = context.Request["GameActivityName"];
        //    int GameActivityID = int.Parse(context.Request["GameActivityID"]);
        //    if (new BLLActivity("").GetActivityInfoByActivityID(GameActivityID.ToString()) == null)
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "活动ID不存在";
        //        return Common.JSONHelper.ObjectToJson(resp);


        //    }

        //    var model = juActivityBll.Get<GameActivityQueryLimit>(string.Format("AutoID={0}", AutoID));
        //    if (model == null)
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "不存在";
        //        Common.JSONHelper.ObjectToJson(resp);

        //    }

        //    model.ActivityID = GameActivityID;
        //    model.ActivityName = GameActivityName;
        //    if (juActivityBll.Update(model))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "保存成功";

        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "保存失败";
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string DeleteGameActivityQueryLimit(HttpContext context)
        //{
        //    string ids = context.Request["ids"];


        //    int count = juActivityBll.Delete(new GameActivityQueryLimit(), string.Format("AutoID in ({0})", ids));
        //    return string.Format("成功删除了 {0} 条数据", count);

        //}

        //#endregion


        #region 微网站 模块

        /// <summary>
        /// 更新微网站配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateCompanyWebsiteConfig(HttpContext context)
        {

            currentWebSiteInfo = bllBase.GetWebsiteInfoModelFromDataBase();

            string websiteTitle = context.Request["WebsiteTitle"];//网站标题
            string copyright = context.Request["Copyright"];//
            string websiteImage = context.Request["WebsiteImage"];//网站缩略图
            string websiteDescription = context.Request["WebsiteDescription"];//网站描述
            string shopNavGroupName = context.Request["ShopNavGroupName"];
            string shopAdType = context.Request["ShopAdType"];
            string buttomtoolbars = context.Request["Buttomtoolbars"];//底部工具栏
            string memberStandard = context.Request["MemberStandard"];//会员标准
            string MemberStandardField = context.Request["MemberStandardField"];//会员标准字段
            string haveComment = context.Request["HaveComment"];//是否显示评论
            string memberStandardDescription = context.Request["MemberStandardDescription"];
            string myCardCouponsTitle = context.Request["MyCardCouponsTitle"];//优惠券显示标题
            string smsSignature = context.Request["SmsSignature"];//短信签名
            string wxAccountNickName = context.Request["WeixinAccountNickName"];//微信公众平台显示名称
            string distributionQRCodeIcon = context.Request["DistributionQRCodeIcon"];
            string articleToolBarGrous = context.Request["ArticleToolBarGrous"];//文章详情页底部导航
            string activityToolBarGrous = context.Request["ActivityToolBarGrous"];//活动详情页底部导航
            string groupBuyIndexUrl = context.Request["GroupBuyIndexUrl"];//团购首页链接
            if (string.IsNullOrWhiteSpace(memberStandard)) memberStandard = "0";
            if (string.IsNullOrWhiteSpace(haveComment)) haveComment = "0";
            string noPermissionsPage = context.Request["NoPermissionsPage"];//无权限跳转页面
            string personalCenterLink = context.Request["PersonalCenterLink"];//个人中心链接
            string lowestAmount = context.Request["LowestAmount"];//最低提现金额
            string hexiaoCode = context.Request["HexiaoCode"];//核销码
            string nimAppKey = context.Request["NIMAppKey"];//云信App Key
            string nimAppSecret = context.Request["NIMAppSecret"];//云信App Secret
            string aliAppKey = context.Request["AliAppKey"];//阿里AppKey
            string aliAppSecret = context.Request["AliAppSecret"];//阿里 AppSecret
            string tel = context.Request["Tel"];//联系电话
            string qq = context.Request["QQ"];//QQ
            string disableReplaceDistributonOwner = context.Request["DisableReplaceDistributonOwner"];//是否替换系统分销员
            string mallOrderPaySuccessUrl = context.Request["MallOrderPaySuccessUrl"];//MallOrderPaySuccessUrl
            string userCenterFieldJson = context.Request["UserCenterFieldJson"];//UserCenterFieldJson
            string userInfoFirstShow = context.Request["UserInfoFirstShow"];//UserInfoFirstShow
            string appPushType = context.Request["AppPushType"];//app推送类型
            string appPushAppId = context.Request["AppPushAppId"];//app推送第三方AppId
            string appPushAppKey = context.Request["AppPushAppKey"];//app推送第三方AppKey
            string appPushAppSecret = context.Request["AppPushAppSecret"];//app推送第三方AppSecret
            string appPushMasterSecret = context.Request["AppPushMasterSecret"];//app推送服务器用MasterSecret
            string elemeAppKey = context.Request["ElemeAppKey"];//饿了么key
            string elemeAppSecret = context.Request["ElemeAppSecret"];//饿了么Secret

            int isDisableKefu = int.Parse(context.Request["IsDisableKefu"].ToString());//是否禁用客服
            string kefuUrl = context.Request["KefuUrl"];//客服链接
            string kefuImage = context.Request["KefuImage"];//客服图标
            string kefuOnLineReply = context.Request["KefuOnLineReply"];
            string kefuOffLineReply = context.Request["KefuOffLineReply"];

            string isEnableCustomizeLoginPage=context.Request["IsEnableCustomizeLoginPage"];
            string loginConfigJson=context.Request["LoginConfigJson"];
            string outletsSearchRange = context.Request["OutletsSearchRange"];//门店搜索范围




            decimal acount = 0;
            if (!decimal.TryParse(lowestAmount, out acount))
            {
                resp.Status = 0;
                resp.Msg = "请输入数字";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (new BLLWebSite().UpdateCompanyWebsiteConfig(websiteTitle, copyright, websiteImage, websiteDescription, shopNavGroupName, shopAdType, buttomtoolbars,
                memberStandard, haveComment, memberStandardDescription, myCardCouponsTitle, wxAccountNickName, distributionQRCodeIcon, articleToolBarGrous, activityToolBarGrous, groupBuyIndexUrl, noPermissionsPage, personalCenterLink, acount, tel, qq, isDisableKefu, kefuUrl, kefuImage, kefuOnLineReply, kefuOffLineReply, isEnableCustomizeLoginPage, loginConfigJson, outletsSearchRange))
            {
                if (!string.IsNullOrWhiteSpace(MemberStandardField) && (memberStandard == "2" || memberStandard == "3"))
                {
                    BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
                    List<TableFieldMapping> baseFieldList = bllTableFieldMap.GetTableFieldMap(null, "ZCJ_UserInfo");
                    List<TableFieldMapping> webFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, true);
                    List<string> memberStandardFieldList = MemberStandardField.Split(',').ToList();
                    foreach (var item in webFieldList)
                    {
                        if (memberStandardFieldList.Contains(item.Field))
                        {
                            if (item.IsDelete != 0)
                            {
                                item.IsDelete = 0;
                                bllTableFieldMap.Update(item);
                            }
                        }
                        else
                        {
                            item.IsDelete = 1;
                            bllTableFieldMap.Update(item);
                        }
                    }
                    List<string> webFieldStringList = webFieldList.Select(p => p.Field).ToList();
                    foreach (var item in baseFieldList.Where(p => !webFieldStringList.Contains(p.Field)))
                    {
                        if (!memberStandardFieldList.Contains(item.Field)) continue;
                        item.AutoId = 0;
                        item.WebSiteOwner = bllTableFieldMap.WebsiteOwner;
                        bllTableFieldMap.Add(item);
                    }
                }

                currentWebSiteInfo.SmsSignature = smsSignature;
                currentWebSiteInfo.HexiaoCode = hexiaoCode;
                currentWebSiteInfo.NIMAppKey = nimAppKey;
                currentWebSiteInfo.NIMAppSecret = nimAppSecret;
                currentWebSiteInfo.AliAppKey = aliAppKey;
                currentWebSiteInfo.AliAppSecret = aliAppSecret;
                currentWebSiteInfo.DisableReplaceDistributonOwner = int.Parse(disableReplaceDistributonOwner);
                currentWebSiteInfo.MallOrderPaySuccessUrl = mallOrderPaySuccessUrl;
                currentWebSiteInfo.UserCenterFieldJson = userCenterFieldJson;
                currentWebSiteInfo.UserInfoFirstShow = Convert.ToInt32(userInfoFirstShow);
                currentWebSiteInfo.AppPushType = appPushType;
                currentWebSiteInfo.AppPushAppId = appPushAppId;
                currentWebSiteInfo.AppPushAppKey = appPushAppKey;
                currentWebSiteInfo.AppPushAppSecret = appPushAppSecret;
                currentWebSiteInfo.AppPushMasterSecret = appPushMasterSecret;
                currentWebSiteInfo.ElemeAppKey = elemeAppKey;
                currentWebSiteInfo.ElemeAppSecret = elemeAppSecret;
                bllBase.Update(currentWebSiteInfo);
                resp.Status = 1;
                resp.Msg = "保存成功";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Website, BLLJIMP.Enums.EnumLogTypeAction.Config, bllLog.GetCurrUserID(), "全局设置[" + bllLog.GetCurrUserID() + "]");
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }


        #region 微网站幻灯片管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCompanyWebsiteProjector(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string projectorName = context.Request["ProjectorName"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(projectorName))
            {
                sbWhere.AppendFormat(" And ProjectorName like '%{0}%'", projectorName);
            }

            int totalCount = this.bllJuActivity.GetCount<CompanyWebsite_Projector>(sbWhere.ToString());
            List<CompanyWebsite_Projector> dataList = this.bllJuActivity.GetLit<CompanyWebsite_Projector>(pageSize, pageIndex, sbWhere.ToString(), " PlayIndex ASC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCompanyWebsiteProjector(HttpContext context)
        {
            string projectorName = context.Request["ProjectorName"];
            string projectorDescription = context.Request["ProjectorDescription"];
            string playIndex = context.Request["PlayIndex"];
            string projectorImage = context.Request["ProjectorImage"];
            string isShow = context.Request["IsShow"];
            string projectorType = context.Request["ProjectorType"];
            string projectorTypeValue = context.Request["ProjectorTypeValue"];

            if (string.IsNullOrEmpty(projectorName))
            {
                resp.Status = 0;
                resp.Msg = "请输名称";
                Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(projectorImage))
            {
                resp.Status = 0;
                resp.Msg = "请上传图片";
                Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyWebsite_Projector model = new CompanyWebsite_Projector();
            model.ProjectorName = projectorName;
            model.ProjectorDescription = projectorDescription;
            model.PlayIndex = int.Parse(playIndex);
            model.ProjectorImage = projectorImage;
            model.IsShow = isShow;
            model.ProjectorImage = projectorImage;
            model.ProjectorType = projectorType;
            model.ProjectorTypeValue = projectorTypeValue;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllJuActivity.Add(model))
            {

                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditCompanyWebsiteProjector(HttpContext context)
        {
            string projectorName = context.Request["ProjectorName"];
            string projectorDescription = context.Request["ProjectorDescription"];
            string playIndex = context.Request["PlayIndex"];
            string projectorImage = context.Request["ProjectorImage"];
            string isShow = context.Request["IsShow"];
            string projectorType = context.Request["ProjectorType"];
            string projectorTypeValue = context.Request["ProjectorTypeValue"];
            string autoID = context.Request["AutoID"];
            if (string.IsNullOrEmpty(projectorName))
            {
                resp.Status = 0;
                resp.Msg = "请输名称";
                Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(projectorImage))
            {
                resp.Status = 0;
                resp.Msg = "请上传图片";
                Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyWebsite_Projector model = bllJuActivity.Get<CompanyWebsite_Projector>(string.Format("AutoID='{0}'", autoID));

            model.ProjectorName = projectorName;
            model.ProjectorDescription = projectorDescription;
            model.PlayIndex = int.Parse(playIndex);
            model.ProjectorImage = projectorImage;
            model.IsShow = isShow;
            model.ProjectorImage = projectorImage;
            model.ProjectorType = projectorType;
            model.ProjectorTypeValue = projectorTypeValue;

            if (bllJuActivity.Update(model))
            {

                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteCompanyWebsiteProjector(HttpContext context)
        {
            string ids = context.Request["ids"];
            CompanyWebsite_Projector model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<CompanyWebsite_Projector>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllJuActivity.Delete(new CompanyWebsite_Projector(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion

        #region 微网站底部工具栏管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCompanyWebsiteToolBar(HttpContext context)
        {

            //int pageIndex = Convert.ToInt32(context.Request["page"]);
            //int pageSize = Convert.ToInt32(context.Request["rows"]);
            string toolBarName = context.Request["ToolBarName"];
            string keyType = context.Request["KeyType"];
            string useType = context.Request["UseType"];
            string isSystem = context.Request["IsSystem"];
            string isPc = context.Request["IsPc"];
            BLLCompanyWebSite bllCompanyWebSite = new BLLCompanyWebSite();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbWhere1 = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bllCompanyWebSite.WebsiteOwner);
            sbWhere1.AppendFormat(" WebsiteOwner Is null");
            if (!string.IsNullOrEmpty(toolBarName))
            {
                sbWhere.AppendFormat(" And ToolBarName like '%{0}%'", toolBarName);
                sbWhere1.AppendFormat(" And ToolBarName like '%{0}%'", toolBarName);
            }
            if (!string.IsNullOrEmpty(isPc))
            {
                sbWhere.AppendFormat(" And IsPc={0}", isPc);
                sbWhere1.AppendFormat(" And IsPc={0}", isPc);
            }
            else
            {
                sbWhere.AppendFormat(" And IsNull(IsPc,0)=0");
                sbWhere1.AppendFormat(" And IsNull(IsPc,0)=0");
            }
            if (!string.IsNullOrWhiteSpace(keyType))
            {
                sbWhere.AppendFormat(" And KeyType = '{0}'", keyType);
                sbWhere1.AppendFormat(" And KeyType = '{0}'", keyType);
            }
            if (!string.IsNullOrWhiteSpace(useType))
            {
                sbWhere.AppendFormat(" And UseType = '{0}'", useType);
                sbWhere1.AppendFormat(" And UseType = '{0}'", useType);
            }
            List<CompanyWebsite_ToolBar> dataList = new List<CompanyWebsite_ToolBar>();
            if (isSystem != "1")
            {
                dataList = bllCompanyWebSite.GetList<CompanyWebsite_ToolBar>(sbWhere.ToString());
                List<CompanyWebsite_ToolBar> dataList1 = bllCompanyWebSite.GetList<CompanyWebsite_ToolBar>(sbWhere1.ToString());
                List<int> nList = dataList.Select(p => p.BaseID).Distinct().ToList();
                foreach (CompanyWebsite_ToolBar item in dataList1.Where(p => !nList.Contains(p.AutoID)))
                {
                    dataList.Add(item);
                }
            }
            else
            {
                dataList = bllCompanyWebSite.GetList<CompanyWebsite_ToolBar>(sbWhere1.ToString());
            }

            int totalCount = dataList.Count;
            dataList = dataList.OrderBy(p => p.KeyType).ThenBy(p => p.PlayIndex).ToList();
            ZentCloud.Common.MyCategories m = new ZentCloud.Common.MyCategories();
            List<ZentCloud.Common.Model.MyCategoryModel> listCate = m.GetCommCateModelList<CompanyWebsite_ToolBar>("AutoID", "PreID", "ToolBarName", dataList);

            List<CompanyWebsite_ToolBar> showList = new List<CompanyWebsite_ToolBar>();
            foreach (ListItem item in m.GetCateListItem(listCate, 0))
            {
                try
                {
                    CompanyWebsite_ToolBar tmpModel = (CompanyWebsite_ToolBar)dataList.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0].Clone();
                    tmpModel.ToolBarName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }
            //showList = showList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = showList
            });
        }

        /// <summary>
        /// 列出该分类导航 （层级关系）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCompanyWebsiteToolBarPreSelect(HttpContext context)
        {
            string useType = context.Request["use_type"];
            string keyType = context.Request["key_type"];
            string isSystem = context.Request["is_system"];
            BLLCompanyWebSite bllCompanyWebSite = new BLLCompanyWebSite();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbWhere1 = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bllCompanyWebSite.WebsiteOwner);
            sbWhere1.AppendFormat(" WebsiteOwner Is null");
            if (!string.IsNullOrWhiteSpace(keyType))
            {
                sbWhere.AppendFormat(" And KeyType = '{0}'", keyType);
                sbWhere1.AppendFormat(" And KeyType = '{0}'", keyType);
            }
            if (!string.IsNullOrWhiteSpace(useType))
            {
                sbWhere.AppendFormat(" And UseType = '{0}'", useType);
                sbWhere1.AppendFormat(" And UseType = '{0}'", useType);
            }
            List<CompanyWebsite_ToolBar> dataList = new List<CompanyWebsite_ToolBar>();
            if (isSystem != "1")
            {
                dataList = bllCompanyWebSite.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,KeyType,ToolBarName,PreID,BaseID");
                List<CompanyWebsite_ToolBar> dataList1 = bllCompanyWebSite.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere1.ToString(), "AutoID,KeyType,ToolBarName,PreID");
                List<int> nList = dataList.Select(p => p.BaseID).Distinct().ToList();
                foreach (CompanyWebsite_ToolBar item in dataList1.Where(p => !nList.Contains(p.AutoID)))
                {
                    dataList.Add(item);
                }
            }
            else
            {
                dataList = bllCompanyWebSite.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere1.ToString(), "AutoID,KeyType,ToolBarName,PreID");
            }
            dataList = dataList.OrderBy(p => p.KeyType).ThenBy(p => p.PlayIndex).ToList();
            ZentCloud.Common.MyCategories m = new ZentCloud.Common.MyCategories();
            List<ZentCloud.Common.Model.MyCategoryModel> listCate = m.GetCommCateModelList<CompanyWebsite_ToolBar>("AutoID", "PreID", "ToolBarName", dataList);
            List<dynamic> showList = new List<dynamic>();
            foreach (ListItem item in m.GetCateListItem(listCate, 0))
            {
                CompanyWebsite_ToolBar tmpModel = (CompanyWebsite_ToolBar)dataList.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0].Clone();
                showList.Add(new
                {
                    id = tmpModel.AutoID,
                    name = item.Text
                });
            }
            return Common.JSONHelper.ObjectToJson(showList);
        }

        /// <summary>
        /// 添加
        /// </summa
        /// ry>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCompanyWebsiteToolBar(HttpContext context)
        {
            string toolBarName = context.Request["ToolBarName"];
            string toolBarDescription = context.Request["ToolBarDescription"];
            string playIndex = context.Request["PlayIndex"];
            string toolBarImage = context.Request["ToolBarImage"];
            string isShow = context.Request["IsShow"];
            string toolBarType = context.Request["ToolBarType"];
            string toolBarTypeValue = context.Request["ToolBarTypeValue"];
            string stype = context.Request["Stype"];
            string svalue = context.Request["Svalue"];
            string stext = context.Request["Stext"];
            string keyType = context.Request["KeyType"];
            string useType = context.Request["UseType"];
            string actBgColor = context.Request["ActBgColor"];
            string bgColor = context.Request["BgColor"];
            string actColor = context.Request["ActColor"];
            string color = context.Request["Color"];
            string icocolor = context.Request["IcoColor"];
            int preID = Convert.ToInt32(context.Request["PreID"]);
            string imageUrl = context.Request["ImageUrl"];
            string isSystem = context.Request["IsSystem"];
            int visibleSet = Convert.ToInt32(context.Request["VisibleSet"]);
            string permissionGroup = context.Request["PermissionGroup"];
            string baseID = context.Request["BaseID"];
            string actBgImage = context.Request["ActBgImage"];
            string bgImage = context.Request["BgImage"];
            string icoPosition = context.Request["IcoPosition"];
            int isPc = !string.IsNullOrEmpty(context.Request["IsPc"]) ? int.Parse(context.Request["IsPc"]) : 0;
            if (string.IsNullOrEmpty(toolBarName))
            {
                resp.Status = 0;
                resp.Msg = "请输名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //if (string.IsNullOrEmpty(toolBarImage))
            //{
            //    resp.Status = 0;
            //    resp.Msg = "请上传图片";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}
            CompanyWebsite_ToolBar model = new CompanyWebsite_ToolBar();
            model.ToolBarName = toolBarName;
            model.ToolBarDescription = toolBarDescription;
            model.PlayIndex = int.Parse(playIndex);
            model.ToolBarImage = toolBarImage;
            model.IsShow = isShow;
            model.ToolBarImage = toolBarImage;
            model.ToolBarType = toolBarType;
            model.ToolBarTypeValue = toolBarTypeValue;
            model.Stype = stype;
            model.Svalue = svalue;
            model.Stext = stext;
            model.KeyType = keyType;
            model.UseType = useType;
            model.ActBgColor = actBgColor;
            model.BgColor = bgColor;
            model.ActColor = actColor;
            model.Color = color;
            model.IcoColor = icocolor;
            model.PreID = preID;
            model.ImageUrl = imageUrl;
            model.ActBgImage = actBgImage;
            model.BgImage = bgImage;
            model.IcoPosition = Convert.ToInt32(icoPosition);
            model.IsPc = isPc;
            model.VisibleSet = visibleSet;
            if (!string.IsNullOrWhiteSpace(isSystem) && isSystem == "1")
            {
                model.WebsiteOwner = null;
            }
            else
            {
                model.WebsiteOwner = bllBase.WebsiteOwner;
            }
            if (permissionGroup != null)
            {
                model.PermissionGroup = permissionGroup;
            }
            if (!string.IsNullOrWhiteSpace(baseID))
            {
                model.BaseID = Convert.ToInt32(baseID);
            }

            if (bllBase.Add(model))
            {

                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditCompanyWebsiteToolBar(HttpContext context)
        {
            string toolBarName = context.Request["ToolBarName"];
            string toolBarDescription = context.Request["ToolBarDescription"];
            string playIndex = context.Request["PlayIndex"];
            string toolBarImage = context.Request["ToolBarImage"];
            string isShow = context.Request["IsShow"];
            string toolBarType = context.Request["ToolBarType"];
            string toolBarTypeValue = context.Request["ToolBarTypeValue"];
            string stype = context.Request["Stype"];
            string svalue = context.Request["Svalue"];
            string stext = context.Request["Stext"];
            string keyType = context.Request["KeyType"];
            string actBgColor = context.Request["ActBgColor"];
            string bgColor = context.Request["BgColor"];
            string actColor = context.Request["ActColor"];
            string color = context.Request["Color"];
            string icocolor = context.Request["IcoColor"];
            string autoID = context.Request["AutoID"];
            int preID = Convert.ToInt32(context.Request["PreID"]);
            string imageUrl = context.Request["ImageUrl"];
            string isSystem = context.Request["IsSystem"];
            int visibleSet = Convert.ToInt32(context.Request["VisibleSet"]);
            string permissionGroup = context.Request["PermissionGroup"];
            string actBgImage = context.Request["ActBgImage"];
            string bgImage = context.Request["BgImage"];
            string icoPosition = context.Request["IcoPosition"];
            int isPc = !string.IsNullOrEmpty(context.Request["IsPc"]) ? int.Parse(context.Request["IsPc"]) : 0;
            if (string.IsNullOrEmpty(toolBarName))
            {
                resp.Status = 0;
                resp.Msg = "请输名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //if (string.IsNullOrEmpty(toolBarImage))
            //{
            //    resp.Status = 0;
            //    resp.Msg = "请上传图片";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}
            CompanyWebsite_ToolBar model = bllJuActivity.Get<CompanyWebsite_ToolBar>(string.Format("AutoID='{0}'", autoID));

            model.ToolBarName = toolBarName;
            model.ToolBarDescription = toolBarDescription;
            model.PlayIndex = int.Parse(playIndex);
            model.ToolBarImage = toolBarImage;
            model.IsShow = isShow;
            model.ToolBarImage = toolBarImage;
            model.ToolBarType = toolBarType;
            model.ToolBarTypeValue = toolBarTypeValue;
            model.Stype = stype;
            model.Svalue = svalue;
            model.Stext = stext;
            model.KeyType = keyType;
            model.ActBgColor = actBgColor;
            model.BgColor = bgColor;
            model.ActColor = actColor;
            model.Color = color;
            model.IcoColor = icocolor;
            model.PreID = preID;
            model.ImageUrl = imageUrl;
            model.ActBgImage = actBgImage;
            model.BgImage = bgImage;
            model.IcoPosition = Convert.ToInt32(icoPosition);
            model.IsPc = isPc;
            model.VisibleSet = visibleSet;
            if (!string.IsNullOrWhiteSpace(isSystem) && isSystem == "1")
            {
                model.WebsiteOwner = null;
            }
            else
            {
                model.WebsiteOwner = bllBase.WebsiteOwner;
            }
            if (permissionGroup != null)
            {
                model.PermissionGroup = permissionGroup;
            }

            if (bllBase.Update(model))
            {

                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteCompanyWebsiteToolBar(HttpContext context)
        {
            string ids = context.Request["ids"];
            //CompanyWebsite_ToolBar model;
            //foreach (var item in ids.Split(','))
            //{
            //    model = bllJuActivity.Get<CompanyWebsite_ToolBar>(string.Format("AutoID={0}", item));
            //    if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
            //    {
            //        return "无权删除";

            //    }

            //}
            int count = bllJuActivity.Delete(new CompanyWebsite_ToolBar(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }
        /// <summary>
        /// 排序保存
        /// </summary>
        /// <returns></returns>
        private string UpdateSortIndex(HttpContext context)
        {
            string id = context.Request["ToolBarID"];
            int sortValue = int.Parse(context.Request["SortIndex"]);
            CompanyWebsite_ToolBar toobar = bllConpanyWebsite.GetCompanyWebsiteToolBarById(id);
            toobar.PlayIndex = sortValue;
            if (bllConpanyWebsite.Update(toobar))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Status = 0;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        #endregion

        #region 微网站底部图标导航管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCompanyWebsiteNavigate(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string navigateName = context.Request["NavigateName"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(navigateName))
            {
                sbWhere.AppendFormat(" And NavigateName like '%{0}%'", navigateName);
            }

            int totalCount = this.bllJuActivity.GetCount<CompanyWebsite_Navigate>(sbWhere.ToString());
            List<CompanyWebsite_Navigate> data = this.bllJuActivity.GetLit<CompanyWebsite_Navigate>(pageSize, pageIndex, sbWhere.ToString(), " PlayIndex ASC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = data
});


        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCompanyWebsiteNavigate(HttpContext context)
        {
            string navigateName = context.Request["NavigateName"];
            string navigateDescription = context.Request["NavigateDescription"];
            string playIndex = context.Request["PlayIndex"];
            string navigateImage = context.Request["NavigateImage"];
            string isShow = context.Request["IsShow"];
            string navigateType = context.Request["NavigateType"];
            string navigateTypeValue = context.Request["NavigateTypeValue"];

            if (string.IsNullOrEmpty(navigateName))
            {
                resp.Msg = "请输名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(navigateImage))
            {
                resp.Msg = "请上传图片";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyWebsite_Navigate model = new CompanyWebsite_Navigate();
            model.NavigateName = navigateName;
            model.NavigateDescription = navigateDescription;
            model.PlayIndex = int.Parse(playIndex);
            model.NavigateImage = navigateImage;
            model.IsShow = isShow;
            model.NavigateImage = navigateImage;
            model.NavigateType = navigateType;
            model.NavigateTypeValue = navigateTypeValue;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllJuActivity.Add(model))
            {

                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditCompanyWebsiteNavigate(HttpContext context)
        {
            string navigateName = context.Request["NavigateName"];
            string navigateDescription = context.Request["NavigateDescription"];
            string playIndex = context.Request["PlayIndex"];
            string navigateImage = context.Request["NavigateImage"];
            string isShow = context.Request["IsShow"];
            string navigateType = context.Request["NavigateType"];
            string navigateTypeValue = context.Request["NavigateTypeValue"];
            string autoID = context.Request["AutoID"];
            if (string.IsNullOrEmpty(navigateName))
            {
                resp.Status = 0;
                resp.Msg = "请输名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(navigateImage))
            {
                resp.Status = 0;
                resp.Msg = "请上传图片";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyWebsite_Navigate model = bllJuActivity.Get<CompanyWebsite_Navigate>(string.Format("AutoID='{0}'", autoID));
            model.NavigateName = navigateName;
            model.NavigateDescription = navigateDescription;
            model.PlayIndex = int.Parse(playIndex);
            model.NavigateImage = navigateImage;
            model.IsShow = isShow;
            model.NavigateImage = navigateImage;
            model.NavigateType = navigateType;
            model.NavigateTypeValue = navigateTypeValue;

            if (bllJuActivity.Update(model))
            {

                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteCompanyWebsiteNavigate(HttpContext context)
        {
            string ids = context.Request["ids"];
            CompanyWebsite_Navigate model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<CompanyWebsite_Navigate>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllJuActivity.Delete(new CompanyWebsite_Navigate(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion


        #region 模板管理模块

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCompanyWebsiteTemplate(HttpContext context)
        {
            string templateName = context.Request["TemplateName"];
            string templatePath = context.Request["TemplatePath"];
            string templateThumbnail = context.Request["TemplateThumbnail"];
            if (string.IsNullOrEmpty(templatePath))
            {
                resp.Status = -1;
                resp.Msg = "请输入模板目录路径";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyWebsiteTemplate model = new CompanyWebsiteTemplate();
            model.TemplateName = templateName;
            model.TemplatePath = templatePath;
            model.TemplateThumbnail = templateThumbnail;
            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";


            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);




        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditCompanyWebsiteTemplate(HttpContext context)
        {
            int autoID = int.Parse(context.Request["AutoID"]);
            string templateName = context.Request["TemplateName"];
            string templatePath = context.Request["TemplatePath"];
            string templateThumbnail = context.Request["TemplateThumbnail"];
            if (string.IsNullOrEmpty(templatePath))
            {
                resp.Status = -1;
                resp.Msg = "请输入模板目录路径";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            CompanyWebsiteTemplate model = new CompanyWebsiteTemplate();
            model.AutoID = autoID;
            model.TemplateName = templateName;
            model.TemplatePath = templatePath;
            model.TemplateThumbnail = templateThumbnail;
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "更新成功";


            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);




        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteCompanyWebsiteTemplate(HttpContext context)
        {

            string ids = context.Request["Ids"];
            int count = bllJuActivity.Delete(new CompanyWebsiteTemplate(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了{0}条数据", count);

        }


        /// <summary>
        /// 查询模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCompanyWebsiteTemplate(HttpContext context)
        {


            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string templateName = context.Request["TemplateName"];
            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(templateName))
            {
                sbWhere.AppendFormat("And TemplateName like'%{0}%'", templateName);
            }
            int totalCount = bllJuActivity.GetCount<CompanyWebsiteTemplate>(sbWhere.ToString());
            List<CompanyWebsiteTemplate> dataList = new List<CompanyWebsiteTemplate>();
            dataList = bllJuActivity.GetLit<CompanyWebsiteTemplate>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});



        }



        /// <summary>
        /// 更新企业微网站模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateCompanyWebSiteTemplate(HttpContext context)
        {

            currentWebSiteInfo.CompanyWebSiteTemplateName = context.Request["CompanyWebSiteTemplateName"];
            if (this.bllJuActivity.Update(currentWebSiteInfo))
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
            }
            else
            {
                resp.Status = 1;
                resp.Msg = "更新失败！";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }



        #endregion

        #endregion

        private string SetPwd(HttpContext context)
        {
            string pwdOld = context.Request["PwdOld"];
            string pwdNew = context.Request["PwdNew"];
            string pwdNewSure = context.Request["PwdNewSure"];
            if (string.IsNullOrEmpty(pwdOld))
            {
                resp.Msg = "旧密码不能为空";
                goto outoff;
            }
            if (!pwdOld.Equals(currentUserInfo.Password))
            {
                resp.Msg = "旧密码错误";
                goto outoff;
            }
            if (string.IsNullOrEmpty(pwdNew))
            {
                resp.Msg = "新密码不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(pwdNewSure))
            {
                resp.Msg = "确认密码不能为空";
                goto outoff;
            }
            if (!pwdNew.Equals(pwdNewSure))
            {
                resp.Msg = "新密码不一致";
                goto outoff;

            }
            currentUserInfo.Password = pwdNewSure;
            if (bllUser.Update(currentUserInfo, string.Format(" Password='{0}'", currentUserInfo.Password), string.Format(" AutoID='{0}'", currentUserInfo.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "密码修改成功";
            }
            else
            {
                resp.Msg = "密码修改失败";

            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        #region 游戏模块


        private string QueryGamePlan(HttpContext context)
        {
            BllGame bllGame = new BllGame();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planName = context.Request["planName"];
            int totalCount = 0;
            var list = bllGame.GetGameAdvertPlanList(pageIndex, pageSize, out totalCount, planName);
            resp.ExObj = list;
            resp.ExInt = totalCount;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string AddGamePlan(HttpContext context)
        {

            int gameId = int.Parse(context.Request["GameId"]);
            string planeName = context.Request["PlaneName"];
            string advertUrl1 = context.Request["AdvertUrl1"];
            string advertUrl2 = context.Request["AdvertUrl2"];
            string advertUrl3 = context.Request["AdvertUrl3"];
            string advertImage1 = context.Request["AdvertImage1"];
            string advertImage2 = context.Request["AdvertImage2"];
            string advertImage3 = context.Request["AdvertImage3"];
            BllGame bllGame = new BllGame();



            GameAdvertPlan model = new GameAdvertPlan();
            model.GameID = gameId;
            model.PlanName = planeName;
            model.WebsiteOwner = bllGame.WebsiteOwner;
            model.AdvertImage1 = advertImage1;
            model.AdvertImage2 = advertImage2;
            model.AdvertImage3 = advertImage3;
            model.AdvertUrl1 = advertUrl1;
            model.AdvertUrl2 = advertUrl2;
            model.AdvertUrl3 = advertUrl3;

            if (bllGame.AddGameAdvertPlan(model))
            {
                resp.Status = 1;
                resp.Msg = "新建游戏任务成功";
            }
            else
            {
                resp.Status = 1;
                resp.Msg = "新建游戏任务失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string DeleteGamePlan(HttpContext context)
        {

            BllGame bllGame = new BllGame();
            int count = bllGame.DeleteGamePlan(context.Request["ids"]);
            return string.Format("成功删除了{0}条数据", count);


        }

        #region 游戏管理模块

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddGameInfo(HttpContext context)
        {
            BllGame bllGame = new BllGame();
            string gameName = context.Request["GameName"];
            string gameImage = context.Request["GameImage"];
            string gameDesc = context.Request["GameDesc"];
            string gameCode = context.Request["GameCode"];
            string gameSort = context.Request["GameSort"];
            string gameViewPort = context.Request["GameViewPort"];
            if (string.IsNullOrEmpty(gameName))
            {
                resp.Status = 0;
                resp.Msg = "请输入游戏名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            GameInfo model = new GameInfo();
            model.GameName = gameName;
            model.GameImage = gameImage;
            model.GameDesc = gameDesc;
            model.GameCode = gameCode;
            model.GameViewPort = gameViewPort;
            if (!string.IsNullOrEmpty(gameSort))
            {
                model.GameSort = int.Parse(gameSort);
            }

            if (bllGame.AddGameInfo(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";


            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);




        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditGameInfo(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            BllGame bllGame = new BllGame();

            string gameName = context.Request["GameName"];
            string gameImage = context.Request["GameImage"];
            string gameDesc = context.Request["GameDesc"];
            string gameCode = context.Request["GameCode"];
            string gameSort = context.Request["GameSort"];
            string gameViewPort = context.Request["GameViewPort"];
            if (string.IsNullOrEmpty(gameName))
            {
                resp.Status = 0;
                resp.Msg = "请输入游戏名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            GameInfo model = bllGame.GetSingleGameInfo(autoId);
            model.GameName = gameName;
            model.GameImage = gameImage;
            model.GameDesc = gameDesc;
            model.GameCode = gameCode;
            model.GameViewPort = gameViewPort;
            if (!string.IsNullOrEmpty(gameSort))
            {
                model.GameSort = int.Parse(gameSort);
            }

            if (bllGame.UpdateGameInfo(model))
            {
                resp.Status = 1;
                resp.Msg = "更新成功";


            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);





        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteGameInfo(HttpContext context)
        {

            string ids = context.Request["Ids"];
            int count = bllJuActivity.Delete(new GameInfo(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了{0}条数据", count);

        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryGameInfo(HttpContext context)
        {

            BllGame bllGame = new BllGame();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //string TemplateName = context.Request["TemplateName"];
            StringBuilder sbWhere = new StringBuilder();
            //if (!string.IsNullOrEmpty(TemplateName))
            //{
            //    sbWhere.AppendFormat("And TemplateName like'%{0}%'", TemplateName);
            //}
            int totalCount = bllGame.GetCount<CompanyWebsiteTemplate>(sbWhere.ToString());
            List<GameInfo> dataList = new List<GameInfo>();
            dataList = bllGame.GetLit<GameInfo>(pageSize, pageIndex, sbWhere.ToString(), "GameSort ASC,AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });



        }






        #endregion

        #region 游戏监测
        private string QueryGameEventDetail(HttpContext context)
        {

            BllGame bllGame = new BllGame();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planId = context.Request["PlanId"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" GamePlanID={0}", planId));
            int totalCount = bllGame.GetCount<GameEventDetailInfo>(sbWhere.ToString());
            List<GameEventDetailInfo> dataList = new List<GameEventDetailInfo>();
            dataList = bllGame.GetLit<GameEventDetailInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });



        }

        private string QueryGameEventDetailClick(HttpContext context)
        {

            BllGame bllGame = new BllGame();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planId = context.Request["PlanId"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" GamePlanID={0}", planId));
            int totalCount = bllGame.GetCount<GameEventDetailInfoClick>(sbWhere.ToString());
            List<GameEventDetailInfoClick> dataList = new List<GameEventDetailInfoClick>();
            dataList = bllGame.GetLit<GameEventDetailInfoClick>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });



        }
        #endregion



        #endregion


        #region 子账户管理
        /// <summary>
        /// 查询子账户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QuerySubAccount(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string trueName = context.Request["TrueName"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And IsSubAccount='1'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(trueName))
            {
                sbWhere.AppendLine(string.Format(" TrueName  LIKE '%{0}%' ", trueName));
            }
            int totalCount = this.bllUser.GetCount<UserInfo>(sbWhere.ToString());
            List<UserInfo> dataList = this.bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});

        }


        /// <summary>
        /// 添加子账户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddSubAccount(HttpContext context)
        {
            string isDelete = context.Request["isDelete"];
            string isExport = context.Request["isExport"];

            UserInfo userModel = new UserInfo();
            userModel.UserID = context.Request["UserID"];
            userModel.Password = context.Request["Pwd"];
            userModel.TrueName = context.Request["TrueName"];
            //if (this.userInfo.UserType == 1)
            //    userModel.WebsiteOwner = string.IsNullOrWhiteSpace(context.Request["WebsiteOwner"]) ? userModel.WebsiteOwner : context.Request["WebsiteOwner"];
            //else
            userModel.WebsiteOwner = bllBase.WebsiteOwner;

            userModel.Company = context.Request["Company"];
            userModel.Phone = context.Request["Phone"];
            userModel.Postion = context.Request["Postion"];
            userModel.WXHeadimgurl = context.Request["HeadImg"];
            userModel.UserType = 2;
            userModel.RegIP = Common.MySpider.GetClientIP();
            userModel.Regtime = DateTime.Now;
            userModel.LoginTotalCount = 0;
            userModel.IsSubAccount = "1";
            userModel.VoteCount = int.Parse(string.IsNullOrEmpty(context.Request["VoteCount"]) ? "0" : context.Request["VoteCount"]);
            userModel.LastLoginDate = DateTime.Now;
            if (string.IsNullOrWhiteSpace(userModel.UserID) || string.IsNullOrWhiteSpace(userModel.Password))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrWhiteSpace(isDelete) || string.IsNullOrWhiteSpace(isExport))
            {
                resp.Status = -1;
                resp.Msg = "参数错误！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (this.bllUser.Exists(userModel, "UserID"))
            {
                resp.Status = -2;
                resp.Msg = "用户名" + userModel.UserID + "已存在！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (string.IsNullOrEmpty(userModel.WebsiteOwner))
            {
                userModel.WebsiteOwner = userModel.UserID;

            }
            if (bllUser.GetCount<UserInfo>(string.Format("WebsiteOwner='{0}' And IsSubAccount='1'", bllBase.WebsiteOwner)) >= currentWebSiteInfo.MaxSubAccountCount)
            {

                resp.Status = -3;
                resp.Msg = "您的可建立的子账号已达到上限";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (this.bllUser.Add(userModel, tran))
                {
                    var group = new ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo()
                    {
                        UserID = userModel.UserID,
                        GroupID = 130273//管理员组
                    };
                    if (Convert.ToBoolean(isDelete))
                    {
                        var relation = new ZentCloud.BLLPermission.Model.PermissionRelationInfo()
                        {
                            RelationID = userModel.UserID,
                            PermissionID = -1,
                            RelationType = 1
                        };
                        if (!bllUser.Add(relation, tran))
                        {
                            tran.Rollback();
                            resp.Status = -2;
                            resp.Msg = "权限关系添加失败";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }


                    }
                    if (Convert.ToBoolean(isExport))
                    {
                        var relations = new ZentCloud.BLLPermission.Model.PermissionRelationInfo()
                        {
                            RelationID = userModel.UserID,
                            PermissionID = -2,
                            RelationType = 1
                        };
                        if (!bllUser.Add(relations, tran))
                        {
                            tran.Rollback();
                            resp.Status = -2;
                            resp.Msg = "权限关系添加失败";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }


                    }
                    if (bllUser.Add(group, tran))//添加权限组
                    {
                        tran.Commit();
                        resp.Status = 1;
                        resp.Msg = "添加成功！";
                        return Common.JSONHelper.ObjectToJson(resp);

                    }
                    else
                    {
                        tran.Rollback();
                        resp.Status = 0;
                        resp.Msg = "添加用户组失败！";
                        return Common.JSONHelper.ObjectToJson(resp);

                    }




                }
                else
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "添加用户信息失败！";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);


            }


        }

        /// <summary>
        /// 编辑子账户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditSubAccount(HttpContext context)
        {
            string userId = context.Request["UserID"];
            UserInfo userInfo = this.bllUser.GetUserInfo(userId);
            if (userInfo == null)
            {
                resp.Status = -3;
                resp.Msg = "用户不存在！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //userModel.Password = context.Request["Pwd"];
            userInfo.TrueName = context.Request["TrueName"];
            userInfo.Company = context.Request["Company"];
            userInfo.Phone = context.Request["Phone"];
            userInfo.Postion = context.Request["Postion"];
            userInfo.WXHeadimgurl = context.Request["HeadImg"];

            userInfo.VoteCount = int.Parse(string.IsNullOrEmpty(context.Request["VoteCount"]) ? "0" : context.Request["VoteCount"]);
            if (string.IsNullOrWhiteSpace(userInfo.UserID) || string.IsNullOrWhiteSpace(userInfo.Password))
            {
                resp.Status = -1;
                resp.Msg = "请提交完整数据！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bllUser.Update(userInfo, string.Format(" TrueName='{0}',Company='{1}',Phone='{2}',Postion='{3}',VoteCount='{4}',WXHeadimgurl='{5}'", userInfo.TrueName, userInfo.Company, userInfo.Phone, userInfo.Postion, userInfo.VoteCount, userInfo.WXHeadimgurl), string.Format(" AutoID={0}", userInfo.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "更新成功！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败！";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }

        #endregion


        #region 投票管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryVoteInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string voteName = context.Request["VoteName"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!currentUserInfo.UserID.Equals(bllBase.WebsiteOwner) && (!currentUserInfo.UserType.Equals(1)))
            {
                sbWhere.AppendFormat(" And CreateUserID ='{0}'", currentUserInfo.UserID);
            }
            if (!string.IsNullOrEmpty(voteName))
            {
                sbWhere.AppendFormat(" And VoteName like '%{0}%'", voteName);
            }
            int totalCount = this.bllJuActivity.GetCount<VoteInfo>(sbWhere.ToString());
            List<VoteInfo> data = this.bllJuActivity.GetLit<VoteInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = data
            });
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteInfo(HttpContext context)
        {

            string voteName = context.Request["VoteName"];
            string voteImage = context.Request["VoteImage"];
            string summary = context.Request["Summary"];
            string offlinePayUrl = context.Request["OfflinePayUrl"];
            string introduction = context.Request["Introduction"];
            int voteStatus = int.Parse(context.Request["VoteStatus"]);
            int isFree = int.Parse(context.Request["IsFree"]);
            int freeVoteCount = int.Parse(context.Request["FreeVoteCount"]);
            int voteType = string.IsNullOrEmpty(context.Request["VoteType"]) ? 0 : int.Parse(context.Request["VoteType"]);
            int voteCountAutoUpdate = string.IsNullOrEmpty(context.Request["VoteCountAutoUpdate"]) ? 0 : int.Parse(context.Request["VoteCountAutoUpdate"]);
            string stopDateStr = context.Request["StopDate"];
            string logo = context.Request["Logo"];
            string bottomContent = context.Request["BottomContent"];

            string prize = context.Request["Prize"];
            string useScore = context.Request["UseScore"];

            string limitType = context.Request["LimitType"];
            string voteObjectLimitVoteCount = context.Request["VoteObjectLimitVoteCount"];

            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];

            //首页背景
            string indexBg = context.Request["IndexBg"];
            //背景Banner图
            string bannerBg = context.Request["BannerBg"];
            //bnner高度
            string bannerHeight = context.Request["BannerHeight"];
            //手持字样
            string handheldWords = context.Request["HandheldWords"];
            //手持图片
            string handheldImg = context.Request["HandheldImg"];
            //参赛宣言别名
            string signUpDeclarationRename = context.Request["SignUpDeclarationRename"];
            //参赛宣言说明
            string signUpDeclarationDescription = context.Request["SignUpDeclarationDescription"];
            // 合作伙伴图片：放在页面底部
            string partnerImg = context.Request["PartnerImg"];
            //分享标题
            string shareTitle = context.Request["ShareTitle"];
            //投票对象详情banner图
            string voteObjDetailBannerImg = context.Request["VoteObjDetailBannerImg"];
            //投票列表详情banner图
            string voteObjListBannerImg = context.Request["VoteObjListBannerImg"];
            //活动未开始海报
            string notStartPoster = context.Request["NotStartPoster"];
            //背景音乐
            string bgMusic = context.Request["BgMusic"];
            //规则页
            string rulePageHtml = context.Request["RulePageHtml"];


            //投票页面背景色
            string votePageBgColor = context.Request["VotePageBgColor"];
            //首页底部菜单是否可以隐藏：1是，0否
            string isHideIndexFooterMenu = context.Request["IsHideIndexFooterMenu"];
            //首页配置
            string indexPageHtml = context.Request["IndexPageHtml"];
            //底部导航按钮组，默认为空，可以选择导航里面其中一组
            string footerMenuGroup = context.Request["FooterMenuGroup"];
            //投票参与者其他资料链接展示文本
            string voteObjectVideoLinkText = context.Request["VoteObjectVideoLinkText"];

            string themeColor = context.Request["ThemeColor"];
            string themeFontColor = context.Request["ThemeFontColor"];

            int useScoreInt = 0;
            if (!string.IsNullOrEmpty(useScore))
            {
                useScoreInt = int.Parse(useScore);
                if (useScoreInt < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "使用积分不能小于0";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }

            DateTime stopDate = new DateTime();

            if (!bllVote.CheckUserCanCreateVote(currentUserInfo.UserID))
            {
                resp.Status = 0;
                resp.Msg = "您的可建投票数达到上限,不能新建投票,请联系管理员";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (string.IsNullOrEmpty(voteName))
            {
                resp.Status = 0;
                resp.Msg = "请输入名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(stopDateStr))
            {
                if (!DateTime.TryParse(stopDateStr, out stopDate))
                {
                    resp.Status = 0;
                    resp.Msg = "时间格式不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }

            }

            VoteInfo model = new VoteInfo();
            model.VoteName = voteName;
            model.VoteStatus = voteStatus;
            model.CreateUserID = currentUserInfo.UserID;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.IsFree = isFree;
            model.VoteImage = voteImage;
            model.Summary = summary;
            model.FreeVoteCount = freeVoteCount;
            model.Introduction = introduction;
            model.OfflinePayUrl = offlinePayUrl;
            model.VoteType = voteType;
            model.VoteCountAutoUpdate = voteCountAutoUpdate;
            model.Logo = logo;
            model.BottomContent = bottomContent;
            model.Prize = prize;
            model.UseScore = useScoreInt;
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.LimitType = int.Parse(limitType);
            model.VoteObjectLimitVoteCount = int.Parse(voteObjectLimitVoteCount);

            model.IndexBg = indexBg;
            model.BannerBg = bannerBg;
            model.BannerHeight = bannerHeight;
            model.HandheldWords = handheldWords;
            model.HandheldImg = handheldImg;
            model.SignUpDeclarationRename = signUpDeclarationRename;
            model.SignUpDeclarationDescription = signUpDeclarationDescription;
            model.PartnerImg = partnerImg;
            model.ShareTitle = shareTitle;
            model.VoteObjDetailBannerImg = voteObjDetailBannerImg;
            model.VoteObjListBannerImg = voteObjListBannerImg;
            model.NotStartPoster = notStartPoster;
            model.BgMusic = bgMusic;
            model.RulePageHtml = rulePageHtml;

            model.VotePageBgColor = votePageBgColor;
            model.IsHideIndexFooterMenu = Convert.ToInt32(isHideIndexFooterMenu);
            model.IndexPageHtml = indexPageHtml;
            model.FooterMenuGroup = footerMenuGroup;
            model.OtherInfoLinkText = voteObjectVideoLinkText;

            model.ThemeColor = themeColor;
            model.ThemeFontColor = themeFontColor;

            if (!string.IsNullOrEmpty(stopDateStr))
            {
                model.StopDate = stopDate.ToString();
            }
            if (bllVote.AddVoteInfo(model))
            {
                //if (isFree.Equals(0))
                //{
                //    //
                //    var LastVoteInfo = bllVote.GetLastVoteInfo();
                //    List<VoteRecharge> RechargeList = new List<VoteRecharge>();
                //    VoteRecharge Recharge1 = new VoteRecharge();
                //    Recharge1.GiftName = "礼品1";
                //    Recharge1.GiftDesc = "礼品1介绍";
                //    Recharge1.RechargeCount = 2;
                //    Recharge1.Amount = 2;
                //    Recharge1.VoteId = LastVoteInfo.AutoID;

                //    VoteRecharge Recharge2 = new VoteRecharge();
                //    Recharge2.GiftName = "礼品2";
                //    Recharge2.GiftDesc = "礼品2介绍";
                //    Recharge2.RechargeCount = 5;
                //    Recharge2.Amount = 5;
                //    Recharge2.VoteId = LastVoteInfo.AutoID;

                //    VoteRecharge Recharge3 = new VoteRecharge();
                //    Recharge3.GiftName = "礼品3";
                //    Recharge3.GiftDesc = "礼品3介绍";
                //    Recharge3.RechargeCount = 10;
                //    Recharge3.Amount = 10;
                //    Recharge3.VoteId = LastVoteInfo.AutoID;


                //    VoteRecharge Recharge4 = new VoteRecharge();
                //    Recharge4.GiftName = "礼品4";
                //    Recharge4.GiftDesc = "礼品4介绍";
                //    Recharge4.RechargeCount = 50;
                //    Recharge4.Amount = 50;
                //    Recharge4.VoteId = LastVoteInfo.AutoID;

                //    VoteRecharge Recharge5 = new VoteRecharge();
                //    Recharge5.GiftName = "礼品5";
                //    Recharge5.GiftDesc = "礼品5介绍";
                //    Recharge5.RechargeCount = 100;
                //    Recharge5.Amount = 100;
                //    Recharge5.VoteId = LastVoteInfo.AutoID;

                //    RechargeList.Add(Recharge1);
                //    RechargeList.Add(Recharge2);
                //    RechargeList.Add(Recharge3);
                //    RechargeList.Add(Recharge4);
                //    RechargeList.Add(Recharge5);
                //    bllVote.AddList(RechargeList);


                //}
                resp.Status = 1;
                resp.Msg = "添加成功";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加排名投票");

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditVoteInfo(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            string voteName = context.Request["VoteName"];
            int voteStatus = int.Parse(context.Request["VoteStatus"]);
            int isFree = int.Parse(context.Request["IsFree"]);
            string voteImage = context.Request["VoteImage"];
            string summary = context.Request["Summary"];
            string offlinePayUrl = context.Request["OfflinePayUrl"];
            string introduction = context.Request["Introduction"];
            int freeVoteCount = int.Parse(context.Request["FreeVoteCount"]);
            int voteType = string.IsNullOrEmpty(context.Request["VoteType"]) ? 0 : int.Parse(context.Request["VoteType"]);
            int voteCountAutoUpdate = string.IsNullOrEmpty(context.Request["VoteCountAutoUpdate"]) ? 0 : int.Parse(context.Request["VoteCountAutoUpdate"]);
            string stopDateStr = context.Request["StopDate"];
            string logo = context.Request["Logo"];
            string bottomContent = context.Request["BottomContent"];

            string prize = context.Request["Prize"];
            string useScore = context.Request["UseScore"];
            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];
            string limitType = context.Request["LimitType"];
            string voteObjectLimitVoteCount = context.Request["VoteObjectLimitVoteCount"];

            string themeColor = context.Request["ThemeColor"];
            string themeFontColor = context.Request["ThemeFontColor"];

            //首页背景
            string indexBg = context.Request["IndexBg"];
            //背景Banner图
            string bannerBg = context.Request["BannerBg"];
            //bnner高度
            string bannerHeight = context.Request["BannerHeight"];
            //手持字样
            string handheldWords = context.Request["HandheldWords"];
            //手持图片
            string handheldImg = context.Request["HandheldImg"];
            //参赛宣言别名
            string signUpDeclarationRename = context.Request["SignUpDeclarationRename"];
            //参赛宣言说明
            string signUpDeclarationDescription = context.Request["SignUpDeclarationDescription"];
            // 合作伙伴图片：放在页面底部
            string partnerImg = context.Request["PartnerImg"];
            //分享标题
            string shareTitle = context.Request["ShareTitle"];
            //投票对象详情banner图
            string voteObjDetailBannerImg = context.Request["VoteObjDetailBannerImg"];
            //投票列表详情banner图
            string voteObjListBannerImg = context.Request["VoteObjListBannerImg"];
            //活动未开始海报
            string notStartPoster = context.Request["NotStartPoster"];
            //背景音乐
            string bgMusic = context.Request["BgMusic"];
            //规则页
            string rulePageHtml = context.Request["RulePageHtml"];

            //投票页面背景色
            string votePageBgColor = context.Request["VotePageBgColor"];
            //首页底部菜单是否可以隐藏：1是，0否
            string isHideIndexFooterMenu = context.Request["IsHideIndexFooterMenu"];
            //首页配置
            string indexPageHtml = context.Request["IndexPageHtml"];
            //底部导航按钮组，默认为空，可以选择导航里面其中一组
            string footerMenuGroup = context.Request["FooterMenuGroup"];
            //投票参与者其他资料链接展示文本
            string voteObjectVideoLinkText = context.Request["VoteObjectVideoLinkText"];

            int useScoreInt = 0;
            if (!string.IsNullOrEmpty(useScore))
            {
                useScoreInt = int.Parse(useScore);
                if (useScoreInt < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "使用积分不能小于0";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            DateTime stopDate = new DateTime();
            if (string.IsNullOrEmpty(voteName))
            {
                resp.Status = 0;
                resp.Msg = "请输入名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(stopDateStr))
            {
                if (!DateTime.TryParse(stopDateStr, out stopDate))
                {
                    resp.Status = 0;
                    resp.Msg = "时间格式不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }

            }


            VoteInfo model = bllVote.GetVoteInfo(autoId);
            model.VoteName = voteName;
            model.VoteStatus = voteStatus;
            model.IsFree = isFree;
            model.VoteImage = voteImage;
            model.Summary = summary;
            model.FreeVoteCount = freeVoteCount;
            model.Introduction = introduction;
            model.OfflinePayUrl = offlinePayUrl;
            model.VoteType = voteType;
            model.VoteCountAutoUpdate = voteCountAutoUpdate;
            model.Logo = logo;
            model.BottomContent = bottomContent;
            model.Prize = prize;
            model.UseScore = useScoreInt;
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.LimitType = int.Parse(limitType);
            model.VoteObjectLimitVoteCount = int.Parse(voteObjectLimitVoteCount);

            model.IndexBg = indexBg;
            model.BannerBg = bannerBg;
            model.BannerHeight = bannerHeight;
            model.HandheldWords = handheldWords;
            model.HandheldImg = handheldImg;
            model.SignUpDeclarationRename = signUpDeclarationRename;
            model.SignUpDeclarationDescription = signUpDeclarationDescription;
            model.PartnerImg = partnerImg;
            model.ShareTitle = shareTitle;
            model.VoteObjDetailBannerImg = voteObjDetailBannerImg;
            model.VoteObjListBannerImg = voteObjListBannerImg;
            model.NotStartPoster = notStartPoster;
            model.BgMusic = bgMusic;
            model.RulePageHtml = rulePageHtml;

            model.VotePageBgColor = votePageBgColor;
            model.IsHideIndexFooterMenu = Convert.ToInt32(isHideIndexFooterMenu);
            model.IndexPageHtml = indexPageHtml;
            model.FooterMenuGroup = footerMenuGroup;
            model.OtherInfoLinkText = voteObjectVideoLinkText;

            model.ThemeColor = themeColor;
            model.ThemeFontColor = themeFontColor;

            if (!string.IsNullOrEmpty(stopDateStr))
            {
                model.StopDate = stopDate.ToString();
            }
            if (bllVote.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "编辑排名投票[id=" + autoId + "]");

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteVoteInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            VoteInfo model;
            foreach (var item in ids.Split(','))
            {
                model = bllVote.GetVoteInfo(int.Parse(item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllVote.Delete(new VoteInfo(), string.Format("AutoID in ({0})", ids));
            bllVote.Delete(new VoteObjectInfo(), string.Format("VoteID in ({0})", ids));
            bllVote.Delete(new VoteLogInfo(), string.Format("VoteID in ({0})", ids));
            bllVote.Delete(new VoteRecharge(), string.Format("VoteId in ({0})", ids));
            bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除排名投票[id=" + ids + "]");
            return string.Format("成功删除了 {0} 条数据", count);

        }


        #region 投票对象管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryVoteObjectInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int voteId = int.Parse(context.Request["VoteID"]);
            string voteObjectName = context.Request["VoteObjectName"];
            string status = context.Request["Status"];
            string orderBy = context.Request["OrderBy"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" VoteID={0}", voteId));
            if (!string.IsNullOrEmpty(voteObjectName))
            {
                sbWhere.AppendFormat("And (VoteObjectName like '%{0}%' Or Number='{0}')", voteObjectName);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat("And Status='{0}'", status);
            }
            int totalCount = this.bllVote.GetCount<VoteObjectInfo>(sbWhere.ToString());
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = " Status ASC,AutoID ASC";
            }

            List<VoteObjectInfo> dataList = this.bllVote.GetLit<VoteObjectInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });


        }




        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteObjectInfo(HttpContext context)
        {
            int voteID = int.Parse(context.Request["VoteID"]);
            string voteObjectName = context.Request["VoteObjectName"];
            string voteObjectGender = context.Request["VoteObjectGender"];
            string voteObjectHeadImage = context.Request["VoteObjectHeadImage"];
            string area = context.Request["Area"];
            string height = context.Request["Height"];
            string constellation = context.Request["Constellation"];
            string hobbies = context.Request["Hobbies"];
            string introduction = context.Request["Introduction"];
            string introductionDetail = context.Request["IntroductionDetail"];
            string age = context.Request["Age"];
            string schoolName = context.Request["SchoolName"];
            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            string showImage5 = context.Request["ShowImage5"];
            string number = (bllVote.GetVoteObjectMaxNumber(voteID) + 1).ToString();
            string bottomContent = context.Request["BottomContent"];
            string otherInfoLink = context.Request["OtherInfoLink"];
            string phone = context.Request["Phone"];
            string ex4 = context.Request["Ex4"];
            string ex6 = context.Request["Ex6"];
            string remark = context.Request["Remark"];
            string status = context.Request["Status"];
            string address = context.Request["Address"];//Totema
            string contact = context.Request["Contact"];

            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];
            string ex3 = context.Request["Ex3"];
            //if (bllVote.GetVoteObjectInfoCountByVoteID(VoteID) >= 100)
            //{
            //    resp.Status = 0;
            //    resp.Msg = "投票对象已达到上限100";
            //    return Common.JSONHelper.ObjectToJson(resp);

            //}
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.Status = 0;
                resp.Msg = "请输入名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //if (string.IsNullOrEmpty(Number))
            //{
            //    resp.Status = 0;
            //    resp.Msg = "请输入编号";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}
            if (bllVote.IsExitsVoteObjectNumber(voteID, number))
            {
                resp.Status = 0;
                resp.Msg = "编号已经存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            VoteObjectInfo model = new VoteObjectInfo();
            model.VoteID = voteID;
            model.Number = number;
            model.VoteObjectName = voteObjectName;
            model.VoteObjectGender = voteObjectGender;
            model.VoteObjectHeadImage = voteObjectHeadImage;
            model.Area = area;
            model.Height = height;
            model.Constellation = constellation;
            model.Hobbies = hobbies;
            model.Introduction = introduction;
            model.IntroductionDetail = introductionDetail;
            model.Age = age;
            model.SchoolName = schoolName;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;
            model.ShowImage3 = showImage3;
            model.ShowImage4 = showImage4;
            model.ShowImage5 = showImage5;
            model.BottomContent = bottomContent;
            model.Address = address;
            model.Contact = contact;
            model.Phone = phone;
            model.Remark = remark;
            model.Ex4 = ex4;
            model.Ex6 = ex6;
            model.OtherInfoLink = otherInfoLink;
            model.Status = Convert.ToInt32(status);
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.Ex3 = ex3;
            if (bllVote.AddVoteObjectInfo(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditVoteObjectInfo(HttpContext context)
        {
            int autoID = int.Parse(context.Request["AutoID"]);
            int voteID = int.Parse(context.Request["VoteID"]);
            string voteObjectName = context.Request["VoteObjectName"];
            string voteObjectGender = context.Request["VoteObjectGender"];
            string voteObjectHeadImage = context.Request["VoteObjectHeadImage"];
            string area = context.Request["Area"];
            string height = context.Request["Height"];
            string constellation = context.Request["Constellation"];
            string hobbies = context.Request["Hobbies"];
            string introduction = context.Request["Introduction"];
            string introductionDetail = context.Request["IntroductionDetail"];
            string age = context.Request["Age"];
            string schoolName = context.Request["SchoolName"];
            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            string showImage5 = context.Request["ShowImage5"];
            string bottomContent = context.Request["BottomContent"];

            string address = context.Request["Address"];//Totema
            string contact = context.Request["Contact"];
            string phone = context.Request["Phone"];
            string status = context.Request["Status"];
            string remark = context.Request["Remark"];
            string ex4 = context.Request["Ex4"];
            string ex6 = context.Request["Ex6"];
            string otherInfoLink = context.Request["OtherInfoLink"];
            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];
            string ex3 = context.Request["Ex3"];
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.Status = 0;
                resp.Msg = "请输入名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            VoteObjectInfo model = bllVote.GetVoteObjectInfo(autoID);
            model.VoteObjectName = voteObjectName;
            model.VoteObjectGender = voteObjectGender;
            model.VoteObjectHeadImage = voteObjectHeadImage;
            model.Area = area;
            model.Height = height;
            model.Constellation = constellation;
            model.Hobbies = hobbies;
            model.Introduction = introduction;
            model.IntroductionDetail = introductionDetail;
            model.Age = age;
            model.SchoolName = schoolName;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;
            model.ShowImage3 = showImage3;
            model.ShowImage4 = showImage4;
            model.ShowImage5 = showImage5;
            model.BottomContent = bottomContent;

            model.Address = address;
            model.Contact = contact;
            model.Phone = phone;
            model.Remark = remark;
            model.Ex4 = ex4;
            model.Ex6 = ex6;
            model.OtherInfoLink = otherInfoLink;
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.Ex3 = ex3;
            if (!string.IsNullOrEmpty(status))
            {
                model.Status = int.Parse(status);
            }
            if (bllVote.UpdateVoteObjectInfo(model))
            {
                if (!bllVote.UpdateVoteObjectRank(voteID, "1"))
                {
                    resp.Msg = "更新排名失败";
                    goto outoff;
                }
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteVoteObjectInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            int count = bllVote.Delete(new VoteObjectInfo(), string.Format("AutoID in ({0})", ids));
            bllVote.Delete(new VoteLogInfo(), string.Format("VoteObjectID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        /// <summary>
        ///旋转图片 向左旋转90度
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string TransformImage(HttpContext context)
        {
            string imagePath = context.Request["ImagePath"];
            if (!string.IsNullOrEmpty(imagePath))
            {
                if (imagePath.StartsWith("http"))
                {
                    imagePath = bllJuActivity.DownLoadRemoteImage(imagePath);
                }
                CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
                string ImgPathPhysics = context.Server.MapPath(imagePath);
                if (imgHelper.RotateImg(ImgPathPhysics, 90, ImgPathPhysics) != null)
                {
                    resp.Status = 1;
                    resp.ExStr = imagePath;
                }

            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询投票购票记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryVoteOrderInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string subAccount = context.Request["SubAccount"];//子账户
            string voteID = context.Request["VoteID"];
            string status = context.Request["Status"];

            if ((!currentUserInfo.UserID.Equals(bllBase.WebsiteOwner)) && (!currentUserInfo.UserType.Equals(1)))
            {
                return null;
            }

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Type=1 And WebsiteOwner='{0}' ", bllMall.WebsiteOwner));
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat("And Status = '{0}'", status);
            }
            if (!string.IsNullOrEmpty(voteID))
            {
                sbWhere.AppendFormat("And Ex2 = '{0}'", voteID);
            }
            if (!string.IsNullOrEmpty(subAccount))//查询子账户
            {
                StringBuilder sbVoteIds = new StringBuilder();
                var list = bllBase.GetList<VoteInfo>(string.Format("CreateUserID='{0}'", subAccount));
                if (list.Count > 0)
                {
                    sbWhere.AppendFormat("And Ex2  in({0})", string.Join(",", list.SelectMany(p => new List<int>() { (int)p.AutoID })));

                }
                else
                {
                    sbWhere.AppendFormat("And 1=0");
                }
            }
            int totalCount = bllMall.GetCount<OrderPay>(sbWhere.ToString());
            List<OrderPay> dataList = bllMall.GetLit<OrderPay>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });

        }




        /// <summary>
        /// 查询投票记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryVoteLogInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string voteID = context.Request["VoteID"];
            string userId = context.Request["UserId"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            //string Status = context.Request["Status"];

            if ((!currentUserInfo.UserID.Equals(bllBase.WebsiteOwner)) && (!currentUserInfo.UserType.Equals(1)))
            {
                //sbWhere.AppendFormat(" And CreateUserID='{0}'",userInfo.UserID);
                return "";
            }
            //if (!string.IsNullOrEmpty(Status))
            //{
            //    sbWhere.AppendFormat("And Status = '{0}'", Status);
            //}
            if (!string.IsNullOrEmpty(voteID))
            {
                sbWhere.AppendFormat("And VoteID = '{0}'", voteID);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat("And UserID = '{0}'", userId);
            }
            int totalCount = bllVote.GetCount<VoteLogInfo>(sbWhere.ToString());
            List<VoteLogInfo> dataList = bllVote.GetLit<VoteLogInfo>(pageSize, pageIndex, sbWhere.ToString(), "VoteID DESC,InsertDate DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }

        #endregion

        #region PK模式管理
        /// <summary>
        /// 新增投票PK模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteGroupInfo(HttpContext context)
        {
            string voteGroupName = context.Request["VoteGroupName"];
            string voteGroupMembers = context.Request["VoteGroupMembers"];
            string sort = context.Request["Sort"];
            string voteId = context.Request["VoteId"];
            if (string.IsNullOrEmpty(voteId))
            {
                resp.Msg = "投票id不能为空.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(voteGroupName))
            {
                resp.Msg = "组名不能为空.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(voteGroupMembers))
            {
                resp.Msg = "参与者不能为空.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string[] objects = voteGroupMembers.Split(',');
            if (objects.Count() < 2)
            {
                resp.Msg = "PK最低选择两个成员.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo modelMembers = bllVote.GetVoteGroupInfoByGroupMembers(voteId, voteGroupMembers);
            if (modelMembers != null)
            {
                resp.Msg = "选中的PK者已经建立PK组.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo modelGroupName = bllVote.GetVoteGroupInfoByGroupName(voteId, voteGroupName);
            if (modelGroupName != null)
            {
                resp.Msg = "组名不能相同.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo voteGroup = new VoteGroupInfo();
            voteGroup.VoteGroupId = int.Parse(bllBase.GetGUID(BLLJIMP.TransacType.AddVoteId));
            voteGroup.VoteGroupName = voteGroupName;
            voteGroup.VoteGroupMembers = voteGroupMembers;
            voteGroup.VoteId = int.Parse(voteId);
            voteGroup.Sort = int.Parse(sort);
            if (bllVote.AddVoteGroupInfo(voteGroup))
            {
                resp.Status = 1;
                resp.Msg = "添加成功.";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "添加出错.";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 编辑投票PK模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditVoteGroupInfo(HttpContext context)
        {
            string voteGroupName = context.Request["VoteGroupName"];
            string sort = context.Request["Sort"];
            string voteGroupId = context.Request["VoteGroupId"];
            string voteId = context.Request["VoteId"];
            if (string.IsNullOrEmpty(voteId))
            {
                resp.Msg = "投票id不能为空.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(voteGroupName))
            {
                resp.Msg = "组名不能为空.";
                resp.Status = 0;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo modelName = bllVote.GetVoteGroupInfoByGroupName(voteId, voteGroupName);
            if (modelName != null)
            {
                if (modelName.VoteGroupId != int.Parse(voteGroupId))
                {
                    resp.Msg = "组名不能相同.";
                    resp.Status = -1;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }

            VoteGroupInfo model = bllVote.GetVoteGroupInfo(int.Parse(voteGroupId));
            model.VoteGroupName = voteGroupName;
            model.Sort = int.Parse(sort);
            if (bllVote.UpdateVoteGroupInfo(model))
            {
                resp.Msg = "修改完成.";
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "修改出错.";
                resp.Status = -1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询投票PK模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryVoteGroupInfo(HttpContext context)
        {
            string voteId = context.Request["VoteId"];
            string sort = context.Request["Sort"];
            string groupName = context.Request["keyWord"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" VoteId={0}", voteId));
            if (!string.IsNullOrEmpty(groupName))
            {
                sbWhere.AppendFormat(" AND VoteGroupName like '%{0}%'", groupName);
            }
            List<VoteGroupInfo> GroupInfoList = bllBase.GetLit<VoteGroupInfo>(int.MaxValue, 1, sbWhere.ToString(), sort);
            int count = bllBase.GetCount<VoteGroupInfo>(sbWhere.ToString());
            List<VoteGroupResp> groupRespList = new List<VoteGroupResp>();
            foreach (VoteGroupInfo item in GroupInfoList)
            {
                string[] autoids = item.VoteGroupMembers.Split(',');
                if (autoids.Count() < 2) continue;
                for (int i = 0; i < autoids.Length; i++)
                {
                    VoteGroupResp groupModel = new VoteGroupResp();
                    VoteObjectInfo objectInfo = bllVote.GetVoteObjectInfo(int.Parse(autoids[i]));
                    groupModel.group_id = item.VoteGroupId;
                    groupModel.group_name = item.VoteGroupName;
                    groupModel.group_sort = item.Sort;
                    groupModel.group_members = item.VoteGroupMembers;
                    groupModel.object_id = objectInfo.AutoID;
                    groupModel.vote_object_name = objectInfo.VoteObjectName;
                    groupModel.vote_number = objectInfo.Number;
                    groupModel.vote_count = objectInfo.VoteCount;
                    groupRespList.Add(groupModel);
                }
            }
            var data = new
            {
                total = count,
                rows = groupRespList
            };
            return Common.JSONHelper.ObjectToJson(data);
        }
        public class VoteGroupResp
        {
            /// <summary>
            /// 组id
            /// </summary>
            public int group_id { get; set; }
            /// <summary>
            /// 组名
            /// </summary>
            public string group_name { get; set; }

            /// <summary>
            /// 参与者id
            /// </summary>
            public string group_members { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int group_sort { get; set; }

            public int object_id { get; set; }

            public int vote_count { get; set; }

            public string vote_number { get; set; }

            public string vote_object_name { get; set; }
        }

        /// <summary>
        /// 删除PK模式分组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteVoteGroupInfo(HttpContext context)
        {
            string groupId = context.Request["id"];
            if (string.IsNullOrEmpty(groupId))
            {
                resp.Msg = "分组Id为空.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo model = bllVote.GetVoteGroupInfo(int.Parse(groupId));
            if (model == null)
            {
                resp.Msg = "分组不存在.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllVote.DeleteVoteGroupInfo(model) > 0)
            {
                resp.Msg = "删除分组完成.";
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "删除出错.";
                resp.Status = -1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 删除组成员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteVoteGroupInfoByMembers(HttpContext context)
        {
            string objectId = context.Request["objectId"];
            string groupId = context.Request["groupId"];
            if (string.IsNullOrEmpty(objectId) || string.IsNullOrEmpty(groupId))
            {
                resp.Msg = "组Id或参与者Id为空";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo groupModel = bllVote.GetVoteGroupInfo(int.Parse(groupId));
            if (groupModel == null)
            {
                resp.Msg = "组不存在";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string[] numbers = groupModel.VoteGroupMembers.Split(',');
            if (numbers.Count() < 3)
            {
                resp.Msg = "PK组最低要存在两个参与者.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            List<string> memberList = numbers.ToList();
            memberList.Remove(objectId);
            groupModel.VoteGroupMembers = string.Join(",", memberList.ToArray());
            if (bllVote.UpdateVoteGroupInfo(groupModel))
            {
                resp.Status = 1;
                resp.Msg = "删除完成";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除出错";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 组中新加参与者
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteGroupByMember(HttpContext context)
        {
            string voteId = context.Request["VoteId"];
            string GroupId = context.Request["VoteGroupId"];
            string VoteGroupMembers = context.Request["VoteGroupMembers"];
            if (string.IsNullOrEmpty(voteId) || string.IsNullOrEmpty(GroupId) || string.IsNullOrEmpty(VoteGroupMembers))
            {
                resp.Msg = "必填参数为空.";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            VoteGroupInfo groupModel = bllVote.GetVoteGroupInfo(int.Parse(GroupId));
            if (groupModel == null)
            {
                resp.Msg = "不存在该组";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string[] objectids = groupModel.VoteGroupMembers.Split(',');
            string[] ids = VoteGroupMembers.Split(',');
            List<string> strlist = objectids.ToList();
            foreach (var item in ids)
            {
                if (strlist.Contains(item)) continue;
                strlist.Add(item);
            }
            groupModel.VoteGroupMembers = string.Join(",", strlist.ToArray());
            VoteGroupInfo model = bllVote.GetVoteGroupInfoByGroupMembers(voteId, groupModel.VoteGroupMembers);
            if (model != null)
            {
                resp.Msg = "选中的PK者已经存在该组";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllVote.UpdateVoteGroupInfo(groupModel))
            {
                resp.Msg = "加入完成";
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "加入出错";
                resp.Status = -1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }



        #endregion

        #endregion


        #region 微商城配送员
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallDeliveryStaff(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string staffName = context.Request["StaffName"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(staffName))
            {
                sbWhere.AppendFormat(" And StaffName like '%{0}%'", staffName);
            }
            int totalCount = this.bllMall.GetCount<WXMallDeliveryStaff>(sbWhere.ToString());
            List<WXMallDeliveryStaff> dataList = this.bllMall.GetLit<WXMallDeliveryStaff>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });

        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallDeliveryStaff(HttpContext context)
        {
            string staffName = context.Request["StaffName"];
            string staffPhone = context.Request["StaffPhone"];
            if (string.IsNullOrEmpty(staffName))
            {
                resp.Status = 0;
                resp.Msg = "请输入分类名称";
                Common.JSONHelper.ObjectToJson(resp);
            }


            WXMallDeliveryStaff model = new WXMallDeliveryStaff();
            model.StaffName = staffName;
            model.StaffPhone = staffPhone;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllJuActivity.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallDeliveryStaff(HttpContext context)
        {


            string staffName = context.Request["StaffName"];
            string staffPhone = context.Request["StaffPhone"];
            int autoID = int.Parse(context.Request["AutoID"]);

            if (string.IsNullOrEmpty(staffName))
            {
                resp.Status = 0;
                resp.Msg = "请输入配送员姓名";
                Common.JSONHelper.ObjectToJson(resp);
            }

            var model = bllMall.Get<WXMallDeliveryStaff>(string.Format("AutoID={0}", autoID));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "不存在";
                Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                Common.JSONHelper.ObjectToJson(resp);

            }
            model.StaffName = staffName;
            model.StaffPhone = staffPhone;
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallDeliveryStaff(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXMallDeliveryStaff model;
            foreach (var item in ids.Split(','))
            {
                model = bllMall.Get<WXMallDeliveryStaff>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllMall.Delete(new WXMallDeliveryStaff(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion

        /// <summary>
        /// 更新积分配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateScoreConfig(HttpContext context)
        {
            //string ScoreRule = context.Request["ScoreRule"];
            //string OrderDateStr = context.Request["OrderDate"];
            //string OrderDateTotalAmountStr = context.Request["OrderDateTotalAmount"];

            string orderAmount = context.Request["OrderAmount"];//订单金额
            string orderScore = context.Request["OrderScore"];//所得积分

            string exchangeAmount = context.Request["ExchangeAmount"];//积分兑换 金额
            string exchangeScore = context.Request["ExchangeScore"];//积分兑换 积分

            //DateTime OrderDate = new DateTime();
            //decimal OrderDateTotalAmount = 0;
            //int OrderScore = 0;
            ////检查
            //if (!string.IsNullOrEmpty(OrderDateStr) || (!string.IsNullOrEmpty(OrderDateTotalAmountStr)) || (!string.IsNullOrEmpty(OrderScoreStr)))
            //{
            //    if (string.IsNullOrEmpty(OrderDateStr) || string.IsNullOrEmpty(OrderDateTotalAmountStr) || string.IsNullOrEmpty(OrderScoreStr))
            //    {
            //        resp.Msg = "日期，金额，积分必须同时填，或者日期，金额，积分都不填";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //}

            //if (!string.IsNullOrEmpty(OrderDateStr))
            //{
            //    if (!DateTime.TryParse(OrderDateStr, out OrderDate))
            //    {
            //        resp.Msg = "日期格式不正确";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //    if ((OrderDate - DateTime.Now).Days < 0)
            //    {
            //        resp.Msg = "日期须晚于当前时间";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //}
            //if (!string.IsNullOrEmpty(OrderDateTotalAmountStr))
            //{
            //    if (!decimal.TryParse(OrderDateTotalAmountStr, out OrderDateTotalAmount))
            //    {
            //        resp.Msg = "金额不正确";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //    if (OrderDateTotalAmount <= 0)
            //    {
            //        resp.Msg = "金额须大于0";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //}
            //if (!string.IsNullOrEmpty(OrderScoreStr))
            //{
            //    if (!int.TryParse(OrderScoreStr, out OrderScore))
            //    {
            //        resp.Msg = "积分不正确";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //    if (OrderScore <= 0)
            //    {
            //        resp.Msg = "积分须大于0";
            //        return Common.JSONHelper.ObjectToJson(resp);

            //    }
            //}





            //检查


            ScoreConfig model = bllScore.GetScoreConfig();
            if (model != null)
            {

                //model.ScoreRule = ScoreRule;
                //if (!string.IsNullOrEmpty(OrderDateStr))
                //{
                //    model.OrderDate = OrderDate;
                //}
                //if (!string.IsNullOrEmpty(OrderDateTotalAmountStr))
                //{
                //    model.OrderDateTotalAmount = OrderDateTotalAmount;
                //}
                //if (!string.IsNullOrEmpty(OrderScoreStr))
                //{
                //    model.OrderScore = OrderScore;
                //}
                if (!string.IsNullOrEmpty(orderAmount))
                {
                    model.OrderAmount = int.Parse(orderAmount);
                }

                if (!string.IsNullOrEmpty(orderScore))
                {
                    model.OrderScore = int.Parse(orderScore);
                }

                if (!string.IsNullOrEmpty(exchangeAmount))
                {
                    model.ExchangeAmount = decimal.Parse(exchangeAmount);
                }
                if (!string.IsNullOrEmpty(exchangeScore))
                {
                    model.ExchangeScore = int.Parse(exchangeScore);
                }
                if (bllScore.Update(model))
                {
                    resp.Msg = "保存成功!";

                }
                else
                {
                    resp.Msg = "保存失败";
                }
            }
            else
            {
                model = new ScoreConfig();
                // model.ScoreRule = ScoreRule;

                model.WebsiteOwner = bllScore.WebsiteOwner;

                if (!string.IsNullOrEmpty(orderAmount))
                {
                    model.OrderAmount = int.Parse(orderAmount);
                }

                if (!string.IsNullOrEmpty(orderScore))
                {
                    model.OrderScore = int.Parse(orderScore);
                }

                if (!string.IsNullOrEmpty(exchangeAmount))
                {
                    model.ExchangeAmount = decimal.Parse(exchangeAmount);
                }
                if (!string.IsNullOrEmpty(exchangeScore))
                {
                    model.ExchangeScore = int.Parse(exchangeScore);
                }
                if (bllScore.Add(model))
                {
                    resp.Msg = "保存成功!";

                }
                else
                {
                    resp.Msg = "保存失败";
                }

            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 更新线上商城配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDistributionMallConfig(HttpContext context)
        {
            resp.IsSuccess = false;

            var distributionShareQrcodeBgImg = context.Request["distributionShareQrcodeBgImg"];

            string distributionRateLevel1 = context.Request["distributionRateLevel1"];
            string distributionRateLevel2 = context.Request["distributionRateLevel2"];
            string distributionRateLevel3 = context.Request["distributionRateLevel3"];

            string isHideHeadImg = context.Request["IsHideHeadImg"];
            string nickShowPosition = context.Request["WXNickShowPosition"];
            string qRCodeUseGuide = context.Request["QRCodeUseGuide"];
            string isShowWXNickName = context.Request["IsShowWXNickName"];
            string wXNickNameFontColor = context.Request["WXNickNameFontColor"];
            string notDistributionMsg = context.Request["NotDistributionMsg"];

            var website = bllBase.GetWebsiteInfoModelFromDataBase();

            website.DistributionShareQrcodeBgImg = distributionShareQrcodeBgImg;

            website.DistributionRateLevel1 = string.IsNullOrEmpty(distributionRateLevel1) ? 0 : double.Parse(distributionRateLevel1);
            website.DistributionRateLevel2 = string.IsNullOrEmpty(distributionRateLevel2) ? 0 : double.Parse(distributionRateLevel2);
            website.DistributionRateLevel3 = string.IsNullOrEmpty(distributionRateLevel3) ? 0 : double.Parse(distributionRateLevel3);
            website.NotDistributionMsg = notDistributionMsg;
            if (bllBase.Update(website))
            {
                CompanyWebsite_Config config = bllWebsite.GetCompanyWebsiteConfig();
                config.IsHideHeadImg = Convert.ToInt32(isHideHeadImg);
                config.WXNickShowPosition = Convert.ToInt32(nickShowPosition);
                config.QRCodeUseGuide = qRCodeUseGuide;
                config.IsShowWXNickName = Convert.ToInt32(isShowWXNickName);
                config.WXNickNameFontColor = wXNickNameFontColor;
                resp.IsSuccess = bllWebsite.Update(config);
            }

            bllLog.Add(BLLJIMP.Enums.EnumLogType.DistributionOffLine, BLLJIMP.Enums.EnumLogTypeAction.Config, bllLog.GetCurrUserID(), "商城分销配置[" + bllLog.GetCurrUserID() + "]");

            return JsonConvert.SerializeObject(resp);
        }



        #region 投票设置
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryVoteRecharge(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string voteId = context.Request["VoteId"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" VoteId={0}", voteId));
            int totalCount = this.bllMall.GetCount<VoteRecharge>(sbWhere.ToString());
            List<VoteRecharge> dataList = this.bllJuActivity.GetLit<VoteRecharge>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteRecharge(HttpContext context)
        {

            int voteId = int.Parse(context.Request["VoteId"]);
            string rechargeCountStr = context.Request["RechargeCount"];
            string amountStr = context.Request["Amount"];
            string giftName = context.Request["GiftName"];
            string giftDesc = context.Request["GiftDesc"];
            int rechargeCount = 0;
            decimal amount = 0;
            if (string.IsNullOrEmpty(rechargeCountStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入票数";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(amountStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入金额";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!int.TryParse(rechargeCountStr, out rechargeCount))
            {
                resp.Status = 0;
                resp.Msg = "票数不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (rechargeCount <= 0)
            {
                resp.Status = 0;
                resp.Msg = "票数须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!decimal.TryParse(amountStr, out amount))
            {
                resp.Status = 0;
                resp.Msg = "金额不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (amount <= 0)
            {
                resp.Status = 0;
                resp.Msg = "金额须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }



            VoteRecharge model = new VoteRecharge();
            model.RechargeCount = rechargeCount;
            model.Amount = amount;
            model.GiftName = giftName;
            model.GiftDesc = giftDesc;
            model.VoteId = voteId;
            if (bllVote.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditVoteRecharge(HttpContext context)
        {
            int autoID = int.Parse(context.Request["AutoID"]);
            int voteId = int.Parse(context.Request["VoteId"]);
            string rechargeCountStr = context.Request["RechargeCount"];
            string amountStr = context.Request["Amount"];
            string giftName = context.Request["GiftName"];
            string giftDesc = context.Request["GiftDesc"];
            int rechargeCount = 0;
            decimal amount = 0;
            if (string.IsNullOrEmpty(rechargeCountStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入票数";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(amountStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入金额";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!int.TryParse(rechargeCountStr, out rechargeCount))
            {
                resp.Status = 0;
                resp.Msg = "票数不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (rechargeCount <= 0)
            {
                resp.Status = 0;
                resp.Msg = "票数须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!decimal.TryParse(amountStr, out amount))
            {
                resp.Status = 0;
                resp.Msg = "金额不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (amount <= 0)
            {
                resp.Status = 0;
                resp.Msg = "金额须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            var model = bllVote.Get<VoteRecharge>(string.Format("AutoID={0}", autoID));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.RechargeCount = rechargeCount;
            model.Amount = amount;
            model.GiftName = giftName;
            model.GiftDesc = giftDesc;
            if (bllJuActivity.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteVoteRecharge(HttpContext context)
        {
            string ids = context.Request["ids"];
            int count = bllVote.Delete(new VoteRecharge(), string.Format("AutoID in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion


        #region 支付配置
        /// <summary>
        /// 支付宝
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetAlipayConfig(HttpContext context)
        {
            string partner = context.Request["Partner"];
            string partnerKey = context.Request["PartnerKey"];
            string sellerAccountName = context.Request["SellerAccountName"];

            var model = bllMall.Get<ZentCloud.BLLJIMP.Model.PayConfig>(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
            if (model != null)
            {
                model.Partner = partner;
                model.PartnerKey = partnerKey;
                model.Seller_Account_Name = sellerAccountName;
                if (bllMall.Update(model))
                {
                    resp.Status = 1;
                }

            }
            else
            {
                model = new BLLJIMP.Model.PayConfig();
                model.Partner = partner;
                model.PartnerKey = partnerKey;
                model.Seller_Account_Name = sellerAccountName;
                model.WebsiteOwner = bllMall.WebsiteOwner;
                if (bllMall.Add(model))
                {
                    resp.Status = 1;
                }

            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 保存支付配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavePayConfig(HttpContext context)
        {

            PayConfig requestModel = bllBase.ConvertRequestToModel<PayConfig>(new PayConfig());
            var model = bllPay.GetPayConfig();
            if (model != null)
            {

                model.WXAppId = requestModel.WXAppId;
                model.WXMCH_ID = requestModel.WXMCH_ID;
                model.WXPartnerKey = requestModel.WXPartnerKey;
                if (bllPay.Update(model))
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.System, BLLJIMP.Enums.EnumLogTypeAction.Config, bllLog.GetCurrUserID(), "支付配置[" + bllLog.GetCurrUserID() + "]");
                    resp.Status = 1;
                }

            }
            else
            {
                requestModel.WebsiteOwner = bllPay.WebsiteOwner;
                if (bllMall.Add(requestModel))
                {
                    resp.Status = 1;
                }

            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }
        #endregion


        #region 配送方式管理

        /// <summary>
        /// 查询配送方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallDelivery(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string deliveryName = context.Request["DeliveryName"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(deliveryName))
            {
                sbWhere.AppendFormat(" And DeliveryName like '%{0}%'", deliveryName);
            }
            int totalCount = bllMall.GetCount<WXMallDelivery>(sbWhere.ToString());
            List<WXMallDelivery> dataList = new List<WXMallDelivery>();
            dataList = bllMall.GetLit<WXMallDelivery>(pageSize, pageIndex, sbWhere.ToString(), "Sort ASC,AutoID ASC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加配送方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallDelivery(HttpContext context)
        {
            string sort = string.IsNullOrEmpty(context.Request["Sort"]) ? "0" : context.Request["Sort"];
            string deliveryTypeStr = context.Request["DeliveryType"];
            string deliveryName = context.Request["DeliveryName"];
            string initialProductCountStr = context.Request["InitialProductCount"];
            string initialDeliveryMoneyStr = context.Request["InitialDeliveryMoney"];
            string addProductCountStr = context.Request["AddProductCount"];
            string addMoneyStr = context.Request["AddMoney"];
            int deliveryTypeInt = 0;
            int initialProductCountInt = 0;
            decimal initialDeliveryMoneyD = 0;
            int addProductCountInt = 0;
            decimal addMoneyD = 0;
            if (string.IsNullOrEmpty(deliveryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入配送方式名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(deliveryTypeStr))
            {
                resp.Status = 0;
                resp.Msg = "请选择配送方式";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (deliveryTypeStr.Equals("0"))
            {
                if (string.IsNullOrEmpty(initialProductCountStr))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入初始商品数量";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (string.IsNullOrEmpty(initialDeliveryMoneyStr))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入初始配送费用";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (string.IsNullOrEmpty(addProductCountStr))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入增量商品数量";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (string.IsNullOrEmpty(addMoneyStr))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入增量配送费用";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (!int.TryParse(deliveryTypeStr, out deliveryTypeInt))
                {
                    resp.Status = 0;
                    resp.Msg = "请选择正确的配送方式";
                    return Common.JSONHelper.ObjectToJson(resp);

                }

                //初始商品数量
                if (!int.TryParse(initialProductCountStr, out initialProductCountInt))
                {
                    resp.Status = 0;
                    resp.Msg = "初始商品数量不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (initialProductCountInt <= 0)
                {
                    resp.Status = 0;
                    resp.Msg = "初始商品数量需大于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //

                //初始配送费用
                if (!decimal.TryParse(initialDeliveryMoneyStr, out initialDeliveryMoneyD))
                {
                    resp.Status = 0;
                    resp.Msg = "初始配送费用不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (initialDeliveryMoneyD < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "初始配送费用不正确需大于等于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //


                //增量商品数量
                if (!int.TryParse(addProductCountStr, out addProductCountInt))
                {
                    resp.Status = 0;
                    resp.Msg = "增量商品数量不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (addProductCountInt <= 0)
                {
                    resp.Status = 0;
                    resp.Msg = "增量商品数量需大于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //

                //增量配送费用
                if (!decimal.TryParse(addMoneyStr, out addMoneyD))
                {
                    resp.Status = 0;
                    resp.Msg = "增量配送费用不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (addMoneyD < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "增量配送费用需要大于等于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //
            }

            WXMallDelivery model = new WXMallDelivery();
            model.Sort = int.Parse(sort);
            model.DeliveryType = int.Parse(deliveryTypeStr);
            model.DeliveryName = deliveryName;
            model.InitialProductCount = initialProductCountInt;
            model.InitialDeliveryMoney = initialDeliveryMoneyD;
            model.AddProductCount = addProductCountInt;
            model.AddMoney = addMoneyD;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllMall.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";

            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 编辑配送方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallDelivery(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoId"]);
            string sortStr = string.IsNullOrEmpty(context.Request["Sort"]) ? "0" : context.Request["Sort"];
            string deliveryTypeStr = context.Request["DeliveryType"];
            string deliveryName = context.Request["DeliveryName"];
            string initialProductCountStr = context.Request["InitialProductCount"];
            string initialDeliveryMoneyStr = context.Request["InitialDeliveryMoney"];
            string addProductCountStr = context.Request["AddProductCount"];
            string addMoneyStr = context.Request["AddMoney"];
            int deliveryTypeInt = 0;
            int initialProductCountInt = 0;
            decimal initialDeliveryMoneyD = 0;
            int addProductCountInt = 0;
            decimal addMoneyD = 0;
            if (string.IsNullOrEmpty(deliveryName))
            {
                resp.Status = 0;
                resp.Msg = "请输入配送方式名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(deliveryTypeStr))
            {
                resp.Status = 0;
                resp.Msg = "请选择配送方式";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(initialProductCountStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入初始商品数量";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(initialDeliveryMoneyStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入初始配送费用";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(addProductCountStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入增量商品数量";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(addMoneyStr))
            {
                resp.Status = 0;
                resp.Msg = "请输入增量配送费用";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!int.TryParse(deliveryTypeStr, out deliveryTypeInt))
            {
                resp.Status = 0;
                resp.Msg = "请选择正确的配送方式";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (deliveryTypeStr.Equals("0"))
            {

                //初始商品数量
                if (!int.TryParse(initialProductCountStr, out initialProductCountInt))
                {
                    resp.Status = 0;
                    resp.Msg = "初始商品数量不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (initialProductCountInt <= 0)
                {
                    resp.Status = 0;
                    resp.Msg = "初始商品数量需大于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //

                //初始配送费用
                if (!decimal.TryParse(initialDeliveryMoneyStr, out initialDeliveryMoneyD))
                {
                    resp.Status = 0;
                    resp.Msg = "初始配送费用不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (initialDeliveryMoneyD < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "初始配送费用不正确需大于等于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //

                //增量商品数量
                if (!int.TryParse(addProductCountStr, out addProductCountInt))
                {
                    resp.Status = 0;
                    resp.Msg = "增量商品数量不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (addProductCountInt <= 0)
                {
                    resp.Status = 0;
                    resp.Msg = "增量商品数量需大于等于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //

                //增量配送费用
                if (!decimal.TryParse(addMoneyStr, out addMoneyD))
                {
                    resp.Status = 0;
                    resp.Msg = "增量配送费用不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                if (addMoneyD < 0)
                {
                    resp.Status = 0;
                    resp.Msg = "增量配送费用需要大于等于0";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
                //
            }

            WXMallDelivery model = bllMall.GetDelivery(autoId);
            model.Sort = int.Parse(sortStr);
            model.DeliveryType = int.Parse(deliveryTypeStr);
            model.DeliveryName = deliveryName;
            model.InitialProductCount = initialProductCountInt;
            model.InitialDeliveryMoney = initialDeliveryMoneyD;
            model.AddProductCount = addProductCountInt;
            model.AddMoney = addMoneyD;
            if (bllMall.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";

            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallDelivery(HttpContext context)
        {

            string ids = context.Request["ids"];
            int count = bllMall.Delete(new WXMallDelivery(), string.Format("AutoId in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);



        }

        #endregion

        #region 支付方式管理

        /// <summary>
        /// 查询支付方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMallPaymentType(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string paymentTypeName = context.Request["PaymentTypeName"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(paymentTypeName))
            {
                sbWhere.AppendFormat(" And PaymentTypeName like '%{0}%'", paymentTypeName);
            }
            int totalCount = bllJuActivity.GetCount<WXMallPaymentType>(sbWhere.ToString());
            List<WXMallPaymentType> dataList = new List<WXMallPaymentType>();
            dataList = bllJuActivity.GetLit<WXMallPaymentType>(pageSize, pageIndex, sbWhere.ToString(), "Sort ASC,AutoID ASC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加支付方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMallPaymentType(HttpContext context)
        {
            string sortStr = string.IsNullOrEmpty(context.Request["Sort"]) ? "0" : context.Request["Sort"];
            string paymentType = context.Request["PaymentType"];
            string paymentTypeName = context.Request["PaymentTypeName"];


            string alipayPartner = context.Request["AlipayPartner"];
            string alipayPartnerKey = context.Request["AlipayPartnerKey"];
            string alipaySeller_Account_Name = context.Request["AlipaySeller_Account_Name"];


            string wxAppId = context.Request["WXAppId"];
            string wxAppKey = context.Request["WXAppKey"];
            string wxPartner = context.Request["WXPartner"];
            string wxPartnerKey = context.Request["WXPartnerKey"];

            if (string.IsNullOrEmpty(paymentTypeName))
            {
                resp.Status = 0;
                resp.Msg = "请输入配送方式名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(paymentType))
            {
                resp.Status = 0;
                resp.Msg = "请选择支付方式";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (paymentType.Equals("1"))//支付宝
            {
                if (string.IsNullOrEmpty(alipayPartner) || string.IsNullOrEmpty(alipayPartnerKey) || string.IsNullOrEmpty(alipaySeller_Account_Name))
                {
                    resp.Status = 0;
                    resp.Msg = "支付宝 PID KEY 支付宝账号必填";
                    return Common.JSONHelper.ObjectToJson(resp);
                }


            }
            if (paymentType.Equals("2"))//微信支付
            {

                if (string.IsNullOrEmpty(wxAppId) || string.IsNullOrEmpty(wxAppKey) || string.IsNullOrEmpty(wxPartner) || string.IsNullOrEmpty(wxPartnerKey))
                {
                    resp.Status = 0;
                    resp.Msg = "微信支付 AppId AppKey WXPartner WXPartnerKey必填";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

            }

            WXMallPaymentType model = new WXMallPaymentType();
            model.Sort = int.Parse(sortStr);
            model.PaymentType = int.Parse(paymentType);
            model.PaymentTypeName = paymentTypeName;
            model.AlipayPartner = alipayPartner;
            model.AlipayPartnerKey = alipayPartnerKey;
            model.AlipaySeller_Account_Name = alipaySeller_Account_Name;
            model.WXAppId = wxAppId;
            model.WXAppKey = wxAppKey;
            model.WXPartner = wxPartner;
            model.WXPartnerKey = wxPartnerKey;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.IsDisable = 0;
            if (bllMall.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";

            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 编辑支付方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMallPaymentType(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoId"]);
            string sort = string.IsNullOrEmpty(context.Request["Sort"]) ? "0" : context.Request["Sort"];
            string paymentType = context.Request["PaymentType"];
            string paymentTypeName = context.Request["PaymentTypeName"];


            string alipayPartner = context.Request["AlipayPartner"];
            string alipayPartnerKey = context.Request["AlipayPartnerKey"];
            string alipaySellerAccountName = context.Request["AlipaySeller_Account_Name"];

            string wxAppId = context.Request["WXAppId"];
            string wxAppKey = context.Request["WXAppKey"];
            string wxPartner = context.Request["WXPartner"];
            string wxPartnerKey = context.Request["WXPartnerKey"];

            if (string.IsNullOrEmpty(paymentTypeName))
            {
                resp.Status = 0;
                resp.Msg = "请输入支付方式名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(paymentType))
            {
                resp.Status = 0;
                resp.Msg = "请选择支付方式";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (paymentType.Equals("1"))//支付宝
            {
                if (string.IsNullOrEmpty(alipayPartner) || string.IsNullOrEmpty(alipayPartnerKey) || string.IsNullOrEmpty(alipaySellerAccountName))
                {
                    resp.Status = 0;
                    resp.Msg = "支付宝 PID KEY 支付宝账号必填";
                    return Common.JSONHelper.ObjectToJson(resp);
                }


            }
            if (paymentType.Equals("2"))//微信支付
            {

                if (string.IsNullOrEmpty(wxAppId) || string.IsNullOrEmpty(wxAppKey) || string.IsNullOrEmpty(wxPartner) || string.IsNullOrEmpty(wxPartnerKey))
                {
                    resp.Status = 0;
                    resp.Msg = "微信支付 AppId AppKey WXPartner WXPartnerKey必填";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

            }

            WXMallPaymentType model = bllMall.GetPaymentType(autoId);
            model.Sort = int.Parse(sort);
            model.PaymentType = int.Parse(paymentType);
            model.PaymentTypeName = paymentTypeName;
            model.AlipayPartner = alipayPartner;
            model.AlipayPartnerKey = alipayPartnerKey;
            model.AlipaySeller_Account_Name = alipaySellerAccountName;
            model.WXAppId = wxAppId;
            model.WXAppKey = wxAppKey;
            model.WXPartner = wxPartner;
            model.WXPartnerKey = wxPartnerKey;

            if (bllMall.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";

            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除支付方式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMallPaymentType(HttpContext context)
        {

            string ids = context.Request["ids"];
            int count = bllMall.Delete(new WXMallPaymentType(), string.Format("AutoId in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);



        }
        /// <summary>
        /// 启用支付类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWXMallPaymentTypeStatu(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (bllMall.Update(new WXMallPaymentType(), string.Format(" IsDisable={0}", context.Request["IsDisable"]), string.Format("AutoId in ({0}) And WebsiteOwner='{1}'", ids, bllMall.WebsiteOwner)) == ids.Split(',').Length)
            {
                resp.Status = 1;
            }

            return Common.JSONHelper.ObjectToJson(resp);



        }



        #endregion

        #region 问卷管理

        /// <summary>
        /// 查询问卷
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryQuestionnaire(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string QuestionnaireType = context.Request["QuestionnaireType"];
            string questionnaireName = context.Request["QuestionnaireName"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(questionnaireName))
            {
                sbWhere.AppendFormat(" And QuestionnaireName like '%{0}%'", questionnaireName);
            }
            if (!string.IsNullOrEmpty(QuestionnaireType))
            {
                sbWhere.AppendFormat(" And QuestionnaireType={0}", QuestionnaireType);
            }

            int totalCount = bllJuActivity.GetCount<Questionnaire>(sbWhere.ToString());
            List<Questionnaire> dataList = new List<Questionnaire>();
            dataList = bllJuActivity.GetLit<Questionnaire>(pageSize, pageIndex, sbWhere.ToString(), "QuestionnaireID DESC");
            foreach (var item in dataList)
            {
                item.SubmitCount = bllBase.GetCount<QuestionnaireRecord>(string.Format("QuestionnaireID={0}", item.QuestionnaireID));
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = dataList
            });
        }


        /// <summary>
        /// 添加问卷
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddQuestionnaire(HttpContext context)
        {
            string json = context.Request["JsonData"];

            QuestionnaireModel model = Common.JSONHelper.JsonToModel<QuestionnaireModel>(json);//jSON 反序列化

            var x = model.QuestionnaireStopDate.ToString();

            if (x == "0001/1/1 0:00:00")
            {
                resp.Msg = "请输入结束时间";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();//事务
            try
            {

                Questionnaire Questionnaire = new Questionnaire();//数据库问卷表模型
                Questionnaire.QuestionnaireID = int.Parse(bllBase.GetGUID(TransacType.AddQuestionnaire));
                Questionnaire.QuestionnaireName = model.QuestionnaireName;
                Questionnaire.QuestionnaireContent = model.QuestionnaireContent;
                Questionnaire.QuestionnaireStopDate = model.QuestionnaireStopDate;

                Questionnaire.QuestionnaireVisible = model.QuestionnaireVisible;
                Questionnaire.QuestionnaireImage = model.QuestionnaireImage;
                Questionnaire.QuestionnaireSummary = model.QuestionnaireSummary;
                Questionnaire.QuestionnaireType = model.QuestionnaireType;
                Questionnaire.EachPageNum = model.EachPageNum;
                Questionnaire.WebsiteOwner = bllBase.WebsiteOwner;
                Questionnaire.InsertDate = DateTime.Now;
                Questionnaire.AddScore = model.AddScore;
                Questionnaire.IsWeiXinLicensing = model.IsWeiXinLicensing;
                Questionnaire.ButtonText = model.ButtonText;
                Questionnaire.ButtonLink = model.ButtonLink;
                Questionnaire.QuestionnaireSubmitUrl = model.QuestionnaireSubmitUrl;
                Questionnaire.QuestionnaireRepeatSubmitUrl = model.QuestionnaireRepeatSubmitUrl;
                if (!string.IsNullOrEmpty(model.ExamMinute))
                {
                    Questionnaire.ExamMinute = int.Parse(model.ExamMinute);
                }
                if (!bllBase.Add(Questionnaire, tran))//添加问卷表
                {
                    resp.Msg = "发布失败";
                    tran.Rollback();
                }
                int sortNum = 0;
                foreach (var item in model.QuestionList)//添加问题表
                {
                    sortNum++;
                    Question question = new Question();
                    question.QuestionID = int.Parse(bllBase.GetGUID(TransacType.AddQuestion));
                    question.QuestionnaireID = Questionnaire.QuestionnaireID;
                    question.QuestionName = item.QuestionName;
                    question.QuestionType = item.QuestionType;
                    question.IsRequired = item.IsRequired;
                    question.AnswerGroupName = item.AnswerGroupName;
                    question.Sort = sortNum;
                    if (!string.IsNullOrEmpty(item.AnswerGroupName))
                    {
                        string[] groupNames = item.AnswerGroupName.Split(',');
                        int bGroup = groupNames.Distinct().Count();
                        if (bGroup < groupNames.Length)
                        {
                            resp.Msg = "多组名称重复";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                    }


                    if (!bllBase.Add(question, tran))
                    {
                        resp.Msg = "发布失败";
                        tran.Rollback();
                    }
                    foreach (var AnswerItem in item.Answer)
                    {
                        Answer answer = new Answer();
                        answer.AnswerID = int.Parse(bllBase.GetGUID(TransacType.AddAnswer));
                        answer.AnswerName = AnswerItem.AnswerName;
                        answer.IsCorrect = AnswerItem.IsCorrect;
                        answer.QuestionID = question.QuestionID;
                        answer.QuestionnaireID = Questionnaire.QuestionnaireID;
                        if (!bllBase.Add(answer, tran))
                        {
                            resp.Msg = "发布失败";
                            tran.Rollback();
                        }
                    }
                }

                tran.Commit();
                resp.Status = 1;
                resp.Msg = "发布成功";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加问卷[id=" + Questionnaire.QuestionnaireID + "]");
            }
            catch (Exception ex)
            {

                resp.Msg = ex.Message;
                tran.Rollback();
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteQuestionnaire(HttpContext context)
        {
            string ids = context.Request["ids"];

            foreach (var item in ids.Split(','))
            {
                if (bllMall.GetCount<WXMallOrderDetailsInfo>(string.Format("ExQuestionnaireID={0}", item)) > 0)
                {
                    return string.Format("考卷已经有用户下单,不能删除");
                }
            }
            int count = bllJuActivity.Delete(new Questionnaire(), string.Format("QuestionnaireID in ({0})", ids));
            bllJuActivity.Delete(new Answer(), string.Format("QuestionnaireID in ({0})", ids));
            bllJuActivity.Delete(new Question(), string.Format("QuestionnaireID in ({0})", ids));
            bllJuActivity.Delete(new QuestionnaireRecord(), string.Format("QuestionnaireID in ({0})", ids));
            bllJuActivity.Delete(new QuestionnaireRecordDetail(), string.Format("QuestionnaireID in ({0})", ids));
            bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除问卷[id=" + ids + "]");
            return string.Format("成功删除了 {0} 个问卷", count);

        }


        /// <summary>
        /// 删除问卷提交记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteQuestionnaireRecord(HttpContext context)
        {
            string ids = context.Request["ids"];
            foreach (var id in ids.Split(','))
            {
                QuestionnaireRecord record = bllBase.Get<QuestionnaireRecord>(string.Format("AutoId={0}", id));
                bllBase.Delete(new QuestionnaireRecordDetail(), string.Format("UserID ='{0}'", record.UserId));
                bllBase.Delete(record);

            }
            return string.Format("成功删除了{0}条记录", ids.Split(',').Length);

        }

        /// <summary>
        /// 查询问卷提交记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryQuestionnaireRecord(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string questionnaireId = context.Request["QuestionnaireID"];
            StringBuilder sbWhere = new StringBuilder(string.Format("QuestionnaireID={0}", questionnaireId));
            int totalCount = bllJuActivity.GetCount<QuestionnaireRecord>(sbWhere.ToString());
            List<QuestionnaireRecord> dataList = new List<QuestionnaireRecord>();
            dataList = bllJuActivity.GetLit<QuestionnaireRecord>(pageSize, pageIndex, sbWhere.ToString(), "AutoId DESC");
            foreach (QuestionnaireRecord item in dataList)
            {
                UserInfo userModel = bllUser.GetUserInfo(item.UserId);
                if (userModel != null)
                {
                    item.WXHeadimgurl = userModel.WXHeadimgurl;
                    item.WXNickname = userModel.WXNickname;
                }
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = dataList
            });


        }

        /// <summary>
        /// 编辑问卷
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditQuestionnaire(HttpContext context)
        {
            string json = context.Request["JsonData"];
            QuestionnaireModel model = Common.JSONHelper.JsonToModel<QuestionnaireModel>(json);//jSON 反序列化


            var x = model.QuestionnaireStopDate.ToString();

            if (x == "0001/1/1 0:00:00")
            {
                resp.Msg = "请输入结束时间";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();//事务
            try
            {

                Questionnaire Questionnaire = bllBase.Get<Questionnaire>(string.Format("QuestionnaireID={0}", model.QuestionnaireID.ToString()));
                if (Questionnaire == null)
                {
                    resp.Status = 0;
                    resp.Msg = "保存失败";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                Questionnaire.QuestionnaireName = model.QuestionnaireName;
                Questionnaire.QuestionnaireContent = model.QuestionnaireContent;
                Questionnaire.QuestionnaireStopDate = model.QuestionnaireStopDate;
                Questionnaire.QuestionnaireVisible = model.QuestionnaireVisible;
                Questionnaire.QuestionnaireImage = model.QuestionnaireImage;
                Questionnaire.QuestionnaireSummary = model.QuestionnaireSummary;
                Questionnaire.QuestionnaireType = model.QuestionnaireType;
                Questionnaire.EachPageNum = model.EachPageNum;
                Questionnaire.AddScore = model.AddScore;
                Questionnaire.ButtonText = model.ButtonText;
                Questionnaire.ButtonLink = model.ButtonLink;
                Questionnaire.QuestionnaireSubmitUrl = model.QuestionnaireSubmitUrl;
                Questionnaire.QuestionnaireRepeatSubmitUrl = model.QuestionnaireRepeatSubmitUrl;
                Questionnaire.IsWeiXinLicensing = model.IsWeiXinLicensing;
                if (!bllBase.Update(Questionnaire, tran))//添加问卷表
                {
                    resp.Msg = "保存失败";
                    tran.Rollback();
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (model.QuestionList != null && model.QuestionList.Count > 0)
                {
                    QuestionnaireRecord oldRecord = bllBase.Get<QuestionnaireRecord>(string.Format("QuestionnaireID={0}", model.QuestionnaireID.ToString()));
                    if (oldRecord != null)
                    {
                        resp.Msg = "已经答题，禁止修改题库内的题目！";
                        tran.Rollback();
                        return Common.JSONHelper.ObjectToJson(resp);
                    }

                    List<Question> oldQuestions = bllBase.GetList<Question>(string.Format("QuestionnaireID={0}", model.QuestionnaireID.ToString()));
                    List<Answer> oldAnswers = bllBase.GetList<Answer>(string.Format("QuestionnaireID={0}", model.QuestionnaireID.ToString()));

                    //if (bllBase.DeleteByKey<QuestionnaireRecordDetail>("QuestionnaireID", model.QuestionnaireID.ToString(), tran) < 0
                    //  || bllBase.DeleteByKey<QuestionnaireRecord>("QuestionnaireID", model.QuestionnaireID.ToString(), tran) < 0
                    //  || bllBase.DeleteByKey<Answer>("QuestionnaireID", model.QuestionnaireID.ToString(), tran) < 0
                    //  || bllBase.DeleteByKey<Question>("QuestionnaireID", model.QuestionnaireID.ToString(), tran) < 0) 
                    //{
                    //    resp.Msg = "删除原题目失败";
                    //    tran.Rollback();
                    //    return Common.JSONHelper.ObjectToJson(resp);
                    //}
                    List<int> nqIdList = model.QuestionList.Select(p => p.QuestionID).ToList();
                    foreach (var item in oldQuestions.Where(p => !nqIdList.Contains(p.QuestionID)))
                    {
                        foreach (var oAnswerItem in oldAnswers.Where(p => p.QuestionID == item.QuestionID))
                        {
                            if (bllBase.Delete(oAnswerItem) < 0)
                            {
                                resp.Msg = "删除旧选项失败";
                                tran.Rollback();
                                return Common.JSONHelper.ObjectToJson(resp);
                            }
                        }
                        if (bllBase.Delete(item) < 0)
                        {
                            resp.Msg = "删除旧题目";
                            tran.Rollback();
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                    }
                    int sortNum = 0;
                    foreach (var item in model.QuestionList)//添加问题表
                    {
                        sortNum++;
                        Question question = new Question();
                        question.QuestionName = item.QuestionName;
                        question.QuestionType = item.QuestionType;
                        question.IsRequired = item.IsRequired;
                        question.QuestionnaireID = Questionnaire.QuestionnaireID;
                        question.AnswerGroupName = item.AnswerGroupName;
                        question.Sort = sortNum;
                        bool nqResult = false;
                        if (item.QuestionID == 0)
                        {
                            question.QuestionID = int.Parse(bllBase.GetGUID(TransacType.AddQuestion));
                            nqResult = bllBase.Add(question, tran);
                        }
                        else
                        {
                            question.QuestionID = item.QuestionID;
                            nqResult = bllBase.Update(question, tran);
                        }
                        if (!nqResult)
                        {
                            resp.Msg = "修改题目失败";
                            tran.Rollback();
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                        if (item.QuestionID != 0)
                        {
                            List<int> naIdList = item.Answer.Select(p => p.AnswerID).ToList();
                            foreach (var oAnswerItem in oldAnswers.Where(p => p.QuestionID == item.QuestionID && !naIdList.Contains(p.AnswerID)))
                            {
                                if (bllBase.Delete(oAnswerItem) < 0)
                                {
                                    resp.Msg = "删除旧选项失败";
                                    tran.Rollback();
                                    return Common.JSONHelper.ObjectToJson(resp);
                                }
                            }
                        }
                        foreach (var AnswerItem in item.Answer)
                        {
                            Answer answer = new Answer();
                            answer.AnswerName = AnswerItem.AnswerName;
                            answer.IsCorrect = AnswerItem.IsCorrect;
                            answer.QuestionID = question.QuestionID;
                            answer.QuestionnaireID = Questionnaire.QuestionnaireID;
                            bool naResult = false;
                            answer.AnswerID = int.Parse(bllBase.GetGUID(TransacType.AddAnswer));
                            if (AnswerItem.AnswerID == 0)
                            {
                                answer.AnswerID = int.Parse(bllBase.GetGUID(TransacType.AddQuestion));
                                naResult = bllBase.Add(answer, tran);
                            }
                            else
                            {
                                answer.AnswerID = AnswerItem.AnswerID;
                                naResult = bllBase.Update(answer, tran);
                            }
                            if (!naResult)
                            {
                                resp.Msg = "修改选项失败";
                                tran.Rollback();
                                return Common.JSONHelper.ObjectToJson(resp);
                            }
                        }
                    }
                }
                tran.Commit();
                resp.Status = 1;
                resp.Msg = "保存成功";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "修改问卷[id=" + Questionnaire.QuestionnaireID + "]");
            }
            catch (Exception ex)
            {

                resp.Msg = ex.Message;
                tran.Rollback();
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }




        /// <summary>
        /// 保存考试结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveExamResult(HttpContext context)
        {
            string id = context.Request["id"];
            string userId = context.Request["userId"];
            string result = context.Request["result"];
            QuestionnaireRecord record = bllBase.Get<QuestionnaireRecord>(string.Format("QuestionnaireID={0} And UserId='{1}'", id, userId));
            record.Result = result;
            if (bllBase.Update(record))
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "保存失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);




        }

        #endregion

        #region 用户标签管理

        /// <summary>
        /// 查询用户标签管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryMemberTag(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string tagName = context.Request["TagName"], tagType = context.Request["TagType"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(tagName))
            {
                sbWhere.AppendFormat(" And TagName like '%{0}%'", tagName);
            }
            if (!string.IsNullOrEmpty(tagType))
            {
                sbWhere.AppendFormat(" And TagType = '{0}'", tagType);
            }
            int totalCount = this.bllBase.GetCount<MemberTag>(sbWhere.ToString());
            List<MemberTag> dataList = new List<MemberTag>();

            dataList.AddRange(this.bllBase.GetLit<MemberTag>(pageSize, pageIndex, sbWhere.ToString()));
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 删除会员标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteMemberTag(HttpContext context)
        {
            BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
            bool isData = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);
            if (isData)
            {
                resp.Status = 0;
                resp.Msg = "权限不足";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            string ids = context.Request["ids"];
            string tagName = context.Request["TagName"];
            MemberTag model;
            foreach (var item in ids.Split(','))
            {
                model = bllBase.Get<MemberTag>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";
                }

            }

            foreach (var itm in tagName.Split(','))
            {
                var modelTagName = bllBase.Get<UserInfo>(string.Format("TagName like '%{0}%' AND WebsiteOwner='{1}' AND UserID !='{1}' ", itm, bllBase.WebsiteOwner));
                if (modelTagName != null)
                {
                    resp.Status = 3;
                    resp.Msg = string.Format("用户已添加'{0}'标签", itm);
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            int count = bllBase.Delete(new MemberTag(), string.Format("AutoID in ({0})", ids));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了 {0} 条数据", count);
            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 更新会员标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateUserTagName(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            string tagName = context.Request["TagName"];
            string accessLevel = context.Request["AccessLevel"];

            string[] str = accessLevel.Split(',');
            int max = 0;
            for (int i = 0; i < str.Length; i++)
            {
                int a = Convert.ToInt32(str[i]);
                if (a > max)
                {
                    max = a;
                }
            }


            int count = bllBase.Update(new UserInfo(), string.Format(" TagName= '{0}',AccessLevel={1}", tagName, max), string.Format(" AutoID in ({0})", autoId));
            if (count > 0)
            {
                resp.Status = 1;

            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 在用户原来的标签基础上增加标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateUserTagNameByAddTag(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            string tagName = context.Request["TagName"];

            string strTag = string.Empty;
            List<string> tags = tagName.Split(',').Select(p => p.Trim()).ToList();
            if (tags.Count > 0)
            {
                List<UserInfo> userList = bllUser.GetMultListByKey<UserInfo>("AutoID", autoId);
                for (int i = 0; i < userList.Count; i++)
                {
                    List<string> arrayList = new List<string>();

                    if (!string.IsNullOrEmpty(userList[i].TagName))
                    {
                        arrayList = userList[i].TagName.Split(',').Select(p => p.Trim()).ToList();
                        arrayList = arrayList.Where(p => !tags.Contains(p)).ToList();
                    }
                    arrayList.AddRange(tags);
                    string nTageName = "";
                    if (arrayList.Count > 0)
                    {
                        nTageName = ZentCloud.Common.MyStringHelper.ListToStr(arrayList, "", ",");
                    }
                    userList[i].TagName = nTageName;
                    //bllBase.Update(userList[i]);
                    bllBase.Update(new UserInfo(), string.Format(" TagName='{0}' ", userList[i].TagName), string.Format(" AutoID = {0} ", userList[i].AutoID));
                    //strTag = string.Empty;
                }
            }
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 在用户原来的标签基础上删除标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateUserTagNameByDeleteTag(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            string tagName = context.Request["TagName"];
            List<string> tags = tagName.Split(',').Select(p => p.Trim()).ToList();
            string strTag = string.Empty;
            if (tags.Count > 0)
            {
                List<UserInfo> userList = bllUser.GetMultListByKey<UserInfo>("AutoID", autoId);
                for (int i = 0; i < userList.Count; i++)
                {
                    if (string.IsNullOrEmpty(userList[i].TagName)) continue;
                    List<string> arrayList = userList[i].TagName.Split(',').Select(p => p.Trim()).ToList();
                    arrayList = arrayList.Where(p => !tags.Contains(p)).ToList();
                    string nTageName = "";
                    if (arrayList.Count > 0)
                    {
                        nTageName = ZentCloud.Common.MyStringHelper.ListToStr(arrayList, "", ",");
                    }
                    userList[i].TagName = nTageName;
                    //bllBase.Update(userList[i]);
                    bllBase.Update(new UserInfo(), string.Format(" TagName='{0}' ", userList[i].TagName), string.Format(" AutoID = {0} ", userList[i].AutoID));
                    //bllBase.Update(new UserInfo(), string.Format(" TagName='{0}' ", strTag), string.Format(" AutoID in ({0})", ids[i]));
                    //strTag = string.Empty;
                }
            }
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加用户标签管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddMemberTag(HttpContext context)
        {
            string tagName = context.Request["TagName"],
                   tagType = context.Request["TagType"],
                   accessLevel = context.Request["AccessLevel"];

            if (string.IsNullOrEmpty(tagName))
            {
                resp.Status = 0;
                resp.Msg = "请输入标签名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            var modelTag = bllBase.Get<MemberTag>(string.Format("TagName='{0}' AND WebsiteOwner='{1}' And TagType='{2}'", tagName, bllBase.WebsiteOwner, tagType));
            if (modelTag != null)
            {
                resp.Status = 3;
                resp.Msg = "标签不能重复添加";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            MemberTag model = new MemberTag();
            model.TagName = tagName;
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.Creator = this.currWebSiteUserInfo.UserID;
            model.TagType = tagType;
            if (string.IsNullOrEmpty(accessLevel))
            {
                model.AccessLevel = 0;
            }
            else
            {
                model.AccessLevel = Convert.ToInt32(accessLevel);
            }
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑用户标签管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditMemberTag(HttpContext context)
        {


            string tagName = context.Request["TagName"];
            string autoID = context.Request["AutoID"];
            string accessLevel = context.Request["AccessLevel"];

            if (string.IsNullOrEmpty(tagName))
            {
                resp.Status = 0;
                resp.Msg = "请输入标签名称";

                return Common.JSONHelper.ObjectToJson(resp);
            }

            var model = bllBase.Get<MemberTag>(string.Format("AutoId={0}", autoID));
            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "标签不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "无权修改";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.TagName = tagName;
            if (!string.IsNullOrEmpty(accessLevel))
            {
                model.AccessLevel = Convert.ToInt32(accessLevel);
            }
            else
            {
                model.AccessLevel = 0;
            }
            if (bllBase.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }




        #endregion

        #region 关注模块


        private string QueryAttention(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string fromUserID = context.Request["FromUserID"];
            string fromTrueName = context.Request["FromTrueName"];
            string toUserID = context.Request["ToUserID"];
            string toTrueName = context.Request["ToTrueName"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(fromUserID))
            {
                sbWhere.AppendFormat(" And FromUserID={0}", fromUserID);
            }
            if (!string.IsNullOrEmpty(fromTrueName))
            {
                sbWhere.AppendFormat(" And FromTrueName={0}", fromTrueName);
            }
            if (!string.IsNullOrEmpty(toUserID))
            {
                sbWhere.AppendFormat(" And ToUserID={0}", toUserID);
            }
            if (!string.IsNullOrEmpty(toTrueName))
            {
                sbWhere.AppendFormat(" And ToTrueName={0}", toTrueName);
            }


            int totalCount = this.bllBase.GetCount<Attention>(sbWhere.ToString());
            List<Attention> dataList = this.bllBase.GetLit<Attention>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });

        }

        #endregion

        #region 系统通知

        /// <summary>
        /// 系统消息列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSystemNotice(HttpContext context)
        {
            int totalCount;
            List<SystemNotice> dataList;
            string title = context.Request["Title"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat(" AND Title lIKE '%{0}%'", title);
            }
            totalCount = this.bllSystemNotice.GetCount<SystemNotice>(sbWhere.ToString());
            dataList = this.bllSystemNotice.GetLit<SystemNotice>(pageSize, pageIndex, sbWhere.ToString(), "AutoId desc");

            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});

        }

        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelSystemNotice(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            if (bllSystemNotice.Delete(new SystemNotice(), string.Format(" AutoID in ({0})", ids)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败。";
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 增加系统通知
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddSystemNotice(HttpContext context)
        {
            string title = context.Request["Title"];
            string ncontent = context.Request["Ncontent"];
            int sendType = Convert.ToInt32(context.Request["SendType"]);
            int messageType = int.Parse(context.Request["MessageType"]);
            string receiver = context.Request["Receive"];
            string redirectUrl = context.Request["RedirectUrl"];

            if (string.IsNullOrEmpty(title))
            {
                resp.Msg = "标题不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(ncontent))
            {
                resp.Msg = "内容不能为空";
                goto outoff;
            }
            if (sendType.Equals(1) || sendType.Equals(2))
            {
                if (string.IsNullOrEmpty(receiver))
                {

                    resp.Msg = "接收人不能为空";
                    goto outoff;
                }
            }
            if (!string.IsNullOrEmpty(redirectUrl))
            {

                System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                System.Text.RegularExpressions.Match math = regUrl.Match(redirectUrl);
                if (!math.Success)
                {
                    resp.Msg = "链接格式不正确,格式示例 http://www.comeoncloud.com.cn";
                    goto outoff;
                }

            }
            ZentCloud.BLLJIMP.ReturnValue rc = bllSystemNotice.SendSystemMessage(title, ncontent, messageType, sendType, receiver, redirectUrl, bllBase.WebsiteOwner);
            resp.Status = 1;
            resp.Msg = rc.Msg;

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        #endregion

        /// <summary>
        /// 接口配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetWXQiyeConfig(HttpContext context)
        {

            string corpID = context.Request["CorpID"];
            string secret = context.Request["Secret"];
            string appId = context.Request["AppId"];

            if (string.IsNullOrEmpty(corpID) || string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(appId))
            {
                resp.Status = 0;
                resp.Msg = "必填项不完整";
                goto OutOf;
            }

            BLLJIMP.Model.WXQiyeConfig model = bllBase.Get<BLLJIMP.Model.WXQiyeConfig>(string.Format("WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (model == null)
            {
                model = new BLLJIMP.Model.WXQiyeConfig();
                model.CorpID = corpID;
                model.Secret = secret;
                model.AppId = appId;
                model.WebsiteOwner = bllBase.WebsiteOwner;
                if (bllBase.Add(model))
                {
                    resp.Status = 1;
                }


            }
            else
            {
                model.CorpID = corpID;
                model.Secret = secret;
                model.AppId = appId;
                if (bllBase.Update(model))
                {
                    resp.Status = 1;
                }
            }

        OutOf:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        #region 用户等级配置管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryUserLevelConfig(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string type = context.Request["type"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And LevelType='{0}'", type);
            }
            int totalCount = this.bllUser.GetCount<UserLevelConfig>(sbWhere.ToString());
            List<UserLevelConfig> dataList = this.bllUser.GetLit<UserLevelConfig>(pageSize, pageIndex, sbWhere.ToString(), "LevelNumber ASC");
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddUserLevelConfig(HttpContext context)
        {
            UserLevelConfig model = bllBase.ConvertRequestToModel<UserLevelConfig>(new UserLevelConfig());
            //if (bllBase.GetCount<UserLevelConfig>(string.Format("WebSiteOwner='{0}' And LevelNumber={1}", bllBase.WebsiteOwner, model.LevelNumber)) > 0)
            //{
            //    resp.Msg = "已经存在该等级";
            //    goto outoff;
            //}
            //if (model.FromHistoryScore >= model.ToHistoryScore)
            //{
            //    resp.Msg = "积分最小值不能大于最大值";
            //    goto outoff;
            //}
            model.WebSiteOwner = bllBase.WebsiteOwner;
            if (bllJuActivity.Add(model))
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加会员新等级");
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            //outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditUserLevelConfig(HttpContext context)
        {


            int autoID = int.Parse(context.Request["AutoID"]);
            UserLevelConfig reqModel = bllBase.ConvertRequestToModel<UserLevelConfig>(new UserLevelConfig());
            var model = bllUser.Get<UserLevelConfig>(string.Format("AutoId={0}", autoID));
            if (model == null)
            {
                resp.Msg = "等级不存在";
                goto outoff;

            }
            //if (reqModel.FromHistoryScore >= reqModel.ToHistoryScore)
            //{
            //    resp.Msg = "积分最小值不能大于最大值";
            //    goto outoff;
            //}
            model.LevelNumber = reqModel.LevelNumber;
            model.LevelString = reqModel.LevelString;
            model.LevelIcon = reqModel.LevelIcon;
            model.FromHistoryScore = reqModel.FromHistoryScore;
            model.ToHistoryScore = reqModel.ToHistoryScore;
            model.Discount = reqModel.Discount;
            model.LevelType = reqModel.LevelType;
            model.DistributionRateLevel0First = reqModel.DistributionRateLevel0First;
            model.DistributionRateLevel0 = reqModel.DistributionRateLevel0;
            model.DistributionRateLevel1First = reqModel.DistributionRateLevel1First;
            model.RebateMemberRate = reqModel.RebateMemberRate;
            model.DistributionRateLevel1 = reqModel.DistributionRateLevel1;
            model.DistributionRateLevel2 = reqModel.DistributionRateLevel2;
            model.DistributionRateLevel3 = reqModel.DistributionRateLevel3;
            model.ChannelRate = reqModel.ChannelRate;
            model.Description = reqModel.Description;
            model.RebateScoreRate = reqModel.RebateScoreRate;
            model.DistributionRateLevel1Ex1 = reqModel.DistributionRateLevel1Ex1;
            model.AccumulationFundRateLevel1 = reqModel.AccumulationFundRateLevel1;
            model.IsDisable = reqModel.IsDisable;
            model.SupplierRate = reqModel.SupplierRate;
            model.AwardAmount = reqModel.AwardAmount;
            if (bllJuActivity.Update(model))
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "编辑会员新等级[id=" + autoID + "]");
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteUserLevelConfig(HttpContext context)
        {
            string ids = context.Request["ids"];
            UserLevelConfig model;
            foreach (var item in ids.Split(','))
            {
                model = bllUser.Get<UserLevelConfig>(string.Format("AutoId={0}", item));
                if (model == null || (!model.WebSiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllUser.Delete(new UserLevelConfig(), string.Format("AutoId in ({0})", ids));
            bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除会员新等级[id=" + ids + "]");
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion

        #region 积分分类
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryScoreTypeInfos(HttpContext context)
        {


            string typeName = context.Request["TypeName"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(typeName))
            {
                sbWhere.AppendFormat(" AND TypeName = '{0}'", typeName);
            }
            int totalCount = this.bllJuActivity.GetCount<BLLJIMP.Model.WXMallScoreTypeInfo>(sbWhere.ToString());
            List<BLLJIMP.Model.WXMallScoreTypeInfo> dataList = this.bllJuActivity.GetLit<BLLJIMP.Model.WXMallScoreTypeInfo>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteScoreTypeInfos(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请选择一行数据";
                goto OutF;
            }
            int count = bllMall.Delete(new BLLJIMP.Model.WXMallScoreTypeInfo(), " Autoid in (" + ids + ")");
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败";
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 添加更新分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ADScoreTypeInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string typeName = context.Request["TName"];
            string img = context.Request["TypeImg"];
            if (string.IsNullOrEmpty(autoId))
            {
                autoId = "0";
            }
            BLLJIMP.Model.WXMallScoreTypeInfo typeInfo = bllJuActivity.Get<BLLJIMP.Model.WXMallScoreTypeInfo>(string.Format(" AutoId={0}", autoId));
            if (typeInfo != null)
            {
                typeInfo.TypeName = typeName;
                typeInfo.TypeImg = img;
                typeInfo.websiteOwner = bllBase.WebsiteOwner;
                bool IsTrue = bllJuActivity.Update(typeInfo);
                if (IsTrue)
                {
                    resp.Status = 0;
                    resp.Msg = "修改成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "修改失败";
                }
            }
            else
            {
                typeInfo = new BLLJIMP.Model.WXMallScoreTypeInfo()
                {
                    TypeName = typeName,
                    TypeImg = img,
                    websiteOwner = bllBase.WebsiteOwner

                };
                bool IsTrue = bllJuActivity.Add(typeInfo);
                if (IsTrue)
                {
                    resp.Status = 0;
                    resp.Msg = "添加成功。";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败。";
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取分类详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreTypeInfo(HttpContext context)
        {
            string autoId = context.Request["Autoid"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.Status = -1;
                resp.Msg = "系统错误请联系管理员";
                goto OutF;
            }
            BLLJIMP.Model.WXMallScoreTypeInfo TypeInfo = bllJuActivity.Get<BLLJIMP.Model.WXMallScoreTypeInfo>(string.Format(" AutoId={0}", autoId));
            if (TypeInfo != null)
            {
                resp.Msg = "";
                resp.Status = 0;
                resp.ExObj = TypeInfo;
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        #endregion

        /// <summary>
        ///消息详细发送记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QuerySendWxMsgList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            string serialNum = context.Request["serialNum"];

            string status = context.Request["status"];

            //StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}' And BroadcastType = '{1}' ", bllDis.WebsiteOwner, CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify)));
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}' ", bllDis.WebsiteOwner));

            if (!string.IsNullOrWhiteSpace(serialNum))
            {
                sbWhere.AppendFormat(" And SerialNum = '{0}' ", serialNum);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                sbWhere.AppendFormat(" AND Status in ({0})  ", status);
            }


            List<WXBroadcastHistory> dataList = BLLStatic.bll.GetLit<WXBroadcastHistory>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");


            for (int i = 0; i < dataList.Count; i++)
            {
                UserInfo user = null;

                if (!string.IsNullOrWhiteSpace(dataList[i].OpenId))
                {
                    user = bllUser.GetUserInfoByOpenId(dataList[i].OpenId);
                }

                if (!string.IsNullOrWhiteSpace(dataList[i].UserId) && user == null)
                {
                    user = bllUser.GetUserInfo(dataList[i].UserId);
                }

                dataList[i].TrueName = user.TrueName;
                dataList[i].WxNikeName = user.WXNickname;
                dataList[i].Phone = user.Phone;
                if (!string.IsNullOrEmpty(dataList[i].StatusMsg))
                {
                    try
                    {
                        JToken jToken = JToken.Parse(dataList[i].StatusMsg);
                        dataList[i].StatusMsg = bllWeixin.GetCodeMessage(int.Parse(jToken["errcode"].ToString()));

                    }
                    catch (Exception)
                    {


                    }
                }

            }


            int totalCount = BLLStatic.bll.GetCount<WXBroadcastHistory>(sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });
        }

        private string QuerySendWxMsgPlan(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int totalCount = 0;
            List<SMSPlanInfo> dataList = bllSms.QuerySMSPlan(out totalCount, pageIndex, pageSize, bllBase.WebsiteOwner);

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });

        }

        //QuerySendWxMsgPlan


        #region 提现管理

        /// <summary>
        /// 查询提现记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWithrawCash(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}' ", bllDis.WebsiteOwner));
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserId"];
            string status = context.Request["Status"];
            string fromDate = context.Request["FromDate"];
            string toDate = context.Request["ToDate"];
            string withdrawCashType = context.Request["WithdrawCashType"];
            string type = context.Request["Type"];
            if (!string.IsNullOrWhiteSpace(userId))
            {
                sbWhere.AppendFormat(" And (UserId like '%{0}%' Or AccountName like '%{0}%') ", userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status = {0} ", status);
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                sbWhere.AppendFormat(" And InsertDate>= '{0}' ", fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                sbWhere.AppendFormat(" And InsertDate< '{0}' ", Convert.ToDateTime(toDate).AddDays(1));
            }
            if (!string.IsNullOrEmpty(withdrawCashType))
            {
                sbWhere.AppendFormat(" And WithdrawCashType = '{0}' ", withdrawCashType);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And TransfersType = {0} ", type);
            }
            List<WithdrawCash> dataList = bllBase.GetLit<WithdrawCash>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC,Status ASC");
            int totalCount = bllBase.GetCount<WithdrawCash>(sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 更改提现状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWithrawCashStatus(HttpContext context)
        {
            string withdrawCashType = context.Request["WithdrawCashType"];//提现类型                         DistributionOffLine 线下分销
            //DistributionOnLine  线上分销
            string tranIds = context.Request["TranIds"];
            string newTranIds = "";
            foreach (var item in tranIds.Split(','))
            {
                newTranIds += "'" + item + "'" + ",";
            }
            newTranIds = newTranIds.TrimEnd(',');
            int status = int.Parse(context.Request["Status"]);
            List<WithdrawCash> data = bllBase.GetList<WithdrawCash>(string.Format(" AutoID in({0})", tranIds));
            string msg = "";
            switch (withdrawCashType)
            {
                case "DistributionOffLine"://业务分销
                    if (bllDisOffLine.UpdateWithrawCashStatus(data, status, out msg))//
                    {
                        resp.Status = 1;
                    }
                    else
                    {
                        resp.Msg = msg;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    break;
                case "DistributionOnLine":
                    if (bllDis.UpdateWithrawCashStatus(data, status, out msg))//线下分销
                    {
                        resp.Status = 1;
                    }
                    else
                    {
                        resp.Msg = msg;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    break;
                default:
                    break;
            }



            resp.Msg = msg;
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }





        /// <summary>
        /// 设置分销上级
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetDistributionOwner(HttpContext context)
        {
            string autoIds = context.Request["AutoIDS"];
            string distributionOwner = context.Request["DistributionOwner"];
            var distributionOwnerInfo = bllUser.GetUserInfo(distributionOwner);
            if (distributionOwnerInfo == null)
            {
                resp.Msg = "上级用户名不存在，请检查";
                goto outoff;
            }
            if (autoIds.Split(',').Contains(distributionOwnerInfo.AutoID.ToString()))
            {
                resp.Msg = "上级用户名不能与选择的用户名相同，请检查";
                goto outoff;
            }

            int count = bllUser.Update(new UserInfo(), string.Format("DistributionOwner='{0}'", distributionOwner), string.Format("AutoID in({0})", autoIds));
            if (count == autoIds.Split(',').Length)
            {
                resp.Status = 1;
                resp.Msg = "设置成功";
            }
            else
            {
                resp.Msg = "更新失败";
            }


        outoff:
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        #endregion


        #region 群发图文管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWXMassArticle(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string kewWord = context.Request["KeyWord"];

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));

            if (!string.IsNullOrEmpty(kewWord))
            {
                sbWhere.AppendFormat(" And (Title like '%{0}%' or Digest like '%{0}%' or Author like '%{0}%')", kewWord);
            }
            int totalCount = bllBase.GetCount<WXMassArticle>(sbWhere.ToString());
            List<WXMassArticle> dataList = bllBase.GetLit<WXMassArticle>(pageSize, pageIndex, sbWhere.ToString(), " Sort ASC,AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});


        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWXMassArticle(HttpContext context)
        {
            WXMassArticle model = bllBase.ConvertRequestToModel<WXMassArticle>(new WXMassArticle());
            model.WebsiteOwner = bllBase.WebsiteOwner;
            model.Content = Common.StringHelper.GetReplaceStr(model.Content);
            model.Title = Common.StringHelper.GetReplaceStr(model.Title);
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWXMassArticle(HttpContext context)
        {
            WXMassArticle model = bllBase.ConvertRequestToModel<WXMassArticle>(new WXMassArticle());
            model.WebsiteOwner = bllBase.WebsiteOwner;
            //model.Content = Common.StringHelper.GetReplaceStr(model.Content);
            //model.Title = Common.StringHelper.GetReplaceStr(model.Title);
            if (bllBase.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWXMassArticle(HttpContext context)
        {
            string ids = context.Request["ids"];
            WXMassArticle model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<WXMassArticle>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    resp.Msg = "无权删除";
                    goto outoff;

                }

            }
            int count = bllJuActivity.Delete(new WXMassArticle(), string.Format("AutoID in ({0})", ids));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了 {0} 条数据", count);
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        #endregion

        /// <summary>
        /// 更新图文素材排序号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWXMassArticleSortIndex(HttpContext context)
        {
            string autoID = context.Request["AutoID"];
            string sortIndex = context.Request["SortIndex"];
            int count = this.bllJuActivity.Update(new WXMassArticle(), string.Format("Sort={0}", sortIndex), string.Format(" AutoID ={0}", autoID));
            if (count.Equals(1))
            {
                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        #region 幻灯片管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QuerySlide(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            int totalCount = bllBase.GetCount<Slide>(sbWhere.ToString());
            List<Slide> dataList = bllBase.GetLit<Slide>(pageSize, pageIndex, sbWhere.ToString(), " Type ASC,Sort DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});

        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddSlide(HttpContext context)
        {
            Slide model = bllBase.ConvertRequestToModel<Slide>(new Slide());
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditSlide(HttpContext context)
        {

            Slide model = bllBase.ConvertRequestToModel<Slide>(new Slide());
            if (bllBase.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteSlide(HttpContext context)
        {
            string ids = context.Request["ids"];
            Slide model;
            foreach (var item in ids.Split(','))
            {
                model = bllJuActivity.Get<Slide>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    resp.Msg = "无权删除";
                    goto outoff;

                }

            }
            int count = bllJuActivity.Delete(new Slide(), string.Format("AutoID in ({0})", ids));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了 {0} 条数据", count);
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        #endregion


        #region 商城导航管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryNavigation(HttpContext context)
        {

            List<Navigation> showList = new List<Navigation>();
            List<Navigation> listTop = new List<Navigation>(); ;
            List<Navigation> listLeft = new List<Navigation>(); ;
            List<Navigation> listBottom = new List<Navigation>();
            string navigationType = context.Request["NavigationType"];
            if (!string.IsNullOrEmpty(navigationType))
            {
                switch (navigationType)
                {
                    case "top":
                        listTop = bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='top' Order by Sort DESC,AutoID ASC", bllBase.WebsiteOwner));
                        break;
                    case "left":
                        listLeft = bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='left' Order by Sort DESC,AutoID ASC", bllBase.WebsiteOwner));
                        break;
                    case "bottom":
                        listBottom = bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='bottom' Order by Sort DESC,AutoID ASC", bllBase.WebsiteOwner));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                listTop = bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='top' Order by Sort DESC,AutoID ASC", bllBase.WebsiteOwner));
                listLeft = bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='left' Order by Sort DESC,AutoID ASC", bllBase.WebsiteOwner));
                listBottom = bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='bottom' Order by Sort DESC,AutoID ASC", bllBase.WebsiteOwner));


            }


            MySpider.MyCategories m = new MySpider.MyCategories();
            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<Navigation>("AutoID", "ParentId", "NavigationName", listTop), 0))
            {
                try
                {
                    Navigation tmpModel = listTop.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0];
                    tmpModel.NavigationName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }

            MySpider.MyCategories mLeft = new MySpider.MyCategories();

            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<Navigation>("AutoID", "ParentId", "NavigationName", listLeft), 0))
            {
                try
                {
                    Navigation tmpModel = listLeft.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0];
                    tmpModel.NavigationName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }


            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<Navigation>("AutoID", "ParentId", "NavigationName", listBottom), 0))
            {
                try
                {
                    Navigation tmpModel = listBottom.Where(p => p.AutoID.ToString().Equals(item.Value)).ToList()[0];
                    tmpModel.NavigationName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }


            int totalCount = showList.Count;

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = showList
    });




        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddNavigation(HttpContext context)
        {
            Navigation model = bllBase.ConvertRequestToModel<Navigation>(new Navigation());
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditNavigation(HttpContext context)
        {

            Navigation model = bllBase.ConvertRequestToModel<Navigation>(new Navigation());
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllBase.Update(model))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteNavigation(HttpContext context)
        {
            string ids = context.Request["ids"];
            Navigation model;
            foreach (var item in ids.Split(','))
            {
                model = bllMall.Get<Navigation>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    resp.Msg = "无权删除";
                    goto outoff;

                }

            }
            int count = bllMall.Delete(new Navigation(), string.Format("AutoID in ({0})", ids));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了 {0} 条数据", count);
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GeNavigationTree(HttpContext context)
        {
            string navigationLinkType = context.Request["NavigationLinkType"];
            //int value=int.Parse(context.Request["SelectValue"]);
            string result = string.Empty;
            result = new MySpider.MyCategories().GetSelectOptionHtml(bllMall.GetList<Navigation>(string.Format("WebsiteOwner='{0}' And NavigationLinkType='{1}'", bllBase.WebsiteOwner, navigationLinkType)), "AutoID", "ParentId", "NavigationName", 0, "ddlPreNavigation", "width:200px", "");
            return result.ToString();


        }
        #endregion

        #region 关键字过滤管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryFilterWord(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));

            string keyWord = context.Request["Word"];
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And Word like '%{0}%'", keyWord);
            }
            int totalCount = bllBase.GetCount<FilterWord>(sbWhere.ToString());
            List<FilterWord> dataList = bllBase.GetLit<FilterWord>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddFilterWord(HttpContext context)
        {
            FilterWord model = bllBase.ConvertRequestToModel<FilterWord>(new FilterWord());
            model.WebsiteOwner = bllBase.WebsiteOwner;
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditFilterWord(HttpContext context)
        {

            FilterWord Model = bllBase.ConvertRequestToModel<FilterWord>(new FilterWord());
            if (bllBase.Update(Model))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteFilterWord(HttpContext context)
        {
            string ids = context.Request["ids"];
            FilterWord model;
            foreach (var item in ids.Split(','))
            {
                model = bllBase.Get<FilterWord>(string.Format("AutoID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    resp.Msg = "无权删除";
                    goto outoff;

                }

            }
            int count = bllJuActivity.Delete(new FilterWord(), string.Format("AutoID in ({0})", ids));
            resp.Status = 1;
            resp.Msg = string.Format("成功删除了 {0} 条数据", count);
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        #endregion

        /// <summary>
        /// 设置用户为经销商
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetAgent(HttpContext context)
        {
            resp.Status = bllMall.SetAgent(int.Parse(context.Request["AutoId"])) == true ? 1 : 0;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 设置用户为渠道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetChannel(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (bllUser.Update(new UserInfo(), string.Format(" IsChannel=1"), string.Format("AutoId in({0})", ids)) > 0)
            {
                resp.Status = 1;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 查询优惠券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCoupon(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int totalCount = 0;
            var dataList = bllMall.GetCouponList(pageIndex, pageSize, out totalCount, context.Request["UserId"], context.Request["CouponNumber"]);
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 生成优惠券 系统生成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCoupon(HttpContext context)
        {
            float discount = float.Parse(context.Request["Discount"]);
            string productId = context.Request["ProductId"];
            string startDate = context.Request["StartDate"];
            string stopDate = context.Request["StopDate"];
            if (bllMall.AddCoupon(discount, productId, startDate, stopDate))
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询优惠券V2
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCouponV2(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string cardCouponType = context.Request["cardCouponType"];
            int totalCount = 0;
            var dataList = bllCardCoupon.GetCardCouponList(cardCouponType, pageIndex, pageSize, out totalCount, "");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 生成优惠券V2 系统生成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCouponV2(HttpContext context)
        {

            string cardCouponType = context.Request["CardCouponType"];//卡券类型
            string cardCouponName = context.Request["CardCouponName"];//卡券名称
            string discount = context.Request["Discount"];//折扣
            string productId = context.Request["ProductId"];//商品编号
            string validFrom = context.Request["ValidFrom"];//有效期开始
            string validTo = context.Request["ValidTo"];//有效期结束
            string deductibleAmount = context.Request["DeductibleAmount"];//可抵扣金额
            string freeFreightAmount = context.Request["FreeFreightAmount"];//满多少元包邮
            string buckleAmount = context.Request["BuckleAmount"];//满扣券 满多少元
            string buckleSubAmount = context.Request["BuckleSubAmount"];//满扣券 洪多少元减多少元
            CardCoupons model = new CardCoupons();
            model.CardId = int.Parse(bllUser.GetGUID(TransacType.AddCardCoupon));
            model.Name = cardCouponName;
            model.CardCouponType = cardCouponType;
            model.Ex1 = discount;
            model.Ex2 = productId;
            model.Ex3 = deductibleAmount;
            model.Ex4 = freeFreightAmount;
            model.Ex5 = buckleAmount;
            model.Ex6 = buckleSubAmount;
            model.InsertDate = DateTime.Now;
            model.WebSiteOwner = bllCardCoupon.WebsiteOwner;
            model.CreateUserId = currentUserInfo.UserID;
            if (!string.IsNullOrEmpty(validFrom))
            {
                model.ValidFrom = DateTime.Parse(validFrom);
            }
            if (!string.IsNullOrEmpty(validTo))
            {
                model.ValidTo = DateTime.Parse(validTo);
            }

            if (bllCardCoupon.Add(model))
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 发放优惠券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SendCouponV2(HttpContext context)
        {

            string cardCouponId = context.Request["CardCouponId"];
            string sendType = context.Request["SendType"];
            string userId = context.Request["UserId"];
            UserInfo userInfo = bllUser.GetUserInfo(userId, bllBase.WebsiteOwner);

            if (userInfo == null)
            {
                userInfo = bllUser.GetUserInfoByPhone(userId);
            }
            if (userInfo == null)
            {
                resp.Msg = "用户不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(int.Parse(cardCouponId));
            switch (sendType)
            {
                case "0"://个人
                    MyCardCoupons model = new MyCardCoupons();
                    model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllMall.GetGUID(BLLJIMP.TransacType.CommAdd));
                    model.CardCouponType = cardCoupon.CardCouponType;
                    model.CardId = cardCoupon.CardId;
                    model.InsertDate = DateTime.Now;
                    model.UserId = userInfo.UserID;
                    model.WebSiteOwner = bllCardCoupon.WebsiteOwner;
                    if (bllCardCoupon.Add(model))
                    {
                        resp.Status = 1;

                    }
                    break;
                default:
                    break;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteCouponV2(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (bllMall.Delete(new CardCoupons(), string.Format("CardId in({0})", ids)) > 0)
            {
                bllMall.Delete(new MyCardCoupons(), string.Format("CardId in({0})", ids));
                resp.Status = 1;

            }


            return Common.JSONHelper.ObjectToJson(resp);
        }


        #region 回收站
        /// <summary>
        /// 查询被删除的活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityByDelete(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserID"];
            string isToJubitActivity = context.Request["IsToJubitActivity"];
            string activityName = context.Request["ActivityName"];
            string isSignUpJubit = context.Request["IsSignUpJubit"];
            string articleType = context.Request["ArticleType"];
            string recommendCate = context.Request["RecommendCate"];
            string articleTypeEx1 = context.Request["ArticleTypeEx1"];
            string categoryId = context.Request["CategoryId"];
            int isShowHide = Convert.ToInt32(context.Request["isShowHide"]);
            if (categoryId == "0")
            {
                categoryId = "";
            }
            else if (!string.IsNullOrEmpty(categoryId))
            {
                categoryId = new BLLArticleCategory().GetCateAndChildIds(int.Parse(categoryId));
            }

            int totalCount = 0;
            List<JuActivityInfo> dataList = this.bllJuActivity.QueryJuActivityData(
                null, out totalCount, null, null, recommendCate, null, activityName, pageIndex, pageSize,
                "",
                null, articleType, bllBase.WebsiteOwner, articleTypeEx1, categoryId, "", "", "", "", "", false, "", false, true, false, "1");
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].ActivityDescription = null;
                if (dataList[i].ActivityEndDate != null)
                {
                    if (DateTime.Now >= (DateTime)dataList[i].ActivityEndDate)
                    {
                        dataList[i].IsHide = 1;
                    }
                }

            }

            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }




        /// <summary>
        /// 恢复活动的数据
        /// </summary>
        /// <returns></returns>
        private string RecoverJuActivity(HttpContext context)
        {
            string juActivityIDs = context.Request["ids"];
            int count = this.bllJuActivity.Update(new JuActivityInfo(), string.Format("isDelete={0}", 0), string.Format(" JuActivityID  in ({0})", juActivityIDs));
            if (count > 0)
            {
                resp.Status = 1;
                resp.Msg = "还原成功";
            }
            else
            {
                resp.Status = 1;
                resp.Msg = "还原失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 查询被删除的商品
        /// </summary>
        /// <returns></returns>
        private string QueryWXMallProductInfoByDelete(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["PName"];
            string categoryId = context.Request["CategoryId"];
            string isOnSale = context.Request["IsOnSale"];

            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'  And IsDelete=1", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And PName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(categoryId) && (!categoryId.Equals("0")))
            {
                sbWhere.AppendFormat(" And CategoryId = '{0}'", categoryId);
            }
            if (!string.IsNullOrEmpty(isOnSale))
            {
                sbWhere.AppendFormat(" And IsOnSale = '{0}'", isOnSale);
            }

            int totalCount = bllJuActivity.GetCount<WXMallProductInfo>(sbWhere.ToString());
            List<WXMallProductInfo> dataList = new List<WXMallProductInfo>();
            dataList = bllJuActivity.GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].PDescription = null;

            }
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });
        }

        /// <summary>
        /// 还原商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RestoreWXMallProductInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            int count = this.bllMall.Update(new WXMallProductInfo(), string.Format("isDelete={0}", 0), string.Format(" PID  in ({0})", ids));
            if (count > 0)
            {
                resp.Status = 1;
                resp.Msg = "还原成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "还原失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        #region 定时任务

        /// <summary>
        /// 获取定时任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTimingTasks(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            string taskType = context.Request["task_type"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}'", bllBase.WebsiteOwner);
            if (!string.IsNullOrEmpty(taskType))
            {
                sbWhere.AppendFormat(" And TaskType={0}", taskType);
            }
            List<ZentCloud.BLLJIMP.Model.TimingTask> list = bllWeixin.GetLit<ZentCloud.BLLJIMP.Model.TimingTask>(pageSize, pageIndex, sbWhere.ToString(), "AutoId DESC");

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = list.Count,
                rows = list
            });
        }

        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelTimingTasks(HttpContext context)
        {
            string ids = context.Request["ids"];
            int result = this.bllJuActivity.Delete(new TimingTask(), string.Format("AutoID in ({0})", ids));
            resp.Status = 0;
            resp.Msg = string.Format("成功删除了{0}条数据", result);
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        ///添加积分统计任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddScoreStatisticsTask(HttpContext context)
        {

            string startTime = context.Request["start_time"];
            string endTime = context.Request["end_time"];
            string userId = context.Request["user_id"];
            TimingTask task = new TimingTask();
            task.WebsiteOwner = bllUser.WebsiteOwner;
            task.InsertDate = DateTime.Now;
            task.Status = 1;
            task.TaskInfo = "积分统计任务";
            task.TaskType = 15;
            task.ScheduleDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startTime))
            {
                task.FromDate = DateTime.Parse(startTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                task.ToDate = DateTime.Parse(endTime);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                task.DistributionUserId = userId;
                task.Title = bllUser.GetUserDispalyName(userId);

            }
            if (bllDis.Add(task))
            {
                resp.IsSuccess = true;
            }
            else
            {
                resp.Msg = "操作失败";
            }
            return JsonConvert.SerializeObject(resp);

        }

        #endregion

        /// <summary>
        /// 获取分销二维码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDistributionWxQrcodeLimitUrl(HttpContext context)
        {
            string result = string.Empty;
            string userId = context.Request["user_id"];
            string type = context.Request["type"];
            string ishecheng = context.Request["ishecheng"];
            if (!string.IsNullOrEmpty(ishecheng))
            {
                CompanyWebsite_Config config = bllWebsite.GetCompanyWebsiteConfig();
                if (!string.IsNullOrEmpty(config.DistributionQRCodeIcon))
                {
                    result = bllWeixin.GetQRCodeImg(bllDis.GetDistributionWxQrcodeLimitUrl(userId), config.DistributionQRCodeIcon);

                    if (!result.ToLower().StartsWith("http"))
                    {
                        result = bllJuActivity.DownLoadImageToOss(context.Server.MapPath(result), bllJuActivity.WebsiteOwner, true);
                    }
                    //bllJuActivity.ToLog("GetDistributionWxQrcodeLimitUrl1:" + result, "D:\\log\\GetDistributionWxQrcodeLimitUrl.txt");
                    return result;
                }

            }
            switch (type)
            {
                case "channel":
                    result = bllDis.GetDistributionWxQrcodeLimitUrl(userId, "channel");
                    break;
                default:
                    result = bllDis.GetDistributionWxQrcodeLimitUrl(userId);
                    break;
            }

            if (!result.ToLower().StartsWith("http"))
            {
                result = bllJuActivity.DownLoadImageToOss(context.Server.MapPath(result), bllJuActivity.WebsiteOwner, true);
            }
            //bllJuActivity.ToLog("GetDistributionWxQrcodeLimitUrl2:" + result, "D:\\log\\GetDistributionWxQrcodeLimitUrl.txt");
            return result;
        }

        /// <summary>
        /// 设置员工
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetEmployee(HttpContext context)
        {
            resp.IsSuccess = true;

            //memberlevel = 2, 上级是系统
            var ids = context.Request["ids"];

            var memberlevelResult = bllUser.Update(new UserInfo(), string.Format(" MemberLevel = 2 "), string.Format(" AutoID IN ({0}) AND MemberLevel = 0 ", ids));

            var disownerResult = bllUser.Update(new UserInfo(), string.Format(" DistributionOwner = '{0}' ", bllUser.WebsiteOwner), " MemberLevel = 2 AND ( DistributionOwner IS NULL  OR DistributionOwner = '' ) ");

            resp.IsSuccess = memberlevelResult > 0;

            resp.Result = memberlevelResult;

            if (resp.IsSuccess)
            {
                //记录操作日志
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "设置员工[id=" + ids + "]");
            }

            return JsonConvert.SerializeObject(resp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 问卷反序列化模型
        /// <summary>
        /// 问卷调查反序列化模型
        /// </summary>
        private class QuestionnaireModel
        {
            /// <summary>
            /// 问卷编号
            /// </summary>
            public int QuestionnaireID { get; set; }
            /// <summary>
            /// 问卷名称
            /// </summary>
            public string QuestionnaireName { get; set; }
            /// <summary>
            /// 问卷介绍及说明
            /// </summary>
            public string QuestionnaireContent { get; set; }
            /// <summary>
            /// 问卷停止日期
            /// </summary>
            public DateTime QuestionnaireStopDate { get; set; }
            /// <summary>
            /// 问卷是否可见 0不可见 1可见
            /// </summary>
            public int QuestionnaireVisible { get; set; }
            /// <summary>
            /// 问卷图片
            /// </summary>
            public string QuestionnaireImage { get; set; }
            /// <summary>
            /// 问卷描述用于分享时显示
            /// </summary>
            public string QuestionnaireSummary { get; set; }
            /// <summary>
            /// 问题列表
            /// </summary>
            public List<QuestionModel> QuestionList { get; set; }
            /// <summary>
            /// 赠送积分
            /// </summary>
            public int AddScore { get; set; }
            /// <summary>
            /// 分类 0题库 1问卷
            /// </summary>
            public int QuestionnaireType { get; set; }
            /// <summary>
            /// 每页题目数
            /// </summary>
            public int EachPageNum { get; set; }
            /// <summary>
            /// 是否高级授权
            /// </summary>
            public int IsWeiXinLicensing { get; set; }
            /// <summary>
            /// 按钮文字
            /// </summary>
            public string ButtonText { get; set; }
            /// <summary>
            /// 按钮链接
            /// </summary>
            public string ButtonLink { get; set; }
            /// <summary>
            /// 问卷提交后跳转
            /// </summary>
            public string QuestionnaireSubmitUrl { get; set; }
            /// <summary>
            /// 问卷重复提交跳转
            /// </summary>
            public string QuestionnaireRepeatSubmitUrl { get; set; }
            /// <summary>
            /// 考试时长 分钟
            /// </summary>
            public string ExamMinute { get; set; }

        }
        /// <summary>
        /// 问卷调查-问题反序列化模型
        /// </summary>
        private class QuestionModel
        {
            /// <summary>
            /// 问题ID
            /// </summary>
            public int QuestionID { get; set; }
            /// <summary>
            /// 问题名称
            /// </summary>
            public string QuestionName { get; set; }
            /// <summary>
            /// 问题类型 0单选1多选2填空
            /// </summary>
            public int QuestionType { get; set; }
            /// <summary>
            /// 是否必填 0否1必填
            /// </summary>
            public int IsRequired { get; set; }
            /// <summary>
            /// 分组名称
            /// </summary>
            public string AnswerGroupName { get; set; }
            /// <summary>
            /// 选项
            /// </summary>
            public List<AnswerModel> Answer { get; set; }

        }

        /// <summary>
        /// 问卷调查-选项反序列化模型
        /// </summary>
        public class AnswerModel
        {
            /// <summary>
            /// 选项ID
            /// </summary>
            public int AnswerID { get; set; }
            /// <summary>
            /// 选项名称
            /// </summary>
            public string AnswerName { get; set; }
            /// <summary>
            /// 是否正确答案
            /// </summary>
            public int IsCorrect { get; set; }

        }
        #endregion

        /// <summary>
        /// 文章活动分类模型
        /// </summary>
        public class CategoryIDModel : ZCBLLEngine.ModelTable
        {
            public int AutoID { get; set; }

        }


        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="log"></param>
        private void ToLog(string log)
        {
            try
            {

                //return;

                //if (currentUrl.ToLower().IndexOf("fuqijiaoyu.comeoncloud.net") > -1)
                //{

                //    using (StreamWriter sw = new StreamWriter(@"D:\WXOpenOAuthDevLog.txt", true, Encoding.GetEncoding("gb2312")))
                //    {
                //        sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                //    }

                //    return;
                //}


                using (StreamWriter sw = new StreamWriter(@"D:\CationHandlerLog.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                }


            }
            catch { }
        }


    }
}
