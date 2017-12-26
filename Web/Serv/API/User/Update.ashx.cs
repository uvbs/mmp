using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginNoAction
    {
        BLLUser bll = new BLLUser();
        BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (!string.IsNullOrEmpty(requestModel.ver_code))
            {
                if (requestModel.ver_code!=context.Session["CheckCode"].ToString().ToLower())
                {

                    resp.errcode = -1;
                    resp.errmsg = "验证码错误";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                
            }

            UserInfo userInfo = bll.GetCurrentUserInfo();
            StringBuilder sbPar = new StringBuilder();
            StringBuilder sbRemark = new StringBuilder("修改会员信息：");

            if (string.IsNullOrWhiteSpace(requestModel.action))
            {
                //默认信息处理方式
                if (!string.IsNullOrEmpty(requestModel.truename))
                {
                    sbPar.AppendFormat(" TrueName='{0}',", requestModel.truename);
                    sbRemark.AppendFormat(" 姓名[{0}-{1}]", userInfo.TrueName, requestModel.truename);
                }
                if (!string.IsNullOrEmpty(requestModel.company))
                {
                    sbPar.AppendFormat(" Company='{0}',", requestModel.company);
                    sbRemark.AppendFormat(" 公司[{0}-{1}]", userInfo.Company, requestModel.company);
                }
                if (!string.IsNullOrEmpty(requestModel.postion))
                {
                    sbPar.AppendFormat(" Postion='{0}',", requestModel.postion);
                    sbRemark.AppendFormat(" 职位[{0}-{1}]", userInfo.Postion, requestModel.postion);
                }
                if (!string.IsNullOrEmpty(requestModel.phone) && requestModel.phone != userInfo.Phone)
                {
                    #region 手机变更进行检查
                    if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(requestModel.phone))
                    {
                        resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                        resp.errmsg = "手机号码格式出错";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (requestModel.check_code == 1)
                    {
                        #region 判断验证码是否正确
                        if (bll.GetUserInfoByPhone(requestModel.phone) != null)
                        {
                            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                            resp.errmsg = "手机已存在";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                        if (string.IsNullOrEmpty(requestModel.code))
                        {
                            resp.errmsg = "请填写验证码";
                            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                        BLLSMS bllSms = new BLLSMS("");
                        SmsVerificationCode sms = bllSms.GetLastSmsVerificationCode(requestModel.phone);
                        if (sms.VerificationCode != requestModel.code)
                        {
                            resp.errmsg = "验证码错误";
                            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                        #endregion
                    }
                    #endregion
                    sbPar.AppendFormat(" Phone='{0}',", requestModel.phone);
                    sbRemark.AppendFormat(" 手机号[{0}-{1}]", userInfo.Phone, requestModel.phone);
                }
                if (!string.IsNullOrEmpty(requestModel.email))
                {
                    sbPar.AppendFormat(" Email='{0}',", requestModel.email);
                    sbRemark.AppendFormat(" 邮箱[{0}-{1}]", userInfo.Email, requestModel.email);
                }
                if (!string.IsNullOrEmpty(requestModel.ex1))
                {
                    sbPar.AppendFormat(" Ex1='{0}',", requestModel.ex1);
                }
                if (!string.IsNullOrEmpty(requestModel.ex2))
                {
                    sbPar.AppendFormat(" Ex2='{0}',", requestModel.ex2);
                }
                if (!string.IsNullOrEmpty(requestModel.district))
                {
                    sbPar.AppendFormat(" District='{0}',", requestModel.district);
                }
                if (!string.IsNullOrEmpty(requestModel.nickname))
                {
                    sbPar.AppendFormat(" WXNickname='{0}',", requestModel.nickname);
                    sbRemark.AppendFormat(" 昵称[{0}-{1}]", userInfo.WXNickname, requestModel.nickname);
                }
                if (!string.IsNullOrEmpty(requestModel.gender))
                {
                    sbPar.AppendFormat(" Gender='{0}',", requestModel.gender);
                    string oG = "";
                    if (userInfo.Gender == "1") oG = "男";
                    else if (userInfo.Gender == "0") oG = "女";
                    string nG = "";
                    if (requestModel.gender == "1") nG = "男";
                    else if (requestModel.gender == "0") nG = "女";
                    sbRemark.AppendFormat(" 性别[{0}-{1}]", oG, nG);
                }
                if (!string.IsNullOrEmpty(requestModel.birthday.ToString()))
                {
                    sbPar.AppendFormat(" BirthDay='{0}',", bll.GetTime(requestModel.birthday).ToString());
                }
                if (!string.IsNullOrEmpty(requestModel.identification))
                {
                    sbPar.AppendFormat(" Ex5='{0}',", requestModel.identification);
                }
                if (requestModel.describe != null)
                {
                    sbPar.AppendFormat(" Description='{0}',", ZentCloud.Common.StringHelper.GetReplaceStr(requestModel.describe));
                }
                if (!string.IsNullOrEmpty(requestModel.province))
                {
                    sbPar.AppendFormat(" Province='{0}',", requestModel.province);
                }
                if (!string.IsNullOrEmpty(requestModel.city))
                {
                    sbPar.AppendFormat(" City='{0}',", requestModel.city);
                }
                if (requestModel.salary >= 0)
                {
                    sbPar.AppendFormat(" Salary={0},", requestModel.salary);
                }
                if (!string.IsNullOrEmpty(requestModel.ex3))
                {
                    sbPar.AppendFormat(" Ex3='{0}',", requestModel.ex3);
                }
                if (!string.IsNullOrEmpty(requestModel.ex4))
                {
                    sbPar.AppendFormat(" Ex4='{0}',", requestModel.ex4);
                }
                if (!string.IsNullOrEmpty(requestModel.avatar))
                {
                    sbPar.AppendFormat(" Avatar='{0}',", requestModel.avatar);
                    sbRemark.AppendFormat(" 头像[{0}-{1}]", userInfo.Avatar, requestModel.avatar);
                }
                if (!string.IsNullOrEmpty(requestModel.identity_card_photo_front))
                {
                    sbPar.AppendFormat(" IdentityCardPhotoFront='{0}',", requestModel.identity_card_photo_front);
                    sbRemark.AppendFormat(" 身份证正面[{0}-{1}]", userInfo.IdentityCardPhotoFront, requestModel.identity_card_photo_front);
                }
                if (!string.IsNullOrEmpty(requestModel.identity_card_photo_behind))
                {
                    sbPar.AppendFormat(" IdentityCardPhotoBehind='{0}',", requestModel.identity_card_photo_behind);
                    sbRemark.AppendFormat(" 身份证反面[{0}-{1}]", userInfo.IdentityCardPhotoBehind, requestModel.identity_card_photo_behind);
                }
                if (!string.IsNullOrEmpty(requestModel.identity_card_photo_handheld))
                {
                    sbPar.AppendFormat(" IdentityCardPhotoHandheld='{0}',", requestModel.identity_card_photo_handheld);
                    sbRemark.AppendFormat(" 身份证手持照[{0}-{1}]", userInfo.IdentityCardPhotoHandheld, requestModel.identity_card_photo_handheld);
                }
                if (!string.IsNullOrEmpty(requestModel.intelligence_certificate_business))
                {
                    sbPar.AppendFormat(" IntelligenceCertificateBusiness='{0}',", requestModel.intelligence_certificate_business);
                }
                if (!string.IsNullOrEmpty(requestModel.business_intelligence_certificate_photo1))
                {
                    sbPar.AppendFormat(" BusinessIntelligenceCertificatePhoto1='{0}',", requestModel.business_intelligence_certificate_photo1);
                    sbRemark.AppendFormat(" 公司资质三证合一[{0}-{1}]", userInfo.BusinessIntelligenceCertificatePhoto1, requestModel.business_intelligence_certificate_photo1);
                }
                if (!string.IsNullOrEmpty(requestModel.business_intelligence_certificate_photo2))
                {
                    sbPar.AppendFormat(" BusinessIntelligenceCertificatePhoto2='{0}',", requestModel.business_intelligence_certificate_photo2);
                    sbRemark.AppendFormat(" 公司资质补充一[{0}-{1}]", userInfo.BusinessIntelligenceCertificatePhoto2, requestModel.business_intelligence_certificate_photo2);
                }
                if (!string.IsNullOrEmpty(requestModel.business_intelligence_certificate_photo3))
                {
                    sbPar.AppendFormat(" BusinessIntelligenceCertificatePhoto3='{0}',", requestModel.business_intelligence_certificate_photo3);
                    sbRemark.AppendFormat(" 公司资质补充二[{0}-{1}]", userInfo.BusinessIntelligenceCertificatePhoto3, requestModel.business_intelligence_certificate_photo3);
                }
                if (!string.IsNullOrEmpty(requestModel.business_intelligence_certificate_photo4))
                {
                    sbPar.AppendFormat(" BusinessIntelligenceCertificatePhoto4='{0}',", requestModel.business_intelligence_certificate_photo4);
                }
                if (!string.IsNullOrEmpty(requestModel.business_intelligence_certificate_photo5))
                {
                    sbPar.AppendFormat(" BusinessIntelligenceCertificatePhoto5='{0}',", requestModel.business_intelligence_certificate_photo5);
                }
                if (requestModel.imgs != null)
                {
                    sbPar.AppendFormat(" Images='{0}',", requestModel.imgs);
                }
                sbPar.AppendFormat(" ViewType='{0}',", requestModel.view_type);
            }
            else if (requestModel.action == "memberattribution")
            {
                //归属地设置
                //Province ProvinceCode City CityCode District DistrictCode ,Town TownCode 暂时设置的的时候就设置为空
                sbPar.AppendFormat(" Province='{0}', ProvinceCode='{1}', City='{2}', CityCode='{3}', District='{4}', DistrictCode='{5}'  ",
                        requestModel.province,
                        requestModel.province_code,
                        requestModel.city,
                        requestModel.city_code,
                        requestModel.district,
                        requestModel.district_code
                    );

            }


            if (requestModel.company_is_repeat == "1")
            {
                if (userInfo.Company != requestModel.company)
                {
                    if (bll.GetUserInfoByCompany(requestModel.company) != null)
                    {
                        resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                        resp.errmsg = "公司名称重复";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }
            }
            //if (requestModel.phone_is_repeat=="1")
            //{
            //    if (userInfo.Phone != requestModel.phone)
            //    {
            //        if (bll.GetUserInfoByPhone(requestModel.phone) != null)
            //        {
            //            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
            //            resp.errmsg = "手机已存在";
            //            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //            return;
            //        }
            //    }
            //}
            if (requestModel.user_is_repeat == "1")
            {
                if (userInfo.WXNickname != requestModel.nickname)
                {
                    if (bll.GetUserInfoByNickName(requestModel.nickname) != null)
                    {
                        resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                        resp.errmsg = "昵称重复";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }
            }

            if (bll.Update(userInfo, sbPar.ToString().TrimEnd(','), string.Format(" AutoID={0}", userInfo.AutoID)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
                resp.returnObj = sbPar.ToString();
                try
                {
                    string remark =  sbRemark.ToString();
                    if (remark != "修改会员信息：" && CurrentUserInfo.MemberLevel >= 10)
                    {
                        bllLog.Add(EnumLogType.ShMember, EnumLogTypeAction.Update, CurrentUserInfo.UserID, remark, targetID: userInfo.UserID);
                    }
                    bll.AddUserScoreDetail(CurrentUserInfo.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.UpdateMyInfo), bll.WebsiteOwner, null, null);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "修改会员数据出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 特殊处理时，赋值该字段
            /// </summary>
            public string action { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            public string truename { get; set; }

            /// <summary>
            /// 多张图片
            /// </summary>
            public string imgs { get; set; }

            /// <summary>
            /// 手机
            /// </summary>
            public string phone { get; set; }
            
            /// <summary>
            /// 单位名称
            /// </summary>
            public string company { get; set; }

            /// <summary>
            /// 职位
            /// </summary>
            public string postion { get; set; }

            /// <summary>
            /// 所属行业
            /// </summary>
            public string ex1 { get; set; }

            /// <summary>
            /// 邮箱
            /// </summary>
            public string email { get; set; }

            /// <summary>
            /// 所在商会
            /// </summary>
            public string ex2 { get; set; }

            /// <summary>
            /// 是否喝酒
            /// </summary>
            public string ex4 { get; set; }

            /// <summary>
            /// 是否抽烟
            /// </summary>
            public string ex3 { get; set; }

            /// <summary>
            /// 薪资
            /// </summary>
            public int salary { get; set; }


            /// <summary>
            /// 省份
            /// </summary>
            public string province { get; set; }
            public string province_code { get; set; }
            /// <summary>
            /// 城市
            /// </summary>
            public string city { get; set; }
            public string city_code { get; set; }
            /// <summary>
            /// 地区
            /// </summary>
            public string district { get; set; }
            public string district_code { get; set; }


            /// <summary>
            /// 签名
            /// </summary>
            public string describe { get; set; }

            /// <summary>
            /// 身份
            /// </summary>
            public string identification { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public long birthday { get; set; }

            /// <summary>
            /// 性别
            /// </summary>
            public string gender { get; set; }

            /// <summary>
            /// 图片
            /// </summary>
            public string avatar { get; set; }

            /// <summary>
            /// 昵称
            /// </summary>
            public string nickname { get; set; }

            /// <summary>
            /// 显示隐藏 手机
            /// </summary>
            public int view_type { get; set; }
            /// <summary>
            /// 验证手机验证码
            /// </summary>
            public int check_code { get; set; }
            /// <summary>
            /// 手机验证码
            /// </summary>
            public string code { get; set; }

            /// <summary>
            /// 判断公司名字是否重复
            /// </summary>
            public string company_is_repeat { get;set; }

            /// <summary>
            /// 判断用户昵称是否重复
            /// </summary>
            public string user_is_repeat { get; set; }

            public string phone_is_repeat { get; set; }

            public string identity_card_photo_front { get; set; }
            public string identity_card_photo_behind { get; set; }
            public string identity_card_photo_handheld { get; set; }
            public string business_intelligence_certificate_photo1 { get; set; }
            public string business_intelligence_certificate_photo2 { get; set; }
            public string business_intelligence_certificate_photo3 { get; set; }
            public string business_intelligence_certificate_photo4 { get; set; }
            public string business_intelligence_certificate_photo5 { get; set; }
            public string intelligence_certificate_business { get; set; }
            /// <summary>
            /// 验证码
            /// </summary>
            public string ver_code { get; set; }
        }
    }
}