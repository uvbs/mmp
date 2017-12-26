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
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            Add.PostModel requestModel = new Add.PostModel();//订单模型
            try
            {
                requestModel = bllJuActivity.ConvertRequestToModel<Add.PostModel>(requestModel);
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
                apiResp.msg = "标题必填";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            JuActivityInfo pInfo = bllJuActivity.GetByKey<JuActivityInfo>("JuActivityID", requestModel.id.ToString(), true);
            pInfo = bllJuActivity.GetByKey<JuActivityInfo>("JuActivityID", requestModel.id.ToString(), true);
            if (pInfo == null)
            {
                apiResp.msg = "网点未找到";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
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

            BLLTransaction tran = new BLLTransaction();
            bool result = bllJuActivity.Update(pInfo, tran);
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
            }
            else
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllJuActivity.ContextResponse(context, apiResp);
        }

    }
}