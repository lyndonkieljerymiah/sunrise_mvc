(function () {
    mainApp.directive("spinner",
        function () {
            return {
                restrict: "EA",
                template: "<div class='spinner' ng-show='_spinnerLoading'><div class='icon'></div><div class='overlay'></div></div>",
                link: function () {

                }
            };
        });

    mainApp.factory("spinnerManager",spinnerManager);

    function spinnerManager() {
        var service ={
            scope: {},
            start: function () {
                service.scope._spinnerLoading = true;
            },
            stop: function (callback) {
                service.scope._spinnerLoading = false;
                if (callback)
                    callback();
            }
        }

        return service;
    }

})();