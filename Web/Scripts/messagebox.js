//调用代码:
//msgText.init(text,timer)
//text：提示框显示文本,类型String
//timer:提示框显示时间,不设置为永久显示 单位为毫秒ms

var msgText = {
    init: function (text, timer) {
        var _cssText = "position: fixed;top: 50%;margin: 0 auto;background: rgba(0, 0, 0, 0.63);color: #fff;padding: 2px 15px;left: 50%;margin:-16px 0 0 -85px;line-height: 2;border-radius: 5px;width: 170px;-webkit-transition:all 0.3s linear;transition:all 0.3s linear;text-align:center;";
        if (document.getElementsByTagName("msg").length > 0) {
            document.getElementsByTagName("msg")[0].innerHTML = text;
            return;

        }
        else {
            this.oP = document.createElement("msg");
            this.oP.innerHTML = text;
            this.setCss(_cssText);
            this.show();
            timer && (this.hide(timer));
        }

    },
    show: function () {
        document.body.appendChild(this.oP);
    },
    hide: function (timer) {
        var _this = this;
        setTimeout(function () {
            _this.oP.style.opacity = "0";
            setTimeout(function () {
                for (var i = 0; i < document.getElementsByTagName("msg").length; i++) {
                    document.getElementsByTagName("msg")[i].remove();
                }


            }, 100)
        }, timer)
    },
    setCss: function (csstext) {
        this.oP.style.cssText = csstext;
    }
}