mainApp.controller("billingController",
        function ($http, $scope, $uibModal, paymentDataManager,confirmationDialog,toaster) {
            var $ctrl = this;

            function start() {
                $scope._spinnerLoading = true;
            }
            function stop() {
                $scope._spinnerLoading = false;
            }
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
                console.log(transactionId);
                start();
                paymentDataManager.createBilling(transactionId,
                    function (data) {
                        $scope.sales = data;
                        $scope.sales.currentPaymentIndex = -1;
                        stop();
                    });
            }

            function addNewPayment() {
                start();
                var payment = $scope.sales.paymentDictionary.initialValue; //get new payment
                $scope.sales.currentPaymentIndex = -1;
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData) {
                    $scope.sales.payments.push(angular.copy(returnData));
                });
                stop();
            }
            function edit($index, $event) {
                $event.preventDefault();
                $event.stopPropagation();
                start();
                var payment = $scope.sales.payments[$index];
                $scope.sales.currentPaymentIndex = $index;
                var modalInstance = openModal(payment);
                modalInstance.result.then(function (returnData) {
                    angular.copy(returnData, $scope.sales.payments[$index]);
                    $scope.sales.currentPaymentIndex = -1;
                    //init(returnData.transactionId);
                }, function () {
                    $scope.sales.currentPaymentIndex = -1;
                });
                stop();
            }
            function remove($index) {

                confirmationDialog.open({
                    title: "Confirmation",
                    description: "Do you want to remove the payment?",
                    valueOk: "Yes",
                    valueCancel: "No",
                    action: function (response) {
                        $scope.sales.payments.splice($index, 1);
                    }
                });
                
            }
            function save() {

                confirmationDialog.open({
                    title: "Save Confirmation",
                    description: "Are you sure do you want to save all changes?",
                    valueOk: "Yes",
                    valueCancel: "No",
                    action: function (response) {
                        console.log(response);
                        paymentDataManager.save($scope.sales, function (returnData) {
                            toaster.pop('success', 'Payment saving', 'Add Successfully');
                            init(returnData.id);
                        });
                    }
                });
            }

            return {
                init: init,
                list: list,
                edit: edit,
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
        $ctrl.payment = angular.copy(payment);
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
            //must be save
            paymentDataManager.validate(this.payment,
                    function (response) {
                        $uibModalInstance.close($ctrl.payment);
                        $scope._spinnerLoading = false;
                    },
                    function (response) {
                        $scope.errorState = response;
                        $scope._spinnerLoading = false;
                    });
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
