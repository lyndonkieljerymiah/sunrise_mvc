
/**
 * 
 * ReceivableController
 * Assumption: Handle overall receivable function
 * searchContract - search active contract and display
 */

mainApp.controller("receivableController", ReceivableController);
ReceivableController.$inject = ["$scope", "receivableDataManager", "alertDialog", "confirmationDialog", "toaster", "modelStateValidation","$uibModal", "spinnerManager"];
function ReceivableController(
    $scope,
    receivableDataManager,
    alertDialog,
    confirmationDialog,
    toaster,
    modelStateValidation,
    $uibModal,
    spinnerManager) {

    spinnerManager.scope = $scope;

    $scope.ctrl = {
        txtSearch: "",
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
        },
        errorState: {}
    };

    var updateOnObserve = function (payments) {
        var totalCleared = 0;
        var totalReconcile = $scope.ctrl.data.reconciles.sum("amount");
        angular.forEach(payments, function (item) {
            if (item.statusCode === "psc") {
                totalCleared = totalCleared + item.amount;
            }
            $scope.ctrl.data.totalCleared = totalCleared + totalReconcile;
        });
        $scope.ctrl.data.balance = $scope.ctrl.data.amount - $scope.ctrl.data.totalCleared;
    };
    var modal = function (type, obj) {
        var parameter = {
            controllerAs: "$ctrl",
            backdrop: false
        };

        if (type === 1) {
            parameter.templateUrl = "/receivable/payment/";
            parameter.controller = "paymentController";
            parameter.resolve = {
                paymentObject: function () { return $scope.ctrl.data.paymentDictionary; },
                payment: function () { return obj; }
            }
        }
        else {
            parameter.templateUrl = "/receivable/reconcile/";
            parameter.controller = "reconcileController";
            parameter.resolve = {
                paymentObject: function () { return $scope.ctrl.data.paymentDictionary; },
                reconcile: function () { return obj; }
            }
        }
        return $uibModal.open(parameter);
    }
    var reset = function () {
        $scope.ctrl.txtSearch = "";
        $scope.ctrl.action.search();
    }

    function searchContract()
    {
        if ($scope.ctrl.txtSearch.trim().length === 0) {
            toaster.pop("error", "", "Please Enter Search...");
            return 0;
        }

        $scope.ctrl.currentIndex = -1;
        spinnerManager.start();

        receivableDataManager.get($scope.ctrl.txtSearch,
            function (data) {
                $scope.ctrl.data = data;
                $scope.ctrl.errorState = modelStateValidation.createState(data, "ctrl.data");
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
    function update() {

        confirmationDialog.open({
            title: 'Update Confirmation',
            description: 'Are you sure you want to update?',
            buttons: ['Yes', 'No'],
            action: function (response) {
                spinnerManager.start();
                receivableDataManager.update($scope.ctrl.data,
                    function (data) {
                        if (data.success) {
                            toaster.pop("success", "Save successful");
                            spinnerManager.stop();
                            //$scope.action.searchContract();
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
    function beginObserver() {

        

        $scope.$watch("ctrl.data.payments", function (nv, ov, ob) {
            if (nv && nv.length > 0) {
                updateOnObserve(nv);
            }
        }, true);

        $scope.$watch("ctrl.data.reconciles", function (nv, ov, ob) {
            if (nv && nv.length > 0) {
                updateOnObserve($scope.ctrl.data.payments);
            }
        }, true);


        
    }
    function editPayment(item)
    {   
        var index = $scope.ctrl.data.payments.indexOf(item);
        if (index >= 0)
        {
            var payment = angular.copy($scope.ctrl.data.payments[index]);
            var modalInstance = modal(1, payment);
            modalInstance.result.then(function (returnData) {
                //return status
                angular.forEach($scope.ctrl.data.paymentDictionary.statuses, function (item) {
                    if (returnData.statusCode == item.value) {
                        returnData.statusDescription = item.text;
                        return;
                    }
                });

                //return payment
                angular.forEach($scope.ctrl.data.paymentDictionary.modes, function (item) {
                    if (returnData.paymentModeCode == item.value)
                    {
                        returnData.paymentModeDescription = item.text;
                        return;
                    }
                });

                //return payment
                angular.forEach($scope.ctrl.data.paymentDictionary.banks, function (item) {
                    if (returnData.bankCode == item.value) {
                        returnData.bankDescription = item.text;
                        return;
                    }
                });

                angular.copy(returnData,$scope.ctrl.data.payments[index]);
            })
            

        }
        

    }
    function addReconciledPayment() {

        var reconcile = angular.copy($scope.ctrl.data.paymentDictionary.reconcileInitialValue);
        var uibModalInstance = modal(2, reconcile);
        uibModalInstance.result.then(function (returnData)
        {
            //return payment
            angular.forEach($scope.ctrl.data.paymentDictionary.terms, function (item)
            {
                if (returnData.paymentTypeCode == item.value)
                {
                    returnData.paymentTypeDescription = item.text;
                    return;
                }
            });
            //return payment
            angular.forEach($scope.ctrl.data.paymentDictionary.banks, function (item) {
                if (returnData.bankCode == item.value)
                {
                    returnData.bankDescription = item.text;
                    return;
                }
            });

            $scope.ctrl.data.reconciles.push(angular.copy(returnData));
        });
    }
    function editReconciledPayment(item) {
        var index = $scope.ctrl.data.reconciles.indexOf(item);
        if (index >= 0) {
            var reconcile = angular.copy($scope.ctrl.data.reconciles[index]);
            var modalInstance = modal(2, reconcile);

            modalInstance.result.then(function (returnData) {

                angular.forEach($scope.ctrl.data.paymentDictionary.terms, function (item) {
                        if (returnData.paymentTypeCode == item.value) {
                            returnData.paymentTypeDescription = item.text;
                            return;
                        }
                    });

                    
                angular.forEach($scope.ctrl.data.paymentDictionary.banks, function (item) {
                    if (returnData.bankCode == item.value) {
                        returnData.bankDescription = item.text;
                        return;
                    }
                });
                    
                    angular.copy(returnData, $scope.ctrl.data.reconciles[index]);
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

mainApp.controller("reconcileController", function (reconcile, paymentObject,$uibModalInstance) {

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