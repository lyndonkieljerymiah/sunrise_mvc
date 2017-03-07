
mainApp.controller("contractController", function (
    $scope, ContractDataService, confirmationDialog, toaster, spinnerManager, $timeout) {

    var isPageLoad = false;
    spinnerManager.scope = $scope;

    $scope.model = {
        action: {
            init: init,
            save: save
        }
    };
    $scope.errorState = {  };
    function create(villaId) {

        spinnerManager.start();
        $scope.model.data = new ContractDataService();
        $scope.model.data.create(villaId, function () {
            //villa
            $scope.nbSlides.images = $scope.model.data.villa.imageGalleries;
            $scope.$watch("model.data.register.tenantType", function (nv, ov, ob) {
                $scope.model.data.register.isIndividual = nv === "ttin" ? true : false;

                if ($scope.model.data.register.isIndividual) {
                    $scope.model.data.register.name = "";
                    $scope.model.data.register.company.businessType = "";
                    $scope.model.data.register.company.crNo = "";
                    $scope.model.data.register.company.representative = "";
                }
                else {
                    $scope.model.data.register.name = "";
                    $scope.model.data.register.individual.qatarId = "";
                    $scope.model.data.register.individual.company = "";
                }
            });
            spinnerManager.stop();
        });
    }
    function init(villaId) {
        create(villaId);
    }

    function save() {
        confirmationDialog.open({
            title: "Saving Confirmation!!",
            description: "Are you sure you want to save?",
            buttons: ["Yes", "No"],
            action: function (response) {
                //reverse
                $scope.errorState.validate(function (item) {
                    if ($scope.model.data.register.isIndividual) {
                        var exception = [
                            "model.data.register.company.businessType",
                            "model.data.register.company.crNo",
                            "model.data.register.company.representative"];

                        for (i = 0; i < exception.length; i++) {
                            if (item == exception[i]) {
                                return true;
                            }
                        }
                        return false;
                    }
                    else {
                        var exception = [
                            "model.data.register.individual.qatarId",
                            "model.data.register.individual.company"];
                        for (i = 0; i < exception.length; i++) {
                            if (item === exception[i]) {
                                return true;
                            }
                        }
                        return false;
                    }
                });
                if ($scope.errorState.clear) {
                    spinnerManager.start();
                    $scope.model.data.save(
                        function (data) {
                            var id = data.returnObject;
                            spinnerManager.stop();
                            $scope.model.data.routeToBilling(id);
                        },
                        function (respError) {
                            $scope.errorState.error = respError;
                            spinnerManager.stop();
                        }
                    , "model.data");
                }
            }
        });
    }
});

mainApp.controller("villaSearchController", function ($scope, VillaListDataService, $uibModalInstance) {

    $ctrl = this;
    
    $ctrl.model = {
        data: new VillaListDataService(),
        action: {
            search: search,
            select: select
        }
    }

    function search() {
        $ctrl.model.data.getVacant(function () {
        });
    }

    function select(id) {
        $uibModalInstance.close(id);
    }


});

/***************************************
 * 
 *  ContractListController
 *  Assumption: display list of contract (active and pending)
 *  Action: 
 *       add new - redirect to register contract
 *       edit - edit pending contract  
 *       active - load when active selected
 *       remove - remove contract 
 *       terminate - terminate contract
 *       renewal - renew contract
 *   
 ***************************************/
mainApp.controller("contractListController", function ($scope,
        ContractListDataService,
        confirmationDialog,
        spinnerManager,
        $uibModal,
        router,
        toaster,
        uiGridConstants) {

    var $ctrl = this;

    $scope.ctrl = {
        action: {
            createNewContract: add,
            editContract: edit,
            cancelContract: remove,
            onSelectActive: onSelectActive,
            openTerminateDialog: terminate,
            openRenewalDialog: renewal
        }
    };

    //initialize outside scope
    spinnerManager.scope = $scope;

    $scope.gridOptions = {
        enableSorting: true,
        showGridFooter: true,
        rowHeight: 39,
        enableFiltering: false,
        enableGridMenu: true,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        },
        gridMenuCustomItems: [
             {
                 title: 'Hide filter',
                 icon: 'fa fa-filter',
                 leaveOpen: true,
                 order: 0,
                 action: function ($event) {
                     $scope.gridOptions.enableFiltering = !$scope.gridOptions.enableFiltering;
                     $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
                 },
                 shown: function () {
                     return $scope.gridOptions.enableFiltering;
                 }
             },
            {
                title: 'Show filter',
                icon: 'fa fa-filter',
                leaveOpen: true,
                order: 0,
                action: function ($event) {
                    $scope.gridOptions.enableFiltering = !$scope.gridOptions.enableFiltering;
                    $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
                },
                shown: function () {
                    return !$scope.gridOptions.enableFiltering;
                }
            }],
        columnDefs: [
            {
                displayName: 'Date',
                field: 'dateCreated',
                type: 'date',
                cellFilter: 'date:"MM/dd/yyyy"',
                width: '10%',
                cellClass: 'text-center',
                enableFiltering: false
            },
            { displayName: 'Contract No', field: 'code', width: '12%', cellClass: 'text-center' },
            { displayName: 'Tenant', field: 'name' },
            { displayName: 'Villa', field: 'villaNo', width: '10%', cellClass: 'text-center' },
            { displayName: 'Start', field: 'periodStart', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', width: '10%', cellClass: 'text-center', enableFiltering: false },
            { displayName: 'End', field: 'periodEnd', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', width: '10%', cellClass: 'text-center', enableFiltering: false },
            { displayName: 'Status', field: 'contractStatusDescription', width: '8%', cellClass: 'text-center' },
            {
                name: 'action',
                displayName: ' ',
                cellTemplate: ['<div ng-show="row.entity.editState"><div class="btn-group" uib-dropdown dropdown-append-to-body>',
                                '<button class="btn btn-primary btn-xs" uib-dropdown-toggle>Action <i class="fa fa-caret-down"></i></button>',
                                '<ul class="dropdown-menu ui-grid-cell" uib-dropdown-menu role="menu">',
                                    '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.editContract(row.entity.id)">Edit</a></li>',
                                    '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.cancelContract(row.entity.id)">Cancel</a></li>',
                                '</ul></div></div>'].join(''),
                width: '10%',
                cellClass: 'text-center',
                enableSorting: false,
                enableFiltering: false

            }]
    }
    $scope.gridActiveOptions = {
        enableSorting: true,
        showGridFooter: true,
        rowHeight: 39,
        enableFiltering: false,
        enableGridMenu: true,
        columnDefs: [
                { name: 'date', field: 'dateCreated', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false },
                { displayName: 'No.', field: 'code', width: '10%', cellClass: 'text-center' },
                { displayName: 'Name', field: 'name', width: '30%' },
                { name: 'villa', field: 'villaNo', cellClass: 'text-center' },
                { name: 'start', field: 'periodStart', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false, cellClass: 'text-center' },
                { name: 'end', field: 'periodEnd', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false, cellClass: 'text-center' },
                { displayName: 'Status', field: 'statusDescription', cellClass: 'text-center', width: '8%' },
                { name: 'due', field: 'monthDue', width: '5%', enableFiltering: false, cellClass: 'text-center' },
                {
                    name: 'action',
                    displayName: ' ',
                    enableSorting: false,
                    cellTemplate: ['<div class="btn-group" uib-dropdown dropdown-append-to-body>',
                                    '<button class="btn btn-primary btn-xs" uib-dropdown-toggle>Action <i class="fa fa-caret-down"></i></button>',
                                    '<ul class="dropdown-menu ui-grid-cell" uib-dropdown-menu role="menu">',
                                    '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.openRenewalDialog(row.entity)">Renew</a></li>',
                                    '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.openTerminateDialog(row.entity)">Terminate</a></li>',
                                '</ul></div>'].join(''),
                    width: '10%',
                    cellClass: 'text-center'
                }]
    };
    var recordSet = new ContractListDataService();
    var terminateDialog = function (entity) {
        var modalInstance = $uibModal.open({
            backdrop: false,
            controller: "terminateDialogController",
            controllerAs: "$ctrl",
            templateUrl: "/contract/terminateTemplate",
            resolve: {
                entity: function () { return entity; }
            }
        });

        modalInstance.result.then(
            function (modalResult) {
                toaster.pop("success", "Contract successfully terminated");
                router.route("contract", "index");
            },
            function (modalResult) {
                if (modalResult) {
                    for (var key in modalResult) {
                        toaster.pop("error", modalResult[key]);
                    }
                }
            });
    }
    var renewalDialog = function (entity) {

        var modalInstance = $uibModal.open({
            backdrop: false,
            templateUrl: "contract/renewalTemplate/",
            controller: "renewDialogController",
            controllerAs: "$ctrl",
            resolve: {
                entity: function () { return entity; }
            }
        });

        modalInstance.result.then(
            function (modalResult) {
                toaster.pop({
                    title: "success",
                    body: "Contract successfully renewed",
                    onShowCallback: function () {
                        router.route("billing", "index", modalResult);
                    }
                })
            },
            function (modalResult) {
                if (modalResult) {
                    for (var key in modalResult) {
                        toaster.pop("error", modalResult[key]);
                    }
                }
            });
    }
    var init = function () {
        onSelectOfficial();
    }
    init();

    function onSelectActive(search) {
        spinnerManager.start();
        if ($scope.gridActiveOptions.data.length > 0) {
            spinnerManager.stop();
            return 0;
        }
        recordSet.loadActive(
            function () {
                spinnerManager.stop();
                $scope.gridActiveOptions.data = recordSet.dataSet;
            },
            function () {
                toaster.pop("error", "Unexpected error occured");
                spinnerManager.stop();
            });
    }
    function onSelectOfficial() {
        spinnerManager.start();
        recordSet.loadOfficial(
            function () {
                spinnerManager.stop();
                $scope.gridOptions.data = recordSet.dataSet;
            },
            function () {
                toaster.pop("error", "Unexpected error occured");
            }
        );

    }
    function add() {
        var modalInstance = $uibModal.open({
            size: "lg",
            templateUrl: '/contract/search',
            controller: 'villaSearchController',
            controllerAs: '$ctrl'
        });
        modalInstance.result.then(function (villaId) {
            router.route("contract", "", villaId);
        });
    }
    function remove(id) {
        confirmationDialog.open({
            title: "Remove Contract",
            description: "Do you want to cancel the contract?",
            action: function (response) {
                spinnerManager.start();
                recordSet.remove(id,
                    function (data) {
                        toaster.pop("success", "Contract successfully remove");
                        spinnerManager.stop();
                        onSelectOfficial();
                    },
                    function (data) {
                        toaster.pop("error", "Contract unable to remove");
                        spinnerManager.stop();
                    });
            }
        });
    }
    function edit(id) {
        recordSet.routeToBilling(id);
    }
    function terminate(entity) {
        confirmationDialog.open({
            title: 'Confirmation',
            description: 'Are you sure Do you want Terminate the Contract No ' + entity.code + '?',
            action: function (response) {
                terminateDialog(entity);
            }
        });
    }
    function renewal(entity) {
        confirmationDialog.open({
            title: 'Confirmation',
            description: 'Are you sure Do you want Renew the Contract No ' + entity.code + '?',
            action: function (response) {
                renewalDialog(entity);
            }
        });
    }
});



/***************************************************
 *  TerminationController
 *  Assumption: terminate contract
 * 
 ***************************************************/
mainApp.controller("terminateDialogController", TerminateController);

function TerminateController($scope, entity, $uibModalInstance, TerminateContractDataService) {

    var $ctrl = this;
    $ctrl.model = new TerminateContractDataService();
    $ctrl.model.create(entity.id);
    $ctrl.save = save;
    $ctrl.cancel = cancel;
    function save() {
        $ctrl.model.save(function (respSuccess) {
            $uibModalInstance.close(respSuccess);
        },
        function (respFailure) {
            $uibModalInstance.dismiss(respFailure);
        });
    }

    function cancel() {
        $uibModalInstance.dismiss();
    }
}



/*******************************************************************
 * 
 * RenewDialogController
 * Assumption: Manages Renewal Entry Form
 * save - validate and send data back for saving
 */

mainApp.controller("renewDialogController", RenewDialogController);

function RenewDialogController($uibModalInstance, confirmationDialog, RenewContractDataService, entity) {

    var $ctrl = this;
    $ctrl.model = new RenewContractDataService();
    $ctrl.model.create(entity.id);
    $ctrl.save = save;
    $ctrl.dismiss = dismiss;

    function save() {
        confirmationDialog.open({
            title: "Saving Confirmation",
            description: "Do you want to Save?",
            buttons: ["Yes", "No"],
            action: function () {
                $ctrl.model.save(
                    function (respSuccess) {
                        $uibModalInstance.close(respSuccess.returnObject);
                    },
                    function (respFailure) {
                        $uibModalInstance.dismiss(respFailure);
                    });
            }
        });
    }

    function dismiss() {
        $uibModalInstance.dismiss();
    }
};




