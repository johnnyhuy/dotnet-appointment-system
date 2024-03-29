﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#dateTimePicker").flatpickr({
    enableTime: true,
    dateFormat: "Y-m-d H:i",
    allowInput: true,
    inline: true
});

$("#datePicker").flatpickr({
    enableTime: false,
    dateFormat: "Y-m-d",
    allowInput: true,
    inline: true
});
