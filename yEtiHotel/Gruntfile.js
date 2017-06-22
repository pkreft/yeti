module.exports = function(grunt) {

    grunt.initConfig({
        concat: {
            bundle: {
                src: [
                    'Scripts/assets/*.js'
                ],
                dest: 'Scripts/dist/scripts.js'
            },
            bowerjs: {
                src: [
                    'bower_components/jquery/dist/jquery.min.js',
                    'bower_components/angular/angular.min.js',
                    'bower_components/angular-ui-notification/dist/angular-ui-notification.min.js',
                    'bower_components/ngDialog/js/ngDialog.min.js',
                    'bower_components/moment/moment.js',
                    'bower_components/bootstrap-daterangepicker/daterangepicker.js',
                ],
                dest: 'Scripts/dist/libs.min.js'
            },
            bowercss: {
                src: [
                    'bower_components/bootstrap-daterangepicker/daterangepicker.css',
                    'bower_components/angular-ui-notification/dist/angular-ui-notification.min.css',
                    'bower_components/ngDialog/css/ngDialog.css',
                    'bower_components/ngDialog/css/ngDialog-theme-default.css',
                ],
                dest: 'Content/dist/libs.min.css'
            }
        },
        uglify: {
            my_target: {
                files: {
                    'Scripts/dist/scripts.min.js': ['Scripts/dist/scripts.js']
                }
            }
        },
    });
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.registerTask('default', ['concat:bundle', 'concat:bowerjs', 'concat:bowercss', 'uglify']);
};
