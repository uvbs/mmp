

var listVm = new Vue({
    el: '.vue-el-left',
    data: {
        handler: '/Serv/API/Admin/LiveChat/Room/',
        roomList: [],
        wait_join_count:0
       
    },
    methods: {
        init: function () {

            
        },
        loadRoomData: function () {
            var _this = this;
           
            $.ajax({
                type: 'post',
                url: _this.handler + "List.ashx",
                data: {

                },
                dataType: 'json',
                success: function (resp) {
                  
                    _this.roomList = resp.list;
                    _this.wait_join_count = resp.wait_join_count;
                    //setTimeout("listVm.loadRoomData()", 5000);

                }
            });
        },
        joinRoom: function (item) {
            var _this = this;
            for (var i = 0; i < _this.roomList.length; i++) {
                if (_this.roomList[i].room_id == item.room_id) {
                    _this.roomList[i].is_kefu_join = true;
                }
            }
            $("#ifLiveChat").attr("src", "");
            var path = "LiveChat.aspx?kefu_id=" + kefuId + "&room_id=" + item.room_id;
            $("#ifLiveChat").attr("src",path);
           
          


        },
        toOrderDetail: function (item) {
            var loca = "/customize/mmpadmin/index.aspx?hidemenu=1#/index/orderList?userAutoId=" + item.user_auto_id;
            window.open(loca);
        },
        toViewDetail: function (item) {
            window.open('/App/Monitor/PVEventDetails.aspx?autoId='+item.user_auto_id);
        }

       




    }
});
setInterval("listVm.loadRoomData();",5000);


