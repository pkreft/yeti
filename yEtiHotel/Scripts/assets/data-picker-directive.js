app.directive('ngDatePicker', function () {
    return function (scope, element, attrs) {
        var roomId = attrs.ngDatePicker,
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
            };

        element.daterangepicker({
            singleDatePicker: true,
            startDate: element.val(),
            locale: {
                format: 'YYYY-MM-DD',
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
            isInvalidDate: function (date) {
                var owned = { owned: false },
                    result = compareDates(date, owned);

                if (result && owned.owned) {
                    return false;
                } else if (result) {
                    return true;
                }

                return false;
            },
            isCustomDate: function (date) {
                var ownes = { owned: false },
                    result = compareDates(date, ownes.owned);

                if (result && ownes.owned) {
                    return 'owned';
                } else if (result) {
                    return 'reserved';
                }

                return '';
            },
        });
    }
});
