
KindEditor.plugin('video', function (K) {
    var self = this, name = 'video';
    self.plugin.video = {
        edit: function () {
            var html = [
				'<div style="padding:20px;">',
				//url
				'<div class="ke-dialog-row">',
				'<label for="keHtml">代码:</label>',
				'<textarea id="keHtml" style="width: 97% !important;padding: 5px;min-height: 95px;line-height: 20px;border: 1px solid rgb(180, 180, 180);"></textarea>',
				'</div>',
				'</div>'
            ].join('');
            var dialog = self.createDialog({
                name: name,
                width: 450,
                title: '分享视频',
                body: html,
                yesBtn: {
                    name: self.lang('yes'),
                    click: function (e) {
                        var html = K.trim(htmlBox.val());

                        var inObj;
                        if (html.indexOf('http') == 0) {
                            inObj = $('<iframe frameborder=0 allowfullscreen></iframe>');
                            $(inObj).attr('src', html);
                        }
                        else {
                            inObj = $(html);
                        }
                        $(inObj).attr('width', '100%');
                        $(inObj).attr('height', '320');
                        var nHtml = $(inObj).prop('outerHTML');
                        self.insertHtml(nHtml).hideDialog().focus();
                    }
                }
            }),
			div = dialog.div,
			htmlBox = K('#keHtml', div);
            htmlBox[0].focus();
            htmlBox[0].select();
        }
    };
    self.clickToolbar(name, self.plugin.video.edit);
});
KindEditor.plugin('cleardoc', function (K) {
    var self = this, name = 'cleardoc';
    self.clickToolbar(name, function () {
        self.html('');
    });
});
KindEditor.plugin('importword', function (K) {
    var self = this, name = 'importword';
    filePostName = K.undef(self.filePostName, 'imgFile')
    self.plugin.importword = {
        edit: function () {
            var html = [
				'<div style="padding:20px;">',
				'<div class="ke-dialog-row">',
				'<label style="width:60px;">word文件</label>',
                '<input type="text" name="localUrl" class="ke-input-text" tabindex="-1" style="width:260px;" readonly="true"> &nbsp;',
                '<div class="ke-inline-block ke-upload-button">',
                '<div class="ke-upload-area" style="width: 60px;">',
                '<span class="ke-button-common"><input type="button" class="ke-button-common ke-button" value="浏览..."></span><input type="file" accept=".doc,.docx" class="ke-upload-file" name="file1" tabindex="-1">',
                '</div>',
                '</div>',
				'</div>',
				'</div>'
            ].join('');
            var dialog = self.createDialog({
                name: name,
                width: 450,
                title: 'word导入',
                body: html,
                yesBtn: {
                    name: self.lang('yes'),
                    click: function (e) {
                        dialog.showLoading(self.lang('uploadLoading'));
                        $.ajaxFileUpload({
                            url: '/serv/api/common/importword.ashx',
                            secureuri: false,
                            fileElement: $(fileBox),
                            dataType: 'json',
                            success: function (result) {
                                dialog.hideLoading();
                                if (result.status) {
                                    self.insertHtml(result.result).hideDialog().focus();
                                }
                                else {
                                    alert(result.msg);
                                }
                            }
                        });
                    }
                }
            }),
			div = dialog.div,
			fileBox = K('.ke-upload-file', div);
            localUrl = K('.ke-input-text', div);
            fileBox.change(function (e) {
                localUrl.val(fileBox.val());
            });
        }
    };
    self.clickToolbar(name, self.plugin.importword.edit);
});

KindEditor.lang({
    video: '分享视频',
    cleardoc: '清空内容',
    importword: 'word导入'
}, 'zh_CN');
