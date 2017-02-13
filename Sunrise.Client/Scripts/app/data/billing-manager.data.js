mainApp.factory("billingDataManager",
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
                                tenant: item.name,
                                totalPayment: item.totalPayment,
                                totalBalance: item.totalBalance,
                                status: item.status
                            });
                        });
                        success(data);
                    });
            },
            createBilling: function (transactionId, success, failure) {
                $http.get(router.apiPath("billing","",transactionId))
                  .then(function (response) {

                      var data = response.data;
                      if (data.payments && data.payments.length > 0) {
                          data.payments.forEach(function (payment)
                          {
                              payment.paymentDate = new Date(payment.paymentDate);
                              payment.paymentDate.setMinutes(payment.paymentDate.getMinutes() + payment.paymentDate.getTimezoneOffset());

                              payment.periodStart = new Date(payment.periodStart);
                              payment.periodStart.setMinutes(payment.periodStart.getMinutes() + payment.periodStart.getTimezoneOffset());
                              payment.periodEnd = new Date(payment.periodEnd);
                              payment.periodEnd.setMinutes(payment.periodEnd.getMinutes() + payment.periodEnd.getTimezoneOffset());

                          });
                      }

                      data.paymentDictionary.initialValue.paymentDate = new Date(data.paymentDictionary.initialValue.paymentDate);
                      data.paymentDictionary.initialValue.periodStart = new Date(data.paymentDictionary.initialValue.periodStart);
                      data.paymentDictionary.initialValue.periodEnd = new Date(data.paymentDictionary.initialValue.periodEnd);

                      success(data);
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
                $http.post(router.apiPath("billing","create"), data).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response));
                    });
            },
            dismiss: function(value,action,failure) {
                $http.post(router.apiPath("billing","dismiss"),value).then(
                    function(response) {
                        action(response.data);
                    },
                    function(response) {
                        failure(response.data);
                    });
            }

    }
});