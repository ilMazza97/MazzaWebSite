$(function () {
    $(document).on('click', '#prova', changePaymentMethod);
})
function changePaymentMethod() {
    var buttons = [];
    buttons.push({
        icon: 'fa fa-ban',
        label: ' Cancel',
        action: function (dialogRef) {
            dialogRef.close();
        }
    });
    BootstrapDialog.show({
        title: 'Change Order Status',
        message: "Figaaaa",
        draggable: true,
        nl2br: false,
        buttons: buttons
    });
}
function notificationAlert(response) {
    $.notify({
        message: response.Message
    }, {
        type: response.Status
    });
}