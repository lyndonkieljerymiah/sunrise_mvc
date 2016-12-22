mainApp.factory("villaService",

    function ($http) {

        var villa = {};

        function getVilla(villaNo, action) {
            $http.get("/api/villa/" + villaNo)
                        .then(function (response) {
                            action(response.data);
                });
        }

        function getVillas(action) {
            $http.get("/api/villa")
                .then(function(response) {
                    action(response.data);
                });
        }

        return {
            getVilla: getVilla,
            getVillas: getVillas
        }
    });