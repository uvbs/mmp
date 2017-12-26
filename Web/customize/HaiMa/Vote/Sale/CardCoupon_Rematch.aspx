<%@ Page Title="" Language="C#" MasterPageFile="~/customize/HaiMa/Vote/Sale/Master.Master"
    AutoEventWireup="true" CodeBehind="CardCoupon_Rematch.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Vote.Sale.CardCoupon_Rematch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    海马真英雄-邀请函
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .col
        {
            padding: 0px;
        }
        select
        {
            border-radius: 2px;
            min-width: 200px;
            height: 30px;
        }
        .bottom
        {
            margin-top: 10px;
            margin-bottom: 20px;
           
        }
        .bottom div
        {
            color: White;
            text-align: left;
            font-size: 14px;
            line-height: 25px;
            margin-left: 5px;
        }
        

        body
        {
               font-family: "Microsoft YaHei" ! important; } 
            
            
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%ZentCloud.BLLJIMP.Model.ActivityDataInfo model = new ZentCloud.BLLJIMP.BLLActivity("").GetActivityDataInfo("447400", CurrentUserInfo.UserID);
      if (model == null)
      {

          Response.End();
      }
    %>
    <div class="wrapUserCenter mBottom48">
        <div class="header">
            <img src="images/yaoqing.jpg?v=1.1" />
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>姓名：</span>
                            <input type="text" value="<%=model.Name %>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>手机：</span>
                            <input type="text" value="<%=model.Phone %>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>身份证号码：</span>
                            <input type="text" value="<%=model.K1%>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>性别：</span>
                            <input type="text" value="<%=model.K2%>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>出发城市：</span>
                            <input type="text" value="<%=model.K10 %>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>

           <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>出发日期：</span>
                            <input type="text" value="2015年8月22日" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>

        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>返回城市：</span>
                            <input type="text" value="<%=model.K11 %>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>

           <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>返回日期：</span>
                            <input type="text" value="2015年8月26日" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>交通方式：</span>
                            <input type="text" value="<%=model.K12 %>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>T恤尺寸:</span>
                            <input type="text" value="<%=model.K9 %>" readonly="readonly">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="bottom">
            <div style="font-size: 16px; font-weight: bold;">
                【复赛信息复核提醒】
            </div>
            <div>
                1. 出发/返回日期为本次竞赛组委会指定,不予更改;
            </div>
            <div>
                2. 请核对以上信息，如有问题，请于8月7日10:00前反馈，逾期不予受理；
            </div>
            <div>
                3. 竞赛组委会紧急联系电话：刘书博13607697267，翟倩倩18116253177 。
            </div>
            <div >
                本次竞赛一切解释权归海马汽车销售有限公司所有。
            </div>
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "海马精英营销大赛邀请函!",
                desc: "海马精英营销大赛邀请函-第一批",
                link: '',
                imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
            })
        })
    </script>
</asp:Content>
