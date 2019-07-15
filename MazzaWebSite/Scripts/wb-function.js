$(document).ready(function () {
    if (window.location.pathname == '/Investor/Deposit') {
        $("#DepositAmount").keyup(function () {
            ButtonEdit();
        });
        $("input[name='Coin']").change(function () {
            ButtonEdit();
        });
    }
});

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
});


function ButtonEdit() {
    var amount = parseFloat($("#DepositAmount").val());
    var button = document.getElementById("deposit-btn");
    if (amount > 0 && $("input[name='Coin']").is(':checked')) {
        button.disabled = false;
        button.style.cursor = "pointer";
    }
    else {
        button.disabled = true;
        button.style.cursor = "not-allowed";
    }
}

function notificationAlert(response) {
    $('#exampleModal').modal('hide');
    $.notify({
        message: response.Message
    }, {
        type: response.Status
    });
};

function addModal() {
    $("body").append('<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true"> <div class="modal-dialog" role="document"> <div class="modal-content"> <div class="modal-header"> <h5 class="modal-title" id="exampleModalLabel">Google Authenticator</h5> <button type="button" class="close" data-dismiss="modal" aria-label="Close"> <span aria-hidden="true">&times;</span> </button> </div><div class="modal-body"> </div><div class="modal-footer"> <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button> <button type="submit" class="btn btn-primary">Save</button> </div></div></div></div>');
};

