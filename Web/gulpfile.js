var gulp = require('gulp'),
uglify = require('gulp-uglify'),
   minifycss = require('gulp-minify-css'),
   concat = require('gulp-concat'),
   less = require('gulp-less'),
   plumber = require('gulp-plumber');

//npm install gulp gulp-uglify gulp-minify-css gulp-concat gulp-less gulp-plumber --save-dev
var baseUbimPath = 'customize/ubi/m/';

var paths = {
    less: baseUbimPath + 'modules/**/*.less',
    js: [
        baseUbimPath + 'js/global.js',
        baseUbimPath + 'js/app.js',
        baseUbimPath + 'js/services.js',
        baseUbimPath + 'modules/panelsProvider/**/*.js',
        baseUbimPath + 'modules/directives/**/*.js',
        baseUbimPath + 'modules/common/*.js',
        baseUbimPath + 'modules/public/**/*.js'
    ],
    purecar_m_js: 'customize/pureCar/m/src/**/*.js',
    purecar_m_css: 'customize/pureCar/m/src/**/*.less',

    care_m_js: [
                'customize/10000care/m/src/common/**/*.js',
                'customize/10000care/m/src/services/**/*.js',
                'customize/10000care/m/src/directives/**/*.js',
                'customize/10000care/m/src/public/**/*.js',
    ],
    care_m_less: 'customize/10000care/m/src/**/*.less',
    backendjs:[
    // 'Scripts/jquery-1.8.2.min.js',
    'Scripts/ajaxfileupload2.1.js',
    // 'MainStyle/Res/easyui/jquery.easyui.min.js',
    'MainStyle/Res/easyui/locale/easyui-lang-zh_CN.js',
    'Ju-Modules/bootstrap/js/bootstrap.js',
    'Scripts/StringBuilder.js',
    'Scripts/Common.js',
    'kindeditor-4.1.10/kindeditor.js',
    'kindeditor-4.1.10/lang/zh_CN.js',
    'kindeditor-4.1.10/kindeditor-plugins.js',
    'lib/layer/2.1/layer.js',
    'Scripts/global.js'
    ]

};


gulp.task('backendjs', function () {
    gulp.src(paths.backendjs)
        .pipe(concat('app.bundle.js'))
        //.pipe(uglify())
        .pipe(gulp.dest('lib'));
});


//---------------ubi start-------------------
gulp.task('ubimjs', function () {
    gulp.src(paths.js)
        .pipe(concat('app.bundle.js'))
        //.pipe(uglify())
        .pipe(gulp.dest('customize/ubi/m/js'));
});

gulp.task('ubimcss', function () {
    gulp.src(paths.less)
        .pipe(concat('all.less'))
        .pipe(plumber())
        .pipe(less())
        .pipe(gulp.dest('customize/ubi/m/css'));
});


gulp.task('ubim', function () {
    gulp.start('ubimcss');
    gulp.start('ubimjs');
});

gulp.task('watch', function () {
    gulp.watch(paths.less, ['ubimcss']);
    for (var i = 0; i < paths.js.length; i++) {
        gulp.watch(paths.js[i], ['ubimjs']);
    };
});
//-------------------------------------------

//------------ pureCar start ------------

gulp.task('purecarjs', function () {
    gulp.src(paths.purecar_m_js)
        .pipe(concat('app.bundle.js'))
        //.pipe(uglify())
        .pipe(gulp.dest('customize/pureCar/m/dist'));
});

gulp.task('purecarcss', function () {
    gulp.src(paths.purecar_m_css)
        .pipe(concat('all.less'))
        .pipe(plumber())
        .pipe(less())
        .pipe(gulp.dest('customize/pureCar/m/dist'));
});

gulp.task('purecarwatch', function () {
    gulp.watch(paths.purecar_m_css, ['purecarcss']);
    gulp.watch(paths.purecar_m_js, ['purecarjs']);
});

//---------------------------------------


//------------ 10000care start ------------

gulp.task('carejs', function () {
    gulp.src(paths.care_m_js)
        .pipe(concat('app.bundle.js'))
        //.pipe(uglify())
        .pipe(gulp.dest('customize/10000care/m/dist'));
});

gulp.task('careless', function () {
    gulp.src(paths.care_m_less)
        .pipe(concat('all.less'))
        .pipe(plumber())
        .pipe(less())
        .pipe(gulp.dest('customize/10000care/m/dist'));
});

gulp.task('carewatch', function () {
    gulp.watch(paths.care_m_less, ['careless']);
    gulp.watch(paths.care_m_js, ['carejs']);
});

//---------------------------------------