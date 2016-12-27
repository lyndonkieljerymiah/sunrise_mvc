mainApp.controller("billingController",

    function ($http, $scope, $uibModal) {

        var $ctrl = this;

        this.init = function(transactionId) {

            $http.get("/api/sales/billing/" + transactionId)
                .then(function (response) {
                    $scope.sales = response.data;
                });

        }
        this.openPaymentDialog = function ()
        {
            $http.get("/api/sales/payment")
                .then(function (response)
                {
                    var payment = response.data;
                    payment.salesTransactionId = $scope.sales.id;
                    payment.villaId = $scope.sales.villa.id;
                    payment.paymentDate = new Date(payment.paymentDate);
                    payment.coveredPeriodFrom = new Date(payment.coveredPeriodFrom);
                    payment.coveredPeriodTo = new Date(payment.coveredPeriodTo);

                    $uibModal.open({
                        size: 'lg',
                        backdrop: false,
                        animation: true,
                        templateUrl: "myModalContent.html",
                        controller: "paymentController",
                        controllerAs: "$ctrl",
                        resolve: {
                            payment: function() {
                                return payment;
                            },
                            billingInstance : function() {
                                return $ctrl;
                            }
                        }
                    });
                });

            
        }
    });

mainApp.controller("paymentController",
    function ($uibModalInstance, $http, payment,billingInstance) {

        payment.chequeFieldDisabled = false;

        function cancel() {
            $uibModalInstance.dismiss("cancel");
        }

        function save() {
            $http.post("/api/sales/payment", payment)
                .then(function (response) {
                    if (response.data.success) {
                        billingInstance.init(payment.salesTransactionId);
                        $uibModalInstance.dismiss("cancel");
                    }
                });
        }


        function changeBehaviourWhenSelectingTerm() {
            console.log(payment.term);
            if (payment.term === "ptcs") {
                payment.chequeNo = "Cash";
                payment.chequeFieldDisabled = true;
            } else {
                payment.chequeNo = "";
                payment.chequeFieldDisabled = false;
            }


        }
        return {
            cancel: cancel,
            save: save,
            payment: payment,
            changeBehaviourWhenSelectingTerm: changeBehaviourWhenSelectingTerm
        }
    });
