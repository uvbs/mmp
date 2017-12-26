using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 首页 滚动信息模型
    /// </summary>
    public class IndexMsg
    {

        /// <summary>
        /// 信息类型 0发表文章 1发表活动 2发表话题 3回复话题
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 信息编号 可根据信息编号跳转到对应的详情页 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string imgurl { get; set; }
        /// <summary>
        /// 资讯标题
        /// </summary>
        public string newstitle { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string activityname { get; set; }
        /// <summary>
        /// 活动时间
        /// </summary>
        public double activitytime { get; set; }
        /// <summary>
        /// 活动地点
        /// </summary>
        public string activityaddress { get; set; }
        /// <summary>
        /// 话题标题
        /// </summary>
        public string asktitle { get; set; }
        /// <summary>
        /// 话题内容
        /// </summary>
        public string askcontent { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string digest { get; set; }


    }
    /// <summary>
    /// 首页信息 api模型
    /// </summary>
    public class IndexMsgApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 信息集合
        /// </summary>
        public List<IndexMsg> list { get; set; }
    }



}
