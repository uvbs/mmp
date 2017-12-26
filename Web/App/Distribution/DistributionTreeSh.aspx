<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/WebMainContent.Master"
    CodeBehind="DistributionTreeSh.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Distribution.DistributionTreeSh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2016122501" rel="stylesheet" />
    <link href="/App/Wap/css/tree.css?v=2016122501" rel="stylesheet" />
    <style type="text/css">
        a.l-btn span span.l-btn-text {
            display: inline-block;
            height: 16px;
            line-height: 16px;
            padding: 0px;
        }

        .tree li {
            padding: 10px 2px 0 40px;
            font-size: 14px;
        }
        .tree strong{
            color:#333333;
        }

            .tree li span {
                padding: 5px 8px;
                left: -15px;
                top: 5px;
            }
            .tree li a{
                position: relative;
                top: 5px;
            }
            .tree li::before, .tree li::after {
                left: 0px;
            }
            .tree li::after{
                top: 30px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div style="font-size: 12px;">
        当前位置：&nbsp;<span>会员排位图</span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="easyui-panel wrapComm">
        <div style="padding: 5px; height: auto">
            <div style="padding-bottom: 5px; border-bottom: solid #ddd 1px;">
                会员：<input id="txtMember" class="easyui-textbox" v-model="member" placeholder="手机/姓名" />
                <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" v-on:click="Search()">查询</a>
            <%if (canTeamExport)
              { %>
                <a id="btnExport" href="javascript:void(0);" class="easyui-linkbutton" v-on:click="SearchExport()">导出</a>
            <%} %>
            </div>
        </div>
        <div class="wrapContent">
            <div class="tree well divNone" v-bind:class="[base_user ?'divBlock':'']">
                <ul>
                    <li class="parent_li">
                        <span v-if="!base_user.issys">
                            <b v-text="base_user.phone"></b>(<strong v-bind:style="{'color':base_user.color}" v-text="base_user.name"></strong>)
                            <b v-text="base_user.member_lvname"></b>
                            <b v-text="base_user.status"></b>
                            <b v-text="base_user.regtime"></b>
                        </span>
                        <span v-if="base_user.issys">
                            <b>月供宝平台</b>
                        </span>
                        <a v-if="!base_user.issys" style="color: red;"  v-on:click="goChildren(base_user,true)">查上级</a>
                        <ul>
                            <li class="parent_li" v-for="li1 in childrens">
                                <span>
                                    <b v-text="li1.phone"></b>(<strong v-bind:style="{'color':li1.color}" v-text="li1.name"></strong>)
                                    <b v-text="li1.member_lvname"></b>
                                    <b v-text="li1.status"></b>
                                    <b v-text="li1.regtime"></b>
                                </span>
                                <a v-if="li1.childrens" style="color: blue;" v-on:click="goChildren(li1,false)">查下级</a>
                                <ul>
                                    <li class="parent_li" v-for="li2 in li1.childrens">
                                        <span>
                                            <b v-text="li2.phone"></b>(<strong v-bind:style="{'color':li2.color}" v-text="li2.name"></strong>)
                                            <b v-text="li2.member_lvname"></b>
                                            <b v-text="li2.status"></b>
                                            <b v-text="li2.regtime"></b>
                                        </span>
                                        <a v-if="li2.childrens" style="color: blue;" v-on:click="goChildren(li2,false)">查下级</a>
                                        <ul>
                                            <li class="parent_li" v-for="li3 in li2.childrens">
                                                <span>
                                                    <b v-text="li3.phone"></b>(<strong v-bind:style="{'color':li3.color}" v-text="li3.name"></strong>)
                                                    <b v-text="li3.member_lvname"></b>
                                                    <b v-text="li3.status"></b>
                                                    <b v-text="li3.regtime"></b>
                                                </span>
                                                <a v-if="li3.childrens" style="color: blue;" v-on:click="goChildren(li3,false)">查下级</a>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="exportDiv" style="width:0px; height:0px; position:absolute; top:-10px;">
        <iframe id="exportIframe" style="width:0px; height:0px; position:absolute; top:-10px;"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="//static-files.socialcrmyun.com/lib/vue/2.0/vue.min.js" type="text/javascript"></script>
    <script src="//static-files.socialcrmyun.com/lib/lodash/lodash.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var commVm = new Vue({
            el: '.wrapComm',
            data: {
                member: '<% = Request["member"] %>',
                max_level: 4,
                base_user: false,
                list: [],
                childrens: false,
                colors: {
                    c_0: '#333333',
                    c_10: '#708090',
                    c_20: '#6CA6CD',
                    c_21: '#6959CD',
                    c_30: '#66CDAA',
                    c_31: '#528B8B',
                    c_40: '#8B864E',
                    c_50: '#CD5555'
                }
            },
            methods: {
                init: function () {
                    this.GetChildrens();
                },
                GetChildrens: function () {
                    var _this = this;
                    $.ajax({
                        type: 'post',
                        url: '/serv/api/admin/distribution/DistributionTree.ashx',
                        data: {
                            member: $.trim(this.member),
                            max_level: this.max_level
                        },
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.status) {
                                _this.base_user = resp.result.base_user;
                                _this.base_user.color = _this.colors['c_'+_this.base_user.member_lv];
                                _this.list = resp.result.list;
                                if (_this.list.length > 0) {
                                    _this.childrens = [];
                                    _this.GetFormatData();
                                } else {
                                    _this.childrens = false;
                                }
                            } else {
                                alert(resp.msg);
                            }
                        },
                        error: function () {
                        }
                    });
                },
                GetFormatData: function (list) {
                    var _this = this;
                    if (_this.base_user) {
                        _this.childrens = _this.GetFormatChildData(_this.list, _this.base_user.id, 0);
                    }
                },
                GetFormatChildData: function (list, id, level) {
                    var _this = this;
                    level++;
                    if (level > _this.max_level) return false;
                    var childrens = _.where(list, { pid: id });
                    if (childrens.length == 0) return false;
                    for (var i = 0; i < childrens.length; i++) {
                        childrens[i].color = _this.colors['c_' + childrens[i].member_lv];
                        childrens[i].childrens = _this.GetFormatChildData(list, childrens[i].id, level);
                    }
                    return childrens;
                },
                Search: function () {
                    this.GetChildrens();
                },
                goChildren: function (li, isBase) {
                    if (isBase) {
                        window.location.href = '/App/Distribution/DistributionTreeSh.aspx?member=' + li.pphone;
                    } else {
                        window.location.href = '/App/Distribution/DistributionTreeSh.aspx?member=' + li.phone;
                    }
                },
                SearchExport: function () {
                    $.messager.progress();
                    $.ajax({
                        type: 'post',
                        url: '/serv/api/admin/distribution/DistributionTreeExport.ashx',
                        data: {
                            member: $.trim(this.member)
                        },
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.status) {
                                $('#exportIframe').attr('src', '/Serv/API/Common/ExportFromCache.ashx?cache=' + resp.result.cache);
                            } else {
                                alert('导出出错');
                            }
                        },
                        error: function () {
                            $.messager.progress('close');
                        }
                    });
                }
            }
        });
        $(function () {
            commVm.init();
            var _h = document.documentElement.clientHeight - 100;
            $('.wrapContent').height(_h);
        });
    </script>
</asp:Content>
