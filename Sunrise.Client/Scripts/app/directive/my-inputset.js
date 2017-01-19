mainApp.directive("searchBox", function () {

    return {
        restrict: "EA",
        scope: {
            searchKey: "=",
            action: "&",
            placeholder: "@"
        },
        template: ["<div class='input-group'>",
                   "<input type='text' class='form-control' ng-model='searchKey' placeholder='{{placeholder}}' />",
                   "<span class='input-group-btn'>",
                   "<button class='btn btn-default' ng-click='trigger()'><i class='fa fa-search'></i></button>",
                   "</span></div>"].join(''),
        link: function (scope, element, attrs)
        {
            scope.trigger = function () {
                scope.action();
            }
        }
    }

});


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
                '</div>', '<ng-transclude></ng-transclude>'].join(''),
        link: function (scope, elem, attrs) {
            scope.opened = [];
            scope.toggleDateTimePicker = function ($event, index) {
                $event.preventDefault();
                $event.stopPropagation();
                scope.opened[scope.index] = scope.opened[scope.index] ? false : true;
            }
        }


    }
});

mainApp.directive("inputSet", function () {
    return {
        restrict: 'EA',
        transclude: true,
        scope: {
            myClass: "@",
            myModel: "=",
            myBlur: "&",
            myRequired: "@",
            myNumeric: "@",
            validState: "=",
            errorMsg: "="
        },
        template: ["<div class='{{myClass}}'>",
                   "<input type='{{myType}}' class='form-control' ng-model='myModel' ng-blur='myBlur' ng-required='myRequired' />",
                   "<ng-transclude></ng-transclude>",
                   "</div>"].join(''),
        link: function (scope, el, attrs) {
            var pattern;

            var inp = el[0].childNodes[0].childNodes[0];
            $(inp).on("change", function () {
                if (scope.myNumeric) {
                    switch (scope.myNumeric) {
                        case "currency":
                            isValid = /^\d+$/.test(scope.myModel);
                            break;
                        default:
                            break;
                    }

                    scope.validState = isValid;
                    if (!scope.validState) {
                        scope.errorMsg = "Value must be numeric";
                    }
                }
            });

        }
    }
});