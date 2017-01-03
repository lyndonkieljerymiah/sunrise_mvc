mainApp.factory("villaDataManager",
    function ($http, modelStateValidation) {
        return {
            getAllVillas: function (success, failure) {
               
                $http.get("/api/sales/list")
                   .then(
                   function (response)
                   {
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
            }
        }
    });