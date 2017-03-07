
mainApp.controller("villaController", VillaController);
function VillaController($scope, Upload, villaDataManager,VillaDataService, toaster, spinnerManager, confirmationDialog) {
    var $ctrl = this;

    spinnerManager.scope = $scope;

    $scope.model = {
        action: {
            create: create,
            save: save,
            remove: remove
        }
    }

    $scope.errorState = {};

    function create(id) {
        spinnerManager.start();
        $scope.model.data = new VillaDataService();
        if (id === null || typeof (id) === "undefined" || id.length === 0) {
            $scope.model.data.create(function (respSucc) {
                spinnerManager.stop();
            });
        }
        else {
            $scope.model.data.txtSelected = id;
            $scope.model.data.edit(function () {
                spinnerManager.stop();
                $scope.$watch("model.data.profileIndex", function (nv, ov, ob) {
                    angular.forEach($scope.model.data.imageGalleries, function (item) {
                        if (item.id === nv) {
                            $scope.model.data.profileImage = item;
                        }
                    });
                }, true);
            });
        }
    }

    function save() {
        $scope.errorState.validate();
        if ($scope.errorState.clear) {
            spinnerManager.start();
            $scope.model.data.save(
                    function () {
                        spinnerManager.stop();
                        toaster.pop("success", "", "Successfully saved!!!");
                    },
                   function () {
                       spinnerManager.stop();
                       toaster.pop("error", "", "Unexpected errror")
                   });
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
                var index = $scope.model.data.imageGalleries.indexOf(gallery);
                $scope.model.data.imageGalleries[index].markDeleted = $scope.model.data.markDeleted ? false : true;
                $scope.model.forDeletion = $scope.model.forDeletion + $scope.model.data.imageGalleries[index].id + ",";
            }
        });
    }

}




mainApp.controller("villaListController",VillaListController);
function VillaListController($scope, VillaListDataService, spinnerManager, router) {
    var $ctrl = this;
    spinnerManager.scope = $scope;
    $scope.model = {
        action: {
            getAll: function () {
                spinnerManager.start();
                $scope.model.data = new VillaListDataService();
                $scope.model.data.getAll(
                    function () {
                        spinnerManager.stop();
                        
                    },
                    function () {
                        spinnerManager.stop();
                    });
            },
            addVilla: function () {
                router.route("villa", "create");
            },
            editVilla: function (id) {
                console.log(id);
                router.route("villa", "edit", id);
            }
        }
    };
   
}

