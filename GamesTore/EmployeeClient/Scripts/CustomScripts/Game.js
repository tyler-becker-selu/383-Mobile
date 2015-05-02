function createGame() {
    var game = {};
    //Fill in the Game
    fillGame(game);

    ajaxCreate(game);

}

function fillGame(game) {
    game.GameName =  $("#name").val();
    game.ReleaseDate = $("#releaseDate").val();
    game.Price = $("#price").val(); 
    game.InventoryStock = $("#inventory").val();
    game.Genres = GetGenres();
    game.Tags = GetTags();
}

function GetTags() {
    var checkedTags = [];

    $("#tags li.active").each(function (idx, li) {
        checkedTags.push({ Name: $(li).text(), Id: $(li).attr('id') })
    });

    return checkedTags;
}

function GetGenres() {
    var checkedGenres = [];

    $("#genres li.active").each(function (idx, li) {
       checkedGenres.push({ Name: $(li).text(), Id: $(li).attr('id')})
    });

    return checkedGenres;
}

function ajaxCreate(game) {
    console.log(game);
    $.ajax({
        contentType: "application/json",
        type: 'POST',
        url: '/Game/Create',
        data: JSON.stringify(game),
        success: function (data) {
            window.location.href = data.Url
        },
        error: function () {
            console.trace();
        }
    });
}