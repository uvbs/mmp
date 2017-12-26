using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    public class BLLTutor : BLL
    {
        public BLLTutor()
            : base()
        {

        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <returns></returns>
        public List<Model.TutorInfo> GetTutorsList(int pageIndex, int pageSize, string province, string city, string keyword, string sort,out int total)
        {
            List<Model.TutorInfo> result = new List<Model.TutorInfo>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner = '{0}'", WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(province)) sbSql.AppendFormat(" AND ProvinceCode = '{0}'", province);
            if (!string.IsNullOrWhiteSpace(city)) sbSql.AppendFormat(" AND ProvinceCode = '{0}'", city);
            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" AND TutorName Like '%{0}%'", keyword);

            string orderBy = " TutorAnswers Desc";//默认排序
            if (!string.IsNullOrWhiteSpace(sort))
            {
                if (sort.Equals("wzNum"))
                    orderBy = "WZNums Desc";
                else if (sort.Equals("new"))
                    orderBy = "RDataTime Desc";
            }

            total = GetCount<Model.TutorInfo>(sbSql.ToString());
            result = GetLit<Model.TutorInfo>(pageSize, pageIndex, sbSql.ToString(), orderBy);//
            return result;
        }

        /// <summary>
        /// 获取专家数
        /// </summary>
        /// <returns></returns>
        public int GetTutorsCount(string province, string city, string keyword)
        {
            List<Model.TutorInfo> result = new List<Model.TutorInfo>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner = '{0}'", WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(province)) sbSql.AppendFormat(" AND ProvinceCode = '{0}'", province);
            if (!string.IsNullOrWhiteSpace(city)) sbSql.AppendFormat(" AND ProvinceCode = '{0}'", city);
            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" AND TutorName Like '%{0}%'", keyword);

            return GetCount<Model.TutorInfo>(sbSql.ToString());
        }
        public Model.TutorInfo  GetTutorInfo(string userId)
        {
            return Get<Model.TutorInfo>(string.Format(" UserId = '{0}'", userId));
        }


        public bool UpdateTutorInfoByUserInfo(Model.UserInfo user)
        {
            Model.TutorInfo tutor = GetTutorInfo(user.UserID);
            BLLUserExpand bllUserExpand = new BLLUserExpand();
            BLLUser bllUser = new BLLUser();
            if (tutor == null)
            {
                tutor = new Model.TutorInfo();
                tutor.UserId = user.UserID;
                tutor.Email = user.Email;
                tutor.TutorImg = user.WXHeadimgurl;
                tutor.TutorName = user.TrueName;
                tutor.Gender = bllUser.GetSex(user);
                tutor.ProvinceCode = user.ProvinceCode;
                tutor.CityCode = user.CityCode;
                tutor.City = user.City;
                tutor.Digest = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, user.UserID);
                tutor.Company = user.Company;
                tutor.Position = user.Postion;
                tutor.WZNums = GetCount<Model.JuActivityInfo>(string.Format(" UserId ='{0}' And IsDelete=0 And IsHide=0  And WebsiteOwner='{1}'", user.UserID, WebsiteOwner));
                var reviewTypeKey = CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.Answer);
                tutor.TutorAnswers = GetCount<Model.ReviewInfo>(string.Format(" UserId ='{0}' And ReviewType='{1}' AND WebsiteOwner='{2}'", user.UserID, reviewTypeKey, WebsiteOwner));
                tutor.RDataTime = DateTime.Now;
                tutor.websiteOwner = WebsiteOwner;
                return Add(tutor);
            }
            else
            {
                tutor.TutorImg = user.WXHeadimgurl;
                tutor.TutorName = user.TrueName;
                tutor.Gender = bllUser.GetSex(user);
                tutor.ProvinceCode = user.ProvinceCode;
                tutor.CityCode = user.CityCode;
                tutor.City = user.City;
                tutor.Digest = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, user.UserID);
                tutor.Company = user.Company;
                tutor.Position = user.Postion;
                return Update(tutor);
            }
        }
        /// <summary>
        /// 更新文章数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateWZNums(string userId)
        {
            var tutor = GetTutorInfo(userId);
            if (tutor != null)
            {
                tutor.WZNums = GetCount<Model.JuActivityInfo>(string.Format(" UserId ='{0}' And IsDelete=0 And IsHide=0  And WebsiteOwner='{1}'", userId, WebsiteOwner));
                return Update(tutor);
            }
            return false;
        }


        /// <summary>
        /// 更新回答数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateAnswers(string userId)
        {
            var tutor = GetTutorInfo(userId);
            if (tutor != null)
            {
                var reviewTypeKey = CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.Answer);
                tutor.TutorAnswers = GetCount<Model.ReviewInfo>(string.Format(" UserId ='{0}' And ReviewType='{1}' AND WebsiteOwner='{2}'", userId, reviewTypeKey, WebsiteOwner));
                return Update(tutor);
            }
            return false;
        }


        /// <summary>
        /// 关系是否已存在
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public bool ExistTutor(string userId)
        {
            Model.TutorInfo tutor = GetTutorInfo(userId);
            return tutor == null ?false : true;
        }

        public bool DelTutor(string userId)
        {
            Model.TutorInfo tutor = GetTutorInfo(userId);
            return Delete(tutor)>0;
        }

    }
}
