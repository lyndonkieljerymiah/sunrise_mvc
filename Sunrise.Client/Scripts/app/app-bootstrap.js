var mainApp = angular.module("mainApp",
    ["ngRoute", "ui.bootstrap", 
    "ngAnimate", "toaster",
    "ngFileUpload",
    "ui.grid", 
    "ui.grid.selection", 
    "ui.grid.edit", 
    "ui.grid.rowEdit", 
    "ui.grid.cellNav"]);


mainApp.factory('modelStateValidation',
    function () {
        function parseError(response,parent) {
            var error = {};
            for (var key in response.modelState) {
                var newKey = parent ? parent + "." +  key.toCamelCase(".") : key.toCamelCase(".");
                if (response.modelState[key] instanceof Array) {
                    error[newKey] = response.modelState[key][0];
                }
                else {
                    error[newKey] = response.modelState[key];
                }
            }
            return error;
        }

        function createState(data, filter) {

        }

        return {
            parseError: parseError,
            createState: createState
        }

    });


/*end service*/

/*directive*/
mainApp.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                return val != null ? parseInt(val, 10) : null;
            });
            ngModel.$formatters.push(function (val) {
                return val != null ? '' + val : null;
            });
        }
    };
});


mainApp.animation('.slide-animation',
    function () {
        return {
            addClass: function (element, className, done) {
                var scope = element.scope();
                if (className === 'ng-hide') {
                    TweenMax.to(element, 0.5, { opacity: 0, onComplete: done });
                }
                else {
                    done();
                }
            },
            removeClass: function (element, className, done) {
                var scope = element.scope();
                if (className == 'ng-hide') {
                    element.removeClass('ng-hide');
                    TweenMax.fromTo(element, 0.5, { opacity: 0 }, { opacity: 1, onComplete: done });
                }
                else {
                    done();
                }
            }
        };
    });


