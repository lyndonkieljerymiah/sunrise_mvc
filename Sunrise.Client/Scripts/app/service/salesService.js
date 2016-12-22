mainApp.factory("salesService",

    function ($http,$window) {


        function getSummary(id, action) {
            $http.get("/api/bill/sales/1")
                .then(
                    function(response) {
                        var data = response.data;
                        action(data);
                    });
        }

        function checkout(villaId) {
            $window.location = "/sales/checkout/" + villaId;
        }

        return {
            getSummary: getSummary,
            checkout: checkout
        }
    });