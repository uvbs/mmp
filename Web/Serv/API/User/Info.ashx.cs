using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;
using System.Reflection;
using Newtonsoft.Json;
using ZentCloud.Common;
namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class Info : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLCarLibrary bllCar = new BLLCarLibrary();
        /// <summary>
        /// 
        /// </summary>
        BLLUserExpand bllUserEx = new BLLUserExpand();
        /// <summary>
        /// 微信
        /// </summary>
        BLLWeixin bllWeiXin = new BLLWeixin();
        /// <summary>
        /// 分销
        /// </summary>
        BLLDistribution bllDis = new BLLDistribution();
        /// <summary>
        /// yike
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();


        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string currentuserinfo(HttpContext context)
        {

            try
            {
                currentUserInfo = bllUser.GetUserInfo(bllUser.GetCurrUserID(), bllUser.WebsiteOwner);
                var websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
                Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                //double totalScore =CurrentUserInfo.TotalScore;
                #region 使用yike积分
                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                {
                    if ((!string.IsNullOrEmpty(currentUserInfo.Ex2)) && (!string.IsNullOrEmpty(currentUserInfo.Phone)))
                    {
                        Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus(currentUserInfo.Ex1, currentUserInfo.Ex2, currentUserInfo.Phone);
                        if (yikeUser != null)
                        {
                            currentUserInfo.TotalScore = yikeUser.Bonus;
                        }
                        else
                        {
                            currentUserInfo.TotalScore = 0;
                        }
                    }
                }
                #endregion

                #region 使用宏巍积分余额
                if (websiteInfo.IsUnionHongware == 1)
                {
                    var hongWareMemberInfo = hongWareClient.GetMemberInfo(currentUserInfo.WXOpenId);
                    if (hongWareMemberInfo.member != null)
                    {

                        currentUserInfo.TotalScore = Convert.ToInt32(hongWareMemberInfo.member.point);
                        currentUserInfo.AccountAmount = (decimal)hongWareMemberInfo.member.balance;

                    }
                    else
                    {
                        currentUserInfo.TotalScore = 0;
                        currentUserInfo.AccountAmount = 0;
                    }


                }
                #endregion


                //var carOwnerInfo = bllUserEx.GetCurrUserCarOwnerInfo();
                //var bankcard = bllUser.Get<BindBankCard>(string.Format("UserId='{0}'", currentUserInfo.UserID));

                var hasidentitycardphoto = !string.IsNullOrWhiteSpace(currentUserInfo.IdentityCardPhotoFront + currentUserInfo.IdentityCardPhotoBehind + currentUserInfo.IdentityCardPhotoHandheld);
                var hasbusinessintelligencecertificatephoto = !string.IsNullOrWhiteSpace(currentUserInfo.BusinessIntelligenceCertificatePhoto1 + currentUserInfo.BusinessIntelligenceCertificatePhoto2
                    + currentUserInfo.BusinessIntelligenceCertificatePhoto3 + currentUserInfo.BusinessIntelligenceCertificatePhoto4 + currentUserInfo.BusinessIntelligenceCertificatePhoto5);
                var hasinvoicinginformation = bllUserEx.ExistUserExpand(UserExpandType.InvoicingInformation, currentUserInfo.UserID);

                var data = new
                {
                    id = currentUserInfo.AutoID,
                    userid = currentUserInfo.UserID,
                    head_img_url = bllUser.GetUserDispalyAvatar(currentUserInfo),//GetHeadImgUrl(context,CurrentUserInfo),
                    nickname = bllUser.GetUserDispalyName(currentUserInfo),//CurrentUserInfo.WXNickname,
                    wx_nickname=currentUserInfo.WXNickname,
                    phone = currentUserInfo.Phone,
                    username = currentUserInfo.UserID,
                    unionid = currentUserInfo.WXUnionID,
                    totalscore = currentUserInfo.TotalScore,
                    history_totalscore = currentUserInfo.HistoryTotalScore,
                    company_name = currentUserInfo.Company,
                    position = currentUserInfo.Postion,
                    email = currentUserInfo.Email,
                    truename = currentUserInfo.TrueName,
                    ex1 = currentUserInfo.Ex1,
                    ex2 = currentUserInfo.Ex2,
                    ex3 = currentUserInfo.Ex3,
                    ex4 = currentUserInfo.Ex4,
                    ex5 = currentUserInfo.Ex5,
                    wxOpenId = currentUserInfo.WXOpenId,
                    //carServerType = CurrentUserInfo.CarServerType,
                    //carOwnerInfo = carOwnerInfo == null ? null : new
                    //{

                    //    carModel = carOwnerInfo.CarModel == null ? null : new
                    //    {
                    //        carModelId = carOwnerInfo.CarModel.CarModelId,
                    //        carBrandId = carOwnerInfo.CarModel.CarBrandId,
                    //        carBrandName = bllCar.GetBrand(carOwnerInfo.CarModel.CarBrandId).CarBrandName,
                    //        carSeriesCateId = carOwnerInfo.CarModel.CarSeriesCateId,
                    //        carSeriesId = carOwnerInfo.CarModel.CarSeriesId,
                    //        carSeriesName = bllCar.GetSeriesInfo(carOwnerInfo.CarModel.CarSeriesId).CarSeriesName,
                    //        carModelName = carOwnerInfo.CarModel.CarModelName,
                    //        showName = carOwnerInfo.CarModel.ShowName,
                    //        allName = carOwnerInfo.CarModel.AllName,
                    //        year = carOwnerInfo.CarModel.Year,
                    //        img = carOwnerInfo.CarModel.Img,
                    //        guidePrice = carOwnerInfo.CarModel.GuidePrice,
                    //        colors = string.IsNullOrWhiteSpace(carOwnerInfo.CarModel.Colors) ? null : JsonConvert.DeserializeObject<List<BLLJIMP.Model.CarModelColorInfo>>(carOwnerInfo.CarModel.Colors).Select(c => new { name = c.Name, value = c.Value })
                    //    },
                    //    carNumber = carOwnerInfo.CarNumber,
                    //    carNumberTime = carOwnerInfo.CarNumberTime == null ? "" : carOwnerInfo.CarNumberTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    //    drivingLicenseTime = carOwnerInfo.DrivingLicenseTime == null ? "" : carOwnerInfo.DrivingLicenseTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    //    drivingLicenseType = carOwnerInfo.DrivingLicenseType,
                    //    vin = carOwnerInfo.VIN
                    //},
                    address = currentUserInfo.Address,
                    imgs = currentUserInfo.Images,
                    province = currentUserInfo.Province,
                    province_code = currentUserInfo.ProvinceCode,
                    city = currentUserInfo.City,
                    city_code = currentUserInfo.CityCode,
                    district = currentUserInfo.District,
                    district_code = currentUserInfo.DistrictCode,
                    identification = currentUserInfo.Ex5,
                    salary = currentUserInfo.Salary,
                    birthday = DateTimeHelper.DateTimeToUnixTimestamp(currentUserInfo.Birthday),
                    birthday_str = DateTimeHelper.DateTimeToString(currentUserInfo.Birthday),
                    gender = currentUserInfo.Gender,
                    sex = currentUserInfo.WXSex.HasValue ? currentUserInfo.WXSex.Value : 0,
                    member_level = currentUserInfo.MemberLevel,
                    hasqrcode = bllUser.IsDistributionMember(currentUserInfo),
                    describe = currentUserInfo.Description,
                    avatar = currentUserInfo.Avatar,
                    credit_acount = currentUserInfo.CreditAcount,
                    //hasbankcard = bankcard == null ? false : true,
                    hasidentitycardphoto = hasidentitycardphoto,
                    identity_card_photo_front = currentUserInfo.IdentityCardPhotoFront,
                    identity_card_photo_behind = currentUserInfo.IdentityCardPhotoBehind,
                    identity_card_photo_handheld = currentUserInfo.IdentityCardPhotoHandheld,
                    hasbusinessintelligencecertificatephoto = hasbusinessintelligencecertificatephoto,
                    intelligence_certificate_business = currentUserInfo.IntelligenceCertificateBusiness,
                    business_intelligence_certificate_photo1 = currentUserInfo.BusinessIntelligenceCertificatePhoto1,
                    business_intelligence_certificate_photo2 = currentUserInfo.BusinessIntelligenceCertificatePhoto2,
                    business_intelligence_certificate_photo3 = currentUserInfo.BusinessIntelligenceCertificatePhoto3,
                    business_intelligence_certificate_photo4 = currentUserInfo.BusinessIntelligenceCertificatePhoto4,
                    business_intelligence_certificate_photo5 = currentUserInfo.BusinessIntelligenceCertificatePhoto5,
                    hasinvoicinginformation = hasinvoicinginformation,
                    websiteowner = bllUser.WebsiteOwner,
                    account_amount = currentUserInfo.AccountAmount,//账户余额
                    is_bind_hongware = bllUser.IsBindHongWare(currentUserInfo, websiteInfo),//是否已经绑定宏巍
                    memberattribution = currentUserInfo.MemberAttribution,
                    distribution_owner=currentUserInfo.DistributionOwner
                    //is_attention_weixin = bllWeiXin.IsWeixinFollower(bllWeiXin.GetAccessToken(), CurrentUserInfo.WXOpenId)
                };
                return ZentCloud.Common.JSONHelper.ObjectToJson(data);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        //public string GetHeadImgUrl(HttpContext context, UserInfo userInfo)
        //{
        //    if (!string.IsNullOrEmpty(userInfo.WXHeadimgurlLocal))
        //    {
        //        return bllCommRelation.GetImgUrl(userInfo.WXHeadimgurlLocal);
        //    }
        //    else
        //    {
        //        return string.Format("http://{0}/img/persion.png", context.Request.Url.Host);
        //    }
        //}
    }
}