//初始化page

function change(data) {
    if (data.cur === 0) {
        $('.sign_btn').on('tap', function () {



            if (canSignUp == "false") {
                layermsg("注册过的销售服务店人员才能报名");
                //alert("注册过的销售服务店人员才能报名");

            }
            else {
                window.location.href = 'SignUp.aspx';
            }



        });
        return 0;
    }


    if (data.cur === 1) {
        $('.rule').on('tap', function () {
            window.location.href = 'http://haima.comeoncloud.net/6353a/details.chtml';
        });

        return 1;
    }

}
; (function (g) {
    $('.container-inner').fullPage({ start: 0, onchange: change });
} (window));



