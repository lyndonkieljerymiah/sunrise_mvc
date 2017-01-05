mainApp.controller("shopController",function ($scope,villaDataManager,contractDataManager) {

    var $ctrl = this;
    $scope.villas = [];

    /*
     * TODO: Initialize
     ***************************/
    function init() {
        $scope._spinnerLoading = true;
        villaDataManager.getAllVillas(
            function (data) {
                $scope.villaGroups = data.villaGroups;
                $scope._spinnerLoading = false;
            });
    }

    /*
     * TODO: proceed to checkout
     *****************************************************/
    function select(villaId) {
        contractDataManager.redirectToContract(villaId);
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