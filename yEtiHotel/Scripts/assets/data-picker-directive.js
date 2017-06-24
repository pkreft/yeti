app.directive('ngDatePicker', function () {
    return function (scope, element, attrs) {
        var roomId = attrs.ngDatePicker,
            date = new Date(),
            compareDates = function (passedDate) {
                if (scope.availabilities[roomId]) {
                    var stringDate = passedDate._d.toDateString();
                    if (-1 !== scope.availabilities[roomId].indexOf(stringDate)) {
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
            isCustomDate: function (date) {
                return compareDates(date) ? 'reserved' : '';
            },
        });
    }
});
