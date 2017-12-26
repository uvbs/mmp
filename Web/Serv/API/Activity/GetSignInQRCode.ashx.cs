﻿using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// 生成签到二维码
    /// </summary>
    public class GetSignInQRCode : BaseHandlerNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected UserInfo CurrentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            Image image = new Bitmap(10, 10);
            CurrentUserInfo = bll.GetCurrentUserInfo();
            if (CurrentUserInfo == null)
            {
                image = ImageHandler.CreateErrorStringImage("您还未登录", 300, 300, Color.Red);
                ContextResponse(context, image);
                return;
            }
            int activity_id = Convert.ToInt32(context.Request["activity_id"]);
            
            JuActivityInfo juAct = bll.GetJuActivity(activity_id);

            if (juAct == null || string.IsNullOrWhiteSpace(juAct.SignUpActivityID))
            {
                image = ImageHandler.CreateErrorStringImage("未找到约会信息", 300, 300, Color.Red);
                ContextResponse(context, image);
                return;
            } 

            ActivityDataInfo signData = bll.GetActivityDataByUserId(juAct.SignUpActivityID, null, CurrentUserInfo.UserID, bll.WebsiteOwner,null);
            if (signData == null || signData.Status != 1)
            {
                image = ImageHandler.CreateErrorStringImage("未找到签到信息", 300, 300, Color.Red);
                ContextResponse(context, image);
                return;
            }

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            String data = juAct.JuActivityID + "|" + signData.UID;
            image = qrCodeEncoder.Encode(data);

            ContextResponse(context, image);
        }
        private void ContextResponse(HttpContext context, Image image)
        {
            MemoryStream memStream = new MemoryStream();
            image.Save(memStream, ImageFormat.Png);
            context.Response.ClearContent();
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(memStream.ToArray());
        }
    }
}