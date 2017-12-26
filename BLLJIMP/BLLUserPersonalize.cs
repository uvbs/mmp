using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 用户个性化信息
    /// </summary>
    public class BLLUserPersonalize : BLL
    {
        public BLLUserPersonalize(string userID)
            : base(userID)
        {

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Model.UserPersonalizeDataInfo model, out string msg)
        {
            try
            {
                //空值检查
                if (string.IsNullOrWhiteSpace(model.UserID) || string.IsNullOrWhiteSpace(model.Val1) || model.PersonalizeType < 1)
                {
                    msg = "关键字段不能为空!";
                    return false;
                }

                //格式检查
                if (model.PersonalizeType == 2 && !Common.ValidatorHelper.EmailLogicJudge(model.Val1))
                {
                    msg = "邮件地址格式不正确!";
                    return false;
                }

                //判断重复
                if (Exists(model, new List<string>() { "UserID", "PersonalizeType", "Val1" }))
                {
                    msg = "已存在!";
                    return false;
                }

                //创建UID
                model.PersonalizeID = int.Parse(GetGUID(TransacType.UserPersonalizeDataAdd));

                if (Add(model))
                {
                    msg = "添加成功!";
                    return true;
                }

                msg = "添加失败!";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public bool Update(Model.UserPersonalizeDataInfo model, out string msg)
        {
            try
            {
                //空值检查
                if (string.IsNullOrWhiteSpace(model.UserID) || string.IsNullOrWhiteSpace(model.Val1) || model.PersonalizeType < 1)
                {
                    msg = "关键字段不能为空!";
                    return false;
                }

                //格式检查
                if (model.PersonalizeType == 2 && !Common.ValidatorHelper.EmailLogicJudge(model.Val1))
                {
                    msg = "邮件地址格式不正确!";
                    return false;
                }

                //判断重复
                if (Exists(model, new List<string>() { "UserID", "PersonalizeType", "Val1" }))
                {
                    msg = "已存在!";
                    return false;
                }


                if (Update(model))
                {
                    msg = "编辑成功!";
                    return true;
                }

                msg = "编辑失败!";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public Model.UserPersonalizeDataInfo Get(int pid)
        {
            return Get<Model.UserPersonalizeDataInfo>(string.Format(" PersonalizeID = {0} ", pid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Model.UserPersonalizeDataInfo> QueryUserPList(string userID)
        {
            return GetList<Model.UserPersonalizeDataInfo>(string.Format(" UserID = '{0}' ", userID));
        }
    }
}
