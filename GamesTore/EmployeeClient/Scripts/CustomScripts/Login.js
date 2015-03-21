function login() {
    'use strict';
    var email, password;

    $('submit').on('click', function(event){
        event.preventDefault();
        email = $('email').val();
        password = $('password').val();

        $.ajax({
            url: sessionStorage.relURL + 'api/ApiKey',
            data: {
                email: email,
                password: password
            }
        }).done(function (data, textStatus, jqXHR) {
            console.log('ajax success');
            sessionStorage.setItem('xcmps383authenticationid', data['UserId']);
            sessionStorage.setItem('xcmps383authenticationkey', data['ApiKey']);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log('ajax fail');
        });
    });
}

$(function () {
    sessionStorage.setItem('relUrl', 'http://localhost:12932/');
    login();
});