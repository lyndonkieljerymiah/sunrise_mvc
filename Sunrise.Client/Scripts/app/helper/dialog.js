mainApp.factory("alertDialog", function ($uibModal) {

    return {
        open: function (title, description) {
            $uibModal.open({
                template: ['<div class="modal-header">{{$ctrl.title}}</div>',
                            '<div class="modal-body">{{$ctrl.description}}</div>',
                            '<div class="modal-footer"><button class="btn btn-success" ng-click="$ctrl.close()">Ok</div>'].join(''),
                controller: function (data, $uibModalInstance) {
                    console.log(data);
                    return {
                        title: data.title,
                        description: data.description,
                        close: function () {
                            $uibModalInstance.dismiss();
                        }
                    }
                },
                controllerAs: "$ctrl",
                resolve: {
                    data: function () {
                        return {
                            title: title,
                            description: description
                        };
                    }
                }
            });
        }
    }

});

mainApp.factory("confirmationDialog",
    function ($uibModal) {
        return {
            open: function (objectValue) {

                var uibModalInstance = $uibModal.open({
                    size: 'sm',
                    template: [
                                '<div class="modal-header">{{$ctrl.title}}</div>',
                                '<div class="modal-header">{{$ctrl.description}}</div>',
                                '<div class="modal-footer">',
                                '<button class="btn btn-success" ng-click="$ctrl.ok()">{{$ctrl.valueOk}}</button> ',
                                '<button class="btn btn-danger" ng-click="$ctrl.cancel()">{{$ctrl.valueCancel}}</button>',
                                '</div>'].join(''),
                    controller: function (data, $uibModalInstance) {

                        function ok() {
                            $uibModalInstance.close(true);
                        }

                        function cancel() {
                            $uibModalInstance.dismiss("");
                        }

                        return {
                            title: data.title,
                            description: data.description,
                            valueOk: data.valueOk,
                            valueCancel: data.valueCancel,
                            ok: ok,
                            cancel: cancel
                        }
                    },
                    controllerAs: "$ctrl",
                    resolve: {
                        data: function () {
                            return {
                                title: objectValue.title,
                                description: objectValue.description,
                                valueOk: objectValue.valueOk,
                                valueCancel: objectValue.valueCancel
                            }

                        }
                    }
                });

                uibModalInstance.result.then(
                    function (response) {
                        objectValue.action(response);
                    },
                    function (response) {
                        objectValue.cancel(response);
                    }
                );

            }
        }
    });