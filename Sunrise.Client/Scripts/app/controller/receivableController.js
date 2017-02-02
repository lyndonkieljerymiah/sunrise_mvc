
/**
 * 
 * ReceivableController
 * Assumption: Handle overall receivable function
 * searchContract - search active contract and display
 */

mainApp.controller("receivableController", ReceivableController);
ReceivableController.$inject = ["$scope","receivableDataManager","alertDialog","confirmationDialog","toaster","spinnerManager","uiGridConstants"];
function ReceivableController(
    $scope, receivableDataManager,
    alertDialog, confirmationDialog,
    toaster, spinnerManager, uiGridConstants) {

    spinnerManager.scope = $scope;

    $scope.ctrl = {
        txtSearch: "",
        showReverse: false,
        currentIndex: -1,
        data: {},
        action: {
            searchContract: searchContract,
            togglePaymentEditButton: togglePaymentEditButton,
            updateStatus: updateStatus,
            reverseContract: reverseContract,
            updateContract: updateContract
        },
        errorState: {

        }
    }

    $scope.gridOptions = {
        enableCellEdit: false,
        showGridFooter: true,
        showColumnFooter: true,
        columnDefs: [
            { displayName: 'Date', field: 'paymentDate', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', },
            { displayName: 'Cheque No', field: 'chequeNo', },
            { displayName: 'Bank', field: 'bank', },
            { displayName: 'From', field: 'coveredPeriodFrom', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', },
            { displayName: 'To', field: 'coveredPeriodTo', type: 'date', cellFilter: 'date:"MM/dd/yyyy"', },
            {
                displayName: 'Amount',
                field: 'amount',
                cellFilter: 'currency:"QR "',
                aggregationType: uiGridConstants.aggregationTypes.sum,
                footerCellFilter: 'currency:" "'
            },
            { displayName: 'Mode', field: 'paymentMode', },
            {
                displayName: 'Status',
                field: 'statusCode',
                editableCellTemplate: 'ui-grid/dropdownEditor',
                cellFilter: 'mapStatus',
                enableCellEdit: true,
                
            },
            {
                displayName: 'Description',
                field: 'description',
                enableCellEdit: true
            }
        ],
        data: [{
            paymentDate: new Date(),
            chequeNo: '',
            bank: '',
            coveredPeriodFrom: new Date(),
            coveredPeriodTo: new Date(),
            amount: '0.00 QR',
            paymentMode: '',
            status: ''
        }]
    }

    function searchContract() {
        spinnerManager.start();
        $scope.ctrl.currentIndex = -1;

        receivableDataManager.load($scope.ctrl.txtSearch,
            function (data) {

                $scope.ctrl.data = data;
                $scope.ctrl.errorState = null;

                var items = [];
                data.paymentDictionary.statuses.forEach(function (item) {
                    items.push({ id: item.value, text: item.text });
                });


                /**********************************************************************************/
                $scope.gridOptions.columnDefs[7].editDropdownValueLabel = "text";
                $scope.gridOptions.columnDefs[7].editDropdownIdLabel = "id";
                $scope.gridOptions.columnDefs[7].editDropdownOptionsArray = items;
                $scope.gridOptions.onRegisterApi = function (gridApi) {
                    $scope.gridApi = gridApi;
                    gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                        $scope.$apply();
                    });
                };
                /**********************************************************************************/
                $scope.gridOptions.data = data.payments;

                spinnerManager.stop();
                beginObserver();
            },
            function (data) {
                $scope.ctrl.errorState = data;
                $scope.ctrl.data = {};
                toaster.pop("error", "Contract not found");
                spinnerManager.stop();
            }
        );
    }
    function togglePaymentEditButton() {
        if ($scope.ctrl.currentIndex != index)
            $scope.ctrl.currentIndex = index;
        else
            $scope.ctrl.currentIndex = -1;
    }
    function updateStatus(item) {
        var currentIndex = $scope.ctrl.currentIndex;
        $scope.ctrl.data.paymentDictionary.statuses.forEach(function (value) {
            if (value.value == item.statusCode) {
                var index = $scope.ctrl.data.payments.indexOf(item);
                $scope.ctrl.data.payments[index].status = value.text;
            }
        });
    }
    function updateContract() {
        confirmationDialog.open({
            title: 'Update Confirmation',
            description: 'Are you sure you want to update?',
            buttons: ['Yes', 'No'],
            action: function (response) {
                //console.log($scope.ctrl.data);

                spinnerManager.start();
                receivableDataManager.update($scope.ctrl.data,
                    function (data) {
                        if (data.success) {
                            toaster.pop("success", "Save successful");
                            spinnerManager.stop();
                            $scope.action.searchContract();
                        }
                    });
            }
        });
    }
    function reverseContract() {
        confirmationDialog.open({
            title: 'Reverse Contract',
            description: 'Are you sure you want to reverse contract?',
            buttons: ['Yes', 'No'],
            action: function (response) {
                spinnerManager.start();
                receivableDataManager.reverse($scope.ctrl.data,
                    function () {
                        reset();
                        toaster.pop("success", "Contract reverse Successfully!");
                        spinnerManager.stop();
                    },
                    function (data) {
                        spinnerManager.stop();
                    });
            }
        });
    }
    function reset() {
        $scope.ctrl.txtSearch = "";
        $scope.ctrl.action.search();
    }
    function beginObserver() {
        $scope.$watch("data.payments", function (nv, ov, ob) {
            if (nv && nv.length > 0) {
                angular.forEach(nv, function (item) {
                    $scope.ctrl.showReverse = true;
                    if (item.statusCode !== "psv") {
                        $scope.ctrl.showReverse = false;
                        throw "Error";
                    }
                });
            }
            else {
                $scope.ctrl.showReverse = false;
            }
        }, true);
    }

};


mainApp.filter('mapStatus', function () {
    var statusMap = {
        psb: 'Bounce',
        psc: 'Clear',
        psh: 'Hold',
        psv: 'Received'
    };
 
    return function(input) {
        if (!input){
            return '';
        } else {
            return statusMap[input];
        }
    };
})
