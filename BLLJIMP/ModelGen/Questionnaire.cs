using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 调查问卷表
    /// </summary>
    public partial class Questionnaire : ZCBLLEngine.ModelTable
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
        /// 问卷介绍说明
        /// </summary>
        public string QuestionnaireContent { get; set; }
        /// <summary>
        /// 问卷结束日期
        /// </summary>
        public DateTime? QuestionnaireStopDate { get; set; }
        /// <summary>
        /// 问卷显示或隐藏 0代表隐藏 1代表显示
        /// </summary>
        public int QuestionnaireVisible { get; set; }
        /// <summary>
        /// 问卷图片
        /// </summary>
        public string QuestionnaireImage { get; set; }
        /// <summary>
        /// 问卷 分享描述
        /// </summary>
        public string QuestionnaireSummary { get; set; }
        /// <summary>
        /// 网站拥有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 问卷入库日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public int AddScore { get; set; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 分类 0题库 1问卷
        /// </summary>
        public int QuestionnaireType { get; set; }
        /// <summary>
        /// 是否高级授权 0否 1是
        /// </summary>
        public int IsWeiXinLicensing { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// 微信阅读人数
        /// </summary>
        public int UV { get; set; }
        /// <summary>
        /// 每页题目数
        /// </summary>
        public int EachPageNum { get; set; }

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
        /// 提交份数
        /// </summary>
        public int SubmitCount { get; set; }
        /// <summary>
        /// 考试时长 分钟
        /// </summary>
        public int ExamMinute { get; set; }
       
    }
}
