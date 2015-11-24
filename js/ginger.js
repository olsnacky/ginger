angular.module('ginger', ['ngRoute', 'ngLoadScript', 'gist-embed'])

.config(function ($routeProvider) {
    $routeProvider

    .when('/', {
        templateUrl: 'js/template/editor.html'
    })
        .when('/lexicon', {
            templateUrl: 'js/template/lexicon.html'
        })
        .when('/grammar', {
            templateUrl: 'js/template/grammar.html'
        })
        .when('/story', {
            templateUrl: 'js/template/story.html'
        })
        .when('/story/name', {
            templateUrl: 'js/template/name.html'
        })
    .otherwise({
        redirectTo: '/'
    })
});