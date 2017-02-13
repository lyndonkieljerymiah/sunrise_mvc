mainApp.directive("searchBox", function () {

    return {
        restrict: "EA",
        replace: true,
        scope: {
            searchKey: "=",
            action: "&",
            placeholder: "@"
        },
        template: ["<div class='input-group'>",
                   "<input type='text' class='form-control' ng-model='searchKey' placeholder='{{placeholder}}' ng-init='' />",
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
        replace: false,
        scope: {
            myName: "@",
            myId: "@",
            format: "@",
            model: "=",
            index: "@",
            required: "@"
        },
        template: ['<div class="input-group">',
                '<input id={{my-id}} name={{my-id}} type="text" class="form-control" uib-datepicker-popup="MM/dd/yyyy" ng-model="model" is-open="opened[index]" datepicker-options="dateOptions" alt-input-formats="altInputFormats" ng-required="required" />',
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
            errorMsg: "=",
            myType: "@",
            myId : "@"
        },
        template: ["<div class='{{myClass}}'>",
                   "<input type='{{myType}}' class='form-control' ng-model='myModel' ng-blur='myBlur' id='{{myId}}' name='{{myId}}' />",
                   "<ng-transclude></ng-transclude>",
                   "</div>"].join(''),
        link: function (scope, el, attrs) {

            var pattern;

            //check input validation
            var inp = el.find("input");
            
            inp.bind("change", function ()
            {   
                if (scope.myNumeric) {
                    scope.validState = doValidation(scope.myNumeric);
                    if (!scope.validState) {
                        scope.errorMsg = "Value must be numeric";
                    }
                }
                else if (scope.myRequired) {
                    if (scope.myModel.length === 0) {
                        scope.validState = false;
                        scope.errorMsg = "Field is required";
                    }
                    else {
                        scope.validState = true;
                    }
                }
            });

            function doValidation(numeric) {
                var isValid = true;
                switch (numeric) {
                    case "currency":
                        if (scope.myModel === null || scope.myModel.length == 0) {
                            scope.myModel = 0;
                        }
                        isValid = /^(\d*?)(\.\d{1,2})?$/.test(scope.myModel);
                        break;
                    case "number":
                        if (scope.myModel === null || scope.myModel.length == 0) {
                            scope.myModel = 0;
                        }
                        isValid = /^\d+$/.test(scope.myModel);
                    default:
                        break;
                }
                return isValid;
            }
        }
    }
});

mainApp.directive("modelStateAttribute", function () {
    return {
        restrict: "A",
        link: function (scope, elem, attrs) {
            var formObject = $(elem);
            scope.errorState = {
                validate: function () {
                    //make it clean at the first time
                    scope.errorState.clear = true;
                    scope.errorState.error = {};

                    var allRequired = formObject.find("[data-val-required]");
                    if (allRequired.length > 0) {
                        for (var i = 0; i <= allRequired.length; i++) {
                            var input = allRequired[i];
                            if (input !== undefined) {
                                console.log($(input).val());
                                if ($(input).val().trim().length === 0 || $(input).val() === undefined) {
                                    scope.errorState.error[$(input).attr("ng-model")] = $(input).attr("data-val-required");
                                    scope.errorState.clear = false;
                                }
                            }
                        }
                    }
                },
                clear: true,
                error: {}
            };
            function doValidation(type, value) {
                var errorMessage = "";
                switch (type) {
                    case "currency":
                        if (value === null || value.trim().length == 0) {
                            errorMessage = "value must be bumeric";
                        }
                        else {
                            isValid = /^(\d*?)(\.\d{1,2})?$/.test(value);
                            if (!isValid)
                                errorMessage = "value must be bumeric";
                        }
                        break;
                    case "number":
                        if (value === null || value == 0) {
                            errorMessage = "value must be bumeric";
                        }
                        else {
                            isValid = /^\d+$/.test(value);
                            if (!isValid)
                                errorMessage = "value must be bumeric";
                        }
                    default:
                        break;
                }
                return errorMessage;
            }
        }
    }
});