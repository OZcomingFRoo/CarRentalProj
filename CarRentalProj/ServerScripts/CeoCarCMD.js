
     var app = angular.module('app', []);
app.controller('CarTableCtrl', function ($scope, $http, $window) {
    $scope.par1 = "";
    $scope.par2 = "";
         
    $scope.ListResult = [];
    $scope.SearchCarName = function () {
        $http.get("/CRSS/SearchCar?name=" + $scope.par1 + "&by=model").then(function (response)
        {
            $scope.ListResult = response.data;
        });
    }
    $scope.SearchCarId = function () {
        $http.get("/CRSS/SearchCar?name=" + $scope.par2 + "&by=num").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $scope.Refresh = function()
    {
        $http.get("/CRSS/SearchCar?name=all").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $http.get("/CRSS/SearchCar?name=all").then(function (response) {
        $scope.ListResult = response.data;
    });
         
    //Model popup for Details and other options
    $scope.obj = { CarID: "", Details: "", Branch: 0, Kilo: 9999, Year: 2000, Isop: false };
    $scope.Details = function (id) {
        $http.get("/CRSS/CarDetails?id=" + id).then(function (response) {
            $scope.obj = response.data;
            alert(response.data);
        });
    }
         
    $scope.Remove = function (id) {
        if ($window.confirm("Do you wish to remove this item, this will also delete Rent Orders with this car"))
        {
            $http.get("/CRSS/RemoveRow?RowId=" + id + "&type=Car").then(function (response) {
                if (response.data == true)
                    alert("Success");
                else
                    alert("Row is already Removed or Not found");
                $scope.Refresh();
            });
        }
    }
});


