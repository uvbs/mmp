<%@ Page Title="上传资质" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="UploadCredentials.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.UploadCredentials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <link href="/App/Wap/css/LoginBinding.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="logo-table">
                <div class="logo-cell">
                    <img class="logo" src="http://file-cdn.songhebao.com/www/jubit/jubit/image/20170309/B2D620E4BFD249AB962DFFD55BF1F72B.png" />
                </div>
            </div>
            <div class="weui-cells__title">资质照片上传：</div>
            <div class="weui-cells weui-cells_form mTop10">
                <div class="weui-cell">
                    <div class="weui-cell__bd">
                        <div class="weui-uploader">
                            <div class="weui-uploader__bd">
                                <ul class="weui-uploader__files" v-bind:class="[images.length>0?'divBlock':'']">
                                    <li class="weui-uploader__file"
                                        v-bind:class="[!item.ok ?'weui-uploader__file_status':'']"
                                        v-bind:style="{'background-image': item.url? 'url('+item.url+')':''}"
                                        v-for="(item,index) in images">
                                        <div class="weui-uploader__file-content" v-if="!item.ok">
                                            <i class="weui-icon-warn" v-if="item.error"></i>
                                            <span v-if="!item.error" v-text="item.progress"></span>
                                        </div>
                                        <input class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup" v-on:change="updateFile($event, index)" />
                                    </li>
                                </ul>
                                <div class="weui-uploader__input-box" v-if="images.length<5">
                                    <input id="uploaderInput" class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup && images.length<5" v-on:change="addFile($event)" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="comm defhide">
                <a href="javascript:void(0);" class="weui-btn btn-comm" v-on:click="change()">上传执照、身份证等信息</a>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var login_user = {
            ex1: '<%= curUser.Ex1 %>',
            ex2: '<%= curUser.Ex2 %>',
            ex3: '<%= curUser.Ex3 %>',
            ex4: '<%= curUser.Ex4 %>',
            ex5: '<%= curUser.Ex5 %>'
        };
    </script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/UploadCredentials.js?v=2017032802"></script>
</asp:Content>
