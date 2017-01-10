mainApp.controller("receivableController", function ($scope, receivableDataManager, alertDialog, confirmationDialog, toaster) {

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

                toaster.pop("error", "Invalid or no existing villa");
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
            $ctrl.contract.paymentDictionary.statuses.forEach(function (value) {
                if (value.value == code) {
                    $ctrl.contract.payments[currentIndex].status = value.text;
                }
            });
        }
    }
    $ctrl.update = function () {

        confirmationDialog.open({
            title: 'Update Confirmation',
            description: 'Are you sure you want to update?',
            buttons: ['Yes','No'],
            action: function (response) {
                receivableDataManager.update($ctrl.contract,
                    function (data) {
                        if (data.success) {
                            alertDialog.open("Update", "Update successful!!!");
                            $ctrl.search();
                        }
                    });
            }
        });
    }

});

