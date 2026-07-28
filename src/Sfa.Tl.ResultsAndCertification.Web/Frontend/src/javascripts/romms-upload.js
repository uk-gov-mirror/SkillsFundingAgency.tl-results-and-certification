"use strict";
$(document).ready(function () {
    $('#uploadRommsForm').submit(function () {
        $('#uploadRommsButton').attr('disabled', 'disabled');
        // set screen-reader attributes
        $('#uploadRommsContainer').attr('aria-hidden', 'true');
        $('#spinnerText').attr('role', 'true');
        $('#spinnerText').attr('aria-live', 'assertive');
        setTimeout(function () {
            $(window).scrollTop(0);
            $('.govuk-breadcrumbs').toggleClass('tl-hide');
            $('#uploadRommsContainer').toggleClass('tl-hide');
            $('#processingRommsContainer').toggleClass('tl-hide');
        }, 500);
    });
});