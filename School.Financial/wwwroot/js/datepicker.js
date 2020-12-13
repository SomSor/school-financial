var clientDateFormat = 'LL';
var clientDateTimeFormat = 'LLL';
var clientTimeFormat = 'LT';
var clientDayDateTimeFormat = 'LLLL';

var serverDateFormat = 'YYYY-MM-DD Z';
var serverDateTimeFormat = 'YYYY-MM-DD H:mm Z';
var serverTimeFormat = 'H:mm Z';

$(() => {
    $('.showtime').each(function () {
        if ($(this).html()) $(this).html(moment.utc($(this).html()).local().format("LTS"));
        else $(this).html("-");
        if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format("LTS"));
    });
    $('.showdate').each(function () {
        if ($(this).html()) $(this).html(moment.utc($(this).html()).local().format("LL"));
        else $(this).html("-");
        if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format("LL"));
    });
    $('.showdatetime').each(function () {
        if ($(this).html()) $(this).html(moment.utc($(this).html()).local().format("LL LT"));
        else $(this).html("-");
        if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format("LL LT"));
    });
    $('.showdaydate').each(function () {
        if ($(this).html()) $(this).html(moment.utc($(this).html()).local().format("dddd LL"));
        else $(this).html("-");
        if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format("dddd LL"));
    });
    $('.showmonthyear').each(function () {
        if ($(this).html()) $(this).html(moment.utc($(this).html()).local().format("MMMM YYYY"));
        else $(this).html("-");
        if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format("MMMM YYYY"));
    });

    $('.showdaydatetime').each(function () {
        if ($(this).val()) $(this).html(moment.utc($(this).html()).local().format(clientDayDateTimeFormat));
    });

    $('input.datepicker').each(function () { if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format(clientDateFormat)); });
    $('input.datepicker').datetimepicker({ locale: 'th', format: clientDateFormat, ignoreReadonly: true, showClear: true, showTodayButton: true, });

    $('input.datetimepicker').each(function () { if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format(clientDateTimeFormat)); });
    $('input.datetimepicker').datetimepicker({ locale: 'th', format: clientDateTimeFormat, ignoreReadonly: true, showClear: true, showTodayButton: true, });

    $('input.timepicker').each(function () { if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format(clientTimeFormat)); });
    $('input.timepicker').datetimepicker({ locale: 'th', format: clientTimeFormat, ignoreReadonly: true, showClear: true, showTodayButton: true, });

    $('input.monthpicker').each(function () { if ($(this).val()) $(this).val(moment.utc($(this).val()).local().format("YYYY-MM")); });
    $('input.monthpicker').datetimepicker({ locale: 'th', format: "YYYY-MM", ignoreReadonly: true, showClear: true, showTodayButton: true, });


    $('form').submit(function (e) {
        if ($(this).valid()) {
            try {
                $('input.datepicker').each(function () { if ($(this).val()) $(this).val(moment($(this).val(), clientDateFormat).format(serverDateFormat)); });
                $('input.datetimepicker').each(function () { if ($(this).val()) $(this).val(moment($(this).val(), clientDateTimeFormat).format(serverDateTimeFormat)); });
                $('input.timepicker').each(function () { if ($(this).val()) $(this).val(moment($(this).val(), clientTimeFormat).format(serverTimeFormat)); });
            } catch (error) {
                console.log(error);
            }
        }
    });
});