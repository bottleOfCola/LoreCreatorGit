$(document).ready(function () {
    $('#elementNameChanger').on("click", function () {
        $('#elementNameText').prop('disabled', !$('#elementNameText').prop('disabled'))
    });
    $('#elementNameText').keyup(function (e) {
        if (e.which == 13) {
            $("#elementNameSubmit").click();
        }
    });
    $('#elementDescriptionChanger').on("click", function () {
        $('#elementDescriptionText').prop('disabled', !$('#elementDescriptionText').prop('disabled'));
        $("#elementDescriptionSubmit").css("display", '');
    });
});