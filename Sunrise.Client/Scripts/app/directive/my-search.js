mainApp.directive("searchBox", function () {

    return {
        restrict: "EA",
        scope: {
            searchKey: "=",
            action: "&",
            placeholder: "@"
        },
        template: ["<div class='input-group'>",
                   '<input type="text" class="form-control" placeholder="Search Villa" ng-model="searchKey"/>',
                   "<span class='input-group-btn'>",
                   "<button class='btn btn-default' ng-click='trigger()'><i class='fa fa-search'></i></button>",
                   "</span></div>"].join(''),
        link: function(scope,element,attrs) {
            scope.trigger = function () {
                scope.action();
            }
        }
    }

});