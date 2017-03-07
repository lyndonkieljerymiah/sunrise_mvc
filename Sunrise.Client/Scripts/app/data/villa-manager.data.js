mainApp.factory("villaDataManager",
    function ($http, modelStateValidation, router, Upload) {

        function validation() {
            return {
                capacity: { isValid: true, errorMessage: "" },
                ratePerMonth: { isValid: true, errorMessage: "" }
            };
        }

        return {
            create: function (action, failuer) {
                $http.get(router.apiPath("villa", "create")).then(
                    function (response) {
                        var data = response.data;
                        data.profileImage = { id: 0, imageConverted: data.defaultImageUrl };
                        data.validation = validation();
                        action(data);
                    });
            },
            getAllVillas: function (success, failure) {
                $http.get(router.apiPath("villa", "list"))
                   .then(
                   function (response) {
                       var data = response.data;
                       var counterRow = 0;
                       var virtVillas = [];
                       var rows = [];

                       for (var i = 0; i < data.length; i++) {
                           rows.push(data[i]);
                           if (((i + 1) % 3) === 0) {
                               virtVillas.push(rows);
                               rows = [];
                           }
                           else if (data.length < 3) {
                               virtVillas.push(rows);
                           }
                       }
                       if (rows.length > 0) {
                           virtVillas.push(rows);
                       }
                       data.villaGroups = virtVillas;
                       success(data);
                   },
                   function (response) {
                       failure(modelStateValidation.parseError(response.data));
                   }
               );
            },
            searchByNo: function (no, action, failure) {
                $http.get(router.apiPath("villa", "search", no))
                   .then(
                   function (response) {
                       var rows = [];
                       if (response.data.length > 0) {
                           angular.forEach(response.data, function (item) {
                               var row = {
                                   id: item.id,
                                   profileImage: item.imageGallery,
                                   description: item.description,
                                   capacity: item.capacity,
                                   ratePerMonth: item.ratePerMonth,
                                   villaNo: item.villaNo,
                                   status: item.status,
                                   elecNo: item.elecNo,
                                   waterNo: item.waterNo
                               };
                               rows.push(row);
                           });
                       }
                       action(rows);
                   });
            },
            list: function (no, action, failure) {
                $http.get(router.apiPath("villa", "list"))
                   .then(
                       function (response) {
                           var data = {
                               rows: response.data.listView,
                               boards: response.data.boards
                           };
                           action(data);
                       },
                       function (response) {

                       });

            },
            update: function (data, action, failure) {
                data.profileImage = null;
                data.imageGalleries = null;
                data.types = null;

                var upload = Upload.upload(
                    {
                        url: "/api/villa/update",
                        method: 'PUT',
                        data: data
                    });
                upload.then(
                    function (resp) {
                        action(resp.data);
                    },
                    function (resp) {
                        failure(resp);
                    });

            },
            save: function (data, action, failure) {
                data.profileImage = null;
                data.imageGalleries = null;
                data.types = null;
                var upload = Upload.upload(
                    {
                        url: "/api/villa/save",
                        method: 'POST',
                        data: data
                    });

                upload.then(
                    function (resp) {
                        action(resp.data);
                    },
                    function (resp) {
                        failure(resp);
                    });
            },
            edit: function (id, action, failure) {
                $http.get(router.apiPath("villa", "edit", id)).then(
                    function (resp) {
                        var data = resp.data;
                        data.validation = validation();
                        data.forDeletion = "";
                        data.profileImage = { id: 0, imageConverted: data.defaultImageUrl };
                        action(data);
                    },
                    function (resp) {
                        failure(resp.data);
                    });
            },
            redirect: function (id) {
                if (id === null || typeof (id) === "undefined")
                    router.route("villa", "create");
                else
                    router.route("villa", "edit", id);
            }
        }
    });

mainApp.factory("VillaDataService", function ($http, modelStateValidation, router,Upload) {

    function VillaDataService(villaData) {
        this.setData(villaData);
        this.txtSelected = "";
    }

    VillaDataService.prototype = {
        setData: function (villaData) {
            angular.extend(this, villaData);
        },
        create: function (succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("villa", "create")).then(
                   function (respErr) {
                       var data = respErr.data;
                       data.profileImage = { id: 0, imageConverted: data.defaultImageUrl };
                       scope.setData(data);
                       succCb();
                   },
                   function (respErr) {
                       errCb(modelStateValidation.parseError(respErr.data));
                   }
            );
        },
        save: function (succCb, errCb) {
            this.profileImage = null;
            this.imageGalleries = null;
            this.types = null;
            var upload = Upload.upload({ url: "/api/villa/save", method: 'POST', data: this });
            upload.then(
                function (resp) {
                    succCb(resp.data);
                },
                function (resp) {
                    errCb(modelStateValidation.parseError(resp.data));
                });
        },
        edit: function (succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("villa", "edit", this.txtSelected)).then(
                    function (resp) {
                        var data = resp.data;
                        data.forDeletion = "";
                        data.profileImage = { id: 0, imageConverted: data.defaultImageUrl };
                        scope.setData(data);
                        succCb();
                    },
                    function (resp) {
                        errCb(modelStateValidation.parseError(resp.data));
                    });
        },
        update: function (succCb,errCb) {
            this.profileImage = null;
            this.imageGalleries = null;
            this.types = null;
            var upload = Upload.upload({ url: "/api/villa/update", method: 'PUT', data: this });

            upload.then(
                function (resp) {
                    action(resp.data);
                },
                function (resp) {
                    failure(resp);
                });
            
        }
    }
    return VillaDataService;
});

mainApp.factory("VillaListDataService", function ($http, modelStateValidation, router) {

    function VillaListDataService(villaData) {
        this.setData(villaData);
        this.txtSearch = "";
    }

    VillaListDataService.prototype = {
        setData: function (villaData) {
            angular.extend(this, villaData);
        },
        getVacant: function (succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("villa", "search", this.txtSearch))
                  .then(
                  function (respSucc) {
                      var data = {rows:[]};
                      if (respSucc.data.length > 0) {
                          angular.forEach(respSucc.data, function (item) {
                              var row = {
                                  id: item.id,
                                  profileImage: item.imageGallery,
                                  description: item.description,
                                  capacity: item.capacity,
                                  ratePerMonth: item.ratePerMonth,
                                  villaNo: item.villaNo,
                                  status: item.statusDescription,
                                  elecNo: item.elecNo,
                                  waterNo: item.waterNo
                              };
                              data.rows.push(row);
                          });
                      }
                      console.log(data);
                      scope.setData(data);
                      succCb();
                },
                function (respErr) {
                    errCb(modelStateValidation.parseError(respErr.data));
                }
            );


        },
        getAll: function (succCb, errCb) {
            var scope = this;
            $http.get(router.apiPath("villa", "list", this.txtSearch))
                  .then(
                      function (resSucc) {
                          var data = {
                              rows: resSucc.data.listView,
                              boards: resSucc.data.boards
                          };
                          scope.setData(data);
                          succCb();
                      },
                      function (respErr) {
                          errCb(modelStateValidation.parseError(respErr.data));
                      });
        }
    };

    return VillaListDataService;
});