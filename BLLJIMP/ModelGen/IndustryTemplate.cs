using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 无用
    /// </summary>
    public class IndustryTemplate : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string IndustryTemplateName { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebsiteName { get; set; }
        /// <summary>
        /// 网站描述
        /// </summary>
        public string WebsiteDescription { get; set; }
        /// <summary>
        /// 网站Logo
        /// </summary>
        public string WebsiteLogo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 课程管理菜单别名
        /// </summary>
        public string CourseManageMenuRName { get; set; }
        /// <summary>
        /// 文章管理别名
        /// </summary>
        public string ArticleManageMenuRName { get; set; }
        /// <summary>
        /// 专家管理别名
        /// </summary>
        public string MasterManageMenuRName { get; set; }
        /// <summary>
        /// 问答管理别名
        /// </summary>
        public string QuestionManageMenuRName { get; set; }
        /// <summary>
        /// 用户管理别名
        /// </summary>
        public string UserManageMenuRName { get; set; }
        /// <summary>
        /// 报名管理别名
        /// </summary>
        public string SignUpCourseMenuRName { get; set; }
        /// <summary>
        /// 活动管理别名
        /// </summary>
        public string ActivityManageMenuRName { get; set; }
        /// <summary>
        /// 商城别名
        /// </summary>
        public string MallMenuRName { get; set; }
        /// <summary>
        /// 网站统计别名
        /// </summary>
        public string WebSiteStatisticsMenuRName { get; set; }

        /// <summary>
        /// 课程分类1
        /// </summary>
        public string CourseCate1 { get; set; }
        /// <summary>
        /// 课程分类2
        /// </summary>
        public string CourseCate2 { get; set; }
        /// <summary>
        /// 文章分类1
        /// </summary>
        public string ArticleCate1 { get; set; }
        /// <summary>
        /// 文章分类2
        /// </summary>
        public string ArticleCate2 { get; set; }
        /// <summary>
        /// 文章分类3
        /// </summary>
        public string ArticleCate3{ get; set; }
        /// <summary>
        /// 文章分类4
        /// </summary>
        public string ArticleCate4 { get; set; }
        /// <summary>
        /// 文章分类5
        /// </summary>
        public string ArticleCate5 { get; set; }
        /// <summary>
        /// 加V 菜单 别名
        /// </summary>
        public string AddVMenuRName { get; set; }
        /// <summary>
        /// 企业核名菜单
        /// </summary>
        public string CompanyNuclearMenuRName { get; set; }





    }
}
