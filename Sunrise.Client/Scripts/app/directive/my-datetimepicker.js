mainApp.directive("dateTimePicker", function () {
    return {
        restrict: "EA",
        transclude: true,
        scope: {
            format: "@",
            model: "=",
            index: "@",
            required: "@"
        },
        template: ['<div class="input-group">',
                '<input type="text" class="form-control" uib-datepicker-popup="MM/dd/yyyy" ng-model="model" is-open="opened[index]" datepicker-options="dateOptions" alt-input-formats="altInputFormats" ng-required="required" />',
                '<span class="input-group-btn">', '<button type="button" class="btn btn-default" ng-click="toggleDateTimePicker($event)"><i class="fa fa-calendar"></i></button></span>',
                '</div>','<ng-transclude></ng-transclude>'].join(''),
        link: function (scope, elem, attrs) {
            scope.opened = [];
            scope.toggleDateTimePicker = function ($event,index) {
                $event.preventDefault();
                $event.stopPropagation();
                scope.opened[scope.index] = scope.opened[scope.index] ? false : true;
            }
        }


    }
});