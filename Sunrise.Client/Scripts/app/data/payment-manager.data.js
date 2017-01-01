mainApp.factory("paymentDataManager",
    function($http,modelStateValidation) {
        return {
            createBilling: function (transactionId, success, failure)
            {
                $http.get("/api/sales/billing/" + transactionId)
                  .then(function (response) {
                      success(response.data);
                  });
            },
            createPayment: function(success,failure) {
                $http.get("/api/sales/payment")
                    .then(
                    function (response) {
                        var payment = response.data;
                        payment.paymentDate = new Date(payment.paymentDate);
                        payment.coveredPeriodFrom = new Date(payment.coveredPeriodFrom);
                        payment.coveredPeriodTo = new Date(payment.coveredPeriodTo);
                        success(payment);

                    });
            },
            save: function(data,success,failure) {
                $http.post("/api/sales/payment", data)
                    .then(
                        function (response) {
                            success(response.data);
                        },
                        function (response) {
                            failure(modelStateValidation.parseError(response.data));
                        }
                    );
            }
        }
    });