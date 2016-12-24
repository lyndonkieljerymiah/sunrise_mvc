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
    function ($scope, $http) {
        var templatePath = "/tenant/register/";

        function init(villaId) {

            $http.get("/api/sales/create/" + villaId)
                .then(function(response) {
                    $scope.tenant = response.data.tenant;
                    $scope.sales = response.data.sales;
                    $scope.template = templatePath + $scope.tenant.type;
                });
        }

        function changeTenantType() {
            $scope.template = templatePath + $scope.tenant.type;
        }


        function save()
        {

            $http.post("/api/sales/update", { tenant: $scope.tenant, sales: $scope.sales })
                .then(function(response) {
                    console.log(response);
                });
        }

        return {
            init: init,
            changeTenantType: changeTenantType,
            save: save
        }
    });