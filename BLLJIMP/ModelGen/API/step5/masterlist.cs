using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 导师模型
    /// </summary>
    public class MasterModel:MasterBase
    {
        ///// <summary>
        ///// 导师头像
        ///// </summary>
        //public string headimg { get; set; }
        ///// <summary>
        ///// 导师用户名
        ///// </summary>
        //public string userid { get; set; }
        ///// <summary>
        ///// 导师姓名
        ///// </summary>
        //public string truename { get; set; }
        ///// <summary>
        ///// 导师职位
        ///// </summary>
        //public string postion { get; set; }
        ///// <summary>
        ///// 导师简要介绍
        ///// </summary>
        //public string digest { get; set; }
        ///// <summary>
        ///// 浏览量
        ///// </summary>
        //public int pv { get; set; }
        ///// <summary>
        ///// 赞数量
        ///// </summary>
        //public int praisecount { get; set; }
        /// <summary>
        /// 是否已经关注 true已关注 false 未关注
        /// </summary>
        public bool isattention { get; set; }
        /// <summary>
        /// 是否可以关注或咨询该导师
        /// </summary>
        public bool canaskorattention { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int fanscount { get; set; }
        /// <summary>
        /// 话题数量
        /// </summary>
        public int questioncount { get; set; }
        /// <summary>
        /// 导师咨询的数量
        /// </summary>
        public int askcount { get; set; }
    }
    /// <summary>
    /// 导师api模型
    /// </summary>
    public class MasteListApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public List<MasterModel> list { get; set; }
    }


}
