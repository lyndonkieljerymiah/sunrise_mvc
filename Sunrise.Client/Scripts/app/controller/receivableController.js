mainApp.controller("receivableController", function ($scope,receivableDataManager,$uibModal,alertDialog) {

    var $ctrl = this;
    $ctrl.contract = {};
    $ctrl.currentIndex = -1;
    $ctrl.txtSearch = "";

    $ctrl.search = function () {

        $scope._spinnerLoading = true;
        $ctrl.currentIndex = -1;
        receivableDataManager.load($ctrl.txtSearch,
            function (data) {
                $scope._spinnerLoading = false;
                $ctrl.contract = data;
                $scope.errorState = null;
            },
            function (data) {
                $scope.errorState = data;
                $ctrl.contract = {};
                $scope._spinnerLoading = false;
                
            }
        );
    }

    $ctrl.toggle = function (index) {
            if ($ctrl.currentIndex != index)
                $ctrl.currentIndex = index;
            else
                $ctrl.currentIndex = -1;
    };

    $ctrl.updateStatus = function (code) {
        var currentIndex = $ctrl.currentIndex;
        if (currentIndex >= 0) {
            $ctrl.contract.paymentObject.statuses.forEach(function (value) {
                if (value.value == code)
                {
                    $ctrl.contract.payments[currentIndex].status = value.text;
                }
            });
        }
    }
    $ctrl.update = function () {
        receivableDataManager.update($ctrl.contract, function (data) {
            if (data.success) {
                alertDialog.open("Update", "Update successful!!!");
                $ctrl.search();
            }
        });
    }

});


mainApp.controller("PaymentModalController", function (payment, $uibModalInstance) {
    console.log(payment);
    function cancel() {
        $uibModalInstance.dismiss();
    }

    function save() {
        $uibModalInstance.close();
    }

    return {
        payment: payment,
        cancel: cancel,
        save: save
    }
})