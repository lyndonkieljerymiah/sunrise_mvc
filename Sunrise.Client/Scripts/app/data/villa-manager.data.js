mainApp.factory("villaDataManager",
    function ($http, modelStateValidation, router) {
        return {
            getAllVillas: function (success, failure) {
                $http.get(router.apiPath("villa", "list"))
                   .then(
                   function (response)
                   {
                       var data = response.data;
                       var counterRow = 0;
                       var virtVillas = [];
                       var rows = [];

                       for (var i = 0; i < data.length; i++)
                       {
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
            }
        }
    });