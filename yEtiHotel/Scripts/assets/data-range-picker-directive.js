app.directive('ngDateRangePicker', ['Notification', function (Notification) {
    return function (scope, element, attrs) {
        var roomId = attrs.ngDateRangePicker,
            auth = (!angular.isUndefined(attrs.ngAuth) && attrs.ngAuth == "true"),
            date = new Date(),
            compareDates = function (passedDate, owned) {
                if (scope.availabilities[roomId]) {
                    var stringDate = passedDate._d.toDateString(),
                        found = false;
                    angular.forEach(scope.availabilities[roomId], function (reservs) {
                        if (reservs.date == stringDate) {
                            owned.owned = reservs.owned;
                            found = true;
                            return;
                        }
                    });
                    if (found) {
                        return true;
                    }
                }

                return false;
            },
            isRangeValid = function (picker) {
                if (scope.availabilities[roomId]) {
                    var invalid = false;
                    angular.forEach(scope.availabilities[roomId], function (availability) {
                        var date = new Date(availability.date);
                        if (date > picker.startDate._d && date < picker.endDate._d) {
                            invalid = true;
                            return;
                        }
                    });
                    if (invalid) {
                        return false;
                    }
                }

                return true;
            };

        element.daterangepicker({
            startDate: date,
            minDate: date,
            opens: "left",
            locale: {
                format: 'YYYY/MM/DD',
                applyLabel: 'Rezerwuj',
                cancelLabel: 'Anuluj',
                daysOfWeek: ["Nie", "Pon", "Wt", "&#x15A;r", "Czw", "Pi", "Sob"],
                monthNames: [
                    "Stycze&#x144;",
                    "Luty",
                    "Marzec",
                    "Kwiecie&#x144;",
                    "Maj",
                    "Czerwiec",
                    "Lipiec",
                    "Sierpie&#x144;",
                    "Wrzesie&#x144;",
                    "Pa&#x17A;dziernik",
                    "Listopad",
                    "Grudzie&#x144;",
                ],
            },
            autoApply: !auth,
            alwaysShowCalendars: true,
            isInvalidDate: function (date) {
                return compareDates(date, false);
            },
            isCustomDate: function (date) {
                var owned = {owned :false},
                    result = compareDates(date, owned);

                if (result && owned.owned) {
                    return 'owned';
                } else if (result) {
                    return 'reserved';
                }

                return '';
            },
        }).on('apply.daterangepicker', function (e, picker) {
            if (auth) {
                if (!isRangeValid(picker)) {
                    Notification.error("Wybrany okres zawiera terminy ju&#x17C; zarezerwowane!");
                    element.click();
                } else {
                    scope.makeReservation(picker, roomId);
                }
            } else {
                element.click();
            }
        });
    }
}]);
