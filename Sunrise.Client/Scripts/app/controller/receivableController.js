
/**
 * 
 * ReceivableController
 * Assumption: Handle overall receivable function
 * searchContract - search active contract and display
 */

mainApp.controller("receivableController", ReceivableController);
ReceivableController.$inject = ["$scope", "receivableDataManager","ReceivableDataService", "alertDialog", "confirmationDialog", "toaster", "modelStateValidation", "$uibModal", "spinnerManager"];

function ReceivableController(
    $scope,
    receivableDataManager,
    ReceivableDataService,
    alertDialog,
    confirmationDialog,
    toaster,
    modelStateValidation,
    $uibModal,
    spinnerManager) {

    spinnerManager.scope = $scope;

    $scope.model = {
        txtSearch: "",
        enabledUpdate: false,
        showReverse: false,
        currentIndex: -1,
        data: {},
        action: {
            searchContract: searchContract,
            editPayment: editPayment,
            reverseContract: reverseContract,
            update: update,
            addReconciledPayment: addReconciledPayment,
            editReconciledPayment: editReconciledPayment
        }
    };

    $scope.errorState = {};
    var modal = function (type, obj) {
        var parameter = {
            controllerAs: "$ctrl",
            backdrop: false
        };

        if (type === 1) {
            parameter.templateUrl = "/receivable/payment/";
            parameter.controller = "paymentController";
            parameter.resolve = {
                paymentObject: function () { return $scope.model.data.paymentDictionary; },
                payment: function () { return obj; }
            }
        }
        else {
            parameter.templateUrl = "/receivable/reconcile/";
            parameter.controller = "reconcileController";
            parameter.resolve = {
                paymentObject: function () { return $scope.model.data.paymentDictionary; },
                reconcile: function () { return obj; }
            }
        }
        return $uibModal.open(parameter);
    }
    var reset = function () {
        $scope.ctrl.txtSearch = "";
        $scope.ctrl.action.search();
    }

    function searchContract() {

        if ($scope.model.txtSearch.trim().length === 0) {
            toaster.pop("error", "", "Please Enter Search...");
            return 0;
        }

        spinnerManager.start();
        $scope.model.data = new ReceivableDataService();
        $scope.model.data.create($scope.model.txtSearch,
            function (respSuccess) {
                
                $scope.$watch("model.data.payments", function (nv, ov, ob) {
                    $scope.model.data.updateTotal();
                }, true);
                $scope.$watch("model.data.reconciles", function (nv, ov, ob) {
                    $scope.model.data.updateTotal();
                }, true);

                $scope.model.enabledUpdate = true;
                spinnerManager.stop();
            },
            function (respError) {
                toaster.pop("error", "Contract not found");
                $scope.model.enabledUpdate = false;
                spinnerManager.stop();
            }
        );
    }
    function update() {
        confirmationDialog.open({
            title: 'Update Confirmation',
            description: 'Are you sure you want to update?',
            buttons: ['Yes', 'No'],
            action: function (response) {
                spinnerManager.start();
                $scope.model.data.save(
                    function (respSuccess) {
                        if (respSuccess.success) {
                            toaster.pop("success", "", "Save successful");
                            searchContract();
                        }
                        spinnerManager.stop();
                    },
                    function (respError) {
                        for (var key in respError) {
                            toaster.pop("error","", respError[key]);
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
    function editPayment(item) {

        var index = $scope.model.data.payments.indexOf(item);
        if (index >= 0) {
            var payment = $scope.model.data.getPayment(index); 
            var modalInstance = modal(1, payment);
            modalInstance.result.then(function (returnData) {
                $scope.model.data.updatePayment(returnData,index);
            });
        }
    }
    function addReconciledPayment() {

        //check first if has dishonored amount
        if ($scope.model.data.hasDishonored()) {
            var reconcile = $scope.model.data.getNewReconcile();
            var uibModalInstance = modal(2, reconcile);
            uibModalInstance.result.then(function (returnData) {
                $scope.model.data.addNewReconcile(returnData);
            });
        }
        else {
            toaster.pop("error", "", "There's nothing to reconcile");
        }
    }
    function editReconciledPayment(item)
    {
        var index = $scope.model.data.reconciles.indexOf(item);
        if (index >= 0) {
            var reconcile = $scope.model.data.getReconcile(index);
            var modalInstance = modal(2, reconcile);
            modalInstance.result.then(function (returnData) {
                $scope.model.data.updateReconcile(returnData,index);
            });
        }
    }
};

mainApp.controller("paymentController",
    function ($uibModalInstance, payment, paymentObject) {

        var $ctrl = this;
        $ctrl.payment = payment;
        $ctrl.paymentObject = paymentObject;

        $ctrl.cancel = function () {
            $uibModalInstance.dismiss();
        }

        $ctrl.save = function () {
            $ctrl.payment.isModify = true;
            $uibModalInstance.close($ctrl.payment);
        }
    });

mainApp.controller("reconcileController",
    function (reconcile, paymentObject, $uibModalInstance) {

    var $ctrl = this;
    $ctrl.paymentObject = paymentObject;
    $ctrl.reconcile = reconcile;

    $ctrl.cancel = function () {
        $uibModalInstance.dismiss();
    }
    
    $ctrl.save = function () {
        $uibModalInstance.close($ctrl.reconcile);
    }

});