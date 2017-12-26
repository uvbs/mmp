



$(function () {
    setHeadSelected(4);
});

function GoWeekForecast(rootType) {
    var rootid = 0;
    if (rootType == 'stock') {
        rootid = catedata.sroot;
    } else if (rootType == 'Metal') {
        rootid = catedata.hroot;
    } else if (rootType == 'Crude') {
        rootid = catedata.yroot;
    }
    window.location.href = '/customize/StockPlayer/Src/PupilDebate/WeekForecast.aspx?rootid=' + rootid;
}