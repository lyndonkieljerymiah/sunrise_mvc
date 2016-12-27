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
                scope.getTemplate = function () {
                    return scope.template;
                }
            },
            template: "<div ng-include='getTemplate()'></div>",
            controller: function ($scope) {
                $scope.changeType = function ()
                {
                    $scope.template = "register/" + $scope.sales.register.type;
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
                    $scope.sales = response.data;
                    $scope.sales.amount = response.data.villa.ratePerMonth;
                    $scope.template = templatePath + $scope.sales.register.tenantType;
                });
        }

        function changeTenantType() {
            $scope.template = templatePath + $scope.sales.register.tenantType;
        }

        function save()
        {
            
            $http.post("/api/sales/create", $scope.sales)
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