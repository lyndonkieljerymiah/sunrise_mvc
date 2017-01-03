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
    function ($scope, salesDataManager) {

        var templatePath = "/tenant/register/";
        var isPageLoad = false;
        $scope.sales = {};
        function start() {
            $scope._spinnerLoading = true;
        }
        function stop() {
            $scope._spinnerLoading = false;
        }
        function restart() {
            $scope.errorState = {};
            isPageLoad = false;
            stop();
        }

        function create(villaId, tenantType) {
            $scope._spinnerLoading = true;
            isPageLoad = true;

            salesDataManager.createCheckout(villaId, tenantType,
                function (data)
                {
                    $scope.sales = data;
                    $scope.nbSlides.images = data.images;
                    $scope.template = templatePath + data.register.tenantType;
                    restart();
                });
        }

        function init(villaId) {

            create(villaId, null);
        }

        function changeTenantType() {
            if (!isPageLoad) {
                create($scope.sales.villa.id, $scope.sales.register.tenantType);
            }

        }

        function save() {
            start();
            //reverse
            $scope.sales.register.gender = parseInt($scope.sales.register.gender);
            salesDataManager.save($scope.sales,
                function(data) {
                    var id = data.id;
                    salesDataManager.proceedToBilling(id);
                    restart();
                },
                function (data) {
                    $scope.errorState  = data;
                    stop();
                });
        }
        return {
            init: init,
            changeTenantType: changeTenantType,
            save: save
        }
    });