$(function () {
    $(document).on("click", ".language-button", function () {
        var language = $(this).text();
        $.ajax({
            url: "/Home/SetCulture",
            data: { "culture": language },
            success: function () {
                location.reload(true);
                window.scrollTo(0, 0);
            }
        });
    });
})


function notificationAlert(response) {
    $('#exampleModal').modal('hide');
    $.notify({
        message: response.Message
    }, {
        type: response.Status
    });
}