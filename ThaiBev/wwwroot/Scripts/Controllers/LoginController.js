var NGLogin = angular.module("NGLogin", [])

NGLogin.controller("LoginController", function ($scope, $http, $q) {

    $scope.initial = function () {

    }

    $scope.Login = function () {
        
        //$http.post("http://localhost:5186/api/Authen/Login", { userName: $scope.user, password: $scope.pass })
        //    .then(function (response) {

        //        $http.post("http://localhost:5110/Login/SetToken",
        //            JSON.stringify(response.data.token),
        //            { headers: { 'Content-Type': 'application/json' } }
        //        )
        //            .then(function (res) {
        //                window.location.href = "http://localhost:5110/Home/Index";
        //            })
        //            .catch(function (err) {
        //                console.log(err);
        //            });

        //    });

        $http.post("http://localhost:5110/Login/Index", JSON.stringify({ username: $scope.user, password: $scope.pass }))
            .then(function (response) {
                debugger
                window.location.href = "http://localhost:5110/Home/Index";

            })
            .catch(function (err) {
                console.log(err);
                $scope.error = err.data;
            });

    };

});

NGLogin.controller("RegisterController", function ($scope, $http, $q) {

    $scope.initial = function () {
        
    }

    $scope.RegisterCancel = function () {

        window.location.href = 'http://localhost:5110/Login'

    };

});