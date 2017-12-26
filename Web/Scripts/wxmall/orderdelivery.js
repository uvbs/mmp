$(function () {

    // check("#delivery","#deliverybox",".checktype");
    // check("#saddress","#saddressbox",".moreinfo");


    var delivery = new checkinfo($("#delivery"), $("#deliverybox"));
    delivery.clickshow(controlbox(delivery, ".checktype"));


    var saddress = new checkinfo($("#saddress"), $("#saddressbox")); //".moreinfo"
    saddress.clickshow(controlbox(saddress, { ".checktype": ".shopnamev", ".moreinfo": ".shopaddressv" }));
    var saddressmoinfo = $("#saddress").find(".moreinfo");
    if (!saddressmoinfo.text()) { saddressmoinfo.css({ "display": "none" }) };

    var paytype = new checkinfo($("#paytype"), $("#paytypebox"));
    paytype.clickshow(controlbox(paytype, ".checktype"));

    var person = new checkinfo($("#person"), $("#personbox"));
    person.clickshow(controlbox(person, {
        ".personname": ".nameinfo",
        ".persontell": ".phone",
        ".personaddress": ".addressinfo" ,
        ".personaddress1": ".addressinfo1" ,
        ".personaddress2": ".addressinfo2" ,
        ".personaddress3": ".addressinfo3" 

    }));

    if ($(".nameinfo").html() != "") {

        $("#person").removeClass("noaddressbox");

    }

})



//显示隐藏选择框
var checkinfo = function (a, b) {
    this.ca = a;
    this.cb = b;
}
checkinfo.prototype.show = function () {
    this.cb.addClass("showcheck");
};
checkinfo.prototype.hide = function () {
    this.cb.removeClass("showcheck");
};

//"点击"显示  "取消"隐藏 f是选择框内的自定义方法
checkinfo.prototype.clickshow = function (f) {
    var _this = this;
    _this.ca.click(function () {
        _this.show()
        f;
    })
    _this.cb.find(".close").click(function () {
        _this.hide()
    })
};


//内容选择与替换
function controlbox(obj, infobox) {
    if (obj.cb.find(".radiocheck")[0]) {
        var ack = obj.cb.find(".value");
        ack.unbind("click");
        ack.bind("click", function () {
            var thisack = $(this);
            if (typeof infobox === "string") {
                var t = $(this).text();
                //var t=$(obj.cb).find(".value:eq("+$(this).index()/2+")").text();
                obj.ca.find(infobox).text(t);
            } else if (typeof infobox === "object") {
                for (dd in infobox) {
                    obj.ca.find(dd).css({ "display": "block" })
                    obj.ca.find(dd).text(thisack.find(infobox[dd]).text());
                }
            }
            obj.hide();
        })
    } else {
        var ack = obj.cb.find(".saveaddress");
        ack.unbind("click");
        ack.bind("click", function () {
            var num = 0
            for (dd in infobox) {
                var t = $(obj.cb).find(dd).val();
                t == "" ? num++ : "";
                obj.ca.find(infobox[dd]).text(t);
            }
            if (num < 3) {
                obj.ca.removeClass("noaddressbox");
            } else {
                obj.ca.addClass("noaddressbox");
            }
        })
    }
}