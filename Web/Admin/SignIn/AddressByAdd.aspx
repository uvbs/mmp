<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AddressByAdd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SignIn.AddressByAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        input {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        textarea {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        input[type=radio], input[type=checkbox] {
            position: relative;
            top: 8px;
            height: 23px;
            margin: 0px 5px 0px 5px !important;
        }

        .showSetField {
            padding: 0px 10px;
            height: 32px;
            line-height: 32px;
            margin-left: 10px;
        }

        .ActivityBox table {
            border-collapse: separate !important;
            border-spacing: 10px !important;
        }

        .combo-panel {
            padding: 2px !important;
        }

            .combo-panel table {
                border-collapse: separate !important;
            }

        #container {
            height: 360px;
            width: 900px;
            border: 1px solid gray;
        }

        #size {
            display: block;
            margin: 0.5rem auto 0 auto;
        }
    </style>
    <link href="/MainStyleV2/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>签到管理</span>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=action=="add"?"添加":"编辑" %>签到地点</span>
    <a href="AddressList.aspx" style="float: right; margin-right: 20px;" title="返回站点管理"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <fileset>
                        <legend>地图配置</legend>
                 <table width="100%" id="tbMain">
                       <tr>
                        <td style="width: 200px;" align="right" valign="middle">选择地点：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                             <div id="container"></div>
                        </td>
                    </tr>
                      <tr>
                        <td style="width: 200px;" align="right" valign="middle">搜索地址:：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                            <div id="r-result"><input type="text" id="suggestId" style="width:100%;" placeholder="请输入你要搜索的地址"  style="height:20px;"/></div>
                        </td>
                    </tr>
                     </table>
                     </fileset>

                <fileset>
                        <legend>签到配置</legend>
                <table width="100%" id="tbMain">
                     <tr>
                         <td colspan="2">
                             <div id="searchResultPanel" style="border: 1px solid #C0C0C0; width: 150px; height: auto; display: none;"></div>
                         </td>
                     </tr>
           
                    <tr>
                        <td style="width: 200px;" align="right" valign="middle">签到地址：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                            <input type="text" id="txtAddress" value="<%=model.Address %>" style="width:100%" placeholder="签到地址"/>
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 200px;" align="right" valign="middle">经度：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                            <input type="text" id="txtLongitude" readonly="readonly" value="<%=model.Longitude %>" style="width:100%" placeholder="经度"/>
                        </td>
                      <tr>
                        <td style="width: 200px;" align="right" valign="middle">纬度：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                            <input type="text" id="txtLatitude" readonly="readonly" style="width:100%" value="<%=model.Latitude %>" placeholder="纬度"/>
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 200px;" align="right" valign="middle">范围：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                            <input type="text" id="txtRange"  style="width:100%" value="<%=model.Range %>"  placeholder="范围"   />
                            单位:(米)
                        </td>
                    </tr>
                       <tr>
                        <td style="width: 200px;" align="right" valign="middle">签到成功跳转链接：
                        </td>
                        <td style="width: *;" align="left" valign="middle">
                            <input type="text" id="txtSignInSuccessUrl" value="<%=model.SignInSuccessUrl %>" style="width:100%" placeholder="签到成功跳转链接"/>
                        </td>
                    </tr>

                      </tr>
                 </table>
                </fileset>
       <fileset>
                        <legend>时间配置</legend>
                      <div class="fields">
                        <div class="field" data-field-index="0" data-field-id="0">
                        <div style="margin-left:135px;">
                           <strong style="font-size:18px;"> 时间段</strong>
                              
                        </div>
                         <table width="100%" id="tb">
                                   <tr>
                                    <td style="width: 200px;"  align="right" valign="middle">时间段名称：
                                    </td>
                                     <td>
                                         <input type="text" style="height:25px;" name="txtName" placeholder="时间段名称"/>
                                     </td>
                                    <td>开始时间</td>
                                    <td style="width: *;"  align="left" valign="middle">
                                        <input class="easyui-datetimebox" data-easyui-input   data-options="width:150,required:true,showSeconds:false" />
                                    </td>
                                        <td>结束时间</td>
                                    <td style="width: *;"  align="left" valign="middle">
                                        <input class="easyui-datetimebox" data-easyui-input  data-options="width:150,required:true,showSeconds:false"/>
                                    </td>
                                    <td>
                                        <img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption" />
                                    </td>
                                </tr>
                              </table>
                            <a style="margin-left:135px;" id="addTime" class="button button-rounded button-primary">添加时间段</a>
                        </div>
                       </div>

                    </fileset>
        <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 70px; line-height: 60px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 14px; left: 0;">
            <a href="javascript:;" style="width: 200px; font-weight: bold; text-decoration: none;" id="btnSave"
                class="button button-rounded button-primary">保存</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <%--<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=QfOTnQOrYQgnwWp6TrV2hg5q"></script>--%>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=224bd7222ce22c01673ff105ffb93fda"></script>
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/SignIn/Address/";
         $(function () {
            if ('<%=action=="update"%>') {
                GetSignInTimes();
            }


            $("#btnSave").live('click', function () {
                var times = [];
                var result = true;
                $("#tb tr").each(function () {
                    var td = $(this).find("input[name=txtName]").val();
                    var td1 = $($(this).find(".easyui-datetimebox").get(0)).datetimebox('getValue');
                    var td2 = $($(this).find(".easyui-datetimebox").get(1)).datetimebox('getValue');
                    if (td == '' && td1 == '' && td2 == '') {
                        return true;
                    }
                    if (td == '' || td1 == '' || td2 == '') {
                        result = false;
                        return false;
                    }
                    times.push({"name":td,"start":td1,"stop":td2});
                })
                if (!result) {
                    alert('请填写完整的时间段.');
                    return;
                }
               
                var dataModel =
                {
                    address: $("#txtAddress").val(),
                    longitude: $("#txtLongitude").val(),
                    latitude: $("#txtLatitude").val(),
                    range: $("#txtRange").val(),
                    id: '<%=model.AutoID%>',
                    successurl: $("#txtSignInSuccessUrl").val(),
                    signintime:times.length==0?"":JSON.stringify(times)
                };
                if (dataModel.address.trim() == "") {
                    Alert("请输入地址");
                    return;
                }
                if (dataModel.longitude.trim() == "") {
                    Alert("请输入经度");
                    return;
                }
                if (dataModel.latitude.trim() == "") {
                    Alert("请输入纬度");
                    return;
                }
                if(dataModel.range==""){
                    Alert("请输入范围");
                    return;
                }
                var submitUrl = "";
                if ("<%=action%>" == "add") {
                submitUrl = handlerUrl + "Add.ashx";
            }
            else {
                submitUrl = handlerUrl + "Update.ashx";
            }
            $.ajax({
                type: 'POST',
                url: submitUrl,
                data: dataModel,
                dataType: 'json',
                success: function (data) {
                    if (data.status) {
                        location.href = "AddressList.aspx";
                    } else {
                        Alert(data.msg);
                    }
                }
            });
            });





            //添加时间段
            $(".datebox :text").attr("readonly", "readonly");

            $("#addTime").live('click', function () {
                var str = new StringBuilder();
                str.AppendFormat('<tr>');
                str.AppendFormat('<td style="width: 200px;"  align="right" valign="middle">时间段名称：</td>');
                str.AppendFormat('<td><input type="text" style="height:25px;" name="txtName" placeholder="时间段名称"/></td>');
                str.AppendFormat('<td>开始时间</td>');
                str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input  class="easyui-datetimebox" data-options="width:150,required:true" /></td>');
                str.AppendFormat('<td>结束时间</td>');
                str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input  class="easyui-datetimebox" data-options="width:150,required:true"/></td>');
                str.AppendFormat('<td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption" /> </td>');
                str.AppendFormat('</tr>');
                $("#tb").append(str.ToString());
                $("#tb tr :last .easyui-datetimebox").datetimebox({
                    showSeconds: false
                });
                //只读
                $(".datebox :text").attr("readonly", "readonly");
            });
            //删除时间段
            $(".deleteoption").live('click', function () {
                $(this).parent().parent().remove();
            })


        });

        function GetSignInTimes() {
            var signinTimes = '<%=model.SignInTime%>';
            if (signinTimes == '') {
                return;
            }
            $("#tb tr").remove();
            var times = JSON.parse(signinTimes);
            for (var i = 0; i < times.length; i++) {
                var str = new StringBuilder();
                str.AppendFormat('<tr>');
                str.AppendFormat('<td style="width: 200px;"  align="right" valign="middle">时间段名称：</td>');
                str.AppendFormat('<td><input type="text" style="height:25px;" value="{0}" name="txtName" placeholder="时间段名称"/></td>',times[i].name);
                str.AppendFormat('<td>开始时间</td>');
                str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input value="{0}"  class="easyui-datetimebox" data-options="width:150,required:true" /></td>',times[i].start);
                str.AppendFormat('<td>结束时间</td>');
                str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input  class="easyui-datetimebox" value="{0}" data-options="width:150,required:true"/></td>',times[i].stop);
                str.AppendFormat('<td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption" /> </td>');
                str.AppendFormat('</tr>');
                $("#tb").append(str.ToString());
                $("#tb tr :last .easyui-datetimebox").datetimebox({
                    showSeconds: false
                });

            }
            
        }


    </script>

    <%--<script>

        // 百度地图API功能
        //var map = new BMap.Map("allmap");  
        //map.centerAndZoom(new BMap.Point(116.4035,39.915),8); 
        //setTimeout(function(){
        //    map.setZoom(14);   
        //}, 2000);  //2秒后放大到14级
        //map.enableScrollWheelZoom(true);


        var gc = new BMap.Geocoder();
        var map = new BMap.Map("baiduMap"); //创建地图对象
        var point = new BMap.Point(121.546457, 31.201715); //默认坐标为天安门
        var tag; //定义状态标记，使得同一时间只能创建一个点和圆
        map.centerAndZoom(point, 12); //暂时切换到默认坐标
        setTimeout(function () {
            map.setZoom(14);
        }, 2000);  //2秒后放大到14级
        map.enableScrollWheelZoom(true);

        pageInit();




            <%
        if (action == "update")
        {
            Response.Write(string.Format("SetPoint({0}, {1})", model.Longitude, model.Latitude));
        }
            %>


        function SetPoint(lng, lat) {

            point = new BMap.Point(lng, lat); //将坐标进行刷新
            var marker = new BMap.Marker(new BMap.Point(lng, lat)); // 创建点

            try {


                var circle = new BMap.Circle(point, 100, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 }); //创建圆
                if (!tag) { //如果之前没有点击过地图
                    map.addOverlay(marker); //增加点
                    map.addOverlay(circle); //增加圆
                    tag = true; //状态标记为已经点击过

                } else { //如果之前点击过地图
                    map.clearOverlays(); //清除之前的数据
                    map.addOverlay(marker); //增加点
                    map.addOverlay(circle); //增加圆


                }

                map.panTo(point);
                //map.centerAndZoom(point, 15); 

            } catch (e) {
                alert(e);
            }



        }



        function pageInit() {
            //基础数据
            //var map = new BMap.Map("baiduMap"); //创建地图对象
            //var point = new BMap.Point(116.331398, 39.897445); //默认坐标为天安门
            //var tag; //定义状态标记，使得同一时间只能创建一个点和圆
            //map.centerAndZoom(point, 12); //暂时切换到默认坐标

            //根据IP切换到当前城市
            //function switchMap(result) { //切换地图函数
            //    var cityName = result.name; //获取城市
            //    map.setCenter(cityName); //设置地图中心点
            //}

            //var myCity = new BMap.LocalCity(); //创建地图坐标对象
            //myCity.get(switchMap); //切换地图

            //鼠标点击效果
            map.addEventListener("click", function (data) { //定义点击事件与数据
                var size = document.getElementById('txtRange').value;
                if (!size) {
                    alert('请填写圆圈的范围');
                    return
                } else {
                    point = new BMap.Point(data.point.lng, data.point.lat); //将坐标进行刷新
                    var marker = new BMap.Marker(new BMap.Point(data.point.lng, data.point.lat)); // 创建点
                    var circle = new BMap.Circle(point, size, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 }); //创建圆
                    if (!tag) { //如果之前没有点击过地图
                        map.addOverlay(marker); //增加点
                        map.addOverlay(circle); //增加圆
                        tag = true; //状态标记为已经点击过
                    } else { //如果之前点击过地图
                        map.clearOverlays(); //清除之前的数据
                        map.addOverlay(marker); //增加点
                        map.addOverlay(circle); //增加圆
                    }
                    $("#txtLongitude").val(point.lng);
                    $("#txtLatitude").val(point.lat);
                    gc.getLocation(point, function (rs) {
                        var addComp = rs.addressComponents;
                        $("#txtAddress").val(addComp.province + addComp.city + addComp.district + addComp.street + addComp.streetNumber);
                    })
                }
            });
        }





        ///////搜索////////////////////////////////////////

        function G(id) {
            return document.getElementById(id);
        }

        //var map=new BMap.Map("l-map");
        //map.centerAndZoom("北京",12);

        var ac = new BMap.Autocomplete(
            {
                "input": "suggestId",
                "location": map
            });

        ac.addEventListener("onhighlight", function (e) {
            var str = "";
            var _value = e.fromitem.value;
            var value = "";
            if (e.fromitem.index > -1) {
                value = _value.province + _value.city + _value.district + _value.street + _value.business;
            }
            str = "FromItem<br />index=" + e.fromitem.index + "<br />value=" + value;

            value = "";
            G("searchResultPanel").innerHTML = str;
        });


        var myValue;
        ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
            var _value = e.item.value;
            myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
            G("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;

            setPlace();
        });

        function setPlace() {
            map.clearOverlays();    //清除地图上所有覆盖物
            function myFun() {
                var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
                map.centerAndZoom(pp, 18);
                map.addOverlay(new BMap.Marker(pp));    //添加标注
            }
            var local = new BMap.LocalSearch(map, { //智能搜索
                onSearchComplete: myFun
            });
            local.search(myValue);
        }
    </script>--%>
    <script>
        //显示地图
        var map = new AMap.Map('container', {
            resizeEnable: true,
            zoom: 15,
            center: ["121.472644", "31.231706"]
        });
        var marker = new AMap.Marker({
            position: ["121.472644", "31.231706"],
            map: map
        });

        var circle = new AMap.Circle({
            center: new AMap.LngLat("121.472644", "31.231706"),// 圆心位置
            radius: 500, //半径
            strokeColor: "#F33", //线颜色
            strokeOpacity: 1, //线透明度
            strokeWeight: 3, //线粗细度
            fillColor: "#ee2200", //填充颜色
            fillOpacity: 0.35//填充透明度
        });

        <%
            if (action == "update")
            {
                Response.Write(string.Format("SetPoint({0}, {1})", model.Longitude, model.Latitude));
            }
        %>

        function SetPoint(lng, lat) {

            map.setCenter([lng, lat]);//将坐标进行刷新
            marker.setMap();
            marker = new AMap.Marker({
                position: [lng, lat],
                map:map
            });
            circle = new AMap.Circle({
                center: new AMap.LngLat(lng, lat),// 圆心位置
                radius: <%=model.Range%>, //半径
                strokeColor: "#F33", //线颜色
                strokeOpacity: 1, //线透明度
                strokeWeight: 3, //线粗细度
                fillColor: "#ee2200", //填充颜色
                fillOpacity: 0.35//填充透明度
             });
            marker.setMap(map);//marker点添加到地图上   marker.setMap();--移除地图上的点
            circle.setMap(map);

        }

        //单击
        AMap.event.addListener(map, 'click', getLnglat); //点击事件

        

        function getLnglat(e)
        {
            marker.setMap();
            circle.setMap();
            var nlng = e.lnglat.getLng();
            var nlat = e.lnglat.getLat();
            map.setCenter([nlng, nlat]);
            marker = new AMap.Marker({
                position: [nlng, nlat]
            });
            
            $("#txtLongitude").val(nlng);
            $("#txtLatitude").val(nlat);
            
            circle = new AMap.Circle({
                center: new AMap.LngLat(nlng, nlat),// 圆心位置
                radius: <%=action=="update"?model.Range:100%>, //半径
                strokeColor: "#F33", //线颜色
                strokeOpacity: 1, //线透明度
                strokeWeight: 3, //线粗细度
                fillColor: "#ee2200", //填充颜色
                fillOpacity: 0.35//填充透明度
            });
            
            marker.setMap(map);
            circle.setMap(map);





            //单击获取地址
            AMap.plugin('AMap.Geocoder', function () {
                var geocoder = new AMap.Geocoder({
                    city: "010"//城市，默认："全国"
                });
                marker.setPosition(e.lnglat);
                geocoder.getAddress(e.lnglat, function (status, result) {
                    if (status == 'complete') {
                        console.log(result.regeocode);
                        $("#txtAddress").val(result.regeocode.formattedAddress);
                    } else {
                        message.innerHTML = '无法获取地址'
                    }
                })
            });
        }

        AMap.plugin(['AMap.Autocomplete', 'AMap.PlaceSearch'], function () {//回调函数
            //实例化Autocomplete
            var autoOptions = {
                city: "", //城市，默认全国
                input: "suggestId"//使用联想输入的input的id
            };
            autocomplete = new AMap.Autocomplete(autoOptions);
            var placeSearch = new AMap.PlaceSearch({
                city: '上海',
                map: map
            })
            AMap.event.addListener(autocomplete, "select", function (e) {
                //TODO 针对选中的poi实现自己的功能
                placeSearch.search(e.poi.name);
            });
        })

    </script>

</asp:Content>
