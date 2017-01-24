mainApp.directive("slider",
    function ($timeout) {
        return {
            strict: "EA",
            transclude: true,
            template: "<div class='nb-slide'><ng-transclude></ng-transclude></div>",
            link: function (scope, elem, attr)
            {
                scope.nbSlides = {
                    images: [
                    {
                        imageConverted: "",
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
