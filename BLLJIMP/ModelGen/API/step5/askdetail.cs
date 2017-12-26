using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    ///回复模型
    /// </summary>
    public class Reply
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 回复人头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 回复人的姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 回复人是否是理财师
        /// </summary>
        public bool ismaster { get; set; }

        /// <summary>
        /// 当前用户名
        /// </summary>
        public string userid { get; set; }


    }
    /// <summary>
    /// 提问信息
    /// </summary>
    public class AskInfo{
    /// <summary>
    /// 头像
    /// </summary>
    public string headimg{get;set;}
    /// <summary>
    /// 提问的人的姓名
    /// </summary>
    public string truename{get;set;}
    /// <summary>
    /// 是否是理财师
    /// </summary>
    public bool ismaster{get;set;}
    /// <summary>
    /// 编号
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string title { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string content { get; set; }
    /// <summary>
    /// 时间
    /// </summary>
    public double time { get; set; }
    /// <summary>
    /// 浏览量
    /// </summary>
    public int pv { get; set; }
    /// <summary>
    /// 转发数量
    /// </summary>
    public int sharecount { get; set; }
    /// <summary>
    /// 是否已经赞过了
    /// </summary>
    public bool ispraise { get; set; }
    /// <summary>
    /// 赞数量
    /// </summary>
    public int praisecount { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string userid { get; set; }
    
    }

    public class ReplyList
    {
        /// <summary>
        /// 总回复数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 回复列表集合
        /// </summary>
        public List<Reply> list { get; set; }

    }



}
