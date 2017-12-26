using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class Live_MatchInfo : ZCBLLEngine.ModelTable
    {
        public Live_MatchInfo()
        { }
        #region Model
        private long _autoid;
        private string _matchid;
        private string _league;
        private string _round;
        private string _startdatestr;
        private string _matchstatus;
        private string _hometeam;
        private string _awayteam;
        private string _score;
        private string _halfscore;
        private DateTime? _insertdate;
        /// <summary>
        /// 
        /// </summary>
        public long AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 比分ID
        /// </summary>
        public string MatchID
        {
            set { _matchid = value; }
            get { return _matchid; }
        }
        /// <summary>
        /// 赛事
        /// </summary>
        public string League
        {
            set { _league = value; }
            get { return _league; }
        }
        /// <summary>
        /// 轮次
        /// </summary>
        public string Round
        {
            set { _round = value; }
            get { return _round; }
        }
        /// <summary>
        /// 比赛开始时间
        /// </summary>
        public string StartDateStr
        {
            set { _startdatestr = value; }
            get { return _startdatestr; }
        }
        /// <summary>
        /// 比赛状态
        /// </summary>
        public string MatchStatus
        {
            set { _matchstatus = value; }
            get { return _matchstatus; }
        }
        /// <summary>
        /// 主队
        /// </summary>
        public string HomeTeam
        {
            set { _hometeam = value; }
            get { return _hometeam; }
        }
        /// <summary>
        /// 客队
        /// </summary>
        public string AwayTeam
        {
            set { _awayteam = value; }
            get { return _awayteam; }
        }
        /// <summary>
        /// 比分
        /// </summary>
        public string Score
        {
            set { _score = value; }
            get { return _score; }
        }
        /// <summary>
        /// 半场比分
        /// </summary>
        public string HalfScore
        {
            set { _halfscore = value; }
            get { return _halfscore; }
        }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        #endregion Model
    }
}
