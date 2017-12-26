using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Outlets
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        /// <summary>
        /// 
        /// </summary>
        BLLWeixinCard bllWeixinCard = new BLLWeixinCard();
        public void ProcessRequest(HttpContext context)
        {
            PostModel requestModel = new PostModel();//订单模型
            try
            {
                requestModel = bllJuActivity.ConvertRequestToModel<PostModel>(requestModel);
            }
            catch (Exception ex)
            {
                apiResp.msg = "提交格式错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            //数据检查
            if (string.IsNullOrEmpty(requestModel.title))
            {
                apiResp.msg = "名称必填";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            //if (string.IsNullOrEmpty(requestModel.city))
            //{
            //    apiResp.msg = "城市必选";
            //    apiResp.code = (int)APIErrCode.OperateFail;
            //    bllJuActivity.ContextResponse(context, apiResp);
            //    return;
            //}
            //if (string.IsNullOrEmpty(requestModel.district))
            //{
            //    apiResp.msg = "区域必选";
            //    apiResp.code = (int)APIErrCode.OperateFail;
            //    bllJuActivity.ContextResponse(context, apiResp);
            //    return;
            //}
            if (string.IsNullOrEmpty(requestModel.address))
            {
                apiResp.msg = "地址必填";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.k4))
            {
                apiResp.msg = "电话必填";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.longitude))
            {
                apiResp.msg = "请选择坐标";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.latitude))
            {
                apiResp.msg = "请选择坐标";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            JuActivityInfo pInfo = new JuActivityInfo();
            pInfo.JuActivityID = int.Parse(bllJuActivity.GetGUID(BLLJIMP.TransacType.ActivityAdd));
            pInfo.ActivityName = requestModel.title;
            pInfo.ActivityAddress = requestModel.address;
            pInfo.ServerTimeMsg = requestModel.server_time;
            pInfo.ServicesMsg = requestModel.server_msg;
            pInfo.ThumbnailsPath = requestModel.img;
            pInfo.ArticleType = "Outlets";
            pInfo.CategoryId = requestModel.cate_id;
            pInfo.Province = requestModel.province;
            pInfo.ProvinceCode = requestModel.province_code;
            pInfo.City = requestModel.city;
            pInfo.CityCode = requestModel.city_code;
            pInfo.District = requestModel.district;
            pInfo.DistrictCode = requestModel.district_code;
            pInfo.K1 = requestModel.k1;
            pInfo.K4 = requestModel.k4; 
            pInfo.K5 = requestModel.k5;
            pInfo.Tags = requestModel.tags;
            pInfo.Sort = requestModel.sort;
            pInfo.UserLongitude = requestModel.longitude;
            pInfo.UserLatitude = requestModel.latitude;
            pInfo.IsHide = 0;
            pInfo.CreateDate = DateTime.Now;
            pInfo.LastUpdateDate = DateTime.Now;
            pInfo.WebsiteOwner = bllJuActivity.WebsiteOwner;
            pInfo.UserID = currentUserInfo.UserID;
            List<string> tagList = null;
            if (!string.IsNullOrWhiteSpace(pInfo.Tags)) tagList = pInfo.Tags.Split(',').ToList();

            bool result = false;
            BLLTransaction tran = new BLLTransaction();
            result = bllJuActivity.Add(pInfo, tran);
            if (!result)
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            result = bllJuActivity.SetJuActivityContentTags(pInfo.JuActivityID, tagList);
            if (result)
            {
                tran.Commit();
                apiResp.status = true;
                apiResp.msg = "提交完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
                string polId="";
                ZentCloud.BLLJIMP.Model.Weixin.WeixinStore model=new BLLJIMP.Model.Weixin.WeixinStore();
                model.business_name = pInfo.ActivityName;
                model.branch_name =pInfo.ActivityName;
                model.province = pInfo.Province;
                model.city = pInfo.City;
                model.district = pInfo.District;
                model.address =pInfo.ActivityAddress;
                model.telephone = pInfo.K4;
                model.categories = new List<string>();
                model.categories.Add("购物,综合商场");
                model.offset_type =1;
                model.longitude =double.Parse(pInfo.UserLongitude);
                model.latitude = double.Parse(pInfo.UserLatitude);
                
                string msg = "";
                if (bllWeixinCard.CreateStore(model, out polId,out msg))
                {
                    bllJuActivity.Update(pInfo, string.Format("K30='{0}'",polId), string.Format("JuActivityID={0}", pInfo.JuActivityID));


                }
                else
                {
                    apiResp.msg = msg;
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllJuActivity.ContextResponse(context, apiResp);
                    return;
                }
            }
            else
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllJuActivity.ContextResponse(context, apiResp);
        }

        /// <summary>
        /// 模型
        /// </summary>
        public class PostModel
        {
            /// <summary>
            /// 编号
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 地址
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// 照片
            /// </summary>
            public string img { get; set; }
            /// <summary>
            /// 服务时间
            /// </summary>
            public string server_time { get; set; }
            /// <summary>
            /// 主办业务
            /// </summary>
            public string server_msg { get; set; }
            /// <summary>
            /// 分类
            /// </summary>
            public string cate_id { get; set; }
            /// <summary>
            /// 省份
            /// </summary>
            public string province_code { get; set; }
            public string province { get; set; }
            /// <summary>
            /// 城市
            /// </summary>
            public string city_code { get; set; }
            public string city{ get; set; }
            /// <summary>
            /// 地区
            /// </summary>
            public string district_code { get; set; }
            public string district { get; set; }
            public string k1 { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            public string tags { get; set; }
            /// <summary>
            /// 经度
            /// </summary>
            public string longitude { get; set; }
            /// <summary>
            /// 纬度
            /// </summary>
            public string latitude { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int sort { get; set; }
            /// <summary>
            /// 电话
            /// </summary>
            public string k4 { get; set; }
            /// <summary>
            /// 供应商Id
            /// </summary>
            public string k5 { get; set; }
        }
    }
}