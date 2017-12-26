var viewUser = function () {
    this.initModule();
}
viewUser.fn = viewUser.prototype;
//组件初始化
viewUser.fn.initModule = function () {
    this.initBase();
    this.initBaseData();
    this.initEvent();
}
//初始化组件html
viewUser.fn.initBase = function () {
    viewUser.body = $(document.body);
    $('.view-user').last().remove();
    viewUser.View = $('<div class="view-user view-hidden" data-id="0"></div>');
    viewUser.ViewImg = $('<div class="view-userimg"><img src="/img/europejobsites.png" /></div>');
    viewUser.ViewName = $('<div class="view-username">昵称</div>');
    viewUser.ViewLevel = $('<div class="view-userlevel"></div>');
    viewUser.ViewDescribe = $('<div class="view-userdescribe"></div>');
    viewUser.ViewBottom = $('<div class="view-bottom"><span class="view-add-friend">好友申请</span></div>');

    viewUser.View.append(viewUser.ViewImg);
    viewUser.View.append(viewUser.ViewName);
    viewUser.View.append(viewUser.ViewLevel);
    viewUser.View.append(viewUser.ViewDescribe);
    viewUser.View.append(viewUser.ViewBottom);
    viewUser.body.append(viewUser.View);
}
//初始化组件data
viewUser.fn.initBaseData = function () {
    viewUser.Users = {};
}
//初始化组件html
viewUser.fn.initEvent = function () {
    //头像名称弹出框(显示)
    $(document).on('mouseover', '.username,.userimg', function () {
        var id = $.trim($(this).attr('data-id'));
        if (u_id == id && u_iscenter) return;
        var nickname = $.trim($(this).attr('data-nickname'));
        var avatar = $.trim($(this).attr('data-avatar'));
        var friend = $.trim($(this).attr('data-friend'));
        var times = $.trim($(this).attr('data-times'));
        var describe = $.trim($(this).attr('data-describe'));
        viewUser.fromElement = this;
        var user;
        if(!!avatar){
            user = {
                id:id,
                nickname:nickname,
                avatar:avatar,
                is_friend: friend,
                times: times,
                describe: describe
            }
        }else{
            user = viewUser.Users[id];
        }
        if (id == "") return;
        if (viewUser.View.hasClass('view-hidden')) {
            var _w = $(document).width();
            var _h = $(document).height();
            var theEvent = window.event || arguments.callee.caller.arguments[0];
            var _x = (_w - theEvent.pageX >= 300) ? theEvent.pageX - 2 : _w - 300;
            var _y = (_h - theEvent.pageY >= 160) ? theEvent.pageY - 1 : _h - 160;
            viewUser.View.attr('data-id', id);
            if (user) {
                viewUser.ViewImg.find('img').attr('src', user.avatar);
                if (user.is_friend == 1) {
                    viewUser.ViewBottom.find('.view-add-friend').hide();
                } else {
                    viewUser.ViewBottom.find('.view-add-friend').show();
                }
                viewUser.ViewName.text(user.nickname);
                var lv = getUserLevelImgs(user.id, user.times);
                for (var i = 0; i < lv.imgs.length; i++) {
                    if (!!lv.imgs[i]) viewUser.ViewLevel.append('<img src="' + lv.imgs[i] + '">')
                }
                viewUser.ViewDescribe.text(user.describe);
            } else {
                $.ajax({
                    type: 'post',
                    url: "/Serv/API/User/Get.ashx",
                    data: { id: id, chk_friend: 1 },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.isSuccess) {
                            user = {
                                id: id,
                                username: resp.returnObj.user_name,
                                nickname: resp.returnObj.nick_name,
                                avatar: $.trim(resp.returnObj.head_img_url) == "" ? '/img/europejobsites.png' : $.trim(resp.returnObj.head_img_url),
                                describe: $.trim(resp.returnObj.describe) == "" ? '' : $.trim(resp.returnObj.describe),
                                is_friend: resp.returnObj.is_friend,
                                times: resp.returnObj.times
                            };
                            viewUser.Users[id] = user;
                            viewUser.ViewImg.find('img').attr('src', user.avatar);
                            if (user.is_friend == 1) {
                                viewUser.ViewBottom.find('.view-add-friend').hide();
                            } else {
                                viewUser.ViewBottom.find('.view-add-friend').show();
                            }
                            viewUser.ViewName.text(user.nickname);
                            var lv = getUserLevelImgs(user.id, user.times);
                            for (var i = 0; i < lv.imgs.length; i++) {
                                if (!!lv.imgs[i]) viewUser.ViewLevel.append('<img src="' + lv.imgs[i] + '">')
                            }
                            viewUser.ViewDescribe.text(user.describe);
                        }
                    }
                });
            }
            viewUser.View.css('left', _x);
            viewUser.View.css('top', _y);
            viewUser.View.removeClass('view-hidden');
        }
    });
    //头像名称弹出框(隐藏)
    $(document).on('mouseleave', '.username,.userimg,.view-user', function () {
        var theEvent = window.event || arguments.callee.caller.arguments[0];
        if (!viewUser.View.hasClass('view-hidden')
            && theEvent.toElement != viewUser.fromElement
            && theEvent.toElement != viewUser.View.get(0)
            && theEvent.toElement != viewUser.ViewImg.get(0)) {
            viewUser.View.attr('data-id', 0);
            viewUser.ViewImg.find('img').attr('src', '/img/europejobsites.png');
            viewUser.ViewName.text('昵称');
            viewUser.ViewLevel.html('');
            viewUser.View.css('left', '-500px');
            viewUser.View.css('top', '-500px');
            viewUser.View.addClass('view-hidden')
        }
    });
    //申请好友
    viewUser.ViewBottom.find('.view-add-friend').click(function () {
        var id = $.trim(viewUser.View.attr('data-id'));
        var nickname = $.trim(viewUser.ViewName.text());
        applyFriend(id, nickname);
    });
}