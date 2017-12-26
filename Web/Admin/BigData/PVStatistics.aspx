<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PVStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.BigData.PVStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <link href="http://static-files.socialcrmyun.com/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/bootstrap/3.3.4/css/bootstrap.css" rel="stylesheet" />
  <style type="text/css">
        body > div:nth-child(1) {
            border:0;
        }
        .sort{
            display: none;
        }
      .button {
          display: inline-block;
          font-size: 12px;
          line-height: 0;
          min-height: 30px;
          height:30px;
          min-width: 95px;
          margin: 3px 5px 0px;
          padding: 0px 12px;
          border-radius: 4px;
      }
      .head {
        text-align:center;
        padding:20px;
      }
      .spadding{
          padding:10px;
      }
      .table{
          width:50% !important;
          min-width:60% !important;
      }
       #tb{
           margin-left:auto;
           margin-right:auto;
       }
       .pageSubTitle{
            padding: 10px 16px;
            background-color: #f9f8f8;
            margin: 0 16px;
            margin-bottom:20px;
        }
        .pageSubTitle .mainText{
            display: inline-block;
            margin: 0 12px 0 0;
            padding: 0 0 0 10px;
            border-left: 4px solid #f70;
            font-size: 14px;
            font-weight: bold;
            line-height: 20px;
        }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
   <p style="font-size:12px;">当前位置：&nbsp;大数据分析&nbsp;&gt;&nbsp;<span>访问统计页面</span></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
      <div style="    
        text-align: center;
        font-size: 20px;
        padding: 10px;
    "><span class="">访问统计</span></div>
    
    <div class="head">
           
        <div>
            <button class="button  button-calm" data-type="article">文章</button>
            <button class="button" data-type="activity">活动</button>
            <button class="button" data-type="product">商品</button>
             <button class="button" data-type="question">问卷</button>
            <button class="button" data-type="shake">摇一摇</button>
            <br />
            <button class="button" data-type="scratch">刮刮奖</button>
            <button class="button" data-type="wshow">微秀</button>
            <button class="button" data-type="questionnaireset">答题</button>
             <button class="button" data-type="thevote">选项投票</button>
            <button class="button" data-type="greetingcard">贺卡</button>
        </div>
     <div style="padding-top:10px;">
            <span class="spadding"><input type="radio" class="positionTop2" name="time" checked="checked" id="all"  value=""/>&nbsp;<label for="all">全部</label></span>
            <span class="spadding"><input type="radio" class="positionTop2" name="time" id="day"  value="day"/>&nbsp;<label for="day">昨天</label></span>
            <span class="spadding"><input type="radio" class="positionTop2" name="time"  id="week" value="week"/>&nbsp;<label for="week">近7天</label></span>
            <span class="spadding"><input type="radio" class="positionTop2" name="time" id="month" value="month" />&nbsp;<label for="month">近30天</label></span>
        </div>
    </div>
    
       <div class="pageSubTitle"><span class="mainText">访问分布图</span>

      </div>
      <div id="main" style="width:500px; height:400px;margin:0 auto;"></div>

     <div class="pageSubTitle"><span class="mainText">访问排行榜</span>
          
         </div>

    <div class="tb">
        <table class="table table-condensed" id="tb">
           <thead>
              <tr>
                  <th>排名</th>
                 <th>标题</th>
                 <th>访问量</th>
              </tr>
           </thead>
           <tbody>
          </tbody>
        </table>
    </div>

        <br /><br /><br />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script src="/Scripts/echarts/echarts.js"></script>
    <script src="/Scripts/echarts/china.js"></script>
    <script type="text/javascript">
        var url = "/serv/api/admin/BigData/pv/";
        var type = 'article';
        var myChart;
        function randomData() {
            return Math.round(Math.random() * 1000);
        }

         var option = {
            title: {
                text: '访问数据分布',
                left: 'center'
            },
            tooltip: {
                trigger: 'item'
            },
            visualMap: {
                min: 0,
                max: 100,
                left: 'left',
                top: 'bottom',
                text: ['高', '低'],           // 文本，默认为数值文本
                calculable: true,
                //inRange: {
                //    color: ['#6cd7d9', '#ff9900']
                //}
            },
            toolbox: {
                show: true,
                orient: 'vertical',
                left: 'right',
                top: 'center',
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            series: [
             
                {
                    name: '访问数量',
                    type: 'map',
                    mapType: 'china',
                    roam: false,
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: [
                    ]
                }
            ]
        };

         $(function () {
             $('#main').width(800);
             $('#main').height(600);
             myChart = echarts.init(document.getElementById('main'));

             //Chart
             GetChartData(type);
             //List
             GetEventDetaail(type);

             $('.button').click(function () {
                 $('.button-calm').removeClass('button-calm');
                 $(this).addClass('button-calm');
                 type = $(this).attr("data-type");
                 GetChartData(type);
                 GetEventDetaail(type);
             });


             $("input[name=time]").click(function () {

                 GetEventDetaail(type);
                 GetChartData(type);
             })

        });
        function GetEventDetaail(moduleType) {
            var data = {
                module_type: moduleType,
                time:$("input[name=time]:checked").val()
            };
            $.ajax({
                method: "POST",
                url: url + 'list.ashx',
                data: data,
                dataType: "json",
                success: function (resp) {
                    $("#tb tbody tr").remove();
                    for (var i = 0; i < resp.length; i++) {
                        var tr = $("<tr></tr>");
                        var td1 = $("<td style='width:25%'>" + (i + 1) + "</td>");
                        var td2;
                        //debugger;
                        if (type == 'article' || type == 'activity' || type == 'greetingcard') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/" + parseInt(resp[i].id).toString(16) + "/details.chtml'>" + resp[i].title + "</a></td>");
                        } else if (type == 'product') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/customize/shop/?v=1.0&ngroute=/productDetail/" + resp[i].id + "#/productDetail/" + resp[i].id + "'>"+resp[i].title+"</a></td>");
                        } else if (type == 'question') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/App/Questionnaire/Wap/Questionnaire.aspx?id="+resp[i].id+"'>" + resp[i].title + "</a></td>");
                        } else if (type == 'shake') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/customize/shake/?ngroute=/shake/" + resp[i].id + "#/shake/" + resp[i].id + "'>" + resp[i].title + "</a></td>");
                        } else if (type == 'scratch') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/App/Lottery/wap/ScratchV1.aspx?id=" + resp[i].id + "'>" + resp[i].title + "</a></td>");
                        } else if (type == 'wshow') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/App/WXShow/wap/WXWAPShowInfo.aspx?autoid="+resp[i].id+"'>" + resp[i].title + "</a></td>");
                        } else if (type == 'questionnaireset') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/customize/dati/index.aspx?id="+resp[i].id+"'>" + resp[i].title + "</a></td>");
                        } else if (type == 'thevote') {
                            td2 = $("<td style='width:50%'><a target='_blank' href='http://" + window.location.host + "/App/TheVote/wap/WxTheVoteInfo.aspx?autoid=" + resp[i].id + "'>" + resp[i].title + "</a></td>");
                        }
                        var td3 = $("<td style='width:25%'>" + resp[i].count + "</td>");
                        tr.append(td1).append(td2).append(td3).appendTo("#tb tbody");
                    }
                }
            });
        }
        function GetModuleTypeName(moduleType){
            var name = '';
            switch (moduleType) {
                case 'article':
                    name = '文章';
                    break;
                case 'activity':
                    name = '活动';
                    break;
                case 'product':
                    name = '商品';
                    break;
                case 'question':
                    name = '问卷';
                    break;
                case 'shake':
                    name = '摇一摇';
                    break;
                case 'scratch':
                    name = '刮刮奖';
                    break;
                case 'wshow':
                    name = '微秀';
                    break;
                case 'questionnaireset':
                    name = '答题';
                    break;
                case 'thevote':
                    name = '选项投票';
                    break;
                case 'greetingcard':
                    name = '贺卡';
                    break;
            }
            return name;
        }
        function GetChartData(moduleType) {
            //debugger;
            option.series[0].name = GetModuleTypeName(moduleType) + "访问量";
            var data = {
                module_type: moduleType,
                time:$("input[name=time]:checked").val()
            };
            layer.load();
            $.ajax({
                method: "POST",
                url: url + 'chart.ashx',
                data: data,
                dataType: "json",
                success: function (resp) {
                    layer.close(layer.load());
                    if (resp.status) {
                        option.series[0].data = resp.result.data;
                        option.visualMap.max = resp.result.max;
                    }
                    else {
                        option.series[0].data = [];
                        option.visualMap.max = 100;
                    }
                    //myChart = echarts.init(document.getElementById('main'));
                    myChart.clear();
                    myChart.setOption(option);
                }
            });
        }
     </script>
</asp:Content>
