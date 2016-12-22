mainApp.controller("shopController", function($scope,villaService,salesService) {

    $scope.villas = [];

    /*
     * Initial display all villas 
     */
    villaService.getVillas(function(data) {
        $scope.villas = data;
    });


    /*
     * When button click select Id and proceed to checkout
     *****************************************************/
    $scope.select = function (villaId) {
        //select id and proceed to checkout
        salesService.checkout(villaId);
    }
})