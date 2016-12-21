
mainApp.factory("tenantService",
    function ($http, $window) {

        var tenantModel = {};
        var tenantsModel = [];

        var module = this;

        function setData(data) {
            tenantModel = data;
        }

        function getTenant() {
            return tenantModel;
        }

        function getTenants() {
            return tenantsModel;
        }

        function create(action) {
            $http.get("/api/tenant/create")
                .then(function (response) {
                    var data = response.data;
                    data.individual.birthday = new Date(data.individual.birthday);
                    data.company.validityDate = new Date(data.company.validityDate);
                    tenantModel = data;
                    action(data);
                });
        }

        function list(action) {
            $http.get("/api/tenant/list")
                .then(function (response) {
                    var data = response.data;
                    tenantsModel = data;
                    action(data);
                });
        }

        function update(data) {
            console.log(data);
            $http.post("/api/tenant/update", data)
                .then(function (response) {
                    console.log(response);
                    //$window.location.replace("/billing/view/1");
                })
                .error(function() {

                });

            
        }

        function getTenantView(view) {
            return "http://" + window.location.host + "/tenant/view/" + view;
        }

        function getSalesView() {
            return "http://" + window.location.host + "/tenant/sales";
        }

        return {
            create: create,
            getTenant: getTenant,
            getTenants: getTenants,
            update: update,
            getTenantView: getTenantView,
            getSalesView: getSalesView
        };
    });
