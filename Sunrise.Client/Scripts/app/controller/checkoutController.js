mainApp.directive("tenantRegister",
    function ($http) {

        return {
            restrict: "EA",
            scope: {
                load: "@?",
                tmp: "=",
                tenant: "="
            },
            replace: true,
            link: function (scope) {
                console.log(scope);
                scope.getTemplate = function () {
                    return scope.template;
                }
            },
            template: "<div ng-include='getTemplate()'></div>",
            controller: function ($scope) {
                $scope.changeType = function () {
                    console.log($scope);
                    $scope.template = "register/" + $scope.tenant.type;
                }
            }
        }
    });



mainApp.controller("salesTransactionController",
    function ($scope, $http,$window) {
        var templatePath = "/tenant/register/";

        function init(villaId)
        {
            $http.get("/api/sales/create/" + villaId)
                .then(function(response) {
                    $scope.tenant = response.data.tenant;
                    $scope.sales = response.data.sales;
                    $scope.template = templatePath + $scope.tenant.tenantType;
                });
        }

        function changeTenantType() {
            $scope.template = templatePath + $scope.tenant.tenantType;
        }

        function save() {
            $http.post("/api/sales/create", { tenant: $scope.tenant, sales: $scope.sales })
                .then(function (response)
                {
                    //route data
                    var id = response.data.id;
                    $window.location = "/sales/billing/" + id;
                });
        }

        return {
            init: init,
            changeTenantType: changeTenantType,
            save: save
        }
    });