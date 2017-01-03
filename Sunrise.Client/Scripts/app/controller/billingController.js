mainApp.controller("billingController",
        function ($http, $scope, $uibModal, paymentDataManager) {

            var $ctrl = this;

            function start() {
                $scope._spinnerLoading = true;
            }   

            function stop() {
                $scope._spinnerLoading = false;
            }

            function init(transactionId) {
                start();
                paymentDataManager.createBilling(transactionId,
                    function (data) {
                        $scope.sales = data;
                        stop();
                    });
            }

            function openPaymentDialog() {
                start();
                paymentDataManager.createPayment(function (data) {
                    data.salesTransactionId = $scope.sales.id;
                    data.villaId = $scope.sales.villa.id;
                    data.amount = $scope.sales.villa.ratePerMonth;

                    var modalInstance = $uibModal
                        .open({
                            size: 'lg',
                            backdrop: false,
                            animation: true,
                            templateUrl: "myModalContent.html",
                            controller: "paymentController",
                            controllerAs: "$ctrl",
                            resolve: {
                                payment: function () { return data; }
                            }
                        });
                    modalInstance.result.then(function (returnData) {
                        init(returnData);
                    });
                    stop();

                });


            }

            return {
                init: init,
                openPaymentDialog: openPaymentDialog
            }
        });

mainApp.controller("paymentController",
    function ($scope,$uibModalInstance, payment, paymentDataManager) {

        
        var $ctrl = this;
        this.payment = payment;
        this.payment.chequeFieldDisabled = false;
        
        this.cancel = function () {
            $uibModalInstance.dismiss("");
        }

        this.save = function () {
            $scope._spinnerLoading = true;
            paymentDataManager.save(this.payment,
                    function (response) {
                        if (response.success) {
                            $uibModalInstance.close($ctrl.payment.salesTransactionId);
                            $scope._spinnerLoading = false;
                        }
                    },
                    function (response) {
                        $scope.errorState = response;
                        $scope._spinnerLoading = false;
                    });
        }


        this.changeBehaviourWhenSelectingTerm = function() {

            if (payment.term === "ptcs") {
                payment.chequeNo = "Cash";
                payment.chequeFieldDisabled = true;
            } else {
                payment.chequeNo = "";
                payment.chequeFieldDisabled = false;
            }
            $scope.errorState = null;
        }


    });
