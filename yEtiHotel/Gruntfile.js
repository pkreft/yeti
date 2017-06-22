module.exports = function(grunt) {

    grunt.initConfig({
        concat: {
            bundle: {
                src: [
                    'Scripts/assets/*.js'
                ],
                dest: 'Scripts/dist/scripts.js'
            },
            bower: {
                src: [
                    'bower_components/angular/**/*min.js',
                    'bower_components/angular-ui-notification/**/*min.js',
                    'bower_components/ngDialog/**/*min.js',
                ],
                dest: 'Scripts/dist/libs.min.js'
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
    grunt.registerTask('default', ['concat:bundle', 'concat:bower', 'uglify']);
};
