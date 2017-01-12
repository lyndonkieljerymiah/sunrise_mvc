mainApp.controller("receivableController", function ($scope, receivableDataManager, alertDialog, confirmationDialog, toaster,spinnerManager) {

    var $ctrl = this;
    $ctrl.contract = {};
    $ctrl.currentIndex = -1;
    $ctrl.txtSearch = "";
    spinnerManager.scope = $scope;

    $ctrl.search = function () {
        spinnerManager.start();
        $ctrl.currentIndex = -1;
        receivableDataManager.load($ctrl.txtSearch,
            function (data) {
                $ctrl.contract = data;
                $scope.errorState = null;
                spinnerManager.stop();
            },
            function (data)
            {
                $scope.errorState = data;
                $ctrl.contract = {};
                toaster.pop("error", "Contract not found");
                spinnerManager.stop();
            }
        );
    }

    $ctrl.toggle = function (index) {
        if ($ctrl.currentIndex != index)
            $ctrl.currentIndex = index;
        else
            $ctrl.currentIndex = -1;
    };

    $ctrl.updateStatus = function (code)
    {
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
                spinnerManager.start();
                receivableDataManager.update($ctrl.contract,
                    function (data) {
                        if (data.success) {
                            toaster.pop("success", "Save successful");
                            spinnerManager.stop();
                            $ctrl.search();
                        }
                    });
            }
        });
    }

});

