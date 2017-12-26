
    Date.prototype.toDateInputValue = (function () {
        var local = new Date(this);
        local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
        return local.toJSON().slice(0, 10);
    });

var app = angular.module('app', []);
app.controller('RCarCtrl', function ($scope, $http, $window) {
    $scope.UserID = "";
    $scope.CarID = "";
    $scope.ListResult = [];
    $scope.SearchCar = function () {
        $http.get("/CRSS/ByCar?CarID=" + $scope.CarID + "&Inc=false").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $scope.SearchUser = function () {
        $http.get("/CRSS/ByUser?UserID=" + $scope.UserID + "&Inc=false").then(function (response)
        {
            $scope.ListResult = response.data;
        });
    }
    $scope.Refresh = function () {
        $http.get("/CRSS/ByUser?Inc=false").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $http.get("/CRSS/ByUser?Inc=false").then(function (response) {
        $scope.ListResult = response.data;
    });

    $scope.obj = { OrderNum: 0, SD: new Date(), ED: new Date(), RDate: new Date(), User: "", Car: "" };
    $scope.Details = function (id)
    {
        $http.get("/CRSS/OrderDetails?id=" + id).then(function (response) {
            $scope.obj = response.data;
            $scope.obj.SD = new Date(response.data.SD);
            $scope.obj.ED = new Date(response.data.ED);
            $scope.obj.RDate = new Date(response.data.RDate);
        });
    }


    $scope.Remove = function (id) {
        if ($window.confirm("Do you wish to remove this item ?")) {
            $http.get("/CRSS/RemoveRow?RowId=" + id + "&type=Order").then(function (response) {
                if (response.data == true)
                    alert("Success");
                else
                    alert("Row is already Removed or Not found");
                $scope.Refresh();
            });
        }
    }
         
});



