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
                    var hasChange = false;
                    $scope.$watch("model.profileIndex", function (nv, ov, ob)
                    {   
                        angular.forEach($scope.model.imageGalleries, function (item) {
                            if(item.id === nv) {
                                $scope.model.profileImage = item;
                                hasChange = true;
                            }
                        });
                    }, true);
                    spinnerManager.stop();
                });
            }
        }

        function save()
        {
            if (validation()) {
                spinnerManager.start();
                if ($scope.model.id !== "") {
                    villaDataManager.update($scope.model, success, failure);
                }
                else {
                    villaDataManager.save($scope.model, success, failure);
                }
            }
            else {
                toaster.pop("error", "Validation error");
            }

        }

        function success(data) {
            toaster.pop("success", "Save successfully");
            spinnerManager.stop();
            create(data.returnObject);
        }

        function failure(data) {
            toaster.pop("error", "Unexpected error occured");
            spinnerManager.stop();
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

        function validation() {
            if (!$scope.model.validation.capacity.isValid) {
                return false;
            }
            return true;
        }
    }

    function VillaListController($scope, villaDataManager, spinnerManager) {
        var $ctrl = this;
        $scope.txtSearch = "";
        $ctrl.list = list;
        $ctrl.redirect = redirect;
        spinnerManager.scope = $scope;
        function list(data) {
            spinnerManager.start();
            villaDataManager.list("", function (data) {
                $scope.models = data.rows;
                $scope.boards = data.boards;
                spinnerManager.stop();
            });
        }

        function redirect(id) {
            villaDataManager.redirect(id);
        }
    }
})();
