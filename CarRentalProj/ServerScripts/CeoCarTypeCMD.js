


     var app = angular.module('app', []);
app.controller('CarTypeTableCtrl', function ($scope, $http, $window) {
    $scope.model = "";
    $scope.company = "Any";
    $scope.GT = "Any";
    $scope.only = true;
    $scope.ListResult = [];
    $scope.SearchCarType = function () {
        $http.get("/CRSS/SearchCarType?model=" + $scope.model +"&GT="+$scope.GT +"&only="+$scope.only + "&company=" + $scope.company).then(function (response)
        {
            $scope.ListResult = response.data;
        });
    }
    $scope.Refresh = function()
    {
        $http.get("/CRSS/SearchCarType?model=all&GT=" + $scope.GT + "&only=true&company=Any").then(function (response) {
            $scope.ListResult = response.data;
            $scope.only = true;
            $scope.company = "Any";
        });
    }
    $http.get("/CRSS/SearchCarType?model=all&GT=" + $scope.GT + "&only=" + $scope.only + "&company=Any").then(function (response) {
        $scope.ListResult = response.data;
    });


    $scope.obj = { Model: "", Company: "", DRent: 0, LRent: 0, Id: "", GT: "", IsObsolete: "" };
    $scope.Details = function (id)
    {
        $http.get("/CRSS/CarTypeDetails?id=" + id).then(function (response)
        {
            $scope.obj = response.data
        });
    }


    $scope.Remove = function (id) {
        if ($window.confirm("Do you wish to send this to Archive ? this will prevent from adding new cars from this model")) {
            $http.get("/CRSS/RemoveRow?RowId=" + id + "&type=CarD").then(function (response) {
                if (response.data == true)
                    alert("Success");
                else
                    alert("Row is already obsolite or Not found");
                $scope.Refresh();
            });
        }
    }
});
