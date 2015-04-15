/// <reference path="../jquery-1.8.2.intellisense.js" />

var relURL = sessionStorage['relURL'],
    authid = sessionStorage['xcmps383authenticationid'],
    key = sessionStorage['xcmps383authenticationkey'],
    myCart,
    myGames;

function getCarts() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Carts',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Carts:' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function getCart(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Carts/' + id,
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Cart: ' + data);
        myCart = data;
        $('#putCart').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function postCart() {
    'use strict';
    updateCart();
    $.ajax({
        url: relURL + 'api/Carts',
        type: 'POST',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myCart
    }).done(function (data, textStatus, jqXHR) {
        console.log('post Cart: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function putCart() {
    'use strict';
    updateCart();
    $.ajax({
        url: myCart.URL,
        type: 'PUT',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myCart
    }).done(function (data, textStatus, jqXHR) {
        console.log('put Cart: ' + data);
        $('#putCart').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function deleteCart(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Carts/' + id,
        type: 'DELETE',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('delete Cart: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function updateCart() {
    myCart.User_Id = authid;
    getGames();
    var gameArr = [];
    myGames.forEach(function (val, index, arr) {
        if (val.GameName === $('#game1').val() ||
            val.GameName === $('#game2').val() ||
            val.GameName === $('#game3').val())
            gameArr.push(val);
    });
    myCart.Games = gameArr;
}

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
        myGames = data;
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}