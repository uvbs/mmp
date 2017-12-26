using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Socket;

namespace ZentCloud.JubitIMP.Web.customize.Communal
{
    public partial class QRCodeLogin : System.Web.UI.Page
    {
        protected UserInfo curUser = null;
        protected bool status = false;
        protected string msg;
        protected string redis_key;
        protected int port = 80;
        ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            redis_key = this.Request["redis_key"];
            if (string.IsNullOrWhiteSpace(redis_key))
            {
                msg = "请通过微信扫二维码打开";
                return;
            }
            //this.Session["currWXOpenId"] = "o99IZtyEBvRNSv54Nt-AzpoqI2Kk";
            if (this.Session["currWXOpenId"] == null)
            {
                msg = "请用微信打开";
                return;
            }
            string opendId = this.Session["currWXOpenId"].ToString();
            curUser = bllUser.GetUserInfoByOpenId(opendId, bllUser.WebsiteOwner);
            if (curUser == null)
            {
                msg = "该微信未绑定账号";
                return;
            }
            int tport = ZentCloud.Common.ConfigHelper.GetConfigInt("WebSocketPort");
            if (tport != 0) port = tport;


            QRCodeLoginRedis qrReids = new QRCodeLoginRedis();
            try{
                qrReids = RedisHelper.RedisHelper.StringGet<QRCodeLoginRedis>(redis_key);
            }
            catch (Exception ex)
            {
                msg = "redis服务错误";
                return;
            }
            if (qrReids == null)
            {
                msg = "redis记录未找到";
                return;
            }
            qrReids.userID = curUser.UserID;
            RedisHelper.RedisHelper.StringSetSerialize(redis_key, qrReids,TimeSpan.FromHours(1));

            status = true;
            msg = "微信用户已找到，请点击登录";
        }
    }
}