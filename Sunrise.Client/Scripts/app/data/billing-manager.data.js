mainApp.factory("billingDataManager",
    function ($http, modelStateValidation, router) {
        return {
            billingList: function (success, failure) {
                $http.get(router.apiPath("billing", "list")).then(
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
                $http.get(router.apiPath("billing", "", transactionId))
                  .then(function (response) {

                      var data = response.data;
                      if (data.payments && data.payments.length > 0) {
                          data.payments.forEach(function (payment) {
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
                $http.post(router.apiPath("billing", "validate"), data)
                    .then(
                        function (response) {
                            success(response.data);
                        },
                        function (response) {
                            failure(modelStateValidation.parseError(response.data));
                        }
                    );
            },
            save: function (data, action, failure) {
                $http.post(router.apiPath("billing", "create"), data).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response));
                    });
            },
            dismiss: function (value, action, failure) {
                $http.post(router.apiPath("billing", "dismiss"), value).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        failure(response.data);
                    });
            }
        }
    });

mainApp.factory("BillingDataService", function ($http, modelStateValidation, router) {
    function BillingDataService(billingData) {
        this.setData(billingData);
    }

    BillingDataService.prototype = {
        setData: function (billingData) {
            angular.extend(this, billingData);
        },
        create: function (transactionId, succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("billing", "", transactionId)).then(
                function (respSuccess) {
                    var data = respSuccess.data;
                    if (data.payments && data.payments.length > 0) {
                        data.payments.forEach(function (payment) {
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
                    scope.setData(data);
                    succCb();
                },
                function (respError) {
                    errCb(modelStateValidation.parseError(respError.data));
                }
            );
        },
        save: function (succCb, errCb) {
            //verify if payment is completed
            if (this.totalReceived >= this.amount) {
                $http.post(router.apiPath("billing", "create"), this).then(
                   function (response) {
                       succCb(response.data);
                   },
                   function (response) {
                       errCb(modelStateValidation.parseError(response.data));
                   });
            }
            else {
                errCb();
            }

        },
        getInitialValue: function (key) {
            return angular.copy(this.paymentDictionary.initialValue)
        },
        getPayment: function (index) {
            return angular.copy(this.payments[index]);
        },
        addNewPayment: function (data) {
            if (data.paymentTypeCode == "ptcs") {
                data.statusCode = "psc";
                data.isClear = true;
            }
            else {
                data.statusCode = "psv";
                data.isClear = false;
            }
            //return status
            angular.forEach(this.paymentDictionary.statuses, function (item) {
                if (data.statusCode == item.value) {
                    data.statusDescription = item.text;
                    return;
                }
            });

            //return payment
            angular.forEach(this.paymentDictionary.modes, function (item) {
                if (data.paymentModeCode == item.value) {
                    data.paymentModeDescription = item.text;
                    return;
                }
            });
            this.payments.push(angular.copy(data));
        },
        editPayment: function (data, index) {
            if (data.paymentTypeCode == "ptcs") {
                data.statusCode = "psc";
                data.isClear = true;
            }
            else {
                data.statusCode = "psv";
                data.isClear = false;
            }

            angular.forEach(this.paymentDictionary.statuses, function (item) {
                if (data.statusCode == item.value) {
                    data.statusDescription = item.text;
                    return;
                }
            });
            //return payment
            angular.forEach(this.paymentDictionary.modes, function (item) {
                if (data.paymentModeCode == item.value) {
                    data.paymentModeDescription = item.text;
                    return;
                }
            });
            angular.copy(data, this.payments[index]);
        },
        removePayment: function (index) {
            this.payments.splice(index, 1);
        },
        updateTotal: function () {
            this.totalReceived = 0;
            this.totalCleared = 0;

            //update total received
            if (this.payments && this.payments.length > 0) {
                for (var i = 0; i < this.payments.length; i++) {
                    if (this.payments[i].isClear)
                        this.totalCleared = this.totalCleared + this.payments[i].amount;
                }
                this.totalReceived = this.payments.sum("amount");
            }
            this.balance = this.amount - this.totalCleared;
        },
        checkItemCollide: function () {
            //update total received
            var conflictIndex = -1;

            if (this.payments && this.payments.length > 0) {
                //clear it out 
                for (var i = 0; i < this.payments.length; i++) {
                    this.payments[i].collideDetected = false;
                }
                for (var i = 0; i < this.payments.length; i++) {
                    if(!this.payments[i].isClear) {
                        conflictIndex = this.payments.getConflictIndex(this.payments[i], "chequeNo", i);
                        if (conflictIndex >= 0) {
                            this.payments[i].collideDetected = true;
                            this.payments[conflictIndex].collideDetected = true;
                        }   
                    }   
                }
            }
        }
    }
    return BillingDataService;
});