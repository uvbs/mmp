pureCarModule.filter('wan', function () {
  return function (num, withUnit) {
    if(num < 10000 ) return num;
    else return (num / 10000).toFixed(2) + (withUnit ? '万' : '');
  };
}).filter('countDown', function () {
  return function (timeStr, startTime) {
    var moment1 = new moment(startTime || new Date);
    var moment2 = new moment(timeStr);

    var diffInMilliSeconds = moment2.diff(moment1);
    var duration = moment.duration(diffInMilliSeconds);
    return duration.days() + '天' + duration.hours() + '小时' + duration.minutes() + '分' + duration.seconds() + '秒';
  };
}).filter('cutString', function () {
  return function (str, length) {
    var cutted = str.slice(0, length);
    if(str === cutted) { return str; }
    else { return cutted + '...'; }
  };
});