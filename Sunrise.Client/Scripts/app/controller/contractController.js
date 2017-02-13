


mainApp.controller("contractController", function ($scope, contractDataManager, confirmationDialog, toaster, spinnerManager) {

    var isPageLoad = false;
    spinnerManager.scope = $scope;

    $scope.model = {
        data: {},
        action: {
            init: init,
            save: save
        },
        errorState: {}
    };

    function restart() {
        $scope.errorState = {};
        isPageLoad = false;
    }
    function create(villaId, tenantType) {
        spinnerManager.start();
        isPageLoad = true;
        contractDataManager.createContract(villaId, tenantType,
            function (data) {

                //villa
                $scope.nbSlides.images = data.villa.imageGalleries;
                $scope.model.data = data;
                $scope.$watch("model.data.register.tenantType", function (nv, ov, ob) {
                    $scope.model.data.register.isIndividual = nv === "ttin" ? true : false;
                });
                spinnerManager.stop();
            });
    }
    function init(villaId) {
        create(villaId, null);
    }
    function save() {
        confirmationDialog.open({
            title: "Saving Confirmation!!",
            description: "Are you sure you want to save?",
            buttons: ["Yes", "No"],
            action: function (response) {
                spinnerManager.start();
                //reverse
                $scope.model.data.register.gender = parseInt($scope.model.data.register.gender);
                contractDataManager.save($scope.model.data,
                    function (data) {
                        var id = data.id;
                        spinnerManager.stop();
                        contractDataManager.proceedToBilling(id);
                    },
                    function (data) {
                        $scope.errorState = data;
                        toaster.pop("error", "Failed to save unexpected error occured");
                        spinnerManager.stop();
                    });
            }
        });
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


/***************************************
 * 
 *  ContractListController
 *  Assumption: display list of contract (active and pending)
 *  Action: 
 *       add new - redirect to register contract
 *       edit - edit pending contract  
 *       active - load when active selected
 *       remove - remove contract 
 *       
 *   
 ***************************************/
mainApp.controller("contractListController", function ($scope, contractDataManager,
        confirmationDialog, spinnerManager,
        $uibModal, router, toaster, uiGridConstants) {

    var $ctrl = this;

    $scope.ctrl = {
        action: {
            list: list,
            createNewContract: add,
            editContract: edit,
            cancelContract: remove,
            onSelectActive: onSelectActive,
            terminateContract: terminate
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
            { displayName: 'Tenant', field: 'tenant' },
            { displayName: 'Villa', field: 'villaNo', width: '10%', cellClass: 'text-center' },
            { displayName: 'Start', field: 'periodStart', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', width: '10%', cellClass: 'text-center', enableFiltering: false },
            { displayName: 'End', field: 'periodEnd', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', width: '10%', cellClass: 'text-center', enableFiltering: false },
            { displayName: 'Status', field: 'status', width: '8%', cellClass: 'text-center' },
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
                    { name: 'tenant', field: 'tenant', width: '30%' },
                    { name: 'villa', field: 'villaNo', cellClass: 'text-center' },
                    { name: 'start', field: 'periodStart', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false, cellClass: 'text-center' },
                    { name: 'end', field: 'periodEnd', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false, cellClass: 'text-center' },
                    { name: 'status', field: 'status', cellClass: 'text-center', width: '8%' },
                    { name: 'due', field: 'monthDue', width: '5%', enableFiltering: false, cellClass: 'text-center' },
                    {
                        name: 'action',
                        displayName: ' ',
                        enableSorting: false,
                        cellTemplate: ['<div class="btn-group" uib-dropdown dropdown-append-to-body>',
                                        '<button class="btn btn-primary btn-xs" uib-dropdown-toggle>Action <i class="fa fa-caret-down"></i></button>',
                                        '<ul class="dropdown-menu ui-grid-cell" uib-dropdown-menu role="menu">',
                                        '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.openRenewDialog(row.entity)">Renew</a></li>',
                                        '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.terminateContract(row.entity)">Terminate</a></li>',
                                    '</ul></div>'].join(''),
                        width: '10%',
                        cellClass: 'text-center'
                    }]
        };

    var terminateDialog = function () {
        var modalInstance = $uibModal.open({
            backdrop: false,
            controller: "terminateController",
            controllerAs: "$ctrl",
            templateUrl: "/contract/terminateTemplate"
        });
    }

    function onSelectActive(search) {
        spinnerManager.start();
        if ($scope.gridActiveOptions.data.length > 0) {
            spinnerManager.stop();
            return 0;
        }
        contractDataManager.getActive(search,
            function (data) {
                $scope.gridActiveOptions.data = data;
                spinnerManager.stop();
            },
            function (data) {
                $scope.ctrl.errorState = data;
                spinnerManager.stop();
            });
    }

    function list() {
        spinnerManager.start();
        contractDataManager.list($scope.txtSearch,
            function (data) {
                $scope.contracts = data;
                $scope.gridOptions.data = data;
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
    function remove(id) {
        confirmationDialog.open({
            title: "Remove Contract",
            description: "Do you want to cancel the contract?",
            action: function (response) {
                spinnerManager.start();
                contractDataManager.remove(id,
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
    function edit(id) {
        contractDataManager.proceedToBilling(id);
    }
    function terminate(entity) {
        confirmationDialog.open({
            title: 'Confirmation',
            description: 'Are you sure Do you want Terminate the Contract No '  + entity.code + '?',
            action: function (response) {
                terminateDialog();
            }
        });
    }
});



/***************************************************
 *  TerminationController
 *  Assumption: terminate contract
 * 
 ***************************************************/
mainApp.controller("terminateController", TerminateController);

function TerminateController($uibModalInstance,contractDataManager) {
    
    var $ctrl = this;
    $ctrl.save = save;
    $ctrl.cancel = cancel;
    $ctrl.terminate = {};

    function save() {
        contractDataManager.terminate($ctrl.terminate,
            function (result) {

            },
            function (result) {

            });
    }

    function cancel() {
        $uibModalInstance.dismiss();
    }
}

/*******************************************************
 *  RenewDialogController
 *  Assumption: display all contracts with 6 months validity
 *  list - display list of contracts
 *  openRenewalDialog - open renewal entry form and save it once it's done
 *  openTerminate - open terminate entry form
 *************************************************/
mainApp.controller("contractExpiryListController", ContractExpiryListController);

function ContractExpiryListController($scope,
    contractRenewalManager,
    spinnerManager,
    toaster,
    confirmationDialog,
    $uibModal, uiGridConstants) {

    $scope.ctrl = {
        action: {
            loadContracts: loadContracts,
            openRenewDialog: openRenewDialog,
            terminate: terminate
        }
    }

    //initialize all outside support
    spinnerManager.scope = $scope;

    $scope.gridOptions = {
        enableSorting: true,
        showGridFooter: true,
        enableGridMenu: true,
        enableFiltering: false,
        rowHeight: 35,
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
            { name: 'date', field: 'dateCreated', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false },
            { displayName: 'No.', field: 'code', width: '10%', cellClass: 'text-center' },
            { name: 'tenant', field: 'tenant', width: '30%' },
            { name: 'villa', field: 'villaNo', cellClass: 'text-center' },
            { name: 'start', field: 'periodStart', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false, cellClass: 'text-center' },
            { name: 'end', field: 'periodEnd', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', enableFiltering: false, cellClass: 'text-center' },
            { name: 'status', field: 'status', cellClass: 'text-center', width: '8%' },
            { name: 'due', field: 'monthDue', width: '5%', enableFiltering: false, cellClass: 'text-center' },
            {
                name: 'action',
                displayName: ' ',
                enableSorting: false,
                cellTemplate: ['<div class="btn-group" uib-dropdown dropdown-append-to-body>',
                                '<button class="btn btn-default btn-sm" uib-dropdown-toggle><i class="fa fa-caret-down"></i></button>',
                                '<ul class="dropdown-menu ui-grid-cell" uib-dropdown-menu role="menu">',
                                    '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.openRenewDialog(row.entity)">Renew</a></li>',
                                    '<li role="menuitem"><a href="#" ng-click="grid.appScope.ctrl.action.terminate(row.entity)">Terminate</a></li>',
                                '</ul></div>'].join(''),
                width: '5%',
                cellClass: 'text-center'
            }
        ]
    }


    function loadContracts() {
        spinnerManager.start();

        contractRenewalManager.expires(
            function (result) {
                $scope.models = result;
                $scope.gridOptions.data = result;
                spinnerManager.stop();
            },
            function (result) {
                toaster.pop("error", result.errornullexception);
                spinnerManager.stop();
            });
    }

    function openRenewDialog(item) {
        spinnerManager.start();
        contractRenewalManager.renewal(item.id,
            function (data) {
                spinnerManager.stop();
                var modalInstance = openModalInstance(data);
                modalInstance.result.then(function (response) {
                    if (response.success) {
                        contractRenewalManager.proceedToBilling(response.returnObject) //redirect to contract billing
                    }
                    else {
                        toaster.pop("error", response.renewcontractexception);
                    }
                })
            },
            function (data) {
                spinnerManager.stop();
                toaster.pop("error", "", data.errornullexception);
            });
    }

    function terminate() {
        confirmationDialog.open({
            title: "Confirmation",
            description: "Do you want to Terminate the contract?",
            buttons: ["Yes", "No"]
        });
    }

    function openModalInstance(data) {
        return $uibModal.open({
            backdrop: false,
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
    }
};


/*******************************************************************
 * 
 * RenewDialogController
 * Assumption: Manages Renewal Entry Form
 * save - validate and send data back for saving
 */


mainApp.controller("renewDialogController", RenewDialogController);

function RenewDialogController($scope, $uibModalInstance, confirmationDialog, contractRenewalManager, item) {
    var $ctrl = this;

    $ctrl.save = save;
    $ctrl.init = init;
    $ctrl.dismiss = dismiss;



    function init() {

        $ctrl.model = item;

        $scope.$watch("nbSlides", function (nv, ov, ob) {
            nv.images = item.villa.imageGalleries;
        });
    }

    function save() {
        confirmationDialog.open({
            title: "Saving Confirmation",
            description: "Do you want to Save?",
            buttons: ["Yes", "No"],
            action: function () {
                contractRenewalManager.renew($ctrl.model,
                    function (response) {
                        $uibModalInstance.close(response);
                    },
                    function (response) {
                        $uibModalInstance.close(response);
                    });
            }
        });
    }



    function dismiss() {
        $uibModalInstance.dismiss();
    }
};




