var ID;
function partial() {
    var modal = $("#textHere");
    ID = $('#userID').val();
    var ajaxHandler = $.ajax({
        type: 'Get',
        url: '/Sale/CartForSale/' + ID,
    });
    ajaxHandler.done(function (result) {
        modal.html(result);
    });
    ajaxHandler.fail(function (xhr, ajaxOptions, thrownError) {
        alert(thrownError.toString());
    });
}

function makeSale() {
    console.log(ID);
    $.ajax({
        type: 'POST',
        url: '/Sale/CreateSale',
        data: { 'cartID': ID },
        success: function (data) {
            window.location.href = data.Url
        },
        error: function () {
            console.trace();
        }
    });
}