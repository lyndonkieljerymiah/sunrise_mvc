mainApp.service("spinnerManager", function ($scope) {

    $scope._spinnerLoading = false;

    return {
        start: function() {
            $scope._spinnerLoading = true;
        },
        stop: function() {
            $scope._spinnerLoading = false;
        }
    }
})
