

var mainApp = angular.module("mainApp", ["ngRoute","ui.bootstrap","ngAnimate"]);


mainApp.directive('spinner',
    function () {
        return {
            restrict: "EA",
            replace: true,
            scope: {
                showLoading: "="
            },
            template: "<div class='spinner' ng-show='showLoading'><div class='icon'></div><div class='overlay'></div></div>"
        }
    });

mainApp.directive("slider", function () {
    return {
        strict: "EA",
        transclude: true,
        template: "<div class='nb-slide'><ng-transclude></ng-transclude></div>",
        controller: function ($scope) {
            $scope.nbSlides = {
                interval: 5000,
                currentIndex: 0,
                setCurrentSlide: function (index) {
                    $scope.nbSlides.currentIndex = index;
                },
                isCurrentSlide: function (index) {
                    return $scope.nbSlides.currentIndex === index;
                },
                prev: function (length) {
                    $scope.nbSlides.currentIndex--;
                    if ($scope.nbSlides.currentIndex < 0)
                        $scope.nbSlides.currentIndex = length - 1;
                },
                next: function (length) {
                    $scope.nbSlides.currentIndex++;
                    if ($scope.nbSlides.currentIndex > length - 1)
                        $scope.nbSlides.currentIndex = 0;
                }
            }
        }
    }
});

mainApp.factory('modelStateValidation',
    function() {
        
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

