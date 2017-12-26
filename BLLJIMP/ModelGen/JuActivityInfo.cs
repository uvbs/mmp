using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 活动文章表
    /// </summary>
    [Serializable]
    public partial class JuActivityInfo : ZCBLLEngine.ModelTable
    {
        BLL bll = new BLL();
        /// <summary>
        /// 构造函数
        /// </summary>
        public JuActivityInfo()
        { }

        #region Model
        /// <summary>
        /// 活动ID
        /// </summary>
        private int _juactivityid;
        /// <summary>
        /// 用户ID
        /// </summary>
        private string _userid;
        /// <summary>
        /// 活动名称
        /// </summary>
        private string _activityname;
        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime? _activitystartdate;
        /// <summary>
        /// 结否时间
        /// </summary>
        private DateTime? _activityenddate;
        /// <summary>
        /// 地址
        /// </summary>
        private string _activityaddress;
        /// <summary>
        /// 
        /// </summary>
        private string _activitywebsite;
        /// <summary>
        /// 内容
        /// </summary>
        private string _activitydescription;
        /// <summary>
        /// 缩略图
        /// </summary>
        private string _thumbnailspath;
        /// <summary>
        /// 
        /// </summary>
        private int _issignupjubit = 0;
        /// <summary>
        /// 
        /// </summary>
        private string _signupactivityid;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        private string _recommendcate;
        /// <summary>
        /// 
        /// </summary>
        private int? _ishide = 0;
        /// <summary>
        /// 
        /// </summary>
        private int? _sort = null;
        /// <summary>
        /// 活动ID
        /// </summary>
        public int JuActivityID
        {
            set { _juactivityid = value; }
            get { return _juactivityid; }
        }
        /// <summary>
        /// 创建账号
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 活动名称 文章标题
        /// </summary>
        public string ActivityName
        {
            set { _activityname = value; }
            get
            {
                if (!string.IsNullOrWhiteSpace(_activityname))
                {
                    _activityname = _activityname.Replace("'", "‘").Replace("\"", "“").Replace("\n", " ");
                }
                return _activityname;

            }
        }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime? ActivityStartDate
        {
            set { _activitystartdate = value; }
            get { return _activitystartdate; }
        }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? ActivityEndDate
        {
            set { _activityenddate = value; }
            get {

                if (_activityenddate!=null)
                {
                    if (_activityenddate.Equals(new DateTime(1970, 1, 1)))
                    {
                        return null;
                    }
                    
                }
                
                return _activityenddate; 
            
            
            }
        }
        /// <summary>
        /// 活动地址
        /// </summary>
        public string ActivityAddress
        {
            set { _activityaddress = value; }
            get { return _activityaddress; }
        }
        /// <summary>
        /// 活动网址
        /// </summary>
        public string ActivityWebsite
        {
            set { _activitywebsite = value; }
            get { return _activitywebsite; }
        }
        /// <summary>
        /// 活动说明
        /// </summary>
        public string ActivityDescription
        {
            set { _activitydescription = value; }
            get { return _activitydescription; }
        }
        /// <summary>
        /// 缩略图路径
        /// </summary>
        public string ThumbnailsPath
        {
            set { _thumbnailspath = value; }
            get { return _thumbnailspath; }
        }
        /// <summary>
        /// 是否报名到平台 无用
        /// 0否
        /// 1自动报名
        /// 2自定义报名
        /// </summary>
        public int IsSignUpJubit
        {
            set { _issignupjubit = value; }
            get { return _issignupjubit; }
        }
        /// <summary>
        /// 报名活动ID
        /// </summary>
        public string SignUpActivityID
        {
            set { _signupactivityid = value; }
            get { return _signupactivityid; }
        }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 推荐分类
        /// </summary>
        public string RecommendCate
        {
            set { _recommendcate = value; }
            get { return _recommendcate; }
        }
        /// <summary>
        /// 是否隐藏
        /// 0 显示
        /// 1 进行中 
        /// -1待开始
        /// </summary>
        public int? IsHide
        {
            set { _ishide = value; }
            get { return _ishide; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort
        {
            set { _sort = value; }
            get { return _sort; }
        }
        /// <summary>
        /// 是否收费 
        /// 0免费 
        /// 1收费
        /// </summary>
        public int IsFee { get; set; }
        /// <summary>
        /// 置顶图片
        /// </summary>
        public string TopImgPath { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
        public int SignUpCount { get; set; }
        /// <summary>
        /// 报名通过数
        /// </summary>
        public int LimitSignUpPassCount { get; set; }
        /// <summary>
        /// 打开人数
        /// </summary>
        public int OpenCount { get; set; }
        /// <summary>
        /// 赞人数
        /// </summary>
        public int UpCount { get; set; }
        /// <summary>
        /// 是否内容来自网页地址 无用
        /// </summary>
        public int IsByWebsiteContent { get; set; }
        /// <summary>
        /// 检测任务ID
        /// </summary>
        public int MonitorPlanID { get; set; }
        /// <summary>
        /// 是否推荐到聚比特活动：未推荐、审核中、审核通过、已驳回
        /// </summary>
        public string IsToJubitActivity { get; set; }

        /// <summary>
        /// 删除标识:1为已删0未删
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 文章类型：
        /// 普通文章article、
        /// 活动文章activity 
        /// 贺卡 greetingcard
        /// </summary>
        public string ArticleType { get; set; }

        /// <summary>
        /// 文章模板：
        /// 1微信官方模板
        /// 2聚比特模板 
        /// 0空模板 
        /// 3 活动模板(有微信高级认证) 
        /// 4活动模板(无微信高级认证)
        /// </summary>
        public int ArticleTemplate { get; set; }
        /// <summary>
        /// 无用
        /// </summary>
        private int _isSpread = 1;
        /// <summary>
        /// 是否进行影响力转发 无用
        /// </summary>
        public int IsSpread
        {
            get { return _isSpread; }
            set { _isSpread = value; }
        }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 讲师
        /// </summary>
        public string ActivityLecturer { get; set; }
        /// <summary>
        /// 文章扩展
        /// </summary>
        public string ArticleTypeEx1 { get; set; }
        /// <summary>
        /// 是否显示在作者的其它发布中
        /// </summary>
        public string IsHideRecommend { get; set; }

        /// <summary>
        /// 报名活动 提交报名后通知的 客服ID 关联表ZCJ_WXKeFu
        /// </summary>
        public string ActivityNoticeKeFuId { get; set; }

        /// <summary>
        /// 分类ID 关联表ZCJ_ArticleCategory
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// PV
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// 微信阅读人数
        /// </summary>
        public int UV { get; set; }      
        /// <summary>
        /// 分享数
        /// </summary>
        public int ShareTotalCount { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        private string _summary;
        /// <summary>
        ///微信分享时显示的描述
        /// </summary>
        public string Summary
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_summary))
                {
                    _summary = _summary.Replace("'", "‘").Replace("\"", "“").Replace("\n", " ");
                }
                return _summary;
            }
            set
            {
                _summary = value;
            }
        }
        /// <summary>
        /// 是否显示报名人数列表 0不显示 1显示
        /// </summary>
        public int IsShowPersonnelList { get; set; }
        /// <summary>
        /// 显示报名人数方式(IsShowPersonnelList 为1的时候有效)  0或空显示姓名全名 1只显示姓,名用*代替
        /// </summary>
        public int ShowPersonnelListType { get; set; }
        /// <summary>
        ///访问权限级别，默认是0（未注册用户），注册用户为>=1，值越大访问权限越大，与文章，活动的权限对应。
        ///比如：权限为3的用户可以访问权限为0,1,2,3的文章
        ///值为0的文章，活动表示公开给所有人访问
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// 最大报名人数 空或0表示无上限
        /// </summary>
        public int MaxSignUpTotalCount { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int PreId { get; set; }

        /// <summary>
        /// 根据类型走的状态 0待确认 2已完成 -1已违约
        /// </summary>
        public int TStatus { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 省份代码
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市代码
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 地区代码
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string District { get; set; }
        /// 创建ip
        /// </summary>
        public string CreateIP { get; set; }
        /// <summary>
        /// 是否隐藏用户名
        /// </summary>
        public int IsHideUserName { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 评论+评论的回复数
        /// </summary>
        public int CommentAndReplayCount { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseCount { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public int FavoriteCount { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K4 { get; set; }
        /// <summary>
        /// 类型为 Outlets 门店时存储的 供应商的Id
        /// </summary>
        public string K5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K7 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K8 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K9 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K11 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K12 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K13 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string K14 { get; set; }
        public string K15 { get; set; }
        public string K16 { get; set; }
        public string K17 { get; set; }
        public string K18 { get; set; }
        public string K19 { get; set; }
        public string K20 { get; set; }
        public string K21 { get; set; }
        public string K22 { get; set; }
        public string K23 { get; set; }
        public string K24 { get; set; }
        public string K25 { get; set; }
        public string K26 { get; set; }
        public string K27 { get; set; }
        public string K28 { get; set; }
        public string K29 { get; set; }
        public string K30 { get; set; }   
             
        /// <summary>
        /// 
        /// </summary>
        public int IsSys { get; set; }
        /// <summary>
        /// 根分类ID
        /// </summary>
        public string RootCateId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal CreditAcount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal GuaranteeCreditAcount { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string UserLongitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string UserLatitude { get; set; }
        /// <summary>
        ///重定向URL
        /// </summary>
        private string _redirect_url;
        /// <summary>
        /// 重定向URL
        /// </summary>
        public string RedirectUrl
        {
            get { return ZentCloud.Common.StringHelper.ReplaceLinkRD(_redirect_url); }
            set { _redirect_url = value; }
        }
        /// <summary>
        /// 报名成功后跳转的
        /// </summary>
        private string _activity_signup_url;
        /// <summary>
        /// 报名成功后跳转的url
        /// </summary>
        public string ActivitySignuptUrl
        {
            get { return ZentCloud.Common.StringHelper.ReplaceLinkRD(_activity_signup_url); }
            set { _activity_signup_url = value; }
        }
        /// <summary>
        /// 是否有文章评论 0默认(读全局配置) 1有评论 2无评论
        /// </summary>
        public int HaveComment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServerTimeMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServicesMsg { get; set; }
        /// <summary>
        /// 活动积分
        /// </summary>
        public int ActivityIntegral { get; set; }
        /// <summary>
        /// 相关阅读
        /// </summary>
        public string RelationArticles { get; set; }
        /// <summary>
        /// 相关商品
        /// </summary>
        public string RelationProducts { get; set; }
        /// <summary>
        /// 活动电话
        /// </summary>
        public string ActivityPhone { get; set; }

        /// <summary>
        /// 可见区域
        /// </summary>
        public int VisibleArea { get; set; }

        /// <summary>
        /// 显示条件
        /// </summary>
        public int ShowCondition { get; set; }

        /// <summary>
        /// 显示内容
        /// </summary>
        public string VisibleContext { get; set; }
        /// <summary>
        /// 打赏总数
        /// </summary>
        public double RewardTotal { get; set; }


        #region tab 扩展字段
        /// <summary>
        /// tab扩展字段1
        /// </summary>
        public string TabExTitle1 { get; set; }

        /// <summary>
        /// tab扩展内容1
        /// </summary>
        public string TabExContent1 { get; set; }

        /// <summary>
        /// tab扩展字段2
        /// </summary>
        public string TabExTitle2 { get; set; }

        /// <summary>
        /// tab扩展内容2
        /// </summary>
        public string TabExContent2 { get; set; }

        /// <summary>
        /// tab扩展字段3
        /// </summary>
        public string TabExTitle3 { get; set; }

        /// <summary>
        /// tab扩展内容3
        /// </summary>
        public string TabExContent3 { get; set; }

        /// <summary>
        /// tab扩展字段4
        /// </summary>
        public string TabExTitle4 { get; set; }

        /// <summary>
        /// tab扩展内容4
        /// </summary>
        public string TabExContent4 { get; set; }

        /// <summary>
        /// tab扩展字段5
        /// </summary>
        public string TabExTitle5 { get; set; }

        /// <summary>
        /// tab扩展内容5
        /// </summary>
        public string TabExContent5 { get; set; }

        #endregion


        #endregion Model

        ///// <summary>
        ///// 自动编号
        ///// </summary>
        //public int AutoId { get; set; }
        ///// <summary>
        ///// 活动ID
        ///// </summary>
        //public string ActivityId { get; set; }
        ///// <summary>
        ///// 名称
        ///// </summary>
        //public string ActivityName { get; set; }
        ///// <summary>
        ///// 图片
        ///// </summary>
        //public string ActivityImg { get; set; }
        ///// <summary>
        ///// 概要
        ///// </summary>
        //public string Summary { get; set; }
        ///// <summary>
        /////类型
        /////activity 活动
        /////match 比赛
        /////train 培训
        ///// </summary>
        //public string ActivityType { get; set; }

        ///// <summary>
        ///// 创建日期
        ///// </summary>
        //public DateTime? InsertDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        ///// <summary>
        ///// 是否需要支付
        ///// 0 需要
        ///// 1 不需要
        ///// </summary>
        //public int IsNeedPay { get; set; }
        ///// <summary>
        ///// 地址
        ///// </summary>
        //public string Address { get; set; }
        ///// <summary>
        ///// 说明
        ///// </summary>
        //public string Description { get; set; }
        ///// <summary>
        ///// 站点所有者
        ///// </summary>
        //public string Websiteowner { get; set; }
        ///// <summary>
        ///// 是否删除
        ///// </summary>
        //public int IsDelete { get; set; }
        /// <summary>
        /// 是否已经发布
        /// </summary>
        public int IsPublish { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Remark { get; set; }
        ///// <summary>
        ///// 最多报名人数
        ///// </summary>
        //public int MaxSignUpCount { get; set; }
        /// <summary>
        /// 要点
        /// </summary>
        public string MainPoints { get; set; }

        #region 扩展

        //public int IP
        //{
        //    get
        //    {
        //        int result = 0;
        //        try
        //        {
        //            BLLMonitor bll = new BLLMonitor();
        //            result = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID = {0} ", MonitorPlanID));

        //        }
        //        catch { }
        //        return result;
        //    }
        //}

        //public int PV
        //{
        //    get
        //    {
        //        int result = 0;
        //        try
        //        {
        //            BLLMonitor bll = new BLLMonitor();
        //            result = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID = {0} ", MonitorPlanID));

        //        }
        //        catch { }
        //        return result;

        //    }
        //}


        ///// <summary>
        ///// 分享总数
        ///// </summary>
        //public int ShareTotalCount {
        //    get {
        //        int result = 0;
        //        try
        //        {
        //            BLLMonitor bll = new BLLMonitor();
        //            result = bll.GetCount<MonitorEventDetailsInfo>("ShareTimestamp", string.Format(" MonitorPlanID = {0} and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ", MonitorPlanID));
        //        }
        //        catch { }
        //        return result;
        //    }
        //}

        /// <summary>
        /// 报名总人数
        /// </summary>
        public int SignUpTotalCount
        {
            get
            {
                int result = 0;
                try
                {
                   
                    result = bll.GetCount<ActivityDataInfo>(string.Format(" ActivityID = {0} and IsDelete = 0 ", SignUpActivityID));

                }
                catch { }
                return result;
            }
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName
        {
            get
            {
                string result = "";
                try
                {
                    if(!string.IsNullOrWhiteSpace(CategoryId))
                    {
                        
                        result = bll.Get<ArticleCategory>(string.Format(" AutoID ={0}", CategoryId)).CategoryName;
                    }
                }
                catch { }
                return result;
            }
        }
        /// <summary>
        /// 活动ID16进制
        /// </summary>
        public string JuActivityIDHex
        {
            get
            {
                return Convert.ToString(JuActivityID, 16);
            }
        }
        /// <summary>
        /// 导师姓名
        /// </summary>
        public string TutorName
        {
            get
            {
                //var tutorInfo = bll.Get<TutorInfo>(string.Format(" UserId ='{0}' And websiteOwner='{1}'", _userid, WebsiteOwner));
                //if (tutorInfo != null)
                //{
                //    return tutorInfo.TutorName;
                //}
                return "";
            }

        }
        /// <summary>
        /// 子内容数
        /// </summary>
        public int SubCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CurrUserIsPraise { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CurrUserIsFavorite { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CurrUserIsFollow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserInfo ReplayToUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserInfo PubUser { get; set; }
        /// <summary>
        /// 活动状态： 0代表进行中 1代表已结束 2代表已满员
        /// </summary>
        public int ActivityStatus 
        {
            get 
            {
                int result = 0;

                try
                {
                    if (IsHide == 1)
                    {
                        result = 1;
                    }
                    if (ActivityEndDate!=null)
                    {
                        if ((DateTime)(ActivityEndDate)<DateTime.Now)
                        {
                             result = 1;
                        }
                    }
                    if ((MaxSignUpTotalCount > 0) && (SignUpTotalCount >= MaxSignUpTotalCount))
                    {
                        result = 2;
                    }
                }
                catch { }

                return result;
            }
        }  
        
        public int ShareMonitorId
        {
            get; set;
        }

        #endregion ModelEX

    }
}
