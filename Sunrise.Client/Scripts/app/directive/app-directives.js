/*Tenant Directive */
mainApp.directive("tenantform", function () {
    return {
        restrict: 'EA',
        link: function (scope, elem) {
            scope.getTemplate = function () {
                return scope.data.template;
            };
        },
        template: '<div ng-include="getTemplate()"></div>'
    };
});

/*Villa Directive */
mainApp.directive("villa", function () {
    return {
        restrict: 'EA',
        link: function(scope) {
            scope.getSalesTemplate = function() {
                return scope.data.salesTemplate;
            }
        },
        template: '<div ng-include="getSalesTemplate()"></div>'
    };
});

/*Payment Directive*/
mainApp.directive("payment", function () {
    return {
        replace: true,
        restrict: "EA",
        templateUrl: 'template/payment.html'
    };
});