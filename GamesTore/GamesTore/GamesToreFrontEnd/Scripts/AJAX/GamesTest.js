/// <reference path="../jquery-1.8.2.intellisense.js" />

var relURL = sessionStorage['relURL'],
    authid = sessionStorage['xcmps383authenticationid'],
    key = sessionStorage['xcmps383authenticationkey'],
    myGame;

function getGames() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Games',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Games:' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function getGame(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Games/' + id,
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Game: ' + data);
        myGame = data;
        $('#putGame').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function postGame() {
    'use strict';
    updateGame();
    $.ajax({
        url: relURL + 'api/Games',
        type: 'POST',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myGame
    }).done(function (data, textStatus, jqXHR) {
        console.log('post Game: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function putGame() {
    'use strict';
    updateGame();
    $.ajax({
        url: myGame.URL,
        type: 'PUT',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myGame
    }).done(function (data, textStatus, jqXHR) {
        console.log('put Game: ' + data);
        $('#putGame').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function deleteGame(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Games/' + id,
        type: 'DELETE',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('delete Game: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function updateGame() {
    myGame.GameName = $('#gameName').val();
    myGame.ReleaseDate = $('#releaseDate').val();
    myGame.Price = $('#price').val();
    myGame.InventoryStock = $('#inventoryStock').val();
    myGame.Genres = Array(
        { name: $('#genre1').val() },
        { name: $('#genre2').val() },
        { name: $('#genre3').val() });
    myGame.Tags = Array(
        { name: $('#tag1').val() },
        { name: $('#tag2').val() },
        { name: $('#tag3').val() });
}
