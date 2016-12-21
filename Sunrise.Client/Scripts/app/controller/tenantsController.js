mainApp.controller("tenantsController",function($scope,tenantService) {

    $scope.tenants = new tenantService();
    $scope.tenants.list();
    console.log($scope.tenants);

});