mainApp.config(function ($routeProvide, $locationProvider) {
    $routeProvide
        .when("/", {
            templateUrl: "/contract/showcase",
            controller: "shopController"
        })
        .when("/contract/vid", {
            templateUrl: "/contract/form"
        })
})