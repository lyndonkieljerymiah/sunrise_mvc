mainApp.factory("router", function ($window) {
    return {
        route: function (controller, action, value) {

            var path = "/" + controller + "/" + action + value;
            $window.location = path;

        },
        apiPath: function (controller, action, value) {
            var path;
            if (action && action.length > 0) {
                path = "/api/" + controller + "/" + action;
            }
            else {
                path = "/api/" + controller;
            }
            
            if (value) {
                path = path + "/" + value;
            }
            console.log(path);
            return path;
        }
    }
});