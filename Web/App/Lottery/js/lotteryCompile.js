

function ProcessPageElementShow(type,isAdd) {

    var $document = $(document);

    if (type == 1 || type == '') {

        if (isAdd) {
            $document.find('#imgshareimg').attr('src', '/App/Lottery/wap/images/ggl.jpg');
        }
        $document.find('.wrapToolBarButtonSet').hide();
    }

    if (type == 2) {
        if (isAdd) {
            $document.find('#imgshareimg').attr('src', 'http://static-files.socialcrmyun.com/img/lottery/shake.png');
        }

        $document.find('.wrapImgThumbnailsPath,.wrapBackGroundColor').hide();

    }

}
    
