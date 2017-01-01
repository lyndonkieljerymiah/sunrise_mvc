mainApp.factory("salesDataManager",
    function($window, $http,modelStateValidation) {
        return {
            redirectToCheckout: function(villaId) {
                $window.location = "/sales/checkout/" + villaId;
            },
            createCheckout: function (villaId, tenantType, success, failure) {
                
                var uriPath = (tenantType === null) ? "/api/sales/create/" + villaId : "/api/sales/create/" + villaId + "/" + tenantType;
                console.log(uriPath);
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
            save: function(data,success,failure) {
                $http.post("/api/sales/create", data)
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
            proceedToBilling: function(id) {
                $window.location = "/sales/billing/" + id;
            }
        }
    });