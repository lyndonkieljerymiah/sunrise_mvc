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
                                   image: item.images[0].imageUrl,
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
                $http.get(router.apiPath("villa", "search", no))
                   .then(
                       function (response) {
                           var rows = [];
                           if (response.data.length > 0) {
                               angular.forEach(response.data, function (item) {
                                   var row = {
                                       id: item.id,
                                       villaNo: item.villaNo,
                                       description: item.description,
                                       elecNo: item.elecNo,
                                       waterNo: item.waterNo,
                                       qtelNo: item.qtelNo,
                                       ratePerMonth: item.ratePerMonth,
                                       status: item.status
                                   };
                                   rows.push(row);
                               });
                           }
                           action(rows);
                       },
                       function (response) {

                       });

            },
            save: function (data, action, failure) {
                var upload = Upload.upload(
                    {
                        url: "/api/villa/save",
                        data: data
                    });
                upload.then(
                    function (resp)
                    {

                    action(resp.data);
                },
                function (resp) {
                    failure(resp);
                });
            },
            edit: function (id, action, failure)
            {
                $http.get(router.apiPath("villa", "edit", id)).then(
                    function (resp)
                    {
                        var data = resp.data;
                        data.validation = validation();
                        data.forDeletion = "";
                        action(data);
                    },
                    function (resp)
                    {

                    });
            },
            redirect: function (id) {
                if (id === null || typeof(id) === "undefined")
                    router.route("villa", "create");
                else
                    router.route("villa", "edit", id);
            }
        }
    });