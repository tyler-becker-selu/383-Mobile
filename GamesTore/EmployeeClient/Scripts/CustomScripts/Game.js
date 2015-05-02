function createGame() {
    var game = {};
    //Fill in the Game
    fillGame(game);

    ajaxCreate(game);

}

function editGame(id) {
    var game = {};
    //Fill in the Game
    fillGame(game);

    game.Id = id;
    game.URL = $('#gameURL').val();

    ajaxEdit(game);

}

function fillGame(game) {
    game.GameName =  $("#name").val();
    game.ReleaseDate = $("#releaseDate").val();
    game.Price = $("#price").val(); 
    game.InventoryStock = $("#inventory").val();

    game.Genres = getGenres();
    game.Tags = getTags();
}

function getTags() {
    var checkedTags = [];

    $("#tags li.active").each(function (idx, li) {
        checkedTags.push({ Name: $(li).text(), Id: $(li).attr('id') })
    });

    return checkedTags;
}

function getGenres() {
    var checkedGenres = [];

    $("#genres li.active").each(function (idx, li) {
       checkedGenres.push({ Name: $(li).text(), Id: $(li).attr('id')})
    });

    return checkedGenres;
}

function ajaxCreate(game) {
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

function ajaxEdit(game) {
    console.log(game);
    $.ajax({
        contentType: "application/json",
        type: 'POST',
        url: '/Game/Edit',
        data: JSON.stringify(game),
        success: function (data) {
            window.location.href = data.Url
        },
        error: function () {
            console.trace();
        }
    });
}