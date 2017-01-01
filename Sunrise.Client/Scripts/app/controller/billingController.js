mainApp.controller("billingController",
        function ($http, $scope, $uibModal, paymentDataManager) {

            var $ctrl = this;

            function start() {
                $scope.spinnerLoading = true;
            }

            function stop() {
                $scope.spinnerLoading = false;
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

                paymentDataManager.createPayment(function (data) {
                    data.salesTransactionId = $scope.sales.id;
                    data.villaId = $scope.sales.villa.id;

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
                });


                /*
                $http.get("/api/sales/payment")
                    .then(function (response) {
        
                        var payment = response.data;
        
                        payment.salesTransactionId = $scope.sales.id;
                        payment.villaId = $scope.sales.villa.id;
                        payment.paymentDate = new Date(payment.paymentDate);
                        payment.coveredPeriodFrom = new Date(payment.coveredPeriodFrom);
                        payment.coveredPeriodTo = new Date(payment.coveredPeriodTo);
        
                        var modalInstance = $uibModal
                            .open({
                                size: 'lg',
                                backdrop: false,
                                animation: true,
                                templateUrl: "myModalContent.html",
                                controller: "paymentController",
                                controllerAs: "$ctrl"
                            });
        
                        modalInstance.result.then(function () {
                            $http.post("/api/sales/payment", payment)
                            .then(
                                    function (response) {
                                        if (response.data.success) {
                                            $ctrl.init(payment.salesTransactionId);
                                        }
                                    },
                                    function (response) {
                                        $scope.errorState = modelStateValidation.parseError(response.data);
                                        console.log($scope.errorState);
                                    }
                            );
                        });
                    });*/
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
            paymentDataManager.save(this.payment,
                    function (response) {
                        if (response.success) {
                            $uibModalInstance.close($ctrl.payment.salesTransactionId);
                        }
                    },
                    function (response) {
                        $scope.errorState = response;
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
        }


    });
