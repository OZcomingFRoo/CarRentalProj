     var app = angular.module('app', []);
app.controller('UserTableCtrl', function ($scope, $http, $window) {
    $scope.name = "";
    $scope.id = "";
    $scope.ListResult = [];
    $scope.SearchByName = function () {
        $http.get("/CRSS/SearchEmps?par=" + $scope.name + "&by=name").then(function (response)
        {
            $scope.ListResult = response.data;
        });
    }
    $scope.SearchById = function () {
        $http.get("/CRSS/SearchEmps?par=" + $scope.id + "&by=id").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $scope.Refresh = function () {
        $http.get("/CRSS/SearchEmps?par=all&by=id").then(function (response) {
            $scope.ListResult = response.data;
        });
    } 
    $http.get("/CRSS/SearchEmps?par=all&by=id").then(function (response) {
        $scope.ListResult = response.data;
    }); // StartUp function

    $scope.Remove = function (id) {
        if ($window.confirm("Do you wish to delete this worker ")) {
            $http.get("/CRSS/RemoveRow?RowId=" + id + "&type=Emp").then(function (response) {
                if (response.data == true)
                    alert("Success");
                else
                    alert("Row is already Removed or Not found");
                $scope.Refresh();
            });
        }
    }

});

