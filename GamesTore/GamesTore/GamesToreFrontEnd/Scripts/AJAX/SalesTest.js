/// <reference path="../jquery-1.8.2.intellisense.js" />

var relURL = sessionStorage['relURL'],
    authid = sessionStorage['xcmps383authenticationid'],
    key = sessionStorage['xcmps383authenticationkey'],
    mySale;

function getSales() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Sales',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Sales:' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function getSale(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Sales/' + id,
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Sale: ' + data);
        mySale = data;
        $('#putSale').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function postSale() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Sales',
        type: 'POST',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: {}
    }).done(function (data, textStatus, jqXHR) {
        console.log('post Sale: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function putSale() {
    'use strict';

    $.ajax({
        url: mySale.URL,
        type: 'PUT',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: mySale
    }).done(function (data, textStatus, jqXHR) {
        console.log('put Sale: ' + data);
        $('#putSale').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function deleteSale(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Sales/' + id,
        type: 'DELETE',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('delete Sale: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}
