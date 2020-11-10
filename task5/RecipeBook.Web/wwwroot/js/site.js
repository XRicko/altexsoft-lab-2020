// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$("input.check:checkbox").change(function () {
    const input = $("." + $(this).data("group"));

    input.prop("disabled", !this.checked);
    input.prop("required", this.checked);
});

$(function () {
    const checkboxes = document.querySelectorAll(".check");
    const inputs = document.querySelectorAll(".amount-field");

    for (var i = 0; i < inputs.length; i++) {
        if (checkboxes[i].checked) {
            inputs[i].disabled = false;
            inputs[i].required = true;
        }
    }
});
