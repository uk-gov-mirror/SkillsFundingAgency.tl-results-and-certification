"use strict";
$(document).ready(function () {
    $('#uploadWithdrawalsForm').submit(function () {
        $('#uploadWithdrawalsButton').attr('disabled', 'disabled');
        // set screen-reader attributes
        $('#uploadWithdrwalsContainer').attr('aria-hidden', 'true');
        $('#spinnerText').attr('role', 'true');
        $('#spinnerText').attr('aria-live', 'assertive');
        setTimeout(function () {
            $(window).scrollTop(0);
            $('.govuk-breadcrumbs').toggleClass('tl-hide');
            $('#uploadWithdrwalsContainer').toggleClass('tl-hide');
            $('#processingWithdrawalsContainer').toggleClass('tl-hide');
        }, 500);
    });
});