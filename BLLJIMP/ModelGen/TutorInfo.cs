using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 导师 理财师表
    /// </summary>
    public class TutorInfo : ZCBLLEngine.ModelTable
    {

        public TutorInfo() { }
        private int _wznums;
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 导师名称
        /// </summary>
        public string TutorName { get; set; }

        /// <summary>
        /// 导师图片
        /// </summary>
        public string TutorImg { get; set; }

        /// <summary>
        /// 导师说明
        /// </summary>
        public string TutorExplain { get; set; }

        /// <summary>
        /// 导师回答数
        /// </summary>
        public int TutorAnswers { get; set; }
        /// <summary>
        /// 导师问题数
        /// </summary>
        public int TutorQuestions { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? RDataTime { get; set; }

        /// <summary>
        ///行业
        /// </summary>
        public string TradeStr { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string ProfessionalStr { get; set; }

        /// <summary>
        /// 喜欢数
        /// </summary>
        public int TutorLikes { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string websiteOwner { get; set; }

        /// <summary>
        /// 文章数
        /// </summary>
        public int WZNums
        {
            get
            {
                int result = 0;
                try
                {
                    BLL bll = new BLL();
                    result = bll.GetCount<JuActivityInfo>(string.Format(" UserId ='{0}' And IsDelete=0 And IsHide=0  And WebsiteOwner='{1}'", UserId, bll.WebsiteOwner));

                }
                catch { return 0; }
                return result;
            }
            set { _wznums = value; }

            
        }

        /// <summary>
        /// 话题数
        /// </summary>
        public int HTNums { get; set; }

        /// <summary>
        /// 最新回复
        /// </summary>
        public DateTime? ReviewDateTime { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        public List<BLLJIMP.Model.ArticleCategory> actegory { get; set; }
        /// <summary>
        /// 企业微信号成员唯一标识,不支持中文
        /// </summary>
        public string WxQiyeUserId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 导师粉丝数
        /// </summary>
        public int FlowerCount
        {

            get
            {

                try
                {
                    BLLUser bll = new BLLUser("");
                    return bll.GetCount<UserFollowChain>(string.Format("ToUserId='{0}'",UserId));

                }
                catch (Exception)
                {
                    return 0;

                }

            }

        }
        /// <summary>
        /// 简要介绍
        /// </summary>
        public string Digest { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int Pv { get; set; }
        /// <summary>
        /// 第几届理财师
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public string CompanyType { get; set; }
        /// <summary>
        /// 工作年限
        /// </summary>
        public int WorkYear { get; set; }
        /// <summary>
        /// 理财师工作年限
        /// </summary>
        public int MasterWorkYear { get; set; }
        /// <summary>
        /// 最高学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 第几名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 省份Key
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 城市Key
        /// </summary>
        public string CityCode { get; set; }
    }
}
