using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 投票对象
    /// </summary>
    public class VoteObjectInfo : ZCBLLEngine.ModelTable
    {
       public int AutoID { get; set; }
       /// <summary>
       /// 所属投票 关联VoteInfo AutoID
       /// </summary>
       public int VoteID { get; set; }
       /// <summary>
       /// 投票对象编号
       /// </summary>
       public string Number { get; set; }
       /// <summary>
       /// 投票对象 名字
       /// </summary>
       public string VoteObjectName { get; set; }
        /// <summary>
        /// 投票对象年龄
        /// </summary>
       public string VoteObjectAge { get; set; }
       /// <summary>
       /// 性别
       /// </summary>
       public string VoteObjectGender { get; set; }
       /// <summary>
       /// 投票对象 头像
       /// </summary>
       public string VoteObjectHeadImage { get; set; }

       /// <summary>
       /// 赛区
       /// </summary>
       public string Area { get; set; }
       /// <summary>
       /// 身高
       /// </summary>
       public string Height { get; set; }
       /// <summary>
       /// 星座
       /// </summary>
       public string Constellation { get; set; }
       /// <summary>
       /// 爱好
       /// </summary>
       public string Hobbies { get; set; }
       /// <summary>
       /// 参赛宣员 口号
       /// </summary>
       public string Introduction { get; set; }

        /// <summary>
        ///详情（视频地址）
        /// </summary>
       public string IntroductionDetail { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
       public string Age { get; set; }
        /// <summary>
        /// 展示图片1
        /// </summary>
       public string ShowImage1 { get; set; }
        /// <summary>
        /// 展示图片2
        /// </summary>
       public string ShowImage2 { get; set; }
        /// <summary>
        /// 展示图上3
        /// </summary>
       public string ShowImage3 { get; set; }
        /// <summary>
        /// 展示图片4
        /// </summary>
       public string ShowImage4 { get; set; }
        /// <summary>
        /// 展示图片5
        /// </summary>
       public string ShowImage5 { get; set; }
       /// <summary>
       /// 所得票数
       /// </summary>
       public int VoteCount { get; set; }
        /// <summary>
        /// 学校名称 (校服-学校全称)
        /// </summary>
       public string SchoolName { get; set; }
        /// <summary>
        /// 详细页底部内容
        /// </summary>
        public string BottomContent { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 审核状态 0审核中 1审核通过 2审核不通过
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 分享id
        /// </summary>
        public string ComeonShareId { get; set; }

        /// <summary>
        /// 备注信息 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// 扩展字段6
        /// </summary>
        public string Ex6 { get;set; }

        /// <summary>
        /// 扩展字段7
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        /// 扩展字段8
        /// </summary>
        public string Ex8 { get; set; }
        /// <summary>
        /// 扩展字段9
        /// </summary>
        public string Ex9 { get; set; }
        /// <summary>
        /// 扩展字段10
        /// </summary>
        public string Ex10 { get; set; }
        
        /// <summary>
        /// 其他资料链接
        /// </summary>
        public string OtherInfoLink { get; set; }

    }
}
