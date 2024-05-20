$(document).ready(function () {
    $('#number').keyup(async function (e) {
        if (e.which == 13) {
            let value = $('#number').val();
            let response = await fetch("http://numbersapi.com/" + value);
            if (response.ok) {
                $('#answer').text(await response.text());
            }
        }
    })
});