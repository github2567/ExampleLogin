var NGLogin = angular.module("NGLogin", [])

NGLogin.controller("LoginController", function ($scope, $http, $q) {

    $scope.initial = function () {

    }

    $scope.Login = function () {
        
        $http.post("http://localhost:5186/api/Authen/Login", { userName: $scope.user, password: $scope.pass })
            .then(function (response) {
                window.location.href = 'http://localhost:5110/Home/Index/'
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