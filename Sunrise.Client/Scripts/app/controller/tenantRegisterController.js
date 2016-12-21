
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
        template: '<div ng-include="getSalesTemplate()"></div>'
    };
});

mainApp.controller("tenantRegisterController", function ($scope, tenantService) {

    var ctrl = this;
    var templateUrl = "";

    $scope.tenant  = new tenantService();

    //select tenant
    $scope.tenant.create(function() {
        $scope.tenantTypeOnChange();
        
    });

    function getTemplateStep(step)
    {
        if (step === 2) {
            $scope.salesTemplate = $scope.tenant.getSalesView();
        }
        if (step === 3)
        {
            
            $scope.tenant.update();
        }
    }

    $scope.tenantTypeOnChange = function() {
        $scope.template = $scope.tenant.getTenantView($scope.tenant.type);
    };

    $scope.step = {
        current: 1,
        stepUp: function() {
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