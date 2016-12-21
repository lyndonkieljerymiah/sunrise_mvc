
/*Tenant Directive */
mainApp.directive("tenantForm", function () {
    return {
        restrict: 'EA',
        link: function (scope, elem) {
            scope.getTemplate = function () {
                return scope.template;
            };
        },
        template: '<div ng-include="getTemplate()"></div>'
    };
});

/*Villa Directive */
mainApp.directive("villa", function () {
    return {
        restrict: 'EA',
        link: function (scope) {
            scope.getSalesTemplate = function () {
                return scope.salesTemplate;
            }
        },
        template: '<div ng-include="getSalesTemplate()"></div>',
        controller: function ($scope, villaService)
        {
            $scope.reset = function ()
            {
                $scope.showWarning = false;
                $scope.villaModel = null;
            }

            $scope.checkAvailability = function ()
            {
                var villaNo = $scope.tenant.sales[0].villaNo;
                villaService.checkAvailability(villaNo,
                    function(data) {
                        $scope.villaModel = data;
                        $scope.showWarning = true;
                    });
            }
        }
    };
});



mainApp.controller("tenantRegisterController", function ($scope, tenantService) {

    var ctrl = this;
    var templateUrl = "";
    
    //select tenant
    tenantService.create(function(data) {
        $scope.tenant = data;
        $scope.tenantTypeOnChange();
    });

    function getTemplateStep(step)
    {
        if (step === 2) {
            $scope.salesTemplate = tenantService.getSalesView();
        }
        if (step === 3)
        {   
            tenantService.update($scope.tenant);
        }
    }

    $scope.tenantTypeOnChange = function() {

        $scope.template = tenantService.getTenantView($scope.tenant.type);

    };
    $scope.step = {
        current: 1,
        stepUp: function ()
        {
            if ($scope.step.current < 3)
                $scope.step.current++;
            getTemplateStep($scope.step.current);
        },
        stepDown: function() {
            if ($scope.step.current > 1)
                $scope.step.current--;
            getTemplateStep($scope.step.current);
        }
    }
});

