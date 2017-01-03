mainApp.controller("shopController",function ($scope,villaDataManager,salesDataManager) {

    var $ctrl = this;
    $scope.villas = [];

    /*
     * TODO: Initialize
     ***************************/
    function init() {
        $scope._spinnerLoading = true;
        villaDataManager.getAllVillas(
            function (data) {
                console.log($scope);
                $scope.villaGroups = data.villaGroups;
                $scope._spinnerLoading = false;
            });
    }

    /*
     * TODO: proceed to checkout
     *****************************************************/
    function select(villaId) {
        salesDataManager.redirectToCheckout(villaId);
    }

    /*
     * TODO: show detail
     ***************************************************/
    function showDetail(villaId) {
        
    }

    return {
        init: init,
        select: select
    };
});