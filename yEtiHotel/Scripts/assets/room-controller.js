app.controller("RoomController",
    ['$scope', '$http', 'ngDialog', '$httpParamSerializerJQLike', 'Notification',
        function ($scope, $http, ngDialog, $httpParamSerializerJQLike, Notification) {
            
            $scope.availabilities = {};

            var dialog,
                message = 'Serwis przechodzi tymczasowe problemy z systemem rezerwacji. Spr&#243;buj ponownie p&#243;&#x17A;niej.';
            
            /**
             * @method handleSuccess
             */
            var handleSuccess = function (response) {
                if (dialog) {
                    dialog.close();
                    Notification.success("Udana rezerwacja!");
                }
                angular.forEach(response.data, function (roomReservations) {
                    $scope.availabilities[roomReservations.roomId] = [];
                    angular.forEach(roomReservations.dates, function (date) {
                        $scope.availabilities[roomReservations.roomId].push({
                            date: (new Date(date.date)).toDateString(),
                            owned: date.owned,
                        });
                    });
                });
            }

            /**
             * @method prepareReservation
             */
            var prepareReservation = function () {
                return {
                    RoomId: $scope.roomId,
                    StartDate: $scope.range.startDate,
                    EndDate: $scope.range.endDate,
                }
            }

            /**
             * @method getAvailabilities
             */
            $scope.getAvailabilities = function () {
                $http.get(
                    '/api/reservations',
                    {
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    }
                ).then(
                    handleSuccess,
                    function () {
                        Notification.error(message);
                    }
                );
            }();

            /**
             * @method makeReservation
             */
            $scope.makeReservation = function (picker, roomId) {
                $scope.roomId = roomId;
                $scope.range = {
                    startDate: moment(picker.startDate._d.toISOString()).format("YYYY-MM-DD"),
                    endDate: moment(picker.endDate._d.toISOString()).format("YYYY-MM-DD"),
                }
                $http.post(
                    '/api/reservations/cost',
                    $httpParamSerializerJQLike(prepareReservation()),
                    {
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        }
                    }
                ).then(
                    function (cost) {
                        dialog = ngDialog.open({
                            template: '/Content/reservation-modal.html',
                            className: 'ngdialog-theme-default',
                            data: {
                                range: $scope.range,
                                cost: cost.data,
                            },
                            scope: $scope,
                        });
                    },
                    function () {
                        Notification.error(message);
                    }
                );
            }

            /**
             * @method reserve
             */
            $scope.reserve = function () {
                $http.post(
                    '/api/reservations',
                    $httpParamSerializerJQLike(prepareReservation()),
                    {
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        }
                    }
                ).then(
                    handleSuccess,
                    function () {
                        dialog.close();
                        Notification.error(message);
                    }
                );
            }

            if (!angular.isUndefined($scope.fetch) && $scope.fetch == true) {
                $http.get('/api/rooms').then(
                    function (response) {
                        $scope.rooms = response.data;
                    },
                    function () {
                        Notification.error(message);
                    }
                );
            }

            /**
             * @method search
             */
            $scope.search = function (picker) {
                var range = {
                    startDate: moment(picker.startDate._d.toISOString()).format("YYYY-MM-DD"),
                    endDate: moment(picker.endDate._d.toISOString()).format("YYYY-MM-DD"),
                }
                $http.post(
                    '/api/rooms/search',
                    $httpParamSerializerJQLike(range),
                    {
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        }
                    }
                ).then(
                    function (response) {
                        $scope.rooms = response.data;
                    },
                    function () {
                        Notification.error(message);
                    }
                );
            }

            /**
             * @method exposure
             */
            $scope.exposure = function (id) {
                var exposures = ["Polnocna", 'Wschodnia', 'Poludniowa', 'Zachodnia']

                return exposures[id];
            }
    }]
);
