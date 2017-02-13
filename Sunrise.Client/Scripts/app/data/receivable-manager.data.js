﻿mainApp.factory("receivableDataManager", function ($http, modelStateValidation,router) {
    return {
        get: function(billCode,success,failure) {
            $http.get("/api/receivable/" + billCode + "")
                .then(
                function(response) {
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

                    if (data.reconciles && data.reconciles.length > 0) {

                        data.reconciles.forEach(function (reconcile) {
                            reconcile.date = new Date(reconcile.date);
                            reconcile.date.setMinutes(reconcile.date.getMinutes() + reconcile.date.getTimezoneOffset());

                            reconcile.periodStart = new Date(reconcile.periodStart);
                            reconcile.periodStart.setMinutes(reconcile.periodStart.getMinutes() + reconcile.periodStart.getTimezoneOffset());
                            reconcile.periodEnd = new Date(reconcile.periodEnd);
                            reconcile.periodEnd.setMinutes(reconcile.periodEnd.getMinutes() + reconcile.periodEnd.getTimezoneOffset());
                        });
                    }

                    data.paymentDictionary.initialValue.paymentDate = new Date(data.paymentDictionary.initialValue.paymentDate);
                    data.paymentDictionary.initialValue.periodStart = new Date(data.paymentDictionary.initialValue.periodStart);
                    data.paymentDictionary.initialValue.periodEnd = new Date(data.paymentDictionary.initialValue.periodEnd);
                    data.paymentDictionary.reconcileInitialValue.date = new Date(data.paymentDictionary.reconcileInitialValue.date);
                    data.paymentDictionary.reconcileInitialValue.periodStart = new Date(data.paymentDictionary.reconcileInitialValue.periodStart);
                    data.paymentDictionary.reconcileInitialValue.periodEnd = new Date(data.paymentDictionary.reconcileInitialValue.periodEnd);
                    success(data);
                },
                function (response)
                {   
                    failure(modelStateValidation.parseError(response.data));
                });
        },
        load: function (villaNo, success, failure) {
            //load data
            $http.get("/api/receivable/" + villaNo)
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