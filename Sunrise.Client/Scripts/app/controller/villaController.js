(function () {

    mainApp.controller("villaController", VillaController);
    mainApp.controller("villaListController", VillaListController);

    VillaController.$inject = ['$scope', 'Upload', 'villaDataManager', 'toaster', 'spinnerManager', 'confirmationDialog'];

    VillaListController.$inject = ['$scope', 'villaDataManager', 'spinnerManager'];

    function VillaController($scope, Upload, villaDataManager, toaster, spinnerManager, confirmationDialog) {
        var $ctrl = this;

        $ctrl.create = create;
        $ctrl.save = save;
        $ctrl.remove = remove;
        spinnerManager.scope = $scope;

        function create(id)
        {
            spinnerManager.start();
            if (id === null || typeof (id) === "undefined" || id.length === 0)
            {
                villaDataManager.create(function (data)
                {
                    $scope.model = data;
                    spinnerManager.stop();
                });
            }
            else
            {
                villaDataManager.edit(id, function (data)
                {
                    $scope.model = data;
                    spinnerManager.stop();
                });
            }
        }

        function save()
        {
            spinnerManager.start();
            villaDataManager.save($scope.model,
                function (data)
                {
                    toaster.pop("success", "Save successfully");
                    spinnerManager.stop();
                    create(data.returnObject);
                },
                function (data)
                {
                    toaster.pop("error","Unexpected error occured");
                    spinnerManager.stop();
                });
        }

        function remove(gallery) {
            confirmationDialog.open({
                title: 'Mark Deleted',
                description: 'Do you to remove from gallery this picture?',
                action: function (resp) {
                    var index = $scope.model.imageGalleries.indexOf(gallery);
                    $scope.model.imageGalleries[index].markDeleted = $scope.model.markDeleted ? false : true;
                    $scope.model.forDeletion = $scope.model.forDeletion + $scope.model.imageGalleries[index].id + ",";
                }
            });
        }
    }


    function VillaListController($scope, villaDataManager, spinnerManager) {
        var $ctrl = this;
        $scope.txtSearch = "";
        $ctrl.list = list;
        $ctrl.redirect = redirect;

        function list(data) {
            villaDataManager.list("", function (data) {
                $scope.models = data;
            });
        }

        function redirect(id) {
            villaDataManager.redirect(id);
        }
    }
})();
