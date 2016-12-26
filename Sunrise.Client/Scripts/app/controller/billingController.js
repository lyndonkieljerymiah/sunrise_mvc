mainApp.controller("billingController",
    function($http,$scope) {
        function init(transactionId) {
            console.log(transactionId);
            $http.get("/api/sales/billing/" + transactionId)
                .then(function(response) {
                    $scope.tenant = response.data.tenant;
                    $scope.sales = [response.data.sales];
                    console.log($scope);
                });
        }

        return {
            init: init
        }
    });