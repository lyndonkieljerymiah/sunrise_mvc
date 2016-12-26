mainApp.controller("salesController",
    function(salesService,$scope) {

        $scope.data = 
        {
            sales : []
        };

        $scope.loading = false;
        salesService.getSummary(1,function (data)
        {
            $scope.data.sales = data;
            $scope.loading = true;
        });

    });

mainApp.controller("paymentController",
    function($uibModal) {

    });
