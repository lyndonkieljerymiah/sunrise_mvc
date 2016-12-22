mainApp.directive("tenantRegister",
    function($http) {

        return {
            restrict: "EA",
            scope: {
                load: "@?",
                template: "="
            },
            replace: true,
            link: function (scope) {
                scope.getTemplate = function () {
                    return "/tenant/template/" + scope.template;    
                }
            },
            template: "<div ng-include='getTemplate()'></div>",
            controller: function ($scope) {

                $scope.tenant = {};
                $scope.template = "register/ttin";

                this.initializeController = function() {
                    $http.get("/api/tenant/create")
                        .then(function(response) {
                            scope.tenants = response.data;
                        });
                }
                $scope.changeType = function () {
                    console.log($scope);
                    $scope.template = "register/" + $scope.tenant.type;
                }
            }
        }
    });

