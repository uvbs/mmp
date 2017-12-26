using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP;
using ZentCloud.JubitIMP.Web.Handler;

namespace ZentCloud.JubitIMP.Web
{
    public partial class QLogin : System.Web.UI.Page
    {
        BLLUser userBll;
        AshxResponse resp = new AshxResponse();

        protected void Page_Load(object sender, EventArgs e)
        {
            var userid = string.Format("WXUser_yike_{0}", Guid.NewGuid().ToString());
            this.userBll = new BLLUser("");

            if (!IsPostBack)
            {


                //检查是不是手机登录，如果是手机登录则直接跳转到手机中心页面
                if (Request.Browser.Platform == null ? false : Request.Browser.Platform.ToLower().StartsWith("win"))
                {
                    this.TimerCheck.Enabled = true;
                }
                else
                {
                    this.TimerCheck.Enabled = true;
                    //this.TimerCheck.Enabled = false;
                    //手机进入
                    //Response.Redirect("/App/Cation/Wap/UserHub.aspx");
                   // return;
                }

                //电脑登录-创建登录凭据-监测登录凭据使用情况
                CreateCreateQRcodeLoginTiket();
            }

        }


        protected void TimerCheck_Tick(object sender, EventArgs e)
        {
            try
            {
                ChcekQRcodeLoginTiket();
                if (resp.Status == 1)
                {
                   
                    if (Request["redirecturl"]!=null)
                    {
                        Response.Redirect(Request["redirecturl"]);
                        return;
                    }
                    else
                    {
                        Response.Redirect("/Index.aspx");
                    }
                   
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 构造登录凭据
        /// </summary>
        private void CreateCreateQRcodeLoginTiket()
        {
            //创建登录凭证
            string tiketValue = Guid.NewGuid().ToString();
            Common.DataCache.SetCache(tiketValue, tiketValue, DateTime.MaxValue, new TimeSpan(0, 0, 7200));
           
            //构造手机登录地址
            StringBuilder strWapUrl = new StringBuilder();
            strWapUrl.AppendFormat("http://{0}/Handler/QLoginV1.ashx?tiket={1}",
                    Request.Url.Host,
                    this.userBll.TransmitStringEnCode(tiketValue)
                );
            //生成二维码
            this.imgQrCode.Src = "/Handler/ImgHandler.ashx?v=" + strWapUrl;

            this.ViewState["tiket"] = tiketValue;
        }

        /// <summary>
        /// 检查登录凭据
        /// </summary>
        private void ChcekQRcodeLoginTiket()
        {
            string tiketKey = this.ViewState["tiket"].ToString();

            if (string.IsNullOrWhiteSpace(tiketKey))
            {
                resp.Status = -1;
                resp.Msg = "登录凭据不能为空!";
                return;
            }

            
            if (Common.DataCache.GetCache(tiketKey) == null)
            {
                resp.Status = -1;
                resp.Msg = "登录凭据不存在!";
                return;
            }


            string tiketValue = Common.DataCache.GetCache(tiketKey).ToString();
            resp.Msg = "tiketValue:" + tiketValue;

            if (string.IsNullOrWhiteSpace(tiketValue))
            {
                resp.Status = -1;
                resp.Msg = "登录凭据不存在!";
                return;
            }

            if (tiketValue.EndsWith("-login"))
            {
                resp.Status = 1;
                resp.Msg = "登录成功!";
                //获取登录用户，并设置登录状态
                string userIdCache = Common.DataCache.GetCache(tiketKey + "-user").ToString();
                BLLJIMP.Model.UserInfo loginUser = this.userBll.GetUserInfo(userIdCache);
                Session[ZentCloud.Common.SessionKey.LoginStatu] = 1;
                Session[ZentCloud.Common.SessionKey.UserID] = loginUser.UserID;
                Session[ZentCloud.Common.SessionKey.UserType] = loginUser.UserType;
                //this.userBll.AddLoginLogs(loginUser.UserID);
                //销毁登录凭据及相关数据
                //Common.DataCache.ClearCache(tiketKey);
                //Common.DataCache.ClearCache(tiketKey + "-user");
                return;
            }

            resp.Status = 0;

        }

    }
}