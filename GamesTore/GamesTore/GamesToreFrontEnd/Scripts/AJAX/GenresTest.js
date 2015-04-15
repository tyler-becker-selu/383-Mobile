/// <reference path="../jquery-1.8.2.intellisense.js" />

var relURL = sessionStorage['relURL'],
    authid = sessionStorage['xcmps383authenticationid'],
    key = sessionStorage['xcmps383authenticationkey'],
    myGenre;

function getGenres() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Genres',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Genres:' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function getGenre(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Genres/' + id,
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Genre: ' + data);
        myGenre = data;
        $('#putGenre').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function postGenre() {
    'use strict';
    updateGenre();
    $.ajax({
        url: relURL + 'api/Genres',
        type: 'POST',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myGenre
    }).done(function (data, textStatus, jqXHR) {
        console.log('post Genre: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function putGenre() {
    'use strict';
    updateGenre();
    $.ajax({
        url: myGenre.URL,
        type: 'PUT',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myGenre
    }).done(function (data, textStatus, jqXHR) {
        console.log('put Genre: ' + data);
        $('#putGenre').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function deleteGenre(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Genres/' + id,
        type: 'DELETE',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('delete Genre: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function updateGenre() {
    myGenre.Name = $('#genreName').val();
}