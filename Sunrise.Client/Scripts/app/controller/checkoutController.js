mainApp.directive("tenantRegister",
    function ($http) {
        return {
            restrict: "EA",
            replace: true,
            link: function (scope) {
                scope.getTemplate = function () {
                    return scope.template;
                }
            },
            template: "<div ng-include='getTemplate()'></div>"
        }
    });




mainApp.controller("salesTransactionController",

    function ($scope, $http, $window, modelStateValidation) {
        var templatePath = "/tenant/register/";

        $scope.sales = {};
        $scope.loading = true;

        function restart() {
            $scope.errorState = {};
            $scope.loading = false;
        }

        function init(villaId) {

            $scope.loading = true;
            $http.get("/api/sales/create/" + villaId)
                .then(function (response) {
                    $scope.sales = response.data;
                    $scope.sales.periodStart = new Date($scope.sales.periodStart);
                    $scope.sales.periodEnd = new Date($scope.sales.periodEnd);
                    $scope.template = templatePath + $scope.sales.register.tenantType;
                    restart();
                });
        }

        function changeTenantType() {

            $scope.loading = true;
            $http.get("/api/sales/create/" + $scope.sales.villa.id + "/" + $scope.sales.register.tenantType)
                .then(function (response) {
                    $scope.sales = response.data;
                    $scope.sales.periodStart = new Date($scope.sales.periodStart);
                    $scope.sales.periodEnd = new Date($scope.sales.periodEnd);
                    $scope.template = templatePath + $scope.sales.register.tenantType;
                    restart();
                });

        }

        function save() {
            $scope.loading = true;
            $http.post("/api/sales/create", $scope.sales)
                .then(
                function (response) {
                    //route data
                    var id = response.data.id;
                    $window.location = "/sales/billing/" + id;
                    restart();
                },
                function errorCallback(response) {
                    restart();
                    $scope.errorState = modelStateValidation.parseError(response.data);
                    $scope.loading = false;
                });
        }
        return {
            init: init,
            changeTenantType: changeTenantType,
            save: save
        }
    });