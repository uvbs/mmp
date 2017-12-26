$.extend({
    //上传文件
    ajaxImgUpload: function (s) {
        var baseSet = {
            fileElementName: 'file1',
            url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
            maxWidth:800,
            data: {}
        };
        s = $.extend({}, baseSet, s);
        var fileElement;
        if (s.fileElementId) {
            s.fileElement = $('#' + s.fileElementId);
        }
        if ($(s.fileElement).get(0).files.length == 0) {
            if (!s.error) s.error({ status: false, msg: '请选择文件' });
            return;
        }
        if (!!s.maxWidth && !!s.data) s.data.maxWidth = s.maxWidth;
        if (!!s.maxHeight && !!s.data) s.data.maxHeight = s.maxHeight;

        if (!s.dataType) s.dataType = 'json';

        //if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            s.file = $(s.fileElement).get(0).files[0];
            if (!s.filename) s.filename = s.file.name;
            s.filetype = s.file.type;
            s.fileext = s.file.name.substr(s.file.name.lastIndexOf('.'));
            $.uploadData(s)
        }
        else {
            if (typeof ($.ajaxFileUpload) == 'function') {
                if (typeof (s.dataType) == 'undefined') {
                    s.dataType = 'json';
                }
                $.ajaxFileUpload(
                 {
                     url: s.url,
                     secureuri: false,
                     fileElement: $(s.fileElement),
                     fromImgUpload: true,
                     maxWidth: s.maxWidth,
                     maxHeight: s.maxHeight,
                     data: s.data,
                     dataType: s.dataType,
                     success: function (result) {
                         if (!!s.success) s.success(result);
                     }
                 });
            }
            else {
                if (!s.error) s.error({ status: false, msg: 'ie9以下浏览器请下载chrome' });
            }
        }
    },
    //上传文件
    uploadData: function (s) {
        //var form = $('<form  action="" method="POST" enctype="multipart/form-data"></form>');
        if (s.filetype.lastIndexOf('image/') != 0) {
            //$(form).append($(s.fileElement).clone())
            //s.formData = new FormData(form);
            s.formData = new FormData();
            s.formData.append(s.fileElementName, s.file);
            $.formatOtherData(s);
            $.postData(s);
        } else {
            $.getbase64(s);
        }
    },
    //格式化其他参数
    formatOtherData: function (s) {
        if (!!s.data) {
            for (var i in s.data) {
                s.formData.append(i, s.data[i]);
            }
        }
    },
    /**
     * 上传文件
     * @param otherData {name:'',value:''}数组
     */
    postData: function (s) {
        $.ajax({
            url: s.url,
            type: "POST",
            data: s.formData,
            contentType: false,        // 告诉jQuery不要去设置Content-Type请求头
            processData: false,         // 告诉jQuery不要去处理发送的数据
            dataType: s.dataType,
            success: function (result) {
                if (s.dataType == 'json') {
                    if (typeof (result) == 'string') {
                        result = JSON.parse(result);
                    }
                } else if (s.dataType == 'text') {
                    if (typeof (result) != 'string') {
                        result = JSON.stringify(result);
                    }
                }
                if (!!s.success) s.success(result);
            },
            beforeSend: function (result) {
                if (s.beforeSend) s.beforeSend(result);
            },
            complete: function (result) {
                if (s.complete) s.complete(result);
            },
            error: function (result) {
                if (!!s.error) s.error(result);
            }
        });
    },
    /**
     * 获取上传图片的base64文件
     * @param otherData {name:'',value:''}数组
     */
    getbase64: function (s) {
        var fReader = new FileReader();
        fReader.onload = function (e) {
            s.base64 = this.result;
            if (!!s.maxWidth || !!s.maxHeight) {
                $.zoomImg(s);//图片缩放
            }
            else {
                $.uploadBase64Data(s); // 回掉上传
            }
        };
        fReader.readAsDataURL(s.file);
    },
    /**
     * 上传base64图片
     * @param otherData {name:'',value:''}数组
     */
    uploadBase64Data: function (s) {
        //var form = $('<form  action="" method="POST" enctype="multipart/form-data"></form>');
        //s.formData = new FormData(form);
        s.formData = new FormData();
        s.formData.append("file1", $.convertBase64UrlToBlob(s), s.filename);
        $.formatOtherData(s);
        $.postData(s);
    },
    //图片缩放
    zoomImg: function (s) {
        if (!!s.maxWidth || !!s.maxHeight) {
            var img = new Image();
            img.onload = function () {

                var hRatio;
                var wRatio;
                var Ratio = 1;
                var w = img.width;
                var h = img.height;

                if (!s.maxHeight) s.maxHeight = 0;
                if (!s.maxWidth) s.maxWidth = 0;

                wRatio = s.maxWidth / w;
                hRatio = s.maxHeight / h;
                if (s.maxWidth == 0 && s.maxHeight == 0) {
                    Ratio = 1;
                } else if (s.maxWidth == 0) {//
                    if (hRatio < 1) Ratio = hRatio;
                } else if (s.maxHeight == 0) {
                    if (wRatio < 1) Ratio = wRatio;
                } else if (wRatio < 1 || hRatio < 1) {
                    Ratio = (wRatio <= hRatio ? wRatio : hRatio);
                }
                if (Ratio < 1) {
                    w = w * Ratio;
                    h = h * Ratio;

                    // 获取 canvas DOM 对象
                    var canvas = $('<canvas></canvas>').get(0);
                    // 获取 canvas的 2d 环境对象,
                    // 可以理解Context是管理员，canvas是房子
                    var ctx = canvas.getContext("2d");
                    // canvas清屏
                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                    // 重置canvas宽高
                    canvas.width = w;
                    canvas.height = h;
                    // 将图像绘制到canvas上
                    ctx.drawImage(img, 0, 0, w, h);
                    s.base64 = canvas.toDataURL(s.filetype);
                }
                $.uploadBase64Data(s); // 回掉上传
            }
            img.src = s.base64;
        }
    },
    /**
     * 将以base64的图片url数据转换为Blob
     * @param urlData
     *            用url方式表示的base64图片数据
     */
    convertBase64UrlToBlob: function (s) {
        var bytes = window.atob(s.base64.split(',')[1]);        //去掉url的头，并转换为byte
        //处理异常,将ascii码小于0的转换为大于0
        var ab = new ArrayBuffer(bytes.length);
        var ia = new Uint8Array(ab);
        for (var i = 0; i < bytes.length; i++) {
            ia[i] = bytes.charCodeAt(i);
        }
        return new Blob([ab], { type: s.filetype });
    }
});