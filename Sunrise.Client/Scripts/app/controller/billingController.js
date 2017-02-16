mainApp.controller("billingController",
        function ($http, $scope, $uibModal,
            BillingDataService,
            confirmationDialog,
            toaster, router, spinnerManager) {
            var $ctrl = this;
            //initialize
            $scope.model = {
                action: {
                    init: init,
                    edit: edit,
                    save: save,
                    dismiss: dismiss,
                    remove: remove,
                    addNewPayment: addNewPayment
                }
            };

            spinnerManager.scope = $scope;


            function init(contractId) {
                spinnerManager.start();
                $scope.model.data = new BillingDataService();
                $scope.model.data.create(contractId,
                    function (respSuccess) {
                        //observer
                        $scope.$watch("model.data.payments",
                            function (nv, ov, ob) {
                                $scope.model.data.checkItemCollide();
                                $scope.model.data.updateTotal();
                                if (nv.length > 0)
                                    $scope.model.data.saveEnabled = true;
                                else
                                    $scope.model.data.saveEnabled = false;
                            },
                            true);
                        windowInit();
                        spinnerManager.stop();
                    },
                    function (respError) {
                        spinnerManager.stop();
                    })
            };

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

            function addNewPayment() {
                spinnerManager.start();
                //get new payment and copy to untouch the orig
                var payment = $scope.model.data.getInitialValue();
                //set up payment
                var lastIndex = $scope.model.data.payments === null ? 0 : $scope.model.data.payments.length;
                $scope.model.data.selectedRow = -1;
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData) {
                    $scope.model.data.addNewPayment(returnData);
                    returnData = null;
                });
                spinnerManager.stop();
            }

            function edit(paymentObject) {
                var index = $scope.model.data.payments.indexOf(paymentObject);
                spinnerManager.start();
                var payment = $scope.model.data.getPayment(index);
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData) {
                    $scope.model.data.editPayment(returnData, index);
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
                        $scope.model.data.removePayment(index);
                        toaster.pop("success", "Successfully remove");
                    }
                });
            }
            function save() {
                confirmationDialog.open({
                    title: "Save Confirmation",
                    description: "Are you sure do you want to save all changes?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        $scope.model.data.save(function (respSuccess) {
                            router.route("contract");
                        },
                        function (respError) {
                            if (typeof (respError) === "undefined") {
                                toaster.pop("error", "Unable to save. Payment should not less than payable amount");
                            }
                        });
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
        $scope.errorState = {};

        //not reference 
        //isolate object value
        $ctrl.payment = payment;
        $ctrl.paymentObject = paymentObject;
        $ctrl.cancel = cancel;
        $ctrl.save = save;

        $ctrl.changeBehaviourWhenSelectingTerm = changeBehaviourWhenSelectingTerm;
        $ctrl.payment.chequeFieldDisabled = $ctrl.payment.paymentTypeCode == "ptcs" ? true : false;
        
        //run
        function cancel() {
            $uibModalInstance.dismiss("cancel");
        }

        function save() {
            $scope.errorState.validate();
            if ($scope.errorState.clear) {
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
        }
    });
