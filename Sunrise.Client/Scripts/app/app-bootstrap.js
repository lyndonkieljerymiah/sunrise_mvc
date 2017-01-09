var mainApp = angular.module("mainApp", ["ngRoute", "ui.bootstrap", "ngAnimate","toaster"]);

/*services*/
mainApp.service("spinnerManager",
    function () {
        var $ctrl = this;

        this.spinnerLoading = false;

        this.start = function () {
            $ctrl.spinnerLoading = true;
        }

        this.stop = function () {
            $ctrl.spinnerLoading = false;
        }
    });


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

mainApp.directive("slider",
    function ($timeout) {
        return {
            strict: "EA",
            transclude: true,
            template: "<div class='nb-slide'><ng-transclude></ng-transclude></div>",
            link: function (scope, elem, attr) {

                scope.nbSlides = {
                    images: [
                    {
                        imageUrl: "",
                        visible: false
                    }],
                    currentIndex: 0,
                    next: function () {
                        scope.nbSlides.currentIndex < scope.nbSlides.images.length - 1
                            ? scope.nbSlides.currentIndex++
                            : scope.nbSlides.currentIndex = 0;
                    },
                    prev: function () {
                        scope.nbSlides.currentIndex > 0
                        ? scope.nbSlides.currentIndex--
                        : scope.nbSlides.currentIndex = scope.nbSlides.images.length - 1;
                    },
                    chooseIndex: function (index) {
                        scope.nbSlides.currentIndex = index;
                    }
                }

                scope.$watch('nbSlides.currentIndex', function () {
                    scope.nbSlides.images.forEach(function (image) {
                        image.visible = false; // make every image invisible
                    });
                    scope.nbSlides.images[scope.nbSlides.currentIndex].visible = true; // make the current image visible
                });

                var timer;
                var sliderFunc = function () {
                    timer = $timeout(function () {
                        scope.nbSlides.next();
                        timer = $timeout(sliderFunc, 3000);
                    }, 3000);
                };

                sliderFunc();
                scope.$on('$destroy', function () {
                    $timeout.cancel(timer); // when the scope is getting destroyed, cancel the timer
                });

            }
        }
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


mainApp.directive("spinner",
    function (spinnerManager) {
        return {
            restrict: "EA",
            template: "<div class='spinner' ng-show='_spinnerLoading'><div class='icon'></div><div class='overlay'></div></div>"
        };
    });
