mainApp.controller("shopController", function ($scope, villaDataManager, contractDataManager, spinnerManager) {

    var $ctrl = this;
    $scope.villas = [];
    spinnerManager.scope = $scope;

    /*
     * TODO: Initialize
     ***************************/
    function init() {
        //$scope._spinnerLoading = true;
        spinnerManager.start();
        villaDataManager.getAllVillas(
            function (data) {
                $scope.villaGroups = data.villaGroups;
                spinnerManager.stop();
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