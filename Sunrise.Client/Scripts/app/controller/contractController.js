

mainApp.directive("tenantRegister", function ($http) {
    return {
        restrict: "EA",
        replace: true,
        link: function (scope) {
            scope.getTemplate = function () {
                return scope.template;
            }
        },
        template: "<div ng-include='getTemplate()'></div>"
    }
});

mainApp.controller("contractController", function ($scope, contractDataManager, confirmationDialog, toaster, spinnerManager) {

    var templatePath = "/tenant/register/";
    var isPageLoad = false;
    spinnerManager.scope = $scope;

    $scope.sales = {};

    function restart() {
        $scope.errorState = {};
        isPageLoad = false;
    }



    function create(villaId, tenantType) {
        spinnerManager.start();
        isPageLoad = true;
        contractDataManager.createContract(villaId, tenantType,
            function (data) {
                console.log(data.villa.imageGalleries);
                $scope.nbSlides.images = data.villa.imageGalleries
                console.log($scope.nbSlides);
                $scope.sales = data;
                $scope.template = templatePath + data.register.tenantType;
                spinnerManager.stop(restart);
            });
    }
    function init(villaId) {
        create(villaId, null);
    }
    function changeTenantType() {
        if (!isPageLoad) {
            create($scope.sales.villa.id, $scope.sales.register.tenantType);
        }
    }
    function save() {
        confirmationDialog.open({
            title: "Saving Confirmation!!",
            description: "Are you sure you want to save?",
            buttons: ["Yes", "No"],
            action: function (response) {
                spinnerManager.start();
                //reverse
                $scope.sales.register.gender = parseInt($scope.sales.register.gender);
                contractDataManager.save($scope.sales,
                    function (data) {
                        var id = data.id;
                        contractDataManager.proceedToBilling(id);
                        spinnerManager.stop(restart);
                    },
                    function (data) {
                        $scope.errorState = data;
                        toaster.pop("error", "Failed to save unexpected error occured");
                        spinnerManager.stop();
                    });
            }
        });
    }

    return {
        init: init,
        changeTenantType: changeTenantType,
        save: save
    }
});

mainApp.controller("villaSearchController", function (villaDataManager, $uibModalInstance) {

    $ctrl = this;
    $ctrl.villas = [];
    $ctrl.txtSearch = "";
    $ctrl.search = search;
    $ctrl.select = select;

    function search() {
        villaDataManager
            .searchByNo($ctrl.txtSearch, function (data) {
                $ctrl.villas = data;
            });
    }

    function select(id) {
        $uibModalInstance.close(id);
    }


});



mainApp.controller("contractListController", function ($scope, contractDataManager,
        confirmationDialog, spinnerManager,
        $uibModal, router, toaster) {
    $scope.txtSearch = "";
    spinnerManager.scope = $scope;

    function list() {
        spinnerManager.start();
        contractDataManager.list($scope.txtSearch,
            function (data) {
                $scope.contracts = data;
                spinnerManager.stop();
            },
            function (data) {

            });
    }
    function add() {

        var modalInstance = $uibModal.open({
            size: "lg",
            templateUrl: '/contract/search',
            controller: 'villaSearchController',
            controllerAs: '$ctrl'
        });

        modalInstance.result.then(function (villaId) {
            contractDataManager.redirectToContract(villaId);
        });
    }
    function cancel(id) {
        confirmationDialog.open({
            title: "Remove Contract",
            description: "Do you want to cancel the contract?",
            action: function (response) {
                spinnerManager.start();
                contractDataManager.cancel(id,
                    function (data) {
                        toaster.pop("success", "Contract successfully remove");
                        spinnerManager.stop();
                        list();
                    },
                    function (data) {
                        toaster.pop("error", "Contract unable to remove");
                        spinnerManager.stop();
                    });

            }
        });
    }

    function view(id) {
        contractDataManager.proceedToBilling(id);
    }
    return {
        list: list,
        add: add,
        view: view,
        cancel: cancel
    }
});

mainApp.controller("contractExpiryListController", function ($scope, contractRenewalManager, spinnerManager,toaster, uiCalendarConfig, $uibModal) {

    var ctrl = this;
    ctrl.list = list;
    ctrl.openRenewDialog = openRenewDialog;

    spinnerManager.scope = $scope;

    function list() {
        spinnerManager.start();
        contractRenewalManager.expires(
            function (result) {
                $scope.models = result;
                spinnerManager.stop();
            },
            function (result) {

            });
    }

    function openRenewDialog(item) {
        spinnerManager.start();
        contractRenewalManager.renewal(item.id,
            function (data) {
                spinnerManager.stop();
                var modalInstance = $uibModal.open({
                    size: 'lg',
                    templateUrl: "/contract/renewalTemplate",
                    controller: "renewDialogController",
                    controllerAs: "$ctrl",
                    resolve: {
                        item: function () {
                            return data;
                        }
                    }
                });

                modalInstance.result.then(function (renewData) {
                    contractRenewalManager.renew(renewData,
                        function (response) {
                            list();
                        },
                        function (response) {

                        });
                })
            },
            function (data) {

            });

    }
});


//renewal Dialog Controller
mainApp.controller("renewDialogController", function (item, $uibModalInstance, confirmationDialog) {
    var $ctrl = this;
    $ctrl.save = save;

    init();

    function init() {
        $ctrl.model = item;
    }

    function save() {
        confirmationDialog.open({
            title: "Saving Confirmation",
            description: "Do you want to Save?",
            buttons: ["Yes", "No"],
            action: function () {
                $uibModalInstance.close($ctrl.model);
            }
        });
    }
});




