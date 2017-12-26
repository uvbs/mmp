
var ue;
$(function () {
    setHeadSelected(5);
    ue = UE.getEditor('cognizance', {
        initialFrameWidth: 560,
        initialFrameHeight: 320,
        scaleEnabled: true,
        autoFloatEnabled: false,
        toolbars: ueditorToobars
    });


    $('.btn-add').click(function () {

        var model = {
            type: 'Cognizance',
            title: $.trim($('#title').val()),
            content: ue.getContent(),
            summary: '',
            action: 'AddArticle',
        };

        if ($.trim(model.title) == '') {
            alert('请输入标题');
            return;
        }

        if ($.trim(model.content) == '') {
            alert('请输入内容');
            return;
        }

        var url = '/Serv/PubApi.ashx';
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: url,
            data: model,
            dataType: 'JSON',
            success: function (resp) {
                layer.close(layerIndex);
                if (resp.isSuccess) {
                    alert('发布成功');
                    $('#title').val('');
                    ue.setContent('');
                } else if (resp.errcode == 10010) {
                    alert('请先登录');
                } else {
                    alert('提交出错');
                }
            }
        });
    });

    //重置
    $('.btn-reset').click(function () {
        $('#title').val('');
        ue.setContent('');
    });

    
});