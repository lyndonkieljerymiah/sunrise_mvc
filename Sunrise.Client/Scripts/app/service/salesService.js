mainApp.factory("salesService",
    function($http) {

        function getSummary(id,action) {
            $http.get("http://" + window.location.host + "/api/bill/sales/1")
                .then(
                    function(response) {
                        var data = response.data;
                        action(data);
                    });
        }

        return {
            getSummary: getSummary
        }
    });