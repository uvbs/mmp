var gulp = require('gulp'),
    less = require('gulp-less'),
    concat = require('gulp-concat');

gulp.task('build-less', function () {
    gulp.src('css/*.less')
        .pipe(less())
        .pipe(gulp.dest('css/'))
});
gulp.task('stylesheets', ['build-less'], function () {
    gulp.src('css/*.css')
        .pipe(concat('all.css'))
        .pipe(gulp.dest('css/'))
});
gulp.task('default', function () {
    gulp.start('stylesheets');
});