var ue;
$(function () {
    if ($('#companypublish').length > 0) {
        ue = UE.getEditor('companypublish', {
            initialFrameWidth: 700,
            initialFrameHeight: 470,
            scaleEnabled: true,
            autoFloatEnabled: false,
            toolbars: ueditorToobars
        });
    } else {
        ue = UE.getEditor('blogs', {
            initialFrameWidth: 700,
            initialFrameHeight: 470,
            scaleEnabled: true,
            autoFloatEnabled: false,
            toolbars: ueditorToobars
        });
    }
    $($('#stock_type option')[1]).val(catedata.sell);
    $($('#stock_type option')[2]).val(catedata.buy);
    //顶部切换
    $('[name=btn]').click(function () {
        $(this).parent().children().removeClass('selected');
        $(this).addClass('selected');

        var val = $(this).val();
        if (val == 'blogs') {
            tt.clearMsg();
            ue = UE.getEditor('blogs', {
                initialFrameWidth: 700,
                initialFrameHeight: 470,
                scaleEnabled: true,
                autoFloatEnabled: false,
                toolbars: ueditorToobars
            });
            $('.content-blogs').show();
            $('.content-comment').hide();
            $('.content-stock').hide();
        } else if (val == 'comment') {
            tt.clearMsg();
            $('.content-blogs').hide();
            $('.content-comment').show();
            $('.content-stock').hide();
        } else {
            tt.clearMsg();
            ue = UE.getEditor('stocks',{
            initialFrameWidth: 700,
            initialFrameHeight: 470,
            scaleEnabled: true,
            autoFloatEnabled: false,
            toolbars: ueditorToobars
            });
            $('.content-blogs').hide();
            $('.content-comment').hide();
            $('.content-stock').show();
        }

    })
    //编辑(赋值)
    if (jid != '') {
        $('#BtnAdd').text('保存');
        $.ajax({
            type: 'POST',
            url: '/Serv/api/article/get.ashx',
            data: { article_id: jid, no_score: '1', no_pv: '1' },
            dataType: 'json',
            success: function (result) {
                switch (result.article_type) {
                    case "Stock":
                        ue = UE.getEditor('stocks', {
                            initialFrameWidth: 700,
                            initialFrameHeight: 470,
                            scaleEnabled: true,
                            autoFloatEnabled: false,
                            toolbars: ueditorToobars
                        });
                        $('.content-blogs').hide();
                        $('.content-comment').hide();
                        $('.content-stock').show();
                        $('#EditTitle').text('编辑股权交易');
                        $('[value=stock]').addClass('selected');
                        $('#stock_title').val(result.article_name);
                        $('#stock_number').val(result.k3);
                        $('#stock_thumbnailsPath').attr('src', result.article_img_url);
                        $('#stock_type option').each(function (k, v) {
                            if ($(v).val() == result.cate_id) {
                                $(this).attr("selected", true);
                            }
                        });
                        
                        break;
                    case "Comment":
                        $('.content-blogs').hide();
                        $('.content-comment').show();
                        $('.content-stock').hide();
                        $('#EditTitle').text('编辑评论');
                        break;
                    case "Bowen":
                        ue = UE.getEditor('blogs', {
                            initialFrameWidth: 700,
                            initialFrameHeight: 470,
                            scaleEnabled: true,
                            autoFloatEnabled: false,
                            toolbars: ueditorToobars
                        });
                        $('.content-blogs').show();
                        $('.content-comment').hide();
                        $('.content-stock').hide();
                        $('#EditTitle').text('编辑博客');
                        $('[value=blogs]').addClass('selected');
                        $('#blogs_title').val(result.article_name);
                        $('#blogs_thumbnailsPath').attr('src', result.article_img_url);
                        break
                    case "CompanyPublish":
                        $('#title').val(result.article_name);
                        $('#companypublish_thumbnailsPath').attr('src', result.article_img_url);
                        $('#BtnSave').text('保存');
                        break;
                    default:
                        break;

                }
            }

        });
    }
    //发布
    //$('#blogs_thumbnailsPath').click(function () {
    //    $('#thumbnailsPath1').click();
    //})
    var url = '/serv/api/common/file.ashx?action=Add';
    $(document).on('change', '#thumbnailsPath1', function () {
        var layerIndex = progress();
        $.ajaxFileUpload({
            url: url,
            secureuri: false,
            fileElementId: 'thumbnailsPath1',
            dataType: 'json',
            success: function (result) {
                layer.close(layerIndex);
                if (result.errcode == 0) {
                    $('#blogs_thumbnailsPath').attr('src', result.file_url_list[0]);
                }
                else {
                    alert(result.errmsg);
                }
            }
        });
    });

    //$('#stock_thumbnailsPath').click(function () {
    //    $('#thumbnailsPath2').click();
    //})
    $(document).on('change', '#thumbnailsPath2', function () {
        var layerIndex = progress();
        $.ajaxFileUpload({
            url: url,
            secureuri: false,
            fileElementId: 'thumbnailsPath2',
            dataType: 'json',
            success: function (result) {
                layer.close(layerIndex);
                if (result.errcode == 0) {
                    $('#stock_thumbnailsPath').attr('src', result.file_url_list[0]);
                }
                else {
                    alert(result.errmsg);
                }
            }
        });
    });

    //公司
    //$('#companypublish_thumbnailsPath').click(function () {
    //    $('#thumbnailsPath3').click();
    //})
    $(document).on('change', '#thumbnailsPath3', function () {
        var layerIndex = progress();
        $.ajaxFileUpload({
            url: url,
            secureuri: false,
            fileElementId: 'thumbnailsPath3',
            dataType: 'json',
            success: function (result) {
                layer.close(layerIndex);
                if (result.errcode == 0) {
                    $('#companypublish_thumbnailsPath').attr('src', result.file_url_list[0]);
                }
                else {
                    alert(result.errmsg);
                }
            }
        });
    });


    tt.vf.req.addId('blogs_title', 'stock_title', 'stock_number', 'stock_type', 'comment_content', 'title', 'content');

    imgValidator = tt.BV.ext({
        v: function (trimedValue, indexOfElements, elements, field) {
            var id = $.trim($(elements).attr('id'));
            var src = $.trim($(elements).attr('src'));
            if (src == '') {
                return false;
            }
            if (id == 'blogs_thumbnailsPath' && src != 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/blogs-img.png') {
                return true;
            }
            else if (id == 'stock_thumbnailsPath' && src != 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/stock-img.png') {
                return true;
            } else if (id == 'companypublish_thumbnailsPath' && src != 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/companyPublish-img.png') {
                return true;
            }
            //第二个元素总是会通过  
            if (indexOfElements == 1) {
                return true;
            }
            return false;
        },
        getI18: function (label) {
            return "请上传图片!";
        },
        /** 
         * 验证通过时，提示信息 
         */
        getTip: function (e, f, v, val) {
            return "恭喜！自定义验证通过!";
        }
    });
    new imgValidator().addId("blogs_thumbnailsPath", "stock_thumbnailsPath", 'companypublish_thumbnailsPath');
    $('#BtnAdd').click(function () {
        var btnType = $('.selected').val();
        if (btnType == 'blogs') {
            if (!tt.validateId('blogs_title', 'blogs_summary', 'blogs_thumbnailsPath')) {
                return false;
            }
            var model = {
                type: 'Bowen',
                title: $.trim($('#blogs_title').val()),
                content:ue.getContent(),
                thumbnails: $.trim($('#blogs_thumbnailsPath').attr('src')),
                action: 'AddArticle',
                summary:'',
                jid:jid
            };
            if (model.content == '') {
                alert('请输入内容');
                return;
            }

            var url = '/Serv/PubApi.ashx';
            if (jid != '') {
                url = '/Serv/API/Article/Update.ashx';
            }
            var layerIndex = progress();
            $.ajax({
                type: 'POST',
                url: url,
                data: model,
                dataType: 'json',
                success: function (result) {
                    layer.close(layerIndex);
                    if (jid == '') {
                        if (result.isSuccess) {
                            alert('添加成功');
                            $('#blogs_title').val('');
                            ue.setContent(''),
                            $('#blogs_thumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/blogs-img.png');
                        } else {
                            alert('添加出错');
                        }
                    } else {
                        if (result.status) {
                            alert('修改成功');
                            setInterval(function () {
                                ToCenter();
                            }, 1000);
                        } else {
                            alert('操作出错');
                        }
                    }
                    
                }
            });
        } else if (btnType == 'stock') {
            if (!tt.validateId('stock_title', 'stock_summary', 'stock_number', 'stock_type', 'stock_thumbnailsPath')) {
                return false;
            }
            var model = {
                type: 'Stock',
                title: $.trim($('#stock_title').val()),
                content: ue.getContent(),
                cateId: $.trim($('#stock_type').val()),
                k3: $.trim($('#stock_number').val()),
                thumbnails: $.trim($('#stock_thumbnailsPath').attr('src')),
                action: 'AddArticle',
                summary: '',
                jid:jid
            };
            if (model.content == '') {
                alert('请输入内容');
                return;
            }
            var layerIndex = progress();
            var url = '/Serv/PubApi.ashx';
            if (jid != '') {
                url = '/Serv/API/Article/Update.ashx';
            }
            $.ajax({
                type: 'POST',
                url: url,
                data: model,
                dataType: 'json',
                success: function (result) {
                    layer.close(layerIndex);
                    if (jid == '') {
                        if (result.isSuccess) {
                            alert('操作成功');
                            $('#stock_title').val('');
                            ue.setContent(''),
                            $('#stock_number').val('');
                            $('#stock_type').val('');
                            $('#stock_thumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/stock-img.png');
                           
                        } else {
                            alert('操作出错');
                        }
                    } else {
                        if (result.status) {
                            alert('修改成功');
                            setInterval(function () {
                                ToCenter();
                            }, 1000);
                            
                        } else {
                            alert('操作出错');
                        }
                    }
                    
                }
            });
        } else {
            if (!tt.validateId('comment_title', 'comment_summary', 'comment_content')) {
                return false;
            }
            var model = {
                type: 'Comment',
                title: '时评',
                content: $.trim($('#comment_content').val()),
                summary: '',
                action: 'AddArticle'
            };
            var layerIndex = progress();
            $.ajax({
                type: 'POST',
                url: '/Serv/PubApi.ashx',
                data: model,
                dataType: 'json',
                success: function (result) {
                    layer.close(layerIndex);
                    if (result.isSuccess) {
                        alert('添加成功');
                        $('#comment_content').val('');
                    } else {
                        alert('添加出错');
                    }
                }
            });

        }


    })

    //公司发布
    $('#BtnSave').click(function () {
        if (!tt.validateId('title', 'summary', 'content', 'companypublish_thumbnailsPath')) {
            return false;
        }
        var model = {
            type: 'CompanyPublish',
            title: $.trim($('#title').val()),
            content: ue.getContent(''),
            thumbnails: $.trim($('#companypublish_thumbnailsPath').attr('src')),
            action: 'AddArticle',
            summary: '',
            jid:jid
        };
        if (model.content == '')
        {
            alert('请输入内容');
            return;
        }
        var url = '/Serv/PubApi.ashx';
        if (jid != '') {
            url = '/serv/api/article/update.ashx';
        }
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: url,
            data: model,
            dataType: 'json',
            success: function (result) {
                layer.close(layerIndex);
                if (jid == '') {
                    if (result.isSuccess) {
                        alert('添加成功');
                        $('#title').val('');
                        ue.setContent('');
                        $('#companypublish_thumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/companyPublish-img.png');
                    } else {
                        alert('添加出错');
                    }
                } else {
                    if (result.status) {
                        alert('修改成功');
                        setInterval(function () {
                            ToCenter();
                        }, 1000);
                    } else {
                        alert('操作出错');
                    }
                }
                
            }
        });
    })

    //个人重置
    $('#BtnReset').click(function () {
        var btnType = $('.selected').val();
        if (btnType == 'blogs') {
            $('#blogs_title').val('');
            ue.setContent('');
            //$('#blogs_summary').val('');
            $('#blogs_thumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/blogs-img.png');
        } else if (btnType == 'stock') {
            $('#stock_title').val('');
            //$('#stock_summary').val('');
            ue.setContent('');
            $('#stock_number').val('');
            $('#stock_type').val('');
            $('#stock_thumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/stock-img.png');
        } else if (btnType == 'comment') {
            $('#comment_title').val('');
            //$('#comment_summary').val('');
            $('#comment_content').val('');
        } 
    })
    //公司重置
    $('#BtnClear').click(function () {
        $('#title').val('');
        $('#companypublish_thumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/companyPublish-img.png');
        ue.setContent('');
    })
})