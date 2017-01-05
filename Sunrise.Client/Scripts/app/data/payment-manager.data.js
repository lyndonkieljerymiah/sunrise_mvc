mainApp.factory("paymentDataManager",
    function($http,modelStateValidation) {
        return {
            createBilling: function (transactionId, success, failure)
            {
                $http.get("/api/billing/" + transactionId)
                  .then(function (response) {
                      success(response.data);
                  });
            },
            createPayment: function(success,failure) {
                $http.get("/api/billing/payment")
                    .then(
                    function (response) {
                        var payment = response.data;

                        payment.paymentDate = new Date(payment.paymentDate);
                        payment.paymentDateObject = {
                            isOpen: false,
                            format: 'MM/dd/yyyy',
                            toggle: function () {
                                payment.paymentDateObject.isOpen = payment.paymentDateObject.isOpen ? false : true;
                            }
                        };

                        payment.coveredPeriodFrom = new Date(payment.coveredPeriodFrom);
                        payment.coveredPeriodFromObject = {
                            isOpen: false,
                            format: 'MM/dd/yyyy',
                            toggle: function () {
                                payment.coveredPeriodFromObject.isOpen = payment.coveredPeriodFromObject.isOpen ? false : true;
                            }
                        };

                        payment.coveredPeriodTo = new Date(payment.coveredPeriodTo);
                        payment.coveredPeriodToObject = {
                            isOpen: false,
                            format: 'MM/dd/yyyy',
                            toggle: function () {
                                payment.coveredPeriodToObject.isOpen = payment.coveredPeriodToObject.isOpen ? false : true;
                            }
                        };

                        success(payment);
                    });
            },
            save: function(data,success,failure) {
                $http.post("/api/billing/payment", data)
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