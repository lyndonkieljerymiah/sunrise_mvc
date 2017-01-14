mainApp.factory("receivableDataManager", function ($http, modelStateValidation,router) {
    return {
        load: function (villaNo, success, failure) {
            //load data
            $http.get("/api/receivable/create/" + villaNo)
                .then(
                    function (response) {
                        var data = response.data;
                        success(data);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    });
        },
        loadUpdate: function (id,success,failure) {
            $http.get("/api/receivable/update/" + id)
                .then(function (response)
                {
                    var data = {
                        chequeNo: response.data.chequeNo,
                        status: response.data.status,
                        statusCode: response.data.statusCode,
                        statusDate: response.data.statusDate,
                        remarks: response.data.remarks,
                        statuses: response.data.statuses
                    };
                    success(data);
                },
                function (response)
                {

                });
        },
        update: function (data,success,failure) {
            $http.post("/api/receivable/update", data)
                .then(function (response) {
                    success(response.data);
                });
        },
        reverse: function (data, action, failure)
        {
            $http.post("api/receivable/reverse", data)
                .then(function (response)
                {
                    console.log(response.data);
                    action(response.data);
                },
                function (response)
                {
                    failure(response.data);
                });
        }
    };
});