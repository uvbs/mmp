<%@ Page Title="我的团队" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="MyTeam.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.MyTeam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <link href="/App/Wap/css/tree.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="tree well divNone" v-bind:class="[me && me.id ?'divBlock':'']">
                <ul>
                    <li class="parent_li">
                        <span>
                            <b v-text="me.phone"></b>(<strong v-text="me.name"></strong>)
                            <b v-text="me.member_lvname"></b>
                        </span>
                        <ul>
                        <li class="parent_li" v-for="li in childrens">
                            <span>
                                <b v-text="li.phone"></b>(<strong v-text="li.name"></strong>)
                                <b v-text="li.member_lvname"></b>
                            </span>
                            <a v-if="li.has_child" v-on:click="goChildren(li)">查下级</a>
                        </li>
                    </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/MyTeam.js?v=2017030101"></script>
</asp:Content>
