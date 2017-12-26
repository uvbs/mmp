using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 积分记录
    /// </summary>
    [Serializable]
    public partial class UserScoreDetailsInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public UserScoreDetailsInfo()
        { }
       
        /// <summary>
        /// 自动编号
        /// </summary>
        private int? _autoid;
        /// <summary>
        /// 账号
        /// </summary>
        private string _userid;
        /// <summary>
        /// 积分变动值
        /// </summary>
        private double _score;
        /// <summary>
        /// 时间
        /// </summary>
        private DateTime _addtime;
        /// <summary>
        /// 说明
        /// </summary>
        private string _addnote;
        /// <summary>
        /// 积分类型 
        /// </summary>
        private string _scoretype;
        /// <summary>
        /// 总积分
        /// </summary>
        private double _totalscore;
        /// <summary>
        /// 自动编号
        /// </summary>
        public int? AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        ///账号
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 积分变动值
        /// 佣金变动值
        /// </summary>
        public double Score
        {
            set { _score = value; }
            get { return _score; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string AddNote
        {
            set { _addnote = value; }
            get { return _addnote; }
        }
        /// <summary>
        /// 积分类型
        /// 空表示积分
        /// AccountAmount表示余额
        /// TotalAmount表示可提现余额
        /// </summary>
        public string ScoreType
        {
            set { _scoretype = value; }
            get { return _scoretype; }
        }
        
        /// <summary>
        /// 当前总积分
        /// </summary>
        public double TotalScore
        {
            set { _totalscore = value; }
            get { return _totalscore; }
        }
        public string RelationID { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 积分事件
        /// 
        /// </summary>
        public string ScoreEvent { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 相关ID （淘股权 相关用户AutoID）
        /// 会员升级 原等级数字
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 原佣金
        /// 撤单记录会员，等级金额
        /// </summary>

        public double EventScore { get; set; }
        /// <summary>
        /// 扣除佣金，
        /// 
        /// 补充说明：公积金，
        /// 注意在 ScoreEvent = 申请提现，提现退款 下是标识扣税的意思
        /// </summary>
        public double DeductScore { get; set; }
        
        /// <summary>
        /// 关联用户ID
        /// </summary>
        public string RelationUserID { get; set; }

        /// <summary>
        /// 会员升级 原等级名称
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        /// 会员升级 新等级数字
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 会员升级 新等级名称
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// 分佣时记录返佣 级别
        /// </summary>
        public string Ex5 { get; set; }

        /// <summary>
        /// 获得 扣除 日期
        /// </summary>
        public string Ex6 { get; set; }
        /// <summary>
        /// 数据导入时源数据库记录ID
        /// </summary>
        public long FromId { get; set; }
        /// <summary>
        /// 是否有打印
        /// 0无,1有
        /// </summary>
        public int IsPrint { get; set; }
    }
}
