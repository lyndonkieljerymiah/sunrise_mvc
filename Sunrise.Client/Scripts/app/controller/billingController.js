mainApp.controller("billingController",
        function ($http, $scope, $uibModal,
            paymentDataManager,
            confirmationDialog,
            toaster, router, spinnerManager) {

            var $ctrl = this;

            //initialize
            $scope.showSaveButton = false;
            $scope.billings = [];
            spinnerManager.scope = $scope;

            //observer
            $scope.$watch("sales.payments", function (nv, ov, ob) {
                $scope.sales.totalReceivedPayment = 0;
                //check length
                if (nv && nv.length > 0) {
                    $scope.sales.totalReceivedPayment = nv.sum("amount");

                    angular.forEach(nv, function (item) {
                        $scope.showSaveButton = false;
                        if (item.id == 0) {
                            $scope.showSaveButton = true;
                            return;
                        }
                    });
                }
                else {
                    $scope.showSaveButton = false;
                }
            }, true);
            
            function openModal(payment) {
                var modalInstance = $uibModal
                          .open({
                              backdrop: false,
                              animation: true,
                              templateUrl: "myModalContent.html",
                              controller: "paymentController",
                              controllerAs: "$ctrl",
                              resolve: {
                                  payment: function () { return payment; },
                                  paymentObject: function () { return $scope.sales.paymentDictionary; },
                                  payments: function () { return $scope.sales.payments }
                              }
                          });
                return modalInstance;
            }

            function list() {
                spinnerManager.start();
                $scope.billings = [];
                paymentDataManager.billingList(function (data) {
                    $scope.billings = data;
                    spinnerManager.stop();
                });
            }

            function init(transactionId)
            {

                spinnerManager.start();
                paymentDataManager.createBilling(transactionId,
                    function (data) {
                        $scope.sales = data;
                        $scope.sales.selectedRow = -1;
                        spinnerManager.stop();
                    });

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

            function addNewPayment() {

                spinnerManager.start();

                //get new payment and copy to untouch the orig
                var payment = angular.copy($scope.sales.paymentDictionary.initialValue);

                //set up payment
                var lastIndex = $scope.sales.payments.length;
                if (lastIndex > 0) {
                    //new should be ahead of one month
                    payment.paymentDate.setMonth($scope.sales.payments[lastIndex - 1].paymentDate.getMonth() + 1);
                    payment.coveredPeriodFrom = payment.paymentDate;
                    payment.coveredPeriodTo.setMonth(payment.coveredPeriodFrom.getMonth() + 1);
                }

                $scope.sales.selectedRow = -1;
                var modalInstance = openModal(payment);

                modalInstance.result.then(function (returnData)
                {
                    //return status
                    angular.forEach($scope.sales.paymentDictionary.statuses, function (item) {
                        if (returnData.statusCode == item.value)
                        {
                            returnData.status = item.text;
                            return;
                        }
                    });

                    //return payment
                    angular.forEach($scope.sales.paymentDictionary.modes, function (item)
                    {
                        if (returnData.paymentModeCode == item.value)
                        {
                            returnData.paymentMode = item.text;
                            return;
                        }
                    });

                    $scope.sales.payments.push(angular.copy(returnData));
                });

                spinnerManager.stop();
            }
            function edit($index, paymentObject, $event)
            {
                $event.preventDefault();
                $event.stopPropagation();
                var index = $scope.sales.payments.indexOf(paymentObject);

                spinnerManager.start();
                var payment = angular.copy($scope.sales.payments[index]);
                $scope.sales.selectedRow = $index;
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData)
                {
                    angular.forEach($scope.sales.paymentDictionary.statuses, function (item)
                    {
                        if (returnData.statusCode == item.value)
                        {
                            returnData.status = item.text;
                            return;
                        }
                    });
                    //return payment
                    angular.forEach($scope.sales.paymentDictionary.modes, function (item) {
                        if (returnData.paymentModeCode == item.value) {
                            returnData.paymentMode = item.text;
                            return;
                        }
                    });
                    angular.copy(returnData, $scope.sales.payments[index]);
                    $scope.sales.selectedRow = -1;
                }, function () {
                    $scope.sales.selectedRow = -1;
                });
                spinnerManager.stop();
                
            }
            function remove($index,paymentObject) {
                var index = $scope.sales.payments.indexOf(paymentObject);
                $scope.sales.selectedRow = $index;
                confirmationDialog.open({
                    title: "Confirmation",
                    description: "Do you want to remove the payment?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        //remove item
                        $scope.sales.payments.splice(index, 1);
                        $scope.sales.selectedRow = -1;
                        toaster.pop("success", "Successfully remove");
                    },
                    cancel: function (response) {
                        $scope.sales.selectedRow = -1;
                    }
                });
            }
            function save() {

                //verify if payment is completed
                if ($scope.sales.totalReceivedPayment < $scope.sales.totalBalance) {
                    toaster.pop("error", "Unable to save. Payment should not less than payable amount");
                    return;
                }

                confirmationDialog.open({
                    title: "Save Confirmation",
                    description: "Are you sure do you want to save all changes?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        paymentDataManager.save($scope.sales,
                            function (returnData) {
                                toaster.pop('success', 'Successfully save');
                                init(returnData.id);
                            },
                            function (returnData) {
                                console.log(returnData);
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
                            router.route("billing");
                        },
                        function (response) {
                            toaster.pop("error", "Failed to cancel unexpected error occured")
                        });
                    }
                });

            }
            return {
                init: init,
                edit: edit,
                list: list,
                save: save,
                dismiss: dismiss,
                remove: remove,
                addNewPayment: addNewPayment
            }
        });

mainApp.controller("paymentController",
    function ($scope, $uibModalInstance, payment, paymentObject,payments, paymentDataManager)
    {
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
        function cancel()
        {
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
            if ($ctrl.payment.paymentTypeCode === "ptcs")
            {
                $ctrl.payment.chequeNo = "Cash";
                $ctrl.payment.chequeFieldDisabled = true;
            }
            else {
                $ctrl.payment.chequeNo = "";
                $ctrl.payment.chequeFieldDisabled = false;
            }

            $scope.errorState = null;
        }
        function validateInputs()
        {
            if ($scope.paymentForm.$invalid)
            {
                return false;
            }

            //do validation
            var isConflict = false;
            angular.forEach(payments, function (item) {
                if ($ctrl.payment.paymentTypeCode === "ptcq" && item.chequeNo === $ctrl.payment.chequeNo) {
                    isConflict = true;
                }
            });

            if (isConflict) {
                $scope.errorState = {chequeNo : "Cheque No already exist!!"};
                return false;
            }


            return true;
        }

    });
