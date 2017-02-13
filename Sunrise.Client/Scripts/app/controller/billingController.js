mainApp.controller("billingController",
        function ($http, $scope, $uibModal,
            billingDataManager,
            confirmationDialog,
            toaster, router, spinnerManager) {

            var $ctrl = this;

            //initialize

            $scope.model = {
                data: [],
                action: {
                    init: init,
                    edit: edit,
                    save: save,
                    dismiss: dismiss,
                    remove: remove,
                    addNewPayment: addNewPayment
                },
                showSaveButton: false
            };
            spinnerManager.scope = $scope;

            //observer
            $scope.$watch("model.data.payments", function (nv, ov, ob) {
                $scope.model.data.totalReceived = 0;
                //check length
                if (nv && nv.length > 0) {
                    $scope.model.data.totalReceived = nv.sum("amount");
                }
                else {
                    $scope.model.showSaveButton = false;
                }
            }, true);

            function windowInit() {
                //preventing to close the page
                window.onbeforeunload = function (event) {
                    var message = 'Sure you want to leave?';
                    if (typeof event == 'undefined') {
                        event = window.event;
                    }
                    if (event) {
                        event.returnValue = message;
                    }
                    return message;
                }
            }
            function openModal(payment) {
                var modalInstance = $uibModal
                          .open({
                              backdrop: false,
                              animation: true,
                              templateUrl: "/billing/payment",
                              controller: "paymentController",
                              controllerAs: "$ctrl",
                              resolve: {
                                  payment: function () { return payment; },
                                  paymentObject: function () { return $scope.model.data.paymentDictionary; }
                              }
                          });
                return modalInstance;
            }

            function init(contractId) {

                spinnerManager.start();
                billingDataManager.createBilling(contractId,
                    function (data) {
                        $scope.model.data = data;
                        $scope.model.data.selectedRow = -1;
                        spinnerManager.stop();
                    });

                windowInit();
            }

            function addNewPayment() {

                spinnerManager.start();

                //get new payment and copy to untouch the orig
                var payment = angular.copy($scope.model.data.paymentDictionary.initialValue);
                //set up payment
                var lastIndex = $scope.model.data.payments === null ? 0 : $scope.model.data.payments.length;
                $scope.model.data.selectedRow = -1;
                var modalInstance = openModal(payment);

                modalInstance.result.then(function (returnData) {
                    //return status
                    angular.forEach($scope.model.data.paymentDictionary.statuses, function (item) {
                        if (returnData.statusCode == item.value) {
                            returnData.statusDescription = item.text;
                            return;
                        }
                    });

                    //return payment
                    angular.forEach($scope.model.data.paymentDictionary.modes, function (item) {
                        if (returnData.paymentModeCode == item.value) {
                            returnData.paymentModeDescription = item.text;
                            return;
                        }
                    });
                    console.log(returnData);
                    $scope.model.data.payments.push(angular.copy(returnData));
                });

                spinnerManager.stop();
            }
            function edit(paymentObject) {
                var index = $scope.model.data.payments.indexOf(paymentObject);
                spinnerManager.start();
                var payment = angular.copy($scope.model.data.payments[index]);
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData) {
                    angular.forEach($scope.model.data.paymentDictionary.statuses, function (item) {
                        if (returnData.statusCode == item.value) {
                            returnData.statusDescription = item.text;
                            return;
                        }
                    });
                    //return payment
                    angular.forEach($scope.model.data.paymentDictionary.modes, function (item) {
                        if (returnData.paymentModeCode == item.value) {
                            returnData.paymentModeDescription = item.text;
                            return;
                        }
                    });
                    angular.copy(returnData, $scope.model.data.payments[index]);
                });
                spinnerManager.stop();
            }
            function remove(paymentObject) {
                var index = $scope.model.data.payments.indexOf(paymentObject);
                confirmationDialog.open({
                    title: "Confirmation",
                    description: "Do you want to remove the payment?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        //remove item
                        $scope.model.data.payments.splice(index, 1);
                        toaster.pop("success", "Successfully remove");
                    }
                });
            }
            function save() {

                //verify if payment is completed
                if ($scope.model.data.totalReceived < $scope.model.data.amount) {
                    toaster.pop("error", "Unable to save. Payment should not less than payable amount");
                    return;
                }

                confirmationDialog.open({
                    title: "Save Confirmation",
                    description: "Are you sure do you want to save all changes?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        billingDataManager.save($scope.model.data,
                            function (returnData) {
                                router.route("contract");
                            },
                            function (returnData) {
                                toaster.pop("error", "Failed to save unexpected error occured!!!");
                            }
                        );
                    }
                });
            }
            function dismiss() {
                confirmationDialog.open({
                    title: "Cancel Billing",
                    description: "Are you sure you want to cancel billing transaction",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        paymentDataManager.dismiss($scope.sales,
                        function (response) {
                            router.route("contract", "");
                        },
                        function (response) {
                            toaster.pop("error", "Failed to cancel unexpected error occured")
                        });
                    }
                });

            }

        });

mainApp.controller("paymentController",
    function ($scope, $uibModalInstance, payment, paymentObject) {
        var $ctrl = this;

        //not reference 
        //isolate object value
        $ctrl.payment = payment;
        $ctrl.paymentObject = paymentObject;
        $ctrl.cancel = cancel;
        $ctrl.save = save;
        $ctrl.changeBehaviourWhenSelectingTerm = changeBehaviourWhenSelectingTerm;
        $ctrl.payment.chequeFieldDisabled = $ctrl.payment.paymentTypeCode == "ptcs" ? true : false;

        //error 
        $scope.errorState = null;

        //run
        function cancel() {
            $uibModalInstance.dismiss("cancel");
        }

        function save() {
            if (validateInputs()) {
                $scope._spinnerLoading = true;
                $ctrl.payment.statusCode = $ctrl.payment.paymentTypeCode == "ptcs" ? "psc" : "psv";
                $uibModalInstance.close($ctrl.payment);
            }
        }
        function changeBehaviourWhenSelectingTerm() {
            if ($ctrl.payment.paymentTypeCode === "ptcs") {
                $ctrl.payment.chequeNo = "Cash";
                $ctrl.payment.chequeFieldDisabled = true;
            }
            else {
                $ctrl.payment.chequeNo = "";
                $ctrl.payment.chequeFieldDisabled = false;
            }

            $scope.errorState = null;
        }
        function validateInputs() {
            if ($scope.paymentForm.$invalid) {
                return false;
            }

            return true;
        }

    });
