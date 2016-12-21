mainApp.factory("villaService",
    function($http) {
        var villa = {};

        function checkAvailability(villaNo, action) {
            $http.get("/api/villa/availability/" + villaNo)
                        .then(function (response) {
                            villa = response.data;
                            action(response.data);
                });
        }

        return {
            checkAvailability: checkAvailability
        }
    });