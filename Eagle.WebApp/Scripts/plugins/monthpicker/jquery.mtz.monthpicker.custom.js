function InvokeMonthPicker() {
    var monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    if (LanguageCode ==="vi-VN")
        monthNames = ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'];

    var currentMonth = new Date().getMonth - 1;
    var currentYear = new Date().getFullYear();
    var selectedMonth = '', selectedYear = '', selectedDate = '';
    $('.monthPicker').monthpicker({
        pattern: 'mm/yyyy',
        selectedYear: currentYear,
        startYear: currentYear - 100,
        finalYear: currentYear + 1,
        monthNames: monthNames
    }).bind('monthpicker-click-month', function (e, month) {
        var selectedDate = $(this).monthpicker('getDate');
        var formatSelectedDate = formattedDate(selectedDate);
        //$(this).next('input[type=hidden]').val(selectedDate);
        $(this).siblings().val(formatSelectedDate);
    }).bind('monthpicker-change-year', function (e, year) {
        var selectedDate = $(this).monthpicker('getDate');
        var formatSelectedDate = formattedDate(selectedDate);
        $(this).siblings().val(formatSelectedDate);
    });   
}

function formattedDate(date) {   
    var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [month, day, year].join('/');
}
