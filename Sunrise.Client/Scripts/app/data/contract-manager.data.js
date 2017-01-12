mainApp.factory("contractDataManager",
    function ($http, modelStateValidation, router) {
        return {
            list: function (action, failure) {
                $http.get(router.apiPath("contract","list")).then(
                    function (response) {
                        var row = [];
                        if (response.data.length > 0) {
                            angular.forEach(response.data, function (item) {
                                console.log(item);
                                var col = {
                                    id: item.id,
                                    code: item.code,
                                    villaNo: item.villa.villaNo,
                                    tenant: item.name,
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
                var uriPath = (tenantType === null) ? router.apiPath("contract", "create", villaId) : router.apiPath("contract", "create", villaId + "/" + tenantType);
                $http.get(uriPath)
                  .then(
                      function (response) {
                          var data = response.data;
                          data.periodStart = new Date(data.periodStart);
                          data.periodEnd = new Date(data.periodEnd);

                          //slides
                          data.images = data.villa.images;
                          if (data.register.individual) {
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
            save: function (data, success, failure) {
                $http.post(router.apiPath("contract", "create"), data)
                .then(
                function (response) {
                    //route data
                    var responseData = response.data;
                    success(responseData);
                },
                function errorCallback(response) {
                    failure(modelStateValidation.parseError(response.data));
                });
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