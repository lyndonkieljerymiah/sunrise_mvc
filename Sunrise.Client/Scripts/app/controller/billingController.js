mainApp.controller("billingController",
        function ($http, $scope, $uibModal, paymentDataManager, confirmationDialog, toaster) {

            var $ctrl = this;

            //initialize
            $scope.showSaveButton = false;
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

            function start() {
                $scope._spinnerLoading = true;
            }
            function stop() {
                $scope._spinnerLoading = false;
            }
            function openModal(payment)
            {
                var modalInstance = $uibModal
                          .open({
                              backdrop: false,
                              animation: true,
                              templateUrl: "myModalContent.html",
                              controller: "paymentController",
                              controllerAs: "$ctrl",
                              resolve: {
                                  payment: function () { return payment; },
                                  paymentObject: function () { return $scope.sales.paymentDictionary; }
                              }
                          });
                return modalInstance;
            }

            function list() {
                start();
                paymentDataManager.billingList(function (data) {
                    $scope.billings = data;
                    stop();
                });
            }

            function init(transactionId) {
                start();
                paymentDataManager.createBilling(transactionId,
                    function (data) {
                        $scope.sales = data;
                        $scope.sales.currentPaymentIndex = -1;
                        stop();
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
                start();
                //get new payment
                var payment = angular.copy($scope.sales.paymentDictionary.initialValue); 

                //set up payment
                var lastIndex = $scope.sales.payments.length;
                if (lastIndex > 0) {
                    //new should be ahead of one month
                    payment.paymentDate.setMonth($scope.sales.payments[lastIndex - 1].paymentDate.getMonth() + 1);
                    payment.coveredPeriodFrom = payment.paymentDate;
                    payment.coveredPeriodTo.setMonth(payment.coveredPeriodFrom.getMonth() + 1);
                }

                $scope.sales.currentPaymentIndex = -1;
                var modalInstance = openModal(payment);
                console.log(modalInstance);
                modalInstance.result.then(function (returnData) {
                    $scope.sales.payments.push(angular.copy(returnData));
                });
                stop();
            }
            function edit($index, $event) {
                $event.preventDefault();
                $event.stopPropagation();
                start();
                var payment = angular.copy($scope.sales.payments[$index]);
                $scope.sales.currentPaymentIndex = $index;
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData) {
                    angular.copy(returnData, $scope.sales.payments[$index]);
                    $scope.sales.currentPaymentIndex = -1;
                }, function () {
                    $scope.sales.currentPaymentIndex = -1;
                });
                stop();
            }
            function remove($index) {
                $scope.sales.currentPaymentIndex = $index;
                confirmationDialog.open({
                    title: "Confirmation",
                    description: "Do you want to remove the payment?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        //remove item
                        $scope.sales.payments.splice($index, 1);
                        $scope.sales.currentPaymentIndex = -1;
                        toaster.pop("success", "Successfully remove");
                    },
                    cancel: function (response) {
                        $scope.sales.currentPaymentIndex = -1;
                    }
                });
            }
            function save() {
                confirmationDialog.open({
                    title: "Save Confirmation",
                    description: "Are you sure do you want to save all changes?",
                    buttons: ["Yes", "No"],
                    action: function (response) {
                        paymentDataManager.save($scope.sales,
                            function (returnData) {
                                toaster.pop('success', 'Payment saving', 'Add Successfully');
                                init(returnData.id);
                            },
                            function (returnData) {
                                toaster.pop("error","Failed to save unexpected error occured!!!");
                            }
                        );
                    }
                });
            }

            return {
                init: init,
                edit: edit,
                list: list,
                save: save,
                remove: remove,
                addNewPayment: addNewPayment
            }
        });

mainApp.controller("paymentController",
    function ($scope, $uibModalInstance, payment, paymentObject, paymentDataManager) {

        var $ctrl = this;

        //not reference 
        //isolate object value
        $ctrl.payment = payment;


        //for date time object
        $ctrl.dateTimePicker = {
            isOpen: [false, false, false],
            toggle: function ($event, key) {
                $event.preventDefault();
                $event.stopPropagation();
                $ctrl.dateTimePicker.isOpen[key] = $ctrl.dateTimePicker.isOpen[key] ? false : true;
            }
        }
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
            $scope._spinnerLoading = true;
            $uibModalInstance.close($ctrl.payment);
        }

        function changeBehaviourWhenSelectingTerm() {
            if ($ctrl.payment.paymentTypeCode === "ptcs") {
                $ctrl.payment.chequeNo = "Cash";
                $ctrl.payment.status = "psc";
                $ctrl.payment.chequeFieldDisabled = true;
            }
            else {
                $ctrl.payment.chequeNo = "";
                $ctrl.payment.status = "psv";
                $ctrl.payment.chequeFieldDisabled = false;
            }
            $scope.errorState = null;
        }


    });
