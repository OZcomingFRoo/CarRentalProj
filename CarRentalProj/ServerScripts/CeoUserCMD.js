
     var app = angular.module('app', []);
app.controller('UserTableCtrl', function ($scope, $http, $window) {
    $scope.name = "";
    $scope.id = "";
    $scope.ListResult = [];
    $scope.SearchByName = function () {
        $http.get("/CRSS/SearchUsers?par=" + $scope.name+"&by=name").then(function (response)
        {
            $scope.ListResult = response.data;
        });
    }
    $scope.SearchById = function () {
        $http.get("/CRSS/SearchUsers?par=" + $scope.id + "&by=id").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $scope.Refresh = function () {
        $http.get("/CRSS/SearchUsers?par=all&by=id").then(function (response) {
            $scope.ListResult = response.data;
        });
    }
    $http.get("/CRSS/SearchUsers?par=all&by=id").then(function (response) {
        $scope.ListResult = response.data;
    });

    $scope.obj = { Idnum: "", Name: "", Sex: "", Mail: "", Password: "", Bday: "" };
    $scope.Details = function (id)
    {
        $http.get("/CRSS/UserDetails?id="+id).then(function (response) {
            $scope.obj = response.data;
        });
    }

    $scope.Remove = function(id)
    {
        if ($window.confirm("Do you wish to remove this item ? this will also delete Rent Orders with this User"))
        {
            $http.get("/CRSS/RemoveRow?RowId=" + id + "&type=User").then(function (response) {
                if (response.data == true)
                    alert("Success");
                else
                    alert("Row is already Removed or Not found");
                $scope.Refresh();
            });
        }
    }
});

