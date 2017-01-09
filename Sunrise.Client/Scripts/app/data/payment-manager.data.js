mainApp.factory("paymentDataManager",
    function ($http, modelStateValidation,router) {
        return {
            billingList: function (success, failure) {
                $http.get(router.apiPath("billing","list")).then(
                    function (response) {

                        var data = [];
                        response.data.forEach(function (item) {
                            data.push({
                                id: item.id,
                                dateCreated: item.dateCreated,
                                villaNo: item.villa.villaNo,
                                tenant: item.register.name,
                                totalPayment: item.totalPayment,
                                totalBalance: item.totalBalance,
                                status: item.status
                            });
                        });
                        success(data);
                    });
            },
            createBilling: function (transactionId, success, failure) {
                console.log(router.apiPath("billing", "", transactionId));
                $http.get(router.apiPath("billing","",transactionId))
                  .then(function (response) {
                      var data = response.data;
                      if (data.payments.length > 0) {
                          data.payments.forEach(function (payment) {
                              payment.paymentDate = new Date(payment.paymentDate);
                              payment.paymentDate.setMinutes(payment.paymentDate.getMinutes() + payment.paymentDate.getTimezoneOffset());

                              payment.coveredPeriodFrom = new Date(payment.coveredPeriodFrom);
                              payment.coveredPeriodFrom.setMinutes(payment.coveredPeriodFrom.getMinutes() + payment.coveredPeriodFrom.getTimezoneOffset());
                              payment.coveredPeriodTo = new Date(payment.coveredPeriodTo);
                              payment.coveredPeriodTo.setMinutes(payment.coveredPeriodTo.getMinutes() + payment.coveredPeriodTo.getTimezoneOffset());

                          });
                      }

                      data.paymentDictionary.initialValue.paymentDate = new Date(data.paymentDictionary.initialValue.paymentDate);
                      data.paymentDictionary.initialValue.coveredPeriodFrom = new Date(data.paymentDictionary.initialValue.coveredPeriodFrom);
                      data.paymentDictionary.initialValue.coveredPeriodTo = new Date(data.paymentDictionary.initialValue.coveredPeriodTo);


                      success(response.data);
                  });
            },
            validate: function (data, success, failure) {
                $http.post(router.apiPath("billing","validate"), data)
                    .then(
                        function (response) {
                            success(response.data);
                        },
                        function (response) {
                            failure(modelStateValidation.parseError(response.data));
                        }
                    );
            },
            save: function (data,action,failure) {
                $http.post(router.apiPath("billing","save"), data).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response));
                    });
            }
        }
    });