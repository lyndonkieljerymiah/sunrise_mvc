var mainApp = angular.module("mainApp", ["ngRoute", "ui.bootstrap", "ngAnimate", "toaster", "ngFileUpload"]);


mainApp.factory('modelStateValidation',
    function () {

        function parseError(response) {
            var error = {};
            for (var key in response.modelState) {
                error[key.toLowerCase()] = response.modelState[key][0];
            }
            return error;
        }

        return {
            parseError: parseError
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


