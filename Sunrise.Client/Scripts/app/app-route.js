mainApp.config(function ($routeProvide,$locationProvider) {
    $routeProvide
        .when("/contract", {
            templateUrl: "/home/index"
        })
        .when("/contract/vid", {
            templateUrl: "/contract/form"
        })
})