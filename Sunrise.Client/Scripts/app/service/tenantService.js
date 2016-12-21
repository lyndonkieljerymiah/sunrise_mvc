
mainApp.factory("tenantService",
    function ($http, $window) {
        function Tenant(tenantData) {
            this.setData(tenantData);
        }

        Tenant.prototype = {
            setData: function (tenantData) {
                angular.extend(this, tenantData);
            },
            create: function(action) {
                var scope = this;
                $http.get("/api/tenant/create")
                    .then(function (response) {
                        var data = response.data;
                        data.individual.birthday = new Date(data.individual.birthday);
                        data.company.validityDate = new Date(data.company.validityDate);
                        scope.setData(data);
                        action(data);
                    });
            },
            list : function() {
                var scope = this;
                $http.get("/api/tenant/list")
                    .then(function(response) {
                        var data = response.data;
                        scope.setData(data);
                    });

            },
            update: function() {
                $window.location.replace("/billing/view/1");
            },
            getTenantView: function(view) {
                return "http://" + window.location.host + "/tenant/view/" + view;
            },
            getSalesView: function() {
                return "http://" + window.location.host + "/tenant/sales";
            }
        }
        return Tenant;
    });
