function editSale(id) {
    console.log(id);
    var sale = {};
    //Fill in the Game
    fillSale(sale);

    sale.Id = id;

    ajaxEdit(sale);
}

function fillSale(sale) {
    sale.Amount = $("#total").val();
    sale.SaleDate = $("#saleDate").val();
   // sale.Games = getSaleGameQuaninty();
    console.log(sale);
}

function getSaleGameQuaninty() {
    var gamesInList = [];
    $("#games li").each(function (idx, li) {
        var gameID = $(li).attr('id');
        gamesInList.push({ Quaninity: $('#' + gameID + '-Quanity').val(), GameID: gameID })
    });
    console.log(gamesInList)
    return gamesInList;
}

function ajaxEdit(sale) {
    console.log(JSON.stringify(sale));
    sale = JSON.stringify({ 'sale': sale });
    $.ajax({
        type: 'POST',
        url: '/Sale/Edit',
        data: sale,
        success: function (data) {
            window.location.href = data.Url
        },
        error: function () {
            console.trace();
        }
    });
}