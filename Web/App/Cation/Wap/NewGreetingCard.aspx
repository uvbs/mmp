<%@ Page Title="创建贺卡" Language="C#" MasterPageFile="~/Master/WapUser.Master" AutoEventWireup="true" CodeBehind="NewGreetingCard.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.NewGreetingCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/css/umeditor/themes/default/umeditor.css" rel="stylesheet"  type="text/css"/>
     <style type="text/css">
        .cntItem
        {
            width: 220px;
        }
        
        #wrapper
        {
           position: relative;
            z-index: 1;
            height: 110px;
            width: 100%;
            background: #ccc;
            overflow: hidden;
            -ms-touch-action: none;
        }
        
        #scroller
        {
            position: absolute;
            z-index: 1;
            -webkit-tap-highlight-color: rgba(0,0,0,0);
            width: 2200px;
            height: 110px;
            -webkit-transform: translateZ(0);
            -moz-transform: translateZ(0);
            -ms-transform: translateZ(0);
            -o-transform: translateZ(0);
            transform: translateZ(0);
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            -webkit-text-size-adjust: none;
            -moz-text-size-adjust: none;
            -ms-text-size-adjust: none;
            -o-text-size-adjust: none;
            text-size-adjust: none;
        }
        
        #scroller ul
        {
            list-style: none;
            width: 100%;
            padding: 0;
            margin: 0;
        }
        
        #scroller li
        {
            width: 140px;
            height: 110px;
            float: left;
            border-right: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            background-color: #fafafa;
            font-size: 14px;
            overflow: hidden;
            text-align: center;
        }
        
        .item
        {
            /*float: left;*/
            position: relative;
            display: inline-block;
            margin: 10px 2.5%;
            width: 120px;
            border-radius: 3px;
            -webkit-box-shadow: 1px 1px 6px #888;
            box-shadow: 1px 1px 6px #888;
            background: #fff;
        }
        
        .item-detail
        {
            display: block;
            float: right;
            width: 15%;
            margin: 0;
            padding: 5px 2.5%;
            color: green;
            line-height: 1.7;
            font-size: 15px;
            text-align: right;
            border: 0;
        }
        
        .item.large
        {
            width: 95%;
        }
        .select-shadow.large
        {
            height: 105px;
            padding-top: 75px;
        }
        .item-title
        {
            overflow: hidden;
            float: left;
            padding: 5px 10px;
            text-align: left;
            font-size: 12px;
            white-space: nowrap;
        }
        .item-image
        {
        }
        .single-item-info
        {
            text-align: left;
        }
        .select-shadow
        {
            display: none;
            position: absolute;
            z-index: 0;
            height: 50px;
            width: 100%;
            top: 25px;
            padding-top: 10px;
            background: rgba(0,0,0,0.5);
            text-align: center;
            color: #fff;
            font-size: 16px;
            cursor: pointer;
        }
        .itemLit
        {
            /*     margin-bottom: 6px;     width: 90px;     height: 26px;*/
            margin-bottom: 6px;
            padding: 1px;
            border: 1px solid #c8dcc0;
            background: #fff;
            position: relative;
            overflow: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div data-role="page" id="page-title" data-theme="b">
        <div data-role="header" data-theme="f" data-tap-toggle="false" style="" id="divTop">
            <a href="/App/Cation/Wap/MyGreetingCard.aspx" data-role="button"  data-icon="arrow-l" data-ajax="false">返回</a>
            <h1>
                新建贺卡
            </h1>
        </div>
        <label for="txtTitle">
            贺卡模板:(左右滑动选择)</label>
        <div id="wrapper">
            <div id="scroller">
                <ul>
                    <%
                        List<Dictionary<string, string>> tpList = new List<Dictionary<string, string>>();
                        Dictionary<string, string> tp1 = new Dictionary<string, string>();
                        tp1.Add("pid", "1");
                        tp1.Add("title", "猴年大吉");
                        tp1.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/1/top.jpg");
                        Dictionary<string, string> tp2 = new Dictionary<string, string>();
                        tp2.Add("pid", "2");
                        tp2.Add("title", "闹新年 贺新春");
                        tp2.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/2/top.jpg");
                        Dictionary<string, string> tp3 = new Dictionary<string, string>();
                        tp3.Add("pid", "3");
                        tp3.Add("title", "招祥纳福");
                        tp3.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/3/top.jpg");
                        Dictionary<string, string> tp4 = new Dictionary<string, string>();
                        tp4.Add("pid", "4");
                        tp4.Add("title", "猴年吉祥");
                        tp4.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/4/top.jpg");
                        Dictionary<string, string> tp5 = new Dictionary<string, string>();
                        tp5.Add("pid", "5");
                        tp5.Add("title", "吉祥如意");
                        tp5.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/5/top.jpg");
                        
                        Dictionary<string, string> tp6 = new Dictionary<string, string>();
                        tp6.Add("pid", "6");
                        tp6.Add("title", "HAPPY NEW YEAR");
                        tp6.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/6/top.jpg");
                        Dictionary<string, string> tp7 = new Dictionary<string, string>();
                        tp7.Add("pid", "7");
                        tp7.Add("title", "金猴贺岁");
                        tp7.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/7/top.jpg");
                        Dictionary<string, string> tp8 = new Dictionary<string, string>();
                        tp8.Add("pid", "8");
                        tp8.Add("title", "2016");
                        tp8.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/8/top.jpg");
                        Dictionary<string, string> tp9 = new Dictionary<string, string>();
                        tp9.Add("pid", "9");
                        tp9.Add("title", "猴年纳福");
                        tp9.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/9/top.jpg");

                        Dictionary<string, string> tp10 = new Dictionary<string, string>();
                        tp10.Add("pid", "10");
                        tp10.Add("title", "新年快乐");
                        tp10.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/10/topmain.jpg");


                        Dictionary<string, string> tp11 = new Dictionary<string, string>();
                        tp11.Add("pid", "11");
                        tp11.Add("title", "猴年大吉");
                        tp11.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/11/top.jpg");

                        Dictionary<string, string> tp12 = new Dictionary<string, string>();
                        tp12.Add("pid", "12");
                        tp12.Add("title", "生意兴隆");
                        tp12.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/12/top.jpg");


                        //Dictionary<string, string> tp13 = new Dictionary<string, string>();
                        //tp13.Add("pid", "13");
                        //tp13.Add("title", "猴年大吉");
                        //tp13.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/13/top.jpg");


                        //Dictionary<string, string> tp14 = new Dictionary<string, string>();
                        //tp14.Add("pid", "14");
                        //tp14.Add("title", "猴到成功");
                        //tp14.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/14/top.jpg");

                        //Dictionary<string, string> tp15 = new Dictionary<string, string>();
                        //tp15.Add("pid", "15");
                        //tp15.Add("title", "Happy New Year");
                        //tp15.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/15/top.jpg");

                        //Dictionary<string, string> tp16 = new Dictionary<string, string>();
                        //tp16.Add("pid", "16");
                        //tp16.Add("title", "猴到成功");
                        //tp16.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/16/top.jpg");


                        //Dictionary<string, string> tp17 = new Dictionary<string, string>();
                        //tp17.Add("pid", "17");
                        //tp17.Add("title", "2014快乐");
                        //tp17.Add("img", "/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/17/top.jpg");

                        tpList.Add(tp1);
                        tpList.Add(tp2);
                        tpList.Add(tp3);
                        tpList.Add(tp4);
                        tpList.Add(tp5);
                        tpList.Add(tp6);
                        tpList.Add(tp7);
                        tpList.Add(tp8);
                        tpList.Add(tp9);
                        tpList.Add(tp10);
                        tpList.Add(tp11);
                        tpList.Add(tp12);
                        StringBuilder strHtml = new StringBuilder();
                        StringBuilder strGetHBjs = new StringBuilder();
                        strGetHBjs.AppendLine("<script>function GetRandomHb() {");
                        strGetHBjs.AppendLine("switch (selectTpID) {");
                        
                        foreach (var item in tpList)
                        {
                            strHtml.AppendLine("<li>");
                            strHtml.AppendFormat("<div class=\"item\" pid=\"{0}\">",item["pid"]);
                            
                            strHtml.AppendLine("<div class=\"item-title\">");
                            strHtml.Append(item["title"]);
                            strHtml.AppendLine("</div>");

                            strHtml.AppendLine("<div class=\"item-image\">");
                            strHtml.AppendFormat(" <img src=\"{0}\" alt=\"item image\" style=\"width: 120px; height: 60px; margin-top: 0px;\" />", item["img"]);
                            strHtml.AppendLine("</div>");

                            strHtml.AppendLine("<div class=\"select-shadow\" style=\"display: none;\">");
                            strHtml.AppendLine("<div><img src=\"/img/check.png\" alt=\"selected\" /><span>已选</span></div>");
                            strHtml.AppendLine("</div>");
                            strHtml.AppendLine("</div>");
                            strHtml.AppendLine("</li>");

                            strGetHBjs.AppendFormat("case \"{0}\":",item["pid"]);
                            strGetHBjs.AppendFormat("return \"{0}\"; ",item["img"]);
                        }

                        strGetHBjs.AppendLine("default:");
                        strGetHBjs.AppendLine("return \"/img/Weixin/ArticleTemplate/GreetingCard_HappyNewYear/1/top.jpg\";");
                        strGetHBjs.AppendLine("}");
                        
                        strGetHBjs.AppendLine("}</script>");
                        
                        Response.Write(strHtml.ToString());
                        Response.Write(strGetHBjs.ToString());
                    %>
                 
                </ul>
            </div>
        </div>
        <label for="txtTitle">
            贺卡标题:</label>
        <input type="text" id="txtTitle" value="在此恭祝您" placeholder="请输入贺卡标题" />
        <label for="txtContent" style="display:inline;">
            您的祝福语:</label>(<a id="btnSelectCnt" href="javascript:;">选择祝福语模板</a>)
        <textarea cols="40" rows="8" style="height: 160px" id="txtContent" placeholder="请输入您的祝福语"></textarea>
                <label for="txtActivityLecturer">
            您的签名:</label>
        <input type="text" id="txtActivityLecturer" value="<%=WxNickName%>" placeholder="请输入您的签名" />
        <a href="#" data-role="button" data-inline="false" data-theme="f" id="btnSave" data-ajax="false"
            data-mini="false" style="margin-bottom:10px;">生成贺卡</a>
        <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <label id="lbDlgMsg">
            </label>
        </div>
        <div data-role="popup" id="dlgSelectCnt" style="padding: 20px; background-color: #F0EFEF;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <table>
                <tr>
                    <td>
                        <%
                            List<string> cntList = new List<string>() { 
                                    "诚心酝酿了周，真心润色了天，在除夕钟声敲响的时候这条祝福终于大功告成：猴年到，祝你合家幸福团圆，快乐永远!",
                                    "酒，越酿越醇厚;茶，越久越清香;朋友，越走越真诚;祝福，越早越真挚。猴年到来，提前拜年，祝你度过一个快乐而开心的新年!",
                                    "新年已来到，向你问个好。开心无烦恼，好运跟着跑。家人共团聚，天伦乐逍遥。朋友相扶持，心情不寂辽。事业风水顺，金银撑荷包。好运从天降生活步步高。",
                                    "祝您在新的一年里：一家和和睦睦，一年开开心心;一生快快乐乐，一世平平安安;天天精神百倍，月月喜气洋洋;年年财源广进，岁岁平安祥和!新年快乐!",
                                    "祝新年快乐，前程似锦，吉星高照，财运亨通，合家欢乐，飞黄腾达，福如东海，寿比南山！酒越久越醇，朋友相交越久越真；水越流越清，世间沧桑越流越淡。祝猴年快乐，时时好心情！",
                                    "祝你新年很灿烂，牛气哄哄冲霄汉，祝你明年业务多，好运连连一火车，祝你工作小轻松，玩玩闹闹很成功，祝你身体特别好，吃嘛嘛香没烦恼。",
                                    "值此2016年新春即将来临之际，请允许我代表×××，并以我个人的名义，向所有关心、支持、参与×××建设的各位领导、各界朋友致以衷心的感谢；向全体员工及家属致以诚挚的问候和衷心的祝福。祝大家新春愉快，身体健康，合家幸福！",
                                    "迎接除夕，惊喜无限；福星高照，福满家园；禄星进门，加爵升官；寿星贺春，寿比南山；喜神报喜，好运无限；携手众仙，共迎新年；祝你新春，吉祥美满！",
                                    "新的1年开始，祝好事接2连3，心情4季如春，生活5颜6色，7彩缤纷，偶尔8点小财，烦恼抛到9霄云外!请接受我10心10意的祝福。祝新春快乐!",
                                    "想念的话，说也说不完；关怀的心，永远不改变；真挚友谊，永远不会忘，愿我的祝福将你围绕，猴年快乐，万事如意！",
                                    "朋友总是心连心，知心朋友值千金；灯光之下思贤友，小小贺卡传佳音；望友见讯如见人，时刻勿忘朋友心。祝新春愉快！"
                                
                                };

                            foreach (var item in cntList)
                            {
                                Response.Write(string.Format("<div class=\"cntItem\">{0}</div><hr /><br />", item));
                            }
                                
                        %>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
    <script src="/Ju-Modules/iscroll/5/iscroll.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/JuActivity/JuActivityHandler.ashx";
        var openId = '<%=currUser.WXOpenId %>';
        var currAcvityID = 0;
        var currAction = 'add';
        var myScroll;
        var selectTpID = 1;
        $(function () {

            $('div[pid="1"]').find(".select-shadow").css({ display: 'block' });

            $('.item').live('click', function () {
                var pid = $(this).attr('pid');
                selectTpID = pid;
                ClearTpSelect();
                //勾选
                $('div[pid="' + pid + '"]').find(".select-shadow").css({ display: 'block' });
                //alert(selectTpID);
            });

            $('#btnSave').live('click', function () {
                try {
                    var model = {
                        IsSignUpJubit: 1,
                        ActivityName: $.trim($('#txtTitle').val()),
                        ActivityWebsite: "",
                        ActivityDescription: $.trim($('#txtContent').val()), //$('#txtContent').val(),
                        Action: currAction == 'add' ? 'AddJuActivity' : 'EditJuActivity',
                        JuActivityID: currAcvityID,
                        ThumbnailsPath: GetRandomHb(),
                        IsHide: 0,
                        IsByWebsiteContent: 0,
                        ArticleType: 'greetingcard',
                        ArticleTemplate: selectTpID,
                        IsSpread: 1,
                        ActivityLecturer: $.trim($('#txtActivityLecturer').val())

                    };
                    if (model.ActivityName == '') {
                        $('#lbDlgMsg').html('请输入贺卡标题');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    if (model.ActivityLecturer == '') {
                        $('#lbDlgMsg').html('请输入您的签名');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    if (model.ActivityDescription == '') {
                        $('#lbDlgMsg').html('请输入您的祝福语');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    if (model.ActivityDescription.length > 500) {
                        $('#lbDlgMsg').html('祝福语长度在500个字以内');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }


                    $.mobile.loading('show', { textVisible: true, text: '正在创建贺卡...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            try {
                                $.mobile.loading('hide');
                                var resp = $.parseJSON(result);
                                if (resp.Status == 1) {

                                    ClearLocalData();

                                    $('#lbDlgMsg').html("已成功生成贺卡!<br />正在转到贺卡页面。。。");
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');


                                    setInterval(function () {
                                        window.location.href = '/' + resp.ExObj.JuActivityIDHex + '/share.chtml';
                                    }, 1000);

                                }
                                else {
                                    $('#lbDlgMsg').html(resp.Msg);
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');
                                }
                            } catch (e) {
                                alert(e);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }

            });


            myScroll = new IScroll('#wrapper', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });

            GetLocalData();

            try {
                setInterval(function () { SetLocalData() }, 500);
            } catch (e) {
                alert(e);
            }

            $('#btnSelectCnt').click(function () {
                $('#dlgSelectCnt').popup('open');
            });

            $('.cntItem').click(function () {
                $("#txtContent").val($.trim($(this).text()))
                $('#dlgSelectCnt').popup('close');
            });
        });

        function SetLocalData() {
            SetCookie('greetingCardCnt', $('#txtContent').val());
            SetCookie('greetingCardTitle', $('#txtTitle').val());
            SetCookie('greetingCardActivityLecturer', $('#txtActivityLecturer').val());
        }

        function GetLocalData() {

            var greetingCardCnt = getCookie('greetingCardCnt');
            var greetingCardTitle = getCookie('greetingCardTitle');
            var greetingCardActivityLecturer = getCookie('greetingCardActivityLecturer');

            $('#txtTitle').val(greetingCardTitle);
            if (greetingCardCnt != null) {
                $('#txtContent').val(greetingCardCnt);
            }
            else {
                $('#txtContent').val("新年已来到，向你问个好。开心无烦恼，好运跟着跑。家人共团聚，天伦乐逍遥。朋友相扶持，心情不寂辽。事业风水顺，金银撑荷包。好运从天降生活步步高。");
            }
        }

        function ClearLocalData() {
            //情空cookie数据
            SetCookie('greetingCardCnt', '');
            SetCookie('greetingCardTitle', '');
        }



        function ClearTpSelect() {
            $('.item').each(function () {
                $(this).find(".select-shadow").css({ display: 'none' });
            });
        }

    </script>

</asp:Content>
