mainApp.factory("contractDataManager",
    function ($http, modelStateValidation, router) {
        return {
            list: function (search, action, failure) {
                $http.get(router.apiPath("contract", "official", search)).then(
                    function (response) {
                        var row = [];
                        if (response.data.length > 0) {
                            angular.forEach(response.data, function (item) {
                                var col = {
                                    id: item.id,
                                    code: item.code,
                                    villaNo: item.villaNo,
                                    tenant: item.name,
                                    dateCreated: new Date(item.dateCreated),
                                    periodStart: new Date(item.periodStart),
                                    periodEnd: new Date(item.periodEnd),
                                    amountPayable: item.amount,
                                    status: item.statusDescription,
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
            getActive: function (search, action, failure) {
                $http.get(router.apiPath("contract", "active", search)).then(
                    function (response) {
                        var row = [];
                        if (response.data.length > 0) {
                            angular.forEach(response.data, function (item) {
                                var col = {
                                    id: item.id,
                                    code: item.code,
                                    villaNo: item.villaNo,
                                    tenant: item.name,
                                    dateCreated: new Date(item.dateCreated),
                                    periodStart: new Date(item.periodStart),
                                    periodEnd: new Date(item.periodEnd),
                                    amountPayable: item.amount,
                                    monthDue: item.monthDue,
                                    status: item.statusDescription,
                                    editState: item.editState
                                }
                                row.push(col);
                            });
                        }
                        action(row);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    })
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
                          if (data.register.individual) {
                              data.register.individual.gender = data.register.individual.gender.toString();
                              data.register.individual.birthday = new Date(data.register.individual.birthday);
                          }
                          else {
                              data.register.company.validityDate = new Date(data.register.company.validityDate);
                          }
                          data.template = "/tenant/register/";
                          success(data);
                      },
                      function (response) {
                          failure(modelStateValidation.parseError(response.data));
                      }
                    );
            },
            save: function (data, success, failure) {
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
                router.route("billing", "", id);
            },
            remove: function (id, action, failure) {
                $http.post(router.apiPath("contract", "remove"), { id: id }).then(
                    function (response) {
                        action(response);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    });
            },
            terminate: function (data, action, failure) {
                $http.post(router.apiPath("contract", "terminate"), data).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    }
                );
            },
            getContractForTermination: function (id, action, failure) {
                $http.get(router.apiPath("contract", "terminate", id)).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        action(modelStateValidation.parseError(response.data));
                    }
                );
            },
            getContractForRenewal: function (id, action, failure) {
                $http.get(router.apiPath("contract", "renewal", id)).then(
                    function (response) {
                        action(response.data);
                    },
                    function (response) {
                        failure(modelStateValidation.parseError(response.data));
                    }

                );
            }
        }
    });


mainApp.factory("ContractListDataService", ["$http", "router", "modelStateValidation", function ($http, router, modelStateValidation) {

    function ContractListDataService(contractData) {
        this.dataSet = [];
        this.setData(contractData);
    }

    ContractListDataService.prototype = {
        setData: function (contractData) {
            angular.extend(this.dataSet, contractData);
        },
        loadOfficial: function (succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("contract", "official")).then(
                function (response) {
                    scope.setData(response.data);
                    succCb();
                },
                function (response) {
                    errCb(modelStateValidation.parseError(response.data));
                });
        },
        loadActive: function (succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("contract", "active")).then(
                function (response) {
                    scope.setData(response.data);
                    succCb();
                },
                function (response) {
                    errCb(modelStateValidation.parseError(response.data));
                });
        }
    };

    return ContractListDataService;
}]);

mainApp.factory("RenewContractDataService",
    ["$http", "router", "modelStateValidation", function ($http, router, modelStateValidation) {

        function RenewContracRenewContractDataService(renewData) {
            this.setData(renewData);
        }
        RenewContracRenewContractDataService.prototype = {
            setData: function (renewData)
            {
                angular.extend(this, renewData);
            },
            create: function (id, errCb) {
                var scope = this;
                $http.get(router.apiPath("contract", "renewal", id)).then(
                       function (response) {
                           var data = response.data;
                           data.periodStart = new Date(data.periodStart);
                           data.periodEnd = new Date(data.periodEnd);
                           scope.setData(response.data);
                       },
                       function (response) {
                           errCb(modelStateValidation.parseError(response.data));
                       });
            },
            save(sucCb, errCb) {
                $http.post(router.apiPath("contract", "renewal"), this).then(
                        function (response) {
                            succCb(response.data);
                        },
                        function (response) {
                            errCb(modelStateValidation.parseError(response.data));
                        }
                );
            }
        };
        return RenewContracRenewContractDataService;
    }]);

mainApp.factory("TerminateContractDataService",
    ["$http", "router", "modelStateValidation", function ($http, router, modelStateValidation) {

        function TerminateContractDataService(contractData) {
            this.setData(contractData);
        }

        TerminateContractDataService.prototype = {
            setData: function (contractData) {
                angular.extend(this, contractData);
            },
            create: function (id, errCb) {
                $http.get(router.apiPath("contract", "terminate", id)).then(
                        function (response) {
                            this.setData(response.data);
                        },
                        function (response) {
                            errCb(modelStateValidation.parseError(response.data));
                        });
            },
            save: function (succCb, errCb)
            {
                $http.post(router.apiPath("contract", "terminate"), this).then(
                        function (response) {
                            succCb(response.data);
                        },
                        function (response) {
                            errCb(modelStateValidation.parseError(response.data));
                        }
                );
            }
        }

        return TerminateContractDataService;
    }]);



