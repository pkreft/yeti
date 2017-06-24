app.directive('ngDateRangeSearchPicker', ['Notification', function (Notification) {
    return function (scope, element, attrs) {
        var date = new Date();
           
        element.daterangepicker({
            startDate: date,
            minDate: date,
            locale: {
                format: 'YYYY/MM/DD',
                applyLabel: 'Szukaj',
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
            alwaysShowCalendars: true,
        }).on('apply.daterangepicker', function (e, picker) {
            console.log(scope);
            scope.search(picker);
        });
    }
}]);
