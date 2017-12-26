using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.Data;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 真实活动BLL
    /// </summary>
    public class BLLActivity : BLL
    {

        /// <summary>
        /// 当前活动发起者的用户名
        /// </summary>
        string ActivityUserID = string.Empty;
        ///// <summary>
        ///// 当前活动发起者的用户信息
        ///// </summary>
        //UserInfo ActivityUserInfo;
        /// <summary>
        /// 返回地址
        /// </summary>
        string CallBackUrl = string.Empty;
        /// <summary>
        /// Web请求
        /// </summary>
        Common.HttpInterFace WebRequest = new Common.HttpInterFace();//
        public BLLActivity(string userID)
            : base(userID)
        {

        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteActivity(List<string> ids)
        {
            //删除活动数据
            //删除活动表映射
            //删除活动

            //改为标识删除 -- 2013.11.13 

            try
            {
                string idsStr = Common.StringHelper.ListToStr<string>(ids, "'", ",");

                //Delete(new Model.ActivityDataInfo(), string.Format(" ActivityID in ({0}) ", idsStr));
                //Delete(new Model.ActivityFieldMappingInfo(), string.Format(" ActivityID in ({0}) ", idsStr));
                //Delete(new Model.ActivityInfo(), string.Format(" ActivityID in ({0}) ", idsStr));

                if (Update(new ActivityInfo(), " IsDelete = 1 ", string.Format(" ActivityID in ({0}) ", idsStr)) > 0)
                    return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 获取数据表字段对照列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<Model.ActivityFieldMappingInfo> GetActivityFieldMappingList(string activityId)
        {
            List<Model.ActivityFieldMappingInfo> result = new List<Model.ActivityFieldMappingInfo>();

            //加载默认字段
            result.Add(new Model.ActivityFieldMappingInfo()
            {
                ActivityID = activityId,
                FieldName = "Name",
                FieldIsDefauld = 1,
                FieldIsNull = 0,
                MappingName = "姓名",
                FormatValiFunc = "",
                FieldType = 0,
                FormatValiExpression = ""
            });
            result.Add(new Model.ActivityFieldMappingInfo()
            {
                ActivityID = activityId,
                FieldName = "Phone",
                FieldIsDefauld = 1,
                FieldIsNull = 0,
                MappingName = "手机号码",
                FormatValiFunc = "手机",
                FieldType = 0,
                FormatValiExpression = ""
            });

            //加载扩展字段
            result.AddRange(GetList<Model.ActivityFieldMappingInfo>(string.Format(" ActivityID = '{0}' ORDER BY ExFieldIndex ASC", activityId)));

            //读取排序
            string fieldSort = GetActivityInfoByActivityID(activityId).FieldSort;

            if (!string.IsNullOrWhiteSpace(fieldSort))
            {
                List<Model.ActivityFieldMappingInfo> tmpArr = new List<ActivityFieldMappingInfo>();
                //排序处理

                List<string> sortList = fieldSort.Split(',').ToList();

                foreach (var item in sortList)
                {
                    tmpArr.AddRange(result.Where(p => p.FieldName.Equals(item, StringComparison.OrdinalIgnoreCase)));
                }

                tmpArr.AddRange(result.Where(p => !tmpArr.Contains(p)));

                result = tmpArr;
            }

            return result;
        }

        ///// <summary>
        ///// 活动报名处理逻辑
        ///// </summary>
        ///// <param name="context"></param>
        //public void APIActionResult(HttpContext context)
        //{

        //    contextthis = context;
        //    var LoginName = GetPostParm("LoginName");
        //    var LoginPwd = GetPostParm("LoginPwd");
        //    var ActivityID = GetPostParm("ActivityID");
        //    CallBackUrl = GetPostParm("CallBackUrl");
        //    CallBackUrl = CallBackUrl.Contains("?") ? CallBackUrl + "&" : CallBackUrl + "?";
        //    ///活动信息
        //    Activity = Get<ActivityInfo>(string.Format("ActivityID='{0}'", ActivityID));
        //    #region IP限制
        //    //获取用户IP;
        //    string UserHostAddress = context.Request.UserHostAddress;
        //    var count = DataCache.GetCache(UserHostAddress);
        //    if (count != null)
        //    {
        //        int newcount = int.Parse(count.ToString()) + 1;
        //        DataCache.SetCache(UserHostAddress, newcount);
        //        int LimitCount = 100;
        //        if (Activity != null)
        //        {
        //            if (Activity.LimitCount != null)
        //            {
        //                LimitCount = Activity.LimitCount;
        //            }

        //        }
        //        if (newcount >= LimitCount)
        //        {
        //            context.ApplicationInstance.CompleteRequest();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        DataCache.SetCache(UserHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
        //    }


        //    #endregion

        //    #region 登录验证
        //    bool loginStatus = false;
        //    ZentCloud.BLLJIMP.BLLUser userBll = new BLLJIMP.BLLUser("");
        //    ActivityUserID = Common.Base64Change.DecodeBase64ByUTF8(LoginName);

        //    loginStatus = userBll.LoginZCEncrypt(ActivityUserID, LoginPwd);
        //    if (!loginStatus)
        //    {
        //        context.Response.Redirect(string.Format("{0}status=4&message=登录失败", CallBackUrl), false);
        //        return;
        //    }
        //    ActivityUserInfo = Get<UserInfo>(string.Format("UserID='{0}'", ActivityUserID));//活动发起者信息
        //    #endregion

        //    #region 活动权限验证

        //    if (Activity == null)
        //    {
        //        context.Response.Redirect(string.Format("{0}status=7&message=活动编号不存在", CallBackUrl), false);
        //        return;
        //    }

        //    if (Activity.ActivityStatus.Equals(0))
        //    {
        //        context.Response.Redirect(string.Format("{0}status=8&message=活动已关闭", CallBackUrl), false);
        //        return;

        //    }
        //    if (!(Activity.UserID.Equals(ActivityUserID)))
        //    {
        //        context.Response.Redirect(string.Format("{0}status=9&message=无权进行此操作", CallBackUrl), false);
        //        return;

        //    }
        //    #endregion

        //    string ActivityType = GetPostParm("ActivityType");
        //    switch (ActivityType)
        //    {
        //        case "normal"://一般报名
        //            NormalApply();
        //            break;
        //        case "phone"://手机快速报名
        //            PhoneApply();
        //            break;
        //        case "weibo"://微博快速报名
        //            WeiBoApply();
        //            break;
        //        case "weixin"://微信快速报名
        //            break;
        //        default:
        //            break;
        //    }




        //}

        ///// <summary>
        ///// 一般报名
        ///// </summary>
        //private void NormalApply()
        //{
        //    try
        //    {
        //        #region 获取报名参数
        //        var Name = GetPostParm("Name");
        //        var Phone = GetPostParm("Phone");
        //        var K1 = GetPostParm("K1");
        //        var K2 = GetPostParm("K2");
        //        var K3 = GetPostParm("K3");
        //        var K4 = GetPostParm("K4");
        //        var K5 = GetPostParm("K5");
        //        var K6 = GetPostParm("K6");
        //        var K7 = GetPostParm("K7");
        //        var K8 = GetPostParm("K8");
        //        var K9 = GetPostParm("K9");
        //        var K10 = GetPostParm("K10");
        //        var K11 = GetPostParm("K11");
        //        var K12 = GetPostParm("K12");
        //        var K13 = GetPostParm("K13");
        //        var K14 = GetPostParm("K14");
        //        var K15 = GetPostParm("K15");
        //        var K16 = GetPostParm("K16");
        //        var K17 = GetPostParm("K17");
        //        var K18 = GetPostParm("K18");
        //        var K19 = GetPostParm("K19");
        //        var K20 = GetPostParm("K20");

        //        #endregion

        //        #region 检查必填项
        //        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=3&message=姓名和手机不能为空!", CallBackUrl), false);
        //            return;
        //        }

        //        if (!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=5&message=手机号码无效!", CallBackUrl), false);

        //            return;
        //        }


        //        #endregion

        //        #region 检查是否已经报名

        //        if (GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}'", Activity.ActivityID, Phone)) > 0)
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=1&message=该用户已经报过名", CallBackUrl), false);
        //            return;

        //        }

        //        #endregion

        //        #region 设置报名表数据
        //        var NewActivityUID = 1001;
        //        var LastActivityDataInfo = Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", Activity.ActivityID));
        //        if (LastActivityDataInfo != null)
        //        {
        //            NewActivityUID = LastActivityDataInfo.UID + 1;
        //        }

        //        ActivityDataInfo model = new ActivityDataInfo();
        //        model.ActivityID = Activity.ActivityID;
        //        model.UID = NewActivityUID;
        //        model.Name = Name;
        //        model.Phone = Phone;
        //        model.K1 = K1;
        //        model.K2 = K2;
        //        model.K3 = K3;
        //        model.K4 = K4;
        //        model.K5 = K5;
        //        model.K6 = K6;
        //        model.K7 = K7;
        //        model.K8 = K8;
        //        model.K9 = K9;
        //        model.K10 = K10;
        //        model.K11 = K11;
        //        model.K12 = K12;
        //        model.K13 = K13;
        //        model.K14 = K14;
        //        model.K15 = K15;
        //        model.K16 = K16;
        //        model.K17 = K17;
        //        model.K18 = K18;
        //        model.K19 = K19;
        //        model.K20 = K20;
        //        #endregion
        //        if (Add(model))
        //        {
        //            //检查是否发送短信
        //            SendSms(Phone);
        //            try
        //            {
        //                #region 报名成功后客户信息处理
        //                //*TODO:一:检查手机号，若手机号不存在，则直接把客户信息插入客户表
        //                ///二：若手机号已经存在 则分以下步骤:
        //                ///1.原有的姓名为空，报名的数据有姓名，则更新客户姓名
        //                ///2.原有的性别为空，报名的数据有性别，则更新客户性别
        //                ///3.原有的生日为空，报名的数据有生日，则更新客户生日
        //                ///4.原有的QQ为空，报名的数据有QQ，则更新客户QQ
        //                ///5.原有的固定电话为空，报名的数据有固定电话，则更新客户固定电话
        //                ///6.原有的网址为空，报名的数据有网址，则更新客户网址
        //                ///7.原有的公司为空，报名的数据有公司，则更新客户公司
        //                ///8.原有的职位为空，报名的数据有职位，则更新客户职位
        //                ///9.原有的地址为空，报名的数据有地址，则更新客户地址
        //                ///10.原有的备注为空，报名的数据有备注，则更新客户备注
        //                ///11.报名的数据有邮箱，则依次检查第一个邮箱，第二个邮箱，第三个邮箱，第四个邮箱是否有数据，没有数据就更新，如果四个邮箱都有数据了则放弃报名邮箱
        //                ///12.报名的数据有微博ID，则依次检查第一个微博ID，第二个微博ID，第三个微博ID，第四个微博ID是否有数据，没有数据就更新，如果四个微博ID都有数据了则放弃报名微博ID
        //                ///13.报名的数据有微信OpenID，则依次检查第一个微信OpenID，第二个微信OpenID，第三个微信OpenID，第四个微信OpenID是否有数据，没有数据就更新，如果四个微信OpenID都有数据了则放弃报名微信OpenID


        //                var MemberInfo = Get<MemberInfo>(string.Format("UserID='{0}' And Mobile='{1}'", ActivityUserID, model.Phone));//属于此活动发起者 的手机号的客户

        //                var RequestMemberInfo = ConvertToMemberInfoModel(model);//报名数据转换成的客户实体

        //                #region 手机号不存在，则直接把客户信息插入客户表
        //                if (MemberInfo == null)//手机号不存在，则直接把客户信息插入客户表
        //                {
        //                    RequestMemberInfo.MemberID = GetGUID(BLLJIMP.TransacType.MemberAdd);
        //                    RequestMemberInfo.UserID = ActivityUserID;
        //                    RequestMemberInfo.MemberType = 0;
        //                    Add(RequestMemberInfo);

        //                }
        //                #endregion

        //                #region 手机号已经存在，检查是否需要更新客户表
        //                else
        //                {
        //                    #region 原有的姓名为空，报名的数据有姓名，则更新客户姓名
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Name))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Name))
        //                        {
        //                            //原有的姓名为空，报名的数据有姓名，则更新客户姓名
        //                            Update(MemberInfo, string.Format("Name='{0}'", RequestMemberInfo.Name), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的性别为空，报名的数据有性别，则更新客户性别
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Sex))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Sex))
        //                        {
        //                            //原有的性别为空，报名的数据有性别，则更新客户性别
        //                            Update(MemberInfo, string.Format("Sex='{0}'", RequestMemberInfo.Sex), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的生日为空，报名的数据有生日，则更新客户生日
        //                    if (RequestMemberInfo.Birthday != null)
        //                    {
        //                        if (MemberInfo.Birthday == null)
        //                        {
        //                            //原有的生日为空，报名的数据有生日，则更新客户生日
        //                            Update(MemberInfo, string.Format("Birthday='{0}'", RequestMemberInfo.Birthday), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的QQ为空，报名的数据有QQ，则更新客户QQ
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.QQ))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.QQ))
        //                        {
        //                            //原有的QQ为空，报名的数据有QQ，则更新客户QQ
        //                            Update(MemberInfo, string.Format("QQ='{0}'", RequestMemberInfo.QQ), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的固定电话为空，报名的数据有固定电话，则更新客户固定电话
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Tel))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Tel))
        //                        {
        //                            //原有的固定电话为空，报名的数据有固定电话，则更新客户固定电话
        //                            Update(MemberInfo, string.Format("Tel='{0}'", RequestMemberInfo.Tel), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }

        //                    #endregion

        //                    #region 原有的网址为空，报名的数据有网址，则更新客户网址
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Website))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Website))
        //                        {
        //                            //原有的网址为空，报名的数据有网址，则更新客户网址
        //                            Update(MemberInfo, string.Format("Website='{0}'", RequestMemberInfo.Website), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的公司为空，报名的数据有公司，则更新客户公司
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Company))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Company))
        //                        {
        //                            //原有的公司为空，报名的数据有公司，则更新客户公司
        //                            Update(MemberInfo, string.Format("Company='{0}'", RequestMemberInfo.Company), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的职位为空，报名的数据有职位，则更新客户职位
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Title))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Title))
        //                        {
        //                            //原有的职位为空，报名的数据有职位，则更新客户职位
        //                            Update(MemberInfo, string.Format("Title='{0}'", RequestMemberInfo.Title), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的地址为空，报名的数据有地址，则更新客户地址
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Address))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Address))
        //                        {
        //                            //原有的地址为空，报名的数据有地址，则更新客户地址
        //                            Update(MemberInfo, string.Format("Address='{0}'", RequestMemberInfo.Address), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 原有的备注为空，报名的数据有备注，则更新客户备注
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Remark))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Remark))
        //                        {
        //                            //原有的备注为空，报名的数据有备注，则更新客户备注
        //                            Update(MemberInfo, string.Format("Remark='{0}'", RequestMemberInfo.Remark), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }
        //                    }
        //                    #endregion

        //                    #region 检查邮箱更新
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.Email))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.Email))//原有的第一个邮箱为空,直接把报名邮箱更新到第一个邮箱
        //                        {
        //                            //更新第一个邮箱
        //                            Update(MemberInfo, string.Format("Email='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }

        //                        else//原有的第一个邮箱不为空
        //                        {

        //                            if (!MemberInfo.Email.Equals(RequestMemberInfo.Email) && (!RequestMemberInfo.Email.Equals(MemberInfo.Email2)) && (!RequestMemberInfo.Email.Equals(MemberInfo.Email3)) && (!RequestMemberInfo.Email.Equals(MemberInfo.Email4)))//报名的邮箱与原来的四个邮箱不相同，则检查能否更新到拓展邮箱里
        //                            {
        //                                if (string.IsNullOrEmpty(MemberInfo.Email2))//第二个邮箱为空，则把报名邮箱更新到第二个邮箱
        //                                {
        //                                    //更新第二个邮箱
        //                                    Update(MemberInfo, string.Format("Email2='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));

        //                                }
        //                                else//第二个邮箱不为空，检查第三个邮箱
        //                                {
        //                                    if (string.IsNullOrEmpty(MemberInfo.Email3))//第三个邮箱为空，则报名邮箱更新到第三个邮箱
        //                                    {
        //                                        //更新第三个邮箱
        //                                        Update(MemberInfo, string.Format("Email3='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));

        //                                    }
        //                                    else//第三个邮箱也有数据了，检查第四个邮箱
        //                                    {
        //                                        if (string.IsNullOrEmpty(MemberInfo.Email4))//第四个邮箱为空，则报名邮箱更新到第四个邮箱
        //                                        {
        //                                            //更新第四个邮箱
        //                                            Update(MemberInfo, string.Format("Email4='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                                        }
        //                                        else//第四个邮箱有数据，则放弃报名邮箱
        //                                        {


        //                                        }

        //                                    }
        //                                }


        //                            }
        //                            else//报名的邮箱与原来第一个邮箱相同，不做操作
        //                            {

        //                            }


        //                        }



        //                    }
        //                    #endregion

        //                    #region 检查微博ID更新
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.WeiboID))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.WeiboID))//原有的第一个微博ID为空
        //                        {
        //                            //更新第一个微博ID
        //                            Update(MemberInfo, string.Format("WeiboID='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }

        //                        else//原有的第一个微博ID不为空
        //                        {

        //                            if (!MemberInfo.WeiboID.Equals(RequestMemberInfo.WeiboID) && (!RequestMemberInfo.WeiboID.Equals(MemberInfo.WeiboID2)) && (!RequestMemberInfo.WeiboID.Equals(MemberInfo.WeiboID3)) && (!RequestMemberInfo.WeiboID.Equals(MemberInfo.WeiboID4)))//报名的微博ID与原来的四个微博ID都不相同，则检查能否更新到拓展微博ID里
        //                            {
        //                                if (string.IsNullOrEmpty(MemberInfo.WeiboID2))//第二个微博ID为空，则把报名微博ID更新到第二个微博ID
        //                                {
        //                                    //更新第二个微博ID
        //                                    Update(MemberInfo, string.Format("WeiboID2='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

        //                                }
        //                                else//第二个微博ID不为空，检查第三个微博ID
        //                                {
        //                                    if (string.IsNullOrEmpty(MemberInfo.WeiboID3))//第三个微博ID为空，则报名微博ID更新到第三个微博ID
        //                                    {
        //                                        //更新第三个微博ID
        //                                        Update(MemberInfo, string.Format("WeiboID3='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

        //                                    }
        //                                    else//第三个微博ID也有数据了，检查第四个微博ID
        //                                    {
        //                                        if (string.IsNullOrEmpty(MemberInfo.WeiboID4))//第四个微博ID为空，则报名微博ID更新到第四个微博ID
        //                                        {
        //                                            //更新第四个微博ID
        //                                            Update(MemberInfo, string.Format("WeiboID4='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                                        }
        //                                        else
        //                                        {
        //                                            //第四个微博ID有数据了，放弃报名微博ID
        //                                        }

        //                                    }
        //                                }


        //                            }
        //                            else//报名的微博ID与原来第一个微博ID相同，不做操作
        //                            {

        //                            }


        //                        }



        //                    }
        //                    #endregion

        //                    #region 检查微信OPenID更新
        //                    if (!string.IsNullOrEmpty(RequestMemberInfo.WeixinOpenID))
        //                    {
        //                        if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID))//原有的第一个微信OPenID为空,直接把报名微信OPenID更新到第一个微信OPenID
        //                        {
        //                            //更新第一个微信OPenID
        //                            Update(MemberInfo, string.Format("WeixinOpenID='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                        }

        //                        else//原有的第一个微信OPenID不为空
        //                        {

        //                            if (!MemberInfo.WeixinOpenID.Equals(RequestMemberInfo.WeixinOpenID) && (!RequestMemberInfo.WeixinOpenID.Equals(MemberInfo.WeixinOpenID2)) && (!RequestMemberInfo.WeixinOpenID.Equals(MemberInfo.WeixinOpenID3)) && (!RequestMemberInfo.WeixinOpenID.Equals(MemberInfo.WeixinOpenID4)))//报名的微信OPenID与原来的四个微信OPenID不相同，则检查能否更新到拓展微信OPenID里
        //                            {
        //                                if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID2))//第二个微信OPenID为空，则把报名微信OPenID更新到第二个微信OPenID
        //                                {
        //                                    //更新第二个微信OPenID
        //                                    Update(MemberInfo, string.Format("WeixinOpenID2='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

        //                                }
        //                                else//第二个微信OPenID不为空，检查第三个微信OPenID
        //                                {
        //                                    if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID3))//第三个微信OPenID为空，则报名微信OPenID更新到第三个微信OPenID
        //                                    {
        //                                        //更新第三个微信OPenID
        //                                        Update(MemberInfo, string.Format("WeixinOpenID3='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

        //                                    }
        //                                    else//第三个微信OPenID也有数据了，检查第四个微信OPenID
        //                                    {
        //                                        if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID4))//第四个微信OPenID为空，则报名微信OPenID更新到第四个微信OPenID
        //                                        {
        //                                            //更新第四个微信OPenID
        //                                            Update(MemberInfo, string.Format("WeixinOpenID4='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
        //                                        }
        //                                        else//第四个微信OPenID有数据，则放弃报名微信OPenID
        //                                        {


        //                                        }

        //                                    }
        //                                }


        //                            }
        //                            else//报名的微信OPenID与原来第一个微信OPenID相同，不做操作
        //                            {

        //                            }


        //                        }



        //                    }
        //                    #endregion

        //                }
        //                #endregion


        //                #endregion
        //            }
        //            catch (Exception ex)
        //            {
        //                //ToDo:客户信息处理异常
        //                //日志记录
        //            }
        //            //回调
        //            contextthis.Response.Redirect(string.Format("{0}status=0&message=报名成功！", CallBackUrl), false);


        //            return;


        //        }
        //        else
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
        //            return;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        contextthis.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
        //        return;
        //    }

        //}

        ///// <summary>
        ///// 手机快速报名
        ///// </summary>
        //private void PhoneApply()
        //{

        //    try
        //    {
        //        var Phone = GetPostParm("Phone");
        //        //检查手机号
        //        if (!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=5&message=手机号码无效!", CallBackUrl), false);
        //            return;
        //        }
        //        //检查该手机号有无相关报名信息
        //        var MemberInfo = Get<MemberInfo>(string.Format("UserID='{0}' And Mobile='{1}'", ActivityUserID, Phone));
        //        if (MemberInfo == null)
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=5&message=该手机号无相关报名信息，无法快速报名!", CallBackUrl), false);
        //            return;
        //        }

        //        #region 检查是否已经报名

        //        if (GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}'", Activity.ActivityID, Phone)) > 0)
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=1&message=该用户已经报过名", CallBackUrl), false);
        //            return;

        //        }

        //        #endregion


        //        var NewActivityUID = 1001;
        //        var LastActivityDataInfo = Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", Activity.ActivityID));
        //        if (LastActivityDataInfo != null)
        //        {
        //            NewActivityUID = LastActivityDataInfo.UID + 1;
        //        }

        //        ActivityDataInfo model = new ActivityDataInfo();
        //        model.ActivityID = Activity.ActivityID;
        //        model.UID = NewActivityUID;
        //        model.Name = MemberInfo.Name;
        //        model.Phone = Phone;

        //        //设置报名表扩展字段
        //        var ActivityExFieldList = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", Activity.ActivityID));
        //        foreach (var item in ActivityExFieldList)
        //        {
        //            model = SetActivityDataInfoModel(item.MappingName, item.ExFieldIndex, MemberInfo, model);
        //        }

        //        if (Add(model))//添加成功
        //        {
        //            SendSms(Phone);//检查是否发送通知短信
        //            contextthis.Response.Redirect(string.Format("{0}status=0&message=报名成功！", CallBackUrl), false);
        //            return;

        //        }
        //        else//添加失败
        //        {

        //            contextthis.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
        //            return;

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        contextthis.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
        //        return;
        //    }


        //}

        ///// <summary>
        ///// 微博快速报名
        ///// </summary>
        //private void WeiBoApply()
        //{

        //    try
        //    {
        //        var WeiBoID = GetPostParm("WeiBoID");
        //        //检查WeiBoID
        //        if (!Common.ValidatorHelper.IsNumber(WeiBoID))
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=5&message=微博ID无效!", CallBackUrl), false);
        //            return;
        //        }
        //        //检查该WeiBoID有无相关报名信息
        //        var MemberInfo = Get<MemberInfo>(string.Format("UserID='{0}' And WeiBoID='{1}'", ActivityUserID, WeiBoID));
        //        if (MemberInfo == null)
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=5&message=该微博ID无相关报名信息，无法快速报名!", CallBackUrl), false);
        //            return;
        //        }

        //        #region 检查是否已经报名

        //        if (GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}'", Activity.ActivityID, MemberInfo.Mobile)) > 0)
        //        {
        //            contextthis.Response.Redirect(string.Format("{0}status=1&message=该用户已经报过名", CallBackUrl), false);
        //            return;

        //        }

        //        #endregion


        //        var NewActivityUID = 1001;
        //        var LastActivityDataInfo = Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", Activity.ActivityID));
        //        if (LastActivityDataInfo != null)
        //        {
        //            NewActivityUID = LastActivityDataInfo.UID + 1;
        //        }

        //        ActivityDataInfo model = new ActivityDataInfo();
        //        model.ActivityID = Activity.ActivityID;
        //        model.UID = NewActivityUID;
        //        model.Name = MemberInfo.Name;
        //        model.Phone = MemberInfo.Mobile;

        //        //设置报名表扩展字段
        //        var ActivityExFieldList = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", Activity.ActivityID));
        //        foreach (var item in ActivityExFieldList)
        //        {
        //            model = SetActivityDataInfoModel(item.MappingName, item.ExFieldIndex, MemberInfo, model);
        //        }

        //        if (Add(model))//添加成功
        //        {
        //            SendSms(MemberInfo.Mobile);//检查是否发送通知短信
        //            contextthis.Response.Redirect(string.Format("{0}status=0&message=报名成功！", CallBackUrl), false);
        //            return;

        //        }
        //        else//添加失败
        //        {

        //            contextthis.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
        //            return;

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        contextthis.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
        //        return;
        //    }


        //}


        /// <summary>
        /// 根据提交的报名数据转换成客户信息实体
        /// </summary>
        /// <param name="activityDataModel">报名数据实体</param>
        /// <returns></returns>
        private MemberInfo ConvertToMemberInfoModel(ActivityDataInfo activityDataModel)
        {
            MemberInfo model = new MemberInfo();
            model.Name = activityDataModel.Name;
            model.Mobile = activityDataModel.Phone;
            if (!string.IsNullOrEmpty(activityDataModel.K1))
            {
                var k1MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 1);
                model = SetMemberInfoModelByMappingName(k1MappingName, activityDataModel.K1, model);

            }
            if (!string.IsNullOrEmpty(activityDataModel.K2))
            {
                var k2MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 2);
                model = SetMemberInfoModelByMappingName(k2MappingName, activityDataModel.K2, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K3))
            {
                var k3MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 3);
                model = SetMemberInfoModelByMappingName(k3MappingName, activityDataModel.K3, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K4))
            {
                var k4MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 4);
                model = SetMemberInfoModelByMappingName(k4MappingName, activityDataModel.K4, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K5))
            {
                var k5MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 5);
                model = SetMemberInfoModelByMappingName(k5MappingName, activityDataModel.K5, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K6))
            {
                var k6MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 6);
                model = SetMemberInfoModelByMappingName(k6MappingName, activityDataModel.K6, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K7))
            {
                var k7MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 7);
                model = SetMemberInfoModelByMappingName(k7MappingName, activityDataModel.K7, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K8))
            {
                var k8MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 8);
                model = SetMemberInfoModelByMappingName(k8MappingName, activityDataModel.K8, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K9))
            {
                var k9MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 9);
                model = SetMemberInfoModelByMappingName(k9MappingName, activityDataModel.K9, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K10))
            {
                var k10MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 10);
                model = SetMemberInfoModelByMappingName(k10MappingName, activityDataModel.K10, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K11))
            {
                var k11MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 11);
                model = SetMemberInfoModelByMappingName(k11MappingName, activityDataModel.K11, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K12))
            {
                var k12MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 12);
                model = SetMemberInfoModelByMappingName(k12MappingName, activityDataModel.K12, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K13))
            {
                var k13MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 13);
                model = SetMemberInfoModelByMappingName(k13MappingName, activityDataModel.K13, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K14))
            {
                var k14MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 14);
                model = SetMemberInfoModelByMappingName(k14MappingName, activityDataModel.K14, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K15))
            {
                var k15MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 15);
                model = SetMemberInfoModelByMappingName(k15MappingName, activityDataModel.K15, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K16))
            {
                var k16MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 16);
                model = SetMemberInfoModelByMappingName(k16MappingName, activityDataModel.K16, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K17))
            {
                var k17MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 17);
                model = SetMemberInfoModelByMappingName(k17MappingName, activityDataModel.K17, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K18))
            {
                var k18MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 18);
                model = SetMemberInfoModelByMappingName(k18MappingName, activityDataModel.K18, model);
            }

            if (!string.IsNullOrEmpty(activityDataModel.K19))
            {
                var k19MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 19);
                model = SetMemberInfoModelByMappingName(k19MappingName, activityDataModel.K19, model);
            }
            if (!string.IsNullOrEmpty(activityDataModel.K20))
            {
                var k20MappingName = GetMappingNameByActivityIDAndExFieldIndex(activityDataModel.ActivityID, 20);
                model = SetMemberInfoModelByMappingName(k20MappingName, activityDataModel.K20, model);
            }


            return model;


        }

        /// <summary>
        /// 根据活动ID跟扩展字段索引获取扩展字段映射名称
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="exFieldIndex"></param>
        /// <returns></returns>
        private string GetMappingNameByActivityIDAndExFieldIndex(string activityId, int exFieldIndex)
        {
            var mappingInfo = Get<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And ExFieldIndex={1}", activityId, exFieldIndex));
            if (mappingInfo != null)
            {
                return mappingInfo.MappingName;
            }
            return "";

        }

        /// <summary>
        /// 根据映射字段名称设置客户Model信息
        /// </summary>
        /// <param name="mappingName">映射名称</param>
        /// <param name="value">值</param>
        /// <param name="model">客户实体</param>
        /// <returns></returns>
        private MemberInfo SetMemberInfoModelByMappingName(string mappingName, string value, MemberInfo model)
        {

            if (mappingName.Equals("微博ID"))
            {
                model.WeiboID = value;
            }
            if (mappingName.Equals("微博昵称"))
            {
                model.WeiboScreenName = value;
            }
            if (mappingName.Equals("性别"))
            {
                model.Sex = value;
            }
            if (mappingName.Equals("生日"))
            {
                DateTime birthday;
                if (DateTime.TryParse(value, out birthday))
                {
                    model.Birthday = birthday;
                }

            }
            if (mappingName.Equals("电子邮箱"))
            {
                model.Email = value;
            }
            if (mappingName.Equals("QQ"))
            {
                model.QQ = value;
            }
            if (mappingName.Equals("固定电话"))
            {
                model.Tel = value;
            }
            if (mappingName.Equals("网址"))
            {
                model.Website = value;
            }
            if (mappingName.Equals("公司"))
            {
                model.Company = value;
            }
            if (mappingName.Equals("职位"))
            {
                model.Title = value;
            }
            if (mappingName.Equals("地址"))
            {
                model.Address = value;
            }
            if (mappingName.Equals("备注"))
            {
                model.Remark = value;
            }
            if (mappingName.Equals("微信OPenID"))
            {
                model.WeixinOpenID = value;
            }

            return model;


        }

        ///// <summary>
        ///// 根据映射字段名称设置报名Model信息
        ///// </summary>
        ///// <param name="MappingName">映射名称</param>
        ///// <param name="ExFieldIndex">扩展字段索引</param>
        ///// <param name="membermodel">客户信息</param>
        ///// <param name="activitydatamodel">报名信息</param>
        ///// <returns></returns>
        //private ActivityDataInfo SetActivityDataInfoModel(string MappingName, int ExFieldIndex, MemberInfo membermodel, ActivityDataInfo activitydatamodel)
        //{


        //    Type t = activitydatamodel.GetType();
        //    var p = t.GetProperties();
        //    if (MappingName.Equals("微博ID"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.WeiboID, null);

        //    }
        //    if (MappingName.Equals("微博昵称"))
        //    {

        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.WeiboScreenName, null);
        //    }
        //    if (MappingName.Equals("性别"))
        //    {

        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Sex, null);

        //    }
        //    if (MappingName.Equals("生日"))
        //    {
        //        if (membermodel.Birthday != null)
        //        {
        //            if (!membermodel.Birthday.ToString().Equals("1900-01-01 0:00:00"))
        //            {
        //                p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Birthday.ToString(), null);
        //            }
        //        }


        //    }
        //    if (MappingName.Equals("电子邮箱"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Email, null);

        //    }
        //    if (MappingName.Equals("QQ"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.QQ, null);
        //    }
        //    if (MappingName.Equals("固定电话"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Tel, null);
        //    }
        //    if (MappingName.Equals("网址"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Website, null);

        //    }
        //    if (MappingName.Equals("公司"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Company, null);
        //    }
        //    if (MappingName.Equals("职位"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Title, null);
        //    }
        //    if (MappingName.Equals("地址"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Address, null);
        //    }
        //    if (MappingName.Equals("备注"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.Remark, null);

        //    }
        //    if (MappingName.Equals("微信OPenID"))
        //    {
        //        p[ExFieldIndex + 3].SetValue(activitydatamodel, membermodel.WeixinOpenID, null);

        //    }

        //    return activitydatamodel;


        //}

        ///// <summary>
        ///// 获取参数
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //private string GetPostParm(string parm)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(parm))
        //            return this.contextthis.Request[parm];
        //    }
        //    catch { return ""; }
        //    return "";
        //}

        ///// <summary>
        ///// 发送通知短信
        ///// </summary>
        ///// <param name="Phone">报名者的手机号</param>
        //private void SendSms(string Phone)
        //{


        //    #region 检查是否向报名者发送短信
        //    if (!string.IsNullOrWhiteSpace(Activity.ConfirmSMSContent))
        //    {


        //        //短信内容不为空，向用户发送通知短信
        //        string Parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membertrigger", ActivityUserInfo.UserID, ActivityUserInfo.Password, Phone, Activity.ConfirmSMSContent);

        //        var result = WebRequest.PostWebRequest(Parm, "http://www.jubit.org/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));



        //    }
        //    #endregion


        //    #region 检查是否向管理员发送能通知短信

        //    if (!string.IsNullOrWhiteSpace(Activity.AdminPhone) && (!string.IsNullOrWhiteSpace(Activity.AdminSMSContent)))
        //    {

        //        foreach (var item in Activity.AdminPhone.Split(','))
        //        {
        //            //向管理员发送通知短信
        //            string Parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membertrigger", ActivityUserInfo.UserID, ActivityUserInfo.Password, item, Activity.AdminSMSContent);

        //            var result = WebRequest.PostWebRequest(Parm, "http://www.jubit.org/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));
        //        }





        //    }
        //    #endregion


        //}

        /// <summary>
        /// 判断活动是否属于指定用户
        /// </summary>
        /// <param name="emailID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckActivityIDAndUser(string activityId, string userId)
        {
            if (string.IsNullOrWhiteSpace(activityId) || string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            try
            {
                if (GetCount<Model.ActivityInfo>(string.Format(" ActivityID = '{0}' and UserID = '{1}' ", activityId, userId)) > 0)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 查询报名数据
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns></returns>
        public DataTable QueryActivityData(string activityId,int pageIndex=1,int pageSize=10)
        {

            DataTable dt = new DataTable();
            try
            {
                StringBuilder strSql = new StringBuilder();


                strSql.Append("SELECT ActivityData.Name as 姓名, ");
                strSql.Append("ActivityData.Phone as 手机号码");

                var activityFieldMappingInfo = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", activityId));//映射名称
                activityFieldMappingInfo = activityFieldMappingInfo.OrderBy(p => p.ExFieldIndex).ToList();

                for (int i = 0; i < activityFieldMappingInfo.Count; i++)
                {
                    strSql.AppendFormat(",ActivityData.K{0} as [{1}]", activityFieldMappingInfo[i].ExFieldIndex, activityFieldMappingInfo[i].MappingName);
                }
                strSql.AppendFormat(",ActivityData.InsertDate as [提交时间]");
                strSql.AppendFormat(",ActivityData.Remarks as [备注]");
                strSql.AppendFormat(",SignIn.SignInTime as [签到时间]");
                strSql.Append(" FROM ZCJ_ActivityDataInfo ActivityData ");
                strSql.Append(" left join ZCJ_WXSignInInfo SignIn on ActivityData.weixinopenid=SignIn.SignInOpenID AND ");
                strSql.AppendFormat(" SignIn.JuActivityID=(select JuActivityID from ZCJ_JuActivityInfo where SignUpActivityID=ActivityData.ActivityID)");
                strSql.AppendFormat(" WHERE ActivityData.ActivityID = '{0}' AND IsDelete = 0   order by UID DESC", activityId);

                dt = Query(strSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {

            }

            return dt;
        }




        /// <summary>
        /// 查询报名数据
        /// </summary>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        public DataTable QueryActivityData(string mid, string activityID)
        {
            DataTable dt = new DataTable();
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT adi.Name as 姓名, ");
                strSql.Append("adi.Phone as 手机号码 ");


                var activityFieldMappingInfo = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", activityID));//映射名称
                activityFieldMappingInfo = activityFieldMappingInfo.OrderBy(p => p.ExFieldIndex).ToList();

                for (int i = 0; i < activityFieldMappingInfo.Count; i++)
                {
                    strSql.AppendFormat(",adi.K{0} as [{1}]", activityFieldMappingInfo[i].ExFieldIndex, activityFieldMappingInfo[i].MappingName);
                }

                strSql.AppendFormat(",{0} as [{1}]", "ui.TrueName", "推广人姓名");
                strSql.AppendFormat(",{0} as [{1}]", "ui.Phone", "推广人手机");
                strSql.AppendFormat(",InsertDate as [提交时间]");

                strSql.Append(" FROM ZCJ_ActivityDataInfo as adi left join dbo.ZCJ_UserInfo as ui on adi.SpreadUserID =ui.UserID  ");

                strSql.AppendFormat(" WHERE adi.ActivityID = '{0}'  AND adi.IsDelete = 0  AND adi.MonitorPlanID={1}   order by ui.TrueName ,adi.InsertDate desc ", activityID, mid);

                dt = Query(strSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {

            }

            return dt;
        }


        /// <summary>
        /// 检查活动是否存在
        /// </summary>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        public bool IsExistActivity(string activityID)
        {
            var activityInfo = Get<ActivityInfo>(string.Format("ActivityID='{0}'", activityID));
            if (activityInfo == null)
            {
                return false;
            }
            else
            {
                return true;
            }



        }
        /// <summary>
        /// 获取活动信息
        /// </summary>
        /// <param name="activityID"></param>
        /// <returns></returns>
        public ActivityInfo GetActivityInfoByActivityID(string activityID)
        {

            return Get<ActivityInfo>(string.Format("ActivityID='{0}'", activityID));

        }
        /// <summary>
        /// 获取报名信息
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public ActivityDataInfo GetActivityDataInfo(string activityID, int uId)
        {
            return Get<ActivityDataInfo>(string.Format(" ActivityID='{0}' And UID = {1} ", activityID, uId));
        }

        /// <summary>
        /// 获取报名信息
        /// </summary>
        /// <param name="activityID">活动编号</param>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public ActivityDataInfo GetActivityDataInfo(string activityID, string userId)
        {
            return Get<ActivityDataInfo>(string.Format(" ActivityID='{0}' And UserId = '{1}' AND IsDelete <> 1 ", activityID, userId));
        }

        /// <summary>
        /// 获取报名信息
        /// </summary>
        /// <param name="activityID">活动编号</param>
        /// <param name="openId">用户名</param>
        /// <returns></returns>
        public ActivityDataInfo GetActivityDataInfoByOpenId(string activityID, string openId)
        {
            return Get<ActivityDataInfo>(string.Format(" ActivityID='{0}' And WeixinOpenID = '{1}' AND IsDelete <> 1 ", activityID, openId));
        }

        /// <summary>
        /// 设置排序字段
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="fieldSort"></param>
        /// <returns></returns>
        public bool SetFieldSort(string activityID, string fieldSort)
        {
            return Update(new ActivityInfo(), string.Format(" FieldSort = '{0}' ", fieldSort), string.Format(" ActivityID = '{0}' ", activityID)) > 0;
        }
        /// <summary>
        /// 获取活动报名人数
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        public int GetSignInCount(string activityid)
        {
            return GetCount<ActivityDataInfo>(string.Format(" ActivityID='{0}'  AND IsDelete = 0", activityid));
        }
        /// <summary>
        /// 获取报名数据列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="activityId"></param>
        /// <param name="keyWords"></param>
        /// <param name="linkName"></param>
        /// <param name="status"></param>
        /// <param name="source"></param>
        /// <param name="sort"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ActivityDataInfo> GetActivityDataInfoList(string activityId,string sort,out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder(string.Format("ActivityID='{0}' AND IsDelete = 0 ", activityId));
            totalCount = GetCount<ActivityDataInfo>(sbWhere.ToString());
            return GetList<ActivityDataInfo>(sbWhere.ToString());;
        }
    }
}
