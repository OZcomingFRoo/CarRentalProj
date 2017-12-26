
     Date.prototype.toDateInputValue = (function () {
         var local = new Date(this);
         local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
         return local.toJSON().slice(0, 10);
     });

var app = angular.module('app', []);
app.controller('RCarCtrl', function ($scope, $http, $window) {
    $scope.UserID = "";
    $scope.CarID = "";
    $scope.Inc = true;
    $scope.ListResult = [];
    $scope.Rdate = null;

    $scope.SearchCar = function () {
        $http.get("/CRSS/ByCar?CarID=" + $scope.CarID + "&Inc=" + $scope.Inc).then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $scope.SearchUser = function () {
        $http.get("/CRSS/ByUser?UserID=" + $scope.UserID + "&Inc=" + $scope.Inc).then(function (response)
        {
            $scope.ListResult = response.data;
        });
    }
    $scope.Refresh = function ()
    {
        $http.get("/CRSS/ByUser?Inc=" + $scope.Inc).then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $http.get("/CRSS/ByUser?Inc="+$scope.Inc).then(function (response) {
        $scope.ListResult = response.data;
    });


    $scope.obj = { OrderNum: 0, SD: new Date(), ED: new Date(), RDate: new Date(), User: "", Car: "" };
    $scope.Details = function (id) {
        $http.get("/CRSS/OrderDetails?id=" + id).then(function (response) {
            $scope.obj = response.data;
            $scope.obj.SD = new Date(response.data.SD);
            $scope.obj.ED = new Date(response.data.ED);
            $scope.obj.RDate = new Date(response.data.RDate);
        });
    }
         
    $scope.Calc = 0;
    $scope.kilo = 2222;
    $scope.RetDate = new Date();
    $scope.OrderId = 0;
    $scope.set = function(id)
    {
        $scope.OrderId = id;
        scope.Kilo = 2222;
        $scope.RetDate = new Date();
    }
    $scope.Retrive = function (id,R,k) {
        $http.get("/CRSS/Retrive?id=" + id + "&R="+ R + "&K=" + k).then(function (response) {
            $scope.Calc = response.data;
        });
        $scope.OpenR();
    }
    $scope.OpenR = function () {
        $("#Calc").fadeIn("slow");
    }
    $scope.CloseR = function ()
    {
        $("#Calc").fadeOut("slow");
    }
});
