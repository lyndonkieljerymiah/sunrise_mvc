mainApp.factory("contractDataManager",
    function ($http, modelStateValidation, router) {
        return {
            list: function (search, action, failure) {
                $http.get(router.apiPath("contract", "list", search)).then(
                    function (response)
                    {
                        var row = [];
                        if (response.data.length > 0) {
                            angular.forEach(response.data, function (item) {
                                var col = {
                                    id: item.id,
                                    code: item.code,
                                    villaNo: item.villaNo,
                                    tenant: item.tenantName,
                                    dateCreated: new Date(item.dateCreated),
                                    periodStart: new Date(item.periodStart),
                                    periodEnd: new Date(item.periodEnd),
                                    amountPayable: item.amountPayable,
                                    status: item.status,
                                    editState: item.editState
                                }
                                row.push(col);
                            });
                        }
                        action(row);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    });
            },
            redirectToContract: function (villaId) {
                router.route("contract", "", villaId);
            },
            createContract: function (villaId, tenantType, success, failure) {

                var uriPath = (tenantType === null) ?
                    router.apiPath("contract", "register", villaId) :
                    router.apiPath("contract", "register", villaId + "/" + tenantType);

                $http.get(uriPath)
                  .then(
                      function (response) {

                          var data = response.data;
                          data.periodStart = new Date(data.periodStart);
                          data.periodEnd = new Date(data.periodEnd);
                          if (data.register.individual)
                          {
                              data.register.individual.gender = data.register.individual.gender.toString();
                              data.register.individual.birthday = new Date(data.register.individual.birthday);
                          }
                          else {
                              data.register.company.validityDate = new Date(data.register.company.validityDate);
                          }

                          data.template = data.register.tenantType;
                          
                          success(data);
                      },
                      function (response) {
                          failure(modelStateValidation.parseError(response.data));
                      }
                    );
            },
            save: function (data, success, failure)
            {
                data.villa.imageGalleries = null;
                $http.post(router.apiPath("contract", "register"), data).then(responseCallback, errorCallback)
                function responseCallback(response) {
                    //route data
                    var responseData = response.data;
                    success(responseData);
                }
                function errorCallback() {
                    failure(modelStateValidation.parseError(response.data));
                }
            },
            proceedToBilling: function (id) {
                router.route("billing", "",id);
            },
            cancel: function (id, action, failure) {
                $http.post(router.apiPath("contract", "cancel"), { id: id }).then(
                    function (response) {
                        action(response);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    });
            }
        }
    });

mainApp.factory("contractRenewalManager", function ($http, modelStateValidation,router) {

    return {
        expires: function (action, failure) {
            $http.get(router.apiPath("contract", "expiries")).then(
                function (response) {
                    var row = [];
                    if (response.data.length > 0) {
                        angular.forEach(response.data, function (item) {
                            var col = {
                                id: item.id,
                                code: item.code,
                                villaNo: item.villaNo,
                                tenant: item.tenantName,
                                dateCreated: new Date(item.dateCreated),
                                periodStart: new Date(item.periodStart),
                                periodEnd: new Date(item.periodEnd),
                                amountBalance: item.amountBalance,
                                status: item.status,
                                monthDue: item.monthDue
                            }
                            row.push(col);
                        });
                    }
                    action(row);
                },
                function (response) {
                    failure(modelStateValidation.parseError(response.data));
                }
            );
        },
        renewal: function (id, action, failure) {
            $http.get(router.apiPath("contract", "renewal", id)).then(
                function (response) {
                    var data = response.data;
                    data.periodStart = new Date(data.periodStart);
                    data.periodEnd = new Date(data.periodEnd);
                    action(data);
                },
                function (response) {
                    failure(modelStateValidation.parseError(response.data));
                });
        },
        renew: function (value, action, failure) {

            var data = {
                id: value.id,
                code: value.code,
                periodStart: value.periodStart,
                periodEnd: value.periodEnd,
                rentalTypeCode: value.rentalTypeCode,
                contractStatusCode: value.contractStatusCode,
                amountPayable: value.amountPayable,
                tenantId: value.tenantId,
                villaId: value.villaId
            };

            //form
            $http.post(router.apiPath("contract", "renewal"), data).then(
                function (response) {
                    if(response.data.success)
                        action(response.data);
                    else
                        failure(modelStateValidation.parseError(response.data));
                },
                function (response) {
                    failure(modelStateValidation.parseError(response.data));
                });
        },
        proceedToBilling: function (id) {
            router.route("billing", "", id);
        }
        
    }

});




