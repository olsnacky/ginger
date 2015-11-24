angular.module('ginger', ['ngRoute', 'ngLoadScript'])
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
    })
    .controller('ExampleController', function() {
        var examples = this;
        
        examples.DECLARATION_IDX = 0;
        examples.ASSIGNMENT_IDX = 1;
        examples.CONTROL_IDX = 2;
        examples.EXPRESSIONS_IDX = 3;
        
        examples.selectedIdx = examples.DECLARATION_IDX;
        
        examples.setSelected = function(index) {
            examples.selectedIdx = index;
            var editor = window.ace.edit("example");
            editor.setValue(examples.selected().source, -1);
        };
        
        examples.selected = function() {
            return examples.list[examples.selectedIdx];
        };
        
        examples.list = [
            {
                name: 'Declaration',
                source: 'int x\nbool y'
            },
            {
                name: 'Assignment',
                source: 'int x\nx = 1'
            },
            {
                name: 'Control',
                source: 'int x\nint y\n\nx = 0\ny = 2\n\nif x < y {\n\tx = y + 1\n}\n\nwhile y < (x + 10) {\n\ty = y + 1\n}'
            },
            {
                name: 'Expressions',
                source: ''
            }
        ];
    });