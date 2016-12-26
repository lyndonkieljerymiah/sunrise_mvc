mainApp.controller("shopController", function ($window,$scope, $http) {

    $scope.villas = [];

    function init()
    {
        $http.get("/api/sales/list")
            .then(function (response) {
                $scope.villas = response.data;
            });
    }

    /*
     * When button click select Id and proceed to checkout
     *****************************************************/
    function select(villaId)
    {
        $window.location = "/sales/checkout/" + villaId;
    }

    return {
        init: init,
        select: select
    };
})