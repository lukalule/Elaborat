$(function () {
    $('#Kola').change(function () {
        var data = {
            brKola: $('#Kola').val()
        }
        $.post("/Kolo/GetKolo", data, function (result) {

            $('#koloContainer').html(result);
        });
    });
});

