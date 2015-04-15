/// <reference path='../jquery-1.8.2.intellisense.js' />

function login() {
    'use strict';
    var email,
        password;

    console.log('we made it');

    $('#submit').on('click', function (event) {
        event.preventDefault();
        email = $('#email').val();
        password = $('#password').val();
    
        $.ajax({
            url: sessionStorage.relURL + 'api/ApiKey',
            data: {
                email: email,
                password: password
            }
        }).done(function (data, textStatus, jqXHR) {
            console.log('ajax success');
            $('#test').text('Your API Key: ' + data['ApiKey']);
            sessionStorage.setItem('xcmps383authenticationid', data['UserId']);
            sessionStorage.setItem('xcmps383authenticationkey', data['ApiKey']);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log('ajax fail');
            $('#test').text(jqXHR.responseText || textStatus);
        });
    });
}

$(function () {
    sessionStorage.setItem('relURL', 'http://localhost:12932/');
    login();
});