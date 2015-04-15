/// <reference path="../jquery-1.8.2.intellisense.js" />

var relURL = sessionStorage['relURL'],
    authid = sessionStorage['xcmps383authenticationid'],
    key = sessionStorage['xcmps383authenticationkey'],
    myUser;

function getUsers() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Users',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get users:' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function getUser(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Users/' + id,
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get user: ' + data);
        myUser = data;
        $('#putUser').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function postUser() {
    'use strict';
    updateUser();
    $.ajax({
        url: relURL + 'api/Users',
        type: 'POST',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myUser
    }).done(function (data, textStatus, jqXHR) {
        console.log('post user: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function putUser() {
    'use strict';
    updateUser();
    $.ajax({
        url: myUser.URL,
        type: 'PUT',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myUser
    }).done(function (data, textStatus, jqXHR) {
        console.log('put user: ' + data);
        $('#putUser').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function deleteUser(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Users/' + id,
        type: 'DELETE',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('delete user: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function updateUser() {
    myUser.FirstName = $('#firstName').val();
    myUser.LastName = $('#lastName').val();
    myUser.Email = $('#email').val();
    myUser.Password = $('#password').val();
    myUser.Role = $('#role').val();
}
